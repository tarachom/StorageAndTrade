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

Модуль сервісних функцій.

*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AccountingSoftware;
using StorageAndTrade_1_0.Константи;
using StorageAndTrade_1_0.РегістриНакопичення;

namespace StorageAndTrade_1_0.Service
{ 
    class CalculateBalancesInRegister
    {
        /// <summary>
        /// Список місяців для яких є рухи по регістру
        /// </summary>
        /// <returns>Список місяців типу 01.05.2022 00:00:00</returns>
        public static List<DateTime> ОтриматиСписокМісяців()
        {
            string query = $@"
SELECT
    date_trunc('month', Рег_ЗамовленняКлієнтів.period) as period_month
FROM 
    {ЗамовленняКлієнтів_Const.TABLE} AS Рег_ЗамовленняКлієнтів
GROUP BY period_month
";

            string[] columnsName;
            List<object[]> listRow;

            Config.Kernel.DataBase.SelectRequest(query, null, out columnsName, out listRow);

            List<DateTime> listRowDateTime = new List<DateTime>();
            foreach (object[] dateTimeObject in listRow)
                listRowDateTime.Add((DateTime)dateTimeObject[0]);

            return listRowDateTime;
        }

        private static void ВидалитиЗалишкиЗаПеріод(DateTime month)
        {
            string query = $@"
DELETE 
    FROM {ВіртуальніТаблиціРегістрівНакопичення.ЗамовленняКлієнтів_Місяць_TablePart.TABLE}
WHERE 
    {ВіртуальніТаблиціРегістрівНакопичення.ЗамовленняКлієнтів_Місяць_TablePart.Період} = '{month}'
";
            Console.WriteLine(query);

            Config.Kernel.DataBase.ExecuteSQL(query);
        }

        private static void ПідключитиДодаток_UUID_OSSP()
        {
            string query = "CREATE EXTENSION IF NOT EXISTS \"uuid-ossp\"";

            Console.WriteLine(query);

            Config.Kernel.DataBase.ExecuteSQL(query);
        }

        /// <summary>
        /// Обчислити залишки за період
        /// </summary>
        /// <param name="month">Дата типу 01.05.2022 00:00:00</param>
        public static void ОбчислитиЗалишкиЗаПеріод(DateTime month)
        {
            ВидалитиЗалишкиЗаПеріод(month);

            ПідключитиДодаток_UUID_OSSP();

            //Заповнення таблиці
            string query = $@"
INSERT INTO {ВіртуальніТаблиціРегістрівНакопичення.ЗамовленняКлієнтів_Місяць_TablePart.TABLE}
(
    uid,
    {ВіртуальніТаблиціРегістрівНакопичення.ЗамовленняКлієнтів_Місяць_TablePart.Період},
    {ВіртуальніТаблиціРегістрівНакопичення.ЗамовленняКлієнтів_Місяць_TablePart.Номенклатура},
    {ВіртуальніТаблиціРегістрівНакопичення.ЗамовленняКлієнтів_Місяць_TablePart.ХарактеристикиНоменклатури},
    {ВіртуальніТаблиціРегістрівНакопичення.ЗамовленняКлієнтів_Місяць_TablePart.Склад},
    {ВіртуальніТаблиціРегістрівНакопичення.ЗамовленняКлієнтів_Місяць_TablePart.Замовлено},
    {ВіртуальніТаблиціРегістрівНакопичення.ЗамовленняКлієнтів_Місяць_TablePart.Сума}
)
SELECT 
    uuid_generate_v4(),
    date_trunc('month', Рег_ЗамовленняКлієнтів.period) as period_month,
    Рег_ЗамовленняКлієнтів.{ЗамовленняКлієнтів_Const.Номенклатура} AS Номенклатура, 
    Рег_ЗамовленняКлієнтів.{ЗамовленняКлієнтів_Const.ХарактеристикаНоменклатури} AS ХарактеристикаНоменклатури,
    Рег_ЗамовленняКлієнтів.{ЗамовленняКлієнтів_Const.Склад} AS Склад,

    SUM(CASE WHEN Рег_ЗамовленняКлієнтів.income = true THEN 
        Рег_ЗамовленняКлієнтів.{ЗамовленняКлієнтів_Const.Замовлено} ELSE 
       -Рег_ЗамовленняКлієнтів.{ЗамовленняКлієнтів_Const.Замовлено} END) AS Замовлено,

    SUM(CASE WHEN Рег_ЗамовленняКлієнтів.income = true THEN 
        Рег_ЗамовленняКлієнтів.{ЗамовленняКлієнтів_Const.Сума} ELSE 
       -Рег_ЗамовленняКлієнтів.{ЗамовленняКлієнтів_Const.Сума} END) AS Сума
FROM 
    {ЗамовленняКлієнтів_Const.TABLE} AS Рег_ЗамовленняКлієнтів
WHERE
    date_trunc('month', Рег_ЗамовленняКлієнтів.period) = '{month}'
GROUP BY 
    period_month, Номенклатура, ХарактеристикаНоменклатури, Склад
HAVING 

   SUM(CASE WHEN Рег_ЗамовленняКлієнтів.income = true THEN 
        Рег_ЗамовленняКлієнтів.{ЗамовленняКлієнтів_Const.Замовлено} ELSE 
       -Рег_ЗамовленняКлієнтів.{ЗамовленняКлієнтів_Const.Замовлено} END) != 0
OR

    SUM(CASE WHEN Рег_ЗамовленняКлієнтів.income = true THEN 
        Рег_ЗамовленняКлієнтів.{ЗамовленняКлієнтів_Const.Сума} ELSE 
       -Рег_ЗамовленняКлієнтів.{ЗамовленняКлієнтів_Const.Сума} END) != 0
";
            Console.WriteLine(query);
            Config.Kernel.DataBase.ExecuteSQL(query);
        }

    }
}


/*
--CREATE EXTENSION IF NOT EXISTS "uuid-ossp";
--SELECT uuid_generate_v4();

--CREATE EXTENSION IF NOT EXISTS "pgcrypto";
--SELECT gen_random_uuid(), uuid_generate_v4(); 
*/