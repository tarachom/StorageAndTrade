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
using Документи = StorageAndTrade_1_0.Документи;
using Перелічення = StorageAndTrade_1_0.Перелічення;

namespace StorageAndTrade
{
    public partial class Form_ПоступленняТоварівТаПослугДокумент : Form
    {
        public Form_ПоступленняТоварівТаПослугДокумент()
        {
            InitializeComponent();
        }

		/// <summary>
		/// Форма списку
		/// </summary>
        public Form_ПоступленняТоварівТаПослугЖурнал OwnerForm { get; set; }
        
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
        private Документи.ПоступленняТоварівТаПослуг_Objest поступленняТоварівТаПослуг_Objest { get; set; }

        private void Form_ПоступленняТоварівТаПослугДокумент_Load(object sender, EventArgs e)
        {
			ConfigurationEnums ГосподарськіОперації = Конфа.Config.Kernel.Conf.Enums["ГосподарськіОперації"];

			//ГосподарськіОперації
			comboBox_ГосподарськаОперація.Items.Add(
				new NameValue<Перелічення.ГосподарськіОперації>(
					ГосподарськіОперації.Fields["ЗакупівляВПостачальника"].Desc,
					Перелічення.ГосподарськіОперації.ЗакупівляВПостачальника));

			comboBox_ГосподарськаОперація.SelectedIndex = 0;

			//Форма Оплати
			foreach (ConfigurationEnumField field in Конфа.Config.Kernel.Conf.Enums["ФормаОплати"].Fields.Values)
				comboBox_ФормаОплати.Items.Add((Перелічення.ФормаОплати)field.Value);

			directoryControl_Контрагент.Init(new Form_Контрагенти(), new Довідники.Контрагенти_Pointer());
			directoryControl_Організація.Init(new Form_Організації(), new Довідники.Організації_Pointer());
			directoryControl_Валюта.Init(new Form_Валюти(), new Довідники.Валюти_Pointer());
			directoryControl_Склад.Init(new Form_Склади(), new Довідники.Склади_Pointer());
			directoryControl_Каса.Init(new Form_Каси(), new Довідники.Каси_Pointer());
			directoryControl_Договір.Init(new Form_ДоговориКонтрагентів(), new Довідники.ДоговориКонтрагентів_Pointer());
			directoryControl_Договір.BeforeClickOpenFunc = () => 
			{
				if (directoryControl_Контрагент.DirectoryPointerItem.IsEmpty())
				{
					MessageBox.Show("Потрібно спочатку вибрати контрагента");
					return false;
				}

				((Form_ДоговориКонтрагентів)directoryControl_Договір.SelectForm).КонтрагентВласник =
					(Довідники.Контрагенти_Pointer)directoryControl_Контрагент.DirectoryPointerItem;

				return true;
			};

			directoryControl_Підрозділ.Init(new Form_СтруктураПідприємства(), new Довідники.СтруктураПідприємства_Pointer());

			if (IsNew.HasValue)
			{
				поступленняТоварівТаПослуг_Objest = new Документи.ПоступленняТоварівТаПослуг_Objest();
				ПоступленняТоварівТаПослуг_ТабличнаЧастина_Товари.ДокументОбєкт = поступленняТоварівТаПослуг_Objest;

				if (IsNew.Value)
				{
					this.Text += " - Новий";
					textBox_НомерДок.Text = поступленняТоварівТаПослуг_Objest.НомерДок = (++Константи.НумераціяДокументів.ПоступленняТоварівТаПослуг_Const).ToString("D8");
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
					if (поступленняТоварівТаПослуг_Objest.Read(new UnigueID(Uid)))
					{
						this.Text = поступленняТоварівТаПослуг_Objest.Назва;

						textBox_НомерДок.Text = поступленняТоварівТаПослуг_Objest.НомерДок;
						dateTimePicker_ДатаДок.Value = поступленняТоварівТаПослуг_Objest.ДатаДок;
						directoryControl_Контрагент.DirectoryPointerItem = new Довідники.Контрагенти_Pointer(поступленняТоварівТаПослуг_Objest.Контрагент.UnigueID);
						directoryControl_Організація.DirectoryPointerItem = new Довідники.Організації_Pointer(поступленняТоварівТаПослуг_Objest.Організація.UnigueID);
						directoryControl_Валюта.DirectoryPointerItem = new Довідники.Валюти_Pointer(поступленняТоварівТаПослуг_Objest.Валюта.UnigueID);
						directoryControl_Склад.DirectoryPointerItem = new Довідники.Склади_Pointer(поступленняТоварівТаПослуг_Objest.Склад.UnigueID);
						comboBox_ФормаОплати.SelectedItem = поступленняТоварівТаПослуг_Objest.ФормаОплати;
						directoryControl_Каса.DirectoryPointerItem = new Довідники.Каси_Pointer(поступленняТоварівТаПослуг_Objest.Каса.UnigueID);
						directoryControl_Договір.DirectoryPointerItem = new Довідники.ДоговориКонтрагентів_Pointer(поступленняТоварівТаПослуг_Objest.Договір.UnigueID);
						directoryControl_Підрозділ.DirectoryPointerItem = new Довідники.СтруктураПідприємства_Pointer(поступленняТоварівТаПослуг_Objest.Підрозділ.UnigueID);
						textBox_Коментар.Text = поступленняТоварівТаПослуг_Objest.Коментар;

						foreach (NameValue<Перелічення.ГосподарськіОперації> операція in comboBox_ГосподарськаОперація.Items)
							if (операція.Value == поступленняТоварівТаПослуг_Objest.ГосподарськаОперація)
							{
								comboBox_ГосподарськаОперація.SelectedItem = операція;
								break;
							}

						ПоступленняТоварівТаПослуг_ТабличнаЧастина_Товари.LoadRecords();
					}
					else
						MessageBox.Show("Error read");
				}
			}
		}

