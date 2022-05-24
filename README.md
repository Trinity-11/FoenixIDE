# FoenixIDE
Development and Debugging Suite for the C256 Foenix Family of Computers.

This project is derived from the Nu64 emulator built by Tom Wilson.  Nu64 is owned by Tom Wilson. 
FoenixIDE is owned by the C256 team, led by Stefany Allaire.

This is a virtual WDC 65816 powered computer. It is intended to be used as a software development tool and learning platform for assembly language and BASIC programming. The software will be relesed with an open source license and may be used for whatever purpose you want, including personal projects, education, and commercial use. 

The C256 is loosely based around the concept of the world's most popular computer, the Commodore 64. When you turn on the system, you see a BASIC screen, with the READY prompt. You can type in BASIC commands, enter programs, or load programs from disk, tape or cartridge. 

The FoenixIDE attempt to replicate this experience on a modern computer: you will be able to write and run BASIC programs, 65816 assembler programs, and save programs to disk to trade with other people. The Nu64 will feature an 80 column text screen, several different graphic resolutions, and the ability to communicate with hardware and other computers through a virtual modem. 

For more information on the project, go to [C256 Foenix Computer](https://www.c256foenix.com/).

The FoenixIDE is a work in progress.

The C256 Foenix and its associated materials are owned by Stefany Allaire. There are three versions of the Foenix: 
* RevB (2 MB RAM, 2xOPL2)
* RevC - aka FMX (4 MB RAM, OPL3, OPN2, OPM and SN76489)
* RevU - 1 MB RAM
* RevU+ - 2 MB RAM

The FoenixIDE looks for kernel.hex and kernel.lst files in the ROMs folder colocated with the executable.  Obtain the Kernel from the sister repository: https://github.com/Trinity-11/Kernel_FMX

# Keyboard shortcuts 
## Main Window
* Shift+F11 - toggle fullscreen
* Shift+F5 - Run, or Pause in the debugger
_Other keyboard activity is forwarded to the emulated C256 itself._

## CPU/Debug Window
* F5 - Run
* F6 - Step (Execute a single opcode)
* F7 - Step Over (Execute a subroutine until RTS/RTL.  Also applies to loops, until the next command is reached)

## Memory Window
* Page Up/Page Down - display the next page (256 bytes) of memory.

# License 
Please see License.txt.

The CPU emulation is available for licensed use in other projects. Please contact us for details. 

Contributed Code: code and data contributed to the Nu256 project may be shared with the C256 Foenix, without restriction. By contributing materials to Nu64, you grant a non-exclusive license to the owners of both Nu256 and C256 Foenix to use for for any purpose. You also certify that contributed works are free of third party Copyright and patent restrictions. 
