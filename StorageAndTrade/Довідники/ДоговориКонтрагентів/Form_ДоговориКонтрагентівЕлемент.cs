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
using Перелічення = StorageAndTrade_1_0.Перелічення;

namespace StorageAndTrade
{
    public partial class Form_ДоговориКонтрагентівЕлемент : Form
    {
        public Form_ДоговориКонтрагентівЕлемент()
        {
            InitializeComponent();
        }

		/// <summary>
		/// Форма списку
		/// </summary>
        public Form_ДоговориКонтрагентів OwnerForm { get; set; }
        
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
        private Довідники.ДоговориКонтрагентів_Objest договориКонтрагентів_Objest { get; set; }

		/// <summary>
		/// Контрагент власник договору
		/// </summary>
		public Довідники.Контрагенти_Pointer КонтрагентВласник { get; set; }

		private void Form_ДоговориКонтрагентівЕлемент_Load(object sender, EventArgs e)
        {
			//Статус
			foreach (ConfigurationEnumField field in Конфа.Config.Kernel.Conf.Enums["СтатусиДоговорівКонтрагентів"].Fields.Values)
				comboBox_Статус.Items.Add(
					new NameValue<Перелічення.СтатусиДоговорівКонтрагентів>(field.Desc, (Перелічення.СтатусиДоговорівКонтрагентів)field.Value));

			//ГосподарськіОперації
			foreach (ConfigurationEnumField field in Конфа.Config.Kernel.Conf.Enums["ГосподарськіОперації"].Fields.Values)
				comboBox_ГосподарськаОперація.Items.Add(
					new NameValue<Перелічення.ГосподарськіОперації>(field.Desc, (Перелічення.ГосподарськіОперації)field.Value));

			//ТипДоговорів
			foreach (ConfigurationEnumField field in Конфа.Config.Kernel.Conf.Enums["ТипДоговорів"].Fields.Values)
				comboBox_ТипДоговору.Items.Add(
					new NameValue<Перелічення.ТипДоговорів>(field.Desc, (Перелічення.ТипДоговорів)field.Value));

			directoryControl_БанківськийРахунок.Init(new Form_БанківськіРахункиОрганізацій(), new Довідники.БанківськіРахункиОрганізацій_Pointer(), ПошуковіЗапити.БанківськіРахункиОрганізацій);
			directoryControl_БанківськийРахунокКонтрагента.Init(new Form_БанківськіРахункиКонтрагентів(), new Довідники.БанківськіРахункиКонтрагентів_Pointer());
			directoryControl_Підрозділ.Init(new Form_СтруктураПідприємства(), new Довідники.СтруктураПідприємства_Pointer());
			directoryControl_Контрагент.Init(new Form_Контрагенти(), new Довідники.Контрагенти_Pointer(), ПошуковіЗапити.Контрагенти);

			if (IsNew.HasValue)
			{
				договориКонтрагентів_Objest = new Довідники.ДоговориКонтрагентів_Objest();

				if (IsNew.Value)
				{
					this.Text += " - Новий";
					textBoxName.Text = "Основний договір";
					textBox_Код.Text = договориКонтрагентів_Objest.Код = (++Константи.НумераціяДовідників.ДоговориКонтрагентів_Const).ToString("D6");
					comboBox_Статус.SelectedIndex = 1;
					comboBox_ГосподарськаОперація.SelectedIndex = 0;
					comboBox_ТипДоговору.SelectedIndex = 0;

					if (КонтрагентВласник != null)
						directoryControl_Контрагент.DirectoryPointerItem = КонтрагентВласник;
				}
				else
				{
					if (договориКонтрагентів_Objest.Read(new UnigueID(Uid)))
					{
						this.Text += " - Редагування";

						textBoxName.Text = договориКонтрагентів_Objest.Назва;
						textBox_Код.Text = договориКонтрагентів_Objest.Код;
						directoryControl_БанківськийРахунок.DirectoryPointerItem = new Довідники.БанківськіРахункиОрганізацій_Pointer(договориКонтрагентів_Objest.БанківськийРахунок.UnigueID);
						directoryControl_БанківськийРахунокКонтрагента.DirectoryPointerItem = new Довідники.БанківськіРахункиКонтрагентів_Pointer(договориКонтрагентів_Objest.БанківськийРахунокКонтрагента.UnigueID);
						directoryControl_Підрозділ.DirectoryPointerItem = new Довідники.СтруктураПідприємства_Pointer(договориКонтрагентів_Objest.Підрозділ.UnigueID);
						directoryControl_Контрагент.DirectoryPointerItem = new Довідники.Контрагенти_Pointer(договориКонтрагентів_Objest.Контрагент.UnigueID);

						ComboBoxNameValue<Перелічення.СтатусиДоговорівКонтрагентів>.SelectItem(comboBox_Статус, договориКонтрагентів_Objest.Статус);
						ComboBoxNameValue<Перелічення.ГосподарськіОперації>.SelectItem(comboBox_ГосподарськаОперація, договориКонтрагентів_Objest.ГосподарськаОперація);
						ComboBoxNameValue<Перелічення.ТипДоговорів>.SelectItem(comboBox_ТипДоговору, договориКонтрагентів_Objest.ТипДоговору);
					}
					else
						MessageBox.Show("Error read");
				}
			}
		}

