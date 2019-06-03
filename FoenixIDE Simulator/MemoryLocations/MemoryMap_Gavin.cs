using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// memory map of everything in GAVIN
namespace FoenixIDE.MemoryLocations
{
    public static partial class MemoryMap
    {
        // c# Direct page Addresses, i.e. global memory map addresses
        #region GAVIN Low memory registers

        public const int GAVIN_LOW_MEM_START = 0x00_0100; // Start of GAVIN's low memory registers
        public const int GAVIN_LOW_MEM_END = 0x00_019F;   // End of GAVIN's low memory registers

        #endregion GAVIN Low memory registers

        // c# Direct page Addresses, i.e. global memory map addresses
        #region Interrupt Controller

        // Interrupt Controller Address Space
        public const int INTCTRL_START = 0x00_0140; // Keyboard input, output buffer
        public const int INTCTRL_END = 0x00_015F;  // keyboard status port
        public const int INTCTRL_SIZE = 0x1F; // length of interrutp controller registers

        // Interrupt Pending Source Registers
        public const int INT_PENDING_REG0 = 0x00_0140; //[0]-Always 1, [1]-VICKY_INT0, [2]-VICKY_INT1, [3]-Timer0, [4]-Timer1, [5]-Timer2, [6]-RTC, [7]-LPC_INT[6] Floppy
        public const int INT_PENDING_REG1 = 0x00_0141; //[0]-LPC_INT[1] KB, [1]-VICKY_INT2, [2]-VICKY_INT3, [4]-LPC_INT[4] COM1, [5]-LPC_INT[5] Midi, [6]-LPC_INT[7] LPT1, [7]-SDCARD
        public const int INT_PENDING_REG2 = 0x00_0142; //[0]-OPL2_Right, [1]-OPL2_Left-, [2]-Beatrix, [3]-Gavin DMA, [4]-Always 1, [5]-DAC Hot-Plug, [6]-Exp Port Con, [7]-Always 1

        // Interrupt Polarity Set
        public const int INT_POL_REG0 = 0x00_0144;
        public const int INT_POL_REG1 = 0x00_0145;
        public const int INT_POL_REG2 = 0x00_0146;

        // Interrupt Edge Detection Enable
        public const int INT_EDGE_REG0 = 0x00_0148;
        public const int INT_EDGE_REG1 = 0x00_0149;
        public const int INT_EDGE_REG2 = 0x00_014A;

        // Interrupt Mask
        public const int INT_MASK_REG0 = 0x00_014C;
        public const int INT_MASK_REG1 = 0x00_014D;
        public const int INT_MASK_REG2 = 0x00_014E;

        #endregion Interrupt Controller

        // c# Direct page Addresses, i.e. global memory map addresses
        #region SuperIO PS2 

        // SuperIO Address Space $AF:1060 - $AF:13FF
        public const int SIO_START = 0xAF_1060; // Keyboard input, output buffer
        public const int SIO_END = 0xAF_13FF;  // keyboard status port
        public const int SIO_SIZE = 0x03A0; // length of interrutp controller registers

        public const int KBD_START = 0xAF_1060;
        public const int KBD_DATA_BUF = 0xAF_1060; // Keyboard input, output buffer
        public const int KBD_STATUS_PORT = 0xAF_1064;  // keyboard status port
        public const int KBD_END = 0xAF_1064;

        public const int PME_START = 0xAF_1100;
        public const int PME_END = 0xAF_117F;

        public const int GAME_START = 0xAF_1200;
        public const int GAME_END = 0xAF_1200;

        public const int COM2_START = 0xAF_12F8;
        public const int COM2_END = 0xAF_12FF;

        public const int MIDI_MPU401_START = 0xAF_1330;
        public const int MIDI_MPU401_END = 0xAF_1331;
        public const int MIDI_DATA = 0xAF_1330;
        public const int MIDI_STATUS_CONTROL = 0xAD_1331;

        public const int LPT_START = 0xAF_1378;
        public const int LPT_END = 0xAF_137F;

        public const int FLPY_START = 0xAF_13F0;
        public const int FLPY_END = 0xAF_13F7;

        public const int COM1_START = 0xAF_13F8;
        public const int COM1_END = 0xAF_13FF;

        #endregion SuperIO PS2
    }

    /// <summary>
    /// GAVIN Interrupt Sources
    /// </summary>
    public enum InterruptSources
    {
        /// GAVIN Interrupt Controller Sources, Register 0
        BIT0_ALAWAYS1,
        VICKY_INT0,
        VICKY_INT1,
        TIMER0,
        TIMER1,
        TIMER2,
        RTC,
        LPC_INT_6_FDC,

        /// GAVIN Interrupt Controller Sources, Register 1
        LPC_INT_1_KB,
        VICKY_INT2,
        VICKY_INT3,
        LPC_INT_COM1,
        LPC_INT_COM2,
        LPC_INT_5_MIDI,
        LPC_INT_LPT1,
        SDCARD_CNTRL,

        /// GAVIN Interrupt Controller Sources, Register 2
        OPL2_RIGHT_CH,
        OPL2_LEFT_CH,
        BEATRIX,
        GAVIN_DMA,
        B4_ALWAYS1,
        DAC_HOTPLUG,
        EXP_PORT_CON,
        B7_ALWAYS1
    }
}
