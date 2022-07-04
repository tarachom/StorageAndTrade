﻿/*
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

Модуль розрахунку віртуальних залишків.
Віртуальні залишки - це згруповані залишки за певний період (рік, місяць, день)

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
        public static void ПідключитиДодаток_UUID_OSSP()
        {
            string query = "CREATE EXTENSION IF NOT EXISTS \"uuid-ossp\"";
            Config.Kernel.DataBase.ExecuteSQL(query);
        }
    }

    class CalculateBalancesInRegister_ЗамовленняКлієнтів
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
ORDER BY period_month
";

            string[] columnsName;
            List<object[]> listRow;

            Config.Kernel.DataBase.SelectRequest(query, null, out columnsName, out listRow);

            List<DateTime> listRowDateTime = new List<DateTime>();
            foreach (object[] dateTimeObject in listRow)
                listRowDateTime.Add((DateTime)dateTimeObject[0]);

            return listRowDateTime;
        }

        /// <summary>
        /// Очистка віртуальної таблиці
        /// </summary>
        public static void ВидалитиЗалишки()
        {
            string query = $@"DELETE FROM {ВіртуальніТаблиціРегістрів.ЗамовленняКлієнтів_Місяць_TablePart.TABLE}";
            Config.Kernel.DataBase.ExecuteSQL(query);

            query = $@"VACUUM (FULL) {ВіртуальніТаблиціРегістрів.ЗамовленняКлієнтів_Місяць_TablePart.TABLE}";
            Config.Kernel.DataBase.ExecuteSQL(query);
        }

        /// <summary>
        /// Обчислити залишки за період
        /// </summary>
        /// <param name="month">Дата типу 01.05.2022 00:00:00</param>
        public static void ОбчислитиЗалишкиЗаМісяць(DateTime month)
        {
            //Заповнення таблиці
            string query = $@"
INSERT INTO {ВіртуальніТаблиціРегістрів.ЗамовленняКлієнтів_Місяць_TablePart.TABLE}
(
    uid,
    {ВіртуальніТаблиціРегістрів.ЗамовленняКлієнтів_Місяць_TablePart.Період},
    {ВіртуальніТаблиціРегістрів.ЗамовленняКлієнтів_Місяць_TablePart.Номенклатура},
    {ВіртуальніТаблиціРегістрів.ЗамовленняКлієнтів_Місяць_TablePart.ХарактеристикиНоменклатури},
    {ВіртуальніТаблиціРегістрів.ЗамовленняКлієнтів_Місяць_TablePart.Склад},
    {ВіртуальніТаблиціРегістрів.ЗамовленняКлієнтів_Місяць_TablePart.Замовлено},
    {ВіртуальніТаблиціРегістрів.ЗамовленняКлієнтів_Місяць_TablePart.Сума}
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
            Config.Kernel.DataBase.ExecuteSQL(query);
        }
    }

    class CalculateBalancesInRegister_ТовариНаСкладах
    {
        /// <summary>
        /// Список місяців для яких є рухи по регістру
        /// </summary>
        /// <returns>Список місяців типу 01.05.2022 00:00:00</returns>
        public static List<DateTime> ОтриматиСписокМісяців()
        {
            string query = $@"
SELECT
    date_trunc('month', Рег_ТовариНаСкладах.period) as period_month
FROM 
    {ТовариНаСкладах_Const.TABLE} AS Рег_ТовариНаСкладах
GROUP BY period_month
ORDER BY period_month
";

            string[] columnsName;
            List<object[]> listRow;

            Config.Kernel.DataBase.SelectRequest(query, null, out columnsName, out listRow);

            List<DateTime> listRowDateTime = new List<DateTime>();
            foreach (object[] dateTimeObject in listRow)
                listRowDateTime.Add((DateTime)dateTimeObject[0]);

            return listRowDateTime;
        }

        /// <summary>
        /// Очистка віртуальної таблиці
        /// </summary>
        public static void ВидалитиЗалишки()
        {
            string query = $@"DELETE FROM {ВіртуальніТаблиціРегістрів.ТовариНаСкладах_Місяць_TablePart.TABLE}";
            Config.Kernel.DataBase.ExecuteSQL(query);

            query = $@"VACUUM (VERBOSE) {ВіртуальніТаблиціРегістрів.ТовариНаСкладах_Місяць_TablePart.TABLE}";
            Config.Kernel.DataBase.ExecuteSQL(query);
        }

        /// <summary>
        /// Обчислити залишки за період
        /// </summary>
        /// <param name="month">Дата типу 01.05.2022 00:00:00</param>
        public static void ОбчислитиЗалишкиЗаМісяць(DateTime month)
        {
            //Заповнення таблиці
            string query = $@"
INSERT INTO {ВіртуальніТаблиціРегістрів.ТовариНаСкладах_Місяць_TablePart.TABLE}
(
    uid,
    {ВіртуальніТаблиціРегістрів.ТовариНаСкладах_Місяць_TablePart.Період},
    {ВіртуальніТаблиціРегістрів.ТовариНаСкладах_Місяць_TablePart.Номенклатура},
    {ВіртуальніТаблиціРегістрів.ТовариНаСкладах_Місяць_TablePart.ХарактеристикаНоменклатури},
    {ВіртуальніТаблиціРегістрів.ТовариНаСкладах_Місяць_TablePart.Склад},
    {ВіртуальніТаблиціРегістрів.ТовариНаСкладах_Місяць_TablePart.ВНаявності},
    {ВіртуальніТаблиціРегістрів.ТовариНаСкладах_Місяць_TablePart.ДоВідвантаження}
)
SELECT 
    uuid_generate_v4(),
    date_trunc('month', Рег_ТовариНаСкладах.period) as period_month,
    Рег_ТовариНаСкладах.{ТовариНаСкладах_Const.Номенклатура} AS Номенклатура, 
    Рег_ТовариНаСкладах.{ТовариНаСкладах_Const.ХарактеристикаНоменклатури} AS ХарактеристикаНоменклатури,
    Рег_ТовариНаСкладах.{ТовариНаСкладах_Const.Склад} AS Склад,

    SUM(CASE WHEN Рег_ТовариНаСкладах.income = true THEN 
        Рег_ТовариНаСкладах.{ТовариНаСкладах_Const.ВНаявності} ELSE 
       -Рег_ТовариНаСкладах.{ТовариНаСкладах_Const.ВНаявності} END) AS ВНаявності,

    SUM(CASE WHEN Рег_ТовариНаСкладах.income = true THEN 
        Рег_ТовариНаСкладах.{ТовариНаСкладах_Const.ДоВідвантаження} ELSE 
       -Рег_ТовариНаСкладах.{ТовариНаСкладах_Const.ДоВідвантаження} END) AS ДоВідвантаження
FROM 
    {ТовариНаСкладах_Const.TABLE} AS Рег_ТовариНаСкладах
WHERE
    date_trunc('month', Рег_ТовариНаСкладах.period) = '{month}'
GROUP BY 
    period_month, Номенклатура, ХарактеристикаНоменклатури, Склад
HAVING 

   SUM(CASE WHEN Рег_ТовариНаСкладах.income = true THEN 
        Рег_ТовариНаСкладах.{ТовариНаСкладах_Const.ВНаявності} ELSE 
       -Рег_ТовариНаСкладах.{ТовариНаСкладах_Const.ВНаявності} END) != 0
OR

    SUM(CASE WHEN Рег_ТовариНаСкладах.income = true THEN 
        Рег_ТовариНаСкладах.{ТовариНаСкладах_Const.ДоВідвантаження} ELSE 
       -Рег_ТовариНаСкладах.{ТовариНаСкладах_Const.ДоВідвантаження} END) != 0
";
            Config.Kernel.DataBase.ExecuteSQL(query);
        }
    }

    class CalculateBalancesInRegister_РозрахункиЗКлієнтами
    {
        /// <summary>
        /// Список місяців для яких є рухи по регістру
        /// </summary>
        /// <returns>Список місяців типу 01.05.2022 00:00:00</returns>
        public static List<DateTime> ОтриматиСписокМісяців()
        {
            string query = $@"
SELECT
    date_trunc('month', Рег_РозрахункиЗКлієнтами.period) as period_month
FROM 
    {РозрахункиЗКлієнтами_Const.TABLE} AS Рег_РозрахункиЗКлієнтами
GROUP BY period_month
ORDER BY period_month
";

            string[] columnsName;
            List<object[]> listRow;

            Config.Kernel.DataBase.SelectRequest(query, null, out columnsName, out listRow);

            List<DateTime> listRowDateTime = new List<DateTime>();
            foreach (object[] dateTimeObject in listRow)
                listRowDateTime.Add((DateTime)dateTimeObject[0]);

            return listRowDateTime;
        }

        /// <summary>
        /// Очистка віртуальної таблиці
        /// </summary>
        public static void ВидалитиЗалишки()
        {
            string query = $@"DELETE FROM {ВіртуальніТаблиціРегістрів.РозрахункиЗКлієнтами_Місяць_TablePart.TABLE}";
            Config.Kernel.DataBase.ExecuteSQL(query);

            query = $@"VACUUM (VERBOSE) {ВіртуальніТаблиціРегістрів.РозрахункиЗКлієнтами_Місяць_TablePart.TABLE}";
            Config.Kernel.DataBase.ExecuteSQL(query);
        }

        /// <summary>
        /// Обчислити залишки за період
        /// </summary>
        /// <param name="month">Дата типу 01.05.2022 00:00:00</param>
        public static void ОбчислитиЗалишкиЗаМісяць(DateTime month)
        {
            //Заповнення таблиці
            string query = $@"
INSERT INTO {ВіртуальніТаблиціРегістрів.РозрахункиЗКлієнтами_Місяць_TablePart.TABLE}
(
    uid,
    {ВіртуальніТаблиціРегістрів.РозрахункиЗКлієнтами_Місяць_TablePart.Період},
    {ВіртуальніТаблиціРегістрів.РозрахункиЗКлієнтами_Місяць_TablePart.Валюта},
    {ВіртуальніТаблиціРегістрів.РозрахункиЗКлієнтами_Місяць_TablePart.Контрагент},
    {ВіртуальніТаблиціРегістрів.РозрахункиЗКлієнтами_Місяць_TablePart.Сума}
)
SELECT 
    uuid_generate_v4(),
    date_trunc('month', Рег_РозрахункиЗКлієнтами.period) as period_month,
    Рег_РозрахункиЗКлієнтами.{РозрахункиЗКлієнтами_Const.Валюта} AS Валюта, 
    Рег_РозрахункиЗКлієнтами.{РозрахункиЗКлієнтами_Const.Контрагент} AS Контрагент,

    SUM(CASE WHEN Рег_РозрахункиЗКлієнтами.income = true THEN 
        Рег_РозрахункиЗКлієнтами.{РозрахункиЗКлієнтами_Const.Сума} ELSE 
       -Рег_РозрахункиЗКлієнтами.{РозрахункиЗКлієнтами_Const.Сума} END) AS Сума
FROM 
    {РозрахункиЗКлієнтами_Const.TABLE} AS Рег_РозрахункиЗКлієнтами
WHERE
    date_trunc('month', Рег_РозрахункиЗКлієнтами.period) = '{month}'
GROUP BY 
    period_month, Валюта, Контрагент
HAVING

   SUM(CASE WHEN Рег_РозрахункиЗКлієнтами.income = true THEN 
        Рег_РозрахункиЗКлієнтами.{РозрахункиЗКлієнтами_Const.Сума} ELSE 
       -Рег_РозрахункиЗКлієнтами.{РозрахункиЗКлієнтами_Const.Сума} END) != 0
";
            Config.Kernel.DataBase.ExecuteSQL(query);
        }
    }

    class CalculateBalancesInRegister_РозрахункиЗПостачальниками
    {
        /// <summary>
        /// Список місяців для яких є рухи по регістру
        /// </summary>
        /// <returns>Список місяців типу 01.05.2022 00:00:00</returns>
        public static List<DateTime> ОтриматиСписокМісяців()
        {
            string query = $@"
SELECT
    date_trunc('month', Рег_РозрахункиЗПостачальниками.period) as period_month
FROM 
    {РозрахункиЗПостачальниками_Const.TABLE} AS Рег_РозрахункиЗПостачальниками
GROUP BY period_month
ORDER BY period_month
";

            string[] columnsName;
            List<object[]> listRow;

            Config.Kernel.DataBase.SelectRequest(query, null, out columnsName, out listRow);

            List<DateTime> listRowDateTime = new List<DateTime>();
            foreach (object[] dateTimeObject in listRow)
                listRowDateTime.Add((DateTime)dateTimeObject[0]);

            return listRowDateTime;
        }

        /// <summary>
        /// Очистка віртуальної таблиці
        /// </summary>
        public static void ВидалитиЗалишки()
        {
            string query = $@"DELETE FROM {ВіртуальніТаблиціРегістрів.РозрахункиЗПостачальниками_Місяць_TablePart.TABLE}";
            Config.Kernel.DataBase.ExecuteSQL(query);

            query = $@"VACUUM (VERBOSE) {ВіртуальніТаблиціРегістрів.РозрахункиЗПостачальниками_Місяць_TablePart.TABLE}";
            Config.Kernel.DataBase.ExecuteSQL(query);
        }

        /// <summary>
        /// Обчислити залишки за період
        /// </summary>
        /// <param name="month">Дата типу 01.05.2022 00:00:00</param>
        public static void ОбчислитиЗалишкиЗаМісяць(DateTime month)
        {
            //Заповнення таблиці
            string query = $@"
INSERT INTO {ВіртуальніТаблиціРегістрів.РозрахункиЗПостачальниками_Місяць_TablePart.TABLE}
(
    uid,
    {ВіртуальніТаблиціРегістрів.РозрахункиЗПостачальниками_Місяць_TablePart.Період},
    {ВіртуальніТаблиціРегістрів.РозрахункиЗПостачальниками_Місяць_TablePart.Валюта},
    {ВіртуальніТаблиціРегістрів.РозрахункиЗПостачальниками_Місяць_TablePart.Контрагент},
    {ВіртуальніТаблиціРегістрів.РозрахункиЗПостачальниками_Місяць_TablePart.Сума}
)
SELECT 
    uuid_generate_v4(),
    date_trunc('month', Рег_РозрахункиЗПостачальниками.period) as period_month,
    Рег_РозрахункиЗПостачальниками.{РозрахункиЗПостачальниками_Const.Валюта} AS Валюта, 
    Рег_РозрахункиЗПостачальниками.{РозрахункиЗПостачальниками_Const.Контрагент} AS Контрагент,

    SUM(CASE WHEN Рег_РозрахункиЗПостачальниками.income = true THEN 
        Рег_РозрахункиЗПостачальниками.{РозрахункиЗПостачальниками_Const.Сума} ELSE 
       -Рег_РозрахункиЗПостачальниками.{РозрахункиЗПостачальниками_Const.Сума} END) AS Сума
FROM 
    {РозрахункиЗПостачальниками_Const.TABLE} AS Рег_РозрахункиЗПостачальниками
WHERE
    date_trunc('month', Рег_РозрахункиЗПостачальниками.period) = '{month}'
GROUP BY 
    period_month, Валюта, Контрагент
HAVING

   SUM(CASE WHEN Рег_РозрахункиЗПостачальниками.income = true THEN 
        Рег_РозрахункиЗПостачальниками.{РозрахункиЗПостачальниками_Const.Сума} ELSE 
       -Рег_РозрахункиЗПостачальниками.{РозрахункиЗПостачальниками_Const.Сума} END) != 0
";
            Config.Kernel.DataBase.ExecuteSQL(query);
        }
    }

    class CalculateBalancesInRegister_ЗамовленняПостачальникам
    {
        /// <summary>
        /// Список місяців для яких є рухи по регістру
        /// </summary>
        /// <returns>Список місяців типу 01.05.2022 00:00:00</returns>
        public static List<DateTime> ОтриматиСписокМісяців()
        {
            string query = $@"
SELECT
    date_trunc('month', Рег_ЗамовленняПостачальникам.period) as period_month
FROM 
    {ЗамовленняПостачальникам_Const.TABLE} AS Рег_ЗамовленняПостачальникам
GROUP BY period_month
ORDER BY period_month
";

            string[] columnsName;
            List<object[]> listRow;

            Config.Kernel.DataBase.SelectRequest(query, null, out columnsName, out listRow);

            List<DateTime> listRowDateTime = new List<DateTime>();
            foreach (object[] dateTimeObject in listRow)
                listRowDateTime.Add((DateTime)dateTimeObject[0]);

            return listRowDateTime;
        }

        /// <summary>
        /// Очистка віртуальної таблиці
        /// </summary>
        public static void ВидалитиЗалишки()
        {
            string query = $@"DELETE FROM {ВіртуальніТаблиціРегістрів.ЗамовленняПостачальникам_Місяць_TablePart.TABLE}";
            Config.Kernel.DataBase.ExecuteSQL(query);

            query = $@"VACUUM (VERBOSE) {ВіртуальніТаблиціРегістрів.ЗамовленняПостачальникам_Місяць_TablePart.TABLE}";
            Config.Kernel.DataBase.ExecuteSQL(query);
        }

        /// <summary>
        /// Обчислити залишки за період
        /// </summary>
        /// <param name="month">Дата типу 01.05.2022 00:00:00</param>
        public static void ОбчислитиЗалишкиЗаМісяць(DateTime month)
        {
            //Заповнення таблиці
            string query = $@"
INSERT INTO {ВіртуальніТаблиціРегістрів.ЗамовленняПостачальникам_Місяць_TablePart.TABLE}
(
    uid,
    {ВіртуальніТаблиціРегістрів.ЗамовленняПостачальникам_Місяць_TablePart.Період},
    {ВіртуальніТаблиціРегістрів.ЗамовленняПостачальникам_Місяць_TablePart.Номенклатура},
    {ВіртуальніТаблиціРегістрів.ЗамовленняПостачальникам_Місяць_TablePart.ХарактеристикиНоменклатури},
    {ВіртуальніТаблиціРегістрів.ЗамовленняПостачальникам_Місяць_TablePart.Склад},
    {ВіртуальніТаблиціРегістрів.ЗамовленняПостачальникам_Місяць_TablePart.Замовлено}
)
SELECT 
    uuid_generate_v4(),
    date_trunc('month', Рег_ЗамовленняПостачальникам.period) as period_month,
    Рег_ЗамовленняПостачальникам.{ЗамовленняПостачальникам_Const.Номенклатура} AS Номенклатура, 
    Рег_ЗамовленняПостачальникам.{ЗамовленняПостачальникам_Const.ХарактеристикаНоменклатури} AS ХарактеристикаНоменклатури,
    Рег_ЗамовленняПостачальникам.{ЗамовленняПостачальникам_Const.Склад} AS Склад,

    SUM(CASE WHEN Рег_ЗамовленняПостачальникам.income = true THEN 
        Рег_ЗамовленняПостачальникам.{ЗамовленняПостачальникам_Const.Замовлено} ELSE 
       -Рег_ЗамовленняПостачальникам.{ЗамовленняПостачальникам_Const.Замовлено} END) AS Замовлено
FROM 
    {ЗамовленняПостачальникам_Const.TABLE} AS Рег_ЗамовленняПостачальникам
WHERE
    date_trunc('month', Рег_ЗамовленняПостачальникам.period) = '{month}'
GROUP BY 
    period_month, Номенклатура, ХарактеристикаНоменклатури, Склад
HAVING 

   SUM(CASE WHEN Рег_ЗамовленняПостачальникам.income = true THEN 
        Рег_ЗамовленняПостачальникам.{ЗамовленняПостачальникам_Const.Замовлено} ELSE 
       -Рег_ЗамовленняПостачальникам.{ЗамовленняПостачальникам_Const.Замовлено} END) != 0
";
            Config.Kernel.DataBase.ExecuteSQL(query);
        }
    }

    class CalculateBalancesInRegister_ВільніЗалишки
    {
        /// <summary>
        /// Список місяців для яких є рухи по регістру
        /// </summary>
        /// <returns>Список місяців типу 01.05.2022 00:00:00</returns>
        public static List<DateTime> ОтриматиСписокМісяців()
        {
            string query = $@"
SELECT
    date_trunc('month', Рег_ВільніЗалишки.period) as period_month
FROM 
    {ВільніЗалишки_Const.TABLE} AS Рег_ВільніЗалишки
GROUP BY period_month
ORDER BY period_month
";

            string[] columnsName;
            List<object[]> listRow;

            Config.Kernel.DataBase.SelectRequest(query, null, out columnsName, out listRow);

            List<DateTime> listRowDateTime = new List<DateTime>();
            foreach (object[] dateTimeObject in listRow)
                listRowDateTime.Add((DateTime)dateTimeObject[0]);

            return listRowDateTime;
        }

        /// <summary>
        /// Очистка віртуальної таблиці
        /// </summary>
        public static void ВидалитиЗалишки()
        {
            string query = $@"DELETE FROM {ВіртуальніТаблиціРегістрів.ВільніЗалишки_Місяць_TablePart.TABLE}";
            Config.Kernel.DataBase.ExecuteSQL(query);

            query = $@"VACUUM (VERBOSE) {ВіртуальніТаблиціРегістрів.ВільніЗалишки_Місяць_TablePart.TABLE}";
            Config.Kernel.DataBase.ExecuteSQL(query);
        }

        /// <summary>
        /// Обчислити залишки за період
        /// </summary>
        /// <param name="month">Дата типу 01.05.2022 00:00:00</param>
        public static void ОбчислитиЗалишкиЗаМісяць(DateTime month)
        {
            //Заповнення таблиці
            string query = $@"
INSERT INTO {ВіртуальніТаблиціРегістрів.ВільніЗалишки_Місяць_TablePart.TABLE}
(
    uid,
    {ВіртуальніТаблиціРегістрів.ВільніЗалишки_Місяць_TablePart.Період},
    {ВіртуальніТаблиціРегістрів.ВільніЗалишки_Місяць_TablePart.Номенклатура},
    {ВіртуальніТаблиціРегістрів.ВільніЗалишки_Місяць_TablePart.ХарактеристикиНоменклатури},
    {ВіртуальніТаблиціРегістрів.ВільніЗалишки_Місяць_TablePart.Склад},
    {ВіртуальніТаблиціРегістрів.ВільніЗалишки_Місяць_TablePart.ВНаявності},
    {ВіртуальніТаблиціРегістрів.ВільніЗалишки_Місяць_TablePart.ВРезервіЗіСкладу},
    {ВіртуальніТаблиціРегістрів.ВільніЗалишки_Місяць_TablePart.ВРезервіПідЗамовлення}
)
SELECT 
    uuid_generate_v4(),
    date_trunc('month', Рег_ВільніЗалишки.period) as period_month,
    Рег_ВільніЗалишки.{ВільніЗалишки_Const.Номенклатура} AS Номенклатура, 
    Рег_ВільніЗалишки.{ВільніЗалишки_Const.ХарактеристикаНоменклатури} AS ХарактеристикаНоменклатури,
    Рег_ВільніЗалишки.{ВільніЗалишки_Const.Склад} AS Склад,

    SUM(CASE WHEN Рег_ВільніЗалишки.income = true THEN 
        Рег_ВільніЗалишки.{ВільніЗалишки_Const.ВНаявності} ELSE 
       -Рег_ВільніЗалишки.{ВільніЗалишки_Const.ВНаявності} END) AS ВНаявності,

    SUM(CASE WHEN Рег_ВільніЗалишки.income = true THEN 
        Рег_ВільніЗалишки.{ВільніЗалишки_Const.ВРезервіЗіСкладу} ELSE 
       -Рег_ВільніЗалишки.{ВільніЗалишки_Const.ВРезервіЗіСкладу} END) AS ВРезервіЗіСкладу,

    SUM(CASE WHEN Рег_ВільніЗалишки.income = true THEN 
        Рег_ВільніЗалишки.{ВільніЗалишки_Const.ВРезервіПідЗамовлення} ELSE 
       -Рег_ВільніЗалишки.{ВільніЗалишки_Const.ВРезервіПідЗамовлення} END) AS ВРезервіПідЗамовлення
FROM 
    {ВільніЗалишки_Const.TABLE} AS Рег_ВільніЗалишки
WHERE
    date_trunc('month', Рег_ВільніЗалишки.period) = '{month}'
GROUP BY 
    period_month, Номенклатура, ХарактеристикаНоменклатури, Склад
HAVING 

   SUM(CASE WHEN Рег_ВільніЗалишки.income = true THEN 
        Рег_ВільніЗалишки.{ВільніЗалишки_Const.ВНаявності} ELSE 
       -Рег_ВільніЗалишки.{ВільніЗалишки_Const.ВНаявності} END) != 0
OR
    SUM(CASE WHEN Рег_ВільніЗалишки.income = true THEN 
        Рег_ВільніЗалишки.{ВільніЗалишки_Const.ВРезервіЗіСкладу} ELSE 
       -Рег_ВільніЗалишки.{ВільніЗалишки_Const.ВРезервіЗіСкладу} END) != 0
OR
    SUM(CASE WHEN Рег_ВільніЗалишки.income = true THEN 
        Рег_ВільніЗалишки.{ВільніЗалишки_Const.ВРезервіПідЗамовлення} ELSE 
       -Рег_ВільніЗалишки.{ВільніЗалишки_Const.ВРезервіПідЗамовлення} END) != 0
";
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