using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FoenixIDE.Processor;

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
    }
}
