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

        private void номенклатураToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_Номенклатура form_Номенклатура = new Form_Номенклатура();
            form_Номенклатура.Show();
        }

        private void контрагентиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_Контрагенти form_Контрагенти = new Form_Контрагенти();
            form_Контрагенти.Show();
        }

        private void касиToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Form_Каси formCash = new Form_Каси();
            formCash.Show();
        }

        private void складиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_Склади form_Склади = new Form_Склади();
            form_Склади.Show();
        }

        private void видиЦінToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_ВидиЦін form_ВидиЦін = new Form_ВидиЦін();
            form_ВидиЦін.Show();
        }

        private void фізичніОсобиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_ФізичніОсоби form_ФізичніОсоби = new Form_ФізичніОсоби();
            form_ФізичніОсоби.Show();
        }

        private void користувачіToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_Користувачі form_Користувачі = new Form_Користувачі();
            form_Користувачі.Show();
        }
    }
}
