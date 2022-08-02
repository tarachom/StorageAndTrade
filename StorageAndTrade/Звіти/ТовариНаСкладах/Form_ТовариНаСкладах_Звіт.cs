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
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Xml;
using System.IO;

using AccountingSoftware;
using StorageAndTrade_1_0;
using StorageAndTrade_1_0.Константи;
using StorageAndTrade_1_0.Довідники;
using StorageAndTrade_1_0.Документи;
using StorageAndTrade_1_0.РегістриНакопичення;
using StorageAndTrade_1_0.Звіти;
using StorageAndTrade_1_0.Журнали;

namespace StorageAndTrade
{
    public partial class Form_ТовариНаСкладах_Звіт : Form
    {
        public Form_ТовариНаСкладах_Звіт()
        {
            InitializeComponent();
        }

        private void Form_ЗамовленняКлієнтів_Звіт_Load(object sender, EventArgs e)
        {
            directoryControl_НоменклатураПапка.Init(new Form_НоменклатураПапкиВибір(), new Номенклатура_Папки_Pointer());
            directoryControl_Номенклатура.Init(new Form_Номенклатура(), new Номенклатура_Pointer());
            directoryControl_ХарактеристикаНоменклатури.Init(new Form_ХарактеристикиНоменклатури(), new ХарактеристикиНоменклатури_Pointer());
            directoryControl_СкладиПапки.Init(new Form_СкладиПапкиВибір(), new Склади_Папки_Pointer());
            directoryControl_Склади.Init(new Form_Склади(), new Склади_Pointer());
        }

