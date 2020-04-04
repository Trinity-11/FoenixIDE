using FoenixIDE.MemoryLocations;
using FoenixIDE.Simulator.Devices;
using FoenixIDE.Simulator.FileFormat;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace FoenixIDE.Simulator.FileFormat
{
    class FoeniXmlFile
    {
        private ResourceChecker Resources;
        private const int PHRASE_LENGTH = 16;
        private Processor.Breakpoints BreakPoints;
        private SortedList<int, string> Labels;
        private MemoryRAM ram;
        public BoardVersion Version = BoardVersion.RevB;

        private FoeniXmlFile() { }

        public FoeniXmlFile(MemoryRAM ram, ResourceChecker resources, Processor.Breakpoints breakpoints, SortedList<int, string> labels)
        {
            this.ram = ram;
            this.Resources = resources;
            this.BreakPoints = breakpoints;
            this.Labels = labels;
        }
        public void Write(String filename, bool compact)
        {
            XmlWriter xmlWriter = XmlWriter.Create(filename);
            xmlWriter.WriteStartDocument();
            xmlWriter.WriteRaw("\r");
            xmlWriter.WriteComment("Export of FoenixIDE for C256.  All values are in hexadecimal form");
            xmlWriter.WriteRaw("\r");


            xmlWriter.WriteStartElement("project");
            xmlWriter.WriteAttributeString("version", Version.ToString());
            xmlWriter.WriteRaw("\r");

            // Write resources
            xmlWriter.WriteStartElement("resources");
            xmlWriter.WriteRaw("\r");
            foreach (ResourceChecker.Resource res in Resources.Items)
            {
                xmlWriter.WriteStartElement("resource");
                xmlWriter.WriteAttributeString("name", res.Name);
                xmlWriter.WriteAttributeString("source", res.SourceFile);
                xmlWriter.WriteAttributeString("start-address", res.StartAddress.ToString("X6"));
                xmlWriter.WriteAttributeString("length", res.Length.ToString("X"));
                xmlWriter.WriteEndElement();  // end resource
                xmlWriter.WriteRaw("\r");
            }
            xmlWriter.WriteEndElement(); // end resources
            xmlWriter.WriteRaw("\r");

            // Write breakpoints
            xmlWriter.WriteStartElement("breakpoints");
            xmlWriter.WriteRaw("\r");
            foreach (string bp in BreakPoints.Values)
            {
                xmlWriter.WriteStartElement("breakpoint");
                xmlWriter.WriteAttributeString("address", bp);
                xmlWriter.WriteEndElement();  // end resource
                xmlWriter.WriteRaw("\r");
            }
            xmlWriter.WriteEndElement(); // end breakpoints
            xmlWriter.WriteRaw("\r");

            // Write labels
            xmlWriter.WriteStartElement("labels");
            xmlWriter.WriteRaw("\r");
            foreach (KeyValuePair<int, string> kvp in Labels)
            {
                xmlWriter.WriteStartElement("label");
                xmlWriter.WriteAttributeString("address", kvp.Key.ToString("X6"));
                xmlWriter.WriteAttributeString("name", kvp.Value);
                xmlWriter.WriteEndElement();  // end resource
                xmlWriter.WriteRaw("\r");
            }
            xmlWriter.WriteEndElement(); // end breakpoints
            xmlWriter.WriteRaw("\r");

            // Write pages
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
            for (int i = 0; i < ram.Length; i = i + 256)
            {
                if (PageChecksum(i) != 0)
                {
                    WriteData(i, xmlWriter, compact);
                }
            }

            //for (int i = 0xAF_0000; i < 0xF0_0000; i = i + 256)
            //{
            //    if (PageChecksum(i) != 0)
            //    {
            //        WriteData(i, xmlWriter, compact);
            //    }
            //}
            xmlWriter.WriteEndElement(); // end pages
            xmlWriter.WriteEndElement(); // end project

            xmlWriter.WriteEndDocument();
            xmlWriter.Close();
        }

        private void WriteData(int startAddress, XmlWriter writer, bool compact)
        {
            writer.WriteStartElement("page");
            writer.WriteAttributeString("start-address", "$" + startAddress.ToString("X6"));
            writer.WriteAttributeString("bank", "$" + startAddress.ToString("X6").Substring(0, 2));
            writer.WriteRaw("\r");

            // Write PHRASE_LENGTH bytes per data line
            for (int i = 0; i < 256; i = i + PHRASE_LENGTH)
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
                for (int i = 0; i < PHRASE_LENGTH; i++)
                {
                    writer.WriteString(ram.ReadByte(startAddress + i).ToString("X2") + " ");
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
                sum += ram.ReadByte(startAddress + i);
            }
            return sum;
        }

        // Sum PHRASE_LENGTH bytes
        private int PhraseChecksum(int startAddress)
        {
            int sum = 0;
            for (int i = 0; i < PHRASE_LENGTH; i++)
            {
                sum += ram.ReadByte(startAddress + i);
            }
            return sum;
        }

        /*
         * Read a file into memory
         */
        public void Load(String filename)
        {
            XmlReader reader = XmlReader.Create(filename);
            Version = BoardVersion.RevB;
            if (Resources == null)
            {
                Resources = new ResourceChecker();
            }
            else
            {
                Resources.Clear();
            }
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    if (reader.Name.Equals("data"))
                    {
                        LoadMemory(reader.GetAttribute("address"), reader.ReadElementContentAsString());
                        continue;
                    }
                    if (reader.Name.Equals("resource"))
                    {
                        ResourceChecker.Resource res = new ResourceChecker.Resource
                        {
                            Name = reader.GetAttribute("name"),
                            SourceFile = reader.GetAttribute("source"),
                            StartAddress = Convert.ToInt32(reader.GetAttribute("start-address"), 16),
                            Length = Convert.ToInt32(reader.GetAttribute("length"), 16)
                        };
                        Resources.Add(res);
                        continue;
                    }
                    if (reader.Name.Equals("label"))
                    {
                        int address = Convert.ToInt32(reader.GetAttribute("address"), 16);
                        string name = reader.GetAttribute("name");
                        Labels.Add(address, name);
                        continue;
                    }
                    if (reader.Name.Equals("breakpoint"))
                    {
                        string address = reader.GetAttribute("address");
                        BreakPoints.Add(address);
                        continue;
                    }
                    if (reader.Name.Equals("project"))
                    {
                        String version = reader.GetAttribute("version");
                        if (version != null)
                        {
                            Enum.TryParse<BoardVersion>(version, out Version);
                        }
                        continue;
                    }
                }
            }
            reader.Close();
        }

        public void LoadMemory(String address, String values)
        {
            int addr = Convert.ToInt32(address.Replace("$",""), 16);
            //Each byte is written as 3 characters (2 Hex and a space)
            if (values.Length % 3 == 0)
            {
                for (int i = 0; i < values.Length / 3; i++)
                {
                    ram.WriteByte(addr++, Convert.ToByte(values.Substring(i * 3, 2), 16));
                }
            }
        }
    }
}
