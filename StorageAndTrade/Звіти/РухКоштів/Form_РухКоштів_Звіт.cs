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
            geckoWebBrowser.Reload();

            directoryControl_Організація.Init(new Form_Організації(), new Організації_Pointer(), ПошуковіЗапити.Організації);
            directoryControl_Каса.Init(new Form_Каси(), new Каси_Pointer(), ПошуковіЗапити.Каси);
            directoryControl_Валюти.Init(new Form_Валюти(), new Валюти_Pointer(), ПошуковіЗапити.Валюти);

            dateTimeStart.Value = DateTime.Parse($"01.{DateTime.Now.Month}.{DateTime.Now.Year}");

            geckoWebBrowser.DomClick += GeckoWebBrowser.DomClick;
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

            //Console.WriteLine(queryDoc);

            XmlDocument xmlDoc =  ФункціїДляЗвітів.CreateXmlDocument();

            ФункціїДляЗвітів.DataHeadToXML(xmlDoc, "head",
                new List<NameValue<string>>()
                {
                    new NameValue<string>("КінецьПеріоду", DateTime.Now.ToString("dd.MM.yyyy"))
                }
            );

            Dictionary<string, object> paramQuery = new Dictionary<string, object>();
            paramQuery.Add("period_start", DateTime.Parse($"01.{dateTimeStart.Value.Month}.{dateTimeStart.Value.Year} 00:00:00"));
            paramQuery.Add("period_end", DateTime.Parse($"01.{dateTimeStop.Value.Month}.{dateTimeStop.Value.Year} 00:00:00"));

            string[] columnsName;
            List<object[]> listRow;

            Config.Kernel.DataBase.SelectRequest(query, paramQuery, out columnsName, out listRow);
            ФункціїДляЗвітів.DataToXML(xmlDoc, "РухКоштів", columnsName, listRow);

            ФункціїДляЗвітів.XmlDocumentSaveAndTransform(xmlDoc, @"Шаблони\РухКоштів_Залишки.xslt", false, "Рух коштів");

            string pathToHtmlFile = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "Report.html");
            geckoWebBrowser.Navigate(pathToHtmlFile);
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void ЗалишкиТаОбороти(XmlDocument xmlDoc)
        {
            bool isExistParent = false;

            string query = $@"
WITH ostatok_month AS
(
    SELECT
        'month' AS block,
        РухКоштів_Місяць.{ВіртуальніТаблиціРегістрів.РухКоштів_Місяць_TablePart.Організація} AS Організація,
        РухКоштів_Місяць.{ВіртуальніТаблиціРегістрів.РухКоштів_Місяць_TablePart.Каса} AS Каса,
        РухКоштів_Місяць.{ВіртуальніТаблиціРегістрів.РухКоштів_Місяць_TablePart.Валюта} AS Валюта,
        SUM(РухКоштів_Місяць.{ВіртуальніТаблиціРегістрів.РухКоштів_Місяць_TablePart.Сума}) AS Залишок
    FROM 
        {ВіртуальніТаблиціРегістрів.РухКоштів_Місяць_TablePart.TABLE} AS РухКоштів_Місяць
    WHERE
        РухКоштів_Місяць.{ВіртуальніТаблиціРегістрів.РухКоштів_Місяць_TablePart.Період} < @period_month_end

    GROUP BY Організація, Каса, Валюта
), 
ostatok_day AS
(
    SELECT
        'day' AS block,
        РухКоштів_День.{ВіртуальніТаблиціРегістрів.РухКоштів_День_TablePart.Організація} AS Організація,
        РухКоштів_День.{ВіртуальніТаблиціРегістрів.РухКоштів_День_TablePart.Каса} AS Каса,
        РухКоштів_День.{ВіртуальніТаблиціРегістрів.РухКоштів_День_TablePart.Валюта} AS Валюта,
        SUM(РухКоштів_День.{ВіртуальніТаблиціРегістрів.РухКоштів_День_TablePart.Сума}) AS Залишок
    FROM 
        {ВіртуальніТаблиціРегістрів.РухКоштів_День_TablePart.TABLE} AS РухКоштів_День
    WHERE
        РухКоштів_День.{ВіртуальніТаблиціРегістрів.РухКоштів_День_TablePart.Період} >= @period_day_start AND
        РухКоштів_День.{ ВіртуальніТаблиціРегістрів.РухКоштів_День_TablePart.Період } < @period_day_end

    GROUP BY Організація, Каса, Валюта
), 
ostatok_period AS
(   
    SELECT
        'period' AS block,
        РухКоштів_День.{ВіртуальніТаблиціРегістрів.РухКоштів_День_TablePart.Організація} AS Організація,
        РухКоштів_День.{ВіртуальніТаблиціРегістрів.РухКоштів_День_TablePart.Каса} AS Каса,
        РухКоштів_День.{ВіртуальніТаблиціРегістрів.РухКоштів_День_TablePart.Валюта} AS Валюта,
        SUM(РухКоштів_День.{ВіртуальніТаблиціРегістрів.РухКоштів_День_TablePart.Сума}) AS Залишок
    FROM 
        {ВіртуальніТаблиціРегістрів.РухКоштів_День_TablePart.TABLE} AS РухКоштів_День
    WHERE
        РухКоштів_День.{ВіртуальніТаблиціРегістрів.РухКоштів_День_TablePart.Період} >= @period_ostatok_start AND
        РухКоштів_День.{ ВіртуальніТаблиціРегістрів.РухКоштів_День_TablePart.Період } <= @period_ostatok_end

    GROUP BY Організація, Каса, Валюта
),
ostatok_na_potshatok_periodu AS
(
    SELECT
       Організація,
       Каса,
       Валюта,
       SUM(Залишок) AS Залишок
    FROM 
    (
        SELECT * FROM ostatok_month
        UNION
        SELECT * FROM ostatok_day
    ) AS ostatok
    GROUP BY Організація, Каса, Валюта
),
ostatok_na_kinec_periodu AS
(
    SELECT
       Організація,
       Каса,
       Валюта,
       SUM(Залишок) AS Залишок
    FROM 
    (
        SELECT * FROM ostatok_month
        UNION
        SELECT * FROM ostatok_day
        UNION
        SELECT * FROM ostatok_period
    ) AS ostatok
    GROUP BY Організація, Каса, Валюта
),
oborot AS
(
    SELECT 
        РухКоштів.{РухКоштів_Const.Організація} AS Організація,
        РухКоштів.{РухКоштів_Const.Каса} AS Каса,
        РухКоштів.{РухКоштів_Const.Валюта} AS Валюта,
        SUM(CASE WHEN РухКоштів.income = true THEN РухКоштів.{РухКоштів_Const.Сума} END) AS Прихід,
        SUM(CASE WHEN РухКоштів.income = false THEN РухКоштів.{РухКоштів_Const.Сума} END) AS Розхід
    FROM
        {РухКоштів_Const.TABLE} AS РухКоштів
    WHERE
        РухКоштів.period >= @period_oborot_start AND
        РухКоштів.period <= @period_oborot_end
    GROUP BY Організація, Каса, Валюта
)

SELECT 
    Організація,
    Довідник_Організації.{Організації_Const.Назва} AS Організація_Назва,
    Каса,
    Довідник_Каси.{Каси_Const.Назва} AS Каса_Назва,
    Валюта,
    Довідник_Валюти.{Валюти_Const.Назва} AS Валюта_Назва,
    SUM(ПочатковийЗалишок) AS ПочатковийЗалишок,
    SUM(Прихід) AS Прихід,
    SUM(Розхід) AS Розхід,
    SUM(Прихід) - SUM(Розхід) AS Оборот,
    SUM(КінцевийЗалишок) AS КінцевийЗалишок
FROM 
(
    SELECT 
        'A',
        Організація,
        Каса,
        Валюта,
        Залишок AS ПочатковийЗалишок,
        0 AS Прихід,
        0 AS Розхід,
        0 AS КінцевийЗалишок
    FROM ostatok_na_potshatok_periodu

    UNION

    SELECT
        'B',
        Організація,
        Каса,
        Валюта,
        0 AS ПочатковийЗалишок,
        0 AS Прихід,
        0 AS Розхід,
        Залишок AS КінцевийЗалишок
    FROM ostatok_na_kinec_periodu

    UNION

    SELECT
        'C',
        Організація,
        Каса,
        Валюта,
        0 AS ПочатковийЗалишок,
        Прихід AS Прихід,
        Розхід AS Розхід,
        0 AS КінцевийЗалишок
    FROM oborot
) AS ЗалишкиТаОбороти

LEFT JOIN {Організації_Const.TABLE} AS Довідник_Організації ON Довідник_Організації.uid = ЗалишкиТаОбороти.Організація
LEFT JOIN {Каси_Const.TABLE} AS Довідник_Каси ON Довідник_Каси.uid = ЗалишкиТаОбороти.Каса
LEFT JOIN {Валюти_Const.TABLE} AS Довідник_Валюти ON Довідник_Валюти.uid = ЗалишкиТаОбороти.Валюта
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

            query += @"
GROUP BY Організація, Організація_Назва, Каса, Каса_Назва, Валюта, Валюта_Назва
HAVING SUM(Прихід) != 0 OR SUM(Розхід) != 0
ORDER BY Організація_Назва, Каса_Назва, Валюта_Назва
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
            ФункціїДляЗвітів.DataToXML(xmlDoc, "ЗалишкиТаОбороти", columnsName, listRow);
        }

        private void button_Report_Click(object sender, EventArgs e)
        {
            XmlDocument xmlDoc = ФункціїДляЗвітів.CreateXmlDocument();

            ФункціїДляЗвітів.DataHeadToXML(xmlDoc, "head",
                new List<NameValue<string>>()
                {
                    new NameValue<string>("ПочатокПеріоду", dateTimeStart.Value.ToString("dd.MM.yyyy")),
                    new NameValue<string>("КінецьПеріоду", dateTimeStop.Value.ToString("dd.MM.yyyy"))
                }
            );

            ЗалишкиТаОбороти(xmlDoc);

            ФункціїДляЗвітів.XmlDocumentSaveAndTransform(xmlDoc, @"Шаблони\РухКоштів_ЗалишкиТаОбороти.xslt", false, "Рух коштів");

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
        РухКоштів.period AS period,
        РухКоштів.owner AS owner,
        РухКоштів.income AS income,
        РухКоштів.{РухКоштів_Const.Сума} AS Сума,
        РухКоштів.{РухКоштів_Const.Організація} AS Організація,
        РухКоштів.{РухКоштів_Const.Каса} AS Каса,
        РухКоштів.{РухКоштів_Const.Валюта} AS Валюта
    FROM
        {РухКоштів_Const.TABLE} AS РухКоштів
    WHERE
        (РухКоштів.period >= @period_start AND РухКоштів.period <= @period_end)
";

            #region WHERE

            isExistParent = true;

            //Відбір по вибраному елементу Організації
            if (!directoryControl_Організація.DirectoryPointerItem.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                query += $@"
РухКоштів.{РухКоштів_Const.Організація} = '{directoryControl_Організація.DirectoryPointerItem.UnigueID}'
";
            }

            //Відбір по вибраному елементу Валюти
            if (!directoryControl_Каса.DirectoryPointerItem.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
РухКоштів.{РухКоштів_Const.Каса} = '{directoryControl_Каса.DirectoryPointerItem.UnigueID}'
";
            }

            //Відбір по вибраному елементу Валюти
            if (!directoryControl_Валюти.DirectoryPointerItem.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
РухКоштів.{РухКоштів_Const.Валюта} = '{directoryControl_Валюти.DirectoryPointerItem.UnigueID}'
";
            }

            #endregion

            query += $@"
),
documents AS
(";
            int counter = 0; 
            foreach (string table in РухКоштів_Const.AllowDocumentSpendTable)
            {
                string docType = РухКоштів_Const.AllowDocumentSpendType[counter];

                string union = (counter > 0 ? "UNION" : "");
                query += $@"
{union}
SELECT 
    '{docType}' AS doctype,
    {table}.uid, 
    {table}.docname, 
    register.period, 
    register.income, 
    register.Сума,
    register.Організація,
    Довідник_Організації.{Організації_Const.Назва} AS Організація_Назва,
    register.Каса,
    Довідник_Каси.{Каси_Const.Назва} AS Каса_Назва,
    register.Валюта,
    Довідник_Валюти.{Валюти_Const.Назва} AS Валюта_Назва
FROM register INNER JOIN {table} ON {table}.uid = register.owner
    LEFT JOIN {Організації_Const.TABLE} AS Довідник_Організації ON Довідник_Організації.uid = register.Організація
    LEFT JOIN {Каси_Const.TABLE} AS Довідник_Каси ON Довідник_Каси.uid = register.Каса
    LEFT JOIN {Валюти_Const.TABLE} AS Довідник_Валюти ON Довідник_Валюти.uid = register.Валюта";

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
    Сума, 
    Організація,
    Організація_Назва,
    Каса,
    Каса_Назва,
    Валюта,
    Валюта_Назва
FROM documents
ORDER BY period ASC
";
            //Console.WriteLine(query);

            Dictionary<string, object> paramQuery = new Dictionary<string, object>();
            paramQuery.Add("period_start", DateTime.Parse($"{dateTimeStart.Value.Day}.{dateTimeStart.Value.Month}.{dateTimeStart.Value.Year} 00:00:00"));
            paramQuery.Add("period_end", DateTime.Parse($"{dateTimeStop.Value.Day}.{dateTimeStop.Value.Month}.{dateTimeStop.Value.Year} 23:59:59"));

            string[] columnsName;
            List<object[]> listRow;

            Config.Kernel.DataBase.SelectRequest(query, paramQuery, out columnsName, out listRow);
            ФункціїДляЗвітів.DataToXML(xmlDoc, "Документи", columnsName, listRow);

            ФункціїДляЗвітів.XmlDocumentSaveAndTransform(xmlDoc, @"Шаблони\РухКоштів_Документи.xslt", false, "Рух коштів");

            string pathToHtmlFile = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "Report.html");
            geckoWebBrowser.Navigate(pathToHtmlFile);
        }
    }
}



