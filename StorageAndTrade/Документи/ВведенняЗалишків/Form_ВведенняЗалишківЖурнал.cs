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
    public partial class Form_ВведенняЗалишківЖурнал : Form
    {
        public Form_ВведенняЗалишківЖурнал()
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

		private void Form_ВведенняЗалишківЖурнал_Load(object sender, EventArgs e)
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

			Документи.ВведенняЗалишків_Select введенняЗалишків_Select = new Документи.ВведенняЗалишків_Select();
			введенняЗалишків_Select.QuerySelect.Field.AddRange(new string[] {
				"spend",
				Документи.ВведенняЗалишків_Const.Назва,
				Документи.ВведенняЗалишків_Const.НомерДок,
				Документи.ВведенняЗалишків_Const.ДатаДок,
				Документи.ВведенняЗалишків_Const.Коментар
			});

			//Контрагент
			введенняЗалишків_Select.QuerySelect.FieldAndAlias.Add(
				new NameValue<string>(Довідники.Контрагенти_Const.TABLE + "." + Довідники.Контрагенти_Const.Назва, "joinContragent"));
			введенняЗалишків_Select.QuerySelect.Joins.Add(
				new Join(Довідники.Контрагенти_Const.TABLE, Документи.ВведенняЗалишків_Const.Контрагент, Документи.ВведенняЗалишків_Const.TABLE));

			//ORDER
			введенняЗалишків_Select.QuerySelect.Order.Add(Документи.ВведенняЗалишків_Const.ДатаДок, SelectOrder.ASC);
			введенняЗалишків_Select.QuerySelect.Order.Add(Документи.ВведенняЗалишків_Const.НомерДок, SelectOrder.ASC);

			Перелічення.ТипПеріодуДляЖурналівДокументів ПеріодЖурналу =
				((NameValue<Перелічення.ТипПеріодуДляЖурналівДокументів>)сomboBox_ТипПеріоду.Items[сomboBox_ТипПеріоду.SelectedIndex]).Value;

			switch (ПеріодЖурналу)
			{

				case Перелічення.ТипПеріодуДляЖурналівДокументів.ЗПочаткуРоку:
					{
						введенняЗалишків_Select.QuerySelect.Where.Add(new Where(Документи.ВведенняЗалишків_Const.ДатаДок, Comparison.QT_EQ, new DateTime(DateTime.Now.Year, 1, 1)));
						break;
					}
				case Перелічення.ТипПеріодуДляЖурналівДокументів.Квартал:
					{
						DateTime ДатаТриМісцяНазад = DateTime.Now.AddMonths(-3);
						введенняЗалишків_Select.QuerySelect.Where.Add(new Where(Документи.ВведенняЗалишків_Const.ДатаДок, Comparison.QT_EQ, new DateTime(ДатаТриМісцяНазад.Year, ДатаТриМісцяНазад.Month, 1)));
						break;
					}
				case Перелічення.ТипПеріодуДляЖурналівДокументів.ЗМинулогоМісяця:
					{
						DateTime ДатаМісцьНазад = DateTime.Now.AddMonths(-1);
						введенняЗалишків_Select.QuerySelect.Where.Add(new Where(Документи.ВведенняЗалишків_Const.ДатаДок, Comparison.QT_EQ, new DateTime(ДатаМісцьНазад.Year, ДатаМісцьНазад.Month, 1)));
						break;
					}
				case Перелічення.ТипПеріодуДляЖурналівДокументів.ЗПочаткуМісяця:
					{
						введенняЗалишків_Select.QuerySelect.Where.Add(new Where(Документи.ВведенняЗалишків_Const.ДатаДок, Comparison.QT_EQ, new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1)));
						break;
					}
				case Перелічення.ТипПеріодуДляЖурналівДокументів.ЗПочаткуТижня:
					{
						DateTime СімДнівНазад = DateTime.Now.AddDays(-7);
						введенняЗалишків_Select.QuerySelect.Where.Add(new Where(Документи.ВведенняЗалишків_Const.ДатаДок, Comparison.QT_EQ, new DateTime(СімДнівНазад.Year, СімДнівНазад.Month, СімДнівНазад.Day)));
						break;
					}
				case Перелічення.ТипПеріодуДляЖурналівДокументів.ПоточнийДень:
					{
						введенняЗалишків_Select.QuerySelect.Where.Add(new Where(Документи.ВведенняЗалишків_Const.ДатаДок, Comparison.QT_EQ, new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day)));
						break;
					}
			}

			введенняЗалишків_Select.Select();
			while (введенняЗалишків_Select.MoveNext())
			{
				Документи.ВведенняЗалишків_Pointer cur = введенняЗалишків_Select.Current;

				RecordsBindingList.Add(new Записи
				{
					ID = cur.UnigueID.ToString(),
					Назва = cur.Fields[Документи.ВведенняЗалишків_Const.Назва].ToString(),
					НомерДок = cur.Fields[Документи.ВведенняЗалишків_Const.НомерДок].ToString(),
					ДатаДок = cur.Fields[Документи.ВведенняЗалишків_Const.ДатаДок].ToString(),
					Контрагент = cur.Fields["joinContragent"].ToString(),
					Коментар = cur.Fields[Документи.ВведенняЗалишків_Const.Коментар].ToString(),
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
					DocumentPointerItem = new Документи.ВведенняЗалишків_Pointer(new UnigueID(Uid));
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
			Form_ВведенняЗалишківДокумент form_ВведенняЗалишківДокумент = new Form_ВведенняЗалишківДокумент();
			form_ВведенняЗалишківДокумент.MdiParent = this.MdiParent;
			form_ВведенняЗалишківДокумент.IsNew = true;
			form_ВведенняЗалишківДокумент.OwnerForm = this;
			form_ВведенняЗалишківДокумент.Show();
        }

        private void toolStripButtonEdit_Click(object sender, EventArgs e)
        {
			if (dataGridViewRecords.SelectedRows.Count > 0)
			{
				int RowIndex = dataGridViewRecords.SelectedRows[0].Index;

				Form_ВведенняЗалишківДокумент form_ВведенняЗалишківДокумент = new Form_ВведенняЗалишківДокумент();
				form_ВведенняЗалишківДокумент.MdiParent = this.MdiParent;
				form_ВведенняЗалишківДокумент.IsNew = false;
				form_ВведенняЗалишківДокумент.OwnerForm = this;
				form_ВведенняЗалишківДокумент.Uid = dataGridViewRecords.Rows[RowIndex].Cells["ID"].Value.ToString();
				form_ВведенняЗалишківДокумент.Show();
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

                    Документи.ВведенняЗалишків_Objest введенняЗалишків_Objest = new Документи.ВведенняЗалишків_Objest();
                    if (введенняЗалишків_Objest.Read(new UnigueID(uid)))
                    {
						Документи.ВведенняЗалишків_Objest введенняЗалишків_Objest_Новий = введенняЗалишків_Objest.Copy();
						введенняЗалишків_Objest_Новий.Назва += " *";
						введенняЗалишків_Objest_Новий.ДатаДок = DateTime.Now;
						введенняЗалишків_Objest_Новий.НомерДок = (++Константи.НумераціяДокументів.ВведенняЗалишків_Const).ToString("D8");

						//Зчитати та скопіювати табличну частину Товари
						введенняЗалишків_Objest.Товари_TablePart.Read();
						введенняЗалишків_Objest_Новий.Товари_TablePart.Records = введенняЗалишків_Objest.Товари_TablePart.Copy();
						введенняЗалишків_Objest_Новий.Товари_TablePart.Save(true);
						введенняЗалишків_Objest_Новий.Save();

						//Зчитати та скопіювати табличну частину Каси
						введенняЗалишків_Objest.Каси_TablePart.Read();
						введенняЗалишків_Objest_Новий.Каси_TablePart.Records = введенняЗалишків_Objest.Каси_TablePart.Copy();
						введенняЗалишків_Objest_Новий.Каси_TablePart.Save(true);
						введенняЗалишків_Objest_Новий.Save();

						//Зчитати та скопіювати табличну частину БанківськіРахунки
						введенняЗалишків_Objest.БанківськіРахунки_TablePart.Read();
						введенняЗалишків_Objest_Новий.БанківськіРахунки_TablePart.Records = введенняЗалишків_Objest.БанківськіРахунки_TablePart.Copy();
						введенняЗалишків_Objest_Новий.БанківськіРахунки_TablePart.Save(true);
						введенняЗалишків_Objest_Новий.Save();

						//Зчитати та скопіювати табличну частину РозрахункиЗКонтрагентами
						введенняЗалишків_Objest.РозрахункиЗКонтрагентами_TablePart.Read();
						введенняЗалишків_Objest_Новий.РозрахункиЗКонтрагентами_TablePart.Records = введенняЗалишків_Objest.РозрахункиЗКонтрагентами_TablePart.Copy();
						введенняЗалишків_Objest_Новий.РозрахункиЗКонтрагентами_TablePart.Save(true);
						введенняЗалишків_Objest_Новий.Save();

						SelectPointerItem = введенняЗалишків_Objest_Новий.GetDocumentPointer();
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

                    Документи.ВведенняЗалишків_Objest введенняЗалишків_Objest = new Документи.ВведенняЗалишків_Objest();
                    if (введенняЗалишків_Objest.Read(new UnigueID(uid)))
                    {
						введенняЗалишків_Objest.Delete();
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

				РухДокументівПоРегістрах.PrintRecords(new Документи.ВведенняЗалишків_Pointer(new UnigueID(uid)));
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

					Документи.ВведенняЗалишків_Pointer введенняЗалишків_Pointer = new Документи.ВведенняЗалишків_Pointer(new UnigueID(uid));
					Документи.ВведенняЗалишків_Objest введенняЗалишків_Objest = введенняЗалишків_Pointer.GetDocumentObject(true);

					if (spend)
						try
						{
							//Проведення
							введенняЗалишків_Objest.SpendTheDocument(введенняЗалишків_Objest.ДатаДок);
						}
						catch (Exception exp)
						{
							введенняЗалишків_Objest.ClearSpendTheDocument();
							MessageBox.Show(exp.Message);
							return;
						}
					else
						введенняЗалишків_Objest.ClearSpendTheDocument();
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

				SelectPointerItem = new Документи.ВведенняЗалишків_Pointer(new UnigueID(dataGridViewRecords.Rows[e.RowIndex].Cells["ID"].Value.ToString()));
			}
		}

        private void сomboBox_ТипПеріоду_SelectedIndexChanged(object sender, EventArgs e)
        {
			dataGridViewRecords.Focus();

			LoadRecords();
		}
    }
}
