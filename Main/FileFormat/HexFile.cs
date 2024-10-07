﻿using FoenixIDE.MemoryLocations;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace FoenixIDE.Simulator.FileFormat
{
    public class HexFile
    {

        static public int Load(MemoryRAM ram, FlashF256 romJr, string Filename, int gabeAddressBank, out List<int> blocks, out List<int> blockLengths)
        {
            int bank = 0;
            int addrCursor = 0;

            String processedFileName = Filename;
            blocks = new List<int>();
            blockLengths = new List<int>();
            int startAddress = -1;

            if (!System.IO.File.Exists(Filename))
            {
                return startAddress;
            }
                

            string[] lines = System.IO.File.ReadAllLines(processedFileName);

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
                        // data row. The next n bytes are data to be loaded into memory
                        case "00":
                            addrCursor = GetByte(offset, 0, 2);
                            
                            if (startAddress == -1 )
                            {
                                startAddress = bank + addrCursor;
                            }
                            // This is to address defect report #26 - https://github.com/Trinity-11/FoenixIDE/issues/26
                            // Check if the new address is consecutive with the previous one - if not, create a new block
                            if (!blocks.Contains(startAddress) || (bank + addrCursor) < startAddress)
                            {
                                startAddress = bank + addrCursor;
                                blocks.Add(startAddress);
                                blockLengths.Add(data.Length / 2);
                            }
                            else
                            {
                                int bl = blocks.IndexOf(startAddress);
                                blockLengths[bl] = (bank + addrCursor + data.Length / 2 - startAddress);
                            }
                            if (bank <= ram.Length)
                            {
                                for (int i = 0; i < data.Length; i += 2)
                                {
                                    int b = GetByte(data, i, 1);

                                    if (romJr != null && bank >= 0x08_0000 && bank <= 0x10_0000)
                                    {
                                        romJr.SetFlash(bank + addrCursor - 0x08_0000, (byte)b);
                                    }
                                    else
                                    {
                                        ram.WriteByte(bank + addrCursor, (byte)b);
                                    }

                                    // Copy bank $38 or $18 to page 0
                                    if (bank == gabeAddressBank)
                                    {
                                        ram.WriteByte(addrCursor, (byte)b);
                                    }
                                    addrCursor++;
                                }
                            }
                            
                            break;

                        // end of file - just ignore
                        case "01":
                            break;

                        case "02":
                            if (startAddress != -1)
                            {
                                startAddress = -1;
                            }
                            bank = GetByte(data, 0, 2) * 16;
                            break;

                        // extended linear address 
                        // lower byte will populate the bank number. 
                        case "04":
                            if (startAddress != -1)
                            {
                                startAddress = -1;
                            }
                            bank = GetByte(data, 0, 2) << 16;
                            break;

                        // set the start address
                        case "05":
                            startAddress = GetByte(data, 0, 4);
                            // If running on a F256, set the start address
                            if (romJr != null)
                            {
                                ram.WriteWord(0xFFFC, startAddress);
                            }
                            break;

                        default:
                            throw new NotImplementedException("Record type not implemented: " + rectype);
                    }
                }
                else
                {
                    MessageBox.Show("This doesn't appear to be an Intel Hex file.", "Error Loading Hex File", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                }
            }
            return startAddress;
        }

        // Read a two-character hex string into a byte
        static public int GetByte(string data, int startPos, int bytes)
        {
            return Convert.ToInt32(data.Substring(startPos, bytes * 2), 16);
        }
    }
}