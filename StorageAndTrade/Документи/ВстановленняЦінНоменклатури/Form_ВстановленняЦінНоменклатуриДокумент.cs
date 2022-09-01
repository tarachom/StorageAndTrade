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
    public partial class Form_ВстановленняЦінНоменклатуриДокумент : Form
    {
        public Form_ВстановленняЦінНоменклатуриДокумент()
        {
            InitializeComponent();
        }

		/// <summary>
		/// Форма списку
		/// </summary>
        public Form_ВстановленняЦінНоменклатуриЖурнал OwnerForm { get; set; }
        
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
        private Документи.ВстановленняЦінНоменклатури_Objest встановленняЦінНоменклатури_Objest { get; set; }

        private void Form_ВстановленняЦінНоменклатуриДокумент_Load(object sender, EventArgs e)
        {
			directoryControl_Організація.Init(new Form_Організації(), new Довідники.Організації_Pointer(), ПошуковіЗапити.Організації);
			directoryControl_Валюта.Init(new Form_Валюти(), new Довідники.Валюти_Pointer(), ПошуковіЗапити.Валюти);
			directoryControl_ВидЦіни.Init(new Form_ВидиЦін(), new Довідники.ВидиЦін_Pointer(), ПошуковіЗапити.ВидиЦін);

			if (IsNew.HasValue)
			{
				встановленняЦінНоменклатури_Objest = new Документи.ВстановленняЦінНоменклатури_Objest();
				ВстановленняЦінНоменклатури_ТабличнаЧастина_Товари.ДокументОбєкт = встановленняЦінНоменклатури_Objest;
				ВстановленняЦінНоменклатури_ТабличнаЧастина_Товари.ОбновитиЗначенняЗФормиДокумента = () =>
				{
					встановленняЦінНоменклатури_Objest.Валюта = (Довідники.Валюти_Pointer)directoryControl_Валюта.DirectoryPointerItem;
					встановленняЦінНоменклатури_Objest.ВидЦіни = (Довідники.ВидиЦін_Pointer)directoryControl_ВидЦіни.DirectoryPointerItem;
				};

				if (IsNew.Value)
				{
					this.Text += " - Новий";
					textBox_НомерДок.Text = встановленняЦінНоменклатури_Objest.НомерДок = (++Константи.НумераціяДокументів.ВстановленняЦінНоменклатури_Const).ToString("D8");

					directoryControl_Організація.DirectoryPointerItem = Константи.ЗначенняЗаЗамовчуванням.ОсновнаОрганізація_Const;
					directoryControl_Валюта.DirectoryPointerItem = Константи.ЗначенняЗаЗамовчуванням.ОсновнаВалюта_Const;
					directoryControl_ВидЦіни.DirectoryPointerItem = Константи.ЗначенняЗаЗамовчуванням.ОсновнийВидЦіни_Const;
				}
				else
				{
					if (встановленняЦінНоменклатури_Objest.Read(new UnigueID(Uid)))
					{
						this.Text = встановленняЦінНоменклатури_Objest.Назва;

						textBox_НомерДок.Text = встановленняЦінНоменклатури_Objest.НомерДок;
						dateTimePicker_ДатаДок.Value = dateTimePicker_ЧасДок.Value = встановленняЦінНоменклатури_Objest.ДатаДок;
						directoryControl_Організація.DirectoryPointerItem = new Довідники.Організації_Pointer(встановленняЦінНоменклатури_Objest.Організація.UnigueID);
						directoryControl_Валюта.DirectoryPointerItem = new Довідники.Валюти_Pointer(встановленняЦінНоменклатури_Objest.Валюта.UnigueID);
						directoryControl_ВидЦіни.DirectoryPointerItem = new Довідники.ВидиЦін_Pointer(встановленняЦінНоменклатури_Objest.ВидЦіни.UnigueID);
						textBox_Коментар.Text = встановленняЦінНоменклатури_Objest.Коментар;

						ВстановленняЦінНоменклатури_ТабличнаЧастина_Товари.LoadRecords();
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
					встановленняЦінНоменклатури_Objest.New();

				встановленняЦінНоменклатури_Objest.НомерДок = textBox_НомерДок.Text;
				встановленняЦінНоменклатури_Objest.ДатаДок = ФункціїДляДокументів.ОбєднатиДатуТаЧас(dateTimePicker_ДатаДок.Value, dateTimePicker_ЧасДок.Value);
				встановленняЦінНоменклатури_Objest.Організація = (Довідники.Організації_Pointer)directoryControl_Організація.DirectoryPointerItem;
				встановленняЦінНоменклатури_Objest.Валюта = (Довідники.Валюти_Pointer)directoryControl_Валюта.DirectoryPointerItem;
				встановленняЦінНоменклатури_Objest.ВидЦіни = (Довідники.ВидиЦін_Pointer)directoryControl_ВидЦіни.DirectoryPointerItem;
				встановленняЦінНоменклатури_Objest.Назва = $"Встановлення цін номенклатури №{встановленняЦінНоменклатури_Objest.НомерДок} від {встановленняЦінНоменклатури_Objest.ДатаДок.ToShortDateString()}";
				встановленняЦінНоменклатури_Objest.Коментар = textBox_Коментар.Text;

				try
				{
					встановленняЦінНоменклатури_Objest.Save();
					ВстановленняЦінНоменклатури_ТабличнаЧастина_Товари.SaveRecords();

					IsNew = false;
				}
				catch (Exception exp)
				{
					MessageBox.Show(exp.Message);
					return;
				}

				if (!closeForm)
					ВстановленняЦінНоменклатури_ТабличнаЧастина_Товари.LoadRecords();

				if (spendDoc)
					try
					{
						//Проведення
						встановленняЦінНоменклатури_Objest.SpendTheDocument(встановленняЦінНоменклатури_Objest.ДатаДок);
					}
					catch (Exception exp)
					{
						встановленняЦінНоменклатури_Objest.ClearSpendTheDocument();
						MessageBox.Show(exp.Message);
						return;
					}
				else
					встановленняЦінНоменклатури_Objest.ClearSpendTheDocument();

				if (OwnerForm != null && !OwnerForm.IsDisposed)
				{
					OwnerForm.SelectPointerItem = встановленняЦінНоменклатури_Objest.GetDocumentPointer();
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
			if (встановленняЦінНоменклатури_Objest.IsSave)
			{
				if (OwnerForm != null && !OwnerForm.IsDisposed)
				{
					OwnerForm.SelectPointerItem = встановленняЦінНоменклатури_Objest.GetDocumentPointer();
					OwnerForm.LoadRecords();

					OwnerForm.Focus();
				}
				else
				{
					Form_ВстановленняЦінНоменклатуриЖурнал form_Журнал = new Form_ВстановленняЦінНоменклатуриЖурнал();
					form_Журнал.MdiParent = this.MdiParent;
					form_Журнал.SelectPointerItem = встановленняЦінНоменклатури_Objest.GetDocumentPointer();
					form_Журнал.Show();
				}
			}
		}

		private void toolStripButtonДрукПроводок_Click(object sender, EventArgs e)
		{
			if (встановленняЦінНоменклатури_Objest.IsSave)
				РухДокументівПоРегістрах.PrintRecords(встановленняЦінНоменклатури_Objest.GetDocumentPointer());
		}

		#endregion
	}
}
