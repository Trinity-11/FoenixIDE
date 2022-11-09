using FoenixIDE.MemoryLocations;
using FoenixIDE.Simulator.Devices;
using FoenixIDE.Simulator.FileFormat;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using System.Threading;
using System.Windows.Forms;

namespace FoenixIDE.UI
{
    public partial class UploaderWindow : Form
    {
        public static byte TxLRC = 0;
        public static byte RxLRC = 0;
        public static byte Stat0 = 0;
        public static byte Stat1 = 0;
        public static byte LRC = 0;
        public static string[] ports;
        public FoenixSystem kernel = null;
        private BoardVersion boardVersion = BoardVersion.RevC;

        SerialPort serial = new SerialPort();
        private Queue<byte> recievedData = new Queue<byte>();

        public void SetBoardVersion(BoardVersion ver)
        {
            boardVersion = ver;
            switch (ver)
            {
                case BoardVersion.RevB:
                    RevModeLabel.Text = "Mode: RevB";
                    break;
                case BoardVersion.RevC:
                    RevModeLabel.Text = "Mode: RevC";
                    break;
                case BoardVersion.RevU:
                    RevModeLabel.Text = "Mode: RevU";
                    break;
                case BoardVersion.RevUPlus:
                    RevModeLabel.Text = "Mode: RevU+";
                    break;
                case BoardVersion.RevJr:
                    RevModeLabel.Text = "Mode: F256Jr";
                    break;
            }
        }

        public UploaderWindow()
        {
            InitializeComponent();

            serial.BaudRate = 6000000;
            serial.Handshake = System.IO.Ports.Handshake.None;
            serial.Parity = Parity.None;
            serial.DataBits = 8;
            serial.StopBits = StopBits.One;
            serial.ReadTimeout = 2000;
            serial.WriteTimeout = 2000;
            //serial.RtsEnable = true;
            //serial.DtrEnable = true;
            ports = SerialPort.GetPortNames();  // Save the Ports Name in a String Array
            // 
            Console.WriteLine("Available Ports:");
            // Save the Ports Name in the Items list of the ComboBox
            foreach (string s in SerialPort.GetPortNames())
            {
                COMPortComboBox.Items.Add(s);

                Console.WriteLine("   {0}", s);
            }
            if (COMPortComboBox.Items.Count == 0)
            {
                COMPortComboBox.Items.Add("-----");
            }
            COMPortComboBox.SelectedItem = COMPortComboBox.Items[0];
        }

        private int GetTransmissionSize()
        {
            int transmissionSize = -1;
            if (SendFileRadio.Checked)
            {
                GetFileLength(FileNameTextBox.Text);
                transmissionSize = Convert.ToInt32(FileSizeResultLabel.Text.Replace("$", "").Replace(":", ""), 16);
            }
            else if (BlockSendRadio.Checked)
            {
                transmissionSize = Convert.ToInt32(EmuSrcSize.Text.Replace("$", "").Replace(":", ""), 16);
            }
            else
            {
                transmissionSize = Convert.ToInt32(C256SrcSize.Text.Replace("$", "").Replace(":", ""), 16);
            }
            return transmissionSize;
        }

