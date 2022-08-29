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
using РегістриВідомостей = StorageAndTrade_1_0.РегістриВідомостей;

namespace StorageAndTrade
{
    public partial class Form_ВстановленняЦінНоменклатури_ТабличнаЧастина_Товари : UserControl
    {
        public Form_ВстановленняЦінНоменклатури_ТабличнаЧастина_Товари()
        {
            InitializeComponent();

			RecordsBindingList = new BindingList<Записи>();
			dataGridViewRecords.DataSource = RecordsBindingList;

			dataGridViewRecords.Columns["ID"].Visible = false;

			dataGridViewRecords.Columns["НомерРядка"].Width = 30;
			dataGridViewRecords.Columns["НомерРядка"].ReadOnly = true;
			dataGridViewRecords.Columns["НомерРядка"].HeaderText = "№";

			dataGridViewRecords.Columns["Номенклатура"].Visible = false;
			dataGridViewRecords.Columns["НоменклатураНазва"].Width = 200;
			dataGridViewRecords.Columns["НоменклатураНазва"].ReadOnly = true;
			dataGridViewRecords.Columns["НоменклатураНазва"].HeaderText = "Номенклатура";

			dataGridViewRecords.Columns["Характеристика"].Visible = false;
			dataGridViewRecords.Columns["ХарактеристикаНазва"].Width = 200;
			dataGridViewRecords.Columns["ХарактеристикаНазва"].ReadOnly = true;
			dataGridViewRecords.Columns["ХарактеристикаНазва"].HeaderText = "Характеристика";

			dataGridViewRecords.Columns["Пакування"].Visible = false;
			dataGridViewRecords.Columns["ПакуванняНазва"].Width = 100;
			dataGridViewRecords.Columns["ПакуванняНазва"].ReadOnly = true;
			dataGridViewRecords.Columns["ПакуванняНазва"].HeaderText = "Пакування";

			dataGridViewRecords.Columns["ВидиЦін"].Visible = false;
			dataGridViewRecords.Columns["ВидиЦінНазва"].Width = 200;
			dataGridViewRecords.Columns["ВидиЦінНазва"].ReadOnly = true;
			dataGridViewRecords.Columns["ВидиЦінНазва"].HeaderText = "Види цін";
		}

		/// <summary>
		/// Власне документ якому належить таблична частина
		/// </summary>
		public Документи.ВстановленняЦінНоменклатури_Objest ДокументОбєкт { get; set; }

		/// <summary>
		/// Процедура яка обновлює значення ДокументОбєкт значеннями з форми
		/// </summary>
		public Action ОбновитиЗначенняЗФормиДокумента { get; set; }

		private void ВстановленняЦінНоменклатури_ТабличнаЧастина_Товари(object sender, EventArgs e)
        {
			
		}

		private BindingList<Записи> RecordsBindingList { get; set; }

