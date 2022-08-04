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
 

*/

using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccountingSoftware;
using Довідники = StorageAndTrade_1_0.Довідники;
using Документи = StorageAndTrade_1_0.Документи;
using Перелічення = StorageAndTrade_1_0.Перелічення;

namespace StorageAndTrade
{
    class GeckoWebBrowser
    {
        public static void DomClick(object sender, Gecko.DomMouseEventArgs e)
        {
            Gecko.GeckoElement geckoElement = e.Target.CastToGeckoElement();
            if (geckoElement.TagName == "A")
            {
                string groupAndName = geckoElement.GetAttribute("name");
                string uid = geckoElement.GetAttribute("id");

                if (!String.IsNullOrEmpty(groupAndName) && !String.IsNullOrEmpty(uid))
                    Open(groupAndName, uid);
            }
        }

        public static void Open(string groupAndName, string uid)
        {
            string[] groupAndNameSplit = groupAndName.Split(new string[] { "." }, StringSplitOptions.None);

            if (groupAndNameSplit.Length != 2)
                return;

            string group = groupAndNameSplit[0];
            string name = groupAndNameSplit[1];

            Form MdiParent = Application.OpenForms["FormStorageAndTrade"];

            if (group == "Довідник")
            {
                switch (name)
                {
                    case "Номенклатура":
                        {
                            Form_НоменклатураЕлемент form_НоменклатураЕлемент = new Form_НоменклатураЕлемент();
                            form_НоменклатураЕлемент.MdiParent = MdiParent;
                            form_НоменклатураЕлемент.IsNew = false;
                            form_НоменклатураЕлемент.Uid = uid;
                            form_НоменклатураЕлемент.Show();

                            break;
                        }
                    default:
                        break;
                }
            }
            else if (group == "Документ")
            {

            } 
        }
    }
}
