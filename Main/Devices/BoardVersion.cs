﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoenixIDE.Simulator.Devices
{
    public enum BoardVersion
    {
        RevB,
        RevC,
        RevU,
        RevUPlus,
        RevJr_6502,
        RevJr_65816,
        RevF256K_6502,
        RevF256K_65816
    }
    
    public static class BoardVersionHelpers
    {
        public static bool IsF256(BoardVersion boardVersion)
        {
            return boardVersion == BoardVersion.RevJr_6502 || 
                   boardVersion == BoardVersion.RevJr_65816 ||
                   boardVersion == BoardVersion.RevF256K_6502 ||
                   boardVersion == BoardVersion.RevF256K_65816;
        }
    }
}