        private void buttonSave_Click(object sender, EventArgs e)
        {
			if (directoryControl_Контрагент.DirectoryPointerItem == null ||
				((Довідники.Контрагенти_Pointer)directoryControl_Контрагент.DirectoryPointerItem).IsEmpty())
            {
				MessageBox.Show("Потрібно вказати контрагента");
				return;
			}

			if (IsNew.HasValue)
			{
				if (IsNew.Value)
					договориКонтрагентів_Objest.New();

				try
				{
					договориКонтрагентів_Objest.Назва = textBoxName.Text;
					договориКонтрагентів_Objest.Код = textBox_Код.Text;
					договориКонтрагентів_Objest.БанківськийРахунок = (Довідники.БанківськіРахункиОрганізацій_Pointer)directoryControl_БанківськийРахунок.DirectoryPointerItem;
					договориКонтрагентів_Objest.БанківськийРахунокКонтрагента = (Довідники.БанківськіРахункиКонтрагентів_Pointer)directoryControl_БанківськийРахунокКонтрагента.DirectoryPointerItem;
					договориКонтрагентів_Objest.Підрозділ = (Довідники.СтруктураПідприємства_Pointer)directoryControl_Підрозділ.DirectoryPointerItem;
					договориКонтрагентів_Objest.Контрагент = (Довідники.Контрагенти_Pointer)directoryControl_Контрагент.DirectoryPointerItem;
					договориКонтрагентів_Objest.Статус = ((NameValue<Перелічення.СтатусиДоговорівКонтрагентів>)comboBox_Статус.SelectedItem).Value;
					договориКонтрагентів_Objest.ГосподарськаОперація = ((NameValue<Перелічення.ГосподарськіОперації>)comboBox_ГосподарськаОперація.SelectedItem).Value;
					договориКонтрагентів_Objest.ТипДоговору = ((NameValue<Перелічення.ТипДоговорів>)comboBox_ТипДоговору.SelectedItem).Value;
					договориКонтрагентів_Objest.Save();
				}
				catch (Exception exp)
				{
					MessageBox.Show(exp.Message);
					return;
				}

				if (OwnerForm != null && !OwnerForm.IsDisposed)
				{
					OwnerForm.SelectPointerItem = договориКонтрагентів_Objest.GetDirectoryPointer();
					OwnerForm.LoadRecords();
				}

				this.Close();
			}
		}

		private void buttonClose_Click(object sender, EventArgs e)
		{
			this.Close();
		}

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void directoryControl_БанківськийРахунокКонтрагента_Load(object sender, EventArgs e)
        {

        }
    }
}
