
namespace StorageAndTrade
{
    partial class Form_КонтрагентиЕлемент
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_КонтрагентиЕлемент));
            this.buttonClose = new System.Windows.Forms.Button();
            this.buttonSave = new System.Windows.Forms.Button();
            this.textBoxName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.textBox_Код = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_ПовнаНазва = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox_Опис = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.Контрагенти_ТабличнаЧастина_Контакти = new StorageAndTrade.Form_Контрагенти_ТабличнаЧастина_Контакти();
            this.directoryControl_КонтрагентПапка = new StorageAndTrade.DirectoryControl();
            this.SuspendLayout();
            // 
            // buttonClose
            // 
            this.buttonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonClose.Location = new System.Drawing.Point(553, 469);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(164, 27);
            this.buttonClose.TabIndex = 16;
            this.buttonClose.Text = "Закрити";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // buttonSave
            // 
            this.buttonSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonSave.Location = new System.Drawing.Point(11, 469);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(164, 27);
            this.buttonSave.TabIndex = 15;
            this.buttonSave.Text = "Зберегти";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // textBoxName
            // 
            this.textBoxName.Location = new System.Drawing.Point(60, 12);
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.Size = new System.Drawing.Size(493, 20);
            this.textBoxName.TabIndex = 20;
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
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(12, 44);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(40, 13);
            this.label9.TabIndex = 56;
            this.label9.Text = "Родич:";
            // 
            // textBox_Код
            // 
            this.textBox_Код.Location = new System.Drawing.Point(593, 12);
            this.textBox_Код.Name = "textBox_Код";
            this.textBox_Код.Size = new System.Drawing.Size(126, 20);
            this.textBox_Код.TabIndex = 58;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(558, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 13);
            this.label1.TabIndex = 57;
            this.label1.Text = "Код:";
            // 
            // textBox_ПовнаНазва
            // 
            this.textBox_ПовнаНазва.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_ПовнаНазва.Location = new System.Drawing.Point(60, 71);
            this.textBox_ПовнаНазва.Multiline = true;
            this.textBox_ПовнаНазва.Name = "textBox_ПовнаНазва";
            this.textBox_ПовнаНазва.Size = new System.Drawing.Size(657, 34);
            this.textBox_ПовнаНазва.TabIndex = 61;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 74);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(42, 13);
            this.label3.TabIndex = 60;
            this.label3.Text = "Повна:";
            // 
            // textBox_Опис
            // 
            this.textBox_Опис.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_Опис.Location = new System.Drawing.Point(60, 113);
            this.textBox_Опис.Multiline = true;
            this.textBox_Опис.Name = "textBox_Опис";
            this.textBox_Опис.Size = new System.Drawing.Size(657, 56);
            this.textBox_Опис.TabIndex = 63;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 116);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(36, 13);
            this.label4.TabIndex = 62;
            this.label4.Text = "Опис:";
            // 
            // Контрагенти_ТабличнаЧастина_Контакти
            // 
            this.Контрагенти_ТабличнаЧастина_Контакти.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Контрагенти_ТабличнаЧастина_Контакти.Location = new System.Drawing.Point(11, 187);
            this.Контрагенти_ТабличнаЧастина_Контакти.Name = "Контрагенти_ТабличнаЧастина_Контакти";
            this.Контрагенти_ТабличнаЧастина_Контакти.Size = new System.Drawing.Size(707, 266);
            this.Контрагенти_ТабличнаЧастина_Контакти.TabIndex = 59;
            this.Контрагенти_ТабличнаЧастина_Контакти.ДовідникОбєкт = null;
            // 
            // directoryControl_КонтрагентПапка
            // 
            this.directoryControl_КонтрагентПапка.AfterSelectFunc = null;
            this.directoryControl_КонтрагентПапка.BeforeClickOpenFunc = null;
            this.directoryControl_КонтрагентПапка.Bind = null;
            this.directoryControl_КонтрагентПапка.DirectoryPointerItem = null;
            this.directoryControl_КонтрагентПапка.Location = new System.Drawing.Point(60, 38);
            this.directoryControl_КонтрагентПапка.Name = "directoryControl_КонтрагентПапка";
            this.directoryControl_КонтрагентПапка.QueryFind = null;
            this.directoryControl_КонтрагентПапка.SelectForm = null;
            this.directoryControl_КонтрагентПапка.Size = new System.Drawing.Size(493, 27);
            this.directoryControl_КонтрагентПапка.TabIndex = 55;
            // 
            // Form_КонтрагентиЕлемент
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(730, 507);
            this.Controls.Add(this.textBox_Опис);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBox_ПовнаНазва);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.Контрагенти_ТабличнаЧастина_Контакти);
            this.Controls.Add(this.textBox_Код);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.directoryControl_КонтрагентПапка);
            this.Controls.Add(this.textBoxName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.buttonSave);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form_КонтрагентиЕлемент";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Контрагенти";
            this.Load += new System.EventHandler(this.Form_КонтрагентиЕлемент_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.TextBox textBoxName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label9;
        private DirectoryControl directoryControl_КонтрагентПапка;
        private System.Windows.Forms.TextBox textBox_Код;
        private System.Windows.Forms.Label label1;
        private Form_Контрагенти_ТабличнаЧастина_Контакти Контрагенти_ТабличнаЧастина_Контакти;
        private System.Windows.Forms.TextBox textBox_ПовнаНазва;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox_Опис;
        private System.Windows.Forms.Label label4;
    }
}