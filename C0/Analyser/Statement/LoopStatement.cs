using System;
using System.Collections.Generic;
using System.Text;
using C0.Analyser.Expression;
using C0.Instruction;
using C0.Tokenizer;
using C0.Utils;

namespace C0.Analyser.Statement
{
    public class LoopStatement
    {
        public Condition Condition { get; set; }
        public Statement Statement { get; set; }

        public static LoopStatement Analyse(string par)
        {
            var res = new LoopStatement();
            TokenProvider tokenProvider = TokenProvider.GetInstance();
            Token t = tokenProvider.PeekNextToken();
            if (t.Type != TokenType.While)
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
            res.Condition = Condition.Analyse(par);
            t = tokenProvider.PeekNextToken();
            if (t.Type != TokenType.BracketsRightRound)
            {
                throw MyC0Exception.InvalidTokenErr(t.BeginPos);
            }
            tokenProvider.Next();
            res.Statement = Statement.Analyse(par);

            return res;
        }
        public List<IInstruction> GetIns(string par, int offset)
        {
            List<IInstruction> res = new List<IInstruction>();
            List<IInstruction> c = Condition.GetIns(par, offset);
            int cnt = c.Count;
            List<IInstruction> ifs = Statement.GetIns(par, offset + cnt);
            int cnt1 = ifs.Count + 1;
            if (c[cnt - 1] is Jl)
            {
                ((Jl)c[cnt - 1]).Param1 = (ushort)(offset + cnt + cnt1);
            }
            else if (c[cnt - 1] is Je)
            {
                ((Je)c[cnt - 1]).Param1 = (ushort)(offset + cnt + cnt1);
            }
            else if (c[cnt - 1] is Jle)
            {
                ((Jle)c[cnt - 1]).Param1 = (ushort)(offset + cnt + cnt1);
            }
            else if (c[cnt - 1] is Jne)
            {
                ((Jne)c[cnt - 1]).Param1 = (ushort)(offset + cnt + cnt1);
            }
            else if (c[cnt - 1] is Jg)
            {
                ((Jg)c[cnt - 1]).Param1 = (ushort)(offset + cnt + cnt1);
            }
            else if (c[cnt - 1] is Jge)
            {
                ((Jge)c[cnt - 1]).Param1 = (ushort)(offset + cnt + cnt1);
            }
            res.AddRange(c);
            res.AddRange(ifs);
            res.Add(new Jmp((ushort)offset));
            return res;
        }
    }
}
