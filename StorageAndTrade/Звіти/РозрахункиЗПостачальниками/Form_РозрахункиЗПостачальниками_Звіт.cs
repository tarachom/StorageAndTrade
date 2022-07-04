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
using System.Xml;
using System.Xml.Xsl;

using StorageAndTrade_1_0;
using StorageAndTrade_1_0.Довідники;
using StorageAndTrade_1_0.Документи;
using StorageAndTrade_1_0.РегістриНакопичення;
using StorageAndTrade_1_0.Звіти;

namespace StorageAndTrade
{
    public partial class Form_РозрахункиЗПостачальниками_Звіт : Form
    {
        public Form_РозрахункиЗПостачальниками_Звіт()
        {
            InitializeComponent();
        }

        private void Form_РозрахункиЗПостачальниками_Звіт_Load(object sender, EventArgs e)
        {
            directoryControl_КонтрагентиПапка.Init(new Form_КонтрагентиПапкиВибір(), new Контрагенти_Папки_Pointer());
            directoryControl_Контрагенти.Init(new Form_Контрагенти(), new Контрагенти_Pointer());
            directoryControl_Валюти.Init(new Form_Валюти(), new Валюти_Pointer());
        }

        private void buttonCreate_Click(object sender, EventArgs e)
        {
            bool isExistParent = false;

            string query = $@"
SELECT 
    Рег_РозрахункиЗПостачальниками.{РозрахункиЗПостачальниками_Const.Контрагент} AS Контрагент,
    Довідник_Контрагенти.{Контрагенти_Const.Назва} AS Контрагент_Назва,
    Рег_РозрахункиЗПостачальниками.{РозрахункиЗПостачальниками_Const.Валюта} AS Валюта, 
    Довідник_Валюти.{Валюти_Const.Назва} AS Валюта_Назва,

    SUM(CASE WHEN Рег_РозрахункиЗПостачальниками.income = true THEN 
        Рег_РозрахункиЗПостачальниками.{РозрахункиЗПостачальниками_Const.Сума} ELSE 
        -Рег_РозрахункиЗПостачальниками.{РозрахункиЗПостачальниками_Const.Сума} END) AS Сума
FROM 
    {РозрахункиЗПостачальниками_Const.TABLE} AS Рег_РозрахункиЗПостачальниками

    LEFT JOIN {Валюти_Const.TABLE} AS Довідник_Валюти ON Довідник_Валюти.uid = 
        Рег_РозрахункиЗПостачальниками.{РозрахункиЗПостачальниками_Const.Валюта}

    LEFT JOIN {Контрагенти_Const.TABLE} AS Довідник_Контрагенти ON Довідник_Контрагенти.uid = 
        Рег_РозрахункиЗПостачальниками.{РозрахункиЗПостачальниками_Const.Контрагент}
";
            #region WHERE

            //Відбір по всіх вкладених папках вибраної папки Контрагенти
            if (!directoryControl_КонтрагентиПапка.DirectoryPointerItem.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
Довідник_Контрагенти.{Контрагенти_Const.Папка} IN 
    (
        WITH RECURSIVE r AS 
        (
            SELECT uid
            FROM {Контрагенти_Папки_Const.TABLE}
            WHERE {Контрагенти_Папки_Const.TABLE}.uid = '{directoryControl_КонтрагентиПапка.DirectoryPointerItem.UnigueID}' 

            UNION ALL

            SELECT {Контрагенти_Папки_Const.TABLE}.uid
            FROM {Контрагенти_Папки_Const.TABLE}
                JOIN r ON {Контрагенти_Папки_Const.TABLE}.{Контрагенти_Папки_Const.Родич} = r.uid
        ) SELECT uid FROM r
    )
";
            }

            //Відбір по вибраному елементу Контрагенту
            if (!directoryControl_Контрагенти.DirectoryPointerItem.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
Довідник_Контрагенти.uid = '{directoryControl_Контрагенти.DirectoryPointerItem.UnigueID}'
";
            }

            //Відбір по вибраному елементу Валюти
            if (!directoryControl_Валюти.DirectoryPointerItem.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
Довідник_Валюти.uid = '{directoryControl_Валюти.DirectoryPointerItem.UnigueID}'
";
            }

            #endregion

            query += $@"
GROUP BY Контрагент, Контрагент_Назва, 
         Валюта, Валюта_Назва

HAVING
     SUM(CASE WHEN Рег_РозрахункиЗПостачальниками.income = true THEN 
        Рег_РозрахункиЗПостачальниками.{РозрахункиЗПостачальниками_Const.Сума} ELSE 
        -Рег_РозрахункиЗПостачальниками.{РозрахункиЗПостачальниками_Const.Сума} END) != 0

ORDER BY Контрагент_Назва
";

            //Console.WriteLine(query);
            
            XmlDocument xmlDoc =  Функції.CreateXmlDocument();

            Dictionary<string, object> paramQuery = new Dictionary<string, object>();

            string[] columnsName;
            List<object[]> listRow;

            Config.Kernel.DataBase.SelectRequest(query, paramQuery, out columnsName, out listRow);

            Функції.DataToXML(xmlDoc, "РозрахункиЗПостачальниками", columnsName, listRow);

            Функції.XmlDocumentSaveAndTransform(xmlDoc,
                @"E:\Project\StorageAndTrade\StorageAndTrade\Звіти\РозрахункиЗПостачальниками\Template_РозрахункиЗПостачальниками_Звіт.xslt");
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
