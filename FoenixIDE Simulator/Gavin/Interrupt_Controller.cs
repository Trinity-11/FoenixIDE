using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoenixIDE
{
    /// <summary>
    /// Emulation of GAVIN's interrupt controller functionality 
    /// Reads are direct, Writes work like they do on actual hardware
    /// </summary>
    public class InterruptControllerRAM : MemoryRAM
    {
        #region locals
        private int startAddress; // Starting address and
        private int length;       // address length in memory map
        private FoenixSystem kernel = null;

        // The address sent by MemoryManager is mapped to device address space
        // so we'll create some locals to precalcualte the global to local address
        private int intPendReg0;  // global INT_PENDING_REG0
        private int intPendReg1;  // global INT_PENDING_REG1
        private int intPendReg2;  // global INT_PENDING_REG2
        private int intPolReg0;   // global NT_POL_REG0
        private int intPolReg1;   // global NT_POL_REG1
        private int intPolReg2;   // global NT_POL_REG2
        private int intEdgeReg0;  // global INT_EDGE_REG0
        private int intEdgeReg1;  // global INT_EDGE_REG1
        private int intEdgeReg2;  // global INT_EDGE_REG2
        private int intMaskReg0;  // global INT_MASK_REG0
        private int intMaskReg1;  // global INT_MASK_REG1
        private int intMaskReg2;  // global INT_MASK_REG2
        #endregion locals

        /// <summary>
        /// GAVIN Interrupt Controller Sources, Register 0
        /// </summary>
        //public const byte BIT0_ALAWAYS1 = 0x01;
        //public const byte VICKY_INT0 = 0x02;
        //public const byte VICKY_INT1 = 0x04;
        //public const byte TIMER0 = 0x08;
        //public const byte TIMER1 = 0x10;
        //public const byte TIMER2 = 0x20;
        //public const byte RTC = 0x40;
        //public const byte LPC_INT_6_FDC = 0x80;

        /// <summary>
        /// GAVIN Interrupt Controller Sources, Register 1
        /// </summary>
        private const byte lpcInt_1_KB = 0x01;
        //public const byte VICKY_INT2 = 0x02;
        //public const byte VICKY_INT3 = 0x04;
        //public const byte LPC_INT_COM1 = 0x08;
        //public const byte LPC_INT_COM2 = 0x10;
        //public const byte LPC_INT_5_MIDI = 0x20;
        //public const byte LPC_INT_LPT1 = 0x40;
        //public const byte SDCARD_CNTRL = 0x80;

        /// <summary>
        /// GAVIN Interrupt Controller Sources, Register 2
        /// </summary>
        //public const byte OPL2_RIGHT_CH = 0x01;
        //public const byte OPL2_LEFT_CH = 0x02;
        //public const byte BEATRIX = 0x04;
        //public const byte GAVIN_DMA = 0x08;
        //public const byte B4_ALWAYS1 = 0x10;
        //public const byte DAC_HOTPLUG = 0x20;
        //public const byte EXP_PORT_CON = 0x40;
        //public const byte B7_ALWAYS1 = 0x80;

        /// <summary>
        /// Creates an instance of the interrupt controller
        /// </summary>
        public InterruptControllerRAM(int StartAddress, int Length) : base(StartAddress, Length)
        {
            this.startAddress = StartAddress;
            this.length = Length;
            data = new byte[Length];

            intPendReg0 = MemoryLocations.MemoryMap.INT_PENDING_REG0 - startAddress;
            intPendReg1 = MemoryLocations.MemoryMap.INT_PENDING_REG1 - startAddress;
            intPendReg2 = MemoryLocations.MemoryMap.INT_PENDING_REG1 - startAddress;
            intPolReg0  = MemoryLocations.MemoryMap.INT_POL_REG0 - startAddress;
            intPolReg1  = MemoryLocations.MemoryMap.INT_POL_REG1 - startAddress;
            intPolReg2  = MemoryLocations.MemoryMap.INT_POL_REG1 - startAddress;
            intEdgeReg0 = MemoryLocations.MemoryMap.INT_EDGE_REG0 - startAddress;
            intEdgeReg1 = MemoryLocations.MemoryMap.INT_EDGE_REG1 - startAddress;
            intEdgeReg2 = MemoryLocations.MemoryMap.INT_EDGE_REG1 - startAddress;
            intMaskReg0 = MemoryLocations.MemoryMap.INT_MASK_REG0 - startAddress;
            intMaskReg1 = MemoryLocations.MemoryMap.INT_MASK_REG1 - startAddress;
            intMaskReg2 = MemoryLocations.MemoryMap.INT_MASK_REG1 - startAddress;
        }

        /// <summary>
        /// Does a direct read of device RAM/REGISTER 
        /// </summary>
        /// <param name="Address">Register address in device memory space</param>  
        public override byte ReadByte(int Address)
        {
            return data[Address];
        }

        /// <summary>
        /// Provides same write functionaly, i.e. cleaering register, as actual HW
        /// </summary>
        /// <param name="Address">Register address in device memory space</param>  
        public override void WriteByte(int Address, byte Value)
        {
            // Clear any bits set in Value in Interrupt pending Registers
            if (Address >= intPendReg0 && Address <= intPendReg2 )
            {
                data[Address] ^= Value;
            }
            else if (Address >= intPolReg0 && Address <= intPolReg2)
            {
                data[Address] = Value;
            }
            else if (Address >= intEdgeReg0 && Address <= intEdgeReg2)
            {
                data[Address] = Value;
            }
            else if (Address >= intMaskReg0 && Address <= intMaskReg2)
            {
                data[Address] = Value;
            }
            else
            {
                // default
                data[Address] = Value;
            }
        }

        /// <summary>
        /// Sets an interrupt based on source and if masked or now
        /// </summary>
        /// <param name="IntSoure">Source of interrupt</param>  
        public void setInterrupt(MemoryLocations.InterruptSources Source)
        {
            if (Source == MemoryLocations.InterruptSources.LPC_INT_1_KB)
            {
                data[intPendReg1] = (byte)(data[intPendReg1] | lpcInt_1_KB);
                kernel.CPU.Pins.IRQ = true;
            }
        }

        /// <summary>
        /// Sets a reference to the kernel created in main window
        /// </summary>
        /// <param name="kernel">Source of interrupt</param>  
        public void setKernel(FoenixSystem kernel)
        {
            this.kernel = kernel;
        }

    }
}
