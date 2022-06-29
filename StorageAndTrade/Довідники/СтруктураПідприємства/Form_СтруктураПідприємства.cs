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
    public partial class Form_СтруктураПідприємства : Form
    {
        public Form_СтруктураПідприємства()
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
		}

		public DirectoryPointer DirectoryPointerItem { get; set; }

        private void Form_СтруктураПідприємства_Load(object sender, EventArgs e)
        {
			LoadRecords();
		}

		private BindingList<Записи> RecordsBindingList { get; set; }

		public void LoadRecords()
		{
			int selectRow = dataGridViewRecords.SelectedRows.Count > 0 ?
				dataGridViewRecords.SelectedRows[dataGridViewRecords.SelectedRows.Count - 1].Index : 0;

			RecordsBindingList.Clear();

			Довідники.СтруктураПідприємства_Select структураПідприємства_Select = new Довідники.СтруктураПідприємства_Select();
			структураПідприємства_Select.QuerySelect.Field.Add(Довідники.СтруктураПідприємства_Const.Назва);
			структураПідприємства_Select.QuerySelect.Field.Add(Довідники.СтруктураПідприємства_Const.Код);

			//ORDER
			структураПідприємства_Select.QuerySelect.Order.Add(Довідники.СтруктураПідприємства_Const.Назва, SelectOrder.ASC);

			структураПідприємства_Select.Select();
			while (структураПідприємства_Select.MoveNext())
			{
				Довідники.СтруктураПідприємства_Pointer cur = структураПідприємства_Select.Current;

				RecordsBindingList.Add(new Записи
				{
					ID = cur.UnigueID.ToString(),
					Назва = cur.Fields[Довідники.СтруктураПідприємства_Const.Назва].ToString(),
					Код = cur.Fields[Довідники.СтруктураПідприємства_Const.Код].ToString()
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
		}

        private void dataGridViewRecords_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
			if (e.RowIndex >= 0 && e.RowIndex < dataGridViewRecords.RowCount)
			{
				string Uid = dataGridViewRecords.Rows[e.RowIndex].Cells["ID"].Value.ToString();

				if (DirectoryPointerItem != null)
				{
					DirectoryPointerItem = new Довідники.СтруктураПідприємства_Pointer(new UnigueID(Uid));
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
			Form_СтруктураПідприємстваЕлемент form_СтруктураПідприємстваЕлемент = new Form_СтруктураПідприємстваЕлемент();
			form_СтруктураПідприємстваЕлемент.IsNew = true;
			form_СтруктураПідприємстваЕлемент.OwnerForm = this;
			form_СтруктураПідприємстваЕлемент.ShowDialog();
        }

        private void toolStripButtonEdit_Click(object sender, EventArgs e)
        {
			if (dataGridViewRecords.SelectedRows.Count > 0)
			{
				int RowIndex = dataGridViewRecords.SelectedRows[0].Index;

				Form_СтруктураПідприємстваЕлемент form_СтруктураПідприємстваЕлемент = new Form_СтруктураПідприємстваЕлемент();
				form_СтруктураПідприємстваЕлемент.IsNew = false;
				form_СтруктураПідприємстваЕлемент.OwnerForm = this;
				form_СтруктураПідприємстваЕлемент.Uid = dataGridViewRecords.Rows[RowIndex].Cells["ID"].Value.ToString();
				form_СтруктураПідприємстваЕлемент.ShowDialog();
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

                    Довідники.СтруктураПідприємства_Objest структураПідприємства_Objest = new Довідники.СтруктураПідприємства_Objest();
                    if (структураПідприємства_Objest.Read(new UnigueID(uid)))
                    {
						Довідники.СтруктураПідприємства_Objest структураПідприємства_Objest_Новий = структураПідприємства_Objest.Copy();
						структураПідприємства_Objest_Новий.Назва = "Копія - " + структураПідприємства_Objest_Новий.Назва;
						структураПідприємства_Objest_Новий.Код = (++Константи.НумераціяДовідників.СтруктураПідприємства_Const).ToString("D6");
						структураПідприємства_Objest_Новий.Save();
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

                    Довідники.СтруктураПідприємства_Objest структураПідприємства_Objest = new Довідники.СтруктураПідприємства_Objest();
                    if (структураПідприємства_Objest.Read(new UnigueID(uid)))
                    {
						структураПідприємства_Objest.Delete();
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
