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
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using AccountingSoftware;
using Конфа = StorageAndTrade_1_0;
using Константи = StorageAndTrade_1_0.Константи;
using Документи = StorageAndTrade_1_0.Документи;
using Journal = StorageAndTrade_1_0.Журнали;
using System.Reflection;
using StorageAndTrade.Service;

namespace StorageAndTrade
{
	public partial class FormService : Form
	{
		public FormService()
		{
			InitializeComponent();
		}

		private object lockobject = new object();
		private bool CancelThread = false;
		private Thread thread;

		private void ApendLine(string head)
		{
			if (richTextBoxInfo.InvokeRequired)
			{
				try
				{
					richTextBoxInfo.Invoke(new Action<string>(ApendLine), head);
				}
				catch { }
			}
			else
			{
				richTextBoxInfo.AppendText("\n" + head);
			}
		}

        private void buttonCancel_Click(object sender, EventArgs e)
        {
			buttonSpendAll.Enabled = true;
			buttonCancel.Enabled = false;

			CancelThread = true;
		}

        private void buttonSpendAll_Click(object sender, EventArgs e)
        {
			CancelThread = false;

			buttonSpendAll.Enabled = false;
			buttonCancel.Enabled = true;
			button_CalculationBalancesAll.Enabled = false;

			thread = new Thread(new ThreadStart(SpendAllDocument));
			thread.Start();
		}

		private void button_CalculationBalancesAll_Click(object sender, EventArgs e)
		{
			CancelThread = false;

			buttonSpendAll.Enabled = false;
			buttonCancel.Enabled = true;
			button_CalculationBalancesAll.Enabled = false;

			thread = new Thread(new ThreadStart(CalculationBalancesAll));
			thread.Start();
		}

		void CalculationBalancesAll()
		{
			Константи.Системні.ВвімкнутиФоновіЗадачі_Const = false;

			CalculationBalancesAll_Func();

			Константи.Системні.ВвімкнутиФоновіЗадачі_Const = true;

			buttonSpendAll.Invoke(new Action(() => buttonSpendAll.Enabled = true));
			buttonCancel.Invoke(new Action(() => buttonCancel.Enabled = false));
			button_CalculationBalancesAll.Invoke(new Action(() => button_CalculationBalancesAll.Enabled = true));
		}

		void CalculationBalancesAll_Func()
        {
			ApendLine("\nПерерахунок залишків");

			ApendLine("\nОбчислення залишків з групуванням по днях");
			foreach (string registerAccumulation in CalculationBalances.СписокДоступнихВіртуальнихРегістрів)
			{
				ApendLine(" --> регістер: " + registerAccumulation);
				CalculationBalances.ОбчисленняВіртуальнихЗалишківПоВсіхДнях(registerAccumulation);
			}

			ApendLine("\nОбновлення актуальності:");
			foreach (string registerAccumulation in CalculationBalances.СписокДоступнихВіртуальнихРегістрів)
			{
				ApendLine(" --> регістер: " + registerAccumulation);
				CalculationBalances.СкинутиЗначенняАктуальностіВіртуальнихЗалишківПоВсіхМісяцях(registerAccumulation);
			}

			ApendLine("\nОбчислення залишків з групуванням по місяцях");
			CalculationBalances.ОбчисленняВіртуальнихЗалишківПоМісяцях();

			ApendLine("\nГотово!");
		}

		void SpendAllDocument()
		{
			Константи.Системні.ВвімкнутиФоновіЗадачі_Const = false;

			Journal.Journal_Select journalSelect = new Journal.Journal_Select();
			journalSelect.Select(DateTime.Parse("01.01.2000 00:00:00"), DateTime.Now);

			while (journalSelect.MoveNext())
            {
				if (CancelThread)
					break;

				if (journalSelect.Current.Spend)
				{
					ApendLine(journalSelect.Current.TypeDocument + " " + journalSelect.Current.SpendDate);

					DocumentObject doc = journalSelect.GetDocumentObject(true);

					if (CancelThread)
						break;

					// !!!
					// треба додати перехват помилки
					//

					if (doc.GetType().GetMember("SpendTheDocument").Length == 1)
					{
						try
						{
							doc.GetType().InvokeMember("SpendTheDocument", BindingFlags.InvokeMethod, null , doc,
								new object[] { journalSelect.Current.SpendDate });
						}
                        catch 
                        {
                            ApendLine("Помилка: ");
                        }
                    }
				}
			}

			ApendLine("Готово!");

			CalculationBalancesAll_Func();

			Константи.Системні.ВвімкнутиФоновіЗадачі_Const = true;

			buttonSpendAll.Invoke(new Action(() => buttonSpendAll.Enabled = true));
			buttonCancel.Invoke(new Action(() => buttonCancel.Enabled = false));
			button_CalculationBalancesAll.Invoke(new Action(() => button_CalculationBalancesAll.Enabled = true));
		}

