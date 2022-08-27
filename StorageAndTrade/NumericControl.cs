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
    public partial class NumericControl : UserControl
    {
        public NumericControl()
        {
            InitializeComponent();
        }

        public event EventHandler OnChanged;

        public decimal Value
        {
            get
            {
                decimal result;
                return IsValidParse(out result) ? decimal.Parse(textBoxNumeric.Text) : 0;
            }
            set
            {
                textBoxNumeric.Text = value.ToString();
            }
        }

        public bool IsValid
        {
            get
            {
                decimal result;
                return IsValidParse(out result);
            }
        }

        private bool IsValidParse(out decimal result)
        {
            return decimal.TryParse(textBoxNumeric.Text, out result);
        }

        private void textBoxNumeric_TextChanged(object sender, EventArgs e)
        {
            if (textBoxNumeric.TextLength > 0)
            {
                string endChar = textBoxNumeric.Text.Substring(textBoxNumeric.TextLength - 1, 1);
                if (endChar == ".")
                {
                    textBoxNumeric.Text = textBoxNumeric.Text.Replace(".", ",");
                    textBoxNumeric.SelectionStart = textBoxNumeric.TextLength;
                }
            }

            decimal result;
            bool isValid = IsValidParse(out result);

            labelError.Visible = !isValid;

            OnChanged?.Invoke(this, null);
        }

        private void NumericControl_Load(object sender, EventArgs e)
        {
            Value = 0;
        }
    }
}
