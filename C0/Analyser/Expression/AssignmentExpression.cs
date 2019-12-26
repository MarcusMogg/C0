using System;
using System.Collections.Generic;
using C0.Instruction;
using C0.Tokenizer;
using C0.Utils;

namespace C0.Analyser.Expression
{
    public class AssignmentExpression
    {
        public string Identifier { get; set; }
        public Expression Expression { get; set; }

        public static AssignmentExpression Analyse(string par)
        {
            var res = new AssignmentExpression();
            TokenProvider tokenProvider = TokenProvider.GetInstance();
            Token t = tokenProvider.PeekNextToken();
            SymbolTable.SymbolTable symbolTable = SymbolTable.SymbolTable.GetInstance();
            if (!symbolTable.IsDeclaredAllDomain(par, t.Content))
            {
                throw MyC0Exception.InvalidTokenErr(t.BeginPos);
            }
            res.Identifier = t.Content;

            if (symbolTable.IsConstVariable(par, res.Identifier))
            {
                throw MyC0Exception.CantConstErr(t.BeginPos);
            }


            tokenProvider.Next();
            if (symbolTable.IsUninitializedVariable(par, res.Identifier))
            {
                symbolTable.InitializeVar(res.Identifier, par);
            }
            t = tokenProvider.PeekNextToken();
            if (t.Type != TokenType.OperatorAssignment)
            {
                throw MyC0Exception.InvalidTokenErr(t.BeginPos);
            }
            tokenProvider.Next();
            res.Expression = Expression.Analyse(par);
            return res;
        }
        public List<IInstruction> GetIns(string par, int offset)
        {
            var syt = SymbolTable.SymbolTable.GetInstance();
            List<IInstruction> res = new List<IInstruction>();
            Tuple<int, int> pos = SymbolTable.SymbolTable.GetInstance().GetLevelOffset(Identifier, par);
            res.Add(new LoadA((ushort)(SymbolTable.SymbolTable.GetInstance().GetFuncLevel(par) - pos.Item1), pos.Item2));
            res.AddRange(Expression.GetIns(par, offset + res.Count));
            if (syt.GetIdType(par, Identifier) == TokenType.Char)
            {
                res.Add(new I2C());
            }
            res.Add(new Istore());
            return res;
        }
    }
}
