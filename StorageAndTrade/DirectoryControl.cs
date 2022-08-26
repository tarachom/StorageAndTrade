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
using System.Windows.Forms;
using System.Reflection;
using System.Drawing;
using System.Collections.Generic;

using Конфа = StorageAndTrade_1_0;

using AccountingSoftware;

namespace StorageAndTrade
{
	/// <summary>
	/// DirectoryControl - Це контрол який відображає вказівник на елемент довідника
	/// </summary>
	public partial class DirectoryControl : UserControl
	{
		public DirectoryControl()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Функція яка викликається перед відкриттям форми вибору
		/// Якщо False то форма не відкривається
		/// </summary>
		public Func<bool> BeforeClickOpenFunc { get; set; }

		/// <summary>
		/// Функція яка викликається після вибору.
		/// Також вона викликається після очистки (buttonClear_Click)
		/// </summary>
		public Func<bool> AfterSelectFunc { get; set; }

		/// <summary>
		/// Функція яка викликається перед пошуком
		/// </summary>
		public Action BeforeFindFunc { get; set; }

		/// <summary>
		/// Процедура яка викликається при встановленні значення DirectoryPointerItem
		/// </summary>
		public Action<DirectoryPointer> Bind { get; set; }

		/// <summary>
		/// Ініціалізація параметрів
		/// </summary>
		/// <param name="selectForm">Форма</param>
		/// <param name="directoryPointerItem">Вказівник</param>
		public void Init(Form selectForm, DirectoryPointer directoryPointerItem, string queryFind = "")
		{
			SelectForm = selectForm;
			DirectoryPointerItem = directoryPointerItem;
			QueryFind = queryFind;
		}

		private string mQueryFind;

		/// <summary>
		/// Пошуковий запит
		/// </summary>
		public string QueryFind 
		{
			get { return mQueryFind; }
            set
            {
				mQueryFind = value;
				buttonFind.Enabled = !String.IsNullOrEmpty(mQueryFind);
			} 
		}

		/// <summary>
		/// Форма для вибору елементу довідника
		/// </summary>
		public Form SelectForm { get; set; }

		private DirectoryPointer mDirectoryPointerItem;

		/// <summary>
		/// Вказівник на елемент довідника
		/// </summary>
		public DirectoryPointer DirectoryPointerItem
		{
			get { return mDirectoryPointerItem; }

			set
			{
				mDirectoryPointerItem = value;

				if (mDirectoryPointerItem != null)
					ReadPresentation();
				else
					textBoxControl.Text = "";

				if (Bind != null)
					Bind.Invoke(mDirectoryPointerItem);
			}
		}

		/// <summary>
		/// Функція викликає функцію вказівника довідника GetPresentation()
		/// для того щоб відобразити значення поля яке представляє даний елемент довідника. 
		/// Наприклад поле Назва
		/// </summary>
		private void ReadPresentation()
		{
			if (mDirectoryPointerItem.GetType().GetMember("GetPresentation").Length == 1)
				textBoxControl.Text = mDirectoryPointerItem.GetType().InvokeMember(
					"GetPresentation", BindingFlags.InvokeMethod, null, mDirectoryPointerItem, new object[] { }).ToString();
		}

		public string GetPresentation()
		{
			return textBoxControl.Text;
		}

		/// <summary>
		/// Кнопка відкриття форми вибору елементу довідника із списку
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void buttonOpen_Click(object sender, EventArgs e)
		{
			if (SelectForm != null)
			{
				if (BeforeClickOpenFunc != null)
					if (!BeforeClickOpenFunc.Invoke())
						return;

				PropertyInfo propertyInfo = SelectForm.GetType().GetProperty("DirectoryPointerItem");
				if (propertyInfo != null)
				{
					propertyInfo.SetValue(SelectForm, DirectoryPointerItem);
					SelectForm.ShowDialog();
					DirectoryPointerItem = (DirectoryPointer)propertyInfo.GetValue(SelectForm);

					if (AfterSelectFunc != null)
						AfterSelectFunc.Invoke();
				}
			}
		}

		private void buttonFind_Click(object sender, EventArgs e)
		{
			if (BeforeFindFunc != null)
				BeforeFindFunc.Invoke();

			if (String.IsNullOrEmpty(QueryFind))
				return;

			Rectangle rectangle = buttonFind.DisplayRectangle;
			rectangle.Offset(-(buttonFind.Parent.Width - buttonFind.Width - 1), buttonFind.Height);
			Point point = buttonFind.PointToScreen(rectangle.Location);

			ContextMenuStrip contextMenu = new ContextMenuStrip();

			ToolStripTextBox findTextBox = new ToolStripTextBox();
			findTextBox.ToolTipText = "Пошук";
			findTextBox.Size = new Size(textBoxControl.Width, 0);
			findTextBox.TextChanged += FindTextChanged;

			contextMenu.Items.Add(findTextBox);
			contextMenu.Show(point);

			findTextBox.Focus();
		}

		private void FindTextChanged(object sender, EventArgs e)
		{
			ToolStripTextBox findMenu = (ToolStripTextBox)sender;

			ToolStrip parent = findMenu.GetCurrentParent();

			for (int counterMenu = parent.Items.Count - 1; counterMenu > 0; counterMenu--)
				parent.Items.RemoveAt(counterMenu);

			Dictionary<string, object> paramQuery = new Dictionary<string, object>();
			paramQuery.Add("like_param", "%" + findMenu.Text.ToLower() + "%");

			string[] columnsName;
			List<Dictionary<string, object>> listRow;

			Конфа.Config.Kernel.DataBase.SelectRequest(QueryFind, paramQuery, out columnsName, out listRow);

			if (listRow.Count > 0)
			{
				ToolStripItem[] mas = new ToolStripItem[listRow.Count];

				int counter = 0;

				foreach (Dictionary<string, object> row in listRow)
				{
					mas[counter] = new ToolStripMenuItem(row["Назва"].ToString(), Properties.Resources.page_white_text, FindClick);
					mas[counter].Tag = row["uid"].ToString();
					counter++;
				}

				parent.Items.AddRange(mas);
			}
		}

		private void FindClick(object sender, EventArgs e)
		{
			ToolStripMenuItem selectMenu = (ToolStripMenuItem)sender;
			string uid = selectMenu.Tag.ToString();

			DirectoryPointerItem.Init(new UnigueID(uid));
			ReadPresentation();

			if (AfterSelectFunc != null)
				AfterSelectFunc.Invoke();
		}

		private void textBoxControl_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Delete)
			{
				DirectoryPointerItem.Init(new UnigueID(Guid.Empty));
				ReadPresentation();

				if (AfterSelectFunc != null)
					AfterSelectFunc.Invoke();
			}
			else if (e.KeyCode == Keys.Escape)
            {
				return;
            }
			else
            {
				buttonFind_Click(this, new EventArgs());
			}
		}
	}
}