        private void buttonbuttonOstatok_Click(object sender, EventArgs e)
        {
            bool isExistParent = false;

            string query = $@"
SELECT 
    ТовариНаСкладах_Місяць.{ВіртуальніТаблиціРегістрів.ТовариНаСкладах_Місяць_TablePart.Номенклатура} AS Номенклатура, 
    Довідник_Номенклатура.{Номенклатура_Const.Назва} AS Номенклатура_Назва, 
    ТовариНаСкладах_Місяць.{ВіртуальніТаблиціРегістрів.ТовариНаСкладах_Місяць_TablePart.ХарактеристикаНоменклатури} AS ХарактеристикаНоменклатури,
    Довідник_ХарактеристикиНоменклатури.{ХарактеристикиНоменклатури_Const.Назва} AS ХарактеристикаНоменклатури_Назва,
    ТовариНаСкладах_Місяць.{ВіртуальніТаблиціРегістрів.ТовариНаСкладах_Місяць_TablePart.Склад} AS Склад,
    Довідник_Склади.{Склади_Const.Назва} AS Склад_Назва, 
    SUM(ТовариНаСкладах_Місяць.{ВіртуальніТаблиціРегістрів.ТовариНаСкладах_Місяць_TablePart.ВНаявності}) AS ВНаявності,
    SUM(ТовариНаСкладах_Місяць.{ВіртуальніТаблиціРегістрів.ТовариНаСкладах_Місяць_TablePart.ДоВідвантаження}) AS ДоВідвантаження
FROM 
    {ВіртуальніТаблиціРегістрів.ТовариНаСкладах_Місяць_TablePart.TABLE} AS ТовариНаСкладах_Місяць

    LEFT JOIN {Номенклатура_Const.TABLE} AS Довідник_Номенклатура ON Довідник_Номенклатура.uid = 
        ТовариНаСкладах_Місяць.{ВіртуальніТаблиціРегістрів.ТовариНаСкладах_Місяць_TablePart.Номенклатура}

    LEFT JOIN {ХарактеристикиНоменклатури_Const.TABLE} AS Довідник_ХарактеристикиНоменклатури ON Довідник_ХарактеристикиНоменклатури.uid = 
        ТовариНаСкладах_Місяць.{ВіртуальніТаблиціРегістрів.ТовариНаСкладах_Місяць_TablePart.ХарактеристикаНоменклатури}

    LEFT JOIN {Склади_Const.TABLE} AS Довідник_Склади ON Довідник_Склади.uid = 
        ТовариНаСкладах_Місяць.{ВіртуальніТаблиціРегістрів.ТовариНаСкладах_Місяць_TablePart.Склад}
";
            #region WHERE

            //Відбір по всіх вкладених папках вибраної папки Номенклатури
            if (!directoryControl_НоменклатураПапка.DirectoryPointerItem.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
Довідник_Номенклатура.{Номенклатура_Const.Папка} IN 
    (
        WITH RECURSIVE r AS 
        (
            SELECT uid
            FROM {Номенклатура_Папки_Const.TABLE}
            WHERE {Номенклатура_Папки_Const.TABLE}.uid = '{directoryControl_НоменклатураПапка.DirectoryPointerItem.UnigueID}' 

            UNION ALL

            SELECT {Номенклатура_Папки_Const.TABLE}.uid
            FROM {Номенклатура_Папки_Const.TABLE}
                JOIN r ON {Номенклатура_Папки_Const.TABLE}.{Номенклатура_Папки_Const.Родич} = r.uid
        ) SELECT uid FROM r
    )
";
            }

            //Відбір по вибраному елементу Номенклатура
            if (!directoryControl_Номенклатура.DirectoryPointerItem.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
Довідник_Номенклатура.uid = '{directoryControl_Номенклатура.DirectoryPointerItem.UnigueID}'
";
            }

            //Відбір по вибраному елементу Характеристики Номенклатури
            if (!directoryControl_ХарактеристикаНоменклатури.DirectoryPointerItem.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
Довідник_ХарактеристикиНоменклатури.uid = '{directoryControl_ХарактеристикаНоменклатури.DirectoryPointerItem.UnigueID}'
";
            }

            //Відбір по всіх вкладених папках вибраної папки Склади
            if (!directoryControl_СкладиПапки.DirectoryPointerItem.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
Довідник_Склади.{Склади_Const.Папка} IN 
    (
        WITH RECURSIVE r AS 
        (
            SELECT uid
            FROM {Склади_Папки_Const.TABLE}
            WHERE {Склади_Папки_Const.TABLE}.uid = '{directoryControl_СкладиПапки.DirectoryPointerItem.UnigueID}' 

            UNION ALL

            SELECT {Склади_Папки_Const.TABLE}.uid
            FROM {Склади_Папки_Const.TABLE}
                JOIN r ON {Склади_Папки_Const.TABLE}.{Склади_Папки_Const.Родич} = r.uid
        ) SELECT uid FROM r
    )
";
            }

            //Відбір по вибраному елементу Склади
            if (!directoryControl_Склади.DirectoryPointerItem.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
Довідник_Склади.uid = '{directoryControl_Склади.DirectoryPointerItem.UnigueID}'
";
            }

            #endregion

            query += $@"
GROUP BY Номенклатура, Номенклатура_Назва, 
         ХарактеристикаНоменклатури, ХарактеристикаНоменклатури_Назва,
         Склад, Склад_Назва

HAVING
    SUM(ТовариНаСкладах_Місяць.{ВіртуальніТаблиціРегістрів.ТовариНаСкладах_Місяць_TablePart.ВНаявності}) != 0 OR
    SUM(ТовариНаСкладах_Місяць.{ВіртуальніТаблиціРегістрів.ТовариНаСкладах_Місяць_TablePart.ДоВідвантаження}) != 0   

ORDER BY Номенклатура_Назва
";

            //Console.WriteLine(query);

            XmlDocument xmlDoc = Функції.CreateXmlDocument();

            Функції.DataHeadToXML(xmlDoc, "head",
                new List<NameValue<string>>()
                {
                    new NameValue<string>("КінецьПеріоду", DateTime.Now.ToString("dd.MM.yyyy"))
                }
            );

            Dictionary<string, object> paramQuery = new Dictionary<string, object>();

            string[] columnsName;
            List<object[]> listRow;

            Config.Kernel.DataBase.SelectRequest(query, paramQuery, out columnsName, out listRow);

            Функції.DataToXML(xmlDoc, "ТовариНаСкладах", columnsName, listRow);

            Функції.XmlDocumentSaveAndTransform(xmlDoc, @"Шаблони\ТовариНаСкладах_Залишки.xslt", false, "Товари на складах");

            string pathToHtmlFile = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "Report.html");
            geckoWebBrowser.Navigate(pathToHtmlFile);
        }

