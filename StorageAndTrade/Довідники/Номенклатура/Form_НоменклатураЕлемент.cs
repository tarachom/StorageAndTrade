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
    public partial class Form_НоменклатураЕлемент : Form
    {
        public Form_НоменклатураЕлемент()
        {
            InitializeComponent();
        }

		/// <summary>
		/// Форма списку
		/// </summary>
        public Form_Номенклатура OwnerForm { get; set; }
        
		/// <summary>
		/// Чи це новий
		/// </summary>
        public Nullable<bool> IsNew { get; set; }

		/// <summary>
		/// Ід запису
		/// </summary>
        public string Uid { get; set; }

		/// <summary>
		/// Ід родителя для нової папки
		/// </summary>
		public string ParentUid { get; set; }

		/// <summary>
		/// Обєкт запису
		/// </summary>
		private Довідники.Номенклатура_Objest номенклатура_Objest { get; set; }

		private void Form_НоменклатураЕлемент_Load(object sender, EventArgs e)
        {	
			//Заповнення елементів перелічення - ТипНоменклатури
			foreach (ConfigurationEnumField field in Конфа.Config.Kernel.Conf.Enums["ТипиНоменклатури"].Fields.Values)
				comboBox_ТипНоменклатури.Items.Add((Перелічення.ТипиНоменклатури)field.Value);

			directoryControl_НоменклатураПапка.Init(new Form_НоменклатураПапкиВибір(), new Довідники.Номенклатура_Папки_Pointer(new UnigueID(ParentUid)), ПошуковіЗапити.Номенклатура_Папки);
			directoryControl_Виробник.Init(new Form_Виробники(), new Довідники.Виробники_Pointer(), ПошуковіЗапити.Виробники);
			directoryControl_ВидНоменклатури.Init(new Form_ВидиНоменклатури(), new Довідники.ВидиНоменклатури_Pointer(), ПошуковіЗапити.ВидиНоменклатури);
			directoryControl_ОдиницяВиміру.Init(new Form_ПакуванняОдиниціВиміру(), new Довідники.ПакуванняОдиниціВиміру_Pointer(), ПошуковіЗапити.ПакуванняОдиниціВиміру);

			if (IsNew.HasValue)
			{
				номенклатура_Objest = new Довідники.Номенклатура_Objest();

				if (IsNew.Value)
				{
					this.Text += " - Новий";
					textBox_Код.Text = номенклатура_Objest.Код = (++Константи.НумераціяДовідників.Номенклатура_Const).ToString("D6");
					comboBox_ТипНоменклатури.SelectedItem = Перелічення.ТипиНоменклатури.Товар;
				}
				else
				{
					if (номенклатура_Objest.Read(new UnigueID(Uid)))
					{
						this.Text += " - Редагування";

						textBox_Назва.Text = номенклатура_Objest.Назва;
						textBox_НазваПовна.Text = номенклатура_Objest.НазваПовна;
						textBox_Артикул.Text = номенклатура_Objest.Артикул;
						textBox_Код.Text = номенклатура_Objest.Код;
						directoryControl_НоменклатураПапка.DirectoryPointerItem = new Довідники.Номенклатура_Папки_Pointer(номенклатура_Objest.Папка.UnigueID);
						directoryControl_Виробник.DirectoryPointerItem = new Довідники.Виробники_Pointer(номенклатура_Objest.Виробник.UnigueID);
						directoryControl_ВидНоменклатури.DirectoryPointerItem = new Довідники.ВидиНоменклатури_Pointer(номенклатура_Objest.ВидНоменклатури.UnigueID);
						directoryControl_ОдиницяВиміру.DirectoryPointerItem = new Довідники.ПакуванняОдиниціВиміру_Pointer(номенклатура_Objest.ОдиницяВиміру.UnigueID);
						comboBox_ТипНоменклатури.SelectedItem = номенклатура_Objest.ТипНоменклатури;
						textBox_Опис.Text = номенклатура_Objest.Опис;
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
					номенклатура_Objest.New();

				try
				{
					номенклатура_Objest.Назва = textBox_Назва.Text;
					номенклатура_Objest.НазваПовна = textBox_НазваПовна.Text;
					номенклатура_Objest.Артикул = textBox_Артикул.Text;
					номенклатура_Objest.Код = textBox_Код.Text;
					номенклатура_Objest.Папка = (Довідники.Номенклатура_Папки_Pointer)directoryControl_НоменклатураПапка.DirectoryPointerItem;
					номенклатура_Objest.Виробник = (Довідники.Виробники_Pointer)directoryControl_Виробник.DirectoryPointerItem;
					номенклатура_Objest.ВидНоменклатури = (Довідники.ВидиНоменклатури_Pointer)directoryControl_ВидНоменклатури.DirectoryPointerItem;
					номенклатура_Objest.ОдиницяВиміру = (Довідники.ПакуванняОдиниціВиміру_Pointer)directoryControl_ОдиницяВиміру.DirectoryPointerItem;
					номенклатура_Objest.ТипНоменклатури = comboBox_ТипНоменклатури.SelectedItem != null ? (Перелічення.ТипиНоменклатури)comboBox_ТипНоменклатури.SelectedItem : 0;
					номенклатура_Objest.Опис = textBox_Опис.Text;
					номенклатура_Objest.Save();
				}
				catch (Exception exp)
				{
					MessageBox.Show(exp.Message);
					return;
				}

				if (OwnerForm != null && !OwnerForm.IsDisposed)
				{
					OwnerForm.SelectPointerItem = номенклатура_Objest.GetDirectoryPointer();
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
