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
 
Модуль функцій для звітів.
 
*/

using System;
using System.Collections.Generic;
using AccountingSoftware;

using StorageAndTrade_1_0.Документи;

namespace StorageAndTrade_1_0.Звіти
{
    class ЗамовленняКлієнта_Report
    {
        public static void PrintRegisterRecords(ЗамовленняКлієнта_Pointer ДокументВказівник)
        {
            //
            //ЗамовленняКлієнтів
            //

            РегістриНакопичення.ЗамовленняКлієнтів_RecordsSet замовленняКлієнтів_RecordsSet = new РегістриНакопичення.ЗамовленняКлієнтів_RecordsSet();
            замовленняКлієнтів_RecordsSet.Filter.ЗамовленняКлієнта = ДокументВказівник;

            замовленняКлієнтів_RecordsSet.Read();

            foreach(РегістриНакопичення.ЗамовленняКлієнтів_RecordsSet.Record record in замовленняКлієнтів_RecordsSet.Records)
            {
                Console.WriteLine(record.UID);

            }
            
        }
    }
}
