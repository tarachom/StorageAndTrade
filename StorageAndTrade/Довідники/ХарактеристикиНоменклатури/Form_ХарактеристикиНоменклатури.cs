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
using Довідники = StorageAndTrade_1_0.Довідники;
using Перелічення = StorageAndTrade_1_0.Перелічення;

namespace StorageAndTrade
{
    public partial class Form_ХарактеристикиНоменклатури : Form
    {
        public Form_ХарактеристикиНоменклатури()
        {
            InitializeComponent();

			dataGridViewRecords.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

			RecordsBindingList = new BindingList<Записи>();
			dataGridViewRecords.DataSource = RecordsBindingList;

			dataGridViewRecords.Columns["Image"].Width = 30;
			dataGridViewRecords.Columns["Image"].HeaderText = "";

			dataGridViewRecords.Columns["ID"].Visible = false;
			dataGridViewRecords.Columns["Назва"].Width = 300;
		}

		public DirectoryPointer DirectoryPointerItem { get; set; }

        private void Form_ХарактеристикиНоменклатури_Load(object sender, EventArgs e)
        {
			LoadRecords();
		}

		private BindingList<Записи> RecordsBindingList { get; set; }

		public void LoadRecords()
		{
			int selectRow = dataGridViewRecords.SelectedRows.Count > 0 ?
				dataGridViewRecords.SelectedRows[dataGridViewRecords.SelectedRows.Count - 1].Index : 0;

			RecordsBindingList.Clear();

			Довідники.ХарактеристикиНоменклатури_Select характеристикиНоменклатури_Select = new Довідники.ХарактеристикиНоменклатури_Select();
			характеристикиНоменклатури_Select.QuerySelect.Field.Add(Довідники.ХарактеристикиНоменклатури_Const.Назва);

			//ORDER
			характеристикиНоменклатури_Select.QuerySelect.Order.Add(Довідники.ХарактеристикиНоменклатури_Const.Назва, SelectOrder.ASC);

			характеристикиНоменклатури_Select.Select();
			while (характеристикиНоменклатури_Select.MoveNext())
			{
				Довідники.ХарактеристикиНоменклатури_Pointer cur = характеристикиНоменклатури_Select.Current;

				RecordsBindingList.Add(new Записи
				{
					ID = cur.UnigueID.ToString(),
					Назва = cur.Fields[Довідники.ХарактеристикиНоменклатури_Const.Назва].ToString()
				});

				if (DirectoryPointerItem != null && selectRow == 0)
					if (cur.UnigueID.ToString() == DirectoryPointerItem.UnigueID.ToString())
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
		}

        private void dataGridViewRecords_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
			if (e.RowIndex >= 0 && e.RowIndex < dataGridViewRecords.RowCount)
			{
				string Uid = dataGridViewRecords.Rows[e.RowIndex].Cells["ID"].Value.ToString();

				if (DirectoryPointerItem != null)
				{
					DirectoryPointerItem = new Довідники.ХарактеристикиНоменклатури_Pointer(new UnigueID(Uid));
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
			Form_ХарактеристикиНоменклатуриЕлемент form_ХарактеристикиНоменклатуриЕлемент = new Form_ХарактеристикиНоменклатуриЕлемент();
			form_ХарактеристикиНоменклатуриЕлемент.IsNew = true;
			form_ХарактеристикиНоменклатуриЕлемент.OwnerForm = this;
			form_ХарактеристикиНоменклатуриЕлемент.ShowDialog();
        }

        private void toolStripButtonEdit_Click(object sender, EventArgs e)
        {
			if (dataGridViewRecords.SelectedRows.Count > 0)
			{
				int RowIndex = dataGridViewRecords.SelectedRows[0].Index;

				Form_ХарактеристикиНоменклатуриЕлемент form_ХарактеристикиНоменклатуриЕлемент = new Form_ХарактеристикиНоменклатуриЕлемент();
				form_ХарактеристикиНоменклатуриЕлемент.IsNew = false;
				form_ХарактеристикиНоменклатуриЕлемент.OwnerForm = this;
				form_ХарактеристикиНоменклатуриЕлемент.Uid = dataGridViewRecords.Rows[RowIndex].Cells["ID"].Value.ToString();
				form_ХарактеристикиНоменклатуриЕлемент.ShowDialog();
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

                    Довідники.ХарактеристикиНоменклатури_Objest характеристикиНоменклатури_Objest = new Довідники.ХарактеристикиНоменклатури_Objest();
                    if (характеристикиНоменклатури_Objest.Read(new UnigueID(uid)))
                    {
						Довідники.ХарактеристикиНоменклатури_Objest характеристикиНоменклатури_Objest_Новий = характеристикиНоменклатури_Objest.Copy();
						характеристикиНоменклатури_Objest_Новий.Назва = "Копія - " + характеристикиНоменклатури_Objest_Новий.Назва;
						характеристикиНоменклатури_Objest_Новий.Save();
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

                    Довідники.ХарактеристикиНоменклатури_Objest характеристикиНоменклатури_Objest = new Довідники.ХарактеристикиНоменклатури_Objest();
                    if (характеристикиНоменклатури_Objest.Read(new UnigueID(uid)))
                    {
						характеристикиНоменклатури_Objest.Delete();
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
