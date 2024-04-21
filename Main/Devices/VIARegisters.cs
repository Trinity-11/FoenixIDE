using FoenixIDE.Basic;

namespace FoenixIDE.Simulator.Devices
{

    public class VIARegisters
    {
        // The VIA0 is used for the joystick in F256JR and F256K
        // While the VIA can be used to output to external devices, this capability is not used in the emulator.
        public class VIA0Range : MemoryLocations.MemoryRAM
        {
            VIARegisters matrix;  

            public VIA0Range(VIARegisters m, int StartAddress, int Length) : base(StartAddress, Length)
            {
                System.Diagnostics.Debug.Assert(Length == 4);
                matrix = m;
            }

            bool CanRead(int Address)
            {
                if (Address == 0) // Port B
                {
                    bool canReadPortB = VIA0_DDRB == 0;
                    return canReadPortB;
                }
                else if (Address == 1) // Port A
                {
                    bool canReadPortA = VIA0_DDRA == 0;
                    return canReadPortA;
                }
                return true;
            }


            public override byte ReadByte(int Address)
            {
                if (!CanRead(Address))
                    return 0;

                switch(Address)
                {
                    case 0:
                        return (byte)(data[0] | matrix.joystickB);
                    case 1:
                        return (byte)(data[1] | matrix.joystickA);
                    case 2:
                    case 3:
                        return data[Address];
                    default:
                        return 0;
                }
            }

            bool CanWrite(int Address)
            {
                if (Address == 0)
                {
                    bool canWritePortB = (VIA0_DDRB & 0x7f) == 0x7F;
                    return canWritePortB;
                }
                if (Address == 1)
                {
                    bool canWritePortA = (VIA0_DDRA & 0x7F) == 0x7F;
                    return canWritePortA;
                }
                return true;
            }

            public override void WriteByte(int Address, byte Value)
            {
                if (!CanWrite(Address))
                    return;

                data[Address] = Value;
            }
            public byte VIA0_PRB { get { return data[0]; } set { data[0] = value; } }
            byte VIA0_DDRA { get { return data[3]; } set { data[3] = value; } }
            byte VIA0_DDRB { get { return data[2]; } set { data[2] = value; } }
        }

        // VIA1 is only available on the F256K - the Matrix Keyboard
        public class VIA1Range : MemoryLocations.MemoryRAM
        {
            VIARegisters matrix;
            byte VIA1_PRB { get { return data[0]; } set { data[0] = value; } }
            byte VIA1_PRA { get { return data[1]; } set { data[1] = value; } }

            byte VIA1_DDRB { get { return data[2]; } set { data[2] = value; } }
            byte VIA1_DDRA { get { return data[3]; } set { data[3] = value; } }

            public VIA1Range(VIARegisters m, int StartAddress, int Length) : base(StartAddress, Length)
            {
                System.Diagnostics.Debug.Assert(Length == 4);
                matrix = m;
            }

            // This is also supposed to be a mask, not an all-or-nothing
            bool CanWrite(int Address)
            {
                if (Address == 0)
                {
                    bool canWritePortB = VIA1_DDRB == 0xFF;
                    return canWritePortB;
                }
                if (Address == 1)
                {
                    bool canWritePortA = VIA1_DDRA == 0xFF;
                    return canWritePortA;
                }
                return true;
            }

            public override void WriteByte(int Address, byte Value)
            {
                if (!CanWrite(Address))
                    return;

                data[Address] = Value;

                switch (Address)
                {
                    case 0:
                        // We wrote to port B - so we now must find Port A.
                        WritePortAValue(Value);
                        break;
                    case 1:
                        // We wrote to port A.
                        WritePortBValue(Value);
                        break;
                }
            }

