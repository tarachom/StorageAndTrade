
namespace StorageAndTrade
{
    partial class Form_НоменклатураПапкиВибір
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_НоменклатураПапкиВибір));
            this.panel2 = new System.Windows.Forms.Panel();
            this.Номенклатура_Папки_Дерево = new StorageAndTrade.Form_Номенклатура_Папки_Дерево();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.Номенклатура_Папки_Дерево);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(555, 738);
            this.panel2.TabIndex = 2;
            // 
            // Номенклатура_Папки_Дерево
            // 
            this.Номенклатура_Папки_Дерево.CallBack_AfterSelect = null;
            this.Номенклатура_Папки_Дерево.CallBack_DoubleClick = null;
            this.Номенклатура_Папки_Дерево.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Номенклатура_Папки_Дерево.Location = new System.Drawing.Point(0, 0);
            this.Номенклатура_Папки_Дерево.Name = "Номенклатура_Папки_Дерево";
            this.Номенклатура_Папки_Дерево.Size = new System.Drawing.Size(555, 738);
            this.Номенклатура_Папки_Дерево.TabIndex = 3;
            this.Номенклатура_Папки_Дерево.UidOpenFolder = null;
            // 
            // Form_НоменклатураПапкиВибір
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(555, 738);
            this.Controls.Add(this.panel2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form_НоменклатураПапкиВибір";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Номенклатура Папки";
            this.Load += new System.EventHandler(this.Form_НоменклатураПапкиВибір_Load);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel2;
        private Form_Номенклатура_Папки_Дерево Номенклатура_Папки_Дерево;
    }
}