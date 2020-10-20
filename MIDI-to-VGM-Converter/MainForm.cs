using System;
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
                FileLabel.Text = dialog.FileName;
                FileInfo info = new FileInfo(dialog.FileName);
                int fileLength = (int)info.Length;
                Stream file = dialog.OpenFile();

                buffer = new byte[fileLength];
                file.Read(buffer, 0, fileLength);

                ReadMIDIFile();
                file.Close();
                GenerateVGMButton.Enabled = true;
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
                tracks = new Track[trackCount];
                
                for (int i = 0; i< trackCount; i++)
                {
                    tracks[i] = new Track()
                    {
                        startOffset = i == 0 ? tracks[0].startOffset = 8 + headerLength : 8 + tracks[i - 1].length + tracks[i - 1].startOffset,
                        length = ReadTrackLength(i)
                    };
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
                sb.Append(deltatime).Append("\t");

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
                        ReadMidiEvent(track, EventType, ptr + offset, out varlen);
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
                    track.microSecondsPerQuarter = buffer[ptr + 2] + (buffer[ptr + 1] << 8) + (buffer[ptr] << 16);
                    track.BPM = 60_000_000 / track.microSecondsPerQuarter;
                    sb.Append("Set Tempo Event: BPM ").Append(track.BPM);
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

        private void ReadMidiEvent(Track track, byte type, int ptr, out int varlen)
        {
            byte ctrlMsg = (byte)(type >> 4);
            byte channel = (byte)(type & 0xF);
            byte note = 0;
            byte velocity = 0;
            switch (ctrlMsg)
            {
                // read two bytes
                case 8:
                    note = buffer[ptr + 1];
                    velocity = buffer[ptr + 2];
                    sb.Append("Note Off Channel: " + channel + ", Note: " + note + ", Velocity: " + velocity + Environment.NewLine);
                    varlen = 2;
                    break;
                case 9:
                    note = buffer[ptr + 1];
                    velocity = buffer[ptr + 2];
                    sb.Append("Note On Channel: " + channel + ", Note: " + note + ", Velocity: " + velocity + Environment.NewLine);
                    varlen = 2;
                    break;
                case 0xA:
                    note = buffer[ptr + 1];
                    velocity = buffer[ptr + 2];
                    sb.Append("Poly AfterTouch Channel: " + channel + ", Note: " + note + ", Velocity: " + velocity + Environment.NewLine);
                    varlen = 2;
                    break;
                case 0xB:
                    byte ctrlr = buffer[ptr + 1];
                    byte value = buffer[ptr + 2];
                    sb.Append("Control Channel: ").Append(channel).Append(", Controller: ").Append(ctrlr).Append(", Value: ").Append(value).Append(Environment.NewLine);
                    varlen = 2;
                    break;
                case 0xE:
                    short pwValue = (short)((buffer[ptr + 1] << 7) + buffer[ptr + 2]);
                    sb.Append("Pitch Bend Channel: " + channel + ", Value: " + pwValue + Environment.NewLine);
                    varlen = 2;
                    break;
                // read one byte
                case 0xC:
                    byte program = buffer[ptr + 1];
                    sb.Append("Program Change Channel: ").Append(channel).Append(", Program: ").Append(program).Append(Environment.NewLine);
                    varlen = 1;
                    break;
                case 0xD:
                    byte pressure = buffer[ptr + 1];
                    sb.Append("Channel Pressure: " + channel + ", Pressure: " + pressure + Environment.NewLine);
                    varlen = 1;
                    break;
                default:
                    varlen = 0;
                    break;
            }
        }

        private void GenerateVGMButton_Click(object sender, EventArgs e)
        {
            // prepare the VGM header
            byte[] vgmHeader = new byte[0x60];
            vgmHeader[0] = (byte)'V';
            vgmHeader[0] = (byte)'g';
            vgmHeader[0] = (byte)'m';
            vgmHeader[0] = (byte)' ';
            // VGM version
            vgmHeader[8] = 0x51;
            vgmHeader[9] = 0x1;

            // End of File
            int filelength = 2000;
            BitConverter.GetBytes(filelength).CopyTo(vgmHeader, 4);
            // GD3 Offset
            int gd3Offset = 1000;
            BitConverter.GetBytes(gd3Offset - 0x14).CopyTo(vgmHeader, 0x14);
            
            // total # of waits
            int totalWaits = 40000;
            BitConverter.GetBytes(totalWaits).CopyTo(vgmHeader, 0x18);
            // loop offset
            int loopOffset = 400;
            BitConverter.GetBytes(loopOffset - 0x1c).CopyTo(vgmHeader, 0x1c);
            // total # of waits in one loop
            int loopWaits = 30000;
            BitConverter.GetBytes(loopWaits).CopyTo(vgmHeader, 0x20);
            // VGM Start offset
            int vgmOffset = 0x60;
            BitConverter.GetBytes(vgmOffset - 0x34).CopyTo(vgmHeader, 0x34);
            // YMF262 clock
            int ymf262Clock = 14318180;
            BitConverter.GetBytes(ymf262Clock).CopyTo(vgmHeader, 0x5c);
            
            // Write the file
            FileStream stream = new FileStream(FileLabel.Text, FileMode.CreateNew);
            stream.Write(vgmHeader, 0, 0x60);
            stream.Flush();
            stream.Close();
        }

        /**
         * The YMF262 has two ports, 0 and 1
           VGM Command: 0x5E aa dd : YMF262 port 0, write value dd to register aa
           VGM Command: 0x5F aa dd : YMF262 port 1, write value dd to register aa
         */
        private byte[] WriteRegister(byte port, byte register, byte value)
        {
            byte[] buffer = new byte[3];
            buffer[0] = 0x5E;
            if (port != 0)
            {
                buffer[0] = 0x5F;
            }
            buffer[1] = register;
            buffer[2] = value;
            return buffer;
        }

    }
}
