using System;
using System.Collections.Generic;
using System.Linq;
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
        // Weighted list of 'Control Pin' types
        private enum PinTypes
        {
            PIN_READY = 0x01,
            PIN_RESET = 0x02,
            PIN_NMI = 0x04,
            PIN_IRQ = 0x08,
            PIN_ABORT = 0x10,
            PIN_VECTOR_PULL = 0x20
        }

        private uint CtrlPins = 0;  // container to hold status biuts for control pins

        /// <summary>
        /// Easy was to check to see if any 'Control Pin' is set
        /// </summary>
        public bool GetCtrlPins
        {
            get { return CtrlPins != 0; }
        }

        /// <summary>
        /// State of the Read input Pin
        /// Pause the CPU to allow slow I/O or memory operations. 
        /// When true, the CPU will not execute the next instruction.
        /// </summary>
        public bool Ready_
        {
            get { return (CtrlPins & (byte)PinTypes.PIN_READY) > 0; }
            set
            {
                CtrlPins &= ~((uint)PinTypes.PIN_READY); // clear bit
                if (value)
                {
                    CtrlPins |= (uint)PinTypes.PIN_READY; // set bit
                }
            }
        }

        /// <summary>
        /// State of the Reset input Pin
        /// When high, the CPU is being reset. The CPU will not execute
        /// instructions while reset is high. Once reset goes low (false),
        /// the CPU will read the reset interrupt vector from memory, set the 
        /// Program Counter to the address in the vector, and begin executing 
        /// instructions
        /// </summary>
        public bool Reset
        {
            get { return (CtrlPins & (byte)PinTypes.PIN_RESET) > 0; }
            set
            {
                CtrlPins &= ~((uint)PinTypes.PIN_RESET); // clear bit
                if (value)
                {
                    CtrlPins |= (uint)PinTypes.PIN_RESET; // set bit
                }
            }
        }

        /// <summary>
        /// State of the Non Maskable Interrupt input Pin
        /// Will cause the NMI handler to be called
        /// </summary>
        public bool NMI
        {
            get { return (CtrlPins & (byte)PinTypes.PIN_NMI) > 0; }
            set
            {
                CtrlPins &= ~((uint)PinTypes.PIN_NMI); // clear bit
                if (value)
                {
                    CtrlPins |= (uint)PinTypes.PIN_NMI; // set bit
                }
            }
        }

        /// <summary>
        /// State of the Interrupt Request input Pin
        /// Will cause the IRQ handler to be called unless masked
        /// </summary>
        public bool IRQ
        {
            get
            {
                return (CtrlPins & (byte)PinTypes.PIN_IRQ) != 0;
            }
            set
            {
                CtrlPins &= ~((uint)PinTypes.PIN_IRQ); // clear bit
                if (value)
                {
                    CtrlPins |= (uint)PinTypes.PIN_IRQ; // set bit
                }
            }
        }

        /// <summary>
        /// State of the Abort input pin
        /// Aborts the current instruction. Control is shifted to the Abort vector.
        /// </summary>
        public bool Abort
        {
            get { return (CtrlPins & (byte)PinTypes.PIN_ABORT) != 0; }
            set
            {
                CtrlPins &= ~((uint)PinTypes.PIN_ABORT); // clear bit
                if (value)
                {
                    CtrlPins |= (uint)PinTypes.PIN_ABORT; // set bit
                }
            }
        }

        /// <summary>
        /// State of Vector Pull output pin
        /// When high, the CPU is reading interrupt/reset vectors
        /// </summary>
        public bool VectorPull
        {
            get { return (CtrlPins & (byte)PinTypes.PIN_VECTOR_PULL) != 0; }
            set
            {
                CtrlPins &= ~((uint)PinTypes.PIN_VECTOR_PULL); // clear bit
                if (value)
                {
                    CtrlPins |= (uint)PinTypes.PIN_VECTOR_PULL; // set bit
                }
            }
    }
        
    }
}
