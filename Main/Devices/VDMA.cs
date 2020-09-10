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
        private MemoryLocations.MemoryRAM System;
        private MemoryLocations.MemoryRAM Video;

        public VDMA(int StartAddress, int Length) : base(StartAddress, Length)
        {
        }

        public void setVideoRam(MemoryLocations.MemoryRAM vram)
        {
            Video = vram;
        }

        public void setSystemRam(MemoryLocations.MemoryRAM vram)
        {
            System = vram;
        }


        public override void WriteByte(int Address, byte Value)
        {
            data[Address] = Value;
            // The only address that matters is the register
            // If the Enable and Transfer bits are set then do the transfer
            if (Address == 0 && (Value & 0x81) == 0x81)
            {
                MemoryLocations.MemoryRAM srcMemory = null;
                MemoryLocations.MemoryRAM destMemory = null;
                int srcAddr = 0;
                int destAddr = 0;
                // Check if the source is system or video
                if ((Value & 0x10) != 0)
                {
                    srcMemory = System;
                    srcAddr = ReadLong(0x22); // Address $AF:0422
                }
                else
                {
                    srcMemory = Video;
                    srcAddr = ReadLong(2); // Address $AF:0402
                }
                if ((Value & 0x20) != 0)
                {
                    destMemory = System;
                    destAddr = ReadLong(0x25); // Address $AF:0425
                }
                else
                {
                    destMemory = Video;
                    destAddr = ReadLong(5); // Address $af:0405
                }

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
                                destMemory.WriteByte(destAddr + i, transferByte);
                            }
                        }
                    }
                    else
                    {
                        int sizeX = ReadWord(8); // Max 65535
                        int sizeY = ReadWord(0xA); // Max 65535
                        int destStride = ReadWord(0xE) & 0xFFFE; // must be an even number
                        if (destMemory != null)
                        {
                            for (int y = 0; y < sizeY; y++)
                            {
                                for (int x = 0; x < sizeX; x++)
                                {
                                    destMemory.WriteByte(destAddr + x + y * destStride, transferByte);
                                }
                            }
                        }
                    }
                }
                else
                {
                    // Transfer data from memory to VRAM
                    if ((Value & 2) == 0)
                    {
                        int size1DTransfer = ReadLong(8); // Address $AF:0408 - maximum 4MB
                        byte[] buffer = new byte[size1DTransfer];
                        if (srcMemory != null)
                        {
                            srcMemory.CopyIntoBuffer(srcAddr, buffer, 0, size1DTransfer);
                        }
                        if (destMemory != null)
                        {
                            destMemory.CopyBuffer(buffer, 0, destAddr, size1DTransfer);
                        }

                    }
                    else
                    {
                        int sizeX = ReadWord(8); // Max 65535
                        int sizeY = ReadWord(0xA); // Max 65535
                        int srcStride = ReadWord(0xC) & 0xFFFE; // must be an event number
                        int destStride = ReadWord(0xE) & 0xFFFE; // must be an even number
                        if (srcMemory != null && destMemory != null)
                        {
                            for (int y = 0; y < sizeY; y++)
                            {
                                for (int x = 0; x < sizeX; x++)
                                {
                                    byte data = srcMemory.ReadByte(x + y * (srcStride == 0 ? sizeX : srcStride));
                                    destMemory.WriteByte(destAddr + x + y * (destStride == 0 ? sizeX : destStride), data);
                                }
                            }
                        }
                    }
                }

                // Raise an interrupt
                if ((Value & 8) == 8)
                {
                
                }
            }
        }
    }
}
