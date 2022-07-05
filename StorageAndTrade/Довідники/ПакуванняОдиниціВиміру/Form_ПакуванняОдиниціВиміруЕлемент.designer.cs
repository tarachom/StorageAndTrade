
namespace StorageAndTrade
{
    partial class Form_ПакуванняОдиниціВиміруЕлемент
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_ПакуванняОдиниціВиміруЕлемент));
            this.buttonClose = new System.Windows.Forms.Button();
            this.buttonSave = new System.Windows.Forms.Button();
            this.textBox_Назва = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_НазваПовна = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_КількістьУпаковок = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox_Код = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonClose
            // 
            this.buttonClose.Location = new System.Drawing.Point(385, 101);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(164, 27);
            this.buttonClose.TabIndex = 16;
            this.buttonClose.Text = "Закрити";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(124, 101);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(164, 27);
            this.buttonSave.TabIndex = 15;
            this.buttonSave.Text = "Зберегти";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // textBox_Назва
            // 
            this.textBox_Назва.Location = new System.Drawing.Point(124, 12);
            this.textBox_Назва.Name = "textBox_Назва";
            this.textBox_Назва.Size = new System.Drawing.Size(425, 20);
            this.textBox_Назва.TabIndex = 20;
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
            // textBox_НазваПовна
            // 
            this.textBox_НазваПовна.Location = new System.Drawing.Point(124, 38);
            this.textBox_НазваПовна.Name = "textBox_НазваПовна";
            this.textBox_НазваПовна.Size = new System.Drawing.Size(591, 20);
            this.textBox_НазваПовна.TabIndex = 22;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 13);
            this.label1.TabIndex = 21;
            this.label1.Text = "Повна назва:";
            // 
            // textBox_КількістьУпаковок
            // 
            this.textBox_КількістьУпаковок.Location = new System.Drawing.Point(124, 64);
            this.textBox_КількістьУпаковок.Name = "textBox_КількістьУпаковок";
            this.textBox_КількістьУпаковок.Size = new System.Drawing.Size(126, 20);
            this.textBox_КількістьУпаковок.TabIndex = 24;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 67);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(106, 13);
            this.label3.TabIndex = 23;
            this.label3.Text = "Кількість упаковок:";
            // 
            // textBox_Код
            // 
            this.textBox_Код.Location = new System.Drawing.Point(588, 12);
            this.textBox_Код.Name = "textBox_Код";
            this.textBox_Код.Size = new System.Drawing.Size(127, 20);
            this.textBox_Код.TabIndex = 26;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(553, 15);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 13);
            this.label4.TabIndex = 25;
            this.label4.Text = "Код:";
            // 
            // Form_ПакуванняОдиниціВиміруЕлемент
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(730, 142);
            this.Controls.Add(this.textBox_Код);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBox_КількістьУпаковок);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBox_НазваПовна);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox_Назва);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.buttonSave);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form_ПакуванняОдиниціВиміруЕлемент";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Пакування одиниці виміру";
            this.Load += new System.EventHandler(this.Form_ПакуванняОдиниціВиміруЕлемент_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.TextBox textBox_Назва;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_НазваПовна;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_КількістьУпаковок;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox_Код;
        private System.Windows.Forms.Label label4;
    }
}