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
    public partial class Form_СтруктураПідприємстваЕлемент : Form
    {
        public Form_СтруктураПідприємстваЕлемент()
        {
            InitializeComponent();
        }

		/// <summary>
		/// Форма списку
		/// </summary>
        public Form_СтруктураПідприємства OwnerForm { get; set; }
        
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
        private Довідники.СтруктураПідприємства_Objest структураПідприємства_Objest { get; set; }

		private void Form_СтруктураПідприємстваЕлемент_Load(object sender, EventArgs e)
        {
			directoryControl_Керівник.Init(new Form_СтруктураПідприємства(), new Довідники.ФізичніОсоби_Pointer(), ПошуковіЗапити.ФізичніОсоби);

			if (IsNew.HasValue)
			{
				структураПідприємства_Objest = new Довідники.СтруктураПідприємства_Objest();

				if (IsNew.Value)
				{
					this.Text += " - Новий";
					textBox_Код.Text = структураПідприємства_Objest.Код = (++Константи.НумераціяДовідників.СтруктураПідприємства_Const).ToString("D6");
				}
				else
				{
					if (структураПідприємства_Objest.Read(new UnigueID(Uid)))
					{
						this.Text += " - Редагування";
						textBoxName.Text = структураПідприємства_Objest.Назва;
						textBox_Код.Text = структураПідприємства_Objest.Код;
						directoryControl_Керівник.DirectoryPointerItem = new Довідники.ФізичніОсоби_Pointer(структураПідприємства_Objest.Керівник.UnigueID);
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
					структураПідприємства_Objest.New();

				try
				{
					структураПідприємства_Objest.Назва = textBoxName.Text;
					структураПідприємства_Objest.Код = textBox_Код.Text;
					структураПідприємства_Objest.Керівник = (Довідники.ФізичніОсоби_Pointer)directoryControl_Керівник.DirectoryPointerItem;
					структураПідприємства_Objest.Save();
				}
				catch (Exception exp)
				{
					MessageBox.Show(exp.Message);
					return;
				}

				if (OwnerForm != null && !OwnerForm.IsDisposed)
				{
					OwnerForm.SelectPointerItem = структураПідприємства_Objest.GetDirectoryPointer();
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
