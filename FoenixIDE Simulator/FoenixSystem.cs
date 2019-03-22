using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FoenixIDE.Basic;
using FoenixIDE.Processor;
using FoenixIDE.Monitor;
using FoenixIDE.Display;
using FoenixIDE.Common;
using System.Threading;
using FoenixIDE.MemoryLocations;

namespace FoenixIDE
{
    public class FoenixSystem
    {
        private const int TAB_WIDTH = 4;
        public MemoryManager Memory = null;
        public Processor.CPU CPU = null;
        public Gpu gpu = null;
        public MemoryBuffer KeyboardBuffer = null;
        public ColorCodes CurrentColor = ColorCodes.Green;
        public bool ConsoleEcho = false;

        public Basic.Immediate Basic = null;
        public Monitor.Monitor Monitor = null;

        public global::System.Timers.Timer TickTimer = new global::System.Timers.Timer();

        public ReadyHandler ReadyHandler = null;

        public DeviceEnum InputDevice = DeviceEnum.Keyboard;
        public DeviceEnum OutputDevice = DeviceEnum.Screen;

        public Thread CPUThread = null;

        public FoenixSystem(Gpu gpu)
        {
            Memory = new MemoryManager
            {
                RAM = new MemoryRAM(MemoryMap.RAM_START, MemoryMap.RAM_SIZE), // 2MB SRAM
                IO = new MemoryRAM(MemoryMap.IO_START, MemoryMap.IO_SIZE),   // 64K IO space
                VIDEO = new MemoryRAM(MemoryMap.VIDEO_START, MemoryMap.VIDEO_SIZE), // 4MB Video
                FLASH = new MemoryRAM(MemoryMap.FLASH_START, MemoryMap.FLASH_SIZE), // 8MB DRAM
            };
            this.CPU = new CPU(Memory);
            this.CPU.SimulatorCommand += CPU_SimulatorCommand;
            this.gpu = gpu;
            gpu.VRAM = Memory.VIDEO;
            gpu.RAM = Memory.RAM;
            gpu.IO = Memory.IO;
            gpu.LoadCharacterData();

            KeyboardBuffer = new MemoryBuffer(
                Memory.RAM,
                MemoryMap.KEY_BUFFER,
                MemoryMap.KEY_BUFFER_SIZE,
                MemoryMap.KEY_BUFFER_RPOS,
                MemoryMap.KEY_BUFFER_WPOS);

            for (int i = MemoryMap.SCREEN_PAGE0; i < MemoryMap.SCREEN_PAGE1; i++)
            {
                this.Memory[i] = 64;
            }

            this.Basic = new Basic.Immediate(this);
            this.Monitor = new Monitor.Monitor(this);
        }

        private void CPU_SimulatorCommand(int EventID)
        {
            switch (EventID)
            {
                case SimulatorCommands.RefreshDisplay:
                    gpu.RefreshTimer = 0;
                    break;
                default:
                    break;
            }
        }

        public void Reset()
        {
            CPU.Halt();

            Cls();
            gpu.Refresh();

            this.ReadyHandler = Monitor;
            HexFile.Load(Memory, @"ROMs\kernel.hex");

            // If the memory at 0 is still $FF, then copy from Page18 to Page00
            if (Memory.ReadByte(0xFF00) == 0)
            {
                Memory.RAM.Copy(0x180000, Memory.RAM, 0, MemoryMap.PAGE_SIZE);
            }
            CPU.Reset();

            //CPUTest test= new CPUTest(this);
            //test.BeginTest(0xf81000);

            this.TickTimer.Interval = 1000 / 60;
            this.TickTimer.Elapsed += TickTimer_Elapsed;
            this.TickTimer.Enabled = true;
        }

        private void PrintCopyright()
        {
            Y = 0;
            PrintTab(60);
            PrintLine("(c)2018 Tom Wilson");
            PrintTab(60);
            PrintLine("wilsontp@gmail.com");
        }

        private void TickTimer_Elapsed(object sender, global::System.Timers.ElapsedEventArgs e)
        {
            DoConsoleEcho();
        }

        public virtual void PrintChar(char c)
        {
            if (OutputDevice == DeviceEnum.Screen
                || OutputDevice == DeviceEnum.Keyboard)
            {
                PrintCharToScreen(c);
            }

            if (OutputDevice == DeviceEnum.DebugWindow)
            {
                UI.CPUWindow.PrintChar(c);
            }
        }

