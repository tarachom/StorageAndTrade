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
    public partial class Form_ОрганізаціїЕлемент : Form
    {
        public Form_ОрганізаціїЕлемент()
        {
            InitializeComponent();
        }

		/// <summary>
		/// Форма списку
		/// </summary>
        public Form_Організації OwnerForm { get; set; }
        
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
        private Довідники.Організації_Objest організації_Objest { get; set; }

		private void Form_ОрганізаціїЕлемент_Load(object sender, EventArgs e)
        {
			if (IsNew.HasValue)
			{
				організації_Objest = new Довідники.Організації_Objest();

				Організація_ТабличнаЧастина_Контакти.ДовідникОбєкт = організації_Objest;

				if (IsNew.Value)
				{
					this.Text += " - Новий";
					textBox_Код.Text = організації_Objest.Код = (++Константи.НумераціяДовідників.Організації_Const).ToString("D6");
				}
				else
				{
					if (організації_Objest.Read(new UnigueID(Uid)))
					{
						this.Text += " - Редагування";

						textBoxName.Text = організації_Objest.Назва;
						textBox_Код.Text = організації_Objest.Код;
						
						textBox_ПовнаНазва.Text = організації_Objest.НазваПовна;
						textBox_НазваСкорочена.Text = організації_Objest.НазваСкорочена;
						textBox_НазваСкорочена.Text = організації_Objest.НазваСкорочена;

						dateTimePicker_ДатаРеєстрації.Value = 
							(організації_Objest.ДатаРеєстрації < dateTimePicker_ДатаРеєстрації.MinDate ? 
							DateTime.Today : організації_Objest.ДатаРеєстрації);

						textBox_КраїнаРеєстрації.Text = організації_Objest.КраїнаРеєстрації;
						textBox_СвідоцтвоСеріяНомер.Text = організації_Objest.СвідоцтвоСеріяНомер;
						textBox_СвідоцтвоДатаВидачі.Text = організації_Objest.СвідоцтвоДатаВидачі;

						Організація_ТабличнаЧастина_Контакти.LoadRecords();
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
					організації_Objest.New();

				організації_Objest.Назва = textBoxName.Text;
				організації_Objest.Код = textBox_Код.Text;

				організації_Objest.НазваПовна = textBox_ПовнаНазва.Text;
				організації_Objest.НазваСкорочена = textBox_НазваСкорочена.Text;
				організації_Objest.НазваСкорочена = textBox_НазваСкорочена.Text;
				організації_Objest.ДатаРеєстрації = dateTimePicker_ДатаРеєстрації.Value;
				організації_Objest.КраїнаРеєстрації = textBox_КраїнаРеєстрації.Text;
				організації_Objest.СвідоцтвоСеріяНомер = textBox_СвідоцтвоСеріяНомер.Text;
				організації_Objest.СвідоцтвоДатаВидачі = textBox_СвідоцтвоДатаВидачі.Text;

				try
				{
					організації_Objest.Save();
					Організація_ТабличнаЧастина_Контакти.SaveRecords();
				}
				catch (Exception exp)
				{
					MessageBox.Show(exp.Message);
					return;
				}

				if (OwnerForm != null && !OwnerForm.IsDisposed)
				{
					OwnerForm.SelectPointerItem = організації_Objest.GetDirectoryPointer();
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
