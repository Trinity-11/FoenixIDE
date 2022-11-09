using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FoenixIDE;
using FoenixIDE.MemoryLocations;

namespace FoenixIDE.Processor
{
    /// <summary>
    /// Operations. This class encompasses the CPU operations and the support routines needed to execute
    /// the operations. Execute reads a single opcode from memory, along with its data bytes, then 
    /// executes that single step. The virtual CPU state is retained until Execute is called again. 
    /// </summary>
    public partial class CPU
    {
        const int BANKSIZE = 0x10000;
        const int PAGESIZE = 0x100;
        private OpcodeList opcodes = null;

        public OpCode CurrentOpcode = null;
        public int SignatureBytes = 0;

        public CPUPins Pins = new CPUPins();

        /// <summary>
        /// When true, the CPU will not execute the next instruction. Used by the debugger
        /// to allow the user to analyze memory and the execution trace. 
        /// </summary>
        public bool DebugPause = false;

        /// <summary>
        /// Length of the currently executing opcode
        /// </summary>
        public int OpcodeLength;
        /// <summary>
        /// Number of clock cycles used by the currently exeucting instruction
        /// </summary>
        public int OpcodeCycles;

        /// <summary>
        ///  The virtual CPU speed
        /// </summary>
        private int clockSpeed = 14000000;
        /// <summary>
        /// number of cycles since the last performance checkpopint
        /// </summary>
        private int clockCyles = 0;
        /// <summary>
        /// the number of cycles to pause at until the next performance checkpoint
        /// </summary>
        private long nextCycleCheck = long.MaxValue;
        /// <summary>
        /// The last time the performance was checked. 
        /// </summary>
        private DateTime checkStartTime = DateTime.Now;

        public MemoryManager MemMgr = null;
        public Thread CPUThread = null;

        public event Operations.SimulatorCommandEvent SimulatorCommand;

        public const int DefaultStackValueNative = 0xd6ff;
        public const int DefaultStackValueEmulation = 0x01ff;

        public int ClockSpeed
        {
            get
            {
                return this.clockSpeed;
            }

            set
            {
                this.clockSpeed = value;
            }
        }

        public int[] Snapshot
        {
            get
            {
                int[] snapshot = new int[8];
                snapshot[0] = PC;
                snapshot[1] = A.Value;
                snapshot[2] = X.Value;
                snapshot[3] = Y.Value;
                snapshot[4] = Stack.Value;
                snapshot[5] = DataBank.Value;
                snapshot[6] = DirectPage.Value;
                snapshot[7] = Flags.Value;
                return snapshot;
            }
        }

        public CPU(MemoryManager mm, int clock, bool is6502)
        {
            MemMgr = mm;
            clockSpeed = clock;
            clockCyles = 0;
            Operations operations = new Operations(this);
            operations.SimulatorCommand += Operations_SimulatorCommand;
            opcodes = new OpcodeList(operations, this, is6502);
            Flags.Emulation = true;
        }