//SUM(CASE WHEN income = true THEN Сума ELSE -Сума END) OVER (order by period) AS СумаПідсумок



//Console.WriteLine(query);

//            Dictionary<string,string> allowDocuments = new Dictionary<string, string>();

//            foreach (ConfigurationDocuments configurationDocuments in Config.Kernel.Conf.Documents.Values)
//            {
//                if (configurationDocuments.AllowRegisterAccumulation.Contains("РухКоштів"))
//                    allowDocuments.Add(configurationDocuments.Table, configurationDocuments.Fields["Назва"].NameInTable);
//            }

//            string queryDoc = $@"
//SELECT
//    uid,
//    Назва
//FROM (";
//            int counter = 0;

//            foreach (KeyValuePair<string, string> document in allowDocuments)
//            {
//                string UNION = counter > 0 ? "UNION" : "";
//                counter++;

//                queryDoc += $@"
//{UNION} 
//(
//SELECT
//    Рег_РухКоштів.owner AS uid,
//    {document.Key}.{document.Value} AS Назва
//FROM
//    {РухКоштів_Const.TABLE} AS Рег_РухКоштів
//        INNER JOIN {document.Key} ON {document.Key}.uid = Рег_РухКоштів.owner
//)";
//            }

//            queryDoc += $@"
//) AS all_documents
//ORDER BY Назва
//";




