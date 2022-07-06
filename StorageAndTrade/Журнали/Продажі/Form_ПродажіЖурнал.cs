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
using StorageAndTrade_1_0;
using StorageAndTrade_1_0.Константи;
using StorageAndTrade_1_0.Довідники;
using StorageAndTrade_1_0.Документи;
using StorageAndTrade_1_0.Перелічення;
using StorageAndTrade_1_0.Звіти;

namespace StorageAndTrade
{
    public partial class Form_ПродажіЖурнал : Form
    {
        public Form_ПродажіЖурнал()
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

        private void Form_ПродажіЖурнал_Load(object sender, EventArgs e)
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
    'ЗамовленняКлієнта',
    Док_ЗамовленняКлієнта.uid,
    Док_ЗамовленняКлієнта.spend,
    Док_ЗамовленняКлієнта.{ЗамовленняКлієнта_Const.Назва} AS Назва,
    Док_ЗамовленняКлієнта.{ЗамовленняКлієнта_Const.НомерДок} AS НомерДок,
    Док_ЗамовленняКлієнта.{ЗамовленняКлієнта_Const.ДатаДок} AS ДатаДок,
    Довідник_Контрагенти.{Контрагенти_Const.Назва} AS КонтрагентНазва,
    Док_ЗамовленняКлієнта.{ЗамовленняКлієнта_Const.СумаДокументу} AS Сума
FROM
	{ЗамовленняКлієнта_Const.TABLE} AS Док_ЗамовленняКлієнта

    LEFT JOIN {Контрагенти_Const.TABLE} AS Довідник_Контрагенти ON Довідник_Контрагенти.uid = 
        Док_ЗамовленняКлієнта.{ЗамовленняКлієнта_Const.Контрагент}

UNION

SELECT
    'РеалізаціяТоварівТаПослуг',
    Док_РеалізаціяТоварівТаПослуг.uid,
    Док_РеалізаціяТоварівТаПослуг.spend,
    Док_РеалізаціяТоварівТаПослуг.{РеалізаціяТоварівТаПослуг_Const.Назва} AS Назва,
    Док_РеалізаціяТоварівТаПослуг.{РеалізаціяТоварівТаПослуг_Const.НомерДок} AS НомерДок,
    Док_РеалізаціяТоварівТаПослуг.{РеалізаціяТоварівТаПослуг_Const.ДатаДок} AS ДатаДок,
    Довідник_Контрагенти.{Контрагенти_Const.Назва} AS КонтрагентНазва,
    Док_РеалізаціяТоварівТаПослуг.{РеалізаціяТоварівТаПослуг_Const.СумаДокументу} AS Сума
FROM
	{РеалізаціяТоварівТаПослуг_Const.TABLE} AS Док_РеалізаціяТоварівТаПослуг

    LEFT JOIN {Контрагенти_Const.TABLE} AS Довідник_Контрагенти ON Довідник_Контрагенти.uid = 
        Док_РеалізаціяТоварівТаПослуг.{РеалізаціяТоварівТаПослуг_Const.Контрагент}

UNION

SELECT
    'ПоверненняТоварівВідКлієнта',
    Док_ПоверненняТоварівВідКлієнта.uid,
    Док_ПоверненняТоварівВідКлієнта.spend,
    Док_ПоверненняТоварівВідКлієнта.{ПоверненняТоварівВідКлієнта_Const.Назва} AS Назва,
    Док_ПоверненняТоварівВідКлієнта.{ПоверненняТоварівВідКлієнта_Const.НомерДок} AS НомерДок,
    Док_ПоверненняТоварівВідКлієнта.{ПоверненняТоварівВідКлієнта_Const.ДатаДок} AS ДатаДок,
    Довідник_Контрагенти.{Контрагенти_Const.Назва} AS КонтрагентНазва,
    Док_ПоверненняТоварівВідКлієнта.{ПоверненняТоварівВідКлієнта_Const.СумаДокументу} AS Сума
FROM
	{ПоверненняТоварівВідКлієнта_Const.TABLE} AS Док_ПоверненняТоварівВідКлієнта

    LEFT JOIN {Контрагенти_Const.TABLE} AS Довідник_Контрагенти ON Довідник_Контрагенти.uid = 
        Док_ПоверненняТоварівВідКлієнта.{ПоверненняТоварівВідКлієнта_Const.Контрагент}

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

