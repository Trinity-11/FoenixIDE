using System;
using System.Collections.Generic;
using System.Text;

namespace FoenixIDE
{
    public class RingBuffer<T>
    {
        readonly int size = 16384;
        public T[] data;
        int readPos = 0;
        int writePos = 0;

        public RingBuffer(int Capacity = 4096)
        {
            this.size = Capacity;
            data = new T[size];
        }

        public bool IsEmpty()
        {
            return ReadPos == WritePos;
        }

        public int Count
        {
            get
            {
                int len = WritePos - ReadPos;
                if (len < 0)
                    len += size;
                return len;
            }
        }

        public int Capacity
        {
            get
            {
                return size;
            }
        }

        public int ReadPos
        {
            get
            {
                return this.readPos;
            }

            protected set
            {
                this.readPos = value;
            }
        }

        public int WritePos
        {
            get
            {
                return this.writePos;
            }

            protected set
            {
                this.writePos = value;
            }
        }

        public int CountToEnd
        {
            get
            {
                if (readPos > writePos)
                    return size - readPos;
                else
                    return writePos - readPos;
            }
        }

        public void Clear()
        {
            WritePos = 0;
            ReadPos = 0;
        }

        public T this[int index]
        {
            get
            {
                return data[(ReadPos + index) % size];
            }
            set
            {
                data[(WritePos + index) % size] = value;
            }
        }

        public void Add(T[] data)
        {
            for (int i = 0; i < data.Length; i++)
            {
                Add(data[i]);
            }
        }

        public void Add(T data)
        {
            this.data[this.WritePos] = data;
            WritePos = (WritePos + 1) % size;
            if (ReadPos == WritePos)
                throw new Exception("Buffer full");
        }

        public T Peek()
        {
            return data[ReadPos];
        }

        public T Read()
        {
            int pos = ReadPos;
            ReadPos = (ReadPos + 1) % size;
            return data[pos];
        }

        public int Read(T[] Data, int Max)
        {
            int max = Max;
            if (this.Count < max)
                max = this.Count;
            if (Data.Length < max)
                max = Data.Length;

            for(int i=0; i<max; i++)
                Data[i] = Read();

            return max;
        }

        public void Read(T[] buffer, int offset, int len)
        {
            for (int i = 0; i < len; ++i)
                buffer[i] = Read();
        }

        public void Discard(int Num)
        {
            if (Count < Num)
                Num = Count;

            ReadPos = (ReadPos + Num) % size;
        }

        //public void Debug_WriteBuffer(int Length)
        //{
        //    int i = 0;
        //    while (i < Length)
        //    {
        //        System.Diagnostics.Debug.Write(i.ToString("X2") + " : ");
        //        for (int j = i; j < i + 16; ++j)
        //        {
        //            if (j < Length)
        //                System.Diagnostics.Debug.Write(this[j].ToString());
        //            else
        //                System.Diagnostics.Debug.Write("  ");
        //            System.Diagnostics.Debug.Write(' ');
        //        }
        //        System.Diagnostics.Debug.Write(": ");
        //        for (int j = i; j < i + 16; ++j)
        //        {
        //            string s;
        //            if (j < Length)
        //                s = this[j].ToString();
        //            else
        //                s = " ";
        //            if (s < " " || s > "~")
        //                s = ".";
        //            System.Diagnostics.Debug.Write(s);
        //        }
        //        i += 16;
        //        System.Diagnostics.Debug.WriteLine(" :");
        //    }
        //    System.Diagnostics.Debug.WriteLine("");
        //}
    }
}
