﻿/*
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

        private void Form_ЗамовленняКлієнтаДокумент_Load(object sender, EventArgs e)
        {
			//Статус
			foreach (ConfigurationEnumField field in Конфа.Config.Kernel.Conf.Enums["СтатусиЗамовленьКлієнтів"].Fields.Values)
				comboBox_Статус.Items.Add((Перелічення.СтатусиЗамовленьКлієнтів)field.Value);

			//Форма Оплати
			foreach (ConfigurationEnumField field in Конфа.Config.Kernel.Conf.Enums["ФормаОплати"].Fields.Values)
				comboBox_ФормаОплати.Items.Add((Перелічення.ФормаОплати)field.Value);

			//ГосподарськіОперації
			foreach (ConfigurationEnumField field in Конфа.Config.Kernel.Conf.Enums["ГосподарськіОперації"].Fields.Values)
				comboBox_ГосподарськаОперація.Items.Add((Перелічення.ГосподарськіОперації)field.Value);

			directoryControl_Контрагент.Init(new Form_Контрагенти(), new Довідники.Контрагенти_Pointer());
			directoryControl_Організація.Init(new Form_Організації(), new Довідники.Організації_Pointer());
			directoryControl_Валюта.Init(new Form_Валюти(), new Довідники.Валюти_Pointer());
			directoryControl_Склад.Init(new Form_Склади(), new Довідники.Склади_Pointer());
			directoryControl_Каса.Init(new Form_Каси(), new Довідники.Каси_Pointer());
			directoryControl_Договір.Init(new Form_ДоговориКонтрагентів(), new Довідники.ДоговориКонтрагентів_Pointer());
			directoryControl_Підрозділ.Init(new Form_СтруктураПідприємства(), new Довідники.СтруктураПідприємства_Pointer());

			if (IsNew.HasValue)
			{
				замовленняКлієнта_Objest = new Документи.ЗамовленняКлієнта_Objest();

				ЗамовленняКлієнта_ТабличнаЧастина_Товари.ДокументОбєкт = замовленняКлієнта_Objest;

				if (IsNew.Value)
				{
					this.Text += " - Новий";
					textBox_НомерДок.Text = замовленняКлієнта_Objest.НомерДок = (++Константи.НумераціяДокументів.ЗамовленняКлієнта_Const).ToString("D8");
					comboBox_Статус.SelectedIndex = 0;
					comboBox_ФормаОплати.SelectedIndex = 0;
					comboBox_ГосподарськаОперація.SelectedIndex = 0;
				}
				else
				{
					if (замовленняКлієнта_Objest.Read(new UnigueID(Uid)))
					{
						this.Text = замовленняКлієнта_Objest.Назва;

						textBox_НомерДок.Text = замовленняКлієнта_Objest.НомерДок;
						dateTimePicker_ДатаДок.Value = замовленняКлієнта_Objest.ДатаДок;
						directoryControl_Контрагент.DirectoryPointerItem = new Довідники.Контрагенти_Pointer(замовленняКлієнта_Objest.Контрагент.UnigueID);
						directoryControl_Організація.DirectoryPointerItem = new Довідники.Організації_Pointer(замовленняКлієнта_Objest.Організація.UnigueID);
						directoryControl_Валюта.DirectoryPointerItem = new Довідники.Валюти_Pointer(замовленняКлієнта_Objest.Валюта.UnigueID);
						directoryControl_Склад.DirectoryPointerItem = new Довідники.Склади_Pointer(замовленняКлієнта_Objest.Склад.UnigueID);
						comboBox_Статус.SelectedItem = замовленняКлієнта_Objest.Статус;
						comboBox_ФормаОплати.SelectedItem = замовленняКлієнта_Objest.ФормаОплати;
						directoryControl_Каса.DirectoryPointerItem = new Довідники.Каси_Pointer(замовленняКлієнта_Objest.Каса.UnigueID);
						comboBox_ГосподарськаОперація.SelectedItem = замовленняКлієнта_Objest.ГосподарськаОперація;
						directoryControl_Договір.DirectoryPointerItem = new Довідники.ДоговориКонтрагентів_Pointer(замовленняКлієнта_Objest.Договір.UnigueID);
						directoryControl_Підрозділ.DirectoryPointerItem = new Довідники.СтруктураПідприємства_Pointer(замовленняКлієнта_Objest.Підрозділ.UnigueID);
						textBox_Коментар.Text = замовленняКлієнта_Objest.Коментар;

						ЗамовленняКлієнта_ТабличнаЧастина_Товари.LoadRecords();
					}
					else
						MessageBox.Show("Error read");
				}
			}
		}

        private void SaveDoc(bool spendDoc, bool closeForm)
		{
			if (IsNew.HasValue)
			{
				if (IsNew.Value)
					замовленняКлієнта_Objest.New();

				замовленняКлієнта_Objest.НомерДок = textBox_НомерДок.Text;
				замовленняКлієнта_Objest.ДатаДок = dateTimePicker_ДатаДок.Value;
				замовленняКлієнта_Objest.Контрагент = (Довідники.Контрагенти_Pointer)directoryControl_Контрагент.DirectoryPointerItem;
				замовленняКлієнта_Objest.Організація = (Довідники.Організації_Pointer)directoryControl_Організація.DirectoryPointerItem;
				замовленняКлієнта_Objest.Валюта = (Довідники.Валюти_Pointer)directoryControl_Валюта.DirectoryPointerItem;
				замовленняКлієнта_Objest.Склад = (Довідники.Склади_Pointer)directoryControl_Склад.DirectoryPointerItem;
				замовленняКлієнта_Objest.Статус = comboBox_Статус.SelectedItem != null ? (Перелічення.СтатусиЗамовленьКлієнтів)comboBox_Статус.SelectedItem : 0;
				замовленняКлієнта_Objest.ФормаОплати = comboBox_ФормаОплати.SelectedItem != null ? (Перелічення.ФормаОплати)comboBox_ФормаОплати.SelectedItem : 0;
				замовленняКлієнта_Objest.Каса = (Довідники.Каси_Pointer)directoryControl_Каса.DirectoryPointerItem;
				замовленняКлієнта_Objest.ГосподарськаОперація = comboBox_ГосподарськаОперація.SelectedItem != null ? (Перелічення.ГосподарськіОперації)comboBox_ГосподарськаОперація.SelectedItem : 0;
				замовленняКлієнта_Objest.Договір = (Довідники.ДоговориКонтрагентів_Pointer)directoryControl_Договір.DirectoryPointerItem;
				замовленняКлієнта_Objest.Підрозділ = (Довідники.СтруктураПідприємства_Pointer)directoryControl_Підрозділ.DirectoryPointerItem;
				замовленняКлієнта_Objest.Назва = $"Замовлення клієнта №{замовленняКлієнта_Objest.НомерДок} від {замовленняКлієнта_Objest.ДатаДок.ToShortDateString()}";

				замовленняКлієнта_Objest.СумаДокументу = ЗамовленняКлієнта_ТабличнаЧастина_Товари.ОбчислитиСумуДокументу();
				замовленняКлієнта_Objest.Коментар = textBox_Коментар.Text;

				try
				{
					замовленняКлієнта_Objest.Save();
					ЗамовленняКлієнта_ТабличнаЧастина_Товари.SaveRecords();

					IsNew = false;
				}
				catch (Exception exp)
				{
					MessageBox.Show(exp.Message);
					return;
				}

				if (spendDoc)
					try
					{
						//Проведення
						замовленняКлієнта_Objest.SpendTheDocument(замовленняКлієнта_Objest.ДатаДок);
					}
					catch (Exception exp)
					{
						замовленняКлієнта_Objest.ClearSpendTheDocument();
						MessageBox.Show(exp.Message);
						return;
					}
				else
					замовленняКлієнта_Objest.ClearSpendTheDocument();

				if (OwnerForm != null)
					OwnerForm.LoadRecords();

				if (closeForm)
					this.Close();
			}
		}

		private void buttonClose_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void buttonSave_Click(object sender, EventArgs e)
		{
			SaveDoc(false, false);
		}

		private void buttonSpend_Click(object sender, EventArgs e)
		{
			SaveDoc(true, false);
		}

		private void buttonSaveAndSpend_Click(object sender, EventArgs e)
		{
			SaveDoc(true, true);
		}
	}
}
