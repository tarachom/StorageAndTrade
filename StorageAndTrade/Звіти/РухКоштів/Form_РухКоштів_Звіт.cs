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

using AccountingSoftware;
using System.Xml;
using System.Xml.Xsl;
using System.IO;

using StorageAndTrade_1_0;
using StorageAndTrade_1_0.Константи;
using StorageAndTrade_1_0.Довідники;
using StorageAndTrade_1_0.Документи;
using StorageAndTrade_1_0.РегістриНакопичення;
using StorageAndTrade_1_0.Звіти;

namespace StorageAndTrade
{
    public partial class Form_РухКоштів_Звіт : Form
    {
        public Form_РухКоштів_Звіт()
        {
            InitializeComponent();
        }

        private void Form_РухКоштів_Звіт_Load(object sender, EventArgs e)
        {
            directoryControl_Організація.Init(new Form_Організації(), new Організації_Pointer());
            directoryControl_Каса.Init(new Form_Каси(), new Каси_Pointer());
            directoryControl_Валюти.Init(new Form_Валюти(), new Валюти_Pointer());

            geckoWebBrowser.LoadHtml("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\"><p>Звіт не сформований</p>");
        }

        private void buttonCreate_Click(object sender, EventArgs e)
        {
            bool isExistParent = false;

            string query = $@"
SELECT 
    РухКоштів_Місяць.{ВіртуальніТаблиціРегістрів.РухКоштів_Місяць_TablePart.Організація} AS Організація,
    Довідник_Організації.{Організації_Const.Назва} AS Організація_Назва,
    РухКоштів_Місяць.{ВіртуальніТаблиціРегістрів.РухКоштів_Місяць_TablePart.Каса} AS Каса,
    Довідник_Каси.{Каси_Const.Назва} AS Каса_Назва,
    РухКоштів_Місяць.{ВіртуальніТаблиціРегістрів.РухКоштів_Місяць_TablePart.Валюта} AS Валюта,
    Довідник_Валюти.{Валюти_Const.Назва} AS Валюта_Назва,
    SUM(РухКоштів_Місяць.{ВіртуальніТаблиціРегістрів.РухКоштів_Місяць_TablePart.Сума}) AS Сума
FROM 
    {ВіртуальніТаблиціРегістрів.РухКоштів_Місяць_TablePart.TABLE} AS РухКоштів_Місяць

    LEFT JOIN {Організації_Const.TABLE} AS Довідник_Організації ON Довідник_Організації.uid = 
        РухКоштів_Місяць.{ВіртуальніТаблиціРегістрів.РухКоштів_Місяць_TablePart.Організація}

    LEFT JOIN {Каси_Const.TABLE} AS Довідник_Каси ON Довідник_Каси.uid = 
        РухКоштів_Місяць.{ВіртуальніТаблиціРегістрів.РухКоштів_Місяць_TablePart.Каса}

    LEFT JOIN {Валюти_Const.TABLE} AS Довідник_Валюти ON Довідник_Валюти.uid = 
        РухКоштів_Місяць.{ВіртуальніТаблиціРегістрів.РухКоштів_Місяць_TablePart.Валюта}
";

            #region WHERE

            //Відбір по вибраному елементу Організації
            if (!directoryControl_Організація.DirectoryPointerItem.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
Довідник_Організації.uid = '{directoryControl_Організація.DirectoryPointerItem.UnigueID}'
";
            }

            //Відбір по вибраному елементу Валюти
            if (!directoryControl_Каса.DirectoryPointerItem.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
Довідник_Каси.uid = '{directoryControl_Каса.DirectoryPointerItem.UnigueID}'
";
            }

            //Відбір по вибраному елементу Валюти
            if (!directoryControl_Валюти.DirectoryPointerItem.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
Довідник_Валюти.uid = '{directoryControl_Валюти.DirectoryPointerItem.UnigueID}'
";
            }

            #endregion

            query += $@"
GROUP BY Організація, Організація_Назва, 
         Каса, Каса_Назва,
         Валюта, Валюта_Назва

HAVING
     SUM(РухКоштів_Місяць.{ВіртуальніТаблиціРегістрів.РухКоштів_Місяць_TablePart.Сума}) != 0

ORDER BY Організація_Назва, Каса_Назва, Валюта_Назва
";

            //Console.WriteLine(query);

            Dictionary<string,string> allowDocuments = new Dictionary<string, string>();

            foreach (ConfigurationDocuments configurationDocuments in Config.Kernel.Conf.Documents.Values)
            {
                if (configurationDocuments.AllowRegisterAccumulation.Contains("РухКоштів"))
                    allowDocuments.Add(configurationDocuments.Table, configurationDocuments.Fields["Назва"].NameInTable);
            }

            string queryDoc = $@"
SELECT
    uid,
    Назва
FROM (";
            int counter = 0;

            foreach (KeyValuePair<string, string> document in allowDocuments)
            {
                string UNION = counter > 0 ? "UNION" : "";
                counter++;

                queryDoc += $@"
{UNION} 
(
SELECT
    Рег_РухКоштів.owner AS uid,
    {document.Key}.{document.Value} AS Назва
FROM
    {РухКоштів_Const.TABLE} AS Рег_РухКоштів
        INNER JOIN {document.Key} ON {document.Key}.uid = Рег_РухКоштів.owner
)";
            }

            queryDoc += $@"
) AS all_documents
ORDER BY Назва
";

            //Console.WriteLine(queryDoc);

            XmlDocument xmlDoc =  Функції.CreateXmlDocument();

            Dictionary<string, object> paramQuery = new Dictionary<string, object>();

            string[] columnsName;
            List<object[]> listRow;

            Config.Kernel.DataBase.SelectRequest(query, paramQuery, out columnsName, out listRow);
            Функції.DataToXML(xmlDoc, "РухКоштів", columnsName, listRow);

            Config.Kernel.DataBase.SelectRequest(queryDoc, paramQuery, out columnsName, out listRow);
            Функції.DataToXML(xmlDoc, "Документи", columnsName, listRow);

            Функції.XmlDocumentSaveAndTransform(xmlDoc, @"Шаблони\РухКоштів.xslt", false, "Рух коштів");

            string pathToHtmlFile = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "Report.html");
            geckoWebBrowser.Navigate(pathToHtmlFile);
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
