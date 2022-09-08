using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Threading;
using System.IO;

using AccountingSoftware;
using Конфа = StorageAndTrade_1_0;
using Константи = StorageAndTrade_1_0.Константи;
using Довідники = StorageAndTrade_1_0.Довідники;
using Документи = StorageAndTrade_1_0.Документи;
using Перелічення = StorageAndTrade_1_0.Перелічення;
using РегістриНакопичення = StorageAndTrade_1_0.РегістриНакопичення;
using StorageAndTrade.Service;

namespace StorageAndTrade
{
    public partial class FormStorageAndTrade : Form
    {
        public FormStorageAndTrade()
        {
            InitializeComponent();
            
        }

        public ConfigurationParam OpenConfigurationParam { get; set; }

        private Thread ThreadBackgroundTask { get; set; }

        private void FormStorageAndTrade_Load(object sender, EventArgs e)
        {
            this.MdiChildActivate += FormStorageAndTrade_MdiChildActivate;

            //
            // Робочий стіл
            //

            //FormDesktop formDesktop = new FormDesktop();
            //formDesktop.MdiParent = this;
            //formDesktop.ControlBox = false;
            //formDesktop.Show();

            //FormStorageAndTrade_Resize(this, new EventArgs());

            //
            // Обробка фонових задач розрахунку віртуальних залишків
            //

            Константи.Системні.ВвімкнутиФоновіЗадачі_Const = true;

            CalculationBalances.ПідключитиДодаток_UUID_OSSP();

            ThreadBackgroundTask = new Thread(new ThreadStart(CalculationVirtualBalances));
            ThreadBackgroundTask.Start();
        }

        private void FormStorageAndTrade_Resize(object sender, EventArgs e)
        {
            Form FormDesktop = Application.OpenForms["FormDesktop"];

            if (FormDesktop != null)
            {
                //FormDesktop.Top = 0;
                //FormDesktop.Left = 0;
                //FormDesktop.Width = this.Width-20;
                //FormDesktop.Height = this.Height-142;

                //FormDesktop.Top = 500;
                //FormDesktop.Left = 0;
                //FormDesktop.Width = this.Width - 20;
                //FormDesktop.Height = this.Height - 142 - 550;
            }
        }

        private void FormStorageAndTrade_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (ThreadBackgroundTask != null)
                ThreadBackgroundTask.Abort();

            Application.Exit();
        }

        #region BackgroundTask

        void CalculationVirtualBalances()
        {
            while (true)
            {
                if (Константи.Системні.ВвімкнутиФоновіЗадачі_Const == false)
                {
                    Thread.Sleep(5000);
                    continue;
                }

                //Console.WriteLine("Обчислення");

                CalculationBalances.ОбчисленняВіртуальнихЗалишківПоДнях();
                CalculationBalances.ОбчисленняВіртуальнихЗалишківПоМісяцях();
                Thread.Sleep(5000);
            }
        }

        #endregion

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

