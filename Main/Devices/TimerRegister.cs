using KGySoft.CoreLibraries;
using System;

namespace FoenixIDE.Simulator.Devices
{
    public class TimerRegister : MemoryLocations.MemoryRAM
    {
        private HiResTimer hiresTimer = null;

        public delegate void RaiseInterruptFunction();
        public RaiseInterruptFunction TimerInterruptDelegate;
        const int CPU_FREQ = 14318000;
        private bool interruptEnabled = false;

        public TimerRegister(int StartAddress, int Length) : base(StartAddress, Length)
        {
            hiresTimer = new HiResTimer(1000);
            hiresTimer.Elapsed += Timer_Tick;
        }

        public override void WriteByte(int Address, byte Value)
        {
            // Address 0 is control register
            data[Address] = Value;
            Console.WriteLine("TimerX Write {0:X2} {1:X2}", Address, Value);
            if (Address == 0)
            {
                bool enabled = (Value & 1) != 0;
                interruptEnabled = (Value & 0x80) != 0;
                if (enabled)
                {
                    hiresTimer.Enabled = true;
                }
                else
                {
                    hiresTimer.Enabled = false;
                }
            }
            else if (Address > 4 && Address < 8)
            {
                // Calculate interval in milliseconds
                int longInterval = data[5] + (data[6] << 8) + (data[7] << 16);
                double msInterval = (double)(longInterval) * 1000/ (double)CPU_FREQ;
                uint adjInterval = (uint)msInterval;
                if (adjInterval==0)
                {
                    hiresTimer.Interval = 1;
                }
                else
                {
                    hiresTimer.Interval = adjInterval;
                }
            }
        }

        void Timer_Tick(object sender, EventArgs e)
        {
            if (interruptEnabled)
            {
                TimerInterruptDelegate?.Invoke();
            }
        }

        public void KillTimer()
        {
            hiresTimer.Kill();
        }
    }
}
