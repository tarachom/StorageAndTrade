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
using StorageAndTrade.СпільніФорми;

namespace StorageAndTrade
{
	public partial class Form_ВнутрішнєСпоживанняТоварів_ТабличнаЧастина_Товари : UserControl
	{
		public Form_ВнутрішнєСпоживанняТоварів_ТабличнаЧастина_Товари()
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

			dataGridViewRecords.Columns["Серія"].Visible = false;
			dataGridViewRecords.Columns["СеріяНазва"].Width = 100;
			dataGridViewRecords.Columns["СеріяНазва"].ReadOnly = true;
			dataGridViewRecords.Columns["СеріяНазва"].HeaderText = "Серія";

			dataGridViewRecords.Columns["КількістьУпаковок"].Width = 50;
			dataGridViewRecords.Columns["КількістьУпаковок"].HeaderText = "Кво.Упак.";

			dataGridViewRecords.Columns["Пакування"].Visible = false;
			dataGridViewRecords.Columns["ПакуванняНазва"].Width = 100;
			dataGridViewRecords.Columns["ПакуванняНазва"].ReadOnly = true;
			dataGridViewRecords.Columns["ПакуванняНазва"].HeaderText = "Пакування";

			dataGridViewRecords.Columns["Ціна"].CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
			dataGridViewRecords.Columns["Сума"].CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
		}

		/// <summary>
		/// Власне документ якому належить таблична частина
		/// </summary>
		public Документи.ВнутрішнєСпоживанняТоварів_Objest ДокументОбєкт { get; set; }

