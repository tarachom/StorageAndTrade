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
using System.IO;
using System.Windows.Forms;
using AccountingSoftware;
using Конфа = StorageAndTrade_1_0;

namespace StorageAndTrade
{
	public partial class ConfigurationSelectionParam : Form
	{
		public ConfigurationSelectionParam()
		{
			InitializeComponent();
		}

		public Action<ConfigurationParam, bool> CallBack_Update { get; set; }

		public bool IsNew { get; set; }

		public ConfigurationParam ItemConfigurationParam { get; set; }

		private void ConfigurationSelectionParam_Load(object sender, EventArgs e)
		{
			if (IsNew)
			{
				ItemConfigurationParam = new ConfigurationParam();
				ItemConfigurationParam.ConfigurationKey = Guid.NewGuid().ToString();
			}
			else
			{
				if (ItemConfigurationParam != null)
				{
					textBoxConfName.Text = ItemConfigurationParam.ConfigurationName;
				}
				else
					new Exception("ItemConfigurationParam null");
			}

			textBoxPostgreSQLServer.Text = ItemConfigurationParam.DataBaseServer;
			textBoxPostgreSQLLogin.Text = ItemConfigurationParam.DataBaseLogin;
			textBoxPostgreSQLPassword.Text = ItemConfigurationParam.DataBasePassword;
			textBoxPostgreSQLPort.Text = ItemConfigurationParam.DataBasePort.ToString();
			textBoxPostgreSQLDataBase.Text = ItemConfigurationParam.DataBaseBaseName;
		}

		private void UpdateItemConfigurationParam()
		{
			ItemConfigurationParam.ConfigurationName = textBoxConfName.Text;
			ItemConfigurationParam.DataBaseServer = textBoxPostgreSQLServer.Text;
			ItemConfigurationParam.DataBaseLogin = textBoxPostgreSQLLogin.Text;
			ItemConfigurationParam.DataBasePassword = textBoxPostgreSQLPassword.Text;
			ItemConfigurationParam.DataBasePort = int.Parse(textBoxPostgreSQLPort.Text);
			ItemConfigurationParam.DataBaseBaseName = textBoxPostgreSQLDataBase.Text;
		}

		private void buttonOK_Click(object sender, EventArgs e)
		{
			UpdateItemConfigurationParam();

			CallBack_Update(ItemConfigurationParam, IsNew);

			this.Close();
		}

		private void buttonClose_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void buttonConnect_Click(object sender, EventArgs e)
		{
			UpdateItemConfigurationParam();

			Конфа.Config.Kernel = new Kernel();

			Exception exception = null;
			bool IsExistsDatabase = false;

			bool flag = Конфа.Config.Kernel.CreateDatabaseIfNotExist(
				ItemConfigurationParam.DataBaseServer,
				ItemConfigurationParam.DataBaseLogin,
				ItemConfigurationParam.DataBasePassword,
				ItemConfigurationParam.DataBasePort,
				ItemConfigurationParam.DataBaseBaseName, out exception, out IsExistsDatabase);

			if (flag)
			{
				MessageBox.Show("ОК. База створена!");				    
			}
			else
			{
				MessageBox.Show("Помилка! " + (exception != null ? exception.Message : ""));
			}
		}

		//private void buttonTryConnect_Click(object sender, EventArgs e)
		//{
		//	UpdateItemConfigurationParam();

		//	Конфа.Config.Kernel = new Kernel();

		//	Exception exception = null;

		//	bool flag = Конфа.Config.Kernel.TryConnectToServer(
  //              ItemConfigurationParam.DataBaseServer,
  //              ItemConfigurationParam.DataBaseLogin,
  //              ItemConfigurationParam.DataBasePassword,
  //              ItemConfigurationParam.DataBasePort,
		//		ItemConfigurationParam.DataBaseBaseName, out exception);

		//	if (flag)
  //          {
		//		MessageBox.Show("ОК. Є підключення!");
		//	}
  //          else
  //          {
		//		MessageBox.Show("Помилка! " + (exception != null ? exception.Message : ""));
		//	}
  //      }
	}
}
