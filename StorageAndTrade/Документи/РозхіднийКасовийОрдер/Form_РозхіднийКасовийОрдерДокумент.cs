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
    public partial class Form_РозхіднийКасовийОрдерДокумент : Form
    {
        public Form_РозхіднийКасовийОрдерДокумент()
        {
            InitializeComponent();
        }

		/// <summary>
		/// Форма списку
		/// </summary>
        public Form_РозхіднийКасовийОрдерЖурнал OwnerForm { get; set; }
        
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
        private Документи.РозхіднийКасовийОрдер_Objest розхіднийКасовийОрдер_Objest { get; set; }

        private void Form_ЗамовленняКлієнтаДокумент_Load(object sender, EventArgs e)
        {
			//ГосподарськіОперації
			foreach (ConfigurationEnumField field in Конфа.Config.Kernel.Conf.Enums["ГосподарськіОперації"].Fields.Values)
				comboBox_ГосподарськаОперація.Items.Add((Перелічення.ГосподарськіОперації)field.Value);

			directoryControl_Контрагент.Init(new Form_Контрагенти(), new Довідники.Контрагенти_Pointer());
			directoryControl_Організація.Init(new Form_Організації(), new Довідники.Організації_Pointer());
			directoryControl_Валюта.Init(new Form_Валюти(), new Довідники.Валюти_Pointer());
			directoryControl_Каса.Init(new Form_Каси(), new Довідники.Каси_Pointer());
			directoryControl_Договір.Init(new Form_ДоговориКонтрагентів(), new Довідники.ДоговориКонтрагентів_Pointer());

			if (IsNew.HasValue)
			{
				розхіднийКасовийОрдер_Objest = new Документи.РозхіднийКасовийОрдер_Objest();

				if (IsNew.Value)
				{
					this.Text += " - Новий запис";
					textBox_НомерДок.Text = розхіднийКасовийОрдер_Objest.НомерДок = (++Константи.НумераціяДокументів.РозхіднийКасовийОрдер_Const).ToString("D8");
					comboBox_ГосподарськаОперація.SelectedIndex = 0;
				}
				else
				{
					if (розхіднийКасовийОрдер_Objest.Read(new UnigueID(Uid)))
					{
						this.Text += " - Редагування запису - " + розхіднийКасовийОрдер_Objest.НомерДок;

						textBox_НомерДок.Text = розхіднийКасовийОрдер_Objest.НомерДок;
						dateTimePicker_ДатаДок.Value = розхіднийКасовийОрдер_Objest.ДатаДок;
						directoryControl_Контрагент.DirectoryPointerItem = new Довідники.Контрагенти_Pointer(розхіднийКасовийОрдер_Objest.Контрагент.UnigueID);
						directoryControl_Організація.DirectoryPointerItem = new Довідники.Організації_Pointer(розхіднийКасовийОрдер_Objest.Організація.UnigueID);
						directoryControl_Валюта.DirectoryPointerItem = new Довідники.Валюти_Pointer(розхіднийКасовийОрдер_Objest.Валюта.UnigueID);
						directoryControl_Каса.DirectoryPointerItem = new Довідники.Каси_Pointer(розхіднийКасовийОрдер_Objest.Каса.UnigueID);
						directoryControl_Договір.DirectoryPointerItem = new Довідники.ДоговориКонтрагентів_Pointer(розхіднийКасовийОрдер_Objest.Договір.UnigueID);
						comboBox_ГосподарськаОперація.SelectedItem = розхіднийКасовийОрдер_Objest.ГосподарськаОперація;
						textBox_СумаДокументу.Text = розхіднийКасовийОрдер_Objest.СумаДокументу.ToString();
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
					розхіднийКасовийОрдер_Objest.New();

				try
				{
					розхіднийКасовийОрдер_Objest.НомерДок = textBox_НомерДок.Text;
					розхіднийКасовийОрдер_Objest.ДатаДок = dateTimePicker_ДатаДок.Value;
					розхіднийКасовийОрдер_Objest.Контрагент = (Довідники.Контрагенти_Pointer)directoryControl_Контрагент.DirectoryPointerItem;
					розхіднийКасовийОрдер_Objest.Організація = (Довідники.Організації_Pointer)directoryControl_Організація.DirectoryPointerItem;
					розхіднийКасовийОрдер_Objest.Валюта = (Довідники.Валюти_Pointer)directoryControl_Валюта.DirectoryPointerItem;
					розхіднийКасовийОрдер_Objest.Каса = (Довідники.Каси_Pointer)directoryControl_Каса.DirectoryPointerItem;
					розхіднийКасовийОрдер_Objest.Договір = (Довідники.ДоговориКонтрагентів_Pointer)directoryControl_Договір.DirectoryPointerItem;
					розхіднийКасовийОрдер_Objest.ГосподарськаОперація = comboBox_ГосподарськаОперація.SelectedItem != null ? (Перелічення.ГосподарськіОперації)comboBox_ГосподарськаОперація.SelectedItem : 0;
					розхіднийКасовийОрдер_Objest.СумаДокументу = decimal.Parse(textBox_СумаДокументу.Text);
					розхіднийКасовийОрдер_Objest.Назва = $"Розхідний касовий ордер №{розхіднийКасовийОрдер_Objest.НомерДок} від {розхіднийКасовийОрдер_Objest.ДатаДок.ToShortDateString()}";
					розхіднийКасовийОрдер_Objest.Проведений = true;

					розхіднийКасовийОрдер_Objest.Save();
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
