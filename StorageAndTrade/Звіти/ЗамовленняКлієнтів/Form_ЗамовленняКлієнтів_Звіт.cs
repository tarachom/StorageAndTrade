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

namespace StorageAndTrade
{
    public partial class Form_ЗамовленняКлієнтів_Звіт : Form
    {
        public Form_ЗамовленняКлієнтів_Звіт()
        {
            InitializeComponent();
        }

        private void Form_ЗамовленняКлієнтів_Звіт_Load(object sender, EventArgs e)
        {
            directoryControl_НоменклатураПапка.SelectForm = new Form_НоменклатураПапкиВибір();
            directoryControl_НоменклатураПапка.DirectoryPointerItem = new Номенклатура_Папки_Pointer();

            directoryControl_Номенклатура.SelectForm = new Form_Номенклатура();
            directoryControl_Номенклатура.DirectoryPointerItem = new Номенклатура_Pointer();

            directoryControl_Склади.SelectForm = new Form_Склади();
            directoryControl_Склади.DirectoryPointerItem = new Склади_Pointer();

            documentControl_ЗамовленняКлієнта.SelectForm = new Form_ЗамовленняКлієнтаЖурнал();
            documentControl_ЗамовленняКлієнта.DocumentPointerItem = new ЗамовленняКлієнта_Pointer();
        }

        private void buttonCreate_Click(object sender, EventArgs e)
        {
            string query = $@"
SELECT 
    Рег_ЗамовленняКлієнтів.{ЗамовленняКлієнтів_Const.Номенклатура} AS Номенклатура, 
    Довідник_Номенклатура.{Номенклатура_Const.Назва} AS Номенклатура_Назва, 
    Рег_ЗамовленняКлієнтів.{ЗамовленняКлієнтів_Const.ХарактеристикаНоменклатури} AS ХарактеристикаНоменклатури,
    Довідник_ХарактеристикиНоменклатури.{ХарактеристикиНоменклатури_Const.Назва} AS ХарактеристикаНоменклатури_Назва,
    Рег_ЗамовленняКлієнтів.{ЗамовленняКлієнтів_Const.Склад} AS Склад,
    Довідник_Склади.{Склади_Const.Назва} AS Склад_Назва, 

    SUM(CASE WHEN Рег_ЗамовленняКлієнтів.income = true THEN 
        Рег_ЗамовленняКлієнтів.{ЗамовленняКлієнтів_Const.Замовлено} ELSE -Рег_ЗамовленняКлієнтів.{ЗамовленняКлієнтів_Const.Замовлено} END) AS Замовлено,

    SUM(CASE WHEN Рег_ЗамовленняКлієнтів.income = true THEN 
        Рег_ЗамовленняКлієнтів.{ЗамовленняКлієнтів_Const.Сума} ELSE -Рег_ЗамовленняКлієнтів.{ЗамовленняКлієнтів_Const.Сума} END) AS Сума
FROM 
    {ЗамовленняКлієнтів_Const.TABLE} AS Рег_ЗамовленняКлієнтів

    LEFT JOIN {Номенклатура_Const.TABLE} AS Довідник_Номенклатура ON Довідник_Номенклатура.uid = 
        Рег_ЗамовленняКлієнтів.{ЗамовленняКлієнтів_Const.Номенклатура}

    LEFT JOIN {ХарактеристикиНоменклатури_Const.TABLE} AS Довідник_ХарактеристикиНоменклатури ON Довідник_ХарактеристикиНоменклатури.uid = 
        Рег_ЗамовленняКлієнтів.{ЗамовленняКлієнтів_Const.ХарактеристикаНоменклатури}

    LEFT JOIN {Склади_Const.TABLE} AS Довідник_Склади ON Довідник_Склади.uid = 
        Рег_ЗамовленняКлієнтів.{ЗамовленняКлієнтів_Const.Склад}
";

            //Відбір по всіх вкладених папках вибраної папки Номенклатури
            if (!directoryControl_НоменклатураПапка.DirectoryPointerItem.IsEmpty())
            {
                query += $@"
WHERE 
    Довідник_Номенклатура.{Номенклатура_Const.Папка} IN 
    (
        WITH RECURSIVE r AS 
        (
            SELECT uid
            FROM {Номенклатура_Папки_Const.TABLE}
            WHERE {Номенклатура_Папки_Const.TABLE}.uid = '{directoryControl_НоменклатураПапка.DirectoryPointerItem.UnigueID.UGuid}' 

            UNION ALL

            SELECT {Номенклатура_Папки_Const.TABLE}.uid
            FROM {Номенклатура_Папки_Const.TABLE}
                JOIN r ON {Номенклатура_Папки_Const.TABLE}.{Номенклатура_Папки_Const.Родич} = r.uid
        ) SELECT uid FROM r
    )
";
            }

            //Відбір по вибраному елементу Номенклатура
            if (!directoryControl_Номенклатура.DirectoryPointerItem.IsEmpty())
            {
                query += $@"
WHERE 
    Довідник_Номенклатура.uid = '{directoryControl_Номенклатура.DirectoryPointerItem.UnigueID.UGuid}'
";
            }

            //Відбір по вибраному елементу Склади
            if (!directoryControl_Склади.DirectoryPointerItem.IsEmpty())
            {
                query += $@"
WHERE 
    Довідник_Склади.uid = '{directoryControl_Склади.DirectoryPointerItem.UnigueID.UGuid}'
";
            }

            //Відбір по документу ЗамовленняКлієнта
            if (!documentControl_ЗамовленняКлієнта.DocumentPointerItem.IsEmpty())
            {
                query += $@"
WHERE 
    Рег_ЗамовленняКлієнтів.{ЗамовленняКлієнтів_Const.ЗамовленняКлієнта} = '{documentControl_ЗамовленняКлієнта.DocumentPointerItem.UnigueID.UGuid}'
";
            }

            query += $@"
GROUP BY Номенклатура, Номенклатура_Назва, 
         ХарактеристикаНоменклатури, ХарактеристикаНоменклатури_Назва,
         Склад, Склад_Назва

ORDER BY Номенклатура_Назва
";

            Console.WriteLine(query);

            Dictionary<string, object> paramQuery = new Dictionary<string, object>();

            //if (!directoryControl_НоменклатураПапка.DirectoryPointerItem.IsEmpty())
            //    paramQuery.Add("Папка", directoryControl_НоменклатураПапка.DirectoryPointerItem.UnigueID.UGuid);

            string[] columnsName;
            List<object[]> listRow;

            Config.Kernel.DataBase.SelectRequest(query, paramQuery, out columnsName, out listRow);

            SaveXML(columnsName, listRow);
        }

