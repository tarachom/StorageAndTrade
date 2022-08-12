
namespace StorageAndTrade
{
    partial class Form_ВстановленняЦінНоменклатури_ТабличнаЧастина_Товари
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_ВстановленняЦінНоменклатури_ТабличнаЧастина_Товари));
            this.panel2 = new System.Windows.Forms.Panel();
            this.dataGridViewRecords = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonAdd = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonCopy = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonDelete = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonFillDirectory = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonFillRegister = new System.Windows.Forms.ToolStripButton();
            this.toolStripDropDownButtonOperation = new System.Windows.Forms.ToolStripDropDownButton();
            this.видалитиТовариЗЦіноюНульToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.встановитиВидЦіниToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewRecords)).BeginInit();
            this.panel1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dataGridViewRecords);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 29);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(733, 219);
            this.panel2.TabIndex = 4;
            // 
            // dataGridViewRecords
            // 
            this.dataGridViewRecords.AllowUserToAddRows = false;
            this.dataGridViewRecords.AllowUserToDeleteRows = false;
            this.dataGridViewRecords.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.dataGridViewRecords.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridViewRecords.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewRecords.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewRecords.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewRecords.Name = "dataGridViewRecords";
            this.dataGridViewRecords.RowHeadersVisible = false;
            this.dataGridViewRecords.Size = new System.Drawing.Size(733, 219);
            this.dataGridViewRecords.TabIndex = 0;
            this.dataGridViewRecords.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewRecords_CellDoubleClick);
            this.dataGridViewRecords.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewRecords_CellEndEdit);
            this.dataGridViewRecords.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dataGridViewRecords_KeyDown);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.toolStrip1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(733, 29);
            this.panel1.TabIndex = 3;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonAdd,
            this.toolStripButtonCopy,
            this.toolStripButtonDelete,
            this.toolStripSeparator1,
            this.toolStripButtonFillDirectory,
            this.toolStripButtonFillRegister,
            this.toolStripDropDownButtonOperation});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(733, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButtonAdd
            // 
            this.toolStripButtonAdd.Image = global::StorageAndTrade.Properties.Resources.add_document;
            this.toolStripButtonAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonAdd.Name = "toolStripButtonAdd";
            this.toolStripButtonAdd.Size = new System.Drawing.Size(66, 22);
            this.toolStripButtonAdd.Text = "Додати";
            this.toolStripButtonAdd.Click += new System.EventHandler(this.toolStripButtonAdd_Click);
            // 
            // toolStripButtonCopy
            // 
            this.toolStripButtonCopy.Image = global::StorageAndTrade.Properties.Resources.page_copy;
            this.toolStripButtonCopy.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonCopy.Name = "toolStripButtonCopy";
            this.toolStripButtonCopy.Size = new System.Drawing.Size(85, 22);
            this.toolStripButtonCopy.Text = "Копіювати";
            this.toolStripButtonCopy.Click += new System.EventHandler(this.toolStripButtonCopy_Click);
            // 
            // toolStripButtonDelete
            // 
            this.toolStripButtonDelete.Image = global::StorageAndTrade.Properties.Resources.page_white_delete;
            this.toolStripButtonDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonDelete.Name = "toolStripButtonDelete";
            this.toolStripButtonDelete.Size = new System.Drawing.Size(79, 22);
            this.toolStripButtonDelete.Text = "Видалити";
            this.toolStripButtonDelete.Click += new System.EventHandler(this.toolStripButtonDelete_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButtonFillDirectory
            // 
            this.toolStripButtonFillDirectory.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonFillDirectory.Image")));
            this.toolStripButtonFillDirectory.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonFillDirectory.Name = "toolStripButtonFillDirectory";
            this.toolStripButtonFillDirectory.Size = new System.Drawing.Size(142, 22);
            this.toolStripButtonFillDirectory.Text = "Заповнити товарами";
            this.toolStripButtonFillDirectory.Click += new System.EventHandler(this.toolStripButtonFillDirectory_Click);
            // 
            // toolStripButtonFillRegister
            // 
            this.toolStripButtonFillRegister.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonFillRegister.Image")));
            this.toolStripButtonFillRegister.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonFillRegister.Name = "toolStripButtonFillRegister";
            this.toolStripButtonFillRegister.Size = new System.Drawing.Size(193, 22);
            this.toolStripButtonFillRegister.Text = "Заповнити існуючими цінами";
            this.toolStripButtonFillRegister.Click += new System.EventHandler(this.toolStripButtonFillRegister_Click);
            // 
            // toolStripDropDownButtonOperation
            // 
            this.toolStripDropDownButtonOperation.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.видалитиТовариЗЦіноюНульToolStripMenuItem,
            this.встановитиВидЦіниToolStripMenuItem});
            this.toolStripDropDownButtonOperation.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButtonOperation.Image")));
            this.toolStripDropDownButtonOperation.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButtonOperation.Name = "toolStripDropDownButtonOperation";
            this.toolStripDropDownButtonOperation.Size = new System.Drawing.Size(84, 22);
            this.toolStripDropDownButtonOperation.Text = "Операції";
            // 
            // видалитиТовариЗЦіноюНульToolStripMenuItem
            // 
            this.видалитиТовариЗЦіноюНульToolStripMenuItem.Name = "видалитиТовариЗЦіноюНульToolStripMenuItem";
            this.видалитиТовариЗЦіноюНульToolStripMenuItem.Size = new System.Drawing.Size(221, 22);
            this.видалитиТовариЗЦіноюНульToolStripMenuItem.Text = "Видалити товари з ціною 0";
            this.видалитиТовариЗЦіноюНульToolStripMenuItem.Click += new System.EventHandler(this.видалитиТовариЗЦіноюНульToolStripMenuItem_Click);
            // 
            // встановитиВидЦіниToolStripMenuItem
            // 
            this.встановитиВидЦіниToolStripMenuItem.Name = "встановитиВидЦіниToolStripMenuItem";
            this.встановитиВидЦіниToolStripMenuItem.Size = new System.Drawing.Size(221, 22);
            this.встановитиВидЦіниToolStripMenuItem.Text = "Встановити вид ціни";
            this.встановитиВидЦіниToolStripMenuItem.Click += new System.EventHandler(this.встановитиВидЦіниToolStripMenuItem_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // Form_ВстановленняЦінНоменклатури_ТабличнаЧастина_Товари
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "Form_ВстановленняЦінНоменклатури_ТабличнаЧастина_Товари";
            this.Size = new System.Drawing.Size(733, 248);
            this.Load += new System.EventHandler(this.ВстановленняЦінНоменклатури_ТабличнаЧастина_Товари);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewRecords)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DataGridView dataGridViewRecords;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButtonAdd;
        private System.Windows.Forms.ToolStripButton toolStripButtonCopy;
        private System.Windows.Forms.ToolStripButton toolStripButtonDelete;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripButtonFillRegister;
        private System.Windows.Forms.ToolStripButton toolStripButtonFillDirectory;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButtonOperation;
        private System.Windows.Forms.ToolStripMenuItem видалитиТовариЗЦіноюНульToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem встановитиВидЦіниToolStripMenuItem;
    }
}
