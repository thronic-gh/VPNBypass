using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Reflection;
using System.IO;
using System.Net;
using System.Timers;
using System.Runtime.InteropServices;
using System.Threading;

namespace VPNBypassService
{
	partial class VPNBypassService : ServiceBase
	{
		private static string exepath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
		private static string logfile = string.Format(@"{0}\{1}",exepath,"ServiceLog.txt");
		private static string hostsfile = @"C:\Windows\System32\drivers\etc\hosts";
		private static string configfile = string.Format(@"{0}\{1}", exepath, "config.ini");
		private static string configfile_default = string.Format(@"{0}\{1}", exepath, "config-default.ini");
		private static string routefile = string.Format(@"{0}\{1}", exepath, "Routes.dat");
		// private System.Timers.Timer timer = new System.Timers.Timer();
		private string conf_loadFile;
		private string conf_gateway;
		private string[] conf_addDomains = new string[1000];
		private int conf_domainCount = 0;
		private List<string> allroutes = new List<string>();
		private bool allroutesLock = false;
		//private Dictionary<string, string> resolved_ips = new Dictionary<string, string>();
		private List<KeyValuePair<string,string>> resolved_ips = new List<KeyValuePair<string, string>>();
		private bool ShuttingDown = false;

		[DllImport("dnsapi.dll", EntryPoint="DnsFlushResolverCache")]
		private static extern UInt32 DnsFlushResolverCache();

		public VPNBypassService()
		{
			InitializeComponent();
			ServicePointManager.DnsRefreshTimeout = 0;
		}

		private void LoadRouteFile()
		{
			try {
			string[] RouteCollection = File.ReadAllLines(routefile);
			foreach (string route in RouteCollection) {
				
				// Ignore comments.
				if (route.Contains("#"))
					continue;
			
				allroutes.Add(route);
			
			}} catch (Exception e) {
				LogErrorMessage(e.Message, "LoadRouteFile()");
			}
		}

		protected override void OnStart(string[] args)
		{
			try {
			LoadConfigFile();

			if (!File.Exists(routefile))
				ResetRouteFile();
			else
				LoadRouteFile();

			// Set up a timer to trigger at interval.
			//timer.Interval = 10000; // 10 seconds
			//timer.Elapsed += new System.Timers.ElapsedEventHandler(this.OnTimer);
			//timer.Start();
			Thread T = new Thread(new ThreadStart(OnTimer));
			T.IsBackground = true;
			T.Start();

			this.logThis("Service started.");

			} catch (Exception e) {
				LogErrorMessage(e.Message, "OnStart()");
			}
		}

		protected override void OnStop()
		{
			// Clean routes and hosts.
			try {
			ShuttingDown = true;
			this.UpdateHostsRoutes(true);
			this.logThis("Service stopped.");

			} catch (Exception e) {
				LogErrorMessage(e.Message, "OnStop()");
			}
		}

		private void OnTimer()
		{
			while (!ShuttingDown) {

			List<string> ResolvedIPs = new List<string>();
			IPAddress _ip_out;

			try {
			// Clean the DNS cache (properly we hope).
			DnsFlushResolverCache();

			// Handle each domain in configuration file every 10 seconds.
			foreach (string _s in conf_addDomains) {
				if (_s == null)
					break;

				ResolvedIPs = FetchTheIP(_s);
				if (ResolvedIPs.Count == 0) {
					logThis(_s +" could not be resolved/renewed this round.");
					continue;
				} else {
					// We get all A records, but we can only route one so we pick the first.
					// Any load balacing should happen on the other side anyways, and if 
					// traffic shows up on eth0 from another IP, we already have ip rule
					// in place.

					// Update 2019.5.2 - Will now route all IPs, for round-robin purposes.
					// Since Dictionary can't use a domain name multiple times as key, I'll
					// use a List of KeyValuePair instead.

					foreach (string _ip in ResolvedIPs) {
						if (IPAddress.TryParse(_ip, out _ip_out)) {
							resolved_ips.Add(new KeyValuePair<string,string>(_s, _ip));
							//break;
						}
					}
				}
			}

			// Handle fresh list of resolved domains and wait to do it all over again.
			UpdateHostsRoutes(false);
			resolved_ips.Clear();

			} catch (Exception e) {
				LogErrorMessage(e.Message, "OnTimer()");
			}
			
			Thread.Sleep(10000); }
		}

