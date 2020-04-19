﻿using FoenixIDE.MemoryLocations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace FoenixIDE.Simulator.Devices
{
    enum GabeCtrlCommand : byte
    {
        DIRECT = 0,
        INIT = 1,
        READ_BLK = 2,
        WRITE_BLK = 3
    }
    class FileEntry
    {
        public String name;
        public int cluster;
        public int size;
    }
    public class GabeSDController : SDCardDevice
    {
        private byte[] mbr = new byte[512];
        private byte[] boot_sector = new byte[512];
        private byte[] fat = new byte[512];
        private byte[] block;
        private byte[] root = new byte[32 * 512]; // root dir is always 32 sectors, except FAT32, which omits it.
        private int blockPtr = 0;
        private int waitCounter = 4;
        private const int BOOT_SECTOR_ADDR = 0x29 * 512;
        private int FAT_OFFSET_START = -1; // must be calcuated based on capacity
        private int ROOT_OFFSET_START = -1; // must be calculated based on capacity
        private int DATA_OFFSET_START = -1; // must be calculated based on capacity
        private int FAT_SIZE = 0;
        private int ROOT_SIZE = 0;
        private int DATA_SIZE = 0;
        private int logicalSectorSize = 512;
        byte sectors_per_cluster = 1;
        private Dictionary<int, FileEntry> FAT = new Dictionary<int, FileEntry>();
        private string spaces = "\0\0\0\0\0\0\0\0";
        private bool fat12 = false;

        private GabeCtrlCommand currentCommand = GabeCtrlCommand.DIRECT;

        public GabeSDController(int StartAddress, int Length) : base(StartAddress, Length)
        {
            data[MemoryMap.GABE_SDC_VERSION_REG - MemoryMap.GABE_SDC_CTRL_START] = 0x12;
            ResetMbrBootSector();
        }

        public override byte ReadByte(int Address)
        {
            switch (Address)
            {
                case MemoryMap.GABE_SDC_TRANS_STATUS_REG - MemoryMap.GABE_SDC_CTRL_START:
                    // fake the wait time
                    return (isPresent && (waitCounter-- > 0)) ? (byte)1 : (byte)0;
                case MemoryMap.GABE_SDC_TRANS_ERROR_REG - MemoryMap.GABE_SDC_CTRL_START:
                    // return 
                    return isPresent ? data[5] : (byte)1;
                case MemoryMap.GABE_SDC_RX_FIFO_DATA_REG - MemoryMap.GABE_SDC_CTRL_START:
                    return isPresent ? (block != null ? block[blockPtr++] : (byte)0xEF) : (byte)0;
            }
            return data[Address];
        }

        public override void WriteByte(int Address, byte Value)
        {
            // Clear the error status
            data[Address] = Value;
            switch (Address)
            {
                // Reset everything
                case MemoryMap.GABE_SDC_CONTROL_REG - MemoryMap.GABE_SDC_CTRL_START:
                    blockPtr = 0;
                    ResetMbrBootSector();
                    data[5] = 0;
                    break;
                case MemoryMap.GABE_SDC_TRANS_TYPE_REG - MemoryMap.GABE_SDC_CTRL_START:
                    currentCommand = (GabeCtrlCommand)Value;
                    data[5] = 0;
                    break;
                case MemoryMap.GABE_SDC_TRANS_CONTROL_REG - MemoryMap.GABE_SDC_CTRL_START:
                    data[5] = 0;
                    switch (currentCommand)
                    {
                        case  GabeCtrlCommand.INIT:
                            waitCounter = 4;
                            blockPtr = 0;
                            break;
                        case GabeCtrlCommand.READ_BLK:
                            waitCounter = 10; // the read operation will be a little longer
                            // determine which block to read
                            blockPtr = 0;
                            block = null;
                            int address = ReadWord(7) + ( ReadWord(9) << 16 ) ;
                            if (GetISOMode() == true)
                            {
                                block = GetData_ISO(address / 512);
                            }
                            else
                            {
                                if (address == 0)
                                {
                                    block = mbr;

                                }
                                else if (address == BOOT_SECTOR_ADDR)
                                {
                                    block = boot_sector;
                                }
                                else if (address >= FAT_OFFSET_START && address <= FAT_OFFSET_START + FAT_SIZE - 1)
                                {
                                    // read the fat table
                                    blockPtr = 0;
                                    int fatPage = (address - FAT_OFFSET_START) / 512;
                                    BuildFatPage(fatPage);
                                    block = fat;
                                }
                                else if (address >= ROOT_OFFSET_START && address <= ROOT_OFFSET_START + ROOT_SIZE - 1)
                                {
                                    // read the root table
                                    block = root;
                                    blockPtr = address - ROOT_OFFSET_START;
                                }
                                else if (address >= DATA_OFFSET_START && address <= DATA_OFFSET_START + DATA_SIZE - 1)
                                {
                                    // read the data
                                    int dataPage = (address - DATA_OFFSET_START) / 512;
                                    block = GetData(dataPage);
                                    blockPtr = 0;
                                }
                            }
                            if (block != null)
                            {
                                // tell the reader that we've got 512 bytes
                                data[0x12] = 2;
                                data[0x13] = 0;
                                // clear the error status
                                data[5] = 0;
                            }
                            else
                            {
                                // the controller returns an error
                                data[5] = 1;
                            }
                            break;
                        case GabeCtrlCommand.WRITE_BLK:
                            waitCounter = 10; // the write operation will be a little longer
                            // determine which block to write
                            blockPtr = 0;
                            
                            int address_WR = ReadWord(7) + (ReadWord(9) << 16);
                            if (GetISOMode() == true)
                            {
                                SetData_ISO(address_WR / 512, block); // write the 512 Byte buffer
                                block = null;
                            }
                            break;
                    }
                    break;
                case MemoryMap.GABE_SDC_RX_FIFO_CTRL_REG - MemoryMap.GABE_SDC_CTRL_START:
                    data[5] = 0;
                    if (Value == 1)
                    {
                        data[0x12] = 0;
                        data[0x13] = 0;
                    }
                    break;
                case MemoryMap.GABE_SDC_TX_FIFO_CTRL_REG - MemoryMap.GABE_SDC_CTRL_START:
                    data[5] = 0;
                    if (Value == 1)
                    {
                        blockPtr = 0;
                        block = new byte[512];
                        data[0x12] = 0;
                        data[0x13] = 0;
                    }
                    break;
                case MemoryMap.GABE_SDC_TX_FIFO_DATA_REG - MemoryMap.GABE_SDC_CTRL_START:
                    block[blockPtr++] = Value;
                    break;
            }
        }

        public override void SetCapacity(int value)
        {
            // set default boot_sector values for the capacity
            // calculate the offsets for the root_dir and data_area
            base.SetCapacity(value);
            ResetMbrBootSector();
        }

        public override void SetISOMode(bool value)
        {
            // set the SDcard module to use an iso file instead of a folder content
            base.SetISOMode(value);
            ResetMbrBootSector();
        }

        public void ResetMbrBootSector()
        {
            if (GetISOMode() == false)
            {
                // Zero the master boot record
                Array.Clear(mbr, 0, mbr.Length);
                // Zero the boot sector
                Array.Clear(boot_sector, 0, boot_sector.Length);
                sectors_per_cluster = 1;
                fat12 = false;
                byte reserved_sectors = 2;
                int small_sectors = 0;
                int large_sectors = 0;
                byte sectors_per_fat = 0;
                switch (GetCapacity())
                {
                    case 8:
                        sectors_per_cluster = 4;
                        fat12 = true;
                        small_sectors = 0x3397;
                        sectors_per_fat = 0xA;

                        break;
                    case 16:
                        sectors_per_cluster = 8;
                        small_sectors = 0x3397;
                        fat12 = true;
                        sectors_per_fat = 0xA;
                        break;
                    case 32:
                        sectors_per_cluster = 1;
                        small_sectors = 0xF460;
                        sectors_per_fat = 0xF3;
                        break;
                    case 64:
                        sectors_per_cluster = 2;
                        large_sectors = 0x1DBD9;
                        sectors_per_fat = 0xED;
                        break;
                    case 2048:
                        sectors_per_cluster = 64;
                        large_sectors = 0x3C9307;
                        sectors_per_fat = 0xF3;
                        break;
                }
                logicalSectorSize = sectors_per_cluster * 512; // this is used to calculate clusters off of filesizes
                if (isPresent)
                {
                    // Assign default values to the MBR
                    mbr[0x1FE] = 0x55;
                    mbr[0x1FF] = 0xAA;
                    waitCounter = 4;
                    // write parition table, with boot sector offset $29
                    byte[] partition = new byte[16]
                    {
                    0x80, 0x01, 0x0a, 0x00, 0x0e, 0x01, 0x20, 0xce,
                    0x29, 0x00, 0x00, 0x00, 0x97, 0x33, 0x00, 0x00
                    };
                    Array.Copy(partition, 0, mbr, 446, 16);

                    // Assign default values to the boot sector
                    boot_sector[0] = 0xEB;
                    boot_sector[1] = 0x3C;
                    boot_sector[2] = 0x90;
                    Array.Copy(Encoding.ASCII.GetBytes("MSDOS5.0"), 0, boot_sector, 3, 8);
                    boot_sector[0xC] = 0x2; // 512 bytes per sector
                    boot_sector[0xD] = sectors_per_cluster; // must be a factor of 2 
                    boot_sector[0xE] = reserved_sectors;
                    boot_sector[0x10] = 0x2; // Number of FATs
                    boot_sector[0x12] = 0x2; // 512 Root Entries
                    boot_sector[0x13] = (byte)(small_sectors & 0xFF);
                    boot_sector[0x14] = (byte)((small_sectors & 0xFF00) >> 8);
                    boot_sector[0x15] = 0xF8; // media type
                    boot_sector[0x16] = sectors_per_fat;
                    boot_sector[0x1C] = 0x29; // hidden sectors
                    boot_sector[0x20] = (byte)(large_sectors & 0xFF);
                    boot_sector[0x21] = (byte)((large_sectors & 0xFF00) >> 8);
                    boot_sector[0x22] = (byte)((large_sectors & 0xFF_0000) >> 16);
                    boot_sector[0x23] = (byte)((large_sectors & 0xFF00_0000) >> 24);

                    // volume label
                    Array.Copy(Encoding.ASCII.GetBytes("NO NAME    "), 0, boot_sector, 0x2B, 11);
                    // system id
                    if (fat12)
                    {
                        Array.Copy(Encoding.ASCII.GetBytes("FAT12   "), 0, boot_sector, 0x36, 8);
                    }
                    else
                    {
                        Array.Copy(Encoding.ASCII.GetBytes("FAT16   "), 0, boot_sector, 0x36, 8);
                    }
                    // marker
                    boot_sector[0x1FE] = 0x55;
                    boot_sector[0x1FF] = 0xAA;
                    // FAT offset
                    FAT_OFFSET_START = BOOT_SECTOR_ADDR + reserved_sectors * 512;
                    FAT_SIZE = 2 * sectors_per_fat * 512;
                    // ROOT offset
                    ROOT_OFFSET_START = FAT_OFFSET_START + FAT_SIZE;
                    ROOT_SIZE = 0x20 * 512;
                    PrepareRootArea();

                    // DATA offset
                    DATA_OFFSET_START = ROOT_OFFSET_START + 0x20 * 512; // the root area is always 32 sectors
                    DATA_SIZE = small_sectors != 0 ? small_sectors * 512 : large_sectors * 512;


                }
            }
        }

        public void PrepareRootArea()
        {
            Array.Clear(root, 0, root.Length);
            Array.Copy(Encoding.ASCII.GetBytes("FOENIXIDE  "), root, 11);
            root[11] = 8; // volume type

            string path = GetSDCardPath();
            string[] dirs = Directory.GetDirectories(path, "*", SearchOption.TopDirectoryOnly);
            string[] files = Directory.GetFiles(path, "*", SearchOption.TopDirectoryOnly);
            // number of entries in the Root area is 1 + dirs + files - we're not storing long file names
            int entries = 1 + dirs.Length + files.Length; // we can put 16 entries per sector
            int rootPages = entries / 16 + 1;
            FAT.Clear();

            int pointer = 32;
            int currentCluster = 4;
            
            foreach (string dir in dirs)
            {
                FileInfo info = new FileInfo(dir);
                string dirname = info.Name.ToUpper();
                if (dirname.Length < 8)
                {
                    dirname += spaces.Substring(8 - dirname.Length);
                }
                
                Array.Copy(Encoding.ASCII.GetBytes(dirname), 0, root, pointer, 8);
                root[pointer + 11] = 0x10;
                pointer += 32;
            }
            foreach (string file in files)
            {
                
                FileInfo info = new FileInfo(file);
                int size = (int)info.Length;
                int clusters = size / logicalSectorSize + 1;
                string extension = info.Extension.ToUpper().Substring(1);
                string filename = info.Name.ToUpper();
                int dot = filename.IndexOf(".");
                if (dot > -1)
                {
                    filename = filename.Substring(0, dot - 1);
                }

                if (filename.Length < 8)
                {
                    filename += spaces.Substring(8 - filename.Length);
                }
                FileEntry entry = new FileEntry()
                {
                    name = file,
                    cluster = clusters,
                    size = size
                };

                FAT.Add(currentCluster, entry);
                Array.Copy(Encoding.ASCII.GetBytes(filename), 0, root, pointer, 8);
                Array.Copy(Encoding.ASCII.GetBytes(extension), 0, root, pointer +8, 3);
                // cluster number
                root[pointer + 0x1a] = (byte)(currentCluster & 0xFF);
                root[pointer + 0x1b] = (byte)((currentCluster & 0xFF00) >> 8);
                // file size
                root[pointer + 0x1c] = (byte)(size & 0xFF);
                root[pointer + 0x1d] = (byte)((size & 0xFF00) >> 8);
                root[pointer + 0x1e] = (byte)((size & 0xFF_0000) >> 16);
                root[pointer + 0x1f] = (byte)((size & 0xFF00_0000) >> 24);
                currentCluster += clusters + 1;
                pointer += 32;
            }
        }
        
        private void BuildFatPage(int page)
        {
            Array.Clear(fat, 0, 512);
            // Determine how many entries are in FAT sector
            int fatCount = 512 / 2; // 256
            int byteOffset = 0;
            if (fat12)
            {
                fatCount = 513 / 3 * 2; //341
                if (page % 3 != 0)
                {
                    byteOffset = (page % 3) - 3;
                }
            }

            if (page == 0)
            {
                if (fat12)
                {
                    fat[0] = 0xF8;
                    fat[1] = 0xFF;
                    fat[2] = 0xFF;
                    fat[3] = 0xFF;
                    fat[4] = 0xFF;
                    fat[5] = 0xFF;
                }
                else
                {
                    fat[0] = 0xF8;
                    fat[1] = 0xFF;
                    fat[2] = 0xFF;
                    fat[3] = 0xFF;
                    fat[4] = 0xFF;
                    fat[5] = 0xFF;
                    fat[6] = 0xFF;
                    fat[7] = 0xFF;
                }
            }
            // scan the FAT entries
            foreach (int key in FAT.Keys)
            {
                FileEntry entry = FAT[key];
                if (key > page * fatCount && key < (page +1) * fatCount ||
                    key + entry.cluster >= page * fatCount && key < (page + 1) * fatCount)
                {
                    int pageOffset = key % fatCount;
                    int startOffset = 0;
                    if (key < page * fatCount)
                    {
                        startOffset = page * fatCount - key + byteOffset;
                        pageOffset = 0;
                    }
                    if (fat12)
                    {
                        
                        // even numbers start at the boundary - odd numbers are at half byte
                        for (int i = startOffset; i < entry.cluster; i++)
                        {
                            int position = (pageOffset + i - startOffset) >> 1;
                            if ((pageOffset - startOffset + i) % 2 == 0)
                            {
                                if (position * 3 + byteOffset >= 0 && position * 3 + byteOffset < 512)
                                {
                                    fat[position * 3 + byteOffset] = (byte)((key + i + 1) & 0xFF);
                                }
                                if ((position * 3) + 1 + byteOffset >= 0 && (position * 3) + 1 + byteOffset < 512)
                                {
                                    fat[(position * 3) + 1 + byteOffset] = (byte)(((key + i + 1) & 0xF00) >> 8);
                                }
                            }
                            else
                            {
                                if ((position * 3) + 1 + byteOffset >= 0)
                                {
                                    int existingNibble = fat[position * 3 + 1 + byteOffset];
                                    fat[position * 3 + 1 + byteOffset] = (byte)((((key + i + 1) & 0xF) << 4) + existingNibble);
                                }
                                if (position * 3 + 2 + byteOffset >= 0 && position * 3 + 2 < 512)
                                {
                                    fat[position * 3 + 2 + byteOffset] = (byte)(((key + i + 1) & 0xFF0) >> 4);
                                }
                            }
                            
                            if ((pageOffset + i - startOffset + 1) > fatCount)
                            {
                                return;
                            }
                        }
                        // write the end cluster
                        if (pageOffset + entry.cluster - startOffset < fatCount)
                        {
                            int position = (pageOffset + entry.cluster - startOffset) >> 1;
                            if ((pageOffset + entry.cluster - startOffset) % 2 == 0)
                            {
                                fat[position * 3 + byteOffset] = 0xFF;
                                fat[(position * 3) + 1 + byteOffset] = 0xF;
                            }
                            else
                            {
                                int existingNibble = fat[position * 3 + 1 + byteOffset];
                                fat[position * 3 + 1 + byteOffset] = (byte)(0xF0 + existingNibble);
                                fat[position * 3 + 2 + byteOffset] = 0xFF;
                            }
                        }
                    }
                    else
                    {
                        for (int i = startOffset; i < entry.cluster; i ++)
                        {
                            fat[(pageOffset + i - startOffset) * 2] = (byte)((key + i + 1) & 0xFF);
                            fat[(pageOffset + i - startOffset) * 2 + 1] = (byte)(((key + i + 1) & 0xFF00) >> 8);
                            if ((pageOffset + i - startOffset + 1) >= fatCount)
                            {
                                return;
                            }
                        }
                        // write the end cluster
                        if (pageOffset + entry.cluster - startOffset < fatCount)
                        {
                            fat[(pageOffset + entry.cluster - startOffset) * 2] = 0xFF;
                            fat[(pageOffset + entry.cluster - startOffset) * 2 + 1] = 0xFF;
                        }
                    }
                }
            }
        }

        /*
         * Return a sector of data from files.
         * Cluster 2 is the first sector.
         */
        private byte[] GetData(int page)
        {
            foreach (int key in FAT.Keys)
            {
                FileEntry entry = FAT[key];
                if (page >= ( key - 2 ) * sectors_per_cluster && page < (key - 2 + entry.cluster) * sectors_per_cluster)
                {
                    byte[] buffer = new byte[512];
                    FileStream stream = new FileStream(entry.name, FileMode.Open, FileAccess.Read);
                    try
                    {
                        stream.Seek((page - key) * 512, SeekOrigin.Begin);
                        stream.Read(buffer, 0, 512);
                        return buffer;
                    }
                    catch(Exception e)
                    {
                        // controller error
                        data[5] = 1;
                        System.Console.WriteLine(e.ToString());
                        return null;
                    }
                    finally
                    {
                        stream.Close();
                    }
                }
            }
            return null;
        }

        /*
         * Return a sector of data from files.
         * by block of 512 Byte
         */
        private byte[] GetData_ISO(int page)
        {
            if ((page >= 0)  && (page < 512*4096)) // test if we are with in 256MB
            {
                byte[] buffer = new byte[512];
                string path = GetSDCardPath();
                FileStream stream = new FileStream(path + "\\SD.img", FileMode.Open, FileAccess.Read);
                try
                {
                    stream.Seek((page) * 512, SeekOrigin.Begin);
                    stream.Read(buffer, 0, 512);
                    return buffer;
                }
                catch (Exception e)
                {
                    // controller error
                    data[5] = 1;
                    System.Console.WriteLine(e.ToString());
                    return null;
                }
                finally
                {
                    stream.Close();
                }
            }
            return null;
        }

        private void SetData_ISO(int page, byte[] buffer)
        {
            if ((page >= 0) && (page < 512 * 4096)) // test if we are with in 256MB
            {
                string path = GetSDCardPath();
                FileStream stream = new FileStream(path + "\\SD.img", FileMode.Open, FileAccess.Write);
                try
                {
                    stream.Seek((page) * 512, SeekOrigin.Begin);
                    stream.Write(buffer, 0, 512);
                    return;
                }
                catch (Exception e)
                {
                    // controller error
                    data[5] = 1;
                    System.Console.WriteLine(e.ToString());
                    return;
                }
                finally
                {
                    stream.Close();
                }
            }
            return;
        }
    }
}
