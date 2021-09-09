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
		/// Обєкт запису
		/// </summary>
        private Довідники.Номенклатура_Objest номенклатура_Objest { get; set; }

		public void CallBack_Виробник(DirectoryPointer directoryPointerItem)
		{
			Form_Виробники form_Виробники = new Form_Виробники();
			form_Виробники.DirectoryPointerItem = directoryPointerItem;
			form_Виробники.DirectoryControlItem = directoryControl_Виробник;
			form_Виробники.ShowDialog();
		}

		public void CallBack_ВидНоменклатури(DirectoryPointer directoryPointerItem)
		{
			Form_ВидиНоменклатури form_ВидиНоменклатури = new Form_ВидиНоменклатури();
			form_ВидиНоменклатури.DirectoryPointerItem = directoryPointerItem;
			form_ВидиНоменклатури.DirectoryControlItem = directoryControl_ВидНоменклатури;
			form_ВидиНоменклатури.ShowDialog();
		}

		public void CallBack_ОдиницяВиміру(DirectoryPointer directoryPointerItem)
		{
			Form_ПакуванняОдиниціВиміру form_ПакуванняОдиниціВиміру = new Form_ПакуванняОдиниціВиміру();
			form_ПакуванняОдиниціВиміру.DirectoryPointerItem = directoryPointerItem;
			form_ПакуванняОдиниціВиміру.DirectoryControlItem = directoryControl_ОдиницяВиміру;
			form_ПакуванняОдиниціВиміру.ShowDialog();
		}

		private void FormAddCash_Load(object sender, EventArgs e)
        {
			//Заповнення елементів перелічення - ТипНоменклатури
			foreach (ConfigurationEnumField field in Конфа.Config.Kernel.Conf.Enums["ТипиНоменклатури"].Fields.Values)
				comboBox_ТипНоменклатури.Items.Add((Перелічення.ТипиНоменклатури)field.Value);

			directoryControl_Виробник.CallBack = CallBack_Виробник;
			directoryControl_ВидНоменклатури.CallBack = CallBack_ВидНоменклатури;
			directoryControl_ОдиницяВиміру.CallBack = CallBack_ОдиницяВиміру;

			if (IsNew.HasValue)
			{
				номенклатура_Objest = new Довідники.Номенклатура_Objest();

				if (IsNew.Value)
				{
					this.Text += " - Новий запис";
					directoryControl_Виробник.DirectoryPointerItem = new Довідники.Виробники_Pointer();
					directoryControl_ВидНоменклатури.DirectoryPointerItem = new Довідники.ВидиНоменклатури_Pointer();
					directoryControl_ОдиницяВиміру.DirectoryPointerItem = new Довідники.ПакуванняОдиниціВиміру_Pointer();
					comboBox_ТипНоменклатури.SelectedIndex = 0;
				}
				else
				{
					if (номенклатура_Objest.Read(new UnigueID(Uid)))
					{
						this.Text += " - Редагування запису - " + номенклатура_Objest.Назва;

						textBox_Назва.Text = номенклатура_Objest.Назва;
						directoryControl_Виробник.DirectoryPointerItem = new Довідники.Виробники_Pointer(номенклатура_Objest.Виробник.UnigueID);
						directoryControl_ВидНоменклатури.DirectoryPointerItem = new Довідники.ВидиНоменклатури_Pointer(номенклатура_Objest.ВидНоменклатури.UnigueID);
						directoryControl_ОдиницяВиміру.DirectoryPointerItem = new Довідники.ПакуванняОдиниціВиміру_Pointer(номенклатура_Objest.ОдиницяВиміру.UnigueID);
						comboBox_ТипНоменклатури.SelectedItem = номенклатура_Objest.ТипНоменклатури;
						textBox_Артикул.Text = номенклатура_Objest.Артикул;
						textBox_НазваПовна.Text = номенклатура_Objest.НазваПовна;
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
					номенклатура_Objest.Виробник = (Довідники.Виробники_Pointer)directoryControl_Виробник.DirectoryPointerItem;
					номенклатура_Objest.ВидНоменклатури = (Довідники.ВидиНоменклатури_Pointer)directoryControl_ВидНоменклатури.DirectoryPointerItem;
					номенклатура_Objest.ОдиницяВиміру = (Довідники.ПакуванняОдиниціВиміру_Pointer)directoryControl_ОдиницяВиміру.DirectoryPointerItem;
					номенклатура_Objest.ТипНоменклатури = comboBox_ТипНоменклатури.SelectedItem != null ? (Перелічення.ТипиНоменклатури)comboBox_ТипНоменклатури.SelectedItem : 0;
					номенклатура_Objest.Артикул = textBox_Артикул.Text;
					номенклатура_Objest.НазваПовна = textBox_НазваПовна.Text;
					номенклатура_Objest.Опис = textBox_Опис.Text;
					номенклатура_Objest.Save();
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