        public virtual void PrintCharToScreen(char c)
        {
            switch (c)
            {
                case (char)PETSCIICommandCodes.Up:
                    Y = Y - 1;
                    break;
                case (char)PETSCIICommandCodes.Down:
                    Y = Y + 1;
                    break;
                case (char)PETSCIICommandCodes.Left:
                    X = X - 1;
                    break;
                case (char)PETSCIICommandCodes.Right:
                    X = X + 1;
                    break;
                case (char)PETSCIICommandCodes.Home:
                    X = 0;
                    Y = 0;
                    break;
                case (char)PETSCIICommandCodes.Clear:
                    Fill(0x20);
                    X = 0;
                    Y = 0;
                    break;
                case '\x8': // backspace
                    PrintBackspace();
                    break;
                case '\t':
                    PrintTab();
                    break;
                case '\xa':
                    PrintLineFeed();
                    break;
                case '\xd':
                    PrintReturn();
                    break;
                case '\x12':
                    gpu.CurrentColor = gpu.CurrentColor | ColorCodes.Reverse;
                    break;
                case '\x92':
                    gpu.CurrentColor = gpu.CurrentColor & (int.MaxValue - ColorCodes.Reverse);
                    break;
                default:
                    Memory[MemoryMap.SCREEN_PAGE0 + gpu.CursorPos] = (byte)c;
                    //gpu.ColorData[gpu.CursorPos] = CurrentColor;
                    AdvanceCursor();
                    break;
            }
            gpu.ResetDrawTimer();
        }

        internal void Run()
        {
            CPU.Run();
        }

        /// <summary>
        /// Places the cursor at the specified column. The leftmost column is 0.
        /// </summary>
        /// <param name="Col"></param>
        public void PrintTab(int Col)
        {
            if (OutputDevice == DeviceEnum.Screen)
            {
                this.X = Col;
            }
            else if (OutputDevice == DeviceEnum.DebugWindow)
            {
                UI.CPUWindow.PrintTab(Col);
            }
        }

        private void PrintBackspace()
        {
            X = X - 1;
            if (X < 0)
                Y = Y - 1;
            Memory[gpu.CursorPos] = 0x20;
        }

        private void PrintTab()
        {
            int i = TAB_WIDTH - X % TAB_WIDTH;
            while (i > 0)
            {
                PrintChar(' ');
                i--;
            }
        }

        ColorCodes _currentForeground = ColorCodes.Green | ColorCodes.LightBlue;
        public ColorCodes CurrentForeground
        {
            get { return _currentForeground; }
            protected set { _currentForeground = value; }
        }

        ColorCodes _currentBackground = ColorCodes.Black;
        public ColorCodes CurrentBackground
        {
            get { return _currentBackground; }
            protected set { _currentBackground = value; }
        }

        public virtual void PrintChars(char[] Chars)
        {
            for (int i = 0; i < Chars.Length; i++)
                PrintChar(Chars[i]);
        }

        public void Scroll1()
        {
            int addr = MemoryMap.SCREEN_PAGE0;
            for (int c = 0; c < gpu.BufferSize - gpu.ColumnsVisible; c++)
            {
                for (int col = 0; col < gpu.ColumnsVisible; col++)
                {
                    Memory[addr + c] = Memory[addr + c + gpu.ColumnsVisible];
                    //gpu.ColorData[c] = gpu.ColorData[c + gpu.Columns];
                }
            }

            for (int c = gpu.BufferSize - gpu.ColumnsVisible; c < gpu.BufferSize; c++)
            {
                Memory[addr + c] = 0x20;
                //gpu.ColorData[c] = _currentForeground;
            }
        }

        public void AdvanceCursor()
        {
            if (X < Columns - 1)
                X += 1;
            else
                PrintLine();
        }

        public void PrintLineFeed()
        {
            if (OutputDevice != DeviceEnum.Screen)
            {
                PrintChar('\n');
            }
            else
            {
                if (Y < Lines - 1)
                    Y += 1;
                else
                {
                    Scroll1();
                    Y = Lines - 1;
                }
            }
        }

        public void PrintReturn()
        {
            if (OutputDevice != DeviceEnum.Screen)
            {
                PrintChar('\r');
            }
            else
            {
                X = 0;
            }
        }