        private void серіїНоменклатуриToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_СеріїНоменклатури form_СеріїНоменклатури = new Form_СеріїНоменклатури();
            form_СеріїНоменклатури.MdiParent = this;
            form_СеріїНоменклатури.Show();
        }

        private void партіїТоварівКомпозитToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_ПартіяТоварівКомпозит form_ПартіяТоварівКомпозит = new Form_ПартіяТоварівКомпозит();
            form_ПартіяТоварівКомпозит.MdiParent = this;
            form_ПартіяТоварівКомпозит.Show();
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

        private void введенняЗалишківToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_ВведенняЗалишківЖурнал form_ВведенняЗалишківЖурнал = new Form_ВведенняЗалишківЖурнал();
            form_ВведенняЗалишківЖурнал.MdiParent = this;
            form_ВведенняЗалишківЖурнал.Show();
        }

        private void актВиконанихРобітToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_АктВиконанихРобітЖурнал form_АктВиконанихРобітЖурнал = new Form_АктВиконанихРобітЖурнал();
            form_АктВиконанихРобітЖурнал.MdiParent = this;
            form_АктВиконанихРобітЖурнал.Show();
        }

        private void внутрішнєCпоживанняТоварівToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_ВнутрішнєСпоживанняТоварівЖурнал form_ВнутрішнєСпоживанняТоварівЖурнал = new Form_ВнутрішнєСпоживанняТоварівЖурнал();
            form_ВнутрішнєСпоживанняТоварівЖурнал.MdiParent = this;
            form_ВнутрішнєСпоживанняТоварівЖурнал.Show();
        }

        private void рахунокФактураToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_РахунокФактураЖурнал form_РахунокФактураЖурнал = new Form_РахунокФактураЖурнал();
            form_РахунокФактураЖурнал.MdiParent = this;
            form_РахунокФактураЖурнал.Show();
        }

        private void псуванняТоварівToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_ПсуванняТоварівЖурнал form_ПсуванняТоварівЖурнал = new Form_ПсуванняТоварівЖурнал();
            form_ПсуванняТоварівЖурнал.MdiParent = this;
            form_ПсуванняТоварівЖурнал.Show();
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

        private void рухКоштівToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_РухКоштів_Звіт form_РухКоштів_Звіт = new Form_РухКоштів_Звіт();
            form_РухКоштів_Звіт.MdiParent = this;
            form_РухКоштів_Звіт.Show();
        }

        private void партіїТоварівToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_ПартіїТоварів_Звіт form_ПартіїТоварів_Звіт = new Form_ПартіїТоварів_Звіт();
            form_ПартіїТоварів_Звіт.MdiParent = this;
            form_ПартіїТоварів_Звіт.Show();
        }

        private void вільніЗалишкиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_ВільніЗалишки_Звіт form_ВільніЗалишки_Звіт = new Form_ВільніЗалишки_Звіт();
            form_ВільніЗалишки_Звіт.MdiParent = this;
            form_ВільніЗалишки_Звіт.Show();
        }

        private void розрахункиЗКонтрагентамиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_РозрахункиЗКонтрагентами_Звіт form_РозрахункиЗКонтрагентами_Звіт = new Form_РозрахункиЗКонтрагентами_Звіт();
            form_РозрахункиЗКонтрагентами_Звіт.MdiParent = this;
            form_РозрахункиЗКонтрагентами_Звіт.Show();
        }

        #endregion

        #region Сервіс Меню

        private void константиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormConstants formConstants = new FormConstants();
            formConstants.MdiParent = this;
            formConstants.Show();
        }

        private void проПрограмуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormAbout formAbout = new FormAbout();
            formAbout.OpenConfigurationParam = OpenConfigurationParam;
            formAbout.ShowDialog();
        }

        private void сервіснаОбробкаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormService formService = new FormService();
            formService.ShowDialog();
        }

        #endregion

        #region Журнали

        private void повнийToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_ПовнийЖурнал form_ПовнийЖурнал = new Form_ПовнийЖурнал();
            form_ПовнийЖурнал.MdiParent = this;
            form_ПовнийЖурнал.Show();
        }

        private void продажіToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_ПродажіЖурнал form_ПродажіЖурнал = new Form_ПродажіЖурнал();
            form_ПродажіЖурнал.MdiParent = this;
            form_ПродажіЖурнал.Show();
        }

        private void закупкиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_ЗакупкиЖурнал form_ЗакупкиЖурнал = new Form_ЗакупкиЖурнал();
            form_ЗакупкиЖурнал.MdiParent = this;
            form_ЗакупкиЖурнал.Show();
        }

        private void фінансиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_ФінансиЖурнал form_ФінансиЖурнал = new Form_ФінансиЖурнал();
            form_ФінансиЖурнал.MdiParent = this;
            form_ФінансиЖурнал.Show();
        }

        private void складToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_СкладЖурнал form_СкладЖурнал = new Form_СкладЖурнал();
            form_СкладЖурнал.MdiParent = this;
            form_СкладЖурнал.Show();
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

       
        private void testToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Console.WriteLine(Константи.Системні.test_Const);

            //Константи.Системні.test_Const = new UuidAndText(Guid.Empty, "Table1");

            //Console.WriteLine(Константи.Системні.test_Const);

            //string query2 = "Insert Into test Values(@guid, @value)";
            //Dictionary<string, object> paramQuery2 = new Dictionary<string, object>();
            //paramQuery2.Add("guid", Guid.NewGuid());
            //paramQuery2.Add("value",new UuidAndText(Guid.NewGuid(), "Table"));

            //Конфа.Config.Kernel.DataBase.ExecuteSQL(query2, paramQuery2);

            //string query = "SELECT * FROM test";

            //Dictionary<string, object> paramQuery = new Dictionary<string, object>();

            //string[] columnsName;
            //List<object[]> listRow;

            //Конфа.Config.Kernel.DataBase.SelectRequest(query, paramQuery, out columnsName, out listRow);

            //Console.WriteLine((UuidAndText)listRow[0][1]);


//            bool f = true;

//            if (f)
//            {
//                string query = $@"
//SELECT id, name, img
//FROM test
//WHERE id = 1
//";


//                Dictionary<string, object> paramQuery = new Dictionary<string, object>();

//                string[] columnsName;
//                List<Dictionary<string, object>> listRow;

//                Конфа.Config.Kernel.DataBase.SelectRequest(query, paramQuery, out columnsName, out listRow);

//                if (listRow.Count > 0)
//                {
//                    Console.WriteLine(listRow[0]["id"].ToString() + " " + listRow[0]["name"].ToString());
//                    File.WriteAllBytes(@"E:\2.bmp", (byte[])listRow[0]["img"]);
//                }
//            }
//            else
//            {
//                string query = $@"
//INSERT INTO test(name, img)
//VALUES(@name, @img)
//";
//                Dictionary<string, object> param = new Dictionary<string, object>();
//                param.Add("name", "name 1");
//                param.Add("img", File.ReadAllBytes(@"E:\1.bmp"));

//                Конфа.Config.Kernel.DataBase.ExecuteSQL(query, param);
//            }

            

        }

        
    }
}
