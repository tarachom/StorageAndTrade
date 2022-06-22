
namespace StorageAndTrade
{
    partial class Form_РеалізаціяТоварівТаПослугДокумент
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
            this.textBox_НомерДок = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.dateTimePicker_ДатаДок = new System.Windows.Forms.DateTimePicker();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label12 = new System.Windows.Forms.Label();
            this.directoryControl_Підрозділ = new StorageAndTrade.DirectoryControl();
            this.label11 = new System.Windows.Forms.Label();
            this.directoryControl_Договір = new StorageAndTrade.DirectoryControl();
            this.comboBox_ГосподарськаОперація = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.directoryControl_Каса = new StorageAndTrade.DirectoryControl();
            this.comboBox_ФормаОплати = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.comboBox_Статус = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.directoryControl_Склад = new StorageAndTrade.DirectoryControl();
            this.label4 = new System.Windows.Forms.Label();
            this.directoryControl_Валюта = new StorageAndTrade.DirectoryControl();
            this.label3 = new System.Windows.Forms.Label();
            this.directoryControl_Організація = new StorageAndTrade.DirectoryControl();
            this.label5 = new System.Windows.Forms.Label();
            this.directoryControl_Контрагент = new StorageAndTrade.DirectoryControl();
            this.panel2 = new System.Windows.Forms.Panel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.РеалізаціяТоварівТаПослуг_ТабличнаЧастина_Товари = new StorageAndTrade.Form_РеалізаціяТоварівТаПослуг_ТабличнаЧастина_Товари();
            this.panel3 = new System.Windows.Forms.Panel();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonClose
            // 
            this.buttonClose.Location = new System.Drawing.Point(173, 3);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(164, 29);
            this.buttonClose.TabIndex = 16;
            this.buttonClose.Text = "Закрити";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(3, 3);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(164, 29);
            this.buttonSave.TabIndex = 15;
            this.buttonSave.Text = "Зберегти";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // textBox_НомерДок
            // 
            this.textBox_НомерДок.Location = new System.Drawing.Point(106, 19);
            this.textBox_НомерДок.Name = "textBox_НомерДок";
            this.textBox_НомерДок.Size = new System.Drawing.Size(174, 20);
            this.textBox_НомерДок.TabIndex = 20;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(50, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 13);
            this.label2.TabIndex = 19;
            this.label2.Text = "Номер:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(286, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 13);
            this.label1.TabIndex = 21;
            this.label1.Text = "Дата:";
            // 
            // dateTimePicker_ДатаДок
            // 
            this.dateTimePicker_ДатаДок.Location = new System.Drawing.Point(328, 19);
            this.dateTimePicker_ДатаДок.Name = "dateTimePicker_ДатаДок";
            this.dateTimePicker_ДатаДок.Size = new System.Drawing.Size(154, 20);
            this.dateTimePicker_ДатаДок.TabIndex = 22;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.label11);
            this.panel1.Controls.Add(this.directoryControl_Договір);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.directoryControl_Каса);
            this.panel1.Controls.Add(this.comboBox_ФормаОплати);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.comboBox_Статус);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.directoryControl_Склад);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.directoryControl_Валюта);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.directoryControl_Організація);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.directoryControl_Контрагент);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.dateTimePicker_ДатаДок);
            this.panel1.Controls.Add(this.textBox_НомерДок);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(3, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1031, 185);
            this.panel1.TabIndex = 23;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(61, 55);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(58, 13);
            this.label12.TabIndex = 62;
            this.label12.Text = "Підрозділ:";
            // 
            // directoryControl_Підрозділ
            // 
            this.directoryControl_Підрозділ.DirectoryPointerItem = null;
            this.directoryControl_Підрозділ.Location = new System.Drawing.Point(154, 48);
            this.directoryControl_Підрозділ.Name = "directoryControl_Підрозділ";
            this.directoryControl_Підрозділ.SelectForm = null;
            this.directoryControl_Підрозділ.Size = new System.Drawing.Size(399, 27);
            this.directoryControl_Підрозділ.TabIndex = 61;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(494, 85);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(50, 13);
            this.label11.TabIndex = 60;
            this.label11.Text = "Договір:";
            // 
            // directoryControl_Договір
            // 
            this.directoryControl_Договір.DirectoryPointerItem = null;
            this.directoryControl_Договір.Location = new System.Drawing.Point(548, 78);
            this.directoryControl_Договір.Name = "directoryControl_Договір";
            this.directoryControl_Договір.SelectForm = null;
            this.directoryControl_Договір.Size = new System.Drawing.Size(376, 27);
            this.directoryControl_Договір.TabIndex = 59;
            // 
            // comboBox_ГосподарськаОперація
            // 
            this.comboBox_ГосподарськаОперація.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_ГосподарськаОперація.FormattingEnabled = true;
            this.comboBox_ГосподарськаОперація.Location = new System.Drawing.Point(154, 21);
            this.comboBox_ГосподарськаОперація.Name = "comboBox_ГосподарськаОперація";
            this.comboBox_ГосподарськаОперація.Size = new System.Drawing.Size(351, 21);
            this.comboBox_ГосподарськаОперація.TabIndex = 58;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(19, 24);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(129, 13);
            this.label10.TabIndex = 57;
            this.label10.Text = "Господарська операція:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(494, 52);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(35, 13);
            this.label9.TabIndex = 56;
            this.label9.Text = "Каса:";
            // 
            // directoryControl_Каса
            // 
            this.directoryControl_Каса.DirectoryPointerItem = null;
            this.directoryControl_Каса.Location = new System.Drawing.Point(548, 45);
            this.directoryControl_Каса.Name = "directoryControl_Каса";
            this.directoryControl_Каса.SelectForm = null;
            this.directoryControl_Каса.Size = new System.Drawing.Size(376, 27);
            this.directoryControl_Каса.TabIndex = 55;
            // 
            // comboBox_ФормаОплати
            // 
            this.comboBox_ФормаОплати.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_ФормаОплати.FormattingEnabled = true;
            this.comboBox_ФормаОплати.Location = new System.Drawing.Point(824, 19);
            this.comboBox_ФормаОплати.Name = "comboBox_ФормаОплати";
            this.comboBox_ФормаОплати.Size = new System.Drawing.Size(154, 21);
            this.comboBox_ФормаОплати.TabIndex = 54;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(733, 22);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(85, 13);
            this.label8.TabIndex = 53;
            this.label8.Text = "Форма оплати:";
            // 
            // comboBox_Статус
            // 
            this.comboBox_Статус.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_Статус.FormattingEnabled = true;
            this.comboBox_Статус.Location = new System.Drawing.Point(548, 19);
            this.comboBox_Статус.Name = "comboBox_Статус";
            this.comboBox_Статус.Size = new System.Drawing.Size(174, 21);
            this.comboBox_Статус.TabIndex = 52;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(498, 22);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(44, 13);
            this.label7.TabIndex = 51;
            this.label7.Text = "Статус:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(53, 151);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 13);
            this.label6.TabIndex = 50;
            this.label6.Text = "Склад:";
            // 
            // directoryControl_Склад
            // 
            this.directoryControl_Склад.DirectoryPointerItem = null;
            this.directoryControl_Склад.Location = new System.Drawing.Point(107, 144);
            this.directoryControl_Склад.Name = "directoryControl_Склад";
            this.directoryControl_Склад.SelectForm = null;
            this.directoryControl_Склад.Size = new System.Drawing.Size(376, 27);
            this.directoryControl_Склад.TabIndex = 49;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(52, 118);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(48, 13);
            this.label4.TabIndex = 48;
            this.label4.Text = "Валюта:";
            // 
            // directoryControl_Валюта
            // 
            this.directoryControl_Валюта.DirectoryPointerItem = null;
            this.directoryControl_Валюта.Location = new System.Drawing.Point(106, 111);
            this.directoryControl_Валюта.Name = "directoryControl_Валюта";
            this.directoryControl_Валюта.SelectForm = null;
            this.directoryControl_Валюта.Size = new System.Drawing.Size(376, 27);
            this.directoryControl_Валюта.TabIndex = 47;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(31, 85);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 13);
            this.label3.TabIndex = 46;
            this.label3.Text = "Організація:";
            // 
            // directoryControl_Організація
            // 
            this.directoryControl_Організація.DirectoryPointerItem = null;
            this.directoryControl_Організація.Location = new System.Drawing.Point(107, 78);
            this.directoryControl_Організація.Name = "directoryControl_Організація";
            this.directoryControl_Організація.SelectForm = null;
            this.directoryControl_Організація.Size = new System.Drawing.Size(375, 27);
            this.directoryControl_Організація.TabIndex = 45;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(29, 52);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(68, 13);
            this.label5.TabIndex = 44;
            this.label5.Text = "Контрагент:";
            // 
            // directoryControl_Контрагент
            // 
            this.directoryControl_Контрагент.DirectoryPointerItem = null;
            this.directoryControl_Контрагент.Location = new System.Drawing.Point(107, 45);
            this.directoryControl_Контрагент.Name = "directoryControl_Контрагент";
            this.directoryControl_Контрагент.SelectForm = null;
            this.directoryControl_Контрагент.Size = new System.Drawing.Size(375, 27);
            this.directoryControl_Контрагент.TabIndex = 43;
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.Controls.Add(this.tabControl1);
            this.panel2.Location = new System.Drawing.Point(3, 193);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1031, 385);
            this.panel2.TabIndex = 24;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1031, 385);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.РеалізаціяТоварівТаПослуг_ТабличнаЧастина_Товари);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1023, 359);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Товари";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // РеалізаціяТоварівТаПослуг_ТабличнаЧастина_Товари
            // 
            this.РеалізаціяТоварівТаПослуг_ТабличнаЧастина_Товари.Dock = System.Windows.Forms.DockStyle.Fill;
            this.РеалізаціяТоварівТаПослуг_ТабличнаЧастина_Товари.Location = new System.Drawing.Point(3, 3);
            this.РеалізаціяТоварівТаПослуг_ТабличнаЧастина_Товари.Name = "РеалізаціяТоварівТаПослуг_ТабличнаЧастина_Товари";
            this.РеалізаціяТоварівТаПослуг_ТабличнаЧастина_Товари.Size = new System.Drawing.Size(1017, 353);
            this.РеалізаціяТоварівТаПослуг_ТабличнаЧастина_Товари.TabIndex = 0;
            this.РеалізаціяТоварівТаПослуг_ТабличнаЧастина_Товари.ДокументОбєкт = null;
            // 
            // panel3
            // 
            this.panel3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel3.Controls.Add(this.buttonClose);
            this.panel3.Controls.Add(this.buttonSave);
            this.panel3.Location = new System.Drawing.Point(3, 584);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1031, 35);
            this.panel3.TabIndex = 25;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.label12);
            this.tabPage2.Controls.Add(this.directoryControl_Підрозділ);
            this.tabPage2.Controls.Add(this.label10);
            this.tabPage2.Controls.Add(this.comboBox_ГосподарськаОперація);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1023, 247);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Додаток";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // Form_РеалізаціяТоварівТаПослугДокумент
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1042, 621);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "Form_РеалізаціяТоварівТаПослугДокумент";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Реалізація товарів та послуг";
            this.Load += new System.EventHandler(this.FormAddCash_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.TextBox textBox_НомерДок;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dateTimePicker_ДатаДок;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Label label5;
        private DirectoryControl directoryControl_Контрагент;
        private System.Windows.Forms.Label label3;
        private DirectoryControl directoryControl_Організація;
        private System.Windows.Forms.Label label4;
        private DirectoryControl directoryControl_Валюта;
        private System.Windows.Forms.Label label6;
        private DirectoryControl directoryControl_Склад;
        private System.Windows.Forms.ComboBox comboBox_Статус;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox comboBox_ФормаОплати;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private DirectoryControl directoryControl_Каса;
        private System.Windows.Forms.ComboBox comboBox_ГосподарськаОперація;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private DirectoryControl directoryControl_Договір;
        private System.Windows.Forms.Label label12;
        private DirectoryControl directoryControl_Підрозділ;
        private StorageAndTrade.Form_РеалізаціяТоварівТаПослуг_ТабличнаЧастина_Товари РеалізаціяТоварівТаПослуг_ТабличнаЧастина_Товари;
        private System.Windows.Forms.TabPage tabPage2;
    }
}