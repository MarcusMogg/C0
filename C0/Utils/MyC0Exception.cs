using System;
using System.Collections.Generic;
using System.Text;
using C0.Tokenizer;

namespace C0.Utils
{
    [Serializable()]
    public class MyC0Exception : Exception
    {
        public Pos ErrPos { get; set; }

        public MyC0Exception(string message, Pos p) : base(message)
        {
            ErrPos = p;
        }

        public static MyC0Exception FileNotFoundErr()
        {
            return new MyC0Exception("文件未找到",new Pos(-1,0));
        }
        public static MyC0Exception FileIoErr()
        {
            return new MyC0Exception("文件读写异常",new Pos(-1,0));
        }
        public static MyC0Exception InvalidInputErr(Pos p)
        {
            return new MyC0Exception("无效的输入字符",p);
        }
        public static MyC0Exception IllegalNumberErr(Pos p)
        {
            return new MyC0Exception("数字不合法（前导0）", p);
        }
        public static MyC0Exception UnreadBeginErr()
        {
            return new MyC0Exception("analyser unreads token from the begining.", new Pos(-1, 0));
        }
        public static MyC0Exception IncompleteErr(Pos p)
        {
            return new MyC0Exception("语句不完整", p);
        }
        public static MyC0Exception InvalidTokenErr(Pos p)
        {
            return new MyC0Exception("token错误", p);
        }
        public static MyC0Exception ConstNotInitializedErr(Pos p)
        {
            return new MyC0Exception("const变量未初始化", p);
        }
        public static MyC0Exception AlreadyExistErr(Pos p)
        {
            return new MyC0Exception("变量已被声明", p);
        }
        public static MyC0Exception NotExistErr(Pos p)
        {
            return new MyC0Exception("变量未被声明", p);
        }
        public static MyC0Exception MissBrackets(Pos p)
        {
            return new MyC0Exception("括号不匹配", p);
        }
        public static MyC0Exception MissSemicolonErr(Pos p)
        {
            return new MyC0Exception("缺少分号", p);
        }
        public static MyC0Exception NotInitializedErr(Pos p)
        {
            return new MyC0Exception("变量未初始化", p);
        }
        public static MyC0Exception VoidErr(Pos p)
        {
            return new MyC0Exception("变量类型不能为void", p);
        }
        public static MyC0Exception CantVoidErr(Pos p)
        {
            return new MyC0Exception("表达式中的函数返回值不能为void", p);
        }
        public static MyC0Exception CantConstErr(Pos p)
        {
            return new MyC0Exception("常量的值不能改变", p);
        }
        public static MyC0Exception MissMainErr()
        {
            return new MyC0Exception("缺少main函数", new Pos(-1,0));
        }
        public static MyC0Exception MissRightComment(Pos p)
        {
            return new MyC0Exception("注释不匹配", p);
        }
    }
}
