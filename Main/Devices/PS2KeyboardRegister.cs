using FoenixIDE.MemoryLocations;
using System;

namespace FoenixIDE.Simulator.Devices
{
    public abstract class PS2KeyboardRegister: IMappable
    {   
        protected byte[] data;

        public delegate void TriggerInterruptDelegate();
        public TriggerInterruptDelegate TriggerKeyboardInterrupt;
        public TriggerInterruptDelegate TriggerMouseInterrupt;

        public int StartAddress { get; }
        public int Length { get; }
        public int EndAddress { get; }

        public PS2KeyboardRegister(int StartAddress, int Length)
        {
            this.StartAddress = StartAddress;
            this.Length = Length;
            this.EndAddress = StartAddress + Length - 1;
               
            data = new byte[Length];
        }

        public abstract byte ReadByte(int Address);

        public abstract void WriteByte(int Address, byte Data);

        public abstract void MousePackets(byte buttons, byte X, byte Y);

        public abstract void WriteScanCodeSequence(byte[] codes, int seqLength);

        public void CopyBuffer(byte[] src, int srcAddress, int destAddress, int length)
        {
            throw new NotImplementedException();
        }

        public void CopyIntoBuffer(int srcAddress, int srcLength, byte[] buffer, int offset)
        {
            throw new NotImplementedException();
        }
    }
}