		private void LoadConfigFile() 
		{
			try {
				conf_domainCount = 0;
				Array.Clear(conf_addDomains, 0, conf_addDomains.Count());

				// Load from default config file if there is no custom yet.
				if (File.Exists(configfile))
					conf_loadFile = configfile;
				else
					conf_loadFile = configfile_default;

				// Parse configuration file.
				string[] configLines = File.ReadAllLines(conf_loadFile);
				for (int n=0; n<configLines.Count(); n++) {
					if (configLines[n].StartsWith("add_domain#")) {
						conf_addDomains[conf_domainCount] = configLines[n].Substring(11);
						conf_domainCount += 1;
					} else if (configLines[n].StartsWith("gateway#")) {
						this.conf_gateway = configLines[n].Substring(8);
					}
				}
			
			} catch (Exception e) {
				this.LogErrorMessage(e.Message, "LoadConfigFile()");
			}
		}

		private void UpdateHostsRoutes(bool JustCleanIt)
		{
			if (resolved_ips.Count == 0 && !JustCleanIt)
				return;

			try {
			string[] HostLines = File.ReadAllLines(hostsfile);
			//Dictionary<string,string> AddedHosts = new Dictionary<string, string>();
			List<KeyValuePair<string,string>> AddedHosts = new List<KeyValuePair<string,string>>();
			string newHostData = "";
			int emptyLines = 0;
			Process p = new Process();

			// First find old hosts records.
			foreach (string HostLine in HostLines) {
				if (HostLine.Contains("# Added by VPN Bypass")) {
					AddedHosts.Add(new KeyValuePair<string,string>(HostLine.Split('\t')[1], HostLine.Split('\t')[0]));
					continue;
				}

				// Ignore excessive empty lines.
				if (HostLine == "")
					emptyLines += 1;
				else
					emptyLines = 0;

				if (emptyLines <= 1)
					newHostData += HostLine + Environment.NewLine;
			}

			// Register new data if we're not just cleaning up.
			p.StartInfo.UseShellExecute = false;
			p.StartInfo.FileName = "route";
			p.StartInfo.RedirectStandardOutput = true;
			p.StartInfo.RedirectStandardError = true;
			p.StartInfo.StandardOutputEncoding = Encoding.ASCII;
			p.StartInfo.StandardErrorEncoding = Encoding.ASCII;

			if (!JustCleanIt) {

				// Add new resolved/updated domains first.
				newHostData += Environment.NewLine;
				allroutesLock = true;
				foreach (KeyValuePair<string,string> IPHost in resolved_ips) {

					newHostData += IPHost.Value +"\t"+ IPHost.Key +"\t"
						+"# Added by VPN Bypass" + Environment.NewLine;
					
					// Add to routing table. OS throws it away if it's already there so no worries.
					p.StartInfo.Arguments = "add "+ IPHost.Value +" mask 255.255.255.255 "+ conf_gateway + " metric 2";
					p.Start();
					if (p.StandardError.ReadToEnd() == "") {
						if (!allroutes.Contains(IPHost.Value)) {
							allroutes.Add(IPHost.Value);
							File.AppendAllText(routefile, IPHost.Value + Environment.NewLine);
						}
					}
					p.WaitForExit();
				}
				allroutesLock = false;

				// Add the already added, last. They may have failed to resolv just for a round or two.
				// If wanted, /cleanup can be used to clear the table if too many builds up over time.
				bool _dom_exists = false;
				foreach (KeyValuePair<string,string> r_ip in AddedHosts) {
					foreach (KeyValuePair<string,string> _dom in resolved_ips) {
						if (_dom.Key == r_ip.Key) 
							_dom_exists = true;
					}

					if (!_dom_exists) {
						// It should already exist in the routing table.
						// So we'll just add it again to /etc/hosts.
						newHostData += r_ip.Value +"\t"+ r_ip.Key +"\t"+ 
							"# Added by VPN Bypass" + Environment.NewLine;
					}
				}
			
			} else {
				logThis("Cleaning up all routes and hosts.");
				DeleteAllRoutes();
			}

			p.Dispose();
			File.WriteAllText(hostsfile, newHostData);
			
			} catch (Exception e) {
				this.LogErrorMessage(e.Message, "UpdateHostsRoutes()");
			}
		}

