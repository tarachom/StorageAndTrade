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
    public partial class Form_ЗамовленняПостачальникуДокумент : Form
    {
        public Form_ЗамовленняПостачальникуДокумент()
        {
            InitializeComponent();
        }

		/// <summary>
		/// Форма списку
		/// </summary>
        public Form_ЗамовленняПостачальникуЖурнал OwnerForm { get; set; }
        
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
        private Документи.ЗамовленняПостачальнику_Objest замовленняПостачальнику_Objest { get; set; }

        private void FormAddCash_Load(object sender, EventArgs e)
        {
			//Статус
			foreach (ConfigurationEnumField field in Конфа.Config.Kernel.Conf.Enums["СтатусиЗамовленьПостачальникам"].Fields.Values)
				comboBox_Статус.Items.Add((Перелічення.СтатусиЗамовленьПостачальникам)field.Value);

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
				замовленняПостачальнику_Objest = new Документи.ЗамовленняПостачальнику_Objest();

				ЗамовленняПостачальнику_ТабличнаЧастина_Товари.ДокументОбєкт = замовленняПостачальнику_Objest;

				if (IsNew.Value)
				{
					this.Text += " - Новий запис";
					textBox_НомерДок.Text = замовленняПостачальнику_Objest.НомерДок = (++Константи.НумераціяДокументів.ЗамовленняПостачальнику_Const).ToString("D8");
					comboBox_Статус.SelectedIndex = 0;
					comboBox_ФормаОплати.SelectedIndex = 0;
					comboBox_ГосподарськаОперація.SelectedIndex = 0;
				}
				else
				{
					if (замовленняПостачальнику_Objest.Read(new UnigueID(Uid)))
					{
						this.Text += " - Редагування запису - " + замовленняПостачальнику_Objest.НомерДок;

						textBox_НомерДок.Text = замовленняПостачальнику_Objest.НомерДок;
						dateTimePicker_ДатаДок.Value = замовленняПостачальнику_Objest.ДатаДок;
						directoryControl_Контрагент.DirectoryPointerItem = new Довідники.Контрагенти_Pointer(замовленняПостачальнику_Objest.Контрагент.UnigueID);
						directoryControl_Організація.DirectoryPointerItem = new Довідники.Організації_Pointer(замовленняПостачальнику_Objest.Організація.UnigueID);
						directoryControl_Валюта.DirectoryPointerItem = new Довідники.Валюти_Pointer(замовленняПостачальнику_Objest.Валюта.UnigueID);
						directoryControl_Склад.DirectoryPointerItem = new Довідники.Склади_Pointer(замовленняПостачальнику_Objest.Склад.UnigueID);
						comboBox_Статус.SelectedItem = замовленняПостачальнику_Objest.Статус;
						comboBox_ФормаОплати.SelectedItem = замовленняПостачальнику_Objest.ФормаОплати;
						directoryControl_Каса.DirectoryPointerItem = new Довідники.Каси_Pointer(замовленняПостачальнику_Objest.Каса.UnigueID);
						comboBox_ГосподарськаОперація.SelectedItem = замовленняПостачальнику_Objest.ГосподарськаОперація;
						directoryControl_Договір.DirectoryPointerItem = new Довідники.ДоговориКонтрагентів_Pointer(замовленняПостачальнику_Objest.Договір.UnigueID);
						directoryControl_Підрозділ.DirectoryPointerItem = new Довідники.СтруктураПідприємства_Pointer(замовленняПостачальнику_Objest.Підрозділ.UnigueID);

						ЗамовленняПостачальнику_ТабличнаЧастина_Товари.LoadRecords();
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
					замовленняПостачальнику_Objest.New();

				замовленняПостачальнику_Objest.НомерДок = textBox_НомерДок.Text;
				замовленняПостачальнику_Objest.ДатаДок = dateTimePicker_ДатаДок.Value;
				замовленняПостачальнику_Objest.Контрагент = (Довідники.Контрагенти_Pointer)directoryControl_Контрагент.DirectoryPointerItem;
				замовленняПостачальнику_Objest.Організація = (Довідники.Організації_Pointer)directoryControl_Організація.DirectoryPointerItem;
				замовленняПостачальнику_Objest.Валюта = (Довідники.Валюти_Pointer)directoryControl_Валюта.DirectoryPointerItem;
				замовленняПостачальнику_Objest.Склад = (Довідники.Склади_Pointer)directoryControl_Склад.DirectoryPointerItem;
				замовленняПостачальнику_Objest.Статус = comboBox_Статус.SelectedItem != null ? (Перелічення.СтатусиЗамовленьПостачальникам)comboBox_Статус.SelectedItem : 0;
				замовленняПостачальнику_Objest.ФормаОплати = comboBox_ФормаОплати.SelectedItem != null ? (Перелічення.ФормаОплати)comboBox_ФормаОплати.SelectedItem : 0;
				замовленняПостачальнику_Objest.Каса = (Довідники.Каси_Pointer)directoryControl_Каса.DirectoryPointerItem;
				замовленняПостачальнику_Objest.ГосподарськаОперація = comboBox_ГосподарськаОперація.SelectedItem != null ? (Перелічення.ГосподарськіОперації)comboBox_ГосподарськаОперація.SelectedItem : 0;
				замовленняПостачальнику_Objest.Договір = (Довідники.ДоговориКонтрагентів_Pointer)directoryControl_Договір.DirectoryPointerItem;
				замовленняПостачальнику_Objest.Підрозділ = (Довідники.СтруктураПідприємства_Pointer)directoryControl_Підрозділ.DirectoryPointerItem;
				замовленняПостачальнику_Objest.Назва = $"Замовлення постачальнику №{замовленняПостачальнику_Objest.НомерДок} від {замовленняПостачальнику_Objest.ДатаДок.ToShortDateString()}";

				try
				{
					замовленняПостачальнику_Objest.Save();
					ЗамовленняПостачальнику_ТабличнаЧастина_Товари.SaveRecords();
				}
				catch (Exception exp)
				{
					MessageBox.Show(exp.Message);
					return;
				}

				//Очищення регістрів
				замовленняПостачальнику_Objest.ClearSpendTheDocument();

				try
				{
					//Проведення
					замовленняПостачальнику_Objest.SpendTheDocument();
				}
				catch (Exception exp)
				{
					замовленняПостачальнику_Objest.ClearSpendTheDocument();
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