        private void button1_Click(object sender, EventArgs e)
        {
			//Journal.Journal_Select journalSelect = new Journal.Journal_Select();
			//journalSelect.Select();

			//while (journalSelect.MoveNext())
			//{
			//	if (Cancel)
			//		return;

			//		ApendLine(journalSelect.Current.TypeDocument + " " + journalSelect.Current.SpendDate);

			//		DocumentObject doc = journalSelect.GetDocumentObject(true);

			//		PropertyInfo property_Назва = doc.GetType().GetProperty("Назва");
			//		PropertyInfo property_ДатаДок = doc.GetType().GetProperty("ДатаДок");
			//		PropertyInfo property_НомерДок = doc.GetType().GetProperty("НомерДок");

			//		PropertyInfo property_Назва2 = doc.GetType().GetProperty("Назва2");
			//		PropertyInfo property_ДатаДок2 = doc.GetType().GetProperty("ДатаДок2");
			//		PropertyInfo property_НомерДок2 = doc.GetType().GetProperty("НомерДок2");

			//		property_Назва2.SetValue(doc, property_Назва.GetValue(doc));
			//		property_ДатаДок2.SetValue(doc, property_ДатаДок.GetValue(doc));
			//		property_НомерДок2.SetValue(doc, property_НомерДок.GetValue(doc));

			//		if (doc.GetType().GetMember("Save").Length == 1)
			//			doc.GetType().InvokeMember(
			//				"Save", BindingFlags.InvokeMethod, null, doc, new object[] { });
			//}

			//ApendLine("Готово!");

			//buttonSpendAll.Invoke(new Action(() => buttonSpendAll.Enabled = true));
			//buttonCancel.Invoke(new Action(() => buttonCancel.Enabled = false));
		}

        private void FormService_Load(object sender, EventArgs e)
        {

        }

		private void FormService_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (thread == null || thread.ThreadState == ThreadState.Stopped)
				return;

			buttonCancel_Click(this, new EventArgs());

			Thread.Sleep(3000);

			thread.Abort();
		}

        
    }
}

//private void CalculateBalance_ЗамовленняКлієнтів()
//{
//	const string registr_name = "ЗамовленняКлієнтів";

//	ApendLine($"[ {registr_name} ] ", "");
//	ApendLine("", " -> очистка ");

//	lock (lockobject)
//	{
//		if (cancel) thread.Abort();
//		//CalculateBalancesInRegister_ЗамовленняКлієнтів.ВидалитиЗалишки();
//	}

//	List<DateTime> ListMonth;

//	lock (lockobject)
//	{
//		if (cancel) thread.Abort();
//		ListMonth = null;// CalculateBalancesInRegister_ЗамовленняКлієнтів.ОтриматиСписокМісяців();
//	}

//	if (ListMonth.Count > 0)
//	{
//		ApendLine("", " -> розрахунок ");

//		foreach (DateTime listMonthItem in ListMonth)
//		{
//			ApendLine("", " --> " + listMonthItem.ToString("dd.MM.yyyy"));

//			lock (lockobject)
//			{
//				if (cancel) thread.Abort();
//				//CalculateBalancesInRegister_ЗамовленняКлієнтів.ОбчислитиЗалишкиЗаМісяць(listMonthItem);
//			}
//		}
//	}
//}

//private void CalculateBalance_ТовариНаСкладах()
//{
//	const string registr_name = "ТовариНаСкладах";

//	ApendLine($"[ {registr_name} ] ", "");
//	ApendLine("", " -> очистка ");

//	lock (lockobject)
//	{
//		if (cancel) thread.Abort();
//		//CalculateBalancesInRegister_ТовариНаСкладах.ВидалитиЗалишки();
//	}

//	List<DateTime> ListMonth;

//	lock (lockobject)
//	{
//		if (cancel) thread.Abort();
//		ListMonth = null;// CalculateBalancesInRegister_ТовариНаСкладах.ОтриматиСписокМісяців();
//	}

//	if (ListMonth.Count > 0)
//	{
//		ApendLine("", " -> розрахунок ");

//		foreach (DateTime listMonthItem in ListMonth)
//		{
//			ApendLine("", " --> " + listMonthItem.ToString("dd.MM.yyyy"));

//			lock (lockobject)
//			{
//				if (cancel) thread.Abort();
//				//CalculateBalancesInRegister_ТовариНаСкладах.ОбчислитиЗалишкиЗаМісяць(listMonthItem);
//			}
//		}
//	}
//}

//private void CalculateBalance_РозрахункиЗКлієнтами()
//{
//	const string registr_name = "РозрахункиЗКлієнтами";

//	ApendLine($"[ {registr_name} ] ", "");
//	ApendLine("", " -> очистка ");

//	lock (lockobject)
//	{
//		if (cancel) thread.Abort();
//		//CalculateBalancesInRegister_РозрахункиЗКлієнтами.ВидалитиЗалишки();
//	}

//	List<DateTime> ListMonth;

