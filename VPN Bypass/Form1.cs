using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Configuration.Install;
using System.ServiceProcess;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;

namespace VPN_Bypass
{
	public partial class Form1 : Form
	{
		[DllImport("user32.dll")]
		private static extern int SendMessage(IntPtr hWnd, int wMsg, IntPtr wParam, IntPtr lParam);
		private static string exepath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
		private string configfile = string.Format(@"{0}\{1}", exepath, "config.ini");
		private string configfile_default = string.Format(@"{0}\{1}", exepath, "config-default.ini");
		private string logfile = string.Format(@"{0}\{1}",exepath,"ProgramLog.txt");
		private string logfile_service = string.Format(@"{0}\{1}",exepath,"ServiceLog.txt");
		private System.Timers.Timer timer = new System.Timers.Timer();
		private bool RestartServiceRequest = false;

		public Form1()
		{
			InitializeComponent();
			this.LoadConfiguration();
			this.loadLogWindow();
			this.Text = "VPN Bypass v"+ Assembly.GetExecutingAssembly().GetName().Version.Major.ToString() +"."+ 
						Assembly.GetExecutingAssembly().GetName().Version.Minor.ToString() +"."+ 
						Assembly.GetExecutingAssembly().GetName().Version.Build.ToString();

			// Check if service is installed.
			if (ServiceIsAlive()) {
				InstallServiceBtn.Text = "Remove service";
				RestartServiceBtn.Enabled = true;
			}

			// Check if a user-saved configuration has been saved.
			// I could run guesswork on gateway, but rather ask user.
			if (!File.Exists(this.configfile))
				this.LoadDefaultGateways();

			// Monitor log window.
			timer.Interval = 5000; // 5 seconds
			timer.Elapsed += new System.Timers.ElapsedEventHandler(this.OnTimer);
			timer.Start();
		}

		private void OnTimer(object sender, System.Timers.ElapsedEventArgs args)
		{
			this.loadLogWindow();
			ServiceIsRunning();
			CheckUpdatedStatus();
		}

		private void CheckUpdatedStatus()
		{
			// Load from default config file if there is no custom yet.
			string loadFile;
			if (File.Exists(this.configfile))
				loadFile = this.configfile;
			else
				loadFile = this.configfile_default;
	
			try {
				string[] configLines = File.ReadAllLines(loadFile);

				// Check if box has any new domains.
				foreach (string BoxDomain in DomainListBox.Items.Cast<string>().ToArray()) {
					if (!configLines.Contains("add_domain#"+ BoxDomain)) {
						if (ServiceIsAlive()) {
							RestartServiceBtn.BackColor = Color.Gold;
							RestartServiceBtn.Text = "Restart service (update)";
						}
						break;
					}	
				}

				// Check if box has removed any domains.
				foreach (string configDomain in configLines) {
					if (configDomain.Contains("add_domain#") && !DomainListBox.Items.Contains(configDomain.Substring(11))) {
						if (ServiceIsAlive()) {
							RestartServiceBtn.BackColor = Color.Gold;
							RestartServiceBtn.Text = "Restart service (update)";
						}
						break;
					}	
				}

			} catch (Exception err) {
				this.LogErrorMessage(err.Message, "CheckUpdatedStatus()");
			}
		}

		private void AddDomainBtn_Click(object sender, EventArgs e)
		{
			if (AddDomainTextBox.Text != "" && 
					DomainListBox.FindStringExact(AddDomainTextBox.Text) == ListBox.NoMatches &&
					DomainListBox.Items.Count <= 1000 &&
					FetchTheIP(AddDomainTextBox.Text) != "NoIP")
			{
				DomainListBox.Items.Add(AddDomainTextBox.Text);
				AddDomainTextBox.Text = "";

			} else if (AddDomainTextBox.Text == "") {
				MessageBox.Show("Type a domain first!", "Oops!", MessageBoxButtons.OK);
			} else if (DomainListBox.FindStringExact(AddDomainTextBox.Text) != ListBox.NoMatches) {
				MessageBox.Show("That domain is already in the list!", "Oops!", MessageBoxButtons.OK);
			} else if (DomainListBox.Items.Count >= 1000) {
				MessageBox.Show("1000 domains is unfortunately the limit!", "Oops!", MessageBoxButtons.OK);
			}
		}

