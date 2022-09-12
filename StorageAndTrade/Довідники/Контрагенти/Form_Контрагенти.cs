/*
Copyright (C) 2019-2022 TARAKHOMYN YURIY IVANOVYCH
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

			dataGridViewRecords.Columns["Image"].Width = 30;
			dataGridViewRecords.Columns["Image"].HeaderText = "";

			dataGridViewRecords.Columns["ID"].Visible = false;
			dataGridViewRecords.Columns["Назва"].Width = 300;
			dataGridViewRecords.Columns["Код"].Width = 50;
		}

		/// <summary>
		/// Вказівник для вибору
		/// </summary>
		public DirectoryPointer DirectoryPointerItem { get; set; }

		/// <summary>
		/// Вказівник для виділення в списку
		/// </summary>
		public DirectoryPointer SelectPointerItem { get; set; }

		private void Form_Контрагенти_Load(object sender, EventArgs e)
		{
			if (DirectoryPointerItem != null && !DirectoryPointerItem.IsEmpty())
			{
				Довідники.Контрагенти_Objest контрагенти_Objest = new Довідники.Контрагенти_Pointer(new UnigueID(DirectoryPointerItem.UnigueID.UGuid)).GetDirectoryObject();
				if (контрагенти_Objest != null)
					Контрагенти_Папки_Дерево.Parent_Pointer = контрагенти_Objest.Папка;
			}

			Контрагенти_Папки_Дерево.CallBack_AfterSelect = TreeFolderAfterSelect;
			Контрагенти_Папки_Дерево.LoadTree();
		}

		private BindingList<Записи> RecordsBindingList { get; set; }

		public void LoadRecords()
		{
			RecordsBindingList.Clear();

			Довідники.Контрагенти_Select контрагенти_Select = new Довідники.Контрагенти_Select();
			контрагенти_Select.QuerySelect.Field.Add(Довідники.Контрагенти_Const.Назва);
			контрагенти_Select.QuerySelect.Field.Add(Довідники.Контрагенти_Const.Код);

			//WHERE
			if (Контрагенти_Папки_Дерево.Parent_Pointer != null)
				контрагенти_Select.QuerySelect.Where.Add(new Where(Довідники.Контрагенти_Const.Папка, Comparison.EQ, Контрагенти_Папки_Дерево.Parent_Pointer.UnigueID.UGuid));

			//ORDER
			контрагенти_Select.QuerySelect.Order.Add(Довідники.Контрагенти_Const.Назва, SelectOrder.ASC);

			контрагенти_Select.Select();
			while (контрагенти_Select.MoveNext())
			{
				Довідники.Контрагенти_Pointer cur = контрагенти_Select.Current;

				RecordsBindingList.Add(new Записи
				{
					ID = cur.UnigueID.ToString(),
					Назва = cur.Fields[Довідники.Контрагенти_Const.Назва].ToString(),
					Код = cur.Fields[Довідники.Контрагенти_Const.Код].ToString()
				});
			}

			if ((DirectoryPointerItem != null || SelectPointerItem != null) && dataGridViewRecords.Rows.Count > 0)
			{
				string UidSelect = SelectPointerItem != null ? SelectPointerItem.UnigueID.ToString() : DirectoryPointerItem.UnigueID.ToString();

				if (UidSelect != Guid.Empty.ToString())
					ФункціїДляІнтерфейсу.ВиділитиЕлементСписку(dataGridViewRecords, "ID", UidSelect);
			}
		}

		private class Записи
		{
			public Записи() { Image = Properties.Resources.doc_text_image; }
			public Bitmap Image { get; set; }
			public string ID { get; set; }
			public string Назва { get; set; }
			public string Код { get; set; }
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
			Form_КонтрагентиЕлемент form_КонтрагентиЕлемент = new Form_КонтрагентиЕлемент();
			form_КонтрагентиЕлемент.MdiParent = this.MdiParent;
			form_КонтрагентиЕлемент.IsNew = true;
			form_КонтрагентиЕлемент.OwnerForm = this;
			if (Контрагенти_Папки_Дерево.Parent_Pointer != null)
				form_КонтрагентиЕлемент.ParentUid = Контрагенти_Папки_Дерево.Parent_Pointer.UnigueID.UGuid.ToString();
			if (DirectoryPointerItem != null && this.MdiParent == null)
				form_КонтрагентиЕлемент.ShowDialog();
			else
				form_КонтрагентиЕлемент.Show();
		}

        private void toolStripButtonEdit_Click(object sender, EventArgs e)
        {
			if (dataGridViewRecords.SelectedRows.Count > 0)
			{
				int RowIndex = dataGridViewRecords.SelectedRows[0].Index;

				Form_КонтрагентиЕлемент form_КонтрагентиЕлемент = new Form_КонтрагентиЕлемент();
				form_КонтрагентиЕлемент.MdiParent = this.MdiParent;
				form_КонтрагентиЕлемент.IsNew = false;
				form_КонтрагентиЕлемент.OwnerForm = this;
				form_КонтрагентиЕлемент.Uid = dataGridViewRecords.Rows[RowIndex].Cells["ID"].Value.ToString();
				if (DirectoryPointerItem != null && this.MdiParent == null)
					form_КонтрагентиЕлемент.ShowDialog();
				else
					form_КонтрагентиЕлемент.Show();
			}
		}

        private void toolStripButtonRefresh_Click(object sender, EventArgs e)
        {
			Контрагенти_Папки_Дерево.LoadTree();
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

                    Довідники.Контрагенти_Objest контрагенти_Objest = new Довідники.Контрагенти_Objest();
                    if (контрагенти_Objest.Read(new UnigueID(uid)))
                    {
						Довідники.Контрагенти_Objest контрагенти_Objest_Новий = контрагенти_Objest.Copy();
						контрагенти_Objest_Новий.Назва = "Копія - " + контрагенти_Objest_Новий.Назва;
						контрагенти_Objest_Новий.Код = (++Константи.НумераціяДовідників.Контрагенти_Const).ToString("D6");
						контрагенти_Objest_Новий.Save();

						SelectPointerItem = контрагенти_Objest_Новий.GetDirectoryPointer();
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

        private void dataGridViewRecords_CellClick(object sender, DataGridViewCellEventArgs e)
        {
			if (dataGridViewRecords.SelectedRows.Count > 0)
			{
				int RowIndex = dataGridViewRecords.SelectedRows[0].Index;

				SelectPointerItem = new Довідники.Контрагенти_Pointer(new UnigueID(dataGridViewRecords.Rows[e.RowIndex].Cells["ID"].Value.ToString()));
			}
		}

        #region Договори

        private void toolStripButtonДоговори_Click(object sender, EventArgs e)
		{
            if (dataGridViewRecords.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dataGridViewRecords.SelectedRows[0];
                string uid = row.Cells["ID"].Value.ToString();

                Form_ДоговориКонтрагентів form_ДоговориКонтрагентів = new Form_ДоговориКонтрагентів();
                form_ДоговориКонтрагентів.MdiParent = this.MdiParent;
                form_ДоговориКонтрагентів.КонтрагентВласник = new Довідники.Контрагенти_Pointer(new UnigueID(uid));
                form_ДоговориКонтрагентів.Show();

            }
        }

        #endregion
    }
}
