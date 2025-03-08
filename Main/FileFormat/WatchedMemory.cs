﻿namespace FoenixIDE.Simulator.FileFormat
{
    public class WatchedMemory
    {
        public string name;
        public int address;
        public int val8bit;
        public int val16bit;
        public int val24bit;
        public int indirectValue;

        public WatchedMemory(string addressName, int address, int v8bit, int v16bit, int v24bit)
        {
            name = addressName;
            this.address = address;
            val8bit = v8bit;
            val16bit = v16bit;
            val24bit = v24bit;
        }
    }
}