		private void RemoveDomainBtn_Click(object sender, EventArgs e)
		{
			if (DomainListBox.SelectedIndex == -1) {
				MessageBox.Show("Select at least one domain to remove!", "Oops!", MessageBoxButtons.OK);
			} else {
				for (int n = DomainListBox.SelectedIndices.Count-1; n>=0; n--) {
					DomainListBox.Items.RemoveAt(DomainListBox.SelectedIndices[n]);
				}
			}
		}

		private void AddDomainTextBox_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
				AddDomainBtn.PerformClick();
		}

		private void InstallServiceBtn_Click(object sender, EventArgs e)
		{
			if (InstallServiceBtn.Text == "Install service") {
				SaveConfiguration();
				InstallServiceBtn.Enabled = false;
				InstallServiceBtn.Text = "Please wait";
				InstallServiceThread.RunWorkerAsync();

			} else if (InstallServiceBtn.Text == "Remove service") {
				InstallServiceBtn.Enabled = false;
				InstallServiceBtn.Text = "Please wait";
				UninstallServiceThread.RunWorkerAsync();
			}
		}

		private void RestartServiceBtn_Click(object sender, EventArgs e)
		{
			SaveConfiguration();
			RestartServiceBtn.Enabled = false;
			InstallServiceBtn.Enabled = false;
			RestartServiceBtn.Text = "Please wait";

			RestartServiceRequest = true;
			UninstallServiceThread.RunWorkerAsync();
		}

		private void InstallServiceThread_DoWork(object sender, DoWorkEventArgs e)
		{
			try {
				DisableButtons(true);
				string servicePath = string.Format(@"{0}\{1}", exepath, "VPNBypassService.exe");
				ManagedInstallerClass.InstallHelper(new string[] { servicePath });

			} catch (Exception err) {
				this.LogErrorMessage(err.Message, "InstallServiceThread_DoWork()");
			}
		}

		private void InstallServiceThread_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			while (!ServiceIsAlive())
				Thread.Sleep(500);
				