            private void  WritePortBValue(byte Value)
            {
                VIA1_PRB = 0xFF;
                matrix.VIA0.VIA0_PRB = (byte)(matrix.joystickB | 0xC0);
                if (Value == unchecked((byte)(~1))) // PA0
                {
                    VIA1_PRB =  (1 << 0); // delete key, unmapped
                    VIA1_PRB |= matrix.scanCodeBuffer[ScanCodes.sc1_enter] ?      (byte)0 : (byte)(1 << 1);
                    VIA1_PRB |= matrix.scanCodeBuffer[ScanCodes.sc1_left_arrow] ? (byte)0 : (byte)(1 << 2);
                    VIA1_PRB |= matrix.scanCodeBuffer[ScanCodes.sc1_F7] ?         (byte)0 : (byte)(1 << 3);
                    VIA1_PRB |= matrix.scanCodeBuffer[ScanCodes.sc1_F1] ?         (byte)0 : (byte)(1 << 4);
                    VIA1_PRB |= matrix.scanCodeBuffer[ScanCodes.sc1_F3] ?         (byte)0 : (byte)(1 << 5);
                    VIA1_PRB |= matrix.scanCodeBuffer[ScanCodes.sc1_F5] ?         (byte)0 : (byte)(1 << 6);
                    VIA1_PRB |= matrix.scanCodeBuffer[ScanCodes.sc1_up_arrow] ?   (byte)0 : (byte)(1 << 7);

                    // Also set VIA0 - don't touch the joystick bits!!
                    matrix.VIA0.VIA0_PRB = matrix.joystickB;
                    matrix.VIA0.VIA0_PRB |= matrix.scanCodeBuffer[(int)ScanCodes.sc1_down_arrow] ? (byte)0 : (byte)(1 << 7);
                }
                else if (Value == unchecked((byte)(~2))) // PA1
                {
                    VIA1_PRB =  matrix.scanCodeBuffer[ScanCodes.sc1_3] ? (byte)0 : (byte)(1 << 0);
                    VIA1_PRB |= matrix.scanCodeBuffer[ScanCodes.sc1_w] ? (byte)0 : (byte)(1 << 1);
                    VIA1_PRB |= matrix.scanCodeBuffer[ScanCodes.sc1_a] ? (byte)0 : (byte)(1 << 2);
                    VIA1_PRB |= matrix.scanCodeBuffer[ScanCodes.sc1_4] ? (byte)0 : (byte)(1 << 3);
                    VIA1_PRB |= matrix.scanCodeBuffer[ScanCodes.sc1_z] ? (byte)0 : (byte)(1 << 4);
                    VIA1_PRB |= matrix.scanCodeBuffer[ScanCodes.sc1_s] ? (byte)0 : (byte)(1 << 5);
                    VIA1_PRB |= matrix.scanCodeBuffer[ScanCodes.sc1_e] ? (byte)0 : (byte)(1 << 6);
                    VIA1_PRB |= matrix.scanCodeBuffer[ScanCodes.sc1_shiftLeft] ? (byte)0 : (byte)(1 << 7);
                }
                else if (Value == unchecked((byte)(~4))) // PA2
                {
                    VIA1_PRB =  matrix.scanCodeBuffer[ScanCodes.sc1_5] ? (byte)0 : (byte)(1 << 0);
                    VIA1_PRB |= matrix.scanCodeBuffer[ScanCodes.sc1_r] ? (byte)0 : (byte)(1 << 1);
                    VIA1_PRB |= matrix.scanCodeBuffer[ScanCodes.sc1_d] ? (byte)0 : (byte)(1 << 2);
                    VIA1_PRB |= matrix.scanCodeBuffer[ScanCodes.sc1_6] ? (byte)0 : (byte)(1 << 3);
                    VIA1_PRB |= matrix.scanCodeBuffer[ScanCodes.sc1_c] ? (byte)0 : (byte)(1 << 4);
                    VIA1_PRB |= matrix.scanCodeBuffer[ScanCodes.sc1_f] ? (byte)0 : (byte)(1 << 5);
                    VIA1_PRB |= matrix.scanCodeBuffer[ScanCodes.sc1_t] ? (byte)0 : (byte)(1 << 6);
                    VIA1_PRB |= matrix.scanCodeBuffer[ScanCodes.sc1_x] ? (byte)0 : (byte)(1 << 7);
                }
                else if (Value == unchecked((byte)(~8))) // PA3
                {
                    VIA1_PRB =  matrix.scanCodeBuffer[ScanCodes.sc1_7] ? (byte)0 : (byte)(1 << 0);
                    VIA1_PRB |= matrix.scanCodeBuffer[ScanCodes.sc1_y] ? (byte)0 : (byte)(1 << 1);
                    VIA1_PRB |= matrix.scanCodeBuffer[ScanCodes.sc1_g] ? (byte)0 : (byte)(1 << 2);
                    VIA1_PRB |= matrix.scanCodeBuffer[ScanCodes.sc1_8] ? (byte)0 : (byte)(1 << 3);
                    VIA1_PRB |= matrix.scanCodeBuffer[ScanCodes.sc1_b] ? (byte)0 : (byte)(1 << 4);
                    VIA1_PRB |= matrix.scanCodeBuffer[ScanCodes.sc1_h] ? (byte)0 : (byte)(1 << 5);
                    VIA1_PRB |= matrix.scanCodeBuffer[ScanCodes.sc1_u] ? (byte)0 : (byte)(1 << 6);
                    VIA1_PRB |= matrix.scanCodeBuffer[ScanCodes.sc1_v] ? (byte)0 : (byte)(1 << 7);
                }
                //Unchecked
                else if (Value == unchecked((byte)~0x10)) // PA4
                {
                    VIA1_PRB =  matrix.scanCodeBuffer[ScanCodes.sc1_9] ? (byte)0 : (byte)(1 << 0);
                    VIA1_PRB |= matrix.scanCodeBuffer[ScanCodes.sc1_i] ? (byte)0 : (byte)(1 << 1);
                    VIA1_PRB |= matrix.scanCodeBuffer[ScanCodes.sc1_j] ? (byte)0 : (byte)(1 << 2);
                    VIA1_PRB |= matrix.scanCodeBuffer[ScanCodes.sc1_0] ? (byte)0 : (byte)(1 << 3);
                    VIA1_PRB |= matrix.scanCodeBuffer[ScanCodes.sc1_m] ? (byte)0 : (byte)(1 << 4);
                    VIA1_PRB |= matrix.scanCodeBuffer[ScanCodes.sc1_k] ? (byte)0 : (byte)(1 << 5);
                    VIA1_PRB |= matrix.scanCodeBuffer[ScanCodes.sc1_o] ? (byte)0 : (byte)(1 << 6);
                    VIA1_PRB |= matrix.scanCodeBuffer[ScanCodes.sc1_n] ? (byte)0 : (byte)(1 << 7);
                }
                else if (Value == unchecked((byte)~0x20)) // PA5
                {
                    VIA1_PRB =  matrix.scanCodeBuffer[ScanCodes.sc1_minus] ?       (byte)0 : (byte)(1 << 0);
                    VIA1_PRB |= matrix.scanCodeBuffer[ScanCodes.sc1_p] ?           (byte)0 : (byte)(1 << 1);
                    VIA1_PRB |= matrix.scanCodeBuffer[ScanCodes.sc1_l] ?           (byte)0 : (byte)(1 << 2);
                    VIA1_PRB |= matrix.scanCodeBuffer[ScanCodes.sc1_capslock] ?    (byte)0 : (byte)(1 << 3);
                    VIA1_PRB |= matrix.scanCodeBuffer[ScanCodes.sc1_period] ?      (byte)0 : (byte)(1 << 4);
                    VIA1_PRB |= matrix.scanCodeBuffer[ScanCodes.sc1_semicolon] ?   (byte)0 : (byte)(1 << 5);
                    VIA1_PRB |= matrix.scanCodeBuffer[ScanCodes.sc1_bracketLeft] ? (byte)0 : (byte)(1 << 6);
                    VIA1_PRB |= matrix.scanCodeBuffer[ScanCodes.sc1_comma] ?       (byte)0 : (byte)(1 << 7);
                }
                else if (Value == unchecked((byte)~0x40)) // PA6
                {
                    VIA1_PRB =  matrix.scanCodeBuffer[ScanCodes.sc1_equals] ?       (byte)0 : (byte)(1 << 0);
                    VIA1_PRB |= matrix.scanCodeBuffer[ScanCodes.sc1_bracketRight] ? (byte)0 : (byte)(1 << 1);
                    VIA1_PRB |= matrix.scanCodeBuffer[ScanCodes.sc1_apostrophe] ?   (byte)0 : (byte)(1 << 2);
                    VIA1_PRB |= matrix.scanCodeBuffer[ScanCodes.sc1_backslash] ?    (byte)0 : (byte)(1 << 3); // A backslash\ is mapped to the HOME key
                    VIA1_PRB |= matrix.scanCodeBuffer[ScanCodes.sc1_shiftRight] ?   (byte)0 : (byte)(1 << 4);
                    VIA1_PRB |= matrix.scanCodeBuffer[ScanCodes.sc1_altLeft] ?      (byte)0 : (byte)(1 << 5);
                    VIA1_PRB |= matrix.scanCodeBuffer[ScanCodes.sc1_tab] ?          (byte)0 : (byte)(1 << 6);
                    VIA1_PRB |= matrix.scanCodeBuffer[ScanCodes.sc1_slash] ?        (byte)0 : (byte)(1 << 7);

                    matrix.VIA0.VIA0_PRB = matrix.joystickB;
                    matrix.VIA0.VIA0_PRB |= matrix.scanCodeBuffer[(int)ScanCodes.sc1_right_arrow] ? (byte)0 : (byte)(1 << 7);
                }
                else if (Value == unchecked((byte)~0x80)) // PA7
                {
                    VIA1_PRB =  matrix.scanCodeBuffer[ScanCodes.sc1_1] ? (byte)0 : (byte)(1 << 0);
                    VIA1_PRB |= matrix.scanCodeBuffer[ScanCodes.sc1_backspace] ?   (byte)0 : (byte)(1 << 1);
                    VIA1_PRB |= matrix.scanCodeBuffer[ScanCodes.sc1_controlLeft] ? (byte)0 : (byte)(1 << 2);
                    VIA1_PRB |= matrix.scanCodeBuffer[ScanCodes.sc1_2] ?           (byte)0 : (byte)(1 << 3);
                    VIA1_PRB |= matrix.scanCodeBuffer[ScanCodes.sc1_space] ?       (byte)0 : (byte)(1 << 4);
                    VIA1_PRB |= matrix.scanCodeBuffer[ScanCodes.sc1_grave] ?       (byte)0 : (byte)(1 << 5); // A backtick` is mapped to the Foenix key
                    VIA1_PRB |= matrix.scanCodeBuffer[ScanCodes.sc1_q] ?           (byte)0 : (byte)(1 << 6);
                    VIA1_PRB |= matrix.scanCodeBuffer[ScanCodes.sc1_escape] ?      (byte)0 : (byte)(1 << 7); // Escape is mapped to RUN/STOP
                }
            }

