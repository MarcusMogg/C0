using System;
using System.Collections.Generic;
using System.Text;

namespace C0.Tokenizer
{
    public class Pos
    {
        public Pos(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; set; }
        public int Y { get; set; }

        public Pos BeginPos { get; set; }
    }
}
