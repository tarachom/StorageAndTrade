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
using Документи = StorageAndTrade_1_0.Документи;
using Перелічення = StorageAndTrade_1_0.Перелічення;
using Звіти = StorageAndTrade_1_0.Звіти;

namespace StorageAndTrade
{
    public partial class Form_ПереміщенняТоварівЖурнал : Form
    {
        public Form_ПереміщенняТоварівЖурнал()
        {
            InitializeComponent();

			dataGridViewRecords.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

			RecordsBindingList = new BindingList<Записи>();
			dataGridViewRecords.DataSource = RecordsBindingList;

			dataGridViewRecords.Columns["Image"].Width = 30;
			dataGridViewRecords.Columns["Image"].HeaderText = "";

			dataGridViewRecords.Columns["ID"].Visible = false;

			dataGridViewRecords.Columns["НомерДок"].Width = 100;
			dataGridViewRecords.Columns["НомерДок"].HeaderText = "Номер";
			dataGridViewRecords.Columns["НомерДок"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

			dataGridViewRecords.Columns["ДатаДок"].Width = 120;
			dataGridViewRecords.Columns["ДатаДок"].HeaderText = "Дата";
			dataGridViewRecords.Columns["ДатаДок"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

			dataGridViewRecords.Columns["Назва"].Width = 350;

			dataGridViewRecords.Columns["СкладВідправник"].Width = 250;
			dataGridViewRecords.Columns["СкладОдержувач"].Width = 250;

			dataGridViewRecords.Columns["Коментар"].Width = 350;

			dataGridViewRecords.Columns["Проведений"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
			dataGridViewRecords.Columns["Проведений"].Width = 80;
		}

		public DocumentPointer DocumentPointerItem { get; set; }

		private void FormCash_Load(object sender, EventArgs e)
        {
			LoadRecords();
		}

		private BindingList<Записи> RecordsBindingList { get; set; }

		public void LoadRecords()
		{
			int selectRow = dataGridViewRecords.SelectedRows.Count > 0 ?
				dataGridViewRecords.SelectedRows[dataGridViewRecords.SelectedRows.Count - 1].Index : 0;

			RecordsBindingList.Clear();

			Документи.ПереміщенняТоварів_Select переміщенняТоварів_Select = new Документи.ПереміщенняТоварів_Select();
			переміщенняТоварів_Select.QuerySelect.Field.Add("spend");
			переміщенняТоварів_Select.QuerySelect.Field.Add(Документи.ПереміщенняТоварів_Const.Назва);
			переміщенняТоварів_Select.QuerySelect.Field.Add(Документи.ПереміщенняТоварів_Const.НомерДок);
			переміщенняТоварів_Select.QuerySelect.Field.Add(Документи.ПереміщенняТоварів_Const.ДатаДок);
			переміщенняТоварів_Select.QuerySelect.Field.Add(Документи.ПереміщенняТоварів_Const.Коментар);

			//СкладВідправник
			переміщенняТоварів_Select.QuerySelect.FieldAndAlias.Add(
				new KeyValuePair<string, string>(Довідники.Склади_Const.TABLE + "." + Довідники.Склади_Const.Назва, "sklad_sender"));
			переміщенняТоварів_Select.QuerySelect.Joins.Add(
				new Join(Довідники.Склади_Const.TABLE, Документи.ПереміщенняТоварів_Const.СкладВідправник, Документи.ПереміщенняТоварів_Const.TABLE));

			//СкладОдержувач
			переміщенняТоварів_Select.QuerySelect.FieldAndAlias.Add(
				new KeyValuePair<string, string>("sklad2." + Довідники.Склади_Const.Назва, "sklad_receiver"));
			переміщенняТоварів_Select.QuerySelect.Joins.Add(
				new Join(Довідники.Склади_Const.TABLE, Документи.ПереміщенняТоварів_Const.СкладОтримувач, Документи.ПереміщенняТоварів_Const.TABLE, "sklad2"));

			//ORDER
			переміщенняТоварів_Select.QuerySelect.Order.Add(Документи.ПереміщенняТоварів_Const.ДатаДок, SelectOrder.ASC);
			переміщенняТоварів_Select.QuerySelect.Order.Add(Документи.ПереміщенняТоварів_Const.НомерДок, SelectOrder.ASC);

			переміщенняТоварів_Select.Select();
			while (переміщенняТоварів_Select.MoveNext())
			{
				Документи.ПереміщенняТоварів_Pointer cur = переміщенняТоварів_Select.Current;

				RecordsBindingList.Add(new Записи
				{
					ID = cur.UnigueID.ToString(),
					Назва = cur.Fields[Документи.ПереміщенняТоварів_Const.Назва].ToString(),
					НомерДок = cur.Fields[Документи.ПереміщенняТоварів_Const.НомерДок].ToString(),
					ДатаДок = cur.Fields[Документи.ПереміщенняТоварів_Const.ДатаДок].ToString(),
					СкладВідправник = cur.Fields["sklad_sender"].ToString(),
					СкладОдержувач = cur.Fields["sklad_receiver"].ToString(),
					Коментар = cur.Fields[Документи.ПереміщенняТоварів_Const.Коментар].ToString(),
					Проведений = (bool)cur.Fields["spend"]
				});

                if (DocumentPointerItem != null)
                    if (cur.UnigueID.ToString() == DocumentPointerItem.UnigueID.ToString())
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
			public string НомерДок { get; set; }
			public string ДатаДок { get; set; }
			public string СкладВідправник { get; set; }
			public string СкладОдержувач { get; set; }
			public string Коментар { get; set; }
			public bool Проведений { get; set; }
		}

        private void dataGridViewRecords_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
			if (e.RowIndex >= 0 && e.RowIndex < dataGridViewRecords.RowCount)
			{
				string Uid = dataGridViewRecords.Rows[e.RowIndex].Cells["ID"].Value.ToString();

                if (DocumentPointerItem != null)
                {
					DocumentPointerItem = new Документи.ПереміщенняТоварів_Pointer(new UnigueID(Uid));
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
			Form_ПереміщенняТоварівДокумент form_ПереміщенняТоварівДокумент = new Form_ПереміщенняТоварівДокумент();
			form_ПереміщенняТоварівДокумент.MdiParent = this.MdiParent;
			form_ПереміщенняТоварівДокумент.IsNew = true;
			form_ПереміщенняТоварівДокумент.OwnerForm = this;
			form_ПереміщенняТоварівДокумент.Show();
        }

        private void toolStripButtonEdit_Click(object sender, EventArgs e)
        {
			if (dataGridViewRecords.SelectedRows.Count > 0)
			{
				int RowIndex = dataGridViewRecords.SelectedRows[0].Index;

				Form_ПереміщенняТоварівДокумент form_ПереміщенняТоварівДокумент = new Form_ПереміщенняТоварівДокумент();
				form_ПереміщенняТоварівДокумент.MdiParent = this.MdiParent;
				form_ПереміщенняТоварівДокумент.IsNew = false;
				form_ПереміщенняТоварівДокумент.OwnerForm = this;
				form_ПереміщенняТоварівДокумент.Uid = dataGridViewRecords.Rows[RowIndex].Cells["ID"].Value.ToString();
				form_ПереміщенняТоварівДокумент.Show();
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

                    Документи.ПереміщенняТоварів_Objest переміщенняТоварів_Objest = new Документи.ПереміщенняТоварів_Objest();
                    if (переміщенняТоварів_Objest.Read(new UnigueID(uid)))
                    {
						Документи.ПереміщенняТоварів_Objest переміщенняТоварів_Objest_Новий = переміщенняТоварів_Objest.Copy();
						переміщенняТоварів_Objest_Новий.Назва += " *";
						переміщенняТоварів_Objest_Новий.ДатаДок = DateTime.Now;
						переміщенняТоварів_Objest_Новий.НомерДок = (++Константи.НумераціяДокументів.ПереміщенняТоварів_Const).ToString("D8");

						//Зчитати та скопіювати табличну частину Товари
						переміщенняТоварів_Objest.Товари_TablePart.Read();
						переміщенняТоварів_Objest_Новий.Товари_TablePart.Records = переміщенняТоварів_Objest.Товари_TablePart.Copy();
						переміщенняТоварів_Objest_Новий.Товари_TablePart.Save(true);
						переміщенняТоварів_Objest_Новий.Save();
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

                    Документи.ПереміщенняТоварів_Objest переміщенняТоварів_Objest = new Документи.ПереміщенняТоварів_Objest();
                    if (переміщенняТоварів_Objest.Read(new UnigueID(uid)))
                    {
						переміщенняТоварів_Objest.Delete();
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

        private void toolStripButtonДрукПроводок_Click(object sender, EventArgs e)
        {
			if (dataGridViewRecords.SelectedRows.Count > 0)
			{
				int RowIndex = dataGridViewRecords.SelectedRows[0].Index;
				string uid = dataGridViewRecords.Rows[RowIndex].Cells["ID"].Value.ToString();

				Звіти.РухДокументівПоРегістрах.PrintRecords(new Документи.ПереміщенняТоварів_Pointer(new UnigueID(uid)));
			}
		}

		private void SpendDocuments(bool spend, string message)
		{
			if (dataGridViewRecords.SelectedRows.Count != 0 &&
				MessageBox.Show(message, "Повідомлення", MessageBoxButtons.YesNo) == DialogResult.Yes)
			{
				for (int i = 0; i < dataGridViewRecords.SelectedRows.Count; i++)
				{
					DataGridViewRow row = dataGridViewRecords.SelectedRows[i];
					string uid = row.Cells["ID"].Value.ToString();

					Документи.ПереміщенняТоварів_Pointer переміщенняТоварів_Pointer = new Документи.ПереміщенняТоварів_Pointer(new UnigueID(uid));
					Документи.ПереміщенняТоварів_Objest переміщенняТоварів_Objest = переміщенняТоварів_Pointer.GetDocumentObject(true);

					//Очищення регістрів
					переміщенняТоварів_Objest.ClearSpendTheDocument();

					if (spend)
						try
						{
							//Проведення
							переміщенняТоварів_Objest.SpendTheDocument();
						}
						catch (Exception exp)
						{
							переміщенняТоварів_Objest.ClearSpendTheDocument();
							MessageBox.Show(exp.Message);
							return;
						}
				}

				LoadRecords();
			}
		}

		private void toolStripButtonSpend_Click(object sender, EventArgs e)
		{
			SpendDocuments(true, "Провести?");
		}

		private void toolStripButtonClearSpend_Click(object sender, EventArgs e)
		{
			SpendDocuments(false, "Відмінити проведення?");
		}
	}
}
