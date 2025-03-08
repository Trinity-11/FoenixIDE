namespace FoenixIDE.Simulator.Devices
{

    // DMA for all F256 class devices
    // F256 Jr, F256K, F256Kc, F256Ke
    public class DMA_F256 : MemoryLocations.MemoryRAM
    {
        private MemoryLocations.MemoryRAM System;

        public DMA_F256(int StartAddress, int Length) : base(StartAddress, Length)
        {
        }

        public void setSystemRam(MemoryLocations.MemoryRAM ram)
        {
            System = ram;
        }

        public override void WriteByte(int Address, byte Value)
        {
            data[Address] = Value;
            // The only address that matters is the register
            // If the Enable and Transfer bits are set then do the transfer
            if ((Address == 0) && (Value & 0x81) == 0x81)
            {
                // Read the Fill Byte
                bool isFillTransfer = (Value & 4) != 0;
                byte fillByte = 0;
                if (isFillTransfer)
                {
                    fillByte = ReadByte(1);
                }
                // Indicate that DMA is busy
                data[1] = 0x80;
                int srcAddr = ReadLong(4);
                int destAddr = ReadLong(8);

                bool is2DTransfer = (Value & 2) != 0;
                int size1DTransfer = ReadLong(0xC);

                // Setup variables
                int width2DTransfer = ReadWord(0xC);
                int height2DTransfer = ReadWord(0xE);
                int srcStride = ReadWord(0x10);
                int destStride = ReadWord(0x12);

                if (isFillTransfer)
                {
                    if (is2DTransfer)
                    {
                        // Copy the fillbyte in the rectangle
                        for (int y = 0; y < height2DTransfer; y++)
                        {
                            for (int x = 0; x < width2DTransfer; x++)
                            {
                                System.WriteByte(destAddr + x + y * destStride, fillByte);
                            }
                        }
                    }
                    else
                    {
                        // This is the easiest use case.  Just fill the same byte in to destination
                        for (int i = 0; i < size1DTransfer; i++)
                        {
                            System.WriteByte(destAddr + i, fillByte);
                        }
                    }
                }
                else
                {
                    if (is2DTransfer)
                    {
                        for (int y = 0; y < height2DTransfer; y++)
                        {
                            for (int x = 0; x < width2DTransfer; x++)
                            {
                                byte srcByte = System.ReadByte(srcAddr + x + y * srcStride);
                                System.WriteByte(destAddr + x + y * destStride, srcByte);
                            }
                        }
                    }
                    else
                    {
                        // Copy the memory from source to destination
                        byte[] buffer = new byte[size1DTransfer];
                        System.CopyIntoBuffer(srcAddr, size1DTransfer, buffer);
                        System.CopyBuffer(buffer, 0, destAddr, size1DTransfer);
                    }
                }

                // Set the status to not busy
                data[1] = 0;

                // Raise an interrupt
                if ((Value & 8) == 8)
                {
                
                }
            }
        }
    }
}
