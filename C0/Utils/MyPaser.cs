using System;
using System.Collections.Generic;
using System.Text;

namespace C0.Utils
{
    public class MyParse
    {
        public static int ParseInt(string s)
        {
            int res = 0;
            if (int.TryParse(s, out res))
            {
                return res;
            }

            unchecked
            {
                foreach (var i in s)
                {
                    res = res * 10 + (i - '0');
                }
            }
            return res;
        }
        public static int ParseHex(string s)
        {
            int res = 0;
            unchecked
            {
                s = s.Remove(0, 2);
                foreach (var i in s)
                {
                    res = res * 16;
                    if (char.IsDigit(i))
                    {
                        res += i - '0';
                    }
                    else
                    {
                        res += char.ToLower(i) - 'a' + 10;
                    }
                }
            }
            return res;
        }
    }
}
