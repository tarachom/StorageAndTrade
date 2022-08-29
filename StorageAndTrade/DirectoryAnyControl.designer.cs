
namespace StorageAndTrade
{
	partial class DirectoryAnyControl
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.buttonOpen = new System.Windows.Forms.Button();
            this.buttonFind = new System.Windows.Forms.Button();
            this.listBoxAny = new System.Windows.Forms.ListBox();
            this.buttonClear = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonOpen
            // 
            this.buttonOpen.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOpen.Location = new System.Drawing.Point(317, 0);
            this.buttonOpen.Name = "buttonOpen";
            this.buttonOpen.Size = new System.Drawing.Size(24, 24);
            this.buttonOpen.TabIndex = 1;
            this.buttonOpen.Text = "..";
            this.buttonOpen.UseVisualStyleBackColor = true;
            this.buttonOpen.Click += new System.EventHandler(this.buttonOpen_Click);
            // 
            // buttonFind
            // 
            this.buttonFind.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonFind.Enabled = false;
            this.buttonFind.Location = new System.Drawing.Point(317, 25);
            this.buttonFind.Name = "buttonFind";
            this.buttonFind.Size = new System.Drawing.Size(24, 24);
            this.buttonFind.TabIndex = 3;
            this.buttonFind.Text = "f";
            this.buttonFind.UseVisualStyleBackColor = true;
            this.buttonFind.Click += new System.EventHandler(this.buttonFind_Click);
            // 
            // listBoxAny
            // 
            this.listBoxAny.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxAny.FormattingEnabled = true;
            this.listBoxAny.Location = new System.Drawing.Point(0, 1);
            this.listBoxAny.Name = "listBoxAny";
            this.listBoxAny.Size = new System.Drawing.Size(315, 95);
            this.listBoxAny.TabIndex = 4;
            // 
            // buttonClear
            // 
            this.buttonClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClear.Location = new System.Drawing.Point(317, 50);
            this.buttonClear.Name = "buttonClear";
            this.buttonClear.Size = new System.Drawing.Size(24, 24);
            this.buttonClear.TabIndex = 5;
            this.buttonClear.Text = "X";
            this.buttonClear.UseVisualStyleBackColor = true;
            this.buttonClear.Click += new System.EventHandler(this.buttonClear_Click);
            // 
            // DirectoryAnyControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.buttonClear);
            this.Controls.Add(this.listBoxAny);
            this.Controls.Add(this.buttonFind);
            this.Controls.Add(this.buttonOpen);
            this.Name = "DirectoryAnyControl";
            this.Size = new System.Drawing.Size(341, 98);
            this.ResumeLayout(false);

		}

		#endregion
		private System.Windows.Forms.Button buttonOpen;
        private System.Windows.Forms.Button buttonFind;
        private System.Windows.Forms.ListBox listBoxAny;
        private System.Windows.Forms.Button buttonClear;
    }
}
