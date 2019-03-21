using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FoenixIDE.MemoryLocations;

namespace FoenixIDE.Processor
{
    class CPUTest : ReadyHandler
    {
        FoenixSystem kernel = null;
        Processor.CPU CPU = null;

        byte[] TestProg = {
            // Switch to native mode. This should be done by the kernel,
            // but we don't have a boot routine yet.
            0x18,             // CLC          Clear carry in preparation to...
            0xFB,             // XCE          Switch to native mode
            0xE2, 0x30,       // REP 30
            0xC2, 0x30,       // REP 30
            
            // test LDA immediate, zero and Negative flags
            0xA9, 0x00, 0X00, // LDA #$0000   Zero should be set
            0xA9, 0x00, 0x80, // LDA #$8000   Negative should be set 
            
            // test LDX and LDY with 16-bit values
            0xA2, 0x00, 0x00, // LDX #$0000   Zero should be set
            0xA2, 0x00, 0x80, // LDX #$8000   Negative should be set
            0xA0, 0x34, 0x12, // LDY #$1234   

            // Switch to 8-bit mode and test load instructions
            0xE2, 0x30,       // SEP #$30     Set 8-bit A and Index registers
            0xA9, 0x00,       // LDA #$00     
            0xA9, 0xFF,       // LDA #$FF
            0xA9, (byte)'?',  // LDA #$4040   Load "?@" into A
            0x8D, 0x30, 0x12, // STA $1190 stores @@ at row 5 on the screen
            0xA2, 0x00,       // LDX #$00                               
            0xA2, 0xFF,       // LDX #$FF                             
            0xA0, 0x00,       // LDY #$00     
            0xA0, 0xFF,       // LDY #$FF     
            // Return to the OS
            0xDB,             // STP          Stops the CPU
            };

        public CPUTest(FoenixSystem newKernel)
        {
            this.kernel = newKernel;
            this.CPU = kernel.CPU;
        }

        public void BeginTest(int Address)
        {
            kernel.Memory.WriteWord(MemoryMap.VECTOR_RESET, 0xc000);
            kernel.Memory.WriteWord(MemoryMap.VECTOR_BRK, 0xc000);
            kernel.CPU.Stack.Value = MemoryMap.STACK_END;
            kernel.CPU.SetLongPC(Address);

            // Wind up the CPU and get it ready. The user will advance the PC
            // using the debug window. 
            CPU.DebugPause = true;

            //kernel.OutputDevice = DeviceEnum.DebugWindow;
            /*
            kernel.PrintTab(REGISTER_COLUMN);
            kernel.Monitor.PrintRegisterHeader();
            kernel.PrintTab(REGISTER_COLUMN);
            kernel.Monitor
            while (!CPU.Halted)
            {

                int p1 = CPU.GetLongPC();
                CPU.ExecuteNext();
                int pc2 = p1 + CPU.OC.Length;

                for (int i = p1; i < pc2; i++)
                {
                    kernel.PrintMemHex(1, i);
                    kernel.Print(" ");
                }
                kernel.PrintTab(MNEMONIC_COLUMN);
                kernel.Print(CPU.OC.ToString(CPU.SignatureBytes));
                kernel.PrintTab(REGISTER_COLUMN);
                kernel.Monitor.PrintRegisters(false);
                kernel.gpu.Refresh();
                System.Windows.Forms.Application.DoEvents();
            }

            kernel.OutputDevice = DeviceEnum.Screen;
            kernel.ReadyHandler = kernel.Monitor;
            kernel.READY();
            */
        }

        internal void BeginTestFast(int Address)
        {
            //kernel.Memory.WriteWord(MemoryMap_DirectPage.VECTOR_RESET, 0xc000);
            //kernel.Memory.WriteWord(MemoryMap_DirectPage.VECTOR_BRK, 0xc000);
            //kernel.CPU.Stack.Value = MemoryMap_DirectPage.STACK_END;
            //kernel.CPU.SetPC(Address);

            //CPU.JumpTo(Address, 0);
            //while (!CPU.Halted)
            //    CPU.ExecuteNext();
        }

        public void BeginTest()
        {
            sbyte test1 = -1;
            byte test2 = (byte)test1;

            int pc = 0xc000;
            for (int i = 0; i < TestProg.Length; i++)
            {
                kernel.Memory[pc] = TestProg[i];
                pc++;
            }

            BeginTest(pc);
        }

        public void Ready()
        {
            BeginTest();
        }

        public void ReturnPressed(int LineStart)
        {
            kernel.PrintLine();
        }

        public void PrintGreeting()
        {
            kernel.PrintLine("Simulator Performance Test. Executing for one second.");
        }

    }
}

