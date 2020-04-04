using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nu64.Processor
{
    public class Instructions
    {
        public MemoryMap Memory = null;
        public Registers R = null;
        public Register DBR = null;
        public Register DP = null;
        public Register PBR = null;

        public Instructions(Registers newRegisters, MemoryMap newMemory)
        {
            this.Memory = newMemory;
            this.R = newRegisters;
            DBR = newRegisters.DBR;
            DP = newRegisters.D;
            PBR = newRegisters.PBR;
        }


        /// <summary>
        /// An address in Direct Page.
        /// </summary>
        /// <param name="Address">an 8-bit Direct Page address.</param>
        /// <returns></returns>
        public int AddressDirect(int Address)
        {
            return DP.Value + (Address & 0xff);
        }

        /// <summary>
        /// A 16-bit address on the current 64K bank
        /// </summary>
        /// <param name="Address">A 16-bit address</param>
        /// <returns></returns>
        public int AddressAbsolute(int Address)
        {
            return (DBR.Value << 16) + (Address & 0xffff);
            
        }

        /// <summary>
        /// A long, 24-bit address
        /// </summary>
        /// <param name="Address"></param>
        /// <returns></returns>
        public int AddressLong(int Address)
        {
            return Address & 0xffffff;
        }

        /// <summary>
        /// 8-bit address on Direct Page points to 16-bit pointer
        /// </summary>
        /// <param name="Pointer"></param>
        /// <returns></returns>
        public int AddressDirectIndirect(int Pointer)
        {
            int addr = AddressDirect(Pointer);
            int val = Memory[addr] + (Memory[addr + 1] << 8);
            return AddressAbsolute(addr);
        }

        /// <summary>
        /// 8-bit address on Direct Page points to 24-bit pointer
        /// </summary>
        /// <param name="Pointer"></param>
        /// <returns></returns>
        public int AddressDirectIndirectLong(int Pointer)
        {
            int addr = AddressDirect(Pointer);
            int val = Memory[addr] + (Memory[addr + 1] << 8) + (Memory[addr + 2] << 16);
            return AddressAbsolute(addr);
        }

        /// <summary>
        /// 16-bit address points to 16-bit pointer
        /// </summary>
        /// <param name="Pointer"></param>
        /// <returns></returns>
        public int AddressAbsoluteIndirect(int Pointer)
        {
            int addr = AddressAbsolute(Pointer);
            int val = Memory[addr] + (Memory[addr + 1] << 8);
            return AddressAbsolute(addr);
        }

        /// <summary>
        /// 16-bit address points to 24-bit poiner
        /// </summary>
        /// <param name="Pointer"></param>
        /// <returns></returns>
        public int AddressAbsoluteIndirectLong(int Pointer)
        {
            int addr = AddressAbsolute(Pointer);
            int val = Memory[addr] + (Memory[addr + 1] << 8) + (Memory[addr + 2] << 16);
            return AddressLong(addr);
        }

        public void LoadImmediate(Register Dest, int Value)
        {
            UInt16 v = (UInt16)Value;
            R.Flags.Negative = (v & 0x8000) == 0x8000;
            R.Flags.Zero = (v == 0);
            Dest.Value = v;
        }

        /// <summary>
        /// Load the register from Direct Page
        /// </summary>
        /// <param name="Dest"></param>
        /// <param name="Address8"></param>
        public void LoadDirect(Register Dest, int Address8)
        {
            int addr = AddressDirect(Address8);
            int val = Memory[addr];
            LoadImmediate(Dest, val);
        }

        /// <summary>
        /// Load the register from a 16-bit address
        /// </summary>
        /// <param name="Dest"></param>
        /// <param name="Address16"></param>
        public void LoadAbsolute(Register Dest, int Address16)
        {
            int addr = AddressAbsolute(Address16);
            int val = Memory[addr];
            LoadImmediate(Dest, val);
        }

        /// <summary>
        /// Load the register from a 24-bit address
        /// </summary>
        /// <param name="Dest"></param>
        /// <param name="Address24"></param>
        public void LoadLong(Register Dest, int Address24)
        {
            int addr = AddressAbsolute(Address24);
            int val = Memory[addr];
            LoadImmediate(Dest, val);
        }

  
    }
}
