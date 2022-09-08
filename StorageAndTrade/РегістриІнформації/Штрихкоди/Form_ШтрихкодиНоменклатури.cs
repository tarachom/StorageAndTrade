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
using РегістриВідомостей = StorageAndTrade_1_0.РегістриВідомостей;

namespace StorageAndTrade
{
    public partial class Form_ШтрихкодиНоменклатури : Form
    {
        public Form_ШтрихкодиНоменклатури()
        {
            InitializeComponent();

			dataGridViewRecords.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

			RecordsBindingList = new BindingList<Записи>();
			dataGridViewRecords.DataSource = RecordsBindingList;

			dataGridViewRecords.Columns["Image"].Width = 30;
			dataGridViewRecords.Columns["Image"].HeaderText = "";

			dataGridViewRecords.Columns["ID"].Visible = false;
			dataGridViewRecords.Columns["Штрихкод"].Width = 300;

			dataGridViewRecords.Columns["Номенклатура"].Visible = false;
			dataGridViewRecords.Columns["НоменклатураНазва"].Width = 300;
			dataGridViewRecords.Columns["НоменклатураНазва"].HeaderText = "Номенклатура";

			dataGridViewRecords.Columns["ХарактеристикаНоменклатури"].Visible = false;
			dataGridViewRecords.Columns["ХарактеристикаНоменклатуриНазва"].Width = 300;
			dataGridViewRecords.Columns["ХарактеристикаНоменклатуриНазва"].HeaderText = "Характеристика";

			dataGridViewRecords.Columns["Пакування"].Visible = false;
			dataGridViewRecords.Columns["ПакуванняНазва"].Width = 300;
			dataGridViewRecords.Columns["ПакуванняНазва"].HeaderText = "Пакування";
		}

		/// <summary>
		/// Номенклатура власник
		/// </summary>
		public Довідники.Номенклатура_Pointer НоменклатураВласник { get; set; }

		/// <summary>
		/// UID для виділення в списку
		/// </summary>
		public string SelectPointerItem { get; set; }

		private void Form_ШтрихкодиНоменклатури_Load(object sender, EventArgs e)
        {
			directoryControl_Номенклатура.Init(new Form_Номенклатура(), new Довідники.Номенклатура_Pointer(), ПошуковіЗапити.Номенклатура);

			if (НоменклатураВласник != null)
				directoryControl_Номенклатура.DirectoryPointerItem = НоменклатураВласник;

			directoryControl_Номенклатура.AfterSelectFunc = () =>
			{
				НоменклатураВласник = (Довідники.Номенклатура_Pointer)directoryControl_Номенклатура.DirectoryPointerItem;
				LoadRecords();

				return true;
			};

			LoadRecords();
		}

		private BindingList<Записи> RecordsBindingList { get; set; }

