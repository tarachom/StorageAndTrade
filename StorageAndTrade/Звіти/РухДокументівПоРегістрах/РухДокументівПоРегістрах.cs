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
using AccountingSoftware;

using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Xml.Xsl;
using System.Xml.XPath;

using StorageAndTrade_1_0;
using StorageAndTrade_1_0.Довідники;
using StorageAndTrade_1_0.Документи;
using StorageAndTrade_1_0.РегістриВідомостей;
using StorageAndTrade_1_0.РегістриНакопичення;

namespace StorageAndTrade_1_0.Звіти
{
    /// <summary>
    /// Рух документу по регістрах
    /// </summary>
    class РухДокументівПоРегістрах
    {
        /// <summary>
        /// Добавляє інформацію про документ
        /// </summary>
        /// <param name="xmlDoc">xmlDoc</param>
        /// <param name="ДокументВказівник">Вказівник</param>
        private static void AddCaptionInfo(XmlDocument xmlDoc, DocumentPointer ДокументВказівник)
        {
            string[] columnsName = new string[] { "uid", "Назва", "ДатаДок", "НомерДок" }; ;
            List<object[]> listRow = new List<object[]>();

            switch (ДокументВказівник.TypeDocument)
            {
                case "ЗамовленняПостачальнику":
                    {
                        ЗамовленняПостачальнику_Objest замовленняПостачальнику_Objest = new ЗамовленняПостачальнику_Objest();
                        замовленняПостачальнику_Objest.Read(ДокументВказівник.UnigueID);

                        object[] fieldValue = new object[]
                        {
                                ДокументВказівник.UnigueID.ToString(),
                                замовленняПостачальнику_Objest.Назва,
                                замовленняПостачальнику_Objest.ДатаДок,
                                замовленняПостачальнику_Objest.НомерДок
                        };

                        listRow.Add(fieldValue);

                        break;
                    }

                case "ПоступленняТоварівТаПослуг":
                    {
                        ПоступленняТоварівТаПослуг_Objest поступленняТоварівТаПослуг_Objest = new ПоступленняТоварівТаПослуг_Objest();
                        поступленняТоварівТаПослуг_Objest.Read(ДокументВказівник.UnigueID);

                        object[] fieldValue = new object[]
                        {
                                ДокументВказівник.UnigueID.ToString(),
                                поступленняТоварівТаПослуг_Objest.Назва,
                                поступленняТоварівТаПослуг_Objest.ДатаДок,
                                поступленняТоварівТаПослуг_Objest.НомерДок
                        };

                        listRow.Add(fieldValue);

                        break;
                    }

                case "ЗамовленняКлієнта":
                    {
                        ЗамовленняКлієнта_Objest замовленняКлієнта_Objest = new ЗамовленняКлієнта_Objest();
                        замовленняКлієнта_Objest.Read(ДокументВказівник.UnigueID);

                        object[] fieldValue = new object[]
                        {
                                ДокументВказівник.UnigueID.ToString(),
                                замовленняКлієнта_Objest.Назва,
                                замовленняКлієнта_Objest.ДатаДок,
                                замовленняКлієнта_Objest.НомерДок
                        };

                        listRow.Add(fieldValue);

                        break;
                    }

                case "РеалізаціяТоварівТаПослуг":
                    {
                        РеалізаціяТоварівТаПослуг_Objest реалізаціяТоварівТаПослуг_Objest = new РеалізаціяТоварівТаПослуг_Objest();
                        реалізаціяТоварівТаПослуг_Objest.Read(ДокументВказівник.UnigueID);

                        object[] fieldValue = new object[]
                        {
                                ДокументВказівник.UnigueID.ToString(),
                                реалізаціяТоварівТаПослуг_Objest.Назва,
                                реалізаціяТоварівТаПослуг_Objest.ДатаДок,
                                реалізаціяТоварівТаПослуг_Objest.НомерДок
                        };

                        listRow.Add(fieldValue);

                        break;
                    }

                case "ВстановленняЦінНоменклатури":
                    {
                        ВстановленняЦінНоменклатури_Objest встановленняЦінНоменклатури_Objest = new ВстановленняЦінНоменклатури_Objest();
                        встановленняЦінНоменклатури_Objest.Read(ДокументВказівник.UnigueID);

                        object[] fieldValue = new object[]
                        {
                                ДокументВказівник.UnigueID.ToString(),
                                встановленняЦінНоменклатури_Objest.Назва,
                                встановленняЦінНоменклатури_Objest.ДатаДок,
                                встановленняЦінНоменклатури_Objest.НомерДок
                        };

                        listRow.Add(fieldValue);

                        break;
                    }

                case "ПрихіднийКасовийОрдер":
                    {
                        ПрихіднийКасовийОрдер_Objest прихіднийКасовийОрдер_Objest = new ПрихіднийКасовийОрдер_Objest();
                        прихіднийКасовийОрдер_Objest.Read(ДокументВказівник.UnigueID);

                        object[] fieldValue = new object[]
                        {
                                ДокументВказівник.UnigueID.ToString(),
                                прихіднийКасовийОрдер_Objest.Назва,
                                прихіднийКасовийОрдер_Objest.ДатаДок,
                                прихіднийКасовийОрдер_Objest.НомерДок
                        };

                        listRow.Add(fieldValue);

                        break;
                    }

                case "РозхіднийКасовийОрдер":
                    {
                        РозхіднийКасовийОрдер_Objest розхіднийКасовийОрдер_Objest = new РозхіднийКасовийОрдер_Objest();
                        розхіднийКасовийОрдер_Objest.Read(ДокументВказівник.UnigueID);

                        object[] fieldValue = new object[]
                        {
                                ДокументВказівник.UnigueID.ToString(),
                                розхіднийКасовийОрдер_Objest.Назва,
                                розхіднийКасовийОрдер_Objest.ДатаДок,
                                розхіднийКасовийОрдер_Objest.НомерДок
                        };

                        listRow.Add(fieldValue);

                        break;
                    }

                case "ПереміщенняТоварів":
                    {
                        ПереміщенняТоварів_Objest переміщенняТоварів_Objest = new ПереміщенняТоварів_Objest();
                        переміщенняТоварів_Objest.Read(ДокументВказівник.UnigueID);

                        object[] fieldValue = new object[]
                        {
                                ДокументВказівник.UnigueID.ToString(),
                                переміщенняТоварів_Objest.Назва,
                                переміщенняТоварів_Objest.ДатаДок,
                                переміщенняТоварів_Objest.НомерДок
                        };

                        listRow.Add(fieldValue);

                        break;
                    }

                case "ПоверненняТоварівПостачальнику":
                    {
                        ПоверненняТоварівПостачальнику_Objest поверненняТоварівПостачальнику_Objest = new ПоверненняТоварівПостачальнику_Objest();
                        поверненняТоварівПостачальнику_Objest.Read(ДокументВказівник.UnigueID);

                        object[] fieldValue = new object[]
                        {
                                ДокументВказівник.UnigueID.ToString(),
                                поверненняТоварівПостачальнику_Objest.Назва,
                                поверненняТоварівПостачальнику_Objest.ДатаДок,
                                поверненняТоварівПостачальнику_Objest.НомерДок
                        };

                        listRow.Add(fieldValue);

                        break;
                    }

                case "ПоверненняТоварівВідКлієнта":
                    {
                        ПоверненняТоварівВідКлієнта_Objest поверненняТоварівВідКлієнта_Objest = new ПоверненняТоварівВідКлієнта_Objest();
                        поверненняТоварівВідКлієнта_Objest.Read(ДокументВказівник.UnigueID);

                        object[] fieldValue = new object[]
                        {
                                ДокументВказівник.UnigueID.ToString(),
                                поверненняТоварівВідКлієнта_Objest.Назва,
                                поверненняТоварівВідКлієнта_Objest.ДатаДок,
                                поверненняТоварівВідКлієнта_Objest.НомерДок
                        };

                        listRow.Add(fieldValue);

                        break;
                    }

                case "ВведенняЗалишків":
                    {
                        ВведенняЗалишків_Objest введенняЗалишків_Objest = new ВведенняЗалишків_Objest();
                        введенняЗалишків_Objest.Read(ДокументВказівник.UnigueID);

                        object[] fieldValue = new object[]
                        {
                                ДокументВказівник.UnigueID.ToString(),
                                введенняЗалишків_Objest.Назва,
                                введенняЗалишків_Objest.ДатаДок,
                                введенняЗалишків_Objest.НомерДок
                        };

                        listRow.Add(fieldValue);

                        break;
                    }

                case "АктВиконанихРобіт":
                    {
                        АктВиконанихРобіт_Objest актВиконанихРобіт_Objest = new АктВиконанихРобіт_Objest();
                        актВиконанихРобіт_Objest.Read(ДокументВказівник.UnigueID);

                        object[] fieldValue = new object[]
                        {
                                ДокументВказівник.UnigueID.ToString(),
                                актВиконанихРобіт_Objest.Назва,
                                актВиконанихРобіт_Objest.ДатаДок,
                                актВиконанихРобіт_Objest.НомерДок
                        };

                        listRow.Add(fieldValue);

                        break;
                    }

                case "ВнутрішнєСпоживанняТоварів":
                    {
                        ВнутрішнєСпоживанняТоварів_Objest внутрішнєСпоживанняТоварів_Objest = new ВнутрішнєСпоживанняТоварів_Objest();
                        внутрішнєСпоживанняТоварів_Objest.Read(ДокументВказівник.UnigueID);

                        object[] fieldValue = new object[]
                        {
                                ДокументВказівник.UnigueID.ToString(),
                                внутрішнєСпоживанняТоварів_Objest.Назва,
                                внутрішнєСпоживанняТоварів_Objest.ДатаДок,
                                внутрішнєСпоживанняТоварів_Objest.НомерДок
                        };

                        listRow.Add(fieldValue);

                        break;
                    }

                default:
                    {
                        object[] fieldValue = new object[] { ДокументВказівник.UnigueID.ToString(), "<Не оприділений тип документу>", "", "" };
                        listRow.Add(fieldValue);

                        break;
                    }
            }

            Функції.DataToXML(xmlDoc, "Заголовок", columnsName, listRow);
        }

