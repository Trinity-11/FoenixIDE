using FoenixIDE.MemoryLocations;
using FoenixIDE.Simulator.Devices.SDCard;

namespace FoenixIDE.Simulator.Devices
{
    public class SDCardDevice : MemoryRAM
    {
        public bool isPresent = false;
        private string SDCardPath = "";
        private int capacity = 8; // Capacity in MB
        protected string sdCurrentPath = "";
        public delegate void SDCardInterruptEvent(CH376SInterrupt irq);
        public SDCardInterruptEvent sdCardIRQMethod;

        public SDCardDevice(int StartAddress, int Length) : base(StartAddress, Length)
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
        public int GetCapacity()
        {
            return capacity;
        }
        public virtual void SetCapacity(int value)
        {
            capacity = value;
        }
    }
}
