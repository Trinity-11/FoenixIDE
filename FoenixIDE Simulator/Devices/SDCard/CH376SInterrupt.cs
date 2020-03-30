using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoenixIDE.Simulator.Devices.SDCard
{
    public enum CH376SInterrupt
    {
        USB_INT_NONE = 0x00,
        USB_INT_SUCCESS = 0x14,
        USB_INT_CONNECT = 0x15,
        USB_INT_DISCONNECT = 0x16,
        USB_INT_BUF_OVER = 0x17,
        USB_INT_USB_READY = 0x18,
        USB_INT_DISK_READ = 0x1d,
        USB_INT_DISK_WRITE = 0x1e,
        USB_INT_DISK_ERR = 0x1f,

        ERR_OPEN_DIR = 0x41,
        ERR_MISS_FIL = 0x42,
        ERR_FOUND_NAME = 0x43
    };
}