        /// <summary>
        /// Функція формує звіт рухів документу по регістрах
        /// </summary>
        /// <param name="ДокументВказівник">Документ для якого формується звіт</param>
        public static void PrintRecords(DocumentPointer ДокументВказівник)
        {
            XmlDocument xmlDoc = Функції.CreateXmlDocument();

            //Заголовок
            AddCaptionInfo(xmlDoc, ДокументВказівник);

            //Список регістрів доступних для документу
            List<string> allowRegisterAccumulation = Config.Kernel.Conf.Documents[ДокументВказівник.TypeDocument].AllowRegisterAccumulation;

            //Словник [ Назва групи, Функція]
            Dictionary<string, Func<string>> funcQuery = new Dictionary<string, Func<string>>();

            //Регістри накопичення
            funcQuery.Add("ТовариНаСкладах", Запит_ТовариНаСкладах);
            funcQuery.Add("ПартіїТоварів", Запит_ПартіїТоварів); 
            funcQuery.Add("РухТоварів", Запит_РухТоварів);
            funcQuery.Add("ЗамовленняКлієнтів", Запит_ЗамовленняКлієнтів);
            funcQuery.Add("РозрахункиЗКлієнтами", Запит_РозрахункиЗКлієнтами);
            funcQuery.Add("ВільніЗалишки", Запит_ВільніЗалишки);
            funcQuery.Add("ЗамовленняПостачальникам", Запит_ЗамовленняПостачальникам);
            funcQuery.Add("РозрахункиЗПостачальниками", Запит_РозрахункиЗПостачальниками);
            funcQuery.Add("ТовариДоПоступлення", Запит_ТовариДоПоступлення);
            funcQuery.Add("РухКоштів", Запит_РухКоштів);

            //Регістри інформації
            funcQuery.Add("ЦіниНоменклатури", Запит_ЦіниНоменклатури);

            Dictionary<string, object> paramQuery = new Dictionary<string, object>();
            paramQuery.Add("ДокументВказівник", ДокументВказівник.UnigueID.UGuid);

            string[] columnsName;
            List<object[]> listRow;

            foreach (KeyValuePair<string, Func<string>> func in funcQuery)
            {
                if (allowRegisterAccumulation.Contains(func.Key))
                {
                    //Console.WriteLine(func.Key);

                    string query = func.Value.Invoke();

                    Config.Kernel.DataBase.SelectRequest(query, paramQuery, out columnsName, out listRow);

                    if (listRow.Count > 0)
                        Функції.DataToXML(xmlDoc, func.Key, columnsName, listRow);
                }
            }

            Функції.XmlDocumentSaveAndTransform(xmlDoc, @"Шаблони\РухДокументівПоРегістрах.xslt", true, "Рух документу по регістрах");
        }

