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

			dataGridViewRecords.Columns["Коментар"].Width = 350;

			dataGridViewRecords.Columns["Проведений"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
			dataGridViewRecords.Columns["Проведений"].Width = 80; 
		}

        private void Form_ЗакупкиЖурнал_Load(object sender, EventArgs e)
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
{ФункціїДляЖурналів.ЗапитВибіркаЗакупки()}

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
							РухДокументівПоРегістрах.PrintRecords(new ЗамовленняПостачальнику_Pointer(new UnigueID(uid)));
							break;
						}
					case "ПоступленняТоварівТаПослуг":
						{
							РухДокументівПоРегістрах.PrintRecords(new ПоступленняТоварівТаПослуг_Pointer(new UnigueID(uid)));
							break;
						}
					case "ПоверненняТоварівПостачальнику":
						{
							РухДокументівПоРегістрах.PrintRecords(new ПоверненняТоварівПостачальнику_Pointer(new UnigueID(uid)));
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

                                if (spend)
                                    try
                                    {
										замовленняПостачальнику_Objest.SpendTheDocument(замовленняПостачальнику_Objest.ДатаДок);
                                    }
                                    catch (Exception exp)
                                    {
										замовленняПостачальнику_Objest.ClearSpendTheDocument();
                                        MessageBox.Show(exp.Message);
                                    }
								else
									замовленняПостачальнику_Objest.ClearSpendTheDocument();

								break;
							}
						case "ПоступленняТоварівТаПослуг":
							{
								ПоступленняТоварівТаПослуг_Pointer поступленняТоварівТаПослуг_Pointer = new ПоступленняТоварівТаПослуг_Pointer(new UnigueID(uid));
								ПоступленняТоварівТаПослуг_Objest поступленняТоварівТаПослуг_Objest = поступленняТоварівТаПослуг_Pointer.GetDocumentObject(true);

								if (spend)
									try
									{
										поступленняТоварівТаПослуг_Objest.SpendTheDocument(поступленняТоварівТаПослуг_Objest.ДатаДок);
									}
									catch (Exception exp)
									{
										поступленняТоварівТаПослуг_Objest.ClearSpendTheDocument();
										MessageBox.Show(exp.Message);
									}
								else
									поступленняТоварівТаПослуг_Objest.ClearSpendTheDocument();

								break;
							}
						case "ПоверненняТоварівПостачальнику":
							{
								ПоверненняТоварівПостачальнику_Pointer поверненняТоварівПостачальнику_Pointer = new ПоверненняТоварівПостачальнику_Pointer(new UnigueID(uid));
								ПоверненняТоварівПостачальнику_Objest поверненняТоварівПостачальнику_Objest = поверненняТоварівПостачальнику_Pointer.GetDocumentObject(true);

								if (spend)
									try
									{
										поверненняТоварівПостачальнику_Objest.SpendTheDocument(поверненняТоварівПостачальнику_Objest.ДатаДок);
									}
									catch (Exception exp)
									{
										поверненняТоварівПостачальнику_Objest.ClearSpendTheDocument();
										MessageBox.Show(exp.Message);
									}
								else
									поверненняТоварівПостачальнику_Objest.ClearSpendTheDocument();

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