        private void ConnectButton_Click(object sender, EventArgs e)
        {
            try
            {
                serial.PortName = COMPortComboBox.Items[COMPortComboBox.SelectedIndex].ToString();
                serial.Open();
                // Enable all the button if the serial Port turns out to be the good one.
                BrowseFileButton.Enabled = SendFileRadio.Checked;

                SendBinaryButton.Enabled = GetTransmissionSize() > 0;
                COMPortComboBox.Enabled = false;
                ConnectButton.Visible = false;
                DisconnectButton.Visible = true;

                Console.WriteLine("Serial Port Connected: " + ports[COMPortComboBox.SelectedIndex]);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Serial Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DisconnectButton_Click(object sender, EventArgs e)
        {
            serial.Close();
            ConnectButton.Visible = true;
            DisconnectButton.Visible = false;
            COMPortComboBox.Enabled = true;
            SendBinaryButton.Enabled = false;
        }

        private void UploaderWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            DisconnectButton_Click(null, null);
        }

        private long GetFileLength(String filename)
        {
            long flen = 0;
            // Display the file length in hex
            if (filename != null && filename.Length > 0)
            {
                string fileExtension = Path.GetExtension(filename).ToUpper();
                if (fileExtension.Equals(".BIN"))
                {
                    FileInfo f = new FileInfo(filename);
                    flen = f.Length;
                }
                else if (fileExtension.Equals(".HEX"))
                {
                    // We're loading a HEX file, so only consider the lines that are record type 00
                    string[] lines = System.IO.File.ReadAllLines(filename);
                    foreach (string l in lines)
                    {
                        if (l.StartsWith(":"))
                        {
                            string mark = l.Substring(0, 1);
                            string reclen = l.Substring(1, 2);
                            string offset = l.Substring(3, 4);
                            string rectype = l.Substring(7, 2);

                            if (rectype.Equals("00"))
                            {
                                flen += Convert.ToInt32(reclen, 16);
                            }
                        }
                    }
                }
                else if (fileExtension.Equals(".PGX"))
                {
                    FileInfo f = new FileInfo(filename);
                    flen = f.Length - 8;
                }
                else if (fileExtension.Equals(".PGZ"))
                {
                    // Read the file to find the number of blocks and the block lengths
                    FileInfo f = new FileInfo(FileNameTextBox.Text);
                    BinaryReader reader = new BinaryReader(f.OpenRead());
                    byte header = reader.ReadByte();  // this should be Z for 24-bits and z for 32-bits
                    int size = header == 'z'?4:3;
                    flen = 0;
                    do
                    {
                        byte[] bufAddr = reader.ReadBytes(size);
                        byte[] bufLength = reader.ReadBytes(size);
                        int blockLen = bufLength[0] + bufLength[1] * 0x100 + bufLength[2] * 0x10000 + (size == 4 ? bufLength[3] * 0x1000000 : 0);
                        flen += blockLen;
                        reader.BaseStream.Seek(blockLen, SeekOrigin.Current);
                    } while (reader.BaseStream.Position < f.Length);
                    reader.Close();
                }
            }
            String hexSize = flen.ToString("X6");
            FileSizeResultLabel.Text = "$" + hexSize.Substring(0, 2) + ":" + hexSize.Substring(2);
            return flen;
        }

        /*
         * Let the user select a file from the file system and display it in a text box.
         */
        private void BrowseFileButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDlg = new OpenFileDialog
            {
                DefaultExt = ".hex",
                Filter = "Hex documents|*.hex|Binary documents|*.bin|PGX Files|*.pgx|PGZ Files|*.pgz",
                Title = "Upload to the C256 Foenix"
            };

            // Load content of file in a TextBlock
            if (openFileDlg.ShowDialog() == DialogResult.OK)
            {
                string extension = Path.GetExtension(openFileDlg.FileName);
                // Display the file name
                FileNameTextBox.Text = openFileDlg.FileName;
                C256DestAddress.Enabled = extension.ToUpper().Equals(".BIN");
                ReflashCheckbox.Enabled = extension.ToUpper().Equals(".BIN");
                // Display the file length
                long flen = GetFileLength(openFileDlg.FileName);
                    
                SendBinaryButton.Enabled = (flen != -1) && !ConnectButton.Visible;
            }
        }

        /**
         * This method fires whenever the radio buttons are changed.
         */
        private void SendFileRadio_CheckedChanged(object sender, EventArgs e)
        {
            FileNameTextBox.Enabled = SendFileRadio.Checked;
            BrowseFileButton.Enabled = SendFileRadio.Checked;

            int transmissionSize = GetTransmissionSize();
            EmuSrcSize.Enabled = BlockSendRadio.Checked;
            EmuSrcAddress.Enabled = BlockSendRadio.Checked;
            if (FileNameTextBox.Text.Length == 0 || BlockSendRadio.Checked)
            {
                C256DestAddress.Enabled = (transmissionSize > 0 || BlockSendRadio.Checked);
            }
            else
            {
                string extension = Path.GetExtension(FileNameTextBox.Text).ToUpper();
                C256DestAddress.Enabled = (transmissionSize > 0 || BlockSendRadio.Checked) && (extension.Equals(".BIN") || ReflashCheckbox.Checked);
            }
            

            C256SrcSize.Enabled = FetchRadio.Checked;
            C256SrcAddress.Enabled = FetchRadio.Checked;

            SendBinaryButton.Enabled = (transmissionSize > 0) && !ConnectButton.Visible;
            SendBinaryButton.Text = FetchRadio.Checked ? "Fetch from C256" : "Send Binary";
        }

