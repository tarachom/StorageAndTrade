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

			dataGridViewRecords.Columns["ТипДоговору"].Width = 200;
			dataGridViewRecords.Columns["ТипДоговору"].HeaderText = "Тип договору";
		}

		/// <summary>
		/// Вказівник для вибору
		/// </summary>
		public DirectoryPointer DirectoryPointerItem { get; set; }

		/// <summary>
		/// Вказівник для виділення в списку
		/// </summary>
		public DirectoryPointer SelectPointerItem { get; set; }

		/// <summary>
		/// Контрагент власник договорів
		/// </summary>
		public Довідники.Контрагенти_Pointer КонтрагентВласник { get; set; }

		private void Form_ДоговориКонтрагентів_Load(object sender, EventArgs e)
		{
			directoryControl_Контрагент.Init(new Form_Контрагенти(), new Довідники.Контрагенти_Pointer(), ПошуковіЗапити.Контрагенти);
			
			if (КонтрагентВласник != null)
				directoryControl_Контрагент.DirectoryPointerItem = КонтрагентВласник;

			directoryControl_Контрагент.AfterSelectFunc = () =>
			{
				КонтрагентВласник = (Довідники.Контрагенти_Pointer)directoryControl_Контрагент.DirectoryPointerItem;
				LoadRecords();

				return true;
			};

			LoadRecords();
		}

		private BindingList<Записи> RecordsBindingList { get; set; }

		public void LoadRecords()
		{
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

			//З конфігурації
			ConfigurationEnums переліченняТипДоговорівКонфа = Конфа.Config.Kernel.Conf.Enums["ТипДоговорів"];

			договориКонтрагентів_Select.Select();
			while (договориКонтрагентів_Select.MoveNext())
			{
				Довідники.ДоговориКонтрагентів_Pointer cur = договориКонтрагентів_Select.Current;

				string ТипДоговоруОпис = "";

				if ((int)(cur.Fields[Довідники.ДоговориКонтрагентів_Const.ТипДоговору]) != 0)
				{
					string ТипДоговору = ((Перелічення.ТипДоговорів)cur.Fields[Довідники.ДоговориКонтрагентів_Const.ТипДоговору]).ToString();
					ТипДоговоруОпис = переліченняТипДоговорівКонфа.Fields[ТипДоговору].Desc;
				}

				RecordsBindingList.Add(new Записи
				{
					ID = cur.UnigueID.ToString(),
					Назва = cur.Fields[Довідники.ДоговориКонтрагентів_Const.Назва].ToString(),
					Код = cur.Fields[Довідники.ДоговориКонтрагентів_Const.Код].ToString(),
					Контрагент = cur.Fields["joinContragent"].ToString(),
					ТипДоговору = ТипДоговоруОпис

				});
			}

			if ((DirectoryPointerItem != null || SelectPointerItem != null) && dataGridViewRecords.Rows.Count > 0)
			{
				string UidSelect = SelectPointerItem != null ? SelectPointerItem.UnigueID.ToString() : DirectoryPointerItem.UnigueID.ToString();

				if (UidSelect != Guid.Empty.ToString())
					ФункціїДляІнтерфейсу.ВиділитиЕлементСписку(dataGridViewRecords, "ID", UidSelect);
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

						SelectPointerItem = договориКонтрагентів_Objest_Новий.GetDirectoryPointer();
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

        private void dataGridViewRecords_CellClick(object sender, DataGridViewCellEventArgs e)
        {
			if (dataGridViewRecords.SelectedRows.Count > 0)
			{
				int RowIndex = dataGridViewRecords.SelectedRows[0].Index;

				SelectPointerItem = new Довідники.ДоговориКонтрагентів_Pointer(new UnigueID(dataGridViewRecords.Rows[e.RowIndex].Cells["ID"].Value.ToString()));
			}
		}
    }
}
