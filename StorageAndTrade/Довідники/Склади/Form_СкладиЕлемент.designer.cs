
namespace StorageAndTrade
{
    partial class Form_СкладиЕлемент
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
            this.textBoxНазва = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBox_ТипСкладу = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.directoryControl_Відповідальний = new StorageAndTrade.DirectoryControl();
            this.label3 = new System.Windows.Forms.Label();
            this.directoryControl_ВидЦін = new StorageAndTrade.DirectoryControl();
            this.label4 = new System.Windows.Forms.Label();
            this.directoryControl_Підрозділ = new StorageAndTrade.DirectoryControl();
            this.SuspendLayout();
            // 
            // buttonClose
            // 
            this.buttonClose.Location = new System.Drawing.Point(384, 177);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(164, 23);
            this.buttonClose.TabIndex = 16;
            this.buttonClose.Text = "Закрити";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(110, 177);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(164, 23);
            this.buttonSave.TabIndex = 15;
            this.buttonSave.Text = "Зберегти";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // textBoxНазва
            // 
            this.textBoxНазва.AcceptsTab = true;
            this.textBoxНазва.Location = new System.Drawing.Point(110, 12);
            this.textBoxНазва.Name = "textBoxНазва";
            this.textBoxНазва.Size = new System.Drawing.Size(438, 20);
            this.textBoxНазва.TabIndex = 20;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(42, 13);
            this.label2.TabIndex = 19;
            this.label2.Text = "Назва:";
            // 
            // comboBox_ТипСкладу
            // 
            this.comboBox_ТипСкладу.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_ТипСкладу.FormattingEnabled = true;
            this.comboBox_ТипСкладу.Location = new System.Drawing.Point(110, 38);
            this.comboBox_ТипСкладу.Name = "comboBox_ТипСкладу";
            this.comboBox_ТипСкладу.Size = new System.Drawing.Size(257, 21);
            this.comboBox_ТипСкладу.TabIndex = 32;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 41);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 13);
            this.label5.TabIndex = 31;
            this.label5.Text = "Тип:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 75);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 13);
            this.label1.TabIndex = 34;
            this.label1.Text = "Відповідальний:";
            // 
            // directoryControl_Відповідальний
            // 
            this.directoryControl_Відповідальний.DirectoryPointerItem = null;
            this.directoryControl_Відповідальний.Location = new System.Drawing.Point(110, 69);
            this.directoryControl_Відповідальний.Name = "directoryControl_Відповідальний";
            this.directoryControl_Відповідальний.SelectForm = null;
            this.directoryControl_Відповідальний.Size = new System.Drawing.Size(438, 27);
            this.directoryControl_Відповідальний.TabIndex = 33;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 108);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 13);
            this.label3.TabIndex = 36;
            this.label3.Text = "Вид цін:";
            // 
            // directoryControl_ВидЦін
            // 
            this.directoryControl_ВидЦін.DirectoryPointerItem = null;
            this.directoryControl_ВидЦін.Location = new System.Drawing.Point(110, 102);
            this.directoryControl_ВидЦін.Name = "directoryControl_ВидЦін";
            this.directoryControl_ВидЦін.SelectForm = null;
            this.directoryControl_ВидЦін.Size = new System.Drawing.Size(438, 27);
            this.directoryControl_ВидЦін.TabIndex = 35;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 142);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(58, 13);
            this.label4.TabIndex = 38;
            this.label4.Text = "Підрозділ:";
            // 
            // directoryControl_Підрозділ
            // 
            this.directoryControl_Підрозділ.DirectoryPointerItem = null;
            this.directoryControl_Підрозділ.Location = new System.Drawing.Point(110, 135);
            this.directoryControl_Підрозділ.Name = "directoryControl_Підрозділ";
            this.directoryControl_Підрозділ.SelectForm = null;
            this.directoryControl_Підрозділ.Size = new System.Drawing.Size(438, 27);
            this.directoryControl_Підрозділ.TabIndex = 37;
            // 
            // Form_СкладиЕлемент
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(564, 221);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.directoryControl_Підрозділ);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.directoryControl_ВидЦін);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.directoryControl_Відповідальний);
            this.Controls.Add(this.comboBox_ТипСкладу);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textBoxНазва);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.buttonSave);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Form_СкладиЕлемент";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Склади";
            this.Load += new System.EventHandler(this.Form_СкладиЕлемент_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.TextBox textBoxНазва;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBox_ТипСкладу;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label1;
        private DirectoryControl directoryControl_Відповідальний;
        private System.Windows.Forms.Label label3;
        private DirectoryControl directoryControl_ВидЦін;
        private System.Windows.Forms.Label label4;
        private DirectoryControl directoryControl_Підрозділ;
    }
}