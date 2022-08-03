using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

//using Gecko;

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
    public partial class Form_Report : Form
    {
        public Form_Report()
        {
            InitializeComponent();
        }

        public string HtmlDocumentPath { get; set; }

        private void Form_Report_Load(object sender, EventArgs e)
        {
            geckoWebBrowser.Navigate(HtmlDocumentPath);

            geckoWebBrowser.DomClick += GeckoWebBrowser_DomClick;
        }

        private void GeckoWebBrowser_DomClick(object sender, Gecko.DomMouseEventArgs e)
        {
            Gecko.GeckoElement geckoElement = e.Target.CastToGeckoElement();
            if (geckoElement.TagName == "A")
            {
                if (geckoElement.GetAttribute("name") == "Довідник.Номенклатура")
                {
                    Form_НоменклатураЕлемент form_НоменклатураЕлемент = new Form_НоменклатураЕлемент();
                    form_НоменклатураЕлемент.MdiParent = this.MdiParent;
                    form_НоменклатураЕлемент.IsNew = false;
                    form_НоменклатураЕлемент.Uid = geckoElement.GetAttribute("id");
                    form_НоменклатураЕлемент.Show();
                }
            }
        }
    }
}