        private void SendBinaryButton_Click(object sender, EventArgs e)
        {
            SendBinaryButton.Enabled = false;
            DisconnectButton.Enabled = false;
            hideLabelTimer_Tick(null, null);
            int transmissionSize = GetTransmissionSize();
            UploadProgressBar.Maximum = transmissionSize;
            UploadProgressBar.Value = 0;
            UploadProgressBar.Visible = true;

            int BaseBankAddress = 0x38_0000;
            if (boardVersion == BoardVersion.RevB || boardVersion == BoardVersion.RevU)
            {
                BaseBankAddress = 0x18_0000;
            }
            else if (boardVersion == BoardVersion.RevJr)
            {
                BaseBankAddress = 0;
            }

            if (SendFileRadio.Checked)
            {
                if (serial.IsOpen)
                {
                    // Get into Debug mode (Reset the CPU and keep it in that state and Gavin will take control of the bus)
                    if (DebugModeCheckbox.Checked)
                    {
                        GetFnxInDebugMode();
                    }
                    string fileExtension = Path.GetExtension(FileNameTextBox.Text).ToUpper();
                    if (fileExtension.Equals(".BIN"))
                    {
                        // Read the bytes and put them in the buffer
                        byte[] DataBuffer = System.IO.File.ReadAllBytes(FileNameTextBox.Text);
                        int FnxAddressPtr = int.Parse(C256DestAddress.Text.Replace(":", ""), System.Globalization.NumberStyles.AllowHexSpecifier);
                        Console.WriteLine("Starting Address: " + FnxAddressPtr);
                        Console.WriteLine("File Size: " + transmissionSize);
                        SendData(DataBuffer, FnxAddressPtr, transmissionSize);

                        // Update the Reset Vectors from the Binary Files Considering that the Files Keeps the Vector @ $00:FF00
                        if (FnxAddressPtr < 0xFF00 && (FnxAddressPtr + DataBuffer.Length) > 0xFFFF || (FnxAddressPtr == BaseBankAddress && DataBuffer.Length > 0xFFFF))
                        {
                            PreparePacket2Write(DataBuffer, 0x00FF00, 0x00FF00, 256);
                        }
                    }
                    else if (fileExtension.Equals(".PGX"))
                    {
                        FileInfo f = new FileInfo(FileNameTextBox.Text);
                        int flen = (int)(f.Length - 8);
                        BinaryReader reader = new BinaryReader(f.OpenRead());
                        // The first four byte contain PGX 0x1
                        byte[] header = reader.ReadBytes(4);
                        // The next four bytes contain the start address
                        int FnxAddressPtr = reader.ReadInt32();
                        // The rest of the file is data
                        byte[] DataBuffer = reader.ReadBytes(flen);
                        reader.Close();

                        Console.WriteLine("Starting Address: " + FnxAddressPtr);
                        Console.WriteLine("File Size: " + transmissionSize);
                        SendData(DataBuffer, FnxAddressPtr, transmissionSize);

                        // Generate a fresh page $FF
                        byte[] pageFF = CreateResetPage(FnxAddressPtr);

                        // Update the Reset Vectors from the Binary Files Considering that the Files Keeps the Vector @ $00:FF00
                        PreparePacket2Write(pageFF, 0x00FF00, 0, 256);
                    }
                    else if (fileExtension.Equals(".PGZ"))
                    {
                        FileInfo f = new FileInfo(FileNameTextBox.Text);
                        BinaryReader reader = new BinaryReader(f.OpenRead());
                        byte header = reader.ReadByte();  // this should be Z for 24-bits and z for 32-bits
                        int size = header == 'z' ? 4 : 3;
                        int FnxAddressPtr = -1;
                        bool resetVector = false;

                        // Read page $FF so we don't clobber everything
                        byte[] pageFF = new byte[256];

                        do
                        {
                            byte[] bufAddr = reader.ReadBytes(size);
                            byte[] bufLength = reader.ReadBytes(size);
                            int address = bufAddr[0] + bufAddr[1] * 0x100 + bufAddr[2] * 0x10000 + (size == 4 ? bufAddr[3] * 0x1000000 : 0);
                            int blockLength = bufLength[0] + bufLength[1] * 0x100 + bufLength[2] * 0x10000 + (size == 4 ? bufLength[3] * 0x1000000 : 0);
                            if (blockLength == 0)
                            {
                                FnxAddressPtr = address;
                            }
                            else
                            {
                                byte[] DataBuffer = reader.ReadBytes(blockLength);
                                SendData(DataBuffer, address, blockLength);

                                // TODO - make this backward compatible
                                if (address >= (BaseBankAddress + 0xFF00) && (address < (BaseBankAddress + 0xFFFF)) )
                                {
                                    int pageFFLen = blockLength - ((address + blockLength) - (BaseBankAddress + 0x1_0000));
                                    if (pageFFLen > blockLength)
                                    {
                                        pageFFLen = blockLength;
                                    }
                                    Array.Copy(DataBuffer, 0, pageFF, address - (BaseBankAddress + 0xFF00), 0x100);
                                    resetVector = true;
                                }
                            }

                        } while (reader.BaseStream.Position < f.Length);
                        reader.Close();

                        // If page FF is not found in code, assume that a standard kernel page is required
                        if (!resetVector)
                        {
                            // Generate a fresh page $FF
                            pageFF = CreateResetPage(FnxAddressPtr);
                            resetVector = true;
                        }
                        // Update pageFF with the start address
                        if (resetVector)
                        {
                            // Update the Reset Vectors from the Binary Files Considering that the Files Keeps the Vector @ $00:FF00
                            PreparePacket2Write(pageFF, 0x00FF00, 0, 256);
                        }
                    }
                    else if (fileExtension.Equals(".HEX"))
                    {
                        bool resetVector = false;
                        // Page FF is used to store IRQ vectors - this is only used when the program modifies the
                        // values between BaseBank + FF00 to BaseBank + FFFF
                        // BaseBank on RevB is $18
                        // BaseBank on RevC is $38
                        byte[] pageFF = PreparePacket2Read(0xFF00, 0x100);
                        // If send HEX files, each time we encounter a "bank" change - record 04 - send a new data block
                        string[] lines = System.IO.File.ReadAllLines(FileNameTextBox.Text);
                        int bank = 0;
                        int address = 0;

                        foreach (string l in lines)
                        {
                            if (l.StartsWith(":"))
                            {
                                string mark = l.Substring(0, 1);
                                string reclen = l.Substring(1, 2);
                                string offset = l.Substring(3, 4);
                                string rectype = l.Substring(7, 2);
                                string data = l.Substring(9, l.Length - 11);
                                string checksum = l.Substring(l.Length - 2);

                                switch (rectype)
                                {
                                    case "00":
                                        int length = Convert.ToInt32(reclen, 16);
                                        byte[] DataBuffer = new byte[length];
                                        address = HexFile.GetByte(offset, 0, 2);
                                        for (int i = 0; i < data.Length; i += 2)
                                        {
                                            DataBuffer[i / 2] = (byte)HexFile.GetByte(data, i, 1);
                                        }
                                        PreparePacket2Write(DataBuffer, bank + address, 0, length);
                                        
                                        // TODO - make this backward compatible
                                        if (bank + address >= (BaseBankAddress + 0xFF00) && (bank + address) < (BaseBankAddress + 0xFFFF))
                                        {
                                            int pageFFLen = length - ((bank + address + length) - (BaseBankAddress + 0x1_0000));
                                            if (pageFFLen > length)
                                            {
                                                pageFFLen = length;
                                            }
                                            Array.Copy(DataBuffer, 0, pageFF, bank + address - (BaseBankAddress + 0xFF00), length);
                                            resetVector = true;
                                        }
                                        UploadProgressBar.Increment(length);

                                        break;
                                    case "01":
                                        // Don't do anything... this is the end of file record.
                                        break;

                                    case "02":
                                        bank = HexFile.GetByte(data, 0, 2) * 16;
                                        break;

                                    case "04":
                                        bank = HexFile.GetByte(data, 0, 2) << 16;
                                        break;

                                    default:
                                        Console.WriteLine("Unsupport HEX record type:" + rectype);
                                        break;

                                }
                            }
                        }
                        if (DebugModeCheckbox.Checked)
                        {
                            // Update the Reset Vectors from the Binary Files Considering that the Files Keeps the Vector @ $00:FF00
                            if (resetVector)
                            {
                                PreparePacket2Write(pageFF, 0x00FF00, 0, 256);
                            }
                        }

                    }
                    if (ReflashCheckbox.Checked && MessageBox.Show("Are you sure you want to reflash your C256 System?", "Reflash", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        CountdownLabel.Visible = true;
                        this.Update();

                        EraseFlash();
                        int SrcFlashAddress = Convert.ToInt32(C256DestAddress.Text.Replace(":", ""), 16);
                        ProgramFlash(SrcFlashAddress);
                        CountdownLabel.Visible = false;
                    }
                    if (DebugModeCheckbox.Checked)
                    {
                        // The Loading of the File is Done, Reset the FNX and Get out of Debug Mode
                        ExitFnxDebugMode();
                    }
                    HideProgressBarAfter5Seconds("Transfer Done! System Reset!");
                }
            }
            else if (BlockSendRadio.Checked && kernel.CPU != null)
            {
                // Get into Debug mode (Reset the CPU and keep it in that state and Gavin will take control of the bus)
                if (DebugModeCheckbox.Checked)
                {
                    GetFnxInDebugMode();
                }
                int blockAddress = Convert.ToInt32(EmuSrcAddress.Text.Replace(":",""), 16);
                // Read the data directly from emulator memory
                int offset = 0;
                int FnxAddressPtr = int.Parse(C256DestAddress.Text.Replace(":", ""), System.Globalization.NumberStyles.AllowHexSpecifier);
                byte[] DataBuffer = new byte[transmissionSize];  // Maximum 2 MB, example from $0 to $1F:FFFF.
                for (int start = blockAddress; start < blockAddress + transmissionSize; start++)
                {
                    DataBuffer[offset++] = kernel.CPU.MemMgr.ReadByte(start);
                }
                SendData(DataBuffer, FnxAddressPtr, transmissionSize);
                // Update the Reset Vectors from the Binary Files Considering that the Files Keeps the Vector @ $00:FF00
                if (FnxAddressPtr < 0xFF00 && (FnxAddressPtr + DataBuffer.Length) > 0xFFFF || (FnxAddressPtr == BaseBankAddress && DataBuffer.Length > 0xFFFF))
                {
                    PreparePacket2Write(DataBuffer, 0x00FF00, 0x00FF00, 256);
                }
                if (DebugModeCheckbox.Checked)
                {
                    // The Loading of the File is Done, Reset the FNX and Get out of Debug Mode
                    ExitFnxDebugMode();
                }
                HideProgressBarAfter5Seconds("Transfer Done! System Reset!");
            }
            else
            {
                int blockAddress = Convert.ToInt32(C256SrcAddress.Text.Replace(":", ""), 16);
                byte[] DataBuffer = new byte[transmissionSize];  // Maximum 2 MB, example from $0 to $1F:FFFF.
                if (FetchData(DataBuffer, blockAddress, transmissionSize, DebugModeCheckbox.Checked))
                {
                    MemoryRAM mem = new MemoryRAM(blockAddress, transmissionSize);
                    mem.Load(DataBuffer, 0, 0, transmissionSize);
                    MemoryWindow tempMem = new MemoryWindow
                    {
                        Memory = mem,
                        Text = "C256 Memory from " + blockAddress.ToString("X6") + 
                            " to " + (blockAddress + transmissionSize - 1).ToString("X6")
                    };
                    tempMem.GotoAddress(blockAddress);
                    tempMem.AllowSave();
                    tempMem.Show();
                }
                SendBinaryButton.Enabled = true;
                DisconnectButton.Enabled = true;
            }
            
        }

        private byte[] CreateResetPage(int startAddress)
        {
            byte[] pageFF = new byte[256];

            /* The Hex code
                : 06 FF00 18 FB 5C 001000  
                : 0B FF10 C2308B0B48DA5A5C081000
                : 0B FF20 C2308B0B48DA5A5C081000
                : 0B FF30 C2308B0B48DA5A5C081000
                : 11 FF40 C2308B0B48DA5A229619387AFA682BAB40
                : 11 FF60 C2308B0B48DA5A22C017387AFA682BAB40
                : 20 FFE0 5C04003A10FF20FF30FF40FF000060FF5C9B063910FF20FF30FF40FF00FF60FF
                */
            Array.Copy(new byte[] { 0x18, 0xFB, 0x5C }, pageFF, 3);

            // Address
            pageFF[3] = (byte)(startAddress & 0xFF);
            pageFF[4] = (byte)((startAddress >> 8) & 0xFF);
            pageFF[5] = (byte)((startAddress >> 16) & 0xFF);

            Array.Copy(new byte[] { 0xC2, 0x30, 0x8B, 0x0B, 0x48, 0xDA, 0x5A, 0x5C, 0x08, 0x10, 0x00 }, 0, pageFF, 0x10, 11);
            Array.Copy(new byte[] { 0xC2, 0x30, 0x8B, 0x0B, 0x48, 0xDA, 0x5A, 0x5C, 0x08, 0x10, 0x00 }, 0, pageFF, 0x20, 11);
            Array.Copy(new byte[] { 0xC2, 0x30, 0x8B, 0x0B, 0x48, 0xDA, 0x5A, 0x5C, 0x08, 0x10, 0x00 }, 0, pageFF, 0x30, 11);
            Array.Copy(new byte[] { 0xC2, 0x30, 0x8B, 0x0B, 0x48, 0xDA, 0x5A, 0x22, 0x96, 0x19, 0x38, 0x7A, 0xFA, 0x68, 0x2B, 0xAB, 0x40 }, 0, pageFF, 0x40, 17);
            Array.Copy(new byte[] { 0xC2, 0x30, 0x8B, 0x0B, 0x48, 0xDA, 0x5A, 0x22, 0xC0, 0x17, 0x38, 0x7A, 0xFA, 0x68, 0x2B, 0xAB, 0x40 }, 0, pageFF, 0x60, 17);
            Array.Copy(new byte[] {0x5C, 0x04, 0x00, 0x3A, 0x10, 0xFF, 0x20, 0xFF, 0x30, 0xFF, 0x40, 0xFF, 0x00, 0x00, 0x60, 0xFF, 0x5C,
                                0x9B, 0x06, 0x39, 0x10, 0xFF, 0x20, 0xFF, 0x30, 0xFF, 0x40, 0xFF, 0x00, 0xFF, 0x60, 0xFF }, 0, pageFF, 0xE0, 32);
            return pageFF;
        }

        private void HideProgressBarAfter5Seconds(string message)
        {
            UploadProgressBar.Visible = false;
            CountdownLabel.Visible = true;
            CountdownLabel.Text = message;
            hideLabelTimer.Enabled = true;
            SendBinaryButton.Enabled = true;
            DisconnectButton.Enabled = true;
        }

        private void hideLabelTimer_Tick(object sender, EventArgs e)
        {
            hideLabelTimer.Enabled = false;
            CountdownLabel.Visible = false;
            CountdownLabel.Text = "";
        }


        private byte Checksum(byte[] buffer, int length)
        {
            byte checksum = 0x55;
            for (int i = 1; i < length; i++)
            {
                checksum ^= buffer[i];
            }
            return checksum;
        }

        private void EraseFlash()
        {
            CountdownLabel.Text = "Erasing Flash";
            this.Update();
            byte[] commandBuffer = new byte[8];
            commandBuffer[0] = 0x55;   // Header
            commandBuffer[1] = 0x11;   // Reset Flash
            commandBuffer[2] = 0x00;
            commandBuffer[3] = 0x00;
            commandBuffer[4] = 0x00;
            commandBuffer[5] = 0x00;
            commandBuffer[6] = 0x00;
            commandBuffer[7] = Checksum(commandBuffer, 7);
            SendMessage(commandBuffer, null);
        }

        private void ProgramFlash(int address)
        {
            CountdownLabel.Text = "Programming Flash";
            this.Update();
            byte[] commandBuffer = new byte[8];
            commandBuffer[0] = 0x55;   // Header
            commandBuffer[1] = 0x10;   // Reset Flash
            commandBuffer[2] = (byte)((address & 0xFF_0000) >> 16);
            commandBuffer[3] = (byte)((address & 0x00_FF00) >> 8);
            commandBuffer[4] = (byte)(address & 0x00_00FF);
            commandBuffer[5] = 0x00;
            commandBuffer[6] = 0x00;
            commandBuffer[7] = Checksum(commandBuffer, 7);
            SendMessage(commandBuffer, null, 10000);
        }

        private void SendData(byte[] buffer, int startAddress, int size)
        {
            try
            {
                if (serial.IsOpen)
                {
                    // Now's let's transfer the code
                    if (size <= 2048)
                    {
                        // DataBuffer = The buffer where the loaded Binary File resides
                        // FnxAddressPtr = Pointer where to put the Data in the Fnx
                        // i = Pointer Inside the data buffer
                        // Size_Of_File = Size of the Payload we want to transfer which ought to be smaller than 2048 bytes
                        PreparePacket2Write(buffer, startAddress, 0, size);
                        UploadProgressBar.Increment(size);
                    }
                    else
                    {
                        int BufferSize = 2048;
                        int Loop = size / BufferSize;
                        int offset = startAddress;
                        for (int j = 0; j < Loop; j++)
                        {
                            PreparePacket2Write(buffer, offset, j * BufferSize, BufferSize);
                            offset += BufferSize;   // Advance the Pointer to the next location where to write Data in the Foenix
                            UploadProgressBar.Increment(BufferSize);
                        }
                        BufferSize = (size % BufferSize);
                        if (BufferSize > 0)
                        {
                            PreparePacket2Write(buffer, offset, size - BufferSize, BufferSize);
                            UploadProgressBar.Increment(BufferSize);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Send Binary Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool FetchData(byte[] buffer, int startAddress, int size, bool debugMode)
        {
            bool success = false;
            byte[] partialBuffer;
            
            try
            {
                if (serial.IsOpen)
                {
                    if (debugMode)
                    {
                        GetFnxInDebugMode();
                    }
                    
                    if (size < 2048)
                    {
                        partialBuffer = PreparePacket2Read(startAddress, size);
                        Array.Copy(partialBuffer, 0, buffer, 0, size);
                        UploadProgressBar.Increment(size);
                    }
                    else
                    {
                        int BufferSize = 2048;
                        int Loop = size / BufferSize;

                        for (int j = 0; j < Loop; j++)
                        {
                            partialBuffer = PreparePacket2Read(startAddress, BufferSize);
                            Array.Copy(partialBuffer, 0, buffer, j * BufferSize, BufferSize);
                            partialBuffer = null;
                            startAddress += BufferSize;   // Advance the Pointer to the next location where to write Data in the Foenix
                            UploadProgressBar.Increment(BufferSize);
                        }
                        BufferSize = (size % BufferSize);
                        if (BufferSize > 0)
                        {
                            partialBuffer = PreparePacket2Read(startAddress, BufferSize);
                            Array.Copy(partialBuffer, 0, buffer, size - BufferSize, BufferSize);
                            UploadProgressBar.Increment(BufferSize);
                        }
                    }

                    if (debugMode)
                    {
                        ExitFnxDebugMode();
                    }
                    success = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Fetch Data Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return success;
        }

        private void AddressTextBox_TextChanged(object sender, EventArgs e)
        {
            int uploadSize = GetTransmissionSize();
            SendBinaryButton.Enabled = uploadSize > 0 && !ConnectButton.Visible;
        }

        private void BlockAddressTextBox_Leave(object sender, EventArgs e)
        {
            TextBox tb = (TextBox)sender;
            string item = tb.Text.Replace(":", "");
            if (item.Length > 0)
            {
                int n = Convert.ToInt32(item, 16);
                String value = n.ToString("X6");
                tb.Text = value.Substring(0, 2) + ":" + value.Substring(2);
            }
        }

        public void GetFnxInDebugMode()
        {
            byte[] commandBuffer = new byte[8];
            commandBuffer[0] = 0x55;   // Header
            commandBuffer[1] = 0x80;   // GetFNXinDebugMode
            commandBuffer[2] = 0x00;
            commandBuffer[3] = 0x00;
            commandBuffer[4] = 0x00;
            commandBuffer[5] = 0x00;
            commandBuffer[6] = 0x00;
            commandBuffer[7] = Checksum(commandBuffer, 7);
            SendMessage(commandBuffer, null);
        }

        public void ExitFnxDebugMode()
        {
            byte[] commandBuffer = new byte[8];
            commandBuffer[0] = 0x55;   // Header
            commandBuffer[1] = 0x81;   // ExitFNXinDebugMode
            commandBuffer[2] = 0x00;
            commandBuffer[3] = 0x00;
            commandBuffer[4] = 0x00;
            commandBuffer[5] = 0x00;
            commandBuffer[6] = 0x00;
            commandBuffer[7] = Checksum(commandBuffer, 7);
            SendMessage(commandBuffer, null);
        }

        /*
        CMD = 0x00 Read Memory Block
        CMD = 0x01 Write Memory Block
        CMD = 0x0E GetFNXinDebugMode - Stop Processor and put Bus in Tri-State - That needs to be done before any transaction.
        CMD = 0x0F 
         */
        public void PreparePacket2Write(byte[] buffer, int FNXMemPointer, int FilePointer, int Size)
        {
            // Maximum transmission size is 8192
            if (Size > 8192)
            {
                if (boardVersion != BoardVersion.RevJr)
                {
                    Size = 8192;
                    Console.WriteLine("PreparePacket2Write: output truncated to 8K bytes.");
                }
                else
                {
                    Size = 2048;
                    Console.WriteLine("PreparePacket2Write: output truncated to 2K bytes.");
                }
            }

            byte[] commandBuffer = new byte[8 + Size];
            commandBuffer[0] = 0x55;   // Header
            commandBuffer[1] = 0x01;   // Write 2 Memory
            commandBuffer[2] = (byte)((FNXMemPointer >> 16) & 0xFF); // (H)24Bit Addy - Where to Store the Data
            commandBuffer[3] = (byte)((FNXMemPointer >> 8) & 0xFF);  // (M)24Bit Addy - Where to Store the Data
            commandBuffer[4] = (byte)(FNXMemPointer & 0xFF);         // (L)24Bit Addy - Where to Store the Data
            commandBuffer[5] = (byte)((Size >> 8) & 0xFF);           // (H)16Bit Size - How many bytes to Store (Max 8Kbytes for now - 2K for Junior)
            commandBuffer[6] = (byte)(Size & 0xFF);                  // (L)16Bit Size - How many bytes to Store (Max 8Kbytes for now - 2K for Junior)
            Array.Copy(buffer, FilePointer, commandBuffer, 7, Size);

            TxProcessLRC(commandBuffer);
            Console.WriteLine("Transmit Data LRC:" + TxLRC);
            //commandBuffer[Size + 7] = TxLRC;

            SendMessage(commandBuffer, null);   // Tx the requested Payload Size (Plus Header and LRC), No Payload to be received aside of the Status.
        }

        /**
         * address: the address to read from, in the machine
         * size: the number of bytes to read
         */
        public byte[] PreparePacket2Read(int address, int size)
        {
            if (size > 0)
            {
                byte[] commandBuffer = new byte[8];
                commandBuffer[0] = 0x55;   // Header
                commandBuffer[1] = 0x00;   // Command READ Memory
                commandBuffer[2] = (byte)(address >> 16); // Address Hi
                commandBuffer[3] = (byte)(address >> 8); // Address Med
                commandBuffer[4] = (byte)(address & 0xFF); //Address Lo
                commandBuffer[5] = (byte)(size >> 8); //Size HI
                commandBuffer[6] = (byte)(size & 0xFF); //Size LO
                commandBuffer[7] = Checksum(commandBuffer, 7);

                byte[] partialBuffer = new byte[size];
                SendMessage(commandBuffer, partialBuffer);
                return partialBuffer;
            }
            return null;
        }

        public void SendMessage(byte[] command, byte[] data, int delay = 0)
        {
            //            int dwStartTime = System.Environment.TickCount;
            byte byte_buffer;
            Stopwatch stopWatch = new Stopwatch();
            serial.Write(command, 0, command.Length);

            Stat0 = 0;
            Stat1 = 0;
            LRC = 0;
            
            if (delay > 2000)
            {
                serial.ReadTimeout = delay;
            }
            if (delay > 0)
            {
                long StartTime = System.DateTimeOffset.Now.ToUnixTimeMilliseconds();
                int roundTime = delay / 1000;
                string label = CountdownLabel.Text;
                do
                {
                    CountdownLabel.Text = label + " - " + roundTime + "s";
                    this.Update();
                    Thread.Sleep(1000);
                    roundTime--;
                }
                while (System.DateTimeOffset.Now.ToUnixTimeMilliseconds() - StartTime < delay);
                CountdownLabel.Text = label + " - Done!";
            }

            stopWatch.Start();
            do
            {
                byte_buffer = (byte)serial.ReadByte();
            }
            while (byte_buffer != 0xAA);
            stopWatch.Stop();
            TimeSpan tsReady = stopWatch.Elapsed;
            if (delay > 2000)
            {
                serial.ReadTimeout = 2000;
            }

            // reset the stop watch
            stopWatch.Reset();
            stopWatch.Start();
            if (byte_buffer == 0xAA)
            {
                Stat0 = (byte)serial.ReadByte();
                Stat1 = (byte)serial.ReadByte();
                if (data != null)
                {
                    serial.Read(data, 0, data.Length);
                }
                LRC = (byte)serial.ReadByte();
            }
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            Console.WriteLine("Ready: " + tsReady.Milliseconds + ", Receive Data LRC:" + RxLRC + ", Time: " + ts.Milliseconds + "ms");
            RxProcessLRC(data);
        }

        public int TxProcessLRC(byte[] buffer)
        {
            int i;
            TxLRC = 0;
            for (i = 0; i < buffer.Length; i++)
                TxLRC = (byte)(TxLRC ^ buffer[i]);
            return TxLRC;
        }

        public int RxProcessLRC(byte[] data)
        {
            int i;
            RxLRC = 0xAA;
            RxLRC = (byte)(RxLRC ^ Stat0);
            RxLRC = (byte)(RxLRC ^ Stat1);
            if (data != null)
            {
                for (i = 0; i < data.Length; i++)
                    RxLRC = (byte)(RxLRC ^ data[i]);
            }
            RxLRC = (byte)(RxLRC ^ LRC);
            return RxLRC;
        }

        private void UploaderWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }
    }
}