            public void WritePortAValue(byte Value)
            {
                // I noticed VIA0 isn't accessible when using the reverse direction on HW, so it is not updated here.
                if (Value == unchecked((byte)~0x1)) // PB0
                {
                    VIA1_PRA  = (1 << 0); // delete key, unmapped
                    VIA1_PRA |= matrix.scanCodeBuffer[ScanCodes.sc1_3] ?            (byte)0 : (byte)(1 << 1);
                    VIA1_PRA |= matrix.scanCodeBuffer[ScanCodes.sc1_5] ?            (byte)0 : (byte)(1 << 2);
                    VIA1_PRA |= matrix.scanCodeBuffer[ScanCodes.sc1_7] ?            (byte)0 : (byte)(1 << 3);
                    VIA1_PRA |= matrix.scanCodeBuffer[ScanCodes.sc1_9] ?            (byte)0 : (byte)(1 << 4);
                    VIA1_PRA |= matrix.scanCodeBuffer[ScanCodes.sc1_minus] ?        (byte)0 : (byte)(1 << 5);
                    VIA1_PRA |= matrix.scanCodeBuffer[ScanCodes.sc1_equals] ?       (byte)0 : (byte)(1 << 6);
                    VIA1_PRA |= matrix.scanCodeBuffer[ScanCodes.sc1_1] ?            (byte)0 : (byte)(1 << 7);
                }
                else if (Value == unchecked((byte)~0x2)) // PB1
                {
                    VIA1_PRA =  matrix.scanCodeBuffer[ScanCodes.sc1_enter] ?        (byte)0 : (byte)(1 << 0);
                    VIA1_PRA |= matrix.scanCodeBuffer[ScanCodes.sc1_w] ?            (byte)0 : (byte)(1 << 1);
                    VIA1_PRA |= matrix.scanCodeBuffer[ScanCodes.sc1_r] ?            (byte)0 : (byte)(1 << 2);
                    VIA1_PRA |= matrix.scanCodeBuffer[ScanCodes.sc1_y] ?            (byte)0 : (byte)(1 << 3);
                    VIA1_PRA |= matrix.scanCodeBuffer[ScanCodes.sc1_i] ?            (byte)0 : (byte)(1 << 4);
                    VIA1_PRA |= matrix.scanCodeBuffer[ScanCodes.sc1_p] ?            (byte)0 : (byte)(1 << 5);
                    VIA1_PRA |= matrix.scanCodeBuffer[ScanCodes.sc1_bracketRight] ? (byte)0 : (byte)(1 << 6);
                    VIA1_PRA |= matrix.scanCodeBuffer[ScanCodes.sc1_backspace] ?    (byte)0 : (byte)(1 << 7);
                }
                else if (Value == unchecked((byte)~0x4)) // PB2
                {
                    VIA1_PRA =  matrix.scanCodeBuffer[ScanCodes.sc1_left_arrow] ?   (byte)0 : (byte)(1 << 0);
                    VIA1_PRA |= matrix.scanCodeBuffer[ScanCodes.sc1_a] ?            (byte)0 : (byte)(1 << 1);
                    VIA1_PRA |= matrix.scanCodeBuffer[ScanCodes.sc1_d] ?            (byte)0 : (byte)(1 << 2);
                    VIA1_PRA |= matrix.scanCodeBuffer[ScanCodes.sc1_g] ?            (byte)0 : (byte)(1 << 3);
                    VIA1_PRA |= matrix.scanCodeBuffer[ScanCodes.sc1_j] ?            (byte)0 : (byte)(1 << 4);
                    VIA1_PRA |= matrix.scanCodeBuffer[ScanCodes.sc1_l] ?            (byte)0 : (byte)(1 << 5);
                    VIA1_PRA |= matrix.scanCodeBuffer[ScanCodes.sc1_semicolon] ?    (byte)0 : (byte)(1 << 6);
                    VIA1_PRA |= matrix.scanCodeBuffer[ScanCodes.sc1_controlLeft] ?  (byte)0 : (byte)(1 << 7);
                }
                else if (Value == unchecked((byte)~0x8)) // PB3
                {
                    VIA1_PRA =  matrix.scanCodeBuffer[ScanCodes.sc1_F7] ?           (byte)0 : (byte)(1 << 0);
                    VIA1_PRA |= matrix.scanCodeBuffer[ScanCodes.sc1_4] ?            (byte)0 : (byte)(1 << 1);
                    VIA1_PRA |= matrix.scanCodeBuffer[ScanCodes.sc1_6] ?            (byte)0 : (byte)(1 << 2);
                    VIA1_PRA |= matrix.scanCodeBuffer[ScanCodes.sc1_8] ?            (byte)0 : (byte)(1 << 3);
                    VIA1_PRA |= matrix.scanCodeBuffer[ScanCodes.sc1_0] ?            (byte)0 : (byte)(1 << 4);
                    VIA1_PRA |= matrix.scanCodeBuffer[ScanCodes.sc1_capslock] ?     (byte)0 : (byte)(1 << 5);
                    VIA1_PRA |= matrix.scanCodeBuffer[ScanCodes.sc1_backslash] ?    (byte)0 : (byte)(1 << 6);  // A backslash\ is mapped to the HOME key
                    VIA1_PRA |= matrix.scanCodeBuffer[ScanCodes.sc1_2] ?            (byte)0 :  (byte)(1 << 7);
                }
                else if (Value == unchecked((byte)~0x10)) // PB4
                {
                    VIA1_PRA =  matrix.scanCodeBuffer[ScanCodes.sc1_F1] ?           (byte)0 : (byte)(1 << 0);
                    VIA1_PRA |= matrix.scanCodeBuffer[ScanCodes.sc1_z] ?            (byte)0 : (byte)(1 << 1);
                    VIA1_PRA |= matrix.scanCodeBuffer[ScanCodes.sc1_c] ?            (byte)0 : (byte)(1 << 2);
                    VIA1_PRA |= matrix.scanCodeBuffer[ScanCodes.sc1_b] ?            (byte)0 : (byte)(1 << 3);
                    VIA1_PRA |= matrix.scanCodeBuffer[ScanCodes.sc1_m] ?            (byte)0 : (byte)(1 << 4);
                    VIA1_PRA |= matrix.scanCodeBuffer[ScanCodes.sc1_period] ?       (byte)0 : (byte)(1 << 5);
                    VIA1_PRA |= matrix.scanCodeBuffer[ScanCodes.sc1_shiftRight] ?   (byte)0 : (byte)(1 << 6);
                    VIA1_PRA |= matrix.scanCodeBuffer[ScanCodes.sc1_space] ?        (byte)0 : (byte)(1 << 7);
                }
                else if (Value == unchecked((byte)~0x20)) // PB5
                {
                    VIA1_PRA =  matrix.scanCodeBuffer[ScanCodes.sc1_F3] ?           (byte)0 : (byte)(1 << 0);
                    VIA1_PRA |= matrix.scanCodeBuffer[ScanCodes.sc1_s] ?            (byte)0 : (byte)(1 << 1);
                    VIA1_PRA |= matrix.scanCodeBuffer[ScanCodes.sc1_f] ?            (byte)0 : (byte)(1 << 2);
                    VIA1_PRA |= matrix.scanCodeBuffer[ScanCodes.sc1_h] ?            (byte)0 : (byte)(1 << 3);
                    VIA1_PRA |= matrix.scanCodeBuffer[ScanCodes.sc1_k] ?            (byte)0 : (byte)(1 << 4);
                    VIA1_PRA |= matrix.scanCodeBuffer[ScanCodes.sc1_apostrophe] ?   (byte)0 : (byte)(1 << 5);
                    VIA1_PRA |= matrix.scanCodeBuffer[ScanCodes.sc1_altLeft] ?      (byte)0 : (byte)(1 << 6);
                    VIA1_PRA |= matrix.scanCodeBuffer[ScanCodes.sc1_grave] ?        (byte)0 : (byte)(1 << 7); // A backtick` is mapped to the Foenix key
                }
                else if (Value == unchecked((byte)~0x40)) // PB6
                {
                    VIA1_PRA =  matrix.scanCodeBuffer[ScanCodes.sc1_F5] ?           (byte)0 : (byte)(1 << 0);
                    VIA1_PRA |= matrix.scanCodeBuffer[ScanCodes.sc1_e] ?            (byte)0 : (byte)(1 << 1);
                    VIA1_PRA |= matrix.scanCodeBuffer[ScanCodes.sc1_t] ?            (byte)0 : (byte)(1 << 2);
                    VIA1_PRA |= matrix.scanCodeBuffer[ScanCodes.sc1_u] ?            (byte)0 : (byte)(1 << 3);
                    VIA1_PRA |= matrix.scanCodeBuffer[ScanCodes.sc1_o] ?            (byte)0 : (byte)(1 << 4);
                    VIA1_PRA |= matrix.scanCodeBuffer[ScanCodes.sc1_bracketLeft] ?  (byte)0 : (byte)(1 << 5);
                    VIA1_PRA |= matrix.scanCodeBuffer[ScanCodes.sc1_tab] ?          (byte)0 : (byte)(1 << 6);
                    VIA1_PRA |= matrix.scanCodeBuffer[ScanCodes.sc1_q] ?            (byte)0 : (byte)(1 << 7);
                }
                else if (Value == unchecked((byte)~0x80)) // PB7
                {
                    VIA1_PRA =  matrix.scanCodeBuffer[ScanCodes.sc1_up_arrow] ?     (byte)0 : (byte)(1 << 0);
                    VIA1_PRA |= matrix.scanCodeBuffer[ScanCodes.sc1_shiftLeft] ?    (byte)0 : (byte)(1 << 1);
                    VIA1_PRA |= matrix.scanCodeBuffer[ScanCodes.sc1_x] ?            (byte)0 : (byte)(1 << 2);
                    VIA1_PRA |= matrix.scanCodeBuffer[ScanCodes.sc1_v] ?            (byte)0 : (byte)(1 << 3);
                    VIA1_PRA |= matrix.scanCodeBuffer[ScanCodes.sc1_n] ?            (byte)0 : (byte)(1 << 4);
                    VIA1_PRA |= matrix.scanCodeBuffer[ScanCodes.sc1_comma] ?        (byte)0 : (byte)(1 << 5);
                    VIA1_PRA |= matrix.scanCodeBuffer[ScanCodes.sc1_slash] ?        (byte)0 : (byte)(1 << 6);
                    VIA1_PRA |= matrix.scanCodeBuffer[ScanCodes.sc1_escape] ?       (byte)0 : (byte)(1 << 7);  // Escape is mapped to RUN/STOP
                }
                else
                {
                    VIA1_PRA = 0xFF;
                }
            }

