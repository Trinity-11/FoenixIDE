using System;

namespace FoenixIDE.Simulator.Devices
{
    /**
     * This PS2Keyboard is used for the F256s.
     * In this case, the mode is not required.
     */
    public class PS2KeyboardRegisterSet2: PS2KeyboardRegister
    {
        bool isK_WR = false;
        bool isM_WR = false;
        bool K_AK = false;
        bool M_AK = false;

        private byte kbPacketCntr = 0; 
        private byte msPacketCntr = 0;
        private int kbQLen = 0;
        private int msQLen = 0;
        private byte[] kbFifo = new byte[6];
        private byte[] msFifo = new byte[3];

        public PS2KeyboardRegisterSet2(int StartAddress, int Length) : base(StartAddress, Length)
        {

        }

        // This is used to simulate the Keyboard Register
        public override void WriteByte(int Address, byte Value)
        {
            // Only addresses 0 and 1 are writable
            switch (Address)
            {
                case 0:
                    data[0] = Value;
                    switch (Value)
                    {
                        case 0:
                            if (isK_WR)
                            {
                                // write out the byte in data[1] to keyboard
                                isK_WR = false;
                                K_AK = true;
                            }
                            if (isM_WR)
                            {
                                // write out the byte in data[1] to mouse
                                isM_WR = false;
                                M_AK = true;
                            }
                            break;
                        case 2:
                            isK_WR = true;
                            break;
                        case 8:
                            isM_WR = true;
                            break;
                        case 0x10: // clear keyboard fifo
                            Array.Clear(kbFifo, 0, 6);
                            break;
                        case 0x20: // clear mouse fifo
                            Array.Clear(msFifo, 0, 3);
                            break;
                    }
                    break;
                case 1:
                    data[1] = Value;
                    break;
            }
        }

        public override byte ReadByte(int Address)
        {
            // Whenever the buffer is read, set the buffer to empty.
            switch(Address)
            {
                case 0:
                case 1:
                    return data[Address];
                case 2:
                    // Read from the keyboard fifo
                    if (kbPacketCntr > kbQLen)
                    {
                        return 0;
                    }
                    byte kbval = kbFifo[kbPacketCntr++];
                    if (kbPacketCntr == kbQLen)
                    {
                        kbPacketCntr = 0;
                        kbQLen = 0;
                        Array.Clear(kbFifo, 0, 6);
                    }
                    return kbval;
                case 3:
                    // Read from the mouse fifo
                    if (msPacketCntr> msQLen)
                    {
                        return 0;
                    }
                    byte msval = msFifo[msPacketCntr++];
                    if (msPacketCntr == msQLen)
                    {
                        msPacketCntr = 0;
                        msQLen = 0;
                        Array.Clear(msFifo, 0, 3);
                    }
                    return msval;
                case 4:
                    K_AK = false;
                    M_AK = false;
                    return (byte)((K_AK ? 0x80:0) + (M_AK ? 0x20 : 0) + (msQLen == 0 ? 2 : 0) + (kbQLen == 0? 1 : 0));
            }

            return data[Address];
        }
        public override void WriteScanCodeSequence(byte[] codes, int seqLength)
        {
            kbPacketCntr = 0;  // the first byte is already written to the 
            kbQLen = seqLength;
            Array.Clear(kbFifo, 0, 6);
            Array.Copy(codes, kbFifo, seqLength);

            TriggerKeyboardInterrupt?.Invoke();
        }
        
        public override void MousePackets(byte buttons, byte X, byte Y)
        {
            msPacketCntr = 0;
            msQLen = 3;
            msFifo[0] = buttons;
            msFifo[1] = X;
            msFifo[2] = Y;

            TriggerMouseInterrupt?.Invoke();
        }

    }
}
