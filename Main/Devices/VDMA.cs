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
        private MemoryLocations.MemoryRAM Vicky;
        private MemoryLocations.MemoryRAM Video;

        public VDMA(int StartAddress, int Length) : base(StartAddress, Length)
        {
        }

        public void setVideoRam(MemoryLocations.MemoryRAM ram)
        {
            Video = ram;
        }

        public void setSystemRam(MemoryLocations.MemoryRAM ram)
        {
            System = ram;
        }

        public void setVickyRam(MemoryLocations.MemoryRAM ram)
        {
            Vicky = ram;
        }


        public override void WriteByte(int Address, byte Value)
        {
            data[Address] = Value;
            // The only address that matters is the register
            // If the Enable and Transfer bits are set then do the transfer
            if ((Address == 0 || Address == 0x20) && (Value & 0x81) == 0x81)
            {
                MemoryLocations.MemoryRAM srcMemory = null;
                MemoryLocations.MemoryRAM destMemory = null;
                int srcAddr = 0;
                int destAddr = 0;
                bool isSystemSource = (Address == 0 && (Value & 0x10) != 0) || (Address == 0x20);
                bool isSystemDest = (Address == 0 && (Value & 0x20) != 0) || (Address == 0x20 && (Value & 0x10) == 0);
                // RAM to VRAM is initiated by SDMA
                if (isSystemSource && !isSystemDest && Address == 0)
                {
                    return;
                }
                bool isIODest = (Value & 0x30) != 0;
                bool isFillTransfer = (Value & 4) != 0;
                bool isSrcTransfer2D = false;
                bool isDestTransfer2D = false;

                // Setup variables
                int sizeSrcX = isSystemSource ? ReadWord(0x28) : ReadWord(8); // Max 65535
                int sizeSrcY = isSystemSource ? ReadWord(0x2A) : ReadWord(0xA); // Max 65535
                int sizeDestX = isSystemDest ? ReadWord(0x28) : ReadWord(8); // Max 65535
                int sizeDestY = isSystemDest ? ReadWord(0x2A) : ReadWord(0xA); // Max 65535
                int srcStride = (isSystemSource ? ReadWord(0x2C) : ReadWord(0xC)) & 0xFFFE; // must be an event number
                int destStride = (isSystemDest ? ReadWord(0x2E) : ReadWord(0xE)) & 0xFFFE; // must be an even number
                // if stride is zero, read data linearly
                srcStride = srcStride == 0 ? sizeSrcX : srcStride;
                // if stride is zero, write data linearly
                destStride = destStride == 0 ? sizeSrcX : destStride;


                // Check if the source is system or video
                if (isSystemSource)
                {
                    srcAddr = ReadLong(0x22); // Address $AF:0422
                    srcMemory = srcAddr < 0x40_0000 ? System : Vicky;
                    isSrcTransfer2D = (ReadByte(0x20) & 2) != 0;
                }
                else
                {
                    srcMemory = Video;
                    srcAddr = ReadLong(2); // Address $AF:0402
                    isSrcTransfer2D = (ReadByte(0) & 2) != 0;
                }
                if (isSystemDest)
                {
                    destAddr = ReadLong(0x25); // Address $AF:0425
                    destMemory = destAddr < 0x40_0000 ? System : Vicky;
                    
                    if (destMemory == Vicky)
                    {
                        destAddr -= Vicky.StartAddress;
                    }
                    isDestTransfer2D = (ReadByte(0x20) & 2) != 0;
                }
                else
                {
                    destMemory = Video;
                    destAddr = ReadLong(5); // Address $af:0405
                    isDestTransfer2D = (ReadByte(0) & 2) != 0;
                }

                // Check for fill transfer
                if (isFillTransfer)
                {
                    // we're copying the same byte in all destination addresses
                    byte transferByte = ReadByte(1); // Address $AF:0401

                    // Linear or 2D
                    if (!isDestTransfer2D) 
                    {
                        int size1DTransfer = isSystemDest ? ReadLong(0x28) : ReadLong(8); // Address $AF:0408 - maximum 4MB
                        if (destMemory != null)
                        {
                            for (int i = 0; i < size1DTransfer; i++)
                            {
                                destMemory.WriteByte(destAddr + i, transferByte);
                            }
                        }
                    }
                    else
                    {
                        for (int y = 0; y < sizeDestY; y++)
                        {
                            for (int x = 0; x < sizeDestX; x++)
                            {
                                int srcPos = x + y * srcStride;
                                int destY = srcPos / sizeDestX;
                                int destX = srcPos % sizeDestX;
                                destMemory.WriteByte(destAddr + x + y * destStride, transferByte);
                            }
                        }
                    }
                }
                else
                {
                    // Load source data in buffer
                    byte[] buffer;
                    if (!isSrcTransfer2D)
                    {
                        int size1DTransfer = isSystemSource ? ReadLong(0x28) : ReadLong(0x8); // Address $AF:0408 - maximum 4MB
                        buffer = new byte[size1DTransfer];
                        srcMemory.CopyIntoBuffer(srcAddr, size1DTransfer, buffer);
                    }
                    else
                    {
                        buffer = new byte[sizeSrcX * sizeSrcY];
                        int ptr = 0;
                        for (int y = 0; y < sizeSrcY; y++)
                        {
                            for (int x = 0; x < sizeSrcX; x++)
                            {
                                byte data = srcMemory.ReadByte(srcAddr + x + y * srcStride);
                                buffer[ptr++] = data;
                            }
                        }
                    }
                    
                    // Transfer data from memory to VRAM
                    if (!isDestTransfer2D)
                    {
                        if (destMemory != null)
                        {
                            destMemory.CopyBuffer(buffer, 0, destAddr, buffer.Length);
                        }

                    }
                    else
                    {
                        int ptr = 0;
                        for (int y = 0; y < sizeDestY; y++)
                        {
                            for (int x = 0; x < sizeDestX; x++)
                            {
                                byte data = buffer[ptr++];
                                destMemory.WriteByte(destAddr + x + y * destStride, data);
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
