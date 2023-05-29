using KGySoft.CoreLibraries;
using System;

namespace FoenixIDE.Simulator.Devices
{
    /**
     * The RTC keeps track of time. 
     * The HiResTimer is set to 1 second to update the date-time register
     * Another HiResTimer is necessary to keep track of the "periodic alarms"
     * 
     * The date alarms are raise by check every second.
     * 
     * Reading the registers is allowed.
     * Writing to the registers is only allowed when UpdateTransferInhibit is set.
     * 
     * Power-failure interrupt is not simulated.
     * 
     */
    public class RTC : MemoryLocations.MemoryRAM
    {
        private HiResTimer secondTimer = null;
        private HiResTimer periodicTimer = null;

        public delegate void RaiseInterruptFunction();
        public RaiseInterruptFunction AlarmPeriodicInterruptDelegate;

        byte[] tempValues = new byte[16];
        bool daylightSavings = false;
        bool Use24Hour = false;
        bool is_UTI_Set = false;
        float freq = 500_000; // half a second in ms
        bool isAlarmEnabled = false;
        bool isPeriodicEnabled = false;

        public RTC(int StartAddress, int Length) : base(StartAddress, Length)
        {
            secondTimer = new HiResTimer(1000);
            secondTimer.Elapsed += Second_Timer_Tick;


            periodicTimer = new HiResTimer(freq);
            periodicTimer.Elapsed += Periodic_Alarm_Tick;
            periodicTimer.Enabled = false;

            // By default, set the current date and time to now - users can overwrite this
            DateTime currentTime = new DateTime();
            // write the time to memory - values are BCD
            data[0] = BCD(currentTime.Second);
            data[2] = BCD(currentTime.Minute);
            data[4] = BCD(currentTime.Hour);  // write the hours in 24 hours - the read will need to convert for AM/PM
            data[6] = BCD(currentTime.Day);
            data[8] = (byte)(currentTime.DayOfWeek + 1);
            data[9] = BCD(currentTime.Month);
            data[10] = BCD(currentTime.Year % 100);
            data[15] = BCD(currentTime.Year / 100);
            secondTimer.Start();
        }

        private byte BCD(int val)
        {
            return (byte)(val / 10 * 0x10 + val % 10);
        }

        void Second_Timer_Tick(object sender, EventArgs e)
        {
            // increment seconds
            byte second = data[0];
            if (second < 59)
            {
                second++;
                data[0] = second;
            }
            else
            {
                data[0] = 0;
                Increment_Minutes();
            }
            if (isAlarmEnabled)
            {
                // Need to check the masks too.  Not sure how this work yet.
                bool maskSecond = (data[1] & 0xC0) != 0;
                bool maskMinute = (data[3] & 0xC0) != 0;
                bool maskHour = (data[5] & 0xC0) != 0;
                bool maskDay = (data[7] & 0xC0) != 0;
                // Refer to bq4802ly PDF Table 6.
                if (
                    ( maskSecond &&  maskMinute &&  maskHour &&  maskDay) ||
                    (!maskSecond &&  maskMinute &&  maskHour &&  maskDay && data[0] == data[1]) ||
                    (!maskSecond && !maskMinute &&  maskHour &&  maskDay && data[0] == data[1] && data[2] == data[3]) ||
                    (!maskSecond && !maskMinute && !maskHour &&  maskDay && data[0] == data[1] && data[2] == data[3] && data[4] == data[5]) ||
                    (!maskSecond && !maskMinute && !maskHour && !maskDay && data[0] == data[1] && data[2] == data[3] && data[4] == data[5] && data[6] == data[7])
                   )
                {
                    // Add the Alarm Flag
                    data[13] = data[13] |= 8;
                    AlarmPeriodicInterruptDelegate?.Invoke();
                    return;
                }
            }
        }

        void Increment_Minutes()
        {
            byte minutes = data[2];
            if (minutes < 59)
            {
                minutes++;
                data[2] = minutes;
            }
            else
            {
                data[2] = 0;
                Increment_Hours();
            }
        }

        void Increment_Hours()
        {
            byte hours = data[4];
            if (hours < 24)
            {
                hours++;
                data[4] = hours;
            }
            else
            {
                data[4] = 0;
                Increment_Days();
            }            
        }

        /**
         * This function is not accurate - it expects every month to have 31 days
         */
        void Increment_Days()
        {
            byte days = data[6];
            if (days < 31)
            {
                days++;
                data[6] = days;
            }
            else
            {
                data[6] = 0;
                Increment_Months();
            }
        }

        void Increment_Months()
        {
            byte months = data[9];
            if (months < 12)
            {
                months++;
                data[9] = months;
            }
            else
            {
                data[9] = 0;
                Increment_Years();
            }
        }

        void Increment_Years()
        {
            // NOT IMPLEMENTED
        }

        void Periodic_Alarm_Tick(object sender, EventArgs e)
        {
            // Add the Periodic Flag
            data[13] = data[13] |= 4;
            AlarmPeriodicInterruptDelegate?.Invoke();
        }

        public void KillTimer()
        {
            secondTimer.Kill();
            secondTimer.Elapsed -= Second_Timer_Tick;
            periodicTimer.Kill();
            periodicTimer.Elapsed -= Periodic_Alarm_Tick;
        }

        /**
         * 
         * Reading the flag register clears the alarm.
         * 
         * TODO: Translate the AM/PM - 24 hours base on the value of the 
         */
        public override byte ReadByte(int Address)
        {
            if (Address == 13)
            {
                byte value = data[13];
                data[13] = 0;
                return value;
            }
            return base.ReadByte(Address);
        }
        public override void WriteByte(int Address, byte Value)
        {
            // Allow writing to the control register
            switch (Address)
            {
                case 0xB:
                    // reset the value of the hi-res periodic timer - the watch dog is ignored
                    freq = 0.0305175f * (1 << (Value & 0xF)); // ms
                    if (freq < 200)
                    {
                        freq = 200;
                    }
                    data[0xB] = (byte)(Value & 0x7F);
                    break;
                case 0xC:
                    // Enables Register
                    data[0xC] = (byte)(Value & 0xF);
                    isAlarmEnabled = (Value & 8) != 0;
                    isPeriodicEnabled = (Value & 4) != 0;
                    if (periodicTimer != null && !isPeriodicEnabled)
                    {
                        periodicTimer.Enabled = false;
                    }
                    if (isPeriodicEnabled)
                    {
                        periodicTimer.Start();
                    }
                    break;
                case 0xD:
                    // Flags register
                    data[0xD] = (byte)(Value & 0xF);
                    break;
                case 0xE:
                    // Control Register
                    daylightSavings = (Value & 1) != 0;
                    Use24Hour = (Value & 2) != 0;
                    bool new_UTI = (Value & 8) != 0;
                    if (is_UTI_Set && !new_UTI)
                    {
                        // transfer the values from the temporary register to external registers
                        data[0] = tempValues[0];   // seconds
                        data[2] = tempValues[2];   // minutes
                        data[4] = tempValues[4];   // hours
                        data[6] = tempValues[6];   // days
                        data[9] = tempValues[9];   // months
                        data[10] = tempValues[10];  // years
                        data[15] = tempValues[15];  // centuries
                    }
                    data[0xE] = (byte)(Value & 0xF);
                    break;
            
                
            }
            if (is_UTI_Set && (Address < 0xB || Address == 0xF))
            {
                tempValues[Address] = Value;
            }
        }
    }
}
