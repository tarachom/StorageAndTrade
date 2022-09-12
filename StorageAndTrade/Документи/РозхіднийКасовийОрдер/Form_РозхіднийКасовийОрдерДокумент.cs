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

        private void Form_РозхіднийКасовийОрдерДокумент_Load(object sender, EventArgs e)
        {
			ConfigurationEnums ГосподарськіОперації = Конфа.Config.Kernel.Conf.Enums["ГосподарськіОперації"];

			//ГосподарськіОперації
			comboBox_ГосподарськаОперація.Items.Add(
				new NameValue<Перелічення.ГосподарськіОперації>(
					ГосподарськіОперації.Fields["ОплатаПостачальнику"].Desc,
					Перелічення.ГосподарськіОперації.ОплатаПостачальнику));

			comboBox_ГосподарськаОперація.Items.Add(
				new NameValue<Перелічення.ГосподарськіОперації>(
					ГосподарськіОперації.Fields["ВидачаКоштівВІншуКасу"].Desc,
					Перелічення.ГосподарськіОперації.ВидачаКоштівВІншуКасу));

			comboBox_ГосподарськаОперація.Items.Add(
				new NameValue<Перелічення.ГосподарськіОперації>(
					ГосподарськіОперації.Fields["ЗдачаКоштівВБанк"].Desc,
					Перелічення.ГосподарськіОперації.ЗдачаКоштівВБанк));

			comboBox_ГосподарськаОперація.Items.Add(
				new NameValue<Перелічення.ГосподарськіОперації>(
					ГосподарськіОперації.Fields["ІншіВитрати"].Desc,
					Перелічення.ГосподарськіОперації.ІншіВитрати));

			comboBox_ГосподарськаОперація.Items.Add(
				new NameValue<Перелічення.ГосподарськіОперації>(
					ГосподарськіОперації.Fields["ПоверненняОплатиКлієнту"].Desc,
					Перелічення.ГосподарськіОперації.ПоверненняОплатиКлієнту));

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
							Перелічення.ТипДоговорів.ЗПостачальниками);

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
			directoryControl_КасаОтримувач.Init(new Form_Каси(), new Довідники.Каси_Pointer(), ПошуковіЗапити.Каси);
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
				розхіднийКасовийОрдер_Objest = new Документи.РозхіднийКасовийОрдер_Objest();

				if (IsNew.Value)
				{
					this.Text += " - Новий";
					textBox_НомерДок.Text = розхіднийКасовийОрдер_Objest.НомерДок = (++Константи.НумераціяДокументів.РозхіднийКасовийОрдер_Const).ToString("D8");

					directoryControl_Контрагент.DirectoryPointerItem = Константи.ЗначенняЗаЗамовчуванням.ОсновнийПостачальник_Const;
					directoryControl_Організація.DirectoryPointerItem = Константи.ЗначенняЗаЗамовчуванням.ОсновнаОрганізація_Const;
					directoryControl_Валюта.DirectoryPointerItem = Константи.ЗначенняЗаЗамовчуванням.ОсновнаВалюта_Const;
					directoryControl_Каса.DirectoryPointerItem = Константи.ЗначенняЗаЗамовчуванням.ОсновнаКаса_Const;
					directoryControl_БанківськийРахунок.DirectoryPointerItem = Константи.ЗначенняЗаЗамовчуванням.ОсновнийБанківськийРахунок_Const;

					//Основний договір
					if (directoryControl_Контрагент.AfterSelectFunc != null)
						directoryControl_Контрагент.AfterSelectFunc.Invoke();
				}
				else
				{
					if (розхіднийКасовийОрдер_Objest.Read(new UnigueID(Uid)))
					{
						this.Text = розхіднийКасовийОрдер_Objest.Назва;
						linkLabel_Основа.Text = розхіднийКасовийОрдер_Objest.Основа.ToString();

						textBox_НомерДок.Text = розхіднийКасовийОрдер_Objest.НомерДок;
						dateTimePicker_ДатаДок.Value = dateTimePicker_ЧасДок.Value = розхіднийКасовийОрдер_Objest.ДатаДок;
						directoryControl_Контрагент.DirectoryPointerItem = new Довідники.Контрагенти_Pointer(розхіднийКасовийОрдер_Objest.Контрагент.UnigueID);
						directoryControl_Організація.DirectoryPointerItem = new Довідники.Організації_Pointer(розхіднийКасовийОрдер_Objest.Організація.UnigueID);
						directoryControl_Валюта.DirectoryPointerItem = new Довідники.Валюти_Pointer(розхіднийКасовийОрдер_Objest.Валюта.UnigueID);
						directoryControl_Каса.DirectoryPointerItem = new Довідники.Каси_Pointer(розхіднийКасовийОрдер_Objest.Каса.UnigueID);
						directoryControl_КасаОтримувач.DirectoryPointerItem = new Довідники.Каси_Pointer(розхіднийКасовийОрдер_Objest.КасаОтримувач.UnigueID);
						directoryControl_БанківськийРахунок.DirectoryPointerItem = new Довідники.БанківськіРахункиОрганізацій_Pointer(розхіднийКасовийОрдер_Objest.БанківськийРахунок.UnigueID);
						directoryControl_Договір.DirectoryPointerItem = new Довідники.ДоговориКонтрагентів_Pointer(розхіднийКасовийОрдер_Objest.Договір.UnigueID);
						textBox_СумаДокументу.Text = розхіднийКасовийОрдер_Objest.СумаДокументу.ToString();
						textBox_Коментар.Text = розхіднийКасовийОрдер_Objest.Коментар;

						ComboBoxNameValue<Перелічення.ГосподарськіОперації>.SelectItem(comboBox_ГосподарськаОперація, розхіднийКасовийОрдер_Objest.ГосподарськаОперація);
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
					розхіднийКасовийОрдер_Objest.New();

				розхіднийКасовийОрдер_Objest.НомерДок = textBox_НомерДок.Text;
				розхіднийКасовийОрдер_Objest.ДатаДок = ФункціїДляДокументів.ОбєднатиДатуТаЧас(dateTimePicker_ДатаДок.Value, dateTimePicker_ЧасДок.Value);
				розхіднийКасовийОрдер_Objest.Контрагент = (Довідники.Контрагенти_Pointer)directoryControl_Контрагент.DirectoryPointerItem;
				розхіднийКасовийОрдер_Objest.Організація = (Довідники.Організації_Pointer)directoryControl_Організація.DirectoryPointerItem;
				розхіднийКасовийОрдер_Objest.Валюта = (Довідники.Валюти_Pointer)directoryControl_Валюта.DirectoryPointerItem;
				розхіднийКасовийОрдер_Objest.Каса = (Довідники.Каси_Pointer)directoryControl_Каса.DirectoryPointerItem;
				розхіднийКасовийОрдер_Objest.КасаОтримувач = (Довідники.Каси_Pointer)directoryControl_КасаОтримувач.DirectoryPointerItem;
				розхіднийКасовийОрдер_Objest.БанківськийРахунок = (Довідники.БанківськіРахункиОрганізацій_Pointer)directoryControl_БанківськийРахунок.DirectoryPointerItem;
				розхіднийКасовийОрдер_Objest.Договір = (Довідники.ДоговориКонтрагентів_Pointer)directoryControl_Договір.DirectoryPointerItem;
				розхіднийКасовийОрдер_Objest.СумаДокументу = decimal.Parse(textBox_СумаДокументу.Text);
				розхіднийКасовийОрдер_Objest.Назва = $"Розхідний касовий ордер №{розхіднийКасовийОрдер_Objest.НомерДок} від {розхіднийКасовийОрдер_Objest.ДатаДок.ToShortDateString()}";
				розхіднийКасовийОрдер_Objest.Коментар = textBox_Коментар.Text;
				розхіднийКасовийОрдер_Objest.ГосподарськаОперація = ((NameValue<Перелічення.ГосподарськіОперації>)comboBox_ГосподарськаОперація.SelectedItem).Value;

				try
				{
					розхіднийКасовийОрдер_Objest.Save();

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
						розхіднийКасовийОрдер_Objest.SpendTheDocument(розхіднийКасовийОрдер_Objest.ДатаДок);
					}
					catch (Exception exp)
					{
						розхіднийКасовийОрдер_Objest.ClearSpendTheDocument();
						MessageBox.Show(exp.Message);
						return;
					}
				else
					розхіднийКасовийОрдер_Objest.ClearSpendTheDocument();

				if (OwnerForm != null && !OwnerForm.IsDisposed)
				{
					OwnerForm.SelectPointerItem = розхіднийКасовийОрдер_Objest.GetDocumentPointer();
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

        private void linkLabel_Основа_Click(object sender, EventArgs e)
        {
			
        }

		#region Панель меню

		private void toolStripButton_FindToJournal_Click(object sender, EventArgs e)
		{
			if (розхіднийКасовийОрдер_Objest.IsSave)
			{
				if (OwnerForm != null && !OwnerForm.IsDisposed)
				{
					OwnerForm.SelectPointerItem = розхіднийКасовийОрдер_Objest.GetDocumentPointer();
					OwnerForm.LoadRecords();

					OwnerForm.Focus();
				}
				else
				{
					Form_РозхіднийКасовийОрдерЖурнал form_Журнал = new Form_РозхіднийКасовийОрдерЖурнал();
					form_Журнал.MdiParent = this.MdiParent;
					form_Журнал.SelectPointerItem = розхіднийКасовийОрдер_Objest.GetDocumentPointer();
					form_Журнал.Show();
				}
			}
		}

		private void toolStripButtonДрукПроводок_Click(object sender, EventArgs e)
		{
			if (розхіднийКасовийОрдер_Objest.IsSave)
				РухДокументівПоРегістрах.PrintRecords(розхіднийКасовийОрдер_Objest.GetDocumentPointer());
		}

		#endregion
	}
}
