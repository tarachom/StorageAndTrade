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

namespace StorageAndTrade
{
    public partial class FormAbout : Form
    {
        public ConfigurationParam OpenConfigurationParam { get; set; }

        public FormAbout()
        {
            InitializeComponent();
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://accounting.org.ua/storage_and_trade.html");
        }

        private void FormAbout_Load(object sender, EventArgs e)
        {
            if (OpenConfigurationParam != null)
                textBoxInfo.Text =
                    "Конфігурація: " + OpenConfigurationParam.ConfigurationName + "\r\n" +
                    "Сервер: " + OpenConfigurationParam.DataBaseServer + "\r\n" +
                    "База: " + OpenConfigurationParam.DataBaseBaseName + "\r\n";
        }
    }
}
