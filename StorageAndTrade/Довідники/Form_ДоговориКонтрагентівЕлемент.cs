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

		public void CallBack_БанківськийРахунок(DirectoryPointer directoryPointerItem)
		{
			Form_БанківськіРахункиОрганізацій form_БанківськіРахункиОрганізацій = new Form_БанківськіРахункиОрганізацій();
			form_БанківськіРахункиОрганізацій.DirectoryPointerItem = directoryPointerItem;
			form_БанківськіРахункиОрганізацій.DirectoryControlItem = directoryControl_БанківськийРахунок;
			form_БанківськіРахункиОрганізацій.ShowDialog();
		}

		public void CallBack_БанківськийРахунокКонтрагента(DirectoryPointer directoryPointerItem)
		{
			Form_БанківськіРахункиКонтрагентів form_БанківськіРахункиКонтрагентів = new Form_БанківськіРахункиКонтрагентів();
			form_БанківськіРахункиКонтрагентів.DirectoryPointerItem = directoryPointerItem;
			form_БанківськіРахункиКонтрагентів.DirectoryControlItem = directoryControl_БанківськийРахунокКонтрагента;
			form_БанківськіРахункиКонтрагентів.ShowDialog();
		}

		public void CallBack_Підрозділ(DirectoryPointer directoryPointerItem)
		{
			Form_СтруктураПідприємства form_СтруктураПідприємства = new Form_СтруктураПідприємства();
			form_СтруктураПідприємства.DirectoryPointerItem = directoryPointerItem;
			form_СтруктураПідприємства.DirectoryControlItem = directoryControl_Підрозділ;
			form_СтруктураПідприємства.ShowDialog();
		}

		public void CallBack_Контрагент(DirectoryPointer directoryPointerItem)
		{
			Form_Контрагенти form_Контрагенти = new Form_Контрагенти();
			form_Контрагенти.DirectoryPointerItem = directoryPointerItem;
			form_Контрагенти.DirectoryControlItem = directoryControl_Контрагент;
			form_Контрагенти.ShowDialog();
		}

		private void FormAddCash_Load(object sender, EventArgs e)
        {
			//Статус
			foreach (ConfigurationEnumField field in Конфа.Config.Kernel.Conf.Enums["СтатусиДоговорівКонтрагентів"].Fields.Values)
				comboBox_Статус.Items.Add((Перелічення.СтатусиДоговорівКонтрагентів)field.Value);

			//ГосподарськіОперації
			foreach (ConfigurationEnumField field in Конфа.Config.Kernel.Conf.Enums["ГосподарськіОперації"].Fields.Values)
				comboBox_ГосподарськаОперація.Items.Add((Перелічення.ГосподарськіОперації)field.Value);

			//ТипДоговорів
			foreach (ConfigurationEnumField field in Конфа.Config.Kernel.Conf.Enums["ТипДоговорів"].Fields.Values)
				comboBox_ТипДоговору.Items.Add((Перелічення.ТипДоговорів)field.Value);

			directoryControl_БанківськийРахунок.CallBack = CallBack_БанківськийРахунок;
			directoryControl_БанківськийРахунокКонтрагента.CallBack = CallBack_БанківськийРахунокКонтрагента;
			directoryControl_Підрозділ.CallBack = CallBack_Підрозділ;
			directoryControl_Контрагент.CallBack = CallBack_Контрагент;

			if (IsNew.HasValue)
			{
				договориКонтрагентів_Objest = new Довідники.ДоговориКонтрагентів_Objest();

				if (IsNew.Value)
				{
					this.Text += " - Новий запис";
					directoryControl_БанківськийРахунок.DirectoryPointerItem = new Довідники.БанківськіРахункиОрганізацій_Pointer();
					directoryControl_БанківськийРахунокКонтрагента.DirectoryPointerItem = new Довідники.БанківськіРахункиКонтрагентів_Pointer();
					directoryControl_Підрозділ.DirectoryPointerItem = new Довідники.СтруктураПідприємства_Pointer();
					directoryControl_Контрагент.DirectoryPointerItem = new Довідники.Контрагенти_Pointer();
					comboBox_Статус.SelectedIndex = 0;
					comboBox_ГосподарськаОперація.SelectedIndex = 0;
					comboBox_ТипДоговору.SelectedIndex = 0;
				}
				else
				{
					if (договориКонтрагентів_Objest.Read(new UnigueID(Uid)))
					{
						this.Text += " - Редагування запису - " + договориКонтрагентів_Objest.Назва;

						textBoxName.Text = договориКонтрагентів_Objest.Назва;
						directoryControl_БанківськийРахунок.DirectoryPointerItem = new Довідники.БанківськіРахункиОрганізацій_Pointer(договориКонтрагентів_Objest.БанківськийРахунок.UnigueID);
						directoryControl_БанківськийРахунокКонтрагента.DirectoryPointerItem = new Довідники.БанківськіРахункиКонтрагентів_Pointer(договориКонтрагентів_Objest.БанківськийРахунокКонтрагента.UnigueID);
						directoryControl_Підрозділ.DirectoryPointerItem = new Довідники.СтруктураПідприємства_Pointer(договориКонтрагентів_Objest.Підрозділ.UnigueID);
						directoryControl_Контрагент.DirectoryPointerItem = new Довідники.Контрагенти_Pointer(договориКонтрагентів_Objest.Контрагент.UnigueID);
						comboBox_Статус.SelectedItem = договориКонтрагентів_Objest.Статус;
						comboBox_ГосподарськаОперація.SelectedItem = договориКонтрагентів_Objest.ГосподарськаОперація;
						comboBox_ТипДоговору.SelectedItem = договориКонтрагентів_Objest.ТипДоговору;
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
					договориКонтрагентів_Objest.New();

				try
				{
					договориКонтрагентів_Objest.Назва = textBoxName.Text;
					договориКонтрагентів_Objest.БанківськийРахунок = (Довідники.БанківськіРахункиОрганізацій_Pointer)directoryControl_БанківськийРахунок.DirectoryPointerItem;
					договориКонтрагентів_Objest.БанківськийРахунокКонтрагента = (Довідники.БанківськіРахункиКонтрагентів_Pointer)directoryControl_БанківськийРахунокКонтрагента.DirectoryPointerItem;
					договориКонтрагентів_Objest.Підрозділ = (Довідники.СтруктураПідприємства_Pointer)directoryControl_Підрозділ.DirectoryPointerItem;
					договориКонтрагентів_Objest.Контрагент = (Довідники.Контрагенти_Pointer)directoryControl_Контрагент.DirectoryPointerItem;
					договориКонтрагентів_Objest.Статус = comboBox_Статус.SelectedItem != null ? (Перелічення.СтатусиДоговорівКонтрагентів)comboBox_Статус.SelectedItem : 0;
					договориКонтрагентів_Objest.ГосподарськаОперація = comboBox_ГосподарськаОперація.SelectedItem != null ? (Перелічення.ГосподарськіОперації)comboBox_ГосподарськаОперація.SelectedItem : 0;
					договориКонтрагентів_Objest.ТипДоговору = comboBox_ТипДоговору.SelectedItem != null ? (Перелічення.ТипДоговорів)comboBox_ТипДоговору.SelectedItem : 0;
					договориКонтрагентів_Objest.Save();
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
