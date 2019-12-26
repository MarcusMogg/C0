using System;
using System.Collections.Generic;
using System.Text;
using C0.Instruction;
using C0.Tokenizer;
using C0.Utils;

namespace C0.Analyser.Statement
{
    public class JumpStatement
    {
        public Expression.Expression Expression { get; set; }

        public static JumpStatement Analyse(string par)
        {
            var res = new JumpStatement();
            TokenProvider tokenProvider = TokenProvider.GetInstance();
            SymbolTable.SymbolTable symbolTable = SymbolTable.SymbolTable.GetInstance();
            Token t = tokenProvider.PeekNextToken();
            tokenProvider.Next();
            if (t.Type != TokenType.Return)
            {
                throw MyC0Exception.InvalidTokenErr(t.BeginPos);
            }

            t = tokenProvider.PeekNextToken();
            if (t.Type != TokenType.Semicolon)
            {
                res.Expression = Analyser.Expression.Expression.Analyse(par);
            }

            t = tokenProvider.PeekNextToken();
            tokenProvider.Next();
            if (t.Type != TokenType.Semicolon)
            {
                throw MyC0Exception.MissSemicolonErr(t.BeginPos);
            }

            var tp = symbolTable.GeFuncType(par);
            if (res.Expression == null ^ tp == TokenType.Void)
            {
                throw MyC0Exception.InvalidTokenErr(t.BeginPos);
            }

            return res;
        }

        public List<IInstruction> GetIns(string par, int offset)
        {
            var res = new List<IInstruction>();
            var syt = SymbolTable.SymbolTable.GetInstance();
            if (Expression != null)
            {
                res.AddRange(Expression.GetIns(par, offset));
                if (syt.GeFuncType(par) == TokenType.Char)
                {
                    res.Add(new I2C());
                }
                res.Add(new IRet());
            }
            else
            {
                res.Add(new Ret());
            }

            return res;
        }
    }
}