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
using System.Xml;
using System.Xml.Xsl;

using StorageAndTrade_1_0;
using StorageAndTrade_1_0.Довідники;
using StorageAndTrade_1_0.Документи;
using StorageAndTrade_1_0.РегістриНакопичення;

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

            SaveXML(columnsName, listRow);
        }

        public static void SaveXML(string[] columnsName, List<object[]> listRow)
        {
            XmlDocument xmlConfDocument = new XmlDocument();
            xmlConfDocument.AppendChild(xmlConfDocument.CreateXmlDeclaration("1.0", "utf-8", ""));

            XmlElement rootNode = xmlConfDocument.CreateElement("root");
            xmlConfDocument.AppendChild(rootNode);

            int counter;

            foreach (object[] row in listRow)
            {
                counter = 0;

                XmlElement nodeRow = xmlConfDocument.CreateElement("row");
                rootNode.AppendChild(nodeRow);

                foreach (string col in columnsName)
                {
                    XmlElement node = xmlConfDocument.CreateElement(col);
                    node.InnerText = row[counter].ToString();
                    nodeRow.AppendChild(node);

                    counter++;
                }
            }

            xmlConfDocument.Save(@"E:\Project\StorageAndTrade\StorageAndTrade\bin\Debug\SaveXML_Report.xml");

            XslCompiledTransform xsltTransform = new XslCompiledTransform();
            xsltTransform.Load(@"E:\Project\StorageAndTrade\StorageAndTrade\Документи\ЗамовленняКлієнта\Report_ЗамовленняКлієнта_РухПоРегістрах.xslt", new XsltSettings(), null);

            xsltTransform.Transform(@"E:\Project\StorageAndTrade\StorageAndTrade\bin\Debug\SaveXML_Report.xml",
                @"E:\Project\StorageAndTrade\StorageAndTrade\bin\Debug\SaveXML_Report.html");


        }

    }
}

