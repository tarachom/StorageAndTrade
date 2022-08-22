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
    public partial class Form_ВидиНоменклатуриЕлемент : Form
    {
        public Form_ВидиНоменклатуриЕлемент()
        {
            InitializeComponent();
        }

		/// <summary>
		/// Форма списку
		/// </summary>
        public Form_ВидиНоменклатури OwnerForm { get; set; }
        
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
        private Довідники.ВидиНоменклатури_Objest видиНоменклатури_Objest { get; set; }

		private void Form_ВидиНоменклатуриЕлемент_Load(object sender, EventArgs e)
        {
			//Заповнення елементів перелічення - ТипНоменклатури
			foreach (ConfigurationEnumField field in Конфа.Config.Kernel.Conf.Enums["ТипиНоменклатури"].Fields.Values)
				comboBox_ТипНоменклатури.Items.Add((Перелічення.ТипиНоменклатури)field.Value);

			directoryControl_ОдиницяВиміру.Init(new Form_ПакуванняОдиниціВиміру(), new Довідники.ПакуванняОдиниціВиміру_Pointer(), ПошуковіЗапити.ПакуванняОдиниціВиміру);

			if (IsNew.HasValue)
			{
				видиНоменклатури_Objest = new Довідники.ВидиНоменклатури_Objest();

				if (IsNew.Value)
				{
					this.Text += " - Новий";
					textBox_Код.Text = видиНоменклатури_Objest.Код = (++Константи.НумераціяДовідників.ВидиНоменклатури_Const).ToString("D6");
					comboBox_ТипНоменклатури.SelectedIndex = 0;
				}
				else
				{
					if (видиНоменклатури_Objest.Read(new UnigueID(Uid)))
					{
						this.Text += " - Редагування";

						textBoxName.Text = видиНоменклатури_Objest.Назва;
						textBox_Код.Text = видиНоменклатури_Objest.Код;
						directoryControl_ОдиницяВиміру.DirectoryPointerItem = new Довідники.ПакуванняОдиниціВиміру_Pointer(видиНоменклатури_Objest.ОдиницяВиміру.UnigueID);
						comboBox_ТипНоменклатури.SelectedItem = видиНоменклатури_Objest.ТипНоменклатури;
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
					видиНоменклатури_Objest.New();

				try
				{
					видиНоменклатури_Objest.Назва = textBoxName.Text;
					видиНоменклатури_Objest.Код = textBox_Код.Text;
					видиНоменклатури_Objest.ОдиницяВиміру = (Довідники.ПакуванняОдиниціВиміру_Pointer)directoryControl_ОдиницяВиміру.DirectoryPointerItem;
					видиНоменклатури_Objest.ТипНоменклатури = comboBox_ТипНоменклатури.SelectedItem != null ? (Перелічення.ТипиНоменклатури)comboBox_ТипНоменклатури.SelectedItem : 0;
					видиНоменклатури_Objest.Save();
				}
				catch (Exception exp)
				{
					MessageBox.Show(exp.Message);
					return;
				}

				if (OwnerForm != null && !OwnerForm.IsDisposed)
				{
					OwnerForm.SelectPointerItem = видиНоменклатури_Objest.GetDirectoryPointer();
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
