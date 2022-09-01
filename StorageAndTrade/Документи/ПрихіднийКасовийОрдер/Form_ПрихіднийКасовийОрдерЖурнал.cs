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
    public partial class Form_ПрихіднийКасовийОрдерЖурнал : Form
    {
        public Form_ПрихіднийКасовийОрдерЖурнал()
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
			dataGridViewRecords.Columns["Каса"].Width = 300;

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

		private void Form_ПрихіднийКасовийОрдерЖурнал_Load(object sender, EventArgs e)
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

			Документи.ПрихіднийКасовийОрдер_Select прихіднийКасовийОрдер_Select = new Документи.ПрихіднийКасовийОрдер_Select();
			прихіднийКасовийОрдер_Select.QuerySelect.Field.AddRange(new string[] {
				"spend",
				Документи.ПрихіднийКасовийОрдер_Const.Назва,
				Документи.ПрихіднийКасовийОрдер_Const.НомерДок,
				Документи.ПрихіднийКасовийОрдер_Const.ДатаДок,
				Документи.ПрихіднийКасовийОрдер_Const.СумаДокументу,
				Документи.ПрихіднийКасовийОрдер_Const.Коментар,
				Документи.ПрихіднийКасовийОрдер_Const.Каса
			});

			//Контрагент
			прихіднийКасовийОрдер_Select.QuerySelect.FieldAndAlias.Add(
				new NameValue<string>(Довідники.Контрагенти_Const.TABLE + "." + Довідники.Контрагенти_Const.Назва, "joinContragent"));
			прихіднийКасовийОрдер_Select.QuerySelect.Joins.Add(
				new Join(Довідники.Контрагенти_Const.TABLE, Документи.ПрихіднийКасовийОрдер_Const.Контрагент, Документи.ПрихіднийКасовийОрдер_Const.TABLE));

			//Каса
			прихіднийКасовийОрдер_Select.QuerySelect.FieldAndAlias.Add(
				new NameValue<string>(Довідники.Каси_Const.TABLE + "." + Довідники.Каси_Const.Назва, "joinCasa"));
			прихіднийКасовийОрдер_Select.QuerySelect.Joins.Add(
				new Join(Довідники.Каси_Const.TABLE, Документи.ПрихіднийКасовийОрдер_Const.Каса, Документи.ПрихіднийКасовийОрдер_Const.TABLE));

			//ORDER
			прихіднийКасовийОрдер_Select.QuerySelect.Order.Add(Документи.ПрихіднийКасовийОрдер_Const.ДатаДок, SelectOrder.ASC);
			прихіднийКасовийОрдер_Select.QuerySelect.Order.Add(Документи.ПрихіднийКасовийОрдер_Const.НомерДок, SelectOrder.ASC);

			Перелічення.ТипПеріодуДляЖурналівДокументів ПеріодЖурналу =
				((NameValue<Перелічення.ТипПеріодуДляЖурналівДокументів>)сomboBox_ТипПеріоду.Items[сomboBox_ТипПеріоду.SelectedIndex]).Value;

			switch (ПеріодЖурналу)
			{

				case Перелічення.ТипПеріодуДляЖурналівДокументів.ЗПочаткуРоку:
					{
						прихіднийКасовийОрдер_Select.QuerySelect.Where.Add(new Where(Документи.ПрихіднийКасовийОрдер_Const.ДатаДок, Comparison.QT_EQ, new DateTime(DateTime.Now.Year, 1, 1)));
						break;
					}
				case Перелічення.ТипПеріодуДляЖурналівДокументів.Квартал:
					{
						DateTime ДатаТриМісцяНазад = DateTime.Now.AddMonths(-3);
						прихіднийКасовийОрдер_Select.QuerySelect.Where.Add(new Where(Документи.ПрихіднийКасовийОрдер_Const.ДатаДок, Comparison.QT_EQ, new DateTime(ДатаТриМісцяНазад.Year, ДатаТриМісцяНазад.Month, 1)));
						break;
					}
				case Перелічення.ТипПеріодуДляЖурналівДокументів.ЗМинулогоМісяця:
					{
						DateTime ДатаМісцьНазад = DateTime.Now.AddMonths(-1);
						прихіднийКасовийОрдер_Select.QuerySelect.Where.Add(new Where(Документи.ПрихіднийКасовийОрдер_Const.ДатаДок, Comparison.QT_EQ, new DateTime(ДатаМісцьНазад.Year, ДатаМісцьНазад.Month, 1)));
						break;
					}
				case Перелічення.ТипПеріодуДляЖурналівДокументів.ЗПочаткуМісяця:
					{
						прихіднийКасовийОрдер_Select.QuerySelect.Where.Add(new Where(Документи.ПрихіднийКасовийОрдер_Const.ДатаДок, Comparison.QT_EQ, new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1)));
						break;
					}
				case Перелічення.ТипПеріодуДляЖурналівДокументів.ЗПочаткуТижня:
					{
						DateTime СімДнівНазад = DateTime.Now.AddDays(-7);
						прихіднийКасовийОрдер_Select.QuerySelect.Where.Add(new Where(Документи.ПрихіднийКасовийОрдер_Const.ДатаДок, Comparison.QT_EQ, new DateTime(СімДнівНазад.Year, СімДнівНазад.Month, СімДнівНазад.Day)));
						break;
					}
				case Перелічення.ТипПеріодуДляЖурналівДокументів.ПоточнийДень:
					{
						прихіднийКасовийОрдер_Select.QuerySelect.Where.Add(new Where(Документи.ПрихіднийКасовийОрдер_Const.ДатаДок, Comparison.QT_EQ, new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day)));
						break;
					}
			}

			прихіднийКасовийОрдер_Select.Select();
			while (прихіднийКасовийОрдер_Select.MoveNext())
			{
				Документи.ПрихіднийКасовийОрдер_Pointer cur = прихіднийКасовийОрдер_Select.Current;

				RecordsBindingList.Add(new Записи
				{
					ID = cur.UnigueID.ToString(),
					Назва = cur.Fields[Документи.ПрихіднийКасовийОрдер_Const.Назва].ToString(),
					НомерДок = cur.Fields[Документи.ПрихіднийКасовийОрдер_Const.НомерДок].ToString(),
					ДатаДок = cur.Fields[Документи.ПрихіднийКасовийОрдер_Const.ДатаДок].ToString(),
					Контрагент = cur.Fields["joinContragent"].ToString(),
					Каса = cur.Fields["joinCasa"].ToString(),
					Сума = Math.Round((decimal)cur.Fields[Документи.ПрихіднийКасовийОрдер_Const.СумаДокументу], 2),
					Коментар = cur.Fields[Документи.ПрихіднийКасовийОрдер_Const.Коментар].ToString(),
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
			public string Каса { get; set; }
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
					DocumentPointerItem = new Документи.ПрихіднийКасовийОрдер_Pointer(new UnigueID(Uid));
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
			Form_ПрихіднийКасовийОрдерДокумент form_ПрихіднийКасовийОрдерДокумент = new Form_ПрихіднийКасовийОрдерДокумент();
			form_ПрихіднийКасовийОрдерДокумент.MdiParent = this.MdiParent;
			form_ПрихіднийКасовийОрдерДокумент.IsNew = true;
			form_ПрихіднийКасовийОрдерДокумент.OwnerForm = this;
			form_ПрихіднийКасовийОрдерДокумент.Show();
        }

        private void toolStripButtonEdit_Click(object sender, EventArgs e)
        {
			if (dataGridViewRecords.SelectedRows.Count > 0)
			{
				int RowIndex = dataGridViewRecords.SelectedRows[0].Index;

				Form_ПрихіднийКасовийОрдерДокумент form_ПрихіднийКасовийОрдерДокумент = new Form_ПрихіднийКасовийОрдерДокумент();
				form_ПрихіднийКасовийОрдерДокумент.MdiParent = this.MdiParent;
				form_ПрихіднийКасовийОрдерДокумент.IsNew = false;
				form_ПрихіднийКасовийОрдерДокумент.OwnerForm = this;
				form_ПрихіднийКасовийОрдерДокумент.Uid = dataGridViewRecords.Rows[RowIndex].Cells["ID"].Value.ToString();
				form_ПрихіднийКасовийОрдерДокумент.Show();
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

					Документи.ПрихіднийКасовийОрдер_Objest прихіднийКасовийОрдер_Objest = new Документи.ПрихіднийКасовийОрдер_Objest();
                    if (прихіднийКасовийОрдер_Objest.Read(new UnigueID(uid)))
                    {
						Документи.ПрихіднийКасовийОрдер_Objest прихіднийКасовийОрдер_Objest_Новий = прихіднийКасовийОрдер_Objest.Copy();
						прихіднийКасовийОрдер_Objest_Новий.Назва += " *";
						прихіднийКасовийОрдер_Objest_Новий.ДатаДок = DateTime.Now;
						прихіднийКасовийОрдер_Objest_Новий.НомерДок = (++Константи.НумераціяДокументів.ПрихіднийКасовийОрдер_Const).ToString("D8");

						//Зчитати та скопіювати табличну частину
						прихіднийКасовийОрдер_Objest.РозшифруванняПлатежу_TablePart.Read();
						прихіднийКасовийОрдер_Objest_Новий.РозшифруванняПлатежу_TablePart.Records = прихіднийКасовийОрдер_Objest.РозшифруванняПлатежу_TablePart.Copy();
						прихіднийКасовийОрдер_Objest_Новий.РозшифруванняПлатежу_TablePart.Save(true);
						прихіднийКасовийОрдер_Objest_Новий.Save();

						SelectPointerItem = прихіднийКасовийОрдер_Objest_Новий.GetDocumentPointer();
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

					Документи.ПрихіднийКасовийОрдер_Objest прихіднийКасовийОрдер_Objest = new Документи.ПрихіднийКасовийОрдер_Objest();
					if (прихіднийКасовийОрдер_Objest.Read(new UnigueID(uid)))
                    {
						прихіднийКасовийОрдер_Objest.Delete();
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

				РухДокументівПоРегістрах.PrintRecords(new Документи.ПрихіднийКасовийОрдер_Pointer(new UnigueID(uid)));
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

					Документи.ПрихіднийКасовийОрдер_Pointer прихіднийКасовийОрдер_Pointer = new Документи.ПрихіднийКасовийОрдер_Pointer(new UnigueID(uid));
					Документи.ПрихіднийКасовийОрдер_Objest прихіднийКасовийОрдер_Objest = прихіднийКасовийОрдер_Pointer.GetDocumentObject(true);

					if (spend)
						try
						{
							//Проведення
							прихіднийКасовийОрдер_Objest.SpendTheDocument(прихіднийКасовийОрдер_Objest.ДатаДок);
						}
						catch (Exception exp)
						{
							прихіднийКасовийОрдер_Objest.ClearSpendTheDocument();
							MessageBox.Show(exp.Message);
							return;
						}
					else
						прихіднийКасовийОрдер_Objest.ClearSpendTheDocument();
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

        private void dataGridViewRecords_CellClick(object sender, DataGridViewCellEventArgs e)
        {
			if (dataGridViewRecords.SelectedRows.Count > 0)
			{
				int RowIndex = dataGridViewRecords.SelectedRows[0].Index;

				SelectPointerItem = new Документи.ПрихіднийКасовийОрдер_Pointer(new UnigueID(dataGridViewRecords.Rows[e.RowIndex].Cells["ID"].Value.ToString()));
			}
		}

        private void сomboBox_ТипПеріоду_SelectedIndexChanged(object sender, EventArgs e)
        {
			dataGridViewRecords.Focus();

			LoadRecords();
		}
    }
}
