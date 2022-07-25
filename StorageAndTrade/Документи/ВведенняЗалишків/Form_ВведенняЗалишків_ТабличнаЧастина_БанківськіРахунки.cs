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
    public partial class Form_ВведенняЗалишків_ТабличнаЧастина_БанківськіРахунки : UserControl
    {
        public Form_ВведенняЗалишків_ТабличнаЧастина_БанківськіРахунки()
        {
            InitializeComponent();

			RecordsBindingList = new BindingList<Записи>();
			dataGridViewRecords.DataSource = RecordsBindingList;
		}

		/// <summary>
		/// Власне документ якому належить таблична частина
		/// </summary>
		public Документи.ВведенняЗалишків_Objest ДокументОбєкт { get; set; }

        private void Form_ВведенняЗалишків_ТабличнаЧастина_БанківськіРахунки_Load(object sender, EventArgs e)
        {
			dataGridViewRecords.Columns["ID"].Visible = false;

			dataGridViewRecords.Columns["НомерРядка"].Width = 30;
			dataGridViewRecords.Columns["НомерРядка"].ReadOnly = true;
			dataGridViewRecords.Columns["НомерРядка"].HeaderText = "№";

			dataGridViewRecords.Columns["БанківськийРахунок"].Visible = false;
			dataGridViewRecords.Columns["БанківськийРахунокНазва"].Width = 300;
			dataGridViewRecords.Columns["БанківськийРахунокНазва"].ReadOnly = true;
			dataGridViewRecords.Columns["БанківськийРахунокНазва"].HeaderText = "Банківський рахунок";

			dataGridViewRecords.Columns["Сума"].CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
		}

		private BindingList<Записи> RecordsBindingList { get; set; }

		public void LoadRecords()
		{
			int selectRow = dataGridViewRecords.SelectedRows.Count > 0 ?
				dataGridViewRecords.SelectedRows[dataGridViewRecords.SelectedRows.Count - 1].Index : 0;

			RecordsBindingList.Clear();

			Query querySelect = ДокументОбєкт.БанківськіРахунки_TablePart.QuerySelect;

			//JOIN 1
			querySelect.FieldAndAlias.Add(
				new NameValue<string>(Довідники.БанківськіРахункиОрганізацій_Const.TABLE + "." + Довідники.БанківськіРахункиОрганізацій_Const.Назва, "bank_rachunok"));
			querySelect.Joins.Add(
				new Join(Довідники.БанківськіРахункиОрганізацій_Const.TABLE, Документи.ВведенняЗалишків_БанківськіРахунки_TablePart.БанківськийРахунок, querySelect.Table));

			//ORDER
			querySelect.Order.Add(Документи.ВведенняЗалишків_БанківськіРахунки_TablePart.НомерРядка, SelectOrder.ASC);

			ДокументОбєкт.БанківськіРахунки_TablePart.Read();

			Dictionary<string, Dictionary<string, string>> JoinValue = ДокументОбєкт.БанківськіРахунки_TablePart.JoinValue;

			foreach (Документи.ВведенняЗалишків_БанківськіРахунки_TablePart.Record record in ДокументОбєкт.БанківськіРахунки_TablePart.Records)
			{
				RecordsBindingList.Add(new Записи
				{
					ID = record.UID.ToString(),
					НомерРядка = record.НомерРядка,
					БанківськийРахунок = record.БанківськийРахунок,
					БанківськийРахунокНазва = JoinValue[record.UID.ToString()]["bank_rachunok"],
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
			ДокументОбєкт.БанківськіРахунки_TablePart.Records.Clear();

			int sequenceNumber = 0;

			foreach (Записи запис in RecordsBindingList)
            {
				sequenceNumber++;

				Документи.ВведенняЗалишків_БанківськіРахунки_TablePart.Record record = new Документи.ВведенняЗалишків_БанківськіРахунки_TablePart.Record();

				record.UID = Guid.Parse(запис.ID);
				record.НомерРядка = sequenceNumber;
				record.БанківськийРахунок = запис.БанківськийРахунок;
				record.Сума = запис.Сума;

				ДокументОбєкт.БанківськіРахунки_TablePart.Records.Add(record);
			}

			ДокументОбєкт.БанківськіРахунки_TablePart.Save(true);
		}

		private class Записи
        {
			public string ID { get; set; }
			public int НомерРядка { get; set; }
			public Довідники.БанківськіРахункиОрганізацій_Pointer БанківськийРахунок { get; set; }
            public string БанківськийРахунокНазва { get; set; }
			public decimal Сума { get; set; }
			public static Записи New()
            {
				return new Записи
				{
					ID = Guid.Empty.ToString(),
					БанківськийРахунок = new Довідники.БанківськіРахункиОрганізацій_Pointer()
				};
			}

			public static Записи Clone(Записи запис)
            {
				return new Записи
				{
					ID = Guid.Empty.ToString(),
					БанківськийРахунок = запис.БанківськийРахунок,
					БанківськийРахунокНазва = запис.БанківськийРахунокНазва,
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

			string[] allowColumn = new string[] { "БанківськийРахунокНазва" };

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
				case "БанківськийРахунокНазва":
					{
						Form_БанківськіРахункиОрганізацій form_БанківськіРахункиОрганізацій = new Form_БанківськіРахункиОрганізацій();
						form_БанківськіРахункиОрганізацій.DirectoryPointerItem = запис.БанківськийРахунок;
						form_БанківськіРахункиОрганізацій.ShowDialog();

						запис.БанківськийРахунок = (Довідники.БанківськіРахункиОрганізацій_Pointer)form_БанківськіРахункиОрганізацій.DirectoryPointerItem;

						Довідники.БанківськіРахункиОрганізацій_Objest банківськіРахункиОрганізацій_Objest = запис.БанківськийРахунок.GetDirectoryObject();
						if (банківськіРахункиОрганізацій_Objest != null)
							запис.БанківськийРахунокНазва = банківськіРахункиОрганізацій_Objest.Назва;
						else
							запис.БанківськийРахунокНазва = "";

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
