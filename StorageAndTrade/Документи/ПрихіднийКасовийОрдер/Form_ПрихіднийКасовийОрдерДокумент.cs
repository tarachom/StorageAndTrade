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
    public partial class Form_ПрихіднийКасовийОрдерДокумент : Form
    {
        public Form_ПрихіднийКасовийОрдерДокумент()
        {
            InitializeComponent();
        }

		/// <summary>
		/// Форма списку
		/// </summary>
        public Form_ПрихіднийКасовийОрдерЖурнал OwnerForm { get; set; }
        
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
        private Документи.ПрихіднийКасовийОрдер_Objest прихіднийКасовийОрдер_Objest { get; set; }

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
				прихіднийКасовийОрдер_Objest = new Документи.ПрихіднийКасовийОрдер_Objest();

				if (IsNew.Value)
				{
					this.Text += " - Новий";
					textBox_НомерДок.Text = прихіднийКасовийОрдер_Objest.НомерДок = (++Константи.НумераціяДокументів.ПрихіднийКасовийОрдер_Const).ToString("D8");
					comboBox_ГосподарськаОперація.SelectedIndex = 0;
				}
				else
				{
					if (прихіднийКасовийОрдер_Objest.Read(new UnigueID(Uid)))
					{
						this.Text = прихіднийКасовийОрдер_Objest.Назва;

						textBox_НомерДок.Text = прихіднийКасовийОрдер_Objest.НомерДок;
						dateTimePicker_ДатаДок.Value = прихіднийКасовийОрдер_Objest.ДатаДок;
						directoryControl_Контрагент.DirectoryPointerItem = new Довідники.Контрагенти_Pointer(прихіднийКасовийОрдер_Objest.Контрагент.UnigueID);
						directoryControl_Організація.DirectoryPointerItem = new Довідники.Організації_Pointer(прихіднийКасовийОрдер_Objest.Організація.UnigueID);
						directoryControl_Валюта.DirectoryPointerItem = new Довідники.Валюти_Pointer(прихіднийКасовийОрдер_Objest.Валюта.UnigueID);
						directoryControl_Каса.DirectoryPointerItem = new Довідники.Каси_Pointer(прихіднийКасовийОрдер_Objest.Каса.UnigueID);
						directoryControl_Договір.DirectoryPointerItem = new Довідники.ДоговориКонтрагентів_Pointer(прихіднийКасовийОрдер_Objest.Договір.UnigueID);
						comboBox_ГосподарськаОперація.SelectedItem = прихіднийКасовийОрдер_Objest.ГосподарськаОперація;
						textBox_СумаДокументу.Text = прихіднийКасовийОрдер_Objest.СумаДокументу.ToString();
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
					прихіднийКасовийОрдер_Objest.New();

				прихіднийКасовийОрдер_Objest.НомерДок = textBox_НомерДок.Text;
				прихіднийКасовийОрдер_Objest.ДатаДок = dateTimePicker_ДатаДок.Value;
				прихіднийКасовийОрдер_Objest.Контрагент = (Довідники.Контрагенти_Pointer)directoryControl_Контрагент.DirectoryPointerItem;
				прихіднийКасовийОрдер_Objest.Організація = (Довідники.Організації_Pointer)directoryControl_Організація.DirectoryPointerItem;
				прихіднийКасовийОрдер_Objest.Валюта = (Довідники.Валюти_Pointer)directoryControl_Валюта.DirectoryPointerItem;
				прихіднийКасовийОрдер_Objest.Каса = (Довідники.Каси_Pointer)directoryControl_Каса.DirectoryPointerItem;
				прихіднийКасовийОрдер_Objest.Договір = (Довідники.ДоговориКонтрагентів_Pointer)directoryControl_Договір.DirectoryPointerItem;
				прихіднийКасовийОрдер_Objest.ГосподарськаОперація = comboBox_ГосподарськаОперація.SelectedItem != null ? (Перелічення.ГосподарськіОперації)comboBox_ГосподарськаОперація.SelectedItem : 0;
				прихіднийКасовийОрдер_Objest.СумаДокументу = decimal.Parse(textBox_СумаДокументу.Text);
				прихіднийКасовийОрдер_Objest.Назва = $"Прихідний касовий ордер №{прихіднийКасовийОрдер_Objest.НомерДок} від {прихіднийКасовийОрдер_Objest.ДатаДок.ToShortDateString()}";

				try
				{
					прихіднийКасовийОрдер_Objest.Save();

					IsNew = false;
				}
				catch (Exception exp)
				{
					MessageBox.Show(exp.Message);
					return;
				}

				//Очищення регістрів
				прихіднийКасовийОрдер_Objest.ClearSpendTheDocument();

				if (spendDoc)
					try
					{
						//Проведення
						прихіднийКасовийОрдер_Objest.SpendTheDocument();
					}
					catch (Exception exp)
					{
						прихіднийКасовийОрдер_Objest.ClearSpendTheDocument();
						MessageBox.Show(exp.Message);
						return;
					}

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
