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

/*
 *
 * Конфігурації "Домашні фінанси 1.0"
 * Автор Тарахомин Юрій Іванович, Україна, м. Львів, accounting.org.ua, tarachom@gmail.com
 * Дата конфігурації: 03.09.2021 16:50:57
 *
 */

using System;
using System.Collections.Generic;
using AccountingSoftware;

namespace StorageAndTrade_1_0.Довідники
{
 //   class Записи_Triggers
	//{
 //       public static void Записи_BeforeRecording(Записи_Objest запис)
 //       {
 //           //Console.WriteLine("BeforeRecording: " + запис.Назва);
	//	}

 //       public static void Записи_AfterRecording(Записи_Objest запис)
 //       {
 //           //Console.WriteLine("AfterRecording: " + запис.Назва);

			
	//	}

	//	public static void Записи_BeforeDelete(Записи_Objest запис)
	//	{
	//		//Console.WriteLine("BeforeDelete: " + запис.Назва);

	//	}
	//}
}

namespace StorageAndTrade_1_0.Документи
{
	class Записи_Triggers
    {
		public static void ЗамовленняКлієнта_BeforeRecording(ЗамовленняКлієнта_Objest запис)
        {

        }

		public static void ЗамовленняКлієнта_AfterRecording(ЗамовленняКлієнта_Objest запис)
		{

		}

		public static void ЗамовленняКлієнта_BeforeDelete(ЗамовленняКлієнта_Objest запис)
		{

		}
	}
}