        public void PrintLine()
        {
            if (OutputDevice != DeviceEnum.Screen)
            {
                PrintChar('\r');
                PrintChar('\n');
            }
            else
            {
                PrintReturn();
                PrintLineFeed();
            }
        }

        public virtual void PrintLine(string s)
        {
            Print(s);
            PrintReturn();
            PrintLineFeed();
        }

        public virtual void Print(string s)
        {
            for (int i = 0; i < s.Length; i++)
            {
                PrintChar(s[i]);
            }
        }

        /// <summary>
        /// Moves the cursor on the screen. This is zero-based
        /// </summary>
        /// <param name="Row">Row number, top of screen is 0</param>
        /// <param name="Col">Column, left side of screen is 0</param>
        public virtual void Locate(int Row, int Col)
        {
            if (Row < 0)
                Row = 0;
            if (Row >= Lines)
                Row = Lines - 1;
            if (Col < 0)
                Col = 0;
            if (Col >= Columns)
                Col = Columns - 1;

            Y = Row;
            X = Col;
        }

        public virtual void Cls()
        {
            Fill(0x20);
            Locate(0, 0);
        }

        public virtual void Fill(byte c)
        {
            int buffersize = gpu.BufferSize;
            for (int i = 0; i < buffersize; i++)
            {
                Memory[MemoryMap.SCREEN_PAGE0 + i] = c;
                //gpu.ColorData[i] = _currentForeground;
            }
        }

        public int GetCharPos(int row, int col)
        {
            return row * Columns + col;
        }

        public int GetStartOfLine()
        {
            return GetCharPos(Y, 0);
        }

        public void READY()
        {
            if (ReadyHandler != null)
                ReadyHandler.Ready();
            ConsoleEcho = true;
        }

        public void ReturnPressed(int pos)
        {
            PrintReturn();
            if (ReadyHandler != null)
                ReadyHandler.ReturnPressed(GetStartOfLine());
        }

        public int X
        {
            get { return gpu.X; }
            set { gpu.X = value; }
        }

        public int Y
        {
            get { return gpu.Y; }
            set { gpu.Y = value; }
        }

        public int Lines
        {
            get { return gpu.LinesVisible; }
        }

        public int Columns
        {
            get { return gpu.ColumnsVisible; }
        }

        public void DoConsoleEcho()
        {
            if (!ConsoleEcho)
                return;
            if (KeyboardBuffer.Count == 0)
                return;

            int c = KeyboardBuffer.Read(1);
            int shift = KeyboardBuffer.Read(1);
            int pos = GetStartOfLine();
            switch ((char)c)
            {
                case '\r':
                    ReturnPressed(pos);
                    break;
                default:
                    PrintChar((char)c);
                    break;
            }
        }

        public void PrintMemBinary(int Bytes, int Address)
        {
            for (int i = Bytes - 1; i >= 0; i--)
            {
                int b = Peek(Address + i);
                for (int j = 0; j < 8; j++)
                {
                    if ((b & 0x80) != 0)
                        Print("1");
                    else
                        Print("0");
                    b = b << 1;
                }
            }
        }

        public void PrintMemHex(int Bytes, int Address)
        {
            for (int i = Bytes - 1; i >= 0; i--)
            {
                byte b = Peek(Address + i);
                Print(b.ToString("X2"));
            }
        }

        public byte Peek(int bank, int Address)
        {
            return Memory[bank, Address];
        }

        public byte Peek(int Address)
        {
            return Memory[Address];
        }

        public void PrintGreeting()
        {
            PrintLine("         FoenixIDE Programming Simulator");
        }

        public void ShowFlag()
        {
            Locate(0, 0);
            //PrintLine(" \xec\xa0\xa8\xa8\xa0\xed");
            //PrintLine(" \xa0\xa6\xa6\xa6\xa6\xa0");
            //PrintLine(" \xdf\xa0\xf9\xf9\xa0\xa9");
            PrintLine(" \xf2\xee\xee\xee\xee\xf3\xa8");
            PrintLine(" \xA1FoenixIDE\xef\xa6");
            PrintLine(" \xf0\xa2\xa2\xa2\xa2\xf1\xa6");
            PrintLine("  \xf9\xf9\xf9\xf9\xf9\xf9");
        }

    }
}
