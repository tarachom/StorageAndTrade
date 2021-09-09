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
using РегістриНакопичення = StorageAndTrade_1_0.РегістриНакопичення;

namespace StorageAndTrade
{
    public partial class FormStorageAndTrade : Form
    {
        public FormStorageAndTrade()
        {
            InitializeComponent();
        }

        private void FormStorageAndTrade_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void касиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_Каси formCash = new Form_Каси();
            formCash.Show();
        }

        private void валютиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_Валюти form_Валюти = new Form_Валюти();
            form_Валюти.Show();
        }

        private void пакуванняОдиниціВиміруToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_ПакуванняОдиниціВиміру form_ПакуванняОдиниціВиміру = new Form_ПакуванняОдиниціВиміру();
            form_ПакуванняОдиниціВиміру.Show();
        }

        private void оганізаціїToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_Організації form_Організації = new Form_Організації();
            form_Організації.Show();
        }
    }
}