		public void LoadRecords()
		{
			int selectRow = dataGridViewRecords.SelectedRows.Count > 0 ?
				dataGridViewRecords.SelectedRows[dataGridViewRecords.SelectedRows.Count - 1].Index : 0;

			RecordsBindingList.Clear();

			Query querySelect = ДокументОбєкт.Товари_TablePart.QuerySelect;
			querySelect.Clear();

			//JOIN 1
			querySelect.FieldAndAlias.Add(
				new NameValue<string>(Довідники.Номенклатура_Const.TABLE + "." + Довідники.Номенклатура_Const.Назва, "tovar_name"));
			querySelect.Joins.Add(
				new Join(Довідники.Номенклатура_Const.TABLE, Документи.ВстановленняЦінНоменклатури_Товари_TablePart.Номенклатура, querySelect.Table));

			//JOIN 2
			querySelect.FieldAndAlias.Add(
				new NameValue<string>(Довідники.ПакуванняОдиниціВиміру_Const.TABLE + "." + Довідники.ПакуванняОдиниціВиміру_Const.Назва, "pak_name"));
			querySelect.Joins.Add(
				new Join(Довідники.ПакуванняОдиниціВиміру_Const.TABLE, Документи.ВстановленняЦінНоменклатури_Товари_TablePart.Пакування, querySelect.Table));

			//JOIN 3
			querySelect.FieldAndAlias.Add(
				new NameValue<string>(Довідники.ХарактеристикиНоменклатури_Const.TABLE + "." + Довідники.ХарактеристикиНоменклатури_Const.Назва, "xar_name"));
			querySelect.Joins.Add(
				new Join(Довідники.ХарактеристикиНоменклатури_Const.TABLE, Документи.ВстановленняЦінНоменклатури_Товари_TablePart.ХарактеристикаНоменклатури, querySelect.Table));

			//JOIN 4
			querySelect.FieldAndAlias.Add(
				new NameValue<string>(Довідники.ВидиЦін_Const.TABLE + "." + Довідники.ВидиЦін_Const.Назва, "vid_cen_name"));
			querySelect.Joins.Add(
				new Join(Довідники.ВидиЦін_Const.TABLE, Документи.ВстановленняЦінНоменклатури_Товари_TablePart.ВидЦіни, querySelect.Table));

			//ORDER
			querySelect.Order.Add(Документи.ВстановленняЦінНоменклатури_Товари_TablePart.НомерРядка, SelectOrder.ASC);

			ДокументОбєкт.Товари_TablePart.Read();

			Dictionary<string, Dictionary<string, string>> JoinValue = ДокументОбєкт.Товари_TablePart.JoinValue;

			foreach (Документи.ВстановленняЦінНоменклатури_Товари_TablePart.Record record in ДокументОбєкт.Товари_TablePart.Records)
			{
				RecordsBindingList.Add(new Записи
				{
					ID = record.UID.ToString(),
					НомерРядка = record.НомерРядка,
					Номенклатура = record.Номенклатура,
					НоменклатураНазва = JoinValue[record.UID.ToString()]["tovar_name"],
					Характеристика = record.ХарактеристикаНоменклатури,
					ХарактеристикаНазва = JoinValue[record.UID.ToString()]["xar_name"],
					Пакування = record.Пакування,
					ПакуванняНазва = JoinValue[record.UID.ToString()]["pak_name"],
					ВидиЦін = record.ВидЦіни,
					ВидиЦінНазва = JoinValue[record.UID.ToString()]["vid_cen_name"],
					Ціна = record.Ціна
				});
			}

			if (selectRow != 0 && selectRow < dataGridViewRecords.Rows.Count)
			{
				dataGridViewRecords.Rows[0].Selected = false;
				dataGridViewRecords.Rows[selectRow].Selected = true;
				dataGridViewRecords.FirstDisplayedScrollingRowIndex = selectRow;
			}
		}

		public void SaveRecords()
        {
			ДокументОбєкт.Товари_TablePart.Records.Clear();

			int sequenceNumber = 0;

			foreach (Записи запис in RecordsBindingList)
			{
				Документи.ВстановленняЦінНоменклатури_Товари_TablePart.Record record = new Документи.ВстановленняЦінНоменклатури_Товари_TablePart.Record();

				record.UID = Guid.Parse(запис.ID);
				record.НомерРядка = ++sequenceNumber;
				record.Номенклатура = запис.Номенклатура;
				record.ХарактеристикаНоменклатури = запис.Характеристика;
				record.Пакування = запис.Пакування;
				record.ВидЦіни = запис.ВидиЦін;
				record.Ціна = запис.Ціна;

				ДокументОбєкт.Товари_TablePart.Records.Add(record);
			}

			ДокументОбєкт.Товари_TablePart.Save(true);
		}

