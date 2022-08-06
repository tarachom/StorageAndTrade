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
    public partial class Form_РеалізаціяТоварівТаПослугДокумент : Form
    {
        public Form_РеалізаціяТоварівТаПослугДокумент()
        {
            InitializeComponent();
        }

		/// <summary>
		/// Форма списку
		/// </summary>
        public Form_РеалізаціяТоварівТаПослугЖурнал OwnerForm { get; set; }
        
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
        private Документи.РеалізаціяТоварівТаПослуг_Objest реалізаціяТоварівТаПослуг_Objest { get; set; }

        private void Form_РеалізаціяТоварівТаПослугДокумент_Load(object sender, EventArgs e)
        {
			ConfigurationEnums ГосподарськіОперації = Конфа.Config.Kernel.Conf.Enums["ГосподарськіОперації"];

			//ГосподарськіОперації
			comboBox_ГосподарськаОперація.Items.Add(
				new NameValue<Перелічення.ГосподарськіОперації>(
					ГосподарськіОперації.Fields["РеалізаціяКлієнту"].Desc,
					Перелічення.ГосподарськіОперації.РеалізаціяКлієнту));

			comboBox_ГосподарськаОперація.SelectedIndex = 0;

			//Статус
			foreach (ConfigurationEnumField field in Конфа.Config.Kernel.Conf.Enums["СтатусиРеалізаціїТоварівТаПослуг"].Fields.Values)
				comboBox_Статус.Items.Add((Перелічення.СтатусиРеалізаціїТоварівТаПослуг)field.Value);

			//Форма Оплати
			foreach (ConfigurationEnumField field in Конфа.Config.Kernel.Conf.Enums["ФормаОплати"].Fields.Values)
				comboBox_ФормаОплати.Items.Add((Перелічення.ФормаОплати)field.Value);

			directoryControl_Контрагент.Init(new Form_Контрагенти(), new Довідники.Контрагенти_Pointer());
			directoryControl_Контрагент.AfterSelectFunc = () =>
			{
				if (directoryControl_Договір.DirectoryPointerItem.IsEmpty())
				{
					Довідники.ДоговориКонтрагентів_Pointer договірКонтрагента =
						ФункціїДляДокументів.ОсновнийДоговірДляКонтрагента(
							(Довідники.Контрагенти_Pointer)directoryControl_Контрагент.DirectoryPointerItem,
							Перелічення.ТипДоговорів.ЗПокупцями);

					if (договірКонтрагента != null)
						directoryControl_Договір.DirectoryPointerItem = договірКонтрагента;
				}

				return true;
			};
			directoryControl_Організація.Init(new Form_Організації(), new Довідники.Організації_Pointer());
			directoryControl_Валюта.Init(new Form_Валюти(), new Довідники.Валюти_Pointer());
			directoryControl_Склад.Init(new Form_Склади(), new Довідники.Склади_Pointer());
			directoryControl_Каса.Init(new Form_Каси(), new Довідники.Каси_Pointer());
			directoryControl_Договір.Init(new Form_ДоговориКонтрагентів(), new Довідники.ДоговориКонтрагентів_Pointer());
			directoryControl_Договір.BeforeClickOpenFunc = () =>
			{
				((Form_ДоговориКонтрагентів)directoryControl_Договір.SelectForm).КонтрагентВласник =
					(Довідники.Контрагенти_Pointer)directoryControl_Контрагент.DirectoryPointerItem;

				return true;
			};
			directoryControl_Підрозділ.Init(new Form_СтруктураПідприємства(), new Довідники.СтруктураПідприємства_Pointer());

			if (IsNew.HasValue)
			{
				реалізаціяТоварівТаПослуг_Objest = new Документи.РеалізаціяТоварівТаПослуг_Objest();

				РеалізаціяТоварівТаПослуг_ТабличнаЧастина_Товари.ДокументОбєкт = реалізаціяТоварівТаПослуг_Objest;

				if (IsNew.Value)
				{
					this.Text += " - Новий";
					textBox_НомерДок.Text = реалізаціяТоварівТаПослуг_Objest.НомерДок = (++Константи.НумераціяДокументів.РеалізаціяТоварівТаПослуг_Const).ToString("D8");
					comboBox_Статус.SelectedIndex = 0;
					comboBox_ФормаОплати.SelectedIndex = 0;

					directoryControl_Контрагент.DirectoryPointerItem = Константи.ЗначенняЗаЗамовчуванням.ОсновнийПостачальник_Const;
					directoryControl_Організація.DirectoryPointerItem = Константи.ЗначенняЗаЗамовчуванням.ОсновнаОрганізація_Const;
					directoryControl_Валюта.DirectoryPointerItem = Константи.ЗначенняЗаЗамовчуванням.ОсновнаВалюта_Const;
					directoryControl_Склад.DirectoryPointerItem = Константи.ЗначенняЗаЗамовчуванням.ОснонийСклад_Const;
					directoryControl_Каса.DirectoryPointerItem = Константи.ЗначенняЗаЗамовчуванням.ОсновнаКаса_Const;
					directoryControl_Підрозділ.DirectoryPointerItem = Константи.ЗначенняЗаЗамовчуванням.ОсновнийПідрозділ_Const;
				}
				else
				{
					if (реалізаціяТоварівТаПослуг_Objest.Read(new UnigueID(Uid)))
					{
						this.Text = реалізаціяТоварівТаПослуг_Objest.Назва;

						textBox_НомерДок.Text = реалізаціяТоварівТаПослуг_Objest.НомерДок;
						dateTimePicker_ДатаДок.Value = реалізаціяТоварівТаПослуг_Objest.ДатаДок;
						directoryControl_Контрагент.DirectoryPointerItem = new Довідники.Контрагенти_Pointer(реалізаціяТоварівТаПослуг_Objest.Контрагент.UnigueID);
						directoryControl_Організація.DirectoryPointerItem = new Довідники.Організації_Pointer(реалізаціяТоварівТаПослуг_Objest.Організація.UnigueID);
						directoryControl_Валюта.DirectoryPointerItem = new Довідники.Валюти_Pointer(реалізаціяТоварівТаПослуг_Objest.Валюта.UnigueID);
						directoryControl_Склад.DirectoryPointerItem = new Довідники.Склади_Pointer(реалізаціяТоварівТаПослуг_Objest.Склад.UnigueID);
						comboBox_Статус.SelectedItem = реалізаціяТоварівТаПослуг_Objest.Статус;
						comboBox_ФормаОплати.SelectedItem = реалізаціяТоварівТаПослуг_Objest.ФормаОплати;
						directoryControl_Каса.DirectoryPointerItem = new Довідники.Каси_Pointer(реалізаціяТоварівТаПослуг_Objest.Каса.UnigueID);
						directoryControl_Договір.DirectoryPointerItem = new Довідники.ДоговориКонтрагентів_Pointer(реалізаціяТоварівТаПослуг_Objest.Договір.UnigueID);
						directoryControl_Підрозділ.DirectoryPointerItem = new Довідники.СтруктураПідприємства_Pointer(реалізаціяТоварівТаПослуг_Objest.Підрозділ.UnigueID);
						textBox_Коментар.Text = реалізаціяТоварівТаПослуг_Objest.Коментар;

						foreach (NameValue<Перелічення.ГосподарськіОперації> операція in comboBox_ГосподарськаОперація.Items)
							if (операція.Value == реалізаціяТоварівТаПослуг_Objest.ГосподарськаОперація)
							{
								comboBox_ГосподарськаОперація.SelectedItem = операція;
								break;
							}

						РеалізаціяТоварівТаПослуг_ТабличнаЧастина_Товари.LoadRecords();
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
					реалізаціяТоварівТаПослуг_Objest.New();

				реалізаціяТоварівТаПослуг_Objest.НомерДок = textBox_НомерДок.Text;
				реалізаціяТоварівТаПослуг_Objest.ДатаДок = dateTimePicker_ДатаДок.Value;
				реалізаціяТоварівТаПослуг_Objest.Контрагент = (Довідники.Контрагенти_Pointer)directoryControl_Контрагент.DirectoryPointerItem;
				реалізаціяТоварівТаПослуг_Objest.Організація = (Довідники.Організації_Pointer)directoryControl_Організація.DirectoryPointerItem;
				реалізаціяТоварівТаПослуг_Objest.Валюта = (Довідники.Валюти_Pointer)directoryControl_Валюта.DirectoryPointerItem;
				реалізаціяТоварівТаПослуг_Objest.Склад = (Довідники.Склади_Pointer)directoryControl_Склад.DirectoryPointerItem;
				реалізаціяТоварівТаПослуг_Objest.Статус = comboBox_Статус.SelectedItem != null ? (Перелічення.СтатусиРеалізаціїТоварівТаПослуг)comboBox_Статус.SelectedItem : 0;
				реалізаціяТоварівТаПослуг_Objest.ФормаОплати = comboBox_ФормаОплати.SelectedItem != null ? (Перелічення.ФормаОплати)comboBox_ФормаОплати.SelectedItem : 0;
				реалізаціяТоварівТаПослуг_Objest.Каса = (Довідники.Каси_Pointer)directoryControl_Каса.DirectoryPointerItem;
				реалізаціяТоварівТаПослуг_Objest.Договір = (Довідники.ДоговориКонтрагентів_Pointer)directoryControl_Договір.DirectoryPointerItem;
				реалізаціяТоварівТаПослуг_Objest.Підрозділ = (Довідники.СтруктураПідприємства_Pointer)directoryControl_Підрозділ.DirectoryPointerItem;
				реалізаціяТоварівТаПослуг_Objest.Назва = $"Реалізація товарів та послуг №{реалізаціяТоварівТаПослуг_Objest.НомерДок} від {реалізаціяТоварівТаПослуг_Objest.ДатаДок.ToShortDateString()}";
				реалізаціяТоварівТаПослуг_Objest.ГосподарськаОперація = ((NameValue<Перелічення.ГосподарськіОперації>)comboBox_ГосподарськаОперація.SelectedItem).Value;

				реалізаціяТоварівТаПослуг_Objest.СумаДокументу = РеалізаціяТоварівТаПослуг_ТабличнаЧастина_Товари.ОбчислитиСумуДокументу();
				реалізаціяТоварівТаПослуг_Objest.Коментар = textBox_Коментар.Text;

				try
				{
					реалізаціяТоварівТаПослуг_Objest.Save();
					РеалізаціяТоварівТаПослуг_ТабличнаЧастина_Товари.SaveRecords();

					IsNew = false;
				}
				catch (Exception exp)
				{
					MessageBox.Show(exp.Message);
					return;
				}

				if (!closeForm)
					РеалізаціяТоварівТаПослуг_ТабличнаЧастина_Товари.LoadRecords();

				if (spendDoc)
					try
					{
						//Проведення
						реалізаціяТоварівТаПослуг_Objest.SpendTheDocument(реалізаціяТоварівТаПослуг_Objest.ДатаДок);
					}
					catch (Exception exp)
					{
						реалізаціяТоварівТаПослуг_Objest.ClearSpendTheDocument();
						MessageBox.Show(exp.Message);
						return;
					}
				else
					реалізаціяТоварівТаПослуг_Objest.ClearSpendTheDocument();

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
