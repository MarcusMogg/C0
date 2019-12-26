using System.Collections.Generic;
using C0.Instruction;
using C0.Tokenizer;
using C0.Utils;

namespace C0.Analyser
{
    public class FunctionCall
    {
        public string Identifier { get; set; }
        public List<Expression.Expression> Expressions { get; set; }

        public FunctionCall()
        {
            Expressions = new List<Expression.Expression>();
        }

        public static FunctionCall Analyse(string par)
        {
            var res = new FunctionCall();
            TokenProvider tokenProvider = TokenProvider.GetInstance();
            SymbolTable.SymbolTable symbolTable = SymbolTable.SymbolTable.GetInstance();
            Token t = tokenProvider.PeekNextToken();
            if (t.Type != TokenType.Identifier)
            {
                throw new MyC0Exception("应该为functionname", t.BeginPos);
            }

            if (!symbolTable.IsFunciton(t.Content))
            {
                throw MyC0Exception.NotExistErr(t.BeginPos);
            }

            res.Identifier = t.Content;
            tokenProvider.Next();
            t = tokenProvider.PeekNextToken();
            tokenProvider.Next();
            if (t.Type != TokenType.BracketsLeftRound)
            {
                throw new MyC0Exception("缺少括号", t.BeginPos);
            }
            while (true)
            {
                t = tokenProvider.PeekNextToken();
                if (t.Type == TokenType.Comma)
                {
                    tokenProvider.Next();
                    continue;
                }
                if (t.Type == TokenType.BracketsRightRound)
                {
                    break;
                }
                res.Expressions.Add(Expression.Expression.Analyse(par));
            }
            if (t.Type != TokenType.BracketsRightRound)
            {
                throw new MyC0Exception("括号不匹配", t.BeginPos);
            }
            tokenProvider.Next();
            var parment = symbolTable.GetParams(res.Identifier);
            if (parment.Count != res.Expressions.Count)
            {
                throw new MyC0Exception("参数数量不匹配", t.BeginPos);
            }
            return res;
        }

        public List<IInstruction> GetIns(string par, int offset)
        {
            List<IInstruction> res = new List<IInstruction>();
            foreach (var e in Expressions)
            {
                res.AddRange(e.GetIns(par, offset + res.Count));
            }
            res.Add(new Call((ushort)SymbolTable.SymbolTable.GetInstance().GetFuncIndex(Identifier)));
            return res;
        }
    }
}