        #region CallBack

        void BeforeClickOpen_Договір()
        {
			if (directoryControl_Контрагент.DirectoryPointerItem.IsEmpty())
			{
				MessageBox.Show("Потрібно вибрати контрагента");
				directoryControl_Договір.StopOpenSelectForm = true;
				return;
			}
			else
				directoryControl_Договір.StopOpenSelectForm = false;

			((Form_ДоговориКонтрагентів)directoryControl_Договір.SelectForm).КонтрагентВласник = 
				(Довідники.Контрагенти_Pointer)directoryControl_Контрагент.DirectoryPointerItem;
		}

        #endregion

        private void SaveDoc(bool spendDoc, bool closeForm)
		{
			if (IsNew.HasValue)
			{
				if (IsNew.Value)
					поступленняТоварівТаПослуг_Objest.New();

				поступленняТоварівТаПослуг_Objest.НомерДок = textBox_НомерДок.Text;
				поступленняТоварівТаПослуг_Objest.ДатаДок = dateTimePicker_ДатаДок.Value;
				поступленняТоварівТаПослуг_Objest.Контрагент = (Довідники.Контрагенти_Pointer)directoryControl_Контрагент.DirectoryPointerItem;
				поступленняТоварівТаПослуг_Objest.Організація = (Довідники.Організації_Pointer)directoryControl_Організація.DirectoryPointerItem;
				поступленняТоварівТаПослуг_Objest.Валюта = (Довідники.Валюти_Pointer)directoryControl_Валюта.DirectoryPointerItem;
				поступленняТоварівТаПослуг_Objest.Склад = (Довідники.Склади_Pointer)directoryControl_Склад.DirectoryPointerItem;
				поступленняТоварівТаПослуг_Objest.ФормаОплати = comboBox_ФормаОплати.SelectedItem != null ? (Перелічення.ФормаОплати)comboBox_ФормаОплати.SelectedItem : 0;
				поступленняТоварівТаПослуг_Objest.Каса = (Довідники.Каси_Pointer)directoryControl_Каса.DirectoryPointerItem;
				поступленняТоварівТаПослуг_Objest.Договір = (Довідники.ДоговориКонтрагентів_Pointer)directoryControl_Договір.DirectoryPointerItem;
				поступленняТоварівТаПослуг_Objest.Підрозділ = (Довідники.СтруктураПідприємства_Pointer)directoryControl_Підрозділ.DirectoryPointerItem;
				поступленняТоварівТаПослуг_Objest.Назва = $"Поступлення товарів та послуг №{поступленняТоварівТаПослуг_Objest.НомерДок} від {поступленняТоварівТаПослуг_Objest.ДатаДок.ToShortDateString()}";
				поступленняТоварівТаПослуг_Objest.ГосподарськаОперація = ((NameValue<Перелічення.ГосподарськіОперації>)comboBox_ГосподарськаОперація.SelectedItem).Value;

				поступленняТоварівТаПослуг_Objest.СумаДокументу = ПоступленняТоварівТаПослуг_ТабличнаЧастина_Товари.ОбчислитиСумуДокументу();
				поступленняТоварівТаПослуг_Objest.Коментар = textBox_Коментар.Text;

				try
				{
					поступленняТоварівТаПослуг_Objest.Save();
					ПоступленняТоварівТаПослуг_ТабличнаЧастина_Товари.SaveRecords();

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
						поступленняТоварівТаПослуг_Objest.SpendTheDocument(поступленняТоварівТаПослуг_Objest.ДатаДок);
					}
					catch (Exception exp)
					{
						поступленняТоварівТаПослуг_Objest.ClearSpendTheDocument();
						MessageBox.Show(exp.Message);
						return;
					}
				else
					поступленняТоварівТаПослуг_Objest.ClearSpendTheDocument();

				if (OwnerForm != null)
					OwnerForm.LoadRecords();

				if (closeForm)
					this.Close();
			}
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

		private void buttonClose_Click(object sender, EventArgs e)
		{
			this.Close();
		}

        
    }
}
