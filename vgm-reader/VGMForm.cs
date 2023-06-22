using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

/// <summary>
/// Read https://www.smspower.org/uploads/Music/vgmspec170.txt for implementation details.
/// </summary>
namespace vgm_reader
{
    public partial class VGMForm : Form
    {
        private byte[] buffer;
        private int data_stream_count;
        private List<int> data_streams;

        enum VGM_Commands : int
        {
            PSG = 0x50,
            YM2612_P0 = 0x52,
            YM2612_P1 = 0x53,
            YM2151 = 0x54,
            YM2608_P0 = 0x56,
            YM2608_P1 = 0x57,
            YM2610_P0 = 0x58,
            YM2610_P1 = 0x59,
            YMF262_P0 = 0x5e,
            YMF262_P1 = 0x5f,
            WAIT_N = 0x61,
            WAIT_60th = 0x62,
            WAIT_50th = 0x63,
            END_SONG = 0x66,
            DATABANK = 0x67,
            WAIT_1 = 0x70,
            WAIT_2 = 0x71,
            WAIT_3 = 0x72,
            WAIT_4 = 0x73,
            WAIT_5 = 0x74,
            WAIT_6 = 0x75,
            WAIT_7 = 0x76,
            WAIT_8 = 0x77,
            WAIT_9 = 0x78,
            WAIT_10 = 0x79,
            WAIT_11 = 0x7A,
            WAIT_12 = 0x7B,
            WAIT_13 = 0x7C,
            WAIT_14 = 0x7D,
            WAIT_15 = 0x7E,
            WAIT_16 = 0x7F,
            AY8910 = 0xA0,
            SEGA_PCM = 0xC0
        }
        public VGMForm()
        {
            InitializeComponent();
        }

        private void ReadFileButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                Filter = "VGM Files | *.vgm"
            };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                FileLabel.Text = dialog.FileName;
                FileInfo info = new FileInfo(dialog.FileName);
                int fileLength = (int)info.Length;
                Stream file = dialog.OpenFile();

                buffer = new byte[fileLength];
                file.Read(buffer, 0, fileLength);