        #region Запити по регістрах інформації

        private static string Запит_ЦіниНоменклатури()
        {
            string query = $@"
SELECT 
    Рег_ЦіниНоменклатури.period,
    Рег_ЦіниНоменклатури.{ЦіниНоменклатури_Const.Номенклатура} AS Номенклатура, 
    Довідник_Номенклатура.{Номенклатура_Const.Назва} AS Номенклатура_Назва, 
    Рег_ЦіниНоменклатури.{ЦіниНоменклатури_Const.ХарактеристикаНоменклатури} AS ХарактеристикаНоменклатури,
    Довідник_ХарактеристикиНоменклатури.{ХарактеристикиНоменклатури_Const.Назва} AS ХарактеристикаНоменклатури_Назва, 
    Рег_ЦіниНоменклатури.{ЦіниНоменклатури_Const.ВидЦіни} AS ВидЦіни,
    Довідник_ВидиЦін.{ВидиЦін_Const.Назва} AS ВидЦіни_Назва,
    Рег_ЦіниНоменклатури.{ЦіниНоменклатури_Const.Ціна} AS Ціна,
    Рег_ЦіниНоменклатури.{ЦіниНоменклатури_Const.Пакування} AS Пакування,
    Довідник_ПакуванняОдиниціВиміру.{ПакуванняОдиниціВиміру_Const.Назва} AS Пакування_Назва,
    Рег_ЦіниНоменклатури.{ЦіниНоменклатури_Const.Валюта} AS Валюта,
    Довідник_Валюти.{Валюти_Const.Назва} AS Валюта_Назва
FROM 
    {ЦіниНоменклатури_Const.TABLE} AS Рег_ЦіниНоменклатури

    LEFT JOIN {Номенклатура_Const.TABLE} AS Довідник_Номенклатура ON Довідник_Номенклатура.uid = 
        Рег_ЦіниНоменклатури.{ЦіниНоменклатури_Const.Номенклатура}

    LEFT JOIN {ХарактеристикиНоменклатури_Const.TABLE} AS Довідник_ХарактеристикиНоменклатури ON Довідник_ХарактеристикиНоменклатури.uid = 
        Рег_ЦіниНоменклатури.{ЦіниНоменклатури_Const.ХарактеристикаНоменклатури}

    LEFT JOIN {ВидиЦін_Const.TABLE} AS Довідник_ВидиЦін ON Довідник_ВидиЦін.uid = 
       Рег_ЦіниНоменклатури.{ЦіниНоменклатури_Const.ВидЦіни}

    LEFT JOIN {ПакуванняОдиниціВиміру_Const.TABLE} AS Довідник_ПакуванняОдиниціВиміру ON Довідник_ПакуванняОдиниціВиміру.uid = 
       Рег_ЦіниНоменклатури.{ЦіниНоменклатури_Const.Пакування}
    
    LEFT JOIN {Валюти_Const.TABLE} AS Довідник_Валюти ON Довідник_Валюти.uid = 
       Рег_ЦіниНоменклатури.{ЦіниНоменклатури_Const.Валюта}
WHERE
    Рег_ЦіниНоменклатури.Owner = @ДокументВказівник
ORDER BY Номенклатура_Назва
";

            return query;
        }

