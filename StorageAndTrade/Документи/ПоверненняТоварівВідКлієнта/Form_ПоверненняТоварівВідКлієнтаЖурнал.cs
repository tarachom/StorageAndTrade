﻿/*
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
    public partial class Form_ПоверненняТоварівВідКлієнтаЖурнал : Form
    {
        public Form_ПоверненняТоварівВідКлієнтаЖурнал()
        {
            InitializeComponent();

			dataGridViewRecords.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

			RecordsBindingList = new BindingList<Записи>();
			dataGridViewRecords.DataSource = RecordsBindingList;

			dataGridViewRecords.Columns["Image"].Width = 30;
			dataGridViewRecords.Columns["Image"].HeaderText = "";

			dataGridViewRecords.Columns["ID"].Visible = false;
			dataGridViewRecords.Columns["НомерДок"].Width = 100;
			dataGridViewRecords.Columns["ДатаДок"].Width = 120;
			dataGridViewRecords.Columns["Назва"].Width = 300;

			dataGridViewRecords.Columns["Проведений"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
			dataGridViewRecords.Columns["Проведений"].Width = 80;
		}

        private void FormCash_Load(object sender, EventArgs e)
        {
			LoadRecords();
		}

		private BindingList<Записи> RecordsBindingList { get; set; }

		public void LoadRecords()
		{
			int selectRow = dataGridViewRecords.SelectedRows.Count > 0 ?
				dataGridViewRecords.SelectedRows[dataGridViewRecords.SelectedRows.Count - 1].Index : 0;

			RecordsBindingList.Clear();

			Документи.ПоверненняТоварівВідКлієнта_Select поверненняТоварівВідКлієнта_Select = new Документи.ПоверненняТоварівВідКлієнта_Select();
			поверненняТоварівВідКлієнта_Select.QuerySelect.Field.Add(Документи.ПоверненняТоварівВідКлієнта_Const.НомерДок);
			поверненняТоварівВідКлієнта_Select.QuerySelect.Field.Add(Документи.ПоверненняТоварівВідКлієнта_Const.ДатаДок);
			поверненняТоварівВідКлієнта_Select.QuerySelect.Field.Add(Документи.ПоверненняТоварівВідКлієнта_Const.СумаДокументу);

			//ORDER
			поверненняТоварівВідКлієнта_Select.QuerySelect.Order.Add(Документи.ПоверненняТоварівВідКлієнта_Const.ДатаДок, SelectOrder.ASC);
			поверненняТоварівВідКлієнта_Select.QuerySelect.Order.Add(Документи.ПоверненняТоварівВідКлієнта_Const.НомерДок, SelectOrder.ASC);

			поверненняТоварівВідКлієнта_Select.Select();
			while (поверненняТоварівВідКлієнта_Select.MoveNext())
			{
				Документи.ПоверненняТоварівВідКлієнта_Pointer cur = поверненняТоварівВідКлієнта_Select.Current;

				RecordsBindingList.Add(new Записи
				{
					ID = cur.UnigueID.ToString(),
					Назва = "Повернення від клієнта №" + cur.Fields[Документи.ПоверненняТоварівВідКлієнта_Const.НомерДок].ToString() + " від " +
							 DateTime.Parse(cur.Fields[Документи.ПоверненняТоварівВідКлієнта_Const.ДатаДок].ToString()).ToShortDateString(),
					НомерДок = cur.Fields[Документи.ПоверненняТоварівВідКлієнта_Const.НомерДок].ToString(),
					ДатаДок = cur.Fields[Документи.ПоверненняТоварівВідКлієнта_Const.ДатаДок].ToString(),
					Сума = Math.Round((decimal)cur.Fields[Документи.ПоверненняТоварівВідКлієнта_Const.СумаДокументу], 2)
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
			Form_ПоверненняТоварівВідКлієнтаДокумент form_ПоверненняТоварівВідКлієнтаДокумент = new Form_ПоверненняТоварівВідКлієнтаДокумент();
			form_ПоверненняТоварівВідКлієнтаДокумент.IsNew = true;
			form_ПоверненняТоварівВідКлієнтаДокумент.OwnerForm = this;
			form_ПоверненняТоварівВідКлієнтаДокумент.ShowDialog();
        }

        private void toolStripButtonEdit_Click(object sender, EventArgs e)
        {
			if (dataGridViewRecords.SelectedRows.Count > 0)
			{
				int RowIndex = dataGridViewRecords.SelectedRows[0].Index;

				Form_ПоверненняТоварівВідКлієнтаДокумент form_ПоверненняТоварівВідКлієнтаДокумент = new Form_ПоверненняТоварівВідКлієнтаДокумент();
				form_ПоверненняТоварівВідКлієнтаДокумент.IsNew = false;
				form_ПоверненняТоварівВідКлієнтаДокумент.OwnerForm = this;
				form_ПоверненняТоварівВідКлієнтаДокумент.Uid = dataGridViewRecords.Rows[RowIndex].Cells["ID"].Value.ToString();
				form_ПоверненняТоварівВідКлієнтаДокумент.ShowDialog();
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
					string uid = row.Cells["ID"].Value.ToString();

                    Документи.ПоверненняТоварівВідКлієнта_Objest поверненняТоварівВідКлієнта_Objest = new Документи.ПоверненняТоварівВідКлієнта_Objest();
                    if (поверненняТоварівВідКлієнта_Objest.Read(new UnigueID(uid)))
                    {
						Документи.ПоверненняТоварівВідКлієнта_Objest поверненняТоварівВідКлієнта_Objest_Новий = поверненняТоварівВідКлієнта_Objest.Copy();
						поверненняТоварівВідКлієнта_Objest_Новий.Save();
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
					string uid = row.Cells["ID"].Value.ToString();

                    Документи.ПоверненняТоварівВідКлієнта_Objest поверненняТоварівВідКлієнта_Objest = new Документи.ПоверненняТоварівВідКлієнта_Objest();
                    if (поверненняТоварівВідКлієнта_Objest.Read(new UnigueID(uid)))
                    {
						поверненняТоварівВідКлієнта_Objest.Delete();
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

				Звіти.РухПоРугістрахНакопичення.PrintRecords(new Документи.ПоверненняТоварівВідКлієнта_Pointer(new UnigueID(uid)));
			}
		}
    }
}