//void ЗалишокНаКінецьМинулогоМісяця(XmlDocument xmlDoc)
//{
//    string query = $@"
//SELECT 
//    РухКоштів_Місяць.{ВіртуальніТаблиціРегістрів.РухКоштів_Місяць_TablePart.Організація} AS Організація,
//    Довідник_Організації.{Організації_Const.Назва} AS Організація_Назва,
//    РухКоштів_Місяць.{ВіртуальніТаблиціРегістрів.РухКоштів_Місяць_TablePart.Каса} AS Каса,
//    Довідник_Каси.{Каси_Const.Назва} AS Каса_Назва,
//    РухКоштів_Місяць.{ВіртуальніТаблиціРегістрів.РухКоштів_Місяць_TablePart.Валюта} AS Валюта,
//    Довідник_Валюти.{Валюти_Const.Назва} AS Валюта_Назва,
//    SUM(РухКоштів_Місяць.{ВіртуальніТаблиціРегістрів.РухКоштів_Місяць_TablePart.Сума}) AS Сума
//FROM 
//    {ВіртуальніТаблиціРегістрів.РухКоштів_Місяць_TablePart.TABLE} AS РухКоштів_Місяць

//    LEFT JOIN {Організації_Const.TABLE} AS Довідник_Організації ON Довідник_Організації.uid = 
//        РухКоштів_Місяць.{ВіртуальніТаблиціРегістрів.РухКоштів_Місяць_TablePart.Організація}

