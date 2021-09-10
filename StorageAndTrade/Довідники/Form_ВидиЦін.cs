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
    public partial class Form_ВидиЦін : Form
    {
        public Form_ВидиЦін()
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

			Довідники.ВидиЦін_Select видиЦін_Select = new Довідники.ВидиЦін_Select();

			видиЦін_Select.QuerySelect.Field.Add(Довідники.ВидиЦін_Select.Назва);
			видиЦін_Select.QuerySelect.Field.Add(Довідники.ВидиЦін_Select.Валюта);
			
			//JOIN
			string JoinTable = Конфа.Config.Kernel.Conf.Directories["Валюти"].Table;
			string ParentField = JoinTable + "." + Конфа.Config.Kernel.Conf.Directories["Валюти"].Fields["Назва"].NameInTable;

			видиЦін_Select.QuerySelect.FieldAndAlias.Add(new KeyValuePair<string, string>(ParentField, "field2"));
			видиЦін_Select.QuerySelect.Joins.Add(new Join(JoinTable, Довідники.ВидиЦін_Select.Валюта, видиЦін_Select.QuerySelect.Table));

			//ORDER
			видиЦін_Select.QuerySelect.Order.Add(Довідники.ВидиЦін_Select.Назва, SelectOrder.ASC);

			видиЦін_Select.Select();
			while (видиЦін_Select.MoveNext())
			{
				Довідники.ВидиЦін_Pointer cur = видиЦін_Select.Current;

				RecordsBindingList.Add(new Записи
				{
					ID = cur.UnigueID.ToString(),
					Назва = cur.Fields[Довідники.ВидиЦін_Select.Назва].ToString(),
					Валюта = cur.Fields["field2"].ToString()
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
			public string Валюта { get; set; }
		}

        private void dataGridViewRecords_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
			if (e.RowIndex >= 0 && e.RowIndex < dataGridViewRecords.RowCount)
			{
				string Uid = dataGridViewRecords.Rows[e.RowIndex].Cells["ID"].Value.ToString();

				if (DirectoryControlItem != null)
				{
					DirectoryControlItem.DirectoryPointerItem = new Довідники.ВидиЦін_Pointer(new UnigueID(Uid));
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
			form_ВидиЦінЕлемент.IsNew = true;
			form_ВидиЦінЕлемент.OwnerForm = this;
			form_ВидиЦінЕлемент.ShowDialog();
        }

        private void toolStripButtonEdit_Click(object sender, EventArgs e)
        {
			if (dataGridViewRecords.SelectedRows.Count > 0)
			{
				int RowIndex = dataGridViewRecords.SelectedRows[0].Index;

				Form_ВидиЦінЕлемент form_ВидиЦінЕлемент = new Form_ВидиЦінЕлемент();
				form_ВидиЦінЕлемент.OwnerForm = this;
				form_ВидиЦінЕлемент.IsNew = false;
				form_ВидиЦінЕлемент.Uid = dataGridViewRecords.Rows[RowIndex].Cells[0].Value.ToString();
				form_ВидиЦінЕлемент.ShowDialog();
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

                    Довідники.ВидиЦін_Objest видиЦін_Objest = new Довідники.ВидиЦін_Objest();
                    if (видиЦін_Objest.Read(new UnigueID(uid)))
                    {
						Довідники.ВидиЦін_Objest ВидиЦін_Objest_Новий = видиЦін_Objest.Copy();
						ВидиЦін_Objest_Новий.Назва = "Копія - " + ВидиЦін_Objest_Новий.Назва;
						ВидиЦін_Objest_Новий.Save();
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
    }
}
