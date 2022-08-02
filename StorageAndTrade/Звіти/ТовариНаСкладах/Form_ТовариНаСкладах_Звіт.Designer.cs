
namespace StorageAndTrade
{
    partial class Form_ТовариНаСкладах_Звіт
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_ТовариНаСкладах_Звіт));
            this.dateTimeStart = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.dateTimeStop = new System.Windows.Forms.DateTimePicker();
            this.label9 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.buttonClose = new System.Windows.Forms.Button();
            this.buttonOstatok = new System.Windows.Forms.Button();
            this.geckoWebBrowser = new Gecko.GeckoWebBrowser();
            this.directoryControl_ХарактеристикаНоменклатури = new StorageAndTrade.DirectoryControl();
            this.directoryControl_Склади = new StorageAndTrade.DirectoryControl();
            this.directoryControl_СкладиПапки = new StorageAndTrade.DirectoryControl();
            this.directoryControl_Номенклатура = new StorageAndTrade.DirectoryControl();
            this.directoryControl_НоменклатураПапка = new StorageAndTrade.DirectoryControl();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.buttonOstatokAndOborot = new System.Windows.Forms.Button();
            this.button_Documents = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // dateTimeStart
            // 
            this.dateTimeStart.Location = new System.Drawing.Point(125, 23);
            this.dateTimeStart.Name = "dateTimeStart";
            this.dateTimeStart.Size = new System.Drawing.Size(195, 20);
            this.dateTimeStart.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(69, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Період з";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(326, 30);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(19, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "по";
            // 
            // dateTimeStop
            // 
            this.dateTimeStop.Location = new System.Drawing.Point(351, 24);
            this.dateTimeStop.Name = "dateTimeStop";
            this.dateTimeStop.Size = new System.Drawing.Size(195, 20);
            this.dateTimeStop.TabIndex = 3;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(14, 26);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(42, 13);
            this.label9.TabIndex = 56;
            this.label9.Text = "Папка:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 59);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(84, 13);
            this.label3.TabIndex = 58;
            this.label3.Text = "Номенклатура:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(20, 27);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(42, 13);
            this.label4.TabIndex = 60;
            this.label4.Text = "Папка:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(20, 60);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 13);
            this.label5.TabIndex = 62;
            this.label5.Text = "Склад:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(14, 92);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(93, 13);
            this.label6.TabIndex = 64;
            this.label6.Text = "Характеристика:";
            // 
            // buttonClose
            // 
            this.buttonClose.Location = new System.Drawing.Point(893, 194);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(90, 27);
            this.buttonClose.TabIndex = 72;
            this.buttonClose.Text = "Закрити";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // buttonOstatok
            // 
            this.buttonOstatok.Location = new System.Drawing.Point(12, 194);
            this.buttonOstatok.Name = "buttonOstatok";
            this.buttonOstatok.Size = new System.Drawing.Size(90, 27);
            this.buttonOstatok.TabIndex = 73;
            this.buttonOstatok.Text = "Залишки";
            this.buttonOstatok.UseVisualStyleBackColor = true;
            this.buttonOstatok.Click += new System.EventHandler(this.buttonbuttonOstatok_Click);
            // 
            // geckoWebBrowser
            // 
            this.geckoWebBrowser.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.geckoWebBrowser.Location = new System.Drawing.Point(1, 244);
            this.geckoWebBrowser.Name = "geckoWebBrowser";
            this.geckoWebBrowser.Size = new System.Drawing.Size(995, 411);
            this.geckoWebBrowser.TabIndex = 74;
            this.geckoWebBrowser.UseHttpActivityObserver = false;
            // 
            // directoryControl_ХарактеристикаНоменклатури
            // 
            this.directoryControl_ХарактеристикаНоменклатури.AfterSelectFunc = null;
            this.directoryControl_ХарактеристикаНоменклатури.BeforeClickOpenFunc = null;
            this.directoryControl_ХарактеристикаНоменклатури.DirectoryPointerItem = null;
            this.directoryControl_ХарактеристикаНоменклатури.Location = new System.Drawing.Point(113, 85);
            this.directoryControl_ХарактеристикаНоменклатури.Name = "directoryControl_ХарактеристикаНоменклатури";
            this.directoryControl_ХарактеристикаНоменклатури.SelectForm = null;
            this.directoryControl_ХарактеристикаНоменклатури.Size = new System.Drawing.Size(402, 27);
            this.directoryControl_ХарактеристикаНоменклатури.TabIndex = 63;
            // 
            // directoryControl_Склади
            // 
            this.directoryControl_Склади.AfterSelectFunc = null;
            this.directoryControl_Склади.BeforeClickOpenFunc = null;
            this.directoryControl_Склади.DirectoryPointerItem = null;
            this.directoryControl_Склади.Location = new System.Drawing.Point(68, 53);
            this.directoryControl_Склади.Name = "directoryControl_Склади";
            this.directoryControl_Склади.SelectForm = null;
            this.directoryControl_Склади.Size = new System.Drawing.Size(330, 27);
            this.directoryControl_Склади.TabIndex = 61;
            // 
            // directoryControl_СкладиПапки
            // 
            this.directoryControl_СкладиПапки.AfterSelectFunc = null;
            this.directoryControl_СкладиПапки.BeforeClickOpenFunc = null;
            this.directoryControl_СкладиПапки.DirectoryPointerItem = null;
            this.directoryControl_СкладиПапки.Location = new System.Drawing.Point(68, 20);
            this.directoryControl_СкладиПапки.Name = "directoryControl_СкладиПапки";
            this.directoryControl_СкладиПапки.SelectForm = null;
            this.directoryControl_СкладиПапки.Size = new System.Drawing.Size(330, 27);
            this.directoryControl_СкладиПапки.TabIndex = 59;
            // 
            // directoryControl_Номенклатура
            // 
            this.directoryControl_Номенклатура.AfterSelectFunc = null;
            this.directoryControl_Номенклатура.BeforeClickOpenFunc = null;
            this.directoryControl_Номенклатура.DirectoryPointerItem = null;
            this.directoryControl_Номенклатура.Location = new System.Drawing.Point(113, 52);
            this.directoryControl_Номенклатура.Name = "directoryControl_Номенклатура";
            this.directoryControl_Номенклатура.SelectForm = null;
            this.directoryControl_Номенклатура.Size = new System.Drawing.Size(402, 27);
            this.directoryControl_Номенклатура.TabIndex = 57;
            // 
            // directoryControl_НоменклатураПапка
            // 
            this.directoryControl_НоменклатураПапка.AfterSelectFunc = null;
            this.directoryControl_НоменклатураПапка.BeforeClickOpenFunc = null;
            this.directoryControl_НоменклатураПапка.DirectoryPointerItem = null;
            this.directoryControl_НоменклатураПапка.Location = new System.Drawing.Point(113, 19);
            this.directoryControl_НоменклатураПапка.Name = "directoryControl_НоменклатураПапка";
            this.directoryControl_НоменклатураПапка.SelectForm = null;
            this.directoryControl_НоменклатураПапка.Size = new System.Drawing.Size(402, 27);
            this.directoryControl_НоменклатураПапка.TabIndex = 55;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.directoryControl_НоменклатураПапка);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.directoryControl_Номенклатура);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.directoryControl_ХарактеристикаНоменклатури);
            this.groupBox1.Location = new System.Drawing.Point(12, 61);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(541, 127);
            this.groupBox1.TabIndex = 75;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Номенклатура";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.directoryControl_Склади);
            this.groupBox2.Controls.Add(this.directoryControl_СкладиПапки);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Location = new System.Drawing.Point(568, 61);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(415, 100);
            this.groupBox2.TabIndex = 65;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Склад";
            // 
            // buttonOstatokAndOborot
            // 
            this.buttonOstatokAndOborot.Location = new System.Drawing.Point(108, 194);
            this.buttonOstatokAndOborot.Name = "buttonOstatokAndOborot";
            this.buttonOstatokAndOborot.Size = new System.Drawing.Size(161, 27);
            this.buttonOstatokAndOborot.TabIndex = 76;
            this.buttonOstatokAndOborot.Text = "Залишки та обороти";
            this.buttonOstatokAndOborot.UseVisualStyleBackColor = true;
            this.buttonOstatokAndOborot.Click += new System.EventHandler(this.buttonOstatokAndOborot_Click);
            // 
            // button_Documents
            // 
            this.button_Documents.Location = new System.Drawing.Point(275, 194);
            this.button_Documents.Name = "button_Documents";
            this.button_Documents.Size = new System.Drawing.Size(96, 27);
            this.button_Documents.TabIndex = 78;
            this.button_Documents.Text = "По документах";
            this.button_Documents.UseVisualStyleBackColor = true;
            this.button_Documents.Click += new System.EventHandler(this.button_Documents_Click);
            // 
            // Form_ТовариНаСкладах_Звіт
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(997, 657);
            this.Controls.Add(this.button_Documents);
            this.Controls.Add(this.buttonOstatokAndOborot);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.geckoWebBrowser);
            this.Controls.Add(this.buttonOstatok);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.dateTimeStop);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dateTimeStart);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form_ТовариНаСкладах_Звіт";
            this.Text = "Звіт \"Товари на складах\"";
            this.Load += new System.EventHandler(this.Form_ЗамовленняКлієнтів_Звіт_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DateTimePicker dateTimeStart;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dateTimeStop;
        private System.Windows.Forms.Label label9;
        private DirectoryControl directoryControl_НоменклатураПапка;
        private System.Windows.Forms.Label label3;
        private DirectoryControl directoryControl_Номенклатура;
        private System.Windows.Forms.Label label4;
        private DirectoryControl directoryControl_СкладиПапки;
        private System.Windows.Forms.Label label5;
        private DirectoryControl directoryControl_Склади;
        private System.Windows.Forms.Label label6;
        private DirectoryControl directoryControl_ХарактеристикаНоменклатури;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Button buttonOstatok;
        private Gecko.GeckoWebBrowser geckoWebBrowser;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button buttonOstatokAndOborot;
        private System.Windows.Forms.Button button_Documents;
    }
}