//    LEFT JOIN {Каси_Const.TABLE} AS Довідник_Каси ON Довідник_Каси.uid = 
//        РухКоштів_Місяць.{ВіртуальніТаблиціРегістрів.РухКоштів_Місяць_TablePart.Каса}

//    LEFT JOIN {Валюти_Const.TABLE} AS Довідник_Валюти ON Довідник_Валюти.uid = 
//        РухКоштів_Місяць.{ВіртуальніТаблиціРегістрів.РухКоштів_Місяць_TablePart.Валюта}
//WHERE
//    РухКоштів_Місяць.{ВіртуальніТаблиціРегістрів.РухКоштів_Місяць_TablePart.Період} < @period_end

//GROUP BY Організація, Організація_Назва, 
//         Каса, Каса_Назва,
//         Валюта, Валюта_Назва

//HAVING
//     SUM(РухКоштів_Місяць.{ВіртуальніТаблиціРегістрів.РухКоштів_Місяць_TablePart.Сума}) != 0

//ORDER BY Організація_Назва, Каса_Назва, Валюта_Назва
//";

//    Dictionary<string, object> paramQuery = new Dictionary<string, object>();
//    paramQuery.Add("period_end", DateTime.Parse($"01.{dateTimeStart.Value.Month}.{dateTimeStart.Value.Year} 00:00:00"));

