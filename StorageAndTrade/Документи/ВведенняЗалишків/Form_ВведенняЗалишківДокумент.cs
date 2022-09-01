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
    public partial class Form_ВведенняЗалишківДокумент : Form
    {
        public Form_ВведенняЗалишківДокумент()
        {
            InitializeComponent();
        }

		/// <summary>
		/// Форма списку
		/// </summary>
        public Form_ВведенняЗалишківЖурнал OwnerForm { get; set; }
        
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
        private Документи.ВведенняЗалишків_Objest введенняЗалишків_Objest { get; set; }

        private void Form_ВведенняЗалишківДокумент_Load(object sender, EventArgs e)
        {
			ConfigurationEnums ГосподарськіОперації = Конфа.Config.Kernel.Conf.Enums["ГосподарськіОперації"];

			//ГосподарськіОперації
			comboBox_ГосподарськаОперація.Items.Add(
				new NameValue<Перелічення.ГосподарськіОперації>(
					ГосподарськіОперації.Fields["ВведенняЗалишків"].Desc,
					Перелічення.ГосподарськіОперації.ВведенняЗалишків));

			comboBox_ГосподарськаОперація.SelectedIndex = 0;

			directoryControl_Контрагент.Init(new Form_Контрагенти(), new Довідники.Контрагенти_Pointer(), ПошуковіЗапити.Контрагенти);
			directoryControl_Контрагент.AfterSelectFunc = () =>
			{
				if (directoryControl_Договір.DirectoryPointerItem.IsEmpty())
				{
					Довідники.ДоговориКонтрагентів_Pointer договірКонтрагента =
						ФункціїДляДокументів.ОсновнийДоговірДляКонтрагента(
							(Довідники.Контрагенти_Pointer)directoryControl_Контрагент.DirectoryPointerItem);

					if (договірКонтрагента != null)
						directoryControl_Договір.DirectoryPointerItem = договірКонтрагента;
				}

				return true;
			};
			directoryControl_Організація.Init(new Form_Організації(), new Довідники.Організації_Pointer(), ПошуковіЗапити.Організації);
			directoryControl_Валюта.Init(new Form_Валюти(), new Довідники.Валюти_Pointer(), ПошуковіЗапити.Валюти);
			directoryControl_Склад.Init(new Form_Склади(), new Довідники.Склади_Pointer(), ПошуковіЗапити.Склади);
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
				введенняЗалишків_Objest = new Документи.ВведенняЗалишків_Objest();

				ВведенняЗалишків_ТабличнаЧастина_Товари.ДокументОбєкт = введенняЗалишків_Objest;
				ВведенняЗалишків_ТабличнаЧастина_Каси.ДокументОбєкт = введенняЗалишків_Objest;
				ВведенняЗалишків_ТабличнаЧастина_БанківськіРахунки.ДокументОбєкт = введенняЗалишків_Objest;
				ВведенняЗалишків_ТабличнаЧастина_РозрахункиЗКонтрагентами.ДокументОбєкт = введенняЗалишків_Objest;

				if (IsNew.Value)
				{
					this.Text += " - Новий";
					textBox_НомерДок.Text = введенняЗалишків_Objest.НомерДок = (++Константи.НумераціяДокументів.ВведенняЗалишків_Const).ToString("D8");

					directoryControl_Контрагент.DirectoryPointerItem = Константи.ЗначенняЗаЗамовчуванням.ОсновнийПостачальник_Const;
					directoryControl_Організація.DirectoryPointerItem = Константи.ЗначенняЗаЗамовчуванням.ОсновнаОрганізація_Const;
					directoryControl_Валюта.DirectoryPointerItem = Константи.ЗначенняЗаЗамовчуванням.ОсновнаВалюта_Const;
					directoryControl_Склад.DirectoryPointerItem = Константи.ЗначенняЗаЗамовчуванням.ОснонийСклад_Const;
					directoryControl_Підрозділ.DirectoryPointerItem = Константи.ЗначенняЗаЗамовчуванням.ОсновнийПідрозділ_Const;

					//Основний договір
					if (directoryControl_Контрагент.AfterSelectFunc != null)
						directoryControl_Контрагент.AfterSelectFunc.Invoke();
				}
				else
				{
					if (введенняЗалишків_Objest.Read(new UnigueID(Uid)))
					{
						this.Text = введенняЗалишків_Objest.Назва;

						textBox_НомерДок.Text = введенняЗалишків_Objest.НомерДок;
						dateTimePicker_ДатаДок.Value = dateTimePicker_ЧасДок.Value = введенняЗалишків_Objest.ДатаДок;
						directoryControl_Контрагент.DirectoryPointerItem = new Довідники.Контрагенти_Pointer(введенняЗалишків_Objest.Контрагент.UnigueID);
						directoryControl_Організація.DirectoryPointerItem = new Довідники.Організації_Pointer(введенняЗалишків_Objest.Організація.UnigueID);
						directoryControl_Валюта.DirectoryPointerItem = new Довідники.Валюти_Pointer(введенняЗалишків_Objest.Валюта.UnigueID);
						directoryControl_Склад.DirectoryPointerItem = new Довідники.Склади_Pointer(введенняЗалишків_Objest.Склад.UnigueID);
						directoryControl_Договір.DirectoryPointerItem = new Довідники.ДоговориКонтрагентів_Pointer(введенняЗалишків_Objest.Договір.UnigueID);
						directoryControl_Підрозділ.DirectoryPointerItem = new Довідники.СтруктураПідприємства_Pointer(введенняЗалишків_Objest.Підрозділ.UnigueID);
						textBox_Коментар.Text = введенняЗалишків_Objest.Коментар;

						ComboBoxNameValue<Перелічення.ГосподарськіОперації>.SelectItem(comboBox_ГосподарськаОперація, введенняЗалишків_Objest.ГосподарськаОперація);

						ВведенняЗалишків_ТабличнаЧастина_Товари.LoadRecords();
						ВведенняЗалишків_ТабличнаЧастина_Каси.LoadRecords();
						ВведенняЗалишків_ТабличнаЧастина_БанківськіРахунки.LoadRecords();
						ВведенняЗалишків_ТабличнаЧастина_РозрахункиЗКонтрагентами.LoadRecords();
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
					введенняЗалишків_Objest.New();

				введенняЗалишків_Objest.НомерДок = textBox_НомерДок.Text;
				введенняЗалишків_Objest.ДатаДок = ФункціїДляДокументів.ОбєднатиДатуТаЧас(dateTimePicker_ДатаДок.Value, dateTimePicker_ЧасДок.Value);
				введенняЗалишків_Objest.Контрагент = (Довідники.Контрагенти_Pointer)directoryControl_Контрагент.DirectoryPointerItem;
				введенняЗалишків_Objest.Організація = (Довідники.Організації_Pointer)directoryControl_Організація.DirectoryPointerItem;
				введенняЗалишків_Objest.Валюта = (Довідники.Валюти_Pointer)directoryControl_Валюта.DirectoryPointerItem;
				введенняЗалишків_Objest.Склад = (Довідники.Склади_Pointer)directoryControl_Склад.DirectoryPointerItem;
				введенняЗалишків_Objest.Договір = (Довідники.ДоговориКонтрагентів_Pointer)directoryControl_Договір.DirectoryPointerItem;
				введенняЗалишків_Objest.Підрозділ = (Довідники.СтруктураПідприємства_Pointer)directoryControl_Підрозділ.DirectoryPointerItem;
				введенняЗалишків_Objest.Назва = $"Введення залишків №{введенняЗалишків_Objest.НомерДок} від {введенняЗалишків_Objest.ДатаДок.ToShortDateString()}";
				введенняЗалишків_Objest.ГосподарськаОперація = ((NameValue<Перелічення.ГосподарськіОперації>)comboBox_ГосподарськаОперація.SelectedItem).Value;
				введенняЗалишків_Objest.Коментар = textBox_Коментар.Text;

				try
				{
					введенняЗалишків_Objest.Save();
					ВведенняЗалишків_ТабличнаЧастина_Товари.SaveRecords();
					ВведенняЗалишків_ТабличнаЧастина_Каси.SaveRecords();
					ВведенняЗалишків_ТабличнаЧастина_БанківськіРахунки.SaveRecords();
					ВведенняЗалишків_ТабличнаЧастина_РозрахункиЗКонтрагентами.SaveRecords();

					IsNew = false;
				}
				catch (Exception exp)
				{
					MessageBox.Show(exp.Message);
					return;
				}

				if (!closeForm)
				{
					ВведенняЗалишків_ТабличнаЧастина_Товари.LoadRecords();
					ВведенняЗалишків_ТабличнаЧастина_Каси.LoadRecords();
					ВведенняЗалишків_ТабличнаЧастина_БанківськіРахунки.LoadRecords();
					ВведенняЗалишків_ТабличнаЧастина_РозрахункиЗКонтрагентами.LoadRecords();
				}

				if (spendDoc)
					try
					{
						//Проведення
						введенняЗалишків_Objest.SpendTheDocument(введенняЗалишків_Objest.ДатаДок);
					}
					catch (Exception exp)
					{
						введенняЗалишків_Objest.ClearSpendTheDocument();
						MessageBox.Show(exp.Message);
						return;
					}
				else
					введенняЗалишків_Objest.ClearSpendTheDocument();

				if (OwnerForm != null && !OwnerForm.IsDisposed)
				{
					OwnerForm.SelectPointerItem = введенняЗалишків_Objest.GetDocumentPointer();
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
			if (введенняЗалишків_Objest.IsSave)
			{
				if (OwnerForm != null && !OwnerForm.IsDisposed)
				{
					OwnerForm.SelectPointerItem = введенняЗалишків_Objest.GetDocumentPointer();
					OwnerForm.LoadRecords();

					OwnerForm.Focus();
				}
				else
				{
					Form_ВведенняЗалишківЖурнал form_Журнал = new Form_ВведенняЗалишківЖурнал();
					form_Журнал.MdiParent = this.MdiParent;
					form_Журнал.SelectPointerItem = введенняЗалишків_Objest.GetDocumentPointer();
					form_Журнал.Show();
				}
			}
		}

		private void toolStripButtonДрукПроводок_Click(object sender, EventArgs e)
		{
			if (введенняЗалишків_Objest.IsSave)
				РухДокументівПоРегістрах.PrintRecords(введенняЗалишків_Objest.GetDocumentPointer());
		}

		#endregion
	}
}
