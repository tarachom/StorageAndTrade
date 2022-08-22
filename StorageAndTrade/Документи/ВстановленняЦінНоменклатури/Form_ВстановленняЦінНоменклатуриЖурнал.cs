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
    public partial class Form_ВстановленняЦінНоменклатуриЖурнал : Form
    {
        public Form_ВстановленняЦінНоменклатуриЖурнал()
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

		private void Form_ВстановленняЦінНоменклатуриЖурнал_Load(object sender, EventArgs e)
        {
			LoadRecords();
		}

		private BindingList<Записи> RecordsBindingList { get; set; }

		public void LoadRecords()
		{
			RecordsBindingList.Clear();

			Документи.ВстановленняЦінНоменклатури_Select встановленняЦінНоменклатури_Select = new Документи.ВстановленняЦінНоменклатури_Select();
			встановленняЦінНоменклатури_Select.QuerySelect.Field.AddRange(new string[] {
				"spend",
				Документи.ВстановленняЦінНоменклатури_Const.Назва,
				Документи.ВстановленняЦінНоменклатури_Const.НомерДок,
				Документи.ВстановленняЦінНоменклатури_Const.ДатаДок,
				Документи.ВстановленняЦінНоменклатури_Const.Коментар
			});

			//ORDER
			встановленняЦінНоменклатури_Select.QuerySelect.Order.Add(Документи.ВстановленняЦінНоменклатури_Const.ДатаДок, SelectOrder.ASC);
			встановленняЦінНоменклатури_Select.QuerySelect.Order.Add(Документи.ВстановленняЦінНоменклатури_Const.НомерДок, SelectOrder.ASC);

			встановленняЦінНоменклатури_Select.Select();
			while (встановленняЦінНоменклатури_Select.MoveNext())
			{
				Документи.ВстановленняЦінНоменклатури_Pointer cur = встановленняЦінНоменклатури_Select.Current;

				RecordsBindingList.Add(new Записи
				{
					ID = cur.UnigueID.ToString(),
					Назва = cur.Fields[Документи.ВстановленняЦінНоменклатури_Const.Назва].ToString(),
					НомерДок = cur.Fields[Документи.ВстановленняЦінНоменклатури_Const.НомерДок].ToString(),
					ДатаДок = cur.Fields[Документи.ВстановленняЦінНоменклатури_Const.ДатаДок].ToString(),
					Коментар = cur.Fields[Документи.ВстановленняЦінНоменклатури_Const.Коментар].ToString(),
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
					DocumentPointerItem = new Документи.ВстановленняЦінНоменклатури_Pointer(new UnigueID(Uid));
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
			Form_ВстановленняЦінНоменклатуриДокумент form_ВстановленняЦінНоменклатуриДокумент = new Form_ВстановленняЦінНоменклатуриДокумент();
			form_ВстановленняЦінНоменклатуриДокумент.MdiParent = this.MdiParent;
			form_ВстановленняЦінНоменклатуриДокумент.IsNew = true;
			form_ВстановленняЦінНоменклатуриДокумент.OwnerForm = this;
			form_ВстановленняЦінНоменклатуриДокумент.Show();
        }

        private void toolStripButtonEdit_Click(object sender, EventArgs e)
        {
			if (dataGridViewRecords.SelectedRows.Count > 0)
			{
				int RowIndex = dataGridViewRecords.SelectedRows[0].Index;

				Form_ВстановленняЦінНоменклатуриДокумент form_ВстановленняЦінНоменклатуриДокумент = new Form_ВстановленняЦінНоменклатуриДокумент();
				form_ВстановленняЦінНоменклатуриДокумент.MdiParent = this.MdiParent;
				form_ВстановленняЦінНоменклатуриДокумент.IsNew = false;
				form_ВстановленняЦінНоменклатуриДокумент.OwnerForm = this;
				form_ВстановленняЦінНоменклатуриДокумент.Uid = dataGridViewRecords.Rows[RowIndex].Cells["ID"].Value.ToString();
				form_ВстановленняЦінНоменклатуриДокумент.Show();
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

                    Документи.ВстановленняЦінНоменклатури_Objest встановленняЦінНоменклатури_Objest = new Документи.ВстановленняЦінНоменклатури_Objest();
                    if (встановленняЦінНоменклатури_Objest.Read(new UnigueID(uid)))
                    {
						Документи.ВстановленняЦінНоменклатури_Objest встановленняЦінНоменклатури_Objest_Новий = встановленняЦінНоменклатури_Objest.Copy();
						встановленняЦінНоменклатури_Objest_Новий.Назва += " *";
						встановленняЦінНоменклатури_Objest_Новий.ДатаДок = DateTime.Now;
						встановленняЦінНоменклатури_Objest_Новий.НомерДок = (++Константи.НумераціяДокументів.ВстановленняЦінНоменклатури_Const).ToString("D8");

						//Зчитати та скопіювати табличну частину Товари
						встановленняЦінНоменклатури_Objest.Товари_TablePart.Read();
						встановленняЦінНоменклатури_Objest_Новий.Товари_TablePart.Records = встановленняЦінНоменклатури_Objest.Товари_TablePart.Copy();
						встановленняЦінНоменклатури_Objest_Новий.Товари_TablePart.Save(true);
						встановленняЦінНоменклатури_Objest_Новий.Save();

						SelectPointerItem = встановленняЦінНоменклатури_Objest_Новий.GetDocumentPointer();
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

                    Документи.ВстановленняЦінНоменклатури_Objest встановленняЦінНоменклатури_Objest = new Документи.ВстановленняЦінНоменклатури_Objest();
                    if (встановленняЦінНоменклатури_Objest.Read(new UnigueID(uid)))
                    {
						встановленняЦінНоменклатури_Objest.Delete();
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

				Звіти.РухДокументівПоРегістрах.PrintRecords(new Документи.ВстановленняЦінНоменклатури_Pointer(new UnigueID(uid)));
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

					Документи.ВстановленняЦінНоменклатури_Pointer встановленняЦінНоменклатури_Pointer = new Документи.ВстановленняЦінНоменклатури_Pointer(new UnigueID(uid));
					Документи.ВстановленняЦінНоменклатури_Objest встановленняЦінНоменклатури_Objest = встановленняЦінНоменклатури_Pointer.GetDocumentObject(true);

					if (spend)
						try
						{
							//Проведення
							встановленняЦінНоменклатури_Objest.SpendTheDocument(встановленняЦінНоменклатури_Objest.ДатаДок);
						}
						catch (Exception exp)
						{
							встановленняЦінНоменклатури_Objest.ClearSpendTheDocument();
							MessageBox.Show(exp.Message);
							return;
						}
					else
						встановленняЦінНоменклатури_Objest.ClearSpendTheDocument();
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
