using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FoenixIDE.Basic
{   
    class ScanCodes
    {
        public const byte sc_null = 0x00;

        // Scan Codes Set 1
        public const byte sc1_escape = 0x01;
        public const byte sc1_1 = 0x02;
        public const byte sc1_2 = 0x03;
        public const byte sc1_3 = 0x04;
        public const byte sc1_4 = 0x05;
        public const byte sc1_5 = 0x06;
        public const byte sc1_6 = 0x07;
        public const byte sc1_7 = 0x08;
        public const byte sc1_8 = 0x09;
        public const byte sc1_9 = 0x0A;
        public const byte sc1_0 = 0x0B;
        public const byte sc1_minus = 0x0C;
        public const byte sc1_equals = 0x0D;
        public const byte sc1_backspace = 0x0E;
        public const byte sc1_tab = 0x0F;
        public const byte sc1_q = 0x10;
        public const byte sc1_w = 0x11;
        public const byte sc1_e = 0x12;
        public const byte sc1_r = 0x13;
        public const byte sc1_t = 0x14;
        public const byte sc1_y = 0x15;
        public const byte sc1_u = 0x16;
        public const byte sc1_i = 0x17;
        public const byte sc1_o = 0x18;
        public const byte sc1_p = 0x19;
        public const byte sc1_bracketLeft = 0x1A;
        public const byte sc1_bracketRight = 0x1B;
        public const byte sc1_enter = 0x1C;
        public const byte sc1_controlLeft = 0x1D;
        public const byte sc1_a = 0x1E;
        public const byte sc1_s = 0x1F;
        public const byte sc1_d = 0x20;
        public const byte sc1_f = 0x21;
        public const byte sc1_g = 0x22;
        public const byte sc1_h = 0x23;
        public const byte sc1_j = 0x24;
        public const byte sc1_k = 0x25;
        public const byte sc1_l = 0x26;
        public const byte sc1_semicolon = 0x27;
        public const byte sc1_apostrophe = 0x28;
        public const byte sc1_grave = 0x29;
        public const byte sc1_shiftLeft = 0x2A;
        public const byte sc1_backslash = 0x2B;
        public const byte sc1_z = 0x2C;
        public const byte sc1_x = 0x2D;
        public const byte sc1_c = 0x2E;
        public const byte sc1_v = 0x2F;
        public const byte sc1_b = 0x30;
        public const byte sc1_n = 0x31;
        public const byte sc1_m = 0x32;
        public const byte sc1_comma = 0x33;
        public const byte sc1_period = 0x34;
        public const byte sc1_slash = 0x35;
        public const byte sc1_shiftRight = 0x36;
        public const byte sc1_numpad_multiply = 0x37;
        public const byte sc1_altLeft = 0x38;
        public const byte sc1_space = 0x39;
        public const byte sc1_capslock = 0x3A;
        public const byte sc1_F1 = 0x3B;
        public const byte sc1_F2 = 0x3C;
        public const byte sc1_F3 = 0x3D;
        public const byte sc1_F4 = 0x3E;
        public const byte sc1_F5 = 0x3F;
        public const byte sc1_F6 = 0x40;
        public const byte sc1_F7 = 0x41;
        public const byte sc1_F8 = 0x42;
        public const byte sc1_F9 = 0x43;
        public const byte sc1_F10 = 0x44;
        public const byte sc1_F11 = 0x57;
        public const byte sc1_F12 = 0x58;
        public const byte sc1_up_arrow = 0x48;    // also maps to num keypad 8
        public const byte sc1_left_arrow = 0x4B;  // also maps to num keypad 4
        public const byte sc1_right_arrow = 0x4D; // also maps to num keypad 6
        public const byte sc1_down_arrow = 0x50;   // also maps to num keypad 2
        public const byte sc1_key_released = 0x80;


        public static byte[] GetScanCodeSet1(Keys key, bool up)
        {
            byte[] result = new byte[1]; // the pause key is handled somewhere else
            switch(key)
            {
                case Keys.D0:
                    result[0] = sc1_0;
                    break;
                case Keys.D1:
                case Keys.D2:
                case Keys.D3:
                case Keys.D4:
                case Keys.D5:
                case Keys.D6:
                case Keys.D7:
                case Keys.D8:
                case Keys.D9:
                    result[0] = (byte)(sc1_1 + (key - Keys.D1));
                    break;
                case Keys.A:
                    result[0] = sc1_a;
                    break;
                case Keys.B:
                    result[0] = sc1_b;
                    break;
                case Keys.C:
                    result[0] = sc1_c;
                    break;
                case Keys.D:
                    result[0] = sc1_d;
                    break;
                case Keys.E:
                    result[0] = sc1_e;
                    break;
                case Keys.F:
                    result[0] = sc1_f;
                    break;
                case Keys.G:
                    result[0] = sc1_g;
                    break;
                case Keys.H:
                    result[0] = sc1_h;
                    break;
                case Keys.I:
                    result[0] = sc1_i;
                    break;
                case Keys.J:
                    result[0] =  sc1_j;
                    break;
                case Keys.K:
                    result[0] =  sc1_k;
                    break;
                case Keys.L:
                    result[0] =  sc1_l;
                    break;
                case Keys.M:
                    result[0] =  sc1_m;
                    break;
                case Keys.N:
                    result[0] =  sc1_n;
                    break;
                case Keys.O:
                    result[0] =  sc1_o;
                    break;
                case Keys.P:
                    result[0] =  sc1_p;
                    break;
                case Keys.Q:
                    result[0] =  sc1_q;
                    break;
                case Keys.R:
                    result[0] =  sc1_r;
                    break;
                case Keys.S:
                    result[0] =  sc1_s;
                    break;
                case Keys.T:
                    result[0] =  sc1_t;
                    break;
                case Keys.U:
                    result[0] =  sc1_u;
                    break;
                case Keys.V:
                    result[0] =  sc1_v;
                    break;
                case Keys.W:
                    result[0] =  sc1_w;
                    break;
                case Keys.X:
                    result[0] =  sc1_x;
                    break;
                case Keys.Y:
                    result[0] =  sc1_y;
                    break;
                case Keys.Z:
                    result[0] =  sc1_z;
                    break;
                case Keys.Return:
                    result[0] =  sc1_enter;
                    break;
                case Keys.Delete:
                case Keys.Back:
                    result[0] =  sc1_backspace;
                    break;
                case Keys.Space:
                    result[0] =  sc1_space;
                    break;
                case Keys.Oemcomma:
                    result[0] =  sc1_comma;
                    break;
                case Keys.OemPeriod:
                    result[0] =  sc1_period;
                    break;
                case Keys.OemSemicolon:
                    result[0] =  sc1_semicolon;
                    break;
                case Keys.Escape:
                    result[0] =  sc1_escape;
                    break;
                case Keys.Oem3: // back tick
                    result[0] =  sc1_grave;
                    break;
                case Keys.OemQuotes:
                    result[0] =  sc1_apostrophe;
                    break;
                case Keys.OemOpenBrackets:
                    result[0] =  sc1_bracketLeft;
                    break;
                case Keys.OemCloseBrackets:
                    result[0] =  sc1_bracketRight;
                    break;
                case Keys.OemMinus:
                    result[0] =  sc1_minus;
                    break;
                case Keys.Oemplus:
                    result[0] =  sc1_equals;
                    break;
                case Keys.Tab:
                    result[0] =  sc1_tab; 
                    break;
                case Keys.Oem2:
                    result[0] =  sc1_slash;
                    break;
                case Keys.Oem5:
                    result[0] =  sc1_backslash;
                    break;
                case Keys.ShiftKey:
                    result[0] =  sc1_shiftLeft;
                    break;
                case Keys.Menu:
                case Keys.Alt:
                    result[0] =  sc1_altLeft;
                    break;
                case Keys.ControlKey:
                    result[0] =  sc1_controlLeft;
                    break;
                case Keys.Up:
                    result[0] =  sc1_up_arrow;
                    break;
                case Keys.Down:
                    result[0] =  sc1_down_arrow;
                    break;
                case Keys.Left:
                    result[0] =  sc1_left_arrow;
                    break;
                case Keys.Right:
                    result[0] =  sc1_right_arrow;
                    break;
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
                    result[0] =  (byte)(sc1_F1 + (key - Keys.F1));
                    break;
                case Keys.F11:
                case Keys.F12:
                    result[0] =  (byte)(sc1_F11 + (key - Keys.F11));
                    break;
            }
            result[0] += up ? (byte)0x80 : (byte)0;
            return result;
        }

        // Scan Codes Set 2
        public const byte sc2_escape = 0x76;
        public const byte sc2_1 = 0x16;
        public const byte sc2_2 = 0x1E;
        public const byte sc2_3 = 0x26;
        public const byte sc2_4 = 0x25;
        public const byte sc2_5 = 0x2E;
        public const byte sc2_6 = 0x36;
        public const byte sc2_7 = 0x3D;
        public const byte sc2_8 = 0x3E;
        public const byte sc2_9 = 0x46;
        public const byte sc2_0 = 0x45;
        public const byte sc2_minus = 0x4E;
        public const byte sc2_equals = 0x55;
        public const byte sc2_backspace = 0x66;
        public const byte sc2_tab = 0x0D;
        public const byte sc2_q = 0x15;
        public const byte sc2_w = 0x1D;
        public const byte sc2_e = 0x24;
        public const byte sc2_r = 0x2D;
        public const byte sc2_t = 0x2C;
        public const byte sc2_y = 0x35;
        public const byte sc2_u = 0x3C;
        public const byte sc2_i = 0x43;
        public const byte sc2_o = 0x44;
        public const byte sc2_p = 0x4D;
        public const byte sc2_bracketLeft = 0x54;
        public const byte sc2_bracketRight = 0x5B;
        public const byte sc2_enter = 0x5A;
        public const byte sc2_controlLeft = 0x14;
        public const byte sc2_a = 0x1C;
        public const byte sc2_s = 0x1B;
        public const byte sc2_d = 0x23;
        public const byte sc2_f = 0x2B;
        public const byte sc2_g = 0x34;
        public const byte sc2_h = 0x33;
        public const byte sc2_j = 0x3B;
        public const byte sc2_k = 0x42;
        public const byte sc2_l = 0x4B;
        public const byte sc2_semicolon = 0x4C;
        public const byte sc2_apostrophe = 0x52;
        public const byte sc2_grave = 0x0E;
        public const byte sc2_shiftLeft = 0x12;
        public const byte sc2_backslash = 0x5D;
        public const byte sc2_z = 0x1A;
        public const byte sc2_x = 0x22;
        public const byte sc2_c = 0x21;
        public const byte sc2_v = 0x2A;
        public const byte sc2_b = 0x32;
        public const byte sc2_n = 0x31;
        public const byte sc2_m = 0x3A;
        public const byte sc2_comma = 0x41;
        public const byte sc2_period = 0x49;
        public const byte sc2_slash = 0x4A;
        public const byte sc2_shiftRight = 0x59;
        public const byte sc2_numpad_multiply = 0x7C;
        public const byte sc2_altLeft = 0x11;
        public const byte sc2_space = 0x29;
        public const byte sc2_capslock = 0x58;
        public const byte sc2_F1 = 0x01;
        public const byte sc2_F2 = 0x06;
        public const byte sc2_F3 = 0x04;
        public const byte sc2_F4 = 0x0C;
        public const byte sc2_F5 = 0x03;
        public const byte sc2_F6 = 0x0B;
        public const byte sc2_F7 = 0x83;
        public const byte sc2_F8 = 0x0A;
        public const byte sc2_F9 = 0x01;
        public const byte sc2_F10 = 0x09;
        public const byte sc2_F11 = 0x78;
        public const byte sc2_F12 = 0x07;
        public const byte sc2_up_arrow = 0x75;    // also keypad 8 - this is easier
        public const byte sc2_left_arrow = 0x6B;  // also keypad 4 - this is easier
        public const byte sc2_right_arrow = 0x74; // also keypad  6 - this is easier
        public const byte sc2_down_arrow = 0x72;   // also keypad 2 - this is easier
        public const byte sc2_key_released = 0xF0;

        public static byte[] GetSCSet2(Keys key, bool up)
        {
            byte[] result = new byte[up?2:1];
            int pos = 0;
            if (up)
            {
                result[0] = sc2_key_released;
                pos = 1;
            }
            switch (key)
            {
                case Keys.D0:
                    result[pos] = sc2_0;
                    break;
                case Keys.D1:
                    result[pos] = sc2_1;
                    break;
                case Keys.D2:
                    result[pos] = sc2_2;
                    break;
                case Keys.D3:
                    result[pos] = sc2_3;
                    break;
                case Keys.D4:
                    result[pos] = sc2_4;
                    break;
                case Keys.D5:
                    result[pos] = sc2_5;
                    break;
                case Keys.D6:
                    result[pos] = sc2_6;
                    break;
                case Keys.D7:
                    result[pos] = sc2_7;
                    break;
                case Keys.D8:
                    result[pos] = sc2_8;
                    break;
                case Keys.D9:
                    result[pos] = sc2_9;
                    break;
                case Keys.A:
                    result[pos] = sc2_a;
                    break;
                case Keys.B:
                    result[pos] = sc2_b;
                    break;
                case Keys.C:
                    result[pos] = sc2_c;
                    break;
                case Keys.D:
                    result[pos] = sc2_d;
                    break;
                case Keys.E:
                    result[pos] =  sc2_e;
                    break;
                case Keys.F:
                    result[pos] =  sc2_f;
                    break;
                case Keys.G:
                    result[pos] =  sc2_g;
                    break;
                case Keys.H:
                    result[pos] =  sc2_h;
                    break;
                case Keys.I:
                    result[pos] =  sc2_i;
                    break;
                case Keys.J:
                    result[pos] =  sc2_j;
                    break;
                case Keys.K:
                    result[pos] =  sc2_k;
                    break;
                case Keys.L:
                    result[pos] =  sc2_l;
                    break;
                case Keys.M:
                    result[pos] =  sc2_m;
                    break;
                case Keys.N:
                    result[pos] =  sc2_n;
                    break;
                case Keys.O:
                    result[pos] =  sc2_o;
                    break;
                case Keys.P:
                    result[pos] =  sc2_p;
                    break;
                case Keys.Q:
                    result[pos] =  sc2_q;
                    break;
                case Keys.R:
                    result[pos] =  sc2_r;
                    break;
                case Keys.S:
                    result[pos] =  sc2_s;
                    break;
                case Keys.T:
                    result[pos] =  sc2_t;
                    break;
                case Keys.U:
                    result[pos] =  sc2_u;
                    break;
                case Keys.V:
                    result[pos] =  sc2_v;
                    break;
                case Keys.W:
                    result[pos] =  sc2_w;
                    break;
                case Keys.X:
                    result[pos] =  sc2_x;
                    break;
                case Keys.Y:
                    result[pos] =  sc2_y;
                    break;
                case Keys.Z:
                    result[pos] =  sc2_z;
                    break;
                case Keys.Return:
                    result[pos] =  sc2_enter;
                    break;
                case Keys.Delete:
                case Keys.Back:
                    result[pos] =  sc2_backspace;
                    break;
                case Keys.Space:
                    result[pos] =  sc2_space;
                    break;
                case Keys.Oemcomma:
                    result[pos] =  sc2_comma;
                    break;
                case Keys.OemPeriod:
                    result[pos] =  sc2_period;
                    break;
                case Keys.OemSemicolon:
                    result[pos] =  sc2_semicolon;
                    break;
                case Keys.Escape:
                    result[pos] =  sc2_escape;
                    break;
                case Keys.Oem3: // back tick
                    result[pos] =  sc2_grave;
                    break;
                case Keys.OemQuotes:
                    result[pos] =  sc2_apostrophe;
                    break;
                case Keys.OemOpenBrackets:
                    result[pos] =  sc2_bracketLeft;
                    break;
                case Keys.OemCloseBrackets:
                    result[pos] =  sc2_bracketRight;
                    break;
                case Keys.OemMinus:
                    result[pos] =  sc2_minus;
                    break;
                case Keys.Oemplus:
                    result[pos] =  sc2_equals;
                    break;
                case Keys.Tab:
                    result[pos] =  sc2_tab;
                    break;
                case Keys.Oem2:
                    result[pos] =  sc2_slash;
                    break;
                case Keys.Oem5:
                    result[pos] =  sc2_backslash;
                    break;
                case Keys.ShiftKey:
                    result[pos] =  sc2_shiftLeft;
                    break;
                case Keys.Menu:
                case Keys.Alt:
                    result[pos] =  sc2_altLeft;
                    break;
                case Keys.ControlKey:
                    result[pos] =  sc2_controlLeft;
                    break;
                case Keys.Up:
                    result[pos] =  sc2_up_arrow;
                    break;
                case Keys.Down:
                    result[pos] =  sc2_down_arrow;
                    break;
                case Keys.Left:
                    result[pos] =  sc2_left_arrow;
                    break;
                case Keys.Right:
                    result[pos] =  sc2_right_arrow;
                    break;
                case Keys.F1:
                    result[pos] = sc2_F1;
                    break;
                case Keys.F2:
                    result[pos] = sc2_F2;
                    break;
                case Keys.F3:
                    result[pos] = sc2_F3;
                    break;
                case Keys.F4:
                    result[pos] = sc2_F4;
                    break;
                case Keys.F5:
                    result[pos] = sc2_F5;
                    break;
                case Keys.F6:
                    result[pos] = sc2_F6;
                    break;
                case Keys.F7:
                    result[pos] = sc2_F7;
                    break;
                case Keys.F8:
                    result[pos] = sc2_F8;
                    break;
                case Keys.F9:
                    result[pos] = sc2_F9;
                    break;
                case Keys.F10:
                    result[pos] = sc2_F10;
                    break;
                case Keys.F11:
                    result[pos] = sc2_F11;
                    break;
                case Keys.F12:
                    result[pos] = sc2_F12;
                    break;
            }
            return result;
        }
    }
}
