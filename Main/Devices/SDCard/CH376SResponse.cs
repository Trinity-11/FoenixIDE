using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoenixIDE.Simulator.Devices.SDCard
{
    public enum CH376SResponse
    {
        CMD_STAT_SUCCESS = 0x14,
        CMD_RET_SUCCESS = 0x51,
        CMD_RET_ABORT = 0x5f,
    };
}
