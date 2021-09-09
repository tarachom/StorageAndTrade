
namespace StorageAndTrade
{
    partial class FormStorageAndTrade
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.довідникиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.касиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.валютиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.пакуванняОдиниціВиміруToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.оганізаціїToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.довідникиToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1040, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // довідникиToolStripMenuItem
            // 
            this.довідникиToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.касиToolStripMenuItem,
            this.валютиToolStripMenuItem,
            this.пакуванняОдиниціВиміруToolStripMenuItem,
            this.оганізаціїToolStripMenuItem});
            this.довідникиToolStripMenuItem.Name = "довідникиToolStripMenuItem";
            this.довідникиToolStripMenuItem.Size = new System.Drawing.Size(76, 20);
            this.довідникиToolStripMenuItem.Text = "Довідники";
            // 
            // касиToolStripMenuItem
            // 
            this.касиToolStripMenuItem.Image = global::StorageAndTrade.Properties.Resources.blog;
            this.касиToolStripMenuItem.Name = "касиToolStripMenuItem";
            this.касиToolStripMenuItem.Size = new System.Drawing.Size(221, 22);
            this.касиToolStripMenuItem.Text = "Каси";
            this.касиToolStripMenuItem.Click += new System.EventHandler(this.касиToolStripMenuItem_Click);
            // 
            // валютиToolStripMenuItem
            // 
            this.валютиToolStripMenuItem.Image = global::StorageAndTrade.Properties.Resources.coins;
            this.валютиToolStripMenuItem.Name = "валютиToolStripMenuItem";
            this.валютиToolStripMenuItem.Size = new System.Drawing.Size(221, 22);
            this.валютиToolStripMenuItem.Text = "Валюти";
            this.валютиToolStripMenuItem.Click += new System.EventHandler(this.валютиToolStripMenuItem_Click);
            // 
            // пакуванняОдиниціВиміруToolStripMenuItem
            // 
            this.пакуванняОдиниціВиміруToolStripMenuItem.Image = global::StorageAndTrade.Properties.Resources.drawer;
            this.пакуванняОдиниціВиміруToolStripMenuItem.Name = "пакуванняОдиниціВиміруToolStripMenuItem";
            this.пакуванняОдиниціВиміруToolStripMenuItem.Size = new System.Drawing.Size(221, 22);
            this.пакуванняОдиниціВиміруToolStripMenuItem.Text = "Пакування одиниці виміру";
            this.пакуванняОдиниціВиміруToolStripMenuItem.Click += new System.EventHandler(this.пакуванняОдиниціВиміруToolStripMenuItem_Click);
            // 
            // оганізаціїToolStripMenuItem
            // 
            this.оганізаціїToolStripMenuItem.Image = global::StorageAndTrade.Properties.Resources.user;
            this.оганізаціїToolStripMenuItem.Name = "оганізаціїToolStripMenuItem";
            this.оганізаціїToolStripMenuItem.Size = new System.Drawing.Size(221, 22);
            this.оганізаціїToolStripMenuItem.Text = "Оганізації";
            this.оганізаціїToolStripMenuItem.Click += new System.EventHandler(this.оганізаціїToolStripMenuItem_Click);
            // 
            // FormStorageAndTrade
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1040, 634);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FormStorageAndTrade";
            this.Text = "Зберігання та Торгівля";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormStorageAndTrade_FormClosing);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem довідникиToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem касиToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem валютиToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem пакуванняОдиниціВиміруToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem оганізаціїToolStripMenuItem;
    }
}

