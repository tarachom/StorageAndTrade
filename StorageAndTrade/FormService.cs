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
					richTextBoxInfo.SelectionFont = new Font("Consolas"/*"Microsoft Sans Serif"*/, 10, FontStyle.Bold);
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

		private void StartThreadCalculateBalance_ЗамовленняКлієнтів()
		{
			const string registr_name = "ЗамовленняКлієнтів";

			List<DateTime> ListMonth;

			lock (lockobject)
			{
				ListMonth = CalculateBalancesInRegister_ЗамовленняКлієнтів.ОтриматиСписокМісяців();
			}

			foreach (DateTime listMonthItem in ListMonth)
			{
				ApendLine($"[ {registr_name} ] ", " -> " + listMonthItem.ToString("dd.MM.yyyy"));

				lock (lockobject)
				{
					CalculateBalancesInRegister_ЗамовленняКлієнтів.ОбчислитиЗалишкиЗаМісяць(listMonthItem);
				}
			}
		}

		private void StartThreadCalculateBalance_ТовариНаСкладах()
		{
			const string registr_name = "ТовариНаСкладах";

			List<DateTime> ListMonth;

			lock (lockobject)
			{
				ListMonth = CalculateBalancesInRegister_ТовариНаСкладах.ОтриматиСписокМісяців();
			}

			foreach (DateTime listMonthItem in ListMonth)
			{
				ApendLine($"[ {registr_name} ] ", " -> " + listMonthItem.ToString("dd.MM.yyyy"));

				lock (lockobject)
				{
					CalculateBalancesInRegister_ТовариНаСкладах.ОбчислитиЗалишкиЗаМісяць(listMonthItem);
				}
			}
		}

		private void StartThreadCalculateBalance_РозрахункиЗКлієнтами()
		{
			const string registr_name = "РозрахункиЗКлієнтами";

			List<DateTime> ListMonth;

			lock (lockobject)
			{
				ListMonth = CalculateBalancesInRegister_РозрахункиЗКлієнтами.ОтриматиСписокМісяців();
			}

			foreach (DateTime listMonthItem in ListMonth)
			{
				ApendLine($"[ {registr_name} ] ", " -> " + listMonthItem.ToString("dd.MM.yyyy"));

				lock (lockobject)
				{
					CalculateBalancesInRegister_РозрахункиЗКлієнтами.ОбчислитиЗалишкиЗаМісяць(listMonthItem);
				}
			}
		}

		private void buttonCalculate_Click(object sender, EventArgs e)
        {
			CalculateBalancesInRegister.ПідключитиДодаток_UUID_OSSP();

			Thread thread_ЗамовленняКлієнтів = new Thread(new ThreadStart(StartThreadCalculateBalance_ЗамовленняКлієнтів));
			thread_ЗамовленняКлієнтів.Start();

			Thread thread_ТовариНаСкладах = new Thread(new ThreadStart(StartThreadCalculateBalance_ТовариНаСкладах));
			thread_ТовариНаСкладах.Start();

			Thread thread_РозрахункиЗКлієнтами = new Thread(new ThreadStart(StartThreadCalculateBalance_РозрахункиЗКлієнтами));
			thread_РозрахункиЗКлієнтами.Start();
		}


    }
}
