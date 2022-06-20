/*
Copyright (C) 2019-2020 TARAKHOMYN YURIY IVANOVYCH
All rights reserved.
Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at
    http://www.apache.org/licenses/LICENSE-2.0
Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

/*
Автор:    Тарахомин Юрій Іванович
Адреса:   Україна, м. Львів
Сайт:     accounting.org.ua
*/

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
using Документи = StorageAndTrade_1_0.Документи;
using Перелічення = StorageAndTrade_1_0.Перелічення;
using Звіти = StorageAndTrade_1_0.Звіти;

namespace StorageAndTrade
{
    public partial class Form_РеалізаціяТоварівТаПослугЖурнал : Form
    {
        public Form_РеалізаціяТоварівТаПослугЖурнал()
        {
            InitializeComponent();
        }

        private void FormCash_Load(object sender, EventArgs e)
        {
			dataGridViewRecords.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

			RecordsBindingList = new BindingList<Записи>();
			dataGridViewRecords.DataSource = RecordsBindingList;

			dataGridViewRecords.Columns.Add(new DataGridViewImageColumn() { Name = "Image", HeaderText = "", Width = 30, DisplayIndex = 0, Image = Properties.Resources.doc_text_image });
			dataGridViewRecords.Columns["ID"].Visible = false;
			dataGridViewRecords.Columns["НомерДок"].Width = 100;
			dataGridViewRecords.Columns["ДатаДок"].Width = 120;
			dataGridViewRecords.Columns["Назва"].Width = 300;

			LoadRecords();
		}

		private BindingList<Записи> RecordsBindingList { get; set; }

		public void LoadRecords()
		{
			int selectRow = dataGridViewRecords.SelectedRows.Count > 0 ?
				dataGridViewRecords.SelectedRows[dataGridViewRecords.SelectedRows.Count - 1].Index : 0;

			RecordsBindingList.Clear();

			Документи.РеалізаціяТоварівТаПослуг_Select реалізаціяТоварівТаПослуг_Select = new Документи.РеалізаціяТоварівТаПослуг_Select();
			реалізаціяТоварівТаПослуг_Select.QuerySelect.Field.Add(Документи.РеалізаціяТоварівТаПослуг_Const.Назва);
			реалізаціяТоварівТаПослуг_Select.QuerySelect.Field.Add(Документи.РеалізаціяТоварівТаПослуг_Const.НомерДок);
			реалізаціяТоварівТаПослуг_Select.QuerySelect.Field.Add(Документи.РеалізаціяТоварівТаПослуг_Const.ДатаДок);
			реалізаціяТоварівТаПослуг_Select.QuerySelect.Field.Add(Документи.РеалізаціяТоварівТаПослуг_Const.СумаДокументу);

			//ORDER
			реалізаціяТоварівТаПослуг_Select.QuerySelect.Order.Add(Документи.РеалізаціяТоварівТаПослуг_Const.ДатаДок, SelectOrder.ASC);
			реалізаціяТоварівТаПослуг_Select.QuerySelect.Order.Add(Документи.РеалізаціяТоварівТаПослуг_Const.НомерДок, SelectOrder.ASC);

			реалізаціяТоварівТаПослуг_Select.Select();
			while (реалізаціяТоварівТаПослуг_Select.MoveNext())
			{
				Документи.РеалізаціяТоварівТаПослуг_Pointer cur = реалізаціяТоварівТаПослуг_Select.Current;

				RecordsBindingList.Add(new Записи
				{
					ID = cur.UnigueID.ToString(),
					Назва = cur.Fields[Документи.РеалізаціяТоварівТаПослуг_Const.Назва].ToString(),
					НомерДок = cur.Fields[Документи.РеалізаціяТоварівТаПослуг_Const.НомерДок].ToString(),
					ДатаДок = cur.Fields[Документи.РеалізаціяТоварівТаПослуг_Const.ДатаДок].ToString(),
					Сума = Math.Round((decimal)cur.Fields[Документи.РеалізаціяТоварівТаПослуг_Const.СумаДокументу], 2)
				});

				//if (DirectoryPointerItem != null && selectRow == 0) 
				//	if (cur.UnigueID.ToString() == DirectoryPointerItem.UnigueID.ToString())
				//		selectRow = RecordsBindingList.Count - 1;
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
			public string Назва { get; set; }
			public string НомерДок { get; set; }
			public string ДатаДок { get; set; }
			public decimal Сума { get; set; }
		}

        private void dataGridViewRecords_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
			if (e.RowIndex >= 0 && e.RowIndex < dataGridViewRecords.RowCount)
			{
				string Uid = dataGridViewRecords.Rows[e.RowIndex].Cells["ID"].Value.ToString();

				//if (DirectoryControlItem != null)
				//{
				//	//DirectoryControlItem.DirectoryPointerItem = new Документи.ЗамовленняКлієнта_Pointer(new UnigueID(Uid));
				//	this.Close();
				//}
				//else
				//{
					toolStripButtonEdit_Click(this, null);
				//}
			}
		}

