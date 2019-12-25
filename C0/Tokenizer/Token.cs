using System;
using System.Collections.Generic;
using System.Text;

namespace C0.Tokenizer
{
    public class Token
    {
        public TokenType Type { get; set; }
        public dynamic Content { get; set; }
        public Pos BeginPos { get; set; }


        public Token(TokenType type, dynamic content, Pos beginPos)
        {
            Type = type;
            Content = content;
            BeginPos = beginPos;
        }

        public override String ToString()
        {
            return $"{BeginPos.X}  {BeginPos.Y}     {Type}    {Content}";
        }
    }
}
