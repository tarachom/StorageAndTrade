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
    public partial class Form_ТовариНаСкладахПоНоменклатурі : Form
    {
        public Form_ТовариНаСкладахПоНоменклатурі()
        {
            InitializeComponent();

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
            directoryControl_СкладиПапки.Init(new Form_СкладиПапкиВибір(), new Склади_Папки_Pointer(), ПошуковіЗапити.Склади_Папки);
            directoryControl_Склади.Init(new Form_Склади(), new Склади_Pointer(), ПошуковіЗапити.Склади);
            directoryControl_Серія.Init(new Form_СеріїНоменклатури(), new СеріїНоменклатури_Pointer(), ПошуковіЗапити.СеріїНоменклатури);

            geckoWebBrowser.DomClick += GeckoWebBrowser.DomClick;
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

        private void Form_ТовариНаСкладахПоНоменклатурі_Load(object sender, EventArgs e)
        {
            
        }

        public void CreateReport()
        {
            bool isExistParent = false;

            string query = $@"
SELECT 
    ТовариНаСкладах_Підсумок.{ВіртуальніТаблиціРегістрів.ТовариНаСкладах_Підсумок_TablePart.Номенклатура} AS Номенклатура, 
    Довідник_Номенклатура.{Номенклатура_Const.Назва} AS Номенклатура_Назва, 
    ТовариНаСкладах_Підсумок.{ВіртуальніТаблиціРегістрів.ТовариНаСкладах_Підсумок_TablePart.ХарактеристикаНоменклатури} AS ХарактеристикаНоменклатури,
    Довідник_ХарактеристикиНоменклатури.{ХарактеристикиНоменклатури_Const.Назва} AS ХарактеристикаНоменклатури_Назва,
    ТовариНаСкладах_Підсумок.{ВіртуальніТаблиціРегістрів.ТовариНаСкладах_Підсумок_TablePart.Склад} AS Склад,
    Довідник_Склади.{Склади_Const.Назва} AS Склад_Назва,
    ТовариНаСкладах_Підсумок.{ВіртуальніТаблиціРегістрів.ТовариНаСкладах_Підсумок_TablePart.Серія} AS Серія,
    Довідник_СеріїНоменклатури.{СеріїНоменклатури_Const.Номер} AS Серія_Номер,
    SUM(ТовариНаСкладах_Підсумок.{ВіртуальніТаблиціРегістрів.ТовариНаСкладах_Підсумок_TablePart.ВНаявності}) AS ВНаявності
FROM 
    {ВіртуальніТаблиціРегістрів.ТовариНаСкладах_Підсумок_TablePart.TABLE} AS ТовариНаСкладах_Підсумок

    LEFT JOIN {Номенклатура_Const.TABLE} AS Довідник_Номенклатура ON Довідник_Номенклатура.uid = 
        ТовариНаСкладах_Підсумок.{ВіртуальніТаблиціРегістрів.ТовариНаСкладах_Підсумок_TablePart.Номенклатура}

    LEFT JOIN {ХарактеристикиНоменклатури_Const.TABLE} AS Довідник_ХарактеристикиНоменклатури ON Довідник_ХарактеристикиНоменклатури.uid = 
        ТовариНаСкладах_Підсумок.{ВіртуальніТаблиціРегістрів.ТовариНаСкладах_Підсумок_TablePart.ХарактеристикаНоменклатури}

    LEFT JOIN {Склади_Const.TABLE} AS Довідник_Склади ON Довідник_Склади.uid = 
        ТовариНаСкладах_Підсумок.{ВіртуальніТаблиціРегістрів.ТовариНаСкладах_Підсумок_TablePart.Склад}

    LEFT JOIN {СеріїНоменклатури_Const.TABLE} AS Довідник_СеріїНоменклатури ON Довідник_СеріїНоменклатури.uid = 
        ТовариНаСкладах_Підсумок.{ВіртуальніТаблиціРегістрів.ТовариНаСкладах_Підсумок_TablePart.Серія}
";

            #region WHERE

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

            //Відбір по вибраному елементу Серії Номенклатури
            if (!directoryControl_Серія.DirectoryPointerItem.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
Довідник_СеріїНоменклатури.uid = '{directoryControl_Серія.DirectoryPointerItem.UnigueID}'
";
            }

            #endregion

            query += $@"
GROUP BY Номенклатура, Номенклатура_Назва, 
         ХарактеристикаНоменклатури, ХарактеристикаНоменклатури_Назва,
         Склад, Склад_Назва,
         Серія, Серія_Номер

HAVING
    SUM(ТовариНаСкладах_Підсумок.{ВіртуальніТаблиціРегістрів.ТовариНаСкладах_Підсумок_TablePart.ВНаявності}) != 0  

";

            XmlDocument xmlDoc = Функції.CreateXmlDocument();

            Функції.DataHeadToXML(xmlDoc, "head",
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

            Функції.DataToXML(xmlDoc, "ТовариНаСкладах", columnsName, listRow);

            if (!Номенклатура.IsEmpty())
                Функції.XmlDocumentSaveAndTransform(xmlDoc, @"Шаблони\ТовариНаСкладах_ЗалишкиПоНоменклатурі.xslt", false, "Товари на складах");
            else
                Функції.XmlDocumentSaveAndTransform(xmlDoc, @"Шаблони\ТовариНаСкладах_Залишки.xslt", false, "Товари на складах");

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