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
using Документи = StorageAndTrade_1_0.Документи;
using Перелічення = StorageAndTrade_1_0.Перелічення;

namespace StorageAndTrade
{
    public partial class ЗамовленняКлієнта_ТабличнаЧастина_Товари : UserControl
    {
        public ЗамовленняКлієнта_ТабличнаЧастина_Товари()
        {
            InitializeComponent();
        }

		public Документи.ЗамовленняКлієнта_Objest ЗамовленняКлієнта_Objest { get; set; }

        private void ЗамовленняКлієнта_ТабличнаЧастина_Товари_Load(object sender, EventArgs e)
        {
			dataGridViewRecords.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

			RecordsBindingList = new BindingList<Записи>();
			dataGridViewRecords.DataSource = RecordsBindingList;

			dataGridViewRecords.Columns.Add(new DataGridViewImageColumn() { Name = "Image", HeaderText = "", Width = 30, DisplayIndex = 0, Image = Properties.Resources.doc_text_image });
			dataGridViewRecords.Columns["ID"].Visible = false;
			dataGridViewRecords.Columns["Номенклатура"].Width = 100;

			//LoadRecords();
		}

		private BindingList<Записи> RecordsBindingList { get; set; }

		public void LoadRecords()
		{
			int selectRow = dataGridViewRecords.SelectedRows.Count > 0 ?
				dataGridViewRecords.SelectedRows[dataGridViewRecords.SelectedRows.Count - 1].Index : 0;

			RecordsBindingList.Clear();

			ЗамовленняКлієнта_Objest.Товари_TablePart.Read();
			foreach (Документи.ЗамовленняКлієнта_Товари_TablePart.Record record in ЗамовленняКлієнта_Objest.Товари_TablePart.Records)
			{
				RecordsBindingList.Add(new Записи
				{
					ID = record.UID.ToString(),
					Номенклатура = record.Номенклатура.UnigueID.ToString()
				});
			}

			if (selectRow != 0 && selectRow < dataGridViewRecords.Rows.Count)
			{
				dataGridViewRecords.Rows[0].Selected = false;
				dataGridViewRecords.Rows[selectRow].Selected = true;
			}
		}

		private class Записи
        {
            public string ID { get; set; }
            public string Номенклатура { get; set; }
        }
    }
}