		private void DeleteAllRoutes()
		{
			Process p = new Process();
			p.StartInfo.UseShellExecute = false;
			p.StartInfo.FileName = "route";
			p.StartInfo.RedirectStandardOutput = true;
			p.StartInfo.RedirectStandardError = true;
			p.StartInfo.StandardOutputEncoding = Encoding.ASCII;
			p.StartInfo.StandardErrorEncoding = Encoding.ASCII;

			while (allroutesLock)
				Thread.Sleep(500);

			try {
			foreach (string _s in allroutes) {

				if (_s != "") {
					p.StartInfo.Arguments = "delete "+ _s;
					p.Start();
					p.WaitForExit();
				}
			}

			p.Dispose();
			allroutes.Clear();
			ResetRouteFile();

			// Clean up service installer logs.
			if (File.Exists(exepath +@"\InstallUtil.InstallLog"))
				File.Delete(exepath +@"\InstallUtil.InstallLog");
			
			if (File.Exists(exepath +@"\VPNBypassService.InstallLog"))	
				File.Delete(exepath +@"\VPNBypassService.InstallLog");

			} catch (Exception e) {
				LogErrorMessage(e.Message, "DeleteAllRoutes()");
			}
		}

		private void ResetRouteFile()
		{
			try {
				File.WriteAllText(routefile, "# USED BY THE PROGRAM. DO NOT MAKE CHANGES TO THIS FILE."+ Environment.NewLine);
			} catch (Exception e) {
				LogErrorMessage(e.Message, "ResetRouteFile()");
			}
		}

		private List<string> FetchTheIP(string host)
		{
			List<string> ip_list = new List<string>();

			try {
				IPAddress[] IpInfo = Dns.GetHostAddresses(host);
				foreach (IPAddress _ip in IpInfo) {
					ip_list.Add(_ip.ToString());
				}
				return ip_list;

			} catch (Exception e) {
				this.logThis("FetchTheIP("+ host +"): "+ e.Message +".");
				return ip_list;
			}
		}

		private void StopTheService()
		{
			try {
				using (ServiceController sc = new ServiceController("VPNBypassService")) {
					if (
						sc.Status == ServiceControllerStatus.Stopped || 
						sc.Status == ServiceControllerStatus.StopPending
					) {
						logThis("Already stopped or stopping.");
					} else {
						sc.Stop();
					}
				}
					
			} catch (Exception e) { logThis(e.Message); }
		}

		private void logThis(string s)
		{
			try {
				File.AppendAllText(logfile, 
					DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") +": "+ 
					s + Environment.NewLine);
		
			} catch (Exception e) {
				LogErrorMessage(e.Message, "logThis()");
			}
		}

		private void LogErrorMessage(string err, string wut)
		{
			try {
				logThis(wut + " ran into an error: " + err + ", Stopping the service.");
				this.StopTheService();
			
			} catch (Exception e) {
				File.WriteAllText(
					logfile, "Internal log error: " + 
					e.Message +", "+
					DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") +"."+
					Environment.NewLine
				);
			}
		}
	}
}
