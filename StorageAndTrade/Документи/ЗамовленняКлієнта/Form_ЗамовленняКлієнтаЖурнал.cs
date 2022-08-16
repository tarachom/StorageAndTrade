﻿/*
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
    public partial class Form_ЗамовленняКлієнтаЖурнал : Form
    {
        public Form_ЗамовленняКлієнтаЖурнал()
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

        private void Form_ЗамовленняКлієнтаЖурнал_Load(object sender, EventArgs e)
        {
			LoadRecords();
		}

		private BindingList<Записи> RecordsBindingList { get; set; }

		public void LoadRecords()
		{
			int selectRow = dataGridViewRecords.SelectedRows.Count > 0 ?
				dataGridViewRecords.SelectedRows[dataGridViewRecords.SelectedRows.Count - 1].Index : 0;

			RecordsBindingList.Clear();

			Документи.ЗамовленняКлієнта_Select замовленняКлієнта_Select = new Документи.ЗамовленняКлієнта_Select();
			замовленняКлієнта_Select.QuerySelect.Field.AddRange(new string[] {
				"spend",
				Документи.ЗамовленняКлієнта_Const.Назва,
				Документи.ЗамовленняКлієнта_Const.НомерДок,
				Документи.ЗамовленняКлієнта_Const.ДатаДок,
				Документи.ЗамовленняКлієнта_Const.СумаДокументу,
				Документи.ЗамовленняКлієнта_Const.Коментар
			});

			//Контрагент
			замовленняКлієнта_Select.QuerySelect.FieldAndAlias.Add(
				new NameValue<string>(Довідники.Контрагенти_Const.TABLE + "." + Довідники.Контрагенти_Const.Назва, "joinContragent"));
			замовленняКлієнта_Select.QuerySelect.Joins.Add(
				new Join(Довідники.Контрагенти_Const.TABLE, Документи.ЗамовленняКлієнта_Const.Контрагент, Документи.ЗамовленняКлієнта_Const.TABLE));

			//ORDER
			замовленняКлієнта_Select.QuerySelect.Order.Add(Документи.ЗамовленняКлієнта_Const.ДатаДок, SelectOrder.ASC);
			замовленняКлієнта_Select.QuerySelect.Order.Add(Документи.ЗамовленняКлієнта_Const.НомерДок, SelectOrder.ASC);

			замовленняКлієнта_Select.Select();
			while (замовленняКлієнта_Select.MoveNext())
			{
				Документи.ЗамовленняКлієнта_Pointer cur = замовленняКлієнта_Select.Current;

				RecordsBindingList.Add(new Записи
				{
					ID = cur.UnigueID.ToString(),
					Назва = cur.Fields[Документи.ЗамовленняКлієнта_Const.Назва].ToString(),
					НомерДок = cur.Fields[Документи.ЗамовленняКлієнта_Const.НомерДок].ToString(),
					ДатаДок = cur.Fields[Документи.ЗамовленняКлієнта_Const.ДатаДок].ToString(),
					Контрагент = cur.Fields["joinContragent"].ToString(),
					Сума = Math.Round((decimal)cur.Fields[Документи.ЗамовленняКлієнта_Const.СумаДокументу], 2),
					Коментар = cur.Fields[Документи.ЗамовленняКлієнта_Const.Коментар].ToString(),
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
					DocumentPointerItem = new Документи.ЗамовленняКлієнта_Pointer(new UnigueID(Uid));
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
			Form_ЗамовленняКлієнтаДокумент form_ЗамовленняКлієнтаДокумент = new Form_ЗамовленняКлієнтаДокумент();
			form_ЗамовленняКлієнтаДокумент.MdiParent = this.MdiParent;
			form_ЗамовленняКлієнтаДокумент.IsNew = true;
			form_ЗамовленняКлієнтаДокумент.OwnerForm = this;
			form_ЗамовленняКлієнтаДокумент.Show();
        }

        private void toolStripButtonEdit_Click(object sender, EventArgs e)
        {
			if (dataGridViewRecords.SelectedRows.Count > 0)
			{
				int RowIndex = dataGridViewRecords.SelectedRows[0].Index;

				Form_ЗамовленняКлієнтаДокумент form_ЗамовленняКлієнтаДокумент = new Form_ЗамовленняКлієнтаДокумент();
				form_ЗамовленняКлієнтаДокумент.MdiParent = this.MdiParent;
				form_ЗамовленняКлієнтаДокумент.IsNew = false;
				form_ЗамовленняКлієнтаДокумент.OwnerForm = this;
				form_ЗамовленняКлієнтаДокумент.Uid = dataGridViewRecords.Rows[RowIndex].Cells["ID"].Value.ToString();
				form_ЗамовленняКлієнтаДокумент.Show();
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

                    Документи.ЗамовленняКлієнта_Objest замовленняКлієнта_Objest = new Документи.ЗамовленняКлієнта_Objest();
                    if (замовленняКлієнта_Objest.Read(new UnigueID(uid)))
                    {
						Документи.ЗамовленняКлієнта_Objest замовленняКлієнта_Objest_Новий = замовленняКлієнта_Objest.Copy();
						замовленняКлієнта_Objest_Новий.Назва += " *";
						замовленняКлієнта_Objest_Новий.ДатаДок = DateTime.Now;
						замовленняКлієнта_Objest_Новий.НомерДок = (++Константи.НумераціяДокументів.ЗамовленняКлієнта_Const).ToString("D8");

						//Зчитати та скопіювати табличну частину Товари
						замовленняКлієнта_Objest.Товари_TablePart.Read();
						замовленняКлієнта_Objest_Новий.Товари_TablePart.Records = замовленняКлієнта_Objest.Товари_TablePart.Copy();
						замовленняКлієнта_Objest_Новий.Товари_TablePart.Save(true);
						замовленняКлієнта_Objest_Новий.Save();
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

                    Документи.ЗамовленняКлієнта_Objest ЗамовленняКлієнта_Objest = new Документи.ЗамовленняКлієнта_Objest();
                    if (ЗамовленняКлієнта_Objest.Read(new UnigueID(uid)))
                    {
						ЗамовленняКлієнта_Objest.Delete();
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

				Звіти.РухДокументівПоРегістрах.PrintRecords(new Документи.ЗамовленняКлієнта_Pointer(new UnigueID(uid)));
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

					Документи.ЗамовленняКлієнта_Pointer замовленняКлієнта_Pointer = new Документи.ЗамовленняКлієнта_Pointer(new UnigueID(uid));
					Документи.ЗамовленняКлієнта_Objest замовленняКлієнта_Objest = замовленняКлієнта_Pointer.GetDocumentObject(true);

					if (spend)
						try
						{
							//Проведення
							замовленняКлієнта_Objest.SpendTheDocument(замовленняКлієнта_Objest.ДатаДок);
						}
						catch (Exception exp)
						{
							замовленняКлієнта_Objest.ClearSpendTheDocument();
							MessageBox.Show(exp.Message);
							return;
						}
					else
						замовленняКлієнта_Objest.ClearSpendTheDocument();
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

		private void реалізаціяТоварівТаПослугtoolStripMenuItem_Click(object sender, EventArgs e)
        {
			if (dataGridViewRecords.SelectedRows.Count > 0)
			{
				DataGridViewRow row = dataGridViewRecords.SelectedRows[0];
				string uid = row.Cells["ID"].Value.ToString();

				Документи.ЗамовленняКлієнта_Pointer замовленняКлієнта_Pointer = new Документи.ЗамовленняКлієнта_Pointer(new UnigueID(uid));
				Документи.ЗамовленняКлієнта_Objest замовленняКлієнта_Objest = замовленняКлієнта_Pointer.GetDocumentObject(true);

				//
				//Новий документ
				//

				Документи.РеалізаціяТоварівТаПослуг_Objest реалізаціяТоварівТаПослуг_Новий = new Документи.РеалізаціяТоварівТаПослуг_Objest();
				реалізаціяТоварівТаПослуг_Новий.New();
				реалізаціяТоварівТаПослуг_Новий.ДатаДок = DateTime.Now;
				реалізаціяТоварівТаПослуг_Новий.НомерДок = (++Константи.НумераціяДокументів.РеалізаціяТоварівТаПослуг_Const).ToString("D8");
				реалізаціяТоварівТаПослуг_Новий.Назва = $"Реалізація товарів та послуг №{реалізаціяТоварівТаПослуг_Новий.НомерДок} від {реалізаціяТоварівТаПослуг_Новий.ДатаДок.ToString("dd.MM.yyyy")}";
				реалізаціяТоварівТаПослуг_Новий.Організація = замовленняКлієнта_Objest.Організація;
				реалізаціяТоварівТаПослуг_Новий.Валюта = замовленняКлієнта_Objest.Валюта;
				реалізаціяТоварівТаПослуг_Новий.Каса = замовленняКлієнта_Objest.Каса;
				реалізаціяТоварівТаПослуг_Новий.Контрагент = замовленняКлієнта_Objest.Контрагент;
				реалізаціяТоварівТаПослуг_Новий.Договір = замовленняКлієнта_Objest.Договір;
				реалізаціяТоварівТаПослуг_Новий.Склад = замовленняКлієнта_Objest.Склад;
				//реалізаціяТоварівТаПослуг_Новий.Коментар = $"На основі \"{замовленняКлієнта_Objest.Назва}\"";
				реалізаціяТоварівТаПослуг_Новий.СумаДокументу = замовленняКлієнта_Objest.СумаДокументу;
				реалізаціяТоварівТаПослуг_Новий.Статус = Перелічення.СтатусиРеалізаціїТоварівТаПослуг.ДоОплати;
				реалізаціяТоварівТаПослуг_Новий.ФормаОплати = замовленняКлієнта_Objest.ФормаОплати;
				реалізаціяТоварівТаПослуг_Новий.Основа = new UuidAndText(замовленняКлієнта_Objest.UnigueID.UGuid, замовленняКлієнта_Objest.TypeDocument);
				реалізаціяТоварівТаПослуг_Новий.Save();

				//Товари
                foreach (Документи.ЗамовленняКлієнта_Товари_TablePart.Record record_замовлення in замовленняКлієнта_Objest.Товари_TablePart.Records)
                {
					Документи.РеалізаціяТоварівТаПослуг_Товари_TablePart.Record record_реалізація = new Документи.РеалізаціяТоварівТаПослуг_Товари_TablePart.Record();
					реалізаціяТоварівТаПослуг_Новий.Товари_TablePart.Records.Add(record_реалізація);

					record_реалізація.Номенклатура = record_замовлення.Номенклатура;
					record_реалізація.ХарактеристикаНоменклатури = record_замовлення.ХарактеристикаНоменклатури;
					record_реалізація.Пакування = record_замовлення.Пакування;
					record_реалізація.КількістьУпаковок = record_замовлення.КількістьУпаковок;
					record_реалізація.Кількість = record_замовлення.Кількість;
					record_реалізація.Ціна = record_замовлення.Ціна;
					record_реалізація.Сума = record_замовлення.Сума;
					record_реалізація.Скидка = record_замовлення.Скидка;
					record_реалізація.ЗамовленняКлієнта = замовленняКлієнта_Objest.GetDocumentPointer();
					record_реалізація.Склад = замовленняКлієнта_Objest.Склад;
					record_реалізація.ВидЦіни = record_замовлення.ВидЦіни;
				}

				реалізаціяТоварівТаПослуг_Новий.Товари_TablePart.Save(false);

				//Відкрити журнал та документ
				Form form_РеалізаціяТоварівТаПослугЖурнал = Application.OpenForms["Form_РеалізаціяТоварівТаПослугЖурнал"];
				if (form_РеалізаціяТоварівТаПослугЖурнал == null)
				{
					form_РеалізаціяТоварівТаПослугЖурнал = new Form_РеалізаціяТоварівТаПослугЖурнал();
					form_РеалізаціяТоварівТаПослугЖурнал.MdiParent = this.MdiParent;
					form_РеалізаціяТоварівТаПослугЖурнал.Show();
				}
				else
					((Form_РеалізаціяТоварівТаПослугЖурнал)form_РеалізаціяТоварівТаПослугЖурнал).LoadRecords();

				Form_РеалізаціяТоварівТаПослугДокумент form_РеалізаціяТоварівТаПослугДокумент = new Form_РеалізаціяТоварівТаПослугДокумент();
				form_РеалізаціяТоварівТаПослугДокумент.MdiParent = this.MdiParent;
				form_РеалізаціяТоварівТаПослугДокумент.IsNew = false;
				form_РеалізаціяТоварівТаПослугДокумент.OwnerForm = (Form_РеалізаціяТоварівТаПослугЖурнал)form_РеалізаціяТоварівТаПослугЖурнал;
				form_РеалізаціяТоварівТаПослугДокумент.Uid = реалізаціяТоварівТаПослуг_Новий.UnigueID.ToString();
				form_РеалізаціяТоварівТаПослугДокумент.Show();
			}
		}

		private void замовленняПостачальникуtoolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (dataGridViewRecords.SelectedRows.Count > 0)
			{
				DataGridViewRow row = dataGridViewRecords.SelectedRows[0];
				string uid = row.Cells["ID"].Value.ToString();

				Документи.ЗамовленняКлієнта_Pointer замовленняКлієнта_Pointer = new Документи.ЗамовленняКлієнта_Pointer(new UnigueID(uid));
				Документи.ЗамовленняКлієнта_Objest замовленняКлієнта_Objest = замовленняКлієнта_Pointer.GetDocumentObject(true);

				//
				//Новий документ
				//

				Документи.ЗамовленняПостачальнику_Objest замовленняПостачальнику_Новий = new Документи.ЗамовленняПостачальнику_Objest();
				замовленняПостачальнику_Новий.New();
				замовленняПостачальнику_Новий.ДатаДок = DateTime.Now;
				замовленняПостачальнику_Новий.НомерДок = (++Константи.НумераціяДокументів.ЗамовленняПостачальнику_Const).ToString("D8");
				замовленняПостачальнику_Новий.Назва = $"Замовлення постачальнику №{замовленняПостачальнику_Новий.НомерДок} від {замовленняПостачальнику_Новий.ДатаДок.ToString("dd.MM.yyyy")}";
				замовленняПостачальнику_Новий.Організація = замовленняКлієнта_Objest.Організація;
				замовленняПостачальнику_Новий.Валюта = замовленняКлієнта_Objest.Валюта;
				замовленняПостачальнику_Новий.Каса = замовленняКлієнта_Objest.Каса;
				замовленняПостачальнику_Новий.Контрагент = замовленняКлієнта_Objest.Контрагент;
				//замовленняПостачальнику_Новий.Договір = замовленняКлієнта_Objest.Договір;
				замовленняПостачальнику_Новий.Склад = замовленняКлієнта_Objest.Склад;
				//замовленняПостачальнику_Новий.Коментар = $"На основі \"{замовленняКлієнта_Objest.Назва}\"";
				замовленняПостачальнику_Новий.СумаДокументу = замовленняКлієнта_Objest.СумаДокументу;
				замовленняПостачальнику_Новий.Статус = Перелічення.СтатусиЗамовленьПостачальникам.Підтверджений;
				замовленняПостачальнику_Новий.ФормаОплати = замовленняКлієнта_Objest.ФормаОплати;
				замовленняПостачальнику_Новий.Основа = new UuidAndText(замовленняКлієнта_Objest.UnigueID.UGuid, замовленняКлієнта_Objest.TypeDocument);
				замовленняПостачальнику_Новий.Save();

				//Товари
				foreach (Документи.ЗамовленняКлієнта_Товари_TablePart.Record record_замовлення in замовленняКлієнта_Objest.Товари_TablePart.Records)
				{
					Документи.ЗамовленняПостачальнику_Товари_TablePart.Record record_замовленняПостачальнику = new Документи.ЗамовленняПостачальнику_Товари_TablePart.Record();
					замовленняПостачальнику_Новий.Товари_TablePart.Records.Add(record_замовленняПостачальнику);

					record_замовленняПостачальнику.Номенклатура = record_замовлення.Номенклатура;
					record_замовленняПостачальнику.ХарактеристикаНоменклатури = record_замовлення.ХарактеристикаНоменклатури;
					record_замовленняПостачальнику.Пакування = record_замовлення.Пакування;
					record_замовленняПостачальнику.КількістьУпаковок = record_замовлення.КількістьУпаковок;
					record_замовленняПостачальнику.Кількість = record_замовлення.Кількість;
					record_замовленняПостачальнику.Ціна = record_замовлення.Ціна;
					record_замовленняПостачальнику.Сума = record_замовлення.Сума;
					record_замовленняПостачальнику.Скидка = record_замовлення.Скидка;
					record_замовленняПостачальнику.Склад = замовленняКлієнта_Objest.Склад;
				}

				замовленняПостачальнику_Новий.Товари_TablePart.Save(false);

				//Відкрити журнал та документ
				Form form_ЗамовленняПостачальникуЖурнал = Application.OpenForms["Form_ЗамовленняПостачальникуЖурнал"];
				if (form_ЗамовленняПостачальникуЖурнал == null)
				{
					form_ЗамовленняПостачальникуЖурнал = new Form_ЗамовленняПостачальникуЖурнал();
					form_ЗамовленняПостачальникуЖурнал.MdiParent = this.MdiParent;
					form_ЗамовленняПостачальникуЖурнал.Show();
				}
				else
					((Form_ЗамовленняПостачальникуЖурнал)form_ЗамовленняПостачальникуЖурнал).LoadRecords();

				Form_ЗамовленняПостачальникуДокумент form_ЗамовленняПостачальникуДокумент = new Form_ЗамовленняПостачальникуДокумент();
				form_ЗамовленняПостачальникуДокумент.MdiParent = this.MdiParent;
				form_ЗамовленняПостачальникуДокумент.IsNew = false;
				form_ЗамовленняПостачальникуДокумент.OwnerForm = (Form_ЗамовленняПостачальникуЖурнал)form_ЗамовленняПостачальникуЖурнал;
				form_ЗамовленняПостачальникуДокумент.Uid = замовленняПостачальнику_Новий.UnigueID.ToString();
				form_ЗамовленняПостачальникуДокумент.Show();
			}
		}

        private void поступленняТоварівТаПослугToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        #endregion
    }
}
