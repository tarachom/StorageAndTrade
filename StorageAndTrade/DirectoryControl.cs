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
		/// Ініціалізація параметрів
		/// </summary>
		/// <param name="selectForm">Форма</param>
		/// <param name="directoryPointerItem">Вказівник</param>
		public void Init(Form selectForm, DirectoryPointer directoryPointerItem)
        {
			SelectForm = selectForm;
			DirectoryPointerItem = directoryPointerItem;
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

		/// <summary>
		/// Кнопка очищення
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void buttonClear_Click(object sender, EventArgs e)
		{
			DirectoryPointerItem.Init(new UnigueID(Guid.Empty));
			ReadPresentation();

			if (AfterSelectFunc != null)
				AfterSelectFunc.Invoke();
		}
	}
}
