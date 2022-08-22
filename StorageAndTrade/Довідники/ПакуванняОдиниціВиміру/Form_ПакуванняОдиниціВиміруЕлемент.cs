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
    public partial class Form_ПакуванняОдиниціВиміруЕлемент : Form
    {
        public Form_ПакуванняОдиниціВиміруЕлемент()
        {
            InitializeComponent();
        }

		/// <summary>
		/// Форма списку
		/// </summary>
        public Form_ПакуванняОдиниціВиміру OwnerForm { get; set; }
        
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
        private Довідники.ПакуванняОдиниціВиміру_Objest пакуванняОдиниціВиміру_Objest { get; set; }

		private void Form_ПакуванняОдиниціВиміруЕлемент_Load(object sender, EventArgs e)
        {
			if (IsNew.HasValue)
			{
				пакуванняОдиниціВиміру_Objest = new Довідники.ПакуванняОдиниціВиміру_Objest();

				if (IsNew.Value)
				{
					this.Text += " - Новий";
					textBox_Код.Text = пакуванняОдиниціВиміру_Objest.Код = (++Константи.НумераціяДовідників.ПакуванняОдиниціВиміру_Const).ToString("D6");
					textBox_КількістьУпаковок.Text = "1";
				}
				else
				{
					if (пакуванняОдиниціВиміру_Objest.Read(new UnigueID(Uid)))
					{
						this.Text += " - Редагування";

						textBox_Назва.Text = пакуванняОдиниціВиміру_Objest.Назва;
						textBox_Код.Text = пакуванняОдиниціВиміру_Objest.Код;
						textBox_НазваПовна.Text = пакуванняОдиниціВиміру_Objest.НазваПовна;
						textBox_КількістьУпаковок.Text = пакуванняОдиниціВиміру_Objest.КількістьУпаковок.ToString();
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
					пакуванняОдиниціВиміру_Objest.New();

				try
				{
					пакуванняОдиниціВиміру_Objest.Назва = textBox_Назва.Text;
					пакуванняОдиниціВиміру_Objest.Код = textBox_Код.Text;
					пакуванняОдиниціВиміру_Objest.НазваПовна = textBox_НазваПовна.Text;
					пакуванняОдиниціВиміру_Objest.КількістьУпаковок = int.Parse(textBox_КількістьУпаковок.Text);
					пакуванняОдиниціВиміру_Objest.Save();
				}
				catch (Exception exp)
				{
					MessageBox.Show(exp.Message);
					return;
				}

				if (OwnerForm != null && !OwnerForm.IsDisposed)
				{
					OwnerForm.SelectPointerItem = пакуванняОдиниціВиміру_Objest.GetDirectoryPointer();
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
