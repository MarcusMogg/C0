using System;
using System.Collections.Generic;
using C0.Instruction;
using C0.Tokenizer;

namespace C0.Analyser.Expression
{
    public class Condition
    {
        public OP Op { get; set; }
        public Analyser.Expression.Expression LExpression { get; set; }
        public Analyser.Expression.Expression RExpression { get; set; }

        public static Condition Analyse(string par)
        {
            var res = new Condition();
            TokenProvider tokenProvider = TokenProvider.GetInstance();
            res.LExpression = Analyser.Expression.Expression.Analyse(par);

            Token t = tokenProvider.PeekNextToken();
            if (t.Type == TokenType.OperatorEqual || t.Type == TokenType.OperatorLess ||
                t.Type == TokenType.OperatorLessEqual
                || t.Type == TokenType.OperatorNotEqual || t.Type == TokenType.OperatorGreater ||
                t.Type == TokenType.OperatorGreaterEqual)
            {
                res.Op = new OP(t.Type);
                tokenProvider.Next(); ;
                res.RExpression = Analyser.Expression.Expression.Analyse(par);
            }
            return res;
        }
        public List<IInstruction> GetIns(string par, int offset)
        {
            List<IInstruction> res = new List<IInstruction>();
            res.AddRange(LExpression.GetIns(par,offset));
            if (RExpression != null)
            {
                res.AddRange(RExpression.GetIns(par,offset + res.Count));
                res.Add(new ICmp());
                res.Add(Op.GetIns());
            }
            else
            {
                res.Add(new Je(0));
            }
            
            return res;
        }
    }
}
