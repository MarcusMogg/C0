using System;
using System.Collections.Generic;
using System.Text;
using C0.Tokenizer;

namespace C0.Analyser
{
    public class TypeSpecifier
    {
        public TypeSpecifier(TokenType tokenType)
        {
            TokenType = tokenType;
        }

        public TokenType TokenType { get; set; }

        public static bool IsTypeSpecifier(TokenType t)
        {
            return t == TokenType.Int || t == TokenType.Void || t == TokenType.Char;
        }
    }
}
