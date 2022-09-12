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
using StorageAndTrade_1_0;
using StorageAndTrade_1_0.Константи;
using StorageAndTrade_1_0.Довідники;
using StorageAndTrade_1_0.Документи;
using StorageAndTrade_1_0.Перелічення;

namespace StorageAndTrade
{
    public partial class Form_ФінансиЖурнал : Form
    {
        public Form_ФінансиЖурнал()
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

			dataGridViewRecords.Columns["Коментар"].Width = 350;

			dataGridViewRecords.Columns["Проведений"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
			dataGridViewRecords.Columns["Проведений"].Width = 80; 
		}

        private void Form_ФінансиЖурнал_Load(object sender, EventArgs e)
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
{ФункціїДляЖурналів.ЗапитВибіркаФінанси()}

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
					Контрагент = row["КонтрагентНазва"].ToString(),
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

		private void ToolStripMenuItem_ПрихіднийКасовийОрдер_Click(object sender, EventArgs e)
		{
			Form_ПрихіднийКасовийОрдерДокумент form_ПрихіднийКасовийОрдерДокумент = new Form_ПрихіднийКасовийОрдерДокумент();
			form_ПрихіднийКасовийОрдерДокумент.MdiParent = this.MdiParent;
			form_ПрихіднийКасовийОрдерДокумент.IsNew = true;
			//form_ПрихіднийКасовийОрдерДокумент.OwnerForm = this;
			form_ПрихіднийКасовийОрдерДокумент.Show();
		}