        private void Operations_SimulatorCommand(int EventID)
        {
            switch (EventID)
            {
                case SimulatorCommands.WaitForInterrupt:
                    break;
                case SimulatorCommands.RefreshDisplay:
                    SimulatorCommand?.Invoke(EventID);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Execute for n cycles, then return. This gives the host a chance to do other things in the meantime.
        /// </summary>
        /// <param name="Cycles"></param>
        public void ExecuteCycles(int Cycles)
        {
            ResetCounter(Cycles);
            while (clockCyles < nextCycleCheck && !DebugPause)
            {
                ExecuteNext();
            }
        }

        /// <summary>
        /// Execute a single 
        /// return true if an interrupt is present
        /// </summary>
        public bool ExecuteNext()
        {
            if (Pins.Ready_)
                return false;
            

            if (this.Pins.GetInterruptPinActive)
            {
                if (ServiceHardwareInterrupt())
                {
                    return true;
                }
            }

            if (Waiting)
            {
                return false;
            }

            // TODO - if pc > RAM size, then throw an exception
            CurrentOpcode = opcodes[MemMgr.ReadByte(PC)];
            if (CurrentOpcode != null)
            {
                OpcodeLength = CurrentOpcode.Length;
                OpcodeCycles = 1;
                SignatureBytes = ReadSignature(OpcodeLength, PC);

                PC += OpcodeLength;

                CurrentOpcode.Execute(SignatureBytes);
                clockCyles += OpcodeCycles;
                return false;
            }
            else
            {
                return true;
            }
        }

        // Simulator State management 
        /// <summary>
        /// Pause the CPU execution due to a STP instruction. The CPU may only be restarted
        /// by the Reset pin. In the simulator, this will close the CPU execution thread.
        /// Restart the CPU by executing Reset() and then Run()
        /// </summary>
        public void Halt()
        {
            if (CPUThread != null && CPUThread.ThreadState == ThreadState.Running)
            {
                DebugPause = true;
                CPUThread = null;
            }
        }

        public void Reset()
        {
            Pins.VectorPull = true;
            MemMgr.VectorPull = true;

            SetEmulationMode();
            Flags.Value = 0;
            A.Value = 0;
            X.Value = 0;
            Y.Value = 0;
            Stack.Value = Flags.Emulation ? DefaultStackValueEmulation : DefaultStackValueNative;
            Stack.TopOfStack = Stack.Value;

            DataBank.Value = 0;
            DirectPage.Value = 0;
            //ProgramBank.Value = 0;

            PC = MemMgr.ReadWord(MemoryMap.VECTOR_ERESET);

            Flags.IrqDisable = true;
            Pins.IRQ = false;
            Pins.VectorPull = false;
            MemMgr.VectorPull = false;
            Waiting = false;
        }

        /// <summary>
        ///  Sets the registers to 8 bits. Sets the emulation flag.
        /// </summary>
        public void SetEmulationMode()
        {
            Flags.Emulation = true;
            A.Width = 1;
            A.DiscardUpper = false;
            X.Width = 1;
            X.DiscardUpper = true;
            Y.Width = 1;
            Y.DiscardUpper = true;
        }

        /// <summary>
        ///  Sets the width of the A, X, and Y registers based on the X and M flags.
        /// </summary>
        public void SyncFlags()
        {
            if (Flags.Emulation)
            {
                Flags.accumulatorShort = true;
                Flags.xRegisterShort = true;
            }
            A.Width = Flags.accumulatorShort? 1 : 2;
            X.Width = Flags.xRegisterShort? 1 : 2;
            Y.Width = Flags.xRegisterShort? 1 : 2;
        }

        /// <summary>
        /// Fetch and decode the next instruction for debugging purposes
        /// </summary>
        public OpCode PreFetch()
        {
            return opcodes[MemMgr[PC]];
        }

        public int ReadSignature(int length, int pc)
        {
            switch (length)
            {
                case 2:
                    return MemMgr.ReadByte(pc + 1);
                case 3:
                    return MemMgr.ReadWord(pc + 1);
                case 4:
                    return MemMgr.ReadLong(pc + 1);
            }

            return 0;
        }

        /// <summary>
        /// Clock cycles used for performance counte This will be periodically reset to zero
        /// as the throttling routine adjusts the system performance. 
        /// </summary>
        public int CycleCounter
        {
            get { return this.clockCyles; }
        }

        #region support routines
        /// <summary>
        /// Gets the address pointed to by a pointer in the data bank.
        /// </summary>
        /// <param name="baseAddress"></param>
        /// <param name="Index"></param>
        /// <returns></returns>
        private int GetPointerLocal(int baseAddress, Register Index = null)
        {
            int addr = DataBank.GetLongAddress(baseAddress);
            if (Index != null)
                addr += Index.Value;
            return addr;
        }

        /// <summary>
        /// Gets the address pointed to by a pointer in Direct page.
        /// be in the Direct Page. The address returned will be DBR+Pointer.
        /// </summary>
        /// <param name="baseAddress"></param>
        /// <param name="Index"></param>
        /// <returns></returns>
        private int GetPointerDirect(int baseAddress, Register Index = null)
        {
            int addr = DirectPage.Value + baseAddress;
            if (Index != null)
                addr += Index.Value;
            int pointer = MemMgr.ReadWord(addr);
            return DataBank.GetLongAddress(pointer);
        }

        /// <summary>
        /// Gets the address pointed to by a pointer referenced by a long address.
        /// </summary>
        /// <param name="baseAddress">24-bit address</param>
        /// <param name="Index"></param>
        /// <returns></returns>
        private int GetPointerLong(int baseAddress, Register Index = null)
        {
            int addr = baseAddress;
            if (Index != null)
                addr += Index.Value;
            return DataBank.GetLongAddress(MemMgr.ReadWord(addr));
        }

        #endregion

        /// <summary>
        /// Change execution to anohter address in the same bank
        /// </summary>
        /// <param name="addr"></param>
        public void JumpShort(int addr)
        {
            PC = (PC & 0xFF_0000) + (addr & 0xFFFF);
        }

        /// <summary>
        /// Change execution to a 24-bit address
        /// </summary>
        /// <param name="addr"></param>
        public void JumpLong(int addr)
        {
            //ProgramBank.Value = addr >> 16;
            // PC.Value = addr;
            PC = addr;
        }

        public void JumpVector(int VectorAddress)
        {
            int addr = MemMgr.ReadWord(VectorAddress);
            //ProgramBank.Value = 0;
            //PC.Value = addr;
            PC = addr;
        }

        public byte GetByte(int Value, int Offset)
        {
            if (Offset == 0)
                return (byte)(Value & 0xff);
            if (Offset == 1)
                return (byte)(Value >> 8 & 0xff);
            if (Offset == 2)
                return (byte)(Value >> 16 & 0xff);

            throw new Exception("Offset must be 0-2. Got " + Offset.ToString());
        }

        public void Push(int value, int bytes)
        {
            if (bytes < 1 || bytes > 3)
                throw new Exception("bytes must be between 1 and 3. Got " + bytes.ToString());

            Stack.Value -= bytes;
            MemMgr.Write(Stack.Value + 1, value, bytes);
        }

        public void Push(Register Reg, int Offset)
        {
            Push(Reg.Value + Offset, Reg.Width);
        }

        public void Push(Register Reg)
        {
            Push(Reg.Value, Reg.Width);
        }

        public int Pull(int bytes)
        {
            if (bytes < 1 || bytes > 3)
                throw new Exception("bytes must be between 1 and 3. got " + bytes.ToString());

            int ret = MemMgr.Read(Stack.Value + 1, bytes);
            
            Stack.Value += bytes;
            return ret;
        }

        public void PullInto(Register Register)
        {
            Register.Value = Pull(Register.Width);
        }

        /// <summary>
        /// Service highest priority interrupt
        /// </summary>
        public bool ServiceHardwareInterrupt()
        {
            if (Pins.IRQ && !Flags.IrqDisable)
            {
                //DebugPause = true;
                Pins.IRQ = false;
                Interrupt(InteruptTypes.IRQ);
                return true;
            }
            else if (Pins.NMI)
            {
                DebugPause = true;
                Pins.NMI = false;
                Interrupt(InteruptTypes.NMI);
                return true;
            }
            else if (Pins.Abort)
            {
                DebugPause = true;
                Pins.Abort = false;
                Interrupt(InteruptTypes.ABORT);
                return true;
            }
            else if (this.Pins.Reset)
            {
                DebugPause = true;
                Pins.Reset = false;
                Interrupt(InteruptTypes.RESET);
                return true;
            }
            return false;
        }

        public void Interrupt(InteruptTypes T)
        {
            //debug
            //DebugPause = true;

            if (!Flags.Emulation)
            {
                Push(PC & 0xFF_FFFF, 3);
            }
            else
            {
                Push(PC & 0xFFFF, 2);
            }
            
            Push(Flags);

            int addr;
            int emuAddr;
            Flags.IrqDisable = true;
            Flags.Decimal = false;

            switch (T)
            {
                case InteruptTypes.BRK:
                    addr = MemoryMap.VECTOR_BRK;
                    emuAddr = MemoryMap.VECTOR_EBRK;
                    break;
                case InteruptTypes.ABORT:
                    emuAddr = MemoryMap.VECTOR_EABORT;
                    addr = MemoryMap.VECTOR_ABORT;
                    break;
                case InteruptTypes.IRQ:
                    emuAddr = MemoryMap.VECTOR_EIRQ;
                    addr = MemoryMap.VECTOR_IRQ;
                    break;
                case InteruptTypes.NMI:
                    emuAddr = MemoryMap.VECTOR_ENMI;
                    addr = MemoryMap.VECTOR_NMI;
                    break;
                case InteruptTypes.RESET:
                    emuAddr = MemoryMap.VECTOR_ERESET;
                    addr = MemoryMap.VECTOR_RESET;
                    break;
                case InteruptTypes.COP:
                    emuAddr = MemoryMap.VECTOR_ECOP;
                    addr = MemoryMap.VECTOR_COP;
                    break;
                default:
                    throw new Exception("Invalid interrupt type: " + T.ToString());
            }
            Waiting = false;
            if (Flags.Emulation)
                JumpVector(emuAddr);
            else
                JumpVector(addr);
        }

        public void ResetCounter(int maxCycles)
        {
            clockCyles = 0;
            nextCycleCheck = maxCycles;
            checkStartTime = DateTime.Now;
        }
    }
}
