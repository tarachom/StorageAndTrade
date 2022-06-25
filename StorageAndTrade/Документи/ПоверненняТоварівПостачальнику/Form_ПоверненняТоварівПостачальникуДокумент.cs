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
    public partial class Form_ПоверненняТоварівПостачальникуДокумент : Form
    {
        public Form_ПоверненняТоварівПостачальникуДокумент()
        {
            InitializeComponent();
        }

		/// <summary>
		/// Форма списку
		/// </summary>
        public Form_ПоверненняТоварівПостачальникуЖурнал OwnerForm { get; set; }
        
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
        private Документи.ПоверненняТоварівПостачальнику_Objest поверненняТоварівПостачальнику_Objest { get; set; }

        private void FormAddCash_Load(object sender, EventArgs e)
        {
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
				поверненняТоварівПостачальнику_Objest = new Документи.ПоверненняТоварівПостачальнику_Objest();

				ПоверненняТоварівПостачальнику_ТабличнаЧастина_Товари.ДокументОбєкт = поверненняТоварівПостачальнику_Objest;

				if (IsNew.Value)
				{
					this.Text += " - Новий запис";
					textBox_НомерДок.Text = поверненняТоварівПостачальнику_Objest.НомерДок = (++Константи.НумераціяДокументів.ПоверненняТоварівПостачальнику_Const).ToString("D8");
					comboBox_ГосподарськаОперація.SelectedIndex = 0;
				}
				else
				{
					if (поверненняТоварівПостачальнику_Objest.Read(new UnigueID(Uid)))
					{
						this.Text += " - Редагування запису - " + поверненняТоварівПостачальнику_Objest.НомерДок;

						textBox_НомерДок.Text = поверненняТоварівПостачальнику_Objest.НомерДок;
						dateTimePicker_ДатаДок.Value = поверненняТоварівПостачальнику_Objest.ДатаДок;
						directoryControl_Контрагент.DirectoryPointerItem = new Довідники.Контрагенти_Pointer(поверненняТоварівПостачальнику_Objest.Контрагент.UnigueID);
						directoryControl_Організація.DirectoryPointerItem = new Довідники.Організації_Pointer(поверненняТоварівПостачальнику_Objest.Організація.UnigueID);
						directoryControl_Валюта.DirectoryPointerItem = new Довідники.Валюти_Pointer(поверненняТоварівПостачальнику_Objest.Валюта.UnigueID);
						directoryControl_Склад.DirectoryPointerItem = new Довідники.Склади_Pointer(поверненняТоварівПостачальнику_Objest.Склад.UnigueID);
						directoryControl_Каса.DirectoryPointerItem = new Довідники.Каси_Pointer(поверненняТоварівПостачальнику_Objest.Каса.UnigueID);
						comboBox_ГосподарськаОперація.SelectedItem = поверненняТоварівПостачальнику_Objest.ГосподарськаОперація;
						directoryControl_Договір.DirectoryPointerItem = new Довідники.ДоговориКонтрагентів_Pointer(поверненняТоварівПостачальнику_Objest.Договір.UnigueID);
						directoryControl_Підрозділ.DirectoryPointerItem = new Довідники.СтруктураПідприємства_Pointer(поверненняТоварівПостачальнику_Objest.Підрозділ.UnigueID);

						ПоверненняТоварівПостачальнику_ТабличнаЧастина_Товари.LoadRecords();
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
					поверненняТоварівПостачальнику_Objest.New();

				try
				{
					поверненняТоварівПостачальнику_Objest.НомерДок = textBox_НомерДок.Text;
					поверненняТоварівПостачальнику_Objest.ДатаДок = dateTimePicker_ДатаДок.Value;
					поверненняТоварівПостачальнику_Objest.Контрагент = (Довідники.Контрагенти_Pointer)directoryControl_Контрагент.DirectoryPointerItem;
					поверненняТоварівПостачальнику_Objest.Організація = (Довідники.Організації_Pointer)directoryControl_Організація.DirectoryPointerItem;
					поверненняТоварівПостачальнику_Objest.Валюта = (Довідники.Валюти_Pointer)directoryControl_Валюта.DirectoryPointerItem;
					поверненняТоварівПостачальнику_Objest.Склад = (Довідники.Склади_Pointer)directoryControl_Склад.DirectoryPointerItem;
					поверненняТоварівПостачальнику_Objest.Каса = (Довідники.Каси_Pointer)directoryControl_Каса.DirectoryPointerItem;
					поверненняТоварівПостачальнику_Objest.ГосподарськаОперація = comboBox_ГосподарськаОперація.SelectedItem != null ? (Перелічення.ГосподарськіОперації)comboBox_ГосподарськаОперація.SelectedItem : 0;
					поверненняТоварівПостачальнику_Objest.Договір = (Довідники.ДоговориКонтрагентів_Pointer)directoryControl_Договір.DirectoryPointerItem;
					поверненняТоварівПостачальнику_Objest.Підрозділ = (Довідники.СтруктураПідприємства_Pointer)directoryControl_Підрозділ.DirectoryPointerItem;
					поверненняТоварівПостачальнику_Objest.Назва = $"Повернення постачальнику №{поверненняТоварівПостачальнику_Objest.НомерДок} від {поверненняТоварівПостачальнику_Objest.ДатаДок.ToShortDateString()}";
					поверненняТоварівПостачальнику_Objest.Проведений = true;

					ПоверненняТоварівПостачальнику_ТабличнаЧастина_Товари.SaveRecords();
					поверненняТоварівПостачальнику_Objest.Save();
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
