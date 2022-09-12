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
    public partial class Form_РахунокФактураДокумент : Form
    {
        public Form_РахунокФактураДокумент()
        {
            InitializeComponent();
        }

		/// <summary>
		/// Форма списку
		/// </summary>
        public Form_РахунокФактураЖурнал OwnerForm { get; set; }
        
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
        private Документи.РахунокФактура_Objest рахунокФактура_Objest { get; set; }

        private void Form_РахунокФактураДокумент_Load(object sender, EventArgs e)
        {
			ConfigurationEnums ГосподарськіОперації = Конфа.Config.Kernel.Conf.Enums["ГосподарськіОперації"];

			//ГосподарськіОперації
			comboBox_ГосподарськаОперація.Items.Add(
				new NameValue<Перелічення.ГосподарськіОперації>(
					ГосподарськіОперації.Fields["ПлануванняПоЗамовленнямКлієнта"].Desc,
					Перелічення.ГосподарськіОперації.ПлануванняПоЗамовленнямКлієнта));

			comboBox_ГосподарськаОперація.SelectedIndex = 0;

			//Статус
			foreach (ConfigurationEnumField field in Конфа.Config.Kernel.Conf.Enums["СтатусиЗамовленьКлієнтів"].Fields.Values)
				comboBox_Статус.Items.Add((Перелічення.СтатусиЗамовленьКлієнтів)field.Value);

			//Форма Оплати
			foreach (ConfigurationEnumField field in Конфа.Config.Kernel.Conf.Enums["ФормаОплати"].Fields.Values)
				comboBox_ФормаОплати.Items.Add((Перелічення.ФормаОплати)field.Value);

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
				рахунокФактура_Objest = new Документи.РахунокФактура_Objest();

				РахунокФактура_ТабличнаЧастина_Товари.ДокументОбєкт = рахунокФактура_Objest;
				РахунокФактура_ТабличнаЧастина_Товари.ОбновитиЗначенняЗФормиДокумента = () =>
				{
					рахунокФактура_Objest.Склад = (Довідники.Склади_Pointer)directoryControl_Склад.DirectoryPointerItem;
				};

				if (IsNew.Value)
				{
					this.Text += " - Новий";
					textBox_НомерДок.Text = рахунокФактура_Objest.НомерДок = (++Константи.НумераціяДокументів.РахунокФактура_Const).ToString("D8");
					comboBox_Статус.SelectedIndex = 0;
					comboBox_ФормаОплати.SelectedIndex = 0;

					directoryControl_Контрагент.DirectoryPointerItem = Константи.ЗначенняЗаЗамовчуванням.ОсновнийПокупець_Const;
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
					if (рахунокФактура_Objest.Read(new UnigueID(Uid)))
					{
						this.Text = рахунокФактура_Objest.Назва;

						textBox_НомерДок.Text = рахунокФактура_Objest.НомерДок;
						dateTimePicker_ДатаДок.Value = dateTimePicker_ЧасДок.Value = рахунокФактура_Objest.ДатаДок;
						directoryControl_Контрагент.DirectoryPointerItem = new Довідники.Контрагенти_Pointer(рахунокФактура_Objest.Контрагент.UnigueID);
						directoryControl_Організація.DirectoryPointerItem = new Довідники.Організації_Pointer(рахунокФактура_Objest.Організація.UnigueID);
						directoryControl_Валюта.DirectoryPointerItem = new Довідники.Валюти_Pointer(рахунокФактура_Objest.Валюта.UnigueID);
						directoryControl_Склад.DirectoryPointerItem = new Довідники.Склади_Pointer(рахунокФактура_Objest.Склад.UnigueID);
						comboBox_Статус.SelectedItem = рахунокФактура_Objest.Статус;
						comboBox_ФормаОплати.SelectedItem = рахунокФактура_Objest.ФормаОплати;
						directoryControl_Каса.DirectoryPointerItem = new Довідники.Каси_Pointer(рахунокФактура_Objest.Каса.UnigueID);
						directoryControl_Договір.DirectoryPointerItem = new Довідники.ДоговориКонтрагентів_Pointer(рахунокФактура_Objest.Договір.UnigueID);
						directoryControl_Підрозділ.DirectoryPointerItem = new Довідники.СтруктураПідприємства_Pointer(рахунокФактура_Objest.Підрозділ.UnigueID);
						textBox_Коментар.Text = рахунокФактура_Objest.Коментар;

						ComboBoxNameValue<Перелічення.ГосподарськіОперації>.SelectItem(comboBox_ГосподарськаОперація, рахунокФактура_Objest.ГосподарськаОперація);

						РахунокФактура_ТабличнаЧастина_Товари.LoadRecords();
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
					рахунокФактура_Objest.New();

				рахунокФактура_Objest.НомерДок = textBox_НомерДок.Text;
				рахунокФактура_Objest.ДатаДок = ФункціїДляДокументів.ОбєднатиДатуТаЧас(dateTimePicker_ДатаДок.Value, dateTimePicker_ЧасДок.Value);
				рахунокФактура_Objest.Контрагент = (Довідники.Контрагенти_Pointer)directoryControl_Контрагент.DirectoryPointerItem;
				рахунокФактура_Objest.Організація = (Довідники.Організації_Pointer)directoryControl_Організація.DirectoryPointerItem;
				рахунокФактура_Objest.Валюта = (Довідники.Валюти_Pointer)directoryControl_Валюта.DirectoryPointerItem;
				рахунокФактура_Objest.Склад = (Довідники.Склади_Pointer)directoryControl_Склад.DirectoryPointerItem;
				рахунокФактура_Objest.Статус = comboBox_Статус.SelectedItem != null ? (Перелічення.СтатусиЗамовленьКлієнтів)comboBox_Статус.SelectedItem : 0;
				рахунокФактура_Objest.ФормаОплати = comboBox_ФормаОплати.SelectedItem != null ? (Перелічення.ФормаОплати)comboBox_ФормаОплати.SelectedItem : 0;
				рахунокФактура_Objest.Каса = (Довідники.Каси_Pointer)directoryControl_Каса.DirectoryPointerItem;
				рахунокФактура_Objest.Договір = (Довідники.ДоговориКонтрагентів_Pointer)directoryControl_Договір.DirectoryPointerItem;
				рахунокФактура_Objest.Підрозділ = (Довідники.СтруктураПідприємства_Pointer)directoryControl_Підрозділ.DirectoryPointerItem;
				рахунокФактура_Objest.Назва = $"Рахунок фактура №{рахунокФактура_Objest.НомерДок} від {рахунокФактура_Objest.ДатаДок.ToShortDateString()}";
				рахунокФактура_Objest.ГосподарськаОперація = ((NameValue<Перелічення.ГосподарськіОперації>)comboBox_ГосподарськаОперація.SelectedItem).Value;

				рахунокФактура_Objest.СумаДокументу = РахунокФактура_ТабличнаЧастина_Товари.ОбчислитиСумуДокументу();
				рахунокФактура_Objest.Коментар = textBox_Коментар.Text;

				try
				{
					рахунокФактура_Objest.Save();
					РахунокФактура_ТабличнаЧастина_Товари.SaveRecords();

					IsNew = false;
				}
				catch (Exception exp)
				{
					MessageBox.Show(exp.Message);
					return;
				}

				if (!closeForm)
					РахунокФактура_ТабличнаЧастина_Товари.LoadRecords();

				if (spendDoc)
					try
					{
						//Проведення
						рахунокФактура_Objest.SpendTheDocument(рахунокФактура_Objest.ДатаДок);
					}
					catch (Exception exp)
					{
						рахунокФактура_Objest.ClearSpendTheDocument();
						MessageBox.Show(exp.Message);
						return;
					}
				else
					рахунокФактура_Objest.ClearSpendTheDocument();

				if (OwnerForm != null && !OwnerForm.IsDisposed)
				{
					OwnerForm.SelectPointerItem = рахунокФактура_Objest.GetDocumentPointer();
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
			if (рахунокФактура_Objest.IsSave)
			{
				if (OwnerForm != null && !OwnerForm.IsDisposed)
				{
					OwnerForm.SelectPointerItem = рахунокФактура_Objest.GetDocumentPointer();
					OwnerForm.LoadRecords();

					OwnerForm.Focus();
				}
				else
				{
					Form_РахунокФактураЖурнал form_Журнал = new Form_РахунокФактураЖурнал();
					form_Журнал.MdiParent = this.MdiParent;
					form_Журнал.SelectPointerItem = рахунокФактура_Objest.GetDocumentPointer();
					form_Журнал.Show();
				}
			}
		}

		private void toolStripButtonДрукПроводок_Click(object sender, EventArgs e)
		{
			if (рахунокФактура_Objest.IsSave)
				РухДокументівПоРегістрах.PrintRecords(рахунокФактура_Objest.GetDocumentPointer());
		}

		#endregion
	}
}