        void ЗалишкиТаОбороти(XmlDocument xmlDoc)
        {
            bool isExistParent = false;

            string query = $@"
WITH ostatok_month AS
(
    SELECT
        'month' AS block,
        ТовариНаСкладах_Місяць.{ВіртуальніТаблиціРегістрів.ТовариНаСкладах_Місяць_TablePart.Номенклатура} AS Номенклатура,
        ТовариНаСкладах_Місяць.{ВіртуальніТаблиціРегістрів.ТовариНаСкладах_Місяць_TablePart.ХарактеристикаНоменклатури} AS ХарактеристикаНоменклатури,
        ТовариНаСкладах_Місяць.{ВіртуальніТаблиціРегістрів.ТовариНаСкладах_Місяць_TablePart.Склад} AS Склад,
        SUM(ТовариНаСкладах_Місяць.{ВіртуальніТаблиціРегістрів.ТовариНаСкладах_Місяць_TablePart.ВНаявності}) AS ВНаявності
    FROM 
        {ВіртуальніТаблиціРегістрів.ТовариНаСкладах_Місяць_TablePart.TABLE} AS ТовариНаСкладах_Місяць
    WHERE
        ТовариНаСкладах_Місяць.{ВіртуальніТаблиціРегістрів.ТовариНаСкладах_Місяць_TablePart.Період} < @period_month_end

    GROUP BY Номенклатура, ХарактеристикаНоменклатури, Склад
), 
ostatok_day AS
(
    SELECT
        'day' AS block,
        ТовариНаСкладах_День.{ВіртуальніТаблиціРегістрів.ТовариНаСкладах_День_TablePart.Номенклатура} AS Номенклатура,
        ТовариНаСкладах_День.{ВіртуальніТаблиціРегістрів.ТовариНаСкладах_День_TablePart.ХарактеристикаНоменклатури} AS ХарактеристикаНоменклатури,
        ТовариНаСкладах_День.{ВіртуальніТаблиціРегістрів.ТовариНаСкладах_День_TablePart.Склад} AS Склад,
        SUM(ТовариНаСкладах_День.{ВіртуальніТаблиціРегістрів.ТовариНаСкладах_День_TablePart.ВНаявності}) AS ВНаявності
    FROM 
        {ВіртуальніТаблиціРегістрів.ТовариНаСкладах_День_TablePart.TABLE} AS ТовариНаСкладах_День
    WHERE
        ТовариНаСкладах_День.{ВіртуальніТаблиціРегістрів.ТовариНаСкладах_День_TablePart.Період} >= @period_day_start AND
        ТовариНаСкладах_День.{ВіртуальніТаблиціРегістрів.ТовариНаСкладах_День_TablePart.Період } < @period_day_end

    GROUP BY Номенклатура, ХарактеристикаНоменклатури, Склад
), 
ostatok_period AS
(   
    SELECT
        'period' AS block,
        ТовариНаСкладах_День.{ВіртуальніТаблиціРегістрів.ТовариНаСкладах_День_TablePart.Номенклатура} AS Номенклатура,
        ТовариНаСкладах_День.{ВіртуальніТаблиціРегістрів.ТовариНаСкладах_День_TablePart.ХарактеристикаНоменклатури} AS ХарактеристикаНоменклатури,
        ТовариНаСкладах_День.{ВіртуальніТаблиціРегістрів.ТовариНаСкладах_День_TablePart.Склад} AS Склад,
        SUM(ТовариНаСкладах_День.{ВіртуальніТаблиціРегістрів.ТовариНаСкладах_День_TablePart.ВНаявності}) AS ВНаявності
    FROM 
        {ВіртуальніТаблиціРегістрів.ТовариНаСкладах_День_TablePart.TABLE} AS ТовариНаСкладах_День
    WHERE
        ТовариНаСкладах_День.{ВіртуальніТаблиціРегістрів.ТовариНаСкладах_День_TablePart.Період} >= @period_ostatok_start AND
        ТовариНаСкладах_День.{ВіртуальніТаблиціРегістрів.ТовариНаСкладах_День_TablePart.Період } <= @period_ostatok_end

    GROUP BY Номенклатура, ХарактеристикаНоменклатури, Склад
),
ostatok_na_potshatok_periodu AS
(
    SELECT
       Номенклатура,
       ХарактеристикаНоменклатури,
       Склад,
       SUM(ВНаявності) AS ВНаявності
    FROM 
    (
        SELECT * FROM ostatok_month
        UNION
        SELECT * FROM ostatok_day
    ) AS ostatok
    GROUP BY Номенклатура, ХарактеристикаНоменклатури, Склад
),
ostatok_na_kinec_periodu AS
(
    SELECT
       Номенклатура,
       ХарактеристикаНоменклатури,
       Склад,
       SUM(ВНаявності) AS ВНаявності
    FROM 
    (
        SELECT * FROM ostatok_month
        UNION
        SELECT * FROM ostatok_day
        UNION
        SELECT * FROM ostatok_period
    ) AS ostatok
    GROUP BY Номенклатура, ХарактеристикаНоменклатури, Склад
),
oborot AS
(
    SELECT 
        ТовариНаСкладах.{ТовариНаСкладах_Const.Номенклатура} AS Номенклатура,
        ТовариНаСкладах.{ТовариНаСкладах_Const.ХарактеристикаНоменклатури} AS ХарактеристикаНоменклатури,
        ТовариНаСкладах.{ТовариНаСкладах_Const.Склад} AS Склад,
        SUM(CASE WHEN ТовариНаСкладах.income = true THEN ТовариНаСкладах.{ТовариНаСкладах_Const.ВНаявності} END) AS Прихід,
        SUM(CASE WHEN ТовариНаСкладах.income = false THEN ТовариНаСкладах.{ТовариНаСкладах_Const.ВНаявності} END) AS Розхід
    FROM
        {ТовариНаСкладах_Const.TABLE} AS ТовариНаСкладах
    WHERE
        ТовариНаСкладах.period >= @period_oborot_start AND
        ТовариНаСкладах.period <= @period_oborot_end
    GROUP BY Номенклатура, ХарактеристикаНоменклатури, Склад
)

SELECT 
    Номенклатура,
    Довідник_Номенклатура.{Номенклатура_Const.Назва} AS Номенклатура_Назва,
    ХарактеристикаНоменклатури,
    Довідник_ХарактеристикиНоменклатури.{ХарактеристикиНоменклатури_Const.Назва} AS ХарактеристикаНоменклатури_Назва,
    Склад,
    Довідник_Склади.{Склади_Const.Назва} AS Склад_Назва,
    SUM(ПочатковийЗалишок) AS ПочатковийЗалишок,
    SUM(Прихід) AS Прихід,
    SUM(Розхід) AS Розхід,
    SUM(Прихід) - SUM(Розхід) AS Оборот,
    SUM(КінцевийЗалишок) AS КінцевийЗалишок
FROM 
(
    SELECT 
        'A',
        Номенклатура,
        ХарактеристикаНоменклатури,
        Склад,
        ВНаявності AS ПочатковийЗалишок,
        0 AS Прихід,
        0 AS Розхід,
        0 AS КінцевийЗалишок
    FROM ostatok_na_potshatok_periodu

    UNION

    SELECT
        'B',
        Номенклатура,
        ХарактеристикаНоменклатури,
        Склад,
        0 AS ПочатковийЗалишок,
        0 AS Прихід,
        0 AS Розхід,
        ВНаявності AS КінцевийЗалишок
    FROM ostatok_na_kinec_periodu

    UNION

    SELECT
        'C',
        Номенклатура,
        ХарактеристикаНоменклатури,
        Склад,
        0 AS ПочатковийЗалишок,
        Прихід AS Прихід,
        Розхід AS Розхід,
        0 AS КінцевийЗалишок
    FROM oborot
) AS ЗалишкиТаОбороти

LEFT JOIN {Номенклатура_Const.TABLE} AS Довідник_Номенклатура ON Довідник_Номенклатура.uid = ЗалишкиТаОбороти.Номенклатура
LEFT JOIN {ХарактеристикиНоменклатури_Const.TABLE} AS Довідник_ХарактеристикиНоменклатури ON Довідник_ХарактеристикиНоменклатури.uid = ЗалишкиТаОбороти.ХарактеристикаНоменклатури
LEFT JOIN {Склади_Const.TABLE} AS Довідник_Склади ON Довідник_Склади.uid = ЗалишкиТаОбороти.Склад
";

            #region WHERE

            //Відбір по всіх вкладених папках вибраної папки Номенклатури
            if (!directoryControl_НоменклатураПапка.DirectoryPointerItem.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
Довідник_Номенклатура.{Номенклатура_Const.Папка} IN 
    (
        WITH RECURSIVE r AS 
        (
            SELECT uid
            FROM {Номенклатура_Папки_Const.TABLE}
            WHERE {Номенклатура_Папки_Const.TABLE}.uid = '{directoryControl_НоменклатураПапка.DirectoryPointerItem.UnigueID}' 

            UNION ALL

            SELECT {Номенклатура_Папки_Const.TABLE}.uid
            FROM {Номенклатура_Папки_Const.TABLE}
                JOIN r ON {Номенклатура_Папки_Const.TABLE}.{Номенклатура_Папки_Const.Родич} = r.uid
        ) SELECT uid FROM r
    )
";
            }

            //Відбір по вибраному елементу Номенклатура
            if (!directoryControl_Номенклатура.DirectoryPointerItem.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
Довідник_Номенклатура.uid = '{directoryControl_Номенклатура.DirectoryPointerItem.UnigueID}'
";
            }

            //Відбір по вибраному елементу Характеристики Номенклатури
            if (!directoryControl_ХарактеристикаНоменклатури.DirectoryPointerItem.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
Довідник_ХарактеристикиНоменклатури.uid = '{directoryControl_ХарактеристикаНоменклатури.DirectoryPointerItem.UnigueID}'
";
            }

            //Відбір по всіх вкладених папках вибраної папки Склади
            if (!directoryControl_СкладиПапки.DirectoryPointerItem.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
Довідник_Склади.{Склади_Const.Папка} IN 
    (
        WITH RECURSIVE r AS 
        (
            SELECT uid
            FROM {Склади_Папки_Const.TABLE}
            WHERE {Склади_Папки_Const.TABLE}.uid = '{directoryControl_СкладиПапки.DirectoryPointerItem.UnigueID}' 

            UNION ALL

            SELECT {Склади_Папки_Const.TABLE}.uid
            FROM {Склади_Папки_Const.TABLE}
                JOIN r ON {Склади_Папки_Const.TABLE}.{Склади_Папки_Const.Родич} = r.uid
        ) SELECT uid FROM r
    )
";
            }

            //Відбір по вибраному елементу Склади
            if (!directoryControl_Склади.DirectoryPointerItem.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
Довідник_Склади.uid = '{directoryControl_Склади.DirectoryPointerItem.UnigueID}'
";
            }

            #endregion

            query += @"
GROUP BY Номенклатура, Номенклатура_Назва, ХарактеристикаНоменклатури, ХарактеристикаНоменклатури_Назва, Склад, Склад_Назва
ORDER BY Номенклатура_Назва, ХарактеристикаНоменклатури, Склад_Назва
";

            Dictionary<string, object> paramQuery = new Dictionary<string, object>();
            paramQuery.Add("period_month_end", DateTime.Parse($"01.{dateTimeStart.Value.Month}.{dateTimeStart.Value.Year} 00:00:00"));

            paramQuery.Add("period_day_start", DateTime.Parse($"01.{dateTimeStart.Value.Month}.{dateTimeStart.Value.Year} 00:00:00"));
            paramQuery.Add("period_day_end", DateTime.Parse($"{dateTimeStart.Value.Day}.{dateTimeStart.Value.Month}.{dateTimeStart.Value.Year} 00:00:00"));

            paramQuery.Add("period_ostatok_start", DateTime.Parse($"{dateTimeStart.Value.Day}.{dateTimeStart.Value.Month}.{dateTimeStart.Value.Year} 00:00:00"));
            paramQuery.Add("period_ostatok_end", DateTime.Parse($"{dateTimeStop.Value.Day}.{dateTimeStop.Value.Month}.{dateTimeStop.Value.Year} 00:00:00"));

            paramQuery.Add("period_oborot_start", DateTime.Parse($"{dateTimeStart.Value.Day}.{dateTimeStart.Value.Month}.{dateTimeStart.Value.Year} 00:00:00"));
            paramQuery.Add("period_oborot_end", DateTime.Parse($"{dateTimeStop.Value.Day}.{dateTimeStop.Value.Month}.{dateTimeStop.Value.Year} 23:59:59"));

            string[] columnsName;
            List<object[]> listRow;

            Config.Kernel.DataBase.SelectRequest(query, paramQuery, out columnsName, out listRow);
            Функції.DataToXML(xmlDoc, "ЗалишкиТаОбороти", columnsName, listRow);
        }

