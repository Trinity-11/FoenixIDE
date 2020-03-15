using FoenixIDE.MemoryLocations;
using System;
using System.Windows.Forms;

namespace FoenixIDE.Simulator.FileFormat
{
    public class HexFile
    {

        static public String Load(MemoryRAM ram, string Filename, int gabeAddressBank)
        {
            int bank = 0;
            int address = 0;
            String processedFileName = Filename;

            if (!System.IO.File.Exists(Filename))
            {
                OpenFileDialog f = new OpenFileDialog
                {
                    Title = "Select a kernel file",
                    Filter = "Hex Files|*.hex|All Files|*.*"
                };
                if (f.ShowDialog() == DialogResult.OK)
                {
                    processedFileName = f.FileName;
                }
                else
                {
                    return null;
                }
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
                            address = GetByte(offset, 0, 2);
                            for (int i = 0; i < data.Length; i += 2)
                            {
                                int b = GetByte(data, i, 1);
                                ram.WriteByte(bank + address, (byte)b);
                                // Copy bank $38 or $18 to page 0
                                if (bank == gabeAddressBank)
                                {
                                    ram.WriteByte(address, (byte)b);
                                }
                                address++;
                            }
                            break;

                        // end of file - just ignore
                        case "01":
                            break;

                        case "02":
                            bank = GetByte(data, 0, 2) * 16;
                            break;

                        // extended linear address 
                        // lower byte will populate the bank number. 
                        case "04":
                            bank = GetByte(data, 0, 2) << 16;
                            break;

                        // extended linear start address
                        // set the initial bank register value. Not used in the simulator.
                        case "05":
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
            return processedFileName;
        }

        // Read a two-character hex string into a byte
        static public int GetByte(string data, int startPos, int bytes)
        {
            return Convert.ToInt32(data.Substring(startPos, bytes * 2), 16);
        }
    }
}