using System;
using System.Collections.Generic;

using System.Text;
using System.Threading.Tasks;

namespace FoenixIDE.MemoryLocations
{
    public static partial class MemoryMap
    {
        #region Direct page
        // c# Direct page Addresses

        // * Addresses are the byte AFTER the block. Use this to confirm block locations and check for overlaps
        public const int BANK0_BEGIN = 0x000000; // Start of bank 0 and Direct page
        public const int RESET = 0x000000; // 4 Bytes Jumps to the beginning of kernel ROM. ($F8:0000). 
        public const int RETURN = 0x000004; // 4 Bytes Called when the RETURN key is pressed in the immediate mode screen. This will process a command in MONITOR, execute a BASIC command, or add a BASIC program line.
        public const int KEYDOWN = 0x000008; // 4 Bytes Custom keyboard handler. This defaults to the kernel keypress handler, but you can redirect this to your own routines. Make sure to JML to the original address at the end of your custom routine. Use this to make F-Key macros or custom keyboard commands. 
        public const int SCREENBEGIN = 0x00000C; // 3 Bytes Start of screen in video RAM. This is the upper-left corrner of the current video page being written to. This may not be what's being displayed by VICKY. Update this if you change VICKY's display page. 
        public const int COLS_VISIBLE = 0x00000F; // 2 Bytes Columns visible per screen line. A virtual line can be longer than displayed, up to COLS_PER_LINE long. Default = 80
        public const int COLS_PER_LINE = 0x000011; // 2 Bytes Columns in memory per screen line. A virtual line can be this long. Default=128
        public const int LINES_VISIBLE = 0x000013; // 2 Bytes The number of rows visible on the screen. Default=25
        public const int LINES_MAX = 0x000015; // 2 Bytes The number of rows in memory for the screen. Default=64
        public const int CURSORPOS = 0x000017; // 3 Bytes The next character written to the screen will be written in this location. 
        public const int CURSORX = 0x00001A; // 2 Bytes This is where the blinking cursor sits. Do not edit this direectly. Call LOCATE to update the location and handle moving the cursor correctly. 
        public const int CURSORY = 0x00001C; // 2 Bytes This is where the blinking cursor sits. Do not edit this direectly. Call LOCATE to update the location and handle moving the cursor correctly. 
        public const int CURCOLOR = 0x00001E; // 2 Bytes Color of next character to be printed to the screen. 
        public const int CURATTR = 0x000020; // 2 Bytes Attribute of next character to be printed to the screen.
        public const int STACKBOT = 0x000022; // 2 Bytes Lowest location the stack should be allowed to write to. If SP falls below this value, the runtime should generate STACK OVERFLOW error and abort.
        public const int STACKTOP = 0x000024; // 2 Bytes Highest location the stack can occupy. If SP goes above this value, the runtime should generate STACK OVERFLOW error and abort. 
        public const int KERNEL_TEMP = 0x0000C0; // 32 Bytes Temp space for kernel
        public const int USER_TEMP = 0x0000E0; // 32 Bytes Temp space for user programs
        public const int PAGE0_END_ = 0x000100; //  Byte 

        public const int CURSORX_JR = 0x00D014; // 2 Bytes This is where the blinking cursor sits. Do not edit this direectly. Call LOCATE to update the location and handle moving the cursor correctly. 
        public const int CURSORY_JR = 0x00D016; // 2 Bytes This is where the blinking cursor sits. Do not edit this direectly. Call LOCATE to update the location and handle moving the cursor correctly. 


        #region GAVIN
        public const int GAVIN_BLOCK = 0x000100; // 256 Bytes Gavin reserved, overlaps debugging registers at $1F0

        // Math co-processor
        public const int MATH_START =    0x00_0100;
        public const int MATH_START_JR = 0x00_DE00;

