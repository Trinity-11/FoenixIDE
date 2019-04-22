using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace FoenixIDE.Common
{
    class FoenixmlFile
    {
        private IMappable Memory;

        private FoenixmlFile() { }

        public FoenixmlFile(IMappable memory)
        {
            this.Memory = memory;
        }
        public void Write(String filename, bool compact)
        {
            XmlWriter xmlWriter = XmlWriter.Create(filename);
            xmlWriter.WriteStartDocument();
            xmlWriter.WriteRaw("\r");
            xmlWriter.WriteComment("Export of FoenixIDE for C256.  All values are in hexadecimal form");
            xmlWriter.WriteRaw("\r");
            xmlWriter.WriteStartElement("pages");
            if (compact)
            {
                xmlWriter.WriteAttributeString("format", "compact");
            }
            else
            {
                xmlWriter.WriteAttributeString("format", "full");
            }

            xmlWriter.WriteRaw("\r");

            // We don't need to scan $FFFF pages, only scan the ones we know are gettings used
            // Scan each of the banks and pages and save to an XML file
            // If a page is blank, don't export it.
            for (int i = 0; i < 0x200000; i = i + 256)
            {
                if (PageChecksum(i) != 0)
                {
                    WriteData(i, xmlWriter, compact);
                }
            }

            for (int i = 0xAF_0000; i < 0xF0_0000; i = i + 256)
            {
                if (PageChecksum(i) != 0)
                {
                    WriteData(i, xmlWriter, compact);
                }
            }
            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndDocument();
            xmlWriter.Close();
        }

        private void WriteData(int startAddress, XmlWriter writer, bool compact)
        {
            writer.WriteStartElement("page");
            writer.WriteAttributeString("start-address", "$" + startAddress.ToString("X6"));
            writer.WriteAttributeString("bank", "$" + startAddress.ToString("X6").Substring(0, 2));
            writer.WriteRaw("\r");

            // Write 8 bytes per data line
            for (int i = 0; i < 256; i = i + 8)
            {
                WritePhrase(startAddress + i, writer, compact);
            }
            writer.WriteEndElement();
            writer.WriteRaw("\r");
        }

        // Only write a phrase if the bytes are non-zero
        private void WritePhrase(int startAddress, XmlWriter writer, bool compact)
        {
            if (PhraseChecksum(startAddress) == 0 && !compact || PhraseChecksum(startAddress) != 0)
            {
                writer.WriteStartElement("data");
                writer.WriteAttributeString("address", "$" + (startAddress).ToString("X6"));
                for (int i = 0; i < 8; i++)
                {
                    writer.WriteString(Memory.ReadByte(startAddress + i).ToString("X2") + " ");
                }
                writer.WriteEndElement();
                writer.WriteRaw("\r");
            }
        }

        // Sum 256 bytes
        private int PageChecksum(int startAddress)
        {
            int sum = 0;
            for (int i = 0; i < 255; i++)
            {
                sum += Memory.ReadByte(startAddress + i);
            }
            return sum;
        }

        // Sum 8 bytes
        private int PhraseChecksum(int startAddress)
        {
            int sum = 0;
            for (int i = 0; i < 8; i++)
            {
                sum += Memory.ReadByte(startAddress + i);
            }
            return sum;
        }

        /*
         * Read a file into memory
         */
        public void Load(String filename)
        {
            XmlReader reader = XmlReader.Create(filename);
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    if (reader.Name.Equals("data"))
                    {
                        LoadMemory(reader.GetAttribute("address"), reader.ReadElementContentAsString());
                    }
                }
            }
        }

        public void LoadMemory(String address, String values)
        {
            int addr = Convert.ToInt32(address.Replace("$",""), 16);
            //Ensure we have 8 values
            if (values.Length == 24)
            {
                for (int i = 0; i < 8; i++)
                {
                    Memory.WriteByte(addr++, Convert.ToByte(values.Substring(i*3,2), 16));
                }
            }
        }
    }
}
