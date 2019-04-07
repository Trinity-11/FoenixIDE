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
        public static byte[] FileBuffer = new byte[1024 * 1024 * 2];   // Located between $00:0000 to $1F:FFFF
        public static int Size_Of_File = -1;

        //        public static byte[] FoenixFlash0Buffer = new byte[512 * 1024];     // Located between $F8:0000 to $FF:FFFF
        //        public static byte[] FoenixFlash1Buffer = new byte[512 * 1024];     // Located between $F0:0000 to $F7:FFFF
        public static byte[] TxSerialBuffer = new byte[1 + 1 + 3 + 2 + 8192 + 1];
        public static byte TxLRC = 0;
        public static byte RxLRC = 0;
        public static byte[] RxSerialBuffer = new byte[1 + 2 + 8192 + 1];
        public static byte Stat0 = 0;
        public static byte Stat1 = 0;
        public static byte LRC = 0;
        public static string[] ports;

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

        private void ConnectButton_Click(object sender, EventArgs e)
        {
            try
            {
                serial.PortName = COMPortComboBox.Items[COMPortComboBox.SelectedIndex].ToString();
                if (serial.PortName.StartsWith("COM"))
                {
                    serial.BaudRate = 115200;
                }
                serial.Open();
                // Enable all the button if the serial Port turns out to be the good one.
                BrowseFileButton.Enabled = true;
                LoadAddressTextBox.Enabled = true;
                SendBinaryButton.Enabled = (Size_Of_File != -1);
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

        /*
         * Let the user select a file from the file system and display it in a text box.
         */
        private void BrowseFileButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDlg = new OpenFileDialog();
            openFileDlg.DefaultExt = ".bin";
            openFileDlg.Filter = "Binary documents (.bin)|*.bin";

            // Set initial directory    
            //openFileDlg.InitialDirectory = @"C:\Temp\";

            // Load content of file in a TextBlock
            if (openFileDlg.ShowDialog() == DialogResult.OK)
            {
                // Display the file length in hex
                FileInfo f = new FileInfo(openFileDlg.FileName);
                Size_Of_File = (int)f.Length;
                String hexSize = Size_Of_File.ToString("X6");
                FileSizeResultLabel.Text = "$" + hexSize.Substring(0, 2) + ":" + hexSize.Substring(2);

                // Display the file name
                FileNameTextBox.Text = openFileDlg.FileName;

                // Read the bytes and put them in the buffer
                FileBuffer = System.IO.File.ReadAllBytes(openFileDlg.FileName);
                

                LoadAddressTextBox.Enabled = true;
                SendBinaryButton.Enabled = (Size_Of_File != -1);            
            }
        }

        private void SendBinaryButton_Click(object sender, EventArgs e)
        {
            SendBinaryButton.Enabled = false;
            UploadProgressBar.Visible = true;
            DisconnectButton.Enabled = false;
            int i, j = 0;
            int Loop = 0;
            int percentage = 0;

            UploadProgressBar.Maximum = Size_Of_File;
            UploadProgressBar.Value = 0;

            int FnxAddressPtr = int.Parse(LoadAddressTextBox.Text.Replace(":",""), System.Globalization.NumberStyles.AllowHexSpecifier);
            Console.WriteLine("Starting Address: " + FnxAddressPtr);
            Console.WriteLine("Size of File: " + Size_Of_File);
            try
            {
                if (serial.IsOpen)
                {
                    // Get into Debug mode (Reset the CPU and keep it in that state and Gavin will take control of the bus)
                    GetFnxInDebugMode();
                    // Now's let's transfer the code
                    if (Size_Of_File <= 2048)
                    {
                        i = 0;
                        // FileBuffer = The buffer where the loaded Binary File resides
                        // FnxAddressPtr = Pointer where to put the Data in the Fnx
                        // i = Pointer Inside the file Buffer
                        // Size_Of_File = Size of the Payload we want to transfer which ought to be smaller than 8192
                        PreparePacket2Write(FnxAddressPtr, i, Size_Of_File);
                        UploadProgressBar.Value = Size_Of_File;
                    }
                    else
                    {
                        int BufferSize = 2048;
                        Loop = Size_Of_File / BufferSize;
                        percentage = 80 / Loop;

                        for (j = 0; j < Loop; j++)
                        {
                            PreparePacket2Write(FnxAddressPtr, j * BufferSize, BufferSize);
                            FnxAddressPtr = FnxAddressPtr + BufferSize;   // Advance the Pointer to the next location where to write Data in the Foenix
                            UploadProgressBar.Value = (j + 1) * BufferSize;
                        }
                        BufferSize = (Size_Of_File % BufferSize);
                        PreparePacket2Write(FnxAddressPtr, Size_Of_File - BufferSize, BufferSize);
                    }

                    // Update the Reset Vectors from the Binary Files Considering that the Files Keeps the Vector @ $00:FF00
                    PreparePacket2Write(0x00FF00, 0x00FF00, 256);

                    // The Loading of the File is Done, Reset the FNX and Get out of Debug Mode
                    ExitFnxDebugMode();

                    MessageBox.Show("Transfer Done! - System Reseted!", "Send Binary Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Send Binary Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            UploadProgressBar.Visible = false;
            SendBinaryButton.Enabled = true;
            DisconnectButton.Enabled = true;
        }

        private void LoadAddressTextBox_TextChanged(object sender, EventArgs e)
        {
            string item = LoadAddressTextBox.Text.Replace(":","");
            int n = 0;
            if (!int.TryParse(item, System.Globalization.NumberStyles.HexNumber, System.Globalization.NumberFormatInfo.CurrentInfo, out n) &&
              item != String.Empty)
            {
                LoadAddressTextBox.Text = item.Remove(item.Length - 1, 1);
                LoadAddressTextBox.SelectionStart = LoadAddressTextBox.Text.Length;
            }
        }

        /*
        CMD = 0x00 Read Memory Block
        CMD = 0x01 Write Memory Block
        CMD = 0x0E GetFNXinDebugMode - Stop Processor and put Bus in Tri-State - That needs to be done before any transaction.
        CMD = 0x0F 
         */
        public void PreparePacket2Write(int FNXMemPointer, int FilePointer, int Size)
        {
            if (Size > 8192)
                Size = 8192;

            TxSerialBuffer[0] = 0x55;   // Header
            TxSerialBuffer[1] = 0x01;   // Write 2 Memory
            TxSerialBuffer[2] = (byte)((FNXMemPointer >> 16) & 0xFF); // (H)24Bit Addy - Where to Store the Data
            TxSerialBuffer[3] = (byte)((FNXMemPointer >> 8) & 0xFF);  // (M)24Bit Addy - Where to Store the Data
            TxSerialBuffer[4] = (byte)(FNXMemPointer & 0xFF);         // (L)24Bit Addy - Where to Store the Data
            TxSerialBuffer[5] = (byte)((Size >> 8) & 0xFF);           // (H)16Bit Size - How many bytes to Store (Max 8Kbytes 4 Now)
            TxSerialBuffer[6] = (byte)(Size & 0xFF);                  // (L)16Bit Size - How many bytes to Store (Max 8Kbytes 4 Now)
            Array.Copy(FileBuffer, FilePointer, TxSerialBuffer, 7, Size);

            TxProcessLRC((Size + 7));
            //TxSerialBuffer[Size + 7] = TxLRC;

            SendMessage(Size + 8, 0);   // Tx the requested Payload Size (Plus Header and LRC), No Payload to be received aside of the Status.
        }

        public void GetFnxInDebugMode()
        {
            TxSerialBuffer[0] = 0x55;   // Header
            TxSerialBuffer[1] = 0x80;   // GetFNXinDebugMode
            TxSerialBuffer[2] = 0x00;
            TxSerialBuffer[3] = 0x00;
            TxSerialBuffer[4] = 0x00;
            TxSerialBuffer[5] = 0x00;
            TxSerialBuffer[6] = 0x00;
            TxSerialBuffer[7] = 0xD5;
            SendMessage(8, 0);
        }

        public void ExitFnxDebugMode()
        {
            TxSerialBuffer[0] = 0x55;   // Header
            TxSerialBuffer[1] = 0x81;   // ExitFNXinDebugMode
            TxSerialBuffer[2] = 0x00;
            TxSerialBuffer[3] = 0x00;
            TxSerialBuffer[4] = 0x00;
            TxSerialBuffer[5] = 0x00;
            TxSerialBuffer[6] = 0x00;
            TxSerialBuffer[7] = 0xD4;
            SendMessage(8, 0);
        }


        public void ReadPacket_Test()
        {
            TxSerialBuffer[0] = 0x55;   // Header
            TxSerialBuffer[1] = 0x00;   // ExitFNXinDebugMode
            TxSerialBuffer[2] = 0x01;
            TxSerialBuffer[3] = 0x00;
            TxSerialBuffer[4] = 0x00;
            TxSerialBuffer[5] = 0x10;
            TxSerialBuffer[6] = 0x00;
            TxSerialBuffer[7] = 0x44;
            SendMessage(8, 0x1000);
        }


        public void SendMessage(int TxSize, int RxSize)
        {
            //            int dwStartTime = System.Environment.TickCount;
            int i;
            byte byte_buffer;

            serial.Write(TxSerialBuffer, 0, TxSize);

            Array.Clear(RxSerialBuffer, 0, RxSerialBuffer.Length);
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
                if (RxSize != 0)
                {
                    for (i = 0; i < RxSize; i++)
                    {
                        RxSerialBuffer[i] = (byte)serial.ReadByte();
                    }
                }
                LRC = (byte)serial.ReadByte();
            }

            RxProcessLRC(RxSize);
            Console.WriteLine("Recieve Data LRC:" + RxLRC);
        }



        public void TxProcessLRC(int Size)
        {
            int i;
            TxLRC = 0;
            for (i = 0; i < Size; i++)
                TxLRC = (byte)(TxLRC ^ TxSerialBuffer[i]);
        }

        public void RxProcessLRC(int Size)
        {
            int i;
            RxLRC = 0xAA;
            RxLRC = (byte)(RxLRC ^ Stat0);
            RxLRC = (byte)(RxLRC ^ Stat1);
            if (Size != 0)
            {
                for (i = 0; i < Size; i++)
                    RxLRC = (byte)(RxLRC ^ RxSerialBuffer[i]);
            }
            RxLRC = (byte)(RxLRC ^ LRC);
        }
    }
}
