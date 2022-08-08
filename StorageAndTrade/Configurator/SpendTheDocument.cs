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
 
Модуль проведення документів
 
*/

using System;
using System.Collections.Generic;
using AccountingSoftware;
using StorageAndTrade.Service;
using StorageAndTrade_1_0.Довідники;
using StorageAndTrade_1_0.РегістриНакопичення;

namespace StorageAndTrade_1_0.Документи
{
	class ЗамовленняКлієнта_SpendTheDocument
	{
		public static bool Spend(ЗамовленняКлієнта_Objest ДокументОбєкт)
		{
			if (ДокументОбєкт.Spend)
			{
				//Якщо дата проведення відрізняється від дати документу
				if (ДокументОбєкт.ДатаДок.ToString("dd.MM.yyyy") != ДокументОбєкт.SpendDate.ToString("dd.MM.yyyy"))
					CalculationBalances.AddTask(ДокументОбєкт.UnigueID.ToString(), ДокументОбєкт.TypeDocument, "Delete", ДокументОбєкт.SpendDate, "");
			}

			#region Рух по регістрах

			//
			//ЗамовленняКлієнтів
			//

			РегістриНакопичення.ЗамовленняКлієнтів_RecordsSet замовленняКлієнтів_RecordsSet = new РегістриНакопичення.ЗамовленняКлієнтів_RecordsSet();

			ДокументОбєкт.Товари_TablePart.Read();

			foreach (ЗамовленняКлієнта_Товари_TablePart.Record Товари_Record in ДокументОбєкт.Товари_TablePart.Records)
			{
				РегістриНакопичення.ЗамовленняКлієнтів_RecordsSet.Record record = new РегістриНакопичення.ЗамовленняКлієнтів_RecordsSet.Record();
				замовленняКлієнтів_RecordsSet.Records.Add(record);

				record.Income = true; // +
				record.Owner = ДокументОбєкт.UnigueID.UGuid;

				record.ЗамовленняКлієнта = ДокументОбєкт.GetDocumentPointer();
				record.Номенклатура = Товари_Record.Номенклатура;
				record.ХарактеристикаНоменклатури = Товари_Record.ХарактеристикаНоменклатури;
				record.Склад = ДокументОбєкт.Склад;
				record.Замовлено = Товари_Record.Кількість;
				record.Сума = Товари_Record.Сума;
			}

			замовленняКлієнтів_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

			//
			//ВільніЗалишки
			//

			РегістриНакопичення.ВільніЗалишки_RecordsSet вільніЗалишки_RecordsSet = new РегістриНакопичення.ВільніЗалишки_RecordsSet();

			foreach (ЗамовленняКлієнта_Товари_TablePart.Record Товари_Record in ДокументОбєкт.Товари_TablePart.Records)
			{
				РегістриНакопичення.ВільніЗалишки_RecordsSet.Record record = new РегістриНакопичення.ВільніЗалишки_RecordsSet.Record();
				вільніЗалишки_RecordsSet.Records.Add(record);

				record.Income = true; // +    | Документ добавляє резерв
				record.Owner = ДокументОбєкт.UnigueID.UGuid;

				record.Номенклатура = Товари_Record.Номенклатура;
				record.ХарактеристикаНоменклатури = Товари_Record.ХарактеристикаНоменклатури;
				record.Склад = ДокументОбєкт.Склад;
				record.ВРезервіПідЗамовлення = Товари_Record.Кількість;
			}

			вільніЗалишки_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

			//
			//РозрахункиЗКлієнтами
			//

			//РегістриНакопичення.РозрахункиЗКлієнтами_RecordsSet розрахункиЗКлієнтами_RecordsSet = new РегістриНакопичення.РозрахункиЗКлієнтами_RecordsSet();

			//РегістриНакопичення.РозрахункиЗКлієнтами_RecordsSet.Record розрахункиЗКлієнтами_Record = new РегістриНакопичення.РозрахункиЗКлієнтами_RecordsSet.Record();
			//розрахункиЗКлієнтами_RecordsSet.Records.Add(розрахункиЗКлієнтами_Record);

			//розрахункиЗКлієнтами_Record.Income = true; // +
			//розрахункиЗКлієнтами_Record.Owner = ДокументОбєкт.UnigueID.UGuid;

			//розрахункиЗКлієнтами_Record.Контрагент = ДокументОбєкт.Контрагент;
			//розрахункиЗКлієнтами_Record.Валюта = ДокументОбєкт.Валюта;
			//розрахункиЗКлієнтами_Record.Сума = ДокументОбєкт.СумаДокументу;

			//розрахункиЗКлієнтами_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

			#endregion

			CalculationBalances.AddTask(ДокументОбєкт.UnigueID.ToString(), ДокументОбєкт.TypeDocument, "Add", ДокументОбєкт.ДатаДок, "");

			return true;
		}

		public static void ClearSpend(ЗамовленняКлієнта_Objest ДокументОбєкт)
		{
			РегістриНакопичення.ЗамовленняКлієнтів_RecordsSet замовленняКлієнтів_RecordsSet = new РегістриНакопичення.ЗамовленняКлієнтів_RecordsSet();
			замовленняКлієнтів_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);

			//РегістриНакопичення.РозрахункиЗКлієнтами_RecordsSet розрахункиЗКлієнтами_RecordsSet = new РегістриНакопичення.РозрахункиЗКлієнтами_RecordsSet();
			//розрахункиЗКлієнтами_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);

			РегістриНакопичення.ВільніЗалишки_RecordsSet вільніЗалишки_RecordsSet = new РегістриНакопичення.ВільніЗалишки_RecordsSet();
			вільніЗалишки_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);

