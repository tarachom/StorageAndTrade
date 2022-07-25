using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using AccountingSoftware;
using Конфа = StorageAndTrade_1_0;
using Константи = StorageAndTrade_1_0.Константи;
using Довідники = StorageAndTrade_1_0.Довідники;
using Документи = StorageAndTrade_1_0.Документи;
using Перелічення = StorageAndTrade_1_0.Перелічення;

namespace StorageAndTrade
{
    public partial class Form_ВведенняЗалишків_ТабличнаЧастина_РозрахункиЗКонтрагентами : UserControl
    {
        public Form_ВведенняЗалишків_ТабличнаЧастина_РозрахункиЗКонтрагентами()
        {
            InitializeComponent();

			RecordsBindingList = new BindingList<Записи>();
			dataGridViewRecords.DataSource = RecordsBindingList;
		}

		/// <summary>
		/// Власне документ якому належить таблична частина
		/// </summary>
		public Документи.ВведенняЗалишків_Objest ДокументОбєкт { get; set; }

        private void Form_ВведенняЗалишків_ТабличнаЧастина_РозрахункиЗКонтрагентами_Load(object sender, EventArgs e)
        {
			dataGridViewRecords.Columns["ID"].Visible = false;

			dataGridViewRecords.Columns["НомерРядка"].Width = 30;
			dataGridViewRecords.Columns["НомерРядка"].ReadOnly = true;
			dataGridViewRecords.Columns["НомерРядка"].HeaderText = "№";

			dataGridViewRecords.Columns["Контрагент"].Visible = false;
			dataGridViewRecords.Columns["КонтрагентНазва"].Width = 300;
			dataGridViewRecords.Columns["КонтрагентНазва"].ReadOnly = true;
			dataGridViewRecords.Columns["КонтрагентНазва"].HeaderText = "Контрагент";

			dataGridViewRecords.Columns["Валюта"].Visible = false;
			dataGridViewRecords.Columns["ВалютаНазва"].Width = 200;
			dataGridViewRecords.Columns["ВалютаНазва"].ReadOnly = true;
			dataGridViewRecords.Columns["ВалютаНазва"].HeaderText = "Валюта";

			dataGridViewRecords.Columns["Сума"].CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
		}

		private BindingList<Записи> RecordsBindingList { get; set; }

		public void LoadRecords()
		{
			int selectRow = dataGridViewRecords.SelectedRows.Count > 0 ?
				dataGridViewRecords.SelectedRows[dataGridViewRecords.SelectedRows.Count - 1].Index : 0;

			RecordsBindingList.Clear();

			Query querySelect = ДокументОбєкт.РозрахункиЗКонтрагентами_TablePart.QuerySelect;

			//JOIN 1
			querySelect.FieldAndAlias.Add(
				new NameValue<string>(Довідники.Контрагенти_Const.TABLE + "." + Довідники.Контрагенти_Const.Назва, "kontragent"));
			querySelect.Joins.Add(
				new Join(Довідники.Контрагенти_Const.TABLE, Документи.ВведенняЗалишків_РозрахункиЗКонтрагентами_TablePart.Контрагент, querySelect.Table));

			//JOIN 2
			querySelect.FieldAndAlias.Add(
				new NameValue<string>(Довідники.Валюти_Const.TABLE + "." + Довідники.Валюти_Const.Назва, "valuta"));
			querySelect.Joins.Add(
				new Join(Довідники.Валюти_Const.TABLE, Документи.ВведенняЗалишків_РозрахункиЗКонтрагентами_TablePart.Валюта, querySelect.Table));

			//ORDER
			querySelect.Order.Add(Документи.ВведенняЗалишків_РозрахункиЗКонтрагентами_TablePart.НомерРядка, SelectOrder.ASC);

			ДокументОбєкт.РозрахункиЗКонтрагентами_TablePart.Read();

			Dictionary<string, Dictionary<string, string>> JoinValue = ДокументОбєкт.РозрахункиЗКонтрагентами_TablePart.JoinValue;

			foreach (Документи.ВведенняЗалишків_РозрахункиЗКонтрагентами_TablePart.Record record in ДокументОбєкт.РозрахункиЗКонтрагентами_TablePart.Records)
			{
				RecordsBindingList.Add(new Записи
				{
					ID = record.UID.ToString(),
					НомерРядка = record.НомерРядка,
					Контрагент = record.Контрагент,
					КонтрагентНазва = JoinValue[record.UID.ToString()]["kontragent"],
					Валюта = record.Валюта,
					ВалютаНазва = JoinValue[record.UID.ToString()]["valuta"],
					Сума = Math.Round(record.Сума, 2)
				});
			}

			if (selectRow != 0 && selectRow < dataGridViewRecords.Rows.Count)
			{
				dataGridViewRecords.Rows[0].Selected = false;
				dataGridViewRecords.Rows[selectRow].Selected = true;
				dataGridViewRecords.FirstDisplayedScrollingRowIndex = selectRow;
			}
		}

