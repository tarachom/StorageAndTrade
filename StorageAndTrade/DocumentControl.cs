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
	public partial class DocumentControl : UserControl
	{
		public DocumentControl()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Ініціалізація параметрів
		/// </summary>
		/// <param name="selectForm">Форма</param>
		/// <param name="directoryPointerItem">Вказівник</param>
		public void Init(Form selectForm, DocumentPointer documentPointerItem)
		{
			SelectForm = selectForm;
			DocumentPointerItem = documentPointerItem;
		}

		/// <summary>
		/// Форма яку потрібно відкрити
		/// </summary>
		public Form SelectForm { get; set; }

		private DocumentPointer mDocumentPointerItem;

		/// <summary>
		/// Ссилка на документ
		/// </summary>
		public DocumentPointer DocumentPointerItem
		{
			get { return mDocumentPointerItem; }

			set
			{
				mDocumentPointerItem = value;

				if (mDocumentPointerItem != null)
					ReadPresentation();
				else
					textBoxControl.Text = "";
			}
		}

		/// <summary>
		/// Функція викликає функцію GetPresentation()
		/// </summary>
		private void ReadPresentation()
		{
			if (mDocumentPointerItem.GetType().GetMember("GetPresentation").Length == 1)
				textBoxControl.Text = mDocumentPointerItem.GetType().InvokeMember(
					"GetPresentation", BindingFlags.InvokeMethod, null, mDocumentPointerItem, new object[] { }).ToString();
		}

		private void buttonOpen_Click(object sender, EventArgs e)
		{
			if (SelectForm != null)
			{
				PropertyInfo propertyInfo = SelectForm.GetType().GetProperty("DocumentPointerItem");
				if (propertyInfo != null)
				{
					propertyInfo.SetValue(SelectForm, mDocumentPointerItem);
					SelectForm.ShowDialog();
					DocumentPointerItem = (DocumentPointer)propertyInfo.GetValue(SelectForm);
				}
			}
		}

		private void buttonClear_Click(object sender, EventArgs e)
		{
			DocumentPointerItem.Init(new UnigueID(Guid.Empty));
			ReadPresentation();
		}
    }
}