		public void LoadRecords()
		{
			RecordsBindingList.Clear();

			РегістриВідомостей.ШтрихкодиНоменклатури_RecordsSet ШтрихкодиНоменклатури = new РегістриВідомостей.ШтрихкодиНоменклатури_RecordsSet();

			//JOIN 1
			ШтрихкодиНоменклатури.QuerySelect.FieldAndAlias.Add(
				new NameValue<string>(Довідники.Номенклатура_Const.TABLE + "." + Довідники.Номенклатура_Const.Назва, "nom_name"));
			ШтрихкодиНоменклатури.QuerySelect.Joins.Add(
				new Join(Довідники.Номенклатура_Const.TABLE, РегістриВідомостей.ШтрихкодиНоменклатури_Const.Номенклатура, РегістриВідомостей.ШтрихкодиНоменклатури_Const.TABLE));

			//JOIN 2
			ШтрихкодиНоменклатури.QuerySelect.FieldAndAlias.Add(
				new NameValue<string>(Довідники.ХарактеристикиНоменклатури_Const.TABLE + "." + Довідники.ХарактеристикиНоменклатури_Const.Назва, "xar_name"));
			ШтрихкодиНоменклатури.QuerySelect.Joins.Add(
				new Join(Довідники.ХарактеристикиНоменклатури_Const.TABLE, РегістриВідомостей.ШтрихкодиНоменклатури_Const.ХарактеристикаНоменклатури, РегістриВідомостей.ШтрихкодиНоменклатури_Const.TABLE));

			//JOIN 3
			ШтрихкодиНоменклатури.QuerySelect.FieldAndAlias.Add(
				new NameValue<string>(Довідники.ПакуванняОдиниціВиміру_Const.TABLE + "." + Довідники.ПакуванняОдиниціВиміру_Const.Назва, "pak_name"));
			ШтрихкодиНоменклатури.QuerySelect.Joins.Add(
				new Join(Довідники.ПакуванняОдиниціВиміру_Const.TABLE, РегістриВідомостей.ШтрихкодиНоменклатури_Const.Пакування, РегістриВідомостей.ШтрихкодиНоменклатури_Const.TABLE));

			//Відбір по номенклатурі
			if (НоменклатураВласник != null && !НоменклатураВласник.IsEmpty())
				ШтрихкодиНоменклатури.QuerySelect.Where.Add(
					new Where(РегістриВідомостей.ШтрихкодиНоменклатури_Const.Номенклатура, Comparison.EQ, НоменклатураВласник.UnigueID.UGuid));

			//ORDER
			ШтрихкодиНоменклатури.QuerySelect.Order.Add(РегістриВідомостей.ШтрихкодиНоменклатури_Const.Штрихкод, SelectOrder.ASC);

			ШтрихкодиНоменклатури.Read();
			foreach (РегістриВідомостей.ШтрихкодиНоменклатури_RecordsSet.Record запис in ШтрихкодиНоменклатури.Records)
			{
				RecordsBindingList.Add(new Записи
				{
					ID = запис.UID.ToString(),
					Штрихкод = запис.Штрихкод,
					Номенклатура = запис.Номенклатура,
					НоменклатураНазва = ШтрихкодиНоменклатури.JoinValue[запис.UID.ToString()]["nom_name"].ToString(),
					ХарактеристикаНоменклатури = запис.ХарактеристикаНоменклатури,
					ХарактеристикаНоменклатуриНазва = ШтрихкодиНоменклатури.JoinValue[запис.UID.ToString()]["xar_name"].ToString(),
					Пакування = запис.Пакування,
					ПакуванняНазва = ШтрихкодиНоменклатури.JoinValue[запис.UID.ToString()]["pak_name"].ToString()
				});
			}

			if (!String.IsNullOrEmpty(SelectPointerItem) && dataGridViewRecords.Rows.Count > 0)
			{
				string UidSelect = SelectPointerItem;

				if (UidSelect != Guid.Empty.ToString())
					ФункціїДляІнтерфейсу.ВиділитиЕлементСписку(dataGridViewRecords, "ID", UidSelect);
			}
		}

		private class Записи
		{
			public Записи() { Image = Properties.Resources.doc_text_image; }
			public Bitmap Image { get; set; }
			public string ID { get; set; }
			public string Штрихкод { get; set; }
			public Довідники.Номенклатура_Pointer Номенклатура { get; set; }
			public string НоменклатураНазва { get; set; }
			public Довідники.ХарактеристикиНоменклатури_Pointer ХарактеристикаНоменклатури { get; set; }
			public string ХарактеристикаНоменклатуриНазва { get; set; }
			public Довідники.ПакуванняОдиниціВиміру_Pointer Пакування { get; set; }
			public string ПакуванняНазва { get; set; }
		}

		private void dataGridViewRecords_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
			if (e.RowIndex >= 0 && e.RowIndex < dataGridViewRecords.RowCount)
			{
				//string Uid = dataGridViewRecords.Rows[e.RowIndex].Cells["ID"].Value.ToString();

				toolStripButtonEdit_Click(this, null);
			}
		}

