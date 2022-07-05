namespace StorageAndTrade
{
	partial class ConfigurationSelectionParam
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfigurationSelectionParam));
            this.labelPostgreSQLServer = new System.Windows.Forms.Label();
            this.textBoxPostgreSQLServer = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonConnect = new System.Windows.Forms.Button();
            this.textBoxPostgreSQLPort = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxPostgreSQLDataBase = new System.Windows.Forms.TextBox();
            this.labelPostgreSQLDataBase = new System.Windows.Forms.Label();
            this.textBoxPostgreSQLPassword = new System.Windows.Forms.TextBox();
            this.labelPostgreSQLPassword = new System.Windows.Forms.Label();
            this.textBoxPostgreSQLLogin = new System.Windows.Forms.TextBox();
            this.labelPostgreSQLLogin = new System.Windows.Forms.Label();
            this.textBoxConfName = new System.Windows.Forms.TextBox();
            this.labelConfName = new System.Windows.Forms.Label();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonClose = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelPostgreSQLServer
            // 
            this.labelPostgreSQLServer.AutoSize = true;
            this.labelPostgreSQLServer.Location = new System.Drawing.Point(12, 25);
            this.labelPostgreSQLServer.Name = "labelPostgreSQLServer";
            this.labelPostgreSQLServer.Size = new System.Drawing.Size(47, 13);
            this.labelPostgreSQLServer.TabIndex = 3;
            this.labelPostgreSQLServer.Text = "Сервер:";
            // 
            // textBoxPostgreSQLServer
            // 
            this.textBoxPostgreSQLServer.Location = new System.Drawing.Point(82, 22);
            this.textBoxPostgreSQLServer.Name = "textBoxPostgreSQLServer";
            this.textBoxPostgreSQLServer.Size = new System.Drawing.Size(257, 20);
            this.textBoxPostgreSQLServer.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.buttonConnect);
            this.groupBox1.Controls.Add(this.textBoxPostgreSQLPort);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.textBoxPostgreSQLDataBase);
            this.groupBox1.Controls.Add(this.labelPostgreSQLDataBase);
            this.groupBox1.Controls.Add(this.textBoxPostgreSQLPassword);
            this.groupBox1.Controls.Add(this.labelPostgreSQLPassword);
            this.groupBox1.Controls.Add(this.textBoxPostgreSQLLogin);
            this.groupBox1.Controls.Add(this.labelPostgreSQLLogin);
            this.groupBox1.Controls.Add(this.textBoxPostgreSQLServer);
            this.groupBox1.Controls.Add(this.labelPostgreSQLServer);
            this.groupBox1.Location = new System.Drawing.Point(12, 59);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(594, 159);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "База даних PostgreSQL";
            // 
            // buttonConnect
            // 
            this.buttonConnect.Location = new System.Drawing.Point(431, 119);
            this.buttonConnect.Name = "buttonConnect";
            this.buttonConnect.Size = new System.Drawing.Size(157, 28);
            this.buttonConnect.TabIndex = 9;
            this.buttonConnect.Text = "Створити базу даних";
            this.buttonConnect.UseVisualStyleBackColor = true;
            this.buttonConnect.Click += new System.EventHandler(this.buttonConnect_Click);
            // 
            // textBoxPostgreSQLPort
            // 
            this.textBoxPostgreSQLPort.Location = new System.Drawing.Point(82, 100);
            this.textBoxPostgreSQLPort.Name = "textBoxPostgreSQLPort";
            this.textBoxPostgreSQLPort.Size = new System.Drawing.Size(257, 20);
            this.textBoxPostgreSQLPort.TabIndex = 8;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 103);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Порт:";
            // 
            // textBoxPostgreSQLDataBase
            // 
            this.textBoxPostgreSQLDataBase.Location = new System.Drawing.Point(82, 127);
            this.textBoxPostgreSQLDataBase.Name = "textBoxPostgreSQLDataBase";
            this.textBoxPostgreSQLDataBase.Size = new System.Drawing.Size(257, 20);
            this.textBoxPostgreSQLDataBase.TabIndex = 3;
            // 
            // labelPostgreSQLDataBase
            // 
            this.labelPostgreSQLDataBase.AutoSize = true;
            this.labelPostgreSQLDataBase.Location = new System.Drawing.Point(12, 130);
            this.labelPostgreSQLDataBase.Name = "labelPostgreSQLDataBase";
            this.labelPostgreSQLDataBase.Size = new System.Drawing.Size(67, 13);
            this.labelPostgreSQLDataBase.TabIndex = 7;
            this.labelPostgreSQLDataBase.Text = "База даних:";
            // 
            // textBoxPostgreSQLPassword
            // 
            this.textBoxPostgreSQLPassword.Location = new System.Drawing.Point(82, 74);
            this.textBoxPostgreSQLPassword.Name = "textBoxPostgreSQLPassword";
            this.textBoxPostgreSQLPassword.Size = new System.Drawing.Size(257, 20);
            this.textBoxPostgreSQLPassword.TabIndex = 2;
            // 
            // labelPostgreSQLPassword
            // 
            this.labelPostgreSQLPassword.AutoSize = true;
            this.labelPostgreSQLPassword.Location = new System.Drawing.Point(12, 77);
            this.labelPostgreSQLPassword.Name = "labelPostgreSQLPassword";
            this.labelPostgreSQLPassword.Size = new System.Drawing.Size(48, 13);
            this.labelPostgreSQLPassword.TabIndex = 7;
            this.labelPostgreSQLPassword.Text = "Пароль:";
            // 
            // textBoxPostgreSQLLogin
            // 
            this.textBoxPostgreSQLLogin.Location = new System.Drawing.Point(82, 48);
            this.textBoxPostgreSQLLogin.Name = "textBoxPostgreSQLLogin";
            this.textBoxPostgreSQLLogin.Size = new System.Drawing.Size(257, 20);
            this.textBoxPostgreSQLLogin.TabIndex = 1;
            // 
            // labelPostgreSQLLogin
            // 
            this.labelPostgreSQLLogin.AutoSize = true;
            this.labelPostgreSQLLogin.Location = new System.Drawing.Point(12, 51);
            this.labelPostgreSQLLogin.Name = "labelPostgreSQLLogin";
            this.labelPostgreSQLLogin.Size = new System.Drawing.Size(37, 13);
            this.labelPostgreSQLLogin.TabIndex = 5;
            this.labelPostgreSQLLogin.Text = "Логін:";
            // 
            // textBoxConfName
            // 
            this.textBoxConfName.Location = new System.Drawing.Point(94, 20);
            this.textBoxConfName.Name = "textBoxConfName";
            this.textBoxConfName.Size = new System.Drawing.Size(512, 20);
            this.textBoxConfName.TabIndex = 0;
            // 
            // labelConfName
            // 
            this.labelConfName.AutoSize = true;
            this.labelConfName.Location = new System.Drawing.Point(14, 23);
            this.labelConfName.Name = "labelConfName";
            this.labelConfName.Size = new System.Drawing.Size(42, 13);
            this.labelConfName.TabIndex = 7;
            this.labelConfName.Text = "Назва:";
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(12, 235);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(110, 28);
            this.buttonOK.TabIndex = 3;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonClose
            // 
            this.buttonClose.Location = new System.Drawing.Point(496, 235);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(110, 28);
            this.buttonClose.TabIndex = 8;
            this.buttonClose.Text = "Закрити";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // ConfigurationSelectionParam
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(613, 275);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.textBoxConfName);
            this.Controls.Add(this.labelConfName);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ConfigurationSelectionParam";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Зберігання та Торгівля - Параметри";
            this.Load += new System.EventHandler(this.ConfigurationSelectionParam_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.Label labelPostgreSQLServer;
		private System.Windows.Forms.TextBox textBoxPostgreSQLServer;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.TextBox textBoxPostgreSQLDataBase;
		private System.Windows.Forms.Label labelPostgreSQLDataBase;
		private System.Windows.Forms.TextBox textBoxPostgreSQLPassword;
		private System.Windows.Forms.Label labelPostgreSQLPassword;
		private System.Windows.Forms.TextBox textBoxPostgreSQLLogin;
		private System.Windows.Forms.Label labelPostgreSQLLogin;
		private System.Windows.Forms.TextBox textBoxConfName;
		private System.Windows.Forms.Label labelConfName;
		private System.Windows.Forms.Button buttonOK;
		private System.Windows.Forms.Button buttonClose;
		private System.Windows.Forms.Button buttonConnect;
		private System.Windows.Forms.TextBox textBoxPostgreSQLPort;
		private System.Windows.Forms.Label label1;
	}
}