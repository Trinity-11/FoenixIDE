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
        // so we'll create some locals to precalcualte the global to local address conversion
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
        private const byte bit0Always1 = 0x01;
        private const byte vickyInt0 = 0x02;
        private const byte vickyInt1 = 0x04;
        private const byte timer0 = 0x08;
        private const byte timer1 = 0x10;
        private const byte timer2 = 0x20;
        private const byte rtc = 0x40;
        private const byte lpcInt6Fdc = 0x80;

        /// <summary>
        /// GAVIN Interrupt Controller Sources, Register 1
        /// </summary>
        private const byte lpcInt_1_KB = 0x01;
        private const byte vickyInt2 = 0x02;
        private const byte vickyInt3 = 0x04;
        private const byte lpcIntCom1 = 0x08;
        private const byte lpcIntCom2 = 0x10;
        private const byte lpcInt5_MIDI = 0x20;
        private const byte lpcIntLpt1 = 0x40;
        private const byte sdCardCntrl = 0x80;

        /// <summary>
        /// GAVIN Interrupt Controller Sources, Register 2
        /// </summary>
        private const byte opl2RightCh = 0x01;
        private const byte opl2LeftCh = 0x02;
        private const byte beatrix = 0x04;
        private const byte gavinDma = 0x08;
        private const byte b4Always1 = 0x10;
        private const byte dacHotpluig = 0x20;
        private const byte expPortCon = 0x40;
        private const byte b7Always1 = 0x80;

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
                // check to see if this interrupt is masked
                if ((lpcInt_1_KB & data[intMaskReg1]) == 0)
                {
                    data[intPendReg1] = (byte)(data[intPendReg1] | lpcInt_1_KB);
                    kernel.CPU.Pins.IRQ = true;
                }
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
