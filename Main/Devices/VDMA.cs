using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoenixIDE.Simulator.Devices
{
    public class VDMA : MemoryLocations.MemoryRAM
    {
        private MemoryLocations.MemoryRAM Video;

        public VDMA(int StartAddress, int Length) : base(StartAddress, Length)
        {
        }

        public void setVideoRam(MemoryLocations.MemoryRAM vram)
        {
            Video = vram;
        }

        public override void WriteByte(int Address, byte Value)
        {
            data[Address] = Value;
            // The only address that matters is the register
            // If the Enable and Transfer bits are set then do the transfer
            if (Address == 0 && (Value & 0x81) == 0x81)
            {
                int destAddr = ReadLong(5); // Address $AF:0405
                if ((Value & 4) == 4)
                {
                    // we're copying the same byte in all destination addresses
                    byte transferByte = ReadByte(1); // Address $AF:0401
                    
                    if ((Value & 2) == 0)
                    {
                        int size1DTransfer = ReadLong(8); // Address $AF:0408 - maximum 4MB
                        if (Video != null)
                        {
                            
                            for (int i = 0; i < size1DTransfer; i++)
                            {
                                Video.WriteByte(destAddr + i , transferByte);
                            }
                        }
                    }
                    else
                    {
                        int sizeX = ReadWord(8); // Max 65535
                        int sizeY = ReadWord(0xA); // Max 65535
                        int destStride = ReadWord(0xE) & 0xFFFE; // must be an even number
                        if (Video != null)
                        {
                            for (int y = 0; y < sizeY; y++)
                            {
                                for (int x = 0; x < sizeX; x++)
                                {
                                    Video.WriteByte(destAddr + x + y * destStride, transferByte);
                                }
                            }
                        }
                    }
                }
                else
                {
                    long srcAddr = ReadLong(2); // Address $AF:0402
                }

                // Raise an interrupt
                if ((Value & 8) == 8)
                {
                
                }
            }
        }
    }
}
