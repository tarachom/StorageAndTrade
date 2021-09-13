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

namespace StorageAndTrade
{
    public partial class Form_ЗамовленняПостачальникуЖурнал : Form
    {
        public Form_ЗамовленняПостачальникуЖурнал()
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

			Документи.ЗамовленняПостачальнику_Select замовленняПостачальнику_Select = new Документи.ЗамовленняПостачальнику_Select();
			замовленняПостачальнику_Select.QuerySelect.Field.Add(Документи.ЗамовленняПостачальнику_Select.НомерДок);
			замовленняПостачальнику_Select.QuerySelect.Field.Add(Документи.ЗамовленняПостачальнику_Select.ДатаДок);
			замовленняПостачальнику_Select.QuerySelect.Field.Add(Документи.ЗамовленняПостачальнику_Select.СумаДокументу);

			//ORDER
			замовленняПостачальнику_Select.QuerySelect.Order.Add(Документи.ЗамовленняПостачальнику_Select.ДатаДок, SelectOrder.ASC);
			замовленняПостачальнику_Select.QuerySelect.Order.Add(Документи.ЗамовленняПостачальнику_Select.НомерДок, SelectOrder.ASC);

			замовленняПостачальнику_Select.Select();
			while (замовленняПостачальнику_Select.MoveNext())
			{
				Документи.ЗамовленняПостачальнику_Pointer cur = замовленняПостачальнику_Select.Current;

				RecordsBindingList.Add(new Записи
				{
					ID = cur.UnigueID.ToString(),
					Назва = "Замовлення постачальнику №" + cur.Fields[Документи.ЗамовленняПостачальнику_Select.НомерДок].ToString() + " від " +
							 DateTime.Parse(cur.Fields[Документи.ЗамовленняПостачальнику_Select.ДатаДок].ToString()).ToShortDateString(),
					НомерДок = cur.Fields[Документи.ЗамовленняПостачальнику_Select.НомерДок].ToString(),
					ДатаДок = cur.Fields[Документи.ЗамовленняПостачальнику_Select.ДатаДок].ToString(),
					Сума = Math.Round((decimal)cur.Fields[Документи.ЗамовленняПостачальнику_Select.СумаДокументу], 2)
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
			Form_ЗамовленняПостачальникуДокумент form_ЗамовленняПостачальникуДокумент = new Form_ЗамовленняПостачальникуДокумент();
			form_ЗамовленняПостачальникуДокумент.IsNew = true;
			form_ЗамовленняПостачальникуДокумент.OwnerForm = this;
			form_ЗамовленняПостачальникуДокумент.ShowDialog();
        }

        private void toolStripButtonEdit_Click(object sender, EventArgs e)
        {
			if (dataGridViewRecords.SelectedRows.Count > 0)
			{
				int RowIndex = dataGridViewRecords.SelectedRows[0].Index;

				Form_ЗамовленняПостачальникуДокумент form_ЗамовленняПостачальникуДокумент = new Form_ЗамовленняПостачальникуДокумент();
				form_ЗамовленняПостачальникуДокумент.IsNew = false;
				form_ЗамовленняПостачальникуДокумент.OwnerForm = this;
				form_ЗамовленняПостачальникуДокумент.Uid = dataGridViewRecords.Rows[RowIndex].Cells[0].Value.ToString();
				form_ЗамовленняПостачальникуДокумент.ShowDialog();
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
    }
}
