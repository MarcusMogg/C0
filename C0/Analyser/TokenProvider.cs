using System;
using System.Collections.Generic;
using System.Text;
using C0.Tokenizer;
using C0.Utils;

namespace C0.Analyser
{
    public class TokenProvider
    {
        private List<Token> _tokens;
        private static TokenProvider _tokenProvider;
        private int _cur;
        private int _num;
        private TokenProvider()
        {
            _tokens = new List<Token>();
            _cur = -1;
            _num = 0;
        }

        public static TokenProvider GetInstance()
        {
            return _tokenProvider ?? (_tokenProvider = new TokenProvider());
        }

        public void SetToken(List<Token> t)
        {
            _tokens = t;
            _num = t.Count;
        }

        public Token PeekNextToken()
        {
            if (_cur + 1 >= _num)
            {
                return null;
            }
            return _tokens[_cur + 1];
        }
        public Token GetCurToken()
        {
            if (_cur == -1 || _cur >= _num) return null;
            return _tokens[_cur];
        }

        public void Next()
        {
            _cur++;
        }

        public void Unread()
        {
            if (_cur == -1)
            {
                throw MyC0Exception.UnreadBeginErr();
            }
            _cur--;
        }
    }
}
