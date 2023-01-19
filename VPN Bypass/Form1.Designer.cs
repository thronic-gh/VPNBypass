namespace VPN_Bypass
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
			this.AddDomainTextBox = new System.Windows.Forms.TextBox();
			this.AddDomainBtn = new System.Windows.Forms.Button();
			this.DomainListBox = new System.Windows.Forms.ListBox();
			this.RemoveDomainBtn = new System.Windows.Forms.Button();
			this.InstallServiceBtn = new System.Windows.Forms.Button();
			this.UninstallServiceThread = new System.ComponentModel.BackgroundWorker();
			this.InstallServiceThread = new System.ComponentModel.BackgroundWorker();
			this.ServiceStatuslbl = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.TargetGatewayIPtxt = new System.Windows.Forms.TextBox();
			this.logWindow = new System.Windows.Forms.RichTextBox();
			this.logWindowClrBtn = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.RestartServiceBtn = new System.Windows.Forms.Button();
			this.btnSetGateway = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// AddDomainTextBox
			// 
			this.AddDomainTextBox.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.AddDomainTextBox.Location = new System.Drawing.Point(15, 29);
			this.AddDomainTextBox.Name = "AddDomainTextBox";
			this.AddDomainTextBox.Size = new System.Drawing.Size(139, 21);
			this.AddDomainTextBox.TabIndex = 1;
			this.AddDomainTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.AddDomainTextBox_KeyDown);
			// 
			// AddDomainBtn
			// 
			this.AddDomainBtn.BackColor = System.Drawing.Color.White;
			this.AddDomainBtn.Cursor = System.Windows.Forms.Cursors.Hand;
			this.AddDomainBtn.FlatAppearance.BorderSize = 0;
			this.AddDomainBtn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DeepSkyBlue;
			this.AddDomainBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.AddDomainBtn.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.AddDomainBtn.ForeColor = System.Drawing.Color.Black;
			this.AddDomainBtn.Location = new System.Drawing.Point(160, 29);
			this.AddDomainBtn.Name = "AddDomainBtn";
			this.AddDomainBtn.Size = new System.Drawing.Size(38, 21);
			this.AddDomainBtn.TabIndex = 2;
			this.AddDomainBtn.Text = "Add";
			this.AddDomainBtn.UseVisualStyleBackColor = false;
			this.AddDomainBtn.Click += new System.EventHandler(this.AddDomainBtn_Click);
			// 
			// DomainListBox
			// 
			this.DomainListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.DomainListBox.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.DomainListBox.FormattingEnabled = true;
			this.DomainListBox.Location = new System.Drawing.Point(14, 55);
			this.DomainListBox.Name = "DomainListBox";
			this.DomainListBox.ScrollAlwaysVisible = true;
			this.DomainListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.DomainListBox.Size = new System.Drawing.Size(184, 251);
			this.DomainListBox.TabIndex = 3;
			// 
			// RemoveDomainBtn
			// 
			this.RemoveDomainBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.RemoveDomainBtn.BackColor = System.Drawing.Color.White;
			this.RemoveDomainBtn.Cursor = System.Windows.Forms.Cursors.Hand;
			this.RemoveDomainBtn.FlatAppearance.BorderSize = 0;
			this.RemoveDomainBtn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DeepSkyBlue;
			this.RemoveDomainBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.RemoveDomainBtn.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.RemoveDomainBtn.ForeColor = System.Drawing.Color.Black;
			this.RemoveDomainBtn.Location = new System.Drawing.Point(15, 312);
			this.RemoveDomainBtn.Name = "RemoveDomainBtn";
			this.RemoveDomainBtn.Size = new System.Drawing.Size(183, 24);
			this.RemoveDomainBtn.TabIndex = 4;
			this.RemoveDomainBtn.Text = "Remove selected";
			this.RemoveDomainBtn.UseVisualStyleBackColor = false;
			this.RemoveDomainBtn.Click += new System.EventHandler(this.RemoveDomainBtn_Click);
			// 
			// InstallServiceBtn
			// 
			this.InstallServiceBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.InstallServiceBtn.BackColor = System.Drawing.Color.White;
			this.InstallServiceBtn.Cursor = System.Windows.Forms.Cursors.Hand;
			this.InstallServiceBtn.FlatAppearance.BorderSize = 0;
			this.InstallServiceBtn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DeepSkyBlue;
			this.InstallServiceBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.InstallServiceBtn.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.InstallServiceBtn.ForeColor = System.Drawing.Color.Black;
			this.InstallServiceBtn.Location = new System.Drawing.Point(15, 399);
			this.InstallServiceBtn.Name = "InstallServiceBtn";
			this.InstallServiceBtn.Size = new System.Drawing.Size(183, 24);
			this.InstallServiceBtn.TabIndex = 7;
			this.InstallServiceBtn.Text = "Install service";
			this.InstallServiceBtn.UseVisualStyleBackColor = false;
			this.InstallServiceBtn.Click += new System.EventHandler(this.InstallServiceBtn_Click);
			// 
			// UninstallServiceThread
			// 
			this.UninstallServiceThread.DoWork += new System.ComponentModel.DoWorkEventHandler(this.UninstallServiceThread_DoWork);
			this.UninstallServiceThread.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.UninstallServiceThread_RunWorkerCompleted);
			// 
			// InstallServiceThread
			// 
			this.InstallServiceThread.DoWork += new System.ComponentModel.DoWorkEventHandler(this.InstallServiceThread_DoWork);
			this.InstallServiceThread.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.InstallServiceThread_RunWorkerCompleted);
			// 
			// ServiceStatuslbl
			// 
			this.ServiceStatuslbl.AutoSize = true;
			this.ServiceStatuslbl.BackColor = System.Drawing.Color.Transparent;
			this.ServiceStatuslbl.ForeColor = System.Drawing.Color.Lime;
			this.ServiceStatuslbl.Location = new System.Drawing.Point(204, 13);
			this.ServiceStatuslbl.Name = "ServiceStatuslbl";
			this.ServiceStatuslbl.Size = new System.Drawing.Size(0, 13);
			this.ServiceStatuslbl.TabIndex = 19;
			// 
			// label2
			// 
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label2.AutoSize = true;
			this.label2.BackColor = System.Drawing.Color.Transparent;
			this.label2.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.ForeColor = System.Drawing.Color.White;
			this.label2.Location = new System.Drawing.Point(12, 355);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(126, 14);
			this.label2.TabIndex = 20;
			this.label2.Text = "Target Gateway IP";
			// 
			// TargetGatewayIPtxt
			// 
			this.TargetGatewayIPtxt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.TargetGatewayIPtxt.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.TargetGatewayIPtxt.Location = new System.Drawing.Point(15, 372);
			this.TargetGatewayIPtxt.Name = "TargetGatewayIPtxt";
			this.TargetGatewayIPtxt.Size = new System.Drawing.Size(139, 21);
			this.TargetGatewayIPtxt.TabIndex = 5;
			// 
			// logWindow
			// 
			this.logWindow.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.logWindow.BackColor = System.Drawing.Color.Black;
			this.logWindow.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.logWindow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
			this.logWindow.Location = new System.Drawing.Point(204, 29);
			this.logWindow.Name = "logWindow";
			this.logWindow.ReadOnly = true;
			this.logWindow.Size = new System.Drawing.Size(598, 454);
			this.logWindow.TabIndex = 22;
			this.logWindow.TabStop = false;
			this.logWindow.Text = "";
			this.logWindow.SizeChanged += new System.EventHandler(this.logWindow_VisibleChanged);
			this.logWindow.TextChanged += new System.EventHandler(this.logWindow_VisibleChanged);
			this.logWindow.VisibleChanged += new System.EventHandler(this.logWindow_VisibleChanged);
			// 
			// logWindowClrBtn
			// 
			this.logWindowClrBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.logWindowClrBtn.BackColor = System.Drawing.Color.White;
			this.logWindowClrBtn.Cursor = System.Windows.Forms.Cursors.Hand;
			this.logWindowClrBtn.FlatAppearance.BorderSize = 0;
			this.logWindowClrBtn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gold;
			this.logWindowClrBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.logWindowClrBtn.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.logWindowClrBtn.ForeColor = System.Drawing.Color.Black;
			this.logWindowClrBtn.Location = new System.Drawing.Point(15, 459);
			this.logWindowClrBtn.Name = "logWindowClrBtn";
			this.logWindowClrBtn.Size = new System.Drawing.Size(183, 24);
			this.logWindowClrBtn.TabIndex = 9;
			this.logWindowClrBtn.TabStop = false;
			this.logWindowClrBtn.Text = "Reset log";
			this.logWindowClrBtn.UseVisualStyleBackColor = false;
			this.logWindowClrBtn.Click += new System.EventHandler(this.logWindowClrBtn_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.BackColor = System.Drawing.Color.Transparent;
			this.label1.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.ForeColor = System.Drawing.Color.White;
			this.label1.Location = new System.Drawing.Point(12, 12);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(133, 14);
			this.label1.TabIndex = 25;
			this.label1.Text = "VPN Bypass domains";
			// 
			// RestartServiceBtn
			// 
			this.RestartServiceBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.RestartServiceBtn.BackColor = System.Drawing.Color.White;
			this.RestartServiceBtn.Cursor = System.Windows.Forms.Cursors.Hand;
			this.RestartServiceBtn.Enabled = false;
			this.RestartServiceBtn.FlatAppearance.BorderSize = 0;
			this.RestartServiceBtn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DeepSkyBlue;
			this.RestartServiceBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.RestartServiceBtn.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.RestartServiceBtn.ForeColor = System.Drawing.Color.Black;
			this.RestartServiceBtn.Location = new System.Drawing.Point(15, 429);
			this.RestartServiceBtn.Name = "RestartServiceBtn";
			this.RestartServiceBtn.Size = new System.Drawing.Size(183, 24);
			this.RestartServiceBtn.TabIndex = 8;
			this.RestartServiceBtn.Text = "Restart service";
			this.RestartServiceBtn.UseVisualStyleBackColor = false;
			this.RestartServiceBtn.Click += new System.EventHandler(this.RestartServiceBtn_Click);
			// 
			// btnSetGateway
			// 
			this.btnSetGateway.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnSetGateway.BackColor = System.Drawing.Color.White;
			this.btnSetGateway.Cursor = System.Windows.Forms.Cursors.Hand;
			this.btnSetGateway.FlatAppearance.BorderSize = 0;
			this.btnSetGateway.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DeepSkyBlue;
			this.btnSetGateway.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnSetGateway.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnSetGateway.Location = new System.Drawing.Point(160, 372);
			this.btnSetGateway.Name = "btnSetGateway";
			this.btnSetGateway.Size = new System.Drawing.Size(38, 21);
			this.btnSetGateway.TabIndex = 6;
			this.btnSetGateway.Text = "Set";
			this.btnSetGateway.UseVisualStyleBackColor = false;
			this.btnSetGateway.Click += new System.EventHandler(this.btnSetGateway_Click);
			// 
			// Form1
			// 
			this.AllowDrop = true;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Black;
			this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
			this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.ClientSize = new System.Drawing.Size(806, 495);
			this.Controls.Add(this.btnSetGateway);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.logWindowClrBtn);
			this.Controls.Add(this.logWindow);
			this.Controls.Add(this.TargetGatewayIPtxt);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.ServiceStatuslbl);
			this.Controls.Add(this.RestartServiceBtn);
			this.Controls.Add(this.InstallServiceBtn);
			this.Controls.Add(this.RemoveDomainBtn);
			this.Controls.Add(this.DomainListBox);
			this.Controls.Add(this.AddDomainBtn);
			this.Controls.Add(this.AddDomainTextBox);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimumSize = new System.Drawing.Size(822, 388);
			this.Name = "Form1";
			this.Text = "VPN Bypass";
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox AddDomainTextBox;
        private System.Windows.Forms.Button AddDomainBtn;
        private System.Windows.Forms.ListBox DomainListBox;
	private System.Windows.Forms.Button RemoveDomainBtn;
        private System.Windows.Forms.Button InstallServiceBtn;
	private System.ComponentModel.BackgroundWorker UninstallServiceThread;
	private System.ComponentModel.BackgroundWorker InstallServiceThread;
	private System.Windows.Forms.Label ServiceStatuslbl;
	private System.Windows.Forms.Label label2;
	private System.Windows.Forms.TextBox TargetGatewayIPtxt;
	private System.Windows.Forms.RichTextBox logWindow;
	private System.Windows.Forms.Button logWindowClrBtn;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button RestartServiceBtn;
		private System.Windows.Forms.Button btnSetGateway;
	}
}