        /** Just for reference
        public const int MULTIPLIER_0 =  0x000100; // 0 Byte Unsigned multiplier
        public const int M0_OPERAND_A =  0x000100; // 2 Bytes Operand A (ie: A x B)
        public const int M0_OPERAND_B =  0x000102; // 2 Bytes Operand B (ie: A x B)
        public const int M0_RESULT =     0x000104; // 4 Bytes Result of A x B
        public const int MULTIPLIER_1 =  0x000108; // 0 Byte Signed Multiplier
        public const int M1_OPERAND_A =  0x000108; // 2 Bytes Operand A (ie: A x B)
        public const int M1_OPERAND_B =  0x00010A; // 2 Bytes Operand B (ie: A x B)
        public const int M1_RESULT =     0x00010C; // 4 Bytes Result of A x B
        public const int DIVIDER_0 =     0x000108; // 0 Byte Unsigned divider
        public const int D0_OPERAND_A =  0x000108; // 2 Bytes Divider 0 Dividend ex: A in  A/B 
        public const int D0_OPERAND_B =  0x00010A; // 2 Bytes Divider 0 Divisor ex B in A/B
        public const int D0_RESULT =     0x00010C; // 2 Bytes Quotient result of A/B ex: 7/2 = 3 r 1
        public const int D0_REMAINDER =  0x00010E; // 2 Bytes Remainder of A/B ex: 1 in 7/2=3 r 1
        public const int DIVIDER_1 =     0x000110; // 0 Byte Signed divider
        public const int D1_OPERAND_A =  0x000110; // 2 Bytes Divider 1 Dividend ex: A in  A/B 
        public const int D1_OPERAND_B =  0x000112; // 2 Bytes Divider 1 Divisor ex B in A/B
        public const int D1_RESULT =     0x000114; // 2 Bytes Signed quotient result of A/B ex: 7/2 = 3 r 1
        public const int D1_REMAINDER =  0x000116; // 2 Bytes Signed remainder of A/B ex: 1 in 7/2=3 r 1
        */
        public const int MATH_END =      0x00_012F;
        public const int MATH_END_JR =   0x00_DE1F;

        // Pending Interrupt (Read and Write Back to Clear)
        public const int INT_PENDING_REG0 = 0x00_0140;
        public const int INT_PENDING_REG1 = 0x00_0141;
        public const int INT_PENDING_REG2 = 0x00_0142;
        public const int INT_PENDING_REG3 = 0x00_0143; // FMX Model
        public const int INT_PENDING_REG0_JR = 0x00_D660;

        // Polarity Set
        public const int INT_POL_REG0 = 0x00_0144 ;
        public const int INT_POL_REG1 = 0x00_0145 ;
        public const int INT_POL_REG2 = 0x00_0146 ;
        public const int INT_POL_REG7 = 0x00_0147 ; // FMX Model

        // Edge Detection Enable
        public const int INT_EDGE_REG0 = 0x00_0148;
        public const int INT_EDGE_REG1 = 0x00_0149;
        public const int INT_EDGE_REG2 = 0x00_014A;
        public const int INT_EDGE_REG3 = 0x00_014B; // FMX Model
        // Mask
        public const int INT_MASK_REG0 = 0x00_014C;
        public const int INT_MASK_REG1 = 0x00_014D;
        public const int INT_MASK_REG2 = 0x00_014E;
        public const int INT_MASK_REG3 = 0x00_014F; // FMX Model
        public const int INT_MASK_REG0_JR = 0x00_D666;

        public const int TIMER0_CTRL_REG = 0x00_0160;
        public const int TIMER0_CTRL_REG_JR = 0xD650;
        public const int TIMER0_CHARGE   = 0x00_0161;
        public const int TIMER0_CMP_REG  = 0x00_0164;
        public const int TIMER0_CMP      = 0x00_0165;

        public const int TIMER1_CTRL_REG = 0x00_0168;
        public const int TIMER1_CTRL_REG_JR = 0xD658;
        public const int TIMER1_CHARGE   = 0x00_0169;
        public const int TIMER1_CMP_REG  = 0x00_016C;
        public const int TIMER1_CMP      = 0x00_016D;

        public const int TIMER2_CTRL_REG = 0x00_0170;
        public const int TIMER2_CHARGE   = 0x00_0171;
        public const int TIMER2_CMP_REG  = 0x00_0174;
        public const int TIMER2_CMP      = 0x00_0175;


        public const int VECTOR_STATE = 0x0001FF; // 1 Byte Interrupt Vector State. See VECTOR_STATE_ENUM