		public void SaveRecords()
        {
			ДокументОбєкт.РозрахункиЗКонтрагентами_TablePart.Records.Clear();

			int sequenceNumber = 0;

			foreach (Записи запис in RecordsBindingList)
            {
				sequenceNumber++;

				Документи.ВведенняЗалишків_РозрахункиЗКонтрагентами_TablePart.Record record = new Документи.ВведенняЗалишків_РозрахункиЗКонтрагентами_TablePart.Record();

				record.UID = Guid.Parse(запис.ID);
				record.НомерРядка = sequenceNumber;
				record.Контрагент = запис.Контрагент;
				record.Валюта = запис.Валюта;
				record.Сума = запис.Сума;

				ДокументОбєкт.РозрахункиЗКонтрагентами_TablePart.Records.Add(record);
			}

			ДокументОбєкт.РозрахункиЗКонтрагентами_TablePart.Save(true);
		}

		private class Записи
        {
			public string ID { get; set; }
			public int НомерРядка { get; set; }
			public Довідники.Контрагенти_Pointer Контрагент { get; set; }
            public string КонтрагентНазва { get; set; }
			public Довідники.Валюти_Pointer Валюта { get; set; }
			public string ВалютаНазва { get; set; }
			public decimal Сума { get; set; }
			public static Записи New()
            {
				return new Записи
				{
					ID = Guid.Empty.ToString(),
					Контрагент = new Довідники.Контрагенти_Pointer(),
					Валюта = new Довідники.Валюти_Pointer()
				};
			}

			public static Записи Clone(Записи запис)
            {
				return new Записи
				{
					ID = Guid.Empty.ToString(),
					Контрагент = запис.Контрагент,
					КонтрагентНазва = запис.КонтрагентНазва,
					Валюта = запис.Валюта,
					ВалютаНазва = запис.ВалютаНазва,
					Сума = запис.Сума
				};
            }
        }

		private void CopyMenuItem_ClickFind(object sender, EventArgs e)
		{
			//Console.WriteLine("Find menu");
		}

		private void ToolStripTextBox_TextChanged(object sender, EventArgs e)
		{
			ToolStripTextBox findMenuItem = (ToolStripTextBox)sender;
			//Console.WriteLine(findMenuItem.Text);

			foreach(ToolStripItem toolStripItem in contextMenuStrip1.Items.Find("find", false))
				contextMenuStrip1.Items.Remove(toolStripItem);

			ToolStripItem[] mas = new ToolStripItem[10];

			for (int i = 0; i < 10; i++)
			{
				mas[i] = new ToolStripMenuItem("Варіанти: " + findMenuItem.Text, Properties.Resources.page_white_text, CopyMenuItem_ClickFind, "find");
			}

			contextMenuStrip1.Items.AddRange(mas);
		}

        #region Контекстне меню вибору із списку

        private void dataGridViewRecords_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
			string columnName = dataGridViewRecords.Columns[e.ColumnIndex].Name;

			string[] allowColumn = new string[] { "КонтрагентНазва", "ВалютаНазва" };

			contextMenuStrip1.Items.Clear();

			if (allowColumn.Contains(columnName))
            {
				ToolStripMenuItem selectMenuItem = new ToolStripMenuItem("Вибрати із списку");
				selectMenuItem.Image = Properties.Resources.data;
				selectMenuItem.Name = columnName;
				selectMenuItem.Tag = RecordsBindingList[e.RowIndex];
				selectMenuItem.Click += SelectMenuItem_Click;
				contextMenuStrip1.Items.Add(selectMenuItem);

				ToolStripTextBox findToolStripTextBox = new ToolStripTextBox();
				findToolStripTextBox.ToolTipText = "Пошук";
				findToolStripTextBox.Size = new Size(300, 0);
				findToolStripTextBox.TextChanged += ToolStripTextBox_TextChanged;
				contextMenuStrip1.Items.Add(findToolStripTextBox);

				Rectangle rectangle = dataGridViewRecords.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);
				rectangle.Offset(0, dataGridViewRecords.Rows[e.RowIndex].Height);
				Point point = dataGridViewRecords.PointToScreen(rectangle.Location);

