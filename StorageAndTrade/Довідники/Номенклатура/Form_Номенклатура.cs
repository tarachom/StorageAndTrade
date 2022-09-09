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
    public partial class Form_Номенклатура : Form
    {
        public Form_Номенклатура()
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
			dataGridViewRecords.Columns["Виробник"].Width = 100;

			dataGridViewRecords.Columns["ВидНоменклатури"].Width = 100;
			dataGridViewRecords.Columns["ВидНоменклатури"].HeaderText = "Вид";

			dataGridViewRecords.Columns["ОдиницяВиміру"].Width = 50;
			dataGridViewRecords.Columns["ОдиницяВиміру"].HeaderText = "Од.";

			dataGridViewRecords.Columns["ТипНоменклатури"].Width = 50;
			dataGridViewRecords.Columns["ТипНоменклатури"].HeaderText = "Тип";
		}

		/// <summary>
		/// Вказівник для вибору
		/// </summary>
		public DirectoryPointer DirectoryPointerItem { get; set; }

		/// <summary>
		/// Вказівник для виділення в списку
		/// </summary>
		public DirectoryPointer SelectPointerItem { get; set; }

		private void Form_Номенклатура_Load(object sender, EventArgs e)
        {
			if (DirectoryPointerItem != null && !DirectoryPointerItem.IsEmpty())
			{
				Довідники.Номенклатура_Objest номенклатура_Objest = new Довідники.Номенклатура_Pointer(new UnigueID(DirectoryPointerItem.UnigueID.UGuid)).GetDirectoryObject();
				if (номенклатура_Objest != null)
					Номенклатура_Папки_Дерево.Parent_Pointer = номенклатура_Objest.Папка;
			}

			Номенклатура_Папки_Дерево.CallBack_AfterSelect = TreeFolderAfterSelect;
			Номенклатура_Папки_Дерево.LoadTree();
		}

		private BindingList<Записи> RecordsBindingList { get; set; }

		public void LoadRecords()
		{
			RecordsBindingList.Clear();

			Довідники.Номенклатура_Select номенклатура_Select = new Довідники.Номенклатура_Select();
			номенклатура_Select.QuerySelect.Field.Add(Довідники.Номенклатура_Const.Назва);
			номенклатура_Select.QuerySelect.Field.Add(Довідники.Номенклатура_Const.Код);
			номенклатура_Select.QuerySelect.Field.Add(Довідники.Номенклатура_Const.ТипНоменклатури);

			//JOIN 1
			номенклатура_Select.QuerySelect.FieldAndAlias.Add(
				new NameValue<string>(Довідники.Виробники_Const.TABLE + "." + Довідники.Виробники_Const.Назва, "join1"));
			номенклатура_Select.QuerySelect.Joins.Add(
				new Join(Довідники.Виробники_Const.TABLE, Довідники.Номенклатура_Const.Виробник, Довідники.Номенклатура_Const.TABLE));

			//JOIN 2
			номенклатура_Select.QuerySelect.FieldAndAlias.Add(
				new NameValue<string>(Довідники.ВидиНоменклатури_Const.TABLE + "." + Довідники.ВидиНоменклатури_Const.Назва, "join2"));
			номенклатура_Select.QuerySelect.Joins.Add(
				new Join(Довідники.ВидиНоменклатури_Const.TABLE, Довідники.Номенклатура_Const.ВидНоменклатури, Довідники.Номенклатура_Const.TABLE));

			//JOIN 3
			номенклатура_Select.QuerySelect.FieldAndAlias.Add(
				new NameValue<string>(Довідники.ПакуванняОдиниціВиміру_Const.TABLE + "." + Довідники.ПакуванняОдиниціВиміру_Const.Назва, "join3"));
			номенклатура_Select.QuerySelect.Joins.Add(
				new Join(Довідники.ПакуванняОдиниціВиміру_Const.TABLE, Довідники.Номенклатура_Const.ОдиницяВиміру, Довідники.Номенклатура_Const.TABLE));

			//Залишки товарів
			номенклатура_Select.QuerySelect.FieldAndAlias.Add(new NameValue<string>($@"
(CASE WHEN {Довідники.Номенклатура_Const.TABLE}.{Довідники.Номенклатура_Const.ТипНоменклатури} = {(int)Перелічення.ТипиНоменклатури.Товар} THEN
	(WITH Залишки AS (
		SELECT
			ЗалишкиТоварів.{Константи.ВіртуальніТаблиціРегістрів.ТовариНаСкладах_Підсумок_TablePart.Номенклатура} AS Номенклатура,
			SUM(ЗалишкиТоварів.{Константи.ВіртуальніТаблиціРегістрів.ТовариНаСкладах_Підсумок_TablePart.ВНаявності}) AS ВНаявності
		FROM
			{Константи.ВіртуальніТаблиціРегістрів.ТовариНаСкладах_Підсумок_TablePart.TABLE} AS ЗалишкиТоварів
		WHERE
			ЗалишкиТоварів.{Константи.ВіртуальніТаблиціРегістрів.ТовариНаСкладах_Підсумок_TablePart.Номенклатура} = {Довідники.Номенклатура_Const.TABLE}.uid
		Group By Номенклатура
	) SELECT ВНаявності FROM Залишки)
ELSE 0 END)", "ostatok"));

			//WHERE
			if (Номенклатура_Папки_Дерево.Parent_Pointer != null)
				номенклатура_Select.QuerySelect.Where.Add(new Where(Довідники.Номенклатура_Const.Папка, Comparison.EQ, Номенклатура_Папки_Дерево.Parent_Pointer.UnigueID.UGuid));

			//ORDER
			номенклатура_Select.QuerySelect.Order.Add(Довідники.Номенклатура_Const.Назва, SelectOrder.ASC);

			номенклатура_Select.Select();
			while (номенклатура_Select.MoveNext())
			{
				Довідники.Номенклатура_Pointer cur = номенклатура_Select.Current;

				RecordsBindingList.Add(new Записи
				{
					ID = cur.UnigueID.ToString(),
					Назва = cur.Fields[Довідники.Номенклатура_Const.Назва].ToString(),
					Код = cur.Fields[Довідники.Номенклатура_Const.Код].ToString(),
					Виробник = cur.Fields["join1"].ToString(),
					ВидНоменклатури = cur.Fields["join2"].ToString(),
					ОдиницяВиміру = cur.Fields["join3"].ToString(),
					ТипНоменклатури = ((Перелічення.ТипиНоменклатури)cur.Fields[Довідники.Номенклатура_Const.ТипНоменклатури]).ToString(),
					Залишок = (cur.Fields["ostatok"] != DBNull.Value ? (decimal)cur.Fields["ostatok"] : 0)
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
			public string Код { get; set; }
			public string Назва { get; set; }
			public decimal Залишок { get; set; }
			public string ОдиницяВиміру { get; set; }
			public string ТипНоменклатури { get; set; }
			public string Виробник { get; set; }
			public string ВидНоменклатури { get; set; }
		}

		public void TreeFolderAfterSelect()
        {
			LoadRecords();
		}

		private void dataGridViewRecords_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
			if (e.RowIndex >= 0 && e.RowIndex < dataGridViewRecords.RowCount)
			{
				string Uid = dataGridViewRecords.Rows[e.RowIndex].Cells["ID"].Value.ToString();

				if (DirectoryPointerItem != null)
				{
					DirectoryPointerItem = new Довідники.Номенклатура_Pointer(new UnigueID(Uid));
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
			Form_НоменклатураЕлемент form_НоменклатураЕлемент = new Form_НоменклатураЕлемент();
			form_НоменклатураЕлемент.MdiParent = this.MdiParent;
			form_НоменклатураЕлемент.IsNew = true;
			form_НоменклатураЕлемент.OwnerForm = this;
			if (Номенклатура_Папки_Дерево.Parent_Pointer != null)
				form_НоменклатураЕлемент.ParentUid = Номенклатура_Папки_Дерево.Parent_Pointer.UnigueID.UGuid.ToString();
			if (DirectoryPointerItem != null && this.MdiParent == null)
				form_НоменклатураЕлемент.ShowDialog();
			else
				form_НоменклатураЕлемент.Show();
		}

        private void toolStripButtonEdit_Click(object sender, EventArgs e)
        {
			if (dataGridViewRecords.SelectedRows.Count > 0)
			{
				int RowIndex = dataGridViewRecords.SelectedRows[0].Index;

				Form_НоменклатураЕлемент form_НоменклатураЕлемент = new Form_НоменклатураЕлемент();
				form_НоменклатураЕлемент.MdiParent = this.MdiParent;
				form_НоменклатураЕлемент.IsNew = false;
				form_НоменклатураЕлемент.OwnerForm = this;
				form_НоменклатураЕлемент.Uid = dataGridViewRecords.Rows[RowIndex].Cells["ID"].Value.ToString();
				if (DirectoryPointerItem != null && this.MdiParent == null)
					form_НоменклатураЕлемент.ShowDialog();
				else
					form_НоменклатураЕлемент.Show();
			}			
		}

        private void toolStripButtonRefresh_Click(object sender, EventArgs e)
        {
			Номенклатура_Папки_Дерево.LoadTree();
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

                    Довідники.Номенклатура_Objest номенклатура_Objest = new Довідники.Номенклатура_Objest();
                    if (номенклатура_Objest.Read(new UnigueID(uid)))
                    {
						Довідники.Номенклатура_Objest номенклатура_Objest_Новий = номенклатура_Objest.Copy();
						номенклатура_Objest_Новий.Назва = "Копія - " + номенклатура_Objest_Новий.Назва;
						номенклатура_Objest_Новий.Код = (++Константи.НумераціяДовідників.Номенклатура_Const).ToString("D6");
						номенклатура_Objest_Новий.Save();

						SelectPointerItem = номенклатура_Objest_Новий.GetDirectoryPointer();
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

                    Довідники.Номенклатура_Objest номенклатура_Objest = new Довідники.Номенклатура_Objest();
                    if (номенклатура_Objest.Read(new UnigueID(uid)))
                    {
						номенклатура_Objest.Delete();
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

        private void товариНаСкладахToolStripMenuItem_Click(object sender, EventArgs e)
        {
			if (dataGridViewRecords.SelectedRows.Count > 0)
			{
				int RowIndex = dataGridViewRecords.SelectedRows[0].Index;

				Form_ТовариНаСкладахПоНоменклатурі form_ТовариНаСкладахПоНоменклатурі = new Form_ТовариНаСкладахПоНоменклатурі();
				form_ТовариНаСкладахПоНоменклатурі.MdiParent = this.MdiParent;
				form_ТовариНаСкладахПоНоменклатурі.Номенклатура = new Довідники.Номенклатура_Pointer(
					new UnigueID(dataGridViewRecords.Rows[RowIndex].Cells["ID"].Value.ToString()));

				form_ТовариНаСкладахПоНоменклатурі.CreateReport();

				if (DirectoryPointerItem != null && this.MdiParent == null)
					form_ТовариНаСкладахПоНоменклатурі.ShowDialog();
				else
					form_ТовариНаСкладахПоНоменклатурі.Show();
			}
		}

        private void партіїToolStripMenuItem_Click(object sender, EventArgs e)
        {
			if (dataGridViewRecords.SelectedRows.Count > 0)
			{
				int RowIndex = dataGridViewRecords.SelectedRows[0].Index;

				Form_ПартіїТоварівПоНоменклатурі form_ПартіїТоварівПоНоменклатурі = new Form_ПартіїТоварівПоНоменклатурі();
				form_ПартіїТоварівПоНоменклатурі.MdiParent = this.MdiParent;
				form_ПартіїТоварівПоНоменклатурі.Номенклатура = new Довідники.Номенклатура_Pointer(
					new UnigueID(dataGridViewRecords.Rows[RowIndex].Cells["ID"].Value.ToString()));

				form_ПартіїТоварівПоНоменклатурі.CreateReport();

				if (DirectoryPointerItem != null && this.MdiParent == null)
					form_ПартіїТоварівПоНоменклатурі.ShowDialog();
				else
					form_ПартіїТоварівПоНоменклатурі.Show();
			}
		}

        private void dataGridViewRecords_CellClick(object sender, DataGridViewCellEventArgs e)
        {
			if (dataGridViewRecords.SelectedRows.Count > 0)
			{
				int RowIndex = dataGridViewRecords.SelectedRows[0].Index;

				SelectPointerItem = new Довідники.Номенклатура_Pointer(new UnigueID(dataGridViewRecords.Rows[RowIndex].Cells["ID"].Value.ToString()));
			}
		}

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
			if (dataGridViewRecords.SelectedRows.Count > 0)
			{
				DataGridViewRow row = dataGridViewRecords.SelectedRows[0];
				string uid = row.Cells["ID"].Value.ToString();

                Form_ШтрихкодиНоменклатури form_ШтрихкодиНоменклатури = new Form_ШтрихкодиНоменклатури();
				form_ШтрихкодиНоменклатури.MdiParent = this.MdiParent;
				form_ШтрихкодиНоменклатури.НоменклатураВласник = new Довідники.Номенклатура_Pointer(new UnigueID(uid));
				form_ШтрихкодиНоменклатури.Show();
			}
		}
    }
}
