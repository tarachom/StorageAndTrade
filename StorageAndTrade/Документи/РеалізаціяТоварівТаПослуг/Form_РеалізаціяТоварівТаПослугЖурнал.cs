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
    public partial class Form_РеалізаціяТоварівТаПослугЖурнал : Form
    {
        public Form_РеалізаціяТоварівТаПослугЖурнал()
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

		private void Form_РеалізаціяТоварівТаПослугЖурнал_Load(object sender, EventArgs e)
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

			Документи.РеалізаціяТоварівТаПослуг_Select реалізаціяТоварівТаПослуг_Select = new Документи.РеалізаціяТоварівТаПослуг_Select();
			реалізаціяТоварівТаПослуг_Select.QuerySelect.Field.AddRange(new string[] {
				"spend",
				Документи.РеалізаціяТоварівТаПослуг_Const.Назва,
				Документи.РеалізаціяТоварівТаПослуг_Const.НомерДок,
				Документи.РеалізаціяТоварівТаПослуг_Const.ДатаДок,
				Документи.РеалізаціяТоварівТаПослуг_Const.СумаДокументу,
				Документи.РеалізаціяТоварівТаПослуг_Const.Коментар
			});

			//Контрагент
			реалізаціяТоварівТаПослуг_Select.QuerySelect.FieldAndAlias.Add(
				new NameValue<string>(Довідники.Контрагенти_Const.TABLE + "." + Довідники.Контрагенти_Const.Назва, "joinContragent"));
			реалізаціяТоварівТаПослуг_Select.QuerySelect.Joins.Add(
				new Join(Довідники.Контрагенти_Const.TABLE, Документи.РеалізаціяТоварівТаПослуг_Const.Контрагент, Документи.РеалізаціяТоварівТаПослуг_Const.TABLE));

			//ORDER
			реалізаціяТоварівТаПослуг_Select.QuerySelect.Order.Add(Документи.РеалізаціяТоварівТаПослуг_Const.ДатаДок, SelectOrder.ASC);
			реалізаціяТоварівТаПослуг_Select.QuerySelect.Order.Add(Документи.РеалізаціяТоварівТаПослуг_Const.НомерДок, SelectOrder.ASC);

			Перелічення.ТипПеріодуДляЖурналівДокументів ПеріодЖурналу =
				((NameValue<Перелічення.ТипПеріодуДляЖурналівДокументів>)сomboBox_ТипПеріоду.Items[сomboBox_ТипПеріоду.SelectedIndex]).Value;

			switch (ПеріодЖурналу)
			{

				case Перелічення.ТипПеріодуДляЖурналівДокументів.ЗПочаткуРоку:
					{
						реалізаціяТоварівТаПослуг_Select.QuerySelect.Where.Add(new Where(Документи.РеалізаціяТоварівТаПослуг_Const.ДатаДок, Comparison.QT_EQ, new DateTime(DateTime.Now.Year, 1, 1)));
						break;
					}
				case Перелічення.ТипПеріодуДляЖурналівДокументів.Квартал:
					{
						DateTime ДатаТриМісцяНазад = DateTime.Now.AddMonths(-3);
						реалізаціяТоварівТаПослуг_Select.QuerySelect.Where.Add(new Where(Документи.РеалізаціяТоварівТаПослуг_Const.ДатаДок, Comparison.QT_EQ, new DateTime(ДатаТриМісцяНазад.Year, ДатаТриМісцяНазад.Month, 1)));
						break;
					}
				case Перелічення.ТипПеріодуДляЖурналівДокументів.ЗМинулогоМісяця:
					{
						DateTime ДатаМісцьНазад = DateTime.Now.AddMonths(-1);
						реалізаціяТоварівТаПослуг_Select.QuerySelect.Where.Add(new Where(Документи.РеалізаціяТоварівТаПослуг_Const.ДатаДок, Comparison.QT_EQ, new DateTime(ДатаМісцьНазад.Year, ДатаМісцьНазад.Month, 1)));
						break;
					}
				case Перелічення.ТипПеріодуДляЖурналівДокументів.ЗПочаткуМісяця:
					{
						реалізаціяТоварівТаПослуг_Select.QuerySelect.Where.Add(new Where(Документи.РеалізаціяТоварівТаПослуг_Const.ДатаДок, Comparison.QT_EQ, new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1)));
						break;
					}
				case Перелічення.ТипПеріодуДляЖурналівДокументів.ЗПочаткуТижня:
					{
						DateTime СімДнівНазад = DateTime.Now.AddDays(-7);
						реалізаціяТоварівТаПослуг_Select.QuerySelect.Where.Add(new Where(Документи.РеалізаціяТоварівТаПослуг_Const.ДатаДок, Comparison.QT_EQ, new DateTime(СімДнівНазад.Year, СімДнівНазад.Month, СімДнівНазад.Day)));
						break;
					}
				case Перелічення.ТипПеріодуДляЖурналівДокументів.ПоточнийДень:
					{
						реалізаціяТоварівТаПослуг_Select.QuerySelect.Where.Add(new Where(Документи.РеалізаціяТоварівТаПослуг_Const.ДатаДок, Comparison.QT_EQ, new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day)));
						break;
					}
			}

			реалізаціяТоварівТаПослуг_Select.Select();
			while (реалізаціяТоварівТаПослуг_Select.MoveNext())
			{
				Документи.РеалізаціяТоварівТаПослуг_Pointer cur = реалізаціяТоварівТаПослуг_Select.Current;

				RecordsBindingList.Add(new Записи
				{
					ID = cur.UnigueID.ToString(),
					Назва = cur.Fields[Документи.РеалізаціяТоварівТаПослуг_Const.Назва].ToString(),
					НомерДок = cur.Fields[Документи.РеалізаціяТоварівТаПослуг_Const.НомерДок].ToString(),
					ДатаДок = cur.Fields[Документи.РеалізаціяТоварівТаПослуг_Const.ДатаДок].ToString(),
					Контрагент = cur.Fields["joinContragent"].ToString(),
					Сума = Math.Round((decimal)cur.Fields[Документи.РеалізаціяТоварівТаПослуг_Const.СумаДокументу], 2),
					Коментар = cur.Fields[Документи.РеалізаціяТоварівТаПослуг_Const.Коментар].ToString(),
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
					DocumentPointerItem = new Документи.РеалізаціяТоварівТаПослуг_Pointer(new UnigueID(Uid));
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
			Form_РеалізаціяТоварівТаПослугДокумент form_РеалізаціяТоварівТаПослугДокумент = new Form_РеалізаціяТоварівТаПослугДокумент();
			form_РеалізаціяТоварівТаПослугДокумент.MdiParent = this.MdiParent;
			form_РеалізаціяТоварівТаПослугДокумент.IsNew = true;
			form_РеалізаціяТоварівТаПослугДокумент.OwnerForm = this;
			form_РеалізаціяТоварівТаПослугДокумент.Show();
        }

        private void toolStripButtonEdit_Click(object sender, EventArgs e)
        {
			if (dataGridViewRecords.SelectedRows.Count > 0)
			{
				int RowIndex = dataGridViewRecords.SelectedRows[0].Index;

				Form_РеалізаціяТоварівТаПослугДокумент form_РеалізаціяТоварівТаПослугДокумент = new Form_РеалізаціяТоварівТаПослугДокумент();
				form_РеалізаціяТоварівТаПослугДокумент.MdiParent = this.MdiParent;
				form_РеалізаціяТоварівТаПослугДокумент.IsNew = false;
				form_РеалізаціяТоварівТаПослугДокумент.OwnerForm = this;
				form_РеалізаціяТоварівТаПослугДокумент.Uid = dataGridViewRecords.Rows[RowIndex].Cells["ID"].Value.ToString();
				form_РеалізаціяТоварівТаПослугДокумент.Show();
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

                    Документи.РеалізаціяТоварівТаПослуг_Objest реалізаціяТоварівТаПослуг_Objest = new Документи.РеалізаціяТоварівТаПослуг_Objest();
                    if (реалізаціяТоварівТаПослуг_Objest.Read(new UnigueID(uid)))
                    {
						Документи.РеалізаціяТоварівТаПослуг_Objest реалізаціяТоварівТаПослуг_Objest_Новий = реалізаціяТоварівТаПослуг_Objest.Copy();
						реалізаціяТоварівТаПослуг_Objest_Новий.Назва += " *";
						реалізаціяТоварівТаПослуг_Objest_Новий.ДатаДок = DateTime.Now;
						реалізаціяТоварівТаПослуг_Objest_Новий.НомерДок = (++Константи.НумераціяДокументів.РеалізаціяТоварівТаПослуг_Const).ToString("D8");

						//Зчитати та скопіювати табличну частину Товари
						реалізаціяТоварівТаПослуг_Objest.Товари_TablePart.Read();
						реалізаціяТоварівТаПослуг_Objest_Новий.Товари_TablePart.Records = реалізаціяТоварівТаПослуг_Objest.Товари_TablePart.Copy();
						реалізаціяТоварівТаПослуг_Objest_Новий.Товари_TablePart.Save(true);
						реалізаціяТоварівТаПослуг_Objest_Новий.Save();

						SelectPointerItem = реалізаціяТоварівТаПослуг_Objest_Новий.GetDocumentPointer();
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

                    Документи.РеалізаціяТоварівТаПослуг_Objest реалізаціяТоварівТаПослуг_Objest = new Документи.РеалізаціяТоварівТаПослуг_Objest();
                    if (реалізаціяТоварівТаПослуг_Objest.Read(new UnigueID(uid)))
                    {
						реалізаціяТоварівТаПослуг_Objest.Delete();
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

				РухДокументівПоРегістрах.PrintRecords(new Документи.РеалізаціяТоварівТаПослуг_Pointer(new UnigueID(uid)));
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

					Документи.РеалізаціяТоварівТаПослуг_Pointer реалізаціяТоварівТаПослуг_Pointer = new Документи.РеалізаціяТоварівТаПослуг_Pointer(new UnigueID(uid));
					Документи.РеалізаціяТоварівТаПослуг_Objest реалізаціяТоварівТаПослуг_Objest = реалізаціяТоварівТаПослуг_Pointer.GetDocumentObject(true);

					if (spend)
						try
						{
							//Проведення
							реалізаціяТоварівТаПослуг_Objest.SpendTheDocument(реалізаціяТоварівТаПослуг_Objest.ДатаДок);
						}
						catch (Exception exp)
						{
							реалізаціяТоварівТаПослуг_Objest.ClearSpendTheDocument();
							MessageBox.Show(exp.Message);
							return;
						}
					else
						реалізаціяТоварівТаПослуг_Objest.ClearSpendTheDocument();
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

		private void прихіднийКасовийОрдерToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (dataGridViewRecords.SelectedRows.Count > 0)
			{
				List<Документи.ПрихіднийКасовийОрдер_Objest> НовіДокументи = new List<Документи.ПрихіднийКасовийОрдер_Objest>();

				for (int i = 0; i < dataGridViewRecords.SelectedRows.Count; i++)
				{
					DataGridViewRow row = dataGridViewRecords.SelectedRows[i];
					string uid = row.Cells["ID"].Value.ToString();

					Документи.РеалізаціяТоварівТаПослуг_Pointer реалізаціяТоварівТаПослуг_Pointer = new Документи.РеалізаціяТоварівТаПослуг_Pointer(new UnigueID(uid));
					Документи.РеалізаціяТоварівТаПослуг_Objest реалізаціяТоварівТаПослуг_Objest = реалізаціяТоварівТаПослуг_Pointer.GetDocumentObject(false);

					//
					//Новий документ
					//

					Документи.ПрихіднийКасовийОрдер_Objest прихіднийКасовийОрдер_Новий = new Документи.ПрихіднийКасовийОрдер_Objest();
					прихіднийКасовийОрдер_Новий.New();
					прихіднийКасовийОрдер_Новий.ДатаДок = DateTime.Now;
					прихіднийКасовийОрдер_Новий.НомерДок = (++Константи.НумераціяДокументів.ПрихіднийКасовийОрдер_Const).ToString("D8");
					прихіднийКасовийОрдер_Новий.Назва = $"Прихідний касовий ордер №{прихіднийКасовийОрдер_Новий.НомерДок} від {прихіднийКасовийОрдер_Новий.ДатаДок.ToString("dd.MM.yyyy")}";
					прихіднийКасовийОрдер_Новий.Організація = реалізаціяТоварівТаПослуг_Objest.Організація;
					прихіднийКасовийОрдер_Новий.Валюта = реалізаціяТоварівТаПослуг_Objest.Валюта;
					прихіднийКасовийОрдер_Новий.Каса = реалізаціяТоварівТаПослуг_Objest.Каса;
					прихіднийКасовийОрдер_Новий.Контрагент = реалізаціяТоварівТаПослуг_Objest.Контрагент;
					прихіднийКасовийОрдер_Новий.Договір = реалізаціяТоварівТаПослуг_Objest.Договір;
					//прихіднийКасовийОрдер_Новий.Коментар = "";
					прихіднийКасовийОрдер_Новий.СумаДокументу = реалізаціяТоварівТаПослуг_Objest.СумаДокументу;
					прихіднийКасовийОрдер_Новий.Основа = new UuidAndText(реалізаціяТоварівТаПослуг_Objest.UnigueID.UGuid, реалізаціяТоварівТаПослуг_Objest.TypeDocument);
					прихіднийКасовийОрдер_Новий.Save();

					НовіДокументи.Add(прихіднийКасовийОрдер_Новий);
				}

				//Відкрити журнал та документ
				Form form_ПрихіднийКасовийОрдерЖурнал = Application.OpenForms["Form_ПрихіднийКасовийОрдерЖурнал"];
				if (form_ПрихіднийКасовийОрдерЖурнал == null)
				{
					form_ПрихіднийКасовийОрдерЖурнал = new Form_ПрихіднийКасовийОрдерЖурнал();
					form_ПрихіднийКасовийОрдерЖурнал.MdiParent = this.MdiParent;
					((Form_ПрихіднийКасовийОрдерЖурнал)form_ПрихіднийКасовийОрдерЖурнал).SelectPointerItem = НовіДокументи[НовіДокументи.Count - 1].GetDocumentPointer();
					form_ПрихіднийКасовийОрдерЖурнал.Show();
				}
				else
				{
					((Form_ПрихіднийКасовийОрдерЖурнал)form_ПрихіднийКасовийОрдерЖурнал).SelectPointerItem = НовіДокументи[НовіДокументи.Count - 1].GetDocumentPointer();
					((Form_ПрихіднийКасовийОрдерЖурнал)form_ПрихіднийКасовийОрдерЖурнал).LoadRecords();
				}

				this.Focus();

				foreach (Документи.ПрихіднийКасовийОрдер_Objest НовийДокумент in НовіДокументи)
				{
					Form_ПрихіднийКасовийОрдерДокумент form_ПрихіднийКасовийОрдерДокумент = new Form_ПрихіднийКасовийОрдерДокумент();
					form_ПрихіднийКасовийОрдерДокумент.MdiParent = this.MdiParent;
					form_ПрихіднийКасовийОрдерДокумент.IsNew = false;
					form_ПрихіднийКасовийОрдерДокумент.OwnerForm = (Form_ПрихіднийКасовийОрдерЖурнал)form_ПрихіднийКасовийОрдерЖурнал;
					form_ПрихіднийКасовийОрдерДокумент.Uid = НовийДокумент.UnigueID.ToString();
					form_ПрихіднийКасовийОрдерДокумент.Show();
				}
			}
		}

        #endregion

        private void dataGridViewRecords_CellClick(object sender, DataGridViewCellEventArgs e)
        {
			if (dataGridViewRecords.SelectedRows.Count > 0)
			{
				int RowIndex = dataGridViewRecords.SelectedRows[0].Index;

				SelectPointerItem = new Документи.РеалізаціяТоварівТаПослуг_Pointer(new UnigueID(dataGridViewRecords.Rows[e.RowIndex].Cells["ID"].Value.ToString()));
			}
		}

        private void сomboBox_ТипПеріоду_SelectedIndexChanged(object sender, EventArgs e)
        {
			dataGridViewRecords.Focus();

			LoadRecords();
		}
    }
}
