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
    public partial class Form_ПартіїТоварівПоНоменклатурі : Form
    {
        public Form_ПартіїТоварівПоНоменклатурі()
        {
            InitializeComponent();

            geckoWebBrowser = GeckoWebBrowser.AddGeckoWebBrowserControl(this, new Point(2, 170));

            directoryControl_Організація.Init(new Form_Організації(), new Організації_Pointer(), ПошуковіЗапити.Організації);
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

            geckoWebBrowser.DomClick += GeckoWebBrowser.DomClick;
        }

        Gecko.GeckoWebBrowser geckoWebBrowser { get; set; }

        private void Form_ПартіїТоварівПоНоменклатурі_Load(object sender, EventArgs e)
        {
            geckoWebBrowser.Reload();
        }

        public Номенклатура_Pointer Номенклатура 
        {
            get
            {
                return (Номенклатура_Pointer)directoryControl_Номенклатура.DirectoryPointerItem;
            }
            set
            {
                directoryControl_Номенклатура.DirectoryPointerItem = value;
            }
        }

        public void CreateReport()
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

            //Відбір по вибраному елементу Номенклатура
            if (!Номенклатура.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
Довідник_Номенклатура.uid = '{Номенклатура.UnigueID}'
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

            //Відбір по вибраному елементу Серії Номенклатури
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

ORDER BY Організація_Назва, ПартіяТоварівКомпозит_Дата, Номенклатура_Назва, ХарактеристикаНоменклатури_Назва
";

            XmlDocument xmlDoc = ФункціїДляЗвітів.CreateXmlDocument();

            ФункціїДляЗвітів.DataHeadToXML(xmlDoc, "head",
                new List<NameValue<string>>()
                {
                    new NameValue<string>("КінецьПеріоду", DateTime.Now.ToString("dd.MM.yyyy")),
                    new NameValue<string>("Номенклатура", directoryControl_Номенклатура.GetPresentation())
                }
            );

            Dictionary<string, object> paramQuery = new Dictionary<string, object>();

            string[] columnsName;
            List<object[]> listRow;

            Config.Kernel.DataBase.SelectRequest(query, paramQuery, out columnsName, out listRow);

            ФункціїДляЗвітів.DataToXML(xmlDoc, "ПартіїТоварів", columnsName, listRow);

            if (!Номенклатура.IsEmpty())
                ФункціїДляЗвітів.XmlDocumentSaveAndTransform(xmlDoc, @"Шаблони\ПартіїТоварів_ЗалишкиПоНоменклатурі.xslt", false);
            else
                ФункціїДляЗвітів.XmlDocumentSaveAndTransform(xmlDoc, @"Шаблони\ПартіїТоварів_Залишки.xslt", false);

            string pathToHtmlFile = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "Report.html");
            geckoWebBrowser.Navigate(pathToHtmlFile);
        }

        private void buttonOstatok_Click(object sender, EventArgs e)
        {
            CreateReport();
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}