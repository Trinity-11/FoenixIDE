using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FoenixIDE.Simulator.Devices
{
    public class FileEntry
    {
        public string fqpn;
        public string shortname;
        public int clusters;
        public int size;
    }
    public class FakeFATSDCardDevice : SDCardDevice
    {
        protected byte[] mbr = new byte[512];
        protected byte[] boot_sector = new byte[512];
        protected readonly byte[] fat = new byte[512];
        protected byte[] readBlock, writeBlock = new byte[512];
        protected byte[] root = new byte[32 * 512]; // root dir is always 32 sectors, except FAT32, which omits it.
        protected Dictionary<string, byte[]> subdirectories = new Dictionary<string, byte[]>(); // these will hold the data files for the sub-folders
        protected int blockPtr = 0;
        protected int BOOT_SECTOR_ADDR = 0x29 * 512;
        protected int FAT_OFFSET_START = -1; // must be calcuated based on capacity
        protected int ROOT_OFFSET_START = -1; // must be calculated based on capacity
        protected int DATA_OFFSET_START = -1; // must be calculated based on capacity
        protected int FAT_SIZE = 0;
        protected int ROOT_SIZE = 0x20 * 512;
        protected int DATA_SIZE = 0;
        protected int logicalSectorSize = 512;
        protected byte sectors_per_cluster = 1;
        protected Dictionary<int, FileEntry> FAT = new Dictionary<int, FileEntry>();
        protected string spaces = "        ";
        protected bool mbrPresent = false;
        protected byte rootEntryCount = 2;
        protected bool vfat = false;

        protected FileEntry voidEntry = null; // this is a "newfile" entry that gets used when creating a new file.

        public FakeFATSDCardDevice(int StartAddress, int Length) : base(StartAddress, Length)
        {
            ResetMbrBootSector();
        }

        public override void ResetMbrBootSector()
        {
            blockPtr = 0;
            if (isPresent)
            {
                mbrPresent = false;
                BOOT_SECTOR_ADDR = 0x29 * 512;
                if (GetISOMode())
                {
                    // Read the first sector, if it starts with $eb, then it's a Boot Sector, otherwise it's a partition table (MBR)
                    byte[] firstSector = GetData_ISO(0);
                    if (firstSector[0] != 0xeb && firstSector[0x1FE] == 0x55 && firstSector[0x1FF] == 0xAA && (firstSector[0x1BE] & 0x80) != 0)
                    {
                        mbrPresent = true;
                        mbr = firstSector;
                        switch (firstSector[0x1C2])
                        {
                            case 1:
                                SetFSType(FSType.FAT12);
                                break;
                            case 4:
                                SetFSType(FSType.FAT16); // FAT16 with less than 65536 sectors (32MB)
                                break;
                            case 0xc:
                                SetFSType(FSType.FAT32);
                                break;
                            case 0xE:
                                SetFSType(FSType.FAT16); // FAT16B with LBA
                                break;
                            case 0x16:
                                SetFSType(FSType.FAT16); //hidden FAT16B
                                break;
                        }
                        BOOT_SECTOR_ADDR = (firstSector[0x1C6] + (firstSector[0x1C7] << 8) + (firstSector[0x1C8] << 16) + (firstSector[0x1C9] << 24)) * 512;
                    }
                    else
                    {
                        sectors_per_cluster = firstSector[0xD];
                        int reserved_sectors = firstSector[0xE] + (firstSector[0xF] << 8);
                        int numberOfFATs = firstSector[0x10]; // should be 2
                        int maxNumberOfRootEntries = firstSector[0x11] + (firstSector[0x12] << 8);
                        byte sectors_per_fat = firstSector[0x16];
                        int small_sectors = firstSector[0x13] + (firstSector[0x14] << 8);
                        int large_sectors = firstSector[0x20] + (firstSector[0x21] << 8) + (firstSector[0x22] << 16) + (firstSector[0x23] << 24);

                        // FAT offset
                        FAT_OFFSET_START = BOOT_SECTOR_ADDR + reserved_sectors * 512;
                        FAT_SIZE = 2 * sectors_per_fat * 512;

                        // ROOT offset
                        ROOT_OFFSET_START = FAT_OFFSET_START + FAT_SIZE;
                        ROOT_SIZE = 0x20 * 512;

                        // DATA offset
                        DATA_OFFSET_START = ROOT_OFFSET_START + 0x20 * 512; // the root area is always 32 sectors
                        DATA_SIZE = small_sectors != 0 ? small_sectors * 512 : large_sectors * 512;
                        if (firstSector[0x36] == 'F' && firstSector[0x37] == 'A' && firstSector[0x38] == 'T' && firstSector[0x39] == '1')
                        {
                            if (firstSector[0x3A] == '2')
                            {
                                SetFSType(FSType.FAT12);
                            }
                            else if (firstSector[0x3A] == '6')
                            {
                                SetFSType(FSType.FAT16);
                            }
                        }
                        else if (firstSector[0x52] == 'F' && firstSector[0x53] == 'A' && firstSector[0x54] == 'T' && firstSector[0x55] == '3' && firstSector[0x56] == '2')
                        {
                            SetFSType(FSType.FAT32);
                        }
                    }
                }

                // Prepare a fake MBR with a very plain partition record
                if (!mbrPresent)
                {
                    // Zero the master boot record
                    Array.Clear(mbr, 0, mbr.Length);

                    // Assign default values to the MBR
                    mbr[0x1FE] = 0x55;
                    mbr[0x1FF] = 0xAA;

                    byte fs = 0xC; //default to FAT32
                    switch (GetFSType())
                    {
                        case FSType.FAT12:
                            fs = 1;
                            break;
                        case FSType.FAT16:
                            fs = 4;
                            break;
                    }

                    byte[] nSectorsInPartition = new byte[4]{ 0x97, 0x33, 0x00, 0x00 }; // Kernel expects this number to be higher
                    //byte[] nSectorsInPartition = new byte[4] { 0xcb, 0x07, 0x02, 0x00 };

                    // There is a single partition in this MBR.
                    // The other three partition slots are left blank.

                    // Write parition table, with boot sector offset $29
                    byte[] partition = new byte[16]
                    {
                        0x80, 0x01, 0x0a, 0x00, fs, 0x01, 0x20, 0xce,
                        0x29, 0x00, 0x00, 0x00, nSectorsInPartition[0], nSectorsInPartition[1], nSectorsInPartition[2], nSectorsInPartition[3]
                    };

                    // partition[0] == Current State of Partition(00h=Inactive, 80h=Active)
                    // partition[1] == 	Beginning of Partition - Head
                    // partition[2-3] == Beginning of Partition - Cylinder/Sector (See Below)
                    // partition[4] == 	Type of Partition (See List Below)
                    // partition[5] == 	End of Partition - Head
                    // partition[6-7] == End of Partition - Cylinder/Sector
                    // partition[8-B] == Number of Sectors Betweenthe MBR and the First Sector in the Partition   - set to 00000029 here
                    // partition[C] ==   Number of Sectors in the partition. set to 0x00003397 = 13207
                    // Note that the Jr kernel wants the number of sectors for the partition to be at least 0x10000.
                    // It also uses sectors and clusters interchageably.

                    Array.Copy(partition, 0, mbr, 446, 16);

                    // If the ISO file didn't have an MBR, ensure that we set the partiton type correctly
                    if (GetISOMode())
                    {

                    }
                }
                if (!GetISOMode())
                {
                    // Zero the boot sector
                    Array.Clear(boot_sector, 0, boot_sector.Length);
                    sectors_per_cluster = (byte)(GetClusterSize() / 512);
                    byte reserved_sectors = 2;
                    byte root_sectors = 32;
                    // we're reserving 32 sectors for FAT32 as well, to simplify the implementation.  This limits how big the directory is to 512 entries
                    int capacity = GetCapacity() * 1024 * 1024 - (1 + 1 + reserved_sectors) * 512 - BOOT_SECTOR_ADDR - root_sectors * 512;  // remove the reserved and offset spaces, boot sector, mbr and root area
                    int sector_count = 2 + BOOT_SECTOR_ADDR / 512 + reserved_sectors + 32;

                    int req_cluster = capacity / GetClusterSize();
                    // The FAT must have enough entries to access each cluster
                    int sectors_per_fat = 0;
                    switch (GetFSType())
                    {
                        case FSType.FAT12:
                            if (req_cluster > 4084)
                            {
                                req_cluster = 4084;
                            }
                            sectors_per_fat = req_cluster * 3 / 1024;
                            break;
                        case FSType.FAT16:
                            if (req_cluster > 65524)
                            {
                                req_cluster = 65524;
                            }
                            sectors_per_fat = req_cluster / 256;
                            break;
                        case FSType.FAT32:
                            // FAT32 can address all cluster sizes within 2GB
                            sectors_per_fat = req_cluster / 128;
                            break;
                    }

                    sector_count += sectors_per_fat * 2;

                    switch (GetFSType())
                    {
                        case FSType.FAT12:
                            sector_count += sectors_per_fat * 1024 / 3 * GetClusterSize() / 512;
                            break;
                        case FSType.FAT16:
                            sector_count += sectors_per_fat * 256 * GetClusterSize() / 512;
                            break;
                        case FSType.FAT32:
                            sector_count += sectors_per_fat * 128 * GetClusterSize() / 512;
                            break;
                    }
                    // Assign sector count
                    int small_sectors = 0;
                    int large_sectors = 0;
                    if (sector_count > 0xFFFF)
                    {
                        large_sectors = sector_count;
                    }
                    else
                    {
                        small_sectors = sector_count;
                    }
                    logicalSectorSize = sectors_per_cluster * 512; // this is used to calculate clusters off of filesizes

                    // Assign default values to the boot sector
                    boot_sector[0] = 0xEB;
                    boot_sector[1] = 0x3C;
                    boot_sector[2] = 0x90;
                    Array.Copy(Encoding.ASCII.GetBytes("MSDOS5.0"), 0, boot_sector, 3, 8);
                    boot_sector[0xC] = 0x2; // 512 bytes per sector
                    boot_sector[0xD] = sectors_per_cluster; // must be a factor of 2 
                    boot_sector[0xE] = reserved_sectors;
                    boot_sector[0xF] = 0;
                    boot_sector[0x10] = 0x2; // Number of FATs
                    boot_sector[0x12] = rootEntryCount;   // rootEntryCount
                    boot_sector[0x13] = (byte)(small_sectors & 0xFF);
                    boot_sector[0x14] = (byte)((small_sectors & 0xFF00) >> 8);
                    boot_sector[0x15] = 0xF8; // media type
                    if (GetFSType() != FSType.FAT32)
                    {
                        boot_sector[0x16] = (byte)(sectors_per_fat & 0xFF);
                        boot_sector[0x17] = (byte)((sectors_per_fat & 0xFF00) >> 8);
                    }
                    else
                    {
                        boot_sector[0x24] = (byte)(sectors_per_fat & 0xFF);
                        boot_sector[0x25] = (byte)((sectors_per_fat & 0xFF00) >> 8);
                        boot_sector[0x26] = (byte)((sectors_per_fat & 0xFF0000) >> 16);
                        boot_sector[0x27] = (byte)((sectors_per_fat & 0xFF000000) >> 24);
                    }
                    boot_sector[0x1C] = 0x29; // hidden sectors
                    boot_sector[0x20] = (byte)(large_sectors & 0xFF);
                    boot_sector[0x21] = (byte)((large_sectors & 0xFF00) >> 8);
                    boot_sector[0x22] = (byte)((large_sectors & 0xFF_0000) >> 16);
                    boot_sector[0x23] = (byte)((large_sectors & 0xFF00_0000) >> 24);

                    // volume label
                    Array.Copy(Encoding.ASCII.GetBytes("NO NAME    "), 0, boot_sector, 0x2B, 11);
                    // system id
                    switch (GetFSType())
                    {
                        case FSType.FAT12:
                            Array.Copy(Encoding.ASCII.GetBytes("FAT12   "), 0, boot_sector, 0x36, 8);
                            break;
                        case FSType.FAT16:
                            Array.Copy(Encoding.ASCII.GetBytes("FAT16   "), 0, boot_sector, 0x36, 8);
                            break;
                        case FSType.FAT32:
                            Array.Copy(Encoding.ASCII.GetBytes("FAT32   "), 0, boot_sector, 0x52, 8);
                            break;
                    }

                    // marker
                    boot_sector[0x1FE] = 0x55;
                    boot_sector[0x1FF] = 0xAA;
                    // FAT offset
                    FAT_OFFSET_START = BOOT_SECTOR_ADDR + reserved_sectors * 512;
                    FAT_SIZE = 2 * sectors_per_fat * 512;

                    // ROOT offset - For FAT32 as well
                    ROOT_OFFSET_START = FAT_OFFSET_START + FAT_SIZE;
                    if (GetFSType() == FSType.FAT32)
                    {
                        // The cluster to the root area
                        boot_sector[0x2C] = 2;
                        boot_sector[0x2D] = 0;
                        boot_sector[0x2E] = 0;
                        boot_sector[0x2F] = 0;
                    }
                    PrepareRootArea();

                    // DATA offset
                    DATA_OFFSET_START = ROOT_OFFSET_START + ROOT_SIZE; // the root area is always 32 sectors
                    DATA_SIZE = small_sectors != 0 ? small_sectors * 512 : large_sectors * 512;
                }
            }
        }

        /**
         * Should probably make this a recursive function.
         */
        public void PrepareRootArea()
        {
            // The first record in the Root Area is the drive name
            Array.Clear(root, 0, root.Length);
            Array.Copy(Encoding.ASCII.GetBytes("FOENIXIDE  "), root, 11);
            root[11] = 8; // volume type

            string path = GetSDCardPath();
            List<string> dirs = new List<string>();
            dirs.Add(path);
            dirs.AddRange(Directory.GetDirectories(path, "*.*", SearchOption.TopDirectoryOnly));
            
            FAT.Clear();
            subdirectories.Clear();

            int pointer = 32;
            // Current cluster points to the start of FAT
            int currentCluster = 4;
            if (GetFSType() == FSType.FAT32)
            {
                // this skips the "root" area which is reserved for directories
                currentCluster = 2 + 32 * 512 / GetClusterSize();
            }
            int dirCount = dirs.Count - 1;
            int rootDirCount = 1;

            byte[] directory = root;
            foreach (string dir in dirs)
            {
                FileInfo dirInfo = new FileInfo(dir);
                string[] files = Directory.GetFiles(dir, "*", SearchOption.TopDirectoryOnly);
                

                // Don't write a record for the root folder
                if (rootDirCount == 0)
                {
                    string dirname = dirInfo.Name.Replace(" ", "").ToUpper();
                    if (dirname.Length < 8)
                    {
                        dirname += spaces.Substring(0, 8 - dirname.Length);
                    }
                    directory = new byte[0x4000];  // we reserving all this space for a new directory
                    subdirectories.Add(dirname, directory);

                    Array.Copy(Encoding.ASCII.GetBytes(dirname), 0, root, pointer, 8);
                    Array.Copy(Encoding.ASCII.GetBytes("   "), 0, root, pointer+8, 3);
                    root[pointer + 11] = 0x10;

                    // each directory uses a new cluster - the number of files in the cluster determines the number of clusters
                    // the starting cluster of directories is beyond the root files
                    // write the start of the cluster - high two bytes
                    root[pointer + 0x14] = (byte)((currentCluster & 0xFF_0000) >> 16);
                    root[pointer + 0x15] = (byte)((currentCluster & 0xFF00_0000) >> 24);
                    // write the start of the cluster - low two bytes
                    root[pointer + 0x1a] = (byte)(currentCluster & 0xFF);
                    root[pointer + 0x1b] = (byte)((currentCluster & 0xFF00) >> 8);
                    pointer += 32;

                    FileEntry entry = new FileEntry()
                    {
                        fqpn = dir,
                        shortname = dirname,
                        clusters = 0x4000/GetClusterSize(),
                        size = 0
                    };

                    FAT.Add(currentCluster, entry);
                    currentCluster += 0x4000 / GetClusterSize();
                }

                // We need to get the number of entries, because F256K may use long filenames
                int entries = BuildFileRecordsToDirectory(files, currentCluster, directory, 32 * (dirCount + rootDirCount), out int oCluster);
                currentCluster = oCluster;

                // number of entries in the Root area is 1 + dirs + files - we're not storing long file names
                // we can put 16 entries per sector (512 bytes / 32)
                // int dirSectors = (rootDirCount + dirCount + entries ) / 16 + 1; // TODO --- this line seems incorrect: is it entries per sector or per cluster?
                
                rootDirCount = 0;
                dirCount = 0;
            }
        }

        private int BuildFileRecordsToDirectory(string[] files, int cluster, byte[] dirBuffer, int dirPointer, out int outCluster)
        {
            bool AlreadyExists(string fn)
            {
                foreach (FileEntry entry in FAT.Values)
                {
                    if (entry.shortname.Equals(fn))
                    {
                        return true;
                    }
                }
                return false;
            }

            int recordCount = 0;
            foreach (string file in files)
            {
                FileInfo info = new FileInfo(file);
                int size = (int)info.Length;
                int clusters = size / logicalSectorSize;
                // ensure there is enough storage for all data
                if (size % logicalSectorSize != 0 || clusters == 0)
                {
                    clusters += 1;
                }
                string originalExtension = info.Extension;
                string extension = originalExtension.Length > 0 ? info.Extension.ToUpper().Substring(1) : "";
                string originalFilename = info.Name;
                string filename = originalFilename.Replace(" ", "").ToUpper();
                int dot = filename.IndexOf(".");
                if (dot > -1)
                {
                    filename = filename.Substring(0, dot);
                }
                // Truncate long file names - until we start supporting long file names
                if (filename.Length > 8)
                {
                    int count = 1;
                    string shortFilename = null;
                    do
                    {
                        shortFilename = filename.Substring(0, 6) + "~" + count++;
                    }
                    while (AlreadyExists(shortFilename) && count < 10);
                    filename = shortFilename;
                }
                if (filename.Length < 8)
                {
                    filename += spaces.Substring(0, 8 - filename.Length);
                }
                if (extension.Length < 3)
                {
                    extension += spaces.Substring(0, 3 - extension.Length);
                }
                FileEntry entry = new FileEntry()
                {
                    fqpn = file,
                    shortname = filename,
                    clusters = clusters,
                    size = size
                };

                FAT.Add(cluster, entry);
                // Create the Long Filename Entries for F256 - LFN entries must precede the file entry
                if (vfat & (originalFilename.Length > 12 | originalExtension.Length > 3))
                {
                    // Compute the checksum of filename
                    byte chksum = LFNCheckSum(filename + extension);
                    byte[] UCS2FN = Encoding.Unicode.GetBytes(originalFilename);
                    // The filename needs to end with 0x0000
                    int records = (UCS2FN.Length + 2) / 26 + 1;
                    byte[] buffer = new byte[26 * records]; // I want an array that's always long enough to not raise errors
                    for (int i = 0; i < 26 * records; i++)
                    {
                        buffer[i] = 0xFF;
                    }
                    Array.Copy(UCS2FN, buffer, UCS2FN.Length);
                    buffer[UCS2FN.Length] = 0;
                    buffer[UCS2FN.Length + 1] = 0;
                    // records are written in reverse order
                    for (int seq = records; seq > 0; seq--)
                    {
                        int offset = (seq - 1) * 26;
                        // Copy 26 bytes - or 13 UCS-2 characters
                        Array.Copy(buffer, offset, dirBuffer, dirPointer + 1, 10);
                        Array.Copy(buffer, offset + 10, dirBuffer, dirPointer + 0xE, 12);
                        Array.Copy(buffer, offset + 22, dirBuffer, dirPointer + 0x1C, 4);

                        // The first byte is the sequence number - if this is the last record, set bit 6.
                        dirBuffer[dirPointer] = (byte)((seq == records ? 0x40 : 0) + seq);
                        dirBuffer[dirPointer + 0xb] = 0xf;
                        dirBuffer[dirPointer + 0xc] = 0;
                        dirBuffer[dirPointer + 0xd] = chksum;
                        dirBuffer[dirPointer + 0x1a] = 0;

                        // increase the root buffer pointer
                        dirPointer += 32;
                        recordCount++;
                    }
                }
                Array.Copy(Encoding.ASCII.GetBytes(filename), 0, dirBuffer, dirPointer, 8);
                Array.Copy(Encoding.ASCII.GetBytes(extension), 0, dirBuffer, dirPointer + 8, 3);
                // cluster number
                dirBuffer[dirPointer + 0x1a] = (byte)(cluster & 0xFF);
                dirBuffer[dirPointer + 0x1b] = (byte)((cluster & 0xFF00) >> 8);
                if (GetFSType() == FSType.FAT32)
                {
                    dirBuffer[dirPointer + 0x14] = (byte)((cluster & 0xFF_0000) >> 16);
                    dirBuffer[dirPointer + 0x15] = (byte)((cluster & 0xFF00_0000) >> 24);
                }
                // file size
                dirBuffer[dirPointer + 0x1c] = (byte)(size & 0xFF);
                dirBuffer[dirPointer + 0x1d] = (byte)((size & 0xFF00) >> 8);
                dirBuffer[dirPointer + 0x1e] = (byte)((size & 0xFF_0000) >> 16);
                dirBuffer[dirPointer + 0x1f] = (byte)((size & 0xFF00_0000) >> 24);
                cluster += clusters;
                dirPointer += 32;
                recordCount++;
            }
            outCluster = cluster;
            return recordCount;
        }

        public static byte LFNCheckSum(string v)
        {
            int sum = 0;
            byte[] buffer = Encoding.ASCII.GetBytes(v);
            for (int i = 0; i < 11; i++)
            {
                sum = (((sum & 1) << 7) | ((sum & 0xFE) >> 1)) + buffer[i];
            }
            return (byte)sum;
        }

        /*
         * Return a sector of data from files.
         * by block of 512 Byte
         */
        protected byte[] GetData_ISO(int page)
        {
            if ((page >= 0) && (page < 512 * 4096)) // test if we are with in 256MB
            {
                byte[] buffer = new byte[512];
                string path = GetSDCardPath();
                FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read);
                try
                {
                    stream.Seek(page * 512, SeekOrigin.Begin);
                    stream.Read(buffer, 0, 512);
                    return buffer;
                }
                catch (Exception e)
                {
                    // controller error
                    ReportError();
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

        protected void GetReadBlock(int readAddress)
        {
            if (GetISOMode())
            {
                // In the case of address 0, return the MBR.
                if (readAddress == 0)
                {
                    readBlock = mbr;
                }
                else
                {
                    readBlock = GetData_ISO((readAddress - (mbrPresent ? 0 : BOOT_SECTOR_ADDR)) / 512);
                }
            }
            else
            {
                if (readAddress == 0)
                {
                    readBlock = mbr;

                }
                else if (readAddress == BOOT_SECTOR_ADDR)
                {
                    readBlock = boot_sector;
                }
                else if (readAddress >= FAT_OFFSET_START && readAddress <= FAT_OFFSET_START + FAT_SIZE - 1)
                {
                    // read the fat table
                    blockPtr = 0;
                    int fatPage = (readAddress - FAT_OFFSET_START) / 512;
                    BuildFatPage(fatPage);
                    readBlock = fat;
                }
                else if (readAddress >= ROOT_OFFSET_START && readAddress <= ROOT_OFFSET_START + ROOT_SIZE - 1)
                {
                    // read the root table
                    readBlock = root;
                    blockPtr = readAddress - ROOT_OFFSET_START;
                }
                else if (readAddress >= DATA_OFFSET_START && readAddress <= DATA_OFFSET_START + DATA_SIZE - 1)
                {
                    // read the data
                    int dataPage = (readAddress - DATA_OFFSET_START) / 512;
                    if (GetFSType() == FSType.FAT32)
                    {
                        dataPage += ROOT_SIZE / 512;
                    }
                    readBlock = GetData(dataPage);
                    blockPtr = 0;
                }
            }
        }

        /*
         * Return a sector of data from files.
         * Cluster 2 is the first sector.
         */
        protected byte[] GetData(int page)
        {
            foreach (int key in FAT.Keys)
            {
                FileEntry entry = FAT[key];
                int firstSector = (key - 2) * sectors_per_cluster;
                if (page >= firstSector && page < (key - 2 + entry.clusters) * sectors_per_cluster)
                {
                    byte[] buffer = new byte[512];
                    if (entry.size != 0)
                    {
                        FileStream stream = null;
                        try
                        {
                            stream = new FileStream(entry.fqpn, FileMode.Open, FileAccess.Read);
                            stream.Seek((page - firstSector) * 512, SeekOrigin.Begin);
                            stream.Read(buffer, 0, 512);
                            return buffer;
                        }
                        catch (Exception e)
                        {
                            // controller error
                            ReportError();
                            System.Console.WriteLine(e.ToString());
                            return null;
                        }
                        finally
                        {
                            if (stream != null)
                            {
                                stream.Close();
                            }
                        }
                    } 
                    else
                    {
                        // this is a directory
                        if (page - firstSector < 32)
                        {
                            byte[] dirStruct = subdirectories[entry.shortname];
                            Array.Copy(dirStruct, (page - firstSector) * 512, buffer, 0, 512);
                            return buffer;
                        }
                    }
                }
            }
            return null;
        }


        protected void BuildFatPage(int page)
        {
            Array.Clear(fat, 0, 512);
            // The most likely used FS is FAT32
            int fatCount = 0;
            int byteOffset = 0;
            switch (GetFSType())
            {
                case FSType.FAT12:
                    fatCount = 513 / 3 * 2; //341
                    if (page % 3 != 0)
                    {
                        byteOffset = (page % 3) - 3;
                    }
                    break;

                case FSType.FAT16:
                    fatCount = 512 / 2; //256
                    byteOffset = 0;
                    break;
                case FSType.FAT32:
                    fatCount = 512 / 4; // 128
                    byteOffset = 0;
                    break;
            }

            if (page == 0)
            {
                switch (GetFSType())
                {
                    case FSType.FAT12:
                        fat[0] = 0xF8;
                        fat[1] = 0xFF;
                        fat[2] = 0xFF;
                        fat[3] = 0xFF;
                        fat[4] = 0xFF;
                        fat[5] = 0xFF;
                        break;
                    case FSType.FAT16:
                        fat[0] = 0xF0;
                        fat[1] = 0xFF;
                        fat[2] = 0xFF;
                        fat[3] = 0xFF;
                        fat[4] = 0xFF;
                        fat[5] = 0xFF;
                        fat[6] = 0xFF;
                        fat[7] = 0xFF;
                        break;
                    case FSType.FAT32:
                        fat[0] = 0xF0;
                        fat[1] = 0xFF;
                        fat[2] = 0xFF;
                        fat[3] = 0xF0;
                        fat[4] = 0xFF;
                        fat[5] = 0xFF;
                        fat[6] = 0xFF;
                        fat[7] = 0x0F;

                        // Generate the Root area FAT
                        int currentCluster = 3;
                        for (int i = 0; i < 32 * 512 / GetClusterSize() - 1; i++)
                        {
                            fat[8 + i * 4] = (byte)(currentCluster++ & 0xFF);
                            fat[8 + i * 4 + 1] = 0;
                            fat[8 + i * 4 + 2] = 0;
                            fat[8 + i * 4 + 3] = 0;

                        }
                        fat[8 + 31 * 4] = 0xFF;
                        fat[8 + 31 * 4 + 1] = 0xFF;
                        fat[8 + 31 * 4 + 2] = 0xFF;
                        fat[8 + 31 * 4 + 3] = 0x0F;
                        break;
                }
            }
            // scan the FAT entries
            foreach (int key in FAT.Keys)
            {
                FileEntry entry = FAT[key];
                if (key > page * fatCount && key < (page + 1) * fatCount ||
                    key + entry.clusters >= page * fatCount && key < (page + 1) * fatCount)
                {
                    int pageOffset = key % fatCount;
                    int startOffset = 0;
                    if (key < page * fatCount)
                    {
                        startOffset = page * fatCount - key + byteOffset;
                        pageOffset = 0;
                    }
                    switch (GetFSType())
                    {
                        case FSType.FAT12:

                            // even numbers start at the boundary - odd numbers are at half byte
                            for (int i = startOffset; i < entry.clusters; i++)
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
                            if (pageOffset + entry.clusters - startOffset < fatCount)
                            {
                                int position = (pageOffset + entry.clusters - startOffset) >> 1;
                                if ((pageOffset + entry.clusters - startOffset) % 2 == 0)
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
                            break;
                        case FSType.FAT16:
                            for (int i = startOffset; i < entry.clusters; i++)
                            {
                                fat[(pageOffset + i - startOffset) * 2] = (byte)((key + i + 1) & 0xFF);
                                fat[(pageOffset + i - startOffset) * 2 + 1] = (byte)(((key + i + 1) & 0xFF00) >> 8);
                                if ((pageOffset + i - startOffset + 1) >= fatCount)
                                {
                                    return;
                                }
                            }
                            // write the end cluster
                            if (pageOffset + entry.clusters - startOffset < fatCount)
                            {
                                fat[(pageOffset + entry.clusters - startOffset) * 2] = 0xFF;
                                fat[(pageOffset + entry.clusters - startOffset) * 2 + 1] = 0xFF;
                            }
                            break;
                        case FSType.FAT32:
                            for (int i = startOffset; i < entry.clusters; i++)
                            {
                                fat[(pageOffset + i - startOffset) * 4] = (byte)((key + i + 1) & 0xFF);
                                fat[(pageOffset + i - startOffset) * 4 + 1] = (byte)(((key + i + 1) & 0xFF00) >> 8);
                                fat[(pageOffset + i - startOffset) * 4 + 2] = (byte)(((key + i + 1) & 0xFF0000) >> 16);
                                fat[(pageOffset + i - startOffset) * 4 + 3] = (byte)(((key + i + 1) & 0xFF000000) >> 24);
                                if ((pageOffset + i - startOffset + 1) >= fatCount)
                                {
                                    return;
                                }
                            }
                            // write the end cluster
                            if (pageOffset + entry.clusters - startOffset < fatCount)
                            {
                                fat[(pageOffset + entry.clusters - startOffset) * 4] = 0xFF;
                                fat[(pageOffset + entry.clusters - startOffset) * 4 + 1] = 0xFF;
                                fat[(pageOffset + entry.clusters - startOffset) * 4 + 2] = 0xFF;
                                fat[(pageOffset + entry.clusters - startOffset) * 4 + 3] = 0xF;
                            }
                            break;
                    }
                }
            }
        }

        private void SetData_ISO(int page, byte[] buffer)
        {
            if ((page >= 0) && (page < 512 * 4096)) // test if we are with in 256MB
            {
                string path = GetSDCardPath();
                FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Write);
                try
                {
                    stream.Seek(page * 512, SeekOrigin.Begin);
                    stream.Write(buffer, 0, 512);
                    return;
                }
                catch (Exception e)
                {
                    // controller error
                    ReportError();
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

        protected void WriteBlockImpl(int writeAddress)
        {
            if (GetISOMode())
            {
                SetData_ISO((writeAddress - (mbrPresent ? 0 : BOOT_SECTOR_ADDR)) / 512, writeBlock); // write the 512 Byte buffer
            }
            else
            {
                if (writeAddress == 0)
                {
                    Console.WriteLine("Gabe is trying to write to MBR!");

                }
                else if (writeAddress == BOOT_SECTOR_ADDR)
                {
                    Console.WriteLine("Gabe is trying to write to Boot Sector!");
                }
                else if (writeAddress >= FAT_OFFSET_START && writeAddress <= FAT_OFFSET_START + FAT_SIZE - 1)
                {
                    // read the fat table
                    blockPtr = 0;
                    int fatPage = (writeAddress - FAT_OFFSET_START) / 512;
                    Console.WriteLine("Gabe is trying to write to FAT Area! Page: " + fatPage);
                    // Compare the last FAT with the writeBlock - based on how many clusters are created, we can determine the filesize
                    PrepareEmptyFileEntry(fatPage);
                    writeBlock = new byte[512];
                }
                else if (writeAddress >= ROOT_OFFSET_START && writeAddress <= ROOT_OFFSET_START + ROOT_SIZE - 1)
                {
                    Console.WriteLine("Gabe is trying to write to Root Area!");
                    blockPtr = writeAddress - ROOT_OFFSET_START;
                    UpdateRootEntries();
                    firstPtr = -1;
                    lastPtr = -1;
                }
                else if (writeAddress >= DATA_OFFSET_START && writeAddress <= DATA_OFFSET_START + DATA_SIZE - 1)
                {
                    // read the data
                    int dataPage = (writeAddress - DATA_OFFSET_START) / 512;
                    if (GetFSType() == FSType.FAT32)
                    {
                        dataPage += 2 + ROOT_SIZE / 512;
                    }
                    Console.WriteLine("Gabe is trying to write to Data Area! Cluster: " + dataPage);
                    SetData(dataPage, writeBlock);
                    blockPtr = 0;
                }
                else
                {
                    // Invalid address
                    Console.WriteLine("Gabe is trying to write to an invalid address:" + writeAddress);
                    ReportError();
                }
            }
        }

        protected virtual void ReportError()
        {
            data[5] = 1;
        }

        protected virtual bool ShouldPreparePlaceholderFileEntry(int page)
        {
            return true;
        }

        void GetNameAndExtFromLfnWriteBlock(int blockStartIndex, out string name, out string ext)
        {
            StringBuilder sb = new StringBuilder();
            int[] indices = { 1, 3, 5, 7, 9, 14, 16, 18, 20, 22, 24 };
            for (int k = 0; k < indices.Length; ++k)
            {
                byte ch0 = writeBlock[blockStartIndex + indices[k]];
                byte ch1 = writeBlock[blockStartIndex + indices[k] + 1];

                if (ch0 == 0) // Null delimited
                    break;
                
                if (ch0 == 0xAC && ch1 == 0x20)
                {
                    sb.Append('€');
                }
                else if (ch0 == 0x60 && ch1 == 0x01)
                {
                    sb.Append('Š');
                }
                else if (ch0 == 0x61 && ch1 == 0x01)
                {
                    sb.Append('š');
                }
                else if (ch0 == 0x7D && ch1 == 0x01)
                {
                    sb.Append('Ž');
                }
                else if (ch0 == 0x7E && ch1 == 0x01)
                {
                    sb.Append('ž');
                }
                else if (ch0 == 0x52 && ch1 == 0x01)
                {
                    sb.Append('Œ');
                }
                else if (ch0 == 0x53 && ch1 == 0x01)
                {
                    sb.Append('œ');
                }
                else if (ch0 == 0x78 && ch1 == 0x01)
                {
                    sb.Append('Ÿ');
                }
                else
                {
                    sb.Append((char)ch0);
                }

            }
            string s = sb.ToString();
            int delim = s.IndexOf('.');
            if (delim > 0)
            {
                name = s.Substring(0, delim);
                ext = s.Substring(delim + 1);
            }
            else
            {
                name = s;
                ext = "";
            }
        }

        protected int firstPtr = -1, lastPtr = -1;
        protected int clusterCount = 0;
        private void PrepareEmptyFileEntry(int page)
        {
            if (!ShouldPreparePlaceholderFileEntry(page))
                return;

            int ffCntr = 0;
            if (firstPtr == -1 && lastPtr == -1)
            {
                for (int i = 0; i < 512; i++)
                {
                    if (firstPtr == -1)
                    {
                        // Look for the first requested change to the fat
                        if (fat[i] != writeBlock[i])
                        {
                            firstPtr = i;
                            if (writeBlock[i] == 0xFF)
                            {
                                ffCntr++;
                            }
                        }
                    }
                    else
                    {
                        if (i % 4 != 3 && writeBlock[i] == 0xff)
                        {
                            ffCntr++;
                        }
                        if (ffCntr == 3 && i % 4 == 3 && writeBlock[i] == 0x0f)
                        {
                            lastPtr = i;
                            break;
                        }
                    }
                }
            }
            // Determine how many clusters have been added.
            if (firstPtr != -1 && lastPtr != -1)
            {
                clusterCount = (lastPtr - firstPtr + 1) / 4;
                // Create a new file entry
                CreateEmptyFile(clusterCount * 512, page);
            }
            else if (firstPtr != -1)
            {
                clusterCount = (512 - firstPtr) / 4;
            }
            else if (lastPtr != -1)
            {
                clusterCount += lastPtr / 4;
                // Create a new file entry
                CreateEmptyFile(clusterCount * 512, page);
            }
        }

        // We're writing to the Root entries, so this means:
        // - A new file was created,
        // - A file was deleted
        // - A file was moved
        private void UpdateRootEntries()
        {
            bool lastEntryWasLfn = false;

            // Detect what changed - each entry is 32 bytes
            for (int i = 0; i < 512; i += 0x20)
            {
                byte byte0 = writeBlock[i];
                byte attrs = writeBlock[i + 0xB];
                if (attrs == 0xf)
                {
                    lastEntryWasLfn = true;
                }
                else if (attrs != 8)
                {
                    // Get the cluster
                    int key = writeBlock[i + 0x1a] + (writeBlock[i + 0x1b] << 8);
                    if (GetFSType() == FSType.FAT32)
                    {
                        key += (writeBlock[i + 0x14] << 16) + (writeBlock[i + 0x15] << 24);
                    }
                    int size = writeBlock[i + 0x1c] + (writeBlock[i + 0x1d] << 8) + (writeBlock[i + 0x1e] << 16) + (writeBlock[i + 0x1f] << 24);
                    if (FAT.ContainsKey(key))
                    {
                        FileEntry entry = FAT[key];
                        // check if the file has been deleted
                        if (byte0 == 0xE5 && entry != null)
                        {
                            File.Delete(entry.fqpn);
                            FAT.Remove(key);
                            lastEntryWasLfn = false;
                            continue;
                        }

                        string name;
                        string ext;
                        if (lastEntryWasLfn)
                        {
                            GetNameAndExtFromLfnWriteBlock(i - 0x20, out name, out ext);
                        }
                        else
                        {
                            name = System.Text.Encoding.UTF8.GetString(writeBlock, i, 8);
                            ext = System.Text.Encoding.UTF8.GetString(writeBlock, i + 8, 3);
                        }

                        if (byte0 != 0xE5 && entry == voidEntry) // Check if we need to create an actual file on Windows to back it
                        {
                            entry.size = size;
                            FileInfo info = new FileInfo(entry.fqpn);
                            string newFileName = Path.Combine(info.DirectoryName, name.Trim() + "." + ext.Trim());
                            FileStream readStream = new FileStream(entry.fqpn, FileMode.Open, FileAccess.Read);
                            FileStream writeStream = new FileStream(newFileName, FileMode.CreateNew);
                            try
                            {
                                byte[] buffer = new byte[512];
                                for (int cluster = 0; cluster < entry.clusters; cluster++)
                                {
                                    readStream.Read(buffer, cluster * 512, 512);
                                    writeStream.Write(buffer, cluster * 512, (cluster == entry.clusters - 1) ? size % 512 : 512);
                                }
                                entry.fqpn = newFileName;
                                entry.shortname = name;
                            }
                            catch (Exception e)
                            {
                                // controller error
                                data[5] = 1;
                                System.Console.WriteLine(e.ToString());
                            }
                            finally
                            {
                                writeStream.Close();
                                readStream.Close();
                            }
                            File.Delete(info.FullName);
                            voidEntry = null;
                        }
                        lastEntryWasLfn = false;

                    }
                }
            }
            Array.Copy(writeBlock, 0, root, blockPtr, 512);
        }

        private void SetData(int page, byte[] buffer)
        {
            // Find the file in FAT
            FileEntry fEntry = null;
            int writeStartCluster = 0;
            foreach (int key in FAT.Keys)
            {
                FileEntry entry = FAT[key];
                if (page >= key && page <= key + entry.clusters)
                {
                    fEntry = entry;
                    writeStartCluster = key;
                    break;
                }
            }

            if (fEntry != null)
            {
                FileStream stream = new FileStream(fEntry.fqpn, FileMode.Open, FileAccess.Write);
                try
                {
                    stream.Seek((page - writeStartCluster) * 512, SeekOrigin.Begin);
                    stream.Write(buffer, 0, 512);
                }
                catch (Exception e)
                {
                    // controller error
                    data[5] = 1;
                    System.Console.WriteLine(e.ToString());
                }
                finally
                {
                    stream.Close();
                }
            }
        }

        private void CreateEmptyFile(int size, int page)
        {
            string path = GetSDCardPath();
            string randomFileName = Guid.NewGuid().ToString();
            string filename = Path.Combine(path, randomFileName + ".new");
            using (var fs = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                fs.SetLength(size);
            }
            voidEntry = new FileEntry
            {
                fqpn = filename,
                clusters = clusterCount
            };


            // calculate the first cluster offset
            int writeStartCluster = 0; // this is used when writing to file, to determine the cluster number
            switch (GetFSType())
            {
                case FSType.FAT16:
                    writeStartCluster = firstPtr / 2 - page * 256;
                    break;
                case FSType.FAT32:
                    writeStartCluster = firstPtr / 4 + page * 128;
                    break;
            }
            FAT.Add(writeStartCluster, voidEntry);
        }
    }

}
