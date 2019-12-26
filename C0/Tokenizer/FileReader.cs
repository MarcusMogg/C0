using System;
using System.Collections.Generic;
using System.IO;

namespace C0.Tokenizer
{
    public class FileReader
    {
        private readonly string _path;
        private readonly List<String> _content;
        private int _lines;
        public Pos CurPos;

        public FileReader(string path)
        {
            _path = path;
            _content = new List<string>();
            CurPos = new Pos(0, 0);
            _lines = 0;
        }

        public void ReadAll()
        {
            using (StreamReader s = new StreamReader(_path))
            {
                while (!s.EndOfStream)
                {
                    _content.Add(s.ReadLine() + "\n");
                    _lines++;
                }
            }
            if (_content.Count > 0)
                _content[0] = " " + _content[0];
        }

        public Pos NextPos()
        {
            try
            {
                if (CurPos.X >= _lines)
                {
                    return CurPos;
                }
                if (CurPos.Y >= _content[CurPos.X].Length - 1)
                {
                    return new Pos(CurPos.X + 1, 0);
                }
                return new Pos(CurPos.X, CurPos.Y + 1);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public char? PeekNextChar()
        {
            Pos p = NextPos();
            if (p.X >= _lines)
            {
                return null;
            }
            return _content[p.X][p.Y];
        }

        public void ReadNext()
        {
            CurPos = NextPos();
        }
    }
}
