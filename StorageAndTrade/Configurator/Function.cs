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
 
Модуль ...

*/

using System;
using System.Collections.Generic;
using AccountingSoftware;
using StorageAndTrade_1_0;
using StorageAndTrade_1_0.Константи;

namespace StorageAndTrade
{
    /// <summary>
    /// Структура для обмеження кількості завантажених даних
    /// для журналів і автоматичної підгрузки при прокрутці
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

}

namespace StorageAndTrade_1_0
{ 
    public static class Function
    {
        /// <summary>
        /// Функція добавляє в таблицю фонових задач
        /// нову задачу для обчислення залишків по регістрах
        /// </summary>
        /// <param name="registerName">Назва регістру</param>
        /// <param name="groupName">Група (Місяць, День)</param>
        /// <param name="typeMovement">Тип руху по регістру (добавлення, видалення)</param>
        /// <param name="periodCalculation">Період розрахунку</param>
        /// <param name="userName">Користувач</param>
        public static void AddBackgroundTask_CalculationVirtualBalances(
            string[] registerName, string groupName, string typeMovement, DateTime periodCalculation, string userName)
        {
            Системні.ФоновіЗадачі_ОбчисленняВіртуальнихЗалишків_TablePart обчисленняВіртуальнихЗалишків_TablePart =
                new Системні.ФоновіЗадачі_ОбчисленняВіртуальнихЗалишків_TablePart();

            foreach (string register in registerName)
            {
                Системні.ФоновіЗадачі_ОбчисленняВіртуальнихЗалишків_TablePart.Record record = new Системні.ФоновіЗадачі_ОбчисленняВіртуальнихЗалишків_TablePart.Record();
                record.НазваРегістру = register;
                record.ПеріодОбчислення = periodCalculation;
                record.ГрупаОбчислення = groupName;
                record.ТипРухуПоРегістру = typeMovement;
                record.Користувач = userName;
                record.Дата = DateTime.Now;

                обчисленняВіртуальнихЗалишків_TablePart.Records.Add(record);
            }

            обчисленняВіртуальнихЗалишків_TablePart.Save(false);
        }
    }
}

