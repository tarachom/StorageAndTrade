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
					Function.AddBackgroundTask_CalculationVirtualBalances(ДокументОбєкт.UnigueID.ToString(), ДокументОбєкт.TypeDocument, "Delete", ДокументОбєкт.SpendDate, "");
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

			Function.AddBackgroundTask_CalculationVirtualBalances(ДокументОбєкт.UnigueID.ToString(), ДокументОбєкт.TypeDocument, "Add", ДокументОбєкт.ДатаДок, "");

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

			Function.AddBackgroundTask_CalculationVirtualBalances(ДокументОбєкт.UnigueID.ToString(), ДокументОбєкт.TypeDocument, "Delete", ДокументОбєкт.ДатаДок, "");
		}
	}

	class РеалізаціяТоварівТаПослуг_SpendTheDocument
	{
		public static bool Spend(РеалізаціяТоварівТаПослуг_Objest ДокументОбєкт)
		{
			if (ДокументОбєкт.Spend)
			{
				//Якщо дата проведення відрізняється від дати документу
				if (ДокументОбєкт.ДатаДок.ToString("dd.MM.yyyy") != ДокументОбєкт.SpendDate.ToString("dd.MM.yyyy"))
					Function.AddBackgroundTask_CalculationVirtualBalances(ДокументОбєкт.UnigueID.ToString(), ДокументОбєкт.TypeDocument, "Delete", ДокументОбєкт.SpendDate, "");
			}

			#region Рух по регістрах

			//
			//ЗамовленняКлієнтів
			//

			РегістриНакопичення.ЗамовленняКлієнтів_RecordsSet замовленняКлієнтів_RecordsSet = new РегістриНакопичення.ЗамовленняКлієнтів_RecordsSet();

			ДокументОбєкт.Товари_TablePart.Read();

			foreach (РеалізаціяТоварівТаПослуг_Товари_TablePart.Record Товари_Record in ДокументОбєкт.Товари_TablePart.Records)
			{
				if (!Товари_Record.ЗамовленняКлієнта.IsEmpty())
				{
					РегістриНакопичення.ЗамовленняКлієнтів_RecordsSet.Record record = new РегістриНакопичення.ЗамовленняКлієнтів_RecordsSet.Record();
					замовленняКлієнтів_RecordsSet.Records.Add(record);

					record.Income = false; // -     | Документ зменшує замовлення
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

			//
			//ВільніЗалишки
			//

			РегістриНакопичення.ВільніЗалишки_RecordsSet вільніЗалишки_RecordsSet = new РегістриНакопичення.ВільніЗалишки_RecordsSet();

			foreach (РеалізаціяТоварівТаПослуг_Товари_TablePart.Record Товари_Record in ДокументОбєкт.Товари_TablePart.Records)
			{
				РегістриНакопичення.ВільніЗалишки_RecordsSet.Record record = new РегістриНакопичення.ВільніЗалишки_RecordsSet.Record();
				вільніЗалишки_RecordsSet.Records.Add(record);

				record.Income = false; // -      | Документ зменшує резерв
				record.Owner = ДокументОбєкт.UnigueID.UGuid;

				record.Номенклатура = Товари_Record.Номенклатура;
				record.ХарактеристикаНоменклатури = Товари_Record.ХарактеристикаНоменклатури;
				record.Склад = (!Товари_Record.Склад.IsEmpty() ? Товари_Record.Склад : ДокументОбєкт.Склад);
				record.ВРезервіПідЗамовлення = Товари_Record.Кількість;
			}

			вільніЗалишки_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

			//
			//Товари на складах
			//

			РегістриНакопичення.ТовариНаСкладах_RecordsSet товариНаСкладах_RecordsSet = new РегістриНакопичення.ТовариНаСкладах_RecordsSet();

			foreach (РеалізаціяТоварівТаПослуг_Товари_TablePart.Record Товари_Record in ДокументОбєкт.Товари_TablePart.Records)
			{
				РегістриНакопичення.ТовариНаСкладах_RecordsSet.Record record = new РегістриНакопичення.ТовариНаСкладах_RecordsSet.Record();
				товариНаСкладах_RecordsSet.Records.Add(record);

				record.Income = false; // -      | Документ зменшує наявність
				record.Owner = ДокументОбєкт.UnigueID.UGuid;

				record.Номенклатура = Товари_Record.Номенклатура;
				record.ХарактеристикаНоменклатури = Товари_Record.ХарактеристикаНоменклатури;
				record.Склад = (!Товари_Record.Склад.IsEmpty() ? Товари_Record.Склад : ДокументОбєкт.Склад);
				record.ВНаявності = Товари_Record.Кількість;
			}

			товариНаСкладах_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

			//??? Договір

			//
			//РозрахункиЗКлієнтами
			//

			РегістриНакопичення.РозрахункиЗКлієнтами_RecordsSet розрахункиЗКлієнтами_RecordsSet = new РегістриНакопичення.РозрахункиЗКлієнтами_RecordsSet();

			РегістриНакопичення.РозрахункиЗКлієнтами_RecordsSet.Record розрахункиЗКлієнтами_Record = new РегістриНакопичення.РозрахункиЗКлієнтами_RecordsSet.Record();
			розрахункиЗКлієнтами_RecordsSet.Records.Add(розрахункиЗКлієнтами_Record);

			розрахункиЗКлієнтами_Record.Income = true; // +       | Документ збільшує борг клієнта 
			розрахункиЗКлієнтами_Record.Owner = ДокументОбєкт.UnigueID.UGuid;

			розрахункиЗКлієнтами_Record.Контрагент = ДокументОбєкт.Контрагент;
			розрахункиЗКлієнтами_Record.Валюта = ДокументОбєкт.Валюта;
			розрахункиЗКлієнтами_Record.Сума = ДокументОбєкт.СумаДокументу;

			розрахункиЗКлієнтами_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

			#endregion

			Function.AddBackgroundTask_CalculationVirtualBalances(ДокументОбєкт.UnigueID.ToString(), ДокументОбєкт.TypeDocument, "Add", ДокументОбєкт.ДатаДок, "");

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

			РегістриНакопичення.РозрахункиЗКлієнтами_RecordsSet розрахункиЗКлієнтами_RecordsSet = new РегістриНакопичення.РозрахункиЗКлієнтами_RecordsSet();
			розрахункиЗКлієнтами_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);

			Function.AddBackgroundTask_CalculationVirtualBalances(ДокументОбєкт.UnigueID.ToString(), ДокументОбєкт.TypeDocument, "Delete", ДокументОбєкт.ДатаДок, "");
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
					Function.AddBackgroundTask_CalculationVirtualBalances(ДокументОбєкт.UnigueID.ToString(), ДокументОбєкт.TypeDocument, "Delete", ДокументОбєкт.SpendDate, "");
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
					record.ВНаявності = Товари_Record.Кількість;
				}
			}

			товариНаСкладах_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

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

            РегістриНакопичення.ТовариДоПоступлення_RecordsSet товариДоПоступлення_RecordsSet = new РегістриНакопичення.ТовариДоПоступлення_RecordsSet();

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

            товариДоПоступлення_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

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

			Function.AddBackgroundTask_CalculationVirtualBalances(ДокументОбєкт.UnigueID.ToString(), ДокументОбєкт.TypeDocument, "Add", ДокументОбєкт.ДатаДок, "");

			return true;
		}

		public static void ClearSpend(ПоступленняТоварівТаПослуг_Objest ДокументОбєкт)
		{
			РегістриНакопичення.ЗамовленняПостачальникам_RecordsSet замовленняПостачальникам_RecordsSet = new РегістриНакопичення.ЗамовленняПостачальникам_RecordsSet();
			замовленняПостачальникам_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);

			РегістриНакопичення.ТовариНаСкладах_RecordsSet товариНаСкладах_RecordsSet = new РегістриНакопичення.ТовариНаСкладах_RecordsSet();
			товариНаСкладах_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);

			РегістриНакопичення.ВільніЗалишки_RecordsSet вільніЗалишки_RecordsSet = new РегістриНакопичення.ВільніЗалишки_RecordsSet();
			вільніЗалишки_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);

			РегістриНакопичення.ТовариДоПоступлення_RecordsSet товариДоПоступлення_RecordsSet = new РегістриНакопичення.ТовариДоПоступлення_RecordsSet();
			товариДоПоступлення_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);

			РегістриНакопичення.РозрахункиЗПостачальниками_RecordsSet розрахункиЗПостачальниками_RecordsSet = new РегістриНакопичення.РозрахункиЗПостачальниками_RecordsSet();
			розрахункиЗПостачальниками_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);

			Function.AddBackgroundTask_CalculationVirtualBalances(ДокументОбєкт.UnigueID.ToString(), ДокументОбєкт.TypeDocument, "Delete", ДокументОбєкт.ДатаДок, "");
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
					Function.AddBackgroundTask_CalculationVirtualBalances(ДокументОбєкт.UnigueID.ToString(), ДокументОбєкт.TypeDocument, "Delete", ДокументОбєкт.SpendDate, "");
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

			Function.AddBackgroundTask_CalculationVirtualBalances(ДокументОбєкт.UnigueID.ToString(), ДокументОбєкт.TypeDocument, "Add", ДокументОбєкт.ДатаДок, "");

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

			Function.AddBackgroundTask_CalculationVirtualBalances(ДокументОбєкт.UnigueID.ToString(), ДокументОбєкт.TypeDocument, "Delete", ДокументОбєкт.ДатаДок, "");
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
					Function.AddBackgroundTask_CalculationVirtualBalances(ДокументОбєкт.UnigueID.ToString(), ДокументОбєкт.TypeDocument, "Delete", ДокументОбєкт.SpendDate, "");
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

			Function.AddBackgroundTask_CalculationVirtualBalances(ДокументОбєкт.UnigueID.ToString(), ДокументОбєкт.TypeDocument, "Add", ДокументОбєкт.ДатаДок, "");

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

			Function.AddBackgroundTask_CalculationVirtualBalances(ДокументОбєкт.UnigueID.ToString(), ДокументОбєкт.TypeDocument, "Delete", ДокументОбєкт.ДатаДок, "");
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
					Function.AddBackgroundTask_CalculationVirtualBalances(ДокументОбєкт.UnigueID.ToString(), ДокументОбєкт.TypeDocument, "Delete", ДокументОбєкт.SpendDate, "");
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

			Function.AddBackgroundTask_CalculationVirtualBalances(ДокументОбєкт.UnigueID.ToString(), ДокументОбєкт.TypeDocument, "Add", ДокументОбєкт.ДатаДок, "");

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

			Function.AddBackgroundTask_CalculationVirtualBalances(ДокументОбєкт.UnigueID.ToString(), ДокументОбєкт.TypeDocument, "Delete", ДокументОбєкт.ДатаДок, "");
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
					Function.AddBackgroundTask_CalculationVirtualBalances(ДокументОбєкт.UnigueID.ToString(), ДокументОбєкт.TypeDocument, "Delete", ДокументОбєкт.SpendDate, "");
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

			Function.AddBackgroundTask_CalculationVirtualBalances(ДокументОбєкт.UnigueID.ToString(), ДокументОбєкт.TypeDocument, "Add", ДокументОбєкт.ДатаДок, "");

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

			Function.AddBackgroundTask_CalculationVirtualBalances(ДокументОбєкт.UnigueID.ToString(), ДокументОбєкт.TypeDocument, "Delete", ДокументОбєкт.ДатаДок, "");
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
					Function.AddBackgroundTask_CalculationVirtualBalances(ДокументОбєкт.UnigueID.ToString(), ДокументОбєкт.TypeDocument, "Delete", ДокументОбєкт.SpendDate, "");
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

			Function.AddBackgroundTask_CalculationVirtualBalances(ДокументОбєкт.UnigueID.ToString(), ДокументОбєкт.TypeDocument, "Add", ДокументОбєкт.ДатаДок, "");

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

			Function.AddBackgroundTask_CalculationVirtualBalances(ДокументОбєкт.UnigueID.ToString(), ДокументОбєкт.TypeDocument, "Delete", ДокументОбєкт.ДатаДок, "");
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
					Function.AddBackgroundTask_CalculationVirtualBalances(ДокументОбєкт.UnigueID.ToString(), ДокументОбєкт.TypeDocument, "Delete", ДокументОбєкт.SpendDate, "");
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
					record2.ВНаявності = Товари_Record.Кількість;
				}
			}

			товариНаСкладах_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

			#endregion

			Function.AddBackgroundTask_CalculationVirtualBalances(ДокументОбєкт.UnigueID.ToString(), ДокументОбєкт.TypeDocument, "Add", ДокументОбєкт.ДатаДок, "");

			return true;
		}

		public static void ClearSpend(ПереміщенняТоварів_Objest ДокументОбєкт)
		{
			РегістриНакопичення.ТовариНаСкладах_RecordsSet товариНаСкладах_RecordsSet = new РегістриНакопичення.ТовариНаСкладах_RecordsSet();
			товариНаСкладах_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);

			Function.AddBackgroundTask_CalculationVirtualBalances(ДокументОбєкт.UnigueID.ToString(), ДокументОбєкт.TypeDocument, "Delete", ДокументОбєкт.ДатаДок, "");
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
					Function.AddBackgroundTask_CalculationVirtualBalances(ДокументОбєкт.UnigueID.ToString(), ДокументОбєкт.TypeDocument, "Delete", ДокументОбєкт.SpendDate, "");
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
				record.Склад = ДокументОбєкт.Склад;
				record.ВНаявності = Товари_Record.Кількість;
			}

			товариНаСкладах_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

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

			Function.AddBackgroundTask_CalculationVirtualBalances(ДокументОбєкт.UnigueID.ToString(), ДокументОбєкт.TypeDocument, "Add", ДокументОбєкт.ДатаДок, "");

			return true;
		}

		public static void ClearSpend(ВведенняЗалишків_Objest ДокументОбєкт)
		{
			РегістриНакопичення.ТовариНаСкладах_RecordsSet товариНаСкладах_RecordsSet = new РегістриНакопичення.ТовариНаСкладах_RecordsSet();
			товариНаСкладах_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);

			РегістриНакопичення.РухКоштів_RecordsSet рухКоштів_RecordsSet = new РегістриНакопичення.РухКоштів_RecordsSet();
			рухКоштів_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);

			Function.AddBackgroundTask_CalculationVirtualBalances(ДокументОбєкт.UnigueID.ToString(), ДокументОбєкт.TypeDocument, "Delete", ДокументОбєкт.ДатаДок, "");
		}
	}
}
