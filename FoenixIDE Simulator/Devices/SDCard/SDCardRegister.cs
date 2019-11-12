using FoenixIDE.Simulator.Devices.SDCard;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace FoenixIDE.Simulator.Devices
{
    public class SDCardRegister: MemoryLocations.MemoryRAM
    {
        private SDCardCommand currentCommand = SDCardCommand.NONE;
        public bool isPresent = false;
        public delegate void SDCardInterruptEvent(SDCardInterrupt irq);
        public SDCardInterruptEvent sdCardIRQMethod;
        string filename = "";
        string spaces = "        ";
        List<string> dircontent = new List<string> ();
        string filedata = "";
        int filepos = -1;
        private string SDCardPath; // this will be null until a path is selected

        public SDCardRegister(int StartAddress, int Length): base(StartAddress, Length)
        {
        }

        public string GetSDCardPath()
        {
            return SDCardPath;
        }
        public void SetSDCardPath(string path)
        {
            SDCardPath = path;
        }

        public override byte ReadByte(int Address)
        {
            if (Address == 0 && currentCommand == SDCardCommand.RD_USB_DATA0)
            {
                if (filepos > -1 && filepos < filedata.Length)
                {
                    return (byte)filedata[filepos++];
                }
                filepos = 0;
            }
            return base.ReadByte(Address);
        }
        public override void WriteByte(int Address, byte Value)
        {
            data[Address] = Value;
            switch (Address)
            {
                case 0:
                    switch (currentCommand)
                    {
                        case SDCardCommand.CHECK_EXIST:
                            data[0] = (byte)~Value; // Return the complement
                            break;
                        case SDCardCommand.SET_USB_MODE:
                            if (isPresent)
                            {
                                data[0] = (byte)SDResponse.CMD_RET_SUCCESS;
                            }
                            else
                            {
                                data[0] = (byte)SDResponse.CMD_RET_ABORT;
                            }
                            break;
                        
                        case SDCardCommand.FILE_OPEN:
                            break;
                        case SDCardCommand.SET_FILE_NAME:
                            if (Value != 0)
                            {
                                filename += (Char)Value;
                            }
                            break;
                    }
                    break;
                case 1:
                    currentCommand = (SDCardCommand)Value;
                    switch (currentCommand)
                    {
                        case SDCardCommand.DISK_MOUNT:
                            // Set the interrupt
                            sdCardIRQMethod?.Invoke(SDCardInterrupt.USB_INT_SUCCESS);
                            break;
                        case SDCardCommand.SET_FILE_NAME:
                            //sdCardIRQMethod?.Invoke(SDCardInterrupt.USB_INT_SUCCESS);
                            filename = "";
                            break;
                        case SDCardCommand.FILE_OPEN:
                            sdCardIRQMethod?.Invoke(SDCardInterrupt.USB_INT_DISK_READ);
                            ReadFile(filename);
                            break;
                        case SDCardCommand.FILE_CLOSE:
                            sdCardIRQMethod?.Invoke(SDCardInterrupt.USB_INT_DISK_READ);
                            break;
                        case SDCardCommand.RD_USB_DATA0:
                            filedata = dircontent[0];
                            dircontent.RemoveAt(0);
                            data[0] = (byte)filedata.Length;
                            filepos = -1;
                            break;
                        case SDCardCommand.FILE_ENUM_GO:
                            if (dircontent.Count > 0)
                            {
                                sdCardIRQMethod?.Invoke(SDCardInterrupt.USB_INT_DISK_READ);
                            }
                            else
                            {
                                sdCardIRQMethod?.Invoke(SDCardInterrupt.ERR_MISS_FIL);
                            }
                            filepos = -1;
                            break;
                    }
                    break;
                case 9:
                    break;
            }
        }


        /* __________________________________________________________________________________________________________
         * | 00 - 07 | 08 - 0A |  	0B     |     0C    |     0D     | 0E  -  0F | 10  -  11 | 12 - 13|  14 - 15 | 16 - 17 | 18 - 19 |   1A - 1B   |  1C  -  1F |
         * |Filename |Extension|File attrib|User attrib|First ch del|Create time|Create date|Owner ID|Acc rights|Mod. time|Mod. date|Start cluster|  File size |
         * ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
         */
        private void ReadFile(string name)
        {
            if (name.Contains("."))
            {

            }
            else
            {
                name = name.Replace("/", "");
                dircontent.Clear();
                string[] dirs = Directory.GetDirectories(SDCardPath, name, SearchOption.TopDirectoryOnly);
                string[] files = Directory.GetFiles(SDCardPath, name, SearchOption.TopDirectoryOnly);
                foreach (string dir in dirs)
                {
                    dircontent.Add(ShortFilename(dir.Replace(SDCardPath + "\\","")) + (char)(byte)FileAttributes.Directory + spaces + spaces + "\0\0\0\0");
                }
                foreach (string file in files)
                {
                    int size = (int)new FileInfo(file).Length;
                    byte[] sizeB = BitConverter.GetBytes(size);
                    UTF8Encoding utf8 = new UTF8Encoding();
                    
                    dircontent.Add(ShortFilename(file.Replace(SDCardPath + "\\", "")) + (char)(byte)FileAttributes.Archive + spaces + spaces + utf8.GetString(sizeB));
                }
            }
        }

        private string ShortFilename(string longname)
        {
            int pos = longname.IndexOf('.');
            if (pos > 0)
            {
                string filename = longname.Substring(0, pos);
                string extension = longname.Substring(pos+1);
                if (filename.Length > 8)
                {
                    filename = filename.Substring(0, 6) + "~1";
                }
                filename += spaces.Substring(0, 8 - filename.Length);
                if (extension.Length > 3)
                {
                    extension = extension.Substring(0, 3);
                }
                extension += spaces.Substring(0,3 - extension.Length);
                return filename.ToUpper() + extension.ToUpper();
            }
            else
            {
                string filename = longname;
                if (filename.Length > 8)
                {
                    filename = longname.Substring(0, 6) + "~1";
                }
                filename += spaces.Substring(0,8-filename.Length);
                return filename.ToUpper() + "   ";
            }
        }
    }
}
