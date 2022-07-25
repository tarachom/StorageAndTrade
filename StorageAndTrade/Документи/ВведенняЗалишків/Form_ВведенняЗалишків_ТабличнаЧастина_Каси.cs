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
    public partial class Form_ВведенняЗалишків_ТабличнаЧастина_Каси : UserControl
    {
        public Form_ВведенняЗалишків_ТабличнаЧастина_Каси()
        {
            InitializeComponent();

			RecordsBindingList = new BindingList<Записи>();
			dataGridViewRecords.DataSource = RecordsBindingList;
		}

		/// <summary>
		/// Власне документ якому належить таблична частина
		/// </summary>
		public Документи.ВведенняЗалишків_Objest ДокументОбєкт { get; set; }

        private void Form_ВведенняЗалишків_ТабличнаЧастина_Каси_Load(object sender, EventArgs e)
        {
			dataGridViewRecords.Columns["ID"].Visible = false;

			dataGridViewRecords.Columns["НомерРядка"].Width = 30;
			dataGridViewRecords.Columns["НомерРядка"].ReadOnly = true;
			dataGridViewRecords.Columns["НомерРядка"].HeaderText = "№";

			dataGridViewRecords.Columns["Каса"].Visible = false;
			dataGridViewRecords.Columns["КасаНазва"].Width = 300;
			dataGridViewRecords.Columns["КасаНазва"].ReadOnly = true;
			dataGridViewRecords.Columns["КасаНазва"].HeaderText = "Каса";

			dataGridViewRecords.Columns["Сума"].CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
		}

		private BindingList<Записи> RecordsBindingList { get; set; }

		public void LoadRecords()
		{
			int selectRow = dataGridViewRecords.SelectedRows.Count > 0 ?
				dataGridViewRecords.SelectedRows[dataGridViewRecords.SelectedRows.Count - 1].Index : 0;

			RecordsBindingList.Clear();

			Query querySelect = ДокументОбєкт.Каси_TablePart.QuerySelect;

			//JOIN 1
			querySelect.FieldAndAlias.Add(
				new NameValue<string>(Довідники.Каси_Const.TABLE + "." + Довідники.Каси_Const.Назва, "kasa"));
			querySelect.Joins.Add(
				new Join(Довідники.Каси_Const.TABLE, Документи.ВведенняЗалишків_Каси_TablePart.Каса, querySelect.Table));

			//ORDER
			querySelect.Order.Add(Документи.ВведенняЗалишків_Каси_TablePart.НомерРядка, SelectOrder.ASC);

			ДокументОбєкт.Каси_TablePart.Read();

			Dictionary<string, Dictionary<string, string>> JoinValue = ДокументОбєкт.Каси_TablePart.JoinValue;

			foreach (Документи.ВведенняЗалишків_Каси_TablePart.Record record in ДокументОбєкт.Каси_TablePart.Records)
			{
				RecordsBindingList.Add(new Записи
				{
					ID = record.UID.ToString(),
					НомерРядка = record.НомерРядка,
					Каса = record.Каса,
					КасаНазва = JoinValue[record.UID.ToString()]["kasa"],
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
			ДокументОбєкт.Каси_TablePart.Records.Clear();

			int sequenceNumber = 0;

			foreach (Записи запис in RecordsBindingList)
            {
				sequenceNumber++;

				Документи.ВведенняЗалишків_Каси_TablePart.Record record = new Документи.ВведенняЗалишків_Каси_TablePart.Record();

				record.UID = Guid.Parse(запис.ID);
				record.НомерРядка = sequenceNumber;
				record.Каса = запис.Каса;
				record.Сума = запис.Сума;

				ДокументОбєкт.Каси_TablePart.Records.Add(record);
			}

			ДокументОбєкт.Каси_TablePart.Save(true);
		}

		private class Записи
        {
			public string ID { get; set; }
			public int НомерРядка { get; set; }
			public Довідники.Каси_Pointer Каса { get; set; }
            public string КасаНазва { get; set; }
			public decimal Сума { get; set; }
			public static Записи New()
            {
				return new Записи
				{
					ID = Guid.Empty.ToString(),
					Каса = new Довідники.Каси_Pointer()
				};
			}

			public static Записи Clone(Записи запис)
            {
				return new Записи
				{
					ID = Guid.Empty.ToString(),
					Каса = запис.Каса,
					КасаНазва = запис.КасаНазва,
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

			string[] allowColumn = new string[] { "КасаНазва" };

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
				case "КасаНазва":
					{
						Form_Каси form_Каси = new Form_Каси();
						form_Каси.DirectoryPointerItem = запис.Каса;
						form_Каси.ShowDialog();

						запис.Каса = (Довідники.Каси_Pointer)form_Каси.DirectoryPointerItem;

						Довідники.Каси_Objest каси_Objest = запис.Каса.GetDirectoryObject();
						if (каси_Objest != null)
							запис.КасаНазва = каси_Objest.Назва;
						else
							запис.КасаНазва = "";

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
