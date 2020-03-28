using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace FoenixIDE.Simulator.Basic
{
    public struct JOYINFOEX
    {
        Int32 dwSize;
        Int32 dwFlags;
        Int32 dwXpos;
        Int32 dwYpos;
        Int32 dwZpos;
        Int32 dwRpos;
        Int32 dwUpos;
        Int32 dwVpos;
        Int32 dwButtons;
        Int32 dwButtonNumber;
        Int32 dwPOV;
        Int32 dwReserved1;
        Int32 dwReserved2;
    }

    public struct JOYINFO
    {
        uint wXpos;
        uint wYpos;
        uint wZpos;
        uint wButtons;
    }
    // , * PJOYINFO, * NPJOYINFO, * LPJOYINFO;

    public struct JOYCAPS
    {
        Int16 wMid;
        Int16 wPid;
        string szPname; // char szPname[MAXPNAMELEN]
        uint wXmin;
        uint wXmax;
        uint wYmin;
        uint wYmax;
        uint wZmin;
        uint wZmax;
        uint wNumButtons;
        uint wPeriodMin;
        uint wPeriodMax;
        uint wRmin;
        uint wRmax;
        uint wUmin;
        uint wUmax;
        uint wVmin;
        uint wVmax;
        uint wCaps;
        uint wMaxAxes;
        uint wNumAxes;
        uint wMaxButtons;
        string szRegKey; // char szRegKey[MAXPNAMELEN];
        string szOEMVxD; // char szOEMVxD[MAX_JOYSTICKOEMVXDNAME];
    }
    //, * PJOYCAPS, * NPJOYCAPS, * LPJOYCAPS;

    public static class Joystick
    {
        public const int MAXPNAMELEN = 255;

        private const string WINMM_NATIVE_LIBRARY = "winmm.dll";
        private const CallingConvention CALLING_CONVENTION = CallingConvention.StdCall;

        [DllImport(WINMM_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
        public static extern int joyGetPosEx(int uJoyID, ref JOYINFOEX pji);

        [DllImport(WINMM_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
        public static extern int joyGetPos(int uJoyID, ref JOYINFO pji);

        [DllImport(WINMM_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
        public static extern int joyGetNumDevs();

        [DllImport(WINMM_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION, EntryPoint = "joyGetDevCaps"), SuppressUnmanagedCodeSecurity]
        public static extern int joyGetDevCapsA(int id, ref JOYCAPS lpCaps, int uSize);
    }
}
