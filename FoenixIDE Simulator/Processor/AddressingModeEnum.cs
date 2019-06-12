using System;
using System.Collections.Generic;

using System.Text;
using System.Threading.Tasks;

namespace FoenixIDE.Processor
{
    public enum AddressModes
    {
        Accumulator,
        BlockMove,
        Immediate,
        Implied,
        Interrupt,

        Absolute,
        AbsoluteLong,
        JmpAbsoluteIndirect,
        JmpAbsoluteIndirectLong,
        JmpAbsoluteIndexedIndirectWithX,
        AbsoluteIndexedWithX,
        AbsoluteLongIndexedWithX,
        AbsoluteIndexedWithY,
        AbsoluteLongIndexedWithY,

        DirectPage,
        DirectPageIndexedWithX,
        DirectPageIndexedWithY,
        DirectPageIndexedIndirectWithX,
        DirectPageIndirect,
        DirectPageIndirectIndexedWithY,
        DirectPageIndirectLong,
        DirectPageIndirectLongIndexedWithY,

        ProgramCounterRelative,
        ProgramCounterRelativeLong,

        StackImplied,
        //StackAbsolute,
        StackDirectPageIndirect,
        StackRelative,
        StackRelativeIndirectIndexedWithY,
        StackProgramCounterRelativeLong,
    }

}
