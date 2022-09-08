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
using РегістриВідомостей = StorageAndTrade_1_0.РегістриВідомостей;
using StorageAndTrade_1_0;
using StorageAndTrade_1_0.Довідники;
using StorageAndTrade_1_0.Константи;
using StorageAndTrade_1_0.РегістриВідомостей;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace StorageAndTrade.СпільніФорми
{
    public partial class Form_ПідбірПоШтрихКоду : Form
    {
        public Form_ПідбірПоШтрихКоду()
        {
            InitializeComponent();

            RecordsBindingList = new BindingList<Записи>();
            dataGridViewRecords.DataSource = RecordsBindingList;

            dataGridViewRecords.Columns["Image"].Width = 30;
            dataGridViewRecords.Columns["Image"].HeaderText = "";

            dataGridViewRecords.Columns["Штрихкод"].Width = 150;

            dataGridViewRecords.Columns["Номенклатура"].Visible = false;
            dataGridViewRecords.Columns["НоменклатураНазва"].Width = 300;
            dataGridViewRecords.Columns["НоменклатураНазва"].HeaderText = "Номенклатура";

            dataGridViewRecords.Columns["Характеристика"].Visible = false;
            dataGridViewRecords.Columns["ХарактеристикаНазва"].Width = 300;
            dataGridViewRecords.Columns["ХарактеристикаНазва"].HeaderText = "Характеристика";

            dataGridViewRecords.Columns["Пакування"].Visible = false;
            dataGridViewRecords.Columns["ПакуванняНазва"].Width = 100;
            dataGridViewRecords.Columns["ПакуванняНазва"].HeaderText = "Пакування";

            dataGridViewRecords.Columns["Кількість"].Width = 80;
            dataGridViewRecords.Columns["Кількість"].ReadOnly = false;
        }

        public Action<List<СписокНоменклатури>> ДодатиТовариВДокумент_CallBack;

        private BindingList<Записи> RecordsBindingList { get; set; }

        private class Записи
        {
            public Записи() { Image = Properties.Resources.doc_text_image; }
            public Bitmap Image { get; set; }
            public string Штрихкод { get; set; }
            public Номенклатура_Pointer Номенклатура { get; set; }
            public string НоменклатураНазва { get; set; }
            public ХарактеристикиНоменклатури_Pointer Характеристика { get; set; }
            public string ХарактеристикаНазва { get; set; }
            public ПакуванняОдиниціВиміру_Pointer Пакування { get; set; }
            public string ПакуванняНазва { get; set; }
            public decimal Кількість { get; set; }

            public static Записи New()
            {
                return new Записи
                {
                    Штрихкод = "",
                    Номенклатура = new Номенклатура_Pointer(),
                    НоменклатураНазва = "",
                    Характеристика = new ХарактеристикиНоменклатури_Pointer(),
                    ХарактеристикаНазва = "",
                    Пакування = new ПакуванняОдиниціВиміру_Pointer(),
                    ПакуванняНазва = "",
                    Кількість = 1
                };
            }

            public static void ПісляЗміни_Номенклатура(Записи запис)
            {
                if (запис.Номенклатура.IsEmpty())
                {
                    запис.НоменклатураНазва = "";
                    return;
                }

                Довідники.Номенклатура_Objest номенклатура_Objest = запис.Номенклатура.GetDirectoryObject();
                if (номенклатура_Objest != null)
                {
                    запис.НоменклатураНазва = номенклатура_Objest.Назва;

                    if (!номенклатура_Objest.ОдиницяВиміру.IsEmpty())
                        запис.Пакування = номенклатура_Objest.ОдиницяВиміру;
                }
                else
                {
                    запис.НоменклатураНазва = "";
                    запис.Пакування = new Довідники.ПакуванняОдиниціВиміру_Pointer();
                }

                if (!запис.Пакування.IsEmpty())
                {
                    запис.ПакуванняНазва = запис.Пакування.GetPresentation();
                }
            }
            public static void ПісляЗміни_Характеристика(Записи запис)
            {
                запис.ХарактеристикаНазва = запис.Характеристика.GetPresentation();
            }
            public static void ПісляЗміни_Пакування(Записи запис)
            {
                запис.ПакуванняНазва = запис.Пакування.GetPresentation();
            }
        }

        #region Вибір, Пошук, Зміна

        private void dataGridViewRecords_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                dataGridViewRecords_CellDoubleClick(sender,
                    new DataGridViewCellEventArgs(dataGridViewRecords.CurrentCell.ColumnIndex, dataGridViewRecords.CurrentCell.RowIndex));
            else if (e.KeyCode == Keys.Delete)
            {
                if (dataGridViewRecords.CurrentCell == null)
                    return;

                string columnName = dataGridViewRecords.Columns[dataGridViewRecords.CurrentCell.ColumnIndex].Name;
                Записи запис = RecordsBindingList[dataGridViewRecords.CurrentCell.RowIndex];

                switch (columnName)
                {
                    case "НоменклатураНазва":
                        {
                            запис.Номенклатура = new Довідники.Номенклатура_Pointer();
                            Записи.ПісляЗміни_Номенклатура(запис);
                            break;
                        }
                    case "ХарактеристикаНазва":
                        {
                            запис.Характеристика = new Довідники.ХарактеристикиНоменклатури_Pointer();
                            Записи.ПісляЗміни_Характеристика(запис);
                            break;
                        }
                    case "ПакуванняНазва":
                        {
                            запис.Пакування = new Довідники.ПакуванняОдиниціВиміру_Pointer();
                            Записи.ПісляЗміни_Пакування(запис);
                            break;
                        }
                    default:
                        break;
                }

                dataGridViewRecords.Refresh();
            }
        }

        private void dataGridViewRecords_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex >= 0 && e.RowIndex >= 0)
                ФункціїДляДокументів.ВідкритиМенюВибору(dataGridViewRecords, e.ColumnIndex, e.RowIndex, RecordsBindingList[e.RowIndex],
                    new string[] { "НоменклатураНазва", "ХарактеристикаНазва", "ПакуванняНазва" }, SelectClick, FindTextChanged);
        }

        private void SelectClick(object sender, EventArgs e)
        {
            ToolStripMenuItem selectMenu = (ToolStripMenuItem)sender;
            Записи запис = (Записи)selectMenu.Tag;

            switch (selectMenu.Name)
            {
                case "НоменклатураНазва":
                    {
                        Form_Номенклатура form_Номенклатура = new Form_Номенклатура();
                        form_Номенклатура.DirectoryPointerItem = запис.Номенклатура;
                        form_Номенклатура.ShowDialog();

                        запис.Номенклатура = (Довідники.Номенклатура_Pointer)form_Номенклатура.DirectoryPointerItem;
                        Записи.ПісляЗміни_Номенклатура(запис);

                        break;
                    }
                case "ХарактеристикаНазва":
                    {
                        Form_ХарактеристикиНоменклатури form_ХарактеристикиНоменклатури = new Form_ХарактеристикиНоменклатури();
                        form_ХарактеристикиНоменклатури.DirectoryPointerItem = запис.Характеристика;
                        form_ХарактеристикиНоменклатури.НоменклатураВласник = запис.Номенклатура;
                        form_ХарактеристикиНоменклатури.ShowDialog();

                        запис.Характеристика = (Довідники.ХарактеристикиНоменклатури_Pointer)form_ХарактеристикиНоменклатури.DirectoryPointerItem;
                        Записи.ПісляЗміни_Характеристика(запис);

                        break;
                    }
                case "ПакуванняНазва":
                    {
                        Form_ПакуванняОдиниціВиміру form_ПакуванняОдиниціВиміру = new Form_ПакуванняОдиниціВиміру();
                        form_ПакуванняОдиниціВиміру.DirectoryPointerItem = запис.Пакування;
                        form_ПакуванняОдиниціВиміру.ShowDialog();

                        запис.Пакування = (Довідники.ПакуванняОдиниціВиміру_Pointer)form_ПакуванняОдиниціВиміру.DirectoryPointerItem;
                        Записи.ПісляЗміни_Пакування(запис);

                        break;
                    }
                default:
                    break;
            }

            dataGridViewRecords.Refresh();
        }

        private void FindTextChanged(object sender, EventArgs e)
        {
            ToolStripTextBox findMenu = (ToolStripTextBox)sender;
            Записи запис = (Записи)findMenu.Tag;

            ToolStrip parent = findMenu.GetCurrentParent();

            ФункціїДляДокументів.ОчиститиМенюПошуку(parent);

            string findText = findMenu.Text.TrimStart();

            if (String.IsNullOrWhiteSpace(findMenu.Text))
                findText = "%";

            string query = "";

            switch (findMenu.Name)
            {
                case "НоменклатураНазва":
                    {
                        query = ПошуковіЗапити.Номенклатура;
                        break;
                    }
                case "ХарактеристикаНазва":
                    {
                        query = ПошуковіЗапити.ХарактеристикаНоменклатуриЗВідбором(запис.Номенклатура);
                        break;
                    }
                case "ПакуванняНазва":
                    {
                        query = ПошуковіЗапити.ПакуванняОдиниціВиміру;
                        break;
                    }
                default:
                    return;
            }

            ФункціїДляДокументів.ЗаповнитиМенюПошуку(parent, query, findText, findMenu.Name, запис, FindClick);
        }

        private void FindClick(object sender, EventArgs e)
        {
            ToolStripMenuItem selectMenu = (ToolStripMenuItem)sender;
            NameValue<object> nameValue = (NameValue<object>)selectMenu.Tag;

            string uid = nameValue.Name;
            Записи запис = (Записи)nameValue.Value;

            switch (selectMenu.Name)
            {
                case "НоменклатураНазва":
                    {
                        запис.Номенклатура = new Довідники.Номенклатура_Pointer(new UnigueID(uid));
                        Записи.ПісляЗміни_Номенклатура(запис);

                        break;
                    }
                case "ХарактеристикаНазва":
                    {
                        запис.Характеристика = new Довідники.ХарактеристикиНоменклатури_Pointer(new UnigueID(uid));
                        Записи.ПісляЗміни_Характеристика(запис);
                        break;
                    }               
                default:
                    break;
            }

            dataGridViewRecords.Refresh();
        }

        #endregion

        private void ДодатиШтрихКод()
        {
            string Штрихкод = textBox_ШтрихКод.Text.Trim();

            foreach (Записи запис in RecordsBindingList)
            {
                if (запис.Штрихкод == Штрихкод)
                {
                    запис.Кількість++;

                    dataGridViewRecords.Refresh();
                    return;
                }
            }

            Записи записНовий = Записи.New();
            записНовий.Штрихкод = Штрихкод;

            string query = $@"
SELECT
    Регістр_ШтрихкодиНоменклатури.{ШтрихкодиНоменклатури_Const.Номенклатура} AS Номенклатура,
    Довідник_Номенклатура.{Номенклатура_Const.Назва} AS НоменклатураНазва,
    Регістр_ШтрихкодиНоменклатури.{ШтрихкодиНоменклатури_Const.ХарактеристикаНоменклатури} AS Характеристика,
    Довідник_ХарактеристикиНоменклатури.{ХарактеристикиНоменклатури_Const.Назва} AS ХарактеристикаНазва,
    Регістр_ШтрихкодиНоменклатури.{ШтрихкодиНоменклатури_Const.Пакування} AS Пакування,
    Довідник_ПакуванняОдиниціВиміру.{ПакуванняОдиниціВиміру_Const.Назва} AS ПакуванняНазва
FROM
    {ШтрихкодиНоменклатури_Const.TABLE} AS Регістр_ШтрихкодиНоменклатури

    LEFT JOIN {Номенклатура_Const.TABLE} AS Довідник_Номенклатура ON Довідник_Номенклатура.uid = 
        Регістр_ШтрихкодиНоменклатури.{ШтрихкодиНоменклатури_Const.Номенклатура}

    LEFT JOIN {ХарактеристикиНоменклатури_Const.TABLE} AS Довідник_ХарактеристикиНоменклатури ON Довідник_ХарактеристикиНоменклатури.uid = 
        Регістр_ШтрихкодиНоменклатури.{ШтрихкодиНоменклатури_Const.ХарактеристикаНоменклатури}

    LEFT JOIN {ПакуванняОдиниціВиміру_Const.TABLE} AS Довідник_ПакуванняОдиниціВиміру ON Довідник_ПакуванняОдиниціВиміру.uid = 
        Регістр_ШтрихкодиНоменклатури.{ШтрихкодиНоменклатури_Const.Пакування}
WHERE
    Регістр_ШтрихкодиНоменклатури.{ШтрихкодиНоменклатури_Const.Штрихкод} = @Штрихкод
LIMIT 1
";
            Dictionary<string, object> paramQuery = new Dictionary<string, object>();
            paramQuery.Add("Штрихкод", записНовий.Штрихкод);

            string[] columnsName;
            List<Dictionary<string, object>> listRow;

            Config.Kernel.DataBase.SelectRequest(query, paramQuery, out columnsName, out listRow);

            if (listRow.Count == 1)
            {
                Dictionary<string, object> Рядок = listRow[0];

                записНовий.Номенклатура = new Номенклатура_Pointer(new UnigueID(Рядок["Номенклатура"]));
                записНовий.НоменклатураНазва = Рядок["НоменклатураНазва"].ToString();
                записНовий.Характеристика = new ХарактеристикиНоменклатури_Pointer(new UnigueID(Рядок["Характеристика"]));
                записНовий.ХарактеристикаНазва = Рядок["ХарактеристикаНазва"].ToString();
                записНовий.Пакування = new ПакуванняОдиниціВиміру_Pointer(new UnigueID(Рядок["Пакування"]));
                записНовий.ПакуванняНазва = Рядок["ПакуванняНазва"].ToString();
            }

            RecordsBindingList.Add(записНовий);
        }

        private void textBox_ШтрихКод_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ДодатиШтрихКод();

                textBox_ШтрихКод.Text = "";
            }
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            string query = $@"
SELECT
    Регістр_ШтрихкодиНоменклатури.uid
