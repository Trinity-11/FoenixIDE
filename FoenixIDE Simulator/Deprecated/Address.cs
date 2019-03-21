using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoenixIDE
{
    public struct Address24
    {
        public byte Bank;
        public UInt16 Address;

        public Address24(int NewAddress)
        {
            if (NewAddress > 0xffffff || NewAddress < 0)
                throw new ArgumentException("Address must be an unsigned, 24-bit number.");

            Bank = (byte)(NewAddress & 0xff0000 >> 16);
            this.Address = (UInt16)(NewAddress & 0xffff);
        }

        public Address24(byte NewBank, byte NewAddress)
        {
            this.Bank = NewBank;
            this.Address = NewAddress;
        }

        public override string ToString()
        {
            return Bank.ToString("X2") + ":" + Address.ToString("X4");
        }

        public int ToInt()
        {
            return Bank * 0x10000 + Address;
        }
    }

}
