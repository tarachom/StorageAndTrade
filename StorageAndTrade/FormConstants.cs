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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using AccountingSoftware;
using Конфа = StorageAndTrade_1_0;
using Константи = StorageAndTrade_1_0.Константи;
using Довідники = StorageAndTrade_1_0.Довідники;
using Перелічення = StorageAndTrade_1_0.Перелічення;

namespace StorageAndTrade
{
    public partial class FormConstants : Form
    {
        public FormConstants()
        {
            InitializeComponent();
        }

        private void FormConstants_Load(object sender, EventArgs e)
        {
            //Перечитати всі константи
            Конфа.Config.ReadAllConstants();

            ConfigurationEnums ТипПеріодуДляЖурналівДокументів = Конфа.Config.Kernel.Conf.Enums["ТипПеріодуДляЖурналівДокументів"];

            foreach (ConfigurationEnumField field in ТипПеріодуДляЖурналівДокументів.Fields.Values)
                comboBox_ТипПеріодуДляЖурналівДокументів.Items.Add(
                    new NameValue<Перелічення.ТипПеріодуДляЖурналівДокументів>(field.Desc, (Перелічення.ТипПеріодуДляЖурналівДокументів)field.Value));

            ConfigurationEnums МетодиСписанняПартій = Конфа.Config.Kernel.Conf.Enums["МетодиСписанняПартій"];

            foreach (ConfigurationEnumField field in МетодиСписанняПартій.Fields.Values)
                comboBox_МетодиСписанняПартій.Items.Add(
                    new NameValue<Перелічення.МетодиСписанняПартій>(field.Desc, (Перелічення.МетодиСписанняПартій)field.Value));


            //
            //
            //

            directoryControl_Організація.Init(new Form_Організації(), new Довідники.Організації_Pointer());
            directoryControl_Склад.Init(new Form_Склади(), new Довідники.Склади_Pointer());
            directoryControl_Валюта.Init(new Form_Валюти(), new Довідники.Валюти_Pointer());
            directoryControl_Постачальник.Init(new Form_Контрагенти(), new Довідники.Контрагенти_Pointer());
            directoryControl_Покупець.Init(new Form_Контрагенти(), new Довідники.Контрагенти_Pointer());
            directoryControl_Каса.Init(new Form_Каси(), new Довідники.Каси_Pointer());
            directoryControl_ОдиницяПакування.Init(new Form_ПакуванняОдиниціВиміру(), new Довідники.ПакуванняОдиниціВиміру_Pointer());
            directoryControl_Підрозділ.Init(new Form_СтруктураПідприємства(), new Довідники.СтруктураПідприємства_Pointer());
            directoryControl_БанківськийРахунок.Init(new Form_БанківськіРахункиОрганізацій(), new Довідники.БанківськіРахункиОрганізацій_Pointer());
            directoryControl_ВидЦіни.Init(new Form_ВидиЦін(), new Довідники.ВидиЦін_Pointer());

            //
            //
            //

            directoryControl_Організація.DirectoryPointerItem = Константи.ЗначенняЗаЗамовчуванням.ОсновнаОрганізація_Const;
            directoryControl_Склад.DirectoryPointerItem = Константи.ЗначенняЗаЗамовчуванням.ОснонийСклад_Const;
            directoryControl_Валюта.DirectoryPointerItem = Константи.ЗначенняЗаЗамовчуванням.ОсновнаВалюта_Const;
            directoryControl_Постачальник.DirectoryPointerItem = Константи.ЗначенняЗаЗамовчуванням.ОсновнийПостачальник_Const;
            directoryControl_Покупець.DirectoryPointerItem = Константи.ЗначенняЗаЗамовчуванням.ОсновнийПокупець_Const;
            directoryControl_Каса.DirectoryPointerItem = Константи.ЗначенняЗаЗамовчуванням.ОсновнаКаса_Const;
            directoryControl_ОдиницяПакування.DirectoryPointerItem = Константи.ЗначенняЗаЗамовчуванням.ОсновнаОдиницяПакування_Const;
            directoryControl_Підрозділ.DirectoryPointerItem = Константи.ЗначенняЗаЗамовчуванням.ОсновнийПідрозділ_Const;
            directoryControl_БанківськийРахунок.DirectoryPointerItem = Константи.ЗначенняЗаЗамовчуванням.ОсновнийБанківськийРахунок_Const;
            directoryControl_ВидЦіни.DirectoryPointerItem = Константи.ЗначенняЗаЗамовчуванням.ОсновнийВидЦіни_Const;

            ComboBoxNameValue<Перелічення.ТипПеріодуДляЖурналівДокументів>.SelectItem(comboBox_ТипПеріодуДляЖурналівДокументів, Константи.ЖурналиДокументів.ОсновнийТипПеріоду_Const);
            ComboBoxNameValue<Перелічення.МетодиСписанняПартій>.SelectItem(comboBox_МетодиСписанняПартій, Константи.ПартіїТоварів.МетодСписанняПартій_Const);
        }

