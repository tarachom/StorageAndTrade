﻿using System;
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
    public partial class ЗамовленняКлієнта_ТабличнаЧастина_Товари : UserControl
    {
        public ЗамовленняКлієнта_ТабличнаЧастина_Товари()
        {
            InitializeComponent();
        }

		public Документи.ЗамовленняКлієнта_Objest ЗамовленняКлієнта_Objest { get; set; }

        private void ЗамовленняКлієнта_ТабличнаЧастина_Товари_Load(object sender, EventArgs e)
        {
			//dataGridViewRecords.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

			RecordsBindingList = new BindingList<Записи>();
			dataGridViewRecords.DataSource = RecordsBindingList;

			dataGridViewRecords.Columns["ID"].Visible = false;
			dataGridViewRecords.Columns["Номенклатура"].Width = 300;
			dataGridViewRecords.Columns["Пакування"].Width = 300;
		}

		private BindingList<Записи> RecordsBindingList { get; set; }

		public void LoadRecords()
		{
			int selectRow = dataGridViewRecords.SelectedRows.Count > 0 ?
				dataGridViewRecords.SelectedRows[dataGridViewRecords.SelectedRows.Count - 1].Index : 0;

			RecordsBindingList.Clear();

			ЗамовленняКлієнта_Objest.Товари_TablePart.Read();
			foreach (Документи.ЗамовленняКлієнта_Товари_TablePart.Record record in ЗамовленняКлієнта_Objest.Товари_TablePart.Records)
			{
				RecordsBindingList.Add(new Записи
				{
					ID = record.UID.ToString(),
					Номенклатура = record.Номенклатура.UnigueID.ToString(),
					Пакування = record.Пакування.UnigueID.ToString(),
					Кількість = (uint)record.Кількість
				});
			}

			if (selectRow != 0 && selectRow < dataGridViewRecords.Rows.Count)
			{
				dataGridViewRecords.Rows[0].Selected = false;
				dataGridViewRecords.Rows[selectRow].Selected = true;
			}
		}

		private class Записи
        {
            public string ID { get; set; }
            public string Номенклатура { get; set; }
			public string Пакування { get; set; }
			public uint Кількість { get; set; }
        }

        private void dataGridViewRecords_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
			string columnName = dataGridViewRecords.Columns[e.ColumnIndex].Name;

			if (columnName == "Номенклатура")
			{
				string Uid = "";
				object a = dataGridViewRecords.Rows[e.RowIndex].Cells["Номенклатура"].Value;
				if (a != null) Uid = a.ToString();


				 DirectoryControl directoryControl = new DirectoryControl();

				Form_Номенклатура form_Номенклатура = new Form_Номенклатура();
				form_Номенклатура.DirectoryPointerItem = new Довідники.Номенклатура_Pointer(new UnigueID(Uid));
				form_Номенклатура.DirectoryControlItem = directoryControl;
				form_Номенклатура.ShowDialog();

				if (directoryControl.DirectoryPointerItem != null)
				{
					if (dataGridViewRecords.Rows[e.RowIndex].IsNewRow)
						dataGridViewRecords.Rows.Add();

					dataGridViewRecords.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = directoryControl.DirectoryPointerItem.ToString();
					
				}


			}

			if (columnName == "Пакування")
			{
				string Uid = "";

				if (e.RowIndex < dataGridViewRecords.RowCount)
					Uid = dataGridViewRecords.Rows[e.RowIndex].Cells["Пакування"].Value.ToString();

				DirectoryControl directoryControl = new DirectoryControl();

				Form_ПакуванняОдиниціВиміру form_ПакуванняОдиниціВиміру = new Form_ПакуванняОдиниціВиміру();
				form_ПакуванняОдиниціВиміру.DirectoryPointerItem = new Довідники.Номенклатура_Pointer(new UnigueID(Uid));
				form_ПакуванняОдиниціВиміру.DirectoryControlItem = directoryControl;
				form_ПакуванняОдиниціВиміру.ShowDialog();

				if (directoryControl.DirectoryPointerItem != null)
				{
					dataGridViewRecords.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = directoryControl.DirectoryPointerItem.ToString();
				}


			}
		}

        private void dataGridViewRecords_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
			
		}
    }
}
