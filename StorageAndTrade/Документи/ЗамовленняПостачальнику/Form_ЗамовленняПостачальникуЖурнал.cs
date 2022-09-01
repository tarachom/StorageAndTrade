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

		/// <summary>
		/// Вказівник для вибору
		/// </summary>
		public DocumentPointer DocumentPointerItem { get; set; }

		/// <summary>
		/// Вказівник для виділення в списку
		/// </summary>
		public DocumentPointer SelectPointerItem { get; set; }

		private void Form_ЗамовленняПостачальникуЖурнал_Load(object sender, EventArgs e)
        {
			ConfigurationEnums ТипПеріодуДляЖурналівДокументів = Конфа.Config.Kernel.Conf.Enums["ТипПеріодуДляЖурналівДокументів"];

			foreach (ConfigurationEnumField field in ТипПеріодуДляЖурналівДокументів.Fields.Values)
			{
				int index = сomboBox_ТипПеріоду.Items.Add(
					new NameValue<Перелічення.ТипПеріодуДляЖурналівДокументів>(field.Desc, (Перелічення.ТипПеріодуДляЖурналівДокументів)field.Value));

				if ((Перелічення.ТипПеріодуДляЖурналівДокументів)field.Value == Константи.ЖурналиДокументів.ОсновнийТипПеріоду_Const)
					сomboBox_ТипПеріоду.SelectedIndex = index;
			}

			if (сomboBox_ТипПеріоду.SelectedIndex == -1)
				сomboBox_ТипПеріоду.SelectedIndex = 0;
		}

		private BindingList<Записи> RecordsBindingList { get; set; }

		public void LoadRecords()
		{
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

			Перелічення.ТипПеріодуДляЖурналівДокументів ПеріодЖурналу =
				((NameValue<Перелічення.ТипПеріодуДляЖурналівДокументів>)сomboBox_ТипПеріоду.Items[сomboBox_ТипПеріоду.SelectedIndex]).Value;

			switch (ПеріодЖурналу)
			{

				case Перелічення.ТипПеріодуДляЖурналівДокументів.ЗПочаткуРоку:
					{
						замовленняПостачальнику_Select.QuerySelect.Where.Add(new Where(Документи.ЗамовленняПостачальнику_Const.ДатаДок, Comparison.QT_EQ, new DateTime(DateTime.Now.Year, 1, 1)));
						break;
					}
				case Перелічення.ТипПеріодуДляЖурналівДокументів.Квартал:
					{
						DateTime ДатаТриМісцяНазад = DateTime.Now.AddMonths(-3);
						замовленняПостачальнику_Select.QuerySelect.Where.Add(new Where(Документи.ЗамовленняПостачальнику_Const.ДатаДок, Comparison.QT_EQ, new DateTime(ДатаТриМісцяНазад.Year, ДатаТриМісцяНазад.Month, 1)));
						break;
					}
				case Перелічення.ТипПеріодуДляЖурналівДокументів.ЗМинулогоМісяця:
					{
						DateTime ДатаМісцьНазад = DateTime.Now.AddMonths(-1);
						замовленняПостачальнику_Select.QuerySelect.Where.Add(new Where(Документи.ЗамовленняПостачальнику_Const.ДатаДок, Comparison.QT_EQ, new DateTime(ДатаМісцьНазад.Year, ДатаМісцьНазад.Month, 1)));
						break;
					}
				case Перелічення.ТипПеріодуДляЖурналівДокументів.ЗПочаткуМісяця:
					{
						замовленняПостачальнику_Select.QuerySelect.Where.Add(new Where(Документи.ЗамовленняПостачальнику_Const.ДатаДок, Comparison.QT_EQ, new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1)));
						break;
					}
				case Перелічення.ТипПеріодуДляЖурналівДокументів.ЗПочаткуТижня:
					{
						DateTime СімДнівНазад = DateTime.Now.AddDays(-7);
						замовленняПостачальнику_Select.QuerySelect.Where.Add(new Where(Документи.ЗамовленняПостачальнику_Const.ДатаДок, Comparison.QT_EQ, new DateTime(СімДнівНазад.Year, СімДнівНазад.Month, СімДнівНазад.Day)));
						break;
					}
				case Перелічення.ТипПеріодуДляЖурналівДокументів.ПоточнийДень:
					{
						замовленняПостачальнику_Select.QuerySelect.Where.Add(new Where(Документи.ЗамовленняПостачальнику_Const.ДатаДок, Comparison.QT_EQ, new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day)));
						break;
					}
			}

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
            }

			if (DocumentPointerItem != null || SelectPointerItem != null)
			{
				string UidSelect = SelectPointerItem != null ? SelectPointerItem.UnigueID.ToString() : DocumentPointerItem.UnigueID.ToString();

				if (UidSelect != Guid.Empty.ToString())
					ФункціїДляІнтерфейсу.ВиділитиЕлементСписку(dataGridViewRecords, "ID", UidSelect);
			}
			else
				ФункціїДляІнтерфейсу.ВиділитиОстаннійЕлементСписку(dataGridViewRecords);
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

						SelectPointerItem = замовленняПостачальнику_Objest_Новий.GetDocumentPointer();
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

				РухДокументівПоРегістрах.PrintRecords(new Документи.ЗамовленняПостачальнику_Pointer(new UnigueID(uid)));
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
				List<Документи.ПоступленняТоварівТаПослуг_Objest> НовіДокументи = new List<Документи.ПоступленняТоварівТаПослуг_Objest>();

				for (int i = 0; i < dataGridViewRecords.SelectedRows.Count; i++)
				{
					DataGridViewRow row = dataGridViewRecords.SelectedRows[i];
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
					//поступленняТоварівТаПослуг_Новий.Коментар = "";
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

					НовіДокументи.Add(поступленняТоварівТаПослуг_Новий);
				}

				//Відкрити журнал та документ
				Form form_ПоступленняТоварівТаПослугЖурнал = Application.OpenForms["Form_ПоступленняТоварівТаПослугЖурнал"];
				if (form_ПоступленняТоварівТаПослугЖурнал == null)
				{
					form_ПоступленняТоварівТаПослугЖурнал = new Form_ПоступленняТоварівТаПослугЖурнал();
					form_ПоступленняТоварівТаПослугЖурнал.MdiParent = this.MdiParent;
					((Form_ПоступленняТоварівТаПослугЖурнал)form_ПоступленняТоварівТаПослугЖурнал).SelectPointerItem = НовіДокументи[НовіДокументи.Count - 1].GetDocumentPointer();
					form_ПоступленняТоварівТаПослугЖурнал.Show();
				}
				else
				{
					((Form_ПоступленняТоварівТаПослугЖурнал)form_ПоступленняТоварівТаПослугЖурнал).SelectPointerItem = НовіДокументи[НовіДокументи.Count - 1].GetDocumentPointer();
					((Form_ПоступленняТоварівТаПослугЖурнал)form_ПоступленняТоварівТаПослугЖурнал).LoadRecords();
				}

				this.Focus();

				foreach (Документи.ПоступленняТоварівТаПослуг_Objest НовийДокумент in НовіДокументи)
				{
					Form_ПоступленняТоварівТаПослугДокумент form_ПоступленняТоварівТаПослугДокумент = new Form_ПоступленняТоварівТаПослугДокумент();
					form_ПоступленняТоварівТаПослугДокумент.MdiParent = this.MdiParent;
					form_ПоступленняТоварівТаПослугДокумент.IsNew = false;
					form_ПоступленняТоварівТаПослугДокумент.OwnerForm = (Form_ПоступленняТоварівТаПослугЖурнал)form_ПоступленняТоварівТаПослугЖурнал;
					form_ПоступленняТоварівТаПослугДокумент.Uid = НовийДокумент.UnigueID.ToString();
					form_ПоступленняТоварівТаПослугДокумент.Show();
				}
			}
		}

        #endregion

        private void dataGridViewRecords_CellClick(object sender, DataGridViewCellEventArgs e)
        {
			if (dataGridViewRecords.SelectedRows.Count > 0)
			{
				int RowIndex = dataGridViewRecords.SelectedRows[0].Index;

				SelectPointerItem = new Документи.ЗамовленняПостачальнику_Pointer(new UnigueID(dataGridViewRecords.Rows[e.RowIndex].Cells["ID"].Value.ToString()));
			}
		}

        private void сomboBox_ТипПеріоду_SelectedIndexChanged(object sender, EventArgs e)
        {
			dataGridViewRecords.Focus();

			LoadRecords();
		}
    }
}
