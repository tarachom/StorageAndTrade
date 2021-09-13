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
    public partial class Form_Номенклатура : Form
    {
        public Form_Номенклатура()
        {
            InitializeComponent();
        }

		public DirectoryPointer DirectoryPointerItem { get; set; }

        private void FormCash_Load(object sender, EventArgs e)
        {
			dataGridViewRecords.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

			RecordsBindingList = new BindingList<Записи>();
			dataGridViewRecords.DataSource = RecordsBindingList;

			dataGridViewRecords.Columns.Add(new DataGridViewImageColumn() { Name = "Image", HeaderText = "", Width = 30, DisplayIndex = 0, Image = Properties.Resources.doc_text_image });
			dataGridViewRecords.Columns["ID"].Visible = false;
			dataGridViewRecords.Columns["Назва"].Width = 300;
			dataGridViewRecords.Columns["Виробник"].Width = 100;

			dataGridViewRecords.Columns["ВидНоменклатури"].Width = 100;
			dataGridViewRecords.Columns["ВидНоменклатури"].HeaderText = "Вид";

			dataGridViewRecords.Columns["ОдиницяВиміру"].Width = 50;
			dataGridViewRecords.Columns["ОдиницяВиміру"].HeaderText = "Од.";

			dataGridViewRecords.Columns["ТипНоменклатури"].Width = 50;
			dataGridViewRecords.Columns["ТипНоменклатури"].HeaderText = "Тип";

			LoadRecords();
		}

		private BindingList<Записи> RecordsBindingList { get; set; }

		public void LoadRecords()
		{
			int selectRow = dataGridViewRecords.SelectedRows.Count > 0 ?
				dataGridViewRecords.SelectedRows[dataGridViewRecords.SelectedRows.Count - 1].Index : 0;

			RecordsBindingList.Clear();

			Довідники.Номенклатура_Select номенклатура_Select = new Довідники.Номенклатура_Select();
			номенклатура_Select.QuerySelect.Field.Add(Довідники.Номенклатура_Select.Назва);
			номенклатура_Select.QuerySelect.Field.Add(Довідники.Номенклатура_Select.ТипНоменклатури);

			//JOIN 1
			string JoinTable = Конфа.Config.Kernel.Conf.Directories["Виробники"].Table;
			string ParentField = JoinTable + "." + Конфа.Config.Kernel.Conf.Directories["Виробники"].Fields["Назва"].NameInTable;

			номенклатура_Select.QuerySelect.FieldAndAlias.Add(new KeyValuePair<string, string>(ParentField, "join1"));
			номенклатура_Select.QuerySelect.Joins.Add(new Join(JoinTable, Довідники.Номенклатура_Select.Виробник, номенклатура_Select.QuerySelect.Table));

			//JOIN 2
			JoinTable = Конфа.Config.Kernel.Conf.Directories["ВидиНоменклатури"].Table;
			ParentField = JoinTable + "." + Конфа.Config.Kernel.Conf.Directories["ВидиНоменклатури"].Fields["Назва"].NameInTable;

			номенклатура_Select.QuerySelect.FieldAndAlias.Add(new KeyValuePair<string, string>(ParentField, "join2"));
			номенклатура_Select.QuerySelect.Joins.Add(new Join(JoinTable, Довідники.Номенклатура_Select.ВидНоменклатури, номенклатура_Select.QuerySelect.Table));

			//JOIN 3
			JoinTable = Конфа.Config.Kernel.Conf.Directories["ПакуванняОдиниціВиміру"].Table;
			ParentField = JoinTable + "." + Конфа.Config.Kernel.Conf.Directories["ПакуванняОдиниціВиміру"].Fields["Назва"].NameInTable;

			номенклатура_Select.QuerySelect.FieldAndAlias.Add(new KeyValuePair<string, string>(ParentField, "join3"));
			номенклатура_Select.QuerySelect.Joins.Add(new Join(JoinTable, Довідники.Номенклатура_Select.ОдиницяВиміру, номенклатура_Select.QuerySelect.Table));

			//ORDER
			номенклатура_Select.QuerySelect.Order.Add(Довідники.Номенклатура_Select.Назва, SelectOrder.ASC);

			номенклатура_Select.Select();
			while (номенклатура_Select.MoveNext())
			{
				Довідники.Номенклатура_Pointer cur = номенклатура_Select.Current;

				RecordsBindingList.Add(new Записи
				{
					ID = cur.UnigueID.ToString(),
					Назва = cur.Fields[Довідники.Номенклатура_Select.Назва].ToString(),
					Виробник = cur.Fields["join1"].ToString(),
					ВидНоменклатури = cur.Fields["join2"].ToString(),
					ОдиницяВиміру = cur.Fields["join3"].ToString(),
					ТипНоменклатури = ((Перелічення.ТипиНоменклатури)cur.Fields[Довідники.Номенклатура_Select.ТипНоменклатури]).ToString()
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
			public string Виробник { get; set; }
			public string ВидНоменклатури { get; set; }
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
					DirectoryPointerItem = new Довідники.Номенклатура_Pointer(new UnigueID(Uid));
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
			Form_НоменклатураЕлемент form_НоменклатураЕлемент = new Form_НоменклатураЕлемент();
			form_НоменклатураЕлемент.IsNew = true;
			form_НоменклатураЕлемент.OwnerForm = this;
			form_НоменклатураЕлемент.ShowDialog();
        }

        private void toolStripButtonEdit_Click(object sender, EventArgs e)
        {
			if (dataGridViewRecords.SelectedRows.Count > 0)
			{
				int RowIndex = dataGridViewRecords.SelectedRows[0].Index;

				Form_НоменклатураЕлемент form_НоменклатураЕлемент = new Form_НоменклатураЕлемент();
				form_НоменклатураЕлемент.IsNew = false;
				form_НоменклатураЕлемент.OwnerForm = this;
				form_НоменклатураЕлемент.Uid = dataGridViewRecords.Rows[RowIndex].Cells[0].Value.ToString();
				form_НоменклатураЕлемент.ShowDialog();
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

                    Довідники.Номенклатура_Objest номенклатура_Objest = new Довідники.Номенклатура_Objest();
                    if (номенклатура_Objest.Read(new UnigueID(uid)))
                    {
						Довідники.Номенклатура_Objest номенклатура_Objest_Новий = номенклатура_Objest.Copy();
						номенклатура_Objest_Новий.Назва = "Копія - " + номенклатура_Objest_Новий.Назва;
						номенклатура_Objest_Новий.Save();
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

                    Довідники.Номенклатура_Objest номенклатура_Objest = new Довідники.Номенклатура_Objest();
                    if (номенклатура_Objest.Read(new UnigueID(uid)))
                    {
						номенклатура_Objest.Delete();
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
