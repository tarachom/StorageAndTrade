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

using StorageAndTrade_1_0;
using StorageAndTrade_1_0.Константи;
using StorageAndTrade_1_0.Довідники;
using StorageAndTrade_1_0.Документи;
using StorageAndTrade_1_0.РегістриНакопичення;


namespace StorageAndTrade
{
    public partial class Form_ВідомістьПоТоварахНаСкладах_Звіт : Form
    {
        public Form_ВідомістьПоТоварахНаСкладах_Звіт()
        {
            InitializeComponent();
        }

        private void Form_РозрахункиЗПостачальниками_Звіт_Load(object sender, EventArgs e)
        {
            //directoryControl_КонтрагентиПапка.Init(new Form_КонтрагентиПапкиВибір(), new Контрагенти_Папки_Pointer());
            directoryControl_Номенклатура.Init(new Form_Номенклатура(), new Номенклатура_Pointer());
            //directoryControl_Валюти.Init(new Form_Валюти(), new Валюти_Pointer());
        }

        private void buttonCreate_Click(object sender, EventArgs e)
        {
            bool isExistParent = false;

            string query = $@"
SELECT
    Склад,
    Довідник_Склади.{Склади_Const.Назва} AS Склад_Назва,
	Номенклатура,
    Довідник_Номенклатура.{Номенклатура_Const.Назва} AS Номенклатура_Назва, 
	ХарактеристикаНоменклатури,
    Довідник_ХарактеристикиНоменклатури.{ХарактеристикиНоменклатури_Const.Назва} AS ХарактеристикаНоменклатури_Назва,
	SUM(ПочатковийЗалишок) AS ПочатковийЗалишок,
	SUM(Прихід) AS Прихід,
	SUM(Розхід) AS Розхід,
	SUM(ПочатковийЗалишок + КінцевийЗалишок) AS КінцевийЗалишок
FROM
(
    SELECT
        'НаПочатокМісяця',
        ТовариНаСкладах_Місяць.{ВіртуальніТаблиціРегістрів.ТовариНаСкладах_Місяць_TablePart.Склад} AS Склад,
        ТовариНаСкладах_Місяць.{ВіртуальніТаблиціРегістрів.ТовариНаСкладах_Місяць_TablePart.Номенклатура} AS Номенклатура, 
        ТовариНаСкладах_Місяць.{ВіртуальніТаблиціРегістрів.ТовариНаСкладах_Місяць_TablePart.ХарактеристикаНоменклатури} AS ХарактеристикаНоменклатури,
        ТовариНаСкладах_Місяць.{ВіртуальніТаблиціРегістрів.ТовариНаСкладах_Місяць_TablePart.ВНаявності} AS ПочатковийЗалишок,
        0 AS Прихід,    
        0 AS Розхід,
        0 AS КінцевийЗалишок
    FROM
        {ВіртуальніТаблиціРегістрів.ТовариНаСкладах_Місяць_TablePart.TABLE} AS ТовариНаСкладах_Місяць
    WHERE
        ТовариНаСкладах_Місяць.{ВіртуальніТаблиціРегістрів.ТовариНаСкладах_Місяць_TablePart.Період} < @date_start_month

UNION

    SELECT
        'ЗПочаткуМісяцяПоДатуСтарт',
        Рег_ТовариНаСкладах.{ТовариНаСкладах_Const.Склад} AS Склад,
        Рег_ТовариНаСкладах.{ТовариНаСкладах_Const.Номенклатура} AS Номенклатура, 
        Рег_ТовариНаСкладах.{ТовариНаСкладах_Const.ХарактеристикаНоменклатури} AS ХарактеристикаНоменклатури,
        CASE WHEN Рег_ТовариНаСкладах.income = true THEN 
            Рег_ТовариНаСкладах.{ТовариНаСкладах_Const.ВНаявності} ELSE 
           -Рег_ТовариНаСкладах.{ТовариНаСкладах_Const.ВНаявності} END AS ПочатковийЗалишок,
        0 AS Прихід,
        0 AS Розхід,
        0 AS КінцевийЗалишок
    FROM 
        {ТовариНаСкладах_Const.TABLE} AS Рег_ТовариНаСкладах
    WHERE 
        Рег_ТовариНаСкладах.period >= @date_start2 AND Рег_ТовариНаСкладах.period < @date_start

UNION

    SELECT
        'ЗДатиСтартПоДатуСтоп',
        Рег_ТовариНаСкладах.{ТовариНаСкладах_Const.Склад} AS Склад,
        Рег_ТовариНаСкладах.{ТовариНаСкладах_Const.Номенклатура} AS Номенклатура, 
        Рег_ТовариНаСкладах.{ТовариНаСкладах_Const.ХарактеристикаНоменклатури} AS ХарактеристикаНоменклатури,
        0 AS ПочатковийЗалишок,    
        CASE WHEN Рег_ТовариНаСкладах.income = true THEN 
            Рег_ТовариНаСкладах.{ТовариНаСкладах_Const.ВНаявності} END AS Прихід,    
        CASE WHEN Рег_ТовариНаСкладах.income = false THEN 
            Рег_ТовариНаСкладах.{ТовариНаСкладах_Const.ВНаявності} END AS Розхід,
        CASE WHEN Рег_ТовариНаСкладах.income = true THEN 
            Рег_ТовариНаСкладах.{ТовариНаСкладах_Const.ВНаявності} ELSE 
           -Рег_ТовариНаСкладах.{ТовариНаСкладах_Const.ВНаявності} END AS КінцевийЗалишок
    FROM 
        {ТовариНаСкладах_Const.TABLE} AS Рег_ТовариНаСкладах
    WHERE 
        Рег_ТовариНаСкладах.period >= @date_start AND Рег_ТовариНаСкладах.period <= @date_stop
) AS ВсіЗалишки

    LEFT JOIN {Склади_Const.TABLE} AS Довідник_Склади 
       ON Довідник_Склади.uid = ВсіЗалишки.Склад
    LEFT JOIN {Номенклатура_Const.TABLE} AS Довідник_Номенклатура 
       ON Довідник_Номенклатура.uid = ВсіЗалишки.Номенклатура
    LEFT JOIN {ХарактеристикиНоменклатури_Const.TABLE} AS Довідник_ХарактеристикиНоменклатури 
       ON Довідник_ХарактеристикиНоменклатури.uid = ВсіЗалишки.ХарактеристикаНоменклатури

";

            //Відбір по вибраному елементу Номенклатура
            if (!directoryControl_Номенклатура.DirectoryPointerItem.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
Довідник_Номенклатура.uid = '{directoryControl_Номенклатура.DirectoryPointerItem.UnigueID}'
";
            }

            query += $@"
GROUP BY Склад, Склад_Назва,
         Номенклатура, Номенклатура_Назва, 
         ХарактеристикаНоменклатури, ХарактеристикаНоменклатури_Назва
         
ORDER BY Номенклатура_Назва
";

            //Console.WriteLine(query);

            XmlDocument xmlDoc =  ФункціїДляЗвітів.CreateXmlDocument();

            DateTime dateStart = new DateTime(dateTimeStart.Value.Year, dateTimeStart.Value.Month, dateTimeStart.Value.Day, 0, 0, 0);
            //Console.WriteLine("dateStart " + dateStart);

            DateTime dateStop = new DateTime(dateTimeStop.Value.Year, dateTimeStop.Value.Month, dateTimeStop.Value.Day, 23, 59, 59);
            //Console.WriteLine("dateStop " + dateStop);

            DateTime dateStartMonth = new DateTime(dateTimeStart.Value.Year, dateTimeStart.Value.Month, 1, 0, 0, 0);
            //Console.WriteLine("dateStartMonth " + dateStartMonth);

            DateTime dateStart2 = new DateTime(dateTimeStart.Value.Year, dateTimeStart.Value.Month, 1, 0, 0, 0);
            //Console.WriteLine("dateStart2 " + dateStart2);

            //DateTime dateStop2 = dateStop.AddDays(-1);

            Dictionary<string, object> paramQuery = new Dictionary<string, object>();

            paramQuery.Add("date_start_month", dateStartMonth);
            paramQuery.Add("date_start", dateStart);
            paramQuery.Add("date_stop", dateStop);
            paramQuery.Add("date_start2", dateStart2);
            //paramQuery.Add("date_stop2", dateStop2);

            string[] columnsName;
            List<object[]> listRow;

            Config.Kernel.DataBase.SelectRequest(query, paramQuery, out columnsName, out listRow);

            ФункціїДляЗвітів.DataToXML(xmlDoc, "ВідомістьПоТоварахНаСкладах", columnsName, listRow);

            ФункціїДляЗвітів.XmlDocumentSaveAndTransform(xmlDoc, @"Шаблони\ВідомістьПоТоварахНаСкладах.xslt", true, "Відомість по товарах на складах");
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
