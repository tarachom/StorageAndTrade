using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Runtime.InteropServices;

using AccountingSoftware;
using Конфа = StorageAndTrade_1_0;
using Константи = StorageAndTrade_1_0.Константи;
using Довідники = StorageAndTrade_1_0.Довідники;
using Перелічення = StorageAndTrade_1_0.Перелічення;
using Документи = StorageAndTrade_1_0.Документи;
using РегістриНакопичення = StorageAndTrade_1_0.РегістриНакопичення;

namespace StorageAndTrade
{
    public partial class Form1 : Form
    {
		[DllImport("user32.dll")]
		public static extern int SendMessage(IntPtr hWnd, Int32 wMsg, bool wParam, Int32 lParam);
		private const int WM_SETREDRAW = 11;

		public Form1()
        {
            InitializeComponent();
        }

		Label CreateHeadCell(string text = "")
		{
			Label label = new Label
			{
				Margin = new Padding(0),
				Dock = DockStyle.Fill,
				BackColor = Color.Azure,
				TextAlign = ContentAlignment.MiddleCenter,
				Font = new Font("Arial", 14),
				Text = text
			};

			return label;
		}

		Label CreateBodyCell(string text = "", ContentAlignment contentAlignment = ContentAlignment.MiddleLeft )
		{
			Label label = new Label
			{
				Margin = new Padding(0),
				Dock = DockStyle.Fill,
				BackColor = Color.Azure,
				TextAlign = contentAlignment,
				Font = new Font("Arial", 10),
				Text = text
			};

			return label;
		}

		private void Form1_Load(object sender, EventArgs e)
        {
			documentControl_ЗамовленняКлієнта.SelectForm = new Form_ЗамовленняКлієнтаЖурнал();
			documentControl_ЗамовленняКлієнта.DocumentPointerItem = new Документи.ЗамовленняКлієнта_Pointer();
		}

		void CreateReport()
        {
			SendMessage(this.Handle, WM_SETREDRAW, false, 0);

			table_head.Controls.Clear();
			table_head.Controls.Add(CreateHeadCell("Рух документу"));

			table_body.Controls.Clear();
			table_body.Top = table_head.Location.Y + (table_head.Height - 1);

			table_body.ColumnCount = 3;
			table_body.ColumnStyles[0] = new ColumnStyle(SizeType.Percent, 5);
			table_body.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 85));
			table_body.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10));

			РегістриНакопичення.ЗамовленняКлієнтів_RecordsSet замовленняКлієнтів_RecordsSet = new РегістриНакопичення.ЗамовленняКлієнтів_RecordsSet();
			замовленняКлієнтів_RecordsSet.Filter.ЗамовленняКлієнта = (Документи.ЗамовленняКлієнта_Pointer)documentControl_ЗамовленняКлієнта.DocumentPointerItem;
			замовленняКлієнтів_RecordsSet.Read();

			int line_number = 0;

			foreach(РегістриНакопичення.ЗамовленняКлієнтів_RecordsSet.Record record in замовленняКлієнтів_RecordsSet.Records)
            {
				table_body.Controls.Add(CreateBodyCell(record.Income == true ? "+" : "-", ContentAlignment.MiddleCenter), 0, line_number);
				table_body.Controls.Add(CreateBodyCell(record.Номенклатура.GetPresentation()), 1, line_number);
				table_body.Controls.Add(CreateBodyCell(record.Сума.ToString(), ContentAlignment.MiddleRight), 2, line_number);

				line_number++;
			}

			table_footer.Controls.Clear();
			table_footer.Top = table_body.Location.Y + (table_body.Height - 1);
			table_footer.Controls.Add(CreateHeadCell("ИТОГО"));

			SendMessage(this.Handle, WM_SETREDRAW, true, 0);
			this.Refresh();
		}

        private void button_ok_Click(object sender, EventArgs e)
        {
			Console.WriteLine(documentControl_ЗамовленняКлієнта.DocumentPointerItem.UnigueID.UGuid);

			CreateReport();
		}

        private void Form1_ResizeBegin(object sender, EventArgs e)
        {
			SendMessage(this.Handle, WM_SETREDRAW, false, 0);
		}

        private void Form1_ResizeEnd(object sender, EventArgs e)
        {
			SendMessage(this.Handle, WM_SETREDRAW, true, 0);
			this.Refresh();
		}
    }
}
