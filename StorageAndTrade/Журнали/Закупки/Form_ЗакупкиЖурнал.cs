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
using StorageAndTrade_1_0;
using StorageAndTrade_1_0.Константи;
using StorageAndTrade_1_0.Довідники;
using StorageAndTrade_1_0.Документи;
using StorageAndTrade_1_0.Перелічення;
using StorageAndTrade_1_0.Звіти;

namespace StorageAndTrade
{
    public partial class Form_ЗакупкиЖурнал : Form
    {
        public Form_ЗакупкиЖурнал()
        {
            InitializeComponent();

			dataGridViewRecords.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

			RecordsBindingList = new BindingList<Записи>();
			dataGridViewRecords.DataSource = RecordsBindingList;

			dataGridViewRecords.Columns["Image"].Width = 30;
			dataGridViewRecords.Columns["Image"].HeaderText = "";

			dataGridViewRecords.Columns["ID"].Visible = false;
			dataGridViewRecords.Columns["DocName"].Visible = false;

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

			dataGridViewRecords.Columns["Проведений"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
			dataGridViewRecords.Columns["Проведений"].Width = 80; 
		}

        private void Form_ЗакупкиЖурнал_Load(object sender, EventArgs e)
        {
			LoadRecords();
		}

		private BindingList<Записи> RecordsBindingList { get; set; }

		public void LoadRecords()
		{
			int selectRow = dataGridViewRecords.SelectedRows.Count > 0 ?
				dataGridViewRecords.SelectedRows[dataGridViewRecords.SelectedRows.Count - 1].Index : 0;

			RecordsBindingList.Clear();

			string query = $@"
SELECT
    'ЗамовленняПостачальнику',
    Док_ЗамовленняПостачальнику.uid,
    Док_ЗамовленняПостачальнику.spend,
    Док_ЗамовленняПостачальнику.{ЗамовленняПостачальнику_Const.Назва} AS Назва,
    Док_ЗамовленняПостачальнику.{ЗамовленняПостачальнику_Const.НомерДок} AS НомерДок,
    Док_ЗамовленняПостачальнику.{ЗамовленняПостачальнику_Const.ДатаДок} AS ДатаДок,
    Довідник_Контрагенти.{Контрагенти_Const.Назва} AS КонтрагентНазва,
    Док_ЗамовленняПостачальнику.{ЗамовленняПостачальнику_Const.СумаДокументу} AS Сума
FROM
	{ЗамовленняПостачальнику_Const.TABLE} AS Док_ЗамовленняПостачальнику

    LEFT JOIN {Контрагенти_Const.TABLE} AS Довідник_Контрагенти ON Довідник_Контрагенти.uid = 
        Док_ЗамовленняПостачальнику.{ЗамовленняПостачальнику_Const.Контрагент}

UNION

SELECT
    'ПоступленняТоварівТаПослуг',
    Док_ПоступленняТоварівТаПослуг.uid,
    Док_ПоступленняТоварівТаПослуг.spend,
    Док_ПоступленняТоварівТаПослуг.{ПоступленняТоварівТаПослуг_Const.Назва} AS Назва,
    Док_ПоступленняТоварівТаПослуг.{ПоступленняТоварівТаПослуг_Const.НомерДок} AS НомерДок,
    Док_ПоступленняТоварівТаПослуг.{ПоступленняТоварівТаПослуг_Const.ДатаДок} AS ДатаДок,
    Довідник_Контрагенти.{Контрагенти_Const.Назва} AS КонтрагентНазва,
    Док_ПоступленняТоварівТаПослуг.{ПоступленняТоварівТаПослуг_Const.СумаДокументу} AS Сума
FROM
	{ПоступленняТоварівТаПослуг_Const.TABLE} AS Док_ПоступленняТоварівТаПослуг

    LEFT JOIN {Контрагенти_Const.TABLE} AS Довідник_Контрагенти ON Довідник_Контрагенти.uid = 
        Док_ПоступленняТоварівТаПослуг.{ПоступленняТоварівТаПослуг_Const.Контрагент}

UNION

SELECT
    'ПоверненняТоварівПостачальнику',
    Док_ПоверненняТоварівПостачальнику.uid,
    Док_ПоверненняТоварівПостачальнику.spend,
    Док_ПоверненняТоварівПостачальнику.{ПоверненняТоварівПостачальнику_Const.Назва} AS Назва,
    Док_ПоверненняТоварівПостачальнику.{ПоверненняТоварівПостачальнику_Const.НомерДок} AS НомерДок,
    Док_ПоверненняТоварівПостачальнику.{ПоверненняТоварівПостачальнику_Const.ДатаДок} AS ДатаДок,
    Довідник_Контрагенти.{Контрагенти_Const.Назва} AS КонтрагентНазва,
    Док_ПоверненняТоварівПостачальнику.{ПоверненняТоварівПостачальнику_Const.СумаДокументу} AS Сума
FROM
	{ПоверненняТоварівПостачальнику_Const.TABLE} AS Док_ПоверненняТоварівПостачальнику

    LEFT JOIN {Контрагенти_Const.TABLE} AS Довідник_Контрагенти ON Довідник_Контрагенти.uid = 
        Док_ПоверненняТоварівПостачальнику.{ПоверненняТоварівПостачальнику_Const.Контрагент}

ORDER BY ДатаДок
";

			Dictionary<string, object> paramQuery = new Dictionary<string, object>();

			string[] columnsName;
			List<object[]> listRow;

			Config.Kernel.DataBase.SelectRequest(query, paramQuery, out columnsName, out listRow);

			foreach(object[] row in listRow)
			{
				RecordsBindingList.Add(new Записи
				{
					DocName = row[0].ToString(),
					ID = row[1].ToString(),
					Проведений = (bool)row[2],
					Назва = row[3].ToString(),
					НомерДок = row[4].ToString(),
					ДатаДок = row[5].ToString(),
					Контрагент = row[6].ToString(),
					Сума = (decimal)row[7]
				});
			}
		}

		private class Записи
		{
			public Записи() { Image = Properties.Resources.doc_text_image; }
			public Bitmap Image { get; set; }
			public string DocName { get; set; }
			public string ID { get; set; }
			public string Назва { get; set; }
			public string НомерДок { get; set; }
			public string ДатаДок { get; set; }
			public string Контрагент { get; set; }
			public decimal Сума { get; set; }
			public bool Проведений { get; set; }
		}

		private void dataGridViewRecords_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex >= 0 && e.RowIndex < dataGridViewRecords.RowCount)
			{
				string Uid = dataGridViewRecords.Rows[e.RowIndex].Cells["ID"].Value.ToString();

				toolStripButtonEdit_Click(this, null);
			}
		}

