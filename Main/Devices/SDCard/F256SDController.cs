using System.Collections.Generic;

namespace FoenixIDE.Simulator.Devices
{
    class F256SDController : FakeFATSDCardDevice
    {
#if DEBUG
        int LoggingLevel;
#endif

        enum CommandOpType
        {
            CMD0__GO_IDLE_STATE = 0x40,
            CMD8__SEND_IF_COND = 0x48,
            CMD17__READ_SINGLE_BLOCK = 0x51,
            CMD24__WRITE_BLOCK = 0x58,
            ACMD41__SD_SEND_OP_COND = 0x69,
            CMD55__APP_CMD = 0x77,
            CMD58__READ_OCR = 0x7A
        }

        class Command
        {
            public List<byte> CmdBytes;
            public List<byte> CmdResponseBytes;

            public int WriteAddressInBytes;
            public List<byte> WriteBytes;
            public List<byte> DataResponseBytes;

            public bool ReportError;
        }
        Command CurrentCommand;

        public F256SDController(int StartAddress, int Length) : base(StartAddress, Length)
        {
#if DEBUG
            LoggingLevel = 1;
#endif
            rootEntryCount = 0;
            vfat = true;
        }

        public override void ResetMbrBootSector()
        {
            base.ResetMbrBootSector();
        }

        byte ReadCtrl()
        {
#if DEBUG
            if (LoggingLevel >= 3)
            {
                System.Diagnostics.Debug.WriteLine(string.Format("    ReadCtrl - returned {0:X2}", data[0]));
            }
#endif

            return data[0];
        }

        byte ReadData()
        {
            if (CurrentCommand != null)
            {                
                if (CurrentCommand.DataResponseBytes != null) // Look to send a data response
                {
                    data[1] = CurrentCommand.DataResponseBytes[0];
                    CurrentCommand.DataResponseBytes.RemoveAt(0);

                    if (CurrentCommand.DataResponseBytes.Count == 0)
                    {
                        CurrentCommand = null; // Command is done
                    }
                }                
                else if (CurrentCommand.CmdResponseBytes != null) // Look to send a command response
                {
                    data[1] = CurrentCommand.CmdResponseBytes[0];
                    CurrentCommand.CmdResponseBytes.RemoveAt(0);

                    if (CurrentCommand.CmdResponseBytes.Count == 0)
                    {
                        CurrentCommand.CmdResponseBytes = null;

                        if (CurrentCommand.CmdBytes[0] != (byte)CommandOpType.CMD24__WRITE_BLOCK)
                        {
                            CurrentCommand = null; // Command is done
                        }
                    }
                }
            }

#if DEBUG
            if (LoggingLevel >= 2)
            {
                System.Diagnostics.Debug.WriteLine(string.Format("    ReadData - returned {0:X2}", data[1]));
            }
#endif

            return data[1];
        }

        public override byte ReadByte(int Address)
        {
            if (Address == 0)
            {
                return ReadCtrl();
            }
            else if (Address == 1)
            {
                return ReadData();
            }
            else
            {
                return 0xFF;
            }
        }

        public void WriteCtrl(byte Value)
        {
#if DEBUG
            if (LoggingLevel >= 2)
            {
                System.Diagnostics.Debug.WriteLine(string.Format("WriteCtrl {0:X2}", Value));
            }
#endif
            data[0] = Value;
        }

