namespace FoenixIDE.MemoryLocations
{
    // Flash for all F256 Class devices
    public class FlashF256 : FoenixIDE.MemoryLocations.MemoryRAM
    {
        public FlashF256(int StartAddress, int Length) : base(StartAddress, Length)
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
