
namespace StorageAndTrade
{
    partial class Form_РозрахункиЗКлієнтами_Звіт
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_РозрахункиЗКлієнтами_Звіт));
            this.dateTimeStart = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.dateTimeStop = new System.Windows.Forms.DateTimePicker();
            this.buttonCreate = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.directoryControl_Валюти = new StorageAndTrade.DirectoryControl();
            this.directoryControl_Контрагенти = new StorageAndTrade.DirectoryControl();
            this.directoryControl_КонтрагентиПапка = new StorageAndTrade.DirectoryControl();
            this.buttonClose = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button_Documents = new System.Windows.Forms.Button();
            this.buttonOstatokAndOborot = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dateTimeStart
            // 
            this.dateTimeStart.Location = new System.Drawing.Point(323, 10);
            this.dateTimeStart.Name = "dateTimeStart";
            this.dateTimeStart.Size = new System.Drawing.Size(197, 20);
            this.dateTimeStart.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(267, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Період з";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(526, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(19, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "по";
            // 
            // dateTimeStop
            // 
            this.dateTimeStop.Location = new System.Drawing.Point(548, 10);
            this.dateTimeStop.Name = "dateTimeStop";
            this.dateTimeStop.Size = new System.Drawing.Size(197, 20);
            this.dateTimeStop.TabIndex = 3;
            // 
            // buttonCreate
            // 
            this.buttonCreate.Location = new System.Drawing.Point(12, 147);
            this.buttonCreate.Name = "buttonCreate";
            this.buttonCreate.Size = new System.Drawing.Size(90, 27);
            this.buttonCreate.TabIndex = 4;
            this.buttonCreate.Text = "Залишки";
            this.buttonCreate.UseVisualStyleBackColor = true;
            this.buttonCreate.Click += new System.EventHandler(this.buttonCreate_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(56, 26);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(42, 13);
            this.label9.TabIndex = 56;
            this.label9.Text = "Папка:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(24, 59);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(74, 13);
            this.label3.TabIndex = 58;
            this.label3.Text = "Контрагенти:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(548, 60);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(48, 13);
            this.label5.TabIndex = 62;
            this.label5.Text = "Валюта:";
            // 
            // directoryControl_Валюти
            // 
            this.directoryControl_Валюти.AfterSelectFunc = null;
            this.directoryControl_Валюти.BeforeClickOpenFunc = null;
            this.directoryControl_Валюти.BeforeFindFunc = null;
            this.directoryControl_Валюти.Bind = null;
            this.directoryControl_Валюти.DirectoryPointerItem = null;
            this.directoryControl_Валюти.Location = new System.Drawing.Point(607, 53);
            this.directoryControl_Валюти.Name = "directoryControl_Валюти";
            this.directoryControl_Валюти.QueryFind = null;
            this.directoryControl_Валюти.SelectForm = null;
            this.directoryControl_Валюти.Size = new System.Drawing.Size(293, 27);
            this.directoryControl_Валюти.TabIndex = 61;
            // 
            // directoryControl_Контрагенти
            // 
            this.directoryControl_Контрагенти.AfterSelectFunc = null;
            this.directoryControl_Контрагенти.BeforeClickOpenFunc = null;
            this.directoryControl_Контрагенти.BeforeFindFunc = null;
            this.directoryControl_Контрагенти.Bind = null;
            this.directoryControl_Контрагенти.DirectoryPointerItem = null;
            this.directoryControl_Контрагенти.Location = new System.Drawing.Point(106, 52);
            this.directoryControl_Контрагенти.Name = "directoryControl_Контрагенти";
            this.directoryControl_Контрагенти.QueryFind = null;
            this.directoryControl_Контрагенти.SelectForm = null;
            this.directoryControl_Контрагенти.Size = new System.Drawing.Size(402, 27);
            this.directoryControl_Контрагенти.TabIndex = 57;
            // 
            // directoryControl_КонтрагентиПапка
            // 
            this.directoryControl_КонтрагентиПапка.AfterSelectFunc = null;
            this.directoryControl_КонтрагентиПапка.BeforeClickOpenFunc = null;
            this.directoryControl_КонтрагентиПапка.BeforeFindFunc = null;
            this.directoryControl_КонтрагентиПапка.Bind = null;
            this.directoryControl_КонтрагентиПапка.DirectoryPointerItem = null;
            this.directoryControl_КонтрагентиПапка.Location = new System.Drawing.Point(106, 19);
            this.directoryControl_КонтрагентиПапка.Name = "directoryControl_КонтрагентиПапка";
            this.directoryControl_КонтрагентиПапка.QueryFind = null;
            this.directoryControl_КонтрагентиПапка.SelectForm = null;
            this.directoryControl_КонтрагентиПапка.Size = new System.Drawing.Size(402, 27);
            this.directoryControl_КонтрагентиПапка.TabIndex = 55;
            // 
            // buttonClose
            // 
            this.buttonClose.Location = new System.Drawing.Point(816, 147);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(90, 27);
            this.buttonClose.TabIndex = 71;
            this.buttonClose.Text = "Закрити";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.directoryControl_Контрагенти);
            this.groupBox1.Controls.Add(this.directoryControl_КонтрагентиПапка);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Location = new System.Drawing.Point(12, 41);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(523, 93);
            this.groupBox1.TabIndex = 72;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Контрагент";
            // 
            // button_Documents
            // 
            this.button_Documents.Location = new System.Drawing.Point(275, 147);
            this.button_Documents.Name = "button_Documents";
            this.button_Documents.Size = new System.Drawing.Size(96, 27);
            this.button_Documents.TabIndex = 79;
            this.button_Documents.Text = "По документах";
            this.button_Documents.UseVisualStyleBackColor = true;
            this.button_Documents.Click += new System.EventHandler(this.button_Documents_Click);
            // 
            // buttonOstatokAndOborot
            // 
            this.buttonOstatokAndOborot.Location = new System.Drawing.Point(108, 147);
            this.buttonOstatokAndOborot.Name = "buttonOstatokAndOborot";
            this.buttonOstatokAndOborot.Size = new System.Drawing.Size(161, 27);
            this.buttonOstatokAndOborot.TabIndex = 80;
            this.buttonOstatokAndOborot.Text = "Залишки та обороти";
            this.buttonOstatokAndOborot.UseVisualStyleBackColor = true;
            this.buttonOstatokAndOborot.Click += new System.EventHandler(this.buttonOstatokAndOborot_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label8.Location = new System.Drawing.Point(12, 7);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(253, 25);
            this.label8.TabIndex = 84;
            this.label8.Text = "Розрахунки з клієнтами";
            // 
            // Form_РозрахункиЗКлієнтами_Звіт
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(918, 579);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.buttonOstatokAndOborot);
            this.Controls.Add(this.button_Documents);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.directoryControl_Валюти);
            this.Controls.Add(this.dateTimeStop);
            this.Controls.Add(this.buttonCreate);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dateTimeStart);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form_РозрахункиЗКлієнтами_Звіт";
            this.Text = "Звіт \"Розрахунки з клієнтами\"";
            this.Load += new System.EventHandler(this.Form_РозрахункиЗКлієнтами_Звіт_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DateTimePicker dateTimeStart;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dateTimeStop;
        private System.Windows.Forms.Button buttonCreate;
        private System.Windows.Forms.Label label9;
        private DirectoryControl directoryControl_КонтрагентиПапка;
        private System.Windows.Forms.Label label3;
        private DirectoryControl directoryControl_Контрагенти;
        private System.Windows.Forms.Label label5;
        private DirectoryControl directoryControl_Валюти;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button_Documents;
        private System.Windows.Forms.Button buttonOstatokAndOborot;
        private System.Windows.Forms.Label label8;
    }
}