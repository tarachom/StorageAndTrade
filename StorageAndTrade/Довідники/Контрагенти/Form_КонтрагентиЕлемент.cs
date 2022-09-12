/*
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
    public partial class Form_КонтрагентиЕлемент : Form
    {
        public Form_КонтрагентиЕлемент()
        {
            InitializeComponent();
        }

		/// <summary>
		/// Форма списку
		/// </summary>
        public Form_Контрагенти OwnerForm { get; set; }
        
		/// <summary>
		/// Чи це новий
		/// </summary>
        public Nullable<bool> IsNew { get; set; }

		/// <summary>
		/// Ід запису
		/// </summary>
        public string Uid { get; set; }

		/// <summary>
		/// Ід родителя для нового елемента
		/// </summary>
		public string ParentUid { get; set; }

		/// <summary>
		/// Обєкт запису
		/// </summary>
		private Довідники.Контрагенти_Objest контрагенти_Objest { get; set; }

		private void Form_КонтрагентиЕлемент_Load(object sender, EventArgs e)
        {
			directoryControl_КонтрагентПапка.Init(new Form_КонтрагентиПапкиВибір(), new Довідники.Контрагенти_Папки_Pointer(new UnigueID(ParentUid)), ПошуковіЗапити.Контрагенти_Папки);

			if (IsNew.HasValue)
			{
				контрагенти_Objest = new Довідники.Контрагенти_Objest();

				Контрагенти_ТабличнаЧастина_Контакти.ДовідникОбєкт = контрагенти_Objest;

				if (IsNew.Value)
				{
					this.Text += " - Новий";
					textBox_Код.Text = контрагенти_Objest.Код = (++Константи.НумераціяДовідників.Контрагенти_Const).ToString("D6");
				}
				else
				{
					if (контрагенти_Objest.Read(new UnigueID(Uid)))
					{
						this.Text += " - Редагування";

						textBoxName.Text = контрагенти_Objest.Назва;
						textBox_Код.Text = контрагенти_Objest.Код;
						directoryControl_КонтрагентПапка.DirectoryPointerItem = new Довідники.Контрагенти_Папки_Pointer(контрагенти_Objest.Папка.UnigueID);
						textBox_ПовнаНазва.Text = контрагенти_Objest.НазваПовна;
						textBox_Опис.Text = контрагенти_Objest.Опис;

						Контрагенти_ТабличнаЧастина_Контакти.LoadRecords();
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
					контрагенти_Objest.New();

				контрагенти_Objest.Назва = textBoxName.Text;
				контрагенти_Objest.Папка = (Довідники.Контрагенти_Папки_Pointer)directoryControl_КонтрагентПапка.DirectoryPointerItem;
				контрагенти_Objest.НазваПовна = textBox_ПовнаНазва.Text;
				контрагенти_Objest.Опис = textBox_Опис.Text;

				try
				{
					контрагенти_Objest.Save();
					Контрагенти_ТабличнаЧастина_Контакти.SaveRecords();
				}
				catch (Exception exp)
				{
					MessageBox.Show(exp.Message);
					return;
				}

				if (OwnerForm != null && !OwnerForm.IsDisposed)
				{
					OwnerForm.SelectPointerItem = контрагенти_Objest.GetDirectoryPointer();
					OwnerForm.LoadRecords();
				}

				this.Close();
			}
		}

		private void buttonClose_Click(object sender, EventArgs e)
		{
			this.Close();
		}

    }
}