		private void toolStripButtonAdd_Click(object sender, EventArgs e)
		{
			Довідники.Номенклатура_Pointer номенклатура_Pointer;			

			if (dataGridViewRecords.SelectedRows.Count > 0)
			{
				DataGridViewRow row = dataGridViewRecords.SelectedRows[0];
				string uid = row.Cells["Номенклатура"].Value.ToString();

				номенклатура_Pointer = new Довідники.Номенклатура_Pointer(new UnigueID(uid));
			}
			else
				номенклатура_Pointer = НоменклатураВласник;

			Form_ШтрихкодиНоменклатуриЕлемент form_ШтрихкодиНоменклатуриЕлемент = new Form_ШтрихкодиНоменклатуриЕлемент();
			form_ШтрихкодиНоменклатуриЕлемент.MdiParent = this.MdiParent;
			form_ШтрихкодиНоменклатуриЕлемент.IsNew = true;
			form_ШтрихкодиНоменклатуриЕлемент.OwnerForm = this;
			form_ШтрихкодиНоменклатуриЕлемент.НоменклатураВласник = номенклатура_Pointer;
			form_ШтрихкодиНоменклатуриЕлемент.Show();
		}

        private void toolStripButtonEdit_Click(object sender, EventArgs e)
        {
			if (dataGridViewRecords.SelectedRows.Count > 0)
			{
				DataGridViewRow row = dataGridViewRecords.SelectedRows[0];
				string uid = row.Cells["ID"].Value.ToString();

				Form_ШтрихкодиНоменклатуриЕлемент form_ШтрихкодиНоменклатуриЕлемент = new Form_ШтрихкодиНоменклатуриЕлемент();
				form_ШтрихкодиНоменклатуриЕлемент.MdiParent = this.MdiParent;
				form_ШтрихкодиНоменклатуриЕлемент.IsNew = false;
				form_ШтрихкодиНоменклатуриЕлемент.OwnerForm = this;
				form_ШтрихкодиНоменклатуриЕлемент.Uid = uid;
				form_ШтрихкодиНоменклатуриЕлемент.Show();
			}
		}

        private void toolStripButtonRefresh_Click(object sender, EventArgs e)
        {
			LoadRecords();
		}

        private void toolStripButtonCopy_Click(object sender, EventArgs e)
        {
            if (dataGridViewRecords.SelectedRows.Count != 0 &&
                MessageBox.Show("Копіювати записи?", "Повідомлення", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                for (int i = 0; i < dataGridViewRecords.SelectedRows.Count; i++)
                {
                    DataGridViewRow row = dataGridViewRecords.SelectedRows[i];
                    string uid = row.Cells["ID"].Value.ToString();

					РегістриВідомостей.ШтрихкодиНоменклатури_Objest штрихкодиНоменклатури_Objest = new РегістриВідомостей.ШтрихкодиНоменклатури_Objest();
                    if (штрихкодиНоменклатури_Objest.Read(new UnigueID(uid)))
                    {
						РегістриВідомостей.ШтрихкодиНоменклатури_Objest штрихкодиНоменклатури_Objest_Новий = штрихкодиНоменклатури_Objest.Copy();
						штрихкодиНоменклатури_Objest_Новий.Save();

                        //SelectPointerItem = номенклатура_Objest_Новий.GetDirectoryPointer();
                    }
                    else
                    {
                        MessageBox.Show("Error read");
                        break;
                    }
                }

                LoadRecords();
            }
        }

        private void toolStripButtonDelete_Click(object sender, EventArgs e)
        {
            if (dataGridViewRecords.SelectedRows.Count != 0 &&
                MessageBox.Show("Видалити записи?", "Повідомлення", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                for (int i = 0; i < dataGridViewRecords.SelectedRows.Count; i++)
                {
                    DataGridViewRow row = dataGridViewRecords.SelectedRows[i];
                    string uid = row.Cells["ID"].Value.ToString();

					РегістриВідомостей.ШтрихкодиНоменклатури_Objest штрихкодиНоменклатури_Objest = new РегістриВідомостей.ШтрихкодиНоменклатури_Objest();
					if (штрихкодиНоменклатури_Objest.Read(new UnigueID(uid)))
                    {
						штрихкодиНоменклатури_Objest.Delete();
                    }
                    else
                    {
                        MessageBox.Show("Error read");
                        break;
                    }
                }

                LoadRecords();
            }
        }

        private void dataGridViewRecords_CellClick(object sender, DataGridViewCellEventArgs e)
        {
			if (dataGridViewRecords.SelectedRows.Count > 0)
			{
				int RowIndex = dataGridViewRecords.SelectedRows[0].Index;

				SelectPointerItem = dataGridViewRecords.Rows[RowIndex].Cells["ID"].Value.ToString();
			}
		}

        private void dataGridViewRecords_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
