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
    public partial class Form_Каси : Form
    {
        public Form_Каси()
        {
            InitializeComponent();
        }

		#region DirectoryControl Open Form

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

			Довідники.Каси_Select каси_Select = new Довідники.Каси_Select();

			каси_Select.QuerySelect.Field.Add(Довідники.Каси_Select.Назва);
			каси_Select.QuerySelect.Field.Add(Довідники.Каси_Select.Валюта);
			
			//JOIN
			string JoinTable = Конфа.Config.Kernel.Conf.Directories["Валюти"].Table;
			string ParentField = JoinTable + "." + Конфа.Config.Kernel.Conf.Directories["Валюти"].Fields["Назва"].NameInTable;

			каси_Select.QuerySelect.FieldAndAlias.Add(new KeyValuePair<string, string>(ParentField, "field2"));
			каси_Select.QuerySelect.Joins.Add(new Join(JoinTable, Довідники.Каси_Select.Валюта, каси_Select.QuerySelect.Table));

			//ORDER
			каси_Select.QuerySelect.Order.Add(Довідники.Каси_Select.Назва, SelectOrder.ASC);

			каси_Select.Select();
			while (каси_Select.MoveNext())
			{
				Довідники.Каси_Pointer cur = каси_Select.Current;

				RecordsBindingList.Add(new Записи(
					cur.UnigueID.ToString(),
					cur.Fields[Довідники.Каси_Select.Назва].ToString(),
					cur.Fields["field2"].ToString()
					));

				if (DirectoryPointerItem != null && selectRow == 0) //??
					if (cur.UnigueID.ToString() == DirectoryPointerItem.UnigueID.ToString())
					{
						dataGridViewRecords.Rows[0].Selected = false;
						dataGridViewRecords.Rows[RecordsBindingList.Count - 1].Selected = true;
					}
			}

			if (selectRow != 0 && selectRow < dataGridViewRecords.Rows.Count)
			{
				dataGridViewRecords.Rows[0].Selected = false;
				dataGridViewRecords.Rows[selectRow].Selected = true;
			}
		}

		private class Записи
		{
			public Записи(string _id, string _Назва, string _Валюта)
			{
				ID = _id;
				Назва = _Назва;
				Валюта = _Валюта;
			}
			public string ID { get; set; }
			public string Назва { get; set; }
			public string Валюта { get; set; }
		}

        private void dataGridViewRecords_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
			if (e.RowIndex >= 0 && e.RowIndex < dataGridViewRecords.RowCount)
			{
				string Uid = dataGridViewRecords.Rows[e.RowIndex].Cells["ID"].Value.ToString();

				if (DirectoryControlItem != null)
				{
					DirectoryControlItem.DirectoryPointerItem = new Довідники.Каси_Pointer(new UnigueID(Uid));
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
            Form_КасиЕлемент formAddCash = new Form_КасиЕлемент();
            formAddCash.IsNew = true;
            formAddCash.OwnerForm = this;
            formAddCash.ShowDialog();
        }

        private void toolStripButtonEdit_Click(object sender, EventArgs e)
        {
			if (dataGridViewRecords.SelectedRows.Count > 0)
			{
				int RowIndex = dataGridViewRecords.SelectedRows[0].Index;

                Form_КасиЕлемент formAddCash = new Form_КасиЕлемент();
                formAddCash.OwnerForm = this;
                formAddCash.IsNew = false;
                formAddCash.Uid = dataGridViewRecords.Rows[RowIndex].Cells[0].Value.ToString();
                formAddCash.ShowDialog();
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

                    Довідники.Каси_Objest каси_Objest = new Довідники.Каси_Objest();
                    if (каси_Objest.Read(new UnigueID(uid)))
                    {
						Довідники.Каси_Objest каси_Новий_Objest = каси_Objest.Copy();
						каси_Новий_Objest.Назва = "Копія - " + каси_Новий_Objest.Назва;
						каси_Новий_Objest.Save();
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

                    Довідники.Каси_Objest каси_Objest = new Довідники.Каси_Objest();
                    if (каси_Objest.Read(new UnigueID(uid)))
                    {
						каси_Objest.Delete();
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
