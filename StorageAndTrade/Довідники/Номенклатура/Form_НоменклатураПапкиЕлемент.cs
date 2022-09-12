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
    public partial class Form_НоменклатураПапкиЕлемент : Form
    {
        public Form_НоменклатураПапкиЕлемент()
        {
            InitializeComponent();
        }

		/// <summary>
		/// Форма списку
		/// </summary>
        public Form_Номенклатура OwnerForm { get; set; }
        
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
        private Довідники.Номенклатура_Папки_Objest номенклатура_Папки_Objest { get; set; }

		private void Form_НоменклатураПапкиЕлемент_Load(object sender, EventArgs e)
        {
			directoryControl_НоменклатураПапка.Init(new Form_НоменклатураПапкиВибір(), new Довідники.Номенклатура_Папки_Pointer(new UnigueID(ParentUid)), ПошуковіЗапити.Номенклатура_Папки);

			if (IsNew.HasValue)
			{
				номенклатура_Папки_Objest = new Довідники.Номенклатура_Папки_Objest();

				if (IsNew.Value)
				{
					this.Text += " - Новий";
					textBox_Код.Text = номенклатура_Папки_Objest.Код = (++Константи.НумераціяДовідників.Номенклатура_Папки_Const).ToString("D6");
				}
				else
				{
					if (номенклатура_Папки_Objest.Read(new UnigueID(Uid)))
					{
						this.Text += " - Редагування";

						textBoxName.Text = номенклатура_Папки_Objest.Назва;
						textBox_Код.Text = номенклатура_Папки_Objest.Код;
						directoryControl_НоменклатураПапка.DirectoryPointerItem = new Довідники.Номенклатура_Папки_Pointer(номенклатура_Папки_Objest.Родич.UnigueID);
						((Form_НоменклатураПапкиВибір)directoryControl_НоменклатураПапка.SelectForm).UidOpenFolder = Uid;
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
					номенклатура_Папки_Objest.New();

				try
				{
					номенклатура_Папки_Objest.Назва = textBoxName.Text;
					номенклатура_Папки_Objest.Родич = (Довідники.Номенклатура_Папки_Pointer)directoryControl_НоменклатураПапка.DirectoryPointerItem;
					номенклатура_Папки_Objest.Save();
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
