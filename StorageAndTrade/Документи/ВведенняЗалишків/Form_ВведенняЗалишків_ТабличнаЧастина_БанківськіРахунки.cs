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
using Конфа = StorageAndTrade_1_0;
using Константи = StorageAndTrade_1_0.Константи;
using Довідники = StorageAndTrade_1_0.Довідники;
using Документи = StorageAndTrade_1_0.Документи;
using Перелічення = StorageAndTrade_1_0.Перелічення;

namespace StorageAndTrade
{
	public partial class Form_ВведенняЗалишків_ТабличнаЧастина_БанківськіРахунки : UserControl
	{
		public Form_ВведенняЗалишків_ТабличнаЧастина_БанківськіРахунки()
		{
			InitializeComponent();

			RecordsBindingList = new BindingList<Записи>();
			dataGridViewRecords.DataSource = RecordsBindingList;

			dataGridViewRecords.Columns["ID"].Visible = false;

			dataGridViewRecords.Columns["НомерРядка"].Width = 30;
			dataGridViewRecords.Columns["НомерРядка"].ReadOnly = true;
			dataGridViewRecords.Columns["НомерРядка"].HeaderText = "№";

			dataGridViewRecords.Columns["БанківськийРахунок"].Visible = false;
			dataGridViewRecords.Columns["БанківськийРахунокНазва"].Width = 300;
			dataGridViewRecords.Columns["БанківськийРахунокНазва"].ReadOnly = true;
			dataGridViewRecords.Columns["БанківськийРахунокНазва"].HeaderText = "Банківський рахунок";

			dataGridViewRecords.Columns["Сума"].CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
		}

		/// <summary>
		/// Власне документ якому належить таблична частина
		/// </summary>
		public Документи.ВведенняЗалишків_Objest ДокументОбєкт { get; set; }

		private void Form_ВведенняЗалишків_ТабличнаЧастина_БанківськіРахунки_Load(object sender, EventArgs e)
		{
			
		}

		private BindingList<Записи> RecordsBindingList { get; set; }

		public void LoadRecords()
		{
			int selectRow = dataGridViewRecords.SelectedRows.Count > 0 ?
				dataGridViewRecords.SelectedRows[dataGridViewRecords.SelectedRows.Count - 1].Index : 0;

			RecordsBindingList.Clear();

			Query querySelect = ДокументОбєкт.БанківськіРахунки_TablePart.QuerySelect;
			querySelect.Clear();

			//JOIN 1
			querySelect.FieldAndAlias.Add(
				new NameValue<string>(Довідники.БанківськіРахункиОрганізацій_Const.TABLE + "." + Довідники.БанківськіРахункиОрганізацій_Const.Назва, "bank_rachunok"));
			querySelect.Joins.Add(
				new Join(Довідники.БанківськіРахункиОрганізацій_Const.TABLE, Документи.ВведенняЗалишків_БанківськіРахунки_TablePart.БанківськийРахунок, querySelect.Table));

			//ORDER
			querySelect.Order.Add(Документи.ВведенняЗалишків_БанківськіРахунки_TablePart.НомерРядка, SelectOrder.ASC);

			ДокументОбєкт.БанківськіРахунки_TablePart.Read();

			Dictionary<string, Dictionary<string, string>> JoinValue = ДокументОбєкт.БанківськіРахунки_TablePart.JoinValue;

			foreach (Документи.ВведенняЗалишків_БанківськіРахунки_TablePart.Record record in ДокументОбєкт.БанківськіРахунки_TablePart.Records)
			{
				RecordsBindingList.Add(new Записи
				{
					ID = record.UID.ToString(),
					НомерРядка = record.НомерРядка,
					БанківськийРахунок = record.БанківськийРахунок,
					БанківськийРахунокНазва = JoinValue[record.UID.ToString()]["bank_rachunok"],
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

		public void SaveRecords()
		{
			ДокументОбєкт.БанківськіРахунки_TablePart.Records.Clear();

			int sequenceNumber = 0;

			foreach (Записи запис in RecordsBindingList)
			{
				Документи.ВведенняЗалишків_БанківськіРахунки_TablePart.Record record = new Документи.ВведенняЗалишків_БанківськіРахунки_TablePart.Record();

				record.UID = Guid.Parse(запис.ID);
				record.НомерРядка = ++sequenceNumber;
				record.БанківськийРахунок = запис.БанківськийРахунок;
				record.Сума = запис.Сума;

				ДокументОбєкт.БанківськіРахунки_TablePart.Records.Add(record);
			}

			ДокументОбєкт.БанківськіРахунки_TablePart.Save(true);
		}

		private class Записи
		{
			public string ID { get; set; }
			public int НомерРядка { get; set; }
			public Довідники.БанківськіРахункиОрганізацій_Pointer БанківськийРахунок { get; set; }
			public string БанківськийРахунокНазва { get; set; }
			public decimal Сума { get; set; }

			public static Записи New()
			{
				return new Записи
				{
					ID = Guid.Empty.ToString(),
					БанківськийРахунок = new Довідники.БанківськіРахункиОрганізацій_Pointer()
				};
			}
			public static Записи Clone(Записи запис)
			{
				return new Записи
				{
					ID = Guid.Empty.ToString(),
					БанківськийРахунок = запис.БанківськийРахунок,
					БанківськийРахунокНазва = запис.БанківськийРахунокНазва,
					Сума = запис.Сума
				};
			}

			public static void ПісляЗміни_БанківськийРахунок(Записи запис)
			{
				запис.БанківськийРахунокНазва = запис.БанківськийРахунок.GetPresentation();
			}
		}

		#region Вибір, Пошук, Зміна

		private void dataGridViewRecords_CellEndEdit(object sender, DataGridViewCellEventArgs e)
		{

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
					case "БанківськийРахунокНазва":
						{
							запис.БанківськийРахунок = new Довідники.БанківськіРахункиОрганізацій_Pointer();
							Записи.ПісляЗміни_БанківськийРахунок(запис);
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
					new string[] { "БанківськийРахунокНазва" }, SelectClick, FindTextChanged);
		}

		private void SelectClick(object sender, EventArgs e)
		{
			ToolStripMenuItem selectMenu = (ToolStripMenuItem)sender;
			Записи запис = (Записи)selectMenu.Tag;

			switch (selectMenu.Name)
			{
				case "БанківськийРахунокНазва":
					{
						Form_БанківськіРахункиОрганізацій form_БанківськіРахункиОрганізацій = new Form_БанківськіРахункиОрганізацій();
						form_БанківськіРахункиОрганізацій.DirectoryPointerItem = запис.БанківськийРахунок;
						form_БанківськіРахункиОрганізацій.ShowDialog();

						запис.БанківськийРахунок = (Довідники.БанківськіРахункиОрганізацій_Pointer)form_БанківськіРахункиОрганізацій.DirectoryPointerItem;
						Записи.ПісляЗміни_БанківськийРахунок(запис);

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
				case "БанківськийРахунокНазва":
					{
						query = ПошуковіЗапити.БанківськіРахункиОрганізацій;
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
				case "БанківськийРахунокНазва":
					{
						запис.БанківськийРахунок = new Довідники.БанківськіРахункиОрганізацій_Pointer(new UnigueID(uid));
						Записи.ПісляЗміни_БанківськийРахунок(запис);

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
			dataGridViewRecords.CurrentCell = dataGridViewRecords.Rows[dataGridViewRecords.Rows.Count - 1].Cells["БанківськийРахунокНазва"];
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
	}
}
