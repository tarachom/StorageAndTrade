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

using StorageAndTrade_1_0.Service;

namespace StorageAndTrade
{
	public partial class FormService : Form
	{
		public FormService()
		{
			InitializeComponent();
		}

		private object lockobject = new object();
		private bool cancel = false;
		private Thread thread;

		private void ApendLine(string head, string bodySelect, string futer = "")
		{
			if (richTextBoxInfo.InvokeRequired)
			{
				richTextBoxInfo.Invoke(new Action<string, string, string>(ApendLine), head, bodySelect, futer);
			}
			else
			{
				richTextBoxInfo.AppendText(head);

				if (!String.IsNullOrEmpty(bodySelect))
				{
					richTextBoxInfo.SelectionFont = new Font("Consolas"/*"Microsoft Sans Serif"*/, 10);
					richTextBoxInfo.SelectionColor = Color.DarkBlue;
					richTextBoxInfo.AppendText(bodySelect);
				}

				if (!String.IsNullOrEmpty(bodySelect))
				{
					richTextBoxInfo.SelectionFont = new Font("Consolas", 10);
					richTextBoxInfo.SelectionColor = Color.Black;
				}

				richTextBoxInfo.AppendText(" " + futer + "\n");
				richTextBoxInfo.ScrollToCaret();
			}
		}

		private void CalculateBalance_ЗамовленняКлієнтів()
		{
			const string registr_name = "ЗамовленняКлієнтів";

			ApendLine($"[ {registr_name} ] ", "");
			ApendLine("", " -> очистка ");

			lock (lockobject)
			{
				if (cancel) thread.Abort();
				CalculateBalancesInRegister_ЗамовленняКлієнтів.ВидалитиЗалишки();
			}

			List<DateTime> ListMonth;

			lock (lockobject)
			{
				if (cancel) thread.Abort();
				ListMonth = CalculateBalancesInRegister_ЗамовленняКлієнтів.ОтриматиСписокМісяців();
			}

			if (ListMonth.Count > 0)
			{
				ApendLine("", " -> розрахунок ");

				foreach (DateTime listMonthItem in ListMonth)
				{
					ApendLine("", " --> " + listMonthItem.ToString("dd.MM.yyyy"));

					lock (lockobject)
					{
						if (cancel) thread.Abort();
						CalculateBalancesInRegister_ЗамовленняКлієнтів.ОбчислитиЗалишкиЗаМісяць(listMonthItem);
					}
				}
			}
		}

		private void CalculateBalance_ТовариНаСкладах()
		{
			const string registr_name = "ТовариНаСкладах";

			ApendLine($"[ {registr_name} ] ", "");
			ApendLine("", " -> очистка ");

			lock (lockobject)
			{
				if (cancel) thread.Abort();
				CalculateBalancesInRegister_ТовариНаСкладах.ВидалитиЗалишки();
			}

			List<DateTime> ListMonth;

			lock (lockobject)
			{
				if (cancel) thread.Abort();
				ListMonth = CalculateBalancesInRegister_ТовариНаСкладах.ОтриматиСписокМісяців();
			}

			if (ListMonth.Count > 0)
			{
				ApendLine("", " -> розрахунок ");

				foreach (DateTime listMonthItem in ListMonth)
				{
					ApendLine("", " --> " + listMonthItem.ToString("dd.MM.yyyy"));

					lock (lockobject)
					{
						if (cancel) thread.Abort();
						CalculateBalancesInRegister_ТовариНаСкладах.ОбчислитиЗалишкиЗаМісяць(listMonthItem);
					}
				}
			}
		}

		private void CalculateBalance_РозрахункиЗКлієнтами()
		{
			const string registr_name = "РозрахункиЗКлієнтами";

			ApendLine($"[ {registr_name} ] ", "");
			ApendLine("", " -> очистка ");

			lock (lockobject)
			{
				if (cancel) thread.Abort();
				CalculateBalancesInRegister_РозрахункиЗКлієнтами.ВидалитиЗалишки();
			}

			List<DateTime> ListMonth;

			lock (lockobject)
			{
				if (cancel) thread.Abort();
				ListMonth = CalculateBalancesInRegister_РозрахункиЗКлієнтами.ОтриматиСписокМісяців();
			}

			if (ListMonth.Count > 0)
			{
				ApendLine("", " -> розрахунок ");

				foreach (DateTime listMonthItem in ListMonth)
				{
					ApendLine("", " --> " + listMonthItem.ToString("dd.MM.yyyy"));

					lock (lockobject)
					{
						if (cancel) thread.Abort();
						CalculateBalancesInRegister_РозрахункиЗКлієнтами.ОбчислитиЗалишкиЗаМісяць(listMonthItem);
					}
				}
			}
		}

