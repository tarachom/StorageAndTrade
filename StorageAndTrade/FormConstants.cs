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
            directoryControl_Склад.Init(new Form_Склади(), new Довідники.Склади_Pointer());
            directoryControl_Валюта.Init(new Form_Валюти(), new Довідники.Валюти_Pointer());
            directoryControl_Постачальник.Init(new Form_Контрагенти(), new Довідники.Контрагенти_Pointer());
            directoryControl_Покупець.Init(new Form_Контрагенти(), new Довідники.Контрагенти_Pointer());
            directoryControl_Каса.Init(new Form_Каси(), new Довідники.Каси_Pointer());
            directoryControl_ОдиницяПакування.Init(new Form_ПакуванняОдиниціВиміру(), new Довідники.ПакуванняОдиниціВиміру_Pointer());

            //
            //
            //

            directoryControl_Організація.DirectoryPointerItem = Константи.ЗначенняЗаЗамовчуванням.ОсновнаОрганізація_Const;
            directoryControl_Склад.DirectoryPointerItem = Константи.ЗначенняЗаЗамовчуванням.ОснонийСклад_Const;
            directoryControl_Валюта.DirectoryPointerItem = Константи.ЗначенняЗаЗамовчуванням.ОсновнаВалюта_Const;
            directoryControl_Постачальник.DirectoryPointerItem = Константи.ЗначенняЗаЗамовчуванням.ОсновнийПостачальник_Const;
            directoryControl_Покупець.DirectoryPointerItem = Константи.ЗначенняЗаЗамовчуванням.ОсновнийПокупець_Const;
            directoryControl_Каса.DirectoryPointerItem = Константи.ЗначенняЗаЗамовчуванням.ОсновнаКаса_Const;
            directoryControl_ОдиницяПакування.DirectoryPointerItem = Константи.ЗначенняЗаЗамовчуванням.ОсновнаОдиницяПакування_Const;
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            Константи.ЗначенняЗаЗамовчуванням.ОсновнаОрганізація_Const = (Довідники.Організації_Pointer)directoryControl_Організація.DirectoryPointerItem;
            Константи.ЗначенняЗаЗамовчуванням.ОснонийСклад_Const = (Довідники.Склади_Pointer)directoryControl_Склад.DirectoryPointerItem;
            Константи.ЗначенняЗаЗамовчуванням.ОсновнаВалюта_Const = (Довідники.Валюти_Pointer)directoryControl_Валюта.DirectoryPointerItem;
            Константи.ЗначенняЗаЗамовчуванням.ОсновнийПостачальник_Const = (Довідники.Контрагенти_Pointer)directoryControl_Постачальник.DirectoryPointerItem;
            Константи.ЗначенняЗаЗамовчуванням.ОсновнийПокупець_Const = (Довідники.Контрагенти_Pointer)directoryControl_Покупець.DirectoryPointerItem;
            Константи.ЗначенняЗаЗамовчуванням.ОсновнаКаса_Const = (Довідники.Каси_Pointer)directoryControl_Каса.DirectoryPointerItem;
            Константи.ЗначенняЗаЗамовчуванням.ОсновнаОдиницяПакування_Const = (Довідники.ПакуванняОдиниціВиміру_Pointer)directoryControl_ОдиницяПакування.DirectoryPointerItem;
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
