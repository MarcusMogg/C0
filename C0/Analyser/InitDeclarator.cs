using System;
using System.Collections.Generic;
using System.Text;
using C0.Instruction;
using C0.Tokenizer;
using C0.Utils;

namespace C0.Analyser
{
    public class InitDeclarator
    {
        public string Identifier { get; set; }
        public Expression.Expression Expression { get; set; }

        public static InitDeclarator Analyse(string par, bool cst, TokenType type)
        {
            var res = new InitDeclarator();
            TokenProvider tokenProvider = TokenProvider.GetInstance();
            SymbolTable.SymbolTable symbolTable = SymbolTable.SymbolTable.GetInstance();
            Token t = tokenProvider.PeekNextToken();
            if (t.Type != TokenType.Identifier)
            {
                throw new MyC0Exception("变量名错误", t.BeginPos);
            }

            if (symbolTable.IsDeclaredCurDomain(par, t.Content) || symbolTable.IsFuncCurDomain(t.Content))
            {
                throw MyC0Exception.AlreadyExistErr(t.BeginPos);
            }
            res.Identifier = t.Content;
            tokenProvider.Next();
            t = tokenProvider.PeekNextToken();
            if (t.Type == TokenType.Comma || t.Type == TokenType.Semicolon)
            {
                if (cst)
                {
                    throw MyC0Exception.ConstNotInitializedErr(t.BeginPos);
                }
                symbolTable.AddUninitializedVariable(par, res.Identifier, type);
            }
            else if (t.Type == TokenType.OperatorAssignment)
            {
                tokenProvider.Next();
                if (cst)
                {
                    symbolTable.AddConstVariable(par, res.Identifier, type);
                }
                else
                {
                    symbolTable.AddInitializedVariable(par, res.Identifier, type);
                }
                res.Expression = Analyser.Expression.Expression.Analyse(par);
            }
            return res;
        }

        public List<IInstruction> GetIns(string par, bool cst, int offset)
        {
            List<IInstruction> res = new List<IInstruction>();
            SymbolTable.SymbolTable s = SymbolTable.SymbolTable.GetInstance();
            if (Expression != null)
            {
                res.AddRange(Expression.GetIns(par, offset));
                if (s.GetIdType(par, Identifier) == TokenType.Char)
                {
                    res.Add(new I2C());
                }
                if (cst)
                {
                    s.UpdateConstOffset(Identifier, par);
                }
                else
                {
                    s.UpdateInitializedOffset(Identifier, par);
                }
            }
            else
            {
                res.Add(new IPush(0));
                s.UpdateUninitializedOffset(Identifier, par);
            }
            return res;
        }
    }
}
