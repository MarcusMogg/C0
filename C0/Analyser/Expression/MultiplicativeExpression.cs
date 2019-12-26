using System;
using System.Collections.Generic;
using System.Text;
using C0.Instruction;
using C0.Tokenizer;

namespace C0.Analyser.Expression
{
    public class MultiplicativeExpression
    {
        public List<UnaryExpression> UnaryExpressions { get; set; }
        public List<OP> Ops { get; set; }
        public TokenType Type { get; set; }

        public MultiplicativeExpression()
        {
            UnaryExpressions = new List<UnaryExpression>();
            Ops = new List<OP>();
        }

        public static MultiplicativeExpression Analyse(string par)
        {
            var res = new MultiplicativeExpression();
            res.UnaryExpressions.Add(UnaryExpression.Analyse(par));
            res.Type = res.UnaryExpressions[0].Type;

            TokenProvider tokenProvider = TokenProvider.GetInstance();
            while (true)
            {
                Token t = tokenProvider.PeekNextToken();
                if (t.Type == TokenType.OperatorMultiply || t.Type == TokenType.OperatorDivision)
                {
                    res.Ops.Add(new OP(t.Type));
                    tokenProvider.Next();
                    res.UnaryExpressions.Add(UnaryExpression.Analyse(par));
                    res.Type = TokenType.Int;
                }
                else
                {
                    break;
                }
            }
            return res;
        }
        public List<IInstruction> GetIns(string par, int offset)
        {
            List<IInstruction> res = new List<IInstruction>();
            res.AddRange(UnaryExpressions[0].GetIns(par,offset));
            int cnt = Ops.Count;
            for (int i = 0; i < cnt; i++)
            {
                res.AddRange(UnaryExpressions[i + 1].GetIns(par,offset + res.Count));
                res.Add(Ops[i].GetIns());
            }
            return res;
        }
    }
}
