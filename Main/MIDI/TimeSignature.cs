using System;

namespace FoenixIDE.UI
{
    public class TimeSignature
    {
        public byte numerator;
        public byte denominator; // negative power of two
        public byte clocksPerMetronomeClick;
        public byte notated32ndNotes;

        public override String ToString()
        {
            return (string)(numerator + "/" + (Math.Pow(2, denominator)));
        }
    }
}
