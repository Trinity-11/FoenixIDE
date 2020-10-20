using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIDI_to_VGM_Converter
{
    public class Track
    {
        public int startOffset = -1;
        public int length = 0;
        public int microSecondsPerQuarter = 0; // ie. tempo
        public int BPM = 0;
        public TimeSignature timeSignature;
        public int totalDeltaTime = 0;
    }
}
