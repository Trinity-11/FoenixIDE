﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FoenixIDE.Simulator.Basic
{
    public enum  ScanCode
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
        sc_space = 0x39
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
                case Keys.Divide:
                    return ScanCode.sc_slash;
            }
            return ScanCode.sc_null;
        }
    }
    
}
