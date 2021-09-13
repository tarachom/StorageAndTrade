﻿/*
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
    public partial class Form_КасиЕлемент : Form
    {
        public Form_КасиЕлемент()
        {
            InitializeComponent();
        }

		/// <summary>
		/// Форма списку
		/// </summary>
        public Form_Каси OwnerForm { get; set; }
        
		/// <summary>
		/// Чи це новий
		/// </summary>
        public Nullable<bool> IsNew { get; set; }

		/// <summary>
		/// Ід запису
		/// </summary>
        public string Uid { get; set; }

		/// <summary>
		/// Обєкт запису
		/// </summary>
        private Довідники.Каси_Objest каси_Objest { get; set; }

		/// <summary>
		/// Зворотня функція для вибору із списку
		/// </summary>
		/// <param name="directoryPointerItem"></param>
		public DirectoryPointer CallBack_Валюта(DirectoryPointer directoryPointerItem)
		{
			Form_Валюти form_Валюти = new Form_Валюти();
			form_Валюти.DirectoryPointerItem = directoryPointerItem;
			form_Валюти.ShowDialog();

			return form_Валюти.DirectoryPointerItem;
		}

		private void FormAddCash_Load(object sender, EventArgs e)
        {
			directoryControl_Валюта.CallBack = CallBack_Валюта;

			if (IsNew.HasValue)
			{
				каси_Objest = new Довідники.Каси_Objest();

				if (IsNew.Value)
				{
					this.Text += " - Новий запис";

					directoryControl_Валюта.DirectoryPointerItem = new Довідники.Валюти_Pointer();
				}
				else
				{
					if (каси_Objest.Read(new UnigueID(Uid)))
					{
						this.Text += " - Редагування запису - " + каси_Objest.Назва;

						textBoxName.Text = каси_Objest.Назва;
						directoryControl_Валюта.DirectoryPointerItem = new Довідники.Валюти_Pointer(каси_Objest.Валюта.UnigueID);
					}
					else
						MessageBox.Show("Error read");
				}
			}
		}

        private void buttonSave_Click(object sender, EventArgs e)
        {
			if (IsNew.HasValue)
			{
				if (IsNew.Value)
					каси_Objest.New();

				try
				{
					каси_Objest.Назва = textBoxName.Text;
					каси_Objest.Валюта = (Довідники.Валюти_Pointer)directoryControl_Валюта.DirectoryPointerItem;
					каси_Objest.Save();
				}
				catch (Exception exp)
				{
					MessageBox.Show(exp.Message);
					return;
				}

				if (OwnerForm != null)
					OwnerForm.LoadRecords();

				this.Close();
			}
		}

		private void buttonClose_Click(object sender, EventArgs e)
		{
			this.Close();
		}
	}
}
