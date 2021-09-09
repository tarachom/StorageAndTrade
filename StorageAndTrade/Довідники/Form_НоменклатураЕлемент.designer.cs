
namespace StorageAndTrade
{
    partial class Form_НоменклатураЕлемент
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
            this.buttonClose = new System.Windows.Forms.Button();
            this.buttonSave = new System.Windows.Forms.Button();
            this.textBoxName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.directoryControl_Виробник = new StorageAndTrade.DirectoryControl();
            this.label3 = new System.Windows.Forms.Label();
            this.directoryControl_ВидНоменклатури = new StorageAndTrade.DirectoryControl();
            this.SuspendLayout();
            // 
            // buttonClose
            // 
            this.buttonClose.Location = new System.Drawing.Point(364, 300);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(164, 23);
            this.buttonClose.TabIndex = 16;
            this.buttonClose.Text = "Закрити";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(67, 300);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(164, 23);
            this.buttonSave.TabIndex = 15;
            this.buttonSave.Text = "Зберегти";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // textBoxName
            // 
            this.textBoxName.Location = new System.Drawing.Point(88, 12);
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.Size = new System.Drawing.Size(461, 20);
            this.textBoxName.TabIndex = 20;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(40, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(42, 13);
            this.label2.TabIndex = 19;
            this.label2.Text = "Назва:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 13);
            this.label1.TabIndex = 24;
            this.label1.Text = "Виробник:";
            // 
            // directoryControl_Виробник
            // 
            this.directoryControl_Виробник.CallBack = null;
            this.directoryControl_Виробник.DirectoryPointerItem = null;
            this.directoryControl_Виробник.Location = new System.Drawing.Point(84, 38);
            this.directoryControl_Виробник.Name = "directoryControl_Виробник";
            this.directoryControl_Виробник.Size = new System.Drawing.Size(465, 27);
            this.directoryControl_Виробник.TabIndex = 23;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(17, 76);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(104, 13);
            this.label3.TabIndex = 26;
            this.label3.Text = "Вид номенклатури:";
            // 
            // directoryControl_ВидНоменклатури
            // 
            this.directoryControl_ВидНоменклатури.CallBack = null;
            this.directoryControl_ВидНоменклатури.DirectoryPointerItem = null;
            this.directoryControl_ВидНоменклатури.Location = new System.Drawing.Point(126, 69);
            this.directoryControl_ВидНоменклатури.Name = "directoryControl_ВидНоменклатури";
            this.directoryControl_ВидНоменклатури.Size = new System.Drawing.Size(422, 27);
            this.directoryControl_ВидНоменклатури.TabIndex = 25;
            // 
            // Form_НоменклатураЕлемент
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(565, 346);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.directoryControl_ВидНоменклатури);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.directoryControl_Виробник);
            this.Controls.Add(this.textBoxName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.buttonSave);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Form_НоменклатураЕлемент";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Номенклатура";
            this.Load += new System.EventHandler(this.FormAddCash_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.TextBox textBoxName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private DirectoryControl directoryControl_Виробник;
        private System.Windows.Forms.Label label3;
        private DirectoryControl directoryControl_ВидНоменклатури;
    }
}