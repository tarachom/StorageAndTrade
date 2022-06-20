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

using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Xml.Xsl;

using StorageAndTrade_1_0;
using StorageAndTrade_1_0.Довідники;
using StorageAndTrade_1_0.Документи;
using StorageAndTrade_1_0.РегістриНакопичення;

namespace StorageAndTrade_1_0.Звіти
{
    class РухПоРугістрахНакопичення
    {
        public void PrintRecords(DocumentPointer ДокументВказівник)
        {
            Dictionary<string, Func<string>> funcQuery = new Dictionary<string, Func<string>>();
            funcQuery.Add("ТовариНаСкладах", Запит_ТовариНаСкладах);
            funcQuery.Add("РухТоварів", Запит_РухТоварів);
            funcQuery.Add("ЗамовленняКлієнтів", Запит_ЗамовленняКлієнтів);
            funcQuery.Add("РозрахункиЗКлієнтами", Запит_РозрахункиЗКлієнтами);
            funcQuery.Add("ВільніЗалишки", Запит_ВільніЗалишки);
            funcQuery.Add("ЗамовленняПостачальникам", Запит_ЗамовленняПостачальникам);
            funcQuery.Add("РозрахункиЗПостачальниками", Запит_РозрахункиЗПостачальниками);
            funcQuery.Add("ТовариДоПоступлення", Запит_ТовариДоПоступлення);

            XmlDocument xmlDoc = CreateXmlDocument();

            Dictionary<string, object> paramQuery = new Dictionary<string, object>();
            paramQuery.Add("ДокументВказівник", ДокументВказівник.UnigueID.UGuid);

            foreach (KeyValuePair<string, Func<string>> func in funcQuery)
            {
                string[] columnsName;
                List<object[]> listRow;

                string query = func.Value.Invoke();
                Console.WriteLine(query);

                Config.Kernel.DataBase.SelectRequest(query, paramQuery, out columnsName, out listRow);

                DataToXML(xmlDoc, func.Key, columnsName, listRow);
            }

            XmlDocumentSaveAndTransform(xmlDoc);
        }

        #region Запити

        private static string Запит_ТовариНаСкладах()
        {
            string query = $@"
SELECT 
    Рег_ТовариНаСкладах.period,
    Рег_ТовариНаСкладах.income, 
    Рег_ТовариНаСкладах.{ТовариНаСкладах_Const.Номенклатура} AS Номенклатура, 
    Довідник_Номенклатура.{Номенклатура_Const.Назва} AS Номенклатура_Назва, 
    Рег_ТовариНаСкладах.{ТовариНаСкладах_Const.ХарактеристикаНоменклатури} AS ХарактеристикаНоменклатури,
    Довідник_ХарактеристикиНоменклатури.{ХарактеристикиНоменклатури_Const.Назва} AS ХарактеристикаНоменклатури_Назва, 
    Рег_ТовариНаСкладах.{ТовариНаСкладах_Const.Склад} AS Склад,
    Довідник_Склади.{Склади_Const.Назва} AS Склад_Назва,
    Рег_ТовариНаСкладах.{ТовариНаСкладах_Const.ВНаявності} AS ВНаявності,
    Рег_ТовариНаСкладах.{ТовариНаСкладах_Const.ДоВідвантаження} AS ДоВідвантаження
FROM 
    {ТовариНаСкладах_Const.TABLE} AS Рег_ТовариНаСкладах

    LEFT JOIN {Номенклатура_Const.TABLE} AS Довідник_Номенклатура ON Довідник_Номенклатура.uid = 
        Рег_ТовариНаСкладах.{ТовариНаСкладах_Const.Номенклатура}

    LEFT JOIN {ХарактеристикиНоменклатури_Const.TABLE} AS Довідник_ХарактеристикиНоменклатури ON Довідник_ХарактеристикиНоменклатури.uid = 
        Рег_ТовариНаСкладах.{ТовариНаСкладах_Const.ХарактеристикаНоменклатури}

    LEFT JOIN {Склади_Const.TABLE} AS Довідник_Склади ON Довідник_Склади.uid = 
       Рег_ТовариНаСкладах.{ТовариНаСкладах_Const.Склад}
WHERE
    Рег_ТовариНаСкладах.Owner = @ДокументВказівник
ORDER BY Номенклатура_Назва
";

            return query;
        }