        #endregion GAVIN

        public const int CPU_REGISTERS = 0x000200; //  Byte 
        public const int CPUPC = 0x000200; // 2 Bytes Debug registers. When BRK is executed, Interrupt service routine will populate this block with the CPU registers. 
        public const int CPUPBR = 0x000202; // 1 Byte Program Bank Register (K)
        public const int CPUDBR = 0x000203; // 1 Byte Data Bank Register (B)
        public const int CPUFLAGS = 0x000204; // 1 Byte Flags (P) (The second byte is ignored)
        public const int CPUA = 0x000205; // 2 Bytes Accumulator (A)
        public const int CPUX = 0x000207; // 2 Bytes X Register
        public const int CPUY = 0x000209; // 2 Bytes Y Index Register
        public const int CPUDP = 0x00020B; // 2 Bytes Direct Page Register (D)
        public const int CPUSTACK = 0x00020D; // 2 Bytes Stack Pointer

        public const int MONITOR_VARS = 0x000210; //  Byte MONITOR Variables. BASIC variables may overlap this space
        public const int MCMDADDR = 0x000210; // 3 Bytes Address of the current line of text being processed by the command parser. Can be in display memory or a variable in memory. MONITOR will parse up to MTEXTLEN characters or to a null character.
        public const int MCMP_TEXT = 0x000213; // 3 Bytes Address of symbol being evaluated for COMPARE routine
        public const int MCMP_LEN = 0x000216; // 2 Bytes Length of symbol being evaluated for COMPARE routine
        public const int MCMD = 0x000218; // 3 Bytes Address of the current command/function string
        public const int MCMD_LEN = 0x00021B; // 2 Bytes Length of the current command/function string
        public const int MARG1 = 0x00021D; // 4 Bytes First command argument. May be data or address, depending on command
        public const int MARG2 = 0x000221; // 4 Bytes First command argument. May be data or address, depending on command. Data is 32-bit number. Address is 24-bit address and 8-bit length.
        public const int MARG3 = 0x000225; // 4 Bytes First command argument. May be data or address, depending on command. Data is 32-bit number. Address is 24-bit address and 8-bit length.
        public const int MARG4 = 0x000229; // 4 Bytes First command argument. May be data or address, depending on command. Data is 32-bit number. Address is 24-bit address and 8-bit length.
        public const int MARG5 = 0x00022D; // 4 Bytes First command argument. May be data or address, depending on command. Data is 32-bit number. Address is 24-bit address and 8-bit length.
        public const int MARG6 = 0x000231; // 4 Bytes First command argument. May be data or address, depending on command. Data is 32-bit number. Address is 24-bit address and 8-bit length.
        public const int MARG7 = 0x000235; // 4 Bytes First command argument. May be data or address, depending on command. Data is 32-bit number. Address is 24-bit address and 8-bit length.
        public const int MARG8 = 0x000239; // 4 Bytes First command argument. May be data or address, depending on command. Data is 32-bit number. Address is 24-bit address and 8-bit length.

        public const int LOADFILE_VARS = 0x000300; //  Byte 
        public const int LOADFILE_NAME = 0x000300; // 3 Bytes (addr) Name of file to load. Address in Data Page
        public const int LOADFILE_LEN = 0x000303; // 1 Byte Length of filename. 0=Null Terminated
        public const int LOADPBR = 0x000304; // 1 Byte First Program Bank of loaded file ($05 segment)
        public const int LOADPC = 0x000305; // 2 Bytes Start address of loaded file ($05 segment)
        public const int LOADDBR = 0x000307; // 1 Byte First data bank of loaded file ($06 segment)
        public const int LOADADDR = 0x000308; // 2 Bytes FIrst data address of loaded file ($06 segment)
        public const int LOADFILE_TYPE = 0x00030A; // 3 Bytes (addr) File type string in loaded data file. Actual string data will be in Bank 1. Valid values are BIN, PRG, P16
        public const int BLOCK_LEN = 0x00030D; // 2 Bytes Length of block being loaded
        public const int BLOCK_ADDR = 0x00030F; // 2 Bytes (temp) Address of block being loaded
        public const int BLOCK_BANK = 0x000311; // 1 Byte (temp) Bank of block being loaded
        public const int BLOCK_COUNT = 0x000312; // 2 Bytes (temp) Counter of bytes read as file is loaded

