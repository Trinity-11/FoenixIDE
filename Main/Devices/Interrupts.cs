using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoenixIDE.Simulator.Devices
{
    enum Register0
    {
        FNX0_INT00_SOF = 1,         // Start of Frame @ 60FPS
        FNX0_INT01_SOL = 2,         // Start of Line (Programmable)
        FNX0_INT02_TMR0 = 4,        // Timer 0 Interrupt
        FNX0_INT03_TMR1 = 8,        // Timer 1 Interrupt
        FNX0_INT04_TMR2 = 0x10,     // Timer 2 Interrupt
        FNX0_INT05_RTC = 0x20,      // Real-Time Clock Interrupt
        FNX0_INT06_FDC = 0x40,      // Floppy Disk Controller
        FNX0_INT07_MOUSE = 0x80     // Mouse Interrupt (INT12 in SuperIO IOspace)
    }

    enum Register1
    {
        FNX1_INT00_KBD = 1,         // Keyboard Interrupt
        FNX1_INT01_SC0 = 2,         // Sprite 2 Sprite Collision
        FNX1_INT02_SC1 = 4,         // Sprite 2 Tiles Collision
        FNX1_INT03_COM2 = 8,        // Serial Port 2
        FNX1_INT04_COM1 = 0x10,     // Serial Port 1
        FNX1_INT05_MPU401 = 0x20,   // Midi Controller Interrupt
        FNX1_INT06_LPT = 0x40,      // Parallel Port
        FNX1_INT07_SDCARD = 0x80    // SD Card Controller Interrupt
    }

    enum Register2
    {
        FNX2_INT00_OPL2R = 1,       // OPL2 Right Channel
        FNX2_INT01_OPL2L = 2,       // OPL2 Left Channel
        FNX2_INT02_BTX_INT = 4,     // GABE Interrupt (TBD)
        FNX2_INT03_SDMA = 8,        // System DMA
        FNX2_INT04_VDMA = 0x10,     // Video DMA
        FNX2_INT05_DACHP = 0x20,    // DAC Hot Plug
        FNX2_INT06_EXT = 0x40,      // External Expansion
        FNX2_INT07_ALLONE = 0x80    // ??
    }

    enum Register2_FMX
    {
        FNX2_INT00_OPL3 = 1,          // OPL3
        FNX2_INT01_GABE_INT0 = 2,     // GABE (INT0) - TBD
        FNX2_INT02_GABE_INT1 = 4,     // GABE (INT1) - TBD
        FNX2_INT03_SDMA = 8,          // VICKY_II (INT4)
        FNX2_INT04_VDMA = 0x10,       // VICKY_II (INT5)
        FNX2_INT05_GABE_INT2 = 0x20,  // GABE (INT2) - TBD
        FNX2_INT06_EXT = 0x40,        // External Expansion
        FNX2_INT07_SDCARD_INS = 0x80  // SDCARD Insertion
    }

    enum Register3_FMX
    {
        FNX3_INT00_OPN2 = 1,        // OPN2
        FNX3_INT01_OPM = 2,         // OPM
        FNX3_INT02_IDE = 4,         // HDD IDE INTERRUPT
        FNX3_INT03_TBD = 8,         // TBD
        FNX3_INT04_TBD = 0x10,      // TBD
        FNX3_INT05_TBD = 0x20,      // GABE (INT2) - TBD
        FNX3_INT06_TBD = 0x40,      // External Expansion
        FNX3_INT07_TBD = 0x80       // SDCARD Insertion
    }

    enum Register0_JR
    {
        JR0_INT00_SOF = 1,
        JR0_INT01_SOL = 2,
        JR0_INT02_KBD = 4,
        JR0_INT03_MOUSE = 8,
        JR0_INT04_TMR0 = 16,
        JR0_INT05_TMR1 = 32,
        JR0_INT06_DMA = 64,
        JR0_INT07_RSVD = 128
    }

    enum Register1_JR
    {
        JR1_INT00_UART = 1,
        JR1_INT01_VKY2 = 2,
        JR1_INT02_VKY3 = 4,
        JR1_INT03_VKY4 = 8,
        JR1_INT04_RTC = 16,
        JR1_INT05_VIA = 32,
        JR1_INT06_IEC = 64,
        JR1_INT07_SDCARD = 128
    }
}
