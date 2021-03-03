using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoenixIDE.Simulator.Devices
{
    public class UART: MemoryLocations.MemoryRAM
    {
        private const int RxTxBuffer = 0;
        private const int InterruptEnable = 1;
        private const int IRQFIFO = 2;
        private const int LineControl = 3;
        private const int ModemControl = 4;
        private const int LineStatus = 5;
        private const int ModemStatus = 6;
        private const int ScratchPad = 7;

        public int Bits = 5;
        public enum StopBits
        {
            Stop1,
            Stop1_5,
            Stop2
        }
        public StopBits StopBitsValue = StopBits.Stop1;
        public bool Parity = false;
        public bool EvenParitySelect = false;
        public bool StickParity = false;
        public bool BreakControl = false;
        public bool DivisorLatchAcces = false;

        public bool XMITFIFOEnabled = false;
        public int TriggerLevel = 0;
        public int TxCounter = 0;
        public int RxCounter = 0;

        public delegate void TransmitByteFunction(byte value);
        public TransmitByteFunction TransmitByte;

        public UART(int StartAddress, int Length) : base(StartAddress, Length)
        {
        }

        public override void WriteByte(int Address, byte Value)
        {
            data[Address] = Value;
            switch (Address)
            {
                case RxTxBuffer:
                    TxCounter++;
                    TransmitByte?.Invoke(Value);
                    break;
                case InterruptEnable:
                    // bit 0 - Received Data Available IRQ
                    // bit 1 - Transmitter Holding Register Empty IRQ
                    // bit 2 - Received Line Status IRQ
                    // bit 3 - MODEM Status IRQ
                    // bits 4 - 7 : always 0
                    break;
                case IRQFIFO:
                    // bit 0 - XMIT and RCVR FIFO enabled
                    XMITFIFOEnabled = (Value & 1) == 1;
                    if ((Value & 2) == 2)
                    {
                        ClearRcvrFIFO();
                    }
                    if ((Value & 4) == 4)
                    {
                        ClearXmitFIFO();
                    }
                    // bit 3,4,5 are ignored
                    TriggerLevel = (Value & 0xC0) >> 5;

                    break;
                case LineControl:
                    // bit 0-1 Serial bits
                    SetBits((byte)(Value & 3));
                    // bit 2 - stop bits
                    SetStopBits((byte)(Value & 4));
                    // bit 3 - parity
                    Parity = (Value & 8) == 8;
                    // bit 4 - even parity select
                    EvenParitySelect = (Value & 0x10) == 0x10;
                    StickParity = (Value & 0x20) == 0x20;
                    BreakControl = (Value & 0x40) == 0x40;
                    DivisorLatchAcces = (Value & 0x80) == 0x80;
                    break;
                case ModemControl:
                    break;
                case LineStatus:
                    break;
                case ModemStatus:
                    break;
                case ScratchPad:
                    break;
            }

        }
        private void SetBits(byte value)
        {
            Bits = 5 + value;
        }
        private void SetStopBits(byte value)
        {
            if (value == 0)
            {
                StopBitsValue = StopBits.Stop1;
            }
            else
            {
                StopBitsValue = Bits == 5 ? StopBits.Stop1_5 : StopBits.Stop2;
            }
        }
        private void ClearRcvrFIFO()
        {
            // Clear the Rx buffer
            // Reset Rx counter to 0
            RxCounter = 0;
        }
        private void ClearXmitFIFO()
        {
            // Clear the Tx buffer
            // Reset Tx counter to 0
            TxCounter = 0;
        }

        public override byte ReadByte(int Address)
        {
            switch(Address)
            {
                case LineStatus:
                    return 0x21;
                default:
                    return data[Address];
            }
        }
    }
}
