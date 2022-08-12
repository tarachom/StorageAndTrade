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

namespace StorageAndTrade
{
    public partial class Form_Організація_ТабличнаЧастина_Контакти : UserControl
    {
        public Form_Організація_ТабличнаЧастина_Контакти()
        {
            InitializeComponent();

			DataGridViewComboBoxColumn comboBoxColumn = new DataGridViewComboBoxColumn() 
			{ Name = "comboBoxColumn", DataSource = Enum.GetNames(typeof(Перелічення.ТипиКонтактноїІнформації)) };
			dataGridViewRecords.Columns.Add(comboBoxColumn);
			dataGridViewRecords.Columns["comboBoxColumn"].DisplayIndex = 0;
			dataGridViewRecords.Columns["comboBoxColumn"].HeaderText = "Тип";

			RecordsBindingList = new BindingList<Записи>();
			dataGridViewRecords.DataSource = RecordsBindingList;

			dataGridViewRecords.Columns["ID"].Visible = false;
			dataGridViewRecords.Columns["Тип"].Visible = false;

			dataGridViewRecords.Columns["Телефон"].Width = 150;

			dataGridViewRecords.Columns["ЕлектроннаПошта"].Width = 150;
			dataGridViewRecords.Columns["ЕлектроннаПошта"].HeaderText = "@ Email";
		}

		/// <summary>
		/// Власне довідник якому належить таблична частина
		/// </summary>
		public Довідники.Організації_Objest ДовідникОбєкт { get; set; }

		private BindingList<Записи> RecordsBindingList { get; set; }

		private void Form_Організація_ТабличнаЧастина_Контакти_Load(object sender, EventArgs e) { }

		public void LoadRecords()
		{
			RecordsBindingList.Clear();

			Довідники.Організації_Контакти_TablePart організації_Контакти_TablePart =
				new Довідники.Організації_Контакти_TablePart(ДовідникОбєкт);

			організації_Контакти_TablePart.Read();

            foreach (Довідники.Організації_Контакти_TablePart.Record record in організації_Контакти_TablePart.Records)
            {
				RecordsBindingList.Add(new Записи
				{
					ID = record.UID.ToString(),
					Тип = record.Тип,
					Країна = record.Країна,
					Район = record.Район,
					Місто = record.Місто,
					Телефон = record.Телефон,
					ЕлектроннаПошта = record.ЕлектроннаПошта,
					Область = record.Область
				});

				dataGridViewRecords["comboBoxColumn", dataGridViewRecords.RowCount - 1].Value = record.Тип.ToString();
			}
		}

		public void SaveRecords()
        {
			Довідники.Організації_Контакти_TablePart організації_Контакти_TablePart =
				new Довідники.Організації_Контакти_TablePart(ДовідникОбєкт);

			організації_Контакти_TablePart.Records.Clear();

			int counter = 0;

			foreach (Записи запис in RecordsBindingList)
            {
				string comboBoxColumnName = (string)dataGridViewRecords["comboBoxColumn", counter].Value;

				Перелічення.ТипиКонтактноїІнформації ТипКІ  = 
					(Перелічення.ТипиКонтактноїІнформації)Enum.Parse(typeof(Перелічення.ТипиКонтактноїІнформації), comboBoxColumnName);

				організації_Контакти_TablePart.Records.Add(
					new Довідники.Організації_Контакти_TablePart.Record()
					{
						Тип = ТипКІ,
						Телефон = запис.Телефон,
						ЕлектроннаПошта = запис.ЕлектроннаПошта,
						Країна = запис.Країна,
						Область = запис.Область,
						Район = запис.Район,
						Місто = запис.Місто
					}
			    );

				counter++;
			}

			організації_Контакти_TablePart.Save(true);
		}

		private class Записи
		{
			public string ID { get; set; }
			public Перелічення.ТипиКонтактноїІнформації Тип { get; set; }
			public string Телефон { get; set; }
			public string ЕлектроннаПошта { get; set; }
			public string Країна { get; set; }
			public string Область { get; set; }
			public string Місто { get; set; }
			public string Район { get; set; }

			public static Записи New()
			{
				return new Записи
				{
					ID = Guid.Empty.ToString(),
					Тип = Перелічення.ТипиКонтактноїІнформації.Адрес,
					Телефон = "",
					ЕлектроннаПошта = "",
					Країна = "",
					Область = "",
					Місто = "",
					Район = ""
				};
			}

			public static Записи Clone(Записи запис)
			{
				return new Записи
				{
					ID = Guid.Empty.ToString(),
					Тип = запис.Тип,
					Телефон = запис.Телефон,
					ЕлектроннаПошта = запис.ЕлектроннаПошта,
					Країна = запис.Країна,
					Область = запис.Область,
					Місто = запис.Місто,
					Район = запис.Район
				};
			}
		}

        private void toolStripButtonAdd_Click(object sender, EventArgs e)
        {
			Записи НовийЗапис = Записи.New();
			RecordsBindingList.Add(НовийЗапис);

			dataGridViewRecords["comboBoxColumn", dataGridViewRecords.RowCount - 1].Value = НовийЗапис.Тип.ToString();
		}

        private void toolStripButtonCopy_Click(object sender, EventArgs e)
        {
			if (dataGridViewRecords.SelectedCells.Count > 0)
			{
				List<int> rowIndexList = new List<int>();

				for (int i = 0; i < dataGridViewRecords.SelectedCells.Count; i++)
					if (!rowIndexList.Contains(dataGridViewRecords.SelectedCells[i].RowIndex) &&
						!dataGridViewRecords.Rows[dataGridViewRecords.SelectedCells[i].RowIndex].IsNewRow)
						rowIndexList.Add(dataGridViewRecords.SelectedCells[i].RowIndex);

				foreach (int rowIndex in rowIndexList)
				{
					Записи запис = Записи.Clone(RecordsBindingList[rowIndex]);
					RecordsBindingList.Add(запис);

					dataGridViewRecords["comboBoxColumn", dataGridViewRecords.RowCount - 1].Value = запис.Тип.ToString();
				}
			}
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

        
    }
}
