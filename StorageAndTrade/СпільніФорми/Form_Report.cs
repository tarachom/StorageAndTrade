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
            geckoWebBrowser.DomClick += GeckoWebBrowser.DomClick;

            //geckoWebBrowser.Navigating += GeckoWebBrowser_Navigating;

            //geckoWebBrowser.Navigated += GeckoWebBrowser_Navigated;

            //geckoWebBrowser.ReadyStateChange += GeckoWebBrowser_ReadyStateChange;

            //geckoWebBrowser.RequestProgressChanged += GeckoWebBrowser_RequestProgressChanged;

            //geckoWebBrowser.DocumentCompleted += GeckoWebBrowser_DocumentCompleted;
        }

        private void GeckoWebBrowser_DocumentCompleted(object sender, Gecko.Events.GeckoDocumentCompletedEventArgs e)
        {
            Console.WriteLine("DocumentCompleted " + e.IsTopLevel);
            geckoWebBrowser.SaveDocument(@"E:\test.html");
        }

        private void GeckoWebBrowser_RequestProgressChanged(object sender, Gecko.GeckoRequestProgressEventArgs e)
        {
            Console.WriteLine(e.CurrentProgress); 
        }

        private void GeckoWebBrowser_ReadyStateChange(object sender, Gecko.DomEventArgs e)
        {
            
        }

        private void GeckoWebBrowser_Navigated(object sender, Gecko.GeckoNavigatedEventArgs e)
        {
            Console.WriteLine(1);
            Console.WriteLine(e.Uri);
            Console.WriteLine(e.Response.HttpResponseStatus);
        }

        private void GeckoWebBrowser_Navigating(object sender, Gecko.Events.GeckoNavigatingEventArgs e)
        {
            Console.WriteLine(2);
            Console.WriteLine(e.Uri);
            //geckoWebBrowser.SaveDocument(@"E:\test.html");
        }
    }
}