		private class Записи
        {
			public string ID { get; set; }
			public int НомерРядка { get; set; }
			public Довідники.Номенклатура_Pointer Номенклатура { get; set; }
            public string НоменклатураНазва { get; set; }
			public Довідники.ХарактеристикиНоменклатури_Pointer Характеристика { get; set; }
			public string ХарактеристикаНазва { get; set; }
			public Довідники.ПакуванняОдиниціВиміру_Pointer Пакування { get; set; }
			public string ПакуванняНазва { get; set; }
			public Довідники.ВидиЦін_Pointer ВидиЦін { get; set; }
			public string ВидиЦінНазва { get; set; }
			public decimal Ціна { get; set; }

			public static Записи New(Документи.ВстановленняЦінНоменклатури_Objest ДокументОбєкт)
            {
				return new Записи
				{
					ID = Guid.Empty.ToString(),
					Номенклатура = new Довідники.Номенклатура_Pointer(),
					Характеристика = new Довідники.ХарактеристикиНоменклатури_Pointer(),
					Пакування = new Довідники.ПакуванняОдиниціВиміру_Pointer(),
					ВидиЦін = ДокументОбєкт.ВидЦіни,
					ВидиЦінНазва = (!ДокументОбєкт.ВидЦіни.IsEmpty() ? ДокументОбєкт.ВидЦіни.GetPresentation(): ""),
					Ціна = 0
				};
			}
			public static Записи Clone(Записи запис)
            {
				return new Записи
				{
					ID = Guid.Empty.ToString(),
					Номенклатура = запис.Номенклатура,
					НоменклатураНазва = запис.НоменклатураНазва,
					Характеристика = запис.Характеристика,
					ХарактеристикаНазва = запис.ХарактеристикаНазва,
					Пакування = запис.Пакування,
					ПакуванняНазва = запис.ПакуванняНазва,
					ВидиЦін = запис.ВидиЦін,
					ВидиЦінНазва = запис.ВидиЦінНазва,
					Ціна = запис.Ціна
				};
            }

			public static void ПісляЗміни_Номенклатура(Записи запис)
			{
				if (запис.Номенклатура.IsEmpty())
				{
					запис.НоменклатураНазва = "";
					return;
				}

				Довідники.Номенклатура_Objest номенклатура_Objest = запис.Номенклатура.GetDirectoryObject();
				if (номенклатура_Objest != null)
				{
					запис.НоменклатураНазва = номенклатура_Objest.Назва;

					if (!номенклатура_Objest.ОдиницяВиміру.IsEmpty())
						запис.Пакування = номенклатура_Objest.ОдиницяВиміру;
				}
				else
				{
					запис.НоменклатураНазва = "";
					запис.Пакування = new Довідники.ПакуванняОдиниціВиміру_Pointer();
				}

				запис.ПакуванняНазва = запис.Пакування.GetPresentation();
			}
			public static void ПісляЗміни_Характеристика(Записи запис)
			{
				запис.ХарактеристикаНазва = запис.Характеристика.GetPresentation();
			}
			public static void ПісляЗміни_Пакування(Записи запис)
			{
				запис.ПакуванняНазва = запис.Пакування.GetPresentation();
			}
			public static void ПісляЗміни_ВидиЦін(Записи запис)
			{
				запис.ВидиЦінНазва = запис.ВидиЦін.GetPresentation();
			}
		}
        
		#region Вибір, Пошук, Зміна

		private void dataGridViewRecords_CellEndEdit(object sender, DataGridViewCellEventArgs e)
		{
			//string columnName = dataGridViewRecords.Columns[e.ColumnIndex].Name;

			//Записи запис = RecordsBindingList[e.RowIndex];
		}

