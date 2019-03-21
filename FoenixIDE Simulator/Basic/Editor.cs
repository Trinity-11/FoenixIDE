using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoenixIDE.Basic
{
    class Editor
    {
        public void Draw(FoenixSystem kernel)
        {
            kernel.Cls();
            kernel.Locate(0, 0);
            //for (i = 0; i < Columns; i += 1)
            //{
            //    string s = (i % 10).ToString();
            //    Print("-");
            //}
            int ln = 1;
            for (int y = 0; y < kernel.Lines - 1; y++)
            {
                kernel.Locate(y, 0);
                kernel.Print(ln.ToString().PadLeft(5) + "\xe062");
                //Print("xxxxxxxxxx");
                ln++;
            }
            kernel.Locate(kernel.Lines - 1, 0);
            kernel.Print("1\x0012Select  \x0092 ");
            kernel.Print("2\x0012Save    \x0092 ");
            kernel.Print("3\x0012Find    \x0092 ");
            kernel.Print("4\x0012Replace \x0092 ");
            kernel.Print("5\x0012Run     \x0092 ");
            kernel.Print("6\x0012Compile \x0092 ");
            kernel.Print("7\x0012Menu    \x0092 ");
            kernel.Print("8\x0012Exit    \x0092");
            kernel.Locate(0, 6);
        }
    }
}
