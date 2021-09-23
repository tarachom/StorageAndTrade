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
    public partial class Form_Контрагенти : Form
    {
        public Form_Контрагенти()
        {
            InitializeComponent();

			dataGridViewRecords.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

			RecordsBindingList = new BindingList<Записи>();
			dataGridViewRecords.DataSource = RecordsBindingList;

			dataGridViewRecords.Columns.Add(new DataGridViewImageColumn() { Name = "Image", HeaderText = "", Width = 30, DisplayIndex = 0, Image = Properties.Resources.doc_text_image });
			dataGridViewRecords.Columns["ID"].Visible = false;
			dataGridViewRecords.Columns["Назва"].Width = 300;

			Контрагенти_Папки_Дерево.CallBack_AfterSelect = TreeFolderAfterSelect;
			Контрагенти_Папки_Дерево.LoadTree();
		}

		public DirectoryPointer DirectoryPointerItem { get; set; }

        private void FormCash_Load(object sender, EventArgs e)
        {
			if (DirectoryPointerItem != null && !DirectoryPointerItem.IsEmpty())
			{
				Довідники.Контрагенти_Pointer контрагенти_Pointer = new Довідники.Контрагенти_Pointer(new UnigueID(DirectoryPointerItem.UnigueID.UGuid));
				if (!контрагенти_Pointer.IsEmpty())
				{
					Довідники.Контрагенти_Objest контрагенти_Objest = контрагенти_Pointer.GetDirectoryObject();
					if (контрагенти_Objest != null)
					{
						Контрагенти_Папки_Дерево.Parent_Pointer = контрагенти_Objest.Папка;
						Контрагенти_Папки_Дерево.SetParentPointer();
					}
				}
			}
		}

		private BindingList<Записи> RecordsBindingList { get; set; }

		public void LoadRecords()
		{
			RecordsBindingList.Clear();

			Довідники.Контрагенти_Select контрагенти_Select = new Довідники.Контрагенти_Select();
			контрагенти_Select.QuerySelect.Field.Add(Довідники.Контрагенти_Select.Назва);

			//WHERE
			if (Контрагенти_Папки_Дерево.Parent_Pointer != null)
				контрагенти_Select.QuerySelect.Where.Add(new Where(Довідники.Контрагенти_Select.Папка, Comparison.EQ, Контрагенти_Папки_Дерево.Parent_Pointer.UnigueID.UGuid));

			//ORDER
			контрагенти_Select.QuerySelect.Order.Add(Довідники.Контрагенти_Select.Назва, SelectOrder.ASC);

			контрагенти_Select.Select();
			while (контрагенти_Select.MoveNext())
			{
				Довідники.Контрагенти_Pointer cur = контрагенти_Select.Current;

				RecordsBindingList.Add(new Записи
				{
					ID = cur.UnigueID.ToString(),
					Назва = cur.Fields[Довідники.Контрагенти_Select.Назва].ToString()
				});
			}

			if (DirectoryPointerItem != null && !DirectoryPointerItem.IsEmpty())
			{
				dataGridViewRecords.Rows[0].Selected = false;

				foreach (DataGridViewRow row in dataGridViewRecords.Rows)
				{
					if (row.Cells["ID"].Value.ToString() == DirectoryPointerItem.UnigueID.ToString())
					{
						row.Selected = true;
						break;
					}
				}
			}
		}

		private class Записи
		{
			public string ID { get; set; }
			public string Назва { get; set; }
		}

		public void TreeFolderAfterSelect()
		{
			LoadRecords();
		}

		private void dataGridViewRecords_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
			if (e.RowIndex >= 0 && e.RowIndex < dataGridViewRecords.RowCount)
			{
				string Uid = dataGridViewRecords.Rows[e.RowIndex].Cells["ID"].Value.ToString();

				if (DirectoryPointerItem != null)
				{
					DirectoryPointerItem = new Довідники.Контрагенти_Pointer(new UnigueID(Uid));
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
			Form_КонтрагентиЕлемент form_КонтрагентиЕлемент = new Form_КонтрагентиЕлемент();
			form_КонтрагентиЕлемент.IsNew = true;
			form_КонтрагентиЕлемент.OwnerForm = this;
			form_КонтрагентиЕлемент.ShowDialog();
        }

        private void toolStripButtonEdit_Click(object sender, EventArgs e)
        {
			if (dataGridViewRecords.SelectedRows.Count > 0)
			{
				int RowIndex = dataGridViewRecords.SelectedRows[0].Index;

				Form_КонтрагентиЕлемент form_КонтрагентиЕлемент = new Form_КонтрагентиЕлемент();
				form_КонтрагентиЕлемент.IsNew = false;
				form_КонтрагентиЕлемент.OwnerForm = this;
				form_КонтрагентиЕлемент.Uid = dataGridViewRecords.Rows[RowIndex].Cells[0].Value.ToString();
				form_КонтрагентиЕлемент.ShowDialog();
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

                    Довідники.Контрагенти_Objest контрагенти_Objest = new Довідники.Контрагенти_Objest();
                    if (контрагенти_Objest.Read(new UnigueID(uid)))
                    {
						Довідники.Контрагенти_Objest контрагенти_Objest_Новий = контрагенти_Objest.Copy();
						контрагенти_Objest_Новий.Назва = "Копія - " + контрагенти_Objest_Новий.Назва;
						контрагенти_Objest_Новий.Save();
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

                    Довідники.Контрагенти_Objest контрагенти_Objest = new Довідники.Контрагенти_Objest();
                    if (контрагенти_Objest.Read(new UnigueID(uid)))
                    {
						контрагенти_Objest.Delete();
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
