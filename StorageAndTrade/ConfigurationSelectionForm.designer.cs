namespace StorageAndTrade
{
	partial class ConfigurationSelectionForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfigurationSelectionForm));
            this.listBoxConfiguration = new System.Windows.Forms.ListBox();
            this.buttonAddConf = new System.Windows.Forms.Button();
            this.buttonOpenConf = new System.Windows.Forms.Button();
            this.buttonDelete = new System.Windows.Forms.Button();
            this.buttonCopy = new System.Windows.Forms.Button();
            this.button_OpenConfigurator = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listBoxConfiguration
            // 
            this.listBoxConfiguration.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.listBoxConfiguration.FormattingEnabled = true;
            this.listBoxConfiguration.ItemHeight = 19;
            this.listBoxConfiguration.Location = new System.Drawing.Point(12, 12);
            this.listBoxConfiguration.Name = "listBoxConfiguration";
            this.listBoxConfiguration.Size = new System.Drawing.Size(502, 270);
            this.listBoxConfiguration.TabIndex = 0;
            this.listBoxConfiguration.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listBoxConfiguration_KeyDown);
            this.listBoxConfiguration.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listBoxConfiguration_MouseDoubleClick);
            // 
            // buttonAddConf
            // 
            this.buttonAddConf.Location = new System.Drawing.Point(520, 180);
            this.buttonAddConf.Name = "buttonAddConf";
            this.buttonAddConf.Size = new System.Drawing.Size(97, 30);
            this.buttonAddConf.TabIndex = 1;
            this.buttonAddConf.Text = "Додати";
            this.buttonAddConf.UseVisualStyleBackColor = true;
            this.buttonAddConf.Click += new System.EventHandler(this.buttonAddConf_Click);
            // 
            // buttonOpenConf
            // 
            this.buttonOpenConf.Location = new System.Drawing.Point(520, 12);
            this.buttonOpenConf.Name = "buttonOpenConf";
            this.buttonOpenConf.Size = new System.Drawing.Size(97, 30);
            this.buttonOpenConf.TabIndex = 3;
            this.buttonOpenConf.Text = "Відкрити";
            this.buttonOpenConf.UseVisualStyleBackColor = true;
            this.buttonOpenConf.Click += new System.EventHandler(this.buttonOpenConf_Click);
            // 
            // buttonDelete
            // 
            this.buttonDelete.Location = new System.Drawing.Point(520, 252);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(97, 30);
            this.buttonDelete.TabIndex = 4;
            this.buttonDelete.Text = "Видалити";
            this.buttonDelete.UseVisualStyleBackColor = true;
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // buttonCopy
            // 
            this.buttonCopy.Location = new System.Drawing.Point(520, 216);
            this.buttonCopy.Name = "buttonCopy";
            this.buttonCopy.Size = new System.Drawing.Size(97, 30);
            this.buttonCopy.TabIndex = 5;
            this.buttonCopy.Text = "Копіювати";
            this.buttonCopy.UseVisualStyleBackColor = true;
            this.buttonCopy.Click += new System.EventHandler(this.buttonCopy_Click);
            // 
            // button_OpenConfigurator
            // 
            this.button_OpenConfigurator.Location = new System.Drawing.Point(520, 144);
            this.button_OpenConfigurator.Name = "button_OpenConfigurator";
            this.button_OpenConfigurator.Size = new System.Drawing.Size(97, 30);
            this.button_OpenConfigurator.TabIndex = 6;
            this.button_OpenConfigurator.Text = "Конфігуратор";
            this.button_OpenConfigurator.UseVisualStyleBackColor = true;
            this.button_OpenConfigurator.Click += new System.EventHandler(this.button_OpenConfigurator_Click);
            // 
            // ConfigurationSelectionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(623, 292);
            this.Controls.Add(this.button_OpenConfigurator);
            this.Controls.Add(this.buttonCopy);
            this.Controls.Add(this.buttonDelete);
            this.Controls.Add(this.buttonOpenConf);
            this.Controls.Add(this.buttonAddConf);
            this.Controls.Add(this.listBoxConfiguration);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ConfigurationSelectionForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Зберігання та Торгівля - Бази даних";
            this.Load += new System.EventHandler(this.ConfigurationSelectionForm_Load);
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ListBox listBoxConfiguration;
		private System.Windows.Forms.Button buttonAddConf;
		private System.Windows.Forms.Button buttonOpenConf;
        private System.Windows.Forms.Button buttonDelete;
        private System.Windows.Forms.Button buttonCopy;
        private System.Windows.Forms.Button button_OpenConfigurator;
    }
}