//    Console.WriteLine(paramQuery["period_end"].ToString());

//    string[] columnsName;
//    List<object[]> listRow;

//    Config.Kernel.DataBase.SelectRequest(query, paramQuery, out columnsName, out listRow);
//    Функції.DataToXML(xmlDoc, "РухКоштів", columnsName, listRow);
//}

//void ЗалишокЗПочаткуМісяцяДоПочаткуПеріоду(XmlDocument xmlDoc)
//{
//    string query = $@"
//SELECT 
//    РухКоштів_День.{ВіртуальніТаблиціРегістрів.РухКоштів_День_TablePart.Організація} AS Організація,
//    Довідник_Організації.{Організації_Const.Назва} AS Організація_Назва,
//    РухКоштів_День.{ВіртуальніТаблиціРегістрів.РухКоштів_День_TablePart.Каса} AS Каса,
//    Довідник_Каси.{Каси_Const.Назва} AS Каса_Назва,
//    РухКоштів_День.{ВіртуальніТаблиціРегістрів.РухКоштів_День_TablePart.Валюта} AS Валюта,
//    Довідник_Валюти.{Валюти_Const.Назва} AS Валюта_Назва,
//    SUM(РухКоштів_День.{ВіртуальніТаблиціРегістрів.РухКоштів_День_TablePart.Сума}) AS Сума
//FROM 
//    {ВіртуальніТаблиціРегістрів.РухКоштів_День_TablePart.TABLE} AS РухКоштів_День

