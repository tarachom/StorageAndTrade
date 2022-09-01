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
    public partial class Form_СеріїНоменклатури : Form
    {
        public Form_СеріїНоменклатури()
        {
            InitializeComponent();

			dataGridViewRecords.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

			RecordsBindingList = new BindingList<Записи>();
			dataGridViewRecords.DataSource = RecordsBindingList;

			dataGridViewRecords.Columns["Image"].Width = 30;
			dataGridViewRecords.Columns["Image"].HeaderText = "";

			dataGridViewRecords.Columns["ID"].Visible = false;
			dataGridViewRecords.Columns["Номер"].Width = 300;
			dataGridViewRecords.Columns["Коментар"].Width = 300;
		}

		/// <summary>
		/// Вказівник для вибору
		/// </summary>
		public DirectoryPointer DirectoryPointerItem { get; set; }

		/// <summary>
		/// Вказівник для виділення в списку
		/// </summary>
		public DirectoryPointer SelectPointerItem { get; set; }

		private void Form_СеріїНоменклатури_Load(object sender, EventArgs e)
        {
			LoadRecords();
		}

		private BindingList<Записи> RecordsBindingList { get; set; }

		public void LoadRecords()
		{
			RecordsBindingList.Clear();

			Довідники.СеріїНоменклатури_Select серіїНоменклатури_Select = new Довідники.СеріїНоменклатури_Select();
			серіїНоменклатури_Select.QuerySelect.Field.Add(Довідники.СеріїНоменклатури_Const.Номер);
			серіїНоменклатури_Select.QuerySelect.Field.Add(Довідники.СеріїНоменклатури_Const.Коментар);

			//ORDER
			серіїНоменклатури_Select.QuerySelect.Order.Add(Довідники.СеріїНоменклатури_Const.Номер, SelectOrder.ASC);

			серіїНоменклатури_Select.Select();
			while (серіїНоменклатури_Select.MoveNext())
			{
				Довідники.СеріїНоменклатури_Pointer cur = серіїНоменклатури_Select.Current;

				RecordsBindingList.Add(new Записи
				{
					ID = cur.UnigueID.ToString(),
					Номер = cur.Fields[Довідники.СеріїНоменклатури_Const.Номер].ToString(),
					Коментар = cur.Fields[Довідники.СеріїНоменклатури_Const.Коментар].ToString(),
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
			public string Номер { get; set; }
			public string Коментар { get; set; }
		}

        private void dataGridViewRecords_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
			if (e.RowIndex >= 0 && e.RowIndex < dataGridViewRecords.RowCount)
			{
				string Uid = dataGridViewRecords.Rows[e.RowIndex].Cells["ID"].Value.ToString();

				if (DirectoryPointerItem != null)
				{
					DirectoryPointerItem = new Довідники.СеріїНоменклатури_Pointer(new UnigueID(Uid));
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
			Form_СеріїНоменклатуриЕлемент form_СеріїНоменклатуриЕлемент = new Form_СеріїНоменклатуриЕлемент();
			form_СеріїНоменклатуриЕлемент.MdiParent = this.MdiParent;
			form_СеріїНоменклатуриЕлемент.IsNew = true;
			form_СеріїНоменклатуриЕлемент.OwnerForm = this;
			if (DirectoryPointerItem != null && this.MdiParent == null)
				form_СеріїНоменклатуриЕлемент.ShowDialog();
			else
				form_СеріїНоменклатуриЕлемент.Show();
		}

        private void toolStripButtonEdit_Click(object sender, EventArgs e)
        {
			if (dataGridViewRecords.SelectedRows.Count > 0)
			{
				int RowIndex = dataGridViewRecords.SelectedRows[0].Index;

				Form_СеріїНоменклатуриЕлемент form_СеріїНоменклатуриЕлемент = new Form_СеріїНоменклатуриЕлемент();
				form_СеріїНоменклатуриЕлемент.MdiParent = this.MdiParent;
				form_СеріїНоменклатуриЕлемент.IsNew = false;
				form_СеріїНоменклатуриЕлемент.OwnerForm = this;
				form_СеріїНоменклатуриЕлемент.Uid = dataGridViewRecords.Rows[RowIndex].Cells["ID"].Value.ToString();
				if (DirectoryPointerItem != null && this.MdiParent == null)
					form_СеріїНоменклатуриЕлемент.ShowDialog();
				else
					form_СеріїНоменклатуриЕлемент.Show();
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

                    Довідники.СеріїНоменклатури_Objest серіїНоменклатури_Objest = new Довідники.СеріїНоменклатури_Objest();
                    if (серіїНоменклатури_Objest.Read(new UnigueID(uid)))
                    {
						Довідники.СеріїНоменклатури_Objest серіїНоменклатури_Objest_Новий = серіїНоменклатури_Objest.Copy();
						серіїНоменклатури_Objest_Новий.Номер = Guid.NewGuid().ToString();
						серіїНоменклатури_Objest_Новий.Коментар = "Копія - " + серіїНоменклатури_Objest.Номер;
						серіїНоменклатури_Objest_Новий.Save();

						SelectPointerItem = серіїНоменклатури_Objest_Новий.GetDirectoryPointer();
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

                    Довідники.СеріїНоменклатури_Objest серіїНоменклатури_Objest = new Довідники.СеріїНоменклатури_Objest();
                    if (серіїНоменклатури_Objest.Read(new UnigueID(uid)))
                    {
						серіїНоменклатури_Objest.Delete();
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

				SelectPointerItem = new Довідники.СеріїНоменклатури_Pointer(new UnigueID(dataGridViewRecords.Rows[e.RowIndex].Cells["ID"].Value.ToString()));
			}
		}
    }
}
