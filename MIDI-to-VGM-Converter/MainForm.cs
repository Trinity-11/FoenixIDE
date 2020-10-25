using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MIDI_to_VGM_Converter
{
    public enum FileType
    {
        SINGLE_MULTI_CHNL_TRACK, 
        SIMULTANEOUS_TRACKS,
        INDEPENDENT_SEQ_TRACKS
    }
    public partial class MainForm : Form
    {
        private byte[] buffer;
        private FileType type = FileType.SINGLE_MULTI_CHNL_TRACK;
        short trackCount = 0;
        private Track[] tracks;
        private short ticksPerQuarter = 0;
        private byte smpteFormat;
        private byte ticksPerFrame = 0;
        private byte RunningStatus = 0;
        private int samplesPerTick = 0;
        private const int samplesPerSecond = 44100;

        // ordered list of events - all mixed together
        public List<MidiEvent> events = null;

        private int microSecondsPerQuarter = 0; // ie. tempo
        private int BPM = 0;

        private StringBuilder sb = null;

        public MainForm()
        {
            InitializeComponent();
        }

        private void ReadFileButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                Filter = "MIDI Files (*.mid *.midi)| *.mid;*.midi"
            };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                RunningStatus = 0;
                events = new List<MidiEvent>();
                FileLabel.Text = dialog.FileName;
                FileInfo info = new FileInfo(dialog.FileName);
                int fileLength = (int)info.Length;
                Stream file = dialog.OpenFile();

                buffer = new byte[fileLength];
                file.Read(buffer, 0, fileLength);

                ReadMIDIFile();
                file.Close();
                GeneratePanel.Enabled = true;
            }
        }

        private void ReadMIDIFile()
        {
            MIDIOutputText.Clear();
            sb = new StringBuilder();
            ReadHeader();
            ReadTracks();
            MIDIOutputText.Text = sb.ToString();
        }

        private void ReadHeader()
        {
            if (buffer[0] == 'M' && buffer[1] == 'T' && buffer[2] == 'h' && buffer[3] == 'd')
            {
                // Read the header length - big endian
                int headerLength = buffer[7] + (buffer[6] <<8) + (buffer[5] <<16) + (buffer[4] << 24);
                short filetype = (short)(buffer[9] + (buffer[8] << 8));
                trackCount = (short)(buffer[11] + (buffer[10] << 8));
                short division = (short)(buffer[13] + (buffer[12] << 8));

                // Need to sort out the SMPTE type
                switch (filetype)
                {
                    case 0:
                        type = FileType.SINGLE_MULTI_CHNL_TRACK;
                        if (trackCount != 1)
                        {
                            throw new Exception("Track Count for filetype 0 must be 1, was " + trackCount);
                        }
                        break;
                    case 1:
                        type = FileType.SIMULTANEOUS_TRACKS;
                        break;
                    case 2:
                        type = FileType.INDEPENDENT_SEQ_TRACKS;
                        break;
                    default:
                        throw new Exception("Invalid File Type: " + filetype);
                }
                sb.Append("Filetype: ").Append(type.ToString()).Append(Environment.NewLine);
                sb.Append("Tracks: ").Append(trackCount).Append(Environment.NewLine);
                if ((division & 0x8000) != 0)
                {
                    smpteFormat = (byte)(division >> 8);
                    ticksPerFrame = (byte)(division & 0xFF);
                    sb.Append("SMPTE Format: ").Append(smpteFormat).Append("Ticks Per Frame: ").Append(ticksPerFrame).Append(Environment.NewLine);
                }
                else
                {
                    ticksPerQuarter = division;
                    sb.Append("Ticks Per Quarter Note: ").Append(ticksPerQuarter).Append(Environment.NewLine);
                }
                sb.AppendLine("-----------------------------------");

                tracks = new Track[trackCount];
                for (int i = 0; i< trackCount; i++)
                {
                    tracks[i] = new Track()
                    {
                        startOffset = i == 0 ? 8 + headerLength : 8 + tracks[i - 1].length + tracks[i - 1].startOffset
                    };
                    tracks[i].length = ReadTrackLength(i);
                }
                sb.AppendLine("-----------------------------------");
            }
            else
            {
                throw new Exception("Invalid Header");
            }
        }

        private int ReadTrackLength(int index)
        {
            int ptr = tracks[index].startOffset;
            if (buffer[ptr] == 'M' && buffer[ptr + 1] == 'T' && buffer[ptr + 2] == 'r' && buffer[ptr + 3] == 'k')
            {
                // Read the track length - big endian
                int trackLength = buffer[ptr + 7] + (buffer[ptr + 6] << 8) + (buffer[ptr + 5] << 16) + (buffer[ptr + 4] << 24);
                sb.Append("Track ").Append(index).Append(" length: ").Append(trackLength).Append(Environment.NewLine);
                return trackLength;
            }
            return -1;
        }

        public class MidiEventComparer : IComparer<MidiEvent>
        {
            public int Compare(MidiEvent e1, MidiEvent e2)
            {
                return (e1.index - e2.index) + (e1.type - e2.type) * 16 + (e1.midiChannel - e2.midiChannel);
            }
        }

        private void ReadTracks()
        {
            
            // Read Track 0
            ReadTrack(0);
            for (int i = 1; i < trackCount; i ++)
            {
                ReadTrack(i);
            }
        }

        private void ReadTrack(int index)
        {
            // Read events
            tracks[index].totalDeltaTime = ReadEvents(tracks[index], tracks[index].startOffset + 8, tracks[index].length);
            sb.AppendLine("-----------------------------------");
            sb.Append("Total Track Time: ").AppendLine(tracks[index].totalDeltaTime.ToString());
            sb.AppendLine("-----------------------------------");
        }

        private int ReadEvents(Track track, int ptr, int length)
        {
            int offset = 0;
            int totalTime = 0;
            while (offset < length)
            {
                // Read the variable-length delta time
                int deltatime = ReadVarInt(ptr + offset, out int varlen);
                totalTime += deltatime;
                

                MidiEvent ev = new MidiEvent()
                {
                    deltaTime = deltatime,
                    wait = samplesPerTick * deltatime,
                    index = totalTime
                };
                sb.AppendFormat("{0,6} {1,6} {2,6} ", ev.index, ev.deltaTime, ev.wait);

                offset += varlen;
                byte EventType = buffer[ptr + offset];
                if ((EventType & 0x80) == 0)
                {
                    EventType = RunningStatus;
                    offset--;
                }
                else
                {
                    RunningStatus = EventType;
                }
                switch (EventType)
                {
                    case 0xFF:
                        // Meta
                        byte metaType = buffer[ptr + offset + 1];
                        int metaLength = ReadVarInt(ptr + offset + 2, out varlen);
                        offset += varlen + 2;
                        // do stuff
                        ReadMeta(track, metaType, metaLength, ptr + offset);
                        // prepare to read the next event
                        offset += metaLength;
                        break;
                    case 0xF0:
                    case 0xF7:
                        // system exclusive message
                        int sysexLength = ReadVarInt(ptr + offset + 1, out varlen);
                        sb.Append("System Exclusive Event: ").Append(EventType).Append(", Length: ").Append(sysexLength).Append(Environment.NewLine);
                        offset += varlen + 1;
                        // do stuff

                        // prepare to read the next event
                        offset += sysexLength;
                        break;
                    default:
                        // MIDI message
                        ReadMidiEvent(track, ev, EventType, ptr + offset, out varlen);
                        offset += varlen + 1;
                        break;
                }
                
            }
            return totalTime;
        }

        private int ReadVarInt(int ptr, out int varlen)
        {
            int len = 0;
            int value = 0;
            bool done = false;
            do
            {
                value = (value << 7) + (buffer[ptr + len] & 0x7F);
                done = (buffer[ptr + len] & 0x80) == 0;
                len++;
            } while (!done && len < 4);
            varlen = len;
            return value;
        }
        private void ReadMeta(Track track, int meta, int length, int ptr)
        {
            sb.Append("Meta ");
            switch (meta)
            {
                case 0:
                    short seqNumber = (short)(buffer[ptr + 1] + (buffer[ptr] << 8));
                    sb.Append("Sequence Number: ").Append(seqNumber);
                    break;
                case 1:
                    string text = ReadText(ptr, length);
                    sb.Append("Text Event: ").Append(text);
                    break;
                case 2:
                    string copyright = ReadText(ptr, length);
                    sb.Append("Copyright Notice: ").Append(copyright);
                    break;
                case 3:
                    string trackName = ReadText(ptr, length);
                    sb.Append("Track Name: ").Append(trackName);
                    break;
                case 4:
                    string instrumentName = ReadText(ptr, length);
                    sb.Append("instrument Name: ").Append(instrumentName);
                    break;
                case 5:
                    string lyric = ReadText(ptr, length);
                    sb.Append("Lyric Name: ").Append(lyric.ToString());
                    break;
                case 6:
                    string marker = ReadText(ptr, length);
                    sb.Append("Marker Name: ").Append(marker);
                    break;
                case 7:
                    string cuePoint = ReadText(ptr, length);
                    sb.Append("Cue Point Name: ").Append(cuePoint);
                    break;
                case 0x20:
                    byte channel = buffer[ptr];
                    sb.Append("Channel Event: ").Append(channel.ToString());
                    break;
                case 0x21:
                    byte port = buffer[ptr];
                    sb.Append("MIDI Port: ").Append(port.ToString());
                    break;
                case 0x2F:
                    sb.Append("End of Track Event");
                    break;
                case 0x51:
                    microSecondsPerQuarter = buffer[ptr + 2] + (buffer[ptr + 1] << 8) + (buffer[ptr] << 16);
                    BPM = 60_000_000 / microSecondsPerQuarter;
                    sb.Append("Set Tempo Event: BPM ").Append(BPM).AppendLine();
                    sb.Append("\tSet Tempo Event: ms per quarter ").Append(microSecondsPerQuarter).AppendLine();
                    samplesPerTick = (int)(microSecondsPerQuarter / ticksPerQuarter * samplesPerSecond / 1000000);
                    sb.Append("\tSet Tempo Event: samples per tick ").Append(samplesPerTick).AppendLine();
                    break;
                case 0x54:
                    int SMPTEOffset = 0; // hr mn se fr ff
                    sb.Append("Set SMPTE Offset Event: ").Append(SMPTEOffset.ToString());
                    break;
                case 0x58:
                    TimeSignature ts = new TimeSignature()
                    {
                        numerator = buffer[ptr],
                        denominator = buffer[ptr + 1],
                        clocksPerMetronomeClick = buffer[ptr + 2],
                        notated32ndNotes = buffer[ptr + 3]
                    };
                    track.timeSignature = ts;
                    sb.Append("Time Signature Event: ").Append(track.timeSignature.ToString());
                    break;
                case 0x59:
                    sbyte sharpFlats = (sbyte)buffer[ptr]; // -7 = 7 flats, 7 = 7 sharps
                    byte majMin = buffer[ptr + 1];  // 0 Major, 1 Minor
                    string sig = (majMin == 0 ? "Major" : "Minor");
                    string key = majMin == 0 ? "C" : "a";

                    switch(sharpFlats)
                    {
                        case -1:
                            key = majMin == 0 ? "F": "d";
                            break;
                        case -2:
                            key = majMin == 0 ? "B♭" : "g";
                            break;
                        case -3:
                            key = majMin == 0 ? "E♭" : "c";
                            break;
                        case -4:
                            key = majMin == 0 ? "A♭" : "f";
                            break;
                        case -5:
                            key = majMin == 0 ? "D♭" : "b♭";
                            break;
                        case -6:
                            key = majMin == 0 ? "G♭" : "e♭";
                            break;
                        case -7:
                            key = majMin == 0 ? "B" : "g♯";
                            break;
                        
                        case 1:
                            key = majMin == 0 ? "G" : "e";
                            break;
                        case 2:
                            key = majMin == 0 ? "D" : "b";
                            break;
                        case 3:
                            key = majMin == 0 ? "A" : "f♯";
                            break;
                        case 4:
                            key = majMin == 0 ? "E" : "c♯";
                            break;
                        case 5:
                            key = majMin == 0 ? "B" : "g♯";
                            break;
                        case 6:
                            key = majMin == 0 ? "F♯" : "d♯";
                            break;
                        case 7:
                            key = majMin == 0 ? "C♯" : "a♯";
                            break;
                    }
                    
                    sb.Append("Key Signature Event: ").AppendFormat("{0} {1}", key, sig);
                    break;
                case 0x7F:
                    string seqSpecific = ReadBinary(ptr, length);
                    sb.Append("Sequencer Specific: ").Append(seqSpecific);
                    break;
                default:
                    sb.Append("Unknown Event: ").Append(meta.ToString("X2"));
                    break;
            }
            sb.Append(Environment.NewLine);
        }

        private string ReadText(int ptr, int length)
        {
            StringBuilder sb = new StringBuilder();
            int offset = 0;
            while (offset < length)
            {
                sb.Append((char)buffer[ptr + offset]);
                offset++;
            }
            return sb.ToString();
        }

        private string ReadBinary(int ptr, int length)
        {
            StringBuilder sb = new StringBuilder();
            int offset = 0;
            while (offset < length)
            {
                sb.Append(buffer[ptr + offset].ToString("X2")).Append(" ");
                offset++;
            }
            return sb.ToString();
        }

        private void ReadMidiEvent(Track track, MidiEvent e, byte type, int ptr, out int varlen)
        {
            byte ctrlMsg = (byte)(type >> 4);
            byte channel = (byte)(type & 0xF);
            e.midiChannel = channel;
            byte note = 0;
            byte velocity = 0;
            switch (ctrlMsg)
            {
                // read two bytes
                case 8:
                    note = buffer[ptr + 1];
                    velocity = buffer[ptr + 2];
                    sb.AppendFormat("Note Off Channel: {0,2}, Note: {1,3}, Velocity: {2,3}", channel, note, velocity).AppendLine();
                    varlen = 2;
                    e.note = note;
                    e.type = METype.noteoff;
                    events.Add(e);
                    break;
                case 9:
                    note = buffer[ptr + 1];
                    velocity = buffer[ptr + 2];
                    sb.AppendFormat("Note On  Channel: {0,2}, Note: {1,3}, Velocity: {2,3}", channel, note, velocity).AppendLine();
                    varlen = 2;
                    e.note = note;
                    e.velocity = velocity;
                    events.Add(e);
                    break;
                case 0xA:
                    note = buffer[ptr + 1];
                    velocity = buffer[ptr + 2];
                    sb.AppendFormat("Poly AT  Channel: {0,2}, Note: {1,3}, Velocity: {2,3}", channel, note, velocity).AppendLine();
                    varlen = 2;
                    break;
                case 0xB:
                    byte ctrlr = buffer[ptr + 1];
                    byte value = buffer[ptr + 2];
                    sb.AppendFormat("Control  Channel: {0,2}, Note: {1,3}, Velocity: {2,3}", channel, note, velocity).AppendLine();
                    varlen = 2;
                    break;
                case 0xE:
                    short pwValue = (short)((buffer[ptr + 1] << 7) + buffer[ptr + 2]);
                    sb.AppendFormat("Pitch Bd Channel: {0,2}, Value: {1,4}", channel, pwValue).AppendLine();
                    varlen = 2;
                    break;
                // read one byte
                case 0xC:
                    byte program = buffer[ptr + 1];
                    sb.AppendFormat("Prog Chg Channel: {0,2}, Program: {1,3}", channel, program).AppendLine();
                    varlen = 1;
                    e.type = METype.progchange;
                    e.program = program;
                    events.Add(e);
                    break;
                case 0xD:
                    byte pressure = buffer[ptr + 1];
                    sb.AppendFormat("Chnl Prs Channel: {0,2}, Pressure: {1,3}", channel, pressure).AppendLine();
                    varlen = 1;
                    break;
                default:
                    varlen = 0;
                    break;
            }
        }

        public static byte PercussionSet = 0x20;  // a permanent version of $BD.
        private void PercussionMode_CheckedChanged(object sender, EventArgs e)
        {
            PercussionSet = (byte)(PercussionMode.Checked ? 0x20 : 0);
        }

        public static byte[] ChannelKSL = new byte[18];
        // midi channel to opl3 channel map - $F0 is unassigned, $FF is not played.
        public static byte[] channelMap = new byte[16];  

        private void GenerateVGMButton_Click(object sender, EventArgs e)
        {
            // Print all events
            events.Sort(new MidiEventComparer());

            StringBuilder sb = new StringBuilder();

            // First pass - map midi channels to OPL3 channels
            byte fourOps = 0;
            byte twoOps = 0;
            List<byte> availableChannels = new List<byte>();
            availableChannels.Add(0);
            availableChannels.Add(1);
            availableChannels.Add(2);
            availableChannels.Add(3);
            availableChannels.Add(4);
            availableChannels.Add(5);
            if (PercussionSet == 0)
            {
                availableChannels.Add(6);
                availableChannels.Add(7);
                availableChannels.Add(8);
            }
            availableChannels.Add(9);
            availableChannels.Add(10);
            availableChannels.Add(11);
            availableChannels.Add(12);
            availableChannels.Add(13);
            availableChannels.Add(14);
            availableChannels.Add(15);
            availableChannels.Add(16);
            availableChannels.Add(17);
            byte[] twoOpChannels = new byte[15];
            for (int i = 0; i < channelMap.Length; i++)
            {
                channelMap[i] = 0xF0;
            }
            if (PercussionSet != 0)
            {
                channelMap[9] = 6;
            }

            foreach (MidiEvent ev in events)
            {
                if (ev.type != METype.progchange)
                {
                    break;
                }

                if (ev.midiChannel != 9 || PercussionSet != 0)
                {
                    byte[] prog = GeneralMidi.GetInstrument(ev.program);
                    if (prog[0] == 4)
                    {
                        // allocate 4 ops until you can't
                        if (fourOps < 7)
                        {
                            // check if the channel has already been assigned
                            if (channelMap[ev.midiChannel] == 0xF0)
                            {
                                byte opChnl = (byte)(fourOps < 3 ? fourOps : fourOps + 5);
                                channelMap[ev.midiChannel] = opChnl;
                                availableChannels.Remove(opChnl);
                                availableChannels.Remove((byte)(opChnl + 3));
                                fourOps++;
                            }
                        }
                        else
                        {
                            // disable this channel
                            channelMap[ev.midiChannel] = 0xFF;
                        }
                    }
                    else
                    {
                        twoOpChannels[twoOps] = ev.midiChannel;
                        twoOps++;
                    }
                }
            }
            if (fourOps * 4 + twoOps * 2 + 6 > 36)
            {
                throw new Exception("Insufficient number of operators!");
            }
            else
            {
                sb.AppendFormat("Two Op Channels: {0}, Four Op Channels: {1}", twoOps, fourOps).AppendLine();
            }
            // given a number of 4 operator channels, return the starting offset
            for (int i = 0; i < twoOps; i++)
            {
                byte top = availableChannels[0];
                channelMap[twoOpChannels[i]] = top;
                availableChannels.Remove(top);
            }
            byte OPL3Mode = (byte)(fourOps != 0 ? 1 : 0);
            byte connectionSel = (byte)(Math.Pow(2, fourOps) -1);
            
            int totalWaits = 0;
            int totalBytes = 0;
            MemoryStream ms = new MemoryStream();
            // set the machine in OPL3 mode - no timers
            byte[] initialRegister = {
                0x5f, 5, OPL3Mode,     // OPL3 mode
                0x5e, 1, 0x20,  // Waveform Select - Test Registers
                0x5e, 2, 0,     // timer 1
                0x5e, 3, 0,     // timer 2
                0x5e, 4, 0x60,  // RST, Timer Masks, Timer Starts
                0x5e, 8, 0x40,  // Keyboard Split
                0x5e, 0xBD, PercussionSet,  // Drum Mode
                // address 1
                0x5f, 1, 0x0,              // Waveform Select - Test Registers
                0x5f, 4, 0,        // connection sel
                0x5e, 0xB6, 0xc,     // clearing KEY ON for percussion
                0x5e, 0xB7, 0xc,     // clearing KEY ON for percussion
                0x5e, 0xB8, 0xc,     // clearing KEY ON for percussion
                0x5e, 0xA6, 0xf0,   // freq bd
                0x5e, 0xA7, 0xf0,   // freq sn
                0x5e, 0xA8, 0xf0,   // freq tt

                //0x5e, 0xC0, 0x0,   // enabling output
                //0x5e, 0xC1, 0x0,   // enabling output
                //0x5e, 0xC2, 0x0,   // enabling output
                //0x5e, 0xC3, 0x0,   // enabling output
                //0x5e, 0xC4, 0x0,   // enabling output
                //0x5e, 0xC5, 0x0,   // enabling output
                0x5e, 0xC6, 0x0,   // enabling output
                0x5e, 0xC7, 0x0,   // enabling output
                0x5e, 0xC8, 0x0,   // enabling output

                // default snare sound
                //01 f6 08 05 0 0c 08 20 f6 04 01 0 0 0  
                0x5e, 0x20 + 0x14, 01,
                0x5e, 0x60 + 0x14, 0xf6,
                0x5e, 0x80 + 0x14, 8,
                0x5e, 0xE0 + 0x14, 5,
                0x5e, 0x40 + 0x14, 0 + 0xC

            };
            ms.Write(initialRegister, 0, initialRegister.Length);
            totalBytes += initialRegister.Length;

            int idx = 0;
            foreach (MidiEvent ev in events)
            {
                if (ev.index - idx > 0)
                {
                    int wait = (ev.index - idx) * samplesPerTick;
                    totalWaits += wait;
                    while (wait > 65535)
                    {
                        sb.Append("Adding Wait: ").Append(65535).AppendLine();
                        ms.Write(new byte[3] { 0x61, 0xFF, 0xFF }, 0, 3);
                        totalBytes += 3;
                        wait -= 65535;
                    }
                    if (wait > 0)
                    {
                        sb.Append("Adding Wait: ").Append(wait).AppendLine();
                        ms.Write(new byte[3] { 0x61, (byte)(wait & 0xFF), (byte)(wait >> 8) }, 0, 3);
                        totalBytes += 3;
                    }
                }
                if (SingleChannel.SelectedIndex == 0 || (SingleChannel.SelectedIndex - 1 == ev.midiChannel))
                {
                    sb.Append(ev.index).Append("\t").Append(ev).AppendLine();
                    byte[] partial = ev.GetBytes();
                    if (partial != null)
                    {
                        ms.Write(partial, 0, partial.Length);
                        totalBytes += partial.Length;
                    }
                }
                idx = ev.index;
            }
            ms.Write(new byte[1] { 0x66 }, 0, 1); // end of song
            totalBytes += 1;
            MIDIOutputText.Text = sb.ToString();
            byte[] gd3 = CreateGD3();

            // Write the file
            string vgmFileName = Path.ChangeExtension(FileLabel.Text, ".vgm");
            if (File.Exists(vgmFileName))
            {
                File.Delete(vgmFileName);
            }
            FileStream stream = new FileStream(vgmFileName, FileMode.CreateNew);
            byte[] header = GetVGMHeader(totalWaits, totalBytes + 0x80, gd3.Length);
            stream.Write(header, 0, header.Length);
            // write data
            byte[] data = ms.GetBuffer();
            stream.Write(data, 0, (int)ms.Length);

            // write GD3 stuff
            stream.Write(gd3, 0, gd3.Length);
            stream.Flush();
            stream.Close();
        }

        // prepare the VGM header
        private byte[] GetVGMHeader(int totalWaits, int totalBytes, int gd3Length)
        {
            byte[] vgmHeader = new byte[0x80];
            vgmHeader[0] = (byte)'V';
            vgmHeader[1] = (byte)'g';
            vgmHeader[2] = (byte)'m';
            vgmHeader[3] = (byte)' ';
            // VGM version
            vgmHeader[8] = 0x51;
            vgmHeader[9] = 0x1;

            // End of File
            int filelength = totalBytes + gd3Length;
            BitConverter.GetBytes(filelength).CopyTo(vgmHeader, 4);

            // GD3 Offset
            int gd3Offset = totalBytes;
            BitConverter.GetBytes(gd3Offset - 0x14).CopyTo(vgmHeader, 0x14);

            // total # of waits
            BitConverter.GetBytes(totalWaits).CopyTo(vgmHeader, 0x18);

            // loop offset
            int loopOffset = 0;
            BitConverter.GetBytes(loopOffset).CopyTo(vgmHeader, 0x1c);
            // total # of waits in one loop
            int loopWaits = 0;
            BitConverter.GetBytes(loopWaits).CopyTo(vgmHeader, 0x20);

            // VGM Start offset
            int vgmOffset = 0x80;
            BitConverter.GetBytes(vgmOffset - 0x34).CopyTo(vgmHeader, 0x34);

            // YMF262 clock
            int ymf262Clock = 14318180;
            BitConverter.GetBytes(ymf262Clock).CopyTo(vgmHeader, 0x5c);
            return vgmHeader;
        }

        private byte[] CreateGD3()
        {
            byte[] buffer = new byte[100];
            buffer[0] = (byte)'G';
            buffer[1] = (byte)'d';
            buffer[2] = (byte)'3';
            buffer[3] = (byte)' ';
            return buffer;
        }
    }
}
