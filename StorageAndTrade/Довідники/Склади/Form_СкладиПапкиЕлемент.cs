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
    public partial class Form_СкладиПапкиЕлемент : Form
    {
        public Form_СкладиПапкиЕлемент()
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
        private Довідники.Склади_Папки_Objest склади_Папки_Objest { get; set; }

		private void Form_СкладиПапкиЕлемент_Load(object sender, EventArgs e)
        {
			directoryControl_СкладиПапка.Init(new Form_СкладиПапкиВибір(), new Довідники.Склади_Папки_Pointer(), ПошуковіЗапити.Склади_Папки);

			if (IsNew.HasValue)
			{
				склади_Папки_Objest = new Довідники.Склади_Папки_Objest();

				if (IsNew.Value)
				{
					this.Text += " - Новий";
					textBox_Код.Text = склади_Папки_Objest.Код = (++Константи.НумераціяДовідників.Склади_Папки_Const).ToString("D6");
					directoryControl_СкладиПапка.DirectoryPointerItem = new Довідники.Склади_Папки_Pointer(new UnigueID(ParentUid));
				}
				else
				{
					if (склади_Папки_Objest.Read(new UnigueID(Uid)))
					{
						this.Text += " - Редагування";

						textBoxName.Text = склади_Папки_Objest.Назва;
						textBox_Код.Text = склади_Папки_Objest.Код;
						directoryControl_СкладиПапка.DirectoryPointerItem = new Довідники.Номенклатура_Папки_Pointer(склади_Папки_Objest.Родич.UnigueID);
						((Form_СкладиПапкиВибір)directoryControl_СкладиПапка.SelectForm).UidOpenFolder = Uid;
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
					склади_Папки_Objest.New();

				try
				{
					склади_Папки_Objest.Назва = textBoxName.Text;
					склади_Папки_Objest.Родич = (Довідники.Склади_Папки_Pointer)directoryControl_СкладиПапка.DirectoryPointerItem;
					склади_Папки_Objest.Save();
				}
				catch (Exception exp)
				{
					MessageBox.Show(exp.Message);
					return;
				}

				if (OwnerForm != null && !OwnerForm.IsDisposed)
					OwnerForm.LoadRecords();

				this.DialogResult = DialogResult.OK;
				this.Close();
			}
		}

		private void buttonClose_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}
	}
}
