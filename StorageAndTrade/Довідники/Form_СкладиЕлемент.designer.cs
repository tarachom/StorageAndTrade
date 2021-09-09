
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
            this.SuspendLayout();
            // 
            // buttonClose
            // 
            this.buttonClose.Location = new System.Drawing.Point(385, 129);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(164, 23);
            this.buttonClose.TabIndex = 16;
            this.buttonClose.Text = "Закрити";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(88, 129);
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
            this.textBoxНазва.Location = new System.Drawing.Point(88, 12);
            this.textBoxНазва.Name = "textBoxНазва";
            this.textBoxНазва.Size = new System.Drawing.Size(461, 20);
            this.textBoxНазва.TabIndex = 20;
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
            // comboBox_ТипСкладу
            // 
            this.comboBox_ТипСкладу.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_ТипСкладу.FormattingEnabled = true;
            this.comboBox_ТипСкладу.Location = new System.Drawing.Point(88, 38);
            this.comboBox_ТипСкладу.Name = "comboBox_ТипСкладу";
            this.comboBox_ТипСкладу.Size = new System.Drawing.Size(257, 21);
            this.comboBox_ТипСкладу.TabIndex = 32;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(40, 41);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 13);
            this.label5.TabIndex = 31;
            this.label5.Text = "Тип:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 76);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 13);
            this.label1.TabIndex = 34;
            this.label1.Text = "Відповідальний:";
            // 
            // directoryControl_Відповідальний
            // 
            this.directoryControl_Відповідальний.CallBack = null;
            this.directoryControl_Відповідальний.DirectoryPointerItem = null;
            this.directoryControl_Відповідальний.Location = new System.Drawing.Point(110, 69);
            this.directoryControl_Відповідальний.Name = "directoryControl_Відповідальний";
            this.directoryControl_Відповідальний.Size = new System.Drawing.Size(438, 27);
            this.directoryControl_Відповідальний.TabIndex = 33;
            // 
            // Form_СкладиЕлемент
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(565, 164);
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
            this.Load += new System.EventHandler(this.FormAddCash_Load);
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
    }
}