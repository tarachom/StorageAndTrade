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
    public partial class Form_ЗамовленняКлієнтаДокумент : Form
    {
        public Form_ЗамовленняКлієнтаДокумент()
        {
            InitializeComponent();
        }

		/// <summary>
		/// Форма списку
		/// </summary>
        public Form_ЗамовленняКлієнтаЖурнал OwnerForm { get; set; }
        
		/// <summary>
		/// Чи це новий
		/// </summary>
        public Nullable<bool> IsNew { get; set; }

		/// <summary>
		/// Ід запису
		/// </summary>
        public string Uid { get; set; }

		/// <summary>
		/// Обєкт запису
		/// </summary>
        private Документи.ЗамовленняКлієнта_Objest замовленняКлієнта_Objest { get; set; }

		private void FormAddCash_Load(object sender, EventArgs e)
        {
			if (IsNew.HasValue)
			{
				замовленняКлієнта_Objest = new Документи.ЗамовленняКлієнта_Objest();

				if (IsNew.Value)
				{
					this.Text += " - Новий запис";
				}
				else
				{
					if (замовленняКлієнта_Objest.Read(new UnigueID(Uid)))
					{
						this.Text += " - Редагування запису - " + замовленняКлієнта_Objest.НомерДок;

						textBox_НомерДок.Text = замовленняКлієнта_Objest.НомерДок;
						dateTimePicker_ДатаДок.Value = замовленняКлієнта_Objest.ДатаДок;

						//Довідники.Номенклатура_Select номенклатура_Select = new Довідники.Номенклатура_Select();
						//Довідники.Номенклатура_Pointer номенклатура_Pointer = номенклатура_Select.FindByField(Довідники.Номенклатура_Select.Назва, "Товар");

						//Довідники.ПакуванняОдиниціВиміру_Select пакуванняОдиниціВиміру_Select = new Довідники.ПакуванняОдиниціВиміру_Select();
						//Довідники.ПакуванняОдиниціВиміру_Pointer пакуванняОдиниціВиміру_Pointer = пакуванняОдиниціВиміру_Select.FindByField(Довідники.ПакуванняОдиниціВиміру_Select.Назва, "шт.");

						//замовленняКлієнта_Objest.Товари_TablePart.Records.Add(new Документи.ЗамовленняКлієнта_Товари_TablePart.Record(номенклатура_Pointer, null, пакуванняОдиниціВиміру_Pointer));
						//замовленняКлієнта_Objest.Товари_TablePart.Save(false);

						замовленняКлієнта_ТабличнаЧастина_Товари.ЗамовленняКлієнта_Objest = замовленняКлієнта_Objest;
						замовленняКлієнта_ТабличнаЧастина_Товари.LoadRecords();
					}
					else
						MessageBox.Show("Error read");
				}
			}
		}

        private void buttonSave_Click(object sender, EventArgs e)
        {
			if (IsNew.HasValue)
			{
				if (IsNew.Value)
					замовленняКлієнта_Objest.New();

				try
				{
					замовленняКлієнта_Objest.НомерДок = textBox_НомерДок.Text;
					замовленняКлієнта_Objest.ДатаДок = dateTimePicker_ДатаДок.Value;
					замовленняКлієнта_Objest.Save();


				}
				catch (Exception exp)
				{
					MessageBox.Show(exp.Message);
					return;
				}

				if (OwnerForm != null)
					OwnerForm.LoadRecords();

				this.Close();
			}
		}

		private void buttonClose_Click(object sender, EventArgs e)
		{
			this.Close();
		}
	}
}
