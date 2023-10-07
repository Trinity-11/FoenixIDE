namespace FoenixIDE.MemoryLocations
{
    public class FlashJr : FoenixIDE.MemoryLocations.MemoryRAM
    {
        public FlashJr(int StartAddress, int Length) : base(StartAddress, Length)
        {
        }

        public void SetFlash(int Address, byte Value)
        {
            data[Address] = Value;
        }

        public override void WriteByte(int Address, byte Value)
        {
            // CPU write of flash memory is not allowed.
        }
    }
}
