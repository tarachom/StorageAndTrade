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
    public partial class Form_ПсуванняТоварівДокумент : Form
    {
        public Form_ПсуванняТоварівДокумент()
        {
            InitializeComponent();
        }

		/// <summary>
		/// Форма списку
		/// </summary>
        public Form_ПсуванняТоварівЖурнал OwnerForm { get; set; }
        
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
        private Документи.ПсуванняТоварів_Objest псуванняТоварів_Objest { get; set; }

        private void Form_ПсуванняТоварівДокумент_Load(object sender, EventArgs e)
        {
			directoryControl_Організація.Init(new Form_Організації(), new Довідники.Організації_Pointer(), ПошуковіЗапити.Організації);
			directoryControl_Склад.Init(new Form_Склади(), new Довідники.Склади_Pointer(), ПошуковіЗапити.Склади);
			directoryControl_ВидиЦін.Init(new Form_ВидиЦін(), new Довідники.ВидиЦін_Pointer(), ПошуковіЗапити.ВидиЦін);
			directoryControl_Підрозділ.Init(new Form_СтруктураПідприємства(), new Довідники.СтруктураПідприємства_Pointer());

			if (IsNew.HasValue)
			{
				псуванняТоварів_Objest = new Документи.ПсуванняТоварів_Objest();

				ПсуванняТоварів_ТабличнаЧастина_Товари.ДокументОбєкт = псуванняТоварів_Objest;
				ПсуванняТоварів_ТабличнаЧастина_Товари.ОбновитиЗначенняЗФормиДокумента = () =>
				{
					псуванняТоварів_Objest.Склад = (Довідники.Склади_Pointer)directoryControl_Склад.DirectoryPointerItem;
					псуванняТоварів_Objest.ВидЦіни = (Довідники.ВидиЦін_Pointer)directoryControl_ВидиЦін.DirectoryPointerItem;
				};

				if (IsNew.Value)
				{
					this.Text += " - Новий";
					textBox_НомерДок.Text = псуванняТоварів_Objest.НомерДок = (++Константи.НумераціяДокументів.ПсуванняТоварів_Const).ToString("D8");

					directoryControl_Організація.DirectoryPointerItem = Константи.ЗначенняЗаЗамовчуванням.ОсновнаОрганізація_Const;
					directoryControl_Склад.DirectoryPointerItem = Константи.ЗначенняЗаЗамовчуванням.ОснонийСклад_Const;
					directoryControl_ВидиЦін.DirectoryPointerItem = Константи.ЗначенняЗаЗамовчуванням.ОсновнийВидЦіни_Const;
					directoryControl_Підрозділ.DirectoryPointerItem = Константи.ЗначенняЗаЗамовчуванням.ОсновнийПідрозділ_Const;
				}
				else
				{
					if (псуванняТоварів_Objest.Read(new UnigueID(Uid)))
					{
						this.Text = псуванняТоварів_Objest.Назва;

						textBox_НомерДок.Text = псуванняТоварів_Objest.НомерДок;
						dateTimePicker_ДатаДок.Value = dateTimePicker_ЧасДок.Value = псуванняТоварів_Objest.ДатаДок;
						directoryControl_Організація.DirectoryPointerItem = new Довідники.Організації_Pointer(псуванняТоварів_Objest.Організація.UnigueID);
						directoryControl_Склад.DirectoryPointerItem = new Довідники.Склади_Pointer(псуванняТоварів_Objest.Склад.UnigueID);
						directoryControl_Підрозділ.DirectoryPointerItem = new Довідники.СтруктураПідприємства_Pointer(псуванняТоварів_Objest.Підрозділ.UnigueID);
						textBox_Коментар.Text = псуванняТоварів_Objest.Коментар;

						ПсуванняТоварів_ТабличнаЧастина_Товари.LoadRecords();
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
					псуванняТоварів_Objest.New();

				псуванняТоварів_Objest.НомерДок = textBox_НомерДок.Text;
				псуванняТоварів_Objest.ДатаДок = ФункціїДляДокументів.ОбєднатиДатуТаЧас(dateTimePicker_ДатаДок.Value, dateTimePicker_ЧасДок.Value);
				псуванняТоварів_Objest.Організація = (Довідники.Організації_Pointer)directoryControl_Організація.DirectoryPointerItem;
				псуванняТоварів_Objest.Склад = (Довідники.Склади_Pointer)directoryControl_Склад.DirectoryPointerItem;
				псуванняТоварів_Objest.ВидЦіни = (Довідники.ВидиЦін_Pointer)directoryControl_ВидиЦін.DirectoryPointerItem;
				псуванняТоварів_Objest.Підрозділ = (Довідники.СтруктураПідприємства_Pointer)directoryControl_Підрозділ.DirectoryPointerItem;
				псуванняТоварів_Objest.Назва = $"Псування товару №{псуванняТоварів_Objest.НомерДок} від {псуванняТоварів_Objest.ДатаДок.ToShortDateString()}";

				псуванняТоварів_Objest.СумаДокументу = ПсуванняТоварів_ТабличнаЧастина_Товари.ОбчислитиСумуДокументу();
				псуванняТоварів_Objest.Коментар = textBox_Коментар.Text;

				try
				{
					псуванняТоварів_Objest.Save();
					ПсуванняТоварів_ТабличнаЧастина_Товари.SaveRecords();

					IsNew = false;
				}
				catch (Exception exp)
				{
					MessageBox.Show(exp.Message);
					return;
				}

				if (!closeForm)
					ПсуванняТоварів_ТабличнаЧастина_Товари.LoadRecords();

				if (spendDoc)
					try
					{
						//Проведення
						псуванняТоварів_Objest.SpendTheDocument(псуванняТоварів_Objest.ДатаДок);
					}
					catch (Exception exp)
					{
						псуванняТоварів_Objest.ClearSpendTheDocument();
						MessageBox.Show(exp.Message);
						return;
					}
				else
					псуванняТоварів_Objest.ClearSpendTheDocument();

				if (OwnerForm != null && !OwnerForm.IsDisposed)
				{
					OwnerForm.SelectPointerItem = псуванняТоварів_Objest.GetDocumentPointer();
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
			if (псуванняТоварів_Objest.IsSave)
			{
				if (OwnerForm != null && !OwnerForm.IsDisposed)
				{
					OwnerForm.SelectPointerItem = псуванняТоварів_Objest.GetDocumentPointer();
					OwnerForm.LoadRecords();

					OwnerForm.Focus();
				}
				else
				{
					Form_ПсуванняТоварівЖурнал form_Журнал = new Form_ПсуванняТоварівЖурнал();
					form_Журнал.MdiParent = this.MdiParent;
					form_Журнал.SelectPointerItem = псуванняТоварів_Objest.GetDocumentPointer();
					form_Журнал.Show();
				}
			}
		}

		private void toolStripButtonДрукПроводок_Click(object sender, EventArgs e)
		{
			if (псуванняТоварів_Objest.IsSave)
				РухДокументівПоРегістрах.PrintRecords(псуванняТоварів_Objest.GetDocumentPointer());
		}

		#endregion
	}
}
