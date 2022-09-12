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

        private void Form_ПрихіднийКасовийОрдерДокумент_Load(object sender, EventArgs e)
        {
			ConfigurationEnums ГосподарськіОперації = Конфа.Config.Kernel.Conf.Enums["ГосподарськіОперації"];

			//ГосподарськіОперації
			comboBox_ГосподарськаОперація.Items.Add(
				new NameValue<Перелічення.ГосподарськіОперації>(
					ГосподарськіОперації.Fields["ПоступленняОплатиВідКлієнта"].Desc,
					Перелічення.ГосподарськіОперації.ПоступленняОплатиВідКлієнта));

			comboBox_ГосподарськаОперація.Items.Add(
				new NameValue<Перелічення.ГосподарськіОперації>(
					ГосподарськіОперації.Fields["ПоступленняКоштівЗІншоїКаси"].Desc,
					Перелічення.ГосподарськіОперації.ПоступленняКоштівЗІншоїКаси));

			comboBox_ГосподарськаОперація.Items.Add(
				new NameValue<Перелічення.ГосподарськіОперації>(
					ГосподарськіОперації.Fields["ПоступленняКоштівЗБанку"].Desc,
					Перелічення.ГосподарськіОперації.ПоступленняКоштівЗБанку));

			comboBox_ГосподарськаОперація.Items.Add(
				new NameValue<Перелічення.ГосподарськіОперації>(
					ГосподарськіОперації.Fields["ПоверненняКоштівПостачальнику"].Desc,
					Перелічення.ГосподарськіОперації.ПоверненняКоштівПостачальнику));

			comboBox_ГосподарськаОперація.Items.Add(
				new NameValue<Перелічення.ГосподарськіОперації>(
					ГосподарськіОперації.Fields["ІншіДоходи"].Desc,
					Перелічення.ГосподарськіОперації.ІншіДоходи));

			//ПоступленняОплатиВідКлієнта
			comboBox_ГосподарськаОперація.SelectedIndex = 0;

			directoryControl_Контрагент.Init(new Form_Контрагенти(), new Довідники.Контрагенти_Pointer(), ПошуковіЗапити.Контрагенти);
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
                else
                {
                    if (directoryControl_Контрагент.DirectoryPointerItem.IsEmpty())
                        directoryControl_Договір.DirectoryPointerItem = new Довідники.ДоговориКонтрагентів_Pointer();
                    else
                    {
                        //
                        //Перевірити чи змінився контрагент
                        //

                        Довідники.ДоговориКонтрагентів_Objest договориКонтрагентів_Objest =
                            ((Довідники.ДоговориКонтрагентів_Pointer)directoryControl_Договір.DirectoryPointerItem).GetDirectoryObject();

                        if (договориКонтрагентів_Objest.Контрагент != (Довідники.Контрагенти_Pointer)directoryControl_Контрагент.DirectoryPointerItem)
                        {
                            directoryControl_Договір.DirectoryPointerItem = new Довідники.ДоговориКонтрагентів_Pointer();
                            directoryControl_Контрагент.AfterSelectFunc.Invoke();
                        }
                    }
                }

                return true;
			};
			directoryControl_Організація.Init(new Form_Організації(), new Довідники.Організації_Pointer(), ПошуковіЗапити.Організації);
			directoryControl_Валюта.Init(new Form_Валюти(), new Довідники.Валюти_Pointer(), ПошуковіЗапити.Валюти);
			directoryControl_Каса.Init(new Form_Каси(), new Довідники.Каси_Pointer(), ПошуковіЗапити.Каси);
			directoryControl_КасаВідправник.Init(new Form_Каси(), new Довідники.Каси_Pointer(), ПошуковіЗапити.Каси);
			directoryControl_БанківськийРахунок.Init(new Form_БанківськіРахункиОрганізацій(), new Довідники.БанківськіРахункиОрганізацій_Pointer(), ПошуковіЗапити.БанківськіРахункиОрганізацій);
			directoryControl_Договір.Init(new Form_ДоговориКонтрагентів(), new Довідники.ДоговориКонтрагентів_Pointer());
			directoryControl_Договір.BeforeClickOpenFunc = () =>
			{
				((Form_ДоговориКонтрагентів)directoryControl_Договір.SelectForm).КонтрагентВласник =
					(Довідники.Контрагенти_Pointer)directoryControl_Контрагент.DirectoryPointerItem;

				return true;
			};

			if (IsNew.HasValue)
			{
				прихіднийКасовийОрдер_Objest = new Документи.ПрихіднийКасовийОрдер_Objest();

				if (IsNew.Value)
				{
					this.Text += " - Новий";
					textBox_НомерДок.Text = прихіднийКасовийОрдер_Objest.НомерДок = (++Константи.НумераціяДокументів.ПрихіднийКасовийОрдер_Const).ToString("D8");

					directoryControl_Контрагент.DirectoryPointerItem = Константи.ЗначенняЗаЗамовчуванням.ОсновнийПостачальник_Const;
					directoryControl_Організація.DirectoryPointerItem = Константи.ЗначенняЗаЗамовчуванням.ОсновнаОрганізація_Const;
					directoryControl_Валюта.DirectoryPointerItem = Константи.ЗначенняЗаЗамовчуванням.ОсновнаВалюта_Const;
					directoryControl_Каса.DirectoryPointerItem = Константи.ЗначенняЗаЗамовчуванням.ОсновнаКаса_Const;
					directoryControl_Організація.DirectoryPointerItem = Константи.ЗначенняЗаЗамовчуванням.ОсновнаОрганізація_Const;
					directoryControl_БанківськийРахунок.DirectoryPointerItem = Константи.ЗначенняЗаЗамовчуванням.ОсновнийБанківськийРахунок_Const;

					//Основний договір
					if (directoryControl_Контрагент.AfterSelectFunc != null)
						directoryControl_Контрагент.AfterSelectFunc.Invoke();
				}
				else
				{
					if (прихіднийКасовийОрдер_Objest.Read(new UnigueID(Uid)))
					{
						this.Text = прихіднийКасовийОрдер_Objest.Назва;

						textBox_НомерДок.Text = прихіднийКасовийОрдер_Objest.НомерДок;
						dateTimePicker_ДатаДок.Value = dateTimePicker_ЧасДок.Value = прихіднийКасовийОрдер_Objest.ДатаДок;
						directoryControl_Контрагент.DirectoryPointerItem = new Довідники.Контрагенти_Pointer(прихіднийКасовийОрдер_Objest.Контрагент.UnigueID);
						directoryControl_Організація.DirectoryPointerItem = new Довідники.Організації_Pointer(прихіднийКасовийОрдер_Objest.Організація.UnigueID);
						directoryControl_Валюта.DirectoryPointerItem = new Довідники.Валюти_Pointer(прихіднийКасовийОрдер_Objest.Валюта.UnigueID);
						directoryControl_Каса.DirectoryPointerItem = new Довідники.Каси_Pointer(прихіднийКасовийОрдер_Objest.Каса.UnigueID);
						directoryControl_КасаВідправник.DirectoryPointerItem = new Довідники.Каси_Pointer(прихіднийКасовийОрдер_Objest.КасаВідправник.UnigueID);
						directoryControl_БанківськийРахунок.DirectoryPointerItem = new Довідники.БанківськіРахункиОрганізацій_Pointer(прихіднийКасовийОрдер_Objest.БанківськийРахунок.UnigueID);
						directoryControl_Договір.DirectoryPointerItem = new Довідники.ДоговориКонтрагентів_Pointer(прихіднийКасовийОрдер_Objest.Договір.UnigueID);
						textBox_СумаДокументу.Text = прихіднийКасовийОрдер_Objest.СумаДокументу.ToString();
						textBox_Коментар.Text = прихіднийКасовийОрдер_Objest.Коментар;

						ComboBoxNameValue<Перелічення.ГосподарськіОперації>.SelectItem(comboBox_ГосподарськаОперація, прихіднийКасовийОрдер_Objest.ГосподарськаОперація);
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
				прихіднийКасовийОрдер_Objest.ДатаДок = ФункціїДляДокументів.ОбєднатиДатуТаЧас(dateTimePicker_ДатаДок.Value, dateTimePicker_ЧасДок.Value);
				прихіднийКасовийОрдер_Objest.Контрагент = (Довідники.Контрагенти_Pointer)directoryControl_Контрагент.DirectoryPointerItem;
				прихіднийКасовийОрдер_Objest.Організація = (Довідники.Організації_Pointer)directoryControl_Організація.DirectoryPointerItem;
				прихіднийКасовийОрдер_Objest.Валюта = (Довідники.Валюти_Pointer)directoryControl_Валюта.DirectoryPointerItem;
				прихіднийКасовийОрдер_Objest.Каса = (Довідники.Каси_Pointer)directoryControl_Каса.DirectoryPointerItem;
				прихіднийКасовийОрдер_Objest.КасаВідправник = (Довідники.Каси_Pointer)directoryControl_КасаВідправник.DirectoryPointerItem;
				прихіднийКасовийОрдер_Objest.БанківськийРахунок = (Довідники.БанківськіРахункиОрганізацій_Pointer)directoryControl_БанківськийРахунок.DirectoryPointerItem;
				прихіднийКасовийОрдер_Objest.Договір = (Довідники.ДоговориКонтрагентів_Pointer)directoryControl_Договір.DirectoryPointerItem;
				прихіднийКасовийОрдер_Objest.СумаДокументу = decimal.Parse(textBox_СумаДокументу.Text);
				прихіднийКасовийОрдер_Objest.Назва = $"Прихідний касовий ордер №{прихіднийКасовийОрдер_Objest.НомерДок} від {прихіднийКасовийОрдер_Objest.ДатаДок.ToShortDateString()}";
				прихіднийКасовийОрдер_Objest.Коментар = textBox_Коментар.Text;
				прихіднийКасовийОрдер_Objest.ГосподарськаОперація = ((NameValue<Перелічення.ГосподарськіОперації>)comboBox_ГосподарськаОперація.SelectedItem).Value;

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

				if (spendDoc)
					try
					{
						//Проведення
						прихіднийКасовийОрдер_Objest.SpendTheDocument(прихіднийКасовийОрдер_Objest.ДатаДок);
					}
					catch (Exception exp)
					{
						прихіднийКасовийОрдер_Objest.ClearSpendTheDocument();
						MessageBox.Show(exp.Message);
						return;
					}
				else
					прихіднийКасовийОрдер_Objest.ClearSpendTheDocument();

				if (OwnerForm != null && !OwnerForm.IsDisposed)
				{
					OwnerForm.SelectPointerItem = прихіднийКасовийОрдер_Objest.GetDocumentPointer();
					OwnerForm.LoadRecords();
				}

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

		#region Панель меню

		private void toolStripButton_FindToJournal_Click(object sender, EventArgs e)
		{
			if (прихіднийКасовийОрдер_Objest.IsSave)
			{
				if (OwnerForm != null && !OwnerForm.IsDisposed)
				{
					OwnerForm.SelectPointerItem = прихіднийКасовийОрдер_Objest.GetDocumentPointer();
					OwnerForm.LoadRecords();

					OwnerForm.Focus();
				}
				else
				{
					Form_ПрихіднийКасовийОрдерЖурнал form_Журнал = new Form_ПрихіднийКасовийОрдерЖурнал();
					form_Журнал.MdiParent = this.MdiParent;
					form_Журнал.SelectPointerItem = прихіднийКасовийОрдер_Objest.GetDocumentPointer();
					form_Журнал.Show();
				}
			}
		}

		private void toolStripButtonДрукПроводок_Click(object sender, EventArgs e)
		{
			if (прихіднийКасовийОрдер_Objest.IsSave)
				РухДокументівПоРегістрах.PrintRecords(прихіднийКасовийОрдер_Objest.GetDocumentPointer());
		}

		#endregion
	}
}
