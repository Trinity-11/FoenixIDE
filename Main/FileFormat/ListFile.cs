using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoenixIDE.Simulator.FileFormat
{
    /// <summary>
    /// This class loads a .lst file.  This should make parsing of byte codes unnecessary.
    /// </summary>
    public class ListFile
    {
        private Dictionary<int, DebugLine> DbgLines = new Dictionary<int, DebugLine>();

        internal Dictionary<int, DebugLine> Lines
        {
            get => DbgLines;
        }

        public ListFile(String KernelFilename)
        {
            int lastDot = KernelFilename.LastIndexOf(".");
            string Filename = KernelFilename.Substring(0, lastDot + 1) + "lst";
            int CommandOffset = 1;
            if (System.IO.File.Exists(Filename))
            {
                string[] lines = System.IO.File.ReadAllLines(Filename);
                int pc = 0;
                DbgLines.Clear();
                foreach (string line in lines)
                {
                    if (line.StartsWith("."))
                    {
                        // Read the PC and Command from the list
                        string[] tokens = line.Split(new char[] { '\t' });
                        if (tokens.Length > 2)
                        {
                            pc = Convert.ToInt32(tokens[0].Replace(".",""), 16);

                            // check if a match already exists
                            DebugLine match;
                            DbgLines.TryGetValue(pc, out match);
                            if (match == null)
                            {
                                match = new DebugLine(pc);
                                DbgLines.Add(pc, match);
                            }
                            string[] strOpcodes = tokens[CommandOffset].Split(new char[] { ' ' });
                            if (strOpcodes[0].Length == 0)
                            {
                                match.SetLabel(tokens[tokens.Length - 1]);
                            }
                            else
                            {
                                byte[] opcodes = Array.ConvertAll(strOpcodes, value => Convert.ToByte(value, 16));
                                match.SetOpcodes(opcodes);
                                match.SetMnemonic(tokens[tokens.Length - 1]);
                            }
                            
                        }
                    } 
                    else if (line.StartsWith(";Offset"))
                    {
                        // sometimes, there's an addition field added, which makes the command offset 2, instead of 1
                        string[] tokens = line.Split(new char[] { '\t' });
                        for (int i = 0; i < tokens.Length; i++) 
                        {
                            if (tokens[i].Equals(";Hex"))
                            {
                                CommandOffset = i;
                                break;
                            }
                        }
                    }
                }
            }
            else
            {
                Debug.WriteLine("List file '" + Filename + "' not found");
            }
        }
    }
}
