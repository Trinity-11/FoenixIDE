using FoenixIDE.Processor;
using System.ComponentModel;
using System.Windows.Forms;

namespace FoenixIDE.UI
{
    public partial class RegisterControl : UserControl
    {
        public RegisterControl()
        {
            InitializeComponent();
        }

        string _caption;
        string _value;
        bool _readOnly;
        FoenixIDE.Processor.Register _register = null;
        FoenixIDE.Processor.RegisterBankNumber _bank = null;

        public string Caption
        {
            get { return this._caption; }
            set
            {
                this._caption = value;
                this.label1.Text = value;
            }
        }

        public string Value
        {
            get
            {
                return this._value;
            }

            set
            {
                this._value = value;
                this.textBox1.Text = value;
            }
        }

        public bool ReadOnly
        {
            get
            {
                return this._readOnly;
            }

            set
            {
                this._readOnly = value;
                this.textBox1.ReadOnly = value;
            }
        }

        [Browsable(false)]
        public Register Register
        {
            get
            {
                return this._register;
            }

            set
            {
                this._register = value;
                if (value != null)
                {
                    UpdateValue();
                }

            }
        }

        public void UpdateValue()
        {
            if (Bank != null && Register != null)
            {
                this.Value = Bank.Value.ToString("X2") + this._register.Value.ToString("X4");
            }
            else if (Register != null)
            {
                this.Value = _register.ToString();
            }
        }

        public RegisterBankNumber Bank
        {
            get { return this._bank; }
            set
            {
                this._bank = value;
                UpdateValue();
            }
        }

        private void textBox1_TextChanged(object sender, System.EventArgs e)
        {
            //if (!ignoreChange)
            //{
            //    if (Bank != null && Register != null)
            //    {
            //        _register.Value = System.Convert.ToInt16(textBox1.Text, 16) & 0xFFFF;
            //    }
            //    else if (Register is Flags)
            //    {
            //        string val = textBox1.Text;
            //        byte bits = 0;
            //        byte bit = 0x80;
            //        for (int i = 0; i < 8; i++)
            //        {
            //            if (!'-'.Equals(val[i]))
            //            {
            //                bits += bit;
            //            }
            //            bit >>= 1;
            //        }
            //        _register.Value = bits;
            //    }
            //    else if (Register != null)
            //    {
            //        try
            //        {
            //            _register.Value = System.Convert.ToInt16(textBox1.Text, 16) & 0xFFFF;
            //        }
            //        catch (System.Exception)
            //        {
            //            textBox1.Text = _register.ToString();
            //        }
            //    }
            //}
        }
    }
}
