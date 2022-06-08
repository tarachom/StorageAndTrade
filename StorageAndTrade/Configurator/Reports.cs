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
 
Модуль функцій для звітів

*/

using System;
using System.Collections.Generic;
using AccountingSoftware;

using StorageAndTrade_1_0;
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

            Console.WriteLine("Док: " + ДокументВказівник.UnigueID.ToString());

            РегістриНакопичення.ЗамовленняКлієнтів_RecordsSet замовленняКлієнтів_RecordsSet = new РегістриНакопичення.ЗамовленняКлієнтів_RecordsSet();
            замовленняКлієнтів_RecordsSet.Filter.ЗамовленняКлієнта = ДокументВказівник;

            замовленняКлієнтів_RecordsSet.Read();

            foreach(РегістриНакопичення.ЗамовленняКлієнтів_RecordsSet.Record record in замовленняКлієнтів_RecordsSet.Records)
            {
                Console.WriteLine(record.UID + " " + record.Номенклатура.GetPresentation());
                 
            }

            Configuration Conf = Config.Kernel.Conf;

            ConfigurationRegistersAccumulation Регістр_ЗамовленняКлієнтів = Conf.RegistersAccumulation["ЗамовленняКлієнтів"];
            ConfigurationDirectories Довідник_Номенклатура = Conf.Directories["Номенклатура"];
            ConfigurationDirectories Довідник_ХарактеристикиНоменклатури = Conf.Directories["ХарактеристикиНоменклатури"];
            ConfigurationDirectories Довідник_Склади = Conf.Directories["Склади"];

            string query = $@"
SELECT 
    Рег_ЗамовленняКлієнтів.period,
    Рег_ЗамовленняКлієнтів.income,
    Рег_ЗамовленняКлієнтів.{Регістр_ЗамовленняКлієнтів.DimensionFields["ЗамовленняКлієнта"].NameInTable} AS ЗамовленняКлієнта, 
    Рег_ЗамовленняКлієнтів.{Регістр_ЗамовленняКлієнтів.DimensionFields["Номенклатура"].NameInTable} AS Номенклатура, 
    Довідник_Номенклатура.{Довідник_Номенклатура.Fields["Назва"].NameInTable} AS Номенклатура_Назва, 
    Рег_ЗамовленняКлієнтів.{Регістр_ЗамовленняКлієнтів.DimensionFields["ХарактеристикаНоменклатури"].NameInTable} AS ХарактеристикаНоменклатури,
    Довідник_ХарактеристикиНоменклатури.{Довідник_ХарактеристикиНоменклатури.Fields["Назва"].NameInTable} AS ХарактеристикаНоменклатури_Назва, 
    Рег_ЗамовленняКлієнтів.{Регістр_ЗамовленняКлієнтів.DimensionFields["Склад"].NameInTable} AS Склад,
    Довідник_Склади.{Довідник_Склади.Fields["Назва"].NameInTable} AS Склад_Назва,
    Рег_ЗамовленняКлієнтів.{Регістр_ЗамовленняКлієнтів.ResourcesFields["Замовлено"].NameInTable} AS Замовлено,
    Рег_ЗамовленняКлієнтів.{Регістр_ЗамовленняКлієнтів.ResourcesFields["Сума"].NameInTable} AS Сума
FROM 
    {Регістр_ЗамовленняКлієнтів.Table} AS Рег_ЗамовленняКлієнтів
    LEFT JOIN {Довідник_Номенклатура.Table} AS Довідник_Номенклатура ON Довідник_Номенклатура.uid = Рег_ЗамовленняКлієнтів.{Регістр_ЗамовленняКлієнтів.DimensionFields["Номенклатура"].NameInTable}
    LEFT JOIN {Довідник_ХарактеристикиНоменклатури.Table} AS Довідник_ХарактеристикиНоменклатури ON Довідник_ХарактеристикиНоменклатури.uid = Рег_ЗамовленняКлієнтів.{Регістр_ЗамовленняКлієнтів.DimensionFields["ХарактеристикаНоменклатури"].NameInTable}
    LEFT JOIN {Довідник_Склади.Table} AS Довідник_Склади ON Довідник_Склади.uid = Рег_ЗамовленняКлієнтів.{Регістр_ЗамовленняКлієнтів.DimensionFields["Склад"].NameInTable}
WHERE
    Рег_ЗамовленняКлієнтів.Owner = @ЗамовленняКлієнта
ORDER BY 
";

            Console.WriteLine(query);

            Dictionary<string, object> paramQuery = new Dictionary<string, object>();
            paramQuery.Add("ЗамовленняКлієнта", ДокументВказівник.UnigueID.UGuid);

            string[] columnsName;
            List<object[]> listRow;

            Config.Kernel.DataBase.SelectRequest(query, paramQuery, out columnsName, out listRow);

            string result = "";

			foreach(object[] o in listRow)
            {
				result += o[2].ToString() + "\n\r";
			}

            Console.WriteLine(result);
        }
    }
}

