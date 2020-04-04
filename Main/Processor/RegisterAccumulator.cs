using System;
using System.Collections.Generic;

using System.Text;
using System.Threading.Tasks;

namespace FoenixIDE.Processor
{
    public class RegisterAccumulator : Register
    {
        public int Value16
        {
            get { return this._value; }
            set { this._value = value; }
        }
    }
}
