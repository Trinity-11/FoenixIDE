using System;
using System.Collections.Generic;

using System.Text;
using System.Threading.Tasks;

namespace FoenixIDE
{
    public enum PETSCIICommandCodes
    {
        Null = 0,
        Stop = 3,
        Run = 131,
        Return = 13,
        Down = 17,
        Right = 29,
        Up = 145,
        Left = 157,
        Home = 19,
        Clear = 147,
        Del = 20,
        Ins = 148,
        Esc = 27,

        F1 = 133,
        F3 = 134,
        F5 = 135,
        F7 = 136,
        F2 = 137,
        F4 = 138,
        F6 = 139,
        F8 = 140,

        DisableCmdShift = 8,
        EnableCmdShift = 9,
        TextMode = 14,
        GraphicMode = 142,
        AsciiPetMode = 15,
        AsciiIbmMode = 144,

        Reverse = 18,
        Reverseoff = 156,
        Black = 144,
        Gray1 = 151,
        Gray2 = 152,
        Gray3 = 155,
        White = 5,
        Red = 28,
        LightRed = 150,
        Green = 30,
        LightGreen = 153,
        Blue = 31,
        LightBlue = 154,
        Cyan = 159,
        Purple = 156,
        Brown = 149,
        Orange = 129,
        Yellow = 159,
    };
}