        #endregion

        #region Запити по регістрах накопичення

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
    Рег_ТовариНаСкладах.{ТовариНаСкладах_Const.Серія} AS Серія,
    Довідник_СеріїНоменклатури.{СеріїНоменклатури_Const.Номер} AS Серія_Номер,
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

    LEFT JOIN {СеріїНоменклатури_Const.TABLE} AS Довідник_СеріїНоменклатури ON Довідник_СеріїНоменклатури.uid = 
       Рег_ТовариНаСкладах.{ТовариНаСкладах_Const.Серія}
WHERE
    Рег_ТовариНаСкладах.Owner = @ДокументВказівник
ORDER BY Номенклатура_Назва
";

            return query;
        }

        private static string Запит_ПартіїТоварів()
        {
            string query = $@"
SELECT 
    Рег_ПартіїТоварів.period,
    Рег_ПартіїТоварів.income, 
    Рег_ПартіїТоварів.{ПартіїТоварів_Const.Організація} AS Організація, 
    Довідник_Організації.{Організації_Const.Назва} AS Організація_Назва,
    Рег_ПартіїТоварів.{ПартіїТоварів_Const.ДокументПоступлення} AS ДокументПоступлення, 
    Документ_ПоступленняТоварівТаПослуг.{ПоступленняТоварівТаПослуг_Const.Назва} AS ДокументПоступлення_Назва,
    Рег_ПартіїТоварів.{ПартіїТоварів_Const.Номенклатура} AS Номенклатура, 
    Довідник_Номенклатура.{Номенклатура_Const.Назва} AS Номенклатура_Назва, 
    Рег_ПартіїТоварів.{ПартіїТоварів_Const.ХарактеристикаНоменклатури} AS ХарактеристикаНоменклатури,
    Довідник_ХарактеристикиНоменклатури.{ХарактеристикиНоменклатури_Const.Назва} AS ХарактеристикаНоменклатури_Назва, 
    Рег_ПартіїТоварів.{ПартіїТоварів_Const.Серія} AS Серія,
    Довідник_СеріїНоменклатури.{СеріїНоменклатури_Const.Номер} AS Серія_Номер,
    Рег_ПартіїТоварів.{ПартіїТоварів_Const.Документ} AS Документ,
    Рег_ПартіїТоварів.{ПартіїТоварів_Const.Кількість} AS Кількість,
    Рег_ПартіїТоварів.{ПартіїТоварів_Const.Собівартість} AS Собівартість,
    Рег_ПартіїТоварів.{ПартіїТоварів_Const.СписанаСобівартість} AS СписанаСобівартість
FROM 
    {ПартіїТоварів_Const.TABLE} AS Рег_ПартіїТоварів

    LEFT JOIN {Організації_Const.TABLE} AS Довідник_Організації ON Довідник_Організації.uid = 
        Рег_ПартіїТоварів.{ПартіїТоварів_Const.Організація}

    LEFT JOIN {ПоступленняТоварівТаПослуг_Const.TABLE} AS Документ_ПоступленняТоварівТаПослуг ON Документ_ПоступленняТоварівТаПослуг.uid = 
        Рег_ПартіїТоварів.{ПартіїТоварів_Const.ДокументПоступлення}

    LEFT JOIN {Номенклатура_Const.TABLE} AS Довідник_Номенклатура ON Довідник_Номенклатура.uid = 
        Рег_ПартіїТоварів.{ПартіїТоварів_Const.Номенклатура}

    LEFT JOIN {ХарактеристикиНоменклатури_Const.TABLE} AS Довідник_ХарактеристикиНоменклатури ON Довідник_ХарактеристикиНоменклатури.uid = 
        Рег_ПартіїТоварів.{ПартіїТоварів_Const.ХарактеристикаНоменклатури}

    LEFT JOIN {СеріїНоменклатури_Const.TABLE} AS Довідник_СеріїНоменклатури ON Довідник_СеріїНоменклатури.uid = 
       Рег_ПартіїТоварів.{ПартіїТоварів_Const.Серія}
WHERE
    Рег_ПартіїТоварів.Owner = @ДокументВказівник
ORDER BY Організація_Назва, ДокументПоступлення_Назва
";

            return query;
        }

        private static string Запит_РухТоварів()
        {
            string query = $@"
SELECT 
    Рег_РухТоварів.period,
    Рег_РухТоварів.income, 
    Рег_РухТоварів.{РухТоварів_Const.Номенклатура} AS Номенклатура, 
    Довідник_Номенклатура.{Номенклатура_Const.Назва} AS Номенклатура_Назва, 
    Рег_РухТоварів.{РухТоварів_Const.ХарактеристикаНоменклатури} AS ХарактеристикаНоменклатури,
    Довідник_ХарактеристикиНоменклатури.{ХарактеристикиНоменклатури_Const.Назва} AS ХарактеристикаНоменклатури_Назва, 
    Рег_РухТоварів.{РухТоварів_Const.Склад} AS Склад,
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
    Рег_ЗамовленняКлієнтів.Owner = @ДокументВказівник
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
    Рег_РозрахункиЗКлієнтами.{РозрахункиЗКлієнтами_Const.Контрагент} AS Контрагент, 
    Довідник_Контрагенти.{Контрагенти_Const.Назва} AS Контрагент_Назва,
    Рег_РозрахункиЗКлієнтами.{РозрахункиЗКлієнтами_Const.Валюта} AS Валюта,
    Довідник_Валюти.{Валюти_Const.Назва} AS Валюта_Назва,
    Рег_РозрахункиЗКлієнтами.{РозрахункиЗКлієнтами_Const.Сума} AS Сума
FROM 
    {РозрахункиЗКлієнтами_Const.TABLE} AS Рег_РозрахункиЗКлієнтами

    LEFT JOIN {Контрагенти_Const.TABLE} AS Довідник_Контрагенти ON Довідник_Контрагенти.uid = 
        Рег_РозрахункиЗКлієнтами.{РозрахункиЗКлієнтами_Const.Контрагент}

    LEFT JOIN {Валюти_Const.TABLE} AS Довідник_Валюти ON Довідник_Валюти.uid = 
       Рег_РозрахункиЗКлієнтами.{РозрахункиЗКлієнтами_Const.Валюта}
WHERE
    Рег_РозрахункиЗКлієнтами.Owner = @ДокументВказівник
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
    Рег_ЗамовленняПостачальникам.{ЗамовленняПостачальникам_Const.Номенклатура} AS Номенклатура, 
    Довідник_Номенклатура.{Номенклатура_Const.Назва} AS Номенклатура_Назва, 
    Рег_ЗамовленняПостачальникам.{ЗамовленняПостачальникам_Const.ХарактеристикаНоменклатури} AS ХарактеристикаНоменклатури,
    Довідник_ХарактеристикиНоменклатури.{ХарактеристикиНоменклатури_Const.Назва} AS ХарактеристикаНоменклатури_Назва, 
    Рег_ЗамовленняПостачальникам.{ЗамовленняПостачальникам_Const.Склад} AS Склад,
    Довідник_Склади.{Склади_Const.Назва} AS Склад_Назва,
    Рег_ЗамовленняПостачальникам.{ЗамовленняПостачальникам_Const.Замовлено} AS Замовлено
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
    Рег_РозрахункиЗПостачальниками.{РозрахункиЗПостачальниками_Const.Контрагент} AS Контрагент, 
    Довідник_Контрагенти.{Контрагенти_Const.Назва} AS Контрагент_Назва,
    Рег_РозрахункиЗПостачальниками.{РозрахункиЗПостачальниками_Const.Валюта} AS Валюта,
    Довідник_Валюти.{Валюти_Const.Назва} AS Валюта_Назва,
    Рег_РозрахункиЗПостачальниками.{РозрахункиЗПостачальниками_Const.Сума} AS Сума
FROM 
    {РозрахункиЗПостачальниками_Const.TABLE} AS Рег_РозрахункиЗПостачальниками

    LEFT JOIN {Контрагенти_Const.TABLE} AS Довідник_Контрагенти ON Довідник_Контрагенти.uid = 
        Рег_РозрахункиЗПостачальниками.{РозрахункиЗПостачальниками_Const.Контрагент}

    LEFT JOIN {Валюти_Const.TABLE} AS Довідник_Валюти ON Довідник_Валюти.uid = 
       Рег_РозрахункиЗПостачальниками.{РозрахункиЗПостачальниками_Const.Валюта}
WHERE
    Рег_РозрахункиЗПостачальниками.Owner = @ДокументВказівник
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

        private static string Запит_РухКоштів()
        {
            string query = $@"
SELECT 
    Рег_РухКоштів.period,
    Рег_РухКоштів.income, 
    Рег_РухКоштів.{РухКоштів_Const.Організація} AS Організація, 
    Довідник_Організації.{Організації_Const.Назва} AS Організація_Назва, 
    Рег_РухКоштів.{РухКоштів_Const.Каса} AS Каса,
    Довідник_Каси.{Каси_Const.Назва} AS Каса_Назва, 
    Рег_РухКоштів.{РухКоштів_Const.Валюта} AS Валюта,
    Довідник_Валюти.{Валюти_Const.Назва} AS Валюта_Назва,
    Рег_РухКоштів.{РухКоштів_Const.Сума} AS Сума
FROM 
    {РухКоштів_Const.TABLE} AS Рег_РухКоштів

    LEFT JOIN {Організації_Const.TABLE} AS Довідник_Організації ON Довідник_Організації.uid = 
        Рег_РухКоштів.{РухКоштів_Const.Організація}

    LEFT JOIN {Каси_Const.TABLE} AS Довідник_Каси ON Довідник_Каси.uid = 
        Рег_РухКоштів.{РухКоштів_Const.Каса}

    LEFT JOIN {Валюти_Const.TABLE} AS Довідник_Валюти ON Довідник_Валюти.uid = 
       Рег_РухКоштів.{РухКоштів_Const.Валюта}
WHERE
    Рег_РухКоштів.Owner = @ДокументВказівник
ORDER BY Організація_Назва
";

            return query;
        }

        #endregion

    }
}