//	lock (lockobject)
//	{
//		if (cancel) thread.Abort();
//		ListMonth = null;// CalculateBalancesInRegister_РозрахункиЗКлієнтами.ОтриматиСписокМісяців();
//	}

//	if (ListMonth.Count > 0)
//	{
//		ApendLine("", " -> розрахунок ");

//		foreach (DateTime listMonthItem in ListMonth)
//		{
//			ApendLine("", " --> " + listMonthItem.ToString("dd.MM.yyyy"));

//			lock (lockobject)
//			{
//				if (cancel) thread.Abort();
//				//CalculateBalancesInRegister_РозрахункиЗКлієнтами.ОбчислитиЗалишкиЗаМісяць(listMonthItem);
//			}
//		}
//	}
//}

//private void CalculateBalance_РозрахункиЗПостачальниками()
//{
//	const string registr_name = "РозрахункиЗПостачальниками";

//	ApendLine($"[ {registr_name} ] ", "");
//	ApendLine("", " -> очистка ");

//	lock (lockobject)
//	{
//		if (cancel) thread.Abort();
//		//CalculateBalancesInRegister_РозрахункиЗПостачальниками.ВидалитиЗалишки();
//	}

//	List<DateTime> ListMonth;

//	lock (lockobject)
//	{
//		if (cancel) thread.Abort();
//		ListMonth = null; // CalculateBalancesInRegister_РозрахункиЗПостачальниками.ОтриматиСписокМісяців();
//	}

//	if (ListMonth.Count > 0)
//	{
//		ApendLine("", " -> розрахунок ");

//		foreach (DateTime listMonthItem in ListMonth)
//		{
//			ApendLine("", " --> " + listMonthItem.ToString("dd.MM.yyyy"));

//			lock (lockobject)
//			{
//				if (cancel) thread.Abort();
//				//CalculateBalancesInRegister_РозрахункиЗПостачальниками.ОбчислитиЗалишкиЗаМісяць(listMonthItem);
//			}
//		}
//	}
//}

//private void CalculateBalance_ЗамовленняПостачальникам()
//{
//	const string registr_name = "ЗамовленняПостачальникам";

//	ApendLine($"[ {registr_name} ] ", "");
//	ApendLine("", " -> очистка ");

//	lock (lockobject)
//	{
//		if (cancel) thread.Abort();
//		//CalculateBalancesInRegister_ЗамовленняПостачальникам.ВидалитиЗалишки();
//	}

//	List<DateTime> ListMonth;

//	lock (lockobject)
//	{
//		if (cancel) thread.Abort();
//		ListMonth = null; // CalculateBalancesInRegister_ЗамовленняПостачальникам.ОтриматиСписокМісяців();
//	}

//	if (ListMonth.Count > 0)
//	{
//		ApendLine("", " -> розрахунок ");

//		foreach (DateTime listMonthItem in ListMonth)
//		{
//			ApendLine("", " --> " + listMonthItem.ToString("dd.MM.yyyy"));

//			lock (lockobject)
//			{
//				if (cancel) thread.Abort();
//				//CalculateBalancesInRegister_ЗамовленняПостачальникам.ОбчислитиЗалишкиЗаМісяць(listMonthItem);
//			}
//		}
//	}
//}

//private void CalculateBalance_ВільніЗалишки()
//{
//	const string registr_name = "ВільніЗалишки";

//	ApendLine($"[ {registr_name} ] ", "");
//	ApendLine("", " -> очистка ");

//	lock (lockobject)
//	{
//		if (cancel) thread.Abort();
//		//CalculateBalancesInRegister_ВільніЗалишки.ВидалитиЗалишки();
//	}

//	List<DateTime> ListMonth;

//	lock (lockobject)
//	{
//		if (cancel) thread.Abort();
//		ListMonth = null; // CalculateBalancesInRegister_ВільніЗалишки.ОтриматиСписокМісяців();
//	}

//	if (ListMonth.Count > 0)
//	{
//		ApendLine("", " -> розрахунок ");

//		foreach (DateTime listMonthItem in ListMonth)
//		{
//			ApendLine("", " --> " + listMonthItem.ToString("dd.MM.yyyy"));

//			lock (lockobject)
//			{
//				if (cancel) thread.Abort();
//				//CalculateBalancesInRegister_ВільніЗалишки.ОбчислитиЗалишкиЗаМісяць(listMonthItem);
//			}
//		}
//	}
//}

//private void StartThreadCalculateBalance()
//      {
//	CalculateBalance_ЗамовленняКлієнтів();
//	CalculateBalance_ТовариНаСкладах();
//	CalculateBalance_РозрахункиЗКлієнтами();
//	CalculateBalance_РозрахункиЗПостачальниками();
//	CalculateBalance_ЗамовленняПостачальникам();
//	CalculateBalance_ВільніЗалишки();

//	buttonCalculate.Invoke(new Action(() => buttonCalculate.Enabled = true));
//	buttonCancel.Invoke(new Action(() => buttonCancel.Enabled = false));
//}