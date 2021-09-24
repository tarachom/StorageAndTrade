
namespace StorageAndTrade
{
    partial class Form1
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
            this.table_body = new System.Windows.Forms.TableLayoutPanel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.table_head = new System.Windows.Forms.TableLayoutPanel();
            this.table_footer = new System.Windows.Forms.TableLayoutPanel();
            this.documentControl_ЗамовленняКлієнта = new StorageAndTrade.DocumentControl();
            this.button_ok = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // table_body
            // 
            this.table_body.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.table_body.AutoSize = true;
            this.table_body.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.table_body.ColumnCount = 11;
            this.table_body.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.table_body.Location = new System.Drawing.Point(3, 43);
            this.table_body.Name = "table_body";
            this.table_body.RowCount = 1;
            this.table_body.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.table_body.Size = new System.Drawing.Size(480, 31);
            this.table_body.TabIndex = 0;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.button_ok);
            this.splitContainer1.Panel1.Controls.Add(this.documentControl_ЗамовленняКлієнта);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.AutoScroll = true;
            this.splitContainer1.Panel2.BackColor = System.Drawing.SystemColors.Info;
            this.splitContainer1.Panel2.Controls.Add(this.table_footer);
            this.splitContainer1.Panel2.Controls.Add(this.table_head);
            this.splitContainer1.Panel2.Controls.Add(this.table_body);
            this.splitContainer1.Size = new System.Drawing.Size(800, 450);
            this.splitContainer1.SplitterDistance = 310;
            this.splitContainer1.TabIndex = 1;
            // 
            // table_head
            // 
            this.table_head.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.table_head.AutoSize = true;
            this.table_head.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.table_head.ColumnCount = 1;
            this.table_head.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.table_head.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.table_head.Location = new System.Drawing.Point(3, 3);
            this.table_head.Name = "table_head";
            this.table_head.RowCount = 1;
            this.table_head.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.table_head.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.table_head.Size = new System.Drawing.Size(480, 37);
            this.table_head.TabIndex = 1;
            // 
            // table_footer
            // 
            this.table_footer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.table_footer.AutoSize = true;
            this.table_footer.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.table_footer.ColumnCount = 1;
            this.table_footer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.table_footer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.table_footer.Location = new System.Drawing.Point(3, 80);
            this.table_footer.Name = "table_footer";
            this.table_footer.RowCount = 1;
            this.table_footer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.table_footer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.table_footer.Size = new System.Drawing.Size(480, 37);
            this.table_footer.TabIndex = 2;
            // 
            // documentControl_ЗамовленняКлієнта
            // 
            this.documentControl_ЗамовленняКлієнта.DocumentPointerItem = null;
            this.documentControl_ЗамовленняКлієнта.Location = new System.Drawing.Point(12, 13);
            this.documentControl_ЗамовленняКлієнта.Name = "documentControl_ЗамовленняКлієнта";
            this.documentControl_ЗамовленняКлієнта.SelectForm = null;
            this.documentControl_ЗамовленняКлієнта.Size = new System.Drawing.Size(285, 27);
            this.documentControl_ЗамовленняКлієнта.TabIndex = 0;
            // 
            // button_ok
            // 
            this.button_ok.Location = new System.Drawing.Point(12, 51);
            this.button_ok.Name = "button_ok";
            this.button_ok.Size = new System.Drawing.Size(75, 23);
            this.button_ok.TabIndex = 1;
            this.button_ok.Text = "OK";
            this.button_ok.UseVisualStyleBackColor = true;
            this.button_ok.Click += new System.EventHandler(this.button_ok_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.splitContainer1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResizeBegin += new System.EventHandler(this.Form1_ResizeBegin);
            this.ResizeEnd += new System.EventHandler(this.Form1_ResizeEnd);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel table_body;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TableLayoutPanel table_head;
        private System.Windows.Forms.TableLayoutPanel table_footer;
        private DocumentControl documentControl_ЗамовленняКлієнта;
        private System.Windows.Forms.Button button_ok;
    }
}