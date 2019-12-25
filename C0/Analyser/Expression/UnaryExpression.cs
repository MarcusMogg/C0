using System;
using System.Collections.Generic;
using System.Text;
using C0.Instruction;
using C0.Tokenizer;

namespace C0.Analyser.Expression
{
    public class UnaryExpression
    {
        public OP Op { get; set; }
        public PrimaryExpression PrimaryExpression { get; set; }

        public static UnaryExpression Analyse(string par)
        {
            var res = new UnaryExpression();
            TokenProvider tokenProvider = TokenProvider.GetInstance();
            Token t = tokenProvider.PeekNextToken();
            if (t.Type == TokenType.OperatorAdd || t.Type == TokenType.OperatorMinus)
            {
                res.Op = new OP(t.Type);
                tokenProvider.Next();
                t = tokenProvider.PeekNextToken();
            }
            res.PrimaryExpression = PrimaryExpression.Analyse(par);
            return res;
        }
        public List<IInstruction> GetIns(string par, int offset)
        {
            List<IInstruction> res = new List<IInstruction>();
            res.AddRange(PrimaryExpression.GetIns(par, offset));
            if (Op != null && Op.Type == TokenType.OperatorMinus)
            {
                res.Add(new INeg());
            }
            return res;
        }
    }
}
