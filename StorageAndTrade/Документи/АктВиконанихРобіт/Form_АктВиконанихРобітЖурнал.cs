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
    public partial class Form_АктВиконанихРобітЖурнал : Form
    {
        public Form_АктВиконанихРобітЖурнал()
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

		private void Form_АктВиконанихРобітЖурнал_Load(object sender, EventArgs e)
        {
			LoadRecords();
		}

		private BindingList<Записи> RecordsBindingList { get; set; }

		public void LoadRecords()
		{
			int selectRow = dataGridViewRecords.SelectedRows.Count > 0 ?
				dataGridViewRecords.SelectedRows[dataGridViewRecords.SelectedRows.Count - 1].Index : 0;

			RecordsBindingList.Clear();

			Документи.АктВиконанихРобіт_Select актВиконанихРобіт_Select = new Документи.АктВиконанихРобіт_Select();
			актВиконанихРобіт_Select.QuerySelect.Field.AddRange(new string[] {
				"spend",
				Документи.АктВиконанихРобіт_Const.Назва,
				Документи.АктВиконанихРобіт_Const.НомерДок,
				Документи.АктВиконанихРобіт_Const.ДатаДок,
				Документи.АктВиконанихРобіт_Const.СумаДокументу,
				Документи.АктВиконанихРобіт_Const.Коментар
			});

			//Контрагент
			актВиконанихРобіт_Select.QuerySelect.FieldAndAlias.Add(
				new NameValue<string>(Довідники.Контрагенти_Const.TABLE + "." + Довідники.Контрагенти_Const.Назва, "joinContragent"));
			актВиконанихРобіт_Select.QuerySelect.Joins.Add(
				new Join(Довідники.Контрагенти_Const.TABLE, Документи.АктВиконанихРобіт_Const.Контрагент, Документи.АктВиконанихРобіт_Const.TABLE));

			//ORDER
			актВиконанихРобіт_Select.QuerySelect.Order.Add(Документи.АктВиконанихРобіт_Const.ДатаДок, SelectOrder.ASC);
			актВиконанихРобіт_Select.QuerySelect.Order.Add(Документи.АктВиконанихРобіт_Const.НомерДок, SelectOrder.ASC);

			актВиконанихРобіт_Select.Select();
			while (актВиконанихРобіт_Select.MoveNext())
			{
				Документи.АктВиконанихРобіт_Pointer cur = актВиконанихРобіт_Select.Current;

				RecordsBindingList.Add(new Записи
				{
					ID = cur.UnigueID.ToString(),
					Назва = cur.Fields[Документи.АктВиконанихРобіт_Const.Назва].ToString(),
					НомерДок = cur.Fields[Документи.АктВиконанихРобіт_Const.НомерДок].ToString(),
					ДатаДок = cur.Fields[Документи.АктВиконанихРобіт_Const.ДатаДок].ToString(),
					Контрагент = cur.Fields["joinContragent"].ToString(),
					Сума = Math.Round((decimal)cur.Fields[Документи.АктВиконанихРобіт_Const.СумаДокументу], 2),
					Коментар = cur.Fields[Документи.АктВиконанихРобіт_Const.Коментар].ToString(),
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
					DocumentPointerItem = new Документи.АктВиконанихРобіт_Pointer(new UnigueID(Uid));
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
			Form_АктВиконанихРобітДокумент form_АктВиконанихРобітДокумент = new Form_АктВиконанихРобітДокумент();
			form_АктВиконанихРобітДокумент.MdiParent = this.MdiParent;
			form_АктВиконанихРобітДокумент.IsNew = true;
			form_АктВиконанихРобітДокумент.OwnerForm = this;
			form_АктВиконанихРобітДокумент.Show();
        }

        private void toolStripButtonEdit_Click(object sender, EventArgs e)
        {
			if (dataGridViewRecords.SelectedRows.Count > 0)
			{
				int RowIndex = dataGridViewRecords.SelectedRows[0].Index;

				Form_АктВиконанихРобітДокумент form_АктВиконанихРобітДокумент = new Form_АктВиконанихРобітДокумент();
				form_АктВиконанихРобітДокумент.MdiParent = this.MdiParent;
				form_АктВиконанихРобітДокумент.IsNew = false;
				form_АктВиконанихРобітДокумент.OwnerForm = this;
				form_АктВиконанихРобітДокумент.Uid = dataGridViewRecords.Rows[RowIndex].Cells["ID"].Value.ToString();
				form_АктВиконанихРобітДокумент.Show();
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

                    Документи.АктВиконанихРобіт_Objest актВиконанихРобіт_Objest = new Документи.АктВиконанихРобіт_Objest();
                    if (актВиконанихРобіт_Objest.Read(new UnigueID(uid)))
                    {
						Документи.АктВиконанихРобіт_Objest актВиконанихРобіт_Objest_Новий = актВиконанихРобіт_Objest.Copy();
						актВиконанихРобіт_Objest_Новий.Назва += " *";
						актВиконанихРобіт_Objest_Новий.ДатаДок = DateTime.Now;
						актВиконанихРобіт_Objest_Новий.НомерДок = (++Константи.НумераціяДокументів.АктВиконанихРобіт_Const).ToString("D8");

						//Зчитати та скопіювати табличну частину Послуги
						актВиконанихРобіт_Objest.Послуги_TablePart.Read();
						актВиконанихРобіт_Objest_Новий.Послуги_TablePart.Records = актВиконанихРобіт_Objest.Послуги_TablePart.Copy();
						актВиконанихРобіт_Objest_Новий.Послуги_TablePart.Save(true);
						актВиконанихРобіт_Objest_Новий.Save();
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

                    Документи.АктВиконанихРобіт_Objest актВиконанихРобіт_Objest = new Документи.АктВиконанихРобіт_Objest();
                    if (актВиконанихРобіт_Objest.Read(new UnigueID(uid)))
                    {
						актВиконанихРобіт_Objest.Delete();
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

				Звіти.РухДокументівПоРегістрах.PrintRecords(new Документи.АктВиконанихРобіт_Pointer(new UnigueID(uid)));
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

					Документи.АктВиконанихРобіт_Pointer актВиконанихРобіт_Pointer = new Документи.АктВиконанихРобіт_Pointer(new UnigueID(uid));
					Документи.АктВиконанихРобіт_Objest актВиконанихРобіт_Objest = актВиконанихРобіт_Pointer.GetDocumentObject(true);

					if (spend)
						try
						{
							//Проведення
							актВиконанихРобіт_Objest.SpendTheDocument(актВиконанихРобіт_Objest.ДатаДок);
						}
						catch (Exception exp)
						{
							актВиконанихРобіт_Objest.ClearSpendTheDocument();
							MessageBox.Show(exp.Message);
							return;
						}
					else
						актВиконанихРобіт_Objest.ClearSpendTheDocument();
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
				DataGridViewRow row = dataGridViewRecords.SelectedRows[0];
				string uid = row.Cells["ID"].Value.ToString();

				Документи.АктВиконанихРобіт_Pointer актВиконанихРобіт_Pointer = new Документи.АктВиконанихРобіт_Pointer(new UnigueID(uid));
				Документи.АктВиконанихРобіт_Objest актВиконанихРобіт_Objest = актВиконанихРобіт_Pointer.GetDocumentObject(false);

				//
				//Новий документ
				//

				Документи.ПрихіднийКасовийОрдер_Objest прихіднийКасовийОрдер_Новий = new Документи.ПрихіднийКасовийОрдер_Objest();
				прихіднийКасовийОрдер_Новий.New();
				прихіднийКасовийОрдер_Новий.ДатаДок = DateTime.Now;
				прихіднийКасовийОрдер_Новий.НомерДок = (++Константи.НумераціяДокументів.ПрихіднийКасовийОрдер_Const).ToString("D8");
				прихіднийКасовийОрдер_Новий.Назва = $"Прихідний касовий ордер №{прихіднийКасовийОрдер_Новий.НомерДок} від {прихіднийКасовийОрдер_Новий.ДатаДок.ToString("dd.MM.yyyy")}";
				прихіднийКасовийОрдер_Новий.Організація = актВиконанихРобіт_Objest.Організація;
				прихіднийКасовийОрдер_Новий.Валюта = актВиконанихРобіт_Objest.Валюта;
				прихіднийКасовийОрдер_Новий.Каса = актВиконанихРобіт_Objest.Каса;
				прихіднийКасовийОрдер_Новий.Контрагент = актВиконанихРобіт_Objest.Контрагент;
				прихіднийКасовийОрдер_Новий.Договір = актВиконанихРобіт_Objest.Договір;
				прихіднийКасовийОрдер_Новий.Коментар = $"На основі \"{актВиконанихРобіт_Objest.Назва}\"";
				прихіднийКасовийОрдер_Новий.СумаДокументу = актВиконанихРобіт_Objest.СумаДокументу;
				прихіднийКасовийОрдер_Новий.Save();

				//Відкрити журнал та документ
				Form form_ПрихіднийКасовийОрдерЖурнал = Application.OpenForms["Form_ПрихіднийКасовийОрдерЖурнал"];
				if (form_ПрихіднийКасовийОрдерЖурнал == null)
				{
					form_ПрихіднийКасовийОрдерЖурнал = new Form_ПрихіднийКасовийОрдерЖурнал();
					form_ПрихіднийКасовийОрдерЖурнал.MdiParent = this.MdiParent;
					form_ПрихіднийКасовийОрдерЖурнал.Show();
				}
				else
					((Form_ПрихіднийКасовийОрдерЖурнал)form_ПрихіднийКасовийОрдерЖурнал).LoadRecords();

				Form_ПрихіднийКасовийОрдерДокумент form_ПрихіднийКасовийОрдерДокумент = new Form_ПрихіднийКасовийОрдерДокумент();
				form_ПрихіднийКасовийОрдерДокумент.MdiParent = this.MdiParent;
				form_ПрихіднийКасовийОрдерДокумент.IsNew = false;
				form_ПрихіднийКасовийОрдерДокумент.OwnerForm =(Form_ПрихіднийКасовийОрдерЖурнал)form_ПрихіднийКасовийОрдерЖурнал;
				form_ПрихіднийКасовийОрдерДокумент.Uid = прихіднийКасовийОрдер_Новий.UnigueID.ToString();
				form_ПрихіднийКасовийОрдерДокумент.Show();
			}
		}

		#endregion
	}
}
