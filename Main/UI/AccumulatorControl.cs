using System.ComponentModel;
using System.Drawing;
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
        bool _readOnly;
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
                    regB.ForeColor = regA.ForeColor;
                    regB.BackColor = regA.BackColor;
                }
                this.regB.Text = value.Substring(0, 2);
                this.regA.Text = value.Substring(2, 2);
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
                this.regA.ReadOnly = value;
                this.regB.ReadOnly = value;
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

        private void regA_TextChanged(object sender, System.EventArgs e)
        {
            //try
            //{
            //    _register.Value = System.Convert.ToInt16(regA.Text, 16) & 0xFF;
            //}
            //catch (System.Exception)
            //{
            //    this.regA.Text = _value.Substring(2, 2);
            //}
            
        }

        private void regB_TextChanged(object sender, System.EventArgs e)
        {
            //try
            //{
            //    _register.Value16 = System.Convert.ToInt16(regB.Text + regA.Text, 16) & 0xFFFF;
            //}
            //catch (System.Exception)
            //{
            //    this.regB.Text = _value.Substring(0, 2);
            //}
        }
    }
}