        private void buttonOstatokAndOborot_Click(object sender, EventArgs e)
        {
            XmlDocument xmlDoc = Функції.CreateXmlDocument();

            Функції.DataHeadToXML(xmlDoc, "head",
                new List<NameValue<string>>()
                {
                    new NameValue<string>("ПочатокПеріоду", dateTimeStart.Value.ToString("dd.MM.yyyy")),
                    new NameValue<string>("КінецьПеріоду", dateTimeStop.Value.ToString("dd.MM.yyyy"))
                }
            );

            ЗалишкиТаОбороти(xmlDoc);

            Функції.XmlDocumentSaveAndTransform(xmlDoc, @"Шаблони\ТовариНаСкладах_ЗалишкиТаОбороти.xslt", false, "Товари на складах");

            string pathToHtmlFile = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "Report.html");
            geckoWebBrowser.Navigate(pathToHtmlFile);
        }

        private void button_Documents_Click(object sender, EventArgs e)
        {
            bool isExistParent = false;

            XmlDocument xmlDoc = Функції.CreateXmlDocument();

            Функції.DataHeadToXML(xmlDoc, "head",
                new List<NameValue<string>>()
                {
                    new NameValue<string>("ПочатокПеріоду", dateTimeStart.Value.ToString("dd.MM.yyyy")),
                    new NameValue<string>("КінецьПеріоду", dateTimeStop.Value.ToString("dd.MM.yyyy"))
                }
            );

            string query = $@"
WITH documents AS
(
     SELECT 
        ТовариНаСкладах.period AS period,
        ТовариНаСкладах.owner AS owner,
        ТовариНаСкладах.income AS income,
        ТовариНаСкладах.{ТовариНаСкладах_Const.Номенклатура} AS Номенклатура,
        ТовариНаСкладах.{ТовариНаСкладах_Const.ХарактеристикаНоменклатури} AS ХарактеристикаНоменклатури,
        ТовариНаСкладах.{ТовариНаСкладах_Const.Склад} AS Склад,
        ТовариНаСкладах.{ТовариНаСкладах_Const.ВНаявності} AS ВНаявності
    FROM
        {ТовариНаСкладах_Const.TABLE} AS ТовариНаСкладах
    WHERE
        (ТовариНаСкладах.period >= @period_start AND ТовариНаСкладах.period <= @period_end)
";

            #region WHERE

            isExistParent = true;

            //Відбір по вибраному елементу Номенклатура
            if (!directoryControl_Номенклатура.DirectoryPointerItem.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                query += $@"
ТовариНаСкладах.{ТовариНаСкладах_Const.Номенклатура} = '{directoryControl_Номенклатура.DirectoryPointerItem.UnigueID}'
";
            }

            //Відбір по вибраному елементу Характеристики Номенклатури
            if (!directoryControl_ХарактеристикаНоменклатури.DirectoryPointerItem.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
ТовариНаСкладах.{ТовариНаСкладах_Const.ХарактеристикаНоменклатури} = '{directoryControl_ХарактеристикаНоменклатури.DirectoryPointerItem.UnigueID}'
";
            }

            //Відбір по вибраному елементу Склади
            if (!directoryControl_Склади.DirectoryPointerItem.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
ТовариНаСкладах.{ТовариНаСкладах_Const.Склад} = '{directoryControl_Склади.DirectoryPointerItem.UnigueID}'
";
            }

            #endregion

            query += $@"
),
doc AS
(";
            int counter = 0;
            foreach (string table in new Journal_Select().Tables)
            {
                string union = (counter > 0 ? "UNION" : "");
                query += $@"
{union}
SELECT 
    {table}.uid, 
    {table}.docname, 
    documents.period, 
    documents.income, 
    documents.ВНаявності,
    Довідник_Номенклатура.{Номенклатура_Const.Назва} AS Номенклатура_Назва,
    Довідник_ХарактеристикиНоменклатури.{ХарактеристикиНоменклатури_Const.Назва} AS ХарактеристикаНоменклатури_Назва,
    Довідник_Склади.{Склади_Const.Назва} AS Склад_Назва
FROM documents INNER JOIN {table} ON {table}.uid = documents.owner
    LEFT JOIN {Номенклатура_Const.TABLE} AS Довідник_Номенклатура ON Довідник_Номенклатура.uid = documents.Номенклатура
    LEFT JOIN {ХарактеристикиНоменклатури_Const.TABLE} AS Довідник_ХарактеристикиНоменклатури ON Довідник_ХарактеристикиНоменклатури.uid = documents.ХарактеристикаНоменклатури
    LEFT JOIN {Склади_Const.TABLE} AS Довідник_Склади ON Довідник_Склади.uid = documents.Склад
";

                #region WHERE

                isExistParent = false;

                //Відбір по всіх вкладених папках вибраної папки Номенклатури
                if (!directoryControl_НоменклатураПапка.DirectoryPointerItem.IsEmpty())
                {
                    query += isExistParent ? "AND" : "WHERE";
                    isExistParent = true;

                    query += $@"
Довідник_Номенклатура.{Номенклатура_Const.Папка} IN 
    (
        WITH RECURSIVE r AS 
        (
            SELECT uid
            FROM {Номенклатура_Папки_Const.TABLE}
            WHERE {Номенклатура_Папки_Const.TABLE}.uid = '{directoryControl_НоменклатураПапка.DirectoryPointerItem.UnigueID}' 

            UNION ALL

            SELECT {Номенклатура_Папки_Const.TABLE}.uid
            FROM {Номенклатура_Папки_Const.TABLE}
                JOIN r ON {Номенклатура_Папки_Const.TABLE}.{Номенклатура_Папки_Const.Родич} = r.uid
        ) SELECT uid FROM r
    )
";
                }

                //Відбір по всіх вкладених папках вибраної папки Склади
                if (!directoryControl_СкладиПапки.DirectoryPointerItem.IsEmpty())
                {
                    query += isExistParent ? "AND" : "WHERE";
                    isExistParent = true;

                    query += $@"
Довідник_Склади.{Склади_Const.Папка} IN 
    (
        WITH RECURSIVE r AS 
        (
            SELECT uid
            FROM {Склади_Папки_Const.TABLE}
            WHERE {Склади_Папки_Const.TABLE}.uid = '{directoryControl_СкладиПапки.DirectoryPointerItem.UnigueID}' 

            UNION ALL

            SELECT {Склади_Папки_Const.TABLE}.uid
            FROM {Склади_Папки_Const.TABLE}
                JOIN r ON {Склади_Папки_Const.TABLE}.{Склади_Папки_Const.Родич} = r.uid
        ) SELECT uid FROM r
    )
";
                }

                #endregion

                counter++;
            }

            query += $@"
)
SELECT 
    uid,
    period,
    docname, 
    income, 
    ВНаявності, 
    Номенклатура_Назва,
    ХарактеристикаНоменклатури_Назва,
    Склад_Назва
FROM doc
ORDER BY period ASC
";
            //Console.WriteLine(query);

            Dictionary<string, object> paramQuery = new Dictionary<string, object>();
            paramQuery.Add("period_start", DateTime.Parse($"{dateTimeStart.Value.Day}.{dateTimeStart.Value.Month}.{dateTimeStart.Value.Year} 00:00:00"));
            paramQuery.Add("period_end", DateTime.Parse($"{dateTimeStop.Value.Day}.{dateTimeStop.Value.Month}.{dateTimeStop.Value.Year} 23:59:59"));

            string[] columnsName;
            List<object[]> listRow;

            Config.Kernel.DataBase.SelectRequest(query, paramQuery, out columnsName, out listRow);
            Функції.DataToXML(xmlDoc, "Документи", columnsName, listRow);

            Функції.XmlDocumentSaveAndTransform(xmlDoc, @"Шаблони\ТовариНаСкладах_Документи.xslt", false, "Товари на складах");

            string pathToHtmlFile = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "Report.html");
            geckoWebBrowser.Navigate(pathToHtmlFile);
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}


