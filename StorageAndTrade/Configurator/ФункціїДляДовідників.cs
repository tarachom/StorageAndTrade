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
using Константи = StorageAndTrade_1_0.Константи;

namespace StorageAndTrade
{
    /// <summary>
    /// Спільні функції для довідників
    /// </summary>
    class ФункціїДляДовідників
    {
        public static void СтворитиДоговориКонтрагентаЗаЗамовчуванням(Довідники.Контрагенти_Pointer Контрагент)
        {
            if (Контрагент.IsEmpty())
                return;

            Довідники.ДоговориКонтрагентів_Objest НовийДоговір = new Довідники.ДоговориКонтрагентів_Objest();
            НовийДоговір.Назва = "Основний договір";
            НовийДоговір.Контрагент = Контрагент;
            НовийДоговір.Статус = Перелічення.СтатусиДоговорівКонтрагентів.Діє;
            НовийДоговір.Дата = DateTime.Now;

            Довідники.ДоговориКонтрагентів_Select ВибіркаДоговорівКонтрагента = new Довідники.ДоговориКонтрагентів_Select();

            ВибіркаДоговорівКонтрагента.QuerySelect.Where.Add(
                new Where(Довідники.ДоговориКонтрагентів_Const.Контрагент, Comparison.EQ, Контрагент.UnigueID.UGuid));

            ВибіркаДоговорівКонтрагента.QuerySelect.Where.Add(
                new Where(Comparison.AND, Довідники.ДоговориКонтрагентів_Const.ТипДоговору, Comparison.EQ, (int)Перелічення.ТипДоговорів.ЗПокупцями));

            if (!ВибіркаДоговорівКонтрагента.Select())
            {
                НовийДоговір.New();
                НовийДоговір.Код = (++Константи.НумераціяДовідників.Контрагенти_Const).ToString("D6");
                НовийДоговір.ТипДоговору = Перелічення.ТипДоговорів.ЗПокупцями;
                НовийДоговір.ГосподарськаОперація = Перелічення.ГосподарськіОперації.ПоступленняОплатиВідКлієнта;
                НовийДоговір.Save();
            }

            ВибіркаДоговорівКонтрагента.QuerySelect.Where[1].Value = (int)Перелічення.ТипДоговорів.ЗПостачальниками;

            if (!ВибіркаДоговорівКонтрагента.Select())
            {
                НовийДоговір.New();
                НовийДоговір.Код = (++Константи.НумераціяДовідників.Контрагенти_Const).ToString("D6");
                НовийДоговір.ТипДоговору = Перелічення.ТипДоговорів.ЗПостачальниками;
                НовийДоговір.ГосподарськаОперація = Перелічення.ГосподарськіОперації.ОплатаПостачальнику;
                НовийДоговір.Save();
            }
        }
    }
}