        #region Add

        private void ToolStripMenuItem_ЗамовленняПостачальнику_Click(object sender, EventArgs e)
		{
			Form_ЗамовленняПостачальникуДокумент form_ЗамовленняПостачальникуДокумент = new Form_ЗамовленняПостачальникуДокумент();
			form_ЗамовленняПостачальникуДокумент.MdiParent = this.MdiParent;
			form_ЗамовленняПостачальникуДокумент.IsNew = true;
			//form_ЗамовленняПостачальникуДокумент.OwnerForm = this;
			form_ЗамовленняПостачальникуДокумент.Show();
		}

		private void ToolStripMenuItem_ПоступленняТоварівТаПослуг_Click(object sender, EventArgs e)
		{
			Form_ПоступленняТоварівТаПослугДокумент form_ПоступленняТоварівТаПослугДокумент = new Form_ПоступленняТоварівТаПослугДокумент();
			form_ПоступленняТоварівТаПослугДокумент.MdiParent = this.MdiParent;
			form_ПоступленняТоварівТаПослугДокумент.IsNew = true;
			//form_ПоступленняТоварівТаПослугДокумент.OwnerForm = this;
			form_ПоступленняТоварівТаПослугДокумент.Show();
		}

		private void ToolStripMenuItem_ПоверненняТоварівПостачальнику_Click(object sender, EventArgs e)
		{
			Form_ПоверненняТоварівПостачальникуДокумент form_ПоверненняТоварівПостачальникуДокумент = new Form_ПоверненняТоварівПостачальникуДокумент();
			form_ПоверненняТоварівПостачальникуДокумент.MdiParent = this.MdiParent;
			form_ПоверненняТоварівПостачальникуДокумент.IsNew = true;
			//form_ПоверненняТоварівПостачальникуДокумент.OwnerForm = this;
			form_ПоверненняТоварівПостачальникуДокумент.Show();
		}