        public static void SaveXML(string[] columnsName, List<object[]> listRow)
        {
            XmlDocument xmlConfDocument = new XmlDocument();
            xmlConfDocument.AppendChild(xmlConfDocument.CreateXmlDeclaration("1.0", "utf-8", ""));

            XmlElement rootNode = xmlConfDocument.CreateElement("root");
            xmlConfDocument.AppendChild(rootNode);

            int counter;

            foreach (object[] row in listRow)
            {
                counter = 0;

                XmlElement nodeRow = xmlConfDocument.CreateElement("row");
                rootNode.AppendChild(nodeRow);

                foreach (string col in columnsName)
                {
                    XmlElement node = xmlConfDocument.CreateElement(col);
                    node.InnerText = row[counter].ToString();
                    nodeRow.AppendChild(node);

                    counter++;
                }
            }

            xmlConfDocument.Save(@"E:\Project\StorageAndTrade\StorageAndTrade\bin\Debug\SaveXML_Report.xml");

            XslCompiledTransform xsltTransform = new XslCompiledTransform();
            xsltTransform.Load(@"E:\Project\StorageAndTrade\StorageAndTrade\Звіти\ЗамовленняКлієнтів\Template_ЗамовленняКлієнта_Звіт.xslt", new XsltSettings(), null);

            xsltTransform.Transform(@"E:\Project\StorageAndTrade\StorageAndTrade\bin\Debug\SaveXML_Report.xml",
                @"E:\Project\StorageAndTrade\StorageAndTrade\bin\Debug\SaveXML_Report.html");

            System.Diagnostics.Process.Start("firefox.exe", @"E:\Project\StorageAndTrade\StorageAndTrade\bin\Debug\SaveXML_Report.html");
        }

        
    }
}
