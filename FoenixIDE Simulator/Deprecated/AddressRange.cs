using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoenixIDE
{
    public class AddressRange : IComparable<int>, IComparable<AddressRange>
    {
        public int Begin = 0;
        public int End = 0;

        public AddressRange() : this(0, 0)
        {
        }

        public AddressRange(int newBegin, int newEnd)
        {
            this.Begin = newBegin;
            this.End = newEnd;
        }

        /// <summary>
        /// Compares a single address with this address range. Returns 0 when the provided address falls in this range. 
        /// Returns -1 when this range < address
        /// Returns 1 when this range > address
        /// </summary>
        /// <paramref name="other">address to compare</paramref>
        /// <returns></returns>
        public int CompareTo(int other)
        {
            if (End < other)
                return -1;
            if (Begin > other)
                return 1;
            return 0;
        }

        public int CompareTo(AddressRange other)
        {
            if (other.End < this.Begin)
                return 1;
            if (other.Begin > this.End)
                return 0;
            return 0;
        }
    }
}
