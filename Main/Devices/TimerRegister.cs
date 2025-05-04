using KGySoft.CoreLibraries;
using System;

namespace FoenixIDE.Simulator.Devices
{
    public class TimerRegister : MemoryLocations.MemoryRAM
    {
        private HiResTimer hiresTimer = null;

        public delegate void RaiseInterruptFunction();
        public RaiseInterruptFunction TimerInterruptDelegate;
        private int clock = 60; // frequency in Hz
        private int period = 1000;
        private bool interruptEnabled = false;
        private int timer_val = 0;
        private bool timer_eq = false;
        private int timer_inc = 1; // this value is computer based on tick and clock

        public TimerRegister(int StartAddress, int Length, int clock_freq) : base(StartAddress, Length)
        {
            hiresTimer = new HiResTimer(period);
            hiresTimer.Elapsed += Timer_Tick;
            clock = clock_freq;
            changeClock(clock);
        }

        public override byte ReadByte(int Address)
        {
            switch(Address)
            {
                case 0:
                    // return if the counter is EQ to CMP value
                    return timer_eq ? (byte)1: (byte)0; 
                case 1:
                    return (byte)(timer_val & 0xFF);
                case 2:
                    return (byte)((timer_val >> 8) & 0xFF);
                case 3:
                    return (byte)((timer_val >> 16) & 0xFF);
                default:
                    return base.ReadByte(Address);
            }
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
                // Clear
                if ((Value & 2) != 0)
                {
                    timer_val = 0;
                }
                // Reload
                if ((Value & 4) != 0)
                {
                    timer_val = data[1] + (data[2] << 8) + (data[3] << 16);
                }
            }
        }

        public void KillTimer()
        {
            hiresTimer.Kill();
        }

        public void changeClock(int freq)
        {
            clock = freq;
            period = 1000 / clock;
            timer_inc = 1;
            if (period < 1)
            {
                period = 1;
                // if the period is adjusted to 1ms, then the increment must be adjusted also
                timer_inc = clock / 1000;
            }
            hiresTimer.Interval = period;
        }

        void Timer_Tick(object sender, EventArgs e)
        {
            // timer_eq was set in the previous cycle, raise an interrupt
            if (timer_eq)
            {
                if (interruptEnabled)
                {
                    TimerInterruptDelegate?.Invoke();
                }
                // clear
                if ((data[4] & 1) != 0)
                {
                    timer_val = 0;
                // reload
                }
                else if ((data[4] & 2) != 0)
                {
                    timer_val = data[1] + (data[2] << 8) + (data[3] << 16);
                }
            }

            if ((data[0] & 8) != 0)
            {
                timer_val+= timer_inc; // account for difference in clock and tick value
            }
            else
            {
                timer_val-= timer_inc; // account for difference in clock and tick value
            }
            
            int timer_cmp = data[5] + (data[6] << 8) + (data[7] << 16);
            timer_eq = timer_val >= timer_cmp;  // we need to do this especially for fast clocks.
        }

        //int
        //        int longInterval = data[5] + (data[6] << 8) + (data[7] << 16);
        //double msInterval = (double)(longInterval) * 1000 / (double)CPU_FREQ;
        //uint adjInterval = (uint)msInterval;
        //        if (adjInterval==0)
        //        {
        //            hiresTimer.Interval = 1;
        //        }
        //        else
        //        {
        //            hiresTimer.Interval = adjInterval;
        //        }
    }
}