            bool CanRead(int Address)
            {
                if (Address == 0) // Port B
                {
                    bool canReadPortB = VIA1_DDRB == 0;
                    return canReadPortB;
                }
                else if (Address == 1) // Port A
                {
                    bool canReadPortA = VIA1_DDRA == 0;
                    return canReadPortA;
                }
                return true;
            }

            public override byte ReadByte(int Address)
            {
                if (!CanRead(Address))
                    return 0;
                if (Address < 4)
                {
                    return data[Address];
                }
                else 
                { 
                    return 0;
                }
            }
        }

        bool[] scanCodeBuffer;
        byte joystickA = 0x7F; // Port A - 6 buttons left, right, up, down, left button, right button
        byte joystickB = 0x7F; // Port B - 6 buttons left, right, up, down, left button, right button

        // Making this public for now, as this is used by the joystick port
        public VIA0Range VIA0; // Used for the joystick and right and down arrow keys
        public VIA1Range VIA1; // Used for all the other keys

        // This constructor is called for the F256Jr
        public VIARegisters(int Range0StartAddress, int Range0Length)
        {
            System.Diagnostics.Debug.Assert(Range0Length == 4);
            scanCodeBuffer = new bool[0x80];
            VIA0 = new VIA0Range(this, Range0StartAddress, Range0Length);        // The joystick port for F256K and F256Jr + 2 keys in F256K
            VIA1 = null;
        }

