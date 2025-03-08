namespace FoenixIDE.Simulator.Devices
{

    /**
     * The SOL Register in F256 allows for 
     * - writing values for LINT_CTRL and LINT_L
     * - reading values for RAST_COL and RAST_ROW
     * 
     */
    public class SOL : MemoryLocations.MemoryRAM
    {
        bool lineInterrupt = false;
        int lineNumber = 0;  // 12 bit value for the line number to raise the interrupt on

        int rasterColum = 0;
        int rasterRow = 0;

        public SOL(int StartAddress, int Length) : base(StartAddress, Length)
        {
        }

        public override byte ReadByte(int Address)
        {
            switch (Address)
            {
                case 0:
                    return (byte)(rasterColum & 0xFF);
                case 1:
                    return (byte)(rasterColum >> 8);
                case 2:
                    return (byte)(rasterRow & 0xFF);
                case 3:
                    return (byte)(rasterRow >> 8);
            }
            return 0;
        }
        public override void WriteByte(int Address, byte Value)
        {
            switch (Address)
            {
                case 0:
                    lineInterrupt = (Value & 1) > 0;
                    break;
                case 1:
                    lineNumber |= Value;
                    break;
                case 2:
                    lineNumber |= (Value << 8);
                    break;
            }
        }

        // This method is called from the GPU class while drawing.
        public void SetRasterRow(int value)
        {
            rasterRow = value;
        }

        public bool IsInterruptEnabled()
        {
            return lineInterrupt;
        }

        public int GetSOLLineNumber()
        {
            return lineNumber;
        }

    }
}