		#endregion

		#region Edit

		private void toolStripButtonEdit_Click(object sender, EventArgs e)
		{
			if (dataGridViewRecords.SelectedRows.Count > 0)
			{
				DataGridViewRow row = dataGridViewRecords.SelectedRows[0];
				string DocName = row.Cells["DocName"].Value.ToString();
				string uid = row.Cells["ID"].Value.ToString();

				switch (DocName)
				{
					case "ЗамовленняПостачальнику":
						{
							Form_ЗамовленняПостачальникуДокумент form_ЗамовленняПостачальникуДокумент = new Form_ЗамовленняПостачальникуДокумент();
							form_ЗамовленняПостачальникуДокумент.MdiParent = this.MdiParent;
							form_ЗамовленняПостачальникуДокумент.IsNew = false;
							//form_ЗамовленняПостачальникуДокумент.OwnerForm = this;
							form_ЗамовленняПостачальникуДокумент.Uid = uid;
							form_ЗамовленняПостачальникуДокумент.Show();

                            break;
						}
					case "ПоступленняТоварівТаПослуг":
						{
							Form_ПоступленняТоварівТаПослугДокумент form_ПоступленняТоварівТаПослугДокумент = new Form_ПоступленняТоварівТаПослугДокумент();
							form_ПоступленняТоварівТаПослугДокумент.MdiParent = this.MdiParent;
							form_ПоступленняТоварівТаПослугДокумент.IsNew = false;
							//form_ПоступленняТоварівТаПослугДокумент.OwnerForm = this;	
							form_ПоступленняТоварівТаПослугДокумент.Uid = uid;
							form_ПоступленняТоварівТаПослугДокумент.Show();

							break;
						}
					case "ПоверненняТоварівПостачальнику":
						{
							Form_ПоверненняТоварівПостачальникуДокумент form_ПоверненняТоварівПостачальникуДокумент = new Form_ПоверненняТоварівПостачальникуДокумент();
							form_ПоверненняТоварівПостачальникуДокумент.MdiParent = this.MdiParent;
							form_ПоверненняТоварівПостачальникуДокумент.IsNew = false;
							//form_ПоверненняТоварівПостачальникуДокумент.OwnerForm = this;
							form_ПоверненняТоварівПостачальникуДокумент.Uid = uid;
							form_ПоверненняТоварівПостачальникуДокумент.Show();

							break;
						}
				}
			}
		}

        #endregion

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
					string DocName = row.Cells["DocName"].Value.ToString();
					string uid = row.Cells["ID"].Value.ToString();

