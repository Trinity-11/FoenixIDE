using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoenixIDE.Basic
{
    public class Immediate : IReadyHandler
    {
        public FoenixSystem kernel = null;

        public Immediate(FoenixSystem NewKernel)
        {
            this.kernel = NewKernel;
        }

        public void PrintGreeting()
        {
            kernel.PrintLine("         FoenixIDE (not implemented)");
        }

        /// <summary>
        /// Print the immediate mode prompt.
        /// </summary>
        public void Ready()
        {
            kernel.PrintLine("Ready.");
        }

        /// <summary>
        /// Handle a line of input. This should interpret the line and either save it to memory (lines with a line number) or 
        /// execute the command (immediate mode commands.)
        /// </summary>
        public void ReturnPressed(int LineStart)
        {
            throw new NotImplementedException();
        }


    }
}
