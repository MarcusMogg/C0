using System;
using System.Collections.Generic;
using System.Text;
using C0.Instruction;
using C0.Tokenizer;
using C0.Utils;

namespace C0.Analyser.Statement
{
    public class PrintStatement
    {
        public PrintStatement()
        {
            Expressions = new List<dynamic>();
        }

        public List<dynamic> Expressions { get; set; }
        public static PrintStatement Analyse(string par)
        {
            var res = new PrintStatement();
            TokenProvider tokenProvider = TokenProvider.GetInstance();
            Token t = tokenProvider.PeekNextToken();

            if (t.Type != TokenType.Print)
            {
                throw MyC0Exception.InvalidTokenErr(t.BeginPos);
            }
            tokenProvider.Next();
            t = tokenProvider.PeekNextToken();
            if (t.Type != TokenType.BracketsLeftRound)
            {
                throw MyC0Exception.InvalidTokenErr(t.BeginPos);
            }
            tokenProvider.Next();

            while (true)
            {
                t = tokenProvider.PeekNextToken();
                if (t.Type == TokenType.BracketsRightRound)
                {
                    break;
                }
                if (t.Type == TokenType.Comma)
                {
                    tokenProvider.Next();
                    continue;
                }

                if (t.Type == TokenType.Char)
                {
                    res.Expressions.Add(t.Content);
                    tokenProvider.Next();
                }
                else if (t.Type == TokenType.String)
                {
                    res.Expressions.Add(t.Content);
                    tokenProvider.Next();
                }
                else
                {
                    res.Expressions.Add(Expression.Expression.Analyse(par));
                }
            }

            t = tokenProvider.PeekNextToken();
            if (t.Type != TokenType.BracketsRightRound)
            {
                throw MyC0Exception.InvalidTokenErr(t.BeginPos);
            }
            tokenProvider.Next();
            t = tokenProvider.PeekNextToken();
            tokenProvider.Next();
            if (t.Type != TokenType.Semicolon)
            {
                throw MyC0Exception.MissSemicolonErr(t.BeginPos);
            }

            return res;
        }

        public List<IInstruction> GetIns(string par, int offset)
        {
            var res = new List<IInstruction>();
            int l = Expressions.Count;
            for (int i = 0; i < l; i++)
            {
                if (Expressions[i] is Expression.Expression)
                {
                    res.AddRange(Expressions[i].GetIns(par, offset + res.Count));
                    if (Expressions[i].Type == TokenType.Char)
                    {
                        res.Add(new CPrint());
                    }
                    else
                    {
                        res.Add(new IPrint());
                    }

                }
                else if (Expressions[i] is char)
                {
                    char tmp = Expressions[i];
                    res.Add(new BiPush((byte)tmp));
                    res.Add(new CPrint());
                }
                else
                {
                    string tmp = (string)Expressions[i];
                    foreach (var j in tmp)
                    {
                        if (j == '\0') break;
                        res.Add(new BiPush((byte)j));
                        res.Add(new CPrint());
                    }
                }
                if (i == l - 1)
                {
                    res.Add(new PrintL());
                }
                else
                {
                    res.Add(new BiPush(32));
                    res.Add(new CPrint());
                }
            }
            return res;
        }
    }
}