			CalculationBalances.AddTask(ДокументОбєкт.UnigueID.ToString(), ДокументОбєкт.TypeDocument, "Delete", ДокументОбєкт.ДатаДок, "");
		}
	}

	class РеалізаціяТоварівТаПослуг_SpendTheDocument
	{
		/// <summary>
		/// Повертає список партій на дату та час документу.
		/// Відбір відбувається по Організації, Номенклатурі, Характеристиці, Серії.
		/// Якщо документ уже був проведений, його записи в регістр не враховуються.
		/// Сортування (на даний момент) по даті документу поступлення. LIFO
		/// </summary>
		/// <param name="ДокументОбєкт">Документ</param>
		/// <param name="ТовариРядок">Рядок з таб частини</param>
		/// <returns>Список з іменованим словником даних</returns>
		private static List<Dictionary<string, object>> ОтриматиСписокПартій(РеалізаціяТоварівТаПослуг_Objest ДокументОбєкт,
			РеалізаціяТоварівТаПослуг_Товари_TablePart.Record ТовариРядок)
		{
			//
			//Вибірка даних з регістру, так як Віртуальні залишки розраховуються із запізненням
			//і в основному віртуальні залишки потрібні для звітів.
			//Також віртуальні залишки можуть не розраховуватися при масовому проведенні документів
			//

			string query = $@"
WITH register AS
(
	SELECT 
		ПартіїТоварів.{ПартіїТоварів_Const.ДокументПоступлення} AS ДокументПоступлення,
		SUM(CASE WHEN ПартіїТоварів.income = true THEN 
			ПартіїТоварів.{ПартіїТоварів_Const.Кількість} ELSE 
			-ПартіїТоварів.{ПартіїТоварів_Const.Кількість} END) AS Кількість,
		SUM(CASE WHEN ПартіїТоварів.income = true THEN 
			ПартіїТоварів.{ПартіїТоварів_Const.Собівартість} ELSE 
			-ПартіїТоварів.{ПартіїТоварів_Const.Собівартість} END) AS Собівартість
	FROM
		{ПартіїТоварів_Const.TABLE} AS ПартіїТоварів
	WHERE
		ПартіїТоварів.period <= @period_end
		AND ПартіїТоварів.{ПартіїТоварів_Const.Організація} = '{ДокументОбєкт.Організація.UnigueID}'
		AND ПартіїТоварів.{ПартіїТоварів_Const.Номенклатура} = '{ТовариРядок.Номенклатура.UnigueID}'
		AND ПартіїТоварів.{ПартіїТоварів_Const.ХарактеристикаНоменклатури} = '{ТовариРядок.ХарактеристикаНоменклатури.UnigueID}'
		AND ПартіїТоварів.{ПартіїТоварів_Const.Серія} = '{ТовариРядок.Серія.UnigueID}'
        AND ПартіїТоварів.owner != '{ДокументОбєкт.UnigueID}'

	GROUP BY ДокументПоступлення

	HAVING
		SUM(CASE WHEN ПартіїТоварів.income = true THEN 
			ПартіїТоварів.{ПартіїТоварів_Const.Кількість} ELSE 
			-ПартіїТоварів.{ПартіїТоварів_Const.Кількість} END) > 0
)";

			//
			//ДостатняКількість обчислюється для того щоб вибирати тільки потрібні партії, а не всі наявні партії.
			//ДостатняКількість = Накопичена кількість >= Потрібній кількості
			//

			query += $@"
, Обчислення AS
(
	SELECT
	   Документ_ПоступленняТоварівТаПослуг.{ПоступленняТоварівТаПослуг_Const.ДатаДок} AS ДатаПоступлення,
	   ДокументПоступлення,
	   Кількість,
	   Собівартість,
	   SUM(Кількість) OVER(ORDER BY Документ_ПоступленняТоварівТаПослуг.{ПоступленняТоварівТаПослуг_Const.ДатаДок}) >= @Кількість AS ДостатняКількість
	FROM
	   register
		  LEFT JOIN {ПоступленняТоварівТаПослуг_Const.TABLE} AS Документ_ПоступленняТоварівТаПослуг ON 
			 Документ_ПоступленняТоварівТаПослуг.uid = register.ДокументПоступлення
	ORDER BY ДатаПоступлення
)";

			//
			//Вибираються дві групи
			//a. Партії які мають кількість меншу потрібній кількості
			//b. Одну партію яка закриває потрібну кількість
			//

			query += $@"
(
	SELECT 
		'a' Група,
		ДокументПоступлення,
		Кількість,
		Собівартість
	FROM Обчислення
	WHERE ДостатняКількість = false
)
UNION
(
	SELECT 
		'b' Група,
		ДокументПоступлення,
		Кількість,
		Собівартість
	FROM Обчислення
	WHERE ДостатняКількість = true
	LIMIT 1
)
";

			Dictionary<string, object> paramQuery = new Dictionary<string, object>();
			Console.WriteLine(ДокументОбєкт.ДатаДок);
			paramQuery.Add("period_end", ДокументОбєкт.ДатаДок);
			paramQuery.Add("Кількість", ТовариРядок.Кількість);

			string[] columnsName;
			List<Dictionary<string, object>> listNameRow;

			Config.Kernel.DataBase.SelectRequest(query, paramQuery, out columnsName, out listNameRow);

			return listNameRow;
		}

		private static void ПеревіркаЗаповненняТабличноїЧастини(РеалізаціяТоварівТаПослуг_Objest ДокументОбєкт)
		{
			List<string> СписокПомилок = new List<string>();

			foreach (РеалізаціяТоварівТаПослуг_Товари_TablePart.Record ТовариРядок in ДокументОбєкт.Товари_TablePart.Records)
			{
				if (ТовариРядок.Номенклатура.IsEmpty())
					СписокПомилок.Add($"Не заповнене поле Номенклатура в рядку {ТовариРядок.НомерРядка}");
			}

			if (СписокПомилок.Count > 0)
				throw new Exception(string.Join("\n", СписокПомилок.ToArray()));
		}

		private static List<Dictionary<string, object>> ОтриматиЗалишкиТоварівНаСкладах(РеалізаціяТоварівТаПослуг_Objest ДокументОбєкт,
			РеалізаціяТоварівТаПослуг_Товари_TablePart.Record ТовариРядок)
        {
			string query = $@"
WITH register AS
(
	SELECT 
		'Залишок' AS Група,
		SUM(CASE WHEN ТовариНаСкладах.income = true THEN 
			ТовариНаСкладах.{ТовариНаСкладах_Const.ВНаявності} ELSE 
			-ТовариНаСкладах.{ТовариНаСкладах_Const.ВНаявності} END) AS ВНаявності
	FROM
		{ТовариНаСкладах_Const.TABLE} AS ТовариНаСкладах
	WHERE
		ТовариНаСкладах.period <= @period_end
		AND ТовариНаСкладах.{ТовариНаСкладах_Const.Номенклатура} = '{ТовариРядок.Номенклатура.UnigueID}'
		AND ТовариНаСкладах.{ТовариНаСкладах_Const.ХарактеристикаНоменклатури} = '{ТовариРядок.ХарактеристикаНоменклатури.UnigueID}'
		AND ТовариНаСкладах.{ТовариНаСкладах_Const.Серія} = '{ТовариРядок.Серія.UnigueID}'
        AND ТовариНаСкладах.{ТовариНаСкладах_Const.Склад} = @Склад
        AND ТовариНаСкладах.owner != '{ДокументОбєкт.UnigueID}'

	GROUP BY Група
)
SELECT
    ВНаявності
FROM
    register
";

			Dictionary<string, object> paramQuery = new Dictionary<string, object>();
			Console.WriteLine(ДокументОбєкт.ДатаДок);
			paramQuery.Add("period_end", ДокументОбєкт.ДатаДок);
			paramQuery.Add("Склад", !ТовариРядок.Склад.IsEmpty() ? ТовариРядок.Склад.UnigueID.UGuid : ДокументОбєкт.Склад.UnigueID.UGuid);

			string[] columnsName;
			List<Dictionary<string, object>> listNameRow;

			Config.Kernel.DataBase.SelectRequest(query, paramQuery, out columnsName, out listNameRow);

			return listNameRow;
		}

		public static bool Spend(РеалізаціяТоварівТаПослуг_Objest ДокументОбєкт)
		{
			#region Підготовка

			List<string> СписокПомилок = new List<string>();

			ПеревіркаЗаповненняТабличноїЧастини(ДокументОбєкт);

			Dictionary<int, Номенклатура_Objest> СписокНоменклатури = new Dictionary<int, Номенклатура_Objest>();
			Dictionary<int, decimal> ЗалишокНоменклатури = new Dictionary<int, decimal>();
			
			foreach (РеалізаціяТоварівТаПослуг_Товари_TablePart.Record ТовариРядок in ДокументОбєкт.Товари_TablePart.Records)
			{
				СписокНоменклатури.Add(ТовариРядок.НомерРядка, ТовариРядок.Номенклатура.GetDirectoryObject());
				ЗалишокНоменклатури.Add(ТовариРядок.НомерРядка, 0);

				//Для товарів отримуємо залишки
				if (СписокНоменклатури[ТовариРядок.НомерРядка].ТипНоменклатури == Перелічення.ТипиНоменклатури.Товар)
				{
					List<Dictionary<string, object>> listNameRow = ОтриматиЗалишкиТоварівНаСкладах(ДокументОбєкт, ТовариРядок);

					if (listNameRow.Count > 0)
						ЗалишокНоменклатури[ТовариРядок.НомерРядка] = (decimal)listNameRow[0]["ВНаявності"];

					if (ЗалишокНоменклатури[ТовариРядок.НомерРядка] < ТовариРядок.Кількість)
						СписокПомилок.Add($"Недостатньо товару {СписокНоменклатури[ТовариРядок.НомерРядка].Назва}." +
							$"Потрібно {ТовариРядок.Кількість} є {ЗалишокНоменклатури[ТовариРядок.НомерРядка]}");
				}
			}

			if (СписокПомилок.Count > 0)
				throw new Exception(string.Join("\n", СписокПомилок.ToArray()));

			#endregion

			if (ДокументОбєкт.Spend)
			{
				//Якщо дата проведення відрізняється від дати документу
				if (ДокументОбєкт.ДатаДок.ToString("dd.MM.yyyy") != ДокументОбєкт.SpendDate.ToString("dd.MM.yyyy"))
					CalculationBalances.AddTask(ДокументОбєкт.UnigueID.ToString(), ДокументОбєкт.TypeDocument, "Delete", ДокументОбєкт.SpendDate, "");
			}

			#region ЗамовленняКлієнтів

			//
			//ЗамовленняКлієнтів
			//

			РегістриНакопичення.ЗамовленняКлієнтів_RecordsSet замовленняКлієнтів_RecordsSet = new РегістриНакопичення.ЗамовленняКлієнтів_RecordsSet();

			foreach (РеалізаціяТоварівТаПослуг_Товари_TablePart.Record Товари_Record in ДокументОбєкт.Товари_TablePart.Records)
			{
				if (!Товари_Record.ЗамовленняКлієнта.IsEmpty())
				{
					РегістриНакопичення.ЗамовленняКлієнтів_RecordsSet.Record record = new РегістриНакопичення.ЗамовленняКлієнтів_RecordsSet.Record();
					замовленняКлієнтів_RecordsSet.Records.Add(record);

					record.Income = false;
					record.Owner = ДокументОбєкт.UnigueID.UGuid;

					record.ЗамовленняКлієнта = Товари_Record.ЗамовленняКлієнта;
					record.Номенклатура = Товари_Record.Номенклатура;
					record.ХарактеристикаНоменклатури = Товари_Record.ХарактеристикаНоменклатури;
					record.Склад = (!Товари_Record.Склад.IsEmpty() ? Товари_Record.Склад : ДокументОбєкт.Склад);
					record.Замовлено = Товари_Record.Кількість;
					record.Сума = Товари_Record.Сума;
				}
			}

			замовленняКлієнтів_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

			#endregion

			#region ВільніЗалишки

			//
			//ВільніЗалишки
			//

			РегістриНакопичення.ВільніЗалишки_RecordsSet вільніЗалишки_RecordsSet = new РегістриНакопичення.ВільніЗалишки_RecordsSet();

			foreach (РеалізаціяТоварівТаПослуг_Товари_TablePart.Record Товари_Record in ДокументОбєкт.Товари_TablePart.Records)
			{
				РегістриНакопичення.ВільніЗалишки_RecordsSet.Record record = new РегістриНакопичення.ВільніЗалишки_RecordsSet.Record();
				вільніЗалишки_RecordsSet.Records.Add(record);

				record.Income = false;
				record.Owner = ДокументОбєкт.UnigueID.UGuid;

				record.Номенклатура = Товари_Record.Номенклатура;
				record.ХарактеристикаНоменклатури = Товари_Record.ХарактеристикаНоменклатури;
				record.Склад = (!Товари_Record.Склад.IsEmpty() ? Товари_Record.Склад : ДокументОбєкт.Склад);
				record.ВРезервіПідЗамовлення = Товари_Record.Кількість;
			}

			вільніЗалишки_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

			#endregion

			#region Товари на складах

			//
			//Товари на складах
			//

			ТовариНаСкладах_RecordsSet товариНаСкладах_RecordsSet = new ТовариНаСкладах_RecordsSet();

			foreach (РеалізаціяТоварівТаПослуг_Товари_TablePart.Record Товари_Record in ДокументОбєкт.Товари_TablePart.Records)
			{
				if (СписокНоменклатури[Товари_Record.НомерРядка].ТипНоменклатури == Перелічення.ТипиНоменклатури.Товар)
				{
					ТовариНаСкладах_RecordsSet.Record record = new ТовариНаСкладах_RecordsSet.Record();
					товариНаСкладах_RecordsSet.Records.Add(record);

					record.Income = false;
					record.Owner = ДокументОбєкт.UnigueID.UGuid;

					record.Номенклатура = Товари_Record.Номенклатура;
					record.ХарактеристикаНоменклатури = Товари_Record.ХарактеристикаНоменклатури;
					record.Серія = Товари_Record.Серія;
					record.Склад = (!Товари_Record.Склад.IsEmpty() ? Товари_Record.Склад : ДокументОбєкт.Склад);
					record.ВНаявності = Товари_Record.Кількість;
				}
			}

			товариНаСкладах_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

			#endregion

			#region Партії товарів

			//
			//Партії товарів
			//

			РегістриНакопичення.ПартіїТоварів_RecordsSet партіїТоварів_RecordsSet = new РегістриНакопичення.ПартіїТоварів_RecordsSet();

			foreach (РеалізаціяТоварівТаПослуг_Товари_TablePart.Record Товари_Record in ДокументОбєкт.Товари_TablePart.Records)
			{
				//Товар
				if (СписокНоменклатури[Товари_Record.НомерРядка].ТипНоменклатури == Перелічення.ТипиНоменклатури.Товар)
				{
					List<Dictionary<string, object>> listNameRow = ОтриматиСписокПартій(ДокументОбєкт, Товари_Record);

					if (listNameRow.Count == 0)
					{
						Console.WriteLine($"Немає доступних партій для товару {СписокНоменклатури[Товари_Record.НомерРядка].Назва}");
					}

					decimal КількістьЯкуПотрібноСписати = Товари_Record.Кількість;

					foreach (Dictionary<string, object> nameRow in listNameRow)
					{
						decimal КількістьВПартії = (decimal)nameRow["Кількість"];
						decimal СобівартістьПартії = (decimal)nameRow["Собівартість"];

						decimal КількістьЩоСписується = 0;
						bool ЗакритиПартію = (КількістьЯкуПотрібноСписати >= КількістьВПартії);

						if (КількістьВПартії >= КількістьЯкуПотрібноСписати)
						{
							КількістьЩоСписується = КількістьЯкуПотрібноСписати;
							КількістьЯкуПотрібноСписати = 0;
						}
						else
						{
							КількістьЩоСписується = КількістьВПартії;
							КількістьЯкуПотрібноСписати -= КількістьВПартії;
						}

						ПартіїТоварів_RecordsSet.Record record = new ПартіїТоварів_RecordsSet.Record();
						партіїТоварів_RecordsSet.Records.Add(record);

						record.Income = false;
						record.Owner = ДокументОбєкт.UnigueID.UGuid;

						record.Організація = ДокументОбєкт.Організація;
						record.ДокументПоступлення = new ПоступленняТоварівТаПослуг_Pointer(nameRow["ДокументПоступлення"]);
						record.Кількість = КількістьЩоСписується;
						record.Собівартість = (ЗакритиПартію ? СобівартістьПартії : 0);
						record.СписанаСобівартість = СобівартістьПартії;
						record.Номенклатура = Товари_Record.Номенклатура;
						record.ХарактеристикаНоменклатури = Товари_Record.ХарактеристикаНоменклатури;
						record.Серія = Товари_Record.Серія;

						if (КількістьЯкуПотрібноСписати == 0)
							break;
					}

					if (КількістьЯкуПотрібноСписати > 0)
					{
						Console.WriteLine($"Невистачило списати {КількістьЯкуПотрібноСписати} товарів");
					}
				}
			}

			партіїТоварів_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

			#endregion

			#region РозрахункиЗКлієнтами

			//??? Договір

			//
			//РозрахункиЗКлієнтами
			//

			РегістриНакопичення.РозрахункиЗКлієнтами_RecordsSet розрахункиЗКлієнтами_RecordsSet = new РегістриНакопичення.РозрахункиЗКлієнтами_RecordsSet();

			РегістриНакопичення.РозрахункиЗКлієнтами_RecordsSet.Record розрахункиЗКлієнтами_Record = new РегістриНакопичення.РозрахункиЗКлієнтами_RecordsSet.Record();
			розрахункиЗКлієнтами_RecordsSet.Records.Add(розрахункиЗКлієнтами_Record);

			розрахункиЗКлієнтами_Record.Income = true;
			розрахункиЗКлієнтами_Record.Owner = ДокументОбєкт.UnigueID.UGuid;

			розрахункиЗКлієнтами_Record.Контрагент = ДокументОбєкт.Контрагент;
			розрахункиЗКлієнтами_Record.Валюта = ДокументОбєкт.Валюта;
			розрахункиЗКлієнтами_Record.Сума = ДокументОбєкт.СумаДокументу;

			розрахункиЗКлієнтами_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

			#endregion

			CalculationBalances.AddTask(ДокументОбєкт.UnigueID.ToString(), ДокументОбєкт.TypeDocument, "Add", ДокументОбєкт.ДатаДок, "");

			return true;
		}

		public static void ClearSpend(РеалізаціяТоварівТаПослуг_Objest ДокументОбєкт)
		{
			РегістриНакопичення.ЗамовленняКлієнтів_RecordsSet замовленняКлієнтів_RecordsSet = new РегістриНакопичення.ЗамовленняКлієнтів_RecordsSet();
			замовленняКлієнтів_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);

			РегістриНакопичення.ВільніЗалишки_RecordsSet вільніЗалишки_RecordsSet = new РегістриНакопичення.ВільніЗалишки_RecordsSet();
			вільніЗалишки_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);

			РегістриНакопичення.ТовариНаСкладах_RecordsSet товариНаСкладах_RecordsSet = new РегістриНакопичення.ТовариНаСкладах_RecordsSet();
			товариНаСкладах_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);

			РегістриНакопичення.ПартіїТоварів_RecordsSet партіїТоварів_RecordsSet = new РегістриНакопичення.ПартіїТоварів_RecordsSet();
			партіїТоварів_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);

			РегістриНакопичення.РозрахункиЗКлієнтами_RecordsSet розрахункиЗКлієнтами_RecordsSet = new РегістриНакопичення.РозрахункиЗКлієнтами_RecordsSet();
			розрахункиЗКлієнтами_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);

			CalculationBalances.AddTask(ДокументОбєкт.UnigueID.ToString(), ДокументОбєкт.TypeDocument, "Delete", ДокументОбєкт.ДатаДок, "");
		}
	}

	class АктВиконанихРобіт_SpendTheDocument
	{
		public static bool Spend(АктВиконанихРобіт_Objest ДокументОбєкт)
		{
			if (ДокументОбєкт.Spend)
			{
				//Якщо дата проведення відрізняється від дати документу
				if (ДокументОбєкт.ДатаДок.ToString("dd.MM.yyyy") != ДокументОбєкт.SpendDate.ToString("dd.MM.yyyy"))
					CalculationBalances.AddTask(ДокументОбєкт.UnigueID.ToString(), ДокументОбєкт.TypeDocument, "Delete", ДокументОбєкт.SpendDate, "");
			}

			#region Рух по регістрах

			//
			//РозрахункиЗКлієнтами
			//

			РегістриНакопичення.РозрахункиЗКлієнтами_RecordsSet розрахункиЗКлієнтами_RecordsSet = new РегістриНакопичення.РозрахункиЗКлієнтами_RecordsSet();

			РегістриНакопичення.РозрахункиЗКлієнтами_RecordsSet.Record розрахункиЗКлієнтами_Record = new РегістриНакопичення.РозрахункиЗКлієнтами_RecordsSet.Record();
			розрахункиЗКлієнтами_RecordsSet.Records.Add(розрахункиЗКлієнтами_Record);

			розрахункиЗКлієнтами_Record.Income = true;
			розрахункиЗКлієнтами_Record.Owner = ДокументОбєкт.UnigueID.UGuid;

			розрахункиЗКлієнтами_Record.Контрагент = ДокументОбєкт.Контрагент;
			розрахункиЗКлієнтами_Record.Валюта = ДокументОбєкт.Валюта;
			розрахункиЗКлієнтами_Record.Сума = ДокументОбєкт.СумаДокументу;

			розрахункиЗКлієнтами_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

			#endregion

			CalculationBalances.AddTask(ДокументОбєкт.UnigueID.ToString(), ДокументОбєкт.TypeDocument, "Add", ДокументОбєкт.ДатаДок, "");

			return true;
		}

		public static void ClearSpend(АктВиконанихРобіт_Objest ДокументОбєкт)
		{
			РегістриНакопичення.РозрахункиЗКлієнтами_RecordsSet розрахункиЗКлієнтами_RecordsSet = new РегістриНакопичення.РозрахункиЗКлієнтами_RecordsSet();
			розрахункиЗКлієнтами_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);

			CalculationBalances.AddTask(ДокументОбєкт.UnigueID.ToString(), ДокументОбєкт.TypeDocument, "Delete", ДокументОбєкт.ДатаДок, "");
		}
	}

	class ПоступленняТоварівТаПослуг_SpendTheDocument
	{
		public static bool Spend(ПоступленняТоварівТаПослуг_Objest ДокументОбєкт)
		{
			if (ДокументОбєкт.Spend)
			{
				//Якщо дата проведення відрізняється від дати документу
				if (ДокументОбєкт.ДатаДок.ToString("dd.MM.yyyy") != ДокументОбєкт.SpendDate.ToString("dd.MM.yyyy"))
					CalculationBalances.AddTask(ДокументОбєкт.UnigueID.ToString(), ДокументОбєкт.TypeDocument, "Delete", ДокументОбєкт.SpendDate, "");
			}

			#region Рух по регістрах

			//
			//Замовлення постачальникам
			//

			РегістриНакопичення.ЗамовленняПостачальникам_RecordsSet замовленняПостачальникам_RecordsSet = new РегістриНакопичення.ЗамовленняПостачальникам_RecordsSet();

			foreach (ПоступленняТоварівТаПослуг_Товари_TablePart.Record Товари_Record in ДокументОбєкт.Товари_TablePart.Records)
			{
				//Якщо заданий документ замовлення
				if (!Товари_Record.ЗамовленняПостачальнику.IsEmpty())
				{
					РегістриНакопичення.ЗамовленняПостачальникам_RecordsSet.Record record = new РегістриНакопичення.ЗамовленняПостачальникам_RecordsSet.Record();
					замовленняПостачальникам_RecordsSet.Records.Add(record);

					record.Income = false; // -
					record.Owner = ДокументОбєкт.UnigueID.UGuid;

					record.ЗамовленняПостачальнику = Товари_Record.ЗамовленняПостачальнику;
					record.Номенклатура = Товари_Record.Номенклатура;
					record.ХарактеристикаНоменклатури = Товари_Record.ХарактеристикаНоменклатури;
					record.Склад = (!Товари_Record.Склад.IsEmpty() ? Товари_Record.Склад : ДокументОбєкт.Склад);
					record.Замовлено = Товари_Record.Кількість;
				}
			}

			замовленняПостачальникам_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

			//
			//Товари на складах
			//

			РегістриНакопичення.ТовариНаСкладах_RecordsSet товариНаСкладах_RecordsSet = new РегістриНакопичення.ТовариНаСкладах_RecordsSet();

			foreach (ПоступленняТоварівТаПослуг_Товари_TablePart.Record Товари_Record in ДокументОбєкт.Товари_TablePart.Records)
			{
				Довідники.Номенклатура_Objest номенклатура_Objest = Товари_Record.Номенклатура.GetDirectoryObject();

				//Товар
				if (номенклатура_Objest.ТипНоменклатури == Перелічення.ТипиНоменклатури.Товар)
				{
					РегістриНакопичення.ТовариНаСкладах_RecordsSet.Record record = new РегістриНакопичення.ТовариНаСкладах_RecordsSet.Record();
					товариНаСкладах_RecordsSet.Records.Add(record);

					record.Income = true; // + 
					record.Owner = ДокументОбєкт.UnigueID.UGuid;

					record.Номенклатура = Товари_Record.Номенклатура;
					record.ХарактеристикаНоменклатури = Товари_Record.ХарактеристикаНоменклатури;
					record.Склад = (!Товари_Record.Склад.IsEmpty() ? Товари_Record.Склад : ДокументОбєкт.Склад);
					record.Серія = Товари_Record.Серія;
					record.ВНаявності = Товари_Record.Кількість;
				}
			}

			товариНаСкладах_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

			//
			//Партії товарів
			//

			РегістриНакопичення.ПартіїТоварів_RecordsSet партіїТоварів_RecordsSet = new РегістриНакопичення.ПартіїТоварів_RecordsSet();

			foreach (ПоступленняТоварівТаПослуг_Товари_TablePart.Record Товари_Record in ДокументОбєкт.Товари_TablePart.Records)
			{
				Довідники.Номенклатура_Objest номенклатура_Objest = Товари_Record.Номенклатура.GetDirectoryObject();

				//Товар
				if (номенклатура_Objest.ТипНоменклатури == Перелічення.ТипиНоменклатури.Товар)
				{
					РегістриНакопичення.ПартіїТоварів_RecordsSet.Record record = new РегістриНакопичення.ПартіїТоварів_RecordsSet.Record();
					партіїТоварів_RecordsSet.Records.Add(record);

					record.Income = true; // + 
					record.Owner = ДокументОбєкт.UnigueID.UGuid;

					record.Організація = ДокументОбєкт.Організація;
					record.ДокументПоступлення = ДокументОбєкт.GetDocumentPointer();
					record.Кількість = Товари_Record.Кількість;
					record.Собівартість = Товари_Record.Ціна;
					record.Номенклатура = Товари_Record.Номенклатура;
					record.ХарактеристикаНоменклатури = Товари_Record.ХарактеристикаНоменклатури;
					record.Серія = Товари_Record.Серія;
				}
			}

			партіїТоварів_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

			//
			//ВільніЗалишки
			//

			РегістриНакопичення.ВільніЗалишки_RecordsSet вільніЗалишки_RecordsSet = new РегістриНакопичення.ВільніЗалишки_RecordsSet();

			foreach (ПоступленняТоварівТаПослуг_Товари_TablePart.Record Товари_Record in ДокументОбєкт.Товари_TablePart.Records)
			{
				Довідники.Номенклатура_Objest номенклатура_Objest = Товари_Record.Номенклатура.GetDirectoryObject();

				//Товар
				if (номенклатура_Objest.ТипНоменклатури == Перелічення.ТипиНоменклатури.Товар)
				{
					РегістриНакопичення.ВільніЗалишки_RecordsSet.Record record = new РегістриНакопичення.ВільніЗалишки_RecordsSet.Record();
					вільніЗалишки_RecordsSet.Records.Add(record);

					record.Income = true; // +
					record.Owner = ДокументОбєкт.UnigueID.UGuid;

					record.Номенклатура = Товари_Record.Номенклатура;
					record.ХарактеристикаНоменклатури = Товари_Record.ХарактеристикаНоменклатури;
					record.Склад = (!Товари_Record.Склад.IsEmpty() ? Товари_Record.Склад : ДокументОбєкт.Склад);
					record.ВНаявності = Товари_Record.Кількість;
				}
			}

			вільніЗалишки_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

            //
            //ТовариДоПоступлення
            //

            //РегістриНакопичення.ТовариДоПоступлення_RecordsSet товариДоПоступлення_RecordsSet = new РегістриНакопичення.ТовариДоПоступлення_RecordsSet();

            //foreach (ПоступленняТоварівТаПослуг_Товари_TablePart.Record Товари_Record in ДокументОбєкт.Товари_TablePart.Records)
            //{
            //    Довідники.Номенклатура_Objest номенклатура_Objest = Товари_Record.Номенклатура.GetDirectoryObject();

            //    //Товар
            //    if (номенклатура_Objest.ТипНоменклатури == Перелічення.ТипиНоменклатури.Товар)
            //    {
            //        РегістриНакопичення.ТовариДоПоступлення_RecordsSet.Record record = new РегістриНакопичення.ТовариДоПоступлення_RecordsSet.Record();
            //        товариДоПоступлення_RecordsSet.Records.Add(record);

            //        record.Income = false; // -
            //        record.Owner = ДокументОбєкт.UnigueID.UGuid;

            //        record.Номенклатура = Товари_Record.Номенклатура;
            //        record.ХарактеристикаНоменклатури = Товари_Record.ХарактеристикаНоменклатури;
            //        record.Склад = (!Товари_Record.Склад.IsEmpty() ? Товари_Record.Склад : ДокументОбєкт.Склад);
            //        record.ДоПоступлення = Товари_Record.Кількість;
            //    }
            //}

            //товариДоПоступлення_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

            //??? Договір
            // !!! Замовлення постачальнику робить рухи по регістру РозрахункиЗПостачальниками
            // значить ПрихНакладна не повинна ще раз робити ці рухи якщо є замовлення

            //
            //РозрахункиЗПостачальниками
            //

            РегістриНакопичення.РозрахункиЗПостачальниками_RecordsSet розрахункиЗПостачальниками_RecordsSet = new РегістриНакопичення.РозрахункиЗПостачальниками_RecordsSet();

			if (ДокументОбєкт.ГосподарськаОперація == Перелічення.ГосподарськіОперації.ЗакупівляВПостачальника)
			{
				РегістриНакопичення.РозрахункиЗПостачальниками_RecordsSet.Record recordContragent = new РегістриНакопичення.РозрахункиЗПостачальниками_RecordsSet.Record();
				розрахункиЗПостачальниками_RecordsSet.Records.Add(recordContragent);

				recordContragent.Income = false;
				recordContragent.Owner = ДокументОбєкт.UnigueID.UGuid;

				recordContragent.Контрагент = ДокументОбєкт.Контрагент;
				recordContragent.Валюта = ДокументОбєкт.Валюта;
				recordContragent.Сума = ДокументОбєкт.СумаДокументу;
			}

			розрахункиЗПостачальниками_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

			#endregion

			CalculationBalances.AddTask(ДокументОбєкт.UnigueID.ToString(), ДокументОбєкт.TypeDocument, "Add", ДокументОбєкт.ДатаДок, "");

			return true;
		}

		public static void ClearSpend(ПоступленняТоварівТаПослуг_Objest ДокументОбєкт)
		{
			РегістриНакопичення.ЗамовленняПостачальникам_RecordsSet замовленняПостачальникам_RecordsSet = new РегістриНакопичення.ЗамовленняПостачальникам_RecordsSet();
			замовленняПостачальникам_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);

			РегістриНакопичення.ТовариНаСкладах_RecordsSet товариНаСкладах_RecordsSet = new РегістриНакопичення.ТовариНаСкладах_RecordsSet();
			товариНаСкладах_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);

			РегістриНакопичення.ПартіїТоварів_RecordsSet партіїТоварів_RecordsSet = new РегістриНакопичення.ПартіїТоварів_RecordsSet();
			партіїТоварів_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);

			РегістриНакопичення.ВільніЗалишки_RecordsSet вільніЗалишки_RecordsSet = new РегістриНакопичення.ВільніЗалишки_RecordsSet();
			вільніЗалишки_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);

			РегістриНакопичення.ТовариДоПоступлення_RecordsSet товариДоПоступлення_RecordsSet = new РегістриНакопичення.ТовариДоПоступлення_RecordsSet();
			товариДоПоступлення_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);

			РегістриНакопичення.РозрахункиЗПостачальниками_RecordsSet розрахункиЗПостачальниками_RecordsSet = new РегістриНакопичення.РозрахункиЗПостачальниками_RecordsSet();
			розрахункиЗПостачальниками_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);

			CalculationBalances.AddTask(ДокументОбєкт.UnigueID.ToString(), ДокументОбєкт.TypeDocument, "Delete", ДокументОбєкт.ДатаДок, "");
		}
	}

	class ЗамовленняПостачальнику_SpendTheDocument
	{
		public static bool Spend(ЗамовленняПостачальнику_Objest ДокументОбєкт)
		{
			if (ДокументОбєкт.Spend)
			{
				//Якщо дата проведення відрізняється від дати документу
				if (ДокументОбєкт.ДатаДок.ToString("dd.MM.yyyy") != ДокументОбєкт.SpendDate.ToString("dd.MM.yyyy"))
					CalculationBalances.AddTask(ДокументОбєкт.UnigueID.ToString(), ДокументОбєкт.TypeDocument, "Delete", ДокументОбєкт.SpendDate, "");
			}

			#region Рух по регістрах

			//
			//Замовлення постачальникам
			//

			РегістриНакопичення.ЗамовленняПостачальникам_RecordsSet замовленняПостачальникам_RecordsSet = new РегістриНакопичення.ЗамовленняПостачальникам_RecordsSet();

			foreach (ЗамовленняПостачальнику_Товари_TablePart.Record Товари_Record in ДокументОбєкт.Товари_TablePart.Records)
			{
				РегістриНакопичення.ЗамовленняПостачальникам_RecordsSet.Record record = new РегістриНакопичення.ЗамовленняПостачальникам_RecordsSet.Record();
				замовленняПостачальникам_RecordsSet.Records.Add(record);

				record.Income = true; // +    | Документ збільшує
				record.Owner = ДокументОбєкт.UnigueID.UGuid;

				record.ЗамовленняПостачальнику = ДокументОбєкт.GetDocumentPointer();
				record.Номенклатура = Товари_Record.Номенклатура;
				record.ХарактеристикаНоменклатури = Товари_Record.ХарактеристикаНоменклатури;
				record.Склад = ДокументОбєкт.Склад;
				record.Замовлено = Товари_Record.Кількість;
			}

			замовленняПостачальникам_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

			//
			//РозрахункиЗПостачальниками
			//

			//РегістриНакопичення.РозрахункиЗПостачальниками_RecordsSet розрахункиЗПостачальниками_RecordsSet = new РегістриНакопичення.РозрахункиЗПостачальниками_RecordsSet();

			//РегістриНакопичення.РозрахункиЗПостачальниками_RecordsSet.Record розрахункиЗПостачальниками_Record = new РегістриНакопичення.РозрахункиЗПостачальниками_RecordsSet.Record();
			//розрахункиЗПостачальниками_RecordsSet.Records.Add(розрахункиЗПостачальниками_Record);

			//розрахункиЗПостачальниками_Record.Income = true; // +
			//розрахункиЗПостачальниками_Record.Owner = ДокументОбєкт.UnigueID.UGuid;

			//розрахункиЗПостачальниками_Record.Контрагент = ДокументОбєкт.Контрагент;
			//розрахункиЗПостачальниками_Record.Валюта = ДокументОбєкт.Валюта;
			//розрахункиЗПостачальниками_Record.Сума = ДокументОбєкт.СумаДокументу;

			//розрахункиЗПостачальниками_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

			//
			//ТовариДоПоступлення
			//

			//РегістриНакопичення.ТовариДоПоступлення_RecordsSet товариДоПоступлення_RecordsSet = new РегістриНакопичення.ТовариДоПоступлення_RecordsSet();

			//foreach (ЗамовленняПостачальнику_Товари_TablePart.Record Товари_Record in ДокументОбєкт.Товари_TablePart.Records)
			//{
			//	РегістриНакопичення.ТовариДоПоступлення_RecordsSet.Record record = new РегістриНакопичення.ТовариДоПоступлення_RecordsSet.Record();
			//	товариДоПоступлення_RecordsSet.Records.Add(record);

			//	record.Income = true; // +    | Документ збільшує
			//	record.Owner = ДокументОбєкт.UnigueID.UGuid;

			//	record.Номенклатура = Товари_Record.Номенклатура;
			//	record.ХарактеристикаНоменклатури = Товари_Record.ХарактеристикаНоменклатури;
			//	record.Склад = ДокументОбєкт.Склад;
			//	record.ДоПоступлення = Товари_Record.Кількість;
			//}

			//товариДоПоступлення_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

			#endregion

			CalculationBalances.AddTask(ДокументОбєкт.UnigueID.ToString(), ДокументОбєкт.TypeDocument, "Add", ДокументОбєкт.ДатаДок, "");

			return true;
		}

		public static void ClearSpend(ЗамовленняПостачальнику_Objest ДокументОбєкт)
		{
			РегістриНакопичення.ЗамовленняПостачальникам_RecordsSet замовленняПостачальникам_RecordsSet = new РегістриНакопичення.ЗамовленняПостачальникам_RecordsSet();
			замовленняПостачальникам_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);

			//РегістриНакопичення.РозрахункиЗПостачальниками_RecordsSet розрахункиЗПостачальниками_RecordsSet = new РегістриНакопичення.РозрахункиЗПостачальниками_RecordsSet();
			//розрахункиЗПостачальниками_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);

			//РегістриНакопичення.ТовариДоПоступлення_RecordsSet товариДоПоступлення_RecordsSet = new РегістриНакопичення.ТовариДоПоступлення_RecordsSet();
			//товариДоПоступлення_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);

			CalculationBalances.AddTask(ДокументОбєкт.UnigueID.ToString(), ДокументОбєкт.TypeDocument, "Delete", ДокументОбєкт.ДатаДок, "");
		}
	}

	class ПоверненняТоварівВідКлієнта_SpendTheDocument
	{
		public static bool Spend(ПоверненняТоварівВідКлієнта_Objest ДокументОбєкт)
		{
			if (ДокументОбєкт.Spend)
			{
				//Якщо дата проведення відрізняється від дати документу
				if (ДокументОбєкт.ДатаДок.ToString("dd.MM.yyyy") != ДокументОбєкт.SpendDate.ToString("dd.MM.yyyy"))
					CalculationBalances.AddTask(ДокументОбєкт.UnigueID.ToString(), ДокументОбєкт.TypeDocument, "Delete", ДокументОбєкт.SpendDate, "");
			}

			#region Рух по регістрах

			//
			//ВільніЗалишки
			//

			РегістриНакопичення.ВільніЗалишки_RecordsSet вільніЗалишки_RecordsSet = new РегістриНакопичення.ВільніЗалишки_RecordsSet();

			foreach (ПоверненняТоварівВідКлієнта_Товари_TablePart.Record Товари_Record in ДокументОбєкт.Товари_TablePart.Records)
			{
				РегістриНакопичення.ВільніЗалишки_RecordsSet.Record record = new РегістриНакопичення.ВільніЗалишки_RecordsSet.Record();
				вільніЗалишки_RecordsSet.Records.Add(record);

				record.Income = true; // +
				record.Owner = ДокументОбєкт.UnigueID.UGuid;

				record.Номенклатура = Товари_Record.Номенклатура;
				record.ХарактеристикаНоменклатури = Товари_Record.ХарактеристикаНоменклатури;
				record.Склад = ДокументОбєкт.Склад;
				record.ВНаявності = Товари_Record.Кількість;
			}

			вільніЗалишки_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

			//
			//Товари на складах
			//

			РегістриНакопичення.ТовариНаСкладах_RecordsSet товариНаСкладах_RecordsSet = new РегістриНакопичення.ТовариНаСкладах_RecordsSet();

			foreach (ПоверненняТоварівВідКлієнта_Товари_TablePart.Record Товари_Record in ДокументОбєкт.Товари_TablePart.Records)
			{
				РегістриНакопичення.ТовариНаСкладах_RecordsSet.Record record = new РегістриНакопичення.ТовариНаСкладах_RecordsSet.Record();
				товариНаСкладах_RecordsSet.Records.Add(record);

				record.Income = true; // +
				record.Owner = ДокументОбєкт.UnigueID.UGuid;

				record.Номенклатура = Товари_Record.Номенклатура;
				record.ХарактеристикаНоменклатури = Товари_Record.ХарактеристикаНоменклатури;
				record.Склад = ДокументОбєкт.Склад;
				record.ВНаявності = Товари_Record.Кількість;
			}

			товариНаСкладах_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

			//
			//РозрахункиЗКлієнтами
			//

			РегістриНакопичення.РозрахункиЗКлієнтами_RecordsSet розрахункиЗКлієнтами_RecordsSet = new РегістриНакопичення.РозрахункиЗКлієнтами_RecordsSet();

			foreach (ПоверненняТоварівВідКлієнта_Товари_TablePart.Record Товари_Record in ДокументОбєкт.Товари_TablePart.Records)
			{
				//З документу реалізації треба витягнути замовлення

				//Товари_Record.ДокументРеалізації.GetDocumentObject().Товари_TablePart.

				РегістриНакопичення.РозрахункиЗКлієнтами_RecordsSet.Record розрахункиЗКлієнтами_Record = new РегістриНакопичення.РозрахункиЗКлієнтами_RecordsSet.Record();
				розрахункиЗКлієнтами_RecordsSet.Records.Add(розрахункиЗКлієнтами_Record);

				розрахункиЗКлієнтами_Record.Income = false;
				розрахункиЗКлієнтами_Record.Owner = ДокументОбєкт.UnigueID.UGuid;

				розрахункиЗКлієнтами_Record.Контрагент = ДокументОбєкт.Контрагент;
				розрахункиЗКлієнтами_Record.Валюта = ДокументОбєкт.Валюта;
				розрахункиЗКлієнтами_Record.Сума = ДокументОбєкт.СумаДокументу;
			}

			розрахункиЗКлієнтами_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

			#endregion

			CalculationBalances.AddTask(ДокументОбєкт.UnigueID.ToString(), ДокументОбєкт.TypeDocument, "Add", ДокументОбєкт.ДатаДок, "");

			return true;
		}

		public static void ClearSpend(ПоверненняТоварівВідКлієнта_Objest ДокументОбєкт)
		{
			РегістриНакопичення.ВільніЗалишки_RecordsSet вільніЗалишки_RecordsSet = new РегістриНакопичення.ВільніЗалишки_RecordsSet();
			вільніЗалишки_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);

			РегістриНакопичення.ТовариНаСкладах_RecordsSet товариНаСкладах_RecordsSet = new РегістриНакопичення.ТовариНаСкладах_RecordsSet();
			товариНаСкладах_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);

			РегістриНакопичення.РозрахункиЗКлієнтами_RecordsSet розрахункиЗКлієнтами_RecordsSet = new РегістриНакопичення.РозрахункиЗКлієнтами_RecordsSet();
			розрахункиЗКлієнтами_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);

			CalculationBalances.AddTask(ДокументОбєкт.UnigueID.ToString(), ДокументОбєкт.TypeDocument, "Delete", ДокументОбєкт.ДатаДок, "");
		}
	}

	class ПоверненняТоварівПостачальнику_SpendTheDocument
	{
		public static bool Spend(ПоверненняТоварівПостачальнику_Objest ДокументОбєкт)
		{
			if (ДокументОбєкт.Spend)
			{
				//Якщо дата проведення відрізняється від дати документу
				if (ДокументОбєкт.ДатаДок.ToString("dd.MM.yyyy") != ДокументОбєкт.SpendDate.ToString("dd.MM.yyyy"))
					CalculationBalances.AddTask(ДокументОбєкт.UnigueID.ToString(), ДокументОбєкт.TypeDocument, "Delete", ДокументОбєкт.SpendDate, "");
			}

			#region Рух по регістрах

			//
			//Товари на складах
			//

			РегістриНакопичення.ТовариНаСкладах_RecordsSet товариНаСкладах_RecordsSet = new РегістриНакопичення.ТовариНаСкладах_RecordsSet();

			foreach (ПоверненняТоварівПостачальнику_Товари_TablePart.Record Товари_Record in ДокументОбєкт.Товари_TablePart.Records)
			{
				РегістриНакопичення.ТовариНаСкладах_RecordsSet.Record record = new РегістриНакопичення.ТовариНаСкладах_RecordsSet.Record();
				товариНаСкладах_RecordsSet.Records.Add(record);

				record.Income = true; 
				record.Owner = ДокументОбєкт.UnigueID.UGuid;

				record.Номенклатура = Товари_Record.Номенклатура;
				record.ХарактеристикаНоменклатури = Товари_Record.ХарактеристикаНоменклатури;
				record.Склад = ДокументОбєкт.Склад;
				record.ВНаявності = Товари_Record.Кількість;
			}

			товариНаСкладах_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

			//
			//ВільніЗалишки
			//

			РегістриНакопичення.ВільніЗалишки_RecordsSet вільніЗалишки_RecordsSet = new РегістриНакопичення.ВільніЗалишки_RecordsSet();

			foreach (ПоверненняТоварівПостачальнику_Товари_TablePart.Record Товари_Record in ДокументОбєкт.Товари_TablePart.Records)
			{
				РегістриНакопичення.ВільніЗалишки_RecordsSet.Record record = new РегістриНакопичення.ВільніЗалишки_RecordsSet.Record();
				вільніЗалишки_RecordsSet.Records.Add(record);

				record.Income = true;
				record.Owner = ДокументОбєкт.UnigueID.UGuid;

				record.Номенклатура = Товари_Record.Номенклатура;
				record.ХарактеристикаНоменклатури = Товари_Record.ХарактеристикаНоменклатури;
				record.Склад = ДокументОбєкт.Склад;
				record.ВНаявності = Товари_Record.Кількість;
			}

			вільніЗалишки_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

			//
			//РозрахункиЗПостачальниками
			//

			РегістриНакопичення.РозрахункиЗПостачальниками_RecordsSet розрахункиЗПостачальниками_RecordsSet = new РегістриНакопичення.РозрахункиЗПостачальниками_RecordsSet();
			foreach (ПоверненняТоварівПостачальнику_Товари_TablePart.Record Товари_Record in ДокументОбєкт.Товари_TablePart.Records)
			{
				//Знайти замовлення посатчальнику в таб. Товари_Record.ДокументПоступлення

				РегістриНакопичення.РозрахункиЗПостачальниками_RecordsSet.Record розрахункиЗПостачальниками_Record = new РегістриНакопичення.РозрахункиЗПостачальниками_RecordsSet.Record();
				розрахункиЗПостачальниками_RecordsSet.Records.Add(розрахункиЗПостачальниками_Record);

				розрахункиЗПостачальниками_Record.Income = false;
				розрахункиЗПостачальниками_Record.Owner = ДокументОбєкт.UnigueID.UGuid;

				//розрахункиЗПостачальниками_Record.ЗамовленняПостачальнику = Товари_Record.ДокументПоступлення;
				розрахункиЗПостачальниками_Record.Валюта = ДокументОбєкт.Валюта;
				розрахункиЗПостачальниками_Record.Сума = ДокументОбєкт.СумаДокументу;
			}

			розрахункиЗПостачальниками_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

			#endregion

			CalculationBalances.AddTask(ДокументОбєкт.UnigueID.ToString(), ДокументОбєкт.TypeDocument, "Add", ДокументОбєкт.ДатаДок, "");

			return true;
		}

		public static void ClearSpend(ПоверненняТоварівПостачальнику_Objest ДокументОбєкт)
		{
			РегістриНакопичення.ТовариНаСкладах_RecordsSet товариНаСкладах_RecordsSet = new РегістриНакопичення.ТовариНаСкладах_RecordsSet();
			товариНаСкладах_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);

			РегістриНакопичення.ВільніЗалишки_RecordsSet вільніЗалишки_RecordsSet = new РегістриНакопичення.ВільніЗалишки_RecordsSet();
			вільніЗалишки_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);

			РегістриНакопичення.РозрахункиЗПостачальниками_RecordsSet розрахункиЗПостачальниками_RecordsSet = new РегістриНакопичення.РозрахункиЗПостачальниками_RecordsSet();
			розрахункиЗПостачальниками_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);

			CalculationBalances.AddTask(ДокументОбєкт.UnigueID.ToString(), ДокументОбєкт.TypeDocument, "Delete", ДокументОбєкт.ДатаДок, "");
		}
	}

	class ПрихіднийКасовийОрдер_SpendTheDocument
	{
		public static bool Spend(ПрихіднийКасовийОрдер_Objest ДокументОбєкт)
		{
			if (ДокументОбєкт.Spend)
			{
				//Якщо дата проведення відрізняється від дати документу
				if (ДокументОбєкт.ДатаДок.ToString("dd.MM.yyyy") != ДокументОбєкт.SpendDate.ToString("dd.MM.yyyy"))
					CalculationBalances.AddTask(ДокументОбєкт.UnigueID.ToString(), ДокументОбєкт.TypeDocument, "Delete", ДокументОбєкт.SpendDate, "");
			}

			#region Рух по регістрах

			//
			//РозрахункиЗКлієнтами
			//

			РегістриНакопичення.РозрахункиЗКлієнтами_RecordsSet розрахункиЗКлієнтами_RecordsSet = new РегістриНакопичення.РозрахункиЗКлієнтами_RecordsSet();

			if (ДокументОбєкт.ГосподарськаОперація == Перелічення.ГосподарськіОперації.ПоступленняОплатиВідКлієнта)
			{
				РегістриНакопичення.РозрахункиЗКлієнтами_RecordsSet.Record record_Клієнт = new РегістриНакопичення.РозрахункиЗКлієнтами_RecordsSet.Record();
				розрахункиЗКлієнтами_RecordsSet.Records.Add(record_Клієнт);

				record_Клієнт.Income = false;
				record_Клієнт.Owner = ДокументОбєкт.UnigueID.UGuid;

				record_Клієнт.Контрагент = ДокументОбєкт.Контрагент;
				record_Клієнт.Валюта = ДокументОбєкт.Валюта;
				record_Клієнт.Сума = ДокументОбєкт.СумаДокументу;
			}

			розрахункиЗКлієнтами_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

			//
			//РозрахункиЗПостачальниками
			//

			РегістриНакопичення.РозрахункиЗПостачальниками_RecordsSet розрахункиЗПостачальниками_RecordsSet = new РегістриНакопичення.РозрахункиЗПостачальниками_RecordsSet();

			if (ДокументОбєкт.ГосподарськаОперація == Перелічення.ГосподарськіОперації.ПоверненняКоштівПостачальнику)
			{
				РегістриНакопичення.РозрахункиЗПостачальниками_RecordsSet.Record record_Постачальник = new РегістриНакопичення.РозрахункиЗПостачальниками_RecordsSet.Record();
				розрахункиЗПостачальниками_RecordsSet.Records.Add(record_Постачальник);

				record_Постачальник.Income = false;
				record_Постачальник.Owner = ДокументОбєкт.UnigueID.UGuid;

				record_Постачальник.Контрагент = ДокументОбєкт.Контрагент;
				record_Постачальник.Валюта = ДокументОбєкт.Валюта;
				record_Постачальник.Сума = ДокументОбєкт.СумаДокументу;
			}

			розрахункиЗПостачальниками_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

			//
			//РухКоштів
			//

			РегістриНакопичення.РухКоштів_RecordsSet рухКоштів_RecordsSet = new РегістриНакопичення.РухКоштів_RecordsSet();

			//Списання коштів з КасаВідправник
			if (ДокументОбєкт.ГосподарськаОперація == Перелічення.ГосподарськіОперації.ПоступленняКоштівЗІншоїКаси)
			{
				РегістриНакопичення.РухКоштів_RecordsSet.Record record_ІншаКаса = new РегістриНакопичення.РухКоштів_RecordsSet.Record();
				рухКоштів_RecordsSet.Records.Add(record_ІншаКаса);

				record_ІншаКаса.Income = false;
				record_ІншаКаса.Owner = ДокументОбєкт.UnigueID.UGuid;

				record_ІншаКаса.Організація = ДокументОбєкт.Організація;
				record_ІншаКаса.Каса = ДокументОбєкт.КасаВідправник;
				record_ІншаКаса.Валюта = ДокументОбєкт.Валюта;
				record_ІншаКаса.Сума = ДокументОбєкт.СумаДокументу;
			}

			//Списання коштів з банківського рахунку
			if (ДокументОбєкт.ГосподарськаОперація == Перелічення.ГосподарськіОперації.ПоступленняКоштівЗБанку)
			{
				// ...
			}

			//Поступлення коштів в касу
			РегістриНакопичення.РухКоштів_RecordsSet.Record record_РухКоштів = new РегістриНакопичення.РухКоштів_RecordsSet.Record();
			рухКоштів_RecordsSet.Records.Add(record_РухКоштів);

			record_РухКоштів.Income = true;
			record_РухКоштів.Owner = ДокументОбєкт.UnigueID.UGuid;

			record_РухКоштів.Організація = ДокументОбєкт.Організація;
			record_РухКоштів.Каса = ДокументОбєкт.Каса;
			record_РухКоштів.Валюта = ДокументОбєкт.Валюта;
			record_РухКоштів.Сума = ДокументОбєкт.СумаДокументу;

			рухКоштів_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

			#endregion

			CalculationBalances.AddTask(ДокументОбєкт.UnigueID.ToString(), ДокументОбєкт.TypeDocument, "Add", ДокументОбєкт.ДатаДок, "");

			return true;
		}

		public static void ClearSpend(ПрихіднийКасовийОрдер_Objest ДокументОбєкт)
		{
			РегістриНакопичення.РозрахункиЗПостачальниками_RecordsSet розрахункиЗПостачальниками_RecordsSet = new РегістриНакопичення.РозрахункиЗПостачальниками_RecordsSet();
			розрахункиЗПостачальниками_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);

			РегістриНакопичення.РозрахункиЗКлієнтами_RecordsSet розрахункиЗКлієнтами_RecordsSet = new РегістриНакопичення.РозрахункиЗКлієнтами_RecordsSet();
			розрахункиЗКлієнтами_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);

			РегістриНакопичення.РухКоштів_RecordsSet рухКоштів_RecordsSet = new РегістриНакопичення.РухКоштів_RecordsSet();
			рухКоштів_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);

			CalculationBalances.AddTask(ДокументОбєкт.UnigueID.ToString(), ДокументОбєкт.TypeDocument, "Delete", ДокументОбєкт.ДатаДок, "");
		}
	}

	class РозхіднийКасовийОрдер_SpendTheDocument
	{
		public static bool Spend(РозхіднийКасовийОрдер_Objest ДокументОбєкт)
		{
			if (ДокументОбєкт.Spend)
			{
				//Якщо дата проведення відрізняється від дати документу
				if (ДокументОбєкт.ДатаДок.ToString("dd.MM.yyyy") != ДокументОбєкт.SpendDate.ToString("dd.MM.yyyy"))
					CalculationBalances.AddTask(ДокументОбєкт.UnigueID.ToString(), ДокументОбєкт.TypeDocument, "Delete", ДокументОбєкт.SpendDate, "");
			}

			#region Рух по регістрах

			//
			//РозрахункиЗКлієнтами
			//

			РегістриНакопичення.РозрахункиЗКлієнтами_RecordsSet розрахункиЗКлієнтами_RecordsSet = new РегістриНакопичення.РозрахункиЗКлієнтами_RecordsSet();

			if (ДокументОбєкт.ГосподарськаОперація == Перелічення.ГосподарськіОперації.ПоверненняОплатиКлієнту)
			{
				РегістриНакопичення.РозрахункиЗКлієнтами_RecordsSet.Record record_Клієнт = new РегістриНакопичення.РозрахункиЗКлієнтами_RecordsSet.Record();
				розрахункиЗКлієнтами_RecordsSet.Records.Add(record_Клієнт);

				record_Клієнт.Income = false;
				record_Клієнт.Owner = ДокументОбєкт.UnigueID.UGuid;

				record_Клієнт.Контрагент = ДокументОбєкт.Контрагент;
				record_Клієнт.Валюта = ДокументОбєкт.Валюта;
				record_Клієнт.Сума = ДокументОбєкт.СумаДокументу;
			}

			розрахункиЗКлієнтами_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

			//
			//РозрахункиЗПостачальниками
			//

			РегістриНакопичення.РозрахункиЗПостачальниками_RecordsSet розрахункиЗПостачальниками_RecordsSet = new РегістриНакопичення.РозрахункиЗПостачальниками_RecordsSet();

			if (ДокументОбєкт.ГосподарськаОперація == Перелічення.ГосподарськіОперації.ОплатаПостачальнику)
			{
				РегістриНакопичення.РозрахункиЗПостачальниками_RecordsSet.Record record_Постачальник = new РегістриНакопичення.РозрахункиЗПостачальниками_RecordsSet.Record();
				розрахункиЗПостачальниками_RecordsSet.Records.Add(record_Постачальник);

				record_Постачальник.Income = true;
				record_Постачальник.Owner = ДокументОбєкт.UnigueID.UGuid;

				record_Постачальник.Контрагент = ДокументОбєкт.Контрагент;
				record_Постачальник.Валюта = ДокументОбєкт.Валюта;
				record_Постачальник.Сума = ДокументОбєкт.СумаДокументу;
			}

			розрахункиЗПостачальниками_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

			//
			//РухКоштів
			//

			РегістриНакопичення.РухКоштів_RecordsSet рухКоштів_RecordsSet = new РегістриНакопичення.РухКоштів_RecordsSet();

			//Поступлення коштів в КасаОтримувач
			if (ДокументОбєкт.ГосподарськаОперація == Перелічення.ГосподарськіОперації.ВидачаКоштівВІншуКасу)
			{
				РегістриНакопичення.РухКоштів_RecordsSet.Record record_ІншаКаса = new РегістриНакопичення.РухКоштів_RecordsSet.Record();
				рухКоштів_RecordsSet.Records.Add(record_ІншаКаса);

				record_ІншаКаса.Income = true;
				record_ІншаКаса.Owner = ДокументОбєкт.UnigueID.UGuid;

				record_ІншаКаса.Організація = ДокументОбєкт.Організація;
				record_ІншаКаса.Каса = ДокументОбєкт.КасаОтримувач;
				record_ІншаКаса.Валюта = ДокументОбєкт.Валюта;
				record_ІншаКаса.Сума = ДокументОбєкт.СумаДокументу;
			}

			//Поступлення коштів на банківський рахунок
			if (ДокументОбєкт.ГосподарськаОперація == Перелічення.ГосподарськіОперації.ЗдачаКоштівВБанк)
			{
				// ...
			}

			//Списання коштів з каси
			РегістриНакопичення.РухКоштів_RecordsSet.Record record_РухКоштів = new РегістриНакопичення.РухКоштів_RecordsSet.Record();
			рухКоштів_RecordsSet.Records.Add(record_РухКоштів);

			record_РухКоштів.Income = false;
			record_РухКоштів.Owner = ДокументОбєкт.UnigueID.UGuid;

			record_РухКоштів.Організація = ДокументОбєкт.Організація;
			record_РухКоштів.Каса = ДокументОбєкт.Каса;
			record_РухКоштів.Валюта = ДокументОбєкт.Валюта;
			record_РухКоштів.Сума = ДокументОбєкт.СумаДокументу;

			рухКоштів_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

			#endregion

			CalculationBalances.AddTask(ДокументОбєкт.UnigueID.ToString(), ДокументОбєкт.TypeDocument, "Add", ДокументОбєкт.ДатаДок, "");

			return true;
		}

		public static void ClearSpend(РозхіднийКасовийОрдер_Objest ДокументОбєкт)
		{
			РегістриНакопичення.РозрахункиЗПостачальниками_RecordsSet розрахункиЗПостачальниками_RecordsSet = new РегістриНакопичення.РозрахункиЗПостачальниками_RecordsSet();
			розрахункиЗПостачальниками_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);

			РегістриНакопичення.РозрахункиЗКлієнтами_RecordsSet розрахункиЗКлієнтами_RecordsSet = new РегістриНакопичення.РозрахункиЗКлієнтами_RecordsSet();
			розрахункиЗКлієнтами_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);

			РегістриНакопичення.РухКоштів_RecordsSet рухКоштів_RecordsSet = new РегістриНакопичення.РухКоштів_RecordsSet();
			рухКоштів_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);

			CalculationBalances.AddTask(ДокументОбєкт.UnigueID.ToString(), ДокументОбєкт.TypeDocument, "Delete", ДокументОбєкт.ДатаДок, "");
		}
	}

	class ПереміщенняТоварів_SpendTheDocument
	{
		public static bool Spend(ПереміщенняТоварів_Objest ДокументОбєкт)
		{
			if (ДокументОбєкт.Spend)
			{
				//Якщо дата проведення відрізняється від дати документу
				if (ДокументОбєкт.ДатаДок.ToString("dd.MM.yyyy") != ДокументОбєкт.SpendDate.ToString("dd.MM.yyyy"))
					CalculationBalances.AddTask(ДокументОбєкт.UnigueID.ToString(), ДокументОбєкт.TypeDocument, "Delete", ДокументОбєкт.SpendDate, "");
			}

			#region Рух по регістрах

			//
			//Товари на складах
			//

			РегістриНакопичення.ТовариНаСкладах_RecordsSet товариНаСкладах_RecordsSet = new РегістриНакопичення.ТовариНаСкладах_RecordsSet();

			foreach (ПереміщенняТоварів_Товари_TablePart.Record Товари_Record in ДокументОбєкт.Товари_TablePart.Records)
			{
				Довідники.Номенклатура_Objest номенклатура_Objest = Товари_Record.Номенклатура.GetDirectoryObject();

				//Товар
				if (номенклатура_Objest.ТипНоменклатури == Перелічення.ТипиНоменклатури.Товар)
				{
					//
					//СкладВідправник
					//

					РегістриНакопичення.ТовариНаСкладах_RecordsSet.Record record1 = new РегістриНакопичення.ТовариНаСкладах_RecordsSet.Record();
					товариНаСкладах_RecordsSet.Records.Add(record1);

					record1.Income = false; // - 
					record1.Owner = ДокументОбєкт.UnigueID.UGuid;

					record1.Номенклатура = Товари_Record.Номенклатура;
					record1.ХарактеристикаНоменклатури = Товари_Record.ХарактеристикаНоменклатури;
					record1.Склад = ДокументОбєкт.СкладВідправник;
					record1.Серія = Товари_Record.Серія;
					record1.ВНаявності = Товари_Record.Кількість;

					//
					//СкладОтримувач
					//

					РегістриНакопичення.ТовариНаСкладах_RecordsSet.Record record2 = new РегістриНакопичення.ТовариНаСкладах_RecordsSet.Record();
					товариНаСкладах_RecordsSet.Records.Add(record2);

					record2.Income = true; // - 
					record2.Owner = ДокументОбєкт.UnigueID.UGuid;

					record2.Номенклатура = Товари_Record.Номенклатура;
					record2.ХарактеристикаНоменклатури = Товари_Record.ХарактеристикаНоменклатури;
					record2.Склад = ДокументОбєкт.СкладОтримувач;
					record2.Серія = Товари_Record.Серія;
					record2.ВНаявності = Товари_Record.Кількість;
				}
			}

			товариНаСкладах_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

			#endregion

			CalculationBalances.AddTask(ДокументОбєкт.UnigueID.ToString(), ДокументОбєкт.TypeDocument, "Add", ДокументОбєкт.ДатаДок, "");

			return true;
		}

		public static void ClearSpend(ПереміщенняТоварів_Objest ДокументОбєкт)
		{
			РегістриНакопичення.ТовариНаСкладах_RecordsSet товариНаСкладах_RecordsSet = new РегістриНакопичення.ТовариНаСкладах_RecordsSet();
			товариНаСкладах_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);

			CalculationBalances.AddTask(ДокументОбєкт.UnigueID.ToString(), ДокументОбєкт.TypeDocument, "Delete", ДокументОбєкт.ДатаДок, "");
		}
	}

	class ВстановленняЦінНоменклатури_SpendTheDocument
	{
		public static bool Spend(ВстановленняЦінНоменклатури_Objest ДокументОбєкт)
		{
			#region Рух по регістрах

			РегістриВідомостей.ЦіниНоменклатури_RecordsSet ціниНоменклатури_RecordsSet = new РегістриВідомостей.ЦіниНоменклатури_RecordsSet();

            foreach (ВстановленняЦінНоменклатури_Товари_TablePart.Record Товари_Record in ДокументОбєкт.Товари_TablePart.Records)
            {
				Довідники.ВидиЦін_Objest видиЦін_Objest = Товари_Record.ВидЦіни.GetDirectoryObject();

				РегістриВідомостей.ЦіниНоменклатури_RecordsSet.Record record = new РегістриВідомостей.ЦіниНоменклатури_RecordsSet.Record();
                ціниНоменклатури_RecordsSet.Records.Add(record);

				record.Номенклатура = Товари_Record.Номенклатура;
				record.ХарактеристикаНоменклатури = Товари_Record.ХарактеристикаНоменклатури;
				record.ВидЦіни = Товари_Record.ВидЦіни;

				record.Ціна = Товари_Record.Ціна;
				record.Пакування = Товари_Record.Пакування;
				record.Валюта = (видиЦін_Objest != null ? видиЦін_Objest.Валюта : new Довідники.Валюти_Pointer());
			}

			ціниНоменклатури_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

			#endregion

			return true;
		}

		public static void ClearSpend(ВстановленняЦінНоменклатури_Objest ДокументОбєкт)
		{
			РегістриВідомостей.ЦіниНоменклатури_RecordsSet ціниНоменклатури_RecordsSet = new РегістриВідомостей.ЦіниНоменклатури_RecordsSet();
			ціниНоменклатури_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);
		}
	}

	class ВведенняЗалишків_SpendTheDocument
	{
		public static bool Spend(ВведенняЗалишків_Objest ДокументОбєкт)
		{
			if (ДокументОбєкт.Spend)
			{
				//Якщо дата проведення відрізняється від дати документу
				if (ДокументОбєкт.ДатаДок.ToString("dd.MM.yyyy") != ДокументОбєкт.SpendDate.ToString("dd.MM.yyyy"))
					CalculationBalances.AddTask(ДокументОбєкт.UnigueID.ToString(), ДокументОбєкт.TypeDocument, "Delete", ДокументОбєкт.SpendDate, "");
			}

			#region Рух по регістрах

			//
			//Товари на складах
			//

			РегістриНакопичення.ТовариНаСкладах_RecordsSet товариНаСкладах_RecordsSet = new РегістриНакопичення.ТовариНаСкладах_RecordsSet();

			foreach (ВведенняЗалишків_Товари_TablePart.Record Товари_Record in ДокументОбєкт.Товари_TablePart.Records)
			{
				РегістриНакопичення.ТовариНаСкладах_RecordsSet.Record record = new РегістриНакопичення.ТовариНаСкладах_RecordsSet.Record();
				товариНаСкладах_RecordsSet.Records.Add(record);

				record.Income = true; //
				record.Owner = ДокументОбєкт.UnigueID.UGuid;

				record.Номенклатура = Товари_Record.Номенклатура;
				record.ХарактеристикаНоменклатури = Товари_Record.ХарактеристикаНоменклатури;
				record.Серія = Товари_Record.Серія;
				record.Склад = ДокументОбєкт.Склад;
				record.ВНаявності = Товари_Record.Кількість;
			}

			товариНаСкладах_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

			//
			//Партії товарів
			//

			ПартіїТоварів_RecordsSet партіїТоварів_RecordsSet = new ПартіїТоварів_RecordsSet();

			foreach (ВведенняЗалишків_Товари_TablePart.Record Товари_Record in ДокументОбєкт.Товари_TablePart.Records)
			{
				ПартіїТоварів_RecordsSet.Record record = new ПартіїТоварів_RecordsSet.Record();
				партіїТоварів_RecordsSet.Records.Add(record);

				record.Income = true; // + 
				record.Owner = ДокументОбєкт.UnigueID.UGuid;

				record.Організація = ДокументОбєкт.Організація;
				record.ДокументПоступлення = new ПоступленняТоварівТаПослуг_Pointer();
				record.Кількість = Товари_Record.Кількість;
				record.Собівартість = Товари_Record.Ціна;
				record.Номенклатура = Товари_Record.Номенклатура;
				record.ХарактеристикаНоменклатури = Товари_Record.ХарактеристикаНоменклатури;
				record.Серія = Товари_Record.Серія;
				record.Документ = new UuidAndText(ДокументОбєкт.UnigueID.UGuid, ДокументОбєкт.Table);
				
			}

			партіїТоварів_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

			//
			//РухКоштів
			//

			РегістриНакопичення.РухКоштів_RecordsSet рухКоштів_RecordsSet = new РегістриНакопичення.РухКоштів_RecordsSet();

			foreach (ВведенняЗалишків_Каси_TablePart.Record Каси_Record in ДокументОбєкт.Каси_TablePart.Records)
			{
				РегістриНакопичення.РухКоштів_RecordsSet.Record record_Каса = new РегістриНакопичення.РухКоштів_RecordsSet.Record();
				рухКоштів_RecordsSet.Records.Add(record_Каса);

				Довідники.Валюти_Pointer валютаКаси =
					(!Каси_Record.Каса.IsEmpty() ? Каси_Record.Каса.GetDirectoryObject().Валюта : ДокументОбєкт.Валюта);

				record_Каса.Income = true;
				record_Каса.Owner = ДокументОбєкт.UnigueID.UGuid;

				record_Каса.Організація = ДокументОбєкт.Організація;
				record_Каса.Каса = Каси_Record.Каса;
				record_Каса.Валюта = валютаКаси;
				record_Каса.Сума = Каси_Record.Сума;
			}

			рухКоштів_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

			#endregion

			CalculationBalances.AddTask(ДокументОбєкт.UnigueID.ToString(), ДокументОбєкт.TypeDocument, "Add", ДокументОбєкт.ДатаДок, "");

			return true;
		}

		public static void ClearSpend(ВведенняЗалишків_Objest ДокументОбєкт)
		{
			РегістриНакопичення.ТовариНаСкладах_RecordsSet товариНаСкладах_RecordsSet = new РегістриНакопичення.ТовариНаСкладах_RecordsSet();
			товариНаСкладах_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);

			РегістриНакопичення.ПартіїТоварів_RecordsSet партіїТоварів_RecordsSet = new РегістриНакопичення.ПартіїТоварів_RecordsSet();
			партіїТоварів_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);

			РегістриНакопичення.РухКоштів_RecordsSet рухКоштів_RecordsSet = new РегістриНакопичення.РухКоштів_RecordsSet();
			рухКоштів_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);

			CalculationBalances.AddTask(ДокументОбєкт.UnigueID.ToString(), ДокументОбєкт.TypeDocument, "Delete", ДокументОбєкт.ДатаДок, "");
		}
	}

	class ВнутрішнєСпоживанняТоварів_SpendTheDocument
	{
		public static bool Spend(ВнутрішнєСпоживанняТоварів_Objest ДокументОбєкт)
		{
			if (ДокументОбєкт.Spend)
			{
				//Якщо дата проведення відрізняється від дати документу
				if (ДокументОбєкт.ДатаДок.ToString("dd.MM.yyyy") != ДокументОбєкт.SpendDate.ToString("dd.MM.yyyy"))
					CalculationBalances.AddTask(ДокументОбєкт.UnigueID.ToString(), ДокументОбєкт.TypeDocument, "Delete", ДокументОбєкт.SpendDate, "");
			}

			#region Рух по регістрах

			//
			//Товари на складах
			//

			ТовариНаСкладах_RecordsSet товариНаСкладах_RecordsSet = new ТовариНаСкладах_RecordsSet();

			foreach (ВнутрішнєСпоживанняТоварів_Товари_TablePart.Record Товари_Record in ДокументОбєкт.Товари_TablePart.Records)
			{
				ТовариНаСкладах_RecordsSet.Record record = new ТовариНаСкладах_RecordsSet.Record();
				товариНаСкладах_RecordsSet.Records.Add(record);

				record.Income = false; //
				record.Owner = ДокументОбєкт.UnigueID.UGuid;

				record.Номенклатура = Товари_Record.Номенклатура;
				record.ХарактеристикаНоменклатури = Товари_Record.ХарактеристикаНоменклатури;
				record.Серія = Товари_Record.Серія;
				record.Склад = ДокументОбєкт.Склад;
				record.ВНаявності = Товари_Record.Кількість;
			}

			товариНаСкладах_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

			#endregion

			CalculationBalances.AddTask(ДокументОбєкт.UnigueID.ToString(), ДокументОбєкт.TypeDocument, "Add", ДокументОбєкт.ДатаДок, "");

			return true;
		}

		public static void ClearSpend(ВнутрішнєСпоживанняТоварів_Objest ДокументОбєкт)
		{
			ТовариНаСкладах_RecordsSet товариНаСкладах_RecordsSet = new ТовариНаСкладах_RecordsSet();
			товариНаСкладах_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);

			CalculationBalances.AddTask(ДокументОбєкт.UnigueID.ToString(), ДокументОбєкт.TypeDocument, "Delete", ДокументОбєкт.ДатаДок, "");
		}
	}
}
