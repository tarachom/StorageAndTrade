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
    public partial class Form_ПереміщенняТоварівДокумент : Form
    {
        public Form_ПереміщенняТоварівДокумент()
        {
            InitializeComponent();
        }

		/// <summary>
		/// Форма списку
		/// </summary>
        public Form_ПереміщенняТоварівЖурнал OwnerForm { get; set; }
        
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
        private Документи.ПереміщенняТоварів_Objest переміщенняТоварів_Objest { get; set; }

        private void Form_ПереміщенняТоварівДокумент_Load(object sender, EventArgs e)
        {
			ConfigurationEnums ГосподарськіОперації = Конфа.Config.Kernel.Conf.Enums["ГосподарськіОперації"];

			//ГосподарськіОперації
			comboBox_ГосподарськаОперація.Items.Add(
				new NameValue<Перелічення.ГосподарськіОперації>(
					ГосподарськіОперації.Fields["ПереміщенняТоварів"].Desc,
					Перелічення.ГосподарськіОперації.ПереміщенняТоварів));

			comboBox_ГосподарськаОперація.SelectedIndex = 0;

			directoryControl_Організація.Init(new Form_Організації(), new Довідники.Організації_Pointer(), ПошуковіЗапити.Організації);
			directoryControl_СкладВідправник.Init(new Form_Склади(), new Довідники.Склади_Pointer(), ПошуковіЗапити.Склади);
			directoryControl_СкладОтримувач.Init(new Form_Склади(), new Довідники.Склади_Pointer(), ПошуковіЗапити.Склади);
			directoryControl_Підрозділ.Init(new Form_СтруктураПідприємства(), new Довідники.СтруктураПідприємства_Pointer());

			if (IsNew.HasValue)
			{
				переміщенняТоварів_Objest = new Документи.ПереміщенняТоварів_Objest();
				ПереміщенняТоварів_ТабличнаЧастина_Товари.ДокументОбєкт = переміщенняТоварів_Objest;

				if (IsNew.Value)
				{
					this.Text += " - Новий";
					textBox_НомерДок.Text = переміщенняТоварів_Objest.НомерДок = (++Константи.НумераціяДокументів.ПереміщенняТоварів_Const).ToString("D8");

					directoryControl_Організація.DirectoryPointerItem = Константи.ЗначенняЗаЗамовчуванням.ОсновнаОрганізація_Const;
					directoryControl_СкладВідправник.DirectoryPointerItem = Константи.ЗначенняЗаЗамовчуванням.ОснонийСклад_Const;
					directoryControl_Підрозділ.DirectoryPointerItem = Константи.ЗначенняЗаЗамовчуванням.ОсновнийПідрозділ_Const;
				}
				else
				{
					if (переміщенняТоварів_Objest.Read(new UnigueID(Uid)))
					{
						this.Text = переміщенняТоварів_Objest.Назва;

						textBox_НомерДок.Text = переміщенняТоварів_Objest.НомерДок;
						dateTimePicker_ДатаДок.Value = dateTimePicker_ЧасДок.Value = переміщенняТоварів_Objest.ДатаДок;
						directoryControl_Організація.DirectoryPointerItem = new Довідники.Організації_Pointer(переміщенняТоварів_Objest.Організація.UnigueID);
						directoryControl_СкладВідправник.DirectoryPointerItem = new Довідники.Склади_Pointer(переміщенняТоварів_Objest.СкладВідправник.UnigueID);
						directoryControl_СкладОтримувач.DirectoryPointerItem = new Довідники.Склади_Pointer(переміщенняТоварів_Objest.СкладОтримувач.UnigueID);
						directoryControl_Підрозділ.DirectoryPointerItem = new Довідники.СтруктураПідприємства_Pointer(переміщенняТоварів_Objest.Підрозділ.UnigueID);
						textBox_Коментар.Text = переміщенняТоварів_Objest.Коментар;

						ComboBoxNameValue<Перелічення.ГосподарськіОперації>.SelectItem(comboBox_ГосподарськаОперація, переміщенняТоварів_Objest.ГосподарськаОперація);

						ПереміщенняТоварів_ТабличнаЧастина_Товари.LoadRecords();
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
					переміщенняТоварів_Objest.New();

				переміщенняТоварів_Objest.НомерДок = textBox_НомерДок.Text;
				переміщенняТоварів_Objest.ДатаДок = ФункціїДляДокументів.ОбєднатиДатуТаЧас(dateTimePicker_ДатаДок.Value, dateTimePicker_ЧасДок.Value);
				переміщенняТоварів_Objest.Організація = (Довідники.Організації_Pointer)directoryControl_Організація.DirectoryPointerItem;
				переміщенняТоварів_Objest.СкладВідправник = (Довідники.Склади_Pointer)directoryControl_СкладВідправник.DirectoryPointerItem;
				переміщенняТоварів_Objest.СкладОтримувач = (Довідники.Склади_Pointer)directoryControl_СкладОтримувач.DirectoryPointerItem;
				переміщенняТоварів_Objest.Підрозділ = (Довідники.СтруктураПідприємства_Pointer)directoryControl_Підрозділ.DirectoryPointerItem;
				переміщенняТоварів_Objest.Назва = $"Переміщення товарів №{переміщенняТоварів_Objest.НомерДок} від {переміщенняТоварів_Objest.ДатаДок.ToShortDateString()}";
				переміщенняТоварів_Objest.Коментар = textBox_Коментар.Text;
				переміщенняТоварів_Objest.ГосподарськаОперація = ((NameValue<Перелічення.ГосподарськіОперації>)comboBox_ГосподарськаОперація.SelectedItem).Value;

				try
				{
					переміщенняТоварів_Objest.Save();
					ПереміщенняТоварів_ТабличнаЧастина_Товари.SaveRecords();

					IsNew = false;
				}
				catch (Exception exp)
				{
					MessageBox.Show(exp.Message);
					return;
				}

				if (!closeForm)
					ПереміщенняТоварів_ТабличнаЧастина_Товари.LoadRecords();

				if (spendDoc)
					try
					{
						//Проведення
						переміщенняТоварів_Objest.SpendTheDocument(переміщенняТоварів_Objest.ДатаДок);
					}
					catch (Exception exp)
					{
						переміщенняТоварів_Objest.ClearSpendTheDocument();
						MessageBox.Show(exp.Message);
						return;
					}
				else
					переміщенняТоварів_Objest.ClearSpendTheDocument();

				if (OwnerForm != null && !OwnerForm.IsDisposed)
				{
					OwnerForm.SelectPointerItem = переміщенняТоварів_Objest.GetDocumentPointer();
					OwnerForm.LoadRecords();
				}

				if (closeForm)
					this.Close();
			}
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

		private void buttonClose_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		#region Панель меню

		private void toolStripButton_FindToJournal_Click(object sender, EventArgs e)
		{
			if (переміщенняТоварів_Objest.IsSave)
			{
				if (OwnerForm != null && !OwnerForm.IsDisposed)
				{
					OwnerForm.SelectPointerItem = переміщенняТоварів_Objest.GetDocumentPointer();
					OwnerForm.LoadRecords();

					OwnerForm.Focus();
				}
				else
				{
					Form_ПереміщенняТоварівЖурнал form_Журнал = new Form_ПереміщенняТоварівЖурнал();
					form_Журнал.MdiParent = this.MdiParent;
					form_Журнал.SelectPointerItem = переміщенняТоварів_Objest.GetDocumentPointer();
					form_Журнал.Show();
				}
			}
		}

		private void toolStripButtonДрукПроводок_Click(object sender, EventArgs e)
		{
			if (переміщенняТоварів_Objest.IsSave)
				РухДокументівПоРегістрах.PrintRecords(переміщенняТоварів_Objest.GetDocumentPointer());
		}

		#endregion
	}
}