                ReadVGMFile();
                file.Close();
            }
        }

        private void ReadVGMFile()
        {
            // confirm that the first 4 bytes contain "Vgm "
            if (buffer[0] == 'V' && buffer[1] == 'g' && buffer[2] == 'm' && buffer[3] == ' ' && buffer[9] == 1)
            {
                // The field values below are not necessary to read the file - without audio chips
                uint end_of_file = ReadUint(4);
                byte ver_maj = buffer[9];
                byte ver_min = buffer[8];
                uint gdm_offset = ReadUint(0x14);
                uint total_samples = ReadUint(0x18);
                uint loop_pt_offset = ReadUint(0x1C);
                uint samples_per_loop = ReadUint(0x20);
                uint rate = ReadUint(0x24); // should be 50 for PAL and 60 for NTSC
                ushort sn_fdbk = ReadUshort(0x28); // 9 sega master, 3 sega computer, 6 SN76494/SN76496
                byte sn_reg_width = buffer[0x2A]; // 16 sega master, 15 sega computer
                byte sn_flags = buffer[0x2B];

                uint ym2612_clk = ReadUint(0x2C);
                uint ym2151_clk = ReadUint(0x30);

                // Start reading the file here
                uint startSongOffset = (ver_min < 0x50) ? 0x40 : ReadUint(0x34) + 0x34;
                uint pcm_clock;
                uint pcm_intf_reg;
                byte loop_mod;
                if (ver_min > 0x50)
                {
                    pcm_clock = ReadUint(0x38);
                    pcm_intf_reg = ReadUint(0x3C);
                    loop_mod = buffer[0x7F];
                }
                if (ver_min >= 0x60)
                {

                }
                ushort loop_base;
                uint extra_hdr_offset;
                if (ver_min >= 0x70)
                {
                    loop_base = ReadUshort(0x7E);
                    extra_hdr_offset = ReadUint(0xBC);
                }
                data_stream_count = 0;
                data_streams = new List<int>();
                bool endOfSong = false; // end of song is represented by byte $66
                // read the commands
                int ptr = (int)startSongOffset;
                StringBuilder sb = new StringBuilder();
                byte reg = 0;
                byte val = 0;
                int wait = 0;
                while (!endOfSong && ptr < buffer.Length)
                {
                    byte command = buffer[ptr++];
                    switch ((command & 0xF0) >> 4)
                    {
                        case 4:
                            if (command == 0x4F)
                            {
                                val = buffer[ptr++];
                                sb.Append("PWM:" + val.ToString("X2") + " ");
                            }
                            else
                            {
                                DisplayError(sb, "Invalid Command:" + command.ToString("X2") + ", offset: " + ptr.ToString("X6"));
                                endOfSong = true;
                            }
                            break;
                        case 5:
                            switch (command)
                            {
                                case (byte)VGM_Commands.PSG:
                                    val = buffer[ptr++];
                                    DisplayPsg(sb, val);
                                    break;
                                case (byte)VGM_Commands.YM2608_P0:
                                case (byte)VGM_Commands.YM2612_P0:
                                case (byte)VGM_Commands.YM2610_P0:
                                    reg = buffer[ptr++];
                                    val = buffer[ptr++];
                                    sb.Append("OPN2-0[" + reg.ToString("X2") + "]:" + val.ToString("X2") + " ");
                                    break;
                                case (byte)VGM_Commands.YM2608_P1:
                                case (byte)VGM_Commands.YM2612_P1:
                                case (byte)VGM_Commands.YM2610_P1:
                                    reg = buffer[ptr++];
                                    val = buffer[ptr++];
                                    sb.Append("OPN2-1[" + reg.ToString("X2") + "]:" + val.ToString("X2") + " ");
                                    break;
                                case (byte)VGM_Commands.YM2151:
                                    reg = buffer[ptr++];
                                    val = buffer[ptr++];
                                    sb.Append("OPM[" + reg.ToString("X2") + "]:" + val.ToString("X2") + " ");
                                    break;
                                case (byte)VGM_Commands.YMF262_P0:
                                case (byte)VGM_Commands.YMF262_P1:
                                    reg = buffer[ptr++];
                                    val = buffer[ptr++];
                                    sb.AppendFormat("OPL3[{0}.{1:X2}]:{2:X2} ", command & 1, reg, val);
                                    break;
                            }
                            break;
                        case 6:
                            switch (command)
                            {
                                case (byte)VGM_Commands.END_SONG:
                                    endOfSong = true;
                                    sb.Append("\r\nEnd of Song Reached\r\n");
                                    break;
                                case (byte)VGM_Commands.DATABANK:
                                    //remember the databank positions
                                    int compat_cmd = buffer[ptr++]; // this byte is always 66
                                    byte data_type = buffer[ptr++];
                                    int stream_size = (int)ReadUint(ptr);
                                    data_streams.Add(ptr + 4);
                                    ptr += stream_size + 4;
                                    DisplayError(sb, "DS[" + data_stream_count + "] dt:" + data_type + " size:$" + stream_size.ToString("X4") + ", next addr: $" + ptr.ToString("X8"));
                                    data_stream_count++;
                                    break;
                                case (byte)VGM_Commands.WAIT_N:
                                    wait = buffer[ptr] + buffer[ptr + 1] * 256;
                                    ptr += 2;
                                    DisplayWait(sb, wait);
                                    break;
                                case (byte)VGM_Commands.WAIT_60th:
                                    DisplayWait(sb, 735);
                                    break;
                                case (byte)VGM_Commands.WAIT_50th:
                                    DisplayWait(sb, 882);
                                    break;
                            }
                            break;
                        case 7:
                            DisplayWait(sb, (command & 0xF) + 1);
                            break;
                        case 8:
                            // YM2612 - DAC Write from Databank
                            wait = command & 0xF;
                            sb.Append("YMDAC:" + wait.ToString("X1") + " ");
                            break;
                        case 9:
                            // DAC Stream Control
                            byte streamId;
                            switch (command)
                            {
                                case 0x90: // setup stream control
                                    streamId = buffer[ptr++];
                                    byte chipType = buffer[ptr++];
                                    byte chipCmd = buffer[ptr++];
                                    byte chipReg = buffer[ptr++];
                                    sb.Append("\r\n").Append("StCtrl Setup:" + streamId + " CT:" + chipType + " Cmd:" + chipCmd + " Reg:" + chipReg).Append("\r\n");
                                    break;
                                case 0x91: // Set stream data
                                    streamId = buffer[ptr++];
                                    byte databankID = buffer[ptr++];
                                    byte stepSize = buffer[ptr++];
                                    byte stepBase = buffer[ptr++];
                                    sb.Append("\r\n").Append("\r\n").Append("StCtrl Set Data:" + streamId + " DB: " + databankID + " SS:" + stepSize + " SB:" + stepBase).Append("\r\n");
                                    break;
                                case 0x92: // Stream Frequency
                                    streamId = buffer[ptr++];
                                    uint sampleRate = ReadUint(ptr);
                                    ptr += 4;
                                    sb.Append("\r\n").Append("StCtrl Freq:" + streamId + "," + sampleRate).Append("\r\n");
                                    break;
                                case 0x93: // Start stream
                                    streamId = buffer[ptr++];
                                    uint dataStart = ReadUint(ptr);
                                    ptr += 4;
                                    byte lengthMode = buffer[ptr++];
                                    uint dataLength = ReadUint(ptr);
                                    ptr += 4;
                                    sb.Append("\r\n").Append("StCtrl Start:" + streamId + ", S:" + dataStart.ToString("X4") + ", Mode:" + lengthMode +", L:" + dataLength.ToString("X4")).Append("\r\n");
                                    break;
                                case 0x94: // Stop stream
                                    streamId = buffer[ptr++];
                                    sb.Append("\r\n").Append("StCtrl Stop: " + streamId).Append("\r\n");
                                    break;
                                case 0x95: // start stream (fast call)
                                    streamId = buffer[ptr++];
                                    ushort blockId = ReadUshort(ptr);
                                    ptr += 2;
                                    byte flags = buffer[ptr++];
                                    sb.Append("\r\n").Append("StCtrl Start Fast: " + streamId + " B:" + blockId + " F:" + flags).Append("\r\n");
                                    break;
                            }
                            break;
                        case 0xa:
                            if (command == (byte)VGM_Commands.AY8910)
                            {
                                // 2 bytes - aa dd - register value
                                reg = buffer[ptr++];
                                val = buffer[ptr++];
                                if (reg > 16)
                                {
                                    //stop processing, there's an error
                                    sb.Append("\r\n").Append("Error reading A0 command: register value is too large: " + reg.ToString("X2") + " ");
                                    endOfSong = true;
                                    break;
                                }
                                DisplayAY38910Value(sb, reg, val);
                            }
                            break;
                        case 0xb:
                            reg = buffer[ptr++];
                            val = buffer[ptr++];
                            switch (command)
                            {
                                case 0xb2:
                                    // ad dd

                                    int sample = (reg & 0xF) * 256 + val;
                                    reg = (byte)((reg & 0xF0) >> 4);
                                    sb.Append("PWM[" + reg.ToString("X1") + "]:" + sample.ToString("X3") + " ");
                                    break;
                                default:

                                    sb.Append("2BC{" + command.ToString("X2") + "}[" + reg.ToString("X2") + "]:" + val.ToString("X2") + " ");
                                    break;
                            }

                            break;
                        case 0xc:
                            int addr = buffer[ptr] + buffer[ptr + 1] * 256;
                            val = buffer[ptr + 2];
                            ptr += 3;
                            switch (command)
                            {
                                case (byte)VGM_Commands.SEGA_PCM:
                                    sb.Append("SPCM[" + addr.ToString("X4") + "]:" + val.ToString("X2") + " ");
                                    break;
                                default:
                                    sb.Append("3BC{" + command.ToString("X2") + "}[" + addr.ToString("X4") + "]:" + val.ToString("X2") + " ");
                                    break;
                            }
                            break;
                        case 0xe:
                            // 4 bytes
                            int offset = buffer[ptr] + buffer[ptr + 1] * 256 + buffer[ptr + 2] * 256 * 256 + buffer[ptr + 3] * 256 * 256 * 256;
                            ptr += 4;
                            if (command == 0xE0)
                            {
                                sb.Append("PCM DBO:" + offset.ToString("X8") + " ");
                            }
                            else
                            {
                                sb.Append("4BC{" + command.ToString("X2") + "}:" + offset.ToString("X8") + " ");
                            }
                            break;
                        default:
                            DisplayError(sb, "Invalid Command:" + command.ToString("X2") + ", offset: " + ptr.ToString("X6"));
                            endOfSong = true;
                            break;
                    }
                }
                AY38910Text.Text = sb.ToString();
            }
            else
            {
                AY38910Text.Text = "Not a valid VGM file";
            }

        }

        byte[] AYRegisters = new byte[15];
        private void DisplayAY38910Value(StringBuilder s, byte register, byte value)
        {
            AYRegisters[register] = value;

            string channel = ((char)(((register & 7) >> 1) + 65)).ToString();
            switch (register)
            {
                case 0:
                case 2:
                case 4:
                    // low byte of the data
                    //AY38910Text.Text += "TL[" + channel + "] " + value.ToString("X2") + " ";
                    int period = AYRegisters[register + 1] * 256 + AYRegisters[register];
                    if (period == 0)
                    {
                        period = 1;
                    }
                    float f = 2000000 / (16 * period);
                    s.Append("AY.T[" + channel + "]:" + AYRegisters[register + 1].ToString("X1") + value.ToString("X2") + " f:" + f + " ");
                    break;
                case 1:
                case 3:
                case 5:
                    //AY38910Text.Text += "TH[" + channel + "] " + value.ToString("X1") + " ";
                    if (value > 7)
                    {
                        s.Append("***");
                    }
                    break;
                case 6:
                    s.Append("AY.N:" + value + " ");
                    break;
                case 7:
                    s.Append("AY.M:" + Convert.ToString(value, 2) + " ");
                    break;
                case 8:
                case 9:
                case 10:
                    s.Append("AY.A[" + channel + "]:" + value + "\r\n");
                    break;
                default:
                    s.Append("AY.#C" + register + ":" + value + " ");
                    break;
            }

        }

        /**
         * SN76489
         * | 7 | 6 | 5 | 4 | 3 | 2 | 1 | 0 |
         * | 1 | R0| R1| 0 | F6| F7| F8| F9|  -> F9 is Least Significant Bit - Tone Data
         * | 0 | X | F0| F1| F2| F3| F4| F5|  -> F0 is Most Significant Bit - Tone Data - 10 bits  - freq = Clock Rate / (32 * data)
         * | 1 | R0| R1| R2| X | FB|NF0|NF1|  -> Noise Source: FB =0 - Periodic Noise, 1 = White Noise
         * | 1 | R0| R1| 1 | A0| A1| A2| A3|  -> Attenuation
         */
        private void DisplayPsg(StringBuilder s, byte value)
        {
            if ((value & 0x80) == 0x80)
            {
                int channelValue = (value & 0x60) >> 5;
                string channel = "";
                if (channelValue == 3)
                {
                    channel = "N";
                }
                else
                {
                    channel = ((char)(channelValue + 65)).ToString();
                }
                bool freq = (value & 0x10) == 0x10;
                int val = value & 0xF;
                s.Append("PSG." + (freq ? "T" : "A") + "[" + channel + "]:" + val.ToString("X1") + " ");
            }
            else
            {
                s.Append("PSG.D:" + value.ToString("X2") + " ");
            }
        }

        private void DisplayWait(StringBuilder s, int value)
        {
            s.Append("W:" + value).Append("\r\n");
        }

        private void DisplayError(StringBuilder s, string error)
        {
            s.Append(error + "\r\n");
        }

        private uint ReadUint(int addr)
        {
            return (uint)(buffer[addr] + 256 * (buffer[addr + 1] + 256 * (buffer[addr + 2] + 256 * buffer[addr + 3])));
        }

        private ushort ReadUshort(int addr)
        {
            return (ushort)(buffer[addr] + 256 * buffer[addr + 1]);
        }
    }
}
