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
    public partial class Form_РозхіднийКасовийОрдерЖурнал : Form
    {
        public Form_РозхіднийКасовийОрдерЖурнал()
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

			Документи.РозхіднийКасовийОрдер_Select розхіднийКасовийОрдер_Select = new Документи.РозхіднийКасовийОрдер_Select();
			розхіднийКасовийОрдер_Select.QuerySelect.Field.Add("spend");
			розхіднийКасовийОрдер_Select.QuerySelect.Field.Add(Документи.РозхіднийКасовийОрдер_Const.Назва);
			розхіднийКасовийОрдер_Select.QuerySelect.Field.Add(Документи.РозхіднийКасовийОрдер_Const.НомерДок);
			розхіднийКасовийОрдер_Select.QuerySelect.Field.Add(Документи.РозхіднийКасовийОрдер_Const.ДатаДок);
			розхіднийКасовийОрдер_Select.QuerySelect.Field.Add(Документи.РозхіднийКасовийОрдер_Const.СумаДокументу);

			//Контрагент
			розхіднийКасовийОрдер_Select.QuerySelect.FieldAndAlias.Add(
				new KeyValuePair<string, string>(Довідники.Контрагенти_Const.TABLE + "." + Довідники.Контрагенти_Const.Назва, "joinContragent"));
			розхіднийКасовийОрдер_Select.QuerySelect.Joins.Add(
				new Join(Довідники.Контрагенти_Const.TABLE, Документи.РозхіднийКасовийОрдер_Const.Контрагент, Документи.РозхіднийКасовийОрдер_Const.TABLE));

			//ORDER
			розхіднийКасовийОрдер_Select.QuerySelect.Order.Add(Документи.РозхіднийКасовийОрдер_Const.ДатаДок, SelectOrder.ASC);
			розхіднийКасовийОрдер_Select.QuerySelect.Order.Add(Документи.РозхіднийКасовийОрдер_Const.НомерДок, SelectOrder.ASC);

			розхіднийКасовийОрдер_Select.Select();
			while (розхіднийКасовийОрдер_Select.MoveNext())
			{
				Документи.РозхіднийКасовийОрдер_Pointer cur = розхіднийКасовийОрдер_Select.Current;

				RecordsBindingList.Add(new Записи
				{
					ID = cur.UnigueID.ToString(),
					Назва = cur.Fields[Документи.РозхіднийКасовийОрдер_Const.Назва].ToString(),
					НомерДок = cur.Fields[Документи.РозхіднийКасовийОрдер_Const.НомерДок].ToString(),
					ДатаДок = cur.Fields[Документи.РозхіднийКасовийОрдер_Const.ДатаДок].ToString(),
					Контрагент = cur.Fields["joinContragent"].ToString(),
					Сума = Math.Round((decimal)cur.Fields[Документи.РозхіднийКасовийОрдер_Const.СумаДокументу], 2),
					Проведений = (bool)cur.Fields["spend"]
				});

				if (DocumentPointerItem != null && selectRow == 0) 
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
			public bool Проведений { get; set; }
		}

        private void dataGridViewRecords_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
			if (e.RowIndex >= 0 && e.RowIndex < dataGridViewRecords.RowCount)
			{
				string Uid = dataGridViewRecords.Rows[e.RowIndex].Cells["ID"].Value.ToString();

                if (DocumentPointerItem != null)
                {
					DocumentPointerItem = new Документи.РозхіднийКасовийОрдер_Pointer(new UnigueID(Uid));
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
			Form_РозхіднийКасовийОрдерДокумент form_РозхіднийКасовийОрдерДокумент = new Form_РозхіднийКасовийОрдерДокумент();
			form_РозхіднийКасовийОрдерДокумент.MdiParent = this.MdiParent;
			form_РозхіднийКасовийОрдерДокумент.IsNew = true;
			form_РозхіднийКасовийОрдерДокумент.OwnerForm = this;
			form_РозхіднийКасовийОрдерДокумент.Show();
        }

        private void toolStripButtonEdit_Click(object sender, EventArgs e)
        {
			if (dataGridViewRecords.SelectedRows.Count > 0)
			{
				int RowIndex = dataGridViewRecords.SelectedRows[0].Index;

				Form_РозхіднийКасовийОрдерДокумент form_РозхіднийКасовийОрдерДокумент = new Form_РозхіднийКасовийОрдерДокумент();
				form_РозхіднийКасовийОрдерДокумент.MdiParent = this.MdiParent;
				form_РозхіднийКасовийОрдерДокумент.IsNew = false;
				form_РозхіднийКасовийОрдерДокумент.OwnerForm = this;
				form_РозхіднийКасовийОрдерДокумент.Uid = dataGridViewRecords.Rows[RowIndex].Cells["ID"].Value.ToString();
				form_РозхіднийКасовийОрдерДокумент.Show();
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

					Документи.РозхіднийКасовийОрдер_Objest розхіднийКасовийОрдер_Objest = new Документи.РозхіднийКасовийОрдер_Objest();
                    if (розхіднийКасовийОрдер_Objest.Read(new UnigueID(uid)))
                    {
						Документи.РозхіднийКасовийОрдер_Objest розхіднийКасовийОрдер_Objest_Новий = розхіднийКасовийОрдер_Objest.Copy();
						розхіднийКасовийОрдер_Objest_Новий.Назва += " *";
						розхіднийКасовийОрдер_Objest_Новий.ДатаДок = DateTime.Now;
						розхіднийКасовийОрдер_Objest_Новий.НомерДок = (++Константи.НумераціяДокументів.ПрихіднийКасовийОрдер_Const).ToString("D8");

						//Зчитати та скопіювати табличну частину
						розхіднийКасовийОрдер_Objest.РозшифруванняПлатежу_TablePart.Read();
						розхіднийКасовийОрдер_Objest_Новий.РозшифруванняПлатежу_TablePart.Records = розхіднийКасовийОрдер_Objest.РозшифруванняПлатежу_TablePart.Copy();
						розхіднийКасовийОрдер_Objest_Новий.РозшифруванняПлатежу_TablePart.Save(true);
						розхіднийКасовийОрдер_Objest_Новий.Save();
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

					Документи.РозхіднийКасовийОрдер_Objest розхіднийКасовийОрдер_Objest = new Документи.РозхіднийКасовийОрдер_Objest();
					if (розхіднийКасовийОрдер_Objest.Read(new UnigueID(uid)))
                    {
						розхіднийКасовийОрдер_Objest.Delete();
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

				Звіти.РухПоРугістрах.PrintRecords(new Документи.РозхіднийКасовийОрдер_Pointer(new UnigueID(uid)));
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

					Документи.РозхіднийКасовийОрдер_Pointer розхіднийКасовийОрдер_Pointer = new Документи.РозхіднийКасовийОрдер_Pointer(new UnigueID(uid));
					Документи.РозхіднийКасовийОрдер_Objest розхіднийКасовийОрдер_Objest = розхіднийКасовийОрдер_Pointer.GetDocumentObject(true);

					// Очищення регістрів
					розхіднийКасовийОрдер_Objest.ClearSpendTheDocument();

					if (spend)
						try
						{
							//Проведення
							розхіднийКасовийОрдер_Objest.SpendTheDocument();
						}
						catch (Exception exp)
						{
							розхіднийКасовийОрдер_Objest.ClearSpendTheDocument();
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
