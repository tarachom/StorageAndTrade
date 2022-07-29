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
using Journal = StorageAndTrade_1_0.Journal ;
using System.Reflection;

namespace StorageAndTrade
{
	public partial class FormService : Form
	{
		public FormService()
		{
			InitializeComponent();
		}

		private object lockobject = new object();
		private bool Cancel = false;
		private Thread thread;

		private void ApendLine(string head)
		{
			if (richTextBoxInfo.InvokeRequired)
			{
				richTextBoxInfo.Invoke(new Action<string>(ApendLine), head);
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

			Cancel = true;
		}

        private void buttonSpendAll_Click(object sender, EventArgs e)
        {
			Cancel = false;

			buttonSpendAll.Enabled = false;
			buttonCancel.Enabled = true;

			thread = new Thread(new ThreadStart(SpendAllDocument));
			thread.Start();
		}

		void SpendAllDocument()
		{
			//Константи.Системні.ВвімкнутиФоновіЗадачі_Const = false;

			//foreach (ConfigurationDocuments документи in Конфа.Config.Kernel.Conf.Documents.Values)
			//{
			//	//Якщо документ робить рухи по регістрах накопичення
			//	if(документи.AllowRegisterAccumulation.Count > 0)
   //             {
			//		//Journal.Journal_Document journal_Document = new Journal.Journal_Document(документи.Name);
			//		//Документи.ПсуванняТоварів_Objest а = (Документи.ПсуванняТоварів_Objest)journal_Document.GetDocumentObject();
			//		//а.Save();

			//		//DocumentObject documentObject = 
			//	}
			//}

			Journal.Journal_Select journalSelect = new Journal.Journal_Select();
			journalSelect.Select();

            while (journalSelect.MoveNext())
            {
				ApendLine(journalSelect.Current.UnigueID.ToString() + " " + journalSelect.Current.TypeDocument);

				//DocumentObject doc = journalSelect.GetDocumentObject();
				
				ApendLine(journalSelect.Current.Spend + "; " + journalSelect.Current.SpendDate);

				//if (doc.GetType().GetMember("GetPresentation").Length == 1)
				//	ApendLine(doc.GetType().InvokeMember(
				//		"GetPresentation", BindingFlags.InvokeMethod, null, doc, new object[] { }).ToString());

				//switch (journalSelect.Current.TypeDocument)
    //            {
				//	case "ПоступленняТоварівТаПослуг":
    //                    {
				//			Документи.ПоступленняТоварівТаПослуг_Objest documen = 
				//				(Документи.ПоступленняТоварівТаПослуг_Objest)journalSelect.GetDocumentObject();

				//			ApendLine(documen.Назва);

				//			break;
				//		}
    //            }
			}

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