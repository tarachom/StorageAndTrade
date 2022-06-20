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
 
Модуль функцій зворотнього виклику.

1. Перед записом
2. Після запису
3. Перед видаленням
 
*/

using System;
using System.Collections.Generic;
using AccountingSoftware;

namespace StorageAndTrade_1_0.Довідники
{
	class Номенклатура_Папки_Triggers
	{
		public static void BeforeRecording(Номенклатура_Папки_Objest ДовідникОбєкт)
		{

		}
		public static void AfterRecording(Номенклатура_Папки_Objest ДовідникОбєкт)
		{

		}

		public static void BeforeDelete(Номенклатура_Папки_Objest ДовідникОбєкт)
		{
			//
			//Елементи переносяться на верхній рівень
			//

			Номенклатура_Select номенклатура_Select = new Номенклатура_Select();
			номенклатура_Select.QuerySelect.Where.Add(new Where(Номенклатура_Const.Папка, Comparison.EQ, ДовідникОбєкт.UnigueID.UGuid));
			номенклатура_Select.Select();

			while(номенклатура_Select.MoveNext())
            {
				Номенклатура_Objest номенклатура_Objest = номенклатура_Select.Current.GetDirectoryObject();
				номенклатура_Objest.Папка = new Номенклатура_Папки_Pointer();
				номенклатура_Objest.Save();
			}

			//
			//Вкладені папки видяляються. Для кожної папки буде викликана функція BeforeDelete
			//

			Номенклатура_Папки_Select номенклатура_Папки_Select = new Номенклатура_Папки_Select();
			номенклатура_Папки_Select.QuerySelect.Where.Add(new Where(Номенклатура_Папки_Const.Родич, Comparison.EQ, ДовідникОбєкт.UnigueID.UGuid));
			номенклатура_Папки_Select.Select();

			while (номенклатура_Папки_Select.MoveNext())
            {
				Номенклатура_Папки_Objest номенклатура_Папки_Objest = номенклатура_Папки_Select.Current.GetDirectoryObject();
				if (номенклатура_Папки_Objest != null)
					номенклатура_Папки_Objest.Delete();

			}
		}
	}
	
	class Контрагенти_Папки_Triggers
	{
		public static void BeforeRecording(Контрагенти_Папки_Objest ДовідникОбєкт)
		{

		}
		public static void AfterRecording(Контрагенти_Папки_Objest ДовідникОбєкт)
		{

		}

		public static void BeforeDelete(Контрагенти_Папки_Objest ДовідникОбєкт)
		{
			//
			//Елементи переносяться на верхній рівень
			//

			Контрагенти_Select контрагенти_Select = new Контрагенти_Select();
			контрагенти_Select.QuerySelect.Where.Add(new Where(Контрагенти_Const.Папка, Comparison.EQ, ДовідникОбєкт.UnigueID.UGuid));
			контрагенти_Select.Select();

			while (контрагенти_Select.MoveNext())
			{
				Контрагенти_Objest контрагенти_Objest = контрагенти_Select.Current.GetDirectoryObject();
				контрагенти_Objest.Папка = new Контрагенти_Папки_Pointer();
				контрагенти_Objest.Save();
			}

			//
			//Вкладені папки видаляються. Для кожної папки буде викликана функція BeforeDelete
			//

			Контрагенти_Папки_Select контрагенти_Папки_Select = new Контрагенти_Папки_Select();
			контрагенти_Папки_Select.QuerySelect.Where.Add(new Where(Контрагенти_Папки_Const.Родич, Comparison.EQ, ДовідникОбєкт.UnigueID.UGuid));
			контрагенти_Папки_Select.Select();

			while (контрагенти_Папки_Select.MoveNext())
			{
				Контрагенти_Папки_Objest контрагенти_Папки_Objest = контрагенти_Папки_Select.Current.GetDirectoryObject();
				if (контрагенти_Папки_Objest != null)
					контрагенти_Папки_Objest.Delete();

			}
		}
	}

	class Склади_Папки_Triggers
	{
		public static void BeforeRecording(Склади_Папки_Objest ДовідникОбєкт)
		{

		}
		public static void AfterRecording(Склади_Папки_Objest ДовідникОбєкт)
		{

		}

		public static void BeforeDelete(Склади_Папки_Objest ДовідникОбєкт)
		{
			//
			//Елементи переносяться на верхній рівень
			//

			Склади_Select склади_Select = new Склади_Select();
			склади_Select.QuerySelect.Where.Add(new Where(Склади_Const.Папка, Comparison.EQ, ДовідникОбєкт.UnigueID.UGuid));
			склади_Select.Select();

			while (склади_Select.MoveNext())
			{
				Склади_Objest склади_Objest = склади_Select.Current.GetDirectoryObject();
				склади_Objest.Папка = new Склади_Папки_Pointer();
				склади_Objest.Save();
			}

			//
			//Вкладені папки видяляються. Для кожної папки буде викликана функція BeforeDelete
			//

			Склади_Папки_Select cклади_Папки_Select = new Склади_Папки_Select();
			cклади_Папки_Select.QuerySelect.Where.Add(new Where(Склади_Папки_Const.Родич, Comparison.EQ, ДовідникОбєкт.UnigueID.UGuid));
			cклади_Папки_Select.Select();

			while (cклади_Папки_Select.MoveNext())
			{
				Склади_Папки_Objest cклади_Папки_Objest = cклади_Папки_Select.Current.GetDirectoryObject();
				if (cклади_Папки_Objest != null)
					cклади_Папки_Objest.Delete();

			}
		}
	}
}

namespace StorageAndTrade_1_0.Документи
{

	class ЗамовленняКлієнта_Triggers
    {
		/// <summary>
		/// Перед записом
		/// </summary>
		/// <param name="ДокументОбєкт"></param>
		public static void BeforeRecording(ЗамовленняКлієнта_Objest ДокументОбєкт)
		{

		}

		/// <summary>
		/// Після запису
		/// </summary>
		/// <param name="ДокументОбєкт"></param>
		public static void AfterRecording(ЗамовленняКлієнта_Objest ДокументОбєкт)
		{
			BeforeDelete(ДокументОбєкт);

			if (ДокументОбєкт.Проведений)
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

			}
		}

		/// <summary>
		/// Перед видаленням
		/// </summary>
		/// <param name="ДокументОбєкт"></param>
        public static void BeforeDelete(ЗамовленняКлієнта_Objest ДокументОбєкт)
		{
			РегістриНакопичення.ЗамовленняКлієнтів_RecordsSet замовленняКлієнтів_RecordsSet = new РегістриНакопичення.ЗамовленняКлієнтів_RecordsSet();
			замовленняКлієнтів_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);

			РегістриНакопичення.РозрахункиЗКлієнтами_RecordsSet розрахункиЗКлієнтами_RecordsSet = new РегістриНакопичення.РозрахункиЗКлієнтами_RecordsSet();
			розрахункиЗКлієнтами_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);

			РегістриНакопичення.ВільніЗалишки_RecordsSet вільніЗалишки_RecordsSet = new РегістриНакопичення.ВільніЗалишки_RecordsSet();
			вільніЗалишки_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);
		}

	}

	class РеалізаціяТоварівТаПослуг_Triggers
    {
		public static void BeforeRecording(РеалізаціяТоварівТаПослуг_Objest ДокументОбєкт)
		{

		}

		public static void AfterRecording(РеалізаціяТоварівТаПослуг_Objest ДокументОбєкт)
		{
			BeforeDelete(ДокументОбєкт);

			if (ДокументОбєкт.Проведений)
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
			}
        }

        public static void BeforeDelete(РеалізаціяТоварівТаПослуг_Objest ДокументОбєкт)
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

	class ПоступленняТоварівТаПослуг_Triggers
	{

		public static void BeforeRecording(ПоступленняТоварівТаПослуг_Objest ДокументОбєкт)
		{

		}

		public static void AfterRecording(ПоступленняТоварівТаПослуг_Objest ДокументОбєкт)
		{
			BeforeDelete(ДокументОбєкт);

			if (ДокументОбєкт.Проведений)
			{
				#region Рух по регістрах

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

				РегістриНакопичення.РозрахункиЗПостачальниками_RecordsSet.Record recordContragent = new РегістриНакопичення.РозрахункиЗПостачальниками_RecordsSet.Record();
				розрахункиЗПостачальниками_RecordsSet.Records.Add(recordContragent);

				recordContragent.Income = false; // -
				recordContragent.Owner = ДокументОбєкт.UnigueID.UGuid;

				recordContragent.Контрагент = ДокументОбєкт.Контрагент;
				recordContragent.Валюта = ДокументОбєкт.Валюта;
				recordContragent.Сума = ДокументОбєкт.СумаДокументу;

				//record.ГосподарськаОперація = ДокументОбєкт.ГосподарськаОперація;
				//record.ФормаОплати = ДокументОбєкт.ФормаОплати;

				розрахункиЗПостачальниками_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

                #endregion
            }
        }

		public static void BeforeDelete(ПоступленняТоварівТаПослуг_Objest ДокументОбєкт)
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

	class Записи_Triggers
    {


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
		}

		public static void ЗамовленняПостачальнику_BeforeDelete(ЗамовленняПостачальнику_Objest ДокументОбєкт)
		{

		}

		#endregion

		

		#region ПоверненняТоварівВідКлієнта

		public static void ПоверненняТоварівВідКлієнта_BeforeRecording(ПоверненняТоварівВідКлієнта_Objest ДокументОбєкт)
		{

		}

		public static void ПоверненняТоварівВідКлієнта_AfterRecording(ПоверненняТоварівВідКлієнта_Objest ДокументОбєкт)
        {
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


		}

		public static void ПоверненняТоварівВідКлієнта_BeforeDelete(ПоверненняТоварівВідКлієнта_Objest ДокументОбєкт)
		{

		}

		#endregion

		#region ПоверненняТоварівПостачальнику

		public static void ПоверненняТоварівПостачальнику_BeforeRecording(ПоверненняТоварівПостачальнику_Objest ДокументОбєкт)
		{

		}

		public static void ПоверненняТоварівПостачальнику_AfterRecording(ПоверненняТоварівПостачальнику_Objest ДокументОбєкт)
		{
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
		}

		public static void ПоверненняТоварівПостачальнику_BeforeDelete(ПоверненняТоварівПостачальнику_Objest ДокументОбєкт)
		{

		}

		#endregion
	}
}