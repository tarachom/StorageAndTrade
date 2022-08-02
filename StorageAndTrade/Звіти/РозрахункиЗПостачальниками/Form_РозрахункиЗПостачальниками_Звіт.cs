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
using System.IO;
using System.Xml;
using System.Xml.Xsl;

using StorageAndTrade_1_0;
using StorageAndTrade_1_0.Довідники;
using StorageAndTrade_1_0.Документи;
using StorageAndTrade_1_0.РегістриНакопичення;
using StorageAndTrade_1_0.Звіти;
using StorageAndTrade_1_0.Константи;
using StorageAndTrade_1_0.Журнали;

namespace StorageAndTrade
{
    public partial class Form_РозрахункиЗПостачальниками_Звіт : Form
    {
        public Form_РозрахункиЗПостачальниками_Звіт()
        {
            InitializeComponent();
        }

        private void Form_РозрахункиЗПостачальниками_Звіт_Load(object sender, EventArgs e)
        {
            directoryControl_КонтрагентиПапка.Init(new Form_КонтрагентиПапкиВибір(), new Контрагенти_Папки_Pointer());
            directoryControl_Контрагенти.Init(new Form_Контрагенти(), new Контрагенти_Pointer());
            directoryControl_Валюти.Init(new Form_Валюти(), new Валюти_Pointer());
        }

