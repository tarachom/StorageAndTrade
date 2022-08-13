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
using Документи = StorageAndTrade_1_0.Документи;
using Перелічення = StorageAndTrade_1_0.Перелічення;

namespace StorageAndTrade
{
    public partial class Form_ПартіяТоварівКомпозитЕлемент : Form
    {
        public Form_ПартіяТоварівКомпозитЕлемент()
        {
            InitializeComponent();
        }

		/// <summary>
		/// Форма списку
		/// </summary>
        public Form_ПартіяТоварівКомпозит OwnerForm { get; set; }
        
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
        private Довідники.ПартіяТоварівКомпозит_Objest партіяТоварівКомпозит_Objest { get; set; }

		private void Form_ПартіяТоварівКомпозит_Load(object sender, EventArgs e)
        {
			if (IsNew.HasValue)
			{
				партіяТоварівКомпозит_Objest = new Довідники.ПартіяТоварівКомпозит_Objest();

				documentControl_ПоступленняТоварів.Init(new Form_ПоступленняТоварівТаПослугЖурнал(), new Документи.ПоступленняТоварівТаПослуг_Pointer());
				documentControl_ВведенняЗалишків.Init(new Form_ВведенняЗалишківЖурнал(), new Документи.ВведенняЗалишків_Pointer());

				if (!IsNew.Value)
				{
					if (партіяТоварівКомпозит_Objest.Read(new UnigueID(Uid)))
					{
						textBoxName.Text = партіяТоварівКомпозит_Objest.Назва;
						dateTimePicker_Дата.Value = партіяТоварівКомпозит_Objest.Дата;
						documentControl_ПоступленняТоварів.DocumentPointerItem = партіяТоварівКомпозит_Objest.ПоступленняТоварівТаПослуг;
						documentControl_ВведенняЗалишків.DocumentPointerItem = партіяТоварівКомпозит_Objest.ВведенняЗалишків;
					}
					else
						MessageBox.Show("Error read");
				}
			}
		}
		private void buttonClose_Click(object sender, EventArgs e)
		{
			this.Close();
		}
	}
}
