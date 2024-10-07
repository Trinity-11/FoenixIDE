using System;
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
        RevF256K_65816,
        RevF256K2e
    }
    
    public static class BoardVersionHelpers
    {
        public static bool IsF256(BoardVersion boardVersion)
        {
            return boardVersion == BoardVersion.RevJr_6502 || 
                   boardVersion == BoardVersion.RevJr_65816 ||
                   boardVersion == BoardVersion.RevF256K_6502 ||
                   boardVersion == BoardVersion.RevF256K_65816 ||
                   boardVersion == BoardVersion.RevF256K2e;
        }
        public static bool Is6502(BoardVersion boardVersion)
        {
            return boardVersion == BoardVersion.RevJr_6502 ||
                   boardVersion == BoardVersion.RevF256K_6502;
        }

        public static bool IsF256_Flat(BoardVersion boardVersion)
        {
            return boardVersion == BoardVersion.RevF256K2e;
        }

        public static bool IsF256_MMU(BoardVersion boardVersion)
        {
            return IsF256(boardVersion) && !IsF256_Flat(boardVersion);
        }
    }
}