        // This constructor is called for the F256K
        public VIARegisters(int Range0StartAddress, int Range0Length, int Range1StartAddress, int Range1Length)
        {
            System.Diagnostics.Debug.Assert(Range0Length == 4);
            scanCodeBuffer = new bool[(int)ScanCodes.sc1_down_arrow + 1];
            VIA0 = new VIA0Range(this, Range0StartAddress, Range0Length);        // The joystick port for F256K and F256Jr + 2 keys in F256K
            VIA1 = new VIA1Range(this, Range1StartAddress, Range1Length);  // only the F256K matrix keyboard
        }

        public void WriteScanCode(byte sc)
        {
            // if bit 7 is set, the key was released
            scanCodeBuffer[sc & 0x7F] = (sc & 0x80) == 0;
        }
        public void JoystickCode(byte port, byte value)
        {
            switch(port)
            {
                case 0: // port B
                    joystickB = (byte)(value & 0x7F);
                    VIA0.WriteByte(2, 0xFF);
                    VIA0.WriteByte(0, joystickB);
                    VIA0.WriteByte(2, 0);
                    break;
                case 1:
                    joystickA = (byte)(value & 0x7F);
                    VIA0.WriteByte(3, 0xFF);
                    VIA0.WriteByte(1, joystickA);
                    VIA0.WriteByte(3, 0);
                    break;
            }
        }
    }
}
