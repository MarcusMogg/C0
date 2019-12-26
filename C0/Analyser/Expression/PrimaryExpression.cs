using System;
using System.Collections.Generic;
using System.Text;
using C0.Instruction;
using C0.Tokenizer;
using C0.Utils;

namespace C0.Analyser.Expression
{
    public class PrimaryExpression
    {
        public dynamic Content { get; set; }
        public TokenType Type { get; set; }

        public static PrimaryExpression Analyse(string par)
        {
            var res = new PrimaryExpression();
            TokenProvider tokenProvider = TokenProvider.GetInstance();
            SymbolTable.SymbolTable symbolTable = SymbolTable.SymbolTable.GetInstance();
            Token t = tokenProvider.PeekNextToken();
            if (t.Type == TokenType.BracketsLeftRound)
            {
                tokenProvider.Next();
                res.Content = Expression.Analyse(par);
                res.Type = res.Content.Type;
                t = tokenProvider.PeekNextToken();
                if (t.Type != TokenType.BracketsRightRound)
                {
                    throw MyC0Exception.MissBrackets(t.BeginPos);
                }
                tokenProvider.Next();
            }
            else if (t.Type == TokenType.LiteralDecimal || t.Type == TokenType.LiteralHexadecimal)
            {
                res.Content = t.Content;
                res.Type = TokenType.Int;
                tokenProvider.Next();
            }
            else if (t.Type == TokenType.Identifier)
            {
                if (symbolTable.IsDeclaredAllDomain(par, t.Content))
                {
                    if (symbolTable.IsUninitializedVariable(par, t.Content))
                    {
                        throw MyC0Exception.NotInitializedErr(t.BeginPos);
                    }
                    res.Content = t.Content;
                    res.Type = symbolTable.GetIdType(par, t.Content);
                    tokenProvider.Next();
                }
                else if (symbolTable.IsFunciton(t.Content))
                {
                    if (symbolTable.GeFuncType(t.Content) == TokenType.Void)
                    {
                        throw MyC0Exception.CantVoidErr(t.BeginPos);
                    }
                    var tmp = FunctionCall.Analyse(par);
                    if (symbolTable.GeFuncType(tmp.Identifier) == TokenType.Void)
                    {
                        throw MyC0Exception.CantVoidErr(t.BeginPos);
                    }

                    res.Content = tmp;
                    res.Type = symbolTable.GeFuncType(tmp.Identifier);
                }
                else
                {
                    throw MyC0Exception.NotExistErr(t.BeginPos);
                }
            }
            else if(t.Type == TokenType.Char)
            {
                res.Content = t.Content;
                res.Type = TokenType.Char;
                tokenProvider.Next();
            }
            else
            {
                throw MyC0Exception.InvalidTokenErr(t.BeginPos);
            }

            return res;
        }
        public List<IInstruction> GetIns(string par, int offset)
        {
            List<IInstruction> res = new List<IInstruction>();
            if (Content is int)
            {
                res.Add(new IPush(Content));
            }
            else if (Content is string)
            {
                Tuple<int,int> pos = SymbolTable.SymbolTable.GetInstance().GetLevelOffset(Content, par);
                res.Add(new LoadA((ushort)(SymbolTable.SymbolTable.GetInstance().GetFuncLevel(par) - pos.Item1), pos.Item2));
                res.Add(new ILoad());
            }
            else if (Content is char)
            {
                res.Add(new IPush(Content));
            }
            else
            {
                res.AddRange(Content.GetIns(par,offset));
            }
            return res;
        }
    }
}