        private void ToolStripMenuItem_ЗамовленняКлієнта_Click(object sender, EventArgs e)
		{
            Form_ЗамовленняКлієнтаДокумент form_ЗамовленняКлієнтаДокумент = new Form_ЗамовленняКлієнтаДокумент();
            form_ЗамовленняКлієнтаДокумент.MdiParent = this.MdiParent;
            form_ЗамовленняКлієнтаДокумент.IsNew = true;
            //form_ЗамовленняКлієнтаДокумент.OwnerForm = this;
            form_ЗамовленняКлієнтаДокумент.Show();
        }

		private void ToolStripMenuItem_РеалізаціяТоварівТаПослуг_Click(object sender, EventArgs e)
		{
			Form_РеалізаціяТоварівТаПослугДокумент form_РеалізаціяТоварівТаПослугДокумент = new Form_РеалізаціяТоварівТаПослугДокумент();
			form_РеалізаціяТоварівТаПослугДокумент.MdiParent = this.MdiParent;
			form_РеалізаціяТоварівТаПослугДокумент.IsNew = true;
			//form_РеалізаціяТоварівТаПослугДокумент.OwnerForm = this;
			form_РеалізаціяТоварівТаПослугДокумент.Show();
		}

		private void ToolStripMenuItem_ПоверненняТоварівВідКлієнта_Click(object sender, EventArgs e)
		{
			Form_ПоверненняТоварівВідКлієнтаДокумент form_ПоверненняТоварівВідКлієнтаДокумент = new Form_ПоверненняТоварівВідКлієнтаДокумент();
			form_ПоверненняТоварівВідКлієнтаДокумент.MdiParent = this.MdiParent;
			form_ПоверненняТоварівВідКлієнтаДокумент.IsNew = true;
			//form_ПоверненняТоварівВідКлієнтаДокумент.OwnerForm = this;
			form_ПоверненняТоварівВідКлієнтаДокумент.Show();
		}

        #endregion

        #region Edit

