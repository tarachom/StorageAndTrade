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

        private void buttonCreate_Click(object sender, EventArgs e)
        {
            Configuration Conf = Config.Kernel.Conf;

            ConfigurationRegistersAccumulation Регістр_ЗамовленняКлієнтів = Conf.RegistersAccumulation["ЗамовленняКлієнтів"];
            ConfigurationDirectories Довідник_Номенклатура = Conf.Directories["Номенклатура"];
            ConfigurationDirectories Довідник_ХарактеристикиНоменклатури = Conf.Directories["ХарактеристикиНоменклатури"];

            string query = $@"
SELECT 
    Рег_ЗамовленняКлієнтів.{ЗамовленняКлієнтів_RecordsSet.Номенклатура} AS Номенклатура, 
    Довідник_Номенклатура.{Номенклатура_Select.Назва} AS Номенклатура_Назва, 
    Рег_ЗамовленняКлієнтів.{ЗамовленняКлієнтів_RecordsSet.ХарактеристикаНоменклатури} AS ХарактеристикаНоменклатури,
    Довідник_ХарактеристикиНоменклатури.{ХарактеристикиНоменклатури_Select.Назва} AS ХарактеристикаНоменклатури_Назва,

    SUM(CASE WHEN Рег_ЗамовленняКлієнтів.income = true THEN 
        Рег_ЗамовленняКлієнтів.{ЗамовленняКлієнтів_RecordsSet.Замовлено} ELSE -Рег_ЗамовленняКлієнтів.{ЗамовленняКлієнтів_RecordsSet.Замовлено} END) AS Замовлено,

    SUM(CASE WHEN Рег_ЗамовленняКлієнтів.income = true THEN 
        Рег_ЗамовленняКлієнтів.{ЗамовленняКлієнтів_RecordsSet.Сума} ELSE -Рег_ЗамовленняКлієнтів.{ЗамовленняКлієнтів_RecordsSet.Сума} END) AS Сума
FROM 
    {Регістр_ЗамовленняКлієнтів.Table} AS Рег_ЗамовленняКлієнтів

    LEFT JOIN {Довідник_Номенклатура.Table} AS Довідник_Номенклатура ON Довідник_Номенклатура.uid = 
        Рег_ЗамовленняКлієнтів.{ЗамовленняКлієнтів_RecordsSet.Номенклатура}

    LEFT JOIN {Довідник_ХарактеристикиНоменклатури.Table} AS Довідник_ХарактеристикиНоменклатури ON Довідник_ХарактеристикиНоменклатури.uid = 
        Рег_ЗамовленняКлієнтів.{ЗамовленняКлієнтів_RecordsSet.ХарактеристикаНоменклатури}

GROUP BY Номенклатура, Номенклатура_Назва, ХарактеристикаНоменклатури, ХарактеристикаНоменклатури_Назва
ORDER BY Номенклатура_Назва
";

            Console.WriteLine(query);

            Dictionary<string, object> paramQuery = new Dictionary<string, object>();
            //paramQuery.Add("ЗамовленняКлієнта", ДокументВказівник.UnigueID.UGuid);

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