        private static string Запит_РухТоварів()
        {
            string query = $@"
SELECT 
    Рег_РухТоварів.period,
    Рег_РухТоварів.income, 
    Рег_РухТоварів.{ТовариДоПоступлення_Const.Номенклатура} AS Номенклатура, 
    Довідник_Номенклатура.{Номенклатура_Const.Назва} AS Номенклатура_Назва, 
    Рег_РухТоварів.{ТовариДоПоступлення_Const.ХарактеристикаНоменклатури} AS ХарактеристикаНоменклатури,
    Довідник_ХарактеристикиНоменклатури.{ХарактеристикиНоменклатури_Const.Назва} AS ХарактеристикаНоменклатури_Назва, 
    Рег_РухТоварів.{ТовариДоПоступлення_Const.Склад} AS Склад,
    Довідник_Склади.{Склади_Const.Назва} AS Склад_Назва,
    Рег_РухТоварів.{РухТоварів_Const.Кількість} AS Кількість
FROM 
    {РухТоварів_Const.TABLE} AS Рег_РухТоварів

    LEFT JOIN {Номенклатура_Const.TABLE} AS Довідник_Номенклатура ON Довідник_Номенклатура.uid = 
        Рег_РухТоварів.{РухТоварів_Const.Номенклатура}

    LEFT JOIN {ХарактеристикиНоменклатури_Const.TABLE} AS Довідник_ХарактеристикиНоменклатури ON Довідник_ХарактеристикиНоменклатури.uid = 
        Рег_РухТоварів.{РухТоварів_Const.ХарактеристикаНоменклатури}

    LEFT JOIN {Склади_Const.TABLE} AS Довідник_Склади ON Довідник_Склади.uid = 
       Рег_РухТоварів.{РухТоварів_Const.Склад}
WHERE
    Рег_РухТоварів.Owner = @ДокументВказівник
ORDER BY Номенклатура_Назва
";

            return query;
        }

        private static string Запит_ЗамовленняКлієнтів()
        {
            string query = $@"
SELECT 
    Рег_ЗамовленняКлієнтів.period,
    Рег_ЗамовленняКлієнтів.income,
    Рег_ЗамовленняКлієнтів.{ЗамовленняКлієнтів_Const.ЗамовленняКлієнта} AS ЗамовленняКлієнта, 
    Документ_ЗамовленняКлієнта.{ЗамовленняКлієнта_Const.Назва} AS ЗамовленняКлієнта_Назва, 
    Рег_ЗамовленняКлієнтів.{ЗамовленняКлієнтів_Const.Номенклатура} AS Номенклатура, 
    Довідник_Номенклатура.{Номенклатура_Const.Назва} AS Номенклатура_Назва, 
    Рег_ЗамовленняКлієнтів.{ЗамовленняКлієнтів_Const.ХарактеристикаНоменклатури} AS ХарактеристикаНоменклатури,
    Довідник_ХарактеристикиНоменклатури.{ХарактеристикиНоменклатури_Const.Назва} AS ХарактеристикаНоменклатури_Назва, 
    Рег_ЗамовленняКлієнтів.{ЗамовленняКлієнтів_Const.Склад} AS Склад,
    Довідник_Склади.{Склади_Const.Назва} AS Склад_Назва,
    Рег_ЗамовленняКлієнтів.{ЗамовленняКлієнтів_Const.Замовлено} AS Замовлено,
    Рег_ЗамовленняКлієнтів.{ЗамовленняКлієнтів_Const.Сума} AS Сума
FROM 
    {ЗамовленняКлієнтів_Const.TABLE} AS Рег_ЗамовленняКлієнтів

    LEFT JOIN {ЗамовленняКлієнта_Const.TABLE} AS Документ_ЗамовленняКлієнта ON Документ_ЗамовленняКлієнта.uid = 
        Рег_ЗамовленняКлієнтів.{ЗамовленняКлієнтів_Const.ЗамовленняКлієнта}

    LEFT JOIN {Номенклатура_Const.TABLE} AS Довідник_Номенклатура ON Довідник_Номенклатура.uid = 
        Рег_ЗамовленняКлієнтів.{ЗамовленняКлієнтів_Const.Номенклатура}

    LEFT JOIN {ХарактеристикиНоменклатури_Const.TABLE} AS Довідник_ХарактеристикиНоменклатури ON Довідник_ХарактеристикиНоменклатури.uid = 
        Рег_ЗамовленняКлієнтів.{ЗамовленняКлієнтів_Const.ХарактеристикаНоменклатури}

    LEFT JOIN {Склади_Const.TABLE} AS Довідник_Склади ON Довідник_Склади.uid = 
       Рег_ЗамовленняКлієнтів.{ЗамовленняКлієнтів_Const.Склад}
WHERE
    Рег_ЗамовленняКлієнтів.Owner = @ЗамовленняКлієнта
ORDER BY Номенклатура_Назва
";

            return query;
        }