		private void dataGridViewRecords_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
				dataGridViewRecords_CellDoubleClick(sender,
					new DataGridViewCellEventArgs(dataGridViewRecords.CurrentCell.ColumnIndex, dataGridViewRecords.CurrentCell.RowIndex));
			else if (e.KeyCode == Keys.Delete)
			{
				if (dataGridViewRecords.CurrentCell == null)
					return;

				string columnName = dataGridViewRecords.Columns[dataGridViewRecords.CurrentCell.ColumnIndex].Name;
				Записи запис = RecordsBindingList[dataGridViewRecords.CurrentCell.RowIndex];

				switch (columnName)
				{
					case "НоменклатураНазва":
						{
							запис.Номенклатура = new Довідники.Номенклатура_Pointer();
							Записи.ПісляЗміни_Номенклатура(запис);
							break;
						}
					case "ХарактеристикаНазва":
						{
							запис.Характеристика = new Довідники.ХарактеристикиНоменклатури_Pointer();
							Записи.ПісляЗміни_Характеристика(запис);
							break;
						}
					case "ПакуванняНазва":
						{
							запис.Пакування = new Довідники.ПакуванняОдиниціВиміру_Pointer();
							Записи.ПісляЗміни_Пакування(запис);
							break;
						}
					case "ВидиЦінНазва":
						{
							запис.ВидиЦін = new Довідники.ВидиЦін_Pointer();
							Записи.ПісляЗміни_ВидиЦін(запис);
							break;
						}
					default:
						break;
				}

				dataGridViewRecords.Refresh();
			}
			else if (e.KeyCode == Keys.Insert)
				toolStripButtonAdd_Click(sender, new EventArgs());
		}