//    LEFT JOIN {Організації_Const.TABLE} AS Довідник_Організації ON Довідник_Організації.uid = 
//        РухКоштів_День.{ВіртуальніТаблиціРегістрів.РухКоштів_День_TablePart.Організація}

//    LEFT JOIN {Каси_Const.TABLE} AS Довідник_Каси ON Довідник_Каси.uid = 
//        РухКоштів_День.{ВіртуальніТаблиціРегістрів.РухКоштів_День_TablePart.Каса}

//    LEFT JOIN {Валюти_Const.TABLE} AS Довідник_Валюти ON Довідник_Валюти.uid = 
//        РухКоштів_День.{ВіртуальніТаблиціРегістрів.РухКоштів_День_TablePart.Валюта}
//WHERE
//    РухКоштів_День.{ВіртуальніТаблиціРегістрів.РухКоштів_День_TablePart.Період} >= @period_start AND
//    РухКоштів_День.{ВіртуальніТаблиціРегістрів.РухКоштів_День_TablePart.Період} < @period_end

//GROUP BY Організація, Організація_Назва, 
//         Каса, Каса_Назва,
//         Валюта, Валюта_Назва

//HAVING
//     SUM(РухКоштів_День.{ВіртуальніТаблиціРегістрів.РухКоштів_День_TablePart.Сума}) != 0

//ORDER BY Організація_Назва, Каса_Назва, Валюта_Назва
//";

//    Dictionary<string, object> paramQuery = new Dictionary<string, object>();
//    paramQuery.Add("period_start", DateTime.Parse($"01.{dateTimeStart.Value.Month}.{dateTimeStart.Value.Year} 00:00:00"));
//    paramQuery.Add("period_end", DateTime.Parse($"{dateTimeStart.Value.Day}.{dateTimeStart.Value.Month}.{dateTimeStart.Value.Year} 00:00:00"));

//    Console.WriteLine(paramQuery["period_start"].ToString() + " - " + paramQuery["period_end"].ToString());

//    string[] columnsName;
//    List<object[]> listRow;

//    Config.Kernel.DataBase.SelectRequest(query, paramQuery, out columnsName, out listRow);
//    Функції.DataToXML(xmlDoc, "РухКоштів2", columnsName, listRow);
//}

//void ОборотЗаПеріод(XmlDocument xmlDoc)
//{
//    string query = $@"
//SELECT 
//    РухКоштів.{РухКоштів_Const.Організація} AS Організація,
//    Довідник_Організації.{Організації_Const.Назва} AS Організація_Назва,
//    РухКоштів.{РухКоштів_Const.Каса} AS Каса,
//    Довідник_Каси.{Каси_Const.Назва} AS Каса_Назва,
//    РухКоштів.{РухКоштів_Const.Валюта} AS Валюта,
//    Довідник_Валюти.{Валюти_Const.Назва} AS Валюта_Назва,
//    SUM(CASE WHEN РухКоштів.income = true THEN РухКоштів.{РухКоштів_Const.Сума} END) AS Прихід,
//    SUM(CASE WHEN РухКоштів.income = false THEN РухКоштів.{РухКоштів_Const.Сума} END) AS Розхід
//FROM
//    {РухКоштів_Const.TABLE} AS РухКоштів

//    LEFT JOIN {Організації_Const.TABLE} AS Довідник_Організації ON Довідник_Організації.uid = 
//        РухКоштів.{РухКоштів_Const.Організація}

