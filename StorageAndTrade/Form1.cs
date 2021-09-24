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
				TextAlign = ContentAlignment.MiddleLeft,
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

		private int PositionTopNextBlock { get; set; }

		TableLayoutPanel CreateTableLayoutPanel()
        {
			TableLayoutPanel tableLayoutPanel = new TableLayoutPanel();
			splitContainer1.Panel2.Controls.Add(tableLayoutPanel);
			tableLayoutPanel.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;
			tableLayoutPanel.Width = splitContainer1.Panel2.Width;
			tableLayoutPanel.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
			tableLayoutPanel.AutoSize = true;

			return tableLayoutPanel;
		}

		void CreateBodyBlock_Reg1()
        {
			TableLayoutPanel tableLayoutPanel = CreateTableLayoutPanel();
			tableLayoutPanel.Height = 20;

			tableLayoutPanel.ColumnCount = 3;
			tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 5));
			tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 85));
			tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10));

			//
			//ЗамовленняКлієнтів
			//

			РегістриНакопичення.ЗамовленняКлієнтів_RecordsSet замовленняКлієнтів_RecordsSet = new РегістриНакопичення.ЗамовленняКлієнтів_RecordsSet();
			замовленняКлієнтів_RecordsSet.QuerySelect.Where.Add(new Where("owner", Comparison.EQ, documentControl_ЗамовленняКлієнта.DocumentPointerItem.UnigueID.UGuid));

			//JOIN 1
			string JoinTable = Конфа.Config.Kernel.Conf.Directories["Номенклатура"].Table;
			string ParentField = JoinTable + "." + Конфа.Config.Kernel.Conf.Directories["Номенклатура"].Fields["Назва"].NameInTable;

			замовленняКлієнтів_RecordsSet.QuerySelect.FieldAndAlias.Add(new KeyValuePair<string, string>(ParentField, "nomenklatura"));
			замовленняКлієнтів_RecordsSet.QuerySelect.Joins.Add(new Join(JoinTable, РегістриНакопичення.ЗамовленняКлієнтів_RecordsSet.Номенклатура, замовленняКлієнтів_RecordsSet.QuerySelect.Table));

			замовленняКлієнтів_RecordsSet.Read();

			int line_number = 0;

			foreach (РегістриНакопичення.ЗамовленняКлієнтів_RecordsSet.Record record in замовленняКлієнтів_RecordsSet.Records)
			{
				tableLayoutPanel.Controls.Add(CreateBodyCell(record.Income == true ? "+" : "-", ContentAlignment.MiddleCenter), 0, line_number);
				tableLayoutPanel.Controls.Add(CreateBodyCell(замовленняКлієнтів_RecordsSet.JoinValue[record.UID.ToString()]["nomenklatura"]), 1, line_number);
				tableLayoutPanel.Controls.Add(CreateBodyCell(record.Сума.ToString(), ContentAlignment.MiddleRight), 2, line_number);

				line_number++;
			}

			tableLayoutPanel.Top = PositionTopNextBlock;
			PositionTopNextBlock += tableLayoutPanel.Height - 1;
		}

		void CreateBodyBlock_Reg2()
		{
			TableLayoutPanel tableLayoutPanel = CreateTableLayoutPanel();
			tableLayoutPanel.Height = 20;

			tableLayoutPanel.ColumnCount = 5;
			tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 5));
			tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 65));
			tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10));
			tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10));
			tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10));

			//
			//ВільніЗалишки
			//

			РегістриНакопичення.ВільніЗалишки_RecordsSet вільніЗалишки_RecordsSet = new РегістриНакопичення.ВільніЗалишки_RecordsSet();
			вільніЗалишки_RecordsSet.QuerySelect.Where.Add(new Where("owner", Comparison.EQ, documentControl_ЗамовленняКлієнта.DocumentPointerItem.UnigueID.UGuid));

			//JOIN 1
			string JoinTable = Конфа.Config.Kernel.Conf.Directories["Номенклатура"].Table;
			string ParentField = JoinTable + "." + Конфа.Config.Kernel.Conf.Directories["Номенклатура"].Fields["Назва"].NameInTable;

			вільніЗалишки_RecordsSet.QuerySelect.FieldAndAlias.Add(new KeyValuePair<string, string>(ParentField, "nomenklatura"));
			вільніЗалишки_RecordsSet.QuerySelect.Joins.Add(new Join(JoinTable, РегістриНакопичення.ВільніЗалишки_RecordsSet.Номенклатура, вільніЗалишки_RecordsSet.QuerySelect.Table));

			вільніЗалишки_RecordsSet.Read();

			int line_number = 0;

			foreach (РегістриНакопичення.ВільніЗалишки_RecordsSet.Record record in вільніЗалишки_RecordsSet.Records)
			{
				tableLayoutPanel.Controls.Add(CreateBodyCell(record.Income == true ? "+" : "-", ContentAlignment.MiddleCenter), 0, line_number);
				tableLayoutPanel.Controls.Add(CreateBodyCell(вільніЗалишки_RecordsSet.JoinValue[record.UID.ToString()]["nomenklatura"]), 1, line_number);
				tableLayoutPanel.Controls.Add(CreateBodyCell(record.ВНаявності.ToString(), ContentAlignment.MiddleRight), 2, line_number);
				tableLayoutPanel.Controls.Add(CreateBodyCell(record.ВРезервіЗіСкладу.ToString(), ContentAlignment.MiddleRight), 3, line_number);
				tableLayoutPanel.Controls.Add(CreateBodyCell(record.ВРезервіПідЗамовлення.ToString(), ContentAlignment.MiddleRight), 4, line_number);

				line_number++;
			}

			tableLayoutPanel.Top = PositionTopNextBlock;
			PositionTopNextBlock += tableLayoutPanel.Height - 1;
		}

		void CreateBodyBlock_Reg3()
		{
			TableLayoutPanel tableLayoutPanel = CreateTableLayoutPanel();
			tableLayoutPanel.Height = 20;

			tableLayoutPanel.ColumnCount = 3;
			tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 5));
			tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 85));
			tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10));

			//
			//РозрахункиЗКлієнтами
			//

			РегістриНакопичення.РозрахункиЗКлієнтами_RecordsSet розрахункиЗКлієнтами_RecordsSet = new РегістриНакопичення.РозрахункиЗКлієнтами_RecordsSet();
			розрахункиЗКлієнтами_RecordsSet.QuerySelect.Where.Add(new Where("owner", Comparison.EQ, documentControl_ЗамовленняКлієнта.DocumentPointerItem.UnigueID.UGuid));

			//JOIN 1
			string JoinTable = Конфа.Config.Kernel.Conf.Directories["Валюти"].Table;
			string ParentField = JoinTable + "." + Конфа.Config.Kernel.Conf.Directories["Валюти"].Fields["Назва"].NameInTable;

			розрахункиЗКлієнтами_RecordsSet.QuerySelect.FieldAndAlias.Add(new KeyValuePair<string, string>(ParentField, "valuta"));
			розрахункиЗКлієнтами_RecordsSet.QuerySelect.Joins.Add(new Join(JoinTable, РегістриНакопичення.РозрахункиЗКлієнтами_RecordsSet.Валюта, розрахункиЗКлієнтами_RecordsSet.QuerySelect.Table));

			розрахункиЗКлієнтами_RecordsSet.Read();

			int line_number = 0;

			foreach (РегістриНакопичення.РозрахункиЗКлієнтами_RecordsSet.Record record in розрахункиЗКлієнтами_RecordsSet.Records)
			{
				tableLayoutPanel.Controls.Add(CreateBodyCell(record.Income == true ? "+" : "-", ContentAlignment.MiddleCenter), 0, line_number);
				tableLayoutPanel.Controls.Add(CreateBodyCell(розрахункиЗКлієнтами_RecordsSet.JoinValue[record.UID.ToString()]["valuta"]), 1, line_number);
				tableLayoutPanel.Controls.Add(CreateBodyCell(record.Сума.ToString(), ContentAlignment.MiddleRight), 2, line_number);

				line_number++;
			}

			tableLayoutPanel.Top = PositionTopNextBlock;
			PositionTopNextBlock += tableLayoutPanel.Height - 1;
		}

		void CreateHeadBlock(string Name)
		{
			TableLayoutPanel tableLayoutPanel = CreateTableLayoutPanel();
			tableLayoutPanel.Height = 37;

			tableLayoutPanel.ColumnCount = 1;
			tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent));

			tableLayoutPanel.Controls.Add(CreateHeadCell(Name));
			tableLayoutPanel.Top = PositionTopNextBlock;

			PositionTopNextBlock += tableLayoutPanel.Height - 1;
		}

		private void button_ok_Click(object sender, EventArgs e)
        {
			//CreateReport();
			SendMessage(this.Handle, WM_SETREDRAW, false, 0);

			splitContainer1.Panel2.Controls.Clear();
			PositionTopNextBlock = 0;

			CreateHeadBlock("Замовлення клієнтів");
			CreateBodyBlock_Reg1();

			CreateHeadBlock("Вільні залишки");
			CreateBodyBlock_Reg2();

			CreateHeadBlock("Розрахунки з клієнтами");
			CreateBodyBlock_Reg3();

			SendMessage(this.Handle, WM_SETREDRAW, true, 0);
			this.Refresh();
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