		private void dataGridViewRecords_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.ColumnIndex >= 0 && e.RowIndex >= 0)
				ФункціїДляДокументів.ВідкритиМенюВибору(dataGridViewRecords, e.ColumnIndex, e.RowIndex, RecordsBindingList[e.RowIndex],
					new string[] {
					"НоменклатураНазва", "ХарактеристикаНазва", "ПакуванняНазва", "ВидиЦінНазва"
					}, SelectClick, FindTextChanged);
		}

		private void SelectClick(object sender, EventArgs e)
		{
			ToolStripMenuItem selectMenu = (ToolStripMenuItem)sender;
			Записи запис = (Записи)selectMenu.Tag;

			switch (selectMenu.Name)
			{
				case "НоменклатураНазва":
					{
						Form_Номенклатура form_Номенклатура = new Form_Номенклатура();
						form_Номенклатура.DirectoryPointerItem = запис.Номенклатура;
						form_Номенклатура.ShowDialog();

						запис.Номенклатура = (Довідники.Номенклатура_Pointer)form_Номенклатура.DirectoryPointerItem;
						Записи.ПісляЗміни_Номенклатура(запис);

						break;
					}
				case "ХарактеристикаНазва":
					{
						Form_ХарактеристикиНоменклатури form_ХарактеристикиНоменклатури = new Form_ХарактеристикиНоменклатури();
						form_ХарактеристикиНоменклатури.DirectoryPointerItem = запис.Характеристика;
						form_ХарактеристикиНоменклатури.НоменклатураВласник = запис.Номенклатура;
						form_ХарактеристикиНоменклатури.ShowDialog();

						запис.Характеристика = (Довідники.ХарактеристикиНоменклатури_Pointer)form_ХарактеристикиНоменклатури.DirectoryPointerItem;
						Записи.ПісляЗміни_Характеристика(запис);

						break;
					}
				case "ПакуванняНазва":
					{
						Form_ПакуванняОдиниціВиміру form_ПакуванняОдиниціВиміру = new Form_ПакуванняОдиниціВиміру();
						form_ПакуванняОдиниціВиміру.DirectoryPointerItem = запис.Пакування;
						form_ПакуванняОдиниціВиміру.ShowDialog();

						запис.Пакування = (Довідники.ПакуванняОдиниціВиміру_Pointer)form_ПакуванняОдиниціВиміру.DirectoryPointerItem;
						Записи.ПісляЗміни_Пакування(запис);

						break;
					}
				case "ВидиЦінНазва":
					{
						Form_ВидиЦін form_ВидиЦін = new Form_ВидиЦін();
						form_ВидиЦін.DirectoryPointerItem = запис.ВидиЦін;
						form_ВидиЦін.ShowDialog();

						запис.ВидиЦін = (Довідники.ВидиЦін_Pointer)form_ВидиЦін.DirectoryPointerItem;
						Записи.ПісляЗміни_ВидиЦін(запис);

						break;
					}
				default:
					break;
			}

			dataGridViewRecords.Refresh();
		}

		private void FindTextChanged(object sender, EventArgs e)
		{
			ToolStripTextBox findMenu = (ToolStripTextBox)sender;
			Записи запис = (Записи)findMenu.Tag;

			ToolStrip parent = findMenu.GetCurrentParent();

			ФункціїДляДокументів.ОчиститиМенюПошуку(parent);

			string findText = findMenu.Text.TrimStart();

			if (String.IsNullOrWhiteSpace(findMenu.Text))
				findText = "%";

			string query = "";

			switch (findMenu.Name)
			{
				case "НоменклатураНазва":
					{
						query = ПошуковіЗапити.Номенклатура;
						break;
					}
				case "ХарактеристикаНазва":
					{
						query = ПошуковіЗапити.ХарактеристикаНоменклатуриЗВідбором(запис.Номенклатура);
						break;
					}
				case "ПакуванняНазва":
					{
						query = ПошуковіЗапити.ПакуванняОдиниціВиміру;
						break;
					}
				case "ВидиЦінНазва":
					{
						query = ПошуковіЗапити.ВидиЦін;
						break;
					}
				default:
					return;
			}

			ФункціїДляДокументів.ЗаповнитиМенюПошуку(parent, query, findText, findMenu.Name, запис, FindClick);
		}

		private void FindClick(object sender, EventArgs e)
		{
			ToolStripMenuItem selectMenu = (ToolStripMenuItem)sender;
			NameValue<object> nameValue = (NameValue<object>)selectMenu.Tag;

			string uid = nameValue.Name;
			Записи запис = (Записи)nameValue.Value;

			switch (selectMenu.Name)
			{
				case "НоменклатураНазва":
					{
						запис.Номенклатура = new Довідники.Номенклатура_Pointer(new UnigueID(uid));
						Записи.ПісляЗміни_Номенклатура(запис);

						break;
					}
				case "ХарактеристикаНазва":
					{
						запис.Характеристика = new Довідники.ХарактеристикиНоменклатури_Pointer(new UnigueID(uid));
						Записи.ПісляЗміни_Характеристика(запис);
						break;
					}
				case "ПакуванняНазва":
					{
						запис.Пакування = new Довідники.ПакуванняОдиниціВиміру_Pointer(new UnigueID(uid));
						Записи.ПісляЗміни_Пакування(запис);
						break;
					}
				case "ВидиЦінНазва":
					{
						запис.ВидиЦін = new Довідники.ВидиЦін_Pointer(new UnigueID(uid));
						Записи.ПісляЗміни_ВидиЦін(запис);

						break;
					}
				default:
					break;
			}

			dataGridViewRecords.Refresh();
		}

        #endregion

        #region Menu TablePart

		private void toolStripButtonAdd_Click(object sender, EventArgs e)
		{
			if (ОбновитиЗначенняЗФормиДокумента != null)
				ОбновитиЗначенняЗФормиДокумента.Invoke();

			RecordsBindingList.Add(Записи.New(ДокументОбєкт));

			dataGridViewRecords.Focus();

			dataGridViewRecords.ClearSelection();
			dataGridViewRecords.CurrentCell = dataGridViewRecords.Rows[dataGridViewRecords.Rows.Count - 1].Cells["НоменклатураНазва"];
			dataGridViewRecords.CurrentCell.Selected = true;
		}

        private void toolStripButtonDelete_Click(object sender, EventArgs e)
        {
			if (dataGridViewRecords.SelectedCells.Count > 0)
			{
				List<int> deleteRowIndex = new List<int>();

				for (int i = 0; i < dataGridViewRecords.SelectedCells.Count; i++)
					if (!deleteRowIndex.Contains(dataGridViewRecords.SelectedCells[i].RowIndex) &&
						!dataGridViewRecords.Rows[dataGridViewRecords.SelectedCells[i].RowIndex].IsNewRow)
						deleteRowIndex.Add(dataGridViewRecords.SelectedCells[i].RowIndex);

				deleteRowIndex.Sort();

				foreach (int rowIndex in deleteRowIndex.Reverse<int>())
					RecordsBindingList.RemoveAt(rowIndex);
			}
        }

		private void toolStripButtonCopy_Click(object sender, EventArgs e)
		{
			if (dataGridViewRecords.SelectedCells.Count > 0)
			{
				List<int> rowIndexList = new List<int>();

				for (int i = 0; i < dataGridViewRecords.SelectedCells.Count; i++)
					if (!rowIndexList.Contains(dataGridViewRecords.SelectedCells[i].RowIndex))
					{
						int rowIndex = dataGridViewRecords.SelectedCells[i].RowIndex;
						rowIndexList.Add(rowIndex);
						RecordsBindingList.Add(Записи.Clone(RecordsBindingList[rowIndex]));
					}
			}
		}

		#endregion

		private void toolStripButtonFillRegister_Click(object sender, EventArgs e)
        {
			if (ОбновитиЗначенняЗФормиДокумента != null)
				ОбновитиЗначенняЗФормиДокумента.Invoke();

			string query = $@"
WITH register AS
(
    SELECT DISTINCT {РегістриВідомостей.ЦіниНоменклатури_Const.Номенклатура} AS Номенклатура,
        {РегістриВідомостей.ЦіниНоменклатури_Const.ХарактеристикаНоменклатури} AS ХарактеристикаНоменклатури,
        {РегістриВідомостей.ЦіниНоменклатури_Const.Пакування} AS Пакування,
        {РегістриВідомостей.ЦіниНоменклатури_Const.ВидЦіни} AS ВидЦіни
    FROM
        {РегістриВідомостей.ЦіниНоменклатури_Const.TABLE}
    WHERE
        {РегістриВідомостей.ЦіниНоменклатури_Const.Валюта} = @valuta";

            #region WHERE

            if (!ДокументОбєкт.ВидЦіни.IsEmpty())
            {
				query += $@"
AND {РегістриВідомостей.ЦіниНоменклатури_Const.ВидЦіни} = @vid_cen
";
            }

            #endregion

            query += $@"
)
SELECT
    register.Номенклатура,
    Довідник_Номенклатура.{Довідники.Номенклатура_Const.Назва} AS Номенклатура_Назва,
    register.ХарактеристикаНоменклатури,
    Довідник_ХарактеристикиНоменклатури.{Довідники.ХарактеристикиНоменклатури_Const.Назва} AS ХарактеристикаНоменклатури_Назва,
    register.Пакування,
    Довідник_ПакуванняОдиниціВиміру.{Довідники.ПакуванняОдиниціВиміру_Const.Назва} AS Пакування_Назва,
    register.ВидЦіни,
    Довідник_ВидиЦін.{Довідники.ВидиЦін_Const.Назва} AS ВидЦіни_Назва,
    (
        SELECT 
            {РегістриВідомостей.ЦіниНоменклатури_Const.Ціна}
        FROM 
            {РегістриВідомостей.ЦіниНоменклатури_Const.TABLE} AS ЦіниНоменклатури
        WHERE
            ЦіниНоменклатури.{РегістриВідомостей.ЦіниНоменклатури_Const.Номенклатура} = register.Номенклатура AND
            ЦіниНоменклатури.{РегістриВідомостей.ЦіниНоменклатури_Const.ХарактеристикаНоменклатури} = register.ХарактеристикаНоменклатури AND
            ЦіниНоменклатури.{РегістриВідомостей.ЦіниНоменклатури_Const.Пакування} = register.Пакування AND
            ЦіниНоменклатури.{РегістриВідомостей.ЦіниНоменклатури_Const.ВидЦіни} = register.ВидЦіни AND
            ЦіниНоменклатури.{РегістриВідомостей.ЦіниНоменклатури_Const.Валюта} = @valuta
        ORDER BY ЦіниНоменклатури.period DESC
        LIMIT 1
    ) AS Ціна
FROM
    register
    
    LEFT JOIN {Довідники.Номенклатура_Const.TABLE} AS Довідник_Номенклатура ON 
        Довідник_Номенклатура.uid = register.Номенклатура

    LEFT JOIN {Довідники.ХарактеристикиНоменклатури_Const.TABLE} AS Довідник_ХарактеристикиНоменклатури ON 
        Довідник_ХарактеристикиНоменклатури.uid = register.ХарактеристикаНоменклатури

    LEFT JOIN {Довідники.ПакуванняОдиниціВиміру_Const.TABLE} AS Довідник_ПакуванняОдиниціВиміру ON 
        Довідник_ПакуванняОдиниціВиміру.uid = register.Пакування

    LEFT JOIN {Довідники.ВидиЦін_Const.TABLE} AS Довідник_ВидиЦін ON 
        Довідник_ВидиЦін.uid = register.ВидЦіни
ORDER BY
    Номенклатура_Назва, ХарактеристикаНоменклатури_Назва, Пакування_Назва, ВидЦіни_Назва
";

			RecordsBindingList.Clear();

			Dictionary<string, object> paramQuery = new Dictionary<string, object>();
			paramQuery.Add("valuta", ДокументОбєкт.Валюта.UnigueID.UGuid);
			paramQuery.Add("vid_cen", ДокументОбєкт.ВидЦіни.UnigueID.UGuid);

			string[] columnsName;
			List<Dictionary<string,object>> listRow;

			Конфа.Config.Kernel.DataBase.SelectRequest(query, paramQuery, out columnsName, out listRow);

			foreach (Dictionary<string, object> row in listRow)
            {
				RecordsBindingList.Add(new Записи
				{
					ID = Guid.Empty.ToString(),
					Номенклатура = new Довідники.Номенклатура_Pointer(row["Номенклатура"]),
					НоменклатураНазва = row["Номенклатура_Назва"].ToString(),
					Характеристика = new Довідники.ХарактеристикиНоменклатури_Pointer(row["ХарактеристикаНоменклатури"]),
					ХарактеристикаНазва = row["ХарактеристикаНоменклатури_Назва"].ToString(),
					Пакування = new Довідники.ПакуванняОдиниціВиміру_Pointer(row["Пакування"]),
					ПакуванняНазва = row["Пакування_Назва"].ToString(),
					ВидиЦін = new Довідники.ВидиЦін_Pointer(row["ВидЦіни"]),
					ВидиЦінНазва = row["ВидЦіни_Назва"].ToString(),
					Ціна = (decimal)row["Ціна"]
				});
			}

		}

        private void toolStripButtonFillDirectory_Click(object sender, EventArgs e)
        {
			if (ОбновитиЗначенняЗФормиДокумента != null)
				ОбновитиЗначенняЗФормиДокумента.Invoke();

			string query = $@"
SELECT
    Номенклатура.uid AS Номенклатура,
    Номенклатура.{Довідники.Номенклатура_Const.Назва} AS Номенклатура_Назва,
    Номенклатура.{Довідники.Номенклатура_Const.ОдиницяВиміру} AS Пакування,
    Довідник_ПакуванняОдиниціВиміру.{Довідники.ПакуванняОдиниціВиміру_Const.Назва} AS Пакування_Назва,
    (
        SELECT
            {РегістриВідомостей.ЦіниНоменклатури_Const.Ціна}
        FROM
            {РегістриВідомостей.ЦіниНоменклатури_Const.TABLE} AS ЦіниНоменклатури
        WHERE
            ЦіниНоменклатури.{РегістриВідомостей.ЦіниНоменклатури_Const.Номенклатура} = Номенклатура.uid AND
            ЦіниНоменклатури.{РегістриВідомостей.ЦіниНоменклатури_Const.ХарактеристикаНоменклатури} = '{Guid.Empty}' AND
            ЦіниНоменклатури.{РегістриВідомостей.ЦіниНоменклатури_Const.Пакування} = Номенклатура.{Довідники.Номенклатура_Const.ОдиницяВиміру} AND
            ЦіниНоменклатури.{РегістриВідомостей.ЦіниНоменклатури_Const.ВидЦіни} = @vid_cen AND
            ЦіниНоменклатури.{РегістриВідомостей.ЦіниНоменклатури_Const.Валюта} = @valuta
        ORDER BY ЦіниНоменклатури.period DESC
        LIMIT 1
    ) AS Ціна
FROM
    {Довідники.Номенклатура_Const.TABLE} AS Номенклатура

    LEFT JOIN {Довідники.ПакуванняОдиниціВиміру_Const.TABLE} AS Довідник_ПакуванняОдиниціВиміру ON 
        Довідник_ПакуванняОдиниціВиміру.uid = Номенклатура.{Довідники.Номенклатура_Const.ОдиницяВиміру}
WHERE
    Номенклатура.{Довідники.Номенклатура_Const.ТипНоменклатури} = {(int)Перелічення.ТипиНоменклатури.Товар} OR
    Номенклатура.{Довідники.Номенклатура_Const.ТипНоменклатури} = {(int)Перелічення.ТипиНоменклатури.Послуга}
ORDER BY Номенклатура_Назва, Пакування_Назва
";

			RecordsBindingList.Clear();

			Dictionary<string, object> paramQuery = new Dictionary<string, object>();
			paramQuery.Add("valuta", ДокументОбєкт.Валюта.UnigueID.UGuid);
			paramQuery.Add("vid_cen", ДокументОбєкт.ВидЦіни.UnigueID.UGuid);

			string[] columnsName;
			List<Dictionary<string, object>> listRow;

			Конфа.Config.Kernel.DataBase.SelectRequest(query, paramQuery, out columnsName, out listRow);

			string ВидиЦінНазва = ДокументОбєкт.ВидЦіни.GetPresentation();

			foreach (Dictionary<string, object> row in listRow)
			{
				RecordsBindingList.Add(new Записи
				{
					ID = Guid.Empty.ToString(),
					Номенклатура = new Довідники.Номенклатура_Pointer(row["Номенклатура"]),
					НоменклатураНазва = row["Номенклатура_Назва"].ToString(),
					Характеристика = new Довідники.ХарактеристикиНоменклатури_Pointer(),
					ХарактеристикаНазва = "",
					Пакування = new Довідники.ПакуванняОдиниціВиміру_Pointer(row["Пакування"]),
					ПакуванняНазва = row["Пакування_Назва"].ToString(),
					ВидиЦін = ДокументОбєкт.ВидЦіни,
					ВидиЦінНазва = ВидиЦінНазва,
					Ціна = (row["Ціна"] != DBNull.Value? (decimal)row["Ціна"] : 0)
				});
			}
		}

        private void видалитиТовариЗЦіноюНульToolStripMenuItem_Click(object sender, EventArgs e)
        {
			List<Записи> removeList = new List<Записи>();

            foreach (Записи запис in RecordsBindingList)
            {
				if (запис.Ціна == 0)
					removeList.Add(запис);
            }

            foreach (Записи запис in removeList)
				RecordsBindingList.Remove(запис);
        }

        private void встановитиВидЦіниToolStripMenuItem_Click(object sender, EventArgs e)
        {
			if (ОбновитиЗначенняЗФормиДокумента != null)
				ОбновитиЗначенняЗФормиДокумента.Invoke();

			string ВидиЦінНазва = ДокументОбєкт.ВидЦіни.GetPresentation();

			foreach (Записи запис in RecordsBindingList)
			{
				запис.ВидиЦін = ДокументОбєкт.ВидЦіни;
				запис.ВидиЦінНазва = ВидиЦінНазва;
			}

			dataGridViewRecords.Refresh();
		}
 
    }
}
