using System;
using System.Collections.Generic;
using System.Text;
using C0.Instruction;
using C0.Tokenizer;

namespace C0.Analyser.Expression
{
    public class AdditiveExpression
    {
        public List<MultiplicativeExpression> MultiplicativeExpression { get; set; }
        public List<OP> Ops { get; set; }
        public TokenType Type { get; set; }

        public AdditiveExpression()
        {
            MultiplicativeExpression = new List<MultiplicativeExpression>();
            Ops = new List<OP>();
        }

        public static AdditiveExpression Analyse(string par)
        {
            var res = new AdditiveExpression();
            res.MultiplicativeExpression.Add(Analyser.Expression.MultiplicativeExpression.Analyse(par));
            res.Type = res.MultiplicativeExpression[0].Type;
            TokenProvider tokenProvider = TokenProvider.GetInstance();
            while (true)
            {
                Token t = tokenProvider.PeekNextToken();
                if (t.Type == TokenType.OperatorAdd || t.Type == TokenType.OperatorMinus)
                {
                    res.Ops.Add(new OP(t.Type));
                    tokenProvider.Next();
                    res.MultiplicativeExpression.Add(Analyser.Expression.MultiplicativeExpression.Analyse(par));
                    res.Type = TokenType.Int;
                }
                else
                {
                    break;
                }
            }
            return res;
        }

        public List<IInstruction> GetIns(string par,int offset)
        {
            List<IInstruction> res = new List<IInstruction>();
            res.AddRange(MultiplicativeExpression[0].GetIns(par,offset));
            int cnt = Ops.Count;
            for (int i = 0; i < cnt; i++)
            {
                res.AddRange(MultiplicativeExpression[i + 1].GetIns(par, offset + res.Count));
                res.Add(Ops[i].GetIns());
            }
            return res;
        }
    }
}
