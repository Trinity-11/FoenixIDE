using FoenixIDE.Simulator.Devices.SDCard;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace FoenixIDE.Simulator.Devices
{
    internal class ShortLongFileName
    {
        public string shortName;
        public string longName;
    }

    public class SDCardRegister: MemoryLocations.MemoryRAM
    {
        private SDCardCommand currentCommand = SDCardCommand.NONE;
        public bool isPresent = false;
        public delegate void SDCardInterruptEvent(SDCardInterrupt irq);
        public SDCardInterruptEvent sdCardIRQMethod;
        string filename = "";
        string fileToReadAsBytes = null;
        string spaces = "\0\0\0\0\0\0\0\0";
        string sdCurrentPath = "";

        List <ShortLongFileName> dircontent = new List<ShortLongFileName> ();
        int dirItem = 0;
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
                if (fileToReadAsBytes == null)
                {
                    if (filepos > -1 && filepos < filedata.Length)
                    {
                        return (byte)filedata[filepos++];
                    }
                    filepos = 0;
                }
                else
                {
                    // Return a byte from the file array buffer
                }
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
                            dirItem = 0;
                            break;
                        case SDCardCommand.FILE_CLOSE:
                            sdCardIRQMethod?.Invoke(SDCardInterrupt.USB_INT_DISK_READ);
                            break;
                        case SDCardCommand.RD_USB_DATA0:
                            if (fileToReadAsBytes == null)
                            {
                                filedata = dircontent[dirItem++].shortName;
                                data[0] = (byte)filedata.Length;
                                filepos = -1;
                            }
                            else
                            {
                                // I'm not sure what I'm supposed to write here - is this the file length?
                            }
                            break;
                        case SDCardCommand.FILE_ENUM_GO:
                            if (dirItem < dircontent.Count)
                            {
                                sdCardIRQMethod?.Invoke(SDCardInterrupt.USB_INT_DISK_READ);
                            }
                            else
                            {
                                sdCardIRQMethod?.Invoke(SDCardInterrupt.ERR_MISS_FIL);
                            }
                            filepos = -1;
                            break;
                        case SDCardCommand.GET_STATUS:
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
            if (name.Contains("*"))
            {
                fileToReadAsBytes = null;
                // Path is compounded, as long as "CLOSE" is not called
                if (name.StartsWith("/"))
                {
                    sdCurrentPath = name.Substring(1).Replace("/*", "");
                }
                else
                {
                    sdCurrentPath += name;
                }
                // Correct the path for Windows
                string rootPath = SDCardPath;
                if (sdCurrentPath.Length > 1)
                {
                    // we only store the name of the file, not the path
                    int lastSlash = sdCurrentPath.LastIndexOf("/");
                    if (lastSlash > 0)
                    {
                        sdCurrentPath = sdCurrentPath.Substring(lastSlash + 1);
                    }
                    ShortLongFileName slf = FindByShortName(sdCurrentPath);
                    if (slf != null)
                    {
                        rootPath = slf.longName;
                    }
                    else
                    {
                        rootPath = SDCardPath + "\\" + sdCurrentPath.Substring(0, sdCurrentPath.Length - 1);
                    }
                }
                dircontent.Clear();
                string[] dirs = Directory.GetDirectories(rootPath, "*", SearchOption.TopDirectoryOnly);
                string[] files = Directory.GetFiles(rootPath, "*", SearchOption.TopDirectoryOnly);

                // Add the parent folder only if the inital name is not /*
                if (!rootPath.Equals(SDCardPath))
                {
                    ShortLongFileName slf = new ShortLongFileName();
                    DirectoryInfo parent = Directory.GetParent(rootPath);
                    slf.longName = parent.ToString();
                    slf.shortName = ".." + spaces + spaces[0] + (char)(byte)FileAttributes.Directory + spaces + spaces + "\0\0\0\0";
                    dircontent.Add(slf);
                }
                foreach (string dir in dirs)
                {
                    ShortLongFileName slf = new ShortLongFileName();
                    slf.longName = dir;
                    slf.shortName = ShortFilename(dir.Substring(rootPath.Length)) + (char)(byte)FileAttributes.Directory + spaces + spaces + "\0\0\0\0";
                    dircontent.Add(slf);
                }
                foreach (string file in files)
                {
                    int size = (int)new FileInfo(file).Length;
                    byte[] sizeB = BitConverter.GetBytes(size);
                    
                    ShortLongFileName slf = new ShortLongFileName();
                    slf.longName = file;
                    slf.shortName = ShortFilename(file.Substring(rootPath.Length)) + (char)(byte)FileAttributes.Archive + spaces + spaces + System.Text.Encoding.Default.GetString(sizeB);
                    while (ListContains(dircontent, slf.shortName.Substring(0,11)))
                    {
                        if (slf.shortName.Substring(7, 1).Equals("\0"))
                        {
                            break;
                        }

                        int fileVal = Convert.ToInt32(slf.shortName.Substring(7, 1));
                        fileVal++;
                        slf.shortName = slf.shortName.Substring(0, 7) + fileVal + slf.shortName.Substring(8);
                        
                        
                    }
                    dircontent.Add(slf);
                }
            }
            else
            {
                string fileToOpen = name;
                if (fileToOpen.StartsWith("/"))
                {
                    fileToOpen = fileToOpen.Substring(1).Replace("/*", "");
                }
                // we only store the name of the file, not the path
                int lastSlash = fileToOpen.LastIndexOf("/");
                if (lastSlash > 0)
                {
                    fileToOpen = fileToOpen.Substring(lastSlash + 1);
                }
                ShortLongFileName slf = FindByShortName(fileToOpen);
                if (slf != null)
                {
                    dircontent.Clear();
                    dircontent.Add(slf);
                    fileToReadAsBytes = slf.longName;
                }
            }
        }

        private string ShortFilename(string longname)
        {
            int pos = longname.IndexOf('.');
            if (pos > 0)
            {
                string filename = longname.Substring(0, pos).Replace(" ", "").Replace("\\", "");
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
                string filename = longname.Replace(" ", "").Replace("\\","");
                if (filename.Length > 8)
                {
                    filename = filename.Substring(0, 6) + "~1";
                }
                filename += spaces.Substring(0,8-filename.Length);
                return filename.ToUpper() + spaces.Substring(0, 3);
            }
        }

        private bool ListContains(List<ShortLongFileName> directory, string shortname)
        {
            foreach (ShortLongFileName slf in directory)
            {
                if (slf.shortName.Substring(0, 11).Equals(shortname))
                {
                    return true;
                }
            }
            return false;
        }

        private ShortLongFileName FindByShortName(string name)
        {
            string shortName = name.Replace(".", "");
            foreach (ShortLongFileName slf in dircontent)
            {
                string partial = slf.shortName.Substring(0, 11).Replace("\0", "");
                if (partial.Equals(shortName))
                {
                    return slf;
                }
            }
            return null;
        }
    }
}
