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
    public partial class Form_БанківськіРахункиКонтрагентів : Form
    {
        public Form_БанківськіРахункиКонтрагентів()
        {
            InitializeComponent();
        }

		public DirectoryPointer DirectoryPointerItem { get; set; }

		private void FormCash_Load(object sender, EventArgs e)
        {
			dataGridViewRecords.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

			RecordsBindingList = new BindingList<Записи>();
			dataGridViewRecords.DataSource = RecordsBindingList;

			dataGridViewRecords.Columns.Add(new DataGridViewImageColumn() { Name = "Image", HeaderText = "", Width = 30, DisplayIndex = 0, Image = Properties.Resources.doc_text_image });
			dataGridViewRecords.Columns["ID"].Visible = false;
			dataGridViewRecords.Columns["Назва"].Width = 300;

			LoadRecords();
		}

		private BindingList<Записи> RecordsBindingList { get; set; }

		public void LoadRecords()
		{
			int selectRow = dataGridViewRecords.SelectedRows.Count > 0 ?
				dataGridViewRecords.SelectedRows[dataGridViewRecords.SelectedRows.Count - 1].Index : 0;

			RecordsBindingList.Clear();

			Довідники.БанківськіРахункиКонтрагентів_Select банківськіРахункиКонтрагентів_Select = new Довідники.БанківськіРахункиКонтрагентів_Select();
			банківськіРахункиКонтрагентів_Select.QuerySelect.Field.Add(Довідники.БанківськіРахункиКонтрагентів_Select.Назва);

			//ORDER
			банківськіРахункиКонтрагентів_Select.QuerySelect.Order.Add(Довідники.БанківськіРахункиКонтрагентів_Select.Назва, SelectOrder.ASC);

			банківськіРахункиКонтрагентів_Select.Select();
			while (банківськіРахункиКонтрагентів_Select.MoveNext())
			{
				Довідники.БанківськіРахункиКонтрагентів_Pointer cur = банківськіРахункиКонтрагентів_Select.Current;

				RecordsBindingList.Add(new Записи
				{
					ID = cur.UnigueID.ToString(),
					Назва = cur.Fields[Довідники.БанківськіРахункиКонтрагентів_Select.Назва].ToString()
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
					DirectoryPointerItem = new Довідники.БанківськіРахункиКонтрагентів_Pointer(new UnigueID(Uid));
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
			Form_БанківськіРахункиКонтрагентівЕлемент form_БанківськіРахункиКонтрагентівЕлемент = new Form_БанківськіРахункиКонтрагентівЕлемент();
			form_БанківськіРахункиКонтрагентівЕлемент.IsNew = true;
			form_БанківськіРахункиКонтрагентівЕлемент.OwnerForm = this;
			form_БанківськіРахункиКонтрагентівЕлемент.ShowDialog();
        }

        private void toolStripButtonEdit_Click(object sender, EventArgs e)
        {
			if (dataGridViewRecords.SelectedRows.Count > 0)
			{
				int RowIndex = dataGridViewRecords.SelectedRows[0].Index;

				Form_БанківськіРахункиКонтрагентівЕлемент form_БанківськіРахункиКонтрагентівЕлемент = new Form_БанківськіРахункиКонтрагентівЕлемент();
				form_БанківськіРахункиКонтрагентівЕлемент.IsNew = false;
				form_БанківськіРахункиКонтрагентівЕлемент.OwnerForm = this;
				form_БанківськіРахункиКонтрагентівЕлемент.Uid = dataGridViewRecords.Rows[RowIndex].Cells[0].Value.ToString();
				form_БанківськіРахункиКонтрагентівЕлемент.ShowDialog();
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

                    Довідники.БанківськіРахункиКонтрагентів_Objest банківськіРахункиКонтрагентів_Objest = new Довідники.БанківськіРахункиКонтрагентів_Objest();
                    if (банківськіРахункиКонтрагентів_Objest.Read(new UnigueID(uid)))
                    {
						Довідники.БанківськіРахункиКонтрагентів_Objest банківськіРахункиКонтрагентів_Objest_Новий = банківськіРахункиКонтрагентів_Objest.Copy();
						банківськіРахункиКонтрагентів_Objest_Новий.Назва = "Копія - " + банківськіРахункиКонтрагентів_Objest_Новий.Назва;
						банківськіРахункиКонтрагентів_Objest_Новий.Save();
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

                    Довідники.БанківськіРахункиКонтрагентів_Objest банківськіРахункиКонтрагентів_Objest = new Довідники.БанківськіРахункиКонтрагентів_Objest();
                    if (банківськіРахункиКонтрагентів_Objest.Read(new UnigueID(uid)))
                    {
						банківськіРахункиКонтрагентів_Objest.Delete();
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
