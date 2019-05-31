using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FoenixIDE.Simulator.Basic;

namespace FoenixIDE
{
    /// <summary>
    /// Emulation of SuperIO chip which is interface with GAVIN's 
    /// but is ampped into VICKY's address space.
    /// Reads are direct, Writes work like they do on actual hardware
    /// </summary>
    public class SuperIO_RAM : MemoryRAM
    {
        #region locals
        private int startAddress; // Starting address and
        private int length;       // address length in memory map
        private FoenixSystem kernel = null;

        #endregion locals

        #region const defs KB

        private const int STATUS_PORT =	0xAF_1064;
        private const int KBD_OUT_BUF = 0xAF_1060;
        private const int KBD_INPT_BUF = 0xAF_1060;
        private const int KBD_CMD_BUF = 0xAF_1064;
        private const int KBD_DATA_BUF = 0xAF_1060;
        private const int PORT_A = 0xAF_1060;
        private const int PORT_B = 0xAF_1061;

        // Status
        private const byte OUT_BUF_FULL = 0x01;
        private const byte INPT_BUF_FULL = 0x02;
        private const byte SYS_FLAG = 0x04;
        private const byte CMD_DATA = 0x08;
        private const byte KEYBD_INH = 0x10;
        private const byte TRANS_TMOUT = 0x20;
        private const byte RCV_TMOUT = 0x40;
        private const byte PARITY_EVEN = 0x80;
        private const byte INH_KEYBOARD = 0x10;
        private const byte KBD_ENA = 0xAE;
        private const byte KBD_DIS = 0xAD;

        // Keyboard Commands
        private const byte KB_MENU = 0xF1;
        private const byte KB_ENABLE = 0xF4;
        private const byte KB_MAKEBREAK = 0xF7;
        private const byte KB_ECHO = 0xFE;
        private const byte KB_RESET = 0xFF;
        private const byte KB_LED_CMD = 0xED;

        // Keyboard responses
        private const byte KB_OK = 0xAA;
        private const byte KB_ACK = 0xFA;
        private const byte KB_OVERRUN = 0xFF;
        private const byte KB_RESEND = 0xFE;
        private const byte KB_BREAK = 0xF0;
        private const byte KB_FA = 0x10;
        private const byte KB_FE = 0x20;
        private const byte KB_PR_LED = 0x40;

        #endregion const defs KB

        /// <summary>
        /// Creates an instance of the SuperIO controller
        /// </summary>
        public SuperIO_RAM(int StartAddress, int Length) : base(StartAddress, Length)
        {
            this.startAddress = StartAddress;
            this.length = Length;
            data = new byte[Length];
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
        /// Provides direct write functionaly to memory or if a postWrite
        /// method has been defined will raise an event to call postWrite
        /// </summary>
        /// <param name="Address">Register address in device memory space</param>
        /// <param name="Value">Value to write into memeory</param> 
        public override void WriteByte(int Address, byte Value)
        {
            if (postWrite != null)
            {
                byte old = data[Address];
                data[Address] = Value;
                postWrite.Invoke(Address, old, Value);
            }
            else
            {
                data[Address] = Value;
            }

        }

        /// <summary>
        /// Called by Main Window to send key press from UI to KB controller
        /// </summary>
        /// <param name="Key">key press scan code</param>
        public void KeyPress(ScanCode Key)
        {
            this.WriteByte(0, (byte)Key);
            this.WriteByte(4, 0);
            kernel.Memory.INTCTRL.setInterrupt(MemoryLocations.InterruptSources.LPC_INT_1_KB);
        }

        /// <summary>
        /// This is used to simulate the Keyboard Controller Registers
        /// </summary>
        /// <param name="address">Memory address</param>
        /// <param name="o">Old value of memory</param>
        /// <param name="n">New value of memory</param>
        public void OnKeyboardStatusCodeChange(int address, byte o, byte n)
        {
            // In order to avoid an infinite loop, we write to the device directly
            switch (address)
            {
                case 0: // data write/read
                    byte command = data[0];
                    switch (command)
                    {
                        case 0x69: // Write data to internal RAM
                            data[4] = 1;
                            break;
                        case 0xEE: // echo command
                            data[4] = 1;
                            break;
                        case 0xF4: // Pulse output line low 6ms
                            data[0] = 0xFA;
                            data[4] = 1;
                            break;
                        case 0xF6: // Pulse output line low 6ms 
                            data[4] = 1;
                            break;
                    }
                    break;
                case 4: // command/status
                    byte reg = data[4];
                    switch (reg)
                    {
                        case 0x20: // read byte 0 from RAM
                            data[4] = 1;
                            break;
                        case 0x60: // write next byte to byte 0 of internal RAM
                            data[4] = 0;
                            break;
                        case 0xAA: // test PS/2 controller
                            data[0] = 0x55;
                            data[4] = 1;
                            break;
                        case 0xA8: // enable 2nd PS/2 port
                            data[4] = 1;
                            break;
                        case 0xA9: // test 2nd PS/2 port
                            data[0] = 0;
                            data[4] = 1;
                            break;
                        case 0xAB: // test 1st PS/2 port
                            data[0] = 0;
                            break;
                        case 0xD4: // write next byte to secnond PS2 port input buffer
                            data[4] = 1;
                            break;
                    }
                    break;
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
