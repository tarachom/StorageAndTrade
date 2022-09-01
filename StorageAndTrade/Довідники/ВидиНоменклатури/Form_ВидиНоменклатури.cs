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
    public partial class Form_ВидиНоменклатури : Form
    {
        public Form_ВидиНоменклатури()
        {
            InitializeComponent();

			dataGridViewRecords.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

			RecordsBindingList = new BindingList<Записи>();
			dataGridViewRecords.DataSource = RecordsBindingList;

			dataGridViewRecords.Columns["Image"].Width = 30;
			dataGridViewRecords.Columns["Image"].HeaderText = ""; 

			dataGridViewRecords.Columns["ID"].Visible = false;
			dataGridViewRecords.Columns["Назва"].Width = 300;

			dataGridViewRecords.Columns["ОдиницяВиміру"].Width = 100;
			dataGridViewRecords.Columns["ОдиницяВиміру"].HeaderText = "Од.";
		}

		/// <summary>
		/// Вказівник для вибору
		/// </summary>
		public DirectoryPointer DirectoryPointerItem { get; set; }

		/// <summary>
		/// Вказівник для виділення в списку
		/// </summary>
		public DirectoryPointer SelectPointerItem { get; set; }

		private void Form_ВидиНоменклатури_Load(object sender, EventArgs e)
		{
			LoadRecords();
		}

		private BindingList<Записи> RecordsBindingList { get; set; }

		public void LoadRecords()
		{
			RecordsBindingList.Clear();

			Довідники.ВидиНоменклатури_Select видиНоменклатури_Select = new Довідники.ВидиНоменклатури_Select();
			видиНоменклатури_Select.QuerySelect.Field.Add(Довідники.ВидиНоменклатури_Const.Назва);
			видиНоменклатури_Select.QuerySelect.Field.Add(Довідники.ВидиНоменклатури_Const.Код);
			видиНоменклатури_Select.QuerySelect.Field.Add(Довідники.ВидиНоменклатури_Const.ТипНоменклатури);

			//JOIN 1
			видиНоменклатури_Select.QuerySelect.FieldAndAlias.Add(
				new NameValue<string>(Довідники.ПакуванняОдиниціВиміру_Const.TABLE + "." + Довідники.ПакуванняОдиниціВиміру_Const.Назва, "join1"));
			видиНоменклатури_Select.QuerySelect.Joins.Add(
				new Join(Довідники.ПакуванняОдиниціВиміру_Const.TABLE, Довідники.ВидиНоменклатури_Const.ОдиницяВиміру, Довідники.ВидиНоменклатури_Const.TABLE));

			//ORDER
			видиНоменклатури_Select.QuerySelect.Order.Add(Довідники.ВидиНоменклатури_Const.Назва, SelectOrder.ASC);

			видиНоменклатури_Select.Select();
			while (видиНоменклатури_Select.MoveNext())
			{
				Довідники.ВидиНоменклатури_Pointer cur = видиНоменклатури_Select.Current;

				RecordsBindingList.Add(new Записи
				{
					ID = cur.UnigueID.ToString(),
					Назва = cur.Fields[Довідники.ВидиНоменклатури_Const.Назва].ToString(),
					Код = cur.Fields[Довідники.ВидиНоменклатури_Const.Код].ToString(),
					ОдиницяВиміру = cur.Fields["join1"].ToString(),
					ТипНоменклатури = ((Перелічення.ТипиНоменклатури)cur.Fields[Довідники.ВидиНоменклатури_Const.ТипНоменклатури]).ToString()
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
			public string ОдиницяВиміру { get; set; }
			public string ТипНоменклатури { get; set; }
		}

        private void dataGridViewRecords_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
			if (e.RowIndex >= 0 && e.RowIndex < dataGridViewRecords.RowCount)
			{
				string Uid = dataGridViewRecords.Rows[e.RowIndex].Cells["ID"].Value.ToString();

				if (DirectoryPointerItem != null)
				{
					DirectoryPointerItem = new Довідники.ВидиНоменклатури_Pointer(new UnigueID(Uid));
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
			Form_ВидиНоменклатуриЕлемент form_ВидиНоменклатуриЕлемент = new Form_ВидиНоменклатуриЕлемент();
			form_ВидиНоменклатуриЕлемент.MdiParent = this.MdiParent;
			form_ВидиНоменклатуриЕлемент.IsNew = true;
			form_ВидиНоменклатуриЕлемент.OwnerForm = this;
			if (DirectoryPointerItem != null && this.MdiParent == null)
				form_ВидиНоменклатуриЕлемент.ShowDialog();
			else
				form_ВидиНоменклатуриЕлемент.Show();
		}

        private void toolStripButtonEdit_Click(object sender, EventArgs e)
        {
			if (dataGridViewRecords.SelectedRows.Count > 0)
			{
				int RowIndex = dataGridViewRecords.SelectedRows[0].Index;

				Form_ВидиНоменклатуриЕлемент form_ВидиНоменклатуриЕлемент = new Form_ВидиНоменклатуриЕлемент();
				form_ВидиНоменклатуриЕлемент.MdiParent = this.MdiParent;
				form_ВидиНоменклатуриЕлемент.IsNew = false;
				form_ВидиНоменклатуриЕлемент.OwnerForm = this;
				form_ВидиНоменклатуриЕлемент.Uid = dataGridViewRecords.Rows[RowIndex].Cells["ID"].Value.ToString();
				if (DirectoryPointerItem != null && this.MdiParent == null)
					form_ВидиНоменклатуриЕлемент.ShowDialog();
				else
					form_ВидиНоменклатуриЕлемент.Show();
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

                    Довідники.ВидиНоменклатури_Objest видиНоменклатури_Objest = new Довідники.ВидиНоменклатури_Objest();
                    if (видиНоменклатури_Objest.Read(new UnigueID(uid)))
                    {
						Довідники.ВидиНоменклатури_Objest видиНоменклатури_Objest_Новий = видиНоменклатури_Objest.Copy();
						видиНоменклатури_Objest_Новий.Назва = "Копія - " + видиНоменклатури_Objest_Новий.Назва;
						видиНоменклатури_Objest_Новий.Код = (++Константи.НумераціяДовідників.ВидиНоменклатури_Const).ToString("D6");
						видиНоменклатури_Objest_Новий.Save();

						SelectPointerItem = видиНоменклатури_Objest_Новий.GetDirectoryPointer();
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

                    Довідники.ВидиНоменклатури_Objest видиНоменклатури_Objest = new Довідники.ВидиНоменклатури_Objest();
                    if (видиНоменклатури_Objest.Read(new UnigueID(uid)))
                    {
						видиНоменклатури_Objest.Delete();
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

				SelectPointerItem = new Довідники.ВидиНоменклатури_Pointer(new UnigueID(dataGridViewRecords.Rows[e.RowIndex].Cells["ID"].Value.ToString()));
			}
		}
    }
}
