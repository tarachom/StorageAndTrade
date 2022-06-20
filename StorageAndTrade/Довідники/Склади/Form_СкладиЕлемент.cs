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

namespace StorageAndTrade
{
    public partial class Form_СкладиЕлемент : Form
    {
        public Form_СкладиЕлемент()
        {
            InitializeComponent();
        }

		/// <summary>
		/// Форма списку
		/// </summary>
        public Form_Склади OwnerForm { get; set; }
        
		/// <summary>
		/// Чи це новий
		/// </summary>
        public Nullable<bool> IsNew { get; set; }

		/// <summary>
		/// Ід запису
		/// </summary>
        public string Uid { get; set; }

		/// <summary>
		/// Ід родителя для нової папки
		/// </summary>
		public string ParentUid { get; set; }

		/// <summary>
		/// Обєкт запису
		/// </summary>
		private Довідники.Склади_Objest склади_Objest { get; set; }

		private void Form_СкладиЕлемент_Load(object sender, EventArgs e)
        {
			//Заповнення елементів перелічення - ТипСкладу
			foreach (ConfigurationEnumField field in Конфа.Config.Kernel.Conf.Enums["ТипиСкладів"].Fields.Values)
				comboBox_ТипСкладу.Items.Add((Перелічення.ТипиСкладів)field.Value);

			directoryControl_СкладиПапка.SelectForm = new Form_СкладиПапкиВибір();
			directoryControl_Відповідальний.SelectForm = new Form_ФізичніОсоби();
			directoryControl_ВидЦін.SelectForm = new Form_ВидиЦін();
			directoryControl_Підрозділ.SelectForm = new Form_СтруктураПідприємства();

			if (IsNew.HasValue)
			{
				склади_Objest = new Довідники.Склади_Objest();

				if (IsNew.Value)
				{
					this.Text += " - Новий запис";
					comboBox_ТипСкладу.SelectedIndex = 0;
					directoryControl_СкладиПапка.DirectoryPointerItem = new Довідники.Склади_Папки_Pointer(new UnigueID(ParentUid));
					directoryControl_Відповідальний.DirectoryPointerItem = new Довідники.ФізичніОсоби_Pointer();
					directoryControl_ВидЦін.DirectoryPointerItem = new Довідники.ВидиЦін_Pointer();
					directoryControl_Підрозділ.DirectoryPointerItem = new Довідники.СтруктураПідприємства_Pointer();
				}
				else
				{
					if (склади_Objest.Read(new UnigueID(Uid)))
					{
						this.Text += " - Редагування запису - " + склади_Objest.Назва;

						textBoxНазва.Text = склади_Objest.Назва;
						comboBox_ТипСкладу.SelectedItem = склади_Objest.ТипСкладу;
						directoryControl_СкладиПапка.DirectoryPointerItem = new Довідники.Склади_Папки_Pointer(склади_Objest.Папка.UnigueID);
						directoryControl_Відповідальний.DirectoryPointerItem = new Довідники.ФізичніОсоби_Pointer(склади_Objest.Відповідальний.UnigueID);
						directoryControl_ВидЦін.DirectoryPointerItem = new Довідники.ВидиЦін_Pointer(склади_Objest.ВидЦін.UnigueID);
						directoryControl_Підрозділ.DirectoryPointerItem = new Довідники.СтруктураПідприємства_Pointer(склади_Objest.Підрозділ.UnigueID);
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
					склади_Objest.New();

				try
				{
					склади_Objest.Назва = textBoxНазва.Text;
					склади_Objest.ТипСкладу = comboBox_ТипСкладу.SelectedItem != null ? (Перелічення.ТипиСкладів)comboBox_ТипСкладу.SelectedItem : 0;
					склади_Objest.Папка = (Довідники.Склади_Папки_Pointer)directoryControl_СкладиПапка.DirectoryPointerItem;
					склади_Objest.Відповідальний = (Довідники.ФізичніОсоби_Pointer)directoryControl_Відповідальний.DirectoryPointerItem;
					склади_Objest.ВидЦін = (Довідники.ВидиЦін_Pointer)directoryControl_ВидЦін.DirectoryPointerItem;
					склади_Objest.Підрозділ = (Довідники.СтруктураПідприємства_Pointer)directoryControl_Підрозділ.DirectoryPointerItem;
					склади_Objest.Save();
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
