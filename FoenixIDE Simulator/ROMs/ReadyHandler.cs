using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoenixIDE
{
    /// <summary>
    /// Use this to designate the object that is invoked when the kernel READY method is invoked. 
    /// This is also the object that is invoked when the Return key on the keyboard is pressed in immediate mode.
    /// </summary>
    public interface IReadyHandler
    {
        void Ready();
        void ReturnPressed(int LineStart);
        void PrintGreeting();
    }
}
