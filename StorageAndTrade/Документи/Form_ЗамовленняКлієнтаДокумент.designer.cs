
namespace StorageAndTrade
{
    partial class Form_ЗамовленняКлієнтаДокумент
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
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.замовленняКлієнта_ТабличнаЧастина_Товари = new StorageAndTrade.ЗамовленняКлієнта_ТабличнаЧастина_Товари();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonClose
            // 
            this.buttonClose.Location = new System.Drawing.Point(173, 12);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(164, 23);
            this.buttonClose.TabIndex = 16;
            this.buttonClose.Text = "Закрити";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(3, 12);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(164, 23);
            this.buttonSave.TabIndex = 15;
            this.buttonSave.Text = "Зберегти";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // textBox_НомерДок
            // 
            this.textBox_НомерДок.Location = new System.Drawing.Point(64, 19);
            this.textBox_НомерДок.Name = "textBox_НомерДок";
            this.textBox_НомерДок.Size = new System.Drawing.Size(145, 20);
            this.textBox_НомерДок.TabIndex = 20;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 13);
            this.label2.TabIndex = 19;
            this.label2.Text = "Номер:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(240, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 13);
            this.label1.TabIndex = 21;
            this.label1.Text = "Дата:";
            // 
            // dateTimePicker_ДатаДок
            // 
            this.dateTimePicker_ДатаДок.Location = new System.Drawing.Point(282, 19);
            this.dateTimePicker_ДатаДок.Name = "dateTimePicker_ДатаДок";
            this.dateTimePicker_ДатаДок.Size = new System.Drawing.Size(200, 20);
            this.dateTimePicker_ДатаДок.TabIndex = 22;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.dateTimePicker_ДатаДок);
            this.panel1.Controls.Add(this.textBox_НомерДок);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(3, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(965, 236);
            this.panel1.TabIndex = 23;
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.Controls.Add(this.замовленняКлієнта_ТабличнаЧастина_Товари);
            this.panel2.Location = new System.Drawing.Point(3, 244);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(965, 310);
            this.panel2.TabIndex = 24;
            // 
            // panel3
            // 
            this.panel3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel3.Controls.Add(this.buttonClose);
            this.panel3.Controls.Add(this.buttonSave);
            this.panel3.Location = new System.Drawing.Point(3, 560);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(965, 47);
            this.panel3.TabIndex = 25;
            // 
            // замовленняКлієнта_ТабличнаЧастина_Товари
            // 
            this.замовленняКлієнта_ТабличнаЧастина_Товари.Dock = System.Windows.Forms.DockStyle.Fill;
            this.замовленняКлієнта_ТабличнаЧастина_Товари.Location = new System.Drawing.Point(0, 0);
            this.замовленняКлієнта_ТабличнаЧастина_Товари.Name = "замовленняКлієнта_ТабличнаЧастина_Товари";
            this.замовленняКлієнта_ТабличнаЧастина_Товари.Size = new System.Drawing.Size(965, 310);
            this.замовленняКлієнта_ТабличнаЧастина_Товари.TabIndex = 0;
            this.замовленняКлієнта_ТабличнаЧастина_Товари.ЗамовленняКлієнта_Objest = null;
            // 
            // Form_ЗамовленняКлієнтаДокумент
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(976, 619);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Form_ЗамовленняКлієнтаДокумент";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Валюти";
            this.Load += new System.EventHandler(this.FormAddCash_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
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
        private ЗамовленняКлієнта_ТабличнаЧастина_Товари замовленняКлієнта_ТабличнаЧастина_Товари;
    }
}