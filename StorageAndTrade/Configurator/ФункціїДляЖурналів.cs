﻿/*
Copyright (C) 2019-2022 TARAKHOMYN YURIY IVANOVYCH
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

Функції для журналів

*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccountingSoftware;
using Довідники = StorageAndTrade_1_0.Довідники;
using Документи = StorageAndTrade_1_0.Документи;
using Перелічення = StorageAndTrade_1_0.Перелічення;

namespace StorageAndTrade
{
    /// <summary>
    /// Структура для обмеження кількості завантажених даних для табличних частин і автоматичної підгрузки при прокрутці
    /// </summary>
    public struct LoadRecordsLimit
    {
        /// <summary>
        /// Індекс сторінки
        /// </summary>
        public int PageIndex;

        /// <summary>
        /// Обмеження для запиту
        /// </summary>
        public int Limit;

        /// <summary>
        /// Кількість даних завантажених останній раз
        /// </summary>
        public int LastCountRow;
    }

    /// <summary>
    /// Спільні функції для журналів
    /// </summary>
    class ФункціїДляЖурналів
    {
        public static DateTime ОтриматиДатуПочаткуПеріоду(Перелічення.ТипПеріодуДляЖурналівДокументів ПеріодЖурналу)
        {
            DateTime whereDateTime = DateTime.MinValue;

            switch (ПеріодЖурналу)
            {
                case Перелічення.ТипПеріодуДляЖурналівДокументів.ЗПочаткуРоку:
                    {
                        whereDateTime = new DateTime(DateTime.Now.Year, 1, 1);
                        break;
                    }
                case Перелічення.ТипПеріодуДляЖурналівДокументів.Квартал:
                    {
                        DateTime ДатаТриМісцяНазад = DateTime.Now.AddMonths(-3);
                        whereDateTime = new DateTime(ДатаТриМісцяНазад.Year, ДатаТриМісцяНазад.Month, 1);
                        break;
                    }
                case Перелічення.ТипПеріодуДляЖурналівДокументів.ЗМинулогоМісяця:
                    {
                        DateTime ДатаМісцьНазад = DateTime.Now.AddMonths(-1);
                        whereDateTime = new DateTime(ДатаМісцьНазад.Year, ДатаМісцьНазад.Month, 1);
                        break;
                    }
                case Перелічення.ТипПеріодуДляЖурналівДокументів.ЗПочаткуМісяця:
                    {
                        whereDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                        break;
                    }
                case Перелічення.ТипПеріодуДляЖурналівДокументів.ЗПочаткуТижня:
                    {
                        DateTime СімДнівНазад = DateTime.Now.AddDays(-7);
                        whereDateTime = new DateTime(СімДнівНазад.Year, СімДнівНазад.Month, СімДнівНазад.Day);
                        break;
                    }
                case Перелічення.ТипПеріодуДляЖурналівДокументів.ПоточнийДень:
                    {
                        whereDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                        break;
                    }
            }

            return whereDateTime;
        }

        public static string ЗапитВибіркаПродажі()
        {
            string query = $@"
SELECT
    'ЗамовленняКлієнта' AS ТипДокументу,
    Док_ЗамовленняКлієнта.uid,
    Док_ЗамовленняКлієнта.spend,
    Док_ЗамовленняКлієнта.{Документи.ЗамовленняКлієнта_Const.Назва} AS Назва,
    Док_ЗамовленняКлієнта.{Документи.ЗамовленняКлієнта_Const.НомерДок} AS НомерДок,
    Док_ЗамовленняКлієнта.{Документи.ЗамовленняКлієнта_Const.ДатаДок} AS ДатаДок,
    Довідник_Контрагенти.{Довідники.Контрагенти_Const.Назва} AS КонтрагентНазва,
    Док_ЗамовленняКлієнта.{Документи.ЗамовленняКлієнта_Const.СумаДокументу} AS Сума,
    Док_ЗамовленняКлієнта.{Документи.ЗамовленняКлієнта_Const.Коментар} AS Коментар
FROM
	{Документи.ЗамовленняКлієнта_Const.TABLE} AS Док_ЗамовленняКлієнта

    LEFT JOIN {Довідники.Контрагенти_Const.TABLE} AS Довідник_Контрагенти ON Довідник_Контрагенти.uid = 
        Док_ЗамовленняКлієнта.{Документи.ЗамовленняКлієнта_Const.Контрагент}

WHERE Док_ЗамовленняКлієнта.docdate >= @docdate

UNION

SELECT
    'РахунокФактура' AS ТипДокументу,
    Док_РахунокФактура.uid,
    Док_РахунокФактура.spend,
    Док_РахунокФактура.{Документи.РахунокФактура_Const.Назва} AS Назва,
    Док_РахунокФактура.{Документи.РахунокФактура_Const.НомерДок} AS НомерДок,
    Док_РахунокФактура.{Документи.РахунокФактура_Const.ДатаДок} AS ДатаДок,
    Довідник_Контрагенти.{Довідники.Контрагенти_Const.Назва} AS КонтрагентНазва,
    Док_РахунокФактура.{Документи.РахунокФактура_Const.СумаДокументу} AS Сума,
    Док_РахунокФактура.{Документи.РахунокФактура_Const.Коментар} AS Коментар
FROM
	{Документи.РахунокФактура_Const.TABLE} AS Док_РахунокФактура

    LEFT JOIN {Довідники.Контрагенти_Const.TABLE} AS Довідник_Контрагенти ON Довідник_Контрагенти.uid = 
        Док_РахунокФактура.{Документи.РахунокФактура_Const.Контрагент}

WHERE Док_РахунокФактура.docdate >= @docdate

UNION

SELECT
    'РеалізаціяТоварівТаПослуг' AS ТипДокументу,
    Док_РеалізаціяТоварівТаПослуг.uid,
    Док_РеалізаціяТоварівТаПослуг.spend,
    Док_РеалізаціяТоварівТаПослуг.{Документи.РеалізаціяТоварівТаПослуг_Const.Назва} AS Назва,
    Док_РеалізаціяТоварівТаПослуг.{Документи.РеалізаціяТоварівТаПослуг_Const.НомерДок} AS НомерДок,
    Док_РеалізаціяТоварівТаПослуг.{Документи.РеалізаціяТоварівТаПослуг_Const.ДатаДок} AS ДатаДок,
    Довідник_Контрагенти.{Довідники.Контрагенти_Const.Назва} AS КонтрагентНазва,
    Док_РеалізаціяТоварівТаПослуг.{Документи.РеалізаціяТоварівТаПослуг_Const.СумаДокументу} AS Сума,
    Док_РеалізаціяТоварівТаПослуг.{Документи.РеалізаціяТоварівТаПослуг_Const.Коментар} AS Коментар
FROM
	{Документи.РеалізаціяТоварівТаПослуг_Const.TABLE} AS Док_РеалізаціяТоварівТаПослуг

    LEFT JOIN {Довідники.Контрагенти_Const.TABLE} AS Довідник_Контрагенти ON Довідник_Контрагенти.uid = 
        Док_РеалізаціяТоварівТаПослуг.{Документи.РеалізаціяТоварівТаПослуг_Const.Контрагент}

WHERE Док_РеалізаціяТоварівТаПослуг.docdate >= @docdate

UNION

SELECT
    'АктВиконанихРобіт' AS ТипДокументу,
    Док_АктВиконанихРобіт.uid,
    Док_АктВиконанихРобіт.spend,
    Док_АктВиконанихРобіт.{Документи.АктВиконанихРобіт_Const.Назва} AS Назва,
    Док_АктВиконанихРобіт.{Документи.АктВиконанихРобіт_Const.НомерДок} AS НомерДок,
    Док_АктВиконанихРобіт.{Документи.АктВиконанихРобіт_Const.ДатаДок} AS ДатаДок,
    Довідник_Контрагенти.{Довідники.Контрагенти_Const.Назва} AS КонтрагентНазва,
    Док_АктВиконанихРобіт.{Документи.АктВиконанихРобіт_Const.СумаДокументу} AS Сума,
    Док_АктВиконанихРобіт.{Документи.АктВиконанихРобіт_Const.Коментар} AS Коментар
FROM
	{Документи.АктВиконанихРобіт_Const.TABLE} AS Док_АктВиконанихРобіт

    LEFT JOIN {Довідники.Контрагенти_Const.TABLE} AS Довідник_Контрагенти ON Довідник_Контрагенти.uid = 
        Док_АктВиконанихРобіт.{Документи.АктВиконанихРобіт_Const.Контрагент}

WHERE Док_АктВиконанихРобіт.docdate >= @docdate

UNION

SELECT
    'ПоверненняТоварівВідКлієнта' AS ТипДокументу,
    Док_ПоверненняТоварівВідКлієнта.uid,
    Док_ПоверненняТоварівВідКлієнта.spend,
    Док_ПоверненняТоварівВідКлієнта.{Документи.ПоверненняТоварівВідКлієнта_Const.Назва} AS Назва,
    Док_ПоверненняТоварівВідКлієнта.{Документи.ПоверненняТоварівВідКлієнта_Const.НомерДок} AS НомерДок,
    Док_ПоверненняТоварівВідКлієнта.{Документи.ПоверненняТоварівВідКлієнта_Const.ДатаДок} AS ДатаДок,
    Довідник_Контрагенти.{Довідники.Контрагенти_Const.Назва} AS КонтрагентНазва,
    Док_ПоверненняТоварівВідКлієнта.{Документи.ПоверненняТоварівВідКлієнта_Const.СумаДокументу} AS Сума,
    Док_ПоверненняТоварівВідКлієнта.{Документи.ПоверненняТоварівВідКлієнта_Const.Коментар} AS Коментар
FROM
	{Документи.ПоверненняТоварівВідКлієнта_Const.TABLE} AS Док_ПоверненняТоварівВідКлієнта

    LEFT JOIN {Довідники.Контрагенти_Const.TABLE} AS Довідник_Контрагенти ON Довідник_Контрагенти.uid = 
        Док_ПоверненняТоварівВідКлієнта.{Документи.ПоверненняТоварівВідКлієнта_Const.Контрагент}

WHERE Док_ПоверненняТоварівВідКлієнта.docdate >= @docdate
";

            return query;
        }
        
        public static string ЗапитВибіркаЗакупки()
        {
            string query = $@"
SELECT
    'ЗамовленняПостачальнику' AS ТипДокументу,
    Док_ЗамовленняПостачальнику.uid,
    Док_ЗамовленняПостачальнику.spend,
    Док_ЗамовленняПостачальнику.{Документи.ЗамовленняПостачальнику_Const.Назва} AS Назва,
    Док_ЗамовленняПостачальнику.{Документи.ЗамовленняПостачальнику_Const.НомерДок} AS НомерДок,
    Док_ЗамовленняПостачальнику.{Документи.ЗамовленняПостачальнику_Const.ДатаДок} AS ДатаДок,
    Довідник_Контрагенти.{Довідники.Контрагенти_Const.Назва} AS КонтрагентНазва,
    Док_ЗамовленняПостачальнику.{Документи.ЗамовленняПостачальнику_Const.СумаДокументу} AS Сума,
    Док_ЗамовленняПостачальнику.{Документи.ЗамовленняПостачальнику_Const.Коментар} AS Коментар
FROM
	{Документи.ЗамовленняПостачальнику_Const.TABLE} AS Док_ЗамовленняПостачальнику

    LEFT JOIN {Довідники.Контрагенти_Const.TABLE} AS Довідник_Контрагенти ON Довідник_Контрагенти.uid = 
        Док_ЗамовленняПостачальнику.{Документи.ЗамовленняПостачальнику_Const.Контрагент}

WHERE Док_ЗамовленняПостачальнику.docdate >= @docdate

UNION

SELECT
    'ПоступленняТоварівТаПослуг' AS ТипДокументу,
    Док_ПоступленняТоварівТаПослуг.uid,
    Док_ПоступленняТоварівТаПослуг.spend,
    Док_ПоступленняТоварівТаПослуг.{Документи.ПоступленняТоварівТаПослуг_Const.Назва} AS Назва,
    Док_ПоступленняТоварівТаПослуг.{Документи.ПоступленняТоварівТаПослуг_Const.НомерДок} AS НомерДок,
    Док_ПоступленняТоварівТаПослуг.{Документи.ПоступленняТоварівТаПослуг_Const.ДатаДок} AS ДатаДок,
    Довідник_Контрагенти.{Довідники.Контрагенти_Const.Назва} AS КонтрагентНазва,
    Док_ПоступленняТоварівТаПослуг.{Документи.ПоступленняТоварівТаПослуг_Const.СумаДокументу} AS Сума,
    Док_ПоступленняТоварівТаПослуг.{Документи.ПоступленняТоварівТаПослуг_Const.Коментар} AS Коментар
FROM
	{Документи.ПоступленняТоварівТаПослуг_Const.TABLE} AS Док_ПоступленняТоварівТаПослуг

    LEFT JOIN {Довідники.Контрагенти_Const.TABLE} AS Довідник_Контрагенти ON Довідник_Контрагенти.uid = 
        Док_ПоступленняТоварівТаПослуг.{Документи.ПоступленняТоварівТаПослуг_Const.Контрагент}

WHERE Док_ПоступленняТоварівТаПослуг.docdate >= @docdate

UNION

SELECT
    'ПоверненняТоварівПостачальнику' AS ТипДокументу,
    Док_ПоверненняТоварівПостачальнику.uid,
    Док_ПоверненняТоварівПостачальнику.spend,
    Док_ПоверненняТоварівПостачальнику.{Документи.ПоверненняТоварівПостачальнику_Const.Назва} AS Назва,
    Док_ПоверненняТоварівПостачальнику.{Документи.ПоверненняТоварівПостачальнику_Const.НомерДок} AS НомерДок,
    Док_ПоверненняТоварівПостачальнику.{Документи.ПоверненняТоварівПостачальнику_Const.ДатаДок} AS ДатаДок,
    Довідник_Контрагенти.{Довідники.Контрагенти_Const.Назва} AS КонтрагентНазва,
    Док_ПоверненняТоварівПостачальнику.{Документи.ПоверненняТоварівПостачальнику_Const.СумаДокументу} AS Сума,
    Док_ПоверненняТоварівПостачальнику.{Документи.ПоверненняТоварівПостачальнику_Const.Коментар} AS Коментар
FROM
	{Документи.ПоверненняТоварівПостачальнику_Const.TABLE} AS Док_ПоверненняТоварівПостачальнику

    LEFT JOIN {Довідники.Контрагенти_Const.TABLE} AS Довідник_Контрагенти ON Довідник_Контрагенти.uid = 
        Док_ПоверненняТоварівПостачальнику.{Документи.ПоверненняТоварівПостачальнику_Const.Контрагент}

WHERE Док_ПоверненняТоварівПостачальнику.docdate >= @docdate
";

            return query;
        }

        public static string ЗапитВибіркаФінанси()
        {
            string query = $@"
SELECT
    'ПрихіднийКасовийОрдер' AS ТипДокументу,
    Док_ПрихіднийКасовийОрдер.uid,
    Док_ПрихіднийКасовийОрдер.spend,
    Док_ПрихіднийКасовийОрдер.{Документи.ПрихіднийКасовийОрдер_Const.Назва} AS Назва,
    Док_ПрихіднийКасовийОрдер.{Документи.ПрихіднийКасовийОрдер_Const.НомерДок} AS НомерДок,
    Док_ПрихіднийКасовийОрдер.{Документи.ПрихіднийКасовийОрдер_Const.ДатаДок} AS ДатаДок,
    Довідник_Контрагенти.{Довідники.Контрагенти_Const.Назва} AS КонтрагентНазва,
    Док_ПрихіднийКасовийОрдер.{Документи.ПрихіднийКасовийОрдер_Const.СумаДокументу} AS Сума,
    Док_ПрихіднийКасовийОрдер.{Документи.ПрихіднийКасовийОрдер_Const.Коментар} AS Коментар
FROM
	{Документи.ПрихіднийКасовийОрдер_Const.TABLE} AS Док_ПрихіднийКасовийОрдер

    LEFT JOIN {Довідники.Контрагенти_Const.TABLE} AS Довідник_Контрагенти ON Довідник_Контрагенти.uid = 
        Док_ПрихіднийКасовийОрдер.{Документи.ПрихіднийКасовийОрдер_Const.Контрагент}

WHERE Док_ПрихіднийКасовийОрдер.docdate >= @docdate

UNION

SELECT
    'РозхіднийКасовийОрдер' AS ТипДокументу,
    Док_РозхіднийКасовийОрдер.uid,
    Док_РозхіднийКасовийОрдер.spend,
    Док_РозхіднийКасовийОрдер.{Документи.РозхіднийКасовийОрдер_Const.Назва} AS Назва,
    Док_РозхіднийКасовийОрдер.{Документи.РозхіднийКасовийОрдер_Const.НомерДок} AS НомерДок,
    Док_РозхіднийКасовийОрдер.{Документи.РозхіднийКасовийОрдер_Const.ДатаДок} AS ДатаДок,
    Довідник_Контрагенти.{Довідники.Контрагенти_Const.Назва} AS КонтрагентНазва,
    Док_РозхіднийКасовийОрдер.{Документи.РозхіднийКасовийОрдер_Const.СумаДокументу} AS Сума,
    Док_РозхіднийКасовийОрдер.{Документи.РозхіднийКасовийОрдер_Const.Коментар} AS Коментар
FROM
	{Документи.РозхіднийКасовийОрдер_Const.TABLE} AS Док_РозхіднийКасовийОрдер

    LEFT JOIN {Довідники.Контрагенти_Const.TABLE} AS Довідник_Контрагенти ON Довідник_Контрагенти.uid = 
        Док_РозхіднийКасовийОрдер.{Документи.РозхіднийКасовийОрдер_Const.Контрагент}

WHERE Док_РозхіднийКасовийОрдер.docdate >= @docdate
";

            return query;
        }

        public static string ЗапитВибіркаСклад()
        {
            string query = $@"
SELECT
    'ПереміщенняТоварів' AS ТипДокументу,
    Док_ПереміщенняТоварів.uid,
    Док_ПереміщенняТоварів.spend,
    Док_ПереміщенняТоварів.{Документи.ПереміщенняТоварів_Const.Назва} AS Назва,
    Док_ПереміщенняТоварів.{Документи.ПереміщенняТоварів_Const.НомерДок} AS НомерДок,
    Док_ПереміщенняТоварів.{Документи.ПереміщенняТоварів_Const.ДатаДок} AS ДатаДок,
    Довідник_Склади.{Довідники.Склади_Const.Назва} AS КонтрагентНазва,
    0 AS Сума,
    Док_ПереміщенняТоварів.{Документи.ПереміщенняТоварів_Const.Коментар} AS Коментар
FROM
	{Документи.ПереміщенняТоварів_Const.TABLE} AS Док_ПереміщенняТоварів

    LEFT JOIN {Довідники.Склади_Const.TABLE} AS Довідник_Склади ON Довідник_Склади.uid = 
        Док_ПереміщенняТоварів.{Документи.ПереміщенняТоварів_Const.СкладВідправник}

WHERE Док_ПереміщенняТоварів.docdate >= @docdate
";

            return query;
        }

        public static string ЗапитВибіркаЦіноутворення()
        {
            string query = $@"
SELECT
    'ВстановленняЦінНоменклатури' AS ТипДокументу,
    Док_ВстановленняЦінНоменклатури.uid,
    Док_ВстановленняЦінНоменклатури.spend,
    Док_ВстановленняЦінНоменклатури.{Документи.ВстановленняЦінНоменклатури_Const.Назва} AS Назва,
    Док_ВстановленняЦінНоменклатури.{Документи.ВстановленняЦінНоменклатури_Const.НомерДок} AS НомерДок,
    Док_ВстановленняЦінНоменклатури.{Документи.ВстановленняЦінНоменклатури_Const.ДатаДок} AS ДатаДок,
    '' AS КонтрагентНазва,
    0 AS Сума,
    Док_ВстановленняЦінНоменклатури.{Документи.ВстановленняЦінНоменклатури_Const.Коментар} AS Коментар
FROM
	{Документи.ВстановленняЦінНоменклатури_Const.TABLE} AS Док_ВстановленняЦінНоменклатури

WHERE Док_ВстановленняЦінНоменклатури.docdate >= @docdate
";

            return query;
        }

        public static string ЗапитВибіркаВведенняЗалишків()
        {
            string query = $@"
SELECT
    'ВведенняЗалишків' AS ТипДокументу,
    Док_ВведенняЗалишків.uid,
    Док_ВведенняЗалишків.spend,
    Док_ВведенняЗалишків.{Документи.ВведенняЗалишків_Const.Назва} AS Назва,
    Док_ВведенняЗалишків.{Документи.ВведенняЗалишків_Const.НомерДок} AS НомерДок,
    Док_ВведенняЗалишків.{Документи.ВведенняЗалишків_Const.ДатаДок} AS ДатаДок,
    '' AS КонтрагентНазва,
    0 AS Сума,
    Док_ВведенняЗалишків.{Документи.ВведенняЗалишків_Const.Коментар} AS Коментар
FROM
	{Документи.ВведенняЗалишків_Const.TABLE} AS Док_ВведенняЗалишків

WHERE Док_ВведенняЗалишків.docdate >= @docdate
";

            return query;
        }

        public static string ЗапитВибіркаВнутрішніДокументи()
        {
            string query = $@"
SELECT
    'ВнутрішнєСпоживанняТоварів' AS ТипДокументу,
    Док_ВнутрішнєСпоживанняТоварів.uid,
    Док_ВнутрішнєСпоживанняТоварів.spend,
    Док_ВнутрішнєСпоживанняТоварів.{Документи.ВнутрішнєСпоживанняТоварів_Const.Назва} AS Назва,
    Док_ВнутрішнєСпоживанняТоварів.{Документи.ВнутрішнєСпоживанняТоварів_Const.НомерДок} AS НомерДок,
    Док_ВнутрішнєСпоживанняТоварів.{Документи.ВнутрішнєСпоживанняТоварів_Const.ДатаДок} AS ДатаДок,
    '' AS КонтрагентНазва,
    0 AS Сума,
    Док_ВнутрішнєСпоживанняТоварів.{Документи.ВнутрішнєСпоживанняТоварів_Const.Коментар} AS Коментар
FROM
	{Документи.ВнутрішнєСпоживанняТоварів_Const.TABLE} AS Док_ВнутрішнєСпоживанняТоварів

WHERE Док_ВнутрішнєСпоживанняТоварів.docdate >= @docdate
";

            return query;
        }

        public static string ЗапитВибіркаВсіхДокументів()
        {
            string query = $@"
{ЗапитВибіркаПродажі()}
UNION
{ЗапитВибіркаЗакупки()}
UNION
{ЗапитВибіркаФінанси()}
UNION
{ЗапитВибіркаСклад()}
UNION
{ЗапитВибіркаЦіноутворення()}
UNION
{ЗапитВибіркаВведенняЗалишків()}
UNION
{ЗапитВибіркаВнутрішніДокументи()}
";

            return query;
        }
    }
}