//    LEFT JOIN {Каси_Const.TABLE} AS Довідник_Каси ON Довідник_Каси.uid = 
//        РухКоштів.{РухКоштів_Const.Каса}

//    LEFT JOIN {Валюти_Const.TABLE} AS Довідник_Валюти ON Довідник_Валюти.uid = 
//        РухКоштів.{РухКоштів_Const.Валюта}
//WHERE
//    РухКоштів.period >= @period_start AND
//    РухКоштів.period <= @period_end

//GROUP BY Організація, Організація_Назва, 
//         Каса, Каса_Назва,
//         Валюта, Валюта_Назва

//ORDER BY Організація_Назва, Каса_Назва, Валюта_Назва
//";

//    Dictionary<string, object> paramQuery = new Dictionary<string, object>();
//    paramQuery.Add("period_start", DateTime.Parse($"{dateTimeStart.Value.Day}.{dateTimeStart.Value.Month}.{dateTimeStart.Value.Year} 00:00:00"));
//    paramQuery.Add("period_end", DateTime.Parse($"{dateTimeStop.Value.Day}.{dateTimeStop.Value.Month}.{dateTimeStop.Value.Year} 23:59:59"));

//    Console.WriteLine(paramQuery["period_start"].ToString() + " - " + paramQuery["period_end"].ToString());

//    string[] columnsName;
//    List<object[]> listRow;

//    Config.Kernel.DataBase.SelectRequest(query, paramQuery, out columnsName, out listRow);
//    Функції.DataToXML(xmlDoc, "РухКоштів3", columnsName, listRow);
//}

//void ЗалишкиТаОбороти_delete(XmlDocument xmlDoc)
//{
//    string query = $@"
//SELECT
//    block,
//    Організація,
//    Довідник_Організації.{Організації_Const.Назва} AS Організація_Назва,
//    Каса,
//    Довідник_Каси.{Каси_Const.Назва} AS Каса_Назва,
//    Валюта,
//    Довідник_Валюти.{Валюти_Const.Назва} AS Валюта_Назва,
//    SUM(ПочатковийЗалишок) AS ПочатковийЗалишок,
//    SUM(Прихід) AS Прихід,
//    SUM(Розхід) AS Розхід,
//    SUM(КінцевийЗалишок) AS КінцевийЗалишок
//FROM (
//    SELECT 
//        'month' AS block,
//        РухКоштів_Місяць.{ВіртуальніТаблиціРегістрів.РухКоштів_Місяць_TablePart.Організація} AS Організація,
//        РухКоштів_Місяць.{ВіртуальніТаблиціРегістрів.РухКоштів_Місяць_TablePart.Каса} AS Каса,
//        РухКоштів_Місяць.{ВіртуальніТаблиціРегістрів.РухКоштів_Місяць_TablePart.Валюта} AS Валюта,
//        РухКоштів_Місяць.{ВіртуальніТаблиціРегістрів.РухКоштів_Місяць_TablePart.Сума} AS ПочатковийЗалишок,
//        0 AS Прихід,
//        0 AS Розхід,
//        0 AS КінцевийЗалишок
//    FROM 
//        {ВіртуальніТаблиціРегістрів.РухКоштів_Місяць_TablePart.TABLE} AS РухКоштів_Місяць
//    WHERE
//        РухКоштів_Місяць.{ВіртуальніТаблиціРегістрів.РухКоштів_Місяць_TablePart.Період} < @period_month_end

//    UNION

