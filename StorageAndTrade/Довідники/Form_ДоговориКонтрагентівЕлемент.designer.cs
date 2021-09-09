
namespace StorageAndTrade
{
    partial class Form_ДоговориКонтрагентівЕлемент
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
            this.textBoxName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.directoryControl_БанківськийРахунок = new StorageAndTrade.DirectoryControl();
            this.label3 = new System.Windows.Forms.Label();
            this.directoryControl_БанківськийРахунокКонтрагента = new StorageAndTrade.DirectoryControl();
            this.label4 = new System.Windows.Forms.Label();
            this.directoryControl_Підрозділ = new StorageAndTrade.DirectoryControl();
            this.label5 = new System.Windows.Forms.Label();
            this.directoryControl_Контрагент = new StorageAndTrade.DirectoryControl();
            this.SuspendLayout();
            // 
            // buttonClose
            // 
            this.buttonClose.Location = new System.Drawing.Point(384, 244);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(164, 23);
            this.buttonClose.TabIndex = 16;
            this.buttonClose.Text = "Закрити";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(87, 244);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(164, 23);
            this.buttonSave.TabIndex = 15;
            this.buttonSave.Text = "Зберегти";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // textBoxName
            // 
            this.textBoxName.Location = new System.Drawing.Point(88, 12);
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.Size = new System.Drawing.Size(461, 20);
            this.textBoxName.TabIndex = 20;
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
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(116, 13);
            this.label1.TabIndex = 36;
            this.label1.Text = "Банківський рахунок:";
            // 
            // directoryControl_БанківськийРахунок
            // 
            this.directoryControl_БанківськийРахунок.CallBack = null;
            this.directoryControl_БанківськийРахунок.DirectoryPointerItem = null;
            this.directoryControl_БанківськийРахунок.Location = new System.Drawing.Point(140, 38);
            this.directoryControl_БанківськийРахунок.Name = "directoryControl_БанківськийРахунок";
            this.directoryControl_БанківськийРахунок.Size = new System.Drawing.Size(409, 27);
            this.directoryControl_БанківськийРахунок.TabIndex = 35;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(17, 76);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(182, 13);
            this.label3.TabIndex = 38;
            this.label3.Text = "Банківський рахунок контрагента:";
            // 
            // directoryControl_БанківськийРахунокКонтрагента
            // 
            this.directoryControl_БанківськийРахунокКонтрагента.CallBack = null;
            this.directoryControl_БанківськийРахунокКонтрагента.DirectoryPointerItem = null;
            this.directoryControl_БанківськийРахунокКонтрагента.Location = new System.Drawing.Point(205, 69);
            this.directoryControl_БанківськийРахунокКонтрагента.Name = "directoryControl_БанківськийРахунокКонтрагента";
            this.directoryControl_БанківськийРахунокКонтрагента.Size = new System.Drawing.Size(343, 27);
            this.directoryControl_БанківськийРахунокКонтрагента.TabIndex = 37;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(17, 109);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(58, 13);
            this.label4.TabIndex = 40;
            this.label4.Text = "Підрозділ:";
            // 
            // directoryControl_Підрозділ
            // 
            this.directoryControl_Підрозділ.CallBack = null;
            this.directoryControl_Підрозділ.DirectoryPointerItem = null;
            this.directoryControl_Підрозділ.Location = new System.Drawing.Point(110, 102);
            this.directoryControl_Підрозділ.Name = "directoryControl_Підрозділ";
            this.directoryControl_Підрозділ.Size = new System.Drawing.Size(438, 27);
            this.directoryControl_Підрозділ.TabIndex = 39;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(17, 142);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(68, 13);
            this.label5.TabIndex = 42;
            this.label5.Text = "Контрагент:";
            // 
            // directoryControl_Контрагент
            // 
            this.directoryControl_Контрагент.CallBack = null;
            this.directoryControl_Контрагент.DirectoryPointerItem = null;
            this.directoryControl_Контрагент.Location = new System.Drawing.Point(110, 135);
            this.directoryControl_Контрагент.Name = "directoryControl_Контрагент";
            this.directoryControl_Контрагент.Size = new System.Drawing.Size(438, 27);
            this.directoryControl_Контрагент.TabIndex = 41;
            // 
            // Form_ДоговориКонтрагентівЕлемент
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(565, 279);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.directoryControl_Контрагент);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.directoryControl_Підрозділ);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.directoryControl_БанківськийРахунокКонтрагента);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.directoryControl_БанківськийРахунок);
            this.Controls.Add(this.textBoxName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.buttonSave);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Form_ДоговориКонтрагентівЕлемент";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Договори контрагентів";
            this.Load += new System.EventHandler(this.FormAddCash_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.TextBox textBoxName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private DirectoryControl directoryControl_БанківськийРахунок;
        private System.Windows.Forms.Label label3;
        private DirectoryControl directoryControl_БанківськийРахунокКонтрагента;
        private System.Windows.Forms.Label label4;
        private DirectoryControl directoryControl_Підрозділ;
        private System.Windows.Forms.Label label5;
        private DirectoryControl directoryControl_Контрагент;
    }
}