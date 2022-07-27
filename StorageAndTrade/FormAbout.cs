using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StorageAndTrade
{
    public partial class FormAbout : Form
    {
        public ConfigurationParam OpenConfigurationParam { get; set; }

        public FormAbout()
        {
            InitializeComponent();
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://accounting.org.ua/storage_and_trade.html");
        }

        private void FormAbout_Load(object sender, EventArgs e)
        {
            if (OpenConfigurationParam != null)
                textBoxInfo.Text =
                    "Конфігурація: " + OpenConfigurationParam.ConfigurationName + "\r\n" +
                    "Сервер: " + OpenConfigurationParam.DataBaseServer + "\r\n" +
                    "База: " + OpenConfigurationParam.DataBaseBaseName + "\r\n";
        }
    }
}
