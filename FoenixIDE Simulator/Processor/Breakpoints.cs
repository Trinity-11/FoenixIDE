using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoenixIDE.Processor
{
    public class Breakpoints : SortedList<int, string>
    {
        /// <summary>
        /// Checks whether the address is a breakpoint
        /// </summary>
        /// <param name="Address"></param>
        /// <returns></returns>
        bool CheckBP(int Address)
        {
            if (this.Count == 0)
                return false;

            if (this.ContainsKey(Address))
                return true;

            return false;
        }

        public string Format(string Hex)
        {
            int val = GetIntFromHex(Hex);
            return GetHex(val);
        }

        public string GetHex(int value)
        {
            return "$" + value.ToString("X6");
        }

        public int GetIntFromHex(string Hex)
        {
            try
            {
                if (Hex.StartsWith("$") && Hex.Length > 1)
                    Hex = Hex.Substring(1);
                int ret = Convert.ToInt32(Hex, 16);
                return ret;
            }
            catch (FormatException)
            {
                return -1;
            }
        }

        public void Add(string HexAddress)
        {
            try
            {
                int Addr = GetIntFromHex(HexAddress);
                this.Add(Addr, "$" + Addr.ToString("X6"));
            }
            catch (Exception ex)
            {
                global::System.Diagnostics.Debug.WriteLine("Breakpoints.Add(" + HexAddress + ")");
                global::System.Diagnostics.Debug.WriteLine("Message:  " + ex.Message);
            }
        }

        public void Remove(string HexAddress)
        {
            try
            {
                int Addr = GetIntFromHex(HexAddress);
                if (this.ContainsKey(Addr))
                    this.Remove(Addr);
            }
            catch (Exception ex)
            {
                global::System.Diagnostics.Debug.WriteLine("Breakpoints.Remove(" + HexAddress + ")");
                global::System.Diagnostics.Debug.WriteLine("Message:  " + ex.Message);
            }
        }

        public string[] GetHexArray()
        {
            string[] ret = new string[this.Count];
            this.Values.CopyTo(ret, 0);
            return ret;
        }

        public int[] GetIntArray()
        {
            int[] ret = new int[this.Count];
            this.Keys.CopyTo(ret, 0);
            return ret;
        }
    }
}
