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
    /// Спільні функції для документів
    /// </summary>
    class ФункціїДляДокументів
    {
        public static void ВідкритиМенюВибору(DataGridView gridView, int columnIndex, int rowIndex, object tag, string[] allowColumn, 
            EventHandler selectClick, EventHandler findTextChanged)
        {
            string columnName = gridView.Columns[columnIndex].Name;

            if (!allowColumn.Contains(columnName))
                return;

            Rectangle rectangle = gridView.GetCellDisplayRectangle(columnIndex, rowIndex, true);
            rectangle.Offset(0, 0);
            Point point = gridView.PointToScreen(rectangle.Location);

            ContextMenuStrip contextMenu = new ContextMenuStrip();

            ToolStripMenuItem select = new ToolStripMenuItem("Відкрити список");
            select.Image = Properties.Resources.data;
            select.Name = columnName;
            select.Tag = tag;
            contextMenu.Items.Add(select);

            ToolStripTextBox findTextBox = new ToolStripTextBox();
            findTextBox.ToolTipText = "Пошук";
            findTextBox.Size = new Size(rectangle.Width, 0);
            findTextBox.Name = columnName;
            findTextBox.Tag = tag;
            contextMenu.Items.Add(findTextBox);

            if (findTextChanged != null)
                findTextBox.TextChanged += findTextChanged;

            if (selectClick != null)
                select.Click += selectClick;

            contextMenu.Show(point);
            findTextBox.Focus();
        }

        public static void ОчиститиМенюПошуку(ToolStrip parentMenu)
        {
            for (int counterMenu = parentMenu.Items.Count - 1; counterMenu > 1; counterMenu--)
                parentMenu.Items.RemoveAt(counterMenu);
        }

        public static void ЗаповнитиМенюПошуку(ToolStrip parentMenu, string queryFind, string findText, string name, object tag,
            EventHandler findClick)
        {
            Dictionary<string, object> paramQuery = new Dictionary<string, object>();
            paramQuery.Add("like_param", "%" + findText.ToLower() + "%");

            string[] columnsName;
            List<Dictionary<string, object>> listRow;

            Конфа.Config.Kernel.DataBase.SelectRequest(queryFind, paramQuery, out columnsName, out listRow);

            if (listRow.Count > 0)
            {
                ToolStripItem[] mas = new ToolStripItem[listRow.Count];

                int counter = 0;

                foreach (Dictionary<string, object> row in listRow)
                {
                    mas[counter] = new ToolStripMenuItem(DateTime.Now.ToString() + " " + row["Назва"].ToString(), Properties.Resources.page_white_text, findClick, name);
                    mas[counter].Tag = new NameValue<object>(row["uid"].ToString(), tag);
                    counter++;
                }

                parentMenu.Items.AddRange(mas);
            }
        }

        /// <summary>
        /// Функція повертає перший із списку договорів - договір контрагента
        /// </summary>
        /// <param name="Контрагент">Контрагент</param>
        /// <param name="ТипДоговору">Тип договору</param>
        /// <returns></returns>
        public static Довідники.ДоговориКонтрагентів_Pointer ОсновнийДоговірДляКонтрагента(
            Довідники.Контрагенти_Pointer Контрагент, Перелічення.ТипДоговорів ТипДоговору = 0)
        {
            if (Контрагент == null || Контрагент.IsEmpty())
                return null;

            Довідники.ДоговориКонтрагентів_Select договориКонтрагентів = new Довідники.ДоговориКонтрагентів_Select();

            //Відбір по контрагенту
            договориКонтрагентів.QuerySelect.Where.Add(
                new Where(Довідники.ДоговориКонтрагентів_Const.Контрагент, Comparison.EQ, Контрагент.UnigueID.UGuid));

            if (ТипДоговору != 0)
            {
                //Відбір по типу договору
                договориКонтрагентів.QuerySelect.Where.Add(
                    new Where(Comparison.AND, Довідники.ДоговориКонтрагентів_Const.ТипДоговору, Comparison.EQ, (int)ТипДоговору));
            }

            if (договориКонтрагентів.SelectSingle())
                return договориКонтрагентів.Current;
            else
                return null;
        }

    }
}
