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
    public partial class Form_БанківськіРахункиКонтрагентівЕлемент : Form
    {
        public Form_БанківськіРахункиКонтрагентівЕлемент()
        {
            InitializeComponent();
        }

		/// <summary>
		/// Форма списку
		/// </summary>
        public Form_БанківськіРахункиКонтрагентів OwnerForm { get; set; }
        
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
        private Довідники.БанківськіРахункиКонтрагентів_Objest банківськіРахункиКонтрагентів_Objest { get; set; }

		private void Form_БанківськіРахункиКонтрагентівЕлемент_Load(object sender, EventArgs e)
        {
			directoryControl_Валюта.Init(new Form_Валюти(), new Довідники.Валюти_Pointer(), ПошуковіЗапити.Валюти);
			//directoryControl_Організація.SelectForm = Form_Організації;

			if (IsNew.HasValue)
			{
				банківськіРахункиКонтрагентів_Objest = new Довідники.БанківськіРахункиКонтрагентів_Objest();

				if (IsNew.Value)
				{
					this.Text += " - Новий";
					textBox_Код.Text = банківськіРахункиКонтрагентів_Objest.Код = (++Константи.НумераціяДовідників.БанківськіРахункиКонтрагентів_Const).ToString("D6");
					//directoryControl_Організація.DirectoryPointerItem = new Довідники.Організації_Pointer();
				}
				else
				{
					if (банківськіРахункиКонтрагентів_Objest.Read(new UnigueID(Uid)))
					{
						this.Text += " - Редагування ";

						textBoxName.Text = банківськіРахункиКонтрагентів_Objest.Назва;
						textBox_Код.Text = банківськіРахункиКонтрагентів_Objest.Код;
						directoryControl_Валюта.DirectoryPointerItem = new Довідники.Валюти_Pointer(банківськіРахункиКонтрагентів_Objest.Валюта.UnigueID);
						//directoryControl_Організація.DirectoryPointerItem = new Довідники.Організації_Pointer(банківськіРахункиКонтрагентів_Objest.Організація.UnigueID);
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
					банківськіРахункиКонтрагентів_Objest.New();

				try
				{
					банківськіРахункиКонтрагентів_Objest.Назва = textBoxName.Text;
					банківськіРахункиКонтрагентів_Objest.Код = textBox_Код.Text;
					банківськіРахункиКонтрагентів_Objest.Валюта = (Довідники.Валюти_Pointer)directoryControl_Валюта.DirectoryPointerItem;
					//банківськіРахункиКонтрагентів_Objest.Організація = (Довідники.Організації_Pointer)directoryControl_Організація.DirectoryPointerItem;
					банківськіРахункиКонтрагентів_Objest.Save();
				}
				catch (Exception exp)
				{
					MessageBox.Show(exp.Message);
					return;
				}

				if (OwnerForm != null && !OwnerForm.IsDisposed)
				{
					OwnerForm.SelectPointerItem = банківськіРахункиКонтрагентів_Objest.GetDirectoryPointer();
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
