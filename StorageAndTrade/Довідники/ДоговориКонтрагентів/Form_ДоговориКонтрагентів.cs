﻿/*
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
    public partial class Form_ДоговориКонтрагентів : Form
    {
        public Form_ДоговориКонтрагентів()
        {
            InitializeComponent();

			dataGridViewRecords.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

			RecordsBindingList = new BindingList<Записи>();
			dataGridViewRecords.DataSource = RecordsBindingList;

			dataGridViewRecords.Columns["Image"].Width = 30;
			dataGridViewRecords.Columns["Image"].HeaderText = "";

			dataGridViewRecords.Columns["ID"].Visible = false;
			dataGridViewRecords.Columns["Назва"].Width = 300;
			dataGridViewRecords.Columns["Код"].Width = 50;
			dataGridViewRecords.Columns["Контрагент"].Width = 300;
		}

		public DirectoryPointer DirectoryPointerItem { get; set; }
		public Довідники.Контрагенти_Pointer КонтрагентВласник { get; set; }

		private void Form_ДоговориКонтрагентів_Load(object sender, EventArgs e)
		{
			if (DirectoryPointerItem != null)
				if (КонтрагентВласник == null || КонтрагентВласник.IsEmpty())
					throw new Exception("Не заданий КонтрагентВласник");

			LoadRecords();
		}

		private BindingList<Записи> RecordsBindingList { get; set; }

		public void LoadRecords()
		{
			int selectRow = dataGridViewRecords.SelectedRows.Count > 0 ?
				dataGridViewRecords.SelectedRows[dataGridViewRecords.SelectedRows.Count - 1].Index : 0;

			RecordsBindingList.Clear();

			Довідники.ДоговориКонтрагентів_Select договориКонтрагентів_Select = new Довідники.ДоговориКонтрагентів_Select();
			договориКонтрагентів_Select.QuerySelect.Field.AddRange(new string[] {
				Довідники.ДоговориКонтрагентів_Const.Назва,
				Довідники.ДоговориКонтрагентів_Const.Код,
				Довідники.ДоговориКонтрагентів_Const.ТипДоговору
			});

			//Контрагент
			договориКонтрагентів_Select.QuerySelect.FieldAndAlias.Add(
				new NameValue<string>(Довідники.Контрагенти_Const.TABLE + "." + Довідники.Контрагенти_Const.Назва, "joinContragent"));
			договориКонтрагентів_Select.QuerySelect.Joins.Add(
				new Join(Довідники.Контрагенти_Const.TABLE, Довідники.ДоговориКонтрагентів_Const.Контрагент, Довідники.ДоговориКонтрагентів_Const.TABLE));

			//Відбір по контрагенту
			if (КонтрагентВласник != null && !КонтрагентВласник.IsEmpty())
				договориКонтрагентів_Select.QuerySelect.Where.Add(
					new Where(Довідники.ДоговориКонтрагентів_Const.Контрагент, Comparison.EQ, КонтрагентВласник.UnigueID.UGuid));

			//ORDER
			договориКонтрагентів_Select.QuerySelect.Order.Add(Довідники.ДоговориКонтрагентів_Const.Назва, SelectOrder.ASC);

			договориКонтрагентів_Select.Select();
			while (договориКонтрагентів_Select.MoveNext())
			{
				Довідники.ДоговориКонтрагентів_Pointer cur = договориКонтрагентів_Select.Current;

				RecordsBindingList.Add(new Записи
				{
					ID = cur.UnigueID.ToString(),
					Назва = cur.Fields[Довідники.ДоговориКонтрагентів_Const.Назва].ToString(),
					Код = cur.Fields[Довідники.ДоговориКонтрагентів_Const.Код].ToString(),
					Контрагент = cur.Fields["joinContragent"].ToString(),
					ТипДоговору = ((Перелічення.ТипДоговорів)cur.Fields[Довідники.ДоговориКонтрагентів_Const.ТипДоговору]).ToString()
				});

				if (DirectoryPointerItem != null && selectRow == 0)
					if (cur.UnigueID.ToString() == DirectoryPointerItem.UnigueID.ToString())
						selectRow = RecordsBindingList.Count - 1;
			}

			if (selectRow != 0 && selectRow < dataGridViewRecords.Rows.Count)
			{
				dataGridViewRecords.Rows[0].Selected = false;
				dataGridViewRecords.Rows[selectRow].Selected = true;
				dataGridViewRecords.FirstDisplayedScrollingRowIndex = selectRow;
			}
		}

		private class Записи
		{
			public Записи() { Image = Properties.Resources.doc_text_image; }
			public Bitmap Image { get; set; }
			public string ID { get; set; }
			public string Назва { get; set; }
			public string Код { get; set; }
			public string Контрагент { get; set; }
			public string ТипДоговору { get; set; }
		}

        private void dataGridViewRecords_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
			if (e.RowIndex >= 0 && e.RowIndex < dataGridViewRecords.RowCount)
			{
				string Uid = dataGridViewRecords.Rows[e.RowIndex].Cells["ID"].Value.ToString();

				if (DirectoryPointerItem != null)
				{
					DirectoryPointerItem = new Довідники.ДоговориКонтрагентів_Pointer(new UnigueID(Uid));
					this.DialogResult = DialogResult.OK;
					this.Close();
				}
				else
				{
					toolStripButtonEdit_Click(this, null);
				}
			}
		}

        private void toolStripButtonAdd_Click(object sender, EventArgs e)
        {
			Form_ДоговориКонтрагентівЕлемент form_ДоговориКонтрагентівЕлемент = new Form_ДоговориКонтрагентівЕлемент();
			form_ДоговориКонтрагентівЕлемент.MdiParent = this.MdiParent;
			form_ДоговориКонтрагентівЕлемент.IsNew = true;
			form_ДоговориКонтрагентівЕлемент.OwnerForm = this;
			form_ДоговориКонтрагентівЕлемент.КонтрагентВласник = КонтрагентВласник;
			if (DirectoryPointerItem != null && this.MdiParent == null)
				form_ДоговориКонтрагентівЕлемент.ShowDialog();
			else
				form_ДоговориКонтрагентівЕлемент.Show();
		}

        private void toolStripButtonEdit_Click(object sender, EventArgs e)
        {
			if (dataGridViewRecords.SelectedRows.Count > 0)
			{
				int RowIndex = dataGridViewRecords.SelectedRows[0].Index;

				Form_ДоговориКонтрагентівЕлемент form_ДоговориКонтрагентівЕлемент = new Form_ДоговориКонтрагентівЕлемент();
				form_ДоговориКонтрагентівЕлемент.MdiParent = this.MdiParent;
				form_ДоговориКонтрагентівЕлемент.IsNew = false;
				form_ДоговориКонтрагентівЕлемент.OwnerForm = this;
				form_ДоговориКонтрагентівЕлемент.Uid = dataGridViewRecords.Rows[RowIndex].Cells["ID"].Value.ToString();
				if (DirectoryPointerItem != null && this.MdiParent == null)
					form_ДоговориКонтрагентівЕлемент.ShowDialog();
				else
					form_ДоговориКонтрагентівЕлемент.Show();
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

                    Довідники.ДоговориКонтрагентів_Objest договориКонтрагентів_Objest = new Довідники.ДоговориКонтрагентів_Objest();
                    if (договориКонтрагентів_Objest.Read(new UnigueID(uid)))
                    {
						Довідники.ДоговориКонтрагентів_Objest договориКонтрагентів_Objest_Новий = договориКонтрагентів_Objest.Copy();
						договориКонтрагентів_Objest_Новий.Назва = "Копія - " + договориКонтрагентів_Objest_Новий.Назва;
						договориКонтрагентів_Objest_Новий.Код = (++Константи.НумераціяДовідників.ДоговориКонтрагентів_Const).ToString("D6");
						договориКонтрагентів_Objest_Новий.Save();
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

                    Довідники.ДоговориКонтрагентів_Objest договориКонтрагентів_Objest = new Довідники.ДоговориКонтрагентів_Objest();
                    if (договориКонтрагентів_Objest.Read(new UnigueID(uid)))
                    {
						договориКонтрагентів_Objest.Delete();
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
    }
}