				contextMenuStrip1.Show(point);
			}
		}

		private void SelectMenuItem_Click(object sender, EventArgs e)
		{
			ToolStripMenuItem copyMenuItem = (ToolStripMenuItem)sender;
			Записи запис = (Записи)copyMenuItem.Tag;

			//Console.WriteLine(copyMenuItem.Name);

			switch (copyMenuItem.Name)
			{
				case "КонтрагентНазва":
					{
						Form_Контрагенти form_Контрагенти = new Form_Контрагенти();
						form_Контрагенти.DirectoryPointerItem = запис.Контрагент;
						form_Контрагенти.ShowDialog();

						запис.Контрагент = (Довідники.Контрагенти_Pointer)form_Контрагенти.DirectoryPointerItem;

						Довідники.Контрагенти_Objest контрагенти_Objest = запис.Контрагент.GetDirectoryObject();
						if (контрагенти_Objest != null)
							запис.КонтрагентНазва = контрагенти_Objest.Назва;
						else
							запис.КонтрагентНазва = "";

						dataGridViewRecords.Refresh();

						break;
					}
				case "ВалютаНазва":
					{
						Form_Валюти form_Валюти = new Form_Валюти();
						form_Валюти.DirectoryPointerItem = запис.Валюта;
						form_Валюти.ShowDialog();

						запис.Валюта = (Довідники.Валюти_Pointer)form_Валюти.DirectoryPointerItem;

						Довідники.Валюти_Objest валюти_Objest = запис.Валюта.GetDirectoryObject();
						if (валюти_Objest != null)
							запис.ВалютаНазва = валюти_Objest.Назва;
						else
							запис.ВалютаНазва = "";

						dataGridViewRecords.Refresh();

						break;
					}
				default:
					break;
			}
		}

        #endregion

        private void dataGridViewRecords_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
			
		}

        private void dataGridViewRecords_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {

		}

        private void dataGridViewRecords_CellClick(object sender, DataGridViewCellEventArgs e)
        {
			
		}

		private void toolStripButtonAdd_Click(object sender, EventArgs e)
		{
			RecordsBindingList.Add(Записи.New());
		}

        private void toolStripButtonDelete_Click(object sender, EventArgs e)
        {
			if (dataGridViewRecords.SelectedCells.Count > 0)
			{
				List<int> deleteRowIndex = new List<int>();

				for (int i = 0; i < dataGridViewRecords.SelectedCells.Count; i++)
					if (!deleteRowIndex.Contains(dataGridViewRecords.SelectedCells[i].RowIndex) &&
						!dataGridViewRecords.Rows[dataGridViewRecords.SelectedCells[i].RowIndex].IsNewRow)
						deleteRowIndex.Add(dataGridViewRecords.SelectedCells[i].RowIndex);

				deleteRowIndex.Sort();

				foreach (int rowIndex in deleteRowIndex.Reverse<int>())
					RecordsBindingList.RemoveAt(rowIndex);
			}
        }

		private void toolStripButtonCopy_Click(object sender, EventArgs e)
		{
			if (dataGridViewRecords.SelectedCells.Count > 0)
			{
				List<int> rowIndexList = new List<int>();

				for (int i = 0; i < dataGridViewRecords.SelectedCells.Count; i++)
					if (!rowIndexList.Contains(dataGridViewRecords.SelectedCells[i].RowIndex) &&
						!dataGridViewRecords.Rows[dataGridViewRecords.SelectedCells[i].RowIndex].IsNewRow)
						rowIndexList.Add(dataGridViewRecords.SelectedCells[i].RowIndex);

				rowIndexList.Sort();

				foreach (int rowIndex in rowIndexList)
					RecordsBindingList.Add(Записи.Clone(RecordsBindingList[rowIndex]));
			}
		}
    }
}
