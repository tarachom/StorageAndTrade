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
	public partial class DirectoryControl : UserControl
	{
		public DirectoryControl()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Зворотня функція для вибору із списку
		/// </summary>
		public Func<DirectoryPointer, DirectoryPointer> CallBack { get; set; }

		private DirectoryPointer mDirectoryPointerItem;

		/// <summary>
		/// Ссилка на елемент довідника
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
		/// Функція викликає функцію ссилки довідника GetPresentation()
		/// </summary>
		private void ReadPresentation()
		{
			if (mDirectoryPointerItem.GetType().GetMember("GetPresentation").Length == 1)
				textBoxControl.Text = mDirectoryPointerItem.GetType().InvokeMember(
					"GetPresentation", BindingFlags.InvokeMethod, null, mDirectoryPointerItem, new object[] { }).ToString();
		}

		private void buttonOpen_Click(object sender, EventArgs e)
		{
			if (CallBack != null)
            {
				if (mDirectoryPointerItem != null)
					DirectoryPointerItem =  CallBack.Invoke(DirectoryPointerItem);
                else
					throw new Exception("DirectoryControl Error: DirectoryPointerItem null");
			}
		}

        private void buttonClear_Click(object sender, EventArgs e)
        {
			DirectoryPointerItem.Init(new UnigueID(Guid.Empty));
			ReadPresentation();
		}
    }
}