//private void buttonCreate_Click(object sender, EventArgs e)
//{
//    bool isExistParent = false;

//    string query = $@"
//SELECT 
//    Рег_ТовариНаСкладах.{ТовариНаСкладах_Const.Номенклатура} AS Номенклатура, 
//    Довідник_Номенклатура.{Номенклатура_Const.Назва} AS Номенклатура_Назва, 
//    Рег_ТовариНаСкладах.{ТовариНаСкладах_Const.ХарактеристикаНоменклатури} AS ХарактеристикаНоменклатури,
//    Довідник_ХарактеристикиНоменклатури.{ХарактеристикиНоменклатури_Const.Назва} AS ХарактеристикаНоменклатури_Назва,
//    Рег_ТовариНаСкладах.{ТовариНаСкладах_Const.Склад} AS Склад,
//    Довідник_Склади.{Склади_Const.Назва} AS Склад_Назва, 

//    SUM(CASE WHEN Рег_ТовариНаСкладах.income = true THEN 
//        Рег_ТовариНаСкладах.{ТовариНаСкладах_Const.ВНаявності} ELSE 
//       -Рег_ТовариНаСкладах.{ТовариНаСкладах_Const.ВНаявності} END) AS ВНаявності,

//    SUM(CASE WHEN Рег_ТовариНаСкладах.income = true THEN 
//        Рег_ТовариНаСкладах.{ТовариНаСкладах_Const.ДоВідвантаження} ELSE 
//        -Рег_ТовариНаСкладах.{ТовариНаСкладах_Const.ДоВідвантаження} END) AS Сума
//FROM 
//    {ТовариНаСкладах_Const.TABLE} AS Рег_ТовариНаСкладах

