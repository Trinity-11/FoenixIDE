using System;
using System.Collections.Generic;

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
            String val = value.ToString("X6");
            return "$" + val.Substring(0,2) + ":"+ val.Substring(2);
        }

        public int GetIntFromHex(string Hex)
        {
            try
            {
                int ret = Convert.ToInt32(Hex.Replace("$","").Replace(":", ""), 16);
                return ret;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public int Add(string HexAddress)
        {
            try
            {
                int Addr = GetIntFromHex(HexAddress);
                this.Add(Addr, GetHex(Addr));
                return Addr;
            }
            catch (Exception ex)
            {
                global::System.Diagnostics.Debug.WriteLine("Breakpoints.Add(" + HexAddress + ")");
                global::System.Diagnostics.Debug.WriteLine("Message:  " + ex.Message);
                return -1;
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
