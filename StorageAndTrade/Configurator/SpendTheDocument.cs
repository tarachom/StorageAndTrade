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

			РегістриНакопичення.РозрахункиЗКлієнтами_RecordsSet розрахункиЗКлієнтами_RecordsSet = new РегістриНакопичення.РозрахункиЗКлієнтами_RecordsSet();

			РегістриНакопичення.РозрахункиЗКлієнтами_RecordsSet.Record розрахункиЗКлієнтами_Record = new РегістриНакопичення.РозрахункиЗКлієнтами_RecordsSet.Record();
			розрахункиЗКлієнтами_RecordsSet.Records.Add(розрахункиЗКлієнтами_Record);

			розрахункиЗКлієнтами_Record.Income = true; // +
			розрахункиЗКлієнтами_Record.Owner = ДокументОбєкт.UnigueID.UGuid;

			розрахункиЗКлієнтами_Record.Контрагент = ДокументОбєкт.Контрагент;
			розрахункиЗКлієнтами_Record.Валюта = ДокументОбєкт.Валюта;
			розрахункиЗКлієнтами_Record.Сума = ДокументОбєкт.СумаДокументу;

			розрахункиЗКлієнтами_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

			#endregion

			return true;
		}

		public static void ClearSpend(ЗамовленняКлієнта_Objest ДокументОбєкт)
		{
			РегістриНакопичення.ЗамовленняКлієнтів_RecordsSet замовленняКлієнтів_RecordsSet = new РегістриНакопичення.ЗамовленняКлієнтів_RecordsSet();
			замовленняКлієнтів_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);

			РегістриНакопичення.РозрахункиЗКлієнтами_RecordsSet розрахункиЗКлієнтами_RecordsSet = new РегістриНакопичення.РозрахункиЗКлієнтами_RecordsSet();
			розрахункиЗКлієнтами_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);

			РегістриНакопичення.ВільніЗалишки_RecordsSet вільніЗалишки_RecordsSet = new РегістриНакопичення.ВільніЗалишки_RecordsSet();
			вільніЗалишки_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);
		}
	}

	class РеалізаціяТоварівТаПослуг_SpendTheDocument
	{
		public static bool Spend(РеалізаціяТоварівТаПослуг_Objest ДокументОбєкт)
		{
			#region Рух по регістрах

			//
			//ЗамовленняКлієнтів
			//

			РегістриНакопичення.ЗамовленняКлієнтів_RecordsSet замовленняКлієнтів_RecordsSet = new РегістриНакопичення.ЗамовленняКлієнтів_RecordsSet();

			ДокументОбєкт.Товари_TablePart.Read();

			foreach (РеалізаціяТоварівТаПослуг_Товари_TablePart.Record Товари_Record in ДокументОбєкт.Товари_TablePart.Records)
			{
				РегістриНакопичення.ЗамовленняКлієнтів_RecordsSet.Record record = new РегістриНакопичення.ЗамовленняКлієнтів_RecordsSet.Record();
				замовленняКлієнтів_RecordsSet.Records.Add(record);

				record.Income = false; // -     | Документ зменшує замовлення
				record.Owner = ДокументОбєкт.UnigueID.UGuid;

				record.ЗамовленняКлієнта = Товари_Record.ЗамовленняКлієнта;
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

			foreach (РеалізаціяТоварівТаПослуг_Товари_TablePart.Record Товари_Record in ДокументОбєкт.Товари_TablePart.Records)
			{
				РегістриНакопичення.ВільніЗалишки_RecordsSet.Record record = new РегістриНакопичення.ВільніЗалишки_RecordsSet.Record();
				вільніЗалишки_RecordsSet.Records.Add(record);

				record.Income = false; // -      | Документ зменшує резерв
				record.Owner = ДокументОбєкт.UnigueID.UGuid;

				record.Номенклатура = Товари_Record.Номенклатура;
				record.ХарактеристикаНоменклатури = Товари_Record.ХарактеристикаНоменклатури;
				record.Склад = ДокументОбєкт.Склад;
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
				record.Склад = ДокументОбєкт.Склад;
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
		}
	}

	class ПоступленняТоварівТаПослуг_SpendTheDocument
	{
		public static bool Spend(ПоступленняТоварівТаПослуг_Objest ДокументОбєкт)
		{
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
					//Документи.ЗамовленняПостачальнику_Objest замовленняПостачальнику_Objest =
					//Товари_Record.ЗамовленняПостачальнику.GetDocumentObject();

					РегістриНакопичення.ЗамовленняПостачальникам_RecordsSet.Record record = new РегістриНакопичення.ЗамовленняПостачальникам_RecordsSet.Record();
					замовленняПостачальникам_RecordsSet.Records.Add(record);

					record.Income = false; // -
					record.Owner = ДокументОбєкт.UnigueID.UGuid;

					record.ЗамовленняПостачальнику = record.ЗамовленняПостачальнику;
					record.Номенклатура = Товари_Record.Номенклатура;
					record.ХарактеристикаНоменклатури = Товари_Record.ХарактеристикаНоменклатури;
					record.Склад = ДокументОбєкт.Склад;
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
					//Якщо склад вказаний в табличні частині, береться цей склад, інаше склад береться з шапки
					Довідники.Склади_Pointer СкладПоступленняТовару = Товари_Record.Склад.IsEmpty() ? ДокументОбєкт.Склад : Товари_Record.Склад;

					РегістриНакопичення.ТовариНаСкладах_RecordsSet.Record record = new РегістриНакопичення.ТовариНаСкладах_RecordsSet.Record();
					товариНаСкладах_RecordsSet.Records.Add(record);

					record.Income = true; // + 
					record.Owner = ДокументОбєкт.UnigueID.UGuid;

					record.Номенклатура = Товари_Record.Номенклатура;
					record.ХарактеристикаНоменклатури = Товари_Record.ХарактеристикаНоменклатури;
					record.Склад = СкладПоступленняТовару;
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
					record.Склад = ДокументОбєкт.Склад;
					record.ВНаявності = Товари_Record.Кількість;
				}
			}

			вільніЗалишки_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

			//
			//ТовариДоПоступлення
			//

			РегістриНакопичення.ТовариДоПоступлення_RecordsSet товариДоПоступлення_RecordsSet = new РегістриНакопичення.ТовариДоПоступлення_RecordsSet();

			foreach (ПоступленняТоварівТаПослуг_Товари_TablePart.Record Товари_Record in ДокументОбєкт.Товари_TablePart.Records)
			{
				Довідники.Номенклатура_Objest номенклатура_Objest = Товари_Record.Номенклатура.GetDirectoryObject();

				//Товар
				if (номенклатура_Objest.ТипНоменклатури == Перелічення.ТипиНоменклатури.Товар)
				{
					РегістриНакопичення.ТовариДоПоступлення_RecordsSet.Record record = new РегістриНакопичення.ТовариДоПоступлення_RecordsSet.Record();
					товариДоПоступлення_RecordsSet.Records.Add(record);

					record.Income = false; // -
					record.Owner = ДокументОбєкт.UnigueID.UGuid;

					record.Номенклатура = Товари_Record.Номенклатура;
					record.ХарактеристикаНоменклатури = Товари_Record.ХарактеристикаНоменклатури;
					record.Склад = ДокументОбєкт.Склад;
					record.ДоПоступлення = Товари_Record.Кількість;
				}
			}

			товариДоПоступлення_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

			//??? Договір

			//
			//РозрахункиЗПостачальниками
			//

			РегістриНакопичення.РозрахункиЗПостачальниками_RecordsSet розрахункиЗПостачальниками_RecordsSet = new РегістриНакопичення.РозрахункиЗПостачальниками_RecordsSet();

			РегістриНакопичення.РозрахункиЗПостачальниками_RecordsSet.Record recordContragent = new РегістриНакопичення.РозрахункиЗПостачальниками_RecordsSet.Record();
			розрахункиЗПостачальниками_RecordsSet.Records.Add(recordContragent);

			recordContragent.Income = true;
			recordContragent.Owner = ДокументОбєкт.UnigueID.UGuid;

			recordContragent.Контрагент = ДокументОбєкт.Контрагент;
			recordContragent.Валюта = ДокументОбєкт.Валюта;
			recordContragent.Сума = ДокументОбєкт.СумаДокументу;

			//record.ГосподарськаОперація = ДокументОбєкт.ГосподарськаОперація;
			//record.ФормаОплати = ДокументОбєкт.ФормаОплати;

			розрахункиЗПостачальниками_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

			#endregion

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
		}
	}

	class ЗамовленняПостачальнику_SpendTheDocument
	{
		public static bool Spend(ЗамовленняПостачальнику_Objest ДокументОбєкт)
		{
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

			РегістриНакопичення.РозрахункиЗПостачальниками_RecordsSet розрахункиЗПостачальниками_RecordsSet = new РегістриНакопичення.РозрахункиЗПостачальниками_RecordsSet();

			РегістриНакопичення.РозрахункиЗПостачальниками_RecordsSet.Record розрахункиЗПостачальниками_Record = new РегістриНакопичення.РозрахункиЗПостачальниками_RecordsSet.Record();
			розрахункиЗПостачальниками_RecordsSet.Records.Add(розрахункиЗПостачальниками_Record);

			розрахункиЗПостачальниками_Record.Income = true; // +
			розрахункиЗПостачальниками_Record.Owner = ДокументОбєкт.UnigueID.UGuid;

			розрахункиЗПостачальниками_Record.Контрагент = ДокументОбєкт.Контрагент;
			розрахункиЗПостачальниками_Record.Валюта = ДокументОбєкт.Валюта;
			розрахункиЗПостачальниками_Record.Сума = ДокументОбєкт.СумаДокументу;

			розрахункиЗПостачальниками_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

			//
			//ТовариДоПоступлення
			//

			РегістриНакопичення.ТовариДоПоступлення_RecordsSet товариДоПоступлення_RecordsSet = new РегістриНакопичення.ТовариДоПоступлення_RecordsSet();

			foreach (ЗамовленняПостачальнику_Товари_TablePart.Record Товари_Record in ДокументОбєкт.Товари_TablePart.Records)
			{
				РегістриНакопичення.ТовариДоПоступлення_RecordsSet.Record record = new РегістриНакопичення.ТовариДоПоступлення_RecordsSet.Record();
				товариДоПоступлення_RecordsSet.Records.Add(record);

				record.Income = true; // +    | Документ збільшує
				record.Owner = ДокументОбєкт.UnigueID.UGuid;

				record.Номенклатура = Товари_Record.Номенклатура;
				record.ХарактеристикаНоменклатури = Товари_Record.ХарактеристикаНоменклатури;
				record.Склад = ДокументОбєкт.Склад;
				record.ДоПоступлення = Товари_Record.Кількість;
			}

			товариДоПоступлення_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

			#endregion

			return true;
		}

		public static void ClearSpend(ЗамовленняПостачальнику_Objest ДокументОбєкт)
		{
			РегістриНакопичення.ЗамовленняПостачальникам_RecordsSet замовленняПостачальникам_RecordsSet = new РегістриНакопичення.ЗамовленняПостачальникам_RecordsSet();
			замовленняПостачальникам_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);

			РегістриНакопичення.РозрахункиЗПостачальниками_RecordsSet розрахункиЗПостачальниками_RecordsSet = new РегістриНакопичення.РозрахункиЗПостачальниками_RecordsSet();
			розрахункиЗПостачальниками_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);

			РегістриНакопичення.ТовариДоПоступлення_RecordsSet товариДоПоступлення_RecordsSet = new РегістриНакопичення.ТовариДоПоступлення_RecordsSet();
			товариДоПоступлення_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);
		}
	}

	class ПоверненняТоварівВідКлієнта_SpendTheDocument
	{
		public static bool Spend(ПоверненняТоварівВідКлієнта_Objest ДокументОбєкт)
		{
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
				record.ВРезервіПідЗамовлення = Товари_Record.Кількість;
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

				розрахункиЗКлієнтами_Record.Income = true; // +
				розрахункиЗКлієнтами_Record.Owner = ДокументОбєкт.UnigueID.UGuid;

				//розрахункиЗКлієнтами_Record.ЗамовленняКлієнта = Товари_Record.ДокументРеалізації;
				розрахункиЗКлієнтами_Record.Валюта = ДокументОбєкт.Валюта;
				розрахункиЗКлієнтами_Record.Сума = ДокументОбєкт.СумаДокументу;
			}

			розрахункиЗКлієнтами_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

			#endregion

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
		}
	}

	class ПоверненняТоварівПостачальнику_SpendTheDocument
	{
		public static bool Spend(ПоверненняТоварівПостачальнику_Objest ДокументОбєкт)
		{
			#region Рух по регістрах

			//
			//Товари на складах
			//

			РегістриНакопичення.ТовариНаСкладах_RecordsSet товариНаСкладах_RecordsSet = new РегістриНакопичення.ТовариНаСкладах_RecordsSet();

			foreach (ПоверненняТоварівПостачальнику_Товари_TablePart.Record Товари_Record in ДокументОбєкт.Товари_TablePart.Records)
			{
				РегістриНакопичення.ТовариНаСкладах_RecordsSet.Record record = new РегістриНакопичення.ТовариНаСкладах_RecordsSet.Record();
				товариНаСкладах_RecordsSet.Records.Add(record);

				record.Income = false; // - 
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

				record.Income = false; // -
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

				розрахункиЗПостачальниками_Record.Income = false; // -
				розрахункиЗПостачальниками_Record.Owner = ДокументОбєкт.UnigueID.UGuid;

				//розрахункиЗПостачальниками_Record.ЗамовленняПостачальнику = Товари_Record.ДокументПоступлення;
				розрахункиЗПостачальниками_Record.Валюта = ДокументОбєкт.Валюта;
				розрахункиЗПостачальниками_Record.Сума = ДокументОбєкт.СумаДокументу;
			}

			розрахункиЗПостачальниками_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

			#endregion

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
		}
	}

	class ПрихіднийКасовийОрдер_SpendTheDocument
	{
		public static bool Spend(ПрихіднийКасовийОрдер_Objest ДокументОбєкт)
		{
			#region Рух по регістрах

			Довідники.ДоговориКонтрагентів_Objest Договір = ДокументОбєкт.Договір.GetDirectoryObject();
			Перелічення.ТипДоговорів типДоговору = Договір.ТипДоговору;

			if (типДоговору == Перелічення.ТипДоговорів.ЗПостачальниками)
			{
				//
				//РозрахункиЗПостачальниками
				//

				РегістриНакопичення.РозрахункиЗПостачальниками_RecordsSet розрахункиЗПостачальниками_RecordsSet = new РегістриНакопичення.РозрахункиЗПостачальниками_RecordsSet();

				РегістриНакопичення.РозрахункиЗПостачальниками_RecordsSet.Record record_Постачальник = new РегістриНакопичення.РозрахункиЗПостачальниками_RecordsSet.Record();
				розрахункиЗПостачальниками_RecordsSet.Records.Add(record_Постачальник);

				record_Постачальник.Income = true;
				record_Постачальник.Owner = ДокументОбєкт.UnigueID.UGuid;

				record_Постачальник.Контрагент = ДокументОбєкт.Контрагент;
				record_Постачальник.Валюта = ДокументОбєкт.Валюта;
				record_Постачальник.Сума = ДокументОбєкт.СумаДокументу;

				розрахункиЗПостачальниками_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);
			}

			if (типДоговору == Перелічення.ТипДоговорів.ЗПокупцями)
			{
				//
				//РозрахункиЗКлієнтами
				//

				РегістриНакопичення.РозрахункиЗКлієнтами_RecordsSet розрахункиЗКлієнтами_RecordsSet = new РегістриНакопичення.РозрахункиЗКлієнтами_RecordsSet();

				РегістриНакопичення.РозрахункиЗКлієнтами_RecordsSet.Record record_Клієнт = new РегістриНакопичення.РозрахункиЗКлієнтами_RecordsSet.Record();
				розрахункиЗКлієнтами_RecordsSet.Records.Add(record_Клієнт);

				record_Клієнт.Income = true;
				record_Клієнт.Owner = ДокументОбєкт.UnigueID.UGuid;

				record_Клієнт.Контрагент = ДокументОбєкт.Контрагент;
				record_Клієнт.Валюта = ДокументОбєкт.Валюта;
				record_Клієнт.Сума = ДокументОбєкт.СумаДокументу;

				розрахункиЗКлієнтами_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);
			}

			#endregion

			return true;
		}

		public static void ClearSpend(ПрихіднийКасовийОрдер_Objest ДокументОбєкт)
		{
			Довідники.ДоговориКонтрагентів_Objest Договір = ДокументОбєкт.Договір.GetDirectoryObject();
			Перелічення.ТипДоговорів типДоговору = Договір.ТипДоговору;

			if (типДоговору == Перелічення.ТипДоговорів.ЗПостачальниками)
			{
				РегістриНакопичення.РозрахункиЗПостачальниками_RecordsSet розрахункиЗПостачальниками_RecordsSet = new РегістриНакопичення.РозрахункиЗПостачальниками_RecordsSet();
				розрахункиЗПостачальниками_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);
			}

			if (типДоговору == Перелічення.ТипДоговорів.ЗПокупцями)
			{
				РегістриНакопичення.РозрахункиЗКлієнтами_RecordsSet розрахункиЗКлієнтами_RecordsSet = new РегістриНакопичення.РозрахункиЗКлієнтами_RecordsSet();
				розрахункиЗКлієнтами_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);
			}
		}
	}

	class РозхіднийКасовийОрдер_SpendTheDocument
	{
		public static bool Spend(РозхіднийКасовийОрдер_Objest ДокументОбєкт)
		{
			#region Рух по регістрах

			Довідники.ДоговориКонтрагентів_Objest Договір = ДокументОбєкт.Договір.GetDirectoryObject();
			Перелічення.ТипДоговорів типДоговору = Договір.ТипДоговору;

			if (типДоговору == Перелічення.ТипДоговорів.ЗПостачальниками)
			{
				//
				//РозрахункиЗПостачальниками
				//

				РегістриНакопичення.РозрахункиЗПостачальниками_RecordsSet розрахункиЗПостачальниками_RecordsSet = new РегістриНакопичення.РозрахункиЗПостачальниками_RecordsSet();

				РегістриНакопичення.РозрахункиЗПостачальниками_RecordsSet.Record record_Постачальник = new РегістриНакопичення.РозрахункиЗПостачальниками_RecordsSet.Record();
				розрахункиЗПостачальниками_RecordsSet.Records.Add(record_Постачальник);

				record_Постачальник.Income = false;
				record_Постачальник.Owner = ДокументОбєкт.UnigueID.UGuid;

				record_Постачальник.Контрагент = ДокументОбєкт.Контрагент;
				record_Постачальник.Валюта = ДокументОбєкт.Валюта;
				record_Постачальник.Сума = ДокументОбєкт.СумаДокументу;

				розрахункиЗПостачальниками_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);
			}

			if (типДоговору == Перелічення.ТипДоговорів.ЗПокупцями)
			{
				//
				//РозрахункиЗКлієнтами
				//

				РегістриНакопичення.РозрахункиЗКлієнтами_RecordsSet розрахункиЗКлієнтами_RecordsSet = new РегістриНакопичення.РозрахункиЗКлієнтами_RecordsSet();

				РегістриНакопичення.РозрахункиЗКлієнтами_RecordsSet.Record record_Клієнт = new РегістриНакопичення.РозрахункиЗКлієнтами_RecordsSet.Record();
				розрахункиЗКлієнтами_RecordsSet.Records.Add(record_Клієнт);

				record_Клієнт.Income = false;
				record_Клієнт.Owner = ДокументОбєкт.UnigueID.UGuid;

				record_Клієнт.Контрагент = ДокументОбєкт.Контрагент;
				record_Клієнт.Валюта = ДокументОбєкт.Валюта;
				record_Клієнт.Сума = ДокументОбєкт.СумаДокументу;

				розрахункиЗКлієнтами_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);
			}

			#endregion

			return true;
		}

		public static void ClearSpend(РозхіднийКасовийОрдер_Objest ДокументОбєкт)
		{
			Довідники.ДоговориКонтрагентів_Objest Договір = ДокументОбєкт.Договір.GetDirectoryObject();
			Перелічення.ТипДоговорів типДоговору = Договір.ТипДоговору;

			if (типДоговору == Перелічення.ТипДоговорів.ЗПостачальниками)
			{
				РегістриНакопичення.РозрахункиЗПостачальниками_RecordsSet розрахункиЗПостачальниками_RecordsSet = new РегістриНакопичення.РозрахункиЗПостачальниками_RecordsSet();
				розрахункиЗПостачальниками_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);
			}

			if (типДоговору == Перелічення.ТипДоговорів.ЗПокупцями)
			{
				РегістриНакопичення.РозрахункиЗКлієнтами_RecordsSet розрахункиЗКлієнтами_RecordsSet = new РегістриНакопичення.РозрахункиЗКлієнтами_RecordsSet();
				розрахункиЗКлієнтами_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);
			}
		}
	}

	class ПереміщенняТоварів_SpendTheDocument
	{
		public static bool Spend(ПереміщенняТоварів_Objest ДокументОбєкт)
		{
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

			return true;
		}

		public static void ClearSpend(ПереміщенняТоварів_Objest ДокументОбєкт)
		{
			РегістриНакопичення.ТовариНаСкладах_RecordsSet товариНаСкладах_RecordsSet = new РегістриНакопичення.ТовариНаСкладах_RecordsSet();
			товариНаСкладах_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);
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
			//РегістриНакопичення.ТовариНаСкладах_RecordsSet товариНаСкладах_RecordsSet = new РегістриНакопичення.ТовариНаСкладах_RecordsSet();
			//товариНаСкладах_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);
		}
	}
}
