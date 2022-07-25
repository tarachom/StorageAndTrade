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
    public partial class Form_АктВиконанихРобітДокумент : Form
    {
        public Form_АктВиконанихРобітДокумент()
        {
            InitializeComponent();
        }

		/// <summary>
		/// Форма списку
		/// </summary>
        public Form_АктВиконанихРобітЖурнал OwnerForm { get; set; }
        
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
        private Документи.АктВиконанихРобіт_Objest актВиконанихРобіт_Objest { get; set; }

        private void FormAddCash_Load(object sender, EventArgs e)
        {
			ConfigurationEnums ГосподарськіОперації = Конфа.Config.Kernel.Conf.Enums["ГосподарськіОперації"];

			//ГосподарськіОперації
			comboBox_ГосподарськаОперація.Items.Add(
				new NameValue<Перелічення.ГосподарськіОперації>(
					ГосподарськіОперації.Fields["РеалізаціяКлієнту"].Desc,
					Перелічення.ГосподарськіОперації.РеалізаціяКлієнту));

			comboBox_ГосподарськаОперація.SelectedIndex = 0;

			//Форма Оплати
			foreach (ConfigurationEnumField field in Конфа.Config.Kernel.Conf.Enums["ФормаОплати"].Fields.Values)
				comboBox_ФормаОплати.Items.Add((Перелічення.ФормаОплати)field.Value);

			directoryControl_Контрагент.Init(new Form_Контрагенти(), new Довідники.Контрагенти_Pointer());
			directoryControl_Організація.Init(new Form_Організації(), new Довідники.Організації_Pointer());
			directoryControl_Валюта.Init(new Form_Валюти(), new Довідники.Валюти_Pointer());
			directoryControl_Каса.Init(new Form_Каси(), new Довідники.Каси_Pointer());
			directoryControl_Договір.Init(new Form_ДоговориКонтрагентів(), new Довідники.ДоговориКонтрагентів_Pointer());
			directoryControl_Підрозділ.Init(new Form_СтруктураПідприємства(), new Довідники.СтруктураПідприємства_Pointer());

			if (IsNew.HasValue)
			{
				актВиконанихРобіт_Objest = new Документи.АктВиконанихРобіт_Objest();

				АктВиконанихРобіт_ТабличнаЧастина_Послуги.ДокументОбєкт = актВиконанихРобіт_Objest;

				if (IsNew.Value)
				{
					this.Text += " - Новий";
					textBox_НомерДок.Text = актВиконанихРобіт_Objest.НомерДок = (++Константи.НумераціяДокументів.АктВиконанихРобіт_Const).ToString("D8");
					comboBox_ФормаОплати.SelectedIndex = 0;
				}
				else
				{
					if (актВиконанихРобіт_Objest.Read(new UnigueID(Uid)))
					{
						this.Text = актВиконанихРобіт_Objest.Назва;

						textBox_НомерДок.Text = актВиконанихРобіт_Objest.НомерДок;
						dateTimePicker_ДатаДок.Value = актВиконанихРобіт_Objest.ДатаДок;
						directoryControl_Контрагент.DirectoryPointerItem = new Довідники.Контрагенти_Pointer(актВиконанихРобіт_Objest.Контрагент.UnigueID);
						directoryControl_Організація.DirectoryPointerItem = new Довідники.Організації_Pointer(актВиконанихРобіт_Objest.Організація.UnigueID);
						directoryControl_Валюта.DirectoryPointerItem = new Довідники.Валюти_Pointer(актВиконанихРобіт_Objest.Валюта.UnigueID);
						comboBox_ФормаОплати.SelectedItem = актВиконанихРобіт_Objest.ФормаОплати;
						directoryControl_Каса.DirectoryPointerItem = new Довідники.Каси_Pointer(актВиконанихРобіт_Objest.Каса.UnigueID);
						directoryControl_Договір.DirectoryPointerItem = new Довідники.ДоговориКонтрагентів_Pointer(актВиконанихРобіт_Objest.Договір.UnigueID);
						directoryControl_Підрозділ.DirectoryPointerItem = new Довідники.СтруктураПідприємства_Pointer(актВиконанихРобіт_Objest.Підрозділ.UnigueID);
						textBox_Коментар.Text = актВиконанихРобіт_Objest.Коментар;

                        foreach (NameValue<Перелічення.ГосподарськіОперації> операція in comboBox_ГосподарськаОперація.Items)
                            if (операція.Value == актВиконанихРобіт_Objest.ГосподарськаОперація)
                            {
                                comboBox_ГосподарськаОперація.SelectedItem = операція;
                                break;
                            }

                        АктВиконанихРобіт_ТабличнаЧастина_Послуги.LoadRecords();
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
					актВиконанихРобіт_Objest.New();

				актВиконанихРобіт_Objest.НомерДок = textBox_НомерДок.Text;
				актВиконанихРобіт_Objest.ДатаДок = dateTimePicker_ДатаДок.Value;
				актВиконанихРобіт_Objest.Контрагент = (Довідники.Контрагенти_Pointer)directoryControl_Контрагент.DirectoryPointerItem;
				актВиконанихРобіт_Objest.Організація = (Довідники.Організації_Pointer)directoryControl_Організація.DirectoryPointerItem;
				актВиконанихРобіт_Objest.Валюта = (Довідники.Валюти_Pointer)directoryControl_Валюта.DirectoryPointerItem;
				актВиконанихРобіт_Objest.ФормаОплати = comboBox_ФормаОплати.SelectedItem != null ? (Перелічення.ФормаОплати)comboBox_ФормаОплати.SelectedItem : 0;
				актВиконанихРобіт_Objest.Каса = (Довідники.Каси_Pointer)directoryControl_Каса.DirectoryPointerItem;
				актВиконанихРобіт_Objest.Договір = (Довідники.ДоговориКонтрагентів_Pointer)directoryControl_Договір.DirectoryPointerItem;
				актВиконанихРобіт_Objest.Підрозділ = (Довідники.СтруктураПідприємства_Pointer)directoryControl_Підрозділ.DirectoryPointerItem;
				актВиконанихРобіт_Objest.Назва = $"Акт виконаних робіт №{актВиконанихРобіт_Objest.НомерДок} від {актВиконанихРобіт_Objest.ДатаДок.ToShortDateString()}";
				актВиконанихРобіт_Objest.ГосподарськаОперація = ((NameValue<Перелічення.ГосподарськіОперації>)comboBox_ГосподарськаОперація.SelectedItem).Value;

				актВиконанихРобіт_Objest.СумаДокументу = АктВиконанихРобіт_ТабличнаЧастина_Послуги.ОбчислитиСумуДокументу();
				актВиконанихРобіт_Objest.Коментар = textBox_Коментар.Text;

				try
				{
					актВиконанихРобіт_Objest.Save();
					АктВиконанихРобіт_ТабличнаЧастина_Послуги.SaveRecords();

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
						актВиконанихРобіт_Objest.SpendTheDocument(актВиконанихРобіт_Objest.ДатаДок);
					}
					catch (Exception exp)
					{
						актВиконанихРобіт_Objest.ClearSpendTheDocument();
						MessageBox.Show(exp.Message);
						return;
					}
				else
					актВиконанихРобіт_Objest.ClearSpendTheDocument();

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