        // TODO: verify that these are still good.  
        public const int KEY_BUFFER = 0x00F00; // 64 Bytes keyboard buffer
        public const int KEY_BUFFER_SIZE = 0x40; // 64 Bytes (constant) keyboard buffer length
        public const int KEY_BUFFER_END = 0x000F3F; // 1 Byte keyboard buffer end address
        public const int KEY_BUFFER_RPOS = 0x000F40; // 2 Bytes keyboard buffer read position
        public const int KEY_BUFFER_WPOS = 0x000F42; // 2 Bytes keyboard buffer write position
        //// public const int KEYBOARD_SC_FLG = 0X000F43; // 1 Bytes that indicate the Status of Left Shift, Left CTRL, Left ALT, Right Shift
        //// public const int KEYBOARD_SC_TMP = 0X000F44; // 1 Byte, Interrupt Save Scan Code while Processing

        public const int TEST_BEGIN = 0x001000; // 28672 Bytes Test/diagnostic code for prototype.
        public const int TEST_END = 0x007FFF; // 0 Byte 

        public const int STACK_BEGIN = 0x008000; // 32512 Bytes The default beginning of stack space
        public const int STACK_END = 0x00FEFF; // 0 Byte End of stack space. Everything below this is I/O space

        public const int ISR_BEGIN = 0x00FF00; //  Byte Beginning of CPU vectors in Direct page
        public const int HRESET = 0x00FF00; // 16 Bytes Handle RESET asserted. Reboot computer and re-initialize the kernel.
        public const int HCOP = 0x00FF10; // 16 Bytes Handle the COP instruction. Program use; not used by OS
        public const int HBRK = 0x00FF20; // 16 Bytes Handle the BRK instruction. Returns to BASIC Ready prompt.
        public const int HABORT = 0x00FF30; // 16 Bytes Handle ABORT asserted. Return to Ready prompt with an error message.
        public const int HNMI = 0x00FF40; // 32 Bytes Handle NMI
        public const int HIRQ = 0x00FF60; // 32 Bytes Handle IRQ
        public const int ISR_END = 0x00FF80; // End of direct page Interrrupt handlers

        public const int VECTORS_BEGIN = 0x00FFE0; // 0 Byte Jumps to ROM READY routine. Modified whenever alternate command interpreter is loaded. 
        public const int JMP_READY = 0x00FFE0; // 4 Bytes Jumps to ROM READY routine. Modified whenever alternate command interpreter is loaded. 
        public const int VECTOR_COP = 0x00FFE4; // 2 Bytes Native COP Interrupt vector
        public const int VECTOR_BRK = 0x00FFE6; // 2 Bytes Native BRK Interrupt vector
        public const int VECTOR_ABORT = 0x00FFE8; // 2 Bytes Native ABORT Interrupt vector
        public const int VECTOR_NMI = 0x00FFEA; // 2 Bytes Native NMI Interrupt vector
        public const int VECTOR_RESET = 0x00FFEC; // 2 Bytes Unused (Native RESET vector)
        public const int VECTOR_IRQ = 0x00FFEE; // 2 Bytes Native IRQ Vector


        public const int VECTOR_ECOP = 0x00FFF4; // 2 Bytes Emulation mode interrupt handler
        public const int VECTOR_EBRK = 0x00FFF6; // 2 Bytes Emulation mode interrupt handler
        public const int VECTOR_EABORT = 0x00FFF8; // 2 Bytes Emulation mode interrupt handler
        public const int VECTOR_ENMI = 0x00FFFA; // 2 Bytes Emulation mode interrupt handler
        public const int VECTOR_ERESET = 0x00FFFC; // 2 Bytes Emulation mode interrupt handler
        public const int VECTOR_EIRQ = 0x00FFFE; // 2 Bytes Emulation mode interrupt handler
        public const int VECTORS_END = 0x010000; // *End of vector space
        public const int BANK0_END = 0x00FFFF; // End of Bank 00 and Direct page
        #endregion

    }
}
