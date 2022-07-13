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
    public partial class Form_ПоступленняТоварівТаПослугЖурнал : Form
    {
        public Form_ПоступленняТоварівТаПослугЖурнал()
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
			dataGridViewRecords.Columns["Контрагент"].Width = 300;

			dataGridViewRecords.Columns["Сума"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
			dataGridViewRecords.Columns["Сума"].CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
			dataGridViewRecords.Columns["Сума"].Width = 100;

			dataGridViewRecords.Columns["Коментар"].Width = 400;

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

			Документи.ПоступленняТоварівТаПослуг_Select поступленняТоварівТаПослуг_Select = new Документи.ПоступленняТоварівТаПослуг_Select();
			поступленняТоварівТаПослуг_Select.QuerySelect.Field.Add("spend");
			поступленняТоварівТаПослуг_Select.QuerySelect.Field.Add(Документи.ПоступленняТоварівТаПослуг_Const.Назва);
			поступленняТоварівТаПослуг_Select.QuerySelect.Field.Add(Документи.ПоступленняТоварівТаПослуг_Const.НомерДок);
			поступленняТоварівТаПослуг_Select.QuerySelect.Field.Add(Документи.ПоступленняТоварівТаПослуг_Const.ДатаДок);
			поступленняТоварівТаПослуг_Select.QuerySelect.Field.Add(Документи.ПоступленняТоварівТаПослуг_Const.СумаДокументу);
			поступленняТоварівТаПослуг_Select.QuerySelect.Field.Add(Документи.ПоступленняТоварівТаПослуг_Const.Коментар);

			//Контрагент
			поступленняТоварівТаПослуг_Select.QuerySelect.FieldAndAlias.Add(
				new KeyValuePair<string, string>(Довідники.Контрагенти_Const.TABLE + "." + Довідники.Контрагенти_Const.Назва, "joinContragent"));
			поступленняТоварівТаПослуг_Select.QuerySelect.Joins.Add(
				new Join(Довідники.Контрагенти_Const.TABLE, Документи.ПоступленняТоварівТаПослуг_Const.Контрагент, Документи.ПоступленняТоварівТаПослуг_Const.TABLE));

			//ORDER
			поступленняТоварівТаПослуг_Select.QuerySelect.Order.Add(Документи.ПоступленняТоварівТаПослуг_Const.ДатаДок, SelectOrder.ASC);
			поступленняТоварівТаПослуг_Select.QuerySelect.Order.Add(Документи.ПоступленняТоварівТаПослуг_Const.НомерДок, SelectOrder.ASC);

			поступленняТоварівТаПослуг_Select.Select();
			while (поступленняТоварівТаПослуг_Select.MoveNext())
			{
				Документи.ПоступленняТоварівТаПослуг_Pointer cur = поступленняТоварівТаПослуг_Select.Current;

				RecordsBindingList.Add(new Записи
				{
					ID = cur.UnigueID.ToString(),
					Назва = cur.Fields[Документи.ПоступленняТоварівТаПослуг_Const.Назва].ToString(),
					НомерДок = cur.Fields[Документи.ПоступленняТоварівТаПослуг_Const.НомерДок].ToString(),
					ДатаДок = cur.Fields[Документи.ПоступленняТоварівТаПослуг_Const.ДатаДок].ToString(),
					Контрагент = cur.Fields["joinContragent"].ToString(),
					Сума = Math.Round((decimal)cur.Fields[Документи.ПоступленняТоварівТаПослуг_Const.СумаДокументу], 2),
					Коментар = cur.Fields[Документи.ПоступленняТоварівТаПослуг_Const.Коментар].ToString(),
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
			public string Контрагент { get; set; }
			public decimal Сума { get; set; }
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
					DocumentPointerItem = new Документи.ПоступленняТоварівТаПослуг_Pointer(new UnigueID(Uid));
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
			Form_ПоступленняТоварівТаПослугДокумент form_ПоступленняТоварівТаПослугДокумент = new Form_ПоступленняТоварівТаПослугДокумент();
			form_ПоступленняТоварівТаПослугДокумент.MdiParent = this.MdiParent;
			form_ПоступленняТоварівТаПослугДокумент.IsNew = true;
			form_ПоступленняТоварівТаПослугДокумент.OwnerForm = this;
			form_ПоступленняТоварівТаПослугДокумент.Show();
        }

        private void toolStripButtonEdit_Click(object sender, EventArgs e)
        {
			if (dataGridViewRecords.SelectedRows.Count > 0)
			{
				int RowIndex = dataGridViewRecords.SelectedRows[0].Index;

				Form_ПоступленняТоварівТаПослугДокумент form_ПоступленняТоварівТаПослугДокумент = new Form_ПоступленняТоварівТаПослугДокумент();
				form_ПоступленняТоварівТаПослугДокумент.MdiParent = this.MdiParent;
				form_ПоступленняТоварівТаПослугДокумент.IsNew = false;
				form_ПоступленняТоварівТаПослугДокумент.OwnerForm = this;
				form_ПоступленняТоварівТаПослугДокумент.Uid = dataGridViewRecords.Rows[RowIndex].Cells["ID"].Value.ToString();
				form_ПоступленняТоварівТаПослугДокумент.Show();
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

                    Документи.ПоступленняТоварівТаПослуг_Objest поступленняТоварівТаПослуг_Objest = new Документи.ПоступленняТоварівТаПослуг_Objest();
                    if (поступленняТоварівТаПослуг_Objest.Read(new UnigueID(uid)))
                    {
						Документи.ПоступленняТоварівТаПослуг_Objest поступленняТоварівТаПослуг_Objest_Новий = поступленняТоварівТаПослуг_Objest.Copy();
						поступленняТоварівТаПослуг_Objest_Новий.Назва += " *";
						поступленняТоварівТаПослуг_Objest_Новий.ДатаДок = DateTime.Now;
						поступленняТоварівТаПослуг_Objest_Новий.НомерДок = (++Константи.НумераціяДокументів.ПоступленняТоварівТаПослуг_Const).ToString("D8");

						//Зчитати та скопіювати табличну частину Товари
						поступленняТоварівТаПослуг_Objest.Товари_TablePart.Read();
						поступленняТоварівТаПослуг_Objest_Новий.Товари_TablePart.Records = поступленняТоварівТаПослуг_Objest.Товари_TablePart.Copy();
						поступленняТоварівТаПослуг_Objest_Новий.Товари_TablePart.Save(true);
						поступленняТоварівТаПослуг_Objest_Новий.Save();
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

                    Документи.ПоступленняТоварівТаПослуг_Objest поступленняТоварівТаПослуг_Objest = new Документи.ПоступленняТоварівТаПослуг_Objest();
                    if (поступленняТоварівТаПослуг_Objest.Read(new UnigueID(uid)))
                    {
						поступленняТоварівТаПослуг_Objest.Delete();
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

				Звіти.РухДокументівПоРегістрах.PrintRecords(new Документи.ПоступленняТоварівТаПослуг_Pointer(new UnigueID(uid)));
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

					Документи.ПоступленняТоварівТаПослуг_Pointer поступленняТоварівТаПослуг_Pointer = new Документи.ПоступленняТоварівТаПослуг_Pointer(new UnigueID(uid));
					Документи.ПоступленняТоварівТаПослуг_Objest поступленняТоварівТаПослуг_Objest = поступленняТоварівТаПослуг_Pointer.GetDocumentObject(true);

					//Очищення регістрів
					поступленняТоварівТаПослуг_Objest.ClearSpendTheDocument();

					if (spend)
						try
						{
							//Проведення
							поступленняТоварівТаПослуг_Objest.SpendTheDocument();
						}
						catch (Exception exp)
						{
							поступленняТоварівТаПослуг_Objest.ClearSpendTheDocument();
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
