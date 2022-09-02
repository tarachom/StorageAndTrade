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

namespace StorageAndTrade
{
    public partial class Form_СкладЖурнал : Form
    {
        public Form_СкладЖурнал()
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
			dataGridViewRecords.Columns["Склад"].Width = 300;

			dataGridViewRecords.Columns["Сума"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
			dataGridViewRecords.Columns["Сума"].CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
			dataGridViewRecords.Columns["Сума"].Width = 100;

			dataGridViewRecords.Columns["Коментар"].Width = 350;

			dataGridViewRecords.Columns["Проведений"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
			dataGridViewRecords.Columns["Проведений"].Width = 80; 
		}

        private void Form_СкладЖурнал_Load(object sender, EventArgs e)
        {
			ConfigurationEnums ТипПеріодуДляЖурналівДокументів = Config.Kernel.Conf.Enums["ТипПеріодуДляЖурналівДокументів"];

			foreach (ConfigurationEnumField field in ТипПеріодуДляЖурналівДокументів.Fields.Values)
			{
				int index = сomboBox_ТипПеріоду.Items.Add(
					new NameValue<ТипПеріодуДляЖурналівДокументів>(field.Desc, (ТипПеріодуДляЖурналівДокументів)field.Value));

				if ((ТипПеріодуДляЖурналівДокументів)field.Value == ЖурналиДокументів.ОсновнийТипПеріоду_Const)
					сomboBox_ТипПеріоду.SelectedIndex = index;
			}

			if (сomboBox_ТипПеріоду.SelectedIndex == -1)
				сomboBox_ТипПеріоду.SelectedIndex = 0;
		}

		private BindingList<Записи> RecordsBindingList { get; set; }
		private LoadRecordsLimit loadRecordsLimit = new LoadRecordsLimit() { Limit = 50 };

		public void LoadRecords()
		{
			ТипПеріодуДляЖурналівДокументів ПеріодЖурналу =
				((NameValue<ТипПеріодуДляЖурналівДокументів>)сomboBox_ТипПеріоду.Items[сomboBox_ТипПеріоду.SelectedIndex]).Value;

			string query = $@"
{ФункціїДляЖурналів.ЗапитВибіркаСклад()}

ORDER BY ДатаДок
LIMIT {loadRecordsLimit.Limit}
OFFSET {loadRecordsLimit.Limit * loadRecordsLimit.PageIndex}
";

			Dictionary<string, object> paramQuery = new Dictionary<string, object>();
			paramQuery.Add("docdate", ФункціїДляЖурналів.ОтриматиДатуПочаткуПеріоду(ПеріодЖурналу));

			string[] columnsName;
			List<Dictionary<string, object>> listRow;

			Config.Kernel.DataBase.SelectRequest(query, paramQuery, out columnsName, out listRow);

			loadRecordsLimit.LastCountRow = listRow.Count;

			foreach (Dictionary<string, object> row in listRow)
			{
                RecordsBindingList.Add(new Записи
                {
                    DocName = row["ТипДокументу"].ToString(),
                    ID = row["uid"].ToString(),
                    Проведений = (bool)row["spend"],
                    Назва = row["Назва"].ToString(),
                    НомерДок = row["НомерДок"].ToString(),
                    ДатаДок = row["ДатаДок"].ToString(),
					Склад = row["КонтрагентНазва"].ToString(),
                    Сума = decimal.Parse(row["Сума"].ToString()),
                    Коментар = row["Коментар"].ToString()
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

				toolStripButtonEdit_Click(this, null);
			}
		}

		private void dataGridViewRecords_Scroll(object sender, ScrollEventArgs e)
		{
			if (e.ScrollOrientation == ScrollOrientation.VerticalScroll)
			{
				int rowHeight = dataGridViewRecords.Rows[dataGridViewRecords.FirstDisplayedScrollingRowIndex].Height;
				int countVisibleRows = dataGridViewRecords.Height / rowHeight;

				if (e.NewValue >= dataGridViewRecords.Rows.Count - countVisibleRows && loadRecordsLimit.LastCountRow == loadRecordsLimit.Limit)
				{
					LoadRecords();
				}
			}
		}

		#region Add

		private void ToolStripMenuItem_ПереміщенняТоварів_Click(object sender, EventArgs e)
		{
			Form_ПереміщенняТоварівДокумент form_ПереміщенняТоварівДокумент = new Form_ПереміщенняТоварівДокумент();
			form_ПереміщенняТоварівДокумент.MdiParent = this.MdiParent;
			form_ПереміщенняТоварівДокумент.IsNew = true;
			//form_ПереміщенняТоварівДокумент.OwnerForm = this;
			form_ПереміщенняТоварівДокумент.Show();
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
					case "ПереміщенняТоварів":
						{
							Form_ПереміщенняТоварівДокумент form_ПереміщенняТоварівДокумент = new Form_ПереміщенняТоварівДокумент();
							form_ПереміщенняТоварівДокумент.MdiParent = this.MdiParent;
							form_ПереміщенняТоварівДокумент.IsNew = false;
							//form_ПереміщенняТоварівДокумент.OwnerForm = this;
							form_ПереміщенняТоварівДокумент.Uid = uid;
							form_ПереміщенняТоварівДокумент.Show();

							break;
						}
				}
			}
		}

		#endregion

		private void toolStripButtonRefresh_Click(object sender, EventArgs e)
        {
			RecordsBindingList.Clear();

			loadRecordsLimit.PageIndex = 0;

			LoadRecords();
		}

        private void toolStripButtonCopy_Click(object sender, EventArgs e)
        {
			if (dataGridViewRecords.SelectedRows.Count != 0 &&
				MessageBox.Show("Копіювати записи?", "Повідомлення", MessageBoxButtons.YesNo) == DialogResult.Yes)
			{
				for (int i = 0; i < dataGridViewRecords.SelectedRows.Count; i++)
				{
					DataGridViewRow row = dataGridViewRecords.SelectedRows[0];
					string DocName = row.Cells["DocName"].Value.ToString();
					string uid = row.Cells["ID"].Value.ToString();

					switch (DocName)
					{
						case "ПереміщенняТоварів":
							{
                                ПереміщенняТоварів_Objest переміщенняТоварів_Objest = new ПереміщенняТоварів_Objest();
                                if (переміщенняТоварів_Objest.Read(new UnigueID(uid)))
                                {
									ПереміщенняТоварів_Objest переміщенняТоварів_Objest_Новий = переміщенняТоварів_Objest.Copy();
									переміщенняТоварів_Objest_Новий.Назва += " *";
									переміщенняТоварів_Objest_Новий.ДатаДок = DateTime.Now;
									переміщенняТоварів_Objest_Новий.НомерДок = (++НумераціяДокументів.ПереміщенняТоварів_Const).ToString("D8");

									//Зчитати та скопіювати табличну частину Товари
									переміщенняТоварів_Objest.Товари_TablePart.Read();
									переміщенняТоварів_Objest_Новий.Товари_TablePart.Records = переміщенняТоварів_Objest.Товари_TablePart.Copy();
									переміщенняТоварів_Objest_Новий.Товари_TablePart.Save(true);
									переміщенняТоварів_Objest_Новий.Save();
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
					DataGridViewRow row = dataGridViewRecords.SelectedRows[0];
					string DocName = row.Cells["DocName"].Value.ToString();
					string uid = row.Cells["ID"].Value.ToString();

					switch (DocName)
					{
						case "ПереміщенняТоварів":
							{
								ПереміщенняТоварів_Objest переміщенняТоварів_Objest = new ПереміщенняТоварів_Objest();
								if (переміщенняТоварів_Objest.Read(new UnigueID(uid)))
									переміщенняТоварів_Objest.Delete();
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
					case "ПереміщенняТоварів":
						{
							РухДокументівПоРегістрах.PrintRecords(new ПереміщенняТоварів_Pointer(new UnigueID(uid)));

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
					DataGridViewRow row = dataGridViewRecords.SelectedRows[0];
					string DocName = row.Cells["DocName"].Value.ToString();
					string uid = row.Cells["ID"].Value.ToString();

					switch (DocName)
					{
						case "ПереміщенняТоварів":
							{
								ПереміщенняТоварів_Pointer переміщенняТоварів_Pointer = new ПереміщенняТоварів_Pointer(new UnigueID(uid));
								ПереміщенняТоварів_Objest переміщенняТоварів_Objest = переміщенняТоварів_Pointer.GetDocumentObject(true);

                                if (spend)
                                    try
                                    {
										//Проведення
										переміщенняТоварів_Objest.SpendTheDocument(переміщенняТоварів_Objest.ДатаДок);
                                    }
                                    catch (Exception exp)
                                    {
										переміщенняТоварів_Objest.ClearSpendTheDocument();
                                        MessageBox.Show(exp.Message);
                                    }
								else
									переміщенняТоварів_Objest.ClearSpendTheDocument();

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

        private void сomboBox_ТипПеріоду_SelectedIndexChanged(object sender, EventArgs e)
        {
			RecordsBindingList.Clear();
			loadRecordsLimit.PageIndex = 0;

			dataGridViewRecords.Focus();

			LoadRecords();
		}
    }
}
