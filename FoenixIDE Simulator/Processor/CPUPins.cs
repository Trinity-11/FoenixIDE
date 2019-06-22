using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FoenixIDE.Processor
{
    /// <summary>
    /// Class to encapsulte the state of the CPU 'Control Pins'. This is a made
    /// up term for those pins that control the CPU execution, Reset, IRQ, etc.
    /// We treat them as a singel register which makes checking to see if any
    /// one set much faster than if they were individual bool objects.
    /// </summary>
    public class CPUPins
    {
        // Pins
        /// <summary>
        /// Pause the CPU to allow slow I/O or memory operations. When true, the CPU will not execute 
        /// the next instruction.
        /// </summary>
        public bool Ready_ = false;

        /// <summary>
        /// When high, the CPU is being reset. The CPU will not execute
        /// instructions while reset is high. Once reset goes low (false),
        /// the CPU will read the reset interrupt vector from memory, set the 
        /// Program Counter to the address in the vector, and begin executing 
        /// instructions
        /// </summary>
        public bool Reset = false;

        /// <summary>
        /// Execute a non-maskable interrupt
        /// </summary>
        public bool NMI = false;

        /// <summary>
        /// Execute an interrupt request
        /// </summary>
        public bool IRQ = false;

        /// <summary>
        /// Aborts the current instruction. Control is shifted to the Abort vector.
        /// </summary>
        public bool Abort = false;

        /// <summary>
        /// When high, the CPU is reading interrupt/reset vectors
        /// </summary>
        public bool VectorPull = false;

        /// <summary>
        /// Helper method to let CPU class know an interrutp pin is high
        /// </summary>
        public bool GetInterruptPinActive
        {
            get { return Reset || NMI || IRQ || Abort; }
        }
    }
}
