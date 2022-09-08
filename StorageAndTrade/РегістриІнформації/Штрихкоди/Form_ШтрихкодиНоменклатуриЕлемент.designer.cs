
namespace StorageAndTrade
{
    partial class Form_ШтрихкодиНоменклатуриЕлемент
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_ШтрихкодиНоменклатуриЕлемент));
            this.buttonClose = new System.Windows.Forms.Button();
            this.buttonSave = new System.Windows.Forms.Button();
            this.textBox_Штрихкод = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.directoryControl_ХарактеристикаНоменклатури = new StorageAndTrade.DirectoryControl();
            this.label7 = new System.Windows.Forms.Label();
            this.directoryControl_Номенклатура = new StorageAndTrade.DirectoryControl();
            this.label3 = new System.Windows.Forms.Label();
            this.directoryControl_Пакування = new StorageAndTrade.DirectoryControl();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonClose
            // 
            this.buttonClose.Location = new System.Drawing.Point(371, 160);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(103, 27);
            this.buttonClose.TabIndex = 16;
            this.buttonClose.Text = "Закрити";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(108, 160);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(164, 27);
            this.buttonSave.TabIndex = 15;
            this.buttonSave.Text = "Зберегти";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // textBox_Штрихкод
            // 
            this.textBox_Штрихкод.Location = new System.Drawing.Point(108, 12);
            this.textBox_Штрихкод.Name = "textBox_Штрихкод";
            this.textBox_Штрихкод.Size = new System.Drawing.Size(366, 20);
            this.textBox_Штрихкод.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(43, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 13);
            this.label2.TabIndex = 19;
            this.label2.Text = "Штрихкод:";
            // 
            // directoryControl_ХарактеристикаНоменклатури
            // 
            this.directoryControl_ХарактеристикаНоменклатури.AfterSelectFunc = null;
            this.directoryControl_ХарактеристикаНоменклатури.BeforeClickOpenFunc = null;
            this.directoryControl_ХарактеристикаНоменклатури.BeforeFindFunc = null;
            this.directoryControl_ХарактеристикаНоменклатури.Bind = null;
            this.directoryControl_ХарактеристикаНоменклатури.DirectoryPointerItem = null;
            this.directoryControl_ХарактеристикаНоменклатури.Location = new System.Drawing.Point(108, 71);
            this.directoryControl_ХарактеристикаНоменклатури.Name = "directoryControl_ХарактеристикаНоменклатури";
            this.directoryControl_ХарактеристикаНоменклатури.QueryFind = null;
            this.directoryControl_ХарактеристикаНоменклатури.SelectForm = null;
            this.directoryControl_ХарактеристикаНоменклатури.Size = new System.Drawing.Size(366, 27);
            this.directoryControl_ХарактеристикаНоменклатури.TabIndex = 70;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(9, 78);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(93, 13);
            this.label7.TabIndex = 71;
            this.label7.Text = "Характеристика:";
            // 
            // directoryControl_Номенклатура
            // 
            this.directoryControl_Номенклатура.AfterSelectFunc = null;
            this.directoryControl_Номенклатура.BeforeClickOpenFunc = null;
            this.directoryControl_Номенклатура.BeforeFindFunc = null;
            this.directoryControl_Номенклатура.Bind = null;
            this.directoryControl_Номенклатура.DirectoryPointerItem = null;
            this.directoryControl_Номенклатура.Location = new System.Drawing.Point(108, 38);
            this.directoryControl_Номенклатура.Name = "directoryControl_Номенклатура";
            this.directoryControl_Номенклатура.QueryFind = null;
            this.directoryControl_Номенклатура.SelectForm = null;
            this.directoryControl_Номенклатура.Size = new System.Drawing.Size(366, 27);
            this.directoryControl_Номенклатура.TabIndex = 68;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 45);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(84, 13);
            this.label3.TabIndex = 69;
            this.label3.Text = "Номенклатура:";
            // 
            // directoryControl_Пакування
            // 
            this.directoryControl_Пакування.AfterSelectFunc = null;
            this.directoryControl_Пакування.BeforeClickOpenFunc = null;
            this.directoryControl_Пакування.BeforeFindFunc = null;
            this.directoryControl_Пакування.Bind = null;
            this.directoryControl_Пакування.DirectoryPointerItem = null;
            this.directoryControl_Пакування.Location = new System.Drawing.Point(108, 104);
            this.directoryControl_Пакування.Name = "directoryControl_Пакування";
            this.directoryControl_Пакування.QueryFind = null;
            this.directoryControl_Пакування.SelectForm = null;
            this.directoryControl_Пакування.Size = new System.Drawing.Size(366, 27);
            this.directoryControl_Пакування.TabIndex = 72;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 111);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 13);
            this.label1.TabIndex = 73;
            this.label1.Text = "Пакування:";
            // 
            // Form_ШтрихкодиНоменклатуриЕлемент
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(506, 199);
            this.Controls.Add(this.directoryControl_Пакування);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.directoryControl_ХарактеристикаНоменклатури);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.directoryControl_Номенклатура);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBox_Штрихкод);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.buttonSave);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form_ШтрихкодиНоменклатуриЕлемент";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Штрихкод";
            this.Load += new System.EventHandler(this.Form_ШтрихкодиНоменклатуриЕлемент_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.TextBox textBox_Штрихкод;
        private System.Windows.Forms.Label label2;
        private DirectoryControl directoryControl_ХарактеристикаНоменклатури;
        private System.Windows.Forms.Label label7;
        private DirectoryControl directoryControl_Номенклатура;
        private System.Windows.Forms.Label label3;
        private DirectoryControl directoryControl_Пакування;
        private System.Windows.Forms.Label label1;
    }
}