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
 

*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

using AccountingSoftware;
using Конфа = StorageAndTrade_1_0;
using Довідники = StorageAndTrade_1_0.Довідники;
using Документи = StorageAndTrade_1_0.Документи;
using Перелічення = StorageAndTrade_1_0.Перелічення;

namespace StorageAndTrade
{
    /// <summary>
    /// Спільні функції для довідників
    /// </summary>
    class ФункціїДляДовідників
    {
        public static void ВиділитиЕлементСписку(DataGridView gridView, string columnName, string columnValue)
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
}
