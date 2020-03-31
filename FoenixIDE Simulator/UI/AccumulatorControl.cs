using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FoenixIDE.Processor;

namespace FoenixIDE.UI
{
    public partial class AccumulatorControl : UserControl
    {
        public AccumulatorControl()
        {
            InitializeComponent();
        }

        string _caption;
        string _value;
        FoenixIDE.Processor.RegisterAccumulator _register = null;

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
                if (_register == null || _register.Width == 1)
                {
                    regB.ForeColor = SystemColors.Info;
                    regB.BackColor = SystemColors.InfoText;
                }
                else
                {
                    regB.ForeColor = SystemColors.WindowText;
                    regB.BackColor = SystemColors.Window;
                }
                this.regB.Text = value.Substring(0, 2);
                this.regA.Text = value.Substring(2, 2);
            }
        }

        [Browsable(false)]
        public RegisterAccumulator Register
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
            if (Register != null)
            {
                this.Value = _register.Value16.ToString("X4");
            }
        }
    }
}