					switch (DocName)
					{
						case "ЗамовленняПостачальнику":
							{
								ЗамовленняПостачальнику_Objest замовленняПостачальнику_Objest = new ЗамовленняПостачальнику_Objest();
                                if (замовленняПостачальнику_Objest.Read(new UnigueID(uid)))
                                {
									ЗамовленняПостачальнику_Objest замовленняПостачальнику_Objest_Новий = замовленняПостачальнику_Objest.Copy();
									замовленняПостачальнику_Objest_Новий.Назва += " *";
									замовленняПостачальнику_Objest_Новий.ДатаДок = DateTime.Now;
									замовленняПостачальнику_Objest_Новий.НомерДок = (++НумераціяДокументів.ЗамовленняПостачальнику_Const).ToString("D8");

									//Зчитати та скопіювати табличну частину Товари
									замовленняПостачальнику_Objest.Товари_TablePart.Read();
									замовленняПостачальнику_Objest_Новий.Товари_TablePart.Records = замовленняПостачальнику_Objest.Товари_TablePart.Copy();
									замовленняПостачальнику_Objest_Новий.Товари_TablePart.Save(true);
									замовленняПостачальнику_Objest_Новий.Save();
                                }
                                else
                                    MessageBox.Show("Error read");

                                break;
							}
						case "ПоступленняТоварівТаПослуг":
							{
								ПоступленняТоварівТаПослуг_Objest поступленняТоварівТаПослуг_Objest = new ПоступленняТоварівТаПослуг_Objest();
								if (поступленняТоварівТаПослуг_Objest.Read(new UnigueID(uid)))
								{
									ПоступленняТоварівТаПослуг_Objest поступленняТоварівТаПослуг_Objest_Новий = поступленняТоварівТаПослуг_Objest.Copy();
									поступленняТоварівТаПослуг_Objest_Новий.Назва += " *";
									поступленняТоварівТаПослуг_Objest_Новий.ДатаДок = DateTime.Now;
									поступленняТоварівТаПослуг_Objest_Новий.НомерДок = (++НумераціяДокументів.ПоступленняТоварівТаПослуг_Const).ToString("D8");

									//Зчитати та скопіювати табличну частину Товари
									поступленняТоварівТаПослуг_Objest.Товари_TablePart.Read();
									поступленняТоварівТаПослуг_Objest_Новий.Товари_TablePart.Records = поступленняТоварівТаПослуг_Objest.Товари_TablePart.Copy();
									поступленняТоварівТаПослуг_Objest_Новий.Товари_TablePart.Save(true);
									поступленняТоварівТаПослуг_Objest_Новий.Save();
								}
								else
									MessageBox.Show("Error read");

								break;
							}
						case "ПоверненняТоварівПостачальнику":
							{
								ПоверненняТоварівПостачальнику_Objest поверненняТоварівПостачальнику_Objest = new ПоверненняТоварівПостачальнику_Objest();
								if (поверненняТоварівПостачальнику_Objest.Read(new UnigueID(uid)))
								{
									ПоверненняТоварівПостачальнику_Objest поверненняТоварівПостачальнику_Objest_Новий = поверненняТоварівПостачальнику_Objest.Copy();
									поверненняТоварівПостачальнику_Objest_Новий.Назва += " *";
									поверненняТоварівПостачальнику_Objest_Новий.ДатаДок = DateTime.Now;
									поверненняТоварівПостачальнику_Objest_Новий.НомерДок = (++НумераціяДокументів.ПоверненняТоварівПостачальнику_Const).ToString("D8");

									//Зчитати та скопіювати табличну частину Товари
									поверненняТоварівПостачальнику_Objest.Товари_TablePart.Read();
									поверненняТоварівПостачальнику_Objest_Новий.Товари_TablePart.Records = поверненняТоварівПостачальнику_Objest.Товари_TablePart.Copy();
									поверненняТоварівПостачальнику_Objest_Новий.Товари_TablePart.Save(true);
									поверненняТоварівПостачальнику_Objest_Новий.Save();
								}
								else
									MessageBox.Show("Error read");

								break;
							}
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
					string DocName = row.Cells["DocName"].Value.ToString();
					string uid = row.Cells["ID"].Value.ToString();

					switch (DocName)
					{
						case "ЗамовленняПостачальнику":
							{
								ЗамовленняПостачальнику_Objest замовленняПостачальнику_Objest = new ЗамовленняПостачальнику_Objest();
								if (замовленняПостачальнику_Objest.Read(new UnigueID(uid)))
									замовленняПостачальнику_Objest.Delete();
								else
									MessageBox.Show("Error read");

								break;
							}
						case "ПоступленняТоварівТаПослуг":
							{
								ПоступленняТоварівТаПослуг_Objest поступленняТоварівТаПослуг_Objest = new ПоступленняТоварівТаПослуг_Objest();
								if (поступленняТоварівТаПослуг_Objest.Read(new UnigueID(uid)))
									поступленняТоварівТаПослуг_Objest.Delete();
								else
									MessageBox.Show("Error read");

								break;
							}
						case "ПоверненняТоварівПостачальнику":
							{
								ПоверненняТоварівПостачальнику_Objest поверненняТоварівПостачальнику_Objest = new ПоверненняТоварівПостачальнику_Objest();
								if (поверненняТоварівПостачальнику_Objest.Read(new UnigueID(uid)))
									поверненняТоварівПостачальнику_Objest.Delete();
								else
									MessageBox.Show("Error read");

								break;
							}
					}
				}