//    SELECT 
//        'day' AS block,
//        РухКоштів_День.{ВіртуальніТаблиціРегістрів.РухКоштів_День_TablePart.Організація} AS Організація,
//        РухКоштів_День.{ВіртуальніТаблиціРегістрів.РухКоштів_День_TablePart.Каса} AS Каса,
//        РухКоштів_День.{ВіртуальніТаблиціРегістрів.РухКоштів_День_TablePart.Валюта} AS Валюта,
//        РухКоштів_День.{ВіртуальніТаблиціРегістрів.РухКоштів_День_TablePart.Сума} AS ПочатковийЗалишок,
//        0 AS Прихід,
//        0 AS Розхід,
//        0 AS КінцевийЗалишок
//    FROM 
//        {ВіртуальніТаблиціРегістрів.РухКоштів_День_TablePart.TABLE} AS РухКоштів_День
//    WHERE
//        РухКоштів_День.{ВіртуальніТаблиціРегістрів.РухКоштів_День_TablePart.Період} >= @period_day_start AND
//        РухКоштів_День.{ВіртуальніТаблиціРегістрів.РухКоштів_День_TablePart.Період} < @period_day_end

//    UNION

//    SELECT 
//        'oborot' AS block,
//        РухКоштів.{РухКоштів_Const.Організація} AS Організація,
//        РухКоштів.{РухКоштів_Const.Каса} AS Каса,
//        РухКоштів.{РухКоштів_Const.Валюта} AS Валюта,
//        0 AS ПочатковийЗалишок,
//        CASE WHEN РухКоштів.income = true THEN РухКоштів.{РухКоштів_Const.Сума} END AS Прихід,
//        CASE WHEN РухКоштів.income = false THEN РухКоштів.{РухКоштів_Const.Сума} END AS Розхід,
//        0 AS КінцевийЗалишок
//    FROM
//        {РухКоштів_Const.TABLE} AS РухКоштів
//    WHERE
//        РухКоштів.period >= @period_oborot_start AND
//        РухКоштів.period <= @period_oborot_end
//) AS ЗалишкиТаОбороти

//    LEFT JOIN {Організації_Const.TABLE} AS Довідник_Організації ON Довідник_Організації.uid = ЗалишкиТаОбороти.Організація
//    LEFT JOIN {Каси_Const.TABLE} AS Довідник_Каси ON Довідник_Каси.uid = ЗалишкиТаОбороти.Каса
//    LEFT JOIN {Валюти_Const.TABLE} AS Довідник_Валюти ON Довідник_Валюти.uid = ЗалишкиТаОбороти.Валюта

//GROUP BY block, Організація, Організація_Назва, 
//         Каса, Каса_Назва,
//         Валюта, Валюта_Назва

//ORDER BY Організація_Назва, Каса_Назва, Валюта_Назва
//";

//    Dictionary<string, object> paramQuery = new Dictionary<string, object>();
//    paramQuery.Add("period_month_end", DateTime.Parse($"01.{dateTimeStart.Value.Month}.{dateTimeStart.Value.Year} 00:00:00"));

//    paramQuery.Add("period_day_start", DateTime.Parse($"01.{dateTimeStart.Value.Month}.{dateTimeStart.Value.Year} 00:00:00"));
//    paramQuery.Add("period_day_end", DateTime.Parse($"{dateTimeStart.Value.Day}.{dateTimeStart.Value.Month}.{dateTimeStart.Value.Year} 00:00:00"));

//    paramQuery.Add("period_oborot_start", DateTime.Parse($"{dateTimeStart.Value.Day}.{dateTimeStart.Value.Month}.{dateTimeStart.Value.Year} 00:00:00"));
//    paramQuery.Add("period_oborot_end", DateTime.Parse($"{dateTimeStop.Value.Day}.{dateTimeStop.Value.Month}.{dateTimeStop.Value.Year} 23:59:59"));

//    foreach (KeyValuePair<string, object> p in paramQuery)
//        Console.WriteLine($"{p.Key} = {p.Value}");

//    string[] columnsName;
//    List<object[]> listRow;

//    Config.Kernel.DataBase.SelectRequest(query, paramQuery, out columnsName, out listRow);
//    Функції.DataToXML(xmlDoc, "ЗалишкиТаОбороти", columnsName, listRow);
//}