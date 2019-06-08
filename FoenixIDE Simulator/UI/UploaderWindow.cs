﻿using FoenixIDE.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public IMappable Memory = null;

        SerialPort serial = new SerialPort();
        private Queue<byte> recievedData = new Queue<byte>();

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
                if (Path.GetExtension(filename).ToUpper().Equals(".BIN"))
                {


                    FileInfo f = new FileInfo(filename);
                    flen = f.Length;
                    
                }
                else
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
                DefaultExt = ".bin",
                Filter = "Binary documents|*.bin|Hex documents|*.hex",
                Title = "Upload to the C256 Foenix"
            };

            // Load content of file in a TextBlock
            if (openFileDlg.ShowDialog() == DialogResult.OK)
            {
                string extension = Path.GetExtension(openFileDlg.FileName);
                // Display the file name
                FileNameTextBox.Text = openFileDlg.FileName;
                C256DestAddress.Enabled = extension.ToUpper().Equals(".BIN");
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
                C256DestAddress.Enabled = (transmissionSize > 0 || BlockSendRadio.Checked) && extension.Equals(".BIN");
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

            int transmissionSize = GetTransmissionSize();
            UploadProgressBar.Maximum = transmissionSize;
            UploadProgressBar.Value = 0;
            UploadProgressBar.Visible = true;
            
            if (SendFileRadio.Checked)
            {
                if (Path.GetExtension(FileNameTextBox.Text).ToUpper().Equals(".BIN"))
                {
                    //byte[] DataBuffer = new byte[transmissionSize];  // Maximum 2 MB, example from $0 to $1F:FFFF.
                    // Read the bytes and put them in the buffer
                    byte[] DataBuffer = System.IO.File.ReadAllBytes(FileNameTextBox.Text);
                    int FnxAddressPtr = int.Parse(C256DestAddress.Text.Replace(":", ""), System.Globalization.NumberStyles.AllowHexSpecifier);
                    Console.WriteLine("Starting Address: " + FnxAddressPtr);
                    Console.WriteLine("Size of File: " + transmissionSize);
                    SendData(DataBuffer, FnxAddressPtr, transmissionSize, DebugModeCheckbox.Checked);
                }
                else
                {
                    if (serial.IsOpen)
                    {
                        // Get into Debug mode (Reset the CPU and keep it in that state and Gavin will take control of the bus)
                        if (DebugModeCheckbox.Checked)
                        {
                            GetFnxInDebugMode();
                        }

                        // If send HEX files, each time we encounter a "bank" change - record 04 - send a new data block
                        string[] lines = System.IO.File.ReadAllLines(FileNameTextBox.Text);
                        int bank = 0;
                        int address = 0;
                        byte[] pageFF = new byte[256];
                        bool resetVector = false;
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
                                            DataBuffer[i/2] = (byte)HexFile.GetByte(data, i, 1);
                                        }
                                        PreparePacket2Write(DataBuffer, bank + address, 0, length);
                                        if (bank + address >= 0xFF00 && (bank + address) < 0xFFFF)
                                        {
                                            Array.Copy(DataBuffer, 0, pageFF, bank+address - 0xFF00, length);
                                            resetVector = true;
                                        } else if (bank + address >= 0x18_FF00 && (bank + address) < 0x18_FFFF)
                                        {
                                            Array.Copy(DataBuffer, 0, pageFF, bank + address - 0x18_FF00, length);
                                            resetVector = true;
                                        }
                                        UploadProgressBar.Increment(length);

                                        break;

                                    case "04":
                                        bank = HexFile.GetByte(data, 0, 2) << 16;
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
                            // The Loading of the File is Done, Reset the FNX and Get out of Debug Mode
                            ExitFnxDebugMode();
                        }
                        MessageBox.Show("Transfer Done! System Reset!", "Send Binary Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            else if (BlockSendRadio.Checked)
            {
                int blockAddress = Convert.ToInt32(EmuSrcAddress.Text.Replace(":",""), 16);
                // Read the data directly from emulator memory
                int offset = 0;
                int FnxAddressPtr = int.Parse(C256DestAddress.Text.Replace(":", ""), System.Globalization.NumberStyles.AllowHexSpecifier);
                byte[] DataBuffer = new byte[transmissionSize];  // Maximum 2 MB, example from $0 to $1F:FFFF.
                for (int start = blockAddress; start < blockAddress + transmissionSize; start++)
                {
                    DataBuffer[offset++] = Memory.ReadByte(start);
                }
                SendData(DataBuffer, FnxAddressPtr, transmissionSize, DebugModeCheckbox.Checked);
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
                    tempMem.Show();
                }
            }
            
            UploadProgressBar.Visible = false;
            SendBinaryButton.Enabled = true;
            DisconnectButton.Enabled = true;
        }

        private void SendData(byte[] buffer, int startAddress, int size, bool debugMode)
        {
            try
            {
                if (serial.IsOpen)
                {
                    // Get into Debug mode (Reset the CPU and keep it in that state and Gavin will take control of the bus)
                    if (debugMode)
                    {
                        GetFnxInDebugMode();
                    }
                    // Now's let's transfer the code
                    if (size <= 2048)
                    {
                        // DataBuffer = The buffer where the loaded Binary File resides
                        // FnxAddressPtr = Pointer where to put the Data in the Fnx
                        // i = Pointer Inside the data buffer
                        // Size_Of_File = Size of the Payload we want to transfer which ought to be smaller than 8192
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
                            offset = offset + BufferSize;   // Advance the Pointer to the next location where to write Data in the Foenix
                            UploadProgressBar.Increment(BufferSize);
                        }
                        BufferSize = (size % BufferSize);
                        if (BufferSize > 0)
                        {
                            PreparePacket2Write(buffer, offset, size - BufferSize, BufferSize);
                            UploadProgressBar.Increment(BufferSize);
                        }
                    }

                    if (debugMode)
                    {
                        // Update the Reset Vectors from the Binary Files Considering that the Files Keeps the Vector @ $00:FF00
                        if (startAddress < 0xFF00 && (startAddress + buffer.Length) > 0xFFFF || (startAddress == 0x18_0000 && buffer.Length > 0xFFFF))
                        {
                            PreparePacket2Write(buffer, 0x00FF00, 0x00FF00, 256);
                        }

                        // The Loading of the File is Done, Reset the FNX and Get out of Debug Mode
                        ExitFnxDebugMode();
                    }

                    MessageBox.Show("Transfer Done! System Reset!", "Send Binary Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                        PreparePacket2Read(buffer, startAddress, 0, size);
                        UploadProgressBar.Increment(size);
                    }
                    else
                    {
                        int BufferSize = 2048;
                        int Loop = size / BufferSize;

                        for (int j = 0; j < Loop; j++)
                        {
                            PreparePacket2Read(buffer, startAddress, j * BufferSize, BufferSize);
                            startAddress += BufferSize;   // Advance the Pointer to the next location where to write Data in the Foenix
                            UploadProgressBar.Increment(BufferSize);
                        }
                        BufferSize = (size % BufferSize);
                        if (BufferSize > 0)
                        {
                            PreparePacket2Read(buffer, startAddress, size - BufferSize, BufferSize);
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
            commandBuffer[7] = 0xD5;
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
            commandBuffer[7] = 0xD4;
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
                Size = 8192;

            byte[] commandBuffer = new byte[8 + Size];
            commandBuffer[0] = 0x55;   // Header
            commandBuffer[1] = 0x01;   // Write 2 Memory
            commandBuffer[2] = (byte)((FNXMemPointer >> 16) & 0xFF); // (H)24Bit Addy - Where to Store the Data
            commandBuffer[3] = (byte)((FNXMemPointer >> 8) & 0xFF);  // (M)24Bit Addy - Where to Store the Data
            commandBuffer[4] = (byte)(FNXMemPointer & 0xFF);         // (L)24Bit Addy - Where to Store the Data
            commandBuffer[5] = (byte)((Size >> 8) & 0xFF);           // (H)16Bit Size - How many bytes to Store (Max 8Kbytes 4 Now)
            commandBuffer[6] = (byte)(Size & 0xFF);                  // (L)16Bit Size - How many bytes to Store (Max 8Kbytes 4 Now)
            Array.Copy(buffer, FilePointer, commandBuffer, 7, Size);

            TxProcessLRC(commandBuffer);
            Console.WriteLine("Transmit Data LRC:" + TxLRC);
            //commandBuffer[Size + 7] = TxLRC;

            SendMessage(commandBuffer, null);   // Tx the requested Payload Size (Plus Header and LRC), No Payload to be received aside of the Status.
        }

        public void PreparePacket2Read(byte[] receiveBuffer, int address, int offset, int size)
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
                commandBuffer[7] = XorCheck(commandBuffer);

                byte[] partialBuffer = new byte[size];
                SendMessage(commandBuffer, partialBuffer);
                Array.Copy(partialBuffer, 0, receiveBuffer, offset, size);
            }
        }

        private byte XorCheck(byte[] buffer)
        {
            byte check = buffer[0];
            for (int i = 1; i < buffer.Length; i++)
            {
                check ^= buffer[i];
            }
            return check;
        }
        public void SendMessage(byte[] command, byte[] data)
        {
            //            int dwStartTime = System.Environment.TickCount;
            byte byte_buffer;

            serial.Write(command, 0, command.Length);

            Stat0 = 0;
            Stat1 = 0;
            LRC = 0;

            do
            {
                byte_buffer = (byte)serial.ReadByte();
            }
            while (byte_buffer != 0xAA);


            if (byte_buffer == 0xAA)
            {
                Stat0 = (byte)serial.ReadByte();
                Stat1 = (byte)serial.ReadByte();
                if (data != null)
                {
                    for (int i = 0; i < data.Length; i++)
                    {
                        data[i] = (byte)serial.ReadByte();
                    }
                }
                LRC = (byte)serial.ReadByte();
            }

            RxProcessLRC(data);
            Console.WriteLine("Receive Data LRC:" + RxLRC);
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
    }
}