			ServiceStatuslbl.Text = "Service is installed.";
			InstallServiceBtn.Text = "Remove service";
			RestartServiceBtn.Text = "Restart service";
			RestartServiceBtn.BackColor = Color.White;
			InstallServiceBtn.Enabled = true;
			RestartServiceBtn.Enabled = true;
			DisableButtons(false);
		}

		private void UninstallServiceThread_DoWork(object sender, DoWorkEventArgs e)
		{
			try {
				DisableButtons(true);
				string servicePath = string.Format(@"{0}\{1}", exepath, "VPNBypassService.exe");
				ManagedInstallerClass.InstallHelper(new string[] { "/u", servicePath });

			} catch (Exception err) {
				this.LogErrorMessage(err.Message, "UninstallServiceThread_DoWork()");
			}
		}

		private void UninstallServiceThread_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			while (ServiceIsAlive())
				Thread.Sleep(500);
			
			ServiceStatuslbl.Text = "Service is removed.";
			InstallServiceBtn.Text = "Install service";
			InstallServiceBtn.Enabled = true;
			RestartServiceBtn.Enabled = false;
			DisableButtons(false);

			if (RestartServiceRequest) {
				RestartServiceRequest = false;
				InstallServiceThread.RunWorkerAsync();
			}
		}

		private void DisableButtons(bool b)
		{
			if (b) {
				AddDomainBtn.Enabled = false;
				RemoveDomainBtn.Enabled = false;
				logWindowClrBtn.Enabled = false;
				btnSetGateway.Enabled = false;
			} else {
				btnSetGateway.Enabled = true;
				AddDomainBtn.Enabled = true;
				RemoveDomainBtn.Enabled = true;
				logWindowClrBtn.Enabled = true;
			}
		}

		private bool ServiceIsAlive()
		{
			try {
			ServiceController ctl = ServiceController.GetServices().FirstOrDefault(s => s.ServiceName == "VPNBypassService");

			if (ctl != null)
				return true;
			else 
				return false;

			} catch (Exception e) {
				this.LogErrorMessage(e.Message, "ServiceIsAlive()");
				return false;
			}
		}

		private void ServiceIsRunning()
		{
			try {
			ServiceController ctl = ServiceController.GetServices().FirstOrDefault(s => s.ServiceName == "VPNBypassService");

			if (ctl != null && ctl.Status == ServiceControllerStatus.Running) {
				ServiceStatuslbl.Text = "Service is running.";

			} else if (ctl != null && ctl.Status == ServiceControllerStatus.Stopped) {
				ServiceStatuslbl.ForeColor = Color.Red;
				ServiceStatuslbl.Text = "Service is stopped.";
				ServiceStatuslbl.ForeColor = Color.Lime;

			}} catch (Exception e) {
				this.LogErrorMessage(e.Message, "ServiceIsRunning()");
			}
		}

		private void SaveConfiguration()
		{
			try {
				// Get domains.
				string FileData = "";
				for (int n=0; n<DomainListBox.Items.Count; n++)
					FileData += "add_domain#" + DomainListBox.Items[n].ToString() + Environment.NewLine;

				// Check valid gateway.
				IPAddress _ip;
				if (!IPAddress.TryParse(TargetGatewayIPtxt.Text, out _ip)) {
					ServiceStatuslbl.Text = "!Invalid gateway IP!";
					InstallServiceBtn.Enabled = false;
					MessageBox.Show("Invalid gateway IP.", "Oops!", MessageBoxButtons.OK);
					return;
				}

				// Get gateway.
				FileData += "gateway#" + TargetGatewayIPtxt.Text + Environment.NewLine;
				File.WriteAllText(this.configfile, FileData);
				
				if (ServiceIsAlive()) {
					RestartServiceBtn.BackColor = Color.Gold;
					RestartServiceBtn.Text = "Restart service (update)";
				}

			} catch (Exception err) {
				this.LogErrorMessage(err.Message, "SaveConfiguration()");
			}
		}

		private void LoadConfiguration()
		{
			// Load from default config file if there is no custom yet.
			string loadFile;
			if (File.Exists(this.configfile))
				loadFile = this.configfile;
			else
				loadFile = this.configfile_default;
	
			// Parse configuration file and load UI.
			try {
				string[] configLines = File.ReadAllLines(loadFile);
				for (int n=0; n<configLines.Count(); n++) {
					if (configLines[n].StartsWith("add_domain#"))
						DomainListBox.Items.Add(configLines[n].Substring(11));	
					if (configLines[n].StartsWith("gateway#"))
						TargetGatewayIPtxt.Text = configLines[n].Substring(8);
				}

			} catch (Exception err) {
				this.LogErrorMessage(err.Message, "LoadConfiguration()");
			}
		}

		private void loadLogWindow()
		{
			try {
				if (File.Exists(this.logfile_service)) {
					logWindow.Clear();
					logWindow.AppendText(File.ReadAllText(logfile_service));
				} else {
					logWindow.Text = "No data yet.";
				}

			} catch (Exception e) {
				this.LogErrorMessage(e.Message, "loadLogWindow()");
			}
		}

		private string FetchTheIP(string host)
		{
			try {
				IPAddress IpInfo = Dns.GetHostAddresses(host)[0];
				return IpInfo.ToString();

			} catch (Exception) {
				MessageBox.Show("Could not resolve that!", "Oops!", MessageBoxButtons.OK);
				return "NoIP";
			}
		}

		private void LoadDefaultGateways()
		{
			try {
				int n;
				NetworkInterface[] _if = NetworkInterface.GetAllNetworkInterfaces();
				for (n=0; n<_if.Count(); n++) {
					if (_if[n].OperationalStatus == OperationalStatus.Up &&
						_if[n].GetIPProperties().GatewayAddresses.Count() > 0)
					{
						TargetGatewayIPtxt.Text = _if[n].GetIPProperties().GatewayAddresses.FirstOrDefault().Address.ToString();
						break;
					}
				}
				
				MessageBox.Show("Gateway IP: "+ TargetGatewayIPtxt.Text +" automatically detected.", "Gateway detection", MessageBoxButtons.OK);

			} catch (Exception e) {
				this.LogErrorMessage(e.Message, "LoadDefaultGateways()");
			}
		}

		private void LogErrorMessage(string err, string wut)
		{
			File.AppendAllText(
				logfile, wut + " ran into an error: " + 
				err + " " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + 
				Environment.NewLine
			);
			
			MessageBox.Show(
				"An error occurred. It has been logged in: " +
				Environment.NewLine + 
				logfile, 
				"Oops!", 
				MessageBoxButtons.OK
			);
		}

		private void logWindowClrBtn_Click(object sender, EventArgs e)
		{
			File.Delete(this.logfile_service);
			logWindow.Text = "";
		}

		private void logWindow_VisibleChanged(object sender, EventArgs e)
		{
			//logWindow.SelectionStart = logWindow.Text.Length;
			//logWindow.ScrollToCaret();
			SendMessage(logWindow.Handle, 277, (IntPtr)7, IntPtr.Zero);
		}

		private void btnSetGateway_Click(object sender, EventArgs e)
		{
			SaveConfiguration();
		}
	}
}