		private void CalculateBalance_РозрахункиЗПостачальниками()
		{
			const string registr_name = "РозрахункиЗПостачальниками";

			ApendLine($"[ {registr_name} ] ", "");
			ApendLine("", " -> очистка ");

			lock (lockobject)
			{
				if (cancel) thread.Abort();
				CalculateBalancesInRegister_РозрахункиЗПостачальниками.ВидалитиЗалишки();
			}

			List<DateTime> ListMonth;

			lock (lockobject)
			{
				if (cancel) thread.Abort();
				ListMonth = CalculateBalancesInRegister_РозрахункиЗПостачальниками.ОтриматиСписокМісяців();
			}

			if (ListMonth.Count > 0)
			{
				ApendLine("", " -> розрахунок ");

				foreach (DateTime listMonthItem in ListMonth)
				{
					ApendLine("", " --> " + listMonthItem.ToString("dd.MM.yyyy"));

					lock (lockobject)
					{
						if (cancel) thread.Abort();
						CalculateBalancesInRegister_РозрахункиЗПостачальниками.ОбчислитиЗалишкиЗаМісяць(listMonthItem);
					}
				}
			}
		}

		private void CalculateBalance_ЗамовленняПостачальникам()
		{
			const string registr_name = "ЗамовленняПостачальникам";

			ApendLine($"[ {registr_name} ] ", "");
			ApendLine("", " -> очистка ");

			lock (lockobject)
			{
				if (cancel) thread.Abort();
				CalculateBalancesInRegister_ЗамовленняПостачальникам.ВидалитиЗалишки();
			}

			List<DateTime> ListMonth;

			lock (lockobject)
			{
				if (cancel) thread.Abort();
				ListMonth = CalculateBalancesInRegister_ЗамовленняПостачальникам.ОтриматиСписокМісяців();
			}

			if (ListMonth.Count > 0)
			{
				ApendLine("", " -> розрахунок ");

				foreach (DateTime listMonthItem in ListMonth)
				{
					ApendLine("", " --> " + listMonthItem.ToString("dd.MM.yyyy"));

					lock (lockobject)
					{
						if (cancel) thread.Abort();
						CalculateBalancesInRegister_ЗамовленняПостачальникам.ОбчислитиЗалишкиЗаМісяць(listMonthItem);
					}
				}
			}
		}

		private void CalculateBalance_ВільніЗалишки()
		{
			const string registr_name = "ВільніЗалишки";

			ApendLine($"[ {registr_name} ] ", "");
			ApendLine("", " -> очистка ");

			lock (lockobject)
			{
				if (cancel) thread.Abort();
				CalculateBalancesInRegister_ВільніЗалишки.ВидалитиЗалишки();
			}

			List<DateTime> ListMonth;

			lock (lockobject)
			{
				if (cancel) thread.Abort();
				ListMonth = CalculateBalancesInRegister_ВільніЗалишки.ОтриматиСписокМісяців();
			}

			if (ListMonth.Count > 0)
			{
				ApendLine("", " -> розрахунок ");

				foreach (DateTime listMonthItem in ListMonth)
				{
					ApendLine("", " --> " + listMonthItem.ToString("dd.MM.yyyy"));

					lock (lockobject)
					{
						if (cancel) thread.Abort();
						CalculateBalancesInRegister_ВільніЗалишки.ОбчислитиЗалишкиЗаМісяць(listMonthItem);
					}
				}
			}
		}

		private void StartThreadCalculateBalance()
        {
			CalculateBalance_ЗамовленняКлієнтів();
			CalculateBalance_ТовариНаСкладах();
			CalculateBalance_РозрахункиЗКлієнтами();
			CalculateBalance_РозрахункиЗПостачальниками();
			CalculateBalance_ЗамовленняПостачальникам();
			CalculateBalance_ВільніЗалишки();

			buttonCalculate.Invoke(new Action(() => buttonCalculate.Enabled = true));
			buttonCancel.Invoke(new Action(() => buttonCancel.Enabled = false));
		}

		private void buttonCalculate_Click(object sender, EventArgs e)
        {
			cancel = false; 

			buttonCalculate.Enabled = false;
			buttonCancel.Enabled = true;

			CalculateBalancesInRegister.ПідключитиДодаток_UUID_OSSP();

			thread = new Thread(new ThreadStart(StartThreadCalculateBalance));
			thread.Start();
		}

        private void buttonCancel_Click(object sender, EventArgs e)
        {
			buttonCalculate.Enabled = true;
			buttonCancel.Enabled = false;
			
			cancel = true;
		}
    }
}
