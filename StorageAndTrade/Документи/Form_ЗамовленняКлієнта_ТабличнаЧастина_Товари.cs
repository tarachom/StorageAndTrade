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
    public partial class Form_ЗамовленняКлієнта_ТабличнаЧастина_Товари : UserControl
    {
        public Form_ЗамовленняКлієнта_ТабличнаЧастина_Товари()
        {
            InitializeComponent();
        }

		/// <summary>
		/// Власне документ якому належить таблична частина
		/// </summary>
		public Документи.ЗамовленняКлієнта_Objest ДокументОбєкт { get; set; }

        private void ЗамовленняКлієнта_ТабличнаЧастина_Товари_Load(object sender, EventArgs e)
        {
			RecordsBindingList = new BindingList<Записи>();
			dataGridViewRecords.DataSource = RecordsBindingList;

			dataGridViewRecords.Columns["ID"].Visible = false;

			dataGridViewRecords.Columns["НомерРядка"].Width = 30;
			dataGridViewRecords.Columns["НомерРядка"].ReadOnly = true;
			dataGridViewRecords.Columns["НомерРядка"].HeaderText = "№";

			dataGridViewRecords.Columns["Номенклатура"].Visible = false;
			dataGridViewRecords.Columns["Характеристика"].Visible = false;
			dataGridViewRecords.Columns["Пакування"].Visible = false;

			dataGridViewRecords.Columns["НоменклатураНазва"].Width = 200;
			dataGridViewRecords.Columns["НоменклатураНазва"].ReadOnly = true;
			dataGridViewRecords.Columns["НоменклатураНазва"].HeaderText = "Номенклатура";

			dataGridViewRecords.Columns["ХарактеристикаНазва"].Width = 200;
			dataGridViewRecords.Columns["ХарактеристикаНазва"].ReadOnly = true;
			dataGridViewRecords.Columns["ХарактеристикаНазва"].HeaderText = "Характеристика";

			dataGridViewRecords.Columns["КількістьУпаковок"].Width = 50;
			dataGridViewRecords.Columns["КількістьУпаковок"].HeaderText = "Кво.Упак.";

			dataGridViewRecords.Columns["ПакуванняНазва"].Width = 100;
			dataGridViewRecords.Columns["ПакуванняНазва"].ReadOnly = true;
			dataGridViewRecords.Columns["ПакуванняНазва"].HeaderText = "Пакування";

			dataGridViewRecords.Columns["Ціна"].CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
			dataGridViewRecords.Columns["Сума"].CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
		}

		private BindingList<Записи> RecordsBindingList { get; set; }

		public void LoadRecords()
		{
			int selectRow = dataGridViewRecords.SelectedRows.Count > 0 ?
				dataGridViewRecords.SelectedRows[dataGridViewRecords.SelectedRows.Count - 1].Index : 0;

			RecordsBindingList.Clear();

			Query querySelect = ДокументОбєкт.Товари_TablePart.QuerySelect;

			//JOIN 1
			string JoinTable = Конфа.Config.Kernel.Conf.Directories["Номенклатура"].Table;
			string ParentField = JoinTable + "." + Довідники.Номенклатура_Select.Назва;

			querySelect.FieldAndAlias.Add(new KeyValuePair<string, string>(ParentField, "tovar_name"));
			querySelect.Joins.Add(new Join(JoinTable, Документи.ЗамовленняКлієнта_Товари_TablePart.Номенклатура, querySelect.Table));

			//JOIN 2
			JoinTable = Конфа.Config.Kernel.Conf.Directories["ПакуванняОдиниціВиміру"].Table;
			ParentField = JoinTable + "." + Довідники.ПакуванняОдиниціВиміру_Select.Назва;

			querySelect.FieldAndAlias.Add(new KeyValuePair<string, string>(ParentField, "pak_name"));
			querySelect.Joins.Add(new Join(JoinTable, Документи.ЗамовленняКлієнта_Товари_TablePart.Пакування, querySelect.Table));

			//JOIN 3
			JoinTable = Конфа.Config.Kernel.Conf.Directories["ХарактеристикиНоменклатури"].Table;
			ParentField = JoinTable + "." + Довідники.ХарактеристикиНоменклатури_Select.Назва;

			querySelect.FieldAndAlias.Add(new KeyValuePair<string, string>(ParentField, "xar_name"));
			querySelect.Joins.Add(new Join(JoinTable, Документи.ЗамовленняКлієнта_Товари_TablePart.ХарактеристикаНоменклатури, querySelect.Table));

			//ORDER
			querySelect.Order.Add(Документи.ЗамовленняКлієнта_Товари_TablePart.НомерРядка, SelectOrder.ASC);

			ДокументОбєкт.Товари_TablePart.Read();

			Dictionary<string, Dictionary<string, string>> JoinValue = ДокументОбєкт.Товари_TablePart.JoinValue;

			foreach (Документи.ЗамовленняКлієнта_Товари_TablePart.Record record in ДокументОбєкт.Товари_TablePart.Records)
			{
				RecordsBindingList.Add(new Записи
				{
					ID = record.UID.ToString(),
					НомерРядка = record.НомерРядка,
					Номенклатура = record.Номенклатура,
					НоменклатураНазва = JoinValue[record.UID.ToString()]["tovar_name"],
					Характеристика = record.ХарактеристикаНоменклатури,
					ХарактеристикаНазва = JoinValue[record.UID.ToString()]["xar_name"],
					КількістьУпаковок = record.КількістьУпаковок,
					Пакування = record.Пакування,
					ПакуванняНазва = JoinValue[record.UID.ToString()]["pak_name"],
					Кількість = (uint)record.Кількість,
					Ціна = Math.Round(record.Ціна, 2),
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
			ДокументОбєкт.Товари_TablePart.Records.Clear();

			int sequenceNumber = 0;
			decimal documentSuma = 0;

			foreach (Записи запис in RecordsBindingList)
            {
				sequenceNumber++;

				Документи.ЗамовленняКлієнта_Товари_TablePart.Record record = new Документи.ЗамовленняКлієнта_Товари_TablePart.Record();

				if (!String.IsNullOrEmpty(запис.ID))
					record.UID = Guid.Parse(запис.ID);

				record.НомерРядка = sequenceNumber;
				record.Номенклатура = запис.Номенклатура;
				record.ХарактеристикаНоменклатури = запис.Характеристика;
				record.КількістьУпаковок = запис.КількістьУпаковок;
				record.Пакування = запис.Пакування;
				record.Кількість = (int)запис.Кількість;
				record.Ціна = запис.Ціна;
				record.Сума = запис.Сума;

				documentSuma += запис.Сума;

				ДокументОбєкт.Товари_TablePart.Records.Add(record);
			}

			ДокументОбєкт.СумаДокументу = Math.Round(documentSuma, 2);

			ДокументОбєкт.Товари_TablePart.Save(true);
		}

		private class Записи
        {
			public string ID { get; set; }
			public int НомерРядка { get; set; }
			public Довідники.Номенклатура_Pointer Номенклатура { get; set; }
            public string НоменклатураНазва { get; set; }
			public Довідники.ХарактеристикиНоменклатури_Pointer Характеристика { get; set; }
			public string ХарактеристикаНазва { get; set; }
			public int КількістьУпаковок { get; set; }
			public Довідники.ПакуванняОдиниціВиміру_Pointer Пакування { get; set; }
			public string ПакуванняНазва { get; set; }
			public uint Кількість { get; set; }
			public decimal Ціна { get; set; }
			public decimal Сума { get; set; }

			public static Записи New()
            {
				return new Записи
				{
					ID = Guid.Empty.ToString(),
					Номенклатура = new Довідники.Номенклатура_Pointer(),
					Характеристика = new Довідники.ХарактеристикиНоменклатури_Pointer(),
					КількістьУпаковок = 1,
					Пакування = new Довідники.ПакуванняОдиниціВиміру_Pointer(),
					Кількість = 1
				};
			}

			public static Записи Clone(Записи запис)
            {
				return new Записи
				{
					ID = Guid.Empty.ToString(),
					Номенклатура = запис.Номенклатура,
					НоменклатураНазва = запис.НоменклатураНазва,
					Пакування = запис.Пакування,
					ПакуванняНазва = запис.ПакуванняНазва,
					Кількість = запис.Кількість,
					Ціна = запис.Ціна,
					Сума = запис.Сума
				};
            }
        }

		private void CopyMenuItem_ClickFind(object sender, EventArgs e)
		{
			Console.WriteLine("Find menu");
		}

		private void ToolStripTextBox_TextChanged(object sender, EventArgs e)
		{
			ToolStripTextBox findMenuItem = (ToolStripTextBox)sender;
			Console.WriteLine(findMenuItem.Text);

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

			string[] allowColumn = new string[] { "НоменклатураНазва", "ХарактеристикаНазва", "ПакуванняНазва" };

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

			Console.WriteLine(copyMenuItem.Name);

			switch (copyMenuItem.Name)
			{
				case "НоменклатураНазва":
					{
						DirectoryControl directoryControl = new DirectoryControl();
						directoryControl.EnablePresentation = false;

						Form_Номенклатура form_Номенклатура = new Form_Номенклатура();
						form_Номенклатура.DirectoryPointerItem = запис.Номенклатура;
						form_Номенклатура.DirectoryControlItem = directoryControl;
						form_Номенклатура.ShowDialog();

						if (directoryControl.DirectoryPointerItem != null)
						{
							запис.Номенклатура = (Довідники.Номенклатура_Pointer)directoryControl.DirectoryPointerItem;

							Довідники.Номенклатура_Objest номенклатура_Objest = запис.Номенклатура.GetDirectoryObject();
							запис.НоменклатураНазва = номенклатура_Objest.Назва;
							запис.Пакування = номенклатура_Objest.ОдиницяВиміру;

							Довідники.ПакуванняОдиниціВиміру_Objest пакуванняОдиниціВиміру_Objest = запис.Пакування.GetDirectoryObject();
							if (пакуванняОдиниціВиміру_Objest != null)
                            {
								запис.ПакуванняНазва = пакуванняОдиниціВиміру_Objest.Назва;
								запис.КількістьУпаковок = пакуванняОдиниціВиміру_Objest.КількістьУпаковок;
							}

							dataGridViewRecords.Refresh();
						}

						break;
					}
				case "ХарактеристикаНазва":
					{
						DirectoryControl directoryControl = new DirectoryControl();
						directoryControl.EnablePresentation = false;

						Form_ХарактеристикиНоменклатури form_ХарактеристикиНоменклатури = new Form_ХарактеристикиНоменклатури();
						form_ХарактеристикиНоменклатури.DirectoryPointerItem = запис.Характеристика;
						form_ХарактеристикиНоменклатури.DirectoryControlItem = directoryControl;
						form_ХарактеристикиНоменклатури.ShowDialog();

						if (directoryControl.DirectoryPointerItem != null)
						{
							запис.Характеристика = (Довідники.ХарактеристикиНоменклатури_Pointer)directoryControl.DirectoryPointerItem;
							запис.ХарактеристикаНазва = запис.Характеристика.GetPresentation();
						}

						break;
					}
				case "ПакуванняНазва":
					{
						DirectoryControl directoryControl = new DirectoryControl();
						directoryControl.EnablePresentation = false;

						Form_ПакуванняОдиниціВиміру form_ПакуванняОдиниціВиміру = new Form_ПакуванняОдиниціВиміру();
						form_ПакуванняОдиниціВиміру.DirectoryPointerItem = запис.Пакування;
						form_ПакуванняОдиниціВиміру.DirectoryControlItem = directoryControl;
						form_ПакуванняОдиниціВиміру.ShowDialog();

						if (directoryControl.DirectoryPointerItem != null)
						{
							запис.Пакування = (Довідники.ПакуванняОдиниціВиміру_Pointer)directoryControl.DirectoryPointerItem;
							запис.ПакуванняНазва = запис.Пакування.GetPresentation();
						}

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
			string columnName = dataGridViewRecords.Columns[e.ColumnIndex].Name;

			Записи запис = RecordsBindingList[e.RowIndex];

			if (columnName == "Кількість" || columnName == "Ціна")
            {
				запис.Сума = запис.Кількість * запис.Ціна;
				dataGridViewRecords.Refresh();
			}

			
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
