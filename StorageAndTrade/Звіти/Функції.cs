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

namespace StorageAndTrade_1_0.Звіти
{
    /// <summary>
    /// Функції для звітів
    /// </summary>
    public class Функції
    {
        /// <summary>
        /// Створює хмл документ та корінну вітку root
        /// </summary>
        /// <returns>хмл документ</returns>
        public static XmlDocument CreateXmlDocument()
        {
            XmlDocument xmlConfDocument = new XmlDocument();
            xmlConfDocument.AppendChild(xmlConfDocument.CreateXmlDeclaration("1.0", "utf-8", ""));

            XmlElement rootNode = xmlConfDocument.CreateElement("root");
            xmlConfDocument.AppendChild(rootNode);

            return xmlConfDocument;
        }

        /// <summary>
        /// Записує блок даних в хмл документ
        /// </summary>
        /// <param name="xmlDoc">Хмл документ</param>
        /// <param name="blockName">Назва блоку даних</param>
        /// <param name="columnsName">Масив стовпчиків</param>
        /// <param name="listRow">Список рядків</param>
        public static void DataToXML(XmlDocument xmlDoc, string blockName, string[] columnsName, List<object[]> listRow)
        {
            XmlNode root= xmlDoc.SelectSingleNode("/root");

            XmlElement rootItemNode = xmlDoc.CreateElement(blockName);
            root.AppendChild(rootItemNode);

            foreach (object[] row in listRow)
            {
                int counter = 0;

                XmlElement nodeRow = xmlDoc.CreateElement("row");
                rootItemNode.AppendChild(nodeRow);

                foreach (string col in columnsName)
                {
                    XmlElement node = xmlDoc.CreateElement(col);
                    node.InnerText = row[counter].ToString();
                    nodeRow.AppendChild(node);

                    counter++;
                }
            }
        }

        /// <summary>
        /// Записує хмл документ та трансформує його в HTML
        /// </summary>
        /// <param name="xmlDoc">хмл документ</param>
        /// <param name="pathToTemplate">шлях до шаблону XSLT</param>
        public static void XmlDocumentSaveAndTransform(XmlDocument xmlDoc, string pathToTemplate)
        {
            string pathToFolder = Path.GetDirectoryName(Application.ExecutablePath);
            string pathToXmlFile = Path.Combine(pathToFolder, "Report.xml");
            string pathToHtmlFile = Path.Combine(pathToFolder, "Report.html");

            xmlDoc.Save(pathToXmlFile);

            XslCompiledTransform xsltTransform = new XslCompiledTransform();
            xsltTransform.Load(pathToTemplate, new XsltSettings(), null);

            xsltTransform.Transform(pathToXmlFile, pathToHtmlFile);

            System.Diagnostics.Process.Start("firefox.exe", pathToHtmlFile); //"iexplore.exe"
        }

    }
}