		private void ВнутрішнєСпоживанняТоварів_ТабличнаЧастина_Товари(object sender, EventArgs e)
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
				new Join(Довідники.Номенклатура_Const.TABLE, Документи.ВнутрішнєСпоживанняТоварів_Товари_TablePart.Номенклатура, querySelect.Table));

			//JOIN 2
			querySelect.FieldAndAlias.Add(
				new NameValue<string>(Довідники.ПакуванняОдиниціВиміру_Const.TABLE + "." + Довідники.ПакуванняОдиниціВиміру_Const.Назва, "pak_name"));
			querySelect.Joins.Add(
				new Join(Довідники.ПакуванняОдиниціВиміру_Const.TABLE, Документи.ВнутрішнєСпоживанняТоварів_Товари_TablePart.Пакування, querySelect.Table));

			//JOIN 3
			querySelect.FieldAndAlias.Add(
				new NameValue<string>(Довідники.ХарактеристикиНоменклатури_Const.TABLE + "." + Довідники.ХарактеристикиНоменклатури_Const.Назва, "xar_name"));
			querySelect.Joins.Add(
				new Join(Довідники.ХарактеристикиНоменклатури_Const.TABLE, Документи.ВнутрішнєСпоживанняТоварів_Товари_TablePart.ХарактеристикаНоменклатури, querySelect.Table));

			//JOIN 4
			querySelect.FieldAndAlias.Add(
				new NameValue<string>(Довідники.СеріїНоменклатури_Const.TABLE + "." + Довідники.СеріїНоменклатури_Const.Номер, "seria_number"));
			querySelect.Joins.Add(
				new Join(Довідники.СеріїНоменклатури_Const.TABLE, Документи.ВнутрішнєСпоживанняТоварів_Товари_TablePart.Серія, querySelect.Table));

			//ORDER
			querySelect.Order.Add(Документи.ВнутрішнєСпоживанняТоварів_Товари_TablePart.НомерРядка, SelectOrder.ASC);

			ДокументОбєкт.Товари_TablePart.Read();

			Dictionary<string, Dictionary<string, string>> JoinValue = ДокументОбєкт.Товари_TablePart.JoinValue;

			foreach (Документи.ВнутрішнєСпоживанняТоварів_Товари_TablePart.Record record in ДокументОбєкт.Товари_TablePart.Records)
			{
				RecordsBindingList.Add(new Записи
				{
					ID = record.UID.ToString(),
					НомерРядка = record.НомерРядка,
					Номенклатура = record.Номенклатура,
					НоменклатураНазва = JoinValue[record.UID.ToString()]["tovar_name"],
					Характеристика = record.ХарактеристикаНоменклатури,
					ХарактеристикаНазва = JoinValue[record.UID.ToString()]["xar_name"],
					Серія = record.Серія,
					СеріяНазва = JoinValue[record.UID.ToString()]["seria_number"],
					КількістьУпаковок = record.КількістьУпаковок,
					Пакування = record.Пакування,
					ПакуванняНазва = JoinValue[record.UID.ToString()]["pak_name"],
					Кількість = record.Кількість,
					Ціна = Math.Round(record.Ціна, 2),
					Сума = Math.Round(record.Сума, 2)
				});
			}

			if (selectRow != 0 && selectRow < dataGridViewRecords.Rows.Count)
			{
				dataGridViewRecords.Rows[0].Selected = false;
				dataGridViewRecords.Rows[selectRow].Selected = true;
				dataGridViewRecords.FirstDisplayedScrollingRowIndex = selectRow;
			}
		}

		public decimal ОбчислитиСумуДокументу()
		{
			decimal documentSuma = 0;

			foreach (Записи запис in RecordsBindingList)
				documentSuma += запис.Сума;

			return Math.Round(documentSuma, 2);
		}

		public void SaveRecords()
		{
			ДокументОбєкт.Товари_TablePart.Records.Clear();

			int sequenceNumber = 0;

			foreach (Записи запис in RecordsBindingList)
			{
				Документи.ВнутрішнєСпоживанняТоварів_Товари_TablePart.Record record = new Документи.ВнутрішнєСпоживанняТоварів_Товари_TablePart.Record();

				record.UID = Guid.Parse(запис.ID);
				record.НомерРядка = ++sequenceNumber;
				record.Номенклатура = запис.Номенклатура;
				record.ХарактеристикаНоменклатури = запис.Характеристика;
				record.Серія = запис.Серія;
				record.КількістьУпаковок = запис.КількістьУпаковок;
				record.Пакування = запис.Пакування;
				record.Кількість = запис.Кількість;
				record.Ціна = запис.Ціна;
				record.Сума = запис.Сума;

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
			public Довідники.СеріїНоменклатури_Pointer Серія { get; set; }
			public string СеріяНазва { get; set; }
			public int КількістьУпаковок { get; set; }
			public Довідники.ПакуванняОдиниціВиміру_Pointer Пакування { get; set; }
			public string ПакуванняНазва { get; set; }
			public decimal Кількість { get; set; }
			public decimal Ціна { get; set; }
			public decimal Сума { get; set; }

			public static Записи New()
			{
				return new Записи
				{
					ID = Guid.Empty.ToString(),
					Номенклатура = new Довідники.Номенклатура_Pointer(),
					Характеристика = new Довідники.ХарактеристикиНоменклатури_Pointer(),
					Серія = new Довідники.СеріїНоменклатури_Pointer(),
					КількістьУпаковок = 1,
					Пакування = new Довідники.ПакуванняОдиниціВиміру_Pointer(),
					Кількість = 1
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
					Серія = запис.Серія,
					СеріяНазва = запис.СеріяНазва,
					КількістьУпаковок = запис.КількістьУпаковок,
					Пакування = запис.Пакування,
					ПакуванняНазва = запис.ПакуванняНазва,
					Кількість = запис.Кількість,
					Ціна = запис.Ціна,
					Сума = запис.Сума
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

				if (!запис.Пакування.IsEmpty())
				{
					Довідники.ПакуванняОдиниціВиміру_Objest пакуванняОдиниціВиміру_Objest = запис.Пакування.GetDirectoryObject();
					if (пакуванняОдиниціВиміру_Objest != null)
					{
						запис.ПакуванняНазва = пакуванняОдиниціВиміру_Objest.Назва;
						запис.КількістьУпаковок = пакуванняОдиниціВиміру_Objest.КількістьУпаковок;
					}
					else
					{
						запис.ПакуванняНазва = "";
						запис.КількістьУпаковок = 1;
					}
				}
			}
			public static void ПісляЗміни_Характеристика(Записи запис)
			{
				запис.ХарактеристикаНазва = запис.Характеристика.GetPresentation();
			}
			public static void ПісляЗміни_Серія(Записи запис)
			{
				запис.СеріяНазва = запис.Серія.GetPresentation();
			}
			public static void ПісляЗміни_Пакування(Записи запис)
			{
				запис.ПакуванняНазва = запис.Пакування.GetPresentation();
			}
		}

		#region Вибір, Пошук, Зміна

		private void dataGridViewRecords_CellEndEdit(object sender, DataGridViewCellEventArgs e)
		{
			string columnName = dataGridViewRecords.Columns[e.ColumnIndex].Name;

			Записи запис = RecordsBindingList[e.RowIndex];

			if (columnName == "Кількість" || columnName == "Ціна")
			{
				запис.Сума = запис.Кількість * запис.Ціна;
				dataGridViewRecords.Refresh();
			}
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
					case "СеріяНазва":
						{
							запис.Серія = new Довідники.СеріїНоменклатури_Pointer();
							Записи.ПісляЗміни_Серія(запис);
							break;
						}
					case "ПакуванняНазва":
						{
							запис.Пакування = new Довідники.ПакуванняОдиниціВиміру_Pointer();
							Записи.ПісляЗміни_Пакування(запис);
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
					new string[] { "НоменклатураНазва", "ХарактеристикаНазва", "СеріяНазва", "ПакуванняНазва" }, SelectClick, FindTextChanged);
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
				case "СеріяНазва":
					{
						Form_СеріїНоменклатури form_СеріїНоменклатури = new Form_СеріїНоменклатури();
						form_СеріїНоменклатури.DirectoryPointerItem = запис.Серія;
						form_СеріїНоменклатури.ShowDialog();

						запис.Серія = (Довідники.СеріїНоменклатури_Pointer)form_СеріїНоменклатури.DirectoryPointerItem;
						Записи.ПісляЗміни_Серія(запис);

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
				case "СеріяНазва":
					{
						query = ПошуковіЗапити.СеріїНоменклатури;
						break;
					}
				case "ПакуванняНазва":
					{
						query = ПошуковіЗапити.ПакуванняОдиниціВиміру;
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
				case "СеріяНазва":
					{
						запис.Серія = new Довідники.СеріїНоменклатури_Pointer(new UnigueID(uid));
						Записи.ПісляЗміни_Серія(запис);
						break;
					}
				case "ПакуванняНазва":
					{
						запис.Пакування = new Довідники.ПакуванняОдиниціВиміру_Pointer(new UnigueID(uid));
						Записи.ПісляЗміни_Пакування(запис);
						break;
					}
				default:
					break;
			}

			dataGridViewRecords.Refresh();
		}

		#endregion

		#region Меню таб.частини

		private void toolStripButtonAdd_Click(object sender, EventArgs e)
		{
			RecordsBindingList.Add(Записи.New());

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

        #region ШтрихКоди

        private void toolStripButton_ШтрихКоди_Click(object sender, EventArgs e)
        {
            Form_ПідбірПоШтрихКоду form_ПідбірПоШтрихКоду = new Form_ПідбірПоШтрихКоду();

            form_ПідбірПоШтрихКоду.ДодатиТовариВДокумент_CallBack = (List<СписокНоменклатури> СписокНоменклатуриЗПідбору) =>
            {
                foreach (СписокНоменклатури списокНоменклатури in СписокНоменклатуриЗПідбору)
                {
                    Записи запис = Записи.New();

                    запис.Номенклатура = списокНоменклатури.Номенклатура;
                    запис.Характеристика = списокНоменклатури.Характеристика;
                    запис.Пакування = списокНоменклатури.Пакування;
                    запис.Кількість = списокНоменклатури.Кількість;

                    Записи.ПісляЗміни_Номенклатура(запис);
                    Записи.ПісляЗміни_Характеристика(запис);

                    RecordsBindingList.Add(запис);
                }
            };

            form_ПідбірПоШтрихКоду.ShowDialog();
        }

        #endregion
    }
}
