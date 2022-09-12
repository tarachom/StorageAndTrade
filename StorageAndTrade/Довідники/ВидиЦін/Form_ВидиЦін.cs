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
    public partial class Form_ВидиЦін : Form
    {
        public Form_ВидиЦін()
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

		private void Form_ВидиЦін_Load(object sender, EventArgs e)
        {
			LoadRecords();
		}

		private BindingList<Записи> RecordsBindingList { get; set; }

		public void LoadRecords()
		{
			RecordsBindingList.Clear();

			Довідники.ВидиЦін_Select видиЦін_Select = new Довідники.ВидиЦін_Select();
			видиЦін_Select.QuerySelect.Field.Add(Довідники.ВидиЦін_Const.Назва);
			видиЦін_Select.QuerySelect.Field.Add(Довідники.ВидиЦін_Const.Код);
			видиЦін_Select.QuerySelect.Field.Add(Довідники.ВидиЦін_Const.Валюта);
			
			//JOIN
			видиЦін_Select.QuerySelect.FieldAndAlias.Add(
				new NameValue<string>(Довідники.Валюти_Const.TABLE + "." + Довідники.Валюти_Const.Назва, "field2"));
			видиЦін_Select.QuerySelect.Joins.Add(
				new Join(Довідники.Валюти_Const.TABLE, Довідники.ВидиЦін_Const.Валюта, Довідники.ВидиЦін_Const.TABLE));

			//ORDER
			видиЦін_Select.QuerySelect.Order.Add(Довідники.ВидиЦін_Const.Назва, SelectOrder.ASC);

			видиЦін_Select.Select();
			while (видиЦін_Select.MoveNext())
			{
				Довідники.ВидиЦін_Pointer cur = видиЦін_Select.Current;

				RecordsBindingList.Add(new Записи
				{
					ID = cur.UnigueID.ToString(),
					Назва = cur.Fields[Довідники.ВидиЦін_Const.Назва].ToString(),
					Код = cur.Fields[Довідники.ВидиЦін_Const.Код].ToString(),
					Валюта = cur.Fields["field2"].ToString()
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
			public string Валюта { get; set; }
		}

        private void dataGridViewRecords_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
			if (e.RowIndex >= 0 && e.RowIndex < dataGridViewRecords.RowCount)
			{
				string Uid = dataGridViewRecords.Rows[e.RowIndex].Cells["ID"].Value.ToString();

				if (DirectoryPointerItem != null)
				{
					DirectoryPointerItem = new Довідники.ВидиЦін_Pointer(new UnigueID(Uid));
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
			Form_ВидиЦінЕлемент form_ВидиЦінЕлемент = new Form_ВидиЦінЕлемент();
			form_ВидиЦінЕлемент.MdiParent = this.MdiParent;
			form_ВидиЦінЕлемент.IsNew = true;
			form_ВидиЦінЕлемент.OwnerForm = this;
			if (DirectoryPointerItem != null && this.MdiParent == null)
				form_ВидиЦінЕлемент.ShowDialog();
			else
				form_ВидиЦінЕлемент.Show();
		}

        private void toolStripButtonEdit_Click(object sender, EventArgs e)
        {
			if (dataGridViewRecords.SelectedRows.Count > 0)
			{
				int RowIndex = dataGridViewRecords.SelectedRows[0].Index;

				Form_ВидиЦінЕлемент form_ВидиЦінЕлемент = new Form_ВидиЦінЕлемент();
				form_ВидиЦінЕлемент.MdiParent = this.MdiParent;
				form_ВидиЦінЕлемент.OwnerForm = this;
				form_ВидиЦінЕлемент.IsNew = false;
				form_ВидиЦінЕлемент.Uid = dataGridViewRecords.Rows[RowIndex].Cells["ID"].Value.ToString();
				if (DirectoryPointerItem != null && this.MdiParent == null)
					form_ВидиЦінЕлемент.ShowDialog();
				else
					form_ВидиЦінЕлемент.Show();
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

                    Довідники.ВидиЦін_Objest видиЦін_Objest = new Довідники.ВидиЦін_Objest();
                    if (видиЦін_Objest.Read(new UnigueID(uid)))
                    {
						Довідники.ВидиЦін_Objest ВидиЦін_Objest_Новий = видиЦін_Objest.Copy();
						ВидиЦін_Objest_Новий.Назва = "Копія - " + ВидиЦін_Objest_Новий.Назва;
						ВидиЦін_Objest_Новий.Код = (++Константи.НумераціяДовідників.ВидиЦін_Const).ToString("D6");
						ВидиЦін_Objest_Новий.Save();

						SelectPointerItem = ВидиЦін_Objest_Новий.GetDirectoryPointer();
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

                    Довідники.ВидиЦін_Objest видиЦін_Objest = new Довідники.ВидиЦін_Objest();
                    if (видиЦін_Objest.Read(new UnigueID(uid)))
                    {
						видиЦін_Objest.Delete();
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

				SelectPointerItem = new Довідники.ВидиЦін_Pointer(new UnigueID(dataGridViewRecords.Rows[e.RowIndex].Cells["ID"].Value.ToString()));
			}
		}
    }
}
