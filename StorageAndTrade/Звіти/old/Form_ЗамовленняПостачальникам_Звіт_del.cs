﻿/*
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

using StorageAndTrade_1_0;
using StorageAndTrade_1_0.Довідники;
using StorageAndTrade_1_0.Документи;
using StorageAndTrade_1_0.РегістриНакопичення;
using StorageAndTrade_1_0.Звіти;

namespace StorageAndTrade
{
    public partial class Form_ЗамовленняПостачальникам_Звіт_del : Form
    {
        public Form_ЗамовленняПостачальникам_Звіт_del()
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

            documentControl_ЗамовленняПостачальнику.Init(new Form_ЗамовленняПостачальникуЖурнал(), new ЗамовленняПостачальнику_Pointer());
        }

        private void buttonCreate_Click(object sender, EventArgs e)
        {
            bool isExistParent = false;

            string query = $@"
SELECT 
    Рег_ЗамовленняПостачальникам.{ЗамовленняПостачальникам_Const.Номенклатура} AS Номенклатура, 
    Довідник_Номенклатура.{Номенклатура_Const.Назва} AS Номенклатура_Назва, 
    Рег_ЗамовленняПостачальникам.{ЗамовленняПостачальникам_Const.ХарактеристикаНоменклатури} AS ХарактеристикаНоменклатури,
    Довідник_ХарактеристикиНоменклатури.{ХарактеристикиНоменклатури_Const.Назва} AS ХарактеристикаНоменклатури_Назва,
    Рег_ЗамовленняПостачальникам.{ЗамовленняПостачальникам_Const.Склад} AS Склад,
    Довідник_Склади.{Склади_Const.Назва} AS Склад_Назва, 

    SUM(CASE WHEN Рег_ЗамовленняПостачальникам.income = true THEN 
        Рег_ЗамовленняПостачальникам.{ЗамовленняПостачальникам_Const.Замовлено} ELSE 
       -Рег_ЗамовленняПостачальникам.{ЗамовленняПостачальникам_Const.Замовлено} END) AS Замовлено
FROM 
    {ЗамовленняПостачальникам_Const.TABLE} AS Рег_ЗамовленняПостачальникам

    LEFT JOIN {Номенклатура_Const.TABLE} AS Довідник_Номенклатура ON Довідник_Номенклатура.uid = 
        Рег_ЗамовленняПостачальникам.{ЗамовленняПостачальникам_Const.Номенклатура}

    LEFT JOIN {ХарактеристикиНоменклатури_Const.TABLE} AS Довідник_ХарактеристикиНоменклатури ON Довідник_ХарактеристикиНоменклатури.uid = 
        Рег_ЗамовленняПостачальникам.{ЗамовленняПостачальникам_Const.ХарактеристикаНоменклатури}

    LEFT JOIN {Склади_Const.TABLE} AS Довідник_Склади ON Довідник_Склади.uid = 
        Рег_ЗамовленняПостачальникам.{ЗамовленняПостачальникам_Const.Склад}
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

            //Відбір по документу ЗамовленняКлієнта
            if (!documentControl_ЗамовленняПостачальнику.DocumentPointerItem.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
Рег_ЗамовленняПостачальникам.{ЗамовленняПостачальникам_Const.ЗамовленняПостачальнику} = '{documentControl_ЗамовленняПостачальнику.DocumentPointerItem.UnigueID}'
";
            }

            #endregion

            query += $@"
GROUP BY Номенклатура, Номенклатура_Назва, 
         ХарактеристикаНоменклатури, ХарактеристикаНоменклатури_Назва,
         Склад, Склад_Назва

HAVING
     SUM(CASE WHEN Рег_ЗамовленняПостачальникам.income = true THEN 
        Рег_ЗамовленняПостачальникам.{ЗамовленняПостачальникам_Const.Замовлено} ELSE 
       -Рег_ЗамовленняПостачальникам.{ЗамовленняПостачальникам_Const.Замовлено} END) != 0 

ORDER BY Номенклатура_Назва
";

            //Console.WriteLine(query);
            
            XmlDocument xmlDoc =  Функції.CreateXmlDocument();

            Dictionary<string, object> paramQuery = new Dictionary<string, object>();

            string[] columnsName;
            List<object[]> listRow;

            Config.Kernel.DataBase.SelectRequest(query, paramQuery, out columnsName, out listRow);

            Функції.DataToXML(xmlDoc, "ЗамовленняПостачальникам", columnsName, listRow);

            Функції.XmlDocumentSaveAndTransform(xmlDoc, @"Шаблони\ЗамовленняПостачальникам.xslt", true, "Замовлення постачальникам");
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