        public void WriteData(byte Value)
        {
#if DEBUG
            if (LoggingLevel >= 2)
            {
                System.Diagnostics.Debug.WriteLine(string.Format("WriteData {0:X2}", Value));
            }
#endif

            if (CurrentCommand != null)
            {
                if (CurrentCommand.CmdBytes.Count == 7 && CurrentCommand.CmdBytes[0] == (byte)CommandOpType.CMD24__WRITE_BLOCK)
                {
                    if (CurrentCommand.WriteBytes == null) // Still waiting on start token
                    {
                        if (Value == 0xFE) // start token
                        {
                            CurrentCommand.WriteBytes = new List<byte>();
                        }
                    }
                    else // Already started
                    {
                        // Ignore CRC
                        if (CurrentCommand.WriteBytes.Count < 512)
                        {
                            CurrentCommand.WriteBytes.Add(Value);
                            if (CurrentCommand.WriteBytes.Count == 512) // Write is done
                            {
                                // Commit the write now
                                blockPtr = 0;
                                writeBlock = CurrentCommand.WriteBytes.ToArray();

                                WriteBlockImpl(CurrentCommand.WriteAddressInBytes); // Note this might not actually put anything in the filesystem.

                                // Need to send response token now
                                CurrentCommand.DataResponseBytes = new List<byte>();

                                CurrentCommand.DataResponseBytes.Add(5);
                            }
                        }
                    }
                }
                else if (CurrentCommand.CmdBytes.Count < 7)
                {
                    CurrentCommand.CmdBytes.Add(Value);

                    // If we've filled the command buffer, we can process the command and form responses
                    if (CurrentCommand.CmdBytes.Count == 7)
                    {
                        if (CurrentCommand.CmdBytes[0] == (byte)CommandOpType.CMD0__GO_IDLE_STATE)
                        {
#if DEBUG
                            if (LoggingLevel >= 1)
                            {
                                System.Diagnostics.Debug.WriteLine(string.Format("GO_IDLE_STATE {0:X2}{1:X2}{2:X2}{3:X2}", CurrentCommand.CmdBytes[1], CurrentCommand.CmdBytes[2], CurrentCommand.CmdBytes[3], CurrentCommand.CmdBytes[4]));
                            }
#endif

                            CurrentCommand.CmdResponseBytes = new List<byte>();
                            CurrentCommand.CmdResponseBytes.Add(1); // Single-byte response, indicates no error. R1 response
                        }
                        else if (CurrentCommand.CmdBytes[0] == (byte)CommandOpType.CMD8__SEND_IF_COND)
                        {
#if DEBUG
                            if (LoggingLevel >= 1)
                            {
                                System.Diagnostics.Debug.WriteLine(string.Format("SEND_IF_COND {0:X2}{1:X2}{2:X2}{3:X2}", CurrentCommand.CmdBytes[1], CurrentCommand.CmdBytes[2], CurrentCommand.CmdBytes[3], CurrentCommand.CmdBytes[4]));
                            }
#endif

                            CurrentCommand.CmdResponseBytes = new List<byte>();
                            CurrentCommand.CmdResponseBytes.Add(1); // Five-byte response, R7. Seems like only the first byte matters though
                            CurrentCommand.CmdResponseBytes.Add(0xFF);
                            CurrentCommand.CmdResponseBytes.Add(0xFF);
                            CurrentCommand.CmdResponseBytes.Add(0xFF);
                            CurrentCommand.CmdResponseBytes.Add(0xFF);
                        }
                        else if (CurrentCommand.CmdBytes[0] == (byte)CommandOpType.CMD55__APP_CMD)
                        {
#if DEBUG
                            if (LoggingLevel >= 1)
                            {
                                System.Diagnostics.Debug.WriteLine(string.Format("APP_CMD {0:X2}{1:X2}{2:X2}{3:X2}", CurrentCommand.CmdBytes[1], CurrentCommand.CmdBytes[2], CurrentCommand.CmdBytes[3], CurrentCommand.CmdBytes[4]));
                            }
#endif

                            CurrentCommand.CmdResponseBytes = new List<byte>();
                            CurrentCommand.CmdResponseBytes.Add(1); // Single-byte R1 response, indicates no error
                        }
                        else if (CurrentCommand.CmdBytes[0] == (byte)CommandOpType.ACMD41__SD_SEND_OP_COND)
                        {
#if DEBUG
                            if (LoggingLevel >= 1)
                            {
                                System.Diagnostics.Debug.WriteLine(string.Format("SD_SEND_OP_COND {0:X2}{1:X2}{2:X2}{3:X2}", CurrentCommand.CmdBytes[1], CurrentCommand.CmdBytes[2], CurrentCommand.CmdBytes[3], CurrentCommand.CmdBytes[4]));
                            }
#endif

                            CurrentCommand.CmdResponseBytes = new List<byte>();
                            CurrentCommand.CmdResponseBytes.Add(0); // Single-byte R1 response, indicates we don't support high capacity cards i guess
                        }
                        else if (CurrentCommand.CmdBytes[0] == (byte)CommandOpType.CMD58__READ_OCR)
                        {
#if DEBUG
                            if (LoggingLevel >= 1)
                            {
                                System.Diagnostics.Debug.WriteLine(string.Format("READ_OCR {0:X2}{1:X2}{2:X2}{3:X2}", CurrentCommand.CmdBytes[1], CurrentCommand.CmdBytes[2], CurrentCommand.CmdBytes[3], CurrentCommand.CmdBytes[4]));
                            }
#endif

                            CurrentCommand.CmdResponseBytes = new List<byte>();
                            CurrentCommand.CmdResponseBytes.Add(0x0); // Caller expects block addressing mode. R3 response
                            CurrentCommand.CmdResponseBytes.Add(0x40);
                            CurrentCommand.CmdResponseBytes.Add(0xFF);
                            CurrentCommand.CmdResponseBytes.Add(0xFF);
                            CurrentCommand.CmdResponseBytes.Add(0xFF);
                        }
                        else if (CurrentCommand.CmdBytes[0] == (byte)CommandOpType.CMD17__READ_SINGLE_BLOCK)
                        {
#if DEBUG
                            if (LoggingLevel >= 1)
                            {
                                System.Diagnostics.Debug.WriteLine(string.Format("READ_SINGLE_BLOCK {0:X2}{1:X2}{2:X2}{3:X2}", CurrentCommand.CmdBytes[1], CurrentCommand.CmdBytes[2], CurrentCommand.CmdBytes[3], CurrentCommand.CmdBytes[4]));
                            }
#endif

                            CurrentCommand.CmdResponseBytes = new List<byte>();
                            CurrentCommand.CmdResponseBytes.Add(0x1); // R1 response
                            CurrentCommand.CmdResponseBytes.Add(0xFE);  // start token

                            uint readAddress = 0;
                            readAddress |= CurrentCommand.CmdBytes[1];
                            readAddress <<= 8;
                            readAddress |= CurrentCommand.CmdBytes[2];
                            readAddress <<= 8;
                            readAddress |= CurrentCommand.CmdBytes[3];
                            readAddress <<= 8;
                            readAddress |= CurrentCommand.CmdBytes[4];

                            int readAddressBytes = (int)readAddress;
                            readAddressBytes *= 512;

                            GetReadBlock(readAddressBytes);

                            for (int i = blockPtr; i < blockPtr + 512; ++i)
                            {
                                if (readBlock != null)
                                {
                                    CurrentCommand.CmdResponseBytes.Add(readBlock[i]);
                                }
                                else
                                {
                                    CurrentCommand.CmdResponseBytes.Add(0);
                                }
                            }

                            // Then 16bit crc
                            CurrentCommand.CmdResponseBytes.Add(0x00);
                            CurrentCommand.CmdResponseBytes.Add(0x00);
                        }
                        else if (CurrentCommand.CmdBytes[0] == (byte)CommandOpType.CMD24__WRITE_BLOCK)
                        {
#if DEBUG
                            if (LoggingLevel >= 1)
                            {
                                System.Diagnostics.Debug.WriteLine(string.Format("WRITE_BLOCK {0:X2}{1:X2}{2:X2}{3:X2}", CurrentCommand.CmdBytes[1], CurrentCommand.CmdBytes[2], CurrentCommand.CmdBytes[3], CurrentCommand.CmdBytes[4]));
                            }
#endif

                            CurrentCommand.CmdResponseBytes = new List<byte>();
                            CurrentCommand.CmdResponseBytes.Add(0x0); // R1 response

                            uint writeAddress = 0;
                            writeAddress |= CurrentCommand.CmdBytes[1];
                            writeAddress <<= 8;
                            writeAddress |= CurrentCommand.CmdBytes[2];
                            writeAddress <<= 8;
                            writeAddress |= CurrentCommand.CmdBytes[3];
                            writeAddress <<= 8;
                            writeAddress |= CurrentCommand.CmdBytes[4];

                            int writeAddressBytes = (int)writeAddress;
                            writeAddressBytes *= 512;

                            CurrentCommand.WriteAddressInBytes = writeAddressBytes;
                        }
                    }

                    data[1] = Value;
                    data[0] &= 0x7F;
                    return;
                }
            }

            // Look to start a new command
            if (CurrentCommand == null)
            {
                // Check for the start of commands we recognize
                bool recognized = false;
                switch (Value)
                {
                    case (byte)CommandOpType.CMD0__GO_IDLE_STATE:
                    case (byte)CommandOpType.CMD8__SEND_IF_COND:
                    case (byte)CommandOpType.CMD55__APP_CMD:
                    case (byte)CommandOpType.ACMD41__SD_SEND_OP_COND:
                    case (byte)CommandOpType.CMD58__READ_OCR:
                    case (byte)CommandOpType.CMD17__READ_SINGLE_BLOCK:
                    case (byte)CommandOpType.CMD24__WRITE_BLOCK:
                        recognized = true;
                        break;
                }
                if (recognized)
                {
                    CurrentCommand = new Command();
                    CurrentCommand.CmdBytes = new List<byte>();
                    CurrentCommand.CmdBytes.Add(Value);
                }

#if DEBUG
                if (Value != 0xFF && !recognized)
                {
                    System.Diagnostics.Debugger.Break();
                }
#endif
            }

            // Side effect: clear SPI_BUSY to 0 to indicate we're immediately done
            data[1] = Value;
            data[0] &= 0x7F;
        }

        public override void WriteByte(int Address, byte Value)
        {
            if (Address == 0)
            {
                WriteCtrl(Value);
            }
            else if (Address == 1)
            {
                WriteData(Value);
            }
        }

        protected override bool ShouldPreparePlaceholderFileEntry(int page)
        {
            // HACK: the Jr kernel for some reason requests updates to the FAT entry for this page, but it has nothing to do with
            // the actual clusters the user is trying to write. In the long run we should try to understand why the kernel is
            // doing this and how it doesn't play nicely with the SD card-related bookkeeping we're doing.
            if (page == 1023)
                return false;

            return true; 
        }

        protected override void ReportError()
        {
#if DEBUG
            // TODO: Have reported errors be returned up as command codes. For now, this is a temporary measure to divert error handling
            // from the GabeSDController path, since it returns errors through its memory-mapped register where there isn't an equivalent
            // for this memory-mapped range.
            System.Diagnostics.Debugger.Break();
#endif
            if (CurrentCommand != null)
            {
                CurrentCommand.ReportError = true;
            }

        }
    }
}
