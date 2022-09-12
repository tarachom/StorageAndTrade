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
    public partial class Form_ПоверненняТоварівВідКлієнтаДокумент : Form
    {
        public Form_ПоверненняТоварівВідКлієнтаДокумент()
        {
            InitializeComponent();
        }

		/// <summary>
		/// Форма списку
		/// </summary>
        public Form_ПоверненняТоварівВідКлієнтаЖурнал OwnerForm { get; set; }
        
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
        private Документи.ПоверненняТоварівВідКлієнта_Objest поверненняТоварівВідКлієнта_Objest { get; set; }

        private void Form_ПоверненняТоварівВідКлієнтаДокумент_Load(object sender, EventArgs e)
        {
			ConfigurationEnums ГосподарськіОперації = Конфа.Config.Kernel.Conf.Enums["ГосподарськіОперації"];

			//ГосподарськіОперації
			comboBox_ГосподарськаОперація.Items.Add(
				new NameValue<Перелічення.ГосподарськіОперації>(
					ГосподарськіОперації.Fields["ПоверненняТоварівВідКлієнта"].Desc,
					Перелічення.ГосподарськіОперації.ПоверненняТоварівВідКлієнта));

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
			directoryControl_Склад.Init(new Form_Склади(), new Довідники.Склади_Pointer(), ПошуковіЗапити.Склади);
			directoryControl_Каса.Init(new Form_Каси(), new Довідники.Каси_Pointer(), ПошуковіЗапити.Каси);
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
				поверненняТоварівВідКлієнта_Objest = new Документи.ПоверненняТоварівВідКлієнта_Objest();

				ПоверненняТоварівВідКлієнта_ТабличнаЧастина_Товари.ДокументОбєкт = поверненняТоварівВідКлієнта_Objest;

				if (IsNew.Value)
				{
					this.Text += " - Новий";
					textBox_НомерДок.Text = поверненняТоварівВідКлієнта_Objest.НомерДок = (++Константи.НумераціяДокументів.ПоверненняТоварівВідКлієнта_Const).ToString("D8");

					directoryControl_Контрагент.DirectoryPointerItem = Константи.ЗначенняЗаЗамовчуванням.ОсновнийПостачальник_Const;
					directoryControl_Організація.DirectoryPointerItem = Константи.ЗначенняЗаЗамовчуванням.ОсновнаОрганізація_Const;
					directoryControl_Валюта.DirectoryPointerItem = Константи.ЗначенняЗаЗамовчуванням.ОсновнаВалюта_Const;
					directoryControl_Склад.DirectoryPointerItem = Константи.ЗначенняЗаЗамовчуванням.ОснонийСклад_Const;
					directoryControl_Каса.DirectoryPointerItem = Константи.ЗначенняЗаЗамовчуванням.ОсновнаКаса_Const;
					directoryControl_Підрозділ.DirectoryPointerItem = Константи.ЗначенняЗаЗамовчуванням.ОсновнийПідрозділ_Const;

					//Основний договір
					if (directoryControl_Контрагент.AfterSelectFunc != null)
						directoryControl_Контрагент.AfterSelectFunc.Invoke();
				}
				else
				{
					if (поверненняТоварівВідКлієнта_Objest.Read(new UnigueID(Uid)))
					{
						this.Text = поверненняТоварівВідКлієнта_Objest.Назва;

						textBox_НомерДок.Text = поверненняТоварівВідКлієнта_Objest.НомерДок;
						dateTimePicker_ДатаДок.Value = dateTimePicker_ЧасДок.Value = поверненняТоварівВідКлієнта_Objest.ДатаДок;
						directoryControl_Контрагент.DirectoryPointerItem = new Довідники.Контрагенти_Pointer(поверненняТоварівВідКлієнта_Objest.Контрагент.UnigueID);
						directoryControl_Організація.DirectoryPointerItem = new Довідники.Організації_Pointer(поверненняТоварівВідКлієнта_Objest.Організація.UnigueID);
						directoryControl_Валюта.DirectoryPointerItem = new Довідники.Валюти_Pointer(поверненняТоварівВідКлієнта_Objest.Валюта.UnigueID);
						directoryControl_Склад.DirectoryPointerItem = new Довідники.Склади_Pointer(поверненняТоварівВідКлієнта_Objest.Склад.UnigueID);
						directoryControl_Каса.DirectoryPointerItem = new Довідники.Каси_Pointer(поверненняТоварівВідКлієнта_Objest.Каса.UnigueID);
						directoryControl_Договір.DirectoryPointerItem = new Довідники.ДоговориКонтрагентів_Pointer(поверненняТоварівВідКлієнта_Objest.Договір.UnigueID);
						directoryControl_Підрозділ.DirectoryPointerItem = new Довідники.СтруктураПідприємства_Pointer(поверненняТоварівВідКлієнта_Objest.Підрозділ.UnigueID);
						textBox_Коментар.Text = поверненняТоварівВідКлієнта_Objest.Коментар;

						ComboBoxNameValue<Перелічення.ГосподарськіОперації>.SelectItem(comboBox_ГосподарськаОперація, поверненняТоварівВідКлієнта_Objest.ГосподарськаОперація);

						ПоверненняТоварівВідКлієнта_ТабличнаЧастина_Товари.LoadRecords();
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
					поверненняТоварівВідКлієнта_Objest.New();

				поверненняТоварівВідКлієнта_Objest.НомерДок = textBox_НомерДок.Text;
				поверненняТоварівВідКлієнта_Objest.ДатаДок = ФункціїДляДокументів.ОбєднатиДатуТаЧас(dateTimePicker_ДатаДок.Value, dateTimePicker_ЧасДок.Value);
				поверненняТоварівВідКлієнта_Objest.Контрагент = (Довідники.Контрагенти_Pointer)directoryControl_Контрагент.DirectoryPointerItem;
				поверненняТоварівВідКлієнта_Objest.Організація = (Довідники.Організації_Pointer)directoryControl_Організація.DirectoryPointerItem;
				поверненняТоварівВідКлієнта_Objest.Валюта = (Довідники.Валюти_Pointer)directoryControl_Валюта.DirectoryPointerItem;
				поверненняТоварівВідКлієнта_Objest.Склад = (Довідники.Склади_Pointer)directoryControl_Склад.DirectoryPointerItem;
				поверненняТоварівВідКлієнта_Objest.Каса = (Довідники.Каси_Pointer)directoryControl_Каса.DirectoryPointerItem;
				поверненняТоварівВідКлієнта_Objest.Договір = (Довідники.ДоговориКонтрагентів_Pointer)directoryControl_Договір.DirectoryPointerItem;
				поверненняТоварівВідКлієнта_Objest.Підрозділ = (Довідники.СтруктураПідприємства_Pointer)directoryControl_Підрозділ.DirectoryPointerItem;
				поверненняТоварівВідКлієнта_Objest.Назва = $"Повернення товарів від клієнта №{поверненняТоварівВідКлієнта_Objest.НомерДок} від {поверненняТоварівВідКлієнта_Objest.ДатаДок.ToShortDateString()}";
				поверненняТоварівВідКлієнта_Objest.ГосподарськаОперація = ((NameValue<Перелічення.ГосподарськіОперації>)comboBox_ГосподарськаОперація.SelectedItem).Value;

				поверненняТоварівВідКлієнта_Objest.СумаДокументу = ПоверненняТоварівВідКлієнта_ТабличнаЧастина_Товари.ОбчислитиСумуДокументу();
				поверненняТоварівВідКлієнта_Objest.Коментар = textBox_Коментар.Text;

				try
				{
					поверненняТоварівВідКлієнта_Objest.Save();
					ПоверненняТоварівВідКлієнта_ТабличнаЧастина_Товари.SaveRecords();

					IsNew = false;
				}
				catch (Exception exp)
				{
					MessageBox.Show(exp.Message);
					return;
				}

				if (!closeForm)
					ПоверненняТоварівВідКлієнта_ТабличнаЧастина_Товари.LoadRecords();

				if (spendDoc)
					try
					{
						//Проведення
						поверненняТоварівВідКлієнта_Objest.SpendTheDocument(поверненняТоварівВідКлієнта_Objest.ДатаДок);
					}
					catch (Exception exp)
					{
						поверненняТоварівВідКлієнта_Objest.ClearSpendTheDocument();
						MessageBox.Show(exp.Message);
						return;
					}
				else
					поверненняТоварівВідКлієнта_Objest.ClearSpendTheDocument();

				if (OwnerForm != null && !OwnerForm.IsDisposed)
				{
					OwnerForm.SelectPointerItem = поверненняТоварівВідКлієнта_Objest.GetDocumentPointer();
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
			if (поверненняТоварівВідКлієнта_Objest.IsSave)
			{
				if (OwnerForm != null && !OwnerForm.IsDisposed)
				{
					OwnerForm.SelectPointerItem = поверненняТоварівВідКлієнта_Objest.GetDocumentPointer();
					OwnerForm.LoadRecords();

					OwnerForm.Focus();
				}
				else
				{
					Form_ПоверненняТоварівВідКлієнтаЖурнал form_Журнал = new Form_ПоверненняТоварівВідКлієнтаЖурнал();
					form_Журнал.MdiParent = this.MdiParent;
					form_Журнал.SelectPointerItem = поверненняТоварівВідКлієнта_Objest.GetDocumentPointer();
					form_Журнал.Show();
				}
			}
		}

		private void toolStripButtonДрукПроводок_Click(object sender, EventArgs e)
		{
			if (поверненняТоварівВідКлієнта_Objest.IsSave)
				РухДокументівПоРегістрах.PrintRecords(поверненняТоварівВідКлієнта_Objest.GetDocumentPointer());
		}

		#endregion
	}
}
