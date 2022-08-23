
namespace StorageAndTrade
{
    partial class Form_ВнутрішнєСпоживанняТоварівДокумент
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_ВнутрішнєСпоживанняТоварівДокумент));
            this.buttonClose = new System.Windows.Forms.Button();
            this.textBox_НомерДок = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.dateTimePicker_ДатаДок = new System.Windows.Forms.DateTimePicker();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.dateTimePicker_ЧасДок = new System.Windows.Forms.DateTimePicker();
            this.label13 = new System.Windows.Forms.Label();
            this.textBox_Коментар = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.comboBox_ГосподарськаОперація = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.directoryControl_Склад = new StorageAndTrade.DirectoryControl();
            this.label4 = new System.Windows.Forms.Label();
            this.directoryControl_Валюта = new StorageAndTrade.DirectoryControl();
            this.label3 = new System.Windows.Forms.Label();
            this.directoryControl_Організація = new StorageAndTrade.DirectoryControl();
            this.label12 = new System.Windows.Forms.Label();
            this.directoryControl_Підрозділ = new StorageAndTrade.DirectoryControl();
            this.panel2 = new System.Windows.Forms.Panel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.ВнутрішнєСпоживанняТоварів_ТабличнаЧастина_Товари = new StorageAndTrade.Form_ВнутрішнєСпоживанняТоварів_ТабличнаЧастина_Товари();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.panel3 = new System.Windows.Forms.Panel();
            this.buttonSaveAndSpend = new System.Windows.Forms.Button();
            this.buttonSpend = new System.Windows.Forms.Button();
            this.buttonSave = new System.Windows.Forms.Button();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton_FindToJournal = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonДрукПроводок = new System.Windows.Forms.ToolStripButton();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonClose
            // 
            this.buttonClose.Location = new System.Drawing.Point(497, 4);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(164, 27);
            this.buttonClose.TabIndex = 16;
            this.buttonClose.Text = "Закрити";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // textBox_НомерДок
            // 
            this.textBox_НомерДок.BackColor = System.Drawing.Color.White;
            this.textBox_НомерДок.Location = new System.Drawing.Point(377, 13);
            this.textBox_НомерДок.Name = "textBox_НомерДок";
            this.textBox_НомерДок.ReadOnly = true;
            this.textBox_НомерДок.Size = new System.Drawing.Size(132, 20);
            this.textBox_НомерДок.TabIndex = 20;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(326, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 13);
            this.label2.TabIndex = 19;
            this.label2.Text = "Номер:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(515, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 13);
            this.label1.TabIndex = 21;
            this.label1.Text = "Дата:";
            // 
            // dateTimePicker_ДатаДок
            // 
            this.dateTimePicker_ДатаДок.Location = new System.Drawing.Point(557, 13);
            this.dateTimePicker_ДатаДок.Name = "dateTimePicker_ДатаДок";
            this.dateTimePicker_ДатаДок.Size = new System.Drawing.Size(196, 20);
            this.dateTimePicker_ДатаДок.TabIndex = 22;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.dateTimePicker_ЧасДок);
            this.panel1.Controls.Add(this.label13);
            this.panel1.Controls.Add(this.textBox_Коментар);
            this.panel1.Controls.Add(this.label10);
            this.panel1.Controls.Add(this.comboBox_ГосподарськаОперація);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.directoryControl_Склад);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.directoryControl_Валюта);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.directoryControl_Організація);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.dateTimePicker_ДатаДок);
            this.panel1.Controls.Add(this.textBox_НомерДок);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(3, 28);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(853, 176);
            this.panel1.TabIndex = 23;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label5.Location = new System.Drawing.Point(2, 7);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(321, 25);
            this.label5.TabIndex = 71;
            this.label5.Text = "Внутрішнє споживання товарів";
            // 
            // dateTimePicker_ЧасДок
            // 
            this.dateTimePicker_ЧасДок.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dateTimePicker_ЧасДок.Location = new System.Drawing.Point(759, 13);
            this.dateTimePicker_ЧасДок.Name = "dateTimePicker_ЧасДок";
            this.dateTimePicker_ЧасДок.ShowUpDown = true;
            this.dateTimePicker_ЧасДок.Size = new System.Drawing.Size(76, 20);
            this.dateTimePicker_ЧасДок.TabIndex = 68;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(18, 142);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(60, 13);
            this.label13.TabIndex = 63;
            this.label13.Text = "Коментар:";
            // 
            // textBox_Коментар
            // 
            this.textBox_Коментар.Location = new System.Drawing.Point(85, 139);
            this.textBox_Коментар.Name = "textBox_Коментар";
            this.textBox_Коментар.Size = new System.Drawing.Size(750, 20);
            this.textBox_Коментар.TabIndex = 1;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(472, 47);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(56, 13);
            this.label10.TabIndex = 57;
            this.label10.Text = "Операція:";
            // 
            // comboBox_ГосподарськаОперація
            // 
            this.comboBox_ГосподарськаОперація.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_ГосподарськаОперація.FormattingEnabled = true;
            this.comboBox_ГосподарськаОперація.Location = new System.Drawing.Point(534, 44);
            this.comboBox_ГосподарськаОперація.Name = "comboBox_ГосподарськаОперація";
            this.comboBox_ГосподарськаОперація.Size = new System.Drawing.Size(301, 21);
            this.comboBox_ГосподарськаОперація.TabIndex = 58;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(37, 113);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 13);
            this.label6.TabIndex = 50;
            this.label6.Text = "Склад:";
            // 
            // directoryControl_Склад
            // 
            this.directoryControl_Склад.AfterSelectFunc = null;
            this.directoryControl_Склад.BeforeClickOpenFunc = null;
            this.directoryControl_Склад.BeforeFindFunc = null;
            this.directoryControl_Склад.Bind = null;
            this.directoryControl_Склад.DirectoryPointerItem = null;
            this.directoryControl_Склад.Location = new System.Drawing.Point(85, 106);
            this.directoryControl_Склад.Name = "directoryControl_Склад";
            this.directoryControl_Склад.QueryFind = null;
            this.directoryControl_Склад.SelectForm = null;
            this.directoryControl_Склад.Size = new System.Drawing.Size(376, 27);
            this.directoryControl_Склад.TabIndex = 49;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(30, 80);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(48, 13);
            this.label4.TabIndex = 48;
            this.label4.Text = "Валюта:";
            // 
            // directoryControl_Валюта
            // 
            this.directoryControl_Валюта.AfterSelectFunc = null;
            this.directoryControl_Валюта.BeforeClickOpenFunc = null;
            this.directoryControl_Валюта.BeforeFindFunc = null;
            this.directoryControl_Валюта.Bind = null;
            this.directoryControl_Валюта.DirectoryPointerItem = null;
            this.directoryControl_Валюта.Location = new System.Drawing.Point(85, 73);
            this.directoryControl_Валюта.Name = "directoryControl_Валюта";
            this.directoryControl_Валюта.QueryFind = null;
            this.directoryControl_Валюта.SelectForm = null;
            this.directoryControl_Валюта.Size = new System.Drawing.Size(376, 27);
            this.directoryControl_Валюта.TabIndex = 47;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 47);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 13);
            this.label3.TabIndex = 46;
            this.label3.Text = "Організація:";
            // 
            // directoryControl_Організація
            // 
            this.directoryControl_Організація.AfterSelectFunc = null;
            this.directoryControl_Організація.BeforeClickOpenFunc = null;
            this.directoryControl_Організація.BeforeFindFunc = null;
            this.directoryControl_Організація.Bind = null;
            this.directoryControl_Організація.DirectoryPointerItem = null;
            this.directoryControl_Організація.Location = new System.Drawing.Point(85, 40);
            this.directoryControl_Організація.Name = "directoryControl_Організація";
            this.directoryControl_Організація.QueryFind = null;
            this.directoryControl_Організація.SelectForm = null;
            this.directoryControl_Організація.Size = new System.Drawing.Size(375, 27);
            this.directoryControl_Організація.TabIndex = 45;
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
            this.directoryControl_Підрозділ.AfterSelectFunc = null;
            this.directoryControl_Підрозділ.BeforeClickOpenFunc = null;
            this.directoryControl_Підрозділ.BeforeFindFunc = null;
            this.directoryControl_Підрозділ.Bind = null;
            this.directoryControl_Підрозділ.DirectoryPointerItem = null;
            this.directoryControl_Підрозділ.Location = new System.Drawing.Point(154, 48);
            this.directoryControl_Підрозділ.Name = "directoryControl_Підрозділ";
            this.directoryControl_Підрозділ.QueryFind = null;
            this.directoryControl_Підрозділ.SelectForm = null;
            this.directoryControl_Підрозділ.Size = new System.Drawing.Size(399, 27);
            this.directoryControl_Підрозділ.TabIndex = 61;
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.Controls.Add(this.tabControl1);
            this.panel2.Location = new System.Drawing.Point(3, 210);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(853, 368);
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
            this.tabControl1.Size = new System.Drawing.Size(853, 368);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.ВнутрішнєСпоживанняТоварів_ТабличнаЧастина_Товари);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(845, 342);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Товари";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // ВнутрішнєСпоживанняТоварів_ТабличнаЧастина_Товари
            // 
            this.ВнутрішнєСпоживанняТоварів_ТабличнаЧастина_Товари.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ВнутрішнєСпоживанняТоварів_ТабличнаЧастина_Товари.Location = new System.Drawing.Point(3, 3);
            this.ВнутрішнєСпоживанняТоварів_ТабличнаЧастина_Товари.Name = "ВнутрішнєСпоживанняТоварів_ТабличнаЧастина_Товари";
            this.ВнутрішнєСпоживанняТоварів_ТабличнаЧастина_Товари.Size = new System.Drawing.Size(839, 336);
            this.ВнутрішнєСпоживанняТоварів_ТабличнаЧастина_Товари.TabIndex = 0;
            this.ВнутрішнєСпоживанняТоварів_ТабличнаЧастина_Товари.ДокументОбєкт = null;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.label12);
            this.tabPage2.Controls.Add(this.directoryControl_Підрозділ);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(845, 342);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Додаток";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // panel3
            // 
            this.panel3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel3.Controls.Add(this.buttonSaveAndSpend);
            this.panel3.Controls.Add(this.buttonSpend);
            this.panel3.Controls.Add(this.buttonSave);
            this.panel3.Controls.Add(this.buttonClose);
            this.panel3.Location = new System.Drawing.Point(3, 584);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(853, 35);
            this.panel3.TabIndex = 25;
            // 
            // buttonSaveAndSpend
            // 
            this.buttonSaveAndSpend.Location = new System.Drawing.Point(3, 4);
            this.buttonSaveAndSpend.Name = "buttonSaveAndSpend";
            this.buttonSaveAndSpend.Size = new System.Drawing.Size(133, 27);
            this.buttonSaveAndSpend.TabIndex = 21;
            this.buttonSaveAndSpend.Text = "Зберегти і провести";
            this.buttonSaveAndSpend.UseVisualStyleBackColor = true;
            this.buttonSaveAndSpend.Click += new System.EventHandler(this.buttonSaveAndSpend_Click);
            // 
            // buttonSpend
            // 
            this.buttonSpend.Location = new System.Drawing.Point(312, 4);
            this.buttonSpend.Name = "buttonSpend";
            this.buttonSpend.Size = new System.Drawing.Size(91, 27);
            this.buttonSpend.TabIndex = 20;
            this.buttonSpend.Text = "Провести";
            this.buttonSpend.UseVisualStyleBackColor = true;
            this.buttonSpend.Click += new System.EventHandler(this.buttonSpend_Click);
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(215, 4);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(91, 27);
            this.buttonSave.TabIndex = 19;
            this.buttonSave.Text = "Зберегти";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton_FindToJournal,
            this.toolStripButtonДрукПроводок});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(864, 25);
            this.toolStrip1.TabIndex = 28;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton_FindToJournal
            // 
            this.toolStripButton_FindToJournal.Image = global::StorageAndTrade.Properties.Resources.up;
            this.toolStripButton_FindToJournal.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_FindToJournal.Name = "toolStripButton_FindToJournal";
            this.toolStripButton_FindToJournal.Size = new System.Drawing.Size(123, 22);
            this.toolStripButton_FindToJournal.Text = "Знайти в журналі";
            this.toolStripButton_FindToJournal.Click += new System.EventHandler(this.toolStripButton_FindToJournal_Click);
            // 
            // toolStripButtonДрукПроводок
            // 
            this.toolStripButtonДрукПроводок.Image = global::StorageAndTrade.Properties.Resources.page_2;
            this.toolStripButtonДрукПроводок.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonДрукПроводок.Name = "toolStripButtonДрукПроводок";
            this.toolStripButtonДрукПроводок.Size = new System.Drawing.Size(82, 22);
            this.toolStripButtonДрукПроводок.Text = "Проводки";
            this.toolStripButtonДрукПроводок.Click += new System.EventHandler(this.toolStripButtonДрукПроводок_Click);
            // 
            // Form_ВнутрішнєСпоживанняТоварівДокумент
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(864, 621);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form_ВнутрішнєСпоживанняТоварівДокумент";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Внутрішнє споживання товарів";
            this.Load += new System.EventHandler(this.Form_ВнутрішнєСпоживанняТоварівДокумент_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.TextBox textBox_НомерДок;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dateTimePicker_ДатаДок;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Label label3;
        private DirectoryControl directoryControl_Організація;
        private System.Windows.Forms.Label label4;
        private DirectoryControl directoryControl_Валюта;
        private System.Windows.Forms.Label label6;
        private DirectoryControl directoryControl_Склад;
        private System.Windows.Forms.ComboBox comboBox_ГосподарськаОперація;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label12;
        private DirectoryControl directoryControl_Підрозділ;
        private StorageAndTrade.Form_ВнутрішнєСпоживанняТоварів_ТабличнаЧастина_Товари ВнутрішнєСпоживанняТоварів_ТабличнаЧастина_Товари;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button buttonSaveAndSpend;
        private System.Windows.Forms.Button buttonSpend;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox textBox_Коментар;
        private System.Windows.Forms.DateTimePicker dateTimePicker_ЧасДок;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton_FindToJournal;
        private System.Windows.Forms.ToolStripButton toolStripButtonДрукПроводок;
    }
}