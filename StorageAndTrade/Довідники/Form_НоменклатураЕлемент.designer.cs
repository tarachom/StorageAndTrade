﻿
namespace StorageAndTrade
{
    partial class Form_НоменклатураЕлемент
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
            this.textBox_Назва = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.comboBox_ТипНоменклатури = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.directoryControl_ОдиницяВиміру = new StorageAndTrade.DirectoryControl();
            this.directoryControl_ВидНоменклатури = new StorageAndTrade.DirectoryControl();
            this.directoryControl_Виробник = new StorageAndTrade.DirectoryControl();
            this.textBox_Артикул = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textBox_НазваПовна = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.textBox_Опис = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonClose
            // 
            this.buttonClose.Location = new System.Drawing.Point(364, 300);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(164, 23);
            this.buttonClose.TabIndex = 16;
            this.buttonClose.Text = "Закрити";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(67, 300);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(164, 23);
            this.buttonSave.TabIndex = 15;
            this.buttonSave.Text = "Зберегти";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // textBox_Назва
            // 
            this.textBox_Назва.Location = new System.Drawing.Point(88, 12);
            this.textBox_Назва.Name = "textBox_Назва";
            this.textBox_Назва.Size = new System.Drawing.Size(461, 20);
            this.textBox_Назва.TabIndex = 20;
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
            this.label1.Size = new System.Drawing.Size(59, 13);
            this.label1.TabIndex = 24;
            this.label1.Text = "Виробник:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(17, 76);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(104, 13);
            this.label3.TabIndex = 26;
            this.label3.Text = "Вид номенклатури:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(18, 109);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(90, 13);
            this.label4.TabIndex = 28;
            this.label4.Text = "Одиниця виміру:";
            // 
            // comboBox_ТипНоменклатури
            // 
            this.comboBox_ТипНоменклатури.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_ТипНоменклатури.FormattingEnabled = true;
            this.comboBox_ТипНоменклатури.Location = new System.Drawing.Point(126, 135);
            this.comboBox_ТипНоменклатури.Name = "comboBox_ТипНоменклатури";
            this.comboBox_ТипНоменклатури.Size = new System.Drawing.Size(257, 21);
            this.comboBox_ТипНоменклатури.TabIndex = 30;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(78, 138);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 13);
            this.label5.TabIndex = 29;
            this.label5.Text = "Тип:";
            // 
            // directoryControl_ОдиницяВиміру
            // 
            this.directoryControl_ОдиницяВиміру.CallBack = null;
            this.directoryControl_ОдиницяВиміру.DirectoryPointerItem = null;
            this.directoryControl_ОдиницяВиміру.Location = new System.Drawing.Point(127, 102);
            this.directoryControl_ОдиницяВиміру.Name = "directoryControl_ОдиницяВиміру";
            this.directoryControl_ОдиницяВиміру.Size = new System.Drawing.Size(422, 27);
            this.directoryControl_ОдиницяВиміру.TabIndex = 27;
            // 
            // directoryControl_ВидНоменклатури
            // 
            this.directoryControl_ВидНоменклатури.CallBack = null;
            this.directoryControl_ВидНоменклатури.DirectoryPointerItem = null;
            this.directoryControl_ВидНоменклатури.Location = new System.Drawing.Point(126, 69);
            this.directoryControl_ВидНоменклатури.Name = "directoryControl_ВидНоменклатури";
            this.directoryControl_ВидНоменклатури.Size = new System.Drawing.Size(422, 27);
            this.directoryControl_ВидНоменклатури.TabIndex = 25;
            // 
            // directoryControl_Виробник
            // 
            this.directoryControl_Виробник.CallBack = null;
            this.directoryControl_Виробник.DirectoryPointerItem = null;
            this.directoryControl_Виробник.Location = new System.Drawing.Point(84, 38);
            this.directoryControl_Виробник.Name = "directoryControl_Виробник";
            this.directoryControl_Виробник.Size = new System.Drawing.Size(465, 27);
            this.directoryControl_Виробник.TabIndex = 23;
            // 
            // textBox_Артикул
            // 
            this.textBox_Артикул.Location = new System.Drawing.Point(87, 163);
            this.textBox_Артикул.Name = "textBox_Артикул";
            this.textBox_Артикул.Size = new System.Drawing.Size(461, 20);
            this.textBox_Артикул.TabIndex = 32;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(28, 166);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(51, 13);
            this.label6.TabIndex = 31;
            this.label6.Text = "Артикул:";
            // 
            // textBox_НазваПовна
            // 
            this.textBox_НазваПовна.Location = new System.Drawing.Point(109, 189);
            this.textBox_НазваПовна.Name = "textBox_НазваПовна";
            this.textBox_НазваПовна.Size = new System.Drawing.Size(439, 20);
            this.textBox_НазваПовна.TabIndex = 34;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(28, 192);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(75, 13);
            this.label7.TabIndex = 33;
            this.label7.Text = "Назва повна:";
            // 
            // textBox_Опис
            // 
            this.textBox_Опис.Location = new System.Drawing.Point(109, 215);
            this.textBox_Опис.Name = "textBox_Опис";
            this.textBox_Опис.Size = new System.Drawing.Size(439, 20);
            this.textBox_Опис.TabIndex = 36;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(28, 218);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(36, 13);
            this.label8.TabIndex = 35;
            this.label8.Text = "Опис:";
            // 
            // Form_НоменклатураЕлемент
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(565, 346);
            this.Controls.Add(this.textBox_Опис);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.textBox_НазваПовна);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.textBox_Артикул);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.comboBox_ТипНоменклатури);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.directoryControl_ОдиницяВиміру);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.directoryControl_ВидНоменклатури);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.directoryControl_Виробник);
            this.Controls.Add(this.textBox_Назва);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.buttonSave);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Form_НоменклатураЕлемент";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Номенклатура";
            this.Load += new System.EventHandler(this.FormAddCash_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.TextBox textBox_Назва;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private DirectoryControl directoryControl_Виробник;
        private System.Windows.Forms.Label label3;
        private DirectoryControl directoryControl_ВидНоменклатури;
        private System.Windows.Forms.Label label4;
        private DirectoryControl directoryControl_ОдиницяВиміру;
        private System.Windows.Forms.ComboBox comboBox_ТипНоменклатури;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox_Артикул;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBox_НазваПовна;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBox_Опис;
        private System.Windows.Forms.Label label8;
    }
}