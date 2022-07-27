
namespace StorageAndTrade
{
    partial class FormConstants
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.buttonSave = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.directoryControl_Організація = new StorageAndTrade.DirectoryControl();
            this.label1 = new System.Windows.Forms.Label();
            this.directoryControl_Склад = new StorageAndTrade.DirectoryControl();
            this.label2 = new System.Windows.Forms.Label();
            this.directoryControl_Валюта = new StorageAndTrade.DirectoryControl();
            this.label3 = new System.Windows.Forms.Label();
            this.directoryControl_Постачальник = new StorageAndTrade.DirectoryControl();
            this.label5 = new System.Windows.Forms.Label();
            this.directoryControl_Покупець = new StorageAndTrade.DirectoryControl();
            this.label6 = new System.Windows.Forms.Label();
            this.directoryControl_Каса = new StorageAndTrade.DirectoryControl();
            this.label7 = new System.Windows.Forms.Label();
            this.directoryControl_ОдиницяПакування = new StorageAndTrade.DirectoryControl();
            this.buttonClose = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.directoryControl_ОдиницяПакування);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.directoryControl_Каса);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.directoryControl_Покупець);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.directoryControl_Постачальник);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.directoryControl_Валюта);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.directoryControl_Склад);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.directoryControl_Організація);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(814, 260);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Значення за замовчуванням";
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(836, 691);
            this.panel1.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.Controls.Add(this.buttonClose);
            this.panel2.Controls.Add(this.buttonSave);
            this.panel2.Location = new System.Drawing.Point(0, 697);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(836, 40);
            this.panel2.TabIndex = 2;
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(3, 3);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(190, 34);
            this.buttonSave.TabIndex = 0;
            this.buttonSave.Text = "Записати";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 26);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(69, 13);
            this.label4.TabIndex = 75;
            this.label4.Text = "Організація:";
            // 
            // directoryControl_Організація
            // 
            this.directoryControl_Організація.DirectoryPointerItem = null;
            this.directoryControl_Організація.Location = new System.Drawing.Point(158, 19);
            this.directoryControl_Організація.Name = "directoryControl_Організація";
            this.directoryControl_Організація.SelectForm = null;
            this.directoryControl_Організація.Size = new System.Drawing.Size(402, 27);
            this.directoryControl_Організація.TabIndex = 74;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 59);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 77;
            this.label1.Text = "Склад:";
            // 
            // directoryControl_Склад
            // 
            this.directoryControl_Склад.DirectoryPointerItem = null;
            this.directoryControl_Склад.Location = new System.Drawing.Point(158, 52);
            this.directoryControl_Склад.Name = "directoryControl_Склад";
            this.directoryControl_Склад.SelectForm = null;
            this.directoryControl_Склад.Size = new System.Drawing.Size(402, 27);
            this.directoryControl_Склад.TabIndex = 76;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 92);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 13);
            this.label2.TabIndex = 79;
            this.label2.Text = "Валюта:";
            // 
            // directoryControl_Валюта
            // 
            this.directoryControl_Валюта.DirectoryPointerItem = null;
            this.directoryControl_Валюта.Location = new System.Drawing.Point(158, 85);
            this.directoryControl_Валюта.Name = "directoryControl_Валюта";
            this.directoryControl_Валюта.SelectForm = null;
            this.directoryControl_Валюта.Size = new System.Drawing.Size(402, 27);
            this.directoryControl_Валюта.TabIndex = 78;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 125);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 13);
            this.label3.TabIndex = 81;
            this.label3.Text = "Постачальник:";
            // 
            // directoryControl_Постачальник
            // 
            this.directoryControl_Постачальник.DirectoryPointerItem = null;
            this.directoryControl_Постачальник.Location = new System.Drawing.Point(158, 118);
            this.directoryControl_Постачальник.Name = "directoryControl_Постачальник";
            this.directoryControl_Постачальник.SelectForm = null;
            this.directoryControl_Постачальник.Size = new System.Drawing.Size(402, 27);
            this.directoryControl_Постачальник.TabIndex = 80;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(16, 158);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(59, 13);
            this.label5.TabIndex = 83;
            this.label5.Text = "Покупець:";
            // 
            // directoryControl_Покупець
            // 
            this.directoryControl_Покупець.DirectoryPointerItem = null;
            this.directoryControl_Покупець.Location = new System.Drawing.Point(158, 151);
            this.directoryControl_Покупець.Name = "directoryControl_Покупець";
            this.directoryControl_Покупець.SelectForm = null;
            this.directoryControl_Покупець.Size = new System.Drawing.Size(402, 27);
            this.directoryControl_Покупець.TabIndex = 82;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(16, 191);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(35, 13);
            this.label6.TabIndex = 85;
            this.label6.Text = "Каса:";
            // 
            // directoryControl_Каса
            // 
            this.directoryControl_Каса.DirectoryPointerItem = null;
            this.directoryControl_Каса.Location = new System.Drawing.Point(158, 184);
            this.directoryControl_Каса.Name = "directoryControl_Каса";
            this.directoryControl_Каса.SelectForm = null;
            this.directoryControl_Каса.Size = new System.Drawing.Size(402, 27);
            this.directoryControl_Каса.TabIndex = 84;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(16, 224);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(110, 13);
            this.label7.TabIndex = 87;
            this.label7.Text = "Одиниця пакування:";
            // 
            // directoryControl_ОдиницяПакування
            // 
            this.directoryControl_ОдиницяПакування.DirectoryPointerItem = null;
            this.directoryControl_ОдиницяПакування.Location = new System.Drawing.Point(158, 217);
            this.directoryControl_ОдиницяПакування.Name = "directoryControl_ОдиницяПакування";
            this.directoryControl_ОдиницяПакування.SelectForm = null;
            this.directoryControl_ОдиницяПакування.Size = new System.Drawing.Size(402, 27);
            this.directoryControl_ОдиницяПакування.TabIndex = 86;
            // 
            // buttonClose
            // 
            this.buttonClose.Location = new System.Drawing.Point(199, 3);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(96, 34);
            this.buttonClose.TabIndex = 1;
            this.buttonClose.Text = "Закрити";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // FormConstants
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(838, 741);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FormConstants";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Константи";
            this.Load += new System.EventHandler(this.FormConstants_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Label label4;
        private DirectoryControl directoryControl_Організація;
        private System.Windows.Forms.Label label2;
        private DirectoryControl directoryControl_Валюта;
        private System.Windows.Forms.Label label1;
        private DirectoryControl directoryControl_Склад;
        private System.Windows.Forms.Label label5;
        private DirectoryControl directoryControl_Покупець;
        private System.Windows.Forms.Label label3;
        private DirectoryControl directoryControl_Постачальник;
        private System.Windows.Forms.Label label7;
        private DirectoryControl directoryControl_ОдиницяПакування;
        private System.Windows.Forms.Label label6;
        private DirectoryControl directoryControl_Каса;
        private System.Windows.Forms.Button buttonClose;
    }
}