FROM
    {ШтрихкодиНоменклатури_Const.TABLE} AS Регістр_ШтрихкодиНоменклатури
WHERE
    Регістр_ШтрихкодиНоменклатури.{ШтрихкодиНоменклатури_Const.Штрихкод} = @Штрихкод
LIMIT 1
";
            List<СписокНоменклатури> СписокНоменклатуриЗПідбору = new List<СписокНоменклатури>();

            foreach (Записи запис in RecordsBindingList)
            {
                Dictionary<string, object> paramQuery = new Dictionary<string, object>();
                paramQuery.Add("Штрихкод", запис.Штрихкод);

                string[] columnsName;
                List<Dictionary<string, object>> listRow;

                Config.Kernel.DataBase.SelectRequest(query, paramQuery, out columnsName, out listRow);

                ШтрихкодиНоменклатури_Objest штрихкодиНоменклатури_Objest = new ШтрихкодиНоменклатури_Objest();

                if (listRow.Count == 1)
                {
                    Dictionary<string, object> Рядок = listRow[0];

                    if (штрихкодиНоменклатури_Objest.Read(new UnigueID(Рядок["uid"])))
                    {
                        штрихкодиНоменклатури_Objest.Номенклатура = запис.Номенклатура;
                        штрихкодиНоменклатури_Objest.ХарактеристикаНоменклатури = запис.Характеристика;
                        штрихкодиНоменклатури_Objest.Пакування = запис.Пакування;

                        штрихкодиНоменклатури_Objest.Save();
                    }
                }
                else if (listRow.Count == 0)
                {
                    штрихкодиНоменклатури_Objest.New();

                    штрихкодиНоменклатури_Objest.Штрихкод = запис.Штрихкод;
                    штрихкодиНоменклатури_Objest.Номенклатура = запис.Номенклатура;
                    штрихкодиНоменклатури_Objest.ХарактеристикаНоменклатури = запис.Характеристика;
                    штрихкодиНоменклатури_Objest.Пакування = запис.Пакування;

                    штрихкодиНоменклатури_Objest.Save();
                }

                СписокНоменклатури СписокНоменклатуриЗапис = new СписокНоменклатури();
                СписокНоменклатуриЗапис.Номенклатура = запис.Номенклатура;
                СписокНоменклатуриЗапис.Характеристика = запис.Характеристика;
                СписокНоменклатуриЗапис.Пакування = запис.Пакування;
                СписокНоменклатуриЗапис.Кількість = запис.Кількість;

                СписокНоменклатуриЗПідбору.Add(СписокНоменклатуриЗапис);
            }

            if (ДодатиТовариВДокумент_CallBack != null)
            {
                ДодатиТовариВДокумент_CallBack.Invoke(СписокНоменклатуриЗПідбору);
                this.Close();
            }
        }

        private void Form_ПідбірПоШтрихКоду_Load(object sender, EventArgs e)
        {

        }
    }

    /// <summary>
    /// Клас для передачі даних з підбору
    /// </summary>
    public class СписокНоменклатури
    {
        public Номенклатура_Pointer Номенклатура { get; set; }
        public ХарактеристикиНоменклатури_Pointer Характеристика { get; set; }
        public ПакуванняОдиниціВиміру_Pointer Пакування { get; set; }
        public decimal Кількість { get; set; }
    }
}