        private static string Запит_РозрахункиЗКлієнтами()
        {
            string query = $@"
SELECT 
    Рег_РозрахункиЗКлієнтами.period,
    Рег_РозрахункиЗКлієнтами.income, 
    Рег_РозрахункиЗКлієнтами.{РозрахункиЗКлієнтами_Const.ЗамовленняКлієнта} AS ЗамовленняКлієнта, 
    Документ_ЗамовленняКлієнта.{ЗамовленняКлієнта_Const.Назва} AS ЗамовленняКлієнта_Назва,
    Рег_РозрахункиЗКлієнтами.{РозрахункиЗКлієнтами_Const.Валюта} AS Валюта,
    Довідник_Валюти.{Валюти_Const.Назва} AS Валюта_Назва,
    Рег_РозрахункиЗКлієнтами.{РозрахункиЗКлієнтами_Const.Сума} AS Замовлено
FROM 
    {РозрахункиЗКлієнтами_Const.TABLE} AS Рег_РозрахункиЗКлієнтами

    LEFT JOIN {ЗамовленняКлієнта_Const.TABLE} AS Документ_ЗамовленняКлієнта ON Документ_ЗамовленняКлієнта.uid = 
        Рег_РозрахункиЗКлієнтами.{РозрахункиЗКлієнтами_Const.ЗамовленняКлієнта}

    LEFT JOIN {Валюти_Const.TABLE} AS Довідник_Валюти ON Довідник_Валюти.uid = 
       Рег_РозрахункиЗКлієнтами.{РозрахункиЗКлієнтами_Const.Валюта}
WHERE
    Рег_РозрахункиЗКлієнтами.Owner = @ДокументВказівник
ORDER BY Номенклатура_Назва
";

            return query;
        }

        private static string Запит_ВільніЗалишки()
        {
            string query = $@"
SELECT 
    Рег_ВільніЗалишки.period,
    Рег_ВільніЗалишки.income, 
    Рег_ВільніЗалишки.{ВільніЗалишки_Const.Номенклатура} AS Номенклатура, 
    Довідник_Номенклатура.{Номенклатура_Const.Назва} AS Номенклатура_Назва, 
    Рег_ВільніЗалишки.{ВільніЗалишки_Const.ХарактеристикаНоменклатури} AS ХарактеристикаНоменклатури,
    Довідник_ХарактеристикиНоменклатури.{ХарактеристикиНоменклатури_Const.Назва} AS ХарактеристикаНоменклатури_Назва, 
    Рег_ВільніЗалишки.{ВільніЗалишки_Const.Склад} AS Склад,
    Довідник_Склади.{Склади_Const.Назва} AS Склад_Назва,
    Рег_ВільніЗалишки.{ВільніЗалишки_Const.ВНаявності} AS ВНаявності,
    Рег_ВільніЗалишки.{ВільніЗалишки_Const.ВРезервіЗіСкладу} AS ВРезервіЗіСкладу,
    Рег_ВільніЗалишки.{ВільніЗалишки_Const.ВРезервіПідЗамовлення} AS ВРезервіПідЗамовлення
FROM 
    {ВільніЗалишки_Const.TABLE} AS Рег_ВільніЗалишки

    LEFT JOIN {Номенклатура_Const.TABLE} AS Довідник_Номенклатура ON Довідник_Номенклатура.uid = 
        Рег_ВільніЗалишки.{ВільніЗалишки_Const.Номенклатура}

    LEFT JOIN {ХарактеристикиНоменклатури_Const.TABLE} AS Довідник_ХарактеристикиНоменклатури ON Довідник_ХарактеристикиНоменклатури.uid = 
        Рег_ВільніЗалишки.{ВільніЗалишки_Const.ХарактеристикаНоменклатури}

    LEFT JOIN {Склади_Const.TABLE} AS Довідник_Склади ON Довідник_Склади.uid = 
       Рег_ВільніЗалишки.{ВільніЗалишки_Const.Склад}
WHERE
    Рег_ВільніЗалишки.Owner = @ДокументВказівник
ORDER BY Номенклатура_Назва
";

            return query;
        }