//    LEFT JOIN {Номенклатура_Const.TABLE} AS Довідник_Номенклатура ON Довідник_Номенклатура.uid = 
//        Рег_ТовариНаСкладах.{ТовариНаСкладах_Const.Номенклатура}

//    LEFT JOIN {ХарактеристикиНоменклатури_Const.TABLE} AS Довідник_ХарактеристикиНоменклатури ON Довідник_ХарактеристикиНоменклатури.uid = 
//        Рег_ТовариНаСкладах.{ТовариНаСкладах_Const.ХарактеристикаНоменклатури}

//    LEFT JOIN {Склади_Const.TABLE} AS Довідник_Склади ON Довідник_Склади.uid = 
//        Рег_ТовариНаСкладах.{ТовариНаСкладах_Const.Склад}
//";
//    #region WHERE

//    //Відбір по всіх вкладених папках вибраної папки Номенклатури
//    if (!directoryControl_НоменклатураПапка.DirectoryPointerItem.IsEmpty())
//    {
//        query += isExistParent ? "AND" : "WHERE";
//        isExistParent = true;

//        query += $@"
//Довідник_Номенклатура.{Номенклатура_Const.Папка} IN 
//    (
//        WITH RECURSIVE r AS 
//        (
//            SELECT uid
//            FROM {Номенклатура_Папки_Const.TABLE}
//            WHERE {Номенклатура_Папки_Const.TABLE}.uid = '{directoryControl_НоменклатураПапка.DirectoryPointerItem.UnigueID}' 

//            UNION ALL

//            SELECT {Номенклатура_Папки_Const.TABLE}.uid
//            FROM {Номенклатура_Папки_Const.TABLE}
//                JOIN r ON {Номенклатура_Папки_Const.TABLE}.{Номенклатура_Папки_Const.Родич} = r.uid
//        ) SELECT uid FROM r
//    )
//";
//    }

//    //Відбір по вибраному елементу Номенклатура
//    if (!directoryControl_Номенклатура.DirectoryPointerItem.IsEmpty())
//    {
//        query += isExistParent ? "AND" : "WHERE";
//        isExistParent = true;

//        query += $@"
//Довідник_Номенклатура.uid = '{directoryControl_Номенклатура.DirectoryPointerItem.UnigueID}'
//";
//    }

//    //Відбір по вибраному елементу Характеристики Номенклатури
//    if (!directoryControl_ХарактеристикаНоменклатури.DirectoryPointerItem.IsEmpty())
//    {
//        query += isExistParent ? "AND" : "WHERE";
//        isExistParent = true;

//        query += $@"
//Довідник_ХарактеристикиНоменклатури.uid = '{directoryControl_ХарактеристикаНоменклатури.DirectoryPointerItem.UnigueID}'
//";
//    }

//    //Відбір по всіх вкладених папках вибраної папки Склади
//    if (!directoryControl_СкладиПапки.DirectoryPointerItem.IsEmpty())
//    {
//        query += isExistParent ? "AND" : "WHERE";
//        isExistParent = true;

