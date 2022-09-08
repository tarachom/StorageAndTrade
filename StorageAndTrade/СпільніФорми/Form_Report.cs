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
using System.Windows.Forms;

using Gecko;

namespace StorageAndTrade
{
    public partial class Form_Report : Form
    {
        public Form_Report()
        {
            InitializeComponent();

            geckoWebBrowser = GeckoWebBrowser.AddGeckoWebBrowserControl(this, new System.Drawing.Point(2, 2));
        }

        Gecko.GeckoWebBrowser geckoWebBrowser { get; set; }

        public string HtmlDocumentPath { get; set; }

        private void Form_Report_Load(object sender, EventArgs e)
        {
            geckoWebBrowser.Navigate(HtmlDocumentPath);
            geckoWebBrowser.DomClick += GeckoWebBrowser.DomClick;
        }
    }
}
