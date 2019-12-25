using System.Collections.Generic;

namespace C0.Tokenizer
{
    public class TokenUtils
    {
        private static readonly HashSet<char> Punctuation = new HashSet<char>
        {
            //'_','[',']', '.', ':', '?','%','^','&','|','~','\\', '\"','\'','`', '$' ,'#' ,'@',
            '(',')','{','}','<','=','>',',',';','!','+','-','*','/'

        };
        public static bool IsSpace(char c)
        {
            return c == ' ' || c == '\t' || c == '\r' || c == '\n';
        }

        public static bool IsNum(char c)
        {
            return c >= '0' && c <= '9';
        }

        public static bool IsLetter(char c)
        {
            return (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z');
        }
        public static bool IsHexLetter(char c)
        {
            return IsNum(c) || (c >= 'a' && c <= 'f') || (c >= 'A' && c <= 'F');
        }

        public static bool IsPun(char c)
        {
            return Punctuation.Contains(c);
        }

        public static bool AcChar(char c)
        {
            return IsNum(c) || IsLetter(c) || IsPun(c) || IsSpace(c);
        }

        public static readonly Dictionary<string, TokenType> KeyWords = new Dictionary<string, TokenType>
        {
            { "const"   , TokenType.Const},
            { "void"    , TokenType.Void },
            { "int"     , TokenType.Int   },
            { "char"    , TokenType.Char   },
            { "double"  , TokenType.Double   },
            { "struct"  , TokenType.Struct   },
            { "if"      , TokenType.If   },
            { "else"    , TokenType.Else   },
            { "switch"  , TokenType.Switch   },
            { "case"    , TokenType.Case   },
            { "default" , TokenType.Default   },
            { "while"   , TokenType.While   },
            { "for"     , TokenType.For   },
            { "do"      , TokenType.Do   },
            { "return"  , TokenType.Return   },
            { "break"   , TokenType.Break   },
            { "continue", TokenType.Continue   },
            { "print"   , TokenType.Print   },
            { "scan"    , TokenType.Scan   }
        };
    }
}
