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
 *
 * Конфігурації "Домашні фінанси 1.0"
 * Автор Тарахомин Юрій Іванович, Україна, м. Львів, accounting.org.ua, tarachom@gmail.com
 * Дата конфігурації: 03.09.2021 16:50:57
 *
 */

using System;
using System.Collections.Generic;
using AccountingSoftware;

namespace StorageAndTrade_1_0.Довідники
{

}

namespace StorageAndTrade_1_0.Документи
{
	class Записи_Triggers
    {
		#region ЗамовленняКлієнта

		public static void ЗамовленняКлієнта_BeforeRecording(ЗамовленняКлієнта_Objest ДокументОбєкт)
        {

        }

		public static void ЗамовленняКлієнта_AfterRecording(ЗамовленняКлієнта_Objest ДокументОбєкт)
		{
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

			розрахункиЗКлієнтами_Record.ЗамовленняКлієнта = ДокументОбєкт.GetDocumentPointer();
			розрахункиЗКлієнтами_Record.Валюта = ДокументОбєкт.Валюта;
			розрахункиЗКлієнтами_Record.Сума = ДокументОбєкт.СумаДокументу;

			розрахункиЗКлієнтами_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);
		}

		public static void ЗамовленняКлієнта_BeforeDelete(ЗамовленняКлієнта_Objest ДокументОбєкт)
		{
			РегістриНакопичення.ЗамовленняКлієнтів_RecordsSet замовленняКлієнтів_RecordsSet = new РегістриНакопичення.ЗамовленняКлієнтів_RecordsSet();
			замовленняКлієнтів_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);