        private void buttonCreate_Click(object sender, EventArgs e)
        {
            bool isExistParent = false;

            string query = $@"
SELECT 
    РозрахункиЗПостачальниками_Місяць.{ВіртуальніТаблиціРегістрів.РозрахункиЗПостачальниками_Місяць_TablePart.Контрагент} AS Контрагент,
    Довідник_Контрагенти.{Контрагенти_Const.Назва} AS Контрагент_Назва,
    РозрахункиЗПостачальниками_Місяць.{ВіртуальніТаблиціРегістрів.РозрахункиЗПостачальниками_Місяць_TablePart.Валюта} AS Валюта, 
    Довідник_Валюти.{Валюти_Const.Назва} AS Валюта_Назва,
    SUM(РозрахункиЗПостачальниками_Місяць.{ВіртуальніТаблиціРегістрів.РозрахункиЗПостачальниками_Місяць_TablePart.Сума}) AS Сума
FROM 
    {ВіртуальніТаблиціРегістрів.РозрахункиЗПостачальниками_Місяць_TablePart.TABLE} AS РозрахункиЗПостачальниками_Місяць

    LEFT JOIN {Валюти_Const.TABLE} AS Довідник_Валюти ON Довідник_Валюти.uid = 
        РозрахункиЗПостачальниками_Місяць.{ВіртуальніТаблиціРегістрів.РозрахункиЗПостачальниками_Місяць_TablePart.Валюта}

    LEFT JOIN {Контрагенти_Const.TABLE} AS Довідник_Контрагенти ON Довідник_Контрагенти.uid = 
        РозрахункиЗПостачальниками_Місяць.{ВіртуальніТаблиціРегістрів.РозрахункиЗПостачальниками_Місяць_TablePart.Контрагент}
";
            #region WHERE

            //Відбір по всіх вкладених папках вибраної папки Контрагенти
            if (!directoryControl_КонтрагентиПапка.DirectoryPointerItem.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
Довідник_Контрагенти.{Контрагенти_Const.Папка} IN 
    (
        WITH RECURSIVE r AS 
        (
            SELECT uid
            FROM {Контрагенти_Папки_Const.TABLE}
            WHERE {Контрагенти_Папки_Const.TABLE}.uid = '{directoryControl_КонтрагентиПапка.DirectoryPointerItem.UnigueID}' 

            UNION ALL

            SELECT {Контрагенти_Папки_Const.TABLE}.uid
            FROM {Контрагенти_Папки_Const.TABLE}
                JOIN r ON {Контрагенти_Папки_Const.TABLE}.{Контрагенти_Папки_Const.Родич} = r.uid
        ) SELECT uid FROM r
    )
";
            }

            //Відбір по вибраному елементу Контрагенту
            if (!directoryControl_Контрагенти.DirectoryPointerItem.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
Довідник_Контрагенти.uid = '{directoryControl_Контрагенти.DirectoryPointerItem.UnigueID}'
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
GROUP BY Контрагент, Контрагент_Назва, 
         Валюта, Валюта_Назва

HAVING
     SUM(РозрахункиЗПостачальниками_Місяць.{ВіртуальніТаблиціРегістрів.РозрахункиЗПостачальниками_Місяць_TablePart.Сума}) != 0

ORDER BY Контрагент_Назва
";
            
            XmlDocument xmlDoc =  Функції.CreateXmlDocument();

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

            Функції.DataToXML(xmlDoc, "РозрахункиЗПостачальниками", columnsName, listRow);

            Функції.XmlDocumentSaveAndTransform(xmlDoc, @"Шаблони\РозрахункиЗПостачальниками_Залишки.xslt", false, "Розрахунки з постачальниками");

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
        РозрахункиЗПостачальниками.period AS period,
        РозрахункиЗПостачальниками.owner AS owner,
        РозрахункиЗПостачальниками.income AS income,
        РозрахункиЗПостачальниками.{РозрахункиЗПостачальниками_Const.Контрагент} AS Контрагент,
        РозрахункиЗПостачальниками.{РозрахункиЗПостачальниками_Const.Валюта} AS Валюта,
        РозрахункиЗПостачальниками.{РозрахункиЗПостачальниками_Const.Сума} AS Сума
    FROM
        {РозрахункиЗПостачальниками_Const.TABLE} AS РозрахункиЗПостачальниками
    WHERE
        (РозрахункиЗПостачальниками.period >= @period_start AND РозрахункиЗПостачальниками.period <= @period_end)
";

            #region WHERE

            isExistParent = true;

            //Відбір по вибраному елементу Контрагенти
            if (!directoryControl_Контрагенти.DirectoryPointerItem.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                query += $@"
РозрахункиЗПостачальниками.{РозрахункиЗПостачальниками_Const.Контрагент} = '{directoryControl_Контрагенти.DirectoryPointerItem.UnigueID}'
";
            }

            //Відбір по вибраному елементу Валюти
            if (!directoryControl_Валюти.DirectoryPointerItem.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
РозрахункиЗПостачальниками.{РозрахункиЗПостачальниками_Const.Валюта} = '{directoryControl_Валюти.DirectoryPointerItem.UnigueID}'
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
    documents.Сума,
    Довідник_Контрагенти.{Контрагенти_Const.Назва} AS Контрагент_Назва,
    Довідник_Валюти.{Валюти_Const.Назва} AS Валюта_Назва
FROM documents INNER JOIN {table} ON {table}.uid = documents.owner
    LEFT JOIN {Контрагенти_Const.TABLE} AS Довідник_Контрагенти ON Довідник_Контрагенти.uid = documents.Контрагент
    LEFT JOIN {Валюти_Const.TABLE} AS Довідник_Валюти ON Довідник_Валюти.uid = documents.Валюта
";

                #region WHERE

                isExistParent = false;

                //Відбір по всіх вкладених папках вибраної папки Контрагенти
                if (!directoryControl_КонтрагентиПапка.DirectoryPointerItem.IsEmpty())
                {
                    query += isExistParent ? "AND" : "WHERE";
                    isExistParent = true;

                    query += $@"
Довідник_Контрагенти.{Контрагенти_Const.Папка} IN 
    (
        WITH RECURSIVE r AS 
        (
            SELECT uid
            FROM {Контрагенти_Папки_Const.TABLE}
            WHERE {Контрагенти_Папки_Const.TABLE}.uid = '{directoryControl_КонтрагентиПапка.DirectoryPointerItem.UnigueID}' 

            UNION ALL

            SELECT {Контрагенти_Папки_Const.TABLE}.uid
            FROM {Контрагенти_Папки_Const.TABLE}
                JOIN r ON {Контрагенти_Папки_Const.TABLE}.{Контрагенти_Папки_Const.Родич} = r.uid
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
    Сума, 
    Контрагент_Назва,
    Валюта_Назва
FROM doc
ORDER BY period ASC
";

            Dictionary<string, object> paramQuery = new Dictionary<string, object>();
            paramQuery.Add("period_start", DateTime.Parse($"{dateTimeStart.Value.Day}.{dateTimeStart.Value.Month}.{dateTimeStart.Value.Year} 00:00:00"));
            paramQuery.Add("period_end", DateTime.Parse($"{dateTimeStop.Value.Day}.{dateTimeStop.Value.Month}.{dateTimeStop.Value.Year} 23:59:59"));

            string[] columnsName;
            List<object[]> listRow;

            Config.Kernel.DataBase.SelectRequest(query, paramQuery, out columnsName, out listRow);
            Функції.DataToXML(xmlDoc, "Документи", columnsName, listRow);

            Функції.XmlDocumentSaveAndTransform(xmlDoc, @"Шаблони\РозрахункиЗПостачальниками_Документи.xslt", false, "Розрахунки з постачальниками");

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
        РозрахункиЗПостачальниками_Місяць.{ВіртуальніТаблиціРегістрів.РозрахункиЗПостачальниками_Місяць_TablePart.Контрагент} AS Контрагент,
        РозрахункиЗПостачальниками_Місяць.{ВіртуальніТаблиціРегістрів.РозрахункиЗПостачальниками_Місяць_TablePart.Валюта} AS Валюта,
        SUM(РозрахункиЗПостачальниками_Місяць.{ВіртуальніТаблиціРегістрів.РозрахункиЗПостачальниками_Місяць_TablePart.Сума}) AS Сума
    FROM 
        {ВіртуальніТаблиціРегістрів.РозрахункиЗПостачальниками_Місяць_TablePart.TABLE} AS РозрахункиЗПостачальниками_Місяць
    WHERE
        РозрахункиЗПостачальниками_Місяць.{ВіртуальніТаблиціРегістрів.РозрахункиЗПостачальниками_Місяць_TablePart.Період} < @period_month_end

    GROUP BY Контрагент, Валюта
), 
ostatok_day AS
(
    SELECT
        'day' AS block,
        РозрахункиЗПостачальниками_День.{ВіртуальніТаблиціРегістрів.РозрахункиЗПостачальниками_День_TablePart.Контрагент} AS Контрагент,
        РозрахункиЗПостачальниками_День.{ВіртуальніТаблиціРегістрів.РозрахункиЗПостачальниками_День_TablePart.Валюта} AS Валюта,
        SUM(РозрахункиЗПостачальниками_День.{ВіртуальніТаблиціРегістрів.РозрахункиЗПостачальниками_День_TablePart.Сума}) AS Сума
    FROM 
        {ВіртуальніТаблиціРегістрів.РозрахункиЗПостачальниками_День_TablePart.TABLE} AS РозрахункиЗПостачальниками_День
    WHERE
        РозрахункиЗПостачальниками_День.{ВіртуальніТаблиціРегістрів.РозрахункиЗПостачальниками_День_TablePart.Період} >= @period_day_start AND
        РозрахункиЗПостачальниками_День.{ВіртуальніТаблиціРегістрів.РозрахункиЗПостачальниками_День_TablePart.Період } < @period_day_end

    GROUP BY Контрагент, Валюта
), 
ostatok_period AS
(   
    SELECT
        'period' AS block,
        РозрахункиЗПостачальниками_День.{ВіртуальніТаблиціРегістрів.РозрахункиЗПостачальниками_День_TablePart.Контрагент} AS Контрагент,
        РозрахункиЗПостачальниками_День.{ВіртуальніТаблиціРегістрів.РозрахункиЗПостачальниками_День_TablePart.Валюта} AS Валюта,
        SUM(РозрахункиЗПостачальниками_День.{ВіртуальніТаблиціРегістрів.РозрахункиЗПостачальниками_День_TablePart.Сума}) AS Сума
    FROM 
        {ВіртуальніТаблиціРегістрів.РозрахункиЗПостачальниками_День_TablePart.TABLE} AS РозрахункиЗПостачальниками_День
    WHERE
        РозрахункиЗПостачальниками_День.{ВіртуальніТаблиціРегістрів.РозрахункиЗПостачальниками_День_TablePart.Період} >= @period_ostatok_start AND
        РозрахункиЗПостачальниками_День.{ВіртуальніТаблиціРегістрів.РозрахункиЗПостачальниками_День_TablePart.Період } <= @period_ostatok_end

    GROUP BY Контрагент, Валюта
),
ostatok_na_potshatok_periodu AS
(
    SELECT
       Контрагент,
       Валюта,
       SUM(Сума) AS Сума
    FROM 
    (
        SELECT * FROM ostatok_month
        UNION
        SELECT * FROM ostatok_day
    ) AS ostatok
    GROUP BY Контрагент, Валюта
),
ostatok_na_kinec_periodu AS
(
    SELECT
       Контрагент,
       Валюта,
       SUM(Сума) AS Сума
    FROM 
    (
        SELECT * FROM ostatok_month
        UNION
        SELECT * FROM ostatok_day
        UNION
        SELECT * FROM ostatok_period
    ) AS ostatok
    GROUP BY Контрагент, Валюта
),
oborot AS
(
    SELECT 
        РозрахункиЗПостачальниками.{РозрахункиЗПостачальниками_Const.Контрагент} AS Контрагент,
        РозрахункиЗПостачальниками.{РозрахункиЗПостачальниками_Const.Валюта} AS Валюта,
        SUM(CASE WHEN РозрахункиЗПостачальниками.income = true THEN РозрахункиЗПостачальниками.{РозрахункиЗПостачальниками_Const.Сума} END) AS Прихід,
        SUM(CASE WHEN РозрахункиЗПостачальниками.income = false THEN РозрахункиЗПостачальниками.{РозрахункиЗПостачальниками_Const.Сума} END) AS Розхід
    FROM
        {РозрахункиЗПостачальниками_Const.TABLE} AS РозрахункиЗПостачальниками
    WHERE
        РозрахункиЗПостачальниками.period >= @period_oborot_start AND
        РозрахункиЗПостачальниками.period <= @period_oborot_end
    GROUP BY Контрагент, Валюта
)

SELECT 
    Контрагент,
    Довідник_Контрагенти.{Контрагенти_Const.Назва} AS Контрагент_Назва,
    Валюта,
    Довідник_Валюти.{Валюти_Const.Назва} AS Валюта_Назва,
    SUM(ПочатковийЗалишок) AS ПочатковийЗалишок,
    SUM(Прихід) AS Прихід,
    SUM(Розхід) AS Розхід,
    SUM(КінцевийЗалишок) AS КінцевийЗалишок
FROM 
(
    SELECT 
        'A',
        Контрагент,
        Валюта,
        Сума AS ПочатковийЗалишок,
        0 AS Прихід,
        0 AS Розхід,
        0 AS КінцевийЗалишок
    FROM ostatok_na_potshatok_periodu

    UNION

    SELECT
        'B',
        Контрагент,
        Валюта,
        0 AS ПочатковийЗалишок,
        0 AS Прихід,
        0 AS Розхід,
        Сума AS КінцевийЗалишок
    FROM ostatok_na_kinec_periodu

    UNION

    SELECT
        'C',
        Контрагент,
        Валюта,
        0 AS ПочатковийЗалишок,
        Прихід AS Прихід,
        Розхід AS Розхід,
        0 AS КінцевийЗалишок
    FROM oborot
) AS ЗалишкиТаОбороти

LEFT JOIN {Контрагенти_Const.TABLE} AS Довідник_Контрагенти ON Довідник_Контрагенти.uid = ЗалишкиТаОбороти.Контрагент
LEFT JOIN {Валюти_Const.TABLE} AS Довідник_Валюти ON Довідник_Валюти.uid = ЗалишкиТаОбороти.Валюта
";

            #region WHERE

            //Відбір по всіх вкладених папках вибраної папки Контрагенти
            if (!directoryControl_КонтрагентиПапка.DirectoryPointerItem.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
Довідник_Контрагенти.{Контрагенти_Const.Папка} IN 
    (
        WITH RECURSIVE r AS 
        (
            SELECT uid
            FROM {Контрагенти_Папки_Const.TABLE}
            WHERE {Контрагенти_Папки_Const.TABLE}.uid = '{directoryControl_КонтрагентиПапка.DirectoryPointerItem.UnigueID}' 

            UNION ALL

            SELECT {Контрагенти_Папки_Const.TABLE}.uid
            FROM {Контрагенти_Папки_Const.TABLE}
                JOIN r ON {Контрагенти_Папки_Const.TABLE}.{Контрагенти_Папки_Const.Родич} = r.uid
        ) SELECT uid FROM r
    )
";
            }

            //Відбір по вибраному елементу Контрагенти
            if (!directoryControl_Контрагенти.DirectoryPointerItem.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
Довідник_Контрагенти.uid = '{directoryControl_Контрагенти.DirectoryPointerItem.UnigueID}'
";
            }

            //Відбір по вибраному елементу Склади
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
GROUP BY Контрагент, Контрагент_Назва, Валюта, Валюта_Назва
ORDER BY Контрагент_Назва, Валюта_Назва
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

            Функції.XmlDocumentSaveAndTransform(xmlDoc, @"Шаблони\РозрахункиЗПостачальниками_ЗалишкиТаОбороти.xslt", false, "Розрахунки з постачальниками");

            string pathToHtmlFile = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "Report.html");
            geckoWebBrowser.Navigate(pathToHtmlFile);
        }
        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
