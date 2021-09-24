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

			dataGridViewRecords.Columns.Add(new DataGridViewImageColumn() { Name = "Image", HeaderText = "", Width = 30, DisplayIndex = 0, Image = Properties.Resources.doc_text_image });
			dataGridViewRecords.Columns["ID"].Visible = false;
			dataGridViewRecords.Columns["НомерДок"].Width = 100;
			dataGridViewRecords.Columns["ДатаДок"].Width = 120;
			dataGridViewRecords.Columns["Назва"].Width = 300;
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
			замовленняКлієнта_Select.QuerySelect.Field.Add(Документи.ЗамовленняКлієнта_Select.НомерДок);
			замовленняКлієнта_Select.QuerySelect.Field.Add(Документи.ЗамовленняКлієнта_Select.ДатаДок);
			замовленняКлієнта_Select.QuerySelect.Field.Add(Документи.ЗамовленняКлієнта_Select.СумаДокументу);

			//ORDER
			замовленняКлієнта_Select.QuerySelect.Order.Add(Документи.ЗамовленняКлієнта_Select.ДатаДок, SelectOrder.ASC);
			замовленняКлієнта_Select.QuerySelect.Order.Add(Документи.ЗамовленняКлієнта_Select.НомерДок, SelectOrder.ASC);

			замовленняКлієнта_Select.Select();
			while (замовленняКлієнта_Select.MoveNext())
			{
				Документи.ЗамовленняКлієнта_Pointer cur = замовленняКлієнта_Select.Current;

				RecordsBindingList.Add(new Записи
				{
					ID = cur.UnigueID.ToString(),
					Назва = "Замовлення клієнта №" + cur.Fields[Документи.ЗамовленняКлієнта_Select.НомерДок].ToString() + " від " +
							 DateTime.Parse(cur.Fields[Документи.ЗамовленняКлієнта_Select.ДатаДок].ToString()).ToShortDateString(),
					НомерДок = cur.Fields[Документи.ЗамовленняКлієнта_Select.НомерДок].ToString(),
					ДатаДок = cur.Fields[Документи.ЗамовленняКлієнта_Select.ДатаДок].ToString(),
					Сума = Math.Round((decimal)cur.Fields[Документи.ЗамовленняКлієнта_Select.СумаДокументу], 2)
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
					string uid = row.Cells[0].Value.ToString();

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
					string uid = row.Cells[0].Value.ToString();

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

				Звіти.ЗамовленняКлієнта_Report.PrintRegisterRecords(
					new Документи.ЗамовленняКлієнта_Pointer(new UnigueID(dataGridViewRecords.Rows[RowIndex].Cells[0].Value.ToString())));

			}
		}
    }
}