				LoadRecords();
			}
		}

        private void toolStripButtonДрукПроводок_Click(object sender, EventArgs e)
        {
			if (dataGridViewRecords.SelectedRows.Count > 0)
			{
				DataGridViewRow row = dataGridViewRecords.SelectedRows[0];
				string DocName = row.Cells["DocName"].Value.ToString();
				string uid = row.Cells["ID"].Value.ToString();

				switch (DocName)
				{
					case "ЗамовленняПостачальнику":
						{
							РухПоРугістрах.PrintRecords(new ЗамовленняПостачальнику_Pointer(new UnigueID(uid)));
							break;
						}
					case "ПоступленняТоварівТаПослуг":
						{
							РухПоРугістрах.PrintRecords(new ПоступленняТоварівТаПослуг_Pointer(new UnigueID(uid)));
							break;
						}
					case "ПоверненняТоварівПостачальнику":
						{
							РухПоРугістрах.PrintRecords(new ПоверненняТоварівПостачальнику_Pointer(new UnigueID(uid)));
							break;
						}
				}
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
					string DocName = row.Cells["DocName"].Value.ToString();
					string uid = row.Cells["ID"].Value.ToString();

					switch (DocName)
					{
						case "ЗамовленняПостачальнику":
							{
								ЗамовленняПостачальнику_Pointer замовленняПостачальнику_Pointer = new ЗамовленняПостачальнику_Pointer(new UnigueID(uid));
								ЗамовленняПостачальнику_Objest замовленняПостачальнику_Objest = замовленняПостачальнику_Pointer.GetDocumentObject(true);

								замовленняПостачальнику_Objest.ClearSpendTheDocument();

                                if (spend)
                                    try
                                    {
										замовленняПостачальнику_Objest.SpendTheDocument();
                                    }
                                    catch (Exception exp)
                                    {
										замовленняПостачальнику_Objest.ClearSpendTheDocument();
                                        MessageBox.Show(exp.Message);
                                    }

                                break;
							}
						case "ПоступленняТоварівТаПослуг":
							{
								ПоступленняТоварівТаПослуг_Pointer поступленняТоварівТаПослуг_Pointer = new ПоступленняТоварівТаПослуг_Pointer(new UnigueID(uid));
								ПоступленняТоварівТаПослуг_Objest поступленняТоварівТаПослуг_Objest = поступленняТоварівТаПослуг_Pointer.GetDocumentObject(true);

								поступленняТоварівТаПослуг_Objest.ClearSpendTheDocument();

								if (spend)
									try
									{
										поступленняТоварівТаПослуг_Objest.SpendTheDocument();
									}
									catch (Exception exp)
									{
										поступленняТоварівТаПослуг_Objest.ClearSpendTheDocument();
										MessageBox.Show(exp.Message);
									}

								break;
							}
						case "ПоверненняТоварівПостачальнику":
							{
								ПоверненняТоварівПостачальнику_Pointer поверненняТоварівПостачальнику_Pointer = new ПоверненняТоварівПостачальнику_Pointer(new UnigueID(uid));
								ПоверненняТоварівПостачальнику_Objest поверненняТоварівПостачальнику_Objest = поверненняТоварівПостачальнику_Pointer.GetDocumentObject(true);

								поверненняТоварівПостачальнику_Objest.ClearSpendTheDocument();

								if (spend)
									try
									{
										поверненняТоварівПостачальнику_Objest.SpendTheDocument();
									}
									catch (Exception exp)
									{
										поверненняТоварівПостачальнику_Objest.ClearSpendTheDocument();
										MessageBox.Show(exp.Message);
									}

								break;
							}
					}
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
