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
    public partial class Form_ХарактеристикиНоменклатури : Form
    {
        public Form_ХарактеристикиНоменклатури()
        {
            InitializeComponent();

			dataGridViewRecords.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

			RecordsBindingList = new BindingList<Записи>();
			dataGridViewRecords.DataSource = RecordsBindingList;

			dataGridViewRecords.Columns["Image"].Width = 30;
			dataGridViewRecords.Columns["Image"].HeaderText = "";

			dataGridViewRecords.Columns["ID"].Visible = false;
			dataGridViewRecords.Columns["Назва"].Width = 500;
			dataGridViewRecords.Columns["Код"].Width = 50;
			dataGridViewRecords.Columns["Номенклатура"].Width = 300;
		}

		/// <summary>
		/// Вказівник для вибору
		/// </summary>
		public DirectoryPointer DirectoryPointerItem { get; set; }

		/// <summary>
		/// Вказівник для виділення в списку
		/// </summary>
		public DirectoryPointer SelectPointerItem { get; set; }

		/// <summary>
		/// Номенклатура власник
		/// </summary>
		public Довідники.Номенклатура_Pointer НоменклатураВласник { get; set; }

		private void Form_ХарактеристикиНоменклатури_Load(object sender, EventArgs e)
        {
			directoryControl_Номенклатура.Init(new Form_Номенклатура(), new Довідники.Номенклатура_Pointer(), ПошуковіЗапити.Номенклатура);

			if (НоменклатураВласник != null)
				directoryControl_Номенклатура.DirectoryPointerItem = НоменклатураВласник;

			directoryControl_Номенклатура.AfterSelectFunc = () =>
			{
				НоменклатураВласник = (Довідники.Номенклатура_Pointer)directoryControl_Номенклатура.DirectoryPointerItem;
				LoadRecords();

				return true;
			};

			LoadRecords();
		}

		private BindingList<Записи> RecordsBindingList { get; set; }

		public void LoadRecords()
		{
			RecordsBindingList.Clear();

			Довідники.ХарактеристикиНоменклатури_Select характеристикиНоменклатури_Select = new Довідники.ХарактеристикиНоменклатури_Select();
			характеристикиНоменклатури_Select.QuerySelect.Field.Add(Довідники.ХарактеристикиНоменклатури_Const.Назва);
			характеристикиНоменклатури_Select.QuerySelect.Field.Add(Довідники.ХарактеристикиНоменклатури_Const.Код);

			//Номенклатура
			характеристикиНоменклатури_Select.QuerySelect.FieldAndAlias.Add(
				new NameValue<string>(Довідники.Номенклатура_Const.TABLE + "." + Довідники.Номенклатура_Const.Назва, "nomenclatura"));
			характеристикиНоменклатури_Select.QuerySelect.Joins.Add(
				new Join(Довідники.Номенклатура_Const.TABLE, Довідники.ХарактеристикиНоменклатури_Const.Номенклатура, характеристикиНоменклатури_Select.QuerySelect.Table));

			//Відбір по номенклатурі
			if (НоменклатураВласник != null && !НоменклатураВласник.IsEmpty())
				характеристикиНоменклатури_Select.QuerySelect.Where.Add(
					new Where(Довідники.ХарактеристикиНоменклатури_Const.Номенклатура, Comparison.EQ, НоменклатураВласник.UnigueID.UGuid));

			//ORDER
			характеристикиНоменклатури_Select.QuerySelect.Order.Add(Довідники.ХарактеристикиНоменклатури_Const.Назва, SelectOrder.ASC);

			характеристикиНоменклатури_Select.Select();
			while (характеристикиНоменклатури_Select.MoveNext())
			{
				Довідники.ХарактеристикиНоменклатури_Pointer cur = характеристикиНоменклатури_Select.Current;

				RecordsBindingList.Add(new Записи
				{
					ID = cur.UnigueID.ToString(),
					Назва = cur.Fields[Довідники.ХарактеристикиНоменклатури_Const.Назва].ToString(),
					Код = cur.Fields[Довідники.ХарактеристикиНоменклатури_Const.Код].ToString(),
					Номенклатура = cur.Fields["nomenclatura"].ToString()
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
			public string Номенклатура { get; set; }
		}

        private void dataGridViewRecords_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
			if (e.RowIndex >= 0 && e.RowIndex < dataGridViewRecords.RowCount)
			{
				string Uid = dataGridViewRecords.Rows[e.RowIndex].Cells["ID"].Value.ToString();

				if (DirectoryPointerItem != null)
				{
					DirectoryPointerItem = new Довідники.ХарактеристикиНоменклатури_Pointer(new UnigueID(Uid));
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
			Form_ХарактеристикиНоменклатуриЕлемент form_ХарактеристикиНоменклатуриЕлемент = new Form_ХарактеристикиНоменклатуриЕлемент();
			form_ХарактеристикиНоменклатуриЕлемент.MdiParent = this.MdiParent;
			form_ХарактеристикиНоменклатуриЕлемент.IsNew = true;
			form_ХарактеристикиНоменклатуриЕлемент.OwnerForm = this;
			form_ХарактеристикиНоменклатуриЕлемент.НоменклатураВласник = НоменклатураВласник;
			if (DirectoryPointerItem != null && this.MdiParent == null)
				form_ХарактеристикиНоменклатуриЕлемент.ShowDialog();
			else
				form_ХарактеристикиНоменклатуриЕлемент.Show();
		}

        private void toolStripButtonEdit_Click(object sender, EventArgs e)
        {
			if (dataGridViewRecords.SelectedRows.Count > 0)
			{
				int RowIndex = dataGridViewRecords.SelectedRows[0].Index;

				Form_ХарактеристикиНоменклатуриЕлемент form_ХарактеристикиНоменклатуриЕлемент = new Form_ХарактеристикиНоменклатуриЕлемент();
				form_ХарактеристикиНоменклатуриЕлемент.MdiParent = this.MdiParent;
				form_ХарактеристикиНоменклатуриЕлемент.IsNew = false;
				form_ХарактеристикиНоменклатуриЕлемент.OwnerForm = this;
				form_ХарактеристикиНоменклатуриЕлемент.Uid = dataGridViewRecords.Rows[RowIndex].Cells["ID"].Value.ToString();
				if (DirectoryPointerItem != null && this.MdiParent == null)
					form_ХарактеристикиНоменклатуриЕлемент.ShowDialog();
				else
					form_ХарактеристикиНоменклатуриЕлемент.Show();
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

                    Довідники.ХарактеристикиНоменклатури_Objest характеристикиНоменклатури_Objest = new Довідники.ХарактеристикиНоменклатури_Objest();
                    if (характеристикиНоменклатури_Objest.Read(new UnigueID(uid)))
                    {
						Довідники.ХарактеристикиНоменклатури_Objest характеристикиНоменклатури_Objest_Новий = характеристикиНоменклатури_Objest.Copy();
						характеристикиНоменклатури_Objest_Новий.Назва = "Копія - " + характеристикиНоменклатури_Objest_Новий.Назва;
						характеристикиНоменклатури_Objest_Новий.Код = (++Константи.НумераціяДовідників.ХарактеристикиНоменклатури_Const).ToString("D6");
						характеристикиНоменклатури_Objest_Новий.Save();

						SelectPointerItem = характеристикиНоменклатури_Objest_Новий.GetDirectoryPointer();
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

                    Довідники.ХарактеристикиНоменклатури_Objest характеристикиНоменклатури_Objest = new Довідники.ХарактеристикиНоменклатури_Objest();
                    if (характеристикиНоменклатури_Objest.Read(new UnigueID(uid)))
                    {
						характеристикиНоменклатури_Objest.Delete();
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

				SelectPointerItem = new Довідники.ХарактеристикиНоменклатури_Pointer(new UnigueID(dataGridViewRecords.Rows[e.RowIndex].Cells["ID"].Value.ToString()));
			}
		}
    }
}
