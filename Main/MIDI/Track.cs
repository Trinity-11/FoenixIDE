using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoenixIDE.UI
{
    public class Track
    {
        public int Index
        {
            get;
            set;
        }
        public int startOffset = -1;
        public int length = 0;
        public TimeSignature timeSignature;
        public int totalDeltaTime = 0;
        public int TotalTime
        {
            get => totalDeltaTime;
        }

        public string name = "unnamed";
        // ordered list of events
        public List<MidiEvent> events;
        public int EventCount
        {
            get => events.Count;
        }

        // does the track contain polyphonic events on a single MIDI channel?
        public bool isPoly
        {
            get;
            set;
        }
        // does the track contain multiple channel events?
        public bool isMultiChannel
        {
            get;
            set;
        }

        // if the Track is single channel, then this is stored here.
        public int midiChannel = -1;
        public string MidiChannel
        {
            get => midiChannel != -1? (midiChannel + 1).ToString():"";
        }
    }

    public enum MidiEventType
    {
        progchange, noteoff, noteon
    }

    // refer to http://www.shikadi.net/moddingwiki/OP2_Bank_Format
    // and the OPL3 Bank Editor: https://github.com/Wohlstand/OPL3BankEditor/blob/master/README.md
    public class GeneralMidi
    {
        private static readonly byte[] GenMidi = FoenixIDE.Simulator.Properties.Resources.GENMIDI;
        public static byte[] GetInstrument(int program)
        {
            byte[] buffer = new byte[36];
            int offset = 8 + 36 * program;
            Array.Copy(GenMidi, offset, buffer, 0, 36);
            return buffer;
        }
    }

    public class MidiEvent
    {
        public int deltaTime;
        public int index; // index in time
        public int wait; // this is the converted sample count at 44100 Hz.
        public MidiEventType type = MidiEventType.noteon;
        public byte note;
        public byte velocity = 0;
        public byte program = 0;
        // we have 18 single voice (2 ops) channels or 6 double-voice (4 ops) + 6 single-voice (2 ops) channels - so use channels cautiously.
        // I need to write an algorithm to pick the correct channels - don't let MIDI do this
        // For example, channel 10 in MIDI is usually a rythm track (drums)
        public byte midiChannel = 0;  

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("I:").Append(index).Append(" D:").Append(deltaTime).Append(" W:").Append(wait);
            sb.Append(" T:").Append(type).Append(" C:").Append(midiChannel);
            switch(type)
            {
                case MidiEventType.progchange:
                    sb.Append(" P:").Append(program);
                    break;
                default:
                    sb.Append(" N:").Append(note).Append(" V:").Append(velocity);
                    break;
            }

            return sb.ToString();
        }

        public byte[] GetInstrumentBuffer(byte baseAddress, int channel, byte[] gmData, byte[] buffer, int offset)
        {
            // Single voice channel
            // operator 1
            Array.Copy(new byte[3] { baseAddress, (byte)(0x20 + channelToOperatorOffset[channel % 9]), gmData[4] }, 0, buffer, offset, 3);  // tremolo/vibrato/sustain/KSR/Freq Mult
            Array.Copy(new byte[3] { baseAddress, (byte)(0x60 + channelToOperatorOffset[channel % 9]), gmData[5] }, 0, buffer, 3 + offset, 3);  // attack/decay
            Array.Copy(new byte[3] { baseAddress, (byte)(0x80 + channelToOperatorOffset[channel % 9]), gmData[6] }, 0, buffer, 6 + offset, 3);  // sustain/release
            Array.Copy(new byte[3] { baseAddress, (byte)(0xE0 + channelToOperatorOffset[channel % 9]), gmData[7] }, 0, buffer, 9 + offset, 3);  // Waveform select
            Array.Copy(new byte[3] { baseAddress, (byte)(0x40 + channelToOperatorOffset[channel % 9]), (byte)(gmData[8] + gmData[9]) }, 0, buffer, 12 + offset, 3);  // KSL/Output Level
            Array.Copy(new byte[3] { baseAddress, (byte)(0xC0 + channel % 9), (byte)(0xF0 | gmData[10]) }, 0, buffer, 15 + offset, 3);  // Speaker/Feedback/Syn Type
                                                                                                                           // operator 2
            Array.Copy(new byte[3] { baseAddress, (byte)(0x23 + channelToOperatorOffset[channel % 9]), gmData[11] }, 0, buffer, 18 + offset, 3);  // tremolo/vibrato/sustain/KSR/Freq Mult
            Array.Copy(new byte[3] { baseAddress, (byte)(0x63 + channelToOperatorOffset[channel % 9]), gmData[12] }, 0, buffer, 21 + offset, 3);  // attack/decay
            Array.Copy(new byte[3] { baseAddress, (byte)(0x83 + channelToOperatorOffset[channel % 9]), gmData[13] }, 0, buffer, 24 + offset, 3);  // sustain/release
            Array.Copy(new byte[3] { baseAddress, (byte)(0xE3 + channelToOperatorOffset[channel % 9]), gmData[14] }, 0, buffer, 27 + offset, 3);  // Waveform select
            Array.Copy(new byte[3] { baseAddress, (byte)(0x43 + channelToOperatorOffset[channel % 9]), (byte)(gmData[15] + gmData[16]) }, 0, buffer, 30 + offset, 3);  // KSL/Output Level
            return buffer;
        }

        // We're mapping channels to use the drums
        private readonly byte[] channelToOperatorOffset = { 0, 1, 2, 8, 9, 0xA, 0x10, 0x11, 0x12};
        public byte[] GetOPL3Bytes(byte[] channelMap)
        {
            byte[] buffer = null;
            byte oplChnl = channelMap[midiChannel];
            if (oplChnl < 18)
            {
                byte baseReg = (oplChnl < 9) ? (byte)0x5e : (byte)0x5f;
                switch (type)
                {
                    case MidiEventType.progchange:
                        if (midiChannel == 9 && MIDI_VGM_From.PercussionSet != 0)
                        {
                            buffer = new byte[99];
                            // Channel 7 - Base Drum
                            byte[] gmData = GeneralMidi.GetInstrument(128);
                            GetInstrumentBuffer(baseReg, 6, gmData, buffer, 0);
                            // Channel 8 - HH and TT
                            gmData = GeneralMidi.GetInstrument(129);
                            GetInstrumentBuffer(baseReg, 7, gmData, buffer, 33);
                            // Channel 9 - SD and CY
                            gmData = GeneralMidi.GetInstrument(130);
                            GetInstrumentBuffer(baseReg, 8, gmData, buffer, 66);
                        }
                        else
                        {
                            // read the patch file
                            byte[] gmData = GeneralMidi.GetInstrument(program);
                            MIDI_VGM_From.ChannelKSL[oplChnl] = gmData[8];

                            if ((gmData[0] & 4) != 0)
                            {
                                buffer = new byte[66];
                                // double voice instrument
                                // operator 1
                                GetInstrumentBuffer(baseReg, oplChnl, gmData, buffer, 0);

                                // operator 3
                                byte addr2 = 0x28;
                                Array.Copy(new byte[3] { baseReg, (byte)(addr2 + channelToOperatorOffset[oplChnl % 9]), gmData[20] }, 0, buffer, 33, 3);
                                Array.Copy(new byte[3] { baseReg, (byte)(addr2 + 0x40 + channelToOperatorOffset[oplChnl % 9]), gmData[21] }, 0, buffer, 36, 3);
                                Array.Copy(new byte[3] { baseReg, (byte)(addr2 + 0x60 + channelToOperatorOffset[oplChnl % 9]), gmData[22] }, 0, buffer, 39, 3);
                                Array.Copy(new byte[3] { baseReg, (byte)(addr2 + 0xC0 + channelToOperatorOffset[oplChnl % 9]), gmData[23] }, 0, buffer, 42, 3);
                                Array.Copy(new byte[3] { baseReg, (byte)(addr2 + 0x20 + channelToOperatorOffset[oplChnl % 9]), (byte)(gmData[24] + gmData[25]) }, 0, buffer, 45, 3);
                                Array.Copy(new byte[3] { baseReg, (byte)(addr2 + 0xA0 + oplChnl % 9), (byte)(gmData[26] | 0xF0) }, 0, buffer, 48, 3);
                                // operator 4
                                Array.Copy(new byte[3] { baseReg, (byte)(addr2 + 3 + channelToOperatorOffset[oplChnl % 9]), gmData[27] }, 0, buffer, 51, 3);
                                Array.Copy(new byte[3] { baseReg, (byte)(addr2 + 0x40 + 3 + channelToOperatorOffset[oplChnl % 9]), gmData[28] }, 0, buffer, 54, 3);
                                Array.Copy(new byte[3] { baseReg, (byte)(addr2 + 0x60 + 3 + channelToOperatorOffset[oplChnl % 9]), gmData[29] }, 0, buffer, 57, 3);
                                Array.Copy(new byte[3] { baseReg, (byte)(addr2 + 0xC0 + 3 + channelToOperatorOffset[oplChnl % 9]), gmData[30] }, 0, buffer, 60, 3);
                                Array.Copy(new byte[3] { baseReg, (byte)(addr2 + 0x20 + 3 + channelToOperatorOffset[oplChnl % 9]), (byte)(gmData[31] + gmData[32]) }, 0, buffer, 63, 3);
                            }
                            else
                            {
                                buffer = new byte[33];
                                GetInstrumentBuffer(baseReg, oplChnl, gmData, buffer, 0);
                            }
                        }
                        break;
                    case MidiEventType.noteon:

                        if (midiChannel == 9 && MIDI_VGM_From.PercussionSet != 0 )
                        {
                            bool BD = (note == 35) | (note == 36) | (note == 39);
                            bool SN = (note == 38) | (note == 40) | (note == 28);
                            bool TT = (note == 41) | (note == 45) | (note == 60);
                            bool CY = (note == 49) | (note == 55) | (note == 57);
                            bool HH = (note == 46) | (note == 42) | (note == 44);

                            if (BD)
                            {
                                MIDI_VGM_From.PercussionSet = velocity == 0 ? (byte)(MIDI_VGM_From.PercussionSet & ~0x10) : (byte)(MIDI_VGM_From.PercussionSet | 0x10);
                                MIDI_VGM_From.drumValueChanged = true;
                            }
                            if (SN)
                            {
                                MIDI_VGM_From.PercussionSet = velocity == 0 ? (byte)(MIDI_VGM_From.PercussionSet & ~0x8) : (byte)(MIDI_VGM_From.PercussionSet | 0x8);
                                MIDI_VGM_From.drumValueChanged = true;
                            }
                            if (TT)
                            {
                                MIDI_VGM_From.PercussionSet = velocity == 0 ? (byte)(MIDI_VGM_From.PercussionSet & ~0x4) : (byte)(MIDI_VGM_From.PercussionSet | 0x4);
                                MIDI_VGM_From.drumValueChanged = true;
                            }
                            if (CY)
                            {
                                MIDI_VGM_From.PercussionSet = velocity == 0 ? (byte)(MIDI_VGM_From.PercussionSet & ~0x2) : (byte)(MIDI_VGM_From.PercussionSet | 0x2);
                                MIDI_VGM_From.drumValueChanged = true;
                            }
                            if (HH)
                            {
                                MIDI_VGM_From.PercussionSet = velocity == 0 ? (byte)(MIDI_VGM_From.PercussionSet & ~0x1) : (byte)(MIDI_VGM_From.PercussionSet | 0x1);
                                MIDI_VGM_From.drumValueChanged = true;
                            }
                        }
                        else
                        {
                            buffer = new byte[9];
                            byte[] onFreq = GetFreq(note);
                            buffer[0] = baseReg;
                            buffer[1] = (byte)(0xA0 + oplChnl % 9);
                            buffer[2] = onFreq[0];
                            buffer[3] = baseReg;
                            buffer[4] = (byte)(0xB0 + oplChnl % 9);
                            buffer[5] = (byte)(onFreq[1] | (velocity != 0 ? 0x20 : 0));  // set KEY on

                            buffer[6] = baseReg;
                            buffer[7] = (byte)(0x40 + channelToOperatorOffset[oplChnl % 9]);
                            buffer[8] = (byte)(MIDI_VGM_From.ChannelKSL[oplChnl] | (0x3F - (velocity >> 1)));  // attenuation
                        }
                        break;
                    case MidiEventType.noteoff:
                        
                        if (midiChannel == 9 && MIDI_VGM_From.PercussionSet != 0)
                        {
                            bool BD = (note == 35) | (note == 36);
                            bool SN = (note == 38) | (note == 40);
                            bool TT = (note == 41) | (note == 45) | (note == 60);
                            bool CY = (note == 49) | (note == 55) | (note == 57);
                            bool HH = (note == 46) | (note == 42) | (note == 44);
                            
                            if (BD)
                            {
                                MIDI_VGM_From.PercussionSet = (byte)(MIDI_VGM_From.PercussionSet & ~0x10);
                                MIDI_VGM_From.drumValueChanged = true;
                            }
                            if (SN)
                            {
                                MIDI_VGM_From.PercussionSet = (byte)(MIDI_VGM_From.PercussionSet & ~0x8);
                                MIDI_VGM_From.drumValueChanged = true;
                            }
                            if (TT)
                            {
                                MIDI_VGM_From.PercussionSet = (byte)(MIDI_VGM_From.PercussionSet & ~0x4);
                                MIDI_VGM_From.drumValueChanged = true;
                            }
                            if (CY)
                            {
                                MIDI_VGM_From.PercussionSet = (byte)(MIDI_VGM_From.PercussionSet & ~0x2);
                                MIDI_VGM_From.drumValueChanged = true;
                            }
                            if (HH)
                            {
                                MIDI_VGM_From.PercussionSet = (byte)(MIDI_VGM_From.PercussionSet & ~0x1);
                                MIDI_VGM_From.drumValueChanged = true;
                            }
                        }
                        else
                        {
                            buffer = new byte[3];
                            buffer[0] = baseReg;
                            buffer[1] = (byte)(0xB0 + oplChnl % 9);
                            buffer[2] = 0;
                        }
                        break;
                }
            }
            else
            {
                // program the drum channels

            }
            return buffer;
        }

        // note 60 is middle C on an 88-note keyboard
        private readonly ushort[] noteFNumbers = { 0x016B, 0x0181, 0x0198, 0x01B0, 0x01CA, 0x01E5, 0x0202, 0x0220, 0x0241, 0x0263, 0x0287, 0x02AE };  // c# to c
        private byte[] GetFreq(byte note)
        {
            byte[] buffer = new byte[2];
            byte octave = (byte)((((note - 1) / 12 - 1) << 2) & 0x1C);
            int offset = (note -1) % 12;
            ushort val = noteFNumbers[offset];
            buffer[0] = (byte)(val & 0xFF);
            buffer[1] = (byte)((val >> 8) | octave);
            return buffer;
        }
    }
}