        private void toolStripButtonEdit_Click(object sender, EventArgs e)
        {
			if (dataGridViewRecords.SelectedRows.Count > 0)
			{
				int RowIndex = dataGridViewRecords.SelectedRows[0].Index;
				string DocName = dataGridViewRecords.Rows[RowIndex].Cells["DocName"].Value.ToString();

                switch (DocName)
                {
					case "ЗамовленняКлієнта":
						{
							Form_ЗамовленняКлієнтаДокумент form_ЗамовленняКлієнтаДокумент = new Form_ЗамовленняКлієнтаДокумент();
							form_ЗамовленняКлієнтаДокумент.MdiParent = this.MdiParent;
							form_ЗамовленняКлієнтаДокумент.IsNew = false;
							//form_ЗамовленняКлієнтаДокумент.OwnerForm = this;
							form_ЗамовленняКлієнтаДокумент.Uid = dataGridViewRecords.Rows[RowIndex].Cells["ID"].Value.ToString();
							form_ЗамовленняКлієнтаДокумент.Show();

							break;
						}

					case "РеалізаціяТоварівТаПослуг":
						{
							Form_РеалізаціяТоварівТаПослугДокумент form_РеалізаціяТоварівТаПослугДокумент = new Form_РеалізаціяТоварівТаПослугДокумент();
							form_РеалізаціяТоварівТаПослугДокумент.MdiParent = this.MdiParent;
							form_РеалізаціяТоварівТаПослугДокумент.IsNew = false;
							//form_РеалізаціяТоварівТаПослугДокумент.OwnerForm = this;
							form_РеалізаціяТоварівТаПослугДокумент.Uid = dataGridViewRecords.Rows[RowIndex].Cells["ID"].Value.ToString();
							form_РеалізаціяТоварівТаПослугДокумент.Show();

							break;
						}

					case "ПоверненняТоварівВідКлієнта":
						{
							Form_ПоверненняТоварівВідКлієнтаДокумент form_ПоверненняТоварівВідКлієнтаДокумент = new Form_ПоверненняТоварівВідКлієнтаДокумент();
							form_ПоверненняТоварівВідКлієнтаДокумент.MdiParent = this.MdiParent;
							form_ПоверненняТоварівВідКлієнтаДокумент.IsNew = false;
							//form_ПоверненняТоварівВідКлієнтаДокумент.OwnerForm = this;
							form_ПоверненняТоварівВідКлієнтаДокумент.Uid = dataGridViewRecords.Rows[RowIndex].Cells["ID"].Value.ToString();
							form_ПоверненняТоварівВідКлієнтаДокумент.Show();

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
						case "ЗамовленняКлієнта":
							{
                                ЗамовленняКлієнта_Objest замовленняКлієнта_Objest = new ЗамовленняКлієнта_Objest();
                                if (замовленняКлієнта_Objest.Read(new UnigueID(uid)))
                                {
                                    ЗамовленняКлієнта_Objest замовленняКлієнта_Objest_Новий = замовленняКлієнта_Objest.Copy();
                                    замовленняКлієнта_Objest_Новий.Назва += " *";
                                    замовленняКлієнта_Objest_Новий.ДатаДок = DateTime.Now;
                                    замовленняКлієнта_Objest_Новий.НомерДок = (++НумераціяДокументів.ЗамовленняКлієнта_Const).ToString("D8");

                                    //Зчитати та скопіювати табличну частину Товари
                                    замовленняКлієнта_Objest.Товари_TablePart.Read();
                                    замовленняКлієнта_Objest_Новий.Товари_TablePart.Records = замовленняКлієнта_Objest.Товари_TablePart.Copy();
                                    замовленняКлієнта_Objest_Новий.Товари_TablePart.Save(true);
                                    замовленняКлієнта_Objest_Новий.Save();
                                }
                                else
                                    MessageBox.Show("Error read");
        
                                break;
							}
						case "РеалізаціяТоварівТаПослуг":
							{
								РеалізаціяТоварівТаПослуг_Objest реалізаціяТоварівТаПослуг_Objest = new РеалізаціяТоварівТаПослуг_Objest();
								if (реалізаціяТоварівТаПослуг_Objest.Read(new UnigueID(uid)))
								{
									РеалізаціяТоварівТаПослуг_Objest реалізаціяТоварівТаПослуг_Objest_Новий = реалізаціяТоварівТаПослуг_Objest.Copy();
									реалізаціяТоварівТаПослуг_Objest_Новий.Назва += " *";
									реалізаціяТоварівТаПослуг_Objest_Новий.ДатаДок = DateTime.Now;
									реалізаціяТоварівТаПослуг_Objest_Новий.НомерДок = (++НумераціяДокументів.РеалізаціяТоварівТаПослуг_Const).ToString("D8");

									//Зчитати та скопіювати табличну частину Товари
									реалізаціяТоварівТаПослуг_Objest.Товари_TablePart.Read();
									реалізаціяТоварівТаПослуг_Objest_Новий.Товари_TablePart.Records = реалізаціяТоварівТаПослуг_Objest.Товари_TablePart.Copy();
									реалізаціяТоварівТаПослуг_Objest_Новий.Товари_TablePart.Save(true);
									реалізаціяТоварівТаПослуг_Objest_Новий.Save();
								}
								else
									MessageBox.Show("Error read");

								break;
							}
						case "ПоверненняТоварівВідКлієнта":
							{
								ПоверненняТоварівВідКлієнта_Objest поверненняТоварівВідКлієнта_Objest = new ПоверненняТоварівВідКлієнта_Objest();
								if (поверненняТоварівВідКлієнта_Objest.Read(new UnigueID(uid)))
								{
									ПоверненняТоварівВідКлієнта_Objest поверненняТоварівВідКлієнта_Objest_Новий = поверненняТоварівВідКлієнта_Objest.Copy();
									поверненняТоварівВідКлієнта_Objest_Новий.Назва += " *";
									поверненняТоварівВідКлієнта_Objest_Новий.ДатаДок = DateTime.Now;
									поверненняТоварівВідКлієнта_Objest_Новий.НомерДок = (++НумераціяДокументів.ПоверненняТоварівВідКлієнта_Const).ToString("D8");

									//Зчитати та скопіювати табличну частину Товари
									поверненняТоварівВідКлієнта_Objest.Товари_TablePart.Read();
									поверненняТоварівВідКлієнта_Objest_Новий.Товари_TablePart.Records = поверненняТоварівВідКлієнта_Objest.Товари_TablePart.Copy();
									поверненняТоварівВідКлієнта_Objest_Новий.Товари_TablePart.Save(true);
									поверненняТоварівВідКлієнта_Objest_Новий.Save();
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
						case "ЗамовленняКлієнта":
							{
								ЗамовленняКлієнта_Objest ЗамовленняКлієнта_Objest = new ЗамовленняКлієнта_Objest();
								if (ЗамовленняКлієнта_Objest.Read(new UnigueID(uid)))
									ЗамовленняКлієнта_Objest.Delete();
								else
									MessageBox.Show("Error read");

								break;
							}
						case "РеалізаціяТоварівТаПослуг":
							{
								РеалізаціяТоварівТаПослуг_Objest реалізаціяТоварівТаПослуг_Objest = new РеалізаціяТоварівТаПослуг_Objest();
								if (реалізаціяТоварівТаПослуг_Objest.Read(new UnigueID(uid)))
									реалізаціяТоварівТаПослуг_Objest.Delete();
								else
									MessageBox.Show("Error read");

								break;
							}
						case "ПоверненняТоварівВідКлієнта":
							{
								ПоверненняТоварівВідКлієнта_Objest поверненняТоварівВідКлієнта_Objest = new ПоверненняТоварівВідКлієнта_Objest();
								if (поверненняТоварівВідКлієнта_Objest.Read(new UnigueID(uid)))
									поверненняТоварівВідКлієнта_Objest.Delete();
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
					case "ЗамовленняКлієнта":
						{
							РухПоРугістрах.PrintRecords(new ЗамовленняКлієнта_Pointer(new UnigueID(uid)));
							break;
						}
					case "РеалізаціяТоварівТаПослуг":
						{
							РухПоРугістрах.PrintRecords(new РеалізаціяТоварівТаПослуг_Pointer(new UnigueID(uid)));
							break;
						}
					case "ПоверненняТоварівВідКлієнта":
						{
							РухПоРугістрах.PrintRecords(new ПоверненняТоварівВідКлієнта_Pointer(new UnigueID(uid)));
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
						case "ЗамовленняКлієнта":
							{
								ЗамовленняКлієнта_Pointer замовленняКлієнта_Pointer = new ЗамовленняКлієнта_Pointer(new UnigueID(uid));
								ЗамовленняКлієнта_Objest замовленняКлієнта_Objest = замовленняКлієнта_Pointer.GetDocumentObject(true);

								//Очищення регістрів
								замовленняКлієнта_Objest.ClearSpendTheDocument();

								if (spend)
									try
									{
										замовленняКлієнта_Objest.SpendTheDocument();
									}
									catch (Exception exp)
									{
										замовленняКлієнта_Objest.ClearSpendTheDocument();
										MessageBox.Show(exp.Message);
									}

								break;
							}
						case "РеалізаціяТоварівТаПослуг":
							{
								РеалізаціяТоварівТаПослуг_Pointer реалізаціяТоварівТаПослуг_Pointer = new РеалізаціяТоварівТаПослуг_Pointer(new UnigueID(uid));
								РеалізаціяТоварівТаПослуг_Objest реалізаціяТоварівТаПослуг_Objest = реалізаціяТоварівТаПослуг_Pointer.GetDocumentObject(true);

								//Очищення регістрів
								реалізаціяТоварівТаПослуг_Objest.ClearSpendTheDocument();

								if (spend)
									try
									{
										реалізаціяТоварівТаПослуг_Objest.SpendTheDocument();
									}
									catch (Exception exp)
									{
										реалізаціяТоварівТаПослуг_Objest.ClearSpendTheDocument();
										MessageBox.Show(exp.Message);
									}
								break;
							}
						case "ПоверненняТоварівВідКлієнта":
							{
								ПоверненняТоварівВідКлієнта_Pointer поверненняТоварівВідКлієнта_Pointer = new ПоверненняТоварівВідКлієнта_Pointer(new UnigueID(uid));
								ПоверненняТоварівВідКлієнта_Objest поверненняТоварівВідКлієнта_Objest = поверненняТоварівВідКлієнта_Pointer.GetDocumentObject(true);

								//Очищення регістрів
								поверненняТоварівВідКлієнта_Objest.ClearSpendTheDocument();

								if (spend)
									try
									{
										поверненняТоварівВідКлієнта_Objest.SpendTheDocument();
									}
									catch (Exception exp)
									{
										поверненняТоварівВідКлієнта_Objest.ClearSpendTheDocument();
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
