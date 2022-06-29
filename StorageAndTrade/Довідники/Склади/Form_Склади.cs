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
    public partial class Form_Склади : Form
    {
        public Form_Склади()
        {
            InitializeComponent();

			dataGridViewRecords.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

			RecordsBindingList = new BindingList<Записи>();
			dataGridViewRecords.DataSource = RecordsBindingList;

			dataGridViewRecords.Columns["Image"].Width = 30;
			dataGridViewRecords.Columns["Image"].HeaderText = "";

			dataGridViewRecords.Columns["ID"].Visible = false;
			dataGridViewRecords.Columns["Назва"].Width = 300;
			dataGridViewRecords.Columns["Код"].Width = 50;

			dataGridViewRecords.Columns["ТипСкладу"].Width = 100;
			dataGridViewRecords.Columns["ТипСкладу"].HeaderText = "Тип";
		}

		public DirectoryPointer DirectoryPointerItem { get; set; }

        private void Form_Склади_Load(object sender, EventArgs e)
        {
			if (DirectoryPointerItem != null && !DirectoryPointerItem.IsEmpty())
			{
				Довідники.Склади_Pointer склади_Pointer = new Довідники.Склади_Pointer(new UnigueID(DirectoryPointerItem.UnigueID.UGuid));
				if (!склади_Pointer.IsEmpty())
				{
					Довідники.Склади_Objest склади_Objest = склади_Pointer.GetDirectoryObject();
					if (склади_Objest != null)
						Склади_Папки_Дерево.Parent_Pointer = склади_Objest.Папка;
				}
			}

			Склади_Папки_Дерево.CallBack_AfterSelect = TreeFolderAfterSelect;
			Склади_Папки_Дерево.LoadTree();
		}

		private BindingList<Записи> RecordsBindingList { get; set; }

		public void LoadRecords()
		{
			int selectRow = dataGridViewRecords.SelectedRows.Count > 0 ?
				dataGridViewRecords.SelectedRows[dataGridViewRecords.SelectedRows.Count - 1].Index : 0;

			RecordsBindingList.Clear();

			Довідники.Склади_Select склади_Select = new Довідники.Склади_Select();
			склади_Select.QuerySelect.Field.Add(Довідники.Склади_Const.Назва);
			склади_Select.QuerySelect.Field.Add(Довідники.Склади_Const.Код);
			склади_Select.QuerySelect.Field.Add(Довідники.Склади_Const.ТипСкладу);

			//WHERE
			if (Склади_Папки_Дерево.Parent_Pointer != null)
				склади_Select.QuerySelect.Where.Add(new Where(Довідники.Склади_Const.Папка, Comparison.EQ, Склади_Папки_Дерево.Parent_Pointer.UnigueID.UGuid));

			//ORDER
			склади_Select.QuerySelect.Order.Add(Довідники.Склади_Const.Назва, SelectOrder.ASC);

			склади_Select.Select();
			while (склади_Select.MoveNext())
			{
				Довідники.Склади_Pointer cur = склади_Select.Current;

				RecordsBindingList.Add(new Записи
				{
					ID = cur.UnigueID.ToString(),
					Назва = cur.Fields[Довідники.Склади_Const.Назва].ToString(),
					Код = cur.Fields[Довідники.Склади_Const.Код].ToString(),
					ТипСкладу = ((Перелічення.ТипиСкладів)cur.Fields[Довідники.Склади_Const.ТипСкладу]).ToString()
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
			public string Код { get; set; }
			public string ТипСкладу { get; set; }
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
					DirectoryPointerItem = new Довідники.Склади_Pointer(new UnigueID(Uid));
					this.DialogResult = DialogResult.OK;
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
			Form_СкладиЕлемент form_СкладиЕлемент = new Form_СкладиЕлемент();
			form_СкладиЕлемент.IsNew = true;
			form_СкладиЕлемент.OwnerForm = this;
			if (Склади_Папки_Дерево.Parent_Pointer != null)
				form_СкладиЕлемент.ParentUid = Склади_Папки_Дерево.Parent_Pointer.UnigueID.UGuid.ToString();
			form_СкладиЕлемент.ShowDialog();
        }

        private void toolStripButtonEdit_Click(object sender, EventArgs e)
        {
			if (dataGridViewRecords.SelectedRows.Count > 0)
			{
				int RowIndex = dataGridViewRecords.SelectedRows[0].Index;

				Form_СкладиЕлемент form_СкладиЕлемент = new Form_СкладиЕлемент();
				form_СкладиЕлемент.IsNew = false;
				form_СкладиЕлемент.OwnerForm = this;
				form_СкладиЕлемент.Uid = dataGridViewRecords.Rows[RowIndex].Cells["ID"].Value.ToString();
				form_СкладиЕлемент.ShowDialog();
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

                    Довідники.Склади_Objest склади_Objest = new Довідники.Склади_Objest();
                    if (склади_Objest.Read(new UnigueID(uid)))
                    {
						Довідники.Склади_Objest склади_Objest_Новий = склади_Objest.Copy();
						склади_Objest_Новий.Назва = "Копія - " + склади_Objest_Новий.Назва;
						склади_Objest_Новий.Код = (++Константи.НумераціяДовідників.Склади_Const).ToString("D6");
						склади_Objest_Новий.Save();
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

                    Довідники.Склади_Objest склади_Objest = new Довідники.Склади_Objest();
                    if (склади_Objest.Read(new UnigueID(uid)))
                    {
						склади_Objest.Delete();
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
