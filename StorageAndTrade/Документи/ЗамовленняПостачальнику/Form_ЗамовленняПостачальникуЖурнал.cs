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
    public partial class Form_ЗамовленняПостачальникуЖурнал : Form
    {
        public Form_ЗамовленняПостачальникуЖурнал()
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

			dataGridViewRecords.Columns["Коментар"].Width = 350;

			dataGridViewRecords.Columns["Проведений"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
			dataGridViewRecords.Columns["Проведений"].Width = 80;
		}

		public DocumentPointer DocumentPointerItem { get; set; }

		private void Form_ЗамовленняПостачальникуЖурнал_Load(object sender, EventArgs e)
        {
			LoadRecords();
		}

		private BindingList<Записи> RecordsBindingList { get; set; }

		public void LoadRecords()
		{
			int selectRow = dataGridViewRecords.SelectedRows.Count > 0 ?
				dataGridViewRecords.SelectedRows[dataGridViewRecords.SelectedRows.Count - 1].Index : 0;

			RecordsBindingList.Clear();

			Документи.ЗамовленняПостачальнику_Select замовленняПостачальнику_Select = new Документи.ЗамовленняПостачальнику_Select();
			замовленняПостачальнику_Select.QuerySelect.Field.AddRange(new string[] {
				"spend",
				Документи.ЗамовленняПостачальнику_Const.Назва,
				Документи.ЗамовленняПостачальнику_Const.НомерДок,
				Документи.ЗамовленняПостачальнику_Const.ДатаДок,
				Документи.ЗамовленняПостачальнику_Const.СумаДокументу,
				Документи.ЗамовленняПостачальнику_Const.Коментар
			});

			//Контрагент
			замовленняПостачальнику_Select.QuerySelect.FieldAndAlias.Add(
				new NameValue<string>(Довідники.Контрагенти_Const.TABLE + "." + Довідники.Контрагенти_Const.Назва, "joinContragent"));
			замовленняПостачальнику_Select.QuerySelect.Joins.Add(
				new Join(Довідники.Контрагенти_Const.TABLE, Документи.ЗамовленняПостачальнику_Const.Контрагент, Документи.ЗамовленняПостачальнику_Const.TABLE));

			//ORDER
			замовленняПостачальнику_Select.QuerySelect.Order.Add(Документи.ЗамовленняПостачальнику_Const.ДатаДок, SelectOrder.ASC);
			замовленняПостачальнику_Select.QuerySelect.Order.Add(Документи.ЗамовленняПостачальнику_Const.НомерДок, SelectOrder.ASC);

			замовленняПостачальнику_Select.Select();
			while (замовленняПостачальнику_Select.MoveNext())
			{
				Документи.ЗамовленняПостачальнику_Pointer cur = замовленняПостачальнику_Select.Current;

				RecordsBindingList.Add(new Записи
				{
					ID = cur.UnigueID.ToString(),
					Назва = cur.Fields[Документи.ЗамовленняПостачальнику_Const.Назва].ToString(),
					НомерДок = cur.Fields[Документи.ЗамовленняПостачальнику_Const.НомерДок].ToString(),
					ДатаДок = cur.Fields[Документи.ЗамовленняПостачальнику_Const.ДатаДок].ToString(),
					Контрагент = cur.Fields["joinContragent"].ToString(),
					Сума = Math.Round((decimal)cur.Fields[Документи.ЗамовленняПостачальнику_Const.СумаДокументу], 2),
					Коментар = cur.Fields[Документи.ЗамовленняПостачальнику_Const.Коментар].ToString(),
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
					DocumentPointerItem = new Документи.ЗамовленняПостачальнику_Pointer(new UnigueID(Uid));
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
			Form_ЗамовленняПостачальникуДокумент form_ЗамовленняПостачальникуДокумент = new Form_ЗамовленняПостачальникуДокумент();
			form_ЗамовленняПостачальникуДокумент.MdiParent = this.MdiParent;
			form_ЗамовленняПостачальникуДокумент.IsNew = true;
			form_ЗамовленняПостачальникуДокумент.OwnerForm = this;
			form_ЗамовленняПостачальникуДокумент.Show();
        }

        private void toolStripButtonEdit_Click(object sender, EventArgs e)
        {
			if (dataGridViewRecords.SelectedRows.Count > 0)
			{
				int RowIndex = dataGridViewRecords.SelectedRows[0].Index;

				Form_ЗамовленняПостачальникуДокумент form_ЗамовленняПостачальникуДокумент = new Form_ЗамовленняПостачальникуДокумент();
				form_ЗамовленняПостачальникуДокумент.MdiParent = this.MdiParent;
				form_ЗамовленняПостачальникуДокумент.IsNew = false;
				form_ЗамовленняПостачальникуДокумент.OwnerForm = this;
				form_ЗамовленняПостачальникуДокумент.Uid = dataGridViewRecords.Rows[RowIndex].Cells["ID"].Value.ToString();
				form_ЗамовленняПостачальникуДокумент.Show();
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

                    Документи.ЗамовленняПостачальнику_Objest замовленняПостачальнику_Objest = new Документи.ЗамовленняПостачальнику_Objest();
                    if (замовленняПостачальнику_Objest.Read(new UnigueID(uid)))
                    {
						Документи.ЗамовленняПостачальнику_Objest замовленняПостачальнику_Objest_Новий = замовленняПостачальнику_Objest.Copy();
						замовленняПостачальнику_Objest_Новий.Назва += " *";
						замовленняПостачальнику_Objest_Новий.ДатаДок = DateTime.Now;
						замовленняПостачальнику_Objest_Новий.НомерДок = (++Константи.НумераціяДокументів.ЗамовленняПостачальнику_Const).ToString("D8");

						//Зчитати та скопіювати табличну частину Товари
						замовленняПостачальнику_Objest.Товари_TablePart.Read();
						замовленняПостачальнику_Objest_Новий.Товари_TablePart.Records = замовленняПостачальнику_Objest.Товари_TablePart.Copy();
						замовленняПостачальнику_Objest_Новий.Товари_TablePart.Save(true);
						замовленняПостачальнику_Objest_Новий.Save();
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

                    Документи.ЗамовленняПостачальнику_Objest замовленняПостачальнику_Objest = new Документи.ЗамовленняПостачальнику_Objest();
                    if (замовленняПостачальнику_Objest.Read(new UnigueID(uid)))
                    {
						замовленняПостачальнику_Objest.Delete();
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

				Звіти.РухДокументівПоРегістрах.PrintRecords(new Документи.ЗамовленняПостачальнику_Pointer(new UnigueID(uid)));
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

					Документи.ЗамовленняПостачальнику_Pointer замовленняПостачальнику_Pointer = new Документи.ЗамовленняПостачальнику_Pointer(new UnigueID(uid));
					Документи.ЗамовленняПостачальнику_Objest замовленняПостачальнику_Objest = замовленняПостачальнику_Pointer.GetDocumentObject(true);

					if (spend)
						try
						{
							//Проведення
							замовленняПостачальнику_Objest.SpendTheDocument(замовленняПостачальнику_Objest.ДатаДок);
						}
						catch (Exception exp)
						{
							замовленняПостачальнику_Objest.ClearSpendTheDocument();
							MessageBox.Show(exp.Message);
							return;
						}
					else
						замовленняПостачальнику_Objest.ClearSpendTheDocument();
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

		#region Ввести на основі

		private void ПоступленняТоварівТаПослугToolStripMenuItem_Click(object sender, EventArgs e)
        {
			if (dataGridViewRecords.SelectedRows.Count > 0)
			{
				DataGridViewRow row = dataGridViewRecords.SelectedRows[0];
				string uid = row.Cells["ID"].Value.ToString();

				Документи.ЗамовленняПостачальнику_Pointer замовленняПостачальнику_Pointer = new Документи.ЗамовленняПостачальнику_Pointer(new UnigueID(uid));
				Документи.ЗамовленняПостачальнику_Objest замовленняПостачальнику_Objest = замовленняПостачальнику_Pointer.GetDocumentObject(true);

				//
				//Новий документ
				//

				Документи.ПоступленняТоварівТаПослуг_Objest поступленняТоварівТаПослуг_Новий = new Документи.ПоступленняТоварівТаПослуг_Objest();
				поступленняТоварівТаПослуг_Новий.New();
				поступленняТоварівТаПослуг_Новий.ДатаДок = DateTime.Now;
				поступленняТоварівТаПослуг_Новий.НомерДок = (++Константи.НумераціяДокументів.ПоступленняТоварівТаПослуг_Const).ToString("D8");
				поступленняТоварівТаПослуг_Новий.Назва = $"Поступлення товарів та послуг №{поступленняТоварівТаПослуг_Новий.НомерДок} від {поступленняТоварівТаПослуг_Новий.ДатаДок.ToString("dd.MM.yyyy")}";
				поступленняТоварівТаПослуг_Новий.Організація = замовленняПостачальнику_Objest.Організація;
				поступленняТоварівТаПослуг_Новий.Валюта = замовленняПостачальнику_Objest.Валюта;
				поступленняТоварівТаПослуг_Новий.Каса = замовленняПостачальнику_Objest.Каса;
				поступленняТоварівТаПослуг_Новий.Контрагент = замовленняПостачальнику_Objest.Контрагент;
				поступленняТоварівТаПослуг_Новий.Договір = замовленняПостачальнику_Objest.Договір;
				поступленняТоварівТаПослуг_Новий.Склад = замовленняПостачальнику_Objest.Склад;
				//поступленняТоварівТаПослуг_Новий.Коментар = $"На основі \"{замовленняПостачальнику_Objest.Назва}\"";
				поступленняТоварівТаПослуг_Новий.СумаДокументу = замовленняПостачальнику_Objest.СумаДокументу;
				поступленняТоварівТаПослуг_Новий.ФормаОплати = замовленняПостачальнику_Objest.ФормаОплати;
				поступленняТоварівТаПослуг_Новий.ЗамовленняПостачальнику = замовленняПостачальнику_Objest.GetDocumentPointer();
				поступленняТоварівТаПослуг_Новий.Основа = new UuidAndText(замовленняПостачальнику_Objest.UnigueID.UGuid, замовленняПостачальнику_Objest.TypeDocument);
				поступленняТоварівТаПослуг_Новий.Save();

				//Товари
				foreach (Документи.ЗамовленняПостачальнику_Товари_TablePart.Record record_замовлення in замовленняПостачальнику_Objest.Товари_TablePart.Records)
				{
					Документи.ПоступленняТоварівТаПослуг_Товари_TablePart.Record record_поступлення = new Документи.ПоступленняТоварівТаПослуг_Товари_TablePart.Record();
					поступленняТоварівТаПослуг_Новий.Товари_TablePart.Records.Add(record_поступлення);

					record_поступлення.Номенклатура = record_замовлення.Номенклатура;
					record_поступлення.ХарактеристикаНоменклатури = record_замовлення.ХарактеристикаНоменклатури;
					record_поступлення.Пакування = record_замовлення.Пакування;
					record_поступлення.КількістьУпаковок = record_замовлення.КількістьУпаковок;
					record_поступлення.Кількість = record_замовлення.Кількість;
					record_поступлення.Ціна = record_замовлення.Ціна;
					record_поступлення.Сума = record_замовлення.Сума;
					record_поступлення.Скидка = record_замовлення.Скидка;
					record_поступлення.ЗамовленняПостачальнику = замовленняПостачальнику_Objest.GetDocumentPointer();
					record_поступлення.Склад = замовленняПостачальнику_Objest.Склад;
				}

				поступленняТоварівТаПослуг_Новий.Товари_TablePart.Save(false);

				//Відкрити журнал та документ
				Form form_ПоступленняТоварівТаПослугЖурнал = Application.OpenForms["Form_ПоступленняТоварівТаПослугЖурнал"];
				if (form_ПоступленняТоварівТаПослугЖурнал == null)
				{
					form_ПоступленняТоварівТаПослугЖурнал = new Form_ПоступленняТоварівТаПослугЖурнал();
					form_ПоступленняТоварівТаПослугЖурнал.MdiParent = this.MdiParent;
					form_ПоступленняТоварівТаПослугЖурнал.Show();
				}
				else
					((Form_ПоступленняТоварівТаПослугЖурнал)form_ПоступленняТоварівТаПослугЖурнал).LoadRecords();

				Form_ПоступленняТоварівТаПослугДокумент form_ПоступленняТоварівТаПослугДокумент = new Form_ПоступленняТоварівТаПослугДокумент();
				form_ПоступленняТоварівТаПослугДокумент.MdiParent = this.MdiParent;
				form_ПоступленняТоварівТаПослугДокумент.IsNew = false;
				form_ПоступленняТоварівТаПослугДокумент.OwnerForm = (Form_ПоступленняТоварівТаПослугЖурнал)form_ПоступленняТоварівТаПослугЖурнал;
				form_ПоступленняТоварівТаПослугДокумент.Uid = поступленняТоварівТаПослуг_Новий.UnigueID.ToString();
				form_ПоступленняТоварівТаПослугДокумент.Show();
			}
		}

        #endregion
    }
}
