using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FoenixIDE.Processor
{
    public partial class CPU
    {

        /// <summary>
        ///  Sets the registers to 8 bits. Sets the emulation flag.
        /// </summary>
        public void SetEmulationMode()
        {
            Flags.Emulation = true;
            A.Width = 1;
            A.DiscardUpper = false;
            X.Width = 1;
            X.DiscardUpper = true;
            Y.Width = 1;
            Y.DiscardUpper = true;
        }

        /// <summary>
        /// Sets the registers to 16 bits. Clears the emulation flag.
        /// </summary>
        public void SetNativeMode()
        {

        }

        /// <summary>
        ///  Sets the width of the A, X, and Y registers based on the X and M flags. 
        /// </summary>
        public void SyncFlags()
        {
            if (Flags.Emulation)
            {
                Flags.accumulatorShort = true;
                Flags.xRegisterShort = true;
            }
            A.Width = Flags.accumulatorShort ? 1 : 2;
            X.Width = Flags.xRegisterShort ? 1 : 2;
            Y.Width = Flags.xRegisterShort ? 1 : 2;
        }

    }
}
