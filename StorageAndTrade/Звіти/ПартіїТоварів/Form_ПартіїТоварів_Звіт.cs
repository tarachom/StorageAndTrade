/*
Copyright (C) 2019-2022 TARAKHOMYN YURIY IVANOVYCH
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
using System.Drawing;
using System.Windows.Forms;

using System.Xml;
using System.IO;

using AccountingSoftware;
using StorageAndTrade_1_0;
using StorageAndTrade_1_0.Константи;
using StorageAndTrade_1_0.Довідники;
using StorageAndTrade_1_0.Документи;
using StorageAndTrade_1_0.РегістриНакопичення;


namespace StorageAndTrade
{
    public partial class Form_ПартіїТоварів_Звіт : Form
    {
        public Form_ПартіїТоварів_Звіт()
        {
            InitializeComponent();

            geckoWebBrowser = GeckoWebBrowser.AddGeckoWebBrowserControl(this, new Point(2, 220));
        }

        Gecko.GeckoWebBrowser geckoWebBrowser { get; set; }

        private void Form_ПартіїТоварів_Звіт_Load(object sender, EventArgs e)
        {
            geckoWebBrowser.Reload();

            directoryControl_Організація.Init(new Form_Організації(), new Організації_Pointer(), ПошуковіЗапити.Організації);
            directoryControl_НоменклатураПапка.Init(new Form_НоменклатураПапкиВибір(), new Номенклатура_Папки_Pointer(), ПошуковіЗапити.Номенклатура_Папки);
            directoryControl_Номенклатура.Init(new Form_Номенклатура(), new Номенклатура_Pointer(), ПошуковіЗапити.Номенклатура);
            directoryControl_ХарактеристикаНоменклатури.Init(new Form_ХарактеристикиНоменклатури(), new ХарактеристикиНоменклатури_Pointer(), ПошуковіЗапити.ХарактеристикаНоменклатуриЗВідбором());
            directoryControl_ХарактеристикаНоменклатури.BeforeClickOpenFunc = () =>
            {
                ((Form_ХарактеристикиНоменклатури)directoryControl_ХарактеристикаНоменклатури.SelectForm).НоменклатураВласник = (Номенклатура_Pointer)directoryControl_Номенклатура.DirectoryPointerItem;
                return true;
            };
            directoryControl_ХарактеристикаНоменклатури.BeforeFindFunc = () =>
            {
                directoryControl_ХарактеристикаНоменклатури.QueryFind =
                   ПошуковіЗапити.ХарактеристикаНоменклатуриЗВідбором((Номенклатура_Pointer)directoryControl_Номенклатура.DirectoryPointerItem);
            };
            directoryControl_Серія.Init(new Form_СеріїНоменклатури(), new СеріїНоменклатури_Pointer(), ПошуковіЗапити.СеріїНоменклатури);
            directoryControl_Склад.Init(new Form_Склади(), new Склади_Pointer(), ПошуковіЗапити.Склади);

            dateTimeStart.Value = DateTime.Parse($"01.{DateTime.Now.Month}.{DateTime.Now.Year}");

            geckoWebBrowser.DomClick += GeckoWebBrowser.DomClick;
        }

        private void buttonOstatok_Click(object sender, EventArgs e)
        {
            bool isExistParent = false;

            string query = $@"
SELECT 
    ПартіїТоварів_Місяць.{ВіртуальніТаблиціРегістрів.ПартіїТоварів_Місяць_TablePart.Організація} AS Організація, 
    Довідник_Організації.{Організації_Const.Назва} AS Організація_Назва, 
    ПартіїТоварів_Місяць.{ВіртуальніТаблиціРегістрів.ПартіїТоварів_Місяць_TablePart.ПартіяТоварівКомпозит} AS ПартіяТоварівКомпозит,
    Довідник_ПартіяТоварівКомпозит.{ПартіяТоварівКомпозит_Const.Назва} AS ПартіяТоварівКомпозит_Назва,
    Довідник_ПартіяТоварівКомпозит.{ПартіяТоварівКомпозит_Const.Дата} AS ПартіяТоварівКомпозит_Дата,
    ПартіїТоварів_Місяць.{ВіртуальніТаблиціРегістрів.ПартіїТоварів_Місяць_TablePart.Номенклатура} AS Номенклатура, 
    Довідник_Номенклатура.{Номенклатура_Const.Назва} AS Номенклатура_Назва, 
    ПартіїТоварів_Місяць.{ВіртуальніТаблиціРегістрів.ПартіїТоварів_Місяць_TablePart.ХарактеристикаНоменклатури} AS ХарактеристикаНоменклатури,
    Довідник_ХарактеристикиНоменклатури.{ХарактеристикиНоменклатури_Const.Назва} AS ХарактеристикаНоменклатури_Назва,
    ПартіїТоварів_Місяць.{ВіртуальніТаблиціРегістрів.ПартіїТоварів_Місяць_TablePart.Серія} AS Серія,
    Довідник_СеріїНоменклатури.{СеріїНоменклатури_Const.Номер} AS Серія_Номер,
    ПартіїТоварів_Місяць.{ВіртуальніТаблиціРегістрів.ПартіїТоварів_Місяць_TablePart.Склад} AS Склад,
    Довідник_Склади.{Склади_Const.Назва} AS Склад_Назва,
    SUM(ПартіїТоварів_Місяць.{ВіртуальніТаблиціРегістрів.ПартіїТоварів_Місяць_TablePart.Кількість}) AS Кількість,
    SUM(ПартіїТоварів_Місяць.{ВіртуальніТаблиціРегістрів.ПартіїТоварів_Місяць_TablePart.Собівартість}) AS Собівартість
FROM 
    {ВіртуальніТаблиціРегістрів.ПартіїТоварів_Місяць_TablePart.TABLE} AS ПартіїТоварів_Місяць

    LEFT JOIN {Організації_Const.TABLE} AS Довідник_Організації ON Довідник_Організації.uid = 
        ПартіїТоварів_Місяць.{ВіртуальніТаблиціРегістрів.ПартіїТоварів_Місяць_TablePart.Організація}

    LEFT JOIN {ПартіяТоварівКомпозит_Const.TABLE} AS Довідник_ПартіяТоварівКомпозит ON Довідник_ПартіяТоварівКомпозит.uid = 
        ПартіїТоварів_Місяць.{ВіртуальніТаблиціРегістрів.ПартіїТоварів_Місяць_TablePart.ПартіяТоварівКомпозит}

    LEFT JOIN {Номенклатура_Const.TABLE} AS Довідник_Номенклатура ON Довідник_Номенклатура.uid = 
        ПартіїТоварів_Місяць.{ВіртуальніТаблиціРегістрів.ПартіїТоварів_Місяць_TablePart.Номенклатура}

    LEFT JOIN {ХарактеристикиНоменклатури_Const.TABLE} AS Довідник_ХарактеристикиНоменклатури ON Довідник_ХарактеристикиНоменклатури.uid = 
        ПартіїТоварів_Місяць.{ВіртуальніТаблиціРегістрів.ПартіїТоварів_Місяць_TablePart.ХарактеристикаНоменклатури}

    LEFT JOIN {СеріїНоменклатури_Const.TABLE} AS Довідник_СеріїНоменклатури ON Довідник_СеріїНоменклатури.uid = 
        ПартіїТоварів_Місяць.{ВіртуальніТаблиціРегістрів.ПартіїТоварів_Місяць_TablePart.Серія}

    LEFT JOIN {Склади_Const.TABLE} AS Довідник_Склади ON Довідник_Склади.uid = 
        ПартіїТоварів_Місяць.{ВіртуальніТаблиціРегістрів.ПартіїТоварів_Місяць_TablePart.Склад}
";

            #region WHERE

            //Відбір по вибраному елементу Організація
            if (!directoryControl_Організація.DirectoryPointerItem.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
Довідник_Організації.uid = '{directoryControl_Організація.DirectoryPointerItem.UnigueID}'
";
            }

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

            //Відбір по вибраному елементу Серія
            if (!directoryControl_Серія.DirectoryPointerItem.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
Довідник_СеріїНоменклатури.uid = '{directoryControl_Серія.DirectoryPointerItem.UnigueID}'
";
            }

            //Відбір по вибраному елементу Склад
            if (!directoryControl_Склад.DirectoryPointerItem.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
Довідник_Склади.uid = '{directoryControl_Склад.DirectoryPointerItem.UnigueID}'
";
            }

            #endregion

            query += $@"
GROUP BY Організація, Організація_Назва, 
         ПартіяТоварівКомпозит, ПартіяТоварівКомпозит_Назва, ПартіяТоварівКомпозит_Дата,
         Номенклатура, Номенклатура_Назва, 
         ХарактеристикаНоменклатури, ХарактеристикаНоменклатури_Назва,
         Серія, Серія_Номер,
         Склад, Склад_Назва

HAVING
    SUM(ПартіїТоварів_Місяць.{ВіртуальніТаблиціРегістрів.ПартіїТоварів_Місяць_TablePart.Кількість}) != 0 OR
    SUM(ПартіїТоварів_Місяць.{ВіртуальніТаблиціРегістрів.ПартіїТоварів_Місяць_TablePart.Собівартість}) != 0   

ORDER BY Організація_Назва, ПартіяТоварівКомпозит_Дата ASC, Номенклатура_Назва, ХарактеристикаНоменклатури_Назва
";

            XmlDocument xmlDoc = ФункціїДляЗвітів.CreateXmlDocument();

            ФункціїДляЗвітів.DataHeadToXML(xmlDoc, "head",
                new List<NameValue<string>>()
                {
                    new NameValue<string>("КінецьПеріоду", DateTime.Now.ToString("dd.MM.yyyy"))
                }
            );

            Dictionary<string, object> paramQuery = new Dictionary<string, object>();

            string[] columnsName;
            List<object[]> listRow;

            Config.Kernel.DataBase.SelectRequest(query, paramQuery, out columnsName, out listRow);

            ФункціїДляЗвітів.DataToXML(xmlDoc, "ПартіїТоварів", columnsName, listRow);

            ФункціїДляЗвітів.XmlDocumentSaveAndTransform(xmlDoc, @"Шаблони\ПартіїТоварів_Залишки.xslt", false);

            string pathToHtmlFile = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "Report.html");
            geckoWebBrowser.Navigate(pathToHtmlFile);
        }

        private void button_Documents_Click(object sender, EventArgs e)
        {
            bool isExistParent = false;

            XmlDocument xmlDoc = ФункціїДляЗвітів.CreateXmlDocument();

            ФункціїДляЗвітів.DataHeadToXML(xmlDoc, "head",
                new List<NameValue<string>>()
                {
                    new NameValue<string>("ПочатокПеріоду", dateTimeStart.Value.ToString("dd.MM.yyyy")),
                    new NameValue<string>("КінецьПеріоду", dateTimeStop.Value.ToString("dd.MM.yyyy"))
                }
            );

            string query = $@"
WITH register AS
(
     SELECT 
        ПартіїТоварів.period AS period,
        ПартіїТоварів.owner AS owner,
        ПартіїТоварів.income AS income,
        ПартіїТоварів.{ПартіїТоварів_Const.Організація} AS Організація,
        ПартіїТоварів.{ПартіїТоварів_Const.ПартіяТоварівКомпозит} AS ПартіяТоварівКомпозит,
        ПартіїТоварів.{ПартіїТоварів_Const.Номенклатура} AS Номенклатура,
        ПартіїТоварів.{ПартіїТоварів_Const.ХарактеристикаНоменклатури} AS ХарактеристикаНоменклатури,
        ПартіїТоварів.{ПартіїТоварів_Const.Серія} AS Серія,
        ПартіїТоварів.{ПартіїТоварів_Const.Склад} AS Склад,
        ПартіїТоварів.{ПартіїТоварів_Const.Кількість} AS Кількість,
        CASE WHEN ПартіїТоварів.{ПартіїТоварів_Const.СписанаСобівартість} != 0 THEN 
            ПартіїТоварів.{ПартіїТоварів_Const.СписанаСобівартість} ELSE 
            ПартіїТоварів.{ПартіїТоварів_Const.Собівартість} END AS Собівартість 
    FROM
        {ПартіїТоварів_Const.TABLE} AS ПартіїТоварів
    WHERE
        (ПартіїТоварів.period >= @period_start AND ПартіїТоварів.period <= @period_end)
";

            #region WHERE

            isExistParent = true;

            //Відбір по вибраному елементу Організація
            if (!directoryControl_Організація.DirectoryPointerItem.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
ПартіїТоварів.{ПартіїТоварів_Const.Організація} = '{directoryControl_Організація.DirectoryPointerItem.UnigueID}'
";
            }

            //Відбір по вибраному елементу Номенклатура
            if (!directoryControl_Номенклатура.DirectoryPointerItem.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                query += $@"
ПартіїТоварів.{ПартіїТоварів_Const.Номенклатура} = '{directoryControl_Номенклатура.DirectoryPointerItem.UnigueID}'
";
            }

            //Відбір по вибраному елементу Характеристики Номенклатури
            if (!directoryControl_ХарактеристикаНоменклатури.DirectoryPointerItem.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
ПартіїТоварів.{ПартіїТоварів_Const.ХарактеристикаНоменклатури} = '{directoryControl_ХарактеристикаНоменклатури.DirectoryPointerItem.UnigueID}'
";
            }

            //Відбір по вибраному елементу Серія
            if (!directoryControl_Серія.DirectoryPointerItem.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
ПартіїТоварів.{ПартіїТоварів_Const.Серія} = '{directoryControl_Серія.DirectoryPointerItem.UnigueID}'
";
            }

            //Відбір по вибраному елементу Склад
            if (!directoryControl_Склад.DirectoryPointerItem.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
ПартіїТоварів.{ПартіїТоварів_Const.Склад} = '{directoryControl_Склад.DirectoryPointerItem.UnigueID}'
";
            }

            #endregion

            query += $@"
),
documents AS
(";
            int counter = 0;
            foreach (string table in ПартіїТоварів_Const.AllowDocumentSpendTable)
            {
                string docType = ПартіїТоварів_Const.AllowDocumentSpendType[counter];

                string union = (counter > 0 ? "UNION" : "");
                query += $@"
{union}
SELECT 
    '{docType}' AS doctype,
    {table}.uid, 
    {table}.docname, 
    register.period, 
    register.income, 
    register.Кількість,
    register.Собівартість,
    register.Організація,
    Довідник_Організації.{Організації_Const.Назва} AS Організація_Назва,
    register.ПартіяТоварівКомпозит,
    Довідник_ПартіяТоварівКомпозит.{ПартіяТоварівКомпозит_Const.Назва} AS ПартіяТоварівКомпозит_Назва,
    register.Номенклатура,
    Довідник_Номенклатура.{Номенклатура_Const.Назва} AS Номенклатура_Назва,
    register.ХарактеристикаНоменклатури,
    Довідник_ХарактеристикиНоменклатури.{ХарактеристикиНоменклатури_Const.Назва} AS ХарактеристикаНоменклатури_Назва,
    register.Серія,
    Довідник_СеріїНоменклатури.{СеріїНоменклатури_Const.Номер} AS Серія_Номер,
    register.Склад,
    Довідник_Склади.{Склади_Const.Назва} AS Склад_Назва
FROM register INNER JOIN {table} ON {table}.uid = register.owner
    LEFT JOIN {Організації_Const.TABLE} AS Довідник_Організації ON Довідник_Організації.uid = register.Організація
    LEFT JOIN {ПартіяТоварівКомпозит_Const.TABLE} AS Довідник_ПартіяТоварівКомпозит ON Довідник_ПартіяТоварівКомпозит.uid = register.ПартіяТоварівКомпозит
    LEFT JOIN {Номенклатура_Const.TABLE} AS Довідник_Номенклатура ON Довідник_Номенклатура.uid = register.Номенклатура
    LEFT JOIN {ХарактеристикиНоменклатури_Const.TABLE} AS Довідник_ХарактеристикиНоменклатури ON Довідник_ХарактеристикиНоменклатури.uid = register.ХарактеристикаНоменклатури
    LEFT JOIN {СеріїНоменклатури_Const.TABLE} AS Довідник_СеріїНоменклатури ON Довідник_СеріїНоменклатури.uid = register.Серія
    LEFT JOIN {Склади_Const.TABLE} AS Довідник_Склади ON Довідник_Склади.uid = register.Склад
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

                #endregion

                counter++;
            }

            query += $@"
)
SELECT 
    doctype,
    uid,
    period,
    docname, 
    income, 
    Кількість, 
    Собівартість,
    Організація,
    Організація_Назва,
    ПартіяТоварівКомпозит,
    ПартіяТоварівКомпозит_Назва,
    Номенклатура,
    Номенклатура_Назва,
    ХарактеристикаНоменклатури,
    ХарактеристикаНоменклатури_Назва,
    Серія,
    Серія_Номер,
    Склад,
    Склад_Назва
FROM documents
ORDER BY period ASC, Організація_Назва, 
         Номенклатура_Назва, 
         ХарактеристикаНоменклатури_Назва
";

            Dictionary<string, object> paramQuery = new Dictionary<string, object>();
            paramQuery.Add("period_start", DateTime.Parse($"{dateTimeStart.Value.Day}.{dateTimeStart.Value.Month}.{dateTimeStart.Value.Year} 00:00:00"));
            paramQuery.Add("period_end", DateTime.Parse($"{dateTimeStop.Value.Day}.{dateTimeStop.Value.Month}.{dateTimeStop.Value.Year} 23:59:59"));

            string[] columnsName;
            List<object[]> listRow;

            Config.Kernel.DataBase.SelectRequest(query, paramQuery, out columnsName, out listRow);
            ФункціїДляЗвітів.DataToXML(xmlDoc, "Документи", columnsName, listRow);

            ФункціїДляЗвітів.XmlDocumentSaveAndTransform(xmlDoc, @"Шаблони\ПартіїТоварів_Документи.xslt", false);

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
//    Рег_ПартіїТоварів.{ПартіїТоварів_Const.Номенклатура} AS Номенклатура, 
//    Довідник_Номенклатура.{Номенклатура_Const.Назва} AS Номенклатура_Назва, 
//    Рег_ПартіїТоварів.{ПартіїТоварів_Const.ХарактеристикаНоменклатури} AS ХарактеристикаНоменклатури,
//    Довідник_ХарактеристикиНоменклатури.{ХарактеристикиНоменклатури_Const.Назва} AS ХарактеристикаНоменклатури_Назва,
//    Рег_ПартіїТоварів.{ПартіїТоварів_Const.Склад} AS Склад,
//    Довідник_Склади.{Склади_Const.Назва} AS Склад_Назва, 

//    SUM(CASE WHEN Рег_ПартіїТоварів.income = true THEN 
//        Рег_ПартіїТоварів.{ПартіїТоварів_Const.ВНаявності} ELSE 
//       -Рег_ПартіїТоварів.{ПартіїТоварів_Const.ВНаявності} END) AS ВНаявності,

//    SUM(CASE WHEN Рег_ПартіїТоварів.income = true THEN 
//        Рег_ПартіїТоварів.{ПартіїТоварів_Const.ДоВідвантаження} ELSE 
//        -Рег_ПартіїТоварів.{ПартіїТоварів_Const.ДоВідвантаження} END) AS Сума
//FROM 
//    {ПартіїТоварів_Const.TABLE} AS Рег_ПартіїТоварів

//    LEFT JOIN {Номенклатура_Const.TABLE} AS Довідник_Номенклатура ON Довідник_Номенклатура.uid = 
//        Рег_ПартіїТоварів.{ПартіїТоварів_Const.Номенклатура}

//    LEFT JOIN {ХарактеристикиНоменклатури_Const.TABLE} AS Довідник_ХарактеристикиНоменклатури ON Довідник_ХарактеристикиНоменклатури.uid = 
//        Рег_ПартіїТоварів.{ПартіїТоварів_Const.ХарактеристикаНоменклатури}

//    LEFT JOIN {Склади_Const.TABLE} AS Довідник_Склади ON Довідник_Склади.uid = 
//        Рег_ПартіїТоварів.{ПартіїТоварів_Const.Склад}
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
//     SUM(CASE WHEN Рег_ПартіїТоварів.income = true THEN 
//        Рег_ПартіїТоварів.{ПартіїТоварів_Const.ВНаявності} ELSE 
//       -Рег_ПартіїТоварів.{ПартіїТоварів_Const.ВНаявності} END) != 0
//OR
//    SUM(CASE WHEN Рег_ПартіїТоварів.income = true THEN 
//        Рег_ПартіїТоварів.{ПартіїТоварів_Const.ДоВідвантаження} ELSE 
//        -Рег_ПартіїТоварів.{ПартіїТоварів_Const.ДоВідвантаження} END) != 0   

//ORDER BY Номенклатура_Назва
//";

//    //Console.WriteLine(query);

//    XmlDocument xmlDoc = Функції.CreateXmlDocument();

//    Dictionary<string, object> paramQuery = new Dictionary<string, object>();

//    string[] columnsName;
//    List<object[]> listRow;

//    Config.Kernel.DataBase.SelectRequest(query, paramQuery, out columnsName, out listRow);

//    Функції.DataToXML(xmlDoc, "ПартіїТоварів", columnsName, listRow);

//    Функції.XmlDocumentSaveAndTransform(xmlDoc, @"Шаблони\ПартіїТоварів.xslt", true, "Товари на складах");
//}