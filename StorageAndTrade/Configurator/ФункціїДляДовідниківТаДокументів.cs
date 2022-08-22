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

/*
 
Модуль ...

*/

using System;
using System.Collections.Generic;
using System.Windows.Forms;

using AccountingSoftware;
using StorageAndTrade_1_0;
using StorageAndTrade_1_0.Константи;

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

    class ФункціїДляДовідниківТаДокументів
    {
        public static void ВиділитиЕлементСписку(DataGridView gridView, string columnName, string columnValue)
        {
            if (gridView.Rows.Count > 0)
            {
                gridView.Rows[0].Selected = false;

                foreach (DataGridViewRow row in gridView.Rows)
                {
                    if (row.Cells[columnName].Value.ToString() == columnValue)
                    {
                        row.Selected = true;
                        gridView.FirstDisplayedScrollingRowIndex = row.Index;
                        break;
                    }
                }
            }
        }

        public static void ВиділитиОстаннійЕлементСписку(DataGridView gridView)
        {
            if (gridView.Rows.Count > 0)
            {
                gridView.Rows[0].Selected = false;

                DataGridViewRow row = gridView.Rows[gridView.Rows.Count - 1];

                row.Selected = true;
                gridView.FirstDisplayedScrollingRowIndex = row.Index;
            }
        }
    }
}

