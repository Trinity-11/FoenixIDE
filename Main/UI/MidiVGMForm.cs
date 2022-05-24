using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace FoenixIDE.UI
{
    public enum FileType
    {
        SINGLE_MULTI_CHNL_TRACK,
        SIMULTANEOUS_TRACKS,
        INDEPENDENT_SEQ_TRACKS
    }
    public partial class MIDI_VGM_From : Form
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
        private Dictionary<String, String> meta;
        private int songTime = 0;

        private int microSecondsPerQuarter = 0; // ie. tempo
        private int BPM = 0;

        private StringBuilder sb = null;

        public MIDI_VGM_From()
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

        /**
         * Read the MIDI file header and the tracks while outputting to a StringBuilder.
         */
        private void ReadMIDIFile()
        {
            MIDIOutputText.Clear();
            sb = new StringBuilder();
            meta = new Dictionary<string, string>();
            ReadHeader();
            sb.AppendLine();
            ReadTracks();
            gridSummary.DataSource = tracks;
            gridSummary.ClearSelection();
            MIDIOutputText.Text = sb.ToString();
        }

        private void ReadHeader()
        {
            // Check the file header of the file
            if (buffer[0] == 'M' && buffer[1] == 'T' && buffer[2] == 'h' && buffer[3] == 'd')
            {
                // Read the header length - big endian
                int headerLength = buffer[7] + (buffer[6] << 8) + (buffer[5] << 16) + (buffer[4] << 24);
                // filetype 0: single multi channel track
                // filetype 1: simultaneous tracks
                // filetype 2: independent sequence tracks
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
                // Initialize the Track dropdown list
                for (int i = 0; i < trackCount; i++)
                {
                    tracks[i] = new Track()
                    {
                        Index = i,
                        isMultiChannel = false,
                        isPoly = false,
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
            // Check the track header
            if (buffer[ptr] == 'M' && buffer[ptr + 1] == 'T' && buffer[ptr + 2] == 'r' && buffer[ptr + 3] == 'k')
            {
                // Read the track length - big endian
                int trackLength = buffer[ptr + 7] + (buffer[ptr + 6] << 8) + (buffer[ptr + 5] << 16) + (buffer[ptr + 4] << 24);
                sb.Append("Track ").Append(index).Append(" length: ").Append(trackLength).Append(Environment.NewLine);
                return trackLength;
            }
            return -1;
        }

        private void ReadTracks()
        {
            songTime = 0;
            for (int i = 0; i < trackCount; i++)
            {
                ReadTrack(i);
                if (tracks[i].totalDeltaTime > songTime)
                {
                    songTime = tracks[i].totalDeltaTime;
                }
            }
        }

        private void ReadTrack(int index)
        {
            // Read events
            sb.AppendLine(string.Format("--------- Track {0}---------", index));
            tracks[index].totalDeltaTime = ReadEvents(tracks[index], tracks[index].startOffset + 8, tracks[index].length);
            sb.AppendLine("-----------------------------------");
            sb.Append("Total Track Time [" + index + "]: ").AppendLine(tracks[index].totalDeltaTime.ToString());
            if (isTrackPolyphonic(tracks[index]))
            {
                sb.Append("Track is polyphonic. ");
            }
            else
            {
                sb.Append("Track is monophonic. ");
            }
            if (isTrackMultichannel(tracks[index]))
            {
                sb.AppendLine("Track is multi-channel.");
            }
            else
            {
                sb.AppendLine("Track is single-channel.");
            }
            sb.AppendLine("-----------------------------------");
        }

        private int ReadEvents(Track track, int ptr, int length)
        {
            int offset = 0;
            int totalTime = 0;
            track.events = new List<MidiEvent>();
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
                byte eventType = buffer[ptr + offset];
                if ((eventType & 0x80) == 0)
                {
                    eventType = RunningStatus;
                    offset--;
                }
                else
                {
                    RunningStatus = eventType;
                }
                switch (eventType)
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
                        sb.Append("System Exclusive Event: ").Append(eventType).Append(", Length: ").Append(sysexLength).Append(Environment.NewLine);
                        offset += varlen + 1;
                        // do stuff

                        // prepare to read the next event
                        offset += sysexLength;
                        break;
                    default:
                        // MIDI message
                        ReadMidiEvent(track, ev, eventType, ptr + offset, out varlen);
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
        private void ReadMeta(Track track, int iMeta, int length, int ptr)
        {
            sb.Append("Meta ");
            switch (iMeta)
            {
                case 0:
                    short seqNumber = (short)(buffer[ptr + 1] + (buffer[ptr] << 8));
                    meta.Add("seq-number", string.Format("{0}", seqNumber));
                    sb.Append("Sequence Number: ").Append(seqNumber);
                    break;
                case 1:
                    string text = ReadText(ptr, length);
                    sb.Append("Text Event: ").Append(text);
                    break;
                case 2:
                    string copyright = ReadText(ptr, length);
                    if (meta.ContainsKey("copyright"))
                    {
                        String newCopyRight = meta["copyright"] + "\r\n" + copyright;
                        meta["copyright"] = newCopyRight;
                    }
                    else
                    {
                        meta.Add("copyright", copyright);
                    }
                    sb.Append("Copyright Notice: ").Append(copyright);
                    break;
                case 3:
                    string trackName = ReadText(ptr, length);
                    track.name = trackName;
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
                    meta.Add("song-name", marker);
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

                    switch (sharpFlats)
                    {
                        case -1:
                            key = majMin == 0 ? "F" : "d";
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
                    sb.Append("Unknown Event: ").Append(iMeta.ToString("X2"));
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
                    e.type = MidiEventType.noteoff;
                    track.events.Add(e);
                    break;
                case 9:
                    note = buffer[ptr + 1];
                    velocity = buffer[ptr + 2];
                    sb.AppendFormat("Note On  Channel: {0,2}, Note: {1,3}, Velocity: {2,3}", channel, note, velocity).AppendLine();
                    varlen = 2;
                    e.note = note;
                    e.velocity = velocity;
                    track.events.Add(e);
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
                    e.type = MidiEventType.progchange;
                    e.program = program;
                    track.events.Add(e);
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
        public static bool drumValueChanged;

        private void GenerateVGMButton_Click(object sender, EventArgs e)
        {
            // Gather all events into one list
            List<MidiEvent> allEvents = new List<MidiEvent>();
            drumValueChanged = false;
            if (gridSummary.SelectedRows.Count == 1)
            {
                allEvents = tracks[gridSummary.SelectedRows[0].Index].events;
            }
            else
            {
                int ix = 0;
                int nexti = 999_999;  // just a big number
                while (ix < songTime)
                {
                    for (int t = 0; t < trackCount; t++)
                    {
                        int evc = tracks[t].events.Count;
                        for (int c = 0; c < evc; c++)
                        {
                            if (tracks[t].events[c].index == ix)
                            {
                                allEvents.Add(tracks[t].events[c]);
                            }
                            else if (tracks[t].events[c].index > ix)
                            {
                                if (tracks[t].events[c].index < nexti)
                                {
                                    nexti = tracks[t].events[c].index;
                                }
                                break;
                            }
                        }
                    }
                    ix = nexti;
                    nexti = 999_999;  // just a big number
                }
            }

            StringBuilder sb = new StringBuilder();

            // First pass - map midi channels to 18 OPL3 channels, with 2 operators or 9 channels with 4 operators.
            byte fourOps = 9;
            // midi channel to opl3 channel map - $F0 is unassigned, $FF is not played.
            byte[] midiToOpMap = new byte[16];
            for (int i = 0; i < 9; i++)
            {
                midiToOpMap[i] = (byte)i;
            }
            for (int i = 9; i < 16; i++)
            {
                midiToOpMap[i] = 0xF0;
            }
            if (PercussionSet != 0)
            {
                midiToOpMap[9 - 1] = 6;
                midiToOpMap[6] = 0xF0;
            }

            //byte twoOps = 0;
            //List<byte> availableChannels = new List<byte>();
            //availableChannels.Add(0);
            //availableChannels.Add(1);
            //availableChannels.Add(2);
            //availableChannels.Add(3);
            //availableChannels.Add(4);
            //availableChannels.Add(5);
            //// If a drum track is selected, channels 6, 7 and 8 are reserved
            //if (PercussionSet == 0)
            //{
            //    availableChannels.Add(6);
            //    availableChannels.Add(7);
            //    availableChannels.Add(8);
            //}
            //availableChannels.Add(9);
            //availableChannels.Add(10);
            //availableChannels.Add(11);
            //availableChannels.Add(12);
            //availableChannels.Add(13);
            //availableChannels.Add(14);
            //availableChannels.Add(15);
            //availableChannels.Add(16);
            //availableChannels.Add(17);

            //byte[] twoOpChannels = new byte[15];
            //// set all channels to unassigned
            //for (int i = 0; i < channelMap.Length; i++)
            //{
            //    channelMap[i] = 0xF0;
            //}


            // Find the program changes
            //foreach (MidiEvent ev in allEvents)
            //{
            //    if (ev.type != MidiEventType.progchange)
            //    {
            //        break;
            //    }

            //    if (ev.midiChannel != 9 || PercussionSet != 0)
            //    {
            //        byte[] prog = GeneralMidi.GetInstrument(ev.program);
            //        // Check if the program uses 4 operators
            //        if (prog[0] == 4)
            //        {
            //            // allocate 4 ops until you can't
            //            if (fourOps < 7)
            //            {
            //                // check if the channel has already been assigned
            //                if (channelMap[ev.midiChannel] == 0xF0)
            //                {
            //                    byte opChnl = (byte)(fourOps < 3 ? fourOps : fourOps + 5);
            //                    channelMap[ev.midiChannel] = opChnl;
            //                    availableChannels.Remove(opChnl);
            //                    availableChannels.Remove((byte)(opChnl + 3));
            //                    fourOps++;
            //                }
            //            }
            //            else
            //            {
            //                // disable this channel
            //                channelMap[ev.midiChannel] = 0xFF;
            //            }
            //        }
            //        else
            //        {
            //            twoOpChannels[twoOps] = ev.midiChannel;
            //            twoOps++;
            //        }
            //    }
            //}
            //if (fourOps * 4 + twoOps * 2 + 6 > 36)
            //{
            //    throw new Exception("Insufficient number of operators!");
            //}
            //else
            //{
            //    sb.AppendFormat("Two Op Channels: {0}, Four Op Channels: {1}", twoOps, fourOps).AppendLine();
            //}
            //// given a number of 4 operator channels, return the starting offset
            //for (int i = 0; i < twoOps; i++)
            //{
            //    byte top = availableChannels[0];
            //    channelMap[twoOpChannels[i]] = top;
            //    availableChannels.Remove(top);
            //}
            byte OPL3Mode = (byte)(fourOps != 0 ? 1 : 0);
            //byte connectionSel = (byte)(Math.Pow(2, fourOps) -1);

            int totalWaits = 0;
            int totalBytes = 0;
            MemoryStream ms = new MemoryStream();
            // set the machine in OPL3 mode - no timers
            byte[] initialRegister = {
                0x5f, 5, OPL3Mode,      // OPL3 mode
                0x5e, 1, 0x20,          // Waveform Select - Test Registers
                0x5e, 2, 0,             // timer 1
                0x5e, 3, 0,             // timer 2
                0x5e, 4, 0x60,          // RST, Timer Masks, Timer Starts
                0x5e, 8, 0x40,          // Keyboard Split
                0x5e, 0xBD, PercussionSet,  // Drum Mode
                // address 1
                0x5f, 1, 0x0,           // Waveform Select - Test Registers
                0x5f, 4, 0,             // connection sel
                0x5e, 0xB6, 0xc,        // clearing KEY ON for percussion
                0x5e, 0xB7, 0xc,        // clearing KEY ON for percussion
                0x5e, 0xB8, 0xc,        // clearing KEY ON for percussion
                0x5e, 0xA6, 0xf0,       // freq bd
                0x5e, 0xA7, 0xf0,       // freq sn
                0x5e, 0xA8, 0xf0,       // freq tt

                0x5e, 0xC0, 0x0,        // enabling output
                0x5e, 0xC1, 0x0,        // enabling output
                0x5e, 0xC2, 0x0,        // enabling output
                0x5e, 0xC3, 0x0,        // enabling output
                0x5e, 0xC4, 0x0,        // enabling output
                0x5e, 0xC5, 0x0,        // enabling output
                0x5e, 0xC6, 0x0,        // enabling output
                0x5e, 0xC7, 0x0,        // enabling output
                0x5e, 0xC8, 0x0,        // enabling output
            };
            ms.Write(initialRegister, 0, initialRegister.Length);
            totalBytes += initialRegister.Length;

            // Convert MIDI events to VGM
            int idx = 0;
            foreach (MidiEvent ev in allEvents)
            {
                if (ev.index - idx > 0)
                {
                    // we accumulate the drum notes into one byte for register $BD
                    if (drumValueChanged)
                    {
                        byte[] drumBuffer = new byte[3];
                        drumBuffer[0] = 0x5e;
                        drumBuffer[1] = 0xBD;
                        drumBuffer[2] = PercussionSet;
                        drumValueChanged = false;
                        ms.Write(drumBuffer, 0, drumBuffer.Length);
                        totalBytes += 3;
                    }

                    // now compute the wait time
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
                sb.Append(ev.index).Append("\t").Append(ev).AppendLine();
                byte[] opl3bytes = ev.GetOPL3Bytes(midiToOpMap);
                if (opl3bytes != null)
                {
                    ms.Write(opl3bytes, 0, opl3bytes.Length);
                    totalBytes += opl3bytes.Length;
                }

                idx = ev.index;
            }
            // Empty the drum buffer
            if (drumValueChanged)
            {
                byte[] drumBuffer = new byte[3];
                drumBuffer[0] = 0x5e;
                drumBuffer[1] = 0xBD;
                drumBuffer[2] = PercussionSet;
                drumValueChanged = false;
                ms.Write(drumBuffer, 0, drumBuffer.Length);
                totalBytes += 3;
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
            byte[] header = new byte[8] { (byte)'G', (byte)'d', (byte)'3', (byte)' ', 0x0, 0x1, 0x0, 0x0 };
            MemoryStream ms = new MemoryStream();
            ms.Write(header, 0, 8);

            // Next, write the size of the data - each character is 2 bytes and each eol is 2 bytes.
            ms.Write(BitConverter.GetBytes(22), 0, 4);

            for (int i = 0; i < 11; i++)
            {
                //write the next string
                ms.Write(new byte[2] { 0, 0 }, 0, 2);
            }
            ms.Flush();
            return ms.ToArray();
        }

        /**
         * For each track, try to determine if they are polyphonic.
         * The Drum track is usually polyphonic.  If Rhythm is enabled in OPL3, then 6 channels are used to produce these.
         * Melodic tracks can have multiple notes on at the same time, so the purpose of this function is to allow other channels in the OPL3 for this.
         */
        private void btnPolyphonic_Click(object sender, EventArgs e)
        {
            for (int t = 0; t < trackCount; t++)
            {
                Track track = tracks[t];

            }
        }

        /** 
         * Check if a track has polyphonic events.
         * Polyphony to be true, two notes on the same channel must be ON at the same time.
         */
        public bool isTrackPolyphonic(Track track)
        {
            List<int> notes = new List<int>();
            for (int i = 0; i < track.events.Count; i++)
            {
                MidiEvent ev = track.events[i];
                switch (ev.type)
                {
                    case MidiEventType.noteon:
                        // velocity = 0 means it's now off.
                        if (ev.velocity == 0)
                        {
                            notes.Remove(ev.midiChannel * 128 + ev.note);
                        }
                        else
                        {
                            if (!notes.Contains(ev.midiChannel * 128 + ev.note))
                            {
                                notes.Add(ev.midiChannel * 128 + ev.note);
                            }
                        }
                        break;

                    case MidiEventType.noteoff:
                        notes.Remove(ev.midiChannel * 128 + ev.note);
                        break;
                }
                if (notes.Count > 1)
                {
                    track.isPoly = true;
                    return true;
                }
            }
            return false;
        }

        public bool isTrackMultichannel(Track track)
        {
            List<int> channels = new List<int>();
            for (int i = 0; i < track.events.Count; i++)
            {
                MidiEvent ev = track.events[i];
                if (!channels.Contains(ev.midiChannel))
                {
                    channels.Add(ev.midiChannel);
                }
                if (channels.Count > 1)
                {
                    track.isMultiChannel = true;
                    return true;
                }
            }
            if (channels.Count == 1)
            {
                track.midiChannel = channels[0];
            }
            return false;
        }

        private void gridSummary_Click(object sender, EventArgs e)
        {

        }
    }
}
