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
    public partial class ЗамовленняКлієнта_ТабличнаЧастина_Товари : UserControl
    {
        public ЗамовленняКлієнта_ТабличнаЧастина_Товари()
        {
            InitializeComponent();
        }

		public Документи.ЗамовленняКлієнта_Objest ЗамовленняКлієнта_Objest { get; set; }

        private void ЗамовленняКлієнта_ТабличнаЧастина_Товари_Load(object sender, EventArgs e)
        {
			//dataGridViewRecords.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

			RecordsBindingList = new BindingList<Записи>();
			dataGridViewRecords.DataSource = RecordsBindingList;

			dataGridViewRecords.Columns["ID"].Visible = false;

			dataGridViewRecords.Columns["Номенклатура"].Visible = false;
			dataGridViewRecords.Columns["Пакування"].Visible = false;

			dataGridViewRecords.Columns["НоменклатураНазва"].Width = 300;
			dataGridViewRecords.Columns["НоменклатураНазва"].ReadOnly = true;

			dataGridViewRecords.Columns["ПакуванняНазва"].Width = 300;
			dataGridViewRecords.Columns["ПакуванняНазва"].ReadOnly = true;
		}

		private BindingList<Записи> RecordsBindingList { get; set; }

		public void LoadRecords()
		{
			int selectRow = dataGridViewRecords.SelectedRows.Count > 0 ?
				dataGridViewRecords.SelectedRows[dataGridViewRecords.SelectedRows.Count - 1].Index : 0;

			RecordsBindingList.Clear();

			Query querySelect = ЗамовленняКлієнта_Objest.Товари_TablePart.QuerySelect;

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

			ЗамовленняКлієнта_Objest.Товари_TablePart.Read();

			Dictionary<string, Dictionary<string, string>> JoinValue = ЗамовленняКлієнта_Objest.Товари_TablePart.JoinValue;

			foreach (Документи.ЗамовленняКлієнта_Товари_TablePart.Record record in ЗамовленняКлієнта_Objest.Товари_TablePart.Records)
			{
				RecordsBindingList.Add(new Записи
				{
					ID = record.UID.ToString(),
					Номенклатура = record.Номенклатура.UnigueID.ToString(),
					НоменклатураНазва = JoinValue[record.UID.ToString()]["tovar_name"],
					Пакування = record.Пакування.UnigueID.ToString(),
					ПакуванняНазва = JoinValue[record.UID.ToString()]["pak_name"],
					Кількість = (uint)record.Кількість,
					Сума = record.Сума
				});
			}

			if (selectRow != 0 && selectRow < dataGridViewRecords.Rows.Count)
			{
				dataGridViewRecords.Rows[0].Selected = false;
				dataGridViewRecords.Rows[selectRow].Selected = true;
				dataGridViewRecords.FirstDisplayedScrollingRowIndex = selectRow;
			}
		}

		private class Записи
        {
            public string ID { get; set; }
			public string Номенклатура { get; set; }
            public string НоменклатураНазва { get; set; }
			public string Пакування { get; set; }
			public string ПакуванняНазва { get; set; }
			public uint Кількість { get; set; }
			public decimal Сума { get; set; }
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

		private void CopyMenuItem_Click(object sender, EventArgs e)
		{
			ToolStripMenuItem copyMenuItem = (ToolStripMenuItem)sender;
			Записи запис = (Записи)copyMenuItem.Tag;

			Console.WriteLine(copyMenuItem.Name);

			switch(copyMenuItem.Name)
            {
				case "НоменклатураНазва":
                    {
                        DirectoryControl directoryControl = new DirectoryControl();

                        Form_Номенклатура form_Номенклатура = new Form_Номенклатура();
                        form_Номенклатура.DirectoryPointerItem = new Довідники.Номенклатура_Pointer(new UnigueID(запис.Номенклатура));
                        form_Номенклатура.DirectoryControlItem = directoryControl;
                        form_Номенклатура.ShowDialog();

                        if (directoryControl.DirectoryPointerItem != null)
                        {
							Довідники.Номенклатура_Pointer номенклатура_Pointer = (Довідники.Номенклатура_Pointer)directoryControl.DirectoryPointerItem;

							запис.Номенклатура = номенклатура_Pointer.ToString();
							запис.НоменклатураНазва = номенклатура_Pointer.GetPresentation();
						}

                        break;
                    }
				case "ПакуванняНазва":
					{
                        DirectoryControl directoryControl = new DirectoryControl();

                        Form_ПакуванняОдиниціВиміру form_ПакуванняОдиниціВиміру = new Form_ПакуванняОдиниціВиміру();
                        form_ПакуванняОдиниціВиміру.DirectoryPointerItem = new Довідники.ПакуванняОдиниціВиміру_Pointer(new UnigueID(запис.Пакування));
                        form_ПакуванняОдиниціВиміру.DirectoryControlItem = directoryControl;
                        form_ПакуванняОдиниціВиміру.ShowDialog();

                        if (directoryControl.DirectoryPointerItem != null)
                        {
							Довідники.ПакуванняОдиниціВиміру_Pointer пакуванняОдиниціВиміру_Pointer = (Довідники.ПакуванняОдиниціВиміру_Pointer)directoryControl.DirectoryPointerItem;

							запис.Пакування = пакуванняОдиниціВиміру_Pointer.ToString();
							запис.ПакуванняНазва = пакуванняОдиниціВиміру_Pointer.GetPresentation();
						}

                        break;
					}
				default:
					break;
            }
		}

		private void dataGridViewRecords_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
			string columnName = dataGridViewRecords.Columns[e.ColumnIndex].Name;

			string[] allowColumn = new string[] { "НоменклатураНазва", "ПакуванняНазва" };

			contextMenuStrip1.Items.Clear();

			if (allowColumn.Contains(columnName))
            {
				ToolStripMenuItem copyMenuItem = new ToolStripMenuItem("Вибрати із списку");
				copyMenuItem.Image = Properties.Resources.data;
				copyMenuItem.Name = columnName;
				copyMenuItem.Tag = RecordsBindingList[e.RowIndex];
				copyMenuItem.Click += CopyMenuItem_Click;
				contextMenuStrip1.Items.Add(copyMenuItem);

				ToolStripTextBox toolStripTextBox = new ToolStripTextBox();
				toolStripTextBox.ToolTipText = "Пошук";
				toolStripTextBox.Size = new Size(300, 0);
				toolStripTextBox.TextChanged += ToolStripTextBox_TextChanged;
				contextMenuStrip1.Items.Add(toolStripTextBox);

				Rectangle rectangle = dataGridViewRecords.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);
				rectangle.Offset(0, dataGridViewRecords.Rows[e.RowIndex].Height);

				Point point = dataGridViewRecords.PointToScreen(rectangle.Location);

				contextMenuStrip1.Show(point);
			}
		}

       
        private void dataGridViewRecords_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
			
		}

        private void dataGridViewRecords_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
			return;

			Console.WriteLine(dataGridViewRecords.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
			Console.WriteLine(RecordsBindingList.Count);

			ToolStripMenuItem copyMenuItem = new ToolStripMenuItem("Вибрати із списку");
			copyMenuItem.Image = Properties.Resources.data;
			copyMenuItem.Name = "Номенклатура";
			copyMenuItem.Tag = "";
			copyMenuItem.Click += CopyMenuItem_Click;

			ToolStripMenuItem pasteMenuItem = new ToolStripMenuItem("Вставить");

			contextMenuStrip1.Items.Clear();
			contextMenuStrip1.Items.Add(copyMenuItem);
			contextMenuStrip1.Items.Add(pasteMenuItem);

			Rectangle rectangle = dataGridViewRecords.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);
			rectangle.Offset(0, dataGridViewRecords.Rows[e.RowIndex].Height);

			Point point = dataGridViewRecords.PointToScreen(rectangle.Location);

			contextMenuStrip1.Show(point);
			
		}

		

        private void toolStripButtonRefresh_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButtonAdd_Click(object sender, EventArgs e)
        {
			RecordsBindingList.AddNew();
		}

        private void dataGridViewRecords_CellClick(object sender, DataGridViewCellEventArgs e)
        {
			
		}
    }
}
