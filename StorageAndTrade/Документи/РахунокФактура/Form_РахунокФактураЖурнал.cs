/*
Copyright (C) 2019-2022 TARAKHOMYN YURIY IVANOVYCH
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
    public partial class Form_РахунокФактураЖурнал : Form
    {
        public Form_РахунокФактураЖурнал()
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

		private void Form_РахунокФактураЖурнал_Load(object sender, EventArgs e)
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

			Документи.РахунокФактура_Select рахунокФактура_Select = new Документи.РахунокФактура_Select();
			рахунокФактура_Select.QuerySelect.Field.AddRange(new string[] {
				"spend",
				Документи.РахунокФактура_Const.Назва,
				Документи.РахунокФактура_Const.НомерДок,
				Документи.РахунокФактура_Const.ДатаДок,
				Документи.РахунокФактура_Const.СумаДокументу,
				Документи.РахунокФактура_Const.Коментар
			});

			//Контрагент
			рахунокФактура_Select.QuerySelect.FieldAndAlias.Add(
				new NameValue<string>(Довідники.Контрагенти_Const.TABLE + "." + Довідники.Контрагенти_Const.Назва, "joinContragent"));
			рахунокФактура_Select.QuerySelect.Joins.Add(
				new Join(Довідники.Контрагенти_Const.TABLE, Документи.РахунокФактура_Const.Контрагент, Документи.РахунокФактура_Const.TABLE));

			//ORDER
			рахунокФактура_Select.QuerySelect.Order.Add(Документи.РахунокФактура_Const.ДатаДок, SelectOrder.ASC);
			рахунокФактура_Select.QuerySelect.Order.Add(Документи.РахунокФактура_Const.НомерДок, SelectOrder.ASC);

			Перелічення.ТипПеріодуДляЖурналівДокументів ПеріодЖурналу =
				((NameValue<Перелічення.ТипПеріодуДляЖурналівДокументів>)сomboBox_ТипПеріоду.Items[сomboBox_ТипПеріоду.SelectedIndex]).Value;

			switch (ПеріодЖурналу)
			{

				case Перелічення.ТипПеріодуДляЖурналівДокументів.ЗПочаткуРоку:
					{
						рахунокФактура_Select.QuerySelect.Where.Add(new Where(Документи.РахунокФактура_Const.ДатаДок, Comparison.QT_EQ, new DateTime(DateTime.Now.Year, 1, 1)));
						break;
					}
				case Перелічення.ТипПеріодуДляЖурналівДокументів.Квартал:
					{
						DateTime ДатаТриМісцяНазад = DateTime.Now.AddMonths(-3);
						рахунокФактура_Select.QuerySelect.Where.Add(new Where(Документи.РахунокФактура_Const.ДатаДок, Comparison.QT_EQ, new DateTime(ДатаТриМісцяНазад.Year, ДатаТриМісцяНазад.Month, 1)));
						break;
					}
				case Перелічення.ТипПеріодуДляЖурналівДокументів.ЗМинулогоМісяця:
					{
						DateTime ДатаМісцьНазад = DateTime.Now.AddMonths(-1);
						рахунокФактура_Select.QuerySelect.Where.Add(new Where(Документи.РахунокФактура_Const.ДатаДок, Comparison.QT_EQ, new DateTime(ДатаМісцьНазад.Year, ДатаМісцьНазад.Month, 1)));
						break;
					}
				case Перелічення.ТипПеріодуДляЖурналівДокументів.ЗПочаткуМісяця:
					{
						рахунокФактура_Select.QuerySelect.Where.Add(new Where(Документи.РахунокФактура_Const.ДатаДок, Comparison.QT_EQ, new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1)));
						break;
					}
				case Перелічення.ТипПеріодуДляЖурналівДокументів.ЗПочаткуТижня:
					{
						DateTime СімДнівНазад = DateTime.Now.AddDays(-7);
						рахунокФактура_Select.QuerySelect.Where.Add(new Where(Документи.РахунокФактура_Const.ДатаДок, Comparison.QT_EQ, new DateTime(СімДнівНазад.Year, СімДнівНазад.Month, СімДнівНазад.Day)));
						break;
					}
				case Перелічення.ТипПеріодуДляЖурналівДокументів.ПоточнийДень:
					{
						рахунокФактура_Select.QuerySelect.Where.Add(new Where(Документи.РахунокФактура_Const.ДатаДок, Comparison.QT_EQ, new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day)));
						break;
					}
			}

			рахунокФактура_Select.Select();
			while (рахунокФактура_Select.MoveNext())
			{
				Документи.РахунокФактура_Pointer cur = рахунокФактура_Select.Current;

				RecordsBindingList.Add(new Записи
				{
					ID = cur.UnigueID.ToString(),
					Назва = cur.Fields[Документи.РахунокФактура_Const.Назва].ToString(),
					НомерДок = cur.Fields[Документи.РахунокФактура_Const.НомерДок].ToString(),
					ДатаДок = cur.Fields[Документи.РахунокФактура_Const.ДатаДок].ToString(),
					Контрагент = cur.Fields["joinContragent"].ToString(),
					Сума = Math.Round((decimal)cur.Fields[Документи.РахунокФактура_Const.СумаДокументу], 2),
					Коментар = cur.Fields[Документи.РахунокФактура_Const.Коментар].ToString(),
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
					DocumentPointerItem = new Документи.РахунокФактура_Pointer(new UnigueID(Uid));
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
			Form_РахунокФактураДокумент form_РахунокФактураДокумент = new Form_РахунокФактураДокумент();
			form_РахунокФактураДокумент.MdiParent = this.MdiParent;
			form_РахунокФактураДокумент.IsNew = true;
			form_РахунокФактураДокумент.OwnerForm = this;
			form_РахунокФактураДокумент.Show();
        }

        private void toolStripButtonEdit_Click(object sender, EventArgs e)
        {
			if (dataGridViewRecords.SelectedRows.Count > 0)
			{
				int RowIndex = dataGridViewRecords.SelectedRows[0].Index;

				Form_РахунокФактураДокумент form_РахунокФактураДокумент = new Form_РахунокФактураДокумент();
				form_РахунокФактураДокумент.MdiParent = this.MdiParent;
				form_РахунокФактураДокумент.IsNew = false;
				form_РахунокФактураДокумент.OwnerForm = this;
				form_РахунокФактураДокумент.Uid = dataGridViewRecords.Rows[RowIndex].Cells["ID"].Value.ToString();
				form_РахунокФактураДокумент.Show();
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

                    Документи.РахунокФактура_Objest рахунокФактура_Objest = new Документи.РахунокФактура_Objest();
                    if (рахунокФактура_Objest.Read(new UnigueID(uid)))
                    {
						Документи.РахунокФактура_Objest РахунокФактура_Objest_Новий = рахунокФактура_Objest.Copy();
						РахунокФактура_Objest_Новий.Назва += " *";
						РахунокФактура_Objest_Новий.ДатаДок = DateTime.Now;
						РахунокФактура_Objest_Новий.НомерДок = (++Константи.НумераціяДокументів.РахунокФактура_Const).ToString("D8");

						//Зчитати та скопіювати табличну частину Товари
						рахунокФактура_Objest.Товари_TablePart.Read();
						РахунокФактура_Objest_Новий.Товари_TablePart.Records = рахунокФактура_Objest.Товари_TablePart.Copy();
						РахунокФактура_Objest_Новий.Товари_TablePart.Save(true);
						РахунокФактура_Objest_Новий.Save();

						SelectPointerItem = РахунокФактура_Objest_Новий.GetDocumentPointer();
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

                    Документи.РахунокФактура_Objest рахунокФактура_Objest = new Документи.РахунокФактура_Objest();
                    if (рахунокФактура_Objest.Read(new UnigueID(uid)))
                    {
						рахунокФактура_Objest.Delete();
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

				РухДокументівПоРегістрах.PrintRecords(new Документи.РахунокФактура_Pointer(new UnigueID(uid)));
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

					Документи.РахунокФактура_Pointer рахунокФактура_Pointer = new Документи.РахунокФактура_Pointer(new UnigueID(uid));
					Документи.РахунокФактура_Objest рахунокФактура_Objest = рахунокФактура_Pointer.GetDocumentObject(true);

					if (spend)
						try
						{
							//Проведення
							рахунокФактура_Objest.SpendTheDocument(рахунокФактура_Objest.ДатаДок);
						}
						catch (Exception exp)
						{
							рахунокФактура_Objest.ClearSpendTheDocument();
							MessageBox.Show(exp.Message);
							return;
						}
					else
						рахунокФактура_Objest.ClearSpendTheDocument();
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
				List<Документи.РеалізаціяТоварівТаПослуг_Objest> НовіДокументи = new List<Документи.РеалізаціяТоварівТаПослуг_Objest>();

				for (int i = 0; i < dataGridViewRecords.SelectedRows.Count; i++)
				{
					DataGridViewRow row = dataGridViewRecords.SelectedRows[i];
					string uid = row.Cells["ID"].Value.ToString();

					Документи.РахунокФактура_Pointer РахунокФактура_Pointer = new Документи.РахунокФактура_Pointer(new UnigueID(uid));
					Документи.РахунокФактура_Objest РахунокФактура_Objest = РахунокФактура_Pointer.GetDocumentObject(true);

					//
					//Новий документ
					//

					Документи.РеалізаціяТоварівТаПослуг_Objest реалізаціяТоварівТаПослуг_Новий = new Документи.РеалізаціяТоварівТаПослуг_Objest();
					реалізаціяТоварівТаПослуг_Новий.New();
					реалізаціяТоварівТаПослуг_Новий.ДатаДок = DateTime.Now;
					реалізаціяТоварівТаПослуг_Новий.НомерДок = (++Константи.НумераціяДокументів.РеалізаціяТоварівТаПослуг_Const).ToString("D8");
					реалізаціяТоварівТаПослуг_Новий.Назва = $"Реалізація товарів та послуг №{реалізаціяТоварівТаПослуг_Новий.НомерДок} від {реалізаціяТоварівТаПослуг_Новий.ДатаДок.ToString("dd.MM.yyyy")}";
					реалізаціяТоварівТаПослуг_Новий.Організація = РахунокФактура_Objest.Організація;
					реалізаціяТоварівТаПослуг_Новий.Валюта = РахунокФактура_Objest.Валюта;
					реалізаціяТоварівТаПослуг_Новий.Каса = РахунокФактура_Objest.Каса;
					реалізаціяТоварівТаПослуг_Новий.Контрагент = РахунокФактура_Objest.Контрагент;
					реалізаціяТоварівТаПослуг_Новий.Договір = РахунокФактура_Objest.Договір;
					реалізаціяТоварівТаПослуг_Новий.Склад = РахунокФактура_Objest.Склад;
					//реалізаціяТоварівТаПослуг_Новий.Коментар = "";
					реалізаціяТоварівТаПослуг_Новий.СумаДокументу = РахунокФактура_Objest.СумаДокументу;
					реалізаціяТоварівТаПослуг_Новий.Статус = Перелічення.СтатусиРеалізаціїТоварівТаПослуг.ДоОплати;
					реалізаціяТоварівТаПослуг_Новий.ФормаОплати = РахунокФактура_Objest.ФормаОплати;
					реалізаціяТоварівТаПослуг_Новий.Основа = new UuidAndText(РахунокФактура_Objest.UnigueID.UGuid, РахунокФактура_Objest.TypeDocument);
					реалізаціяТоварівТаПослуг_Новий.Save();

					//Товари
					foreach (Документи.РахунокФактура_Товари_TablePart.Record record_замовлення in РахунокФактура_Objest.Товари_TablePart.Records)
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
						record_реалізація.РахунокФактура = РахунокФактура_Objest.GetDocumentPointer();
						record_реалізація.Склад = РахунокФактура_Objest.Склад;
						record_реалізація.ВидЦіни = record_замовлення.ВидЦіни;
					}

					реалізаціяТоварівТаПослуг_Новий.Товари_TablePart.Save(false);

					НовіДокументи.Add(реалізаціяТоварівТаПослуг_Новий);
				}

				//Відкрити журнал та документ
				Form form_РеалізаціяТоварівТаПослугЖурнал = Application.OpenForms["Form_РеалізаціяТоварівТаПослугЖурнал"];
				if (form_РеалізаціяТоварівТаПослугЖурнал == null)
				{
					form_РеалізаціяТоварівТаПослугЖурнал = new Form_РеалізаціяТоварівТаПослугЖурнал();
					form_РеалізаціяТоварівТаПослугЖурнал.MdiParent = this.MdiParent;
					((Form_РеалізаціяТоварівТаПослугЖурнал)form_РеалізаціяТоварівТаПослугЖурнал).SelectPointerItem = НовіДокументи[НовіДокументи.Count - 1].GetDocumentPointer();
					form_РеалізаціяТоварівТаПослугЖурнал.Show();
				}
                else
                {
					((Form_РеалізаціяТоварівТаПослугЖурнал)form_РеалізаціяТоварівТаПослугЖурнал).SelectPointerItem = НовіДокументи[НовіДокументи.Count - 1].GetDocumentPointer();
					((Form_РеалізаціяТоварівТаПослугЖурнал)form_РеалізаціяТоварівТаПослугЖурнал).LoadRecords();
				}

				this.Focus();

				foreach (Документи.РеалізаціяТоварівТаПослуг_Objest НовийДокумент in НовіДокументи)
				{
					Form_РеалізаціяТоварівТаПослугДокумент form_РеалізаціяТоварівТаПослугДокумент = new Form_РеалізаціяТоварівТаПослугДокумент();
					form_РеалізаціяТоварівТаПослугДокумент.MdiParent = this.MdiParent;
					form_РеалізаціяТоварівТаПослугДокумент.IsNew = false;
					form_РеалізаціяТоварівТаПослугДокумент.OwnerForm = (Form_РеалізаціяТоварівТаПослугЖурнал)form_РеалізаціяТоварівТаПослугЖурнал;
					form_РеалізаціяТоварівТаПослугДокумент.Uid = НовийДокумент.UnigueID.ToString();
					form_РеалізаціяТоварівТаПослугДокумент.Show();
				}
			}
		}

		private void замовленняПостачальникуtoolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (dataGridViewRecords.SelectedRows.Count > 0)
			{
				List<Документи.ЗамовленняПостачальнику_Objest> НовіДокументи = new List<Документи.ЗамовленняПостачальнику_Objest>();

				for (int i = 0; i < dataGridViewRecords.SelectedRows.Count; i++)
				{
					DataGridViewRow row = dataGridViewRecords.SelectedRows[i];
					string uid = row.Cells["ID"].Value.ToString();

					Документи.РахунокФактура_Pointer рахунокФактура_Pointer = new Документи.РахунокФактура_Pointer(new UnigueID(uid));
					Документи.РахунокФактура_Objest рахунокФактура_Objest = рахунокФактура_Pointer.GetDocumentObject(true);

					//
					//Новий документ
					//

					Документи.ЗамовленняПостачальнику_Objest замовленняПостачальнику_Новий = new Документи.ЗамовленняПостачальнику_Objest();
					замовленняПостачальнику_Новий.New();
					замовленняПостачальнику_Новий.ДатаДок = DateTime.Now;
					замовленняПостачальнику_Новий.НомерДок = (++Константи.НумераціяДокументів.ЗамовленняПостачальнику_Const).ToString("D8");
					замовленняПостачальнику_Новий.Назва = $"Замовлення постачальнику №{замовленняПостачальнику_Новий.НомерДок} від {замовленняПостачальнику_Новий.ДатаДок.ToString("dd.MM.yyyy")}";
					замовленняПостачальнику_Новий.Організація = рахунокФактура_Objest.Організація;
					замовленняПостачальнику_Новий.Валюта = рахунокФактура_Objest.Валюта;
					замовленняПостачальнику_Новий.Каса = рахунокФактура_Objest.Каса;
					замовленняПостачальнику_Новий.Контрагент = рахунокФактура_Objest.Контрагент;
					//замовленняПостачальнику_Новий.Договір = замовленняКлієнта_Objest.Договір;
					замовленняПостачальнику_Новий.Склад = рахунокФактура_Objest.Склад;
					//замовленняПостачальнику_Новий.Коментар = "";
					замовленняПостачальнику_Новий.СумаДокументу = рахунокФактура_Objest.СумаДокументу;
					замовленняПостачальнику_Новий.Статус = Перелічення.СтатусиЗамовленьПостачальникам.Підтверджений;
					замовленняПостачальнику_Новий.ФормаОплати = рахунокФактура_Objest.ФормаОплати;
					замовленняПостачальнику_Новий.Основа = new UuidAndText(рахунокФактура_Objest.UnigueID.UGuid, рахунокФактура_Objest.TypeDocument);
					замовленняПостачальнику_Новий.Save();

					//Товари
					foreach (Документи.РахунокФактура_Товари_TablePart.Record record_замовлення in рахунокФактура_Objest.Товари_TablePart.Records)
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
						record_замовленняПостачальнику.Склад = рахунокФактура_Objest.Склад;
					}

					замовленняПостачальнику_Новий.Товари_TablePart.Save(false);

					НовіДокументи.Add(замовленняПостачальнику_Новий);
				}

				//Відкрити журнал та документ
				Form form_ЗамовленняПостачальникуЖурнал = Application.OpenForms["Form_ЗамовленняПостачальникуЖурнал"];
				if (form_ЗамовленняПостачальникуЖурнал == null)
				{
					form_ЗамовленняПостачальникуЖурнал = new Form_ЗамовленняПостачальникуЖурнал();
					form_ЗамовленняПостачальникуЖурнал.MdiParent = this.MdiParent;
					((Form_ЗамовленняПостачальникуЖурнал)form_ЗамовленняПостачальникуЖурнал).SelectPointerItem = НовіДокументи[НовіДокументи.Count - 1].GetDocumentPointer();
					form_ЗамовленняПостачальникуЖурнал.Show();
				}
				else
				{
					((Form_ЗамовленняПостачальникуЖурнал)form_ЗамовленняПостачальникуЖурнал).SelectPointerItem = НовіДокументи[НовіДокументи.Count - 1].GetDocumentPointer();
					((Form_ЗамовленняПостачальникуЖурнал)form_ЗамовленняПостачальникуЖурнал).LoadRecords();
				}

				this.Focus();

				foreach (Документи.ЗамовленняПостачальнику_Objest НовийДокумент in НовіДокументи)
				{
					Form_ЗамовленняПостачальникуДокумент form_ЗамовленняПостачальникуДокумент = new Form_ЗамовленняПостачальникуДокумент();
					form_ЗамовленняПостачальникуДокумент.MdiParent = this.MdiParent;
					form_ЗамовленняПостачальникуДокумент.IsNew = false;
					form_ЗамовленняПостачальникуДокумент.OwnerForm = (Form_ЗамовленняПостачальникуЖурнал)form_ЗамовленняПостачальникуЖурнал;
					form_ЗамовленняПостачальникуДокумент.Uid = НовийДокумент.UnigueID.ToString();
					form_ЗамовленняПостачальникуДокумент.Show();
				}
			}
		}

        #endregion

        private void dataGridViewRecords_CellClick(object sender, DataGridViewCellEventArgs e)
        {
			if (dataGridViewRecords.SelectedRows.Count > 0)
			{
				int RowIndex = dataGridViewRecords.SelectedRows[0].Index;

				SelectPointerItem = new Документи.РахунокФактура_Pointer(new UnigueID(dataGridViewRecords.Rows[e.RowIndex].Cells["ID"].Value.ToString()));
			}
		}

        private void сomboBox_ТипПеріоду_SelectedIndexChanged(object sender, EventArgs e)
        {
			dataGridViewRecords.Focus();

			LoadRecords();
		}
    }
}