        private void toolStripButtonAdd_Click(object sender, EventArgs e)
        {
			Form_РеалізаціяТоварівТаПослугДокумент form_РеалізаціяТоварівТаПослугДокумент = new Form_РеалізаціяТоварівТаПослугДокумент();
			form_РеалізаціяТоварівТаПослугДокумент.IsNew = true;
			form_РеалізаціяТоварівТаПослугДокумент.OwnerForm = this;
			form_РеалізаціяТоварівТаПослугДокумент.ShowDialog();
        }

        private void toolStripButtonEdit_Click(object sender, EventArgs e)
        {
			if (dataGridViewRecords.SelectedRows.Count > 0)
			{
				int RowIndex = dataGridViewRecords.SelectedRows[0].Index;

				Form_РеалізаціяТоварівТаПослугДокумент form_РеалізаціяТоварівТаПослугДокумент = new Form_РеалізаціяТоварівТаПослугДокумент();
				form_РеалізаціяТоварівТаПослугДокумент.IsNew = false;
				form_РеалізаціяТоварівТаПослугДокумент.OwnerForm = this;
				form_РеалізаціяТоварівТаПослугДокумент.Uid = dataGridViewRecords.Rows[RowIndex].Cells[0].Value.ToString();
				form_РеалізаціяТоварівТаПослугДокумент.ShowDialog();
			}
		}

        private void toolStripButtonRefresh_Click(object sender, EventArgs e)
        {
			LoadRecords();
		}

        private void toolStripButtonCopy_Click(object sender, EventArgs e)
        {
			if (dataGridViewRecords.SelectedRows.Count != 0 &&
				MessageBox.Show("Копіювати записи?", "Повідомлення", MessageBoxButtons.YesNo) == DialogResult.Yes)
			{
				for (int i = 0; i < dataGridViewRecords.SelectedRows.Count; i++)
				{
					DataGridViewRow row = dataGridViewRecords.SelectedRows[i];
					string uid = row.Cells[0].Value.ToString();

                    Документи.РеалізаціяТоварівТаПослуг_Objest реалізаціяТоварівТаПослуг_Objest = new Документи.РеалізаціяТоварівТаПослуг_Objest();
                    if (реалізаціяТоварівТаПослуг_Objest.Read(new UnigueID(uid)))
                    {
						Документи.РеалізаціяТоварівТаПослуг_Objest реалізаціяТоварівТаПослуг_Objest_Новий = реалізаціяТоварівТаПослуг_Objest.Copy();
						реалізаціяТоварівТаПослуг_Objest_Новий.Save();
					}
                    else
                    {
                        MessageBox.Show("Error read");
                        break;
                    }
                }

				LoadRecords();
			}
		}

        private void toolStripButtonDelete_Click(object sender, EventArgs e)
        {
			if (dataGridViewRecords.SelectedRows.Count != 0 &&
				MessageBox.Show("Видалити записи?", "Повідомлення", MessageBoxButtons.YesNo) == DialogResult.Yes)
			{
				for (int i = 0; i < dataGridViewRecords.SelectedRows.Count; i++)
				{
					DataGridViewRow row = dataGridViewRecords.SelectedRows[i];
					string uid = row.Cells[0].Value.ToString();

                    Документи.РеалізаціяТоварівТаПослуг_Objest реалізаціяТоварівТаПослуг_Objest = new Документи.РеалізаціяТоварівТаПослуг_Objest();
                    if (реалізаціяТоварівТаПослуг_Objest.Read(new UnigueID(uid)))
                    {
						реалізаціяТоварівТаПослуг_Objest.Delete();
                    }
                    else
                    {
                        MessageBox.Show("Error read");
                        break;
                    }
                }

				LoadRecords();
			}
		}

        private void toolStripButtonДрукПроводок_Click(object sender, EventArgs e)
        {
			if (dataGridViewRecords.SelectedRows.Count > 0)
			{
				int RowIndex = dataGridViewRecords.SelectedRows[0].Index;
				string uid = dataGridViewRecords.Rows[RowIndex].Cells["ID"].Value.ToString();

				Звіти.РухПоРугістрахНакопичення.PrintRecords(new Документи.РеалізаціяТоварівТаПослуг_Pointer(new UnigueID(uid)));
			}
		}
    }
}
