using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FoenixIDE.Basic
{
    public enum ScanCode
    {
        sc_null = 0x00,
        sc_escape = 0x01,
        sc_1 = 0x02,
        sc_2 = 0x03,
        sc_3 = 0x04,
        sc_4 = 0x05,
        sc_5 = 0x06,
        sc_6 = 0x07,
        sc_7 = 0x08,
        sc_8 = 0x09,
        sc_9 = 0x0A,
        sc_0 = 0x0B,
        sc_minus = 0x0C,
        sc_equals = 0x0D,
        sc_backspace = 0x0E,
        sc_tab = 0x0F,
        sc_q = 0x10,
        sc_w = 0x11,
        sc_e = 0x12,
        sc_r = 0x13,
        sc_t = 0x14,
        sc_y = 0x15,
        sc_u = 0x16,
        sc_i = 0x17,
        sc_o = 0x18,
        sc_p = 0x19,
        sc_bracketLeft = 0x1A,
        sc_bracketRight = 0x1B,
        sc_enter = 0x1C,
        sc_controlLeft = 0x1D,
        sc_a = 0x1E,
        sc_s =0x1F,
        sc_d = 0x20,
        sc_f = 0x21,
        sc_g = 0x22,
        sc_h = 0x23,
        sc_j = 0x24,
        sc_k = 0x25,
        sc_l = 0x26,
        sc_semicolon = 0x27,
        sc_apostrophe = 0x28,
        sc_grave = 0x29,
        sc_shiftLeft = 0x2A,
        sc_backslash = 0x2B,
        sc_z = 0x2C,
        sc_x = 0x2D,
        sc_c = 0x2E,
        sc_v = 0x2F,
        sc_b = 0x30,
        sc_n = 0x31,
        sc_m = 0x32,
        sc_comma = 0x33,
        sc_period = 0x34,
        sc_slash = 0x35,
        sc_shiftRight = 0x36,
        sc_numpad_multiply = 0x37,
        sc_altLeft = 0x38,
        sc_space = 0x39,
        sc_capslock = 0x3A,
        sc_F1 = 0x3B,
        sc_F2 = 0x3C,
        sc_F3 = 0x3D,
        sc_F4 = 0x3E,
        sc_F5 = 0x3F,
        sc_F6 = 0x40,
        sc_F7 = 0x41,
        sc_F8 = 0x42,
        sc_F9 = 0x43,
        sc_F10 = 0x44,
        sc_F11 = 0x57,
        sc_F12 = 0x58,
        sc_up_arrow = 0x48,    // also maps to num keypad 8
        sc_left_arrow = 0x4B,  // also maps to num keypad 4
        sc_right_arrow = 0x4D, // also maps to num keypad 6
        sc_down_arrow = 0x50   // also maps to num keypad 2
    }
    class ScanCodes
    {
        public static ScanCode GetScanCode(Keys key)
        {
            switch(key)
            {
                case Keys.D0:
                    return ScanCode.sc_0;
                case Keys.D1:
                case Keys.D2:
                case Keys.D3:
                case Keys.D4:
                case Keys.D5:
                case Keys.D6:
                case Keys.D7:
                case Keys.D8:
                case Keys.D9:
                    return ScanCode.sc_1 + (key - Keys.D1);
                case Keys.A:
                    return ScanCode.sc_a;
                case Keys.B:
                    return ScanCode.sc_b;
                case Keys.C:
                    return ScanCode.sc_c;
                case Keys.D:
                    return ScanCode.sc_d;
                case Keys.E:
                    return ScanCode.sc_e;
                case Keys.F:
                    return ScanCode.sc_f;
                case Keys.G:
                    return ScanCode.sc_g;
                case Keys.H:
                    return ScanCode.sc_h;
                case Keys.I:
                    return ScanCode.sc_i;
                case Keys.J:
                    return ScanCode.sc_j;
                case Keys.K:
                    return ScanCode.sc_k;
                case Keys.L:
                    return ScanCode.sc_l;
                case Keys.M:
                    return ScanCode.sc_m;
                case Keys.N:
                    return ScanCode.sc_n;
                case Keys.O:
                    return ScanCode.sc_o;
                case Keys.P:
                    return ScanCode.sc_p;
                case Keys.Q:
                    return ScanCode.sc_q;
                case Keys.R:
                    return ScanCode.sc_r;
                case Keys.S:
                    return ScanCode.sc_s;
                case Keys.T:
                    return ScanCode.sc_t;
                case Keys.U:
                    return ScanCode.sc_u;
                case Keys.V:
                    return ScanCode.sc_v;
                case Keys.W:
                    return ScanCode.sc_w;
                case Keys.X:
                    return ScanCode.sc_x;
                case Keys.Y:
                    return ScanCode.sc_y;
                case Keys.Z:
                    return ScanCode.sc_z;
                case Keys.Return:
                    return ScanCode.sc_enter;
                case Keys.Delete:
                case Keys.Back:
                    return ScanCode.sc_backspace;
                case Keys.Space:
                    return ScanCode.sc_space;
                case Keys.Oemcomma:
                    return ScanCode.sc_comma;
                case Keys.OemPeriod:
                    return ScanCode.sc_period;
                case Keys.OemSemicolon:
                    return ScanCode.sc_semicolon;
                case Keys.Escape:
                    return ScanCode.sc_escape;
                case Keys.Oem3: // back tick
                    return ScanCode.sc_grave;
                case Keys.OemQuotes:
                    return ScanCode.sc_apostrophe;
                case Keys.OemOpenBrackets:
                    return ScanCode.sc_bracketLeft;
                case Keys.OemCloseBrackets:
                    return ScanCode.sc_bracketRight;
                case Keys.OemMinus:
                    return ScanCode.sc_minus;
                case Keys.Oemplus:
                    return ScanCode.sc_equals;
                case Keys.Tab:
                    return ScanCode.sc_tab;
                case Keys.Oem2:
                    return ScanCode.sc_slash;
                case Keys.Oem5:
                    return ScanCode.sc_backslash;
                case Keys.ShiftKey:
                    return ScanCode.sc_shiftLeft;
                case Keys.Menu:
                case Keys.Alt:
                    return ScanCode.sc_altLeft;
                case Keys.ControlKey:
                    return ScanCode.sc_controlLeft;
                case Keys.Up:
                    return ScanCode.sc_up_arrow;
                case Keys.Down:
                    return ScanCode.sc_down_arrow;
                case Keys.Left:
                    return ScanCode.sc_left_arrow;
                case Keys.Right:
                    return ScanCode.sc_right_arrow;
                case Keys.F1:
                case Keys.F2:
                case Keys.F3:
                case Keys.F4:
                case Keys.F5:
                case Keys.F6:
                case Keys.F7:
                case Keys.F8:
                case Keys.F9:
                case Keys.F10:
                    return ScanCode.sc_F1 + (key - Keys.F1);
                case Keys.F11:
                case Keys.F12:
                    return ScanCode.sc_F11 + (key - Keys.F11);
            }
            return ScanCode.sc_null;
        }
    }
    
}
