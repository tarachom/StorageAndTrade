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
    public partial class Form_КонтрагентиПапкиЕлемент : Form
    {
        public Form_КонтрагентиПапкиЕлемент()
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
		/// Ід родителя для нової папки
		/// </summary>
		public string ParentUid { get; set; }

		/// <summary>
		/// Обєкт запису
		/// </summary>
        private Довідники.Контрагенти_Папки_Objest контрагенти_Папки_Objest { get; set; }

		private void Form_КонтрагентиПапкиЕлемент_Load(object sender, EventArgs e)
        {
			directoryControl_КонтрагентиПапка.Init(new Form_КонтрагентиПапкиВибір(), new Довідники.Контрагенти_Папки_Pointer(), ПошуковіЗапити.Контрагенти_Папки);

			if (IsNew.HasValue)
			{
				контрагенти_Папки_Objest = new Довідники.Контрагенти_Папки_Objest();

				if (IsNew.Value)
				{
					this.Text += " - Новий";
					textBox_Код.Text = контрагенти_Папки_Objest.Код = (++Константи.НумераціяДовідників.Контрагенти_Папки_Const).ToString("D6");
					directoryControl_КонтрагентиПапка.DirectoryPointerItem = new Довідники.Контрагенти_Папки_Pointer(new UnigueID(ParentUid));
				}
				else
				{
					if (контрагенти_Папки_Objest.Read(new UnigueID(Uid)))
					{
						this.Text += " - Редагування";

						textBoxName.Text = контрагенти_Папки_Objest.Назва;
						textBox_Код.Text = контрагенти_Папки_Objest.Код;
						directoryControl_КонтрагентиПапка.DirectoryPointerItem = new Довідники.Контрагенти_Папки_Pointer(контрагенти_Папки_Objest.Родич.UnigueID);
						((Form_КонтрагентиПапкиВибір)directoryControl_КонтрагентиПапка.SelectForm).UidOpenFolder = Uid;
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
					контрагенти_Папки_Objest.New();

				try
				{
					контрагенти_Папки_Objest.Назва = textBoxName.Text;
					контрагенти_Папки_Objest.Родич = (Довідники.Контрагенти_Папки_Pointer)directoryControl_КонтрагентиПапка.DirectoryPointerItem;
					контрагенти_Папки_Objest.Save();
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
