using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nu64.Processor
{
    class CPUTest : ReadyHandler
    {
        Kernel kernel = null;
        Processor.CPU CPU = null;

        byte[] TestProg = {
             0x18, 0xFB, 0xC2, 0x30, 0xA9, 0x00, 0x00, 0xA9,
             0x80, 0x00, 0xA9, 0x00, 0x80, 0xA2, 0x00, 0x00,
             0xA2, 0x00, 0x80, 0xA0, 0x34, 0x12, 0xE2, 0x30,
             0xA2, 0x00, 0xA2, 0x80, 0x38, 0xFB, 0x00, 0x00 };

        public CPUTest(Kernel newKernel)
        {
            this.kernel = newKernel;
            this.CPU = kernel.CPU;
        }

        public void BeginTest()
        {
            int pc = 0xc000;
            for (int i = 0; i < TestProg.Length; i++)
            {
                kernel.MemoryMap[pc] = TestProg[i];
                pc++;
            }
            kernel.MemoryMap.WriteWord(MemoryMap_DirectPage.VECTOR_ERESET, 0xc000);
            kernel.MemoryMap.WriteWord(MemoryMap_DirectPage.VECTOR_BRK, 0xc000);

            DateTime t = DateTime.Now.AddSeconds(1);
            while (DateTime.Now < t)
            {
                CPU.Stack.Value = MemoryMap_DirectPage.END_OF_STACK;
                CPU.ExecuteNext();
            }

            kernel.PrintLine("Cycles executed: " + CPU.CycleCounter.ToString());
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

