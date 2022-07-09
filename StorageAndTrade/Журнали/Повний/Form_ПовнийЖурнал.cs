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

			dataGridViewRecords.Columns["Проведений"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
			dataGridViewRecords.Columns["Проведений"].Width = 80; 
		}

        private void Form_ПовнийЖурнал_Load(object sender, EventArgs e)
        {
			LoadRecords();
		}

		private BindingList<Записи> RecordsBindingList { get; set; }

		private struct LoadRecordsLimit
		{
			public int PageIndex;
			public int Limit;
			public int LastCountRow;
        }

		private LoadRecordsLimit loadRecordsLimit = new LoadRecordsLimit() { Limit = 50};

		public void LoadRecords()
		{
			int selectRow = dataGridViewRecords.SelectedRows.Count > 0 ?
				dataGridViewRecords.SelectedRows[dataGridViewRecords.SelectedRows.Count - 1].Index : 0;

			string query_Продажі = $@"
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
";

			string query_Закупки = $@"
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
";

			string query_Фінанси = $@"
SELECT
    'ПрихіднийКасовийОрдер',
    Док_ПрихіднийКасовийОрдер.uid,
    Док_ПрихіднийКасовийОрдер.spend,
    Док_ПрихіднийКасовийОрдер.{ПрихіднийКасовийОрдер_Const.Назва} AS Назва,
    Док_ПрихіднийКасовийОрдер.{ПрихіднийКасовийОрдер_Const.НомерДок} AS НомерДок,
    Док_ПрихіднийКасовийОрдер.{ПрихіднийКасовийОрдер_Const.ДатаДок} AS ДатаДок,
    Довідник_Контрагенти.{Контрагенти_Const.Назва} AS КонтрагентНазва,
    Док_ПрихіднийКасовийОрдер.{ПрихіднийКасовийОрдер_Const.СумаДокументу} AS Сума
FROM
	{ПрихіднийКасовийОрдер_Const.TABLE} AS Док_ПрихіднийКасовийОрдер

    LEFT JOIN {Контрагенти_Const.TABLE} AS Довідник_Контрагенти ON Довідник_Контрагенти.uid = 
        Док_ПрихіднийКасовийОрдер.{ПрихіднийКасовийОрдер_Const.Контрагент}

UNION

SELECT
    'РозхіднийКасовийОрдер',
    Док_РозхіднийКасовийОрдер.uid,
    Док_РозхіднийКасовийОрдер.spend,
    Док_РозхіднийКасовийОрдер.{РозхіднийКасовийОрдер_Const.Назва} AS Назва,
    Док_РозхіднийКасовийОрдер.{РозхіднийКасовийОрдер_Const.НомерДок} AS НомерДок,
    Док_РозхіднийКасовийОрдер.{РозхіднийКасовийОрдер_Const.ДатаДок} AS ДатаДок,
    Довідник_Контрагенти.{Контрагенти_Const.Назва} AS КонтрагентНазва,
    Док_РозхіднийКасовийОрдер.{РозхіднийКасовийОрдер_Const.СумаДокументу} AS Сума
FROM
	{РозхіднийКасовийОрдер_Const.TABLE} AS Док_РозхіднийКасовийОрдер

    LEFT JOIN {Контрагенти_Const.TABLE} AS Довідник_Контрагенти ON Довідник_Контрагенти.uid = 
        Док_РозхіднийКасовийОрдер.{РозхіднийКасовийОрдер_Const.Контрагент}
";

			string query_Склад =$@"
SELECT
    'ПереміщенняТоварів',
    Док_ПереміщенняТоварів.uid,
    Док_ПереміщенняТоварів.spend,
    Док_ПереміщенняТоварів.{ПереміщенняТоварів_Const.Назва} AS Назва,
    Док_ПереміщенняТоварів.{ПереміщенняТоварів_Const.НомерДок} AS НомерДок,
    Док_ПереміщенняТоварів.{ПереміщенняТоварів_Const.ДатаДок} AS ДатаДок,
    '' AS КонтрагентНазва,
    0 AS Сума
FROM
	{ПереміщенняТоварів_Const.TABLE} AS Док_ПереміщенняТоварів
";

			string query_Ціноутворення = $@"
SELECT
    'ВстановленняЦінНоменклатури',
    Док_ВстановленняЦінНоменклатури.uid,
    Док_ВстановленняЦінНоменклатури.spend,
    Док_ВстановленняЦінНоменклатури.{ВстановленняЦінНоменклатури_Const.Назва} AS Назва,
    Док_ВстановленняЦінНоменклатури.{ВстановленняЦінНоменклатури_Const.НомерДок} AS НомерДок,
    Док_ВстановленняЦінНоменклатури.{ВстановленняЦінНоменклатури_Const.ДатаДок} AS ДатаДок,
    '' AS КонтрагентНазва,
    0 AS Сума
FROM
	{ВстановленняЦінНоменклатури_Const.TABLE} AS Док_ВстановленняЦінНоменклатури
";

			string query = $@"
{query_Продажі}
UNION
{query_Закупки}
UNION
{query_Фінанси}
UNION
{query_Склад}
UNION
{query_Ціноутворення}

ORDER BY ДатаДок
LIMIT {loadRecordsLimit.Limit}
OFFSET {loadRecordsLimit.Limit * loadRecordsLimit.PageIndex}
";

			Dictionary<string, object> paramQuery = new Dictionary<string, object>();

			string[] columnsName;
			List<object[]> listRow;

			Config.Kernel.DataBase.SelectRequest(query, paramQuery, out columnsName, out listRow);

			loadRecordsLimit.LastCountRow = listRow.Count;

			foreach (object[] row in listRow)
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

        private void toolStripButtonAdd_Click(object sender, EventArgs e)
        {
			//Form_ЗамовленняКлієнтаДокумент form_ЗамовленняКлієнтаДокумент = new Form_ЗамовленняКлієнтаДокумент();
			//form_ЗамовленняКлієнтаДокумент.MdiParent = this.MdiParent;
			//form_ЗамовленняКлієнтаДокумент.IsNew = true;
			//form_ЗамовленняКлієнтаДокумент.OwnerForm = this;
			//form_ЗамовленняКлієнтаДокумент.Show();
        }

        private void toolStripButtonEdit_Click(object sender, EventArgs e)
        {
			if (dataGridViewRecords.SelectedRows.Count > 0)
			{
				int RowIndex = dataGridViewRecords.SelectedRows[0].Index;

				//Form_ЗамовленняКлієнтаДокумент form_ЗамовленняКлієнтаДокумент = new Form_ЗамовленняКлієнтаДокумент();
				//form_ЗамовленняКлієнтаДокумент.MdiParent = this.MdiParent;
				//form_ЗамовленняКлієнтаДокумент.IsNew = false;
				//form_ЗамовленняКлієнтаДокумент.OwnerForm = this;
				//form_ЗамовленняКлієнтаДокумент.Uid = dataGridViewRecords.Rows[RowIndex].Cells["ID"].Value.ToString();
				//form_ЗамовленняКлієнтаДокумент.Show();
			}
		}

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
					string uid = row.Cells["ID"].Value.ToString();

      //              Документи.ЗамовленняКлієнта_Objest замовленняКлієнта_Objest = new Документи.ЗамовленняКлієнта_Objest();
      //              if (замовленняКлієнта_Objest.Read(new UnigueID(uid)))
      //              {
						//Документи.ЗамовленняКлієнта_Objest замовленняКлієнта_Objest_Новий = замовленняКлієнта_Objest.Copy();
						//замовленняКлієнта_Objest_Новий.Назва += " *";
						//замовленняКлієнта_Objest_Новий.ДатаДок = DateTime.Now;
						//замовленняКлієнта_Objest_Новий.НомерДок = (++Константи.НумераціяДокументів.ЗамовленняКлієнта_Const).ToString("D8");

						////Зчитати та скопіювати табличну частину Товари
						//замовленняКлієнта_Objest.Товари_TablePart.Read();
						//замовленняКлієнта_Objest_Новий.Товари_TablePart.Records = замовленняКлієнта_Objest.Товари_TablePart.Copy();
						//замовленняКлієнта_Objest_Новий.Товари_TablePart.Save(true);
						//замовленняКлієнта_Objest_Новий.Save();
					//}
     //               else
     //               {
     //                   MessageBox.Show("Error read");
     //                   break;
     //               }
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

      //              Документи.ЗамовленняКлієнта_Objest ЗамовленняКлієнта_Objest = new Документи.ЗамовленняКлієнта_Objest();
      //              if (ЗамовленняКлієнта_Objest.Read(new UnigueID(uid)))
      //              {
						//ЗамовленняКлієнта_Objest.Delete();
      //              }
      //              else
      //              {
      //                  MessageBox.Show("Error read");
      //                  break;
      //              }
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

				//Звіти.РухПоРугістрах.PrintRecords(new Документи.ЗамовленняКлієнта_Pointer(new UnigueID(uid)));
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

					//Документи.ЗамовленняКлієнта_Pointer замовленняКлієнта_Pointer = new Документи.ЗамовленняКлієнта_Pointer(new UnigueID(uid));
					//Документи.ЗамовленняКлієнта_Objest замовленняКлієнта_Objest = замовленняКлієнта_Pointer.GetDocumentObject(true);

					////Очищення регістрів
					//замовленняКлієнта_Objest.ClearSpendTheDocument();

					//if (spend)
					//	try
					//	{
					//		//Проведення
					//		замовленняКлієнта_Objest.SpendTheDocument();
					//	}
					//	catch (Exception exp)
					//	{
					//		замовленняКлієнта_Objest.ClearSpendTheDocument();
					//		MessageBox.Show(exp.Message);
					//		return;
					//	}
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
    }
}
