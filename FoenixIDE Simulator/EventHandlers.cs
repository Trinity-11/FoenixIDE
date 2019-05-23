using System;
using System.Windows.Forms;
using FoenixIDE.Display;

namespace FoenixIDE
{
    public class TerminalKeyEventArgs : global::System.Windows.Forms.KeyPressEventArgs
    {
        public Keys Modifiers;
        public TerminalKeyEventArgs(char KeyChar, Keys Modifiers = Keys.None) : base(KeyChar)
        {
            this.Modifiers = Modifiers;
        }
    }
    //public delegate void KeyPressEventHandler(Gpu frameBuffer, TerminalKeyEventArgs e);
}