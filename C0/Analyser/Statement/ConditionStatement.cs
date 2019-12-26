using System;
using System.Collections.Generic;
using System.Text;
using C0.Analyser.Expression;
using C0.Instruction;
using C0.Tokenizer;
using C0.Utils;

namespace C0.Analyser.Statement
{
    public class ConditionStatement
    {
        public Condition Condition { get; set; }
        public Statement IfStatement { get; set; }
        public Statement ElseStatement { get; set; }

        public static ConditionStatement Analyse(string par)
        {
            var res = new ConditionStatement();
            TokenProvider tokenProvider = TokenProvider.GetInstance();
            Token t = tokenProvider.PeekNextToken();
            if (t.Type != TokenType.If)
            {
                throw new MyC0Exception("应该为if", t.BeginPos);
            }

            tokenProvider.Next();
            t = tokenProvider.PeekNextToken();
            if (t.Type != TokenType.BracketsLeftRound)
            {
                throw new MyC0Exception("缺少括号", t.BeginPos);
            }

            tokenProvider.Next();
            res.Condition = Condition.Analyse(par);
            t = tokenProvider.PeekNextToken();
            if (t.Type != TokenType.BracketsRightRound)
            {
                throw new MyC0Exception("括号不匹配", t.BeginPos);
            }

            tokenProvider.Next();
            res.IfStatement = Statement.Analyse(par);

            t = tokenProvider.PeekNextToken();
            if (t.Type != TokenType.Else)
            {
                return res;
            }

            tokenProvider.Next();
            res.ElseStatement = Statement.Analyse(par);
            return res;
        }

        public List<IInstruction> GetIns(string par, int offset)
        {
            List<IInstruction> res = new List<IInstruction>();
            List<IInstruction> c = Condition.GetIns(par, offset);
            int cnt = c.Count;
            List<IInstruction> ifs = IfStatement.GetIns(par, offset + cnt);
            int cnt1 = ifs.Count;
            List<IInstruction> elses = new List<IInstruction>();
            if (ElseStatement != null)
            {
                elses = ElseStatement.GetIns(par, offset + cnt + cnt1 + 1);
            }

            if (elses.Count != 0)
            {
                cnt1 += 1;
            }
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
            if (elses.Count != 0)
            {
                res.Add(new Jmp((ushort)(offset + cnt + cnt1 + elses.Count)));
                res.AddRange(elses);
            }

            return res;
        }

    }
}