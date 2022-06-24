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
    public partial class Form_ЗамовленняКлієнтаЖурнал : Form
    {
        public Form_ЗамовленняКлієнтаЖурнал()
        {
            InitializeComponent();

			dataGridViewRecords.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

			RecordsBindingList = new BindingList<Записи>();
			dataGridViewRecords.DataSource = RecordsBindingList;

			dataGridViewRecords.Columns["Image"].Width = 30;
			dataGridViewRecords.Columns["Image"].HeaderText = "";

			dataGridViewRecords.Columns["ID"].Visible = false;

			dataGridViewRecords.Columns["НомерДок"].Width = 100;
			dataGridViewRecords.Columns["НомерДок"].HeaderText = "Номер";
			dataGridViewRecords.Columns["НомерДок"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

			dataGridViewRecords.Columns["ДатаДок"].Width = 120;
			dataGridViewRecords.Columns["ДатаДок"].HeaderText = "Дата";
			dataGridViewRecords.Columns["ДатаДок"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

			dataGridViewRecords.Columns["Назва"].Width = 500;

			dataGridViewRecords.Columns["Сума"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
			dataGridViewRecords.Columns["Сума"].CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
			dataGridViewRecords.Columns["Сума"].Width = 100;

			dataGridViewRecords.Columns["Проведений"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
			dataGridViewRecords.Columns["Проведений"].Width = 80; 
		}

		public DocumentPointer DocumentPointerItem { get; set; }

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

			Документи.ЗамовленняКлієнта_Select замовленняКлієнта_Select = new Документи.ЗамовленняКлієнта_Select();
			замовленняКлієнта_Select.QuerySelect.Field.Add(Документи.ЗамовленняКлієнта_Const.Проведений);
			замовленняКлієнта_Select.QuerySelect.Field.Add(Документи.ЗамовленняКлієнта_Const.Назва);			
			замовленняКлієнта_Select.QuerySelect.Field.Add(Документи.ЗамовленняКлієнта_Const.НомерДок);
			замовленняКлієнта_Select.QuerySelect.Field.Add(Документи.ЗамовленняКлієнта_Const.ДатаДок);
			замовленняКлієнта_Select.QuerySelect.Field.Add(Документи.ЗамовленняКлієнта_Const.СумаДокументу);

			//ORDER
			замовленняКлієнта_Select.QuerySelect.Order.Add(Документи.ЗамовленняКлієнта_Const.ДатаДок, SelectOrder.ASC);
			замовленняКлієнта_Select.QuerySelect.Order.Add(Документи.ЗамовленняКлієнта_Const.НомерДок, SelectOrder.ASC);

			замовленняКлієнта_Select.Select();
			while (замовленняКлієнта_Select.MoveNext())
			{
				Документи.ЗамовленняКлієнта_Pointer cur = замовленняКлієнта_Select.Current;

				RecordsBindingList.Add(new Записи
				{
					ID = cur.UnigueID.ToString(),
					Назва = cur.Fields[Документи.ЗамовленняКлієнта_Const.Назва].ToString(),
					НомерДок = cur.Fields[Документи.ЗамовленняКлієнта_Const.НомерДок].ToString(),
					ДатаДок = cur.Fields[Документи.ЗамовленняКлієнта_Const.ДатаДок].ToString(),
					Сума = Math.Round((decimal)cur.Fields[Документи.ЗамовленняКлієнта_Const.СумаДокументу], 2),
					Проведений = (bool)cur.Fields[Документи.ЗамовленняКлієнта_Const.Проведений]
				});

				if (DocumentPointerItem != null && selectRow == 0) 
					if (cur.UnigueID.ToString() == DocumentPointerItem.UnigueID.ToString())
						selectRow = RecordsBindingList.Count - 1;
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
			public Записи() { Image = Properties.Resources.doc_text_image; }
			public Bitmap Image { get; set; }
			public string ID { get; set; }
			public string Назва { get; set; }
			public string НомерДок { get; set; }
			public string ДатаДок { get; set; }
			public decimal Сума { get; set; }
			public bool Проведений { get; set; }
		}

        private void dataGridViewRecords_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
			if (e.RowIndex >= 0 && e.RowIndex < dataGridViewRecords.RowCount)
			{
				string Uid = dataGridViewRecords.Rows[e.RowIndex].Cells["ID"].Value.ToString();

                if (DocumentPointerItem != null)
                {
					DocumentPointerItem = new Документи.ЗамовленняКлієнта_Pointer(new UnigueID(Uid));
					this.Close();
                }
                else
                {
                    toolStripButtonEdit_Click(this, null);
                }
            }
		}

        private void toolStripButtonAdd_Click(object sender, EventArgs e)
        {
			Form_ЗамовленняКлієнтаДокумент form_ЗамовленняКлієнтаДокумент = new Form_ЗамовленняКлієнтаДокумент();
			form_ЗамовленняКлієнтаДокумент.IsNew = true;
			form_ЗамовленняКлієнтаДокумент.OwnerForm = this;
			form_ЗамовленняКлієнтаДокумент.ShowDialog();
        }

        private void toolStripButtonEdit_Click(object sender, EventArgs e)
        {
			if (dataGridViewRecords.SelectedRows.Count > 0)
			{
				int RowIndex = dataGridViewRecords.SelectedRows[0].Index;

				Form_ЗамовленняКлієнтаДокумент form_ЗамовленняКлієнтаДокумент = new Form_ЗамовленняКлієнтаДокумент();
				form_ЗамовленняКлієнтаДокумент.IsNew = false;
				form_ЗамовленняКлієнтаДокумент.OwnerForm = this;
				form_ЗамовленняКлієнтаДокумент.Uid = dataGridViewRecords.Rows[RowIndex].Cells["ID"].Value.ToString();
				form_ЗамовленняКлієнтаДокумент.ShowDialog();
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

                    Документи.ЗамовленняКлієнта_Objest замовленняКлієнта_Objest = new Документи.ЗамовленняКлієнта_Objest();
                    if (замовленняКлієнта_Objest.Read(new UnigueID(uid)))
                    {
						Документи.ЗамовленняКлієнта_Objest ЗамовленняКлієнта_Objest_Новий = замовленняКлієнта_Objest.Copy();
						ЗамовленняКлієнта_Objest_Новий.Save();
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

                    Документи.ЗамовленняКлієнта_Objest ЗамовленняКлієнта_Objest = new Документи.ЗамовленняКлієнта_Objest();
                    if (ЗамовленняКлієнта_Objest.Read(new UnigueID(uid)))
                    {
						ЗамовленняКлієнта_Objest.Delete();
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

				Звіти.РухПоРугістрахНакопичення.PrintRecords(new Документи.ЗамовленняКлієнта_Pointer(new UnigueID(uid)));
			}
		}

		private void SpendDocuments(bool spend, string message)
        {
			if (dataGridViewRecords.SelectedRows.Count != 0 &&
				MessageBox.Show(message, "Повідомлення", MessageBoxButtons.YesNo) == DialogResult.Yes)
			{
				for (int i = 0; i < dataGridViewRecords.SelectedRows.Count; i++)
				{
					DataGridViewRow row = dataGridViewRecords.SelectedRows[i];
					string uid = row.Cells["ID"].Value.ToString();

					Документи.ЗамовленняКлієнта_Pointer замовленняКлієнта_Pointer = new Документи.ЗамовленняКлієнта_Pointer(new UnigueID(uid));
					Документи.ЗамовленняКлієнта_Objest замовленняКлієнта_Objest = замовленняКлієнта_Pointer.GetDocumentObject(true);

					замовленняКлієнта_Objest.Проведений = spend;
					замовленняКлієнта_Objest.Save();
				}

				LoadRecords();
			}
		}


		private void toolStripButtonSpend_Click(object sender, EventArgs e)
        {
			SpendDocuments(true, "Провести?");
		}

        private void toolStripButtonClearSpend_Click(object sender, EventArgs e)
        {
			SpendDocuments(false, "Відмінити проведення?");
		}
    }
}
