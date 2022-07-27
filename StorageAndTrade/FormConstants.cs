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

namespace StorageAndTrade
{
    public partial class FormConstants : Form
    {
        public FormConstants()
        {
            InitializeComponent();
        }

        private void FormConstants_Load(object sender, EventArgs e)
        {
            directoryControl_Організація.Init(new Form_Організації(), new Довідники.Організації_Pointer());

            //
            //
            //

            directoryControl_Організація.DirectoryPointerItem = Константи.ЗначенняЗаЗамовчуванням.ОсновнаОрганізація_Const;
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            Константи.ЗначенняЗаЗамовчуванням.ОсновнаОрганізація_Const = (Довідники.Організації_Pointer)directoryControl_Організація.DirectoryPointerItem;
        }
    }
}
