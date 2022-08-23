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

namespace StorageAndTrade
{
    public partial class Form_ХарактеристикиНоменклатуриЕлемент : Form
    {
        public Form_ХарактеристикиНоменклатуриЕлемент()
        {
            InitializeComponent();
        }

		/// <summary>
		/// Форма списку
		/// </summary>
        public Form_ХарактеристикиНоменклатури OwnerForm { get; set; }
        
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
        private Довідники.ХарактеристикиНоменклатури_Objest характеристикиНоменклатури_Objest { get; set; }

		/// <summary>
		/// Номенклатура власник
		/// </summary>
		public Довідники.Номенклатура_Pointer НоменклатураВласник { get; set; }

		private void Form_ХарактеристикиНоменклатуриЕлемент_Load(object sender, EventArgs e)
        {
			directoryControl_Номенклатура.Init(new Form_Номенклатура(), new Довідники.Номенклатура_Pointer(), ПошуковіЗапити.Номенклатура);

			if (IsNew.HasValue)
			{
				характеристикиНоменклатури_Objest = new Довідники.ХарактеристикиНоменклатури_Objest();

				if (IsNew.Value)
				{
					this.Text += " - Новий";
					textBox_Код.Text = характеристикиНоменклатури_Objest.Код = (++Константи.НумераціяДовідників.ХарактеристикиНоменклатури_Const).ToString("D6");

					if (НоменклатураВласник != null)
						directoryControl_Номенклатура.DirectoryPointerItem = НоменклатураВласник;
				}
				else
				{
					if (характеристикиНоменклатури_Objest.Read(new UnigueID(Uid)))
					{
						this.Text += " - Редагування";

						textBoxName.Text = характеристикиНоменклатури_Objest.Назва;
						textBox_Код.Text = характеристикиНоменклатури_Objest.Код;

						directoryControl_Номенклатура.DirectoryPointerItem = new Довідники.Номенклатура_Pointer(характеристикиНоменклатури_Objest.Номенклатура.UnigueID);
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
					характеристикиНоменклатури_Objest.New();

				try
				{
					характеристикиНоменклатури_Objest.Назва = textBoxName.Text;
					характеристикиНоменклатури_Objest.Код = textBox_Код.Text;
					характеристикиНоменклатури_Objest.Номенклатура = (Довідники.Номенклатура_Pointer)directoryControl_Номенклатура.DirectoryPointerItem;
					характеристикиНоменклатури_Objest.Save();
				}
				catch (Exception exp)
				{
					MessageBox.Show(exp.Message);
					return;
				}

				if (OwnerForm != null && !OwnerForm.IsDisposed)
				{
					OwnerForm.SelectPointerItem = характеристикиНоменклатури_Objest.GetDirectoryPointer();
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
