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
    public partial class Form_ДоговориКонтрагентів : Form
    {
        public Form_ДоговориКонтрагентів()
        {
            InitializeComponent();
        }

		#region DirectoryControl OpenForm

		/// <summary>
		/// Ссилка на елемент довідника
		/// </summary>
		public DirectoryPointer DirectoryPointerItem { get; set; }

		/// <summary>
		/// Контрол який викликав вибір
		/// </summary>
		public DirectoryControl DirectoryControlItem { get; set; }

        #endregion

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

			Довідники.ДоговориКонтрагентів_Select договориКонтрагентів_Select = new Довідники.ДоговориКонтрагентів_Select();
			договориКонтрагентів_Select.QuerySelect.Field.Add(Довідники.ДоговориКонтрагентів_Select.Назва);

			//ORDER
			договориКонтрагентів_Select.QuerySelect.Order.Add(Довідники.ДоговориКонтрагентів_Select.Назва, SelectOrder.ASC);

			договориКонтрагентів_Select.Select();
			while (договориКонтрагентів_Select.MoveNext())
			{
				Довідники.ДоговориКонтрагентів_Pointer cur = договориКонтрагентів_Select.Current;

				RecordsBindingList.Add(new Записи
				{
					ID = cur.UnigueID.ToString(),
					Назва = cur.Fields[Довідники.ДоговориКонтрагентів_Select.Назва].ToString()
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

				if (DirectoryControlItem != null)
				{
					DirectoryControlItem.DirectoryPointerItem = new Довідники.Валюти_Pointer(new UnigueID(Uid));
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
			Form_ДоговориКонтрагентівЕлемент form_ДоговориКонтрагентівЕлемент = new Form_ДоговориКонтрагентівЕлемент();
			form_ДоговориКонтрагентівЕлемент.IsNew = true;
			form_ДоговориКонтрагентівЕлемент.OwnerForm = this;
			form_ДоговориКонтрагентівЕлемент.ShowDialog();
        }

        private void toolStripButtonEdit_Click(object sender, EventArgs e)
        {
			if (dataGridViewRecords.SelectedRows.Count > 0)
			{
				int RowIndex = dataGridViewRecords.SelectedRows[0].Index;

				Form_ДоговориКонтрагентівЕлемент form_ДоговориКонтрагентівЕлемент = new Form_ДоговориКонтрагентівЕлемент();
				form_ДоговориКонтрагентівЕлемент.IsNew = false;
				form_ДоговориКонтрагентівЕлемент.OwnerForm = this;
				form_ДоговориКонтрагентівЕлемент.Uid = dataGridViewRecords.Rows[RowIndex].Cells[0].Value.ToString();
				form_ДоговориКонтрагентівЕлемент.ShowDialog();
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

                    Довідники.ДоговориКонтрагентів_Objest договориКонтрагентів_Objest = new Довідники.ДоговориКонтрагентів_Objest();
                    if (договориКонтрагентів_Objest.Read(new UnigueID(uid)))
                    {
						Довідники.ДоговориКонтрагентів_Objest договориКонтрагентів_Objest_Новий = договориКонтрагентів_Objest.Copy();
						договориКонтрагентів_Objest_Новий.Назва = "Копія - " + договориКонтрагентів_Objest_Новий.Назва;
						договориКонтрагентів_Objest_Новий.Save();
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

                    Довідники.ДоговориКонтрагентів_Objest договориКонтрагентів_Objest = new Довідники.ДоговориКонтрагентів_Objest();
                    if (договориКонтрагентів_Objest.Read(new UnigueID(uid)))
                    {
						договориКонтрагентів_Objest.Delete();
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
