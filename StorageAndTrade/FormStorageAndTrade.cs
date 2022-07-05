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
using Документи = StorageAndTrade_1_0.Документи;
using Перелічення = StorageAndTrade_1_0.Перелічення;
using РегістриНакопичення = StorageAndTrade_1_0.РегістриНакопичення;
using StorageAndTrade_1_0.Service;

namespace StorageAndTrade
{
    public partial class FormStorageAndTrade : Form
    {
        public FormStorageAndTrade()
        {
            InitializeComponent();
        }

        private void FormStorageAndTrade_Load(object sender, EventArgs e)
        {
            this.MdiChildActivate += FormStorageAndTrade_MdiChildActivate;
        }

        private void FormStorageAndTrade_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        #region Довідники Меню

        private void валютиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_Валюти form_Валюти = new Form_Валюти();
            form_Валюти.MdiParent = this;
            form_Валюти.Show();
        }

        private void пакуванняОдиниціВиміруToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_ПакуванняОдиниціВиміру form_ПакуванняОдиниціВиміру =new Form_ПакуванняОдиниціВиміру();
            form_ПакуванняОдиниціВиміру.MdiParent = this;
            form_ПакуванняОдиниціВиміру.Show();
        }

        private void оганізаціїToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_Організації form_Організації = new Form_Організації();
            form_Організації.MdiParent = this;
            form_Організації.Show();
        }

        private void номенклатураToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_Номенклатура form_Номенклатура = new Form_Номенклатура();
            form_Номенклатура.MdiParent = this;
            form_Номенклатура.Show();
        }

        private void контрагентиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_Контрагенти form_Контрагенти = new Form_Контрагенти();
            form_Контрагенти.MdiParent = this;
            form_Контрагенти.Show();
        }

        private void касиToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Form_Каси formCash = new Form_Каси();
            formCash.MdiParent = this;
            formCash.Show();
        }

        private void складиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_Склади form_Склади = new Form_Склади();
            form_Склади.MdiParent = this;
            form_Склади.Show();
        }

        private void видиЦінToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_ВидиЦін form_ВидиЦін = new Form_ВидиЦін();
            form_ВидиЦін.MdiParent = this;
            form_ВидиЦін.Show();
        }

        private void фізичніОсобиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_ФізичніОсоби form_ФізичніОсоби = new Form_ФізичніОсоби();
            form_ФізичніОсоби.MdiParent = this;
            form_ФізичніОсоби.Show();
        }

        private void користувачіToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_Користувачі form_Користувачі = new Form_Користувачі();
            form_Користувачі.MdiParent = this;
            form_Користувачі.Show();
        }

        private void виробникиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_Виробники form_Виробники = new Form_Виробники();
            form_Виробники.MdiParent = this;
            form_Виробники.Show();
        }

        private void видиНоменклатуриToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_ВидиНоменклатури form_ВидиНоменклатури = new Form_ВидиНоменклатури();
            form_ВидиНоменклатури.MdiParent = this;
            form_ВидиНоменклатури.Show();
        }

        private void структураПідприємстваToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_СтруктураПідприємства form_СтруктураПідприємства = new Form_СтруктураПідприємства();
            form_СтруктураПідприємства.MdiParent = this;
            form_СтруктураПідприємства.Show();
        }

        private void банківськіРахункиОрганізаційToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_БанківськіРахункиОрганізацій form_БанківськіРахункиОрганізацій = new Form_БанківськіРахункиОрганізацій();
            form_БанківськіРахункиОрганізацій.MdiParent = this;
            form_БанківськіРахункиОрганізацій.Show();
        }

        private void договориКонтрагентівToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_ДоговориКонтрагентів form_ДоговориКонтрагентів = new Form_ДоговориКонтрагентів();
            form_ДоговориКонтрагентів.MdiParent = this;
            form_ДоговориКонтрагентів.Show();
        }

        private void банківськіРахункиКонтрагентівToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_БанківськіРахункиКонтрагентів form_БанківськіРахункиКонтрагентів = new Form_БанківськіРахункиКонтрагентів();
            form_БанківськіРахункиКонтрагентів.MdiParent = this;
            form_БанківськіРахункиКонтрагентів.Show();
        }

        private void характеристикиНоменклатуриToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_ХарактеристикиНоменклатури form_ХарактеристикиНоменклатури = new Form_ХарактеристикиНоменклатури();
            form_ХарактеристикиНоменклатури.MdiParent = this;
            form_ХарактеристикиНоменклатури.Show();
        }

        #endregion

        #region Документи Меню

        private void замовленняКлієнтаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_ЗамовленняКлієнтаЖурнал form_ЗамовленняКлієнтаЖурнал = new Form_ЗамовленняКлієнтаЖурнал();
            form_ЗамовленняКлієнтаЖурнал.MdiParent = this;
            form_ЗамовленняКлієнтаЖурнал.Show();
        }

        private void реалізаціяТоварівТаПослугToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_РеалізаціяТоварівТаПослугЖурнал form_РеалізаціяТоварівТаПослугЖурнал = new Form_РеалізаціяТоварівТаПослугЖурнал();
            form_РеалізаціяТоварівТаПослугЖурнал.MdiParent = this;
            form_РеалізаціяТоварівТаПослугЖурнал.Show();
        }

        private void замовленняПостачальникуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_ЗамовленняПостачальникуЖурнал form_ЗамовленняПостачальникуЖурнал = new Form_ЗамовленняПостачальникуЖурнал();
            form_ЗамовленняПостачальникуЖурнал.MdiParent = this;
            form_ЗамовленняПостачальникуЖурнал.Show();
        }

        private void поступленняТоварівТаПослугToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_ПоступленняТоварівТаПослугЖурнал form_ПоступленняТоварівТаПослугЖурнал = new Form_ПоступленняТоварівТаПослугЖурнал();
            form_ПоступленняТоварівТаПослугЖурнал.MdiParent = this;
            form_ПоступленняТоварівТаПослугЖурнал.Show();
        }

        private void поверненняТоварівПостачальникуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_ПоверненняТоварівПостачальникуЖурнал form_ПоверненняТоварівПостачальникуЖурнал = new Form_ПоверненняТоварівПостачальникуЖурнал();
            form_ПоверненняТоварівПостачальникуЖурнал.MdiParent = this;
            form_ПоверненняТоварівПостачальникуЖурнал.Show();
        }

        private void поверненняТоварівКлієнтуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_ПоверненняТоварівВідКлієнтаЖурнал form_ПоверненняТоварівВідКлієнтаЖурнал = new Form_ПоверненняТоварівВідКлієнтаЖурнал();
            form_ПоверненняТоварівВідКлієнтаЖурнал.MdiParent = this;
            form_ПоверненняТоварівВідКлієнтаЖурнал.Show();
        }

        private void прихіднийКасовийОрдерToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_ПрихіднийКасовийОрдерЖурнал form_ПрихіднийКасовийОрдерЖурнал = new Form_ПрихіднийКасовийОрдерЖурнал();
            form_ПрихіднийКасовийОрдерЖурнал.MdiParent = this;
            form_ПрихіднийКасовийОрдерЖурнал.Show();
        }

        private void розхіднийКасовийОрдерToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_РозхіднийКасовийОрдерЖурнал form_РозхіднийКасовийОрдерЖурнал = new Form_РозхіднийКасовийОрдерЖурнал();
            form_РозхіднийКасовийОрдерЖурнал.MdiParent = this;
            form_РозхіднийКасовийОрдерЖурнал.Show();
        }

        private void переміщенняТоварівМіжСкладамиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_ПереміщенняТоварівЖурнал form_ПереміщенняТоварівЖурнал = new Form_ПереміщенняТоварівЖурнал();
            form_ПереміщенняТоварівЖурнал.MdiParent = this;
            form_ПереміщенняТоварівЖурнал.Show();
        }

        private void встановленняЦінНоменклатуриToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_ВстановленняЦінНоменклатуриЖурнал form_ВстановленняЦінНоменклатуриЖурнал = new Form_ВстановленняЦінНоменклатуриЖурнал();
            form_ВстановленняЦінНоменклатуриЖурнал.MdiParent = this;
            form_ВстановленняЦінНоменклатуриЖурнал.Show();
        }

        #endregion

        #region Звіти Меню

        private void замовленняКлієнтівToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_ЗамовленняКлієнтів_Звіт form_ЗамовленняКлієнтів_Звіт = new Form_ЗамовленняКлієнтів_Звіт();
            form_ЗамовленняКлієнтів_Звіт.MdiParent = this;
            form_ЗамовленняКлієнтів_Звіт.Show();
        }

        private void розрахункиЗКлієнтамиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_РозрахункиЗКлієнтами_Звіт form_РозрахункиЗКлієнтами_Звіт = new Form_РозрахункиЗКлієнтами_Звіт();
            form_РозрахункиЗКлієнтами_Звіт.MdiParent = this;
            form_РозрахункиЗКлієнтами_Звіт.Show();
        }

        private void розрахункиЗПостачальникамиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_РозрахункиЗПостачальниками_Звіт form_РозрахункиЗПостачальниками_Звіт = new Form_РозрахункиЗПостачальниками_Звіт();
            form_РозрахункиЗПостачальниками_Звіт.MdiParent = this;
            form_РозрахункиЗПостачальниками_Звіт.Show();
        }

        private void товариНаСкладахToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_ТовариНаСкладах_Звіт form_ТовариНаСкладах_Звіт = new Form_ТовариНаСкладах_Звіт();
            form_ТовариНаСкладах_Звіт.MdiParent = this;
            form_ТовариНаСкладах_Звіт.Show();
        }

        private void замовленняПостачальникамToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_ЗамовленняПостачальникам_Звіт form_ЗамовленняПостачальникам_Звіт = new Form_ЗамовленняПостачальникам_Звіт();
            form_ЗамовленняПостачальникам_Звіт.MdiParent = this;
            form_ЗамовленняПостачальникам_Звіт.Show();
        }

        #endregion

        #region Сервіс Меню

        private void обчислитиЗалишкиПоРегістрахToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormService formService = new FormService();
            formService.MdiParent = this;
            formService.Show();            
        }

        private void проПрограмуToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        #endregion

        #region Журнали

        private void продажіToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_ПродажіЖурнал form_ПродажіЖурнал = new Form_ПродажіЖурнал();
            form_ПродажіЖурнал.MdiParent = this;
            form_ПродажіЖурнал.Show();
        }

        #endregion

        #region Mdi

        private List<Form> ChildFormList = new List<Form>();

        private void ChildFormClosed(object sender, FormClosedEventArgs e)
        {
            Form form = (Form)sender;
            ChildFormList.Remove(form);
            ReloadMdiChild();
        }

        private void ReloadMdiChild()
        {
            int counter = 0;

            toolStrip_ВідкритіФорми.Items.Clear();

            foreach (Form form in ChildFormList)
            {
                ToolStripButton toolStripButton =
                    new ToolStripButton(form.Text, Properties.Resources.doc_text_image, ToolStripButton_Click, counter.ToString())
                    { ImageAlign = ContentAlignment.MiddleLeft };

                toolStrip_ВідкритіФорми.Items.Add(toolStripButton);

                counter++;
            }
        }

        private void FormStorageAndTrade_MdiChildActivate(object sender, EventArgs e)
        {
            Form form = this.ActiveMdiChild;

            if (form == null)
            {
                ChildFormList.Clear();
                ReloadMdiChild();
            }
            else if (!ChildFormList.Contains(form))
            {
                ChildFormList.Add(form);
                form.FormClosed += new FormClosedEventHandler(ChildFormClosed);

                ReloadMdiChild();
            }
        }

        private void ToolStripButton_Click(object sender, EventArgs e)
        {
            ToolStripButton toolStripButton = (ToolStripButton)sender;
            int counter = int.Parse(toolStripButton.Name);

            Form form = this.MdiChildren[counter];

            if (form != null)
            {
                form.Activate();
                this.Refresh();
            }
        }

        #endregion

        #region ДОВІДНИКИ Панель

        // ДОВІДНИКИ

        private void toolStripButton_Валюти_Click(object sender, EventArgs e)
        {
            валютиToolStripMenuItem_Click(this, new EventArgs());
        }

        private void toolStripButton_ПакуванняОдиниціВиміру_Click(object sender, EventArgs e)
        {
            пакуванняОдиниціВиміруToolStripMenuItem_Click(this, new EventArgs());
        }

        private void toolStripButton_Оганізації_Click(object sender, EventArgs e)
        {
            оганізаціїToolStripMenuItem_Click(this, new EventArgs());
        }

        private void toolStripButton_Номенклатура_Click(object sender, EventArgs e)
        {
            номенклатураToolStripMenuItem_Click(this, new EventArgs());
        }

        private void toolStripButton_Контрагенти_Click(object sender, EventArgs e)
        {
            контрагентиToolStripMenuItem_Click(this, new EventArgs());
        }

        private void toolStripButton_Склади_Click(object sender, EventArgs e)
        {
            складиToolStripMenuItem_Click(this, new EventArgs());
        }

        #endregion

        #region ДОКУМЕНТИ Панель

        // ДОКУМЕНТИ

        private void toolStripButton_ЗамовленняКлієнта_Click(object sender, EventArgs e)
        {
            замовленняКлієнтаToolStripMenuItem_Click(this, new EventArgs());
        }

        private void toolStripButton_РеалізаціяТоварівТаПослуг_Click(object sender, EventArgs e)
        {
            реалізаціяТоварівТаПослугToolStripMenuItem_Click(this, new EventArgs());
        }

        private void toolStripButton_ЗамовленняПостачальнику_Click(object sender, EventArgs e)
        {
            замовленняПостачальникуToolStripMenuItem_Click(this, new EventArgs());
        }

        private void toolStripButton_ПоступленняТоварівТаПослуг_Click(object sender, EventArgs e)
        {
            поступленняТоварівТаПослугToolStripMenuItem_Click(this, new EventArgs());
        }

        private void toolStripButton_ПоверненняТоварівПостачальнику_Click(object sender, EventArgs e)
        {
            поверненняТоварівПостачальникуToolStripMenuItem_Click(this, new EventArgs());
        }

        private void toolStripButton_ПоверненняТоварівВідКлієнтів_Click(object sender, EventArgs e)
        {
            поверненняТоварівКлієнтуToolStripMenuItem_Click(this, new EventArgs());
        }

        private void toolStripButton_ПрихіднийКасовийОрдер_Click(object sender, EventArgs e)
        {
            прихіднийКасовийОрдерToolStripMenuItem_Click(this, new EventArgs());
        }

        private void toolStripButton_РозхіднийКасовийОрдер_Click(object sender, EventArgs e)
        {
            розхіднийКасовийОрдерToolStripMenuItem_Click(this, new EventArgs());
        }

        private void toolStripButton_ПереміщенняТоварівМіжСкладами_Click(object sender, EventArgs e)
        {
            переміщенняТоварівМіжСкладамиToolStripMenuItem_Click(this, new EventArgs());
        }

        private void toolStripButton_ВстановленняЦінНоменклатури_Click(object sender, EventArgs e)
        {
            встановленняЦінНоменклатуриToolStripMenuItem_Click(this, new EventArgs());
        }




        #endregion

        
    }
}
