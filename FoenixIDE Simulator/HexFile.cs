using System;

namespace Nu64
{
    public class HexFile
    {
        int bank = 0;
        int address = 0;

        public HexFile()
        {
        }

        public HexFile(IMappable memory, string HexFilename)
        {
            Load(memory, HexFilename);
        }

        public void Load(IMappable memory, string Filename)
        {
            if (!System.IO.File.Exists(Filename))
                throw new System.IO.FileNotFoundException("Could not find Hex file \"" + Filename + "\"");

            string[] lines = System.IO.File.ReadAllLines(Filename);

            foreach (string l in lines)
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
                            memory.WriteByte(bank + address, (byte)b);
                            address++;
                        }
                        break;

                    // end of file - just ignore
                    case "01":
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
        }

        private int GetByte(string data, int startPos, int bytes)
        {
            return Convert.ToInt32(data.Substring(startPos, bytes * 2), 16);
        }
    }
}