        private static string Запит_ЗамовленняПостачальникам()
        {
            string query = $@"
SELECT 
    Рег_ЗамовленняПостачальникам.period,
    Рег_ЗамовленняПостачальникам.income, 
    Рег_ЗамовленняПостачальникам.{ЗамовленняПостачальникам_Const.ЗамовленняПостачальнику} AS ЗамовленняПостачальнику, 
    Документ_ЗамовленняПостачальнику.{ЗамовленняПостачальнику_Const.Назва} AS ЗамовленняПостачальнику_Назва,
    Рег_ЗамовленняПостачальникам.{ТовариНаСкладах_Const.Номенклатура} AS Номенклатура, 
    Довідник_Номенклатура.{Номенклатура_Const.Назва} AS Номенклатура_Назва, 
    Рег_ЗамовленняПостачальникам.{ТовариНаСкладах_Const.ХарактеристикаНоменклатури} AS ХарактеристикаНоменклатури,
    Довідник_ХарактеристикиНоменклатури.{ХарактеристикиНоменклатури_Const.Назва} AS ХарактеристикаНоменклатури_Назва, 
    Рег_ЗамовленняПостачальникам.{ТовариНаСкладах_Const.Склад} AS Склад,
    Довідник_Склади.{Склади_Const.Назва} AS Склад_Назва,
    Рег_ЗамовленняПостачальникам.{ТовариНаСкладах_Const.ВНаявності} AS Замовлено
FROM 
    {ЗамовленняПостачальникам_Const.TABLE} AS Рег_ЗамовленняПостачальникам

    LEFT JOIN {ЗамовленняПостачальнику_Const.TABLE} AS Документ_ЗамовленняПостачальнику ON Документ_ЗамовленняПостачальнику.uid = 
        Рег_ЗамовленняПостачальникам.{ЗамовленняПостачальникам_Const.ЗамовленняПостачальнику}

    LEFT JOIN {Номенклатура_Const.TABLE} AS Довідник_Номенклатура ON Довідник_Номенклатура.uid = 
        Рег_ЗамовленняПостачальникам.{ЗамовленняПостачальникам_Const.Номенклатура}

    LEFT JOIN {ХарактеристикиНоменклатури_Const.TABLE} AS Довідник_ХарактеристикиНоменклатури ON Довідник_ХарактеристикиНоменклатури.uid = 
        Рег_ЗамовленняПостачальникам.{ЗамовленняПостачальникам_Const.ХарактеристикаНоменклатури}

    LEFT JOIN {Склади_Const.TABLE} AS Довідник_Склади ON Довідник_Склади.uid = 
       Рег_ЗамовленняПостачальникам.{ЗамовленняПостачальникам_Const.Склад}
WHERE
    Рег_ЗамовленняПостачальникам.Owner = @ДокументВказівник
ORDER BY Номенклатура_Назва
";

            return query;
        }

        private static string Запит_РозрахункиЗПостачальниками()
        {
            string query = $@"
SELECT 
    Рег_РозрахункиЗПостачальниками.period,
    Рег_РозрахункиЗПостачальниками.income, 
    Рег_РозрахункиЗПостачальниками.{РозрахункиЗПостачальниками_Const.ЗамовленняПостачальнику} AS ЗамовленняПостачальнику, 
    Документ_ЗамовленняПостачальнику.{ЗамовленняПостачальнику_Const.Назва} AS ЗамовленняПостачальнику_Назва,
    Рег_РозрахункиЗПостачальниками.{РозрахункиЗПостачальниками_Const.Валюта} AS Валюта,
    Довідник_Валюти.{Валюти_Const.Назва} AS Валюта_Назва,
    Рег_РозрахункиЗПостачальниками.{РозрахункиЗПостачальниками_Const.Сума} AS Замовлено,
    Рег_РозрахункиЗПостачальниками.{РозрахункиЗПостачальниками_Const.ФормаОплати} AS ФормаОплати,
    Рег_РозрахункиЗПостачальниками.{РозрахункиЗПостачальниками_Const.ГосподарськаОперація} AS ГосподарськаОперація
FROM 
    {РозрахункиЗПостачальниками_Const.TABLE} AS Рег_РозрахункиЗПостачальниками

    LEFT JOIN {ЗамовленняПостачальнику_Const.TABLE} AS Документ_ЗамовленняПостачальнику ON Документ_ЗамовленняПостачальнику.uid = 
        Рег_РозрахункиЗПостачальниками.{РозрахункиЗПостачальниками_Const.ЗамовленняПостачальнику}

    LEFT JOIN {Валюти_Const.TABLE} AS Довідник_Валюти ON Довідник_Валюти.uid = 
       Рег_РозрахункиЗПостачальниками.{РозрахункиЗПостачальниками_Const.Валюта}
WHERE
    Рег_РозрахункиЗПостачальниками.Owner = @ДокументВказівник
ORDER BY Номенклатура_Назва
";

            return query;
        }

        private static string Запит_ТовариДоПоступлення()
        {
            string query = $@"
SELECT 
    Рег_ТовариДоПоступлення.period,
    Рег_ТовариДоПоступлення.income, 
    Рег_ТовариДоПоступлення.{ТовариДоПоступлення_Const.Номенклатура} AS Номенклатура, 
    Довідник_Номенклатура.{Номенклатура_Const.Назва} AS Номенклатура_Назва, 
    Рег_ТовариДоПоступлення.{ТовариДоПоступлення_Const.ХарактеристикаНоменклатури} AS ХарактеристикаНоменклатури,
    Довідник_ХарактеристикиНоменклатури.{ХарактеристикиНоменклатури_Const.Назва} AS ХарактеристикаНоменклатури_Назва, 
    Рег_ТовариДоПоступлення.{ТовариДоПоступлення_Const.Склад} AS Склад,
    Довідник_Склади.{Склади_Const.Назва} AS Склад_Назва,
    Рег_ТовариДоПоступлення.{ТовариДоПоступлення_Const.ДоПоступлення} AS ДоПоступлення
FROM 
    {ТовариДоПоступлення_Const.TABLE} AS Рег_ТовариДоПоступлення

    LEFT JOIN {Номенклатура_Const.TABLE} AS Довідник_Номенклатура ON Довідник_Номенклатура.uid = 
        Рег_ТовариДоПоступлення.{ТовариДоПоступлення_Const.Номенклатура}

    LEFT JOIN {ХарактеристикиНоменклатури_Const.TABLE} AS Довідник_ХарактеристикиНоменклатури ON Довідник_ХарактеристикиНоменклатури.uid = 
        Рег_ТовариДоПоступлення.{ТовариДоПоступлення_Const.ХарактеристикаНоменклатури}

    LEFT JOIN {Склади_Const.TABLE} AS Довідник_Склади ON Довідник_Склади.uid = 
       Рег_ТовариДоПоступлення.{ТовариДоПоступлення_Const.Склад}
WHERE
    Рег_ТовариДоПоступлення.Owner = @ДокументВказівник
ORDER BY Номенклатура_Назва
";

            return query;
        }

        #endregion

        #region Функції

        private static void DataToXML(XmlDocument xmlDoc, string registerName, string[] columnsName, List<object[]> listRow)
        {
            XmlNode root = xmlDoc.ChildNodes[0];

            XmlElement rootItemNode = xmlDoc.CreateElement(registerName);
            root.AppendChild(rootItemNode);

            foreach (object[] row in listRow)
            {
                int counter = 0;

                XmlElement nodeRow = xmlDoc.CreateElement("row");
                root.AppendChild(nodeRow);

                foreach (string col in columnsName)
                {
                    XmlElement node = xmlDoc.CreateElement(col);
                    node.InnerText = row[counter].ToString();
                    nodeRow.AppendChild(node);

                    counter++;
                }
            }
        }

        private XmlDocument CreateXmlDocument()
        {
            XmlDocument xmlConfDocument = new XmlDocument();
            xmlConfDocument.AppendChild(xmlConfDocument.CreateXmlDeclaration("1.0", "utf-8", ""));

            XmlElement rootNode = xmlConfDocument.CreateElement("root");
            xmlConfDocument.AppendChild(rootNode);

            return xmlConfDocument;
        }

        private void XmlDocumentSaveAndTransform(XmlDocument xmlDoc)
        {
            string pathToFolder = Path.GetDirectoryName(Application.ExecutablePath);
            string pathToTemplate = Path.Combine(@"E:\Project\StorageAndTrade\StorageAndTrade\Звіти\РухДокументівПоРегістрах", "Template_РухДокументівПоРегістрах.xslt");

            string pathToXmlFile = Path.Combine(pathToFolder, "SaveXML_Report.xml");
            string pathToHtmlFile = Path.Combine(pathToFolder, "SaveXML_Report.html");

            xmlDoc.Save(pathToXmlFile);

            XslCompiledTransform xsltTransform = new XslCompiledTransform();
            xsltTransform.Load(pathToTemplate, new XsltSettings(), null);

            xsltTransform.Transform(pathToXmlFile, pathToHtmlFile);

            System.Diagnostics.Process.Start("firefox.exe", pathToHtmlFile);
        }

        #endregion

        public static void PrintRegisterRecords(ЗамовленняКлієнта_Pointer ДокументВказівник)
        {
            //
            //ЗамовленняКлієнтів
            //

            Console.WriteLine("Док: " + ДокументВказівник.UnigueID.ToString());

            string query = $@"
SELECT 
    Рег_ЗамовленняКлієнтів.period,
    Рег_ЗамовленняКлієнтів.income,
    Рег_ЗамовленняКлієнтів.{ЗамовленняКлієнтів_Const.ЗамовленняКлієнта} AS ЗамовленняКлієнта, 
    Документ_ЗамовленняКлієнта.{ЗамовленняКлієнта_Const.Назва} AS ЗамовленняКлієнта_Назва, 
    Рег_ЗамовленняКлієнтів.{ЗамовленняКлієнтів_Const.Номенклатура} AS Номенклатура, 
    Довідник_Номенклатура.{Номенклатура_Const.Назва} AS Номенклатура_Назва, 
    Рег_ЗамовленняКлієнтів.{ЗамовленняКлієнтів_Const.ХарактеристикаНоменклатури} AS ХарактеристикаНоменклатури,
    Довідник_ХарактеристикиНоменклатури.{ХарактеристикиНоменклатури_Const.Назва} AS ХарактеристикаНоменклатури_Назва, 
    Рег_ЗамовленняКлієнтів.{ЗамовленняКлієнтів_Const.Склад} AS Склад,
    Довідник_Склади.{Склади_Const.Назва} AS Склад_Назва,
    Рег_ЗамовленняКлієнтів.{ЗамовленняКлієнтів_Const.Замовлено} AS Замовлено,
    Рег_ЗамовленняКлієнтів.{ЗамовленняКлієнтів_Const.Сума} AS Сума
FROM 
    {ЗамовленняКлієнтів_Const.TABLE} AS Рег_ЗамовленняКлієнтів

    LEFT JOIN {ЗамовленняКлієнта_Const.TABLE} AS Документ_ЗамовленняКлієнта ON Документ_ЗамовленняКлієнта.uid = 
        Рег_ЗамовленняКлієнтів.{ЗамовленняКлієнтів_Const.ЗамовленняКлієнта}

    LEFT JOIN {Номенклатура_Const.TABLE} AS Довідник_Номенклатура ON Довідник_Номенклатура.uid = 
        Рег_ЗамовленняКлієнтів.{ЗамовленняКлієнтів_Const.Номенклатура}

    LEFT JOIN {ХарактеристикиНоменклатури_Const.TABLE} AS Довідник_ХарактеристикиНоменклатури ON Довідник_ХарактеристикиНоменклатури.uid = 
        Рег_ЗамовленняКлієнтів.{ЗамовленняКлієнтів_Const.ХарактеристикаНоменклатури}

    LEFT JOIN {Склади_Const.TABLE} AS Довідник_Склади ON Довідник_Склади.uid = 
       Рег_ЗамовленняКлієнтів.{ЗамовленняКлієнтів_Const.Склад}
WHERE
    Рег_ЗамовленняКлієнтів.Owner = @ЗамовленняКлієнта
ORDER BY Номенклатура_Назва
";

            Console.WriteLine(query);

            Dictionary<string, object> paramQuery = new Dictionary<string, object>();
            paramQuery.Add("ЗамовленняКлієнта", ДокументВказівник.UnigueID.UGuid);

            string[] columnsName;
            List<object[]> listRow;

            Config.Kernel.DataBase.SelectRequest(query, paramQuery, out columnsName, out listRow);

            //SaveXML(columnsName, listRow);
        }

    }
}