        void Save()
        {
            Константи.ЗначенняЗаЗамовчуванням.ОсновнаОрганізація_Const = (Довідники.Організації_Pointer)directoryControl_Організація.DirectoryPointerItem;
            Константи.ЗначенняЗаЗамовчуванням.ОснонийСклад_Const = (Довідники.Склади_Pointer)directoryControl_Склад.DirectoryPointerItem;
            Константи.ЗначенняЗаЗамовчуванням.ОсновнаВалюта_Const = (Довідники.Валюти_Pointer)directoryControl_Валюта.DirectoryPointerItem;
            Константи.ЗначенняЗаЗамовчуванням.ОсновнийПостачальник_Const = (Довідники.Контрагенти_Pointer)directoryControl_Постачальник.DirectoryPointerItem;
            Константи.ЗначенняЗаЗамовчуванням.ОсновнийПокупець_Const = (Довідники.Контрагенти_Pointer)directoryControl_Покупець.DirectoryPointerItem;
            Константи.ЗначенняЗаЗамовчуванням.ОсновнаКаса_Const = (Довідники.Каси_Pointer)directoryControl_Каса.DirectoryPointerItem;
            Константи.ЗначенняЗаЗамовчуванням.ОсновнаОдиницяПакування_Const = (Довідники.ПакуванняОдиниціВиміру_Pointer)directoryControl_ОдиницяПакування.DirectoryPointerItem;
            Константи.ЗначенняЗаЗамовчуванням.ОсновнийПідрозділ_Const = (Довідники.СтруктураПідприємства_Pointer)directoryControl_Підрозділ.DirectoryPointerItem;
            Константи.ЗначенняЗаЗамовчуванням.ОсновнийБанківськийРахунок_Const = (Довідники.БанківськіРахункиОрганізацій_Pointer)directoryControl_БанківськийРахунок.DirectoryPointerItem;
            Константи.ЗначенняЗаЗамовчуванням.ОсновнийВидЦіни_Const = (Довідники.ВидиЦін_Pointer)directoryControl_ВидЦіни.DirectoryPointerItem;

            if (comboBox_ТипПеріодуДляЖурналівДокументів.SelectedIndex >= 0)
                Константи.ЖурналиДокументів.ОсновнийТипПеріоду_Const = ((NameValue<Перелічення.ТипПеріодуДляЖурналівДокументів>)comboBox_ТипПеріодуДляЖурналівДокументів.SelectedItem).Value;

            if (comboBox_МетодиСписанняПартій.SelectedIndex >= 0)
                Константи.ПартіїТоварів.МетодСписанняПартій_Const = ((NameValue<Перелічення.МетодиСписанняПартій>)comboBox_МетодиСписанняПартій.SelectedItem).Value;
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            Save();
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonSaveAndClose_Click(object sender, EventArgs e)
        {
            Save();
            this.Close();
        }
    }
}