//        query += $@"
//Довідник_Склади.{Склади_Const.Папка} IN 
//    (
//        WITH RECURSIVE r AS 
//        (
//            SELECT uid
//            FROM {Склади_Папки_Const.TABLE}
//            WHERE {Склади_Папки_Const.TABLE}.uid = '{directoryControl_СкладиПапки.DirectoryPointerItem.UnigueID}' 

//            UNION ALL

//            SELECT {Склади_Папки_Const.TABLE}.uid
//            FROM {Склади_Папки_Const.TABLE}
//                JOIN r ON {Склади_Папки_Const.TABLE}.{Склади_Папки_Const.Родич} = r.uid
//        ) SELECT uid FROM r
//    )
//";
//    }

//    //Відбір по вибраному елементу Склади
//    if (!directoryControl_Склади.DirectoryPointerItem.IsEmpty())
//    {
//        query += isExistParent ? "AND" : "WHERE";
//        isExistParent = true;

//        query += $@"
//Довідник_Склади.uid = '{directoryControl_Склади.DirectoryPointerItem.UnigueID}'
//";
//    }

//    #endregion

//    query += $@"
//GROUP BY Номенклатура, Номенклатура_Назва, 
//         ХарактеристикаНоменклатури, ХарактеристикаНоменклатури_Назва,
//         Склад, Склад_Назва

//HAVING
//     SUM(CASE WHEN Рег_ТовариНаСкладах.income = true THEN 
//        Рег_ТовариНаСкладах.{ТовариНаСкладах_Const.ВНаявності} ELSE 
//       -Рег_ТовариНаСкладах.{ТовариНаСкладах_Const.ВНаявності} END) != 0
//OR
//    SUM(CASE WHEN Рег_ТовариНаСкладах.income = true THEN 
//        Рег_ТовариНаСкладах.{ТовариНаСкладах_Const.ДоВідвантаження} ELSE 
//        -Рег_ТовариНаСкладах.{ТовариНаСкладах_Const.ДоВідвантаження} END) != 0   

//ORDER BY Номенклатура_Назва
//";

//    //Console.WriteLine(query);

//    XmlDocument xmlDoc = Функції.CreateXmlDocument();

//    Dictionary<string, object> paramQuery = new Dictionary<string, object>();

//    string[] columnsName;
//    List<object[]> listRow;

//    Config.Kernel.DataBase.SelectRequest(query, paramQuery, out columnsName, out listRow);

//    Функції.DataToXML(xmlDoc, "ТовариНаСкладах", columnsName, listRow);

//    Функції.XmlDocumentSaveAndTransform(xmlDoc, @"Шаблони\ТовариНаСкладах.xslt", true, "Товари на складах");
//}