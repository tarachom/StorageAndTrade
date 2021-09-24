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
    public partial class Form_НоменклатураЕлемент : Form
    {
        public Form_НоменклатураЕлемент()
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
		private Довідники.Номенклатура_Objest номенклатура_Objest { get; set; }

		private void FormAddCash_Load(object sender, EventArgs e)
        {

			tableLayoutPanel1.ColumnCount = 5;
			tableLayoutPanel1.RowCount = 5;

			tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10f));
			tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20f));
			tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30f));
			tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20f));
			tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20f));

			for (int i = 0; i < 5; i++)
			{
				tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.AutoSize));

				for (int j = 0; j < 5; j++)
				{
					Label label = new Label();
					label.Margin = new Padding(0);
					label.BackColor = Color.Azure;
					label.Dock = DockStyle.Fill;
					label.Text = "text " + i.ToString();

					tableLayoutPanel1.Controls.Add(label, j, i);
				}
			}
			

			documentControl1.SelectForm = new Form_ЗамовленняКлієнтаЖурнал();
			documentControl1.DocumentPointerItem = new Документи.ЗамовленняКлієнта_Pointer(new UnigueID("7ec5a77f-ed45-4aaf-a375-eed72a5f69fe"));

			//Заповнення елементів перелічення - ТипНоменклатури
			foreach (ConfigurationEnumField field in Конфа.Config.Kernel.Conf.Enums["ТипиНоменклатури"].Fields.Values)
				comboBox_ТипНоменклатури.Items.Add((Перелічення.ТипиНоменклатури)field.Value);

			directoryControl_НоменклатураПапка.SelectForm = new Form_НоменклатураПапкиВибір();
			directoryControl_Виробник.SelectForm = new Form_Виробники();
			directoryControl_ВидНоменклатури.SelectForm = new Form_ВидиНоменклатури();
			directoryControl_ОдиницяВиміру.SelectForm = new Form_ПакуванняОдиниціВиміру();

			if (IsNew.HasValue)
			{
				номенклатура_Objest = new Довідники.Номенклатура_Objest();

				if (IsNew.Value)
				{
					this.Text += " - Новий запис";
					directoryControl_НоменклатураПапка.DirectoryPointerItem = new Довідники.Номенклатура_Папки_Pointer(new UnigueID(ParentUid));
					directoryControl_Виробник.DirectoryPointerItem = new Довідники.Виробники_Pointer();
					directoryControl_ВидНоменклатури.DirectoryPointerItem = new Довідники.ВидиНоменклатури_Pointer();
					directoryControl_ОдиницяВиміру.DirectoryPointerItem = new Довідники.ПакуванняОдиниціВиміру_Pointer();
					comboBox_ТипНоменклатури.SelectedIndex = 0;
				}
				else
				{
					if (номенклатура_Objest.Read(new UnigueID(Uid)))
					{
						this.Text += " - Редагування запису - " + номенклатура_Objest.Назва;

						textBox_Назва.Text = номенклатура_Objest.Назва;
						directoryControl_НоменклатураПапка.DirectoryPointerItem = new Довідники.Номенклатура_Папки_Pointer(номенклатура_Objest.Папка.UnigueID);
						directoryControl_Виробник.DirectoryPointerItem = new Довідники.Виробники_Pointer(номенклатура_Objest.Виробник.UnigueID);
						directoryControl_ВидНоменклатури.DirectoryPointerItem = new Довідники.ВидиНоменклатури_Pointer(номенклатура_Objest.ВидНоменклатури.UnigueID);
						directoryControl_ОдиницяВиміру.DirectoryPointerItem = new Довідники.ПакуванняОдиниціВиміру_Pointer(номенклатура_Objest.ОдиницяВиміру.UnigueID);
						comboBox_ТипНоменклатури.SelectedItem = номенклатура_Objest.ТипНоменклатури;
						textBox_Артикул.Text = номенклатура_Objest.Артикул;
						textBox_НазваПовна.Text = номенклатура_Objest.НазваПовна;
						textBox_Опис.Text = номенклатура_Objest.Опис;
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
					номенклатура_Objest.New();

				try
				{
					номенклатура_Objest.Назва = textBox_Назва.Text;
					номенклатура_Objest.Папка = (Довідники.Номенклатура_Папки_Pointer)directoryControl_НоменклатураПапка.DirectoryPointerItem;
					номенклатура_Objest.Виробник = (Довідники.Виробники_Pointer)directoryControl_Виробник.DirectoryPointerItem;
					номенклатура_Objest.ВидНоменклатури = (Довідники.ВидиНоменклатури_Pointer)directoryControl_ВидНоменклатури.DirectoryPointerItem;
					номенклатура_Objest.ОдиницяВиміру = (Довідники.ПакуванняОдиниціВиміру_Pointer)directoryControl_ОдиницяВиміру.DirectoryPointerItem;
					номенклатура_Objest.ТипНоменклатури = comboBox_ТипНоменклатури.SelectedItem != null ? (Перелічення.ТипиНоменклатури)comboBox_ТипНоменклатури.SelectedItem : 0;
					номенклатура_Objest.Артикул = textBox_Артикул.Text;
					номенклатура_Objest.НазваПовна = textBox_НазваПовна.Text;
					номенклатура_Objest.Опис = textBox_Опис.Text;
					номенклатура_Objest.Save();
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
