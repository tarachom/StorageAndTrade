﻿/*
Copyright (C) 2019-2022 TARAKHOMYN YURIY IVANOVYCH
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
    public partial class Form_КонтрагентиПапкиВибір : Form
    {
        public Form_КонтрагентиПапкиВибір()
        {
            InitializeComponent();
        }

		public DirectoryPointer DirectoryPointerItem { get; set; }

        public string UidOpenFolder { get; set; }

        private void Form_КонтрагентиПапкиВибір_Load(object sender, EventArgs e)
        {
            Контрагенти_Папки_Дерево.Parent_Pointer = (Довідники.Контрагенти_Папки_Pointer)DirectoryPointerItem;
            Контрагенти_Папки_Дерево.UidOpenFolder = UidOpenFolder;
            Контрагенти_Папки_Дерево.CallBack_DoubleClick = TreeFolderDoubleClick;
            Контрагенти_Папки_Дерево.LoadTree();
        }

        public void TreeFolderDoubleClick()
        {
            DirectoryPointerItem = Контрагенти_Папки_Дерево.Parent_Pointer;
            this.Close();
        }
    }
}
