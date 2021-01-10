using FoenixIDE.MemoryLocations;
using FoenixIDE.Simulator.Devices.SDCard;

namespace FoenixIDE.Simulator.Devices
{
    public enum FSType
    {
        FAT12, FAT16, FAT32
    };

    public abstract class SDCardDevice : MemoryRAM
    {
        public bool isPresent = false;
        private string SDCardPath = "";
        private bool ISOMode = false;
        private int capacity = 8; // Capacity in MB
        private int clusterSize = 512;
        private FSType fsType = FSType.FAT32;
        protected string sdCurrentPath = "";
        public delegate void SDCardInterruptEvent(CH376SInterrupt irq);
        public SDCardInterruptEvent sdCardIRQMethod;

        public SDCardDevice(int StartAddress, int Length) : base(StartAddress, Length)
        {
        }

        // Path
        public string GetSDCardPath()
        {
            return SDCardPath;
        }
        public void SetSDCardPath(string path)
        {
            SDCardPath = path;
        }

        // Capacity
        public int GetCapacity()
        {
            return capacity;
        }
        public virtual void SetCapacity(int value)
        {
            capacity = value;
        }

        // ISO mode
        public bool GetISOMode()
        {
            return ISOMode;
        }
        public virtual void SetISOMode(bool mode)
        {
            ISOMode = mode;
        }

        // Cluster Size
        public void SetClusterSize(int value)
        {
            clusterSize = value;
        }
        public int GetClusterSize()
        {
            return clusterSize;
        }

        // Filesystem Type
        public void SetFSType(FSType value)
        {
            fsType = value;

        }
        public FSType GetFSType()
        {
            return fsType;
        }

        public abstract void ResetMbrBootSector();
    }
}
