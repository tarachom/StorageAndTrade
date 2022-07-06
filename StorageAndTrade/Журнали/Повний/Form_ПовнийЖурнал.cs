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

		public void LoadRecords()
		{
			int selectRow = dataGridViewRecords.SelectedRows.Count > 0 ?
				dataGridViewRecords.SelectedRows[dataGridViewRecords.SelectedRows.Count - 1].Index : 0;

			//RecordsBindingList.Clear();

			string query_Продажі = $@"
SELECT
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

			string query_Закупки = $@"
SELECT
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

			string query = $@"
{query_Продажі}
UNION
{query_Закупки}
UNION
{query_Фінанси}

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
					ID = row[0].ToString(),
					Проведений = (bool)row[1],
					Назва = row[2].ToString(),
					НомерДок = row[3].ToString(),
					ДатаДок = row[4].ToString(),
					Контрагент = row[5].ToString(),
					Сума = (decimal)row[6]
				});
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

		private int GetDisplayedRowsCount()
		{
			int count = dataGridViewRecords.Rows[dataGridViewRecords.FirstDisplayedScrollingRowIndex].Height;
			count = dataGridViewRecords.Height / count;
			return count;
		}

		private void dataGridViewRecords_Scroll(object sender, ScrollEventArgs e)
        {
			int display = dataGridViewRecords.Rows.Count - dataGridViewRecords.DisplayedRowCount(false);
			if (e.ScrollOrientation == ScrollOrientation.VerticalScroll)
			{
				if (e.NewValue >= dataGridViewRecords.Rows.Count - GetDisplayedRowsCount())
				{
					LoadRecords();
					dataGridViewRecords.ClearSelection();
					dataGridViewRecords.FirstDisplayedScrollingRowIndex = display;
					//pageIndex++;
				}
			}
		}
    }
}
