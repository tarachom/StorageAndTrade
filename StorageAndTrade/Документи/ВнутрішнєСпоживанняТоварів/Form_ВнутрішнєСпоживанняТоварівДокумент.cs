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
    public partial class Form_ВнутрішнєСпоживанняТоварівДокумент : Form
    {
        public Form_ВнутрішнєСпоживанняТоварівДокумент()
        {
            InitializeComponent();
        }

		/// <summary>
		/// Форма списку
		/// </summary>
        public Form_ВнутрішнєСпоживанняТоварівЖурнал OwnerForm { get; set; }
        
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
        private Документи.ВнутрішнєСпоживанняТоварів_Objest внутрішнєСпоживанняТоварів_Objest { get; set; }

        private void Form_ВнутрішнєСпоживанняТоварівДокумент_Load(object sender, EventArgs e)
        {
			ConfigurationEnums ГосподарськіОперації = Конфа.Config.Kernel.Conf.Enums["ГосподарськіОперації"];

			//ГосподарськіОперації
			comboBox_ГосподарськаОперація.Items.Add(
				new NameValue<Перелічення.ГосподарськіОперації>(
					ГосподарськіОперації.Fields["ВнутрішнєСпоживанняТоварів"].Desc,
					Перелічення.ГосподарськіОперації.ВнутрішнєСпоживанняТоварів));

			comboBox_ГосподарськаОперація.SelectedIndex = 0;

			directoryControl_Організація.Init(new Form_Організації(), new Довідники.Організації_Pointer(), ПошуковіЗапити.Організації);
			directoryControl_Валюта.Init(new Form_Валюти(), new Довідники.Валюти_Pointer(), ПошуковіЗапити.Валюти);
			directoryControl_Склад.Init(new Form_Склади(), new Довідники.Склади_Pointer(), ПошуковіЗапити.Склади);
			directoryControl_Підрозділ.Init(new Form_СтруктураПідприємства(), new Довідники.СтруктураПідприємства_Pointer());

			if (IsNew.HasValue)
			{
				внутрішнєСпоживанняТоварів_Objest = new Документи.ВнутрішнєСпоживанняТоварів_Objest();

				ВнутрішнєСпоживанняТоварів_ТабличнаЧастина_Товари.ДокументОбєкт = внутрішнєСпоживанняТоварів_Objest;

				if (IsNew.Value)
				{
					this.Text += " - Новий";
					textBox_НомерДок.Text = внутрішнєСпоживанняТоварів_Objest.НомерДок = (++Константи.НумераціяДокументів.ВнутрішнєСпоживанняТоварів_Const).ToString("D8");

					directoryControl_Організація.DirectoryPointerItem = Константи.ЗначенняЗаЗамовчуванням.ОсновнаОрганізація_Const;
					directoryControl_Валюта.DirectoryPointerItem = Константи.ЗначенняЗаЗамовчуванням.ОсновнаВалюта_Const;
					directoryControl_Склад.DirectoryPointerItem = Константи.ЗначенняЗаЗамовчуванням.ОснонийСклад_Const;
					directoryControl_Підрозділ.DirectoryPointerItem = Константи.ЗначенняЗаЗамовчуванням.ОсновнийПідрозділ_Const;
				}
				else
				{
					if (внутрішнєСпоживанняТоварів_Objest.Read(new UnigueID(Uid)))
					{
						this.Text = внутрішнєСпоживанняТоварів_Objest.Назва;

						textBox_НомерДок.Text = внутрішнєСпоживанняТоварів_Objest.НомерДок;
						dateTimePicker_ДатаДок.Value = dateTimePicker_ЧасДок.Value = внутрішнєСпоживанняТоварів_Objest.ДатаДок;
						directoryControl_Організація.DirectoryPointerItem = new Довідники.Організації_Pointer(внутрішнєСпоживанняТоварів_Objest.Організація.UnigueID);
						directoryControl_Валюта.DirectoryPointerItem = new Довідники.Валюти_Pointer(внутрішнєСпоживанняТоварів_Objest.Валюта.UnigueID);
						directoryControl_Склад.DirectoryPointerItem = new Довідники.Склади_Pointer(внутрішнєСпоживанняТоварів_Objest.Склад.UnigueID);
						directoryControl_Підрозділ.DirectoryPointerItem = new Довідники.СтруктураПідприємства_Pointer(внутрішнєСпоживанняТоварів_Objest.Підрозділ.UnigueID);
						textBox_Коментар.Text = внутрішнєСпоживанняТоварів_Objest.Коментар;

						ComboBoxNameValue<Перелічення.ГосподарськіОперації>.SelectItem(comboBox_ГосподарськаОперація, внутрішнєСпоживанняТоварів_Objest.ГосподарськаОперація);

						ВнутрішнєСпоживанняТоварів_ТабличнаЧастина_Товари.LoadRecords();
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
					внутрішнєСпоживанняТоварів_Objest.New();

				внутрішнєСпоживанняТоварів_Objest.НомерДок = textBox_НомерДок.Text;
				внутрішнєСпоживанняТоварів_Objest.ДатаДок = ФункціїДляДокументів.ОбєднатиДатуТаЧас(dateTimePicker_ДатаДок.Value, dateTimePicker_ЧасДок.Value);
				внутрішнєСпоживанняТоварів_Objest.Організація = (Довідники.Організації_Pointer)directoryControl_Організація.DirectoryPointerItem;
				внутрішнєСпоживанняТоварів_Objest.Валюта = (Довідники.Валюти_Pointer)directoryControl_Валюта.DirectoryPointerItem;
				внутрішнєСпоживанняТоварів_Objest.Склад = (Довідники.Склади_Pointer)directoryControl_Склад.DirectoryPointerItem;
				внутрішнєСпоживанняТоварів_Objest.Підрозділ = (Довідники.СтруктураПідприємства_Pointer)directoryControl_Підрозділ.DirectoryPointerItem;
				внутрішнєСпоживанняТоварів_Objest.Назва = $"Внутрішнє споживання товарів №{внутрішнєСпоживанняТоварів_Objest.НомерДок} від {внутрішнєСпоживанняТоварів_Objest.ДатаДок.ToShortDateString()}";
				внутрішнєСпоживанняТоварів_Objest.ГосподарськаОперація = ((NameValue<Перелічення.ГосподарськіОперації>)comboBox_ГосподарськаОперація.SelectedItem).Value;

				внутрішнєСпоживанняТоварів_Objest.СумаДокументу = ВнутрішнєСпоживанняТоварів_ТабличнаЧастина_Товари.ОбчислитиСумуДокументу();
				внутрішнєСпоживанняТоварів_Objest.Коментар = textBox_Коментар.Text;

				try
				{
					внутрішнєСпоживанняТоварів_Objest.Save();
					ВнутрішнєСпоживанняТоварів_ТабличнаЧастина_Товари.SaveRecords();

					IsNew = false;
				}
				catch (Exception exp)
				{
					MessageBox.Show(exp.Message);
					return;
				}

				if (!closeForm)
					ВнутрішнєСпоживанняТоварів_ТабличнаЧастина_Товари.LoadRecords();

				if (spendDoc)
					try
					{
						//Проведення
						внутрішнєСпоживанняТоварів_Objest.SpendTheDocument(внутрішнєСпоживанняТоварів_Objest.ДатаДок);
					}
					catch (Exception exp)
					{
						внутрішнєСпоживанняТоварів_Objest.ClearSpendTheDocument();
						MessageBox.Show(exp.Message);
						return;
					}
				else
					внутрішнєСпоживанняТоварів_Objest.ClearSpendTheDocument();

				if (OwnerForm != null && !OwnerForm.IsDisposed)
				{
					OwnerForm.SelectPointerItem = внутрішнєСпоживанняТоварів_Objest.GetDocumentPointer();
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
			if (внутрішнєСпоживанняТоварів_Objest.IsSave)
			{
				if (OwnerForm != null && !OwnerForm.IsDisposed)
				{
					OwnerForm.SelectPointerItem = внутрішнєСпоживанняТоварів_Objest.GetDocumentPointer();
					OwnerForm.LoadRecords();

					OwnerForm.Focus();
				}
				else
				{
					Form_ВнутрішнєСпоживанняТоварівЖурнал form_Журнал = new Form_ВнутрішнєСпоживанняТоварівЖурнал();
					form_Журнал.MdiParent = this.MdiParent;
					form_Журнал.SelectPointerItem = внутрішнєСпоживанняТоварів_Objest.GetDocumentPointer();
					form_Журнал.Show();
				}
			}
		}

		private void toolStripButtonДрукПроводок_Click(object sender, EventArgs e)
		{
			if (внутрішнєСпоживанняТоварів_Objest.IsSave)
				РухДокументівПоРегістрах.PrintRecords(внутрішнєСпоживанняТоварів_Objest.GetDocumentPointer());
		}

		#endregion
	}
}
