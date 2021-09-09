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
		/// Обєкт запису
		/// </summary>
        private Довідники.Склади_Objest склади_Objest { get; set; }

		public void CallBack_Відповідальний(DirectoryPointer directoryPointerItem)
		{
			Form_ФізичніОсоби form_ФізичніОсоби = new Form_ФізичніОсоби();
			form_ФізичніОсоби.DirectoryPointerItem = directoryPointerItem;
			form_ФізичніОсоби.DirectoryControlItem = directoryControl_Відповідальний;
			form_ФізичніОсоби.ShowDialog();
		}

		public void CallBack_ВидЦін(DirectoryPointer directoryPointerItem)
		{
			Form_ВидиЦін form_ВидиЦін = new Form_ВидиЦін();
			form_ВидиЦін.DirectoryPointerItem = directoryPointerItem;
			form_ВидиЦін.DirectoryControlItem = directoryControl_ВидЦін;
			form_ВидиЦін.ShowDialog();
		}

		private void FormAddCash_Load(object sender, EventArgs e)
        {
			//Заповнення елементів перелічення - ТипСкладу
			foreach (ConfigurationEnumField field in Конфа.Config.Kernel.Conf.Enums["ТипиСкладів"].Fields.Values)
				comboBox_ТипСкладу.Items.Add((Перелічення.ТипиСкладів)field.Value);

			directoryControl_Відповідальний.CallBack = CallBack_Відповідальний;
			directoryControl_ВидЦін.CallBack = CallBack_ВидЦін;

			if (IsNew.HasValue)
			{
				склади_Objest = new Довідники.Склади_Objest();

				if (IsNew.Value)
				{
					this.Text += " - Новий запис";
					comboBox_ТипСкладу.SelectedIndex = 0;
					directoryControl_Відповідальний.DirectoryPointerItem = new Довідники.ФізичніОсоби_Pointer();
					directoryControl_ВидЦін.DirectoryPointerItem = new Довідники.ВидиЦін_Pointer();
				}
				else
				{
					if (склади_Objest.Read(new UnigueID(Uid)))
					{
						this.Text += " - Редагування запису - " + склади_Objest.Назва;

						textBoxНазва.Text = склади_Objest.Назва;
						comboBox_ТипСкладу.SelectedItem = склади_Objest.ТипСкладу;
						directoryControl_Відповідальний.DirectoryPointerItem = new Довідники.ФізичніОсоби_Pointer(склади_Objest.Відповідальний.UnigueID);
						directoryControl_ВидЦін.DirectoryPointerItem = new Довідники.ВидиЦін_Pointer(склади_Objest.ВидЦін.UnigueID);
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
					склади_Objest.Відповідальний = (Довідники.ФізичніОсоби_Pointer)directoryControl_Відповідальний.DirectoryPointerItem;
					склади_Objest.ВидЦін = (Довідники.ВидиЦін_Pointer)directoryControl_ВидЦін.DirectoryPointerItem;
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
