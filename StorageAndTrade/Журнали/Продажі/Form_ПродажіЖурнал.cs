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

			dataGridViewRecords.Columns["Коментар"].Width = 350;

			dataGridViewRecords.Columns["Проведений"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
			dataGridViewRecords.Columns["Проведений"].Width = 80; 
		}

        private void Form_ПродажіЖурнал_Load(object sender, EventArgs e)
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
{ФункціїДляЖурналів.ЗапитВибіркаПродажі()}

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

			loadRecordsLimit.PageIndex++;
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

		private void актВиконанихРобітToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Form_АктВиконанихРобітДокумент form_АктВиконанихРобітДокумент = new Form_АктВиконанихРобітДокумент();
			form_АктВиконанихРобітДокумент.MdiParent = this.MdiParent;
			form_АктВиконанихРобітДокумент.IsNew = true;
			//form_ПереміщенняТоварівДокумент.OwnerForm = this;
			form_АктВиконанихРобітДокумент.Show();
		}

		private void рахунокФактураToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Form_РахунокФактураДокумент form_РахунокФактураДокумент = new Form_РахунокФактураДокумент();
			form_РахунокФактураДокумент.MdiParent = this.MdiParent;
			form_РахунокФактураДокумент.IsNew = true;
			//form_ПереміщенняТоварівДокумент.OwnerForm = this;
			form_РахунокФактураДокумент.Show();
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
					case "ЗамовленняКлієнта":
						{
							Form_ЗамовленняКлієнтаДокумент form_ЗамовленняКлієнтаДокумент = new Form_ЗамовленняКлієнтаДокумент();
							form_ЗамовленняКлієнтаДокумент.MdiParent = this.MdiParent;
							form_ЗамовленняКлієнтаДокумент.IsNew = false;
							//form_ЗамовленняКлієнтаДокумент.OwnerForm = this;
							form_ЗамовленняКлієнтаДокумент.Uid = uid;
							form_ЗамовленняКлієнтаДокумент.Show();

							break;
						}

					case "РахунокФактура":
						{
							Form_РахунокФактураДокумент form_РахунокФактураДокумент = new Form_РахунокФактураДокумент();
							form_РахунокФактураДокумент.MdiParent = this.MdiParent;
							form_РахунокФактураДокумент.IsNew = false;
							//form_ЗамовленняКлієнтаДокумент.OwnerForm = this;
							form_РахунокФактураДокумент.Uid = uid;
							form_РахунокФактураДокумент.Show();

							break;
						}

					case "РеалізаціяТоварівТаПослуг":
						{
							Form_РеалізаціяТоварівТаПослугДокумент form_РеалізаціяТоварівТаПослугДокумент = new Form_РеалізаціяТоварівТаПослугДокумент();
							form_РеалізаціяТоварівТаПослугДокумент.MdiParent = this.MdiParent;
							form_РеалізаціяТоварівТаПослугДокумент.IsNew = false;
							//form_РеалізаціяТоварівТаПослугДокумент.OwnerForm = this;
							form_РеалізаціяТоварівТаПослугДокумент.Uid = uid;
							form_РеалізаціяТоварівТаПослугДокумент.Show();

							break;
						}
					case "АктВиконанихРобіт":
						{
							Form_АктВиконанихРобітДокумент form_АктВиконанихРобітДокумент = new Form_АктВиконанихРобітДокумент();
							form_АктВиконанихРобітДокумент.MdiParent = this.MdiParent;
							form_АктВиконанихРобітДокумент.IsNew = false;
							//form_РеалізаціяТоварівТаПослугДокумент.OwnerForm = this;
							form_АктВиконанихРобітДокумент.Uid = uid;
							form_АктВиконанихРобітДокумент.Show();

							break;
						}
					case "ПоверненняТоварівВідКлієнта":
						{
							Form_ПоверненняТоварівВідКлієнтаДокумент form_ПоверненняТоварівВідКлієнтаДокумент = new Form_ПоверненняТоварівВідКлієнтаДокумент();
							form_ПоверненняТоварівВідКлієнтаДокумент.MdiParent = this.MdiParent;
							form_ПоверненняТоварівВідКлієнтаДокумент.IsNew = false;
							//form_ПоверненняТоварівВідКлієнтаДокумент.OwnerForm = this;
							form_ПоверненняТоварівВідКлієнтаДокумент.Uid = uid;
							form_ПоверненняТоварівВідКлієнтаДокумент.Show();

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
						case "РахунокФактура":
							{
								РахунокФактура_Objest рахунокФактура_Objest = new РахунокФактура_Objest();
								if (рахунокФактура_Objest.Read(new UnigueID(uid)))
								{
									РахунокФактура_Objest рахунокФактура_Objest_Новий = рахунокФактура_Objest.Copy();
									рахунокФактура_Objest_Новий.Назва += " *";
									рахунокФактура_Objest_Новий.ДатаДок = DateTime.Now;
									рахунокФактура_Objest_Новий.НомерДок = (++НумераціяДокументів.ЗамовленняКлієнта_Const).ToString("D8");

									//Зчитати та скопіювати табличну частину Товари
									рахунокФактура_Objest.Товари_TablePart.Read();
									рахунокФактура_Objest_Новий.Товари_TablePart.Records = рахунокФактура_Objest.Товари_TablePart.Copy();
									рахунокФактура_Objest_Новий.Товари_TablePart.Save(true);
									рахунокФактура_Objest_Новий.Save();
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
						case "АктВиконанихРобіт":
							{
								АктВиконанихРобіт_Objest актВиконанихРобіт_Objest = new АктВиконанихРобіт_Objest();
								if (актВиконанихРобіт_Objest.Read(new UnigueID(uid)))
								{
									АктВиконанихРобіт_Objest актВиконанихРобіт_Objest_Новий = актВиконанихРобіт_Objest.Copy();
									актВиконанихРобіт_Objest_Новий.Назва += " *";
									актВиконанихРобіт_Objest_Новий.ДатаДок = DateTime.Now;
									актВиконанихРобіт_Objest_Новий.НомерДок = (++НумераціяДокументів.АктВиконанихРобіт_Const).ToString("D8");

									//Зчитати та скопіювати табличну частину Послуги
									актВиконанихРобіт_Objest.Послуги_TablePart.Read();
									актВиконанихРобіт_Objest_Новий.Послуги_TablePart.Records = актВиконанихРобіт_Objest.Послуги_TablePart.Copy();
									актВиконанихРобіт_Objest_Новий.Послуги_TablePart.Save(true);
									актВиконанихРобіт_Objest_Новий.Save();
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
						case "РахунокФактура":
							{
								РахунокФактура_Objest РахунокФактура_Objest = new РахунокФактура_Objest();
								if (РахунокФактура_Objest.Read(new UnigueID(uid)))
									РахунокФактура_Objest.Delete();
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
						case "АктВиконанихРобіт":
							{
								АктВиконанихРобіт_Objest актВиконанихРобіт_Objest = new АктВиконанихРобіт_Objest();
								if (актВиконанихРобіт_Objest.Read(new UnigueID(uid)))
									актВиконанихРобіт_Objest.Delete();
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
							РухДокументівПоРегістрах.PrintRecords(new ЗамовленняКлієнта_Pointer(new UnigueID(uid)));
							break;
						}
					case "РахунокФактура":
						{
							РухДокументівПоРегістрах.PrintRecords(new РахунокФактура_Pointer(new UnigueID(uid)));
							break;
						}
					case "РеалізаціяТоварівТаПослуг":
						{
							РухДокументівПоРегістрах.PrintRecords(new РеалізаціяТоварівТаПослуг_Pointer(new UnigueID(uid)));
							break;
						}
					case "АктВиконанихРобіт":
						{
							РухДокументівПоРегістрах.PrintRecords(new АктВиконанихРобіт_Pointer(new UnigueID(uid)));
							break;
						}
					case "ПоверненняТоварівВідКлієнта":
						{
							РухДокументівПоРегістрах.PrintRecords(new ПоверненняТоварівВідКлієнта_Pointer(new UnigueID(uid)));
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

								if (spend)
									try
									{
										замовленняКлієнта_Objest.SpendTheDocument(замовленняКлієнта_Objest.ДатаДок);
									}
									catch (Exception exp)
									{
										замовленняКлієнта_Objest.ClearSpendTheDocument();
										MessageBox.Show(exp.Message);
									}
								else
									замовленняКлієнта_Objest.ClearSpendTheDocument();

								break;
							}
						case "РахунокФактура":
							{
								РахунокФактура_Pointer рахунокФактура_Pointer = new РахунокФактура_Pointer(new UnigueID(uid));
								РахунокФактура_Objest рахунокФактура_Objest = рахунокФактура_Pointer.GetDocumentObject(true);

								if (spend)
									try
									{
										рахунокФактура_Objest.SpendTheDocument(рахунокФактура_Objest.ДатаДок);
									}
									catch (Exception exp)
									{
										рахунокФактура_Objest.ClearSpendTheDocument();
										MessageBox.Show(exp.Message);
									}
								else
									рахунокФактура_Objest.ClearSpendTheDocument();

								break;
							}
						case "РеалізаціяТоварівТаПослуг":
							{
								РеалізаціяТоварівТаПослуг_Pointer реалізаціяТоварівТаПослуг_Pointer = new РеалізаціяТоварівТаПослуг_Pointer(new UnigueID(uid));
								РеалізаціяТоварівТаПослуг_Objest реалізаціяТоварівТаПослуг_Objest = реалізаціяТоварівТаПослуг_Pointer.GetDocumentObject(true);

								if (spend)
									try
									{
										реалізаціяТоварівТаПослуг_Objest.SpendTheDocument(реалізаціяТоварівТаПослуг_Objest.ДатаДок);
									}
									catch (Exception exp)
									{
										реалізаціяТоварівТаПослуг_Objest.ClearSpendTheDocument();
										MessageBox.Show(exp.Message);
									}
								else
									реалізаціяТоварівТаПослуг_Objest.ClearSpendTheDocument();

								break;
							}
						case "АктВиконанихРобіт":
							{
								АктВиконанихРобіт_Pointer актВиконанихРобіт_Pointer = new АктВиконанихРобіт_Pointer(new UnigueID(uid));
								АктВиконанихРобіт_Objest актВиконанихРобіт_Objest = актВиконанихРобіт_Pointer.GetDocumentObject(true);

								if (spend)
									try
									{
										актВиконанихРобіт_Objest.SpendTheDocument(актВиконанихРобіт_Objest.ДатаДок);
									}
									catch (Exception exp)
									{
										актВиконанихРобіт_Objest.ClearSpendTheDocument();
										MessageBox.Show(exp.Message);
									}
								else
									актВиконанихРобіт_Objest.ClearSpendTheDocument();

								break;
							}
						case "ПоверненняТоварівВідКлієнта":
							{
								ПоверненняТоварівВідКлієнта_Pointer поверненняТоварівВідКлієнта_Pointer = new ПоверненняТоварівВідКлієнта_Pointer(new UnigueID(uid));
								ПоверненняТоварівВідКлієнта_Objest поверненняТоварівВідКлієнта_Objest = поверненняТоварівВідКлієнта_Pointer.GetDocumentObject(true);

								if (spend)
									try
									{
										поверненняТоварівВідКлієнта_Objest.SpendTheDocument(поверненняТоварівВідКлієнта_Objest.ДатаДок);
									}
									catch (Exception exp)
									{
										поверненняТоварівВідКлієнта_Objest.ClearSpendTheDocument();
										MessageBox.Show(exp.Message);
									}
								else
									поверненняТоварівВідКлієнта_Objest.ClearSpendTheDocument();

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
