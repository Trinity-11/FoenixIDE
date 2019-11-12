using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoenixIDE.Simulator.Devices
{
    enum Register0
    {
        FNX0_INT00_SOF = 1,      // Start of Frame @ 60FPS
        FNX0_INT01_SOL = 2,      // Start of Line (Programmable)
        FNX0_INT02_TMR0 = 4,     // Timer 0 Interrupt
        FNX0_INT03_TMR1 = 8,     // Timer 1 Interrupt
        FNX0_INT04_TMR2 = 0x10,  // Timer 2 Interrupt
        FNX0_INT05_RTC = 0x20,   // Real-Time Clock Interrupt
        FNX0_INT06_FDC = 0x40,   // Floppy Disk Controller
        FNX0_INT07_MOUSE = 0x80  // Mouse Interrupt (INT12 in SuperIO IOspace)
    }

    enum Register1
    {
        FNX1_INT00_KBD = 1,      // Keyboard Interrupt
        FNX1_INT01_SC0 = 2,      // Sprite 2 Sprite Collision
        FNX1_INT02_SC1 = 4,      // Sprite 2 Tiles Collision
        FNX1_INT03_COM2 = 8,     // Serial Port 2
        FNX1_INT04_COM1 = 0x10,  // Serial Port 1
        FNX1_INT05_MPU401 = 0x20,// Midi Controller Interrupt
        FNX1_INT06_LPT = 0x40,   // Parallel Port
        FNX1_INT07_SDCARD = 0x80 // SD Card Controller Interrupt
    }

    enum Register2
    {
        FNX2_INT00_OPL2R = 1,    // OPl2 Right Channel
        FNX2_INT01_OPL2L = 2,    // OPL2 Left Channel
        FNX2_INT02_BTX_INT = 4,  // Beatrix Interrupt (TBD)
        FNX2_INT03_SDMA = 8,     // System DMA
        FNX2_INT04_VDMA = 0x10,  // Video DMA
        FNX2_INT05_DACHP = 0x20, // DAC Hot Plug
        FNX2_INT06_EXT = 0x40,   // External Expansion
        FNX2_INT07_ALLONE = 0x80 // ??
    }

}