		private void ToolStripMenuItem_РозхіднийКасовийОрдер_Click(object sender, EventArgs e)
		{
			Form_РозхіднийКасовийОрдерДокумент form_РозхіднийКасовийОрдерДокумент = new Form_РозхіднийКасовийОрдерДокумент();
			form_РозхіднийКасовийОрдерДокумент.MdiParent = this.MdiParent;
			form_РозхіднийКасовийОрдерДокумент.IsNew = true;
			//form_РозхіднийКасовийОрдерДокумент.OwnerForm = this;
			form_РозхіднийКасовийОрдерДокумент.Show();
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
					case "ПрихіднийКасовийОрдер":
						{
							Form_ПрихіднийКасовийОрдерДокумент form_ПрихіднийКасовийОрдерДокумент = new Form_ПрихіднийКасовийОрдерДокумент();
							form_ПрихіднийКасовийОрдерДокумент.MdiParent = this.MdiParent;
							form_ПрихіднийКасовийОрдерДокумент.IsNew = false;
							//form_ПрихіднийКасовийОрдерДокумент.OwnerForm = this;
							form_ПрихіднийКасовийОрдерДокумент.Uid = uid;
							form_ПрихіднийКасовийОрдерДокумент.Show();

							break;
						}
					case "РозхіднийКасовийОрдер":
						{
							Form_РозхіднийКасовийОрдерДокумент form_РозхіднийКасовийОрдерДокумент = new Form_РозхіднийКасовийОрдерДокумент();
							form_РозхіднийКасовийОрдерДокумент.MdiParent = this.MdiParent;
							form_РозхіднийКасовийОрдерДокумент.IsNew = false;
							//form_РозхіднийКасовийОрдерДокумент.OwnerForm = this;
							form_РозхіднийКасовийОрдерДокумент.Uid = uid;
							form_РозхіднийКасовийОрдерДокумент.Show();

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
					DataGridViewRow row = dataGridViewRecords.SelectedRows[i];
					string DocName = row.Cells["DocName"].Value.ToString();
					string uid = row.Cells["ID"].Value.ToString();

					switch (DocName)
					{
						case "ПрихіднийКасовийОрдер":
							{
                                ПрихіднийКасовийОрдер_Objest прихіднийКасовийОрдер_Objest = new ПрихіднийКасовийОрдер_Objest();
                                if (прихіднийКасовийОрдер_Objest.Read(new UnigueID(uid)))
                                {
                                    ПрихіднийКасовийОрдер_Objest прихіднийКасовийОрдер_Objest_Новий = прихіднийКасовийОрдер_Objest.Copy();
                                    прихіднийКасовийОрдер_Objest_Новий.Назва += " *";
                                    прихіднийКасовийОрдер_Objest_Новий.ДатаДок = DateTime.Now;
                                    прихіднийКасовийОрдер_Objest_Новий.НомерДок = (++НумераціяДокументів.ПрихіднийКасовийОрдер_Const).ToString("D8");

                                    //Зчитати та скопіювати табличну частину
									прихіднийКасовийОрдер_Objest.РозшифруванняПлатежу_TablePart.Read();
                                    прихіднийКасовийОрдер_Objest_Новий.РозшифруванняПлатежу_TablePart.Records = прихіднийКасовийОрдер_Objest.РозшифруванняПлатежу_TablePart.Copy();
                                    прихіднийКасовийОрдер_Objest_Новий.РозшифруванняПлатежу_TablePart.Save(true);
                                    прихіднийКасовийОрдер_Objest_Новий.Save();
                                }
                                else
                                    MessageBox.Show("Error read");

                                break;
							}
						case "РозхіднийКасовийОрдер":
							{
								РозхіднийКасовийОрдер_Objest розхіднийКасовийОрдер_Objest = new РозхіднийКасовийОрдер_Objest();
								if (розхіднийКасовийОрдер_Objest.Read(new UnigueID(uid)))
								{
									РозхіднийКасовийОрдер_Objest розхіднийКасовийОрдер_Objest_Новий = розхіднийКасовийОрдер_Objest.Copy();
									розхіднийКасовийОрдер_Objest_Новий.Назва += " *";
									розхіднийКасовийОрдер_Objest_Новий.ДатаДок = DateTime.Now;
									розхіднийКасовийОрдер_Objest_Новий.НомерДок = (++НумераціяДокументів.РозхіднийКасовийОрдер_Const).ToString("D8");

									//Зчитати та скопіювати табличну частину
									розхіднийКасовийОрдер_Objest.РозшифруванняПлатежу_TablePart.Read();
									розхіднийКасовийОрдер_Objest_Новий.РозшифруванняПлатежу_TablePart.Records = розхіднийКасовийОрдер_Objest.РозшифруванняПлатежу_TablePart.Copy();
									розхіднийКасовийОрдер_Objest_Новий.РозшифруванняПлатежу_TablePart.Save(true);
									розхіднийКасовийОрдер_Objest_Новий.Save();
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
						case "ПрихіднийКасовийОрдер":
							{
								ПрихіднийКасовийОрдер_Objest прихіднийКасовийОрдер_Objest = new ПрихіднийКасовийОрдер_Objest();
								if (прихіднийКасовийОрдер_Objest.Read(new UnigueID(uid)))
								{
									прихіднийКасовийОрдер_Objest.Delete();
								}
								else
									MessageBox.Show("Error read");

								break;
							}
						case "РозхіднийКасовийОрдер":
							{
								РозхіднийКасовийОрдер_Objest розхіднийКасовийОрдер_Objest = new РозхіднийКасовийОрдер_Objest();
								if (розхіднийКасовийОрдер_Objest.Read(new UnigueID(uid)))
								{
									розхіднийКасовийОрдер_Objest.Delete();
								}
								else
									MessageBox.Show("Error read");

								break;
							}
					}

					LoadRecords();
				}
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
					case "ПрихіднийКасовийОрдер":
						{
							РухДокументівПоРегістрах.PrintRecords(new ПрихіднийКасовийОрдер_Pointer(new UnigueID(uid)));

							break;
						}
					case "РозхіднийКасовийОрдер":
						{
							РухДокументівПоРегістрах.PrintRecords(new РозхіднийКасовийОрдер_Pointer(new UnigueID(uid)));

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
						case "ПрихіднийКасовийОрдер":
							{
								ПрихіднийКасовийОрдер_Pointer прихіднийКасовийОрдер_Pointer = new ПрихіднийКасовийОрдер_Pointer(new UnigueID(uid));
								ПрихіднийКасовийОрдер_Objest прихіднийКасовийОрдер_Objest = прихіднийКасовийОрдер_Pointer.GetDocumentObject(true);

                                if (spend)
                                    try
                                    {
										прихіднийКасовийОрдер_Objest.SpendTheDocument(прихіднийКасовийОрдер_Objest.ДатаДок);
                                    }
                                    catch (Exception exp)
                                    {
										прихіднийКасовийОрдер_Objest.ClearSpendTheDocument();
                                        MessageBox.Show(exp.Message);
                                    }
								else
									прихіднийКасовийОрдер_Objest.ClearSpendTheDocument();

								break;
							}
						case "РозхіднийКасовийОрдер":
							{
								РозхіднийКасовийОрдер_Pointer розхіднийКасовийОрдер_Pointer = new РозхіднийКасовийОрдер_Pointer(new UnigueID(uid));
								РозхіднийКасовийОрдер_Objest розхіднийКасовийОрдер_Objest = розхіднийКасовийОрдер_Pointer.GetDocumentObject(true);

								if (spend)
									try
									{
										розхіднийКасовийОрдер_Objest.SpendTheDocument(розхіднийКасовийОрдер_Objest.ДатаДок);
									}
									catch (Exception exp)
									{
										розхіднийКасовийОрдер_Objest.ClearSpendTheDocument();
										MessageBox.Show(exp.Message);
									}
								else
									розхіднийКасовийОрдер_Objest.ClearSpendTheDocument();

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
