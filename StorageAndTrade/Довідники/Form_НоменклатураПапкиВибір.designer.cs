
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
            this.panel2 = new System.Windows.Forms.Panel();
            this.form_Номенклатура_Папки_Дерево1 = new StorageAndTrade.Form_Номенклатура_Папки_Дерево();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.form_Номенклатура_Папки_Дерево1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(555, 738);
            this.panel2.TabIndex = 2;
            // 
            // form_Номенклатура_Папки_Дерево1
            // 
            this.form_Номенклатура_Папки_Дерево1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.form_Номенклатура_Папки_Дерево1.Location = new System.Drawing.Point(0, 0);
            this.form_Номенклатура_Папки_Дерево1.Name = "form_Номенклатура_Папки_Дерево1";
            this.form_Номенклатура_Папки_Дерево1.OwnerForm = null;
            this.form_Номенклатура_Папки_Дерево1.Size = new System.Drawing.Size(555, 738);
            this.form_Номенклатура_Папки_Дерево1.TabIndex = 3;
            // 
            // Form_НоменклатураПапки
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(555, 738);
            this.Controls.Add(this.panel2);
            this.Name = "Form_НоменклатураПапки";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Номенклатура Папки";
            this.Load += new System.EventHandler(this.FormCash_Load);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel2;
        private Form_Номенклатура_Папки_Дерево form_Номенклатура_Папки_Дерево1;
    }
}