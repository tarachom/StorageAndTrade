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
    public partial class Form_ПсуванняТоварівЖурнал : Form
    {
        public Form_ПсуванняТоварівЖурнал()
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
			dataGridViewRecords.Columns["Склад"].Width = 300;

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

		private void Form_ПсуванняТоварівЖурнал_Load(object sender, EventArgs e)
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

			Документи.ПсуванняТоварів_Select псуванняТоварів_Select = new Документи.ПсуванняТоварів_Select();
			псуванняТоварів_Select.QuerySelect.Field.AddRange(new string[] {
				"spend",
				Документи.ПсуванняТоварів_Const.Назва,
				Документи.ПсуванняТоварів_Const.НомерДок,
				Документи.ПсуванняТоварів_Const.ДатаДок,
				Документи.ПсуванняТоварів_Const.СумаДокументу,
				Документи.ПсуванняТоварів_Const.Коментар
			});

			//Контрагент
			псуванняТоварів_Select.QuerySelect.FieldAndAlias.Add(
				new NameValue<string>(Довідники.Склади_Const.TABLE + "." + Довідники.Склади_Const.Назва, "sklad"));
			псуванняТоварів_Select.QuerySelect.Joins.Add(
				new Join(Довідники.Склади_Const.TABLE, Документи.ПсуванняТоварів_Const.Склад, Документи.ПсуванняТоварів_Const.TABLE));

			//ORDER
			псуванняТоварів_Select.QuerySelect.Order.Add(Документи.ПсуванняТоварів_Const.ДатаДок, SelectOrder.ASC);
			псуванняТоварів_Select.QuerySelect.Order.Add(Документи.ПсуванняТоварів_Const.НомерДок, SelectOrder.ASC);

			Перелічення.ТипПеріодуДляЖурналівДокументів ПеріодЖурналу =
				((NameValue<Перелічення.ТипПеріодуДляЖурналівДокументів>)сomboBox_ТипПеріоду.Items[сomboBox_ТипПеріоду.SelectedIndex]).Value;

			switch (ПеріодЖурналу)
			{

				case Перелічення.ТипПеріодуДляЖурналівДокументів.ЗПочаткуРоку:
					{
						псуванняТоварів_Select.QuerySelect.Where.Add(new Where(Документи.ПсуванняТоварів_Const.ДатаДок, Comparison.QT_EQ, new DateTime(DateTime.Now.Year, 1, 1)));
						break;
					}
				case Перелічення.ТипПеріодуДляЖурналівДокументів.Квартал:
					{
						DateTime ДатаТриМісцяНазад = DateTime.Now.AddMonths(-3);
						псуванняТоварів_Select.QuerySelect.Where.Add(new Where(Документи.ПсуванняТоварів_Const.ДатаДок, Comparison.QT_EQ, new DateTime(ДатаТриМісцяНазад.Year, ДатаТриМісцяНазад.Month, 1)));
						break;
					}
				case Перелічення.ТипПеріодуДляЖурналівДокументів.ЗМинулогоМісяця:
					{
						DateTime ДатаМісцьНазад = DateTime.Now.AddMonths(-1);
						псуванняТоварів_Select.QuerySelect.Where.Add(new Where(Документи.ПсуванняТоварів_Const.ДатаДок, Comparison.QT_EQ, new DateTime(ДатаМісцьНазад.Year, ДатаМісцьНазад.Month, 1)));
						break;
					}
				case Перелічення.ТипПеріодуДляЖурналівДокументів.ЗПочаткуМісяця:
					{
						псуванняТоварів_Select.QuerySelect.Where.Add(new Where(Документи.ПсуванняТоварів_Const.ДатаДок, Comparison.QT_EQ, new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1)));
						break;
					}
				case Перелічення.ТипПеріодуДляЖурналівДокументів.ЗПочаткуТижня:
					{
						DateTime СімДнівНазад = DateTime.Now.AddDays(-7);
						псуванняТоварів_Select.QuerySelect.Where.Add(new Where(Документи.ПсуванняТоварів_Const.ДатаДок, Comparison.QT_EQ, new DateTime(СімДнівНазад.Year, СімДнівНазад.Month, СімДнівНазад.Day)));
						break;
					}
				case Перелічення.ТипПеріодуДляЖурналівДокументів.ПоточнийДень:
					{
						псуванняТоварів_Select.QuerySelect.Where.Add(new Where(Документи.ПсуванняТоварів_Const.ДатаДок, Comparison.QT_EQ, new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day)));
						break;
					}
			}

			псуванняТоварів_Select.Select();
			while (псуванняТоварів_Select.MoveNext())
			{
				Документи.ПсуванняТоварів_Pointer cur = псуванняТоварів_Select.Current;

				RecordsBindingList.Add(new Записи
				{
					ID = cur.UnigueID.ToString(),
					Назва = cur.Fields[Документи.ПсуванняТоварів_Const.Назва].ToString(),
					НомерДок = cur.Fields[Документи.ПсуванняТоварів_Const.НомерДок].ToString(),
					ДатаДок = cur.Fields[Документи.ПсуванняТоварів_Const.ДатаДок].ToString(),
					Склад = cur.Fields["sklad"].ToString(),
					Сума = Math.Round((decimal)cur.Fields[Документи.ПсуванняТоварів_Const.СумаДокументу], 2),
					Коментар = cur.Fields[Документи.ПсуванняТоварів_Const.Коментар].ToString(),
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
			public string Склад { get; set; }
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
			Form_ПсуванняТоварівДокумент form_ПсуванняТоварівДокумент = new Form_ПсуванняТоварівДокумент();
			form_ПсуванняТоварівДокумент.MdiParent = this.MdiParent;
			form_ПсуванняТоварівДокумент.IsNew = true;
			form_ПсуванняТоварівДокумент.OwnerForm = this;
			form_ПсуванняТоварівДокумент.Show();
        }

        private void toolStripButtonEdit_Click(object sender, EventArgs e)
        {
			if (dataGridViewRecords.SelectedRows.Count > 0)
			{
				int RowIndex = dataGridViewRecords.SelectedRows[0].Index;

				Form_ПсуванняТоварівДокумент form_ПсуванняТоварівДокумент = new Form_ПсуванняТоварівДокумент();
				form_ПсуванняТоварівДокумент.MdiParent = this.MdiParent;
				form_ПсуванняТоварівДокумент.IsNew = false;
				form_ПсуванняТоварівДокумент.OwnerForm = this;
				form_ПсуванняТоварівДокумент.Uid = dataGridViewRecords.Rows[RowIndex].Cells["ID"].Value.ToString();
				form_ПсуванняТоварівДокумент.Show();
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

                    Документи.ПсуванняТоварів_Objest псуванняТоварів_Objest = new Документи.ПсуванняТоварів_Objest();
                    if (псуванняТоварів_Objest.Read(new UnigueID(uid)))
                    {
						Документи.ПсуванняТоварів_Objest псуванняТоварів_Objest_Новий = псуванняТоварів_Objest.Copy();
						псуванняТоварів_Objest_Новий.Назва += " *";
						псуванняТоварів_Objest_Новий.ДатаДок = DateTime.Now;
						псуванняТоварів_Objest_Новий.НомерДок = (++Константи.НумераціяДокументів.ПсуванняТоварів_Const).ToString("D8");

						//Зчитати та скопіювати табличну частину Товари
						псуванняТоварів_Objest.Товари_TablePart.Read();
						псуванняТоварів_Objest_Новий.Товари_TablePart.Records = псуванняТоварів_Objest.Товари_TablePart.Copy();
						псуванняТоварів_Objest_Новий.Товари_TablePart.Save(true);
						псуванняТоварів_Objest_Новий.Save();

						SelectPointerItem = псуванняТоварів_Objest_Новий.GetDocumentPointer();
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

                    Документи.ПсуванняТоварів_Objest ПсуванняТоварів_Objest = new Документи.ПсуванняТоварів_Objest();
                    if (ПсуванняТоварів_Objest.Read(new UnigueID(uid)))
                    {
						ПсуванняТоварів_Objest.Delete();
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

				РухДокументівПоРегістрах.PrintRecords(new Документи.ПсуванняТоварів_Pointer(new UnigueID(uid)));
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

					Документи.ПсуванняТоварів_Pointer псуванняТоварів_Pointer = new Документи.ПсуванняТоварів_Pointer(new UnigueID(uid));
					Документи.ПсуванняТоварів_Objest псуванняТоварів_Objest = псуванняТоварів_Pointer.GetDocumentObject(true);

					if (spend)
						try
						{
							//Проведення
							псуванняТоварів_Objest.SpendTheDocument(псуванняТоварів_Objest.ДатаДок);
						}
						catch (Exception exp)
						{
							псуванняТоварів_Objest.ClearSpendTheDocument();
							MessageBox.Show(exp.Message);
							return;
						}
					else
						псуванняТоварів_Objest.ClearSpendTheDocument();
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

        private void dataGridViewRecords_MouseClick(object sender, MouseEventArgs e)
        {
			//if (dataGridViewRecords.SelectedCells.Count > 0 && e.Button == MouseButtons.Right)
			//{
			//	DataGridViewRow row = dataGridViewRecords.SelectedRows[0];
			//	string uid = row.Cells["ID"].Value.ToString();

			//	//Документи.ЗамовленняКлієнта_Pointer замовленняКлієнта_Pointer = new Документи.ЗамовленняКлієнта_Pointer(new UnigueID(uid));
			//	//Документи.ЗамовленняКлієнта_Objest замовленняКлієнта_Objest = замовленняКлієнта_Pointer.GetDocumentObject(true);

			//	Rectangle rectangle = dataGridViewRecords.GetCellDisplayRectangle(dataGridViewRecords.CurrentCell.ColumnIndex, dataGridViewRecords.CurrentCell.RowIndex, true);
			//	rectangle.Offset(0, 0);
			//	Point point = dataGridViewRecords.PointToScreen(rectangle.Location);

			//	ContextMenuStrip contextMenu = new ContextMenuStrip();

			//	ToolStripMenuItem select = new ToolStripMenuItem("Відкрити список");
			//	select.Image = Properties.Resources.data;
			//	select.Name = "";
			//	//select.Tag = tag;
			//	contextMenu.Items.Add(select);

			//	contextMenu.Show(point);
			//}
		}

        private void dataGridViewRecords_CellClick(object sender, DataGridViewCellEventArgs e)
        {
			if (dataGridViewRecords.SelectedRows.Count > 0)
			{
				int RowIndex = dataGridViewRecords.SelectedRows[0].Index;

				SelectPointerItem = new Документи.ПсуванняТоварів_Pointer(new UnigueID(dataGridViewRecords.Rows[e.RowIndex].Cells["ID"].Value.ToString()));
			}
		}

        private void сomboBox_ТипПеріоду_SelectedIndexChanged(object sender, EventArgs e)
        {
			dataGridViewRecords.Focus();

			LoadRecords();
		}
    }
}
