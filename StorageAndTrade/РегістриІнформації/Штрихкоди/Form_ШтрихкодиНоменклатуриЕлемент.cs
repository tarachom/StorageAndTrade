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
using Перелічення = StorageAndTrade_1_0.Перелічення;
using РегістриВідомостей = StorageAndTrade_1_0.РегістриВідомостей;

namespace StorageAndTrade
{
    public partial class Form_ШтрихкодиНоменклатуриЕлемент : Form
    {
        public Form_ШтрихкодиНоменклатуриЕлемент()
        {
            InitializeComponent();
        }

		/// <summary>
		/// Форма списку
		/// </summary>
        public Form_ШтрихкодиНоменклатури OwnerForm { get; set; }
        
		/// <summary>
		/// Чи це новий
		/// </summary>
        public Nullable<bool> IsNew { get; set; }

		/// <summary>
		/// Ід запису
		/// </summary>
        public string Uid { get; set; }

		/// <summary>
		/// Номенклатура власник
		/// </summary>
		public Довідники.Номенклатура_Pointer НоменклатураВласник { get; set; }

		/// <summary>
		/// Обєкт запису
		/// </summary>
		private РегістриВідомостей.ШтрихкодиНоменклатури_Objest штрихкодиНоменклатури_Objest { get; set; }

		private void Form_ШтрихкодиНоменклатуриЕлемент_Load(object sender, EventArgs e)
        {
			штрихкодиНоменклатури_Objest = new РегістриВідомостей.ШтрихкодиНоменклатури_Objest();

			directoryControl_Номенклатура.Init(new Form_Номенклатура(), new Довідники.Номенклатура_Pointer(), ПошуковіЗапити.Номенклатура);
			directoryControl_Номенклатура.AfterSelectFunc = () =>
			{
				if (directoryControl_Номенклатура.DirectoryPointerItem != null && !directoryControl_Номенклатура.DirectoryPointerItem.IsEmpty())
				{
					Довідники.Номенклатура_Objest номенклатура_Objest = new Довідники.Номенклатура_Objest();
					if (номенклатура_Objest.Read(directoryControl_Номенклатура.DirectoryPointerItem.UnigueID))
					{
						directoryControl_Пакування.DirectoryPointerItem = номенклатура_Objest.ОдиницяВиміру;
					}
				}

				return true;
			};
			directoryControl_ХарактеристикаНоменклатури.Init(new Form_ХарактеристикиНоменклатури(), new Довідники.ХарактеристикиНоменклатури_Pointer(), ПошуковіЗапити.ХарактеристикаНоменклатуриЗВідбором());
			directoryControl_ХарактеристикаНоменклатури.BeforeClickOpenFunc = () =>
			{
				((Form_ХарактеристикиНоменклатури)directoryControl_ХарактеристикаНоменклатури.SelectForm).НоменклатураВласник = (Довідники.Номенклатура_Pointer)directoryControl_Номенклатура.DirectoryPointerItem;
				return true;
			};
			directoryControl_ХарактеристикаНоменклатури.BeforeFindFunc = () =>
			{
				directoryControl_ХарактеристикаНоменклатури.QueryFind =
				   ПошуковіЗапити.ХарактеристикаНоменклатуриЗВідбором((Довідники.Номенклатура_Pointer)directoryControl_Номенклатура.DirectoryPointerItem);
			};
			directoryControl_Пакування.Init(new Form_ПакуванняОдиниціВиміру(), new Довідники.ПакуванняОдиниціВиміру_Pointer(), ПошуковіЗапити.ПакуванняОдиниціВиміру);

			if (IsNew.HasValue)
			{
				if (IsNew.Value)
				{
					this.Text += " - Новий";
					directoryControl_Номенклатура.DirectoryPointerItem = НоменклатураВласник;

					if (directoryControl_Номенклатура.AfterSelectFunc != null)
						directoryControl_Номенклатура.AfterSelectFunc.Invoke();
				}
				else
				{
					if (штрихкодиНоменклатури_Objest.Read(new UnigueID(Uid)))
                    {
						this.Text += " - Редагування";

						textBox_Штрихкод.Text = штрихкодиНоменклатури_Objest.Штрихкод;
						directoryControl_Номенклатура.DirectoryPointerItem = штрихкодиНоменклатури_Objest.Номенклатура;
						directoryControl_ХарактеристикаНоменклатури.DirectoryPointerItem = штрихкодиНоменклатури_Objest.ХарактеристикаНоменклатури;
						directoryControl_Пакування.DirectoryPointerItem = штрихкодиНоменклатури_Objest.Пакування;
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
					штрихкодиНоменклатури_Objest.New();

				штрихкодиНоменклатури_Objest.Штрихкод = textBox_Штрихкод.Text;
				штрихкодиНоменклатури_Objest.Номенклатура = (Довідники.Номенклатура_Pointer)directoryControl_Номенклатура.DirectoryPointerItem;
				штрихкодиНоменклатури_Objest.ХарактеристикаНоменклатури = (Довідники.ХарактеристикиНоменклатури_Pointer)directoryControl_ХарактеристикаНоменклатури.DirectoryPointerItem;
				штрихкодиНоменклатури_Objest.Пакування = (Довідники.ПакуванняОдиниціВиміру_Pointer)directoryControl_Пакування.DirectoryPointerItem;

				try
				{
					штрихкодиНоменклатури_Objest.Save();
				}
				catch (Exception exp)
				{
					MessageBox.Show(exp.Message);
					return;
				}

				if (OwnerForm != null && !OwnerForm.IsDisposed)
				{
					OwnerForm.SelectPointerItem = штрихкодиНоменклатури_Objest.UnigueID.ToString();
					OwnerForm.LoadRecords();
				}

				this.Close();
			}
		}

		private void buttonClose_Click(object sender, EventArgs e)
		{
			this.Close();
		}
	}
}
