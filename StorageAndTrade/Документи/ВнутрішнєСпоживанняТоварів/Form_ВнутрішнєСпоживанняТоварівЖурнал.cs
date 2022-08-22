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
    public partial class Form_ВнутрішнєСпоживанняТоварівЖурнал : Form
    {
        public Form_ВнутрішнєСпоживанняТоварівЖурнал()
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

		private void Form_Form_ВнутрішнєСпоживанняТоварівЖурнал_Load(object sender, EventArgs e)
        {
			LoadRecords();
		}

		private BindingList<Записи> RecordsBindingList { get; set; }

		public void LoadRecords()
		{
			RecordsBindingList.Clear();

			Документи.ВнутрішнєСпоживанняТоварів_Select внутрішнєСпоживанняТоварів_Select = new Документи.ВнутрішнєСпоживанняТоварів_Select();
			внутрішнєСпоживанняТоварів_Select.QuerySelect.Field.AddRange(new string[] {
				"spend",
				Документи.ВнутрішнєСпоживанняТоварів_Const.Назва,
				Документи.ВнутрішнєСпоживанняТоварів_Const.НомерДок,
				Документи.ВнутрішнєСпоживанняТоварів_Const.ДатаДок,
				Документи.ВнутрішнєСпоживанняТоварів_Const.СумаДокументу,
				Документи.ВнутрішнєСпоживанняТоварів_Const.Коментар
			});

			//ORDER
			внутрішнєСпоживанняТоварів_Select.QuerySelect.Order.Add(Документи.ВнутрішнєСпоживанняТоварів_Const.ДатаДок, SelectOrder.ASC);
			внутрішнєСпоживанняТоварів_Select.QuerySelect.Order.Add(Документи.ВнутрішнєСпоживанняТоварів_Const.НомерДок, SelectOrder.ASC);

			внутрішнєСпоживанняТоварів_Select.Select();
			while (внутрішнєСпоживанняТоварів_Select.MoveNext())
			{
				Документи.ВнутрішнєСпоживанняТоварів_Pointer cur = внутрішнєСпоживанняТоварів_Select.Current;

				RecordsBindingList.Add(new Записи
				{
					ID = cur.UnigueID.ToString(),
					Назва = cur.Fields[Документи.ВнутрішнєСпоживанняТоварів_Const.Назва].ToString(),
					НомерДок = cur.Fields[Документи.ВнутрішнєСпоживанняТоварів_Const.НомерДок].ToString(),
					ДатаДок = cur.Fields[Документи.ВнутрішнєСпоживанняТоварів_Const.ДатаДок].ToString(),
					Сума = Math.Round((decimal)cur.Fields[Документи.ВнутрішнєСпоживанняТоварів_Const.СумаДокументу], 2),
					Коментар = cur.Fields[Документи.ВнутрішнєСпоживанняТоварів_Const.Коментар].ToString(),
					Проведений = (bool)cur.Fields["spend"]
				});
			}

			if ((DocumentPointerItem != null || SelectPointerItem != null) && dataGridViewRecords.Rows.Count > 0)
			{
				string UidSelect = SelectPointerItem != null ? SelectPointerItem.UnigueID.ToString() : DocumentPointerItem.UnigueID.ToString();

				if (UidSelect != Guid.Empty.ToString())
					ФункціїДляДовідниківТаДокументів.ВиділитиЕлементСписку(dataGridViewRecords, "ID", UidSelect);
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
					DocumentPointerItem = new Документи.ВнутрішнєСпоживанняТоварів_Pointer(new UnigueID(Uid));
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
			Form_ВнутрішнєСпоживанняТоварівДокумент form_ВнутрішнєСпоживанняТоварівДокумент = new Form_ВнутрішнєСпоживанняТоварівДокумент();
			form_ВнутрішнєСпоживанняТоварівДокумент.MdiParent = this.MdiParent;
			form_ВнутрішнєСпоживанняТоварівДокумент.IsNew = true;
			form_ВнутрішнєСпоживанняТоварівДокумент.OwnerForm = this;
			form_ВнутрішнєСпоживанняТоварівДокумент.Show();
        }

        private void toolStripButtonEdit_Click(object sender, EventArgs e)
        {
			if (dataGridViewRecords.SelectedRows.Count > 0)
			{
				int RowIndex = dataGridViewRecords.SelectedRows[0].Index;

				Form_ВнутрішнєСпоживанняТоварівДокумент form_ВнутрішнєСпоживанняТоварівДокумент = new Form_ВнутрішнєСпоживанняТоварівДокумент();
				form_ВнутрішнєСпоживанняТоварівДокумент.MdiParent = this.MdiParent;
				form_ВнутрішнєСпоживанняТоварівДокумент.IsNew = false;
				form_ВнутрішнєСпоживанняТоварівДокумент.OwnerForm = this;
				form_ВнутрішнєСпоживанняТоварівДокумент.Uid = dataGridViewRecords.Rows[RowIndex].Cells["ID"].Value.ToString();
				form_ВнутрішнєСпоживанняТоварівДокумент.Show();
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

                    Документи.ВнутрішнєСпоживанняТоварів_Objest внутрішнєСпоживанняТоварів_Objest = new Документи.ВнутрішнєСпоживанняТоварів_Objest();
                    if (внутрішнєСпоживанняТоварів_Objest.Read(new UnigueID(uid)))
                    {
						Документи.ВнутрішнєСпоживанняТоварів_Objest внутрішнєСпоживанняТоварів_Objest_Новий = внутрішнєСпоживанняТоварів_Objest.Copy();
						внутрішнєСпоживанняТоварів_Objest_Новий.Назва += " *";
						внутрішнєСпоживанняТоварів_Objest_Новий.ДатаДок = DateTime.Now;
						внутрішнєСпоживанняТоварів_Objest_Новий.НомерДок = (++Константи.НумераціяДокументів.ВнутрішнєСпоживанняТоварів_Const).ToString("D8");

						//Зчитати та скопіювати табличну частину Товари
						внутрішнєСпоживанняТоварів_Objest.Товари_TablePart.Read();
						внутрішнєСпоживанняТоварів_Objest_Новий.Товари_TablePart.Records = внутрішнєСпоживанняТоварів_Objest.Товари_TablePart.Copy();
						внутрішнєСпоживанняТоварів_Objest_Новий.Товари_TablePart.Save(true);
						внутрішнєСпоживанняТоварів_Objest_Новий.Save();

						SelectPointerItem = внутрішнєСпоживанняТоварів_Objest_Новий.GetDocumentPointer();
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

                    Документи.ВнутрішнєСпоживанняТоварів_Objest внутрішнєСпоживанняТоварів_Objest = new Документи.ВнутрішнєСпоживанняТоварів_Objest();
                    if (внутрішнєСпоживанняТоварів_Objest.Read(new UnigueID(uid)))
                    {
						внутрішнєСпоживанняТоварів_Objest.Delete();
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

				Звіти.РухДокументівПоРегістрах.PrintRecords(new Документи.ВнутрішнєСпоживанняТоварів_Pointer(new UnigueID(uid)));
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

					Документи.ВнутрішнєСпоживанняТоварів_Pointer внутрішнєСпоживанняТоварів_Pointer = new Документи.ВнутрішнєСпоживанняТоварів_Pointer(new UnigueID(uid));
					Документи.ВнутрішнєСпоживанняТоварів_Objest внутрішнєСпоживанняТоварів_Objest = внутрішнєСпоживанняТоварів_Pointer.GetDocumentObject(true);

					if (spend)
						try
						{
							//Проведення
							внутрішнєСпоживанняТоварів_Objest.SpendTheDocument(внутрішнєСпоживанняТоварів_Objest.ДатаДок);
						}
						catch (Exception exp)
						{
							внутрішнєСпоживанняТоварів_Objest.ClearSpendTheDocument();
							MessageBox.Show(exp.Message);
							return;
						}
					else
						внутрішнєСпоживанняТоварів_Objest.ClearSpendTheDocument();
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