			РегістриНакопичення.РозрахункиЗКлієнтами_RecordsSet розрахункиЗКлієнтами_RecordsSet = new РегістриНакопичення.РозрахункиЗКлієнтами_RecordsSet();
			розрахункиЗКлієнтами_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);

			РегістриНакопичення.ВільніЗалишки_RecordsSet вільніЗалишки_RecordsSet = new РегістриНакопичення.ВільніЗалишки_RecordsSet();
			вільніЗалишки_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);
		}

		#endregion

		#region РеалізаціяТоварівТаПослуг

		public static void РеалізаціяТоварівТаПослуг_BeforeRecording(РеалізаціяТоварівТаПослуг_Objest ДокументОбєкт)
		{

		}

		public static void РеалізаціяТоварівТаПослуг_AfterRecording(РеалізаціяТоварівТаПослуг_Objest ДокументОбєкт)
        {
			//
			//ЗамовленняКлієнтів
			//

			РегістриНакопичення.ЗамовленняКлієнтів_RecordsSet замовленняКлієнтів_RecordsSet = new РегістриНакопичення.ЗамовленняКлієнтів_RecordsSet();

			ДокументОбєкт.Товари_TablePart.Read();

			foreach (РеалізаціяТоварівТаПослуг_Товари_TablePart.Record Товари_Record in ДокументОбєкт.Товари_TablePart.Records)
			{
				РегістриНакопичення.ЗамовленняКлієнтів_RecordsSet.Record record = new РегістриНакопичення.ЗамовленняКлієнтів_RecordsSet.Record();
				замовленняКлієнтів_RecordsSet.Records.Add(record);

				record.Income = false; // -
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

				record.Income = false; // -    | Документ зменшує резерв
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

				record.Income = false; // -    | Документ зменшує наявність
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

			foreach (РеалізаціяТоварівТаПослуг_Товари_TablePart.Record Товари_Record in ДокументОбєкт.Товари_TablePart.Records)
            {
				РегістриНакопичення.РозрахункиЗКлієнтами_RecordsSet.Record розрахункиЗКлієнтами_Record = new РегістриНакопичення.РозрахункиЗКлієнтами_RecordsSet.Record();
				розрахункиЗКлієнтами_RecordsSet.Records.Add(розрахункиЗКлієнтами_Record);

				розрахункиЗКлієнтами_Record.Income = false; // -
				розрахункиЗКлієнтами_Record.Owner = ДокументОбєкт.UnigueID.UGuid;

				розрахункиЗКлієнтами_Record.ЗамовленняКлієнта = Товари_Record.ЗамовленняКлієнта;
				розрахункиЗКлієнтами_Record.Валюта = ДокументОбєкт.Валюта;
				розрахункиЗКлієнтами_Record.Сума = ДокументОбєкт.СумаДокументу;
			}

			розрахункиЗКлієнтами_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);


		}

		public static void РеалізаціяТоварівТаПослуг_BeforeDelete(РеалізаціяТоварівТаПослуг_Objest ДокументОбєкт)
        {
			РегістриНакопичення.ЗамовленняКлієнтів_RecordsSet замовленняКлієнтів_RecordsSet = new РегістриНакопичення.ЗамовленняКлієнтів_RecordsSet();
			замовленняКлієнтів_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);

			РегістриНакопичення.ВільніЗалишки_RecordsSet вільніЗалишки_RecordsSet = new РегістриНакопичення.ВільніЗалишки_RecordsSet();
			вільніЗалишки_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);

			РегістриНакопичення.ТовариНаСкладах_RecordsSet товариНаСкладах_RecordsSet = new РегістриНакопичення.ТовариНаСкладах_RecordsSet();
			товариНаСкладах_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);

			//РегістриНакопичення.РозрахункиЗКлієнтами_RecordsSet розрахункиЗКлієнтами_RecordsSet = new РегістриНакопичення.РозрахункиЗКлієнтами_RecordsSet();
			//розрахункиЗКлієнтами_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);
		}

		#endregion

		#region ЗамовленняПостачальнику

		public static void ЗамовленняПостачальнику_BeforeRecording(ЗамовленняПостачальнику_Objest ДокументОбєкт)
		{

		}

		public static void ЗамовленняПостачальнику_AfterRecording(ЗамовленняПостачальнику_Objest ДокументОбєкт)
		{
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

			розрахункиЗПостачальниками_Record.ЗамовленняПостачальнику = ДокументОбєкт.GetDocumentPointer();
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
		}

		public static void ЗамовленняПостачальнику_BeforeDelete(ЗамовленняПостачальнику_Objest ДокументОбєкт)
		{

		}

		#endregion

		#region ПоступленняТоварівТаПослуг

		public static void ПоступленняТоварівТаПослуг_BeforeRecording(ПоступленняТоварівТаПослуг_Objest ДокументОбєкт)
		{

		}

		public static void ПоступленняТоварівТаПослуг_AfterRecording(ПоступленняТоварівТаПослуг_Objest ДокументОбєкт)
		{
			//
			//Замовлення постачальникам
			//

			РегістриНакопичення.ЗамовленняПостачальникам_RecordsSet замовленняПостачальникам_RecordsSet = new РегістриНакопичення.ЗамовленняПостачальникам_RecordsSet();

			foreach (ПоступленняТоварівТаПослуг_Товари_TablePart.Record Товари_Record in ДокументОбєкт.Товари_TablePart.Records)
			{
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

			замовленняПостачальникам_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

			//
			//Товари на складах
			//

			РегістриНакопичення.ТовариНаСкладах_RecordsSet товариНаСкладах_RecordsSet = new РегістриНакопичення.ТовариНаСкладах_RecordsSet();

			foreach (ПоступленняТоварівТаПослуг_Товари_TablePart.Record Товари_Record in ДокументОбєкт.Товари_TablePart.Records)
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
			//ВільніЗалишки
			//

			РегістриНакопичення.ВільніЗалишки_RecordsSet вільніЗалишки_RecordsSet = new РегістриНакопичення.ВільніЗалишки_RecordsSet();

			foreach (ПоступленняТоварівТаПослуг_Товари_TablePart.Record Товари_Record in ДокументОбєкт.Товари_TablePart.Records)
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
			//ТовариДоПоступлення
			//

			РегістриНакопичення.ТовариДоПоступлення_RecordsSet товариДоПоступлення_RecordsSet = new РегістриНакопичення.ТовариДоПоступлення_RecordsSet();

			foreach (ПоступленняТоварівТаПослуг_Товари_TablePart.Record Товари_Record in ДокументОбєкт.Товари_TablePart.Records)
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

			товариДоПоступлення_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

			//
			//РозрахункиЗПостачальниками
			//

			РегістриНакопичення.РозрахункиЗПостачальниками_RecordsSet розрахункиЗПостачальниками_RecordsSet = new РегістриНакопичення.РозрахункиЗПостачальниками_RecordsSet();
			foreach (ПоступленняТоварівТаПослуг_Товари_TablePart.Record Товари_Record in ДокументОбєкт.Товари_TablePart.Records)
            {
				РегістриНакопичення.РозрахункиЗПостачальниками_RecordsSet.Record розрахункиЗПостачальниками_Record = new РегістриНакопичення.РозрахункиЗПостачальниками_RecordsSet.Record();
				розрахункиЗПостачальниками_RecordsSet.Records.Add(розрахункиЗПостачальниками_Record);

				розрахункиЗПостачальниками_Record.Income = false; // -
				розрахункиЗПостачальниками_Record.Owner = ДокументОбєкт.UnigueID.UGuid;

				розрахункиЗПостачальниками_Record.ЗамовленняПостачальнику = Товари_Record.ЗамовленняПостачальнику;
				розрахункиЗПостачальниками_Record.Валюта = ДокументОбєкт.Валюта;
				розрахункиЗПостачальниками_Record.Сума = ДокументОбєкт.СумаДокументу;
			}

			розрахункиЗПостачальниками_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);
		}

		public static void ПоступленняТоварівТаПослуг_BeforeDelete(ПоступленняТоварівТаПослуг_Objest ДокументОбєкт)
		{
			
		}

		#endregion
	}
}