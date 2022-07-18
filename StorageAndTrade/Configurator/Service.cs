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

    class CalculationBalances
    {
        public static void ПідключитиДодаток_UUID_OSSP()
        {
            string query = "CREATE EXTENSION IF NOT EXISTS \"uuid-ossp\"";
            Config.KernelBackgroundTask.DataBase.ExecuteSQL(query);
        }

        /// <summary>
        /// Функція перевіряє список фонових задач для обчислення віртуальних залишків
        /// та обчислює залишки на дату проведення документу.
        /// Алгоритм обчислення:
        /// 1. Отримати список задач
        /// 2. Отримати список регістрів доступних для документу який вказаний в задачі
        /// 3. Розрахувати залишки на дату проведення документу по всіх доступних регістрах
        /// 4. Зафікусувати що задача виконана
        /// </summary>
        public static void ОбчисленняВіртуальнихЗалишківПоДнях()
        {
            string querySelectTask = $@"
SELECT
    Задачі.uid,
    Задачі.{Системні.ФоновіЗадачі_ОбчисленняВіртуальнихЗалишків_TablePart.Дата} AS Дата,
    Задачі.{Системні.ФоновіЗадачі_ОбчисленняВіртуальнихЗалишків_TablePart.Документ} AS Документ,
    Задачі.{Системні.ФоновіЗадачі_ОбчисленняВіртуальнихЗалишків_TablePart.ТипДокументу} AS ТипДокументу,
    Задачі.{Системні.ФоновіЗадачі_ОбчисленняВіртуальнихЗалишків_TablePart.ПеріодОбчислення} AS Період
FROM
    {Системні.ФоновіЗадачі_ОбчисленняВіртуальнихЗалишків_TablePart.TABLE} AS Задачі
WHERE
    Задачі.{Системні.ФоновіЗадачі_ОбчисленняВіртуальнихЗалишків_TablePart.Виконано} = false
ORDER BY Дата ASC
";
            Dictionary<string, List<string>> documentProcessedList = new Dictionary<string, List<string>>();

            string[] columnsName;
            List<object[]> listRow;

            Config.KernelBackgroundTask.DataBase.SelectRequest(querySelectTask, null, out columnsName, out listRow);

            Config.KernelBackgroundTask.DataBase.BeginTransaction();

            //Обробка задач
            foreach (object[] row in listRow)
            {
                string uid = row[0].ToString();
                string Дата = row[1].ToString();
                string Документ = row[2].ToString();
                string ТипДокументу = row[3].ToString();
                string Період = row[4].ToString();

                //Console.WriteLine($"Документ: {Документ} ТипДокументу: {ТипДокументу} Період:{Період}");

                bool documentProcessed = false;

                if (documentProcessedList.ContainsKey(Період))
                {
                    if (documentProcessedList[Період].Contains(ТипДокументу))
                    {
                        //Console.WriteLine($"continue: {Період} {ТипДокументу}");
                        documentProcessed = true;
                    }
                    else
                        documentProcessedList[Період].Add(ТипДокументу);
                }
                else
                    documentProcessedList.Add(Період, new List<string>() { ТипДокументу });

                //Список регістрів доступних для документу
                List<string> allowRegisterAccumulation = Config.Kernel.Conf.Documents[ТипДокументу].AllowRegisterAccumulation;

                if (!documentProcessed)
                    foreach (string registerAccumulation in allowRegisterAccumulation)
                    {
                        switch (registerAccumulation)
                        {
                            case "ЗамовленняКлієнтів":
                                {
                                    string query = $@"
DELETE FROM {ВіртуальніТаблиціРегістрів.ЗамовленняКлієнтів_День_TablePart.TABLE}
WHERE date_trunc('day', {ВіртуальніТаблиціРегістрів.ЗамовленняКлієнтів_День_TablePart.Період}) = '{Період}';

INSERT INTO {ВіртуальніТаблиціРегістрів.ЗамовленняКлієнтів_День_TablePart.TABLE}
(
    uid,
    {ВіртуальніТаблиціРегістрів.ЗамовленняКлієнтів_День_TablePart.Період},
    {ВіртуальніТаблиціРегістрів.ЗамовленняКлієнтів_День_TablePart.Номенклатура},
    {ВіртуальніТаблиціРегістрів.ЗамовленняКлієнтів_День_TablePart.ХарактеристикаНоменклатури},
    {ВіртуальніТаблиціРегістрів.ЗамовленняКлієнтів_День_TablePart.Склад},
    {ВіртуальніТаблиціРегістрів.ЗамовленняКлієнтів_День_TablePart.Замовлено},
    {ВіртуальніТаблиціРегістрів.ЗамовленняКлієнтів_День_TablePart.Сума}
)
SELECT 
    uuid_generate_v4(),
    date_trunc('day', Рег_ЗамовленняКлієнтів.period) as period_month,
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
    date_trunc('day', Рег_ЗамовленняКлієнтів.period) = '{Період}'
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

                                    //Console.WriteLine(query);
                                    Config.KernelBackgroundTask.DataBase.ExecuteSQL(query);

                                    break;
                                }
                            case "ТовариНаСкладах":
                                {
                                    string query = $@"
DELETE FROM {ВіртуальніТаблиціРегістрів.ТовариНаСкладах_День_TablePart.TABLE}
WHERE date_trunc('day', {ВіртуальніТаблиціРегістрів.ТовариНаСкладах_День_TablePart.Період}) = '{Період}';

INSERT INTO {ВіртуальніТаблиціРегістрів.ТовариНаСкладах_День_TablePart.TABLE}
(
    uid,
    {ВіртуальніТаблиціРегістрів.ТовариНаСкладах_День_TablePart.Період},
    {ВіртуальніТаблиціРегістрів.ТовариНаСкладах_День_TablePart.Номенклатура},
    {ВіртуальніТаблиціРегістрів.ТовариНаСкладах_День_TablePart.ХарактеристикаНоменклатури},
    {ВіртуальніТаблиціРегістрів.ТовариНаСкладах_День_TablePart.Склад},
    {ВіртуальніТаблиціРегістрів.ТовариНаСкладах_День_TablePart.ВНаявності},
    {ВіртуальніТаблиціРегістрів.ТовариНаСкладах_День_TablePart.ДоВідвантаження}
)
SELECT 
    uuid_generate_v4(),
    date_trunc('day', Рег_ТовариНаСкладах.period) as period_day,
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
    date_trunc('day', Рег_ТовариНаСкладах.period) = '{Період}'
GROUP BY 
    period_day, Номенклатура, ХарактеристикаНоменклатури, Склад
HAVING 
   SUM(CASE WHEN Рег_ТовариНаСкладах.income = true THEN 
        Рег_ТовариНаСкладах.{ТовариНаСкладах_Const.ВНаявності} ELSE 
       -Рег_ТовариНаСкладах.{ТовариНаСкладах_Const.ВНаявності} END) != 0
OR
    SUM(CASE WHEN Рег_ТовариНаСкладах.income = true THEN 
        Рег_ТовариНаСкладах.{ТовариНаСкладах_Const.ДоВідвантаження} ELSE 
       -Рег_ТовариНаСкладах.{ТовариНаСкладах_Const.ДоВідвантаження} END) != 0
";

                                    //Console.WriteLine(query);
                                    Config.KernelBackgroundTask.DataBase.ExecuteSQL(query);

                                    break;
                                }
                            case "РозрахункиЗКлієнтами":
                                {
                                    string query = $@"
DELETE FROM {ВіртуальніТаблиціРегістрів.РозрахункиЗКлієнтами_День_TablePart.TABLE}
WHERE date_trunc('day', {ВіртуальніТаблиціРегістрів.РозрахункиЗКлієнтами_День_TablePart.Період}) = '{Період}';

INSERT INTO {ВіртуальніТаблиціРегістрів.РозрахункиЗКлієнтами_День_TablePart.TABLE}
(
    uid,
    {ВіртуальніТаблиціРегістрів.РозрахункиЗКлієнтами_День_TablePart.Період},
    {ВіртуальніТаблиціРегістрів.РозрахункиЗКлієнтами_День_TablePart.Валюта},
    {ВіртуальніТаблиціРегістрів.РозрахункиЗКлієнтами_День_TablePart.Контрагент},
    {ВіртуальніТаблиціРегістрів.РозрахункиЗКлієнтами_День_TablePart.Сума}
)
SELECT 
    uuid_generate_v4(),
    date_trunc('day', Рег_РозрахункиЗКлієнтами.period) as period_month,
    Рег_РозрахункиЗКлієнтами.{РозрахункиЗКлієнтами_Const.Валюта} AS Валюта, 
    Рег_РозрахункиЗКлієнтами.{РозрахункиЗКлієнтами_Const.Контрагент} AS Контрагент,
    SUM(CASE WHEN Рег_РозрахункиЗКлієнтами.income = true THEN 
        Рег_РозрахункиЗКлієнтами.{РозрахункиЗКлієнтами_Const.Сума} ELSE 
       -Рег_РозрахункиЗКлієнтами.{РозрахункиЗКлієнтами_Const.Сума} END) AS Сума
FROM 
    {РозрахункиЗКлієнтами_Const.TABLE} AS Рег_РозрахункиЗКлієнтами
WHERE
    date_trunc('day', Рег_РозрахункиЗКлієнтами.period) = '{Період}'
GROUP BY 
    period_month, Валюта, Контрагент
HAVING
   SUM(CASE WHEN Рег_РозрахункиЗКлієнтами.income = true THEN 
        Рег_РозрахункиЗКлієнтами.{РозрахункиЗКлієнтами_Const.Сума} ELSE 
       -Рег_РозрахункиЗКлієнтами.{РозрахункиЗКлієнтами_Const.Сума} END) != 0
";

                                    //Console.WriteLine(query);
                                    Config.KernelBackgroundTask.DataBase.ExecuteSQL(query);

                                    break;
                                }
                            case "РозрахункиЗПостачальниками":
                                {
                                    string query = $@"
DELETE FROM {ВіртуальніТаблиціРегістрів.РозрахункиЗПостачальниками_День_TablePart.TABLE}
WHERE date_trunc('day', {ВіртуальніТаблиціРегістрів.РозрахункиЗПостачальниками_День_TablePart.Період}) = '{Період}';

INSERT INTO {ВіртуальніТаблиціРегістрів.РозрахункиЗПостачальниками_День_TablePart.TABLE}
(
    uid,
    {ВіртуальніТаблиціРегістрів.РозрахункиЗПостачальниками_День_TablePart.Період},
    {ВіртуальніТаблиціРегістрів.РозрахункиЗПостачальниками_День_TablePart.Валюта},
    {ВіртуальніТаблиціРегістрів.РозрахункиЗПостачальниками_День_TablePart.Контрагент},
    {ВіртуальніТаблиціРегістрів.РозрахункиЗПостачальниками_День_TablePart.Сума}
)
SELECT 
    uuid_generate_v4(),
    date_trunc('day', Рег_РозрахункиЗПостачальниками.period) as period_month,
    Рег_РозрахункиЗПостачальниками.{РозрахункиЗПостачальниками_Const.Валюта} AS Валюта, 
    Рег_РозрахункиЗПостачальниками.{РозрахункиЗПостачальниками_Const.Контрагент} AS Контрагент,
    SUM(CASE WHEN Рег_РозрахункиЗПостачальниками.income = true THEN 
        Рег_РозрахункиЗПостачальниками.{РозрахункиЗПостачальниками_Const.Сума} ELSE 
       -Рег_РозрахункиЗПостачальниками.{РозрахункиЗПостачальниками_Const.Сума} END) AS Сума
FROM 
    {РозрахункиЗПостачальниками_Const.TABLE} AS Рег_РозрахункиЗПостачальниками
WHERE
    date_trunc('day', Рег_РозрахункиЗПостачальниками.period) = '{Період}'
GROUP BY 
    period_month, Валюта, Контрагент
HAVING
   SUM(CASE WHEN Рег_РозрахункиЗПостачальниками.income = true THEN 
        Рег_РозрахункиЗПостачальниками.{РозрахункиЗПостачальниками_Const.Сума} ELSE 
       -Рег_РозрахункиЗПостачальниками.{РозрахункиЗПостачальниками_Const.Сума} END) != 0
";

                                    //Console.WriteLine(query);
                                    Config.KernelBackgroundTask.DataBase.ExecuteSQL(query);

                                    break;
                                }
                            case "ЗамовленняПостачальникам":
                                {
                                    string query = $@"
DELETE FROM {ВіртуальніТаблиціРегістрів.ЗамовленняПостачальникам_День_TablePart.TABLE}
WHERE date_trunc('day', {ВіртуальніТаблиціРегістрів.ЗамовленняПостачальникам_День_TablePart.Період}) = '{Період}';

INSERT INTO {ВіртуальніТаблиціРегістрів.ЗамовленняПостачальникам_День_TablePart.TABLE}
(
    uid,
    {ВіртуальніТаблиціРегістрів.ЗамовленняПостачальникам_День_TablePart.Період},
    {ВіртуальніТаблиціРегістрів.ЗамовленняПостачальникам_День_TablePart.Номенклатура},
    {ВіртуальніТаблиціРегістрів.ЗамовленняПостачальникам_День_TablePart.ХарактеристикаНоменклатури},
    {ВіртуальніТаблиціРегістрів.ЗамовленняПостачальникам_День_TablePart.Склад},
    {ВіртуальніТаблиціРегістрів.ЗамовленняПостачальникам_День_TablePart.Замовлено}
)
SELECT 
    uuid_generate_v4(),
    date_trunc('day', Рег_ЗамовленняПостачальникам.period) as period_month,
    Рег_ЗамовленняПостачальникам.{ЗамовленняПостачальникам_Const.Номенклатура} AS Номенклатура, 
    Рег_ЗамовленняПостачальникам.{ЗамовленняПостачальникам_Const.ХарактеристикаНоменклатури} AS ХарактеристикаНоменклатури,
    Рег_ЗамовленняПостачальникам.{ЗамовленняПостачальникам_Const.Склад} AS Склад,
    SUM(CASE WHEN Рег_ЗамовленняПостачальникам.income = true THEN 
        Рег_ЗамовленняПостачальникам.{ЗамовленняПостачальникам_Const.Замовлено} ELSE 
       -Рег_ЗамовленняПостачальникам.{ЗамовленняПостачальникам_Const.Замовлено} END) AS Замовлено
FROM 
    {ЗамовленняПостачальникам_Const.TABLE} AS Рег_ЗамовленняПостачальникам
WHERE
    date_trunc('day', Рег_ЗамовленняПостачальникам.period) = '{Період}'
GROUP BY 
    period_month, Номенклатура, ХарактеристикаНоменклатури, Склад
HAVING 
   SUM(CASE WHEN Рег_ЗамовленняПостачальникам.income = true THEN 
        Рег_ЗамовленняПостачальникам.{ЗамовленняПостачальникам_Const.Замовлено} ELSE 
       -Рег_ЗамовленняПостачальникам.{ЗамовленняПостачальникам_Const.Замовлено} END) != 0
";

                                    //Console.WriteLine(query);
                                    Config.KernelBackgroundTask.DataBase.ExecuteSQL(query);

                                    break;
                                }
                            case "ВільніЗалишки":
                                {
                                    string query = $@"
DELETE FROM {ВіртуальніТаблиціРегістрів.ВільніЗалишки_День_TablePart.TABLE}
WHERE date_trunc('day', {ВіртуальніТаблиціРегістрів.ВільніЗалишки_День_TablePart.Період}) = '{Період}';

INSERT INTO {ВіртуальніТаблиціРегістрів.ВільніЗалишки_День_TablePart.TABLE}
(
    uid,
    {ВіртуальніТаблиціРегістрів.ВільніЗалишки_День_TablePart.Період},
    {ВіртуальніТаблиціРегістрів.ВільніЗалишки_День_TablePart.Номенклатура},
    {ВіртуальніТаблиціРегістрів.ВільніЗалишки_День_TablePart.ХарактеристикаНоменклатури},
    {ВіртуальніТаблиціРегістрів.ВільніЗалишки_День_TablePart.Склад},
    {ВіртуальніТаблиціРегістрів.ВільніЗалишки_День_TablePart.ВНаявності},
    {ВіртуальніТаблиціРегістрів.ВільніЗалишки_День_TablePart.ВРезервіЗіСкладу},
    {ВіртуальніТаблиціРегістрів.ВільніЗалишки_День_TablePart.ВРезервіПідЗамовлення}
)
SELECT 
    uuid_generate_v4(),
    date_trunc('day', Рег_ВільніЗалишки.period) as period_month,
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
    date_trunc('day', Рег_ВільніЗалишки.period) = '{Період}'
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

                                    //Console.WriteLine(query);
                                    Config.KernelBackgroundTask.DataBase.ExecuteSQL(query);

                                    break;
                                }
                        }
                    }

                string queryUpdate = $@"
UPDATE {Системні.ФоновіЗадачі_ОбчисленняВіртуальнихЗалишків_TablePart.TABLE}
    SET {Системні.ФоновіЗадачі_ОбчисленняВіртуальнихЗалишків_TablePart.Виконано} = true
WHERE uid = '{uid}'
";
                Config.KernelBackgroundTask.DataBase.ExecuteSQL(queryUpdate);
            }

            //Обновлення актуальності віртуальних залишків по місяцях
            foreach (KeyValuePair<string, List<string>> documentProcessed in documentProcessedList)
            {
                List<string> allAllowRegisterAccumulation = new List<string>();

                foreach (string documentType in documentProcessed.Value)
                {
                    //Список регістрів доступних для документу
                    List<string> allowRegisterAccumulation = Config.Kernel.Conf.Documents[documentType].AllowRegisterAccumulation;

                    foreach (string registerAccumulation in allowRegisterAccumulation)
                    {
                        if (!allAllowRegisterAccumulation.Contains(registerAccumulation))
                            allAllowRegisterAccumulation.Add(registerAccumulation);
                    }
                }

                ОбновитиЗначенняАктуальностіВіртуальнихЗалишківПоМісяцях(documentProcessed.Key, allAllowRegisterAccumulation);
            }

            Config.KernelBackgroundTask.DataBase.CommitTransaction();
        }

        /// <summary>
        /// Функція обновляє значення актуальності для ВіртальнихТаблицьРегістрів по місяцях.
        /// </summary>
        /// <param name="period">Дата</param>
        /// <param name="allowRegisterAccumulation">Список регістрів</param>
        public static void ОбновитиЗначенняАктуальностіВіртуальнихЗалишківПоМісяцях(string period, List<string> allowRegisterAccumulation)
        {
            string queryPartRegisterAccumulation = "'" + string.Join("','", allowRegisterAccumulation) + "'";

            string queryDelete = $@"
DELETE FROM {Системні.ФоновіЗадачі_АктуальністьВіртуальнихЗалишків_TablePart.TABLE}
WHERE date_trunc('month', '{period}'::timestamp) = {Системні.ФоновіЗадачі_АктуальністьВіртуальнихЗалишків_TablePart.Місяць} AND
    {Системні.ФоновіЗадачі_АктуальністьВіртуальнихЗалишків_TablePart.Регістр} IN({ queryPartRegisterAccumulation})";

            //Console.WriteLine(queryDelete);
            Config.KernelBackgroundTask.DataBase.ExecuteSQL(queryDelete);

            foreach (string registerAccumulation in allowRegisterAccumulation)
            {
                string queryInsert = $@"
INSERT INTO {Системні.ФоновіЗадачі_АктуальністьВіртуальнихЗалишків_TablePart.TABLE}
(
    uid,
    {Системні.ФоновіЗадачі_АктуальністьВіртуальнихЗалишків_TablePart.Регістр},
    {Системні.ФоновіЗадачі_АктуальністьВіртуальнихЗалишків_TablePart.Місяць},
    {Системні.ФоновіЗадачі_АктуальністьВіртуальнихЗалишків_TablePart.Актуально}
)
VALUES
(
    uuid_generate_v4(),
    '{registerAccumulation}',
    date_trunc('month', '{period}'::timestamp),
    false
)";
                //Console.WriteLine(queryInsert);
                Config.KernelBackgroundTask.DataBase.ExecuteSQL(queryInsert);
            }
        }

        public static void ОбчисленняВіртуальнихЗалишківПоМісяцях()
        {
            string querySelect = $@"
SELECT
    Актуальність.uid,
    Актуальність.{Системні.ФоновіЗадачі_АктуальністьВіртуальнихЗалишків_TablePart.Регістр} AS Регістр,
    Актуальність.{Системні.ФоновіЗадачі_АктуальністьВіртуальнихЗалишків_TablePart.Місяць} AS Місяць
FROM
    {Системні.ФоновіЗадачі_АктуальністьВіртуальнихЗалишків_TablePart.TABLE} AS Актуальність
WHERE
    Актуальність.{Системні.ФоновіЗадачі_АктуальністьВіртуальнихЗалишків_TablePart.Актуально} = false 
-- AND Актуальність.{Системні.ФоновіЗадачі_АктуальністьВіртуальнихЗалишків_TablePart.Місяць} != date_trunc('month', '{DateTime.Now}'::timestamp)
ORDER BY Місяць ASC
";

            string[] columnsName;
            List<object[]> listRow;

            Config.KernelBackgroundTask.DataBase.SelectRequest(querySelect, null, out columnsName, out listRow);

            //Обробка
            foreach (object[] row in listRow)
            {
                string uid = row[0].ToString();
                string Регістр = row[1].ToString();
                string Місяць = row[2].ToString();

                Console.WriteLine($"Регістр {Регістр} Місяць {Місяць}");

                switch (Регістр)
                {
                    case "ЗамовленняКлієнтів":
                        {
                            string query = $@"
DELETE FROM {ВіртуальніТаблиціРегістрів.ЗамовленняКлієнтів_Місяць_TablePart.TABLE}
WHERE date_trunc('month', {ВіртуальніТаблиціРегістрів.ЗамовленняКлієнтів_Місяць_TablePart.Період}) = '{Місяць}';

INSERT INTO {ВіртуальніТаблиціРегістрів.ЗамовленняКлієнтів_Місяць_TablePart.TABLE}
(
    uid,
    {ВіртуальніТаблиціРегістрів.ЗамовленняКлієнтів_Місяць_TablePart.Період},
    {ВіртуальніТаблиціРегістрів.ЗамовленняКлієнтів_Місяць_TablePart.Номенклатура},
    {ВіртуальніТаблиціРегістрів.ЗамовленняКлієнтів_Місяць_TablePart.ХарактеристикаНоменклатури},
    {ВіртуальніТаблиціРегістрів.ЗамовленняКлієнтів_Місяць_TablePart.Склад},
    {ВіртуальніТаблиціРегістрів.ЗамовленняКлієнтів_Місяць_TablePart.Замовлено},
    {ВіртуальніТаблиціРегістрів.ЗамовленняКлієнтів_Місяць_TablePart.Сума}
)
SELECT 
    uuid_generate_v4(),
    date_trunc('month', ЗамовленняКлієнтів_День.{ВіртуальніТаблиціРегістрів.ЗамовленняКлієнтів_День_TablePart.Період}) as Період,
    ЗамовленняКлієнтів_День.{ВіртуальніТаблиціРегістрів.ЗамовленняКлієнтів_День_TablePart.Номенклатура} AS Номенклатура, 
    ЗамовленняКлієнтів_День.{ВіртуальніТаблиціРегістрів.ЗамовленняКлієнтів_День_TablePart.ХарактеристикаНоменклатури} AS ХарактеристикаНоменклатури,
    ЗамовленняКлієнтів_День.{ВіртуальніТаблиціРегістрів.ЗамовленняКлієнтів_День_TablePart.Склад} AS Склад,
    SUM(ЗамовленняКлієнтів_День.{ВіртуальніТаблиціРегістрів.ЗамовленняКлієнтів_День_TablePart.Замовлено}) AS Замовлено,
    SUM(ЗамовленняКлієнтів_День.{ВіртуальніТаблиціРегістрів.ЗамовленняКлієнтів_День_TablePart.Сума}) AS Сума
FROM 
    {ВіртуальніТаблиціРегістрів.ЗамовленняКлієнтів_День_TablePart.TABLE} AS ЗамовленняКлієнтів_День
WHERE
    date_trunc('month', ЗамовленняКлієнтів_День.{ВіртуальніТаблиціРегістрів.ЗамовленняКлієнтів_День_TablePart.Період}) = '{Місяць}'
GROUP BY 
    Період, Номенклатура, ХарактеристикаНоменклатури, Склад
HAVING 
    SUM(ЗамовленняКлієнтів_День.{ВіртуальніТаблиціРегістрів.ЗамовленняКлієнтів_День_TablePart.Замовлено}) != 0 OR
    SUM(ЗамовленняКлієнтів_День.{ВіртуальніТаблиціРегістрів.ЗамовленняКлієнтів_День_TablePart.Сума}) != 0
";
                            
                            //Console.WriteLine(query);
                            Config.KernelBackgroundTask.DataBase.ExecuteSQL(query);

                            break;
                        }
                    case "ТовариНаСкладах":
                        {
                            string query = $@"
DELETE FROM {ВіртуальніТаблиціРегістрів.ТовариНаСкладах_Місяць_TablePart.TABLE}
WHERE date_trunc('month', {ВіртуальніТаблиціРегістрів.ТовариНаСкладах_Місяць_TablePart.Період}) = '{Місяць}';

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
    date_trunc('month', ТовариНаСкладах_День.{ВіртуальніТаблиціРегістрів.ТовариНаСкладах_День_TablePart.Період}) as Період,
    ТовариНаСкладах_День.{ВіртуальніТаблиціРегістрів.ТовариНаСкладах_День_TablePart.Номенклатура} AS Номенклатура, 
    ТовариНаСкладах_День.{ВіртуальніТаблиціРегістрів.ТовариНаСкладах_День_TablePart.ХарактеристикаНоменклатури} AS ХарактеристикаНоменклатури,
    ТовариНаСкладах_День.{ВіртуальніТаблиціРегістрів.ТовариНаСкладах_День_TablePart.Склад} AS Склад,
    SUM(ТовариНаСкладах_День.{ВіртуальніТаблиціРегістрів.ТовариНаСкладах_День_TablePart.ВНаявності}) AS ВНаявності,
    SUM(ТовариНаСкладах_День.{ВіртуальніТаблиціРегістрів.ТовариНаСкладах_День_TablePart.ДоВідвантаження}) AS ДоВідвантаження
FROM 
    {ВіртуальніТаблиціРегістрів.ТовариНаСкладах_День_TablePart.TABLE} AS ТовариНаСкладах_День
WHERE
    date_trunc('month', ТовариНаСкладах_День.{ВіртуальніТаблиціРегістрів.ТовариНаСкладах_День_TablePart.Період}) = '{Місяць}'
GROUP BY 
    Період, Номенклатура, ХарактеристикаНоменклатури, Склад
HAVING 
   SUM(ТовариНаСкладах_День.{ВіртуальніТаблиціРегістрів.ТовариНаСкладах_День_TablePart.ВНаявності}) != 0 OR
   SUM(ТовариНаСкладах_День.{ВіртуальніТаблиціРегістрів.ТовариНаСкладах_День_TablePart.ДоВідвантаження}) != 0
";

                            //Console.WriteLine(query);
                            Config.KernelBackgroundTask.DataBase.ExecuteSQL(query);

                            break;
                        }
                    case "РозрахункиЗКлієнтами":
                        {
                            string query = $@"
DELETE FROM {ВіртуальніТаблиціРегістрів.РозрахункиЗКлієнтами_Місяць_TablePart.TABLE}
WHERE date_trunc('month', {ВіртуальніТаблиціРегістрів.РозрахункиЗКлієнтами_Місяць_TablePart.Період}) = '{Місяць}';

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
    date_trunc('month', РозрахункиЗКлієнтами_День.{ВіртуальніТаблиціРегістрів.РозрахункиЗКлієнтами_День_TablePart.Період}) as Період,
    РозрахункиЗКлієнтами_День.{ВіртуальніТаблиціРегістрів.РозрахункиЗКлієнтами_День_TablePart.Валюта} AS Валюта, 
    РозрахункиЗКлієнтами_День.{ВіртуальніТаблиціРегістрів.РозрахункиЗКлієнтами_День_TablePart.Контрагент} AS Контрагент,
    SUM(РозрахункиЗКлієнтами_День.{ВіртуальніТаблиціРегістрів.РозрахункиЗКлієнтами_День_TablePart.Сума}) AS Сума
FROM 
    {ВіртуальніТаблиціРегістрів.РозрахункиЗКлієнтами_День_TablePart.TABLE} AS РозрахункиЗКлієнтами_День
WHERE
    date_trunc('month', РозрахункиЗКлієнтами_День.{ВіртуальніТаблиціРегістрів.РозрахункиЗКлієнтами_День_TablePart.Період}) = '{Місяць}'
GROUP BY 
    Період, Валюта, Контрагент
HAVING
   SUM(РозрахункиЗКлієнтами_День.{ВіртуальніТаблиціРегістрів.РозрахункиЗКлієнтами_День_TablePart.Сума}) != 0
";

                            //Console.WriteLine(query);
                            Config.KernelBackgroundTask.DataBase.ExecuteSQL(query);

                            break;
                        }
                    case "РозрахункиЗПостачальниками":
                        {
                            string query = $@"
DELETE FROM {ВіртуальніТаблиціРегістрів.РозрахункиЗПостачальниками_Місяць_TablePart.TABLE}
WHERE date_trunc('month', {ВіртуальніТаблиціРегістрів.РозрахункиЗПостачальниками_Місяць_TablePart.Період}) = '{Місяць}';

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
    date_trunc('month', РозрахункиЗПостачальниками_День.{ВіртуальніТаблиціРегістрів.РозрахункиЗПостачальниками_День_TablePart.Період}) as Період,
    РозрахункиЗПостачальниками_День.{ВіртуальніТаблиціРегістрів.РозрахункиЗПостачальниками_День_TablePart.Валюта} AS Валюта, 
    РозрахункиЗПостачальниками_День.{ВіртуальніТаблиціРегістрів.РозрахункиЗПостачальниками_День_TablePart.Контрагент} AS Контрагент,
    SUM(РозрахункиЗПостачальниками_День.{ВіртуальніТаблиціРегістрів.РозрахункиЗПостачальниками_День_TablePart.Сума}) AS Сума
FROM 
    {ВіртуальніТаблиціРегістрів.РозрахункиЗПостачальниками_День_TablePart.TABLE} AS РозрахункиЗПостачальниками_День
WHERE
    date_trunc('month', РозрахункиЗПостачальниками_День.{ВіртуальніТаблиціРегістрів.РозрахункиЗПостачальниками_День_TablePart.Період}) = '{Місяць}'
GROUP BY
    Період, Валюта, Контрагент
HAVING
    SUM(РозрахункиЗПостачальниками_День.{ВіртуальніТаблиціРегістрів.РозрахункиЗПостачальниками_День_TablePart.Сума}) != 0
";

                            //Console.WriteLine(query);
                            Config.KernelBackgroundTask.DataBase.ExecuteSQL(query);

                            break;
                        }
                    case "ЗамовленняПостачальникам":
                        {
                            string query = $@"
DELETE FROM {ВіртуальніТаблиціРегістрів.ЗамовленняПостачальникам_Місяць_TablePart.TABLE}
WHERE date_trunc('month', {ВіртуальніТаблиціРегістрів.ЗамовленняПостачальникам_Місяць_TablePart.Період}) = '{Місяць}';

INSERT INTO {ВіртуальніТаблиціРегістрів.ЗамовленняПостачальникам_Місяць_TablePart.TABLE}
(
    uid,
    {ВіртуальніТаблиціРегістрів.ЗамовленняПостачальникам_Місяць_TablePart.Період},
    {ВіртуальніТаблиціРегістрів.ЗамовленняПостачальникам_Місяць_TablePart.Номенклатура},
    {ВіртуальніТаблиціРегістрів.ЗамовленняПостачальникам_Місяць_TablePart.ХарактеристикаНоменклатури},
    {ВіртуальніТаблиціРегістрів.ЗамовленняПостачальникам_Місяць_TablePart.Склад},
    {ВіртуальніТаблиціРегістрів.ЗамовленняПостачальникам_Місяць_TablePart.Замовлено}
)
SELECT 
    uuid_generate_v4(),
    date_trunc('month', ЗамовленняПостачальникам_День.{ВіртуальніТаблиціРегістрів.ЗамовленняПостачальникам_День_TablePart.Період}) as Період,
    ЗамовленняПостачальникам_День.{ВіртуальніТаблиціРегістрів.ЗамовленняПостачальникам_День_TablePart.Номенклатура} AS Номенклатура, 
    ЗамовленняПостачальникам_День.{ВіртуальніТаблиціРегістрів.ЗамовленняПостачальникам_День_TablePart.ХарактеристикаНоменклатури} AS ХарактеристикаНоменклатури,
    ЗамовленняПостачальникам_День.{ВіртуальніТаблиціРегістрів.ЗамовленняПостачальникам_День_TablePart.Склад} AS Склад,
    SUM(ЗамовленняПостачальникам_День.{ВіртуальніТаблиціРегістрів.ЗамовленняПостачальникам_День_TablePart.Замовлено}) AS Замовлено
FROM 
    {ВіртуальніТаблиціРегістрів.ЗамовленняПостачальникам_День_TablePart.TABLE} AS ЗамовленняПостачальникам_День
WHERE
    date_trunc('month', ЗамовленняПостачальникам_День.{ВіртуальніТаблиціРегістрів.ЗамовленняПостачальникам_День_TablePart.Період}) = '{Місяць}'
GROUP BY 
    Період, Номенклатура, ХарактеристикаНоменклатури, Склад
HAVING 
    SUM(ЗамовленняПостачальникам_День.{ВіртуальніТаблиціРегістрів.ЗамовленняПостачальникам_День_TablePart.Замовлено}) != 0
";

                            //Console.WriteLine(query);
                            Config.KernelBackgroundTask.DataBase.ExecuteSQL(query);

                            break;
                        }
                    case "ВільніЗалишки":
                        {
                            string query = $@"
DELETE FROM {ВіртуальніТаблиціРегістрів.ВільніЗалишки_Місяць_TablePart.TABLE}
WHERE date_trunc('month', {ВіртуальніТаблиціРегістрів.ВільніЗалишки_Місяць_TablePart.Період}) = '{Місяць}';

INSERT INTO {ВіртуальніТаблиціРегістрів.ВільніЗалишки_Місяць_TablePart.TABLE}
(
    uid,
    {ВіртуальніТаблиціРегістрів.ВільніЗалишки_Місяць_TablePart.Період},
    {ВіртуальніТаблиціРегістрів.ВільніЗалишки_Місяць_TablePart.Номенклатура},
    {ВіртуальніТаблиціРегістрів.ВільніЗалишки_Місяць_TablePart.ХарактеристикаНоменклатури},
    {ВіртуальніТаблиціРегістрів.ВільніЗалишки_Місяць_TablePart.Склад},
    {ВіртуальніТаблиціРегістрів.ВільніЗалишки_Місяць_TablePart.ВНаявності},
    {ВіртуальніТаблиціРегістрів.ВільніЗалишки_Місяць_TablePart.ВРезервіЗіСкладу},
    {ВіртуальніТаблиціРегістрів.ВільніЗалишки_Місяць_TablePart.ВРезервіПідЗамовлення}
)
SELECT 
    uuid_generate_v4(),
    date_trunc('month', ВільніЗалишки_День.{ВіртуальніТаблиціРегістрів.ВільніЗалишки_День_TablePart.Період}) as Період,
    ВільніЗалишки_День.{ВіртуальніТаблиціРегістрів.ВільніЗалишки_День_TablePart.Номенклатура} AS Номенклатура, 
    ВільніЗалишки_День.{ВіртуальніТаблиціРегістрів.ВільніЗалишки_День_TablePart.ХарактеристикаНоменклатури} AS ХарактеристикаНоменклатури,
    ВільніЗалишки_День.{ВіртуальніТаблиціРегістрів.ВільніЗалишки_День_TablePart.Склад} AS Склад,
    SUM(ВільніЗалишки_День.{ВіртуальніТаблиціРегістрів.ВільніЗалишки_День_TablePart.ВНаявності}) AS ВНаявності,
    SUM(ВільніЗалишки_День.{ВіртуальніТаблиціРегістрів.ВільніЗалишки_День_TablePart.ВРезервіЗіСкладу}) AS ВРезервіЗіСкладу,
    SUM(ВільніЗалишки_День.{ВіртуальніТаблиціРегістрів.ВільніЗалишки_День_TablePart.ВРезервіПідЗамовлення}) AS ВРезервіПідЗамовлення
FROM 
    {ВіртуальніТаблиціРегістрів.ВільніЗалишки_День_TablePart.TABLE} AS ВільніЗалишки_День
WHERE
    date_trunc('month', ВільніЗалишки_День.{ВіртуальніТаблиціРегістрів.ВільніЗалишки_День_TablePart.Період}) = '{Місяць}'
GROUP BY 
    Період, Номенклатура, ХарактеристикаНоменклатури, Склад
HAVING 
   SUM(ВільніЗалишки_День.{ВіртуальніТаблиціРегістрів.ВільніЗалишки_День_TablePart.ВНаявності}) != 0 OR
   SUM(ВільніЗалишки_День.{ВіртуальніТаблиціРегістрів.ВільніЗалишки_День_TablePart.ВРезервіЗіСкладу}) != 0 OR
   SUM(ВільніЗалишки_День.{ВіртуальніТаблиціРегістрів.ВільніЗалишки_День_TablePart.ВРезервіПідЗамовлення}) != 0
";

                            //Console.WriteLine(query);
                            Config.KernelBackgroundTask.DataBase.ExecuteSQL(query);

                            break;
                        }
                }

                string queryUpdate = $@"
UPDATE {Системні.ФоновіЗадачі_АктуальністьВіртуальнихЗалишків_TablePart.TABLE}
    SET {Системні.ФоновіЗадачі_АктуальністьВіртуальнихЗалишків_TablePart.Актуально} = true
WHERE uid = '{uid}'
";
                Config.KernelBackgroundTask.DataBase.ExecuteSQL(queryUpdate);
            }
        }
    }


    //        public static void СписокЗадач()
    //        {
    //            string TempTable = "tmp_" + Guid.NewGuid().ToString().Replace("-", "");

    //            string queryTempTable = $@"
    //CREATE TEMP TABLE {TempTable} AS
    //SELECT
    //    Задачі.uid,
    //    Задачі.{Системні.ФоновіЗадачі_ОбчисленняВіртуальнихЗалишків_TablePart.Дата} AS Дата,
    //    Задачі.{Системні.ФоновіЗадачі_ОбчисленняВіртуальнихЗалишків_TablePart.НазваРегістру} AS Регістр,
    //    Задачі.{Системні.ФоновіЗадачі_ОбчисленняВіртуальнихЗалишків_TablePart.ГрупаОбчислення} AS Група,
    //    Задачі.{Системні.ФоновіЗадачі_ОбчисленняВіртуальнихЗалишків_TablePart.ПеріодОбчислення} AS Період,
    //    Задачі.{Системні.ФоновіЗадачі_ОбчисленняВіртуальнихЗалишків_TablePart.ТипРухуПоРегістру} AS ТипРуху
    //FROM
    //    {Системні.ФоновіЗадачі_ОбчисленняВіртуальнихЗалишків_TablePart.TABLE} AS Задачі
    //WHERE
    //    Задачі.{Системні.ФоновіЗадачі_ОбчисленняВіртуальнихЗалишків_TablePart.Виконано} = false AND 
    //    Задачі.{Системні.ФоновіЗадачі_ОбчисленняВіртуальнихЗалишків_TablePart.Заблоковано} = false
    //ORDER BY Дата ASC
    //;

    //UPDATE {Системні.ФоновіЗадачі_ОбчисленняВіртуальнихЗалишків_TablePart.TABLE}
    //    SET {Системні.ФоновіЗадачі_ОбчисленняВіртуальнихЗалишків_TablePart.Заблоковано} = true
    //WHERE 
    //    uid IN (SELECT uid FROM {TempTable});
    //";

    //            string querySelectTask = $@"
    //SELECT
    //    Задачі.Регістр,
    //    Задачі.Група,
    //    Задачі.Період,
    //    Задачі.ТипРуху
    //FROM
    //    {TempTable} AS Задачі
    //GROUP BY
    //    Регістр, Група, Період, ТипРуху
    //;
    //";
    //            //Console.WriteLine(queryTempTable + querySelectTask);

    //            string[] columnsName;
    //            List<object[]> listRow;

    //            Console.WriteLine("----------------");

    //            Config.KernelBackgroundTask.DataBase.BeginTransaction();
    //            Config.KernelBackgroundTask.DataBase.ExecuteSQL(queryTempTable);

    //            Config.KernelBackgroundTask.DataBase.SelectRequest(querySelectTask, null, out columnsName, out listRow);

    //            foreach (object[] row in listRow)
    //            {
    //                string Регістр = row[0].ToString();
    //                string Група = row[1].ToString();
    //                string Період = row[2].ToString();
    //                string ТипРуху = row[3].ToString();

    //                Console.WriteLine($"Регістр: {Регістр} Група: {Група} Період:{Період} ТипРуху:{ТипРуху}");

    //                switch (Регістр)
    //                {
    //                    case "ЗамовленняКлієнтів":
    //                        {
    //                            string query = $@"
    //DELETE FROM {ВіртуальніТаблиціРегістрів.ЗамовленняКлієнтів_День_TablePart.TABLE}
    //WHERE date_trunc('day', {ВіртуальніТаблиціРегістрів.ЗамовленняКлієнтів_День_TablePart.Період}) = '{Період}';
    //";

    //                            if (ТипРуху == "Add")
    //                            {
    //                                query += $@"
    //INSERT INTO {ВіртуальніТаблиціРегістрів.ЗамовленняКлієнтів_День_TablePart.TABLE}
    //(
    //    uid,
    //    {ВіртуальніТаблиціРегістрів.ЗамовленняКлієнтів_День_TablePart.Період},
    //    {ВіртуальніТаблиціРегістрів.ЗамовленняКлієнтів_День_TablePart.Номенклатура},
    //    {ВіртуальніТаблиціРегістрів.ЗамовленняКлієнтів_День_TablePart.ХарактеристикиНоменклатури},
    //    {ВіртуальніТаблиціРегістрів.ЗамовленняКлієнтів_День_TablePart.Склад},
    //    {ВіртуальніТаблиціРегістрів.ЗамовленняКлієнтів_День_TablePart.Замовлено},
    //    {ВіртуальніТаблиціРегістрів.ЗамовленняКлієнтів_День_TablePart.Сума}
    //)
    //SELECT 
    //    uuid_generate_v4(),
    //    date_trunc('day', Рег_ЗамовленняКлієнтів.period) as period_month,
    //    Рег_ЗамовленняКлієнтів.{ЗамовленняКлієнтів_Const.Номенклатура} AS Номенклатура, 
    //    Рег_ЗамовленняКлієнтів.{ЗамовленняКлієнтів_Const.ХарактеристикаНоменклатури} AS ХарактеристикаНоменклатури,
    //    Рег_ЗамовленняКлієнтів.{ЗамовленняКлієнтів_Const.Склад} AS Склад,
    //    SUM(CASE WHEN Рег_ЗамовленняКлієнтів.income = true THEN 
    //        Рег_ЗамовленняКлієнтів.{ЗамовленняКлієнтів_Const.Замовлено} ELSE 
    //       -Рег_ЗамовленняКлієнтів.{ЗамовленняКлієнтів_Const.Замовлено} END) AS Замовлено,
    //    SUM(CASE WHEN Рег_ЗамовленняКлієнтів.income = true THEN 
    //        Рег_ЗамовленняКлієнтів.{ЗамовленняКлієнтів_Const.Сума} ELSE 
    //       -Рег_ЗамовленняКлієнтів.{ЗамовленняКлієнтів_Const.Сума} END) AS Сума
    //FROM 
    //    {ЗамовленняКлієнтів_Const.TABLE} AS Рег_ЗамовленняКлієнтів
    //WHERE
    //    date_trunc('day', Рег_ЗамовленняКлієнтів.period) = '{Період}'
    //GROUP BY 
    //    period_month, Номенклатура, ХарактеристикаНоменклатури, Склад
    //HAVING 
    //   SUM(CASE WHEN Рег_ЗамовленняКлієнтів.income = true THEN 
    //        Рег_ЗамовленняКлієнтів.{ЗамовленняКлієнтів_Const.Замовлено} ELSE 
    //       -Рег_ЗамовленняКлієнтів.{ЗамовленняКлієнтів_Const.Замовлено} END) != 0
    //OR
    //    SUM(CASE WHEN Рег_ЗамовленняКлієнтів.income = true THEN 
    //        Рег_ЗамовленняКлієнтів.{ЗамовленняКлієнтів_Const.Сума} ELSE 
    //       -Рег_ЗамовленняКлієнтів.{ЗамовленняКлієнтів_Const.Сума} END) != 0
    //";
    //                            }

    //                            //Console.WriteLine(query);
    //                            Config.KernelBackgroundTask.DataBase.ExecuteSQL(query);

    //                            break;
    //                        }
    //                    case "ТовариНаСкладах":
    //                        {
    //                            string query = $@"
    //DELETE FROM {ВіртуальніТаблиціРегістрів.ТовариНаСкладах_День_TablePart.TABLE}
    //WHERE date_trunc('day', {ВіртуальніТаблиціРегістрів.ТовариНаСкладах_День_TablePart.Період}) = '{Період}';
    //";

    //                            if (ТипРуху == "Add")
    //                            {
    //                                query += $@"
    //INSERT INTO {ВіртуальніТаблиціРегістрів.ТовариНаСкладах_День_TablePart.TABLE}
    //(
    //    uid,
    //    {ВіртуальніТаблиціРегістрів.ТовариНаСкладах_День_TablePart.Період},
    //    {ВіртуальніТаблиціРегістрів.ТовариНаСкладах_День_TablePart.Номенклатура},
    //    {ВіртуальніТаблиціРегістрів.ТовариНаСкладах_День_TablePart.ХарактеристикаНоменклатури},
    //    {ВіртуальніТаблиціРегістрів.ТовариНаСкладах_День_TablePart.Склад},
    //    {ВіртуальніТаблиціРегістрів.ТовариНаСкладах_День_TablePart.ВНаявності},
    //    {ВіртуальніТаблиціРегістрів.ТовариНаСкладах_День_TablePart.ДоВідвантаження}
    //)
    //SELECT 
    //    uuid_generate_v4(),
    //    date_trunc('day', Рег_ТовариНаСкладах.period) as period_day,
    //    Рег_ТовариНаСкладах.{ТовариНаСкладах_Const.Номенклатура} AS Номенклатура, 
    //    Рег_ТовариНаСкладах.{ТовариНаСкладах_Const.ХарактеристикаНоменклатури} AS ХарактеристикаНоменклатури,
    //    Рег_ТовариНаСкладах.{ТовариНаСкладах_Const.Склад} AS Склад,
    //    SUM(CASE WHEN Рег_ТовариНаСкладах.income = true THEN 
    //        Рег_ТовариНаСкладах.{ТовариНаСкладах_Const.ВНаявності} ELSE 
    //       -Рег_ТовариНаСкладах.{ТовариНаСкладах_Const.ВНаявності} END) AS ВНаявності,
    //    SUM(CASE WHEN Рег_ТовариНаСкладах.income = true THEN 
    //        Рег_ТовариНаСкладах.{ТовариНаСкладах_Const.ДоВідвантаження} ELSE 
    //       -Рег_ТовариНаСкладах.{ТовариНаСкладах_Const.ДоВідвантаження} END) AS ДоВідвантаження
    //FROM 
    //    {ТовариНаСкладах_Const.TABLE} AS Рег_ТовариНаСкладах
    //WHERE
    //    date_trunc('day', Рег_ТовариНаСкладах.period) = '{Період}'
    //GROUP BY 
    //    period_day, Номенклатура, ХарактеристикаНоменклатури, Склад
    //HAVING 
    //   SUM(CASE WHEN Рег_ТовариНаСкладах.income = true THEN 
    //        Рег_ТовариНаСкладах.{ТовариНаСкладах_Const.ВНаявності} ELSE 
    //       -Рег_ТовариНаСкладах.{ТовариНаСкладах_Const.ВНаявності} END) != 0
    //OR
    //    SUM(CASE WHEN Рег_ТовариНаСкладах.income = true THEN 
    //        Рег_ТовариНаСкладах.{ТовариНаСкладах_Const.ДоВідвантаження} ELSE 
    //       -Рег_ТовариНаСкладах.{ТовариНаСкладах_Const.ДоВідвантаження} END) != 0
    //";
    //                            }

    //                            //Console.WriteLine(query);
    //                            Config.KernelBackgroundTask.DataBase.ExecuteSQL(query);

    //                            break;
    //                        }
    //                    case "РозрахункиЗКлієнтами":
    //                        {
    //                            string query = $@"
    //DELETE FROM {ВіртуальніТаблиціРегістрів.РозрахункиЗКлієнтами_День_TablePart.TABLE}
    //WHERE date_trunc('day', {ВіртуальніТаблиціРегістрів.РозрахункиЗКлієнтами_День_TablePart.Період}) = '{Період}';
    //";
    //                            if (ТипРуху == "Add")
    //                            {
    //                                query += $@"
    //INSERT INTO {ВіртуальніТаблиціРегістрів.РозрахункиЗКлієнтами_День_TablePart.TABLE}
    //(
    //    uid,
    //    {ВіртуальніТаблиціРегістрів.РозрахункиЗКлієнтами_День_TablePart.Період},
    //    {ВіртуальніТаблиціРегістрів.РозрахункиЗКлієнтами_День_TablePart.Валюта},
    //    {ВіртуальніТаблиціРегістрів.РозрахункиЗКлієнтами_День_TablePart.Контрагент},
    //    {ВіртуальніТаблиціРегістрів.РозрахункиЗКлієнтами_День_TablePart.Сума}
    //)
    //SELECT 
    //    uuid_generate_v4(),
    //    date_trunc('day', Рег_РозрахункиЗКлієнтами.period) as period_month,
    //    Рег_РозрахункиЗКлієнтами.{РозрахункиЗКлієнтами_Const.Валюта} AS Валюта, 
    //    Рег_РозрахункиЗКлієнтами.{РозрахункиЗКлієнтами_Const.Контрагент} AS Контрагент,
    //    SUM(CASE WHEN Рег_РозрахункиЗКлієнтами.income = true THEN 
    //        Рег_РозрахункиЗКлієнтами.{РозрахункиЗКлієнтами_Const.Сума} ELSE 
    //       -Рег_РозрахункиЗКлієнтами.{РозрахункиЗКлієнтами_Const.Сума} END) AS Сума
    //FROM 
    //    {РозрахункиЗКлієнтами_Const.TABLE} AS Рег_РозрахункиЗКлієнтами
    //WHERE
    //    date_trunc('day', Рег_РозрахункиЗКлієнтами.period) = '{Період}'
    //GROUP BY 
    //    period_month, Валюта, Контрагент
    //HAVING
    //   SUM(CASE WHEN Рег_РозрахункиЗКлієнтами.income = true THEN 
    //        Рег_РозрахункиЗКлієнтами.{РозрахункиЗКлієнтами_Const.Сума} ELSE 
    //       -Рег_РозрахункиЗКлієнтами.{РозрахункиЗКлієнтами_Const.Сума} END) != 0
    //";
    //                            }

    //                            //Console.WriteLine(query);
    //                            Config.KernelBackgroundTask.DataBase.ExecuteSQL(query);

    //                            break;
    //                        }
    //                    case "РозрахункиЗПостачальниками":
    //                        {
    //                            string query = $@"
    //DELETE FROM {ВіртуальніТаблиціРегістрів.РозрахункиЗПостачальниками_День_TablePart.TABLE}
    //WHERE date_trunc('day', {ВіртуальніТаблиціРегістрів.РозрахункиЗПостачальниками_День_TablePart.Період}) = '{Період}';
    //";
    //                            if (ТипРуху == "Add")
    //                            {
    //                                query += $@"
    //INSERT INTO {ВіртуальніТаблиціРегістрів.РозрахункиЗПостачальниками_День_TablePart.TABLE}
    //(
    //    uid,
    //    {ВіртуальніТаблиціРегістрів.РозрахункиЗПостачальниками_День_TablePart.Період},
    //    {ВіртуальніТаблиціРегістрів.РозрахункиЗПостачальниками_День_TablePart.Валюта},
    //    {ВіртуальніТаблиціРегістрів.РозрахункиЗПостачальниками_День_TablePart.Контрагент},
    //    {ВіртуальніТаблиціРегістрів.РозрахункиЗПостачальниками_День_TablePart.Сума}
    //)
    //SELECT 
    //    uuid_generate_v4(),
    //    date_trunc('day', Рег_РозрахункиЗПостачальниками.period) as period_month,
    //    Рег_РозрахункиЗПостачальниками.{РозрахункиЗПостачальниками_Const.Валюта} AS Валюта, 
    //    Рег_РозрахункиЗПостачальниками.{РозрахункиЗПостачальниками_Const.Контрагент} AS Контрагент,
    //    SUM(CASE WHEN Рег_РозрахункиЗПостачальниками.income = true THEN 
    //        Рег_РозрахункиЗПостачальниками.{РозрахункиЗПостачальниками_Const.Сума} ELSE 
    //       -Рег_РозрахункиЗПостачальниками.{РозрахункиЗПостачальниками_Const.Сума} END) AS Сума
    //FROM 
    //    {РозрахункиЗПостачальниками_Const.TABLE} AS Рег_РозрахункиЗПостачальниками
    //WHERE
    //    date_trunc('day', Рег_РозрахункиЗПостачальниками.period) = '{Період}'
    //GROUP BY 
    //    period_month, Валюта, Контрагент
    //HAVING
    //   SUM(CASE WHEN Рег_РозрахункиЗПостачальниками.income = true THEN 
    //        Рег_РозрахункиЗПостачальниками.{РозрахункиЗПостачальниками_Const.Сума} ELSE 
    //       -Рег_РозрахункиЗПостачальниками.{РозрахункиЗПостачальниками_Const.Сума} END) != 0
    //";
    //                            }

    //                            //Console.WriteLine(query);
    //                            Config.KernelBackgroundTask.DataBase.ExecuteSQL(query);

    //                            break;
    //                        }
    //                    case "ЗамовленняПостачальникам":
    //                        {
    //                            string query = $@"
    //DELETE FROM {ВіртуальніТаблиціРегістрів.ЗамовленняПостачальникам_День_TablePart.TABLE}
    //WHERE date_trunc('day', {ВіртуальніТаблиціРегістрів.ЗамовленняПостачальникам_День_TablePart.Період}) = '{Період}';
    //";
    //                            if (ТипРуху == "Add")
    //                            {
    //                                query += $@"
    //INSERT INTO {ВіртуальніТаблиціРегістрів.ЗамовленняПостачальникам_День_TablePart.TABLE}
    //(
    //    uid,
    //    {ВіртуальніТаблиціРегістрів.ЗамовленняПостачальникам_День_TablePart.Період},
    //    {ВіртуальніТаблиціРегістрів.ЗамовленняПостачальникам_День_TablePart.Номенклатура},
    //    {ВіртуальніТаблиціРегістрів.ЗамовленняПостачальникам_День_TablePart.ХарактеристикиНоменклатури},
    //    {ВіртуальніТаблиціРегістрів.ЗамовленняПостачальникам_День_TablePart.Склад},
    //    {ВіртуальніТаблиціРегістрів.ЗамовленняПостачальникам_День_TablePart.Замовлено}
    //)
    //SELECT 
    //    uuid_generate_v4(),
    //    date_trunc('day', Рег_ЗамовленняПостачальникам.period) as period_month,
    //    Рег_ЗамовленняПостачальникам.{ЗамовленняПостачальникам_Const.Номенклатура} AS Номенклатура, 
    //    Рег_ЗамовленняПостачальникам.{ЗамовленняПостачальникам_Const.ХарактеристикаНоменклатури} AS ХарактеристикаНоменклатури,
    //    Рег_ЗамовленняПостачальникам.{ЗамовленняПостачальникам_Const.Склад} AS Склад,
    //    SUM(CASE WHEN Рег_ЗамовленняПостачальникам.income = true THEN 
    //        Рег_ЗамовленняПостачальникам.{ЗамовленняПостачальникам_Const.Замовлено} ELSE 
    //       -Рег_ЗамовленняПостачальникам.{ЗамовленняПостачальникам_Const.Замовлено} END) AS Замовлено
    //FROM 
    //    {ЗамовленняПостачальникам_Const.TABLE} AS Рег_ЗамовленняПостачальникам
    //WHERE
    //    date_trunc('day', Рег_ЗамовленняПостачальникам.period) = '{Період}'
    //GROUP BY 
    //    period_month, Номенклатура, ХарактеристикаНоменклатури, Склад
    //HAVING 
    //   SUM(CASE WHEN Рег_ЗамовленняПостачальникам.income = true THEN 
    //        Рег_ЗамовленняПостачальникам.{ЗамовленняПостачальникам_Const.Замовлено} ELSE 
    //       -Рег_ЗамовленняПостачальникам.{ЗамовленняПостачальникам_Const.Замовлено} END) != 0
    //";
    //                            }

    //                            //Console.WriteLine(query);
    //                            Config.KernelBackgroundTask.DataBase.ExecuteSQL(query);

    //                            break;
    //                        }
    //                    case "ВільніЗалишки":
    //                        {
    //                            string query = $@"
    //DELETE FROM {ВіртуальніТаблиціРегістрів.ВільніЗалишки_День_TablePart.TABLE}
    //WHERE date_trunc('day', {ВіртуальніТаблиціРегістрів.ВільніЗалишки_День_TablePart.Період}) = '{Період}';
    //";

    //                            if (ТипРуху == "Add")
    //                            {
    //                                query += $@"
    //INSERT INTO {ВіртуальніТаблиціРегістрів.ВільніЗалишки_День_TablePart.TABLE}
    //(
    //    uid,
    //    {ВіртуальніТаблиціРегістрів.ВільніЗалишки_День_TablePart.Період},
    //    {ВіртуальніТаблиціРегістрів.ВільніЗалишки_День_TablePart.Номенклатура},
    //    {ВіртуальніТаблиціРегістрів.ВільніЗалишки_День_TablePart.ХарактеристикиНоменклатури},
    //    {ВіртуальніТаблиціРегістрів.ВільніЗалишки_День_TablePart.Склад},
    //    {ВіртуальніТаблиціРегістрів.ВільніЗалишки_День_TablePart.ВНаявності},
    //    {ВіртуальніТаблиціРегістрів.ВільніЗалишки_День_TablePart.ВРезервіЗіСкладу},
    //    {ВіртуальніТаблиціРегістрів.ВільніЗалишки_День_TablePart.ВРезервіПідЗамовлення}
    //)
    //SELECT 
    //    uuid_generate_v4(),
    //    date_trunc('day', Рег_ВільніЗалишки.period) as period_month,
    //    Рег_ВільніЗалишки.{ВільніЗалишки_Const.Номенклатура} AS Номенклатура, 
    //    Рег_ВільніЗалишки.{ВільніЗалишки_Const.ХарактеристикаНоменклатури} AS ХарактеристикаНоменклатури,
    //    Рег_ВільніЗалишки.{ВільніЗалишки_Const.Склад} AS Склад,
    //    SUM(CASE WHEN Рег_ВільніЗалишки.income = true THEN 
    //        Рег_ВільніЗалишки.{ВільніЗалишки_Const.ВНаявності} ELSE 
    //       -Рег_ВільніЗалишки.{ВільніЗалишки_Const.ВНаявності} END) AS ВНаявності,
    //    SUM(CASE WHEN Рег_ВільніЗалишки.income = true THEN 
    //        Рег_ВільніЗалишки.{ВільніЗалишки_Const.ВРезервіЗіСкладу} ELSE 
    //       -Рег_ВільніЗалишки.{ВільніЗалишки_Const.ВРезервіЗіСкладу} END) AS ВРезервіЗіСкладу,
    //    SUM(CASE WHEN Рег_ВільніЗалишки.income = true THEN 
    //        Рег_ВільніЗалишки.{ВільніЗалишки_Const.ВРезервіПідЗамовлення} ELSE 
    //       -Рег_ВільніЗалишки.{ВільніЗалишки_Const.ВРезервіПідЗамовлення} END) AS ВРезервіПідЗамовлення
    //FROM 
    //    {ВільніЗалишки_Const.TABLE} AS Рег_ВільніЗалишки
    //WHERE
    //    date_trunc('day', Рег_ВільніЗалишки.period) = '{Період}'
    //GROUP BY 
    //    period_month, Номенклатура, ХарактеристикаНоменклатури, Склад
    //HAVING 
    //   SUM(CASE WHEN Рег_ВільніЗалишки.income = true THEN 
    //        Рег_ВільніЗалишки.{ВільніЗалишки_Const.ВНаявності} ELSE 
    //       -Рег_ВільніЗалишки.{ВільніЗалишки_Const.ВНаявності} END) != 0
    //OR
    //    SUM(CASE WHEN Рег_ВільніЗалишки.income = true THEN 
    //        Рег_ВільніЗалишки.{ВільніЗалишки_Const.ВРезервіЗіСкладу} ELSE 
    //       -Рег_ВільніЗалишки.{ВільніЗалишки_Const.ВРезервіЗіСкладу} END) != 0
    //OR
    //    SUM(CASE WHEN Рег_ВільніЗалишки.income = true THEN 
    //        Рег_ВільніЗалишки.{ВільніЗалишки_Const.ВРезервіПідЗамовлення} ELSE 
    //       -Рег_ВільніЗалишки.{ВільніЗалишки_Const.ВРезервіПідЗамовлення} END) != 0
    //";
    //                            }

    //                            //Console.WriteLine(query);
    //                            Config.KernelBackgroundTask.DataBase.ExecuteSQL(query);

    //                            break;
    //                        }
    //                }
    //            }

    //            string queryUpdate = $@"
    //UPDATE {Системні.ФоновіЗадачі_ОбчисленняВіртуальнихЗалишків_TablePart.TABLE}
    //    SET {Системні.ФоновіЗадачі_ОбчисленняВіртуальнихЗалишків_TablePart.Виконано} = true,
    //        {Системні.ФоновіЗадачі_ОбчисленняВіртуальнихЗалишків_TablePart.Заблоковано} = false
    //WHERE 
    //    uid IN (SELECT uid FROM {TempTable});

    //DROP TABLE {TempTable};
    //";
    //            //Console.WriteLine(queryUpdate);
    //            Config.KernelBackgroundTask.DataBase.ExecuteSQL(queryUpdate);
    //            Config.KernelBackgroundTask.DataBase.CommitTransaction();
    //        }


    /************************/

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
    {ВіртуальніТаблиціРегістрів.ЗамовленняКлієнтів_Місяць_TablePart.ХарактеристикаНоменклатури},
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
    {ВіртуальніТаблиціРегістрів.ЗамовленняПостачальникам_Місяць_TablePart.ХарактеристикаНоменклатури},
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
    {ВіртуальніТаблиціРегістрів.ВільніЗалишки_Місяць_TablePart.ХарактеристикаНоменклатури},
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