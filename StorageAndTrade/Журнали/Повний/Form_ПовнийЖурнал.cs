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
    public partial class Form_ПовнийЖурнал : Form
    {
        public Form_ПовнийЖурнал()
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

        private void Form_ПовнийЖурнал_Load(object sender, EventArgs e)
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
		private LoadRecordsLimit loadRecordsLimit = new LoadRecordsLimit() { Limit = 50};

		public void LoadRecords()
		{
			ТипПеріодуДляЖурналівДокументів ПеріодЖурналу =
				((NameValue<ТипПеріодуДляЖурналівДокументів>)сomboBox_ТипПеріоду.Items[сomboBox_ТипПеріоду.SelectedIndex]).Value;

			string query = $@"
{ФункціїДляЖурналів.ЗапитВибіркаВсіхДокументів()}

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
			//int display = dataGridViewRecords.Rows.Count - dataGridViewRecords.DisplayedRowCount(false);
			if (e.ScrollOrientation == ScrollOrientation.VerticalScroll)
			{
				int rowHeight = dataGridViewRecords.Rows[dataGridViewRecords.FirstDisplayedScrollingRowIndex].Height;
				int countVisibleRows = dataGridViewRecords.Height / rowHeight;

				if (e.NewValue >= dataGridViewRecords.Rows.Count - countVisibleRows && loadRecordsLimit.LastCountRow == loadRecordsLimit.Limit)
				{
					LoadRecords();
					//Console.WriteLine("LoadRecords");
					//dataGridViewRecords.ClearSelection();
					//dataGridViewRecords.FirstDisplayedScrollingRowIndex = display;
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

		private void ToolStripMenuItem_ПереміщенняТоварів_Click(object sender, EventArgs e)
		{
			Form_ПереміщенняТоварівДокумент form_ПереміщенняТоварівДокумент = new Form_ПереміщенняТоварівДокумент();
			form_ПереміщенняТоварівДокумент.MdiParent = this.MdiParent;
			form_ПереміщенняТоварівДокумент.IsNew = true;
			//form_ПереміщенняТоварівДокумент.OwnerForm = this;
			form_ПереміщенняТоварівДокумент.Show();
		}

		private void ToolStripMenuItem_ВстановленняЦінНоменклатури_Click(object sender, EventArgs e)
		{
			Form_ВстановленняЦінНоменклатуриДокумент form_ВстановленняЦінНоменклатуриДокумент = new Form_ВстановленняЦінНоменклатуриДокумент();
			form_ВстановленняЦінНоменклатуриДокумент.MdiParent = this.MdiParent;
			form_ВстановленняЦінНоменклатуриДокумент.IsNew = true;
			//form_ПереміщенняТоварівДокумент.OwnerForm = this;
			form_ВстановленняЦінНоменклатуриДокумент.Show();
		}

		private void введенняЗалишківToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Form_ВведенняЗалишківДокумент form_ВведенняЗалишківДокумент = new Form_ВведенняЗалишківДокумент();
			form_ВведенняЗалишківДокумент.MdiParent = this.MdiParent;
			form_ВведенняЗалишківДокумент.IsNew = true;
			//form_ПереміщенняТоварівДокумент.OwnerForm = this;
			form_ВведенняЗалишківДокумент.Show();
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

		private void внутрішнєСпоживанняТоварівToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Form_ВнутрішнєСпоживанняТоварівДокумент form_ВнутрішнєСпоживанняТоварівДокумент = new Form_ВнутрішнєСпоживанняТоварівДокумент();
			form_ВнутрішнєСпоживанняТоварівДокумент.MdiParent = this.MdiParent;
			form_ВнутрішнєСпоживанняТоварівДокумент.IsNew = true;
			//form_ПереміщенняТоварівДокумент.OwnerForm = this;
			form_ВнутрішнєСпоживанняТоварівДокумент.Show();
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
					#region Продажі

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

					#endregion

					#region Закупки

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

					#endregion

					#region Склад

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

					#endregion

					#region Фінанси

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

					#endregion

					#region Ціноутворення

					case "ВстановленняЦінНоменклатури":
						{
							Form_ВстановленняЦінНоменклатуриДокумент form_ВстановленняЦінНоменклатуриДокумент = new Form_ВстановленняЦінНоменклатуриДокумент();
							form_ВстановленняЦінНоменклатуриДокумент.MdiParent = this.MdiParent;
							form_ВстановленняЦінНоменклатуриДокумент.IsNew = false;
							//form_ВстановленняЦінНоменклатуриДокумент.OwnerForm = this;
							form_ВстановленняЦінНоменклатуриДокумент.Uid = uid;
							form_ВстановленняЦінНоменклатуриДокумент.Show();

							break;
						}

					#endregion

					#region ВведенняЗалишків

					case "ВведенняЗалишків":
						{
							Form_ВведенняЗалишківДокумент form_ВведенняЗалишківДокумент = new Form_ВведенняЗалишківДокумент();
							form_ВведенняЗалишківДокумент.MdiParent = this.MdiParent;
							form_ВведенняЗалишківДокумент.IsNew = false;
							//form_ВведенняЗалишківДокумент.OwnerForm = this;
							form_ВведенняЗалишківДокумент.Uid = uid;
							form_ВведенняЗалишківДокумент.Show();

							break;
						}

					#endregion

					#region Внутрішні документи

					case "ВнутрішнєСпоживанняТоварів":
						{
							Form_ВнутрішнєСпоживанняТоварівДокумент form_ВнутрішнєСпоживанняТоварівДокумент = new Form_ВнутрішнєСпоживанняТоварівДокумент();
							form_ВнутрішнєСпоживанняТоварівДокумент.MdiParent = this.MdiParent;
							form_ВнутрішнєСпоживанняТоварівДокумент.IsNew = false;
							//form_ВведенняЗалишківДокумент.OwnerForm = this;
							form_ВнутрішнєСпоживанняТоварівДокумент.Uid = uid;
							form_ВнутрішнєСпоживанняТоварівДокумент.Show();

							break;
						}

						#endregion
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
						#region Продажі

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

						#endregion

						#region Закупки

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

						#endregion

						#region Склад

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

						#endregion

						#region Фінанси

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

						#endregion

						#region Ціноутворення

						case "ВстановленняЦінНоменклатури":
							{
								ВстановленняЦінНоменклатури_Objest встановленняЦінНоменклатури_Objest = new ВстановленняЦінНоменклатури_Objest();
								if (встановленняЦінНоменклатури_Objest.Read(new UnigueID(uid)))
								{
									ВстановленняЦінНоменклатури_Objest встановленняЦінНоменклатури_Objest_Новий = встановленняЦінНоменклатури_Objest.Copy();
									встановленняЦінНоменклатури_Objest_Новий.Назва += " *";
									встановленняЦінНоменклатури_Objest_Новий.ДатаДок = DateTime.Now;
									встановленняЦінНоменклатури_Objest_Новий.НомерДок = (++НумераціяДокументів.ВстановленняЦінНоменклатури_Const).ToString("D8");

									//Зчитати та скопіювати табличну частину Товари
									встановленняЦінНоменклатури_Objest.Товари_TablePart.Read();
									встановленняЦінНоменклатури_Objest_Новий.Товари_TablePart.Records = встановленняЦінНоменклатури_Objest.Товари_TablePart.Copy();
									встановленняЦінНоменклатури_Objest_Новий.Товари_TablePart.Save(true);
									встановленняЦінНоменклатури_Objest_Новий.Save();
								}
								else
									MessageBox.Show("Error read");

								break;
							}

						#endregion

						#region ВведенняЗалишків

						case "ВведенняЗалишків":
							{
								ВведенняЗалишків_Objest введенняЗалишків_Objest = new ВведенняЗалишків_Objest();
								if (введенняЗалишків_Objest.Read(new UnigueID(uid)))
								{
									ВведенняЗалишків_Objest введенняЗалишків_Objest_Новий = введенняЗалишків_Objest.Copy();
									введенняЗалишків_Objest_Новий.Назва += " *";
									введенняЗалишків_Objest_Новий.ДатаДок = DateTime.Now;
									введенняЗалишків_Objest_Новий.НомерДок = (++НумераціяДокументів.ВведенняЗалишків_Const).ToString("D8");

									//Зчитати та скопіювати табличну частину Товари
									введенняЗалишків_Objest.Товари_TablePart.Read();
									введенняЗалишків_Objest_Новий.Товари_TablePart.Records = введенняЗалишків_Objest.Товари_TablePart.Copy();
									введенняЗалишків_Objest_Новий.Товари_TablePart.Save(true);
									введенняЗалишків_Objest_Новий.Save();

									//Зчитати та скопіювати табличну частину Каси
									введенняЗалишків_Objest.Каси_TablePart.Read();
									введенняЗалишків_Objest_Новий.Каси_TablePart.Records = введенняЗалишків_Objest.Каси_TablePart.Copy();
									введенняЗалишків_Objest_Новий.Каси_TablePart.Save(true);
									введенняЗалишків_Objest_Новий.Save();

									//Зчитати та скопіювати табличну частину БанківськіРахунки
									введенняЗалишків_Objest.БанківськіРахунки_TablePart.Read();
									введенняЗалишків_Objest_Новий.БанківськіРахунки_TablePart.Records = введенняЗалишків_Objest.БанківськіРахунки_TablePart.Copy();
									введенняЗалишків_Objest_Новий.БанківськіРахунки_TablePart.Save(true);
									введенняЗалишків_Objest_Новий.Save();

									//Зчитати та скопіювати табличну частину РозрахункиЗКонтрагентами
									введенняЗалишків_Objest.РозрахункиЗКонтрагентами_TablePart.Read();
									введенняЗалишків_Objest_Новий.РозрахункиЗКонтрагентами_TablePart.Records = введенняЗалишків_Objest.РозрахункиЗКонтрагентами_TablePart.Copy();
									введенняЗалишків_Objest_Новий.РозрахункиЗКонтрагентами_TablePart.Save(true);
									введенняЗалишків_Objest_Новий.Save();
								}
								else
									MessageBox.Show("Error read");

								break;
							}

						#endregion

						#region Внутрішні документи

						case "ВнутрішнєСпоживанняТоварів":
							{
								ВнутрішнєСпоживанняТоварів_Objest внутрішнєСпоживанняТоварів_Objest = new ВнутрішнєСпоживанняТоварів_Objest();
								if (внутрішнєСпоживанняТоварів_Objest.Read(new UnigueID(uid)))
								{
									ВнутрішнєСпоживанняТоварів_Objest внутрішнєСпоживанняТоварів_Objest_Новий = внутрішнєСпоживанняТоварів_Objest.Copy();
									внутрішнєСпоживанняТоварів_Objest_Новий.Назва += " *";
									внутрішнєСпоживанняТоварів_Objest_Новий.ДатаДок = DateTime.Now;
									внутрішнєСпоживанняТоварів_Objest_Новий.НомерДок = (++НумераціяДокументів.ВнутрішнєСпоживанняТоварів_Const).ToString("D8");

									//Зчитати та скопіювати табличну частину Товари
									внутрішнєСпоживанняТоварів_Objest.Товари_TablePart.Read();
									внутрішнєСпоживанняТоварів_Objest_Новий.Товари_TablePart.Records = внутрішнєСпоживанняТоварів_Objest.Товари_TablePart.Copy();
									внутрішнєСпоживанняТоварів_Objest_Новий.Товари_TablePart.Save(true);
									внутрішнєСпоживанняТоварів_Objest_Новий.Save();
								}
								else
									MessageBox.Show("Error read");

								break;
							}

							#endregion
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
						#region Продажі

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

						#endregion

						#region Закупки

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

						#endregion

						#region Склад

						case "ПереміщенняТоварів":
							{
								ПереміщенняТоварів_Objest переміщенняТоварів_Objest = new ПереміщенняТоварів_Objest();
								if (переміщенняТоварів_Objest.Read(new UnigueID(uid)))
									переміщенняТоварів_Objest.Delete();
								else
									MessageBox.Show("Error read");

								break;
							}

						#endregion

						#region Фінанси

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

						#endregion

						#region Ціноутворення

						case "ВстановленняЦінНоменклатури":
							{
								ВстановленняЦінНоменклатури_Objest встановленняЦінНоменклатури_Objest = new ВстановленняЦінНоменклатури_Objest();
								if (встановленняЦінНоменклатури_Objest.Read(new UnigueID(uid)))
								{
									встановленняЦінНоменклатури_Objest.Delete();
								}
								else
									MessageBox.Show("Error read");
								break;
							}

						#endregion

						#region ВведенняЗалишків

						case "ВведенняЗалишків":
							{
								ВведенняЗалишків_Objest введенняЗалишків_Objest = new ВведенняЗалишків_Objest();
								if (введенняЗалишків_Objest.Read(new UnigueID(uid)))
								{
									введенняЗалишків_Objest.Delete();
								}
								else
									MessageBox.Show("Error read");
								break;
							}

						#endregion

						#region Внутрішні документи

						case "ВнутрішнєСпоживанняТоварів":
							{
								ВнутрішнєСпоживанняТоварів_Objest внутрішнєСпоживанняТоварів_Objest = new ВнутрішнєСпоживанняТоварів_Objest();
								if (внутрішнєСпоживанняТоварів_Objest.Read(new UnigueID(uid)))
								{
									внутрішнєСпоживанняТоварів_Objest.Delete();
								}
								else
									MessageBox.Show("Error read");
								break;
							}

						#endregion
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
                    #region Продажі

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

					#endregion

					#region Закупки

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

					#endregion

					#region Склад

					case "ПереміщенняТоварів":
						{
							РухДокументівПоРегістрах.PrintRecords(new ПереміщенняТоварів_Pointer(new UnigueID(uid)));

							break;
						}

					#endregion

					#region Фінанси

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

					#endregion

					#region Ціноутворення

					case "ВстановленняЦінНоменклатури":
						{
							РухДокументівПоРегістрах.PrintRecords(new ВстановленняЦінНоменклатури_Pointer(new UnigueID(uid)));

							break;
						}

					#endregion

					#region ВведенняЗалишків

					case "ВведенняЗалишків":
						{
							РухДокументівПоРегістрах.PrintRecords(new ВведенняЗалишків_Pointer(new UnigueID(uid)));

							break;
						}

					#endregion

					#region Внутрішні документи

					case "ВнутрішнєСпоживанняТоварів":
						{
							РухДокументівПоРегістрах.PrintRecords(new ВнутрішнєСпоживанняТоварів_Pointer(new UnigueID(uid)));

							break;
						}

						#endregion

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
                        #region Продажі

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

						#endregion

						#region Закупки

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

						#endregion

						#region Склад

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

						#endregion

						#region Фінанси

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

						#endregion

						#region Ціноутворення

						case "ВстановленняЦінНоменклатури":
							{
								ВстановленняЦінНоменклатури_Pointer встановленняЦінНоменклатури_Pointer = new ВстановленняЦінНоменклатури_Pointer(new UnigueID(uid));
								ВстановленняЦінНоменклатури_Objest встановленняЦінНоменклатури_Objest = встановленняЦінНоменклатури_Pointer.GetDocumentObject(true);

								if (spend)
									try
									{
										встановленняЦінНоменклатури_Objest.SpendTheDocument(встановленняЦінНоменклатури_Objest.ДатаДок);
									}
									catch (Exception exp)
									{
										встановленняЦінНоменклатури_Objest.ClearSpendTheDocument();
										MessageBox.Show(exp.Message);
									}
								else
									встановленняЦінНоменклатури_Objest.ClearSpendTheDocument();

								break;
							}

						#endregion

						#region ВведенняЗалишків

						case "ВведенняЗалишків":
							{
								ВведенняЗалишків_Pointer введенняЗалишків_Pointer = new ВведенняЗалишків_Pointer(new UnigueID(uid));
								ВведенняЗалишків_Objest введенняЗалишків_Objest = введенняЗалишків_Pointer.GetDocumentObject(true);

								if (spend)
									try
									{
										введенняЗалишків_Objest.SpendTheDocument(введенняЗалишків_Objest.ДатаДок);
									}
									catch (Exception exp)
									{
										введенняЗалишків_Objest.ClearSpendTheDocument();
										MessageBox.Show(exp.Message);
									}
								else
									введенняЗалишків_Objest.ClearSpendTheDocument();

								break;
							}

						#endregion

						#region Внутрішні документи

						case "ВнутрішнєСпоживанняТоварів":
							{
								ВнутрішнєСпоживанняТоварів_Pointer внутрішнєСпоживанняТоварів_Pointer = new ВнутрішнєСпоживанняТоварів_Pointer(new UnigueID(uid));
								ВнутрішнєСпоживанняТоварів_Objest внутрішнєСпоживанняТоварів_Objest = внутрішнєСпоживанняТоварів_Pointer.GetDocumentObject(true);

								if (spend)
									try
									{
										внутрішнєСпоживанняТоварів_Objest.SpendTheDocument(внутрішнєСпоживанняТоварів_Objest.ДатаДок);
									}
									catch (Exception exp)
									{
										внутрішнєСпоживанняТоварів_Objest.ClearSpendTheDocument();
										MessageBox.Show(exp.Message);
									}
								else
									внутрішнєСпоживанняТоварів_Objest.ClearSpendTheDocument();

								break;
							}

							#endregion
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