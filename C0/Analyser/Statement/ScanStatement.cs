using System;
using System.Collections.Generic;
using System.Text;
using C0.Instruction;
using C0.Tokenizer;
using C0.Utils;

namespace C0.Analyser.Statement
{
    public class ScanStatement
    {
        public String Identifier { get; set; }

        public static ScanStatement Analyse(string par)
        {
            var res = new ScanStatement();
            TokenProvider tokenProvider = TokenProvider.GetInstance();
            Token t = tokenProvider.PeekNextToken();
            var syt = SymbolTable.SymbolTable.GetInstance();
            if (t.Type != TokenType.Scan)
            {
                throw new MyC0Exception("应该为scan", t.BeginPos);
            }
            tokenProvider.Next();
            t = tokenProvider.PeekNextToken();
            if (t.Type != TokenType.BracketsLeftRound)
            {
                throw new MyC0Exception("缺少括号", t.BeginPos);
            }
            tokenProvider.Next();
            t = tokenProvider.PeekNextToken();
            if (t.Type != TokenType.Identifier)
            {
                throw new MyC0Exception("缺少变量名", t.BeginPos);
            }
            res.Identifier = t.Content;
            if (syt.IsConstVariable(par, res.Identifier))
            {
                throw MyC0Exception.CantConstErr(t.BeginPos);
            }

            if (syt.IsUninitializedVariable(par,res.Identifier))
            {
                syt.InitializeVar(res.Identifier, par);
            }

            tokenProvider.Next();
            t = tokenProvider.PeekNextToken();
            if (t.Type != TokenType.BracketsRightRound)
            {
                throw new MyC0Exception("括号不匹配", t.BeginPos);
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
            var syt = SymbolTable.SymbolTable.GetInstance();
            Tuple<int, int> pos = syt.GetLevelOffset(Identifier, par);
            res.Add(new LoadA((ushort)(SymbolTable.SymbolTable.GetInstance().GetFuncLevel(par) - pos.Item1), pos.Item2));

            if (syt.GetIdType(par, Identifier) == TokenType.Char)
            {
                res.Add(new CScan());
            }
            else
            {
                res.Add(new IScan());
            }
            res.Add(new Istore());

            return res;
        }
    }
}
