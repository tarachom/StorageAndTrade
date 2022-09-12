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
    public partial class Form_БанківськіРахункиКонтрагентів : Form
    {
        public Form_БанківськіРахункиКонтрагентів()
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

		private void Form_БанківськіРахункиКонтрагентів_Load(object sender, EventArgs e)
        {
			LoadRecords();
		}

		private BindingList<Записи> RecordsBindingList { get; set; }

		public void LoadRecords()
		{
			RecordsBindingList.Clear();

			Довідники.БанківськіРахункиКонтрагентів_Select банківськіРахункиКонтрагентів_Select = new Довідники.БанківськіРахункиКонтрагентів_Select();
			банківськіРахункиКонтрагентів_Select.QuerySelect.Field.Add(Довідники.БанківськіРахункиКонтрагентів_Const.Назва);
			банківськіРахункиКонтрагентів_Select.QuerySelect.Field.Add(Довідники.БанківськіРахункиКонтрагентів_Const.Код);

			//ORDER
			банківськіРахункиКонтрагентів_Select.QuerySelect.Order.Add(Довідники.БанківськіРахункиКонтрагентів_Const.Назва, SelectOrder.ASC);

			банківськіРахункиКонтрагентів_Select.Select();
			while (банківськіРахункиКонтрагентів_Select.MoveNext())
			{
				Довідники.БанківськіРахункиКонтрагентів_Pointer cur = банківськіРахункиКонтрагентів_Select.Current;

				RecordsBindingList.Add(new Записи
				{
					ID = cur.UnigueID.ToString(),
					Назва = cur.Fields[Довідники.БанківськіРахункиКонтрагентів_Const.Назва].ToString(),
					Код = cur.Fields[Довідники.БанківськіРахункиКонтрагентів_Const.Код].ToString()
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

        private void dataGridViewRecords_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
			if (e.RowIndex >= 0 && e.RowIndex < dataGridViewRecords.RowCount)
			{
				string Uid = dataGridViewRecords.Rows[e.RowIndex].Cells["ID"].Value.ToString();

				if (DirectoryPointerItem != null)
				{
					DirectoryPointerItem = new Довідники.БанківськіРахункиКонтрагентів_Pointer(new UnigueID(Uid));
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
			Form_БанківськіРахункиКонтрагентівЕлемент form_БанківськіРахункиКонтрагентівЕлемент = new Form_БанківськіРахункиКонтрагентівЕлемент();
			form_БанківськіРахункиКонтрагентівЕлемент.MdiParent = this.MdiParent;
			form_БанківськіРахункиКонтрагентівЕлемент.IsNew = true;
			form_БанківськіРахункиКонтрагентівЕлемент.OwnerForm = this;
			if (DirectoryPointerItem != null && this.MdiParent == null)
				form_БанківськіРахункиКонтрагентівЕлемент.ShowDialog();
			else
				form_БанківськіРахункиКонтрагентівЕлемент.Show();
		}

        private void toolStripButtonEdit_Click(object sender, EventArgs e)
        {
			if (dataGridViewRecords.SelectedRows.Count > 0)
			{
				int RowIndex = dataGridViewRecords.SelectedRows[0].Index;

				Form_БанківськіРахункиКонтрагентівЕлемент form_БанківськіРахункиКонтрагентівЕлемент = new Form_БанківськіРахункиКонтрагентівЕлемент();
				form_БанківськіРахункиКонтрагентівЕлемент.MdiParent = this.MdiParent;
				form_БанківськіРахункиКонтрагентівЕлемент.IsNew = false;
				form_БанківськіРахункиКонтрагентівЕлемент.OwnerForm = this;
				form_БанківськіРахункиКонтрагентівЕлемент.Uid = dataGridViewRecords.Rows[RowIndex].Cells["ID"].Value.ToString();
				if (DirectoryPointerItem != null && this.MdiParent == null)
					form_БанківськіРахункиКонтрагентівЕлемент.ShowDialog();
				else
					form_БанківськіРахункиКонтрагентівЕлемент.Show();
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

                    Довідники.БанківськіРахункиКонтрагентів_Objest банківськіРахункиКонтрагентів_Objest = new Довідники.БанківськіРахункиКонтрагентів_Objest();
                    if (банківськіРахункиКонтрагентів_Objest.Read(new UnigueID(uid)))
                    {
						Довідники.БанківськіРахункиКонтрагентів_Objest банківськіРахункиКонтрагентів_Objest_Новий = банківськіРахункиКонтрагентів_Objest.Copy();
						банківськіРахункиКонтрагентів_Objest_Новий.Назва = "Копія - " + банківськіРахункиКонтрагентів_Objest_Новий.Назва;
						банківськіРахункиКонтрагентів_Objest_Новий.Код = (++Константи.НумераціяДовідників.БанківськіРахункиКонтрагентів_Const).ToString("D6");
						банківськіРахункиКонтрагентів_Objest_Новий.Save();

						SelectPointerItem = банківськіРахункиКонтрагентів_Objest_Новий.GetDirectoryPointer();
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

        private void dataGridViewRecords_CellClick(object sender, DataGridViewCellEventArgs e)
        {
			if (dataGridViewRecords.SelectedRows.Count > 0)
			{
				int RowIndex = dataGridViewRecords.SelectedRows[0].Index;

				SelectPointerItem = new Довідники.БанківськіРахункиКонтрагентів_Pointer(new UnigueID(dataGridViewRecords.Rows[e.RowIndex].Cells["ID"].Value.ToString()));
			}
		}
    }
}
