using System.Collections.Generic;
using C0.Instruction;
using C0.Tokenizer;

namespace C0.Analyser.Expression
{
    public class Expression
    {
        public TokenType Type { get; set; }
        public AdditiveExpression AdditiveExpression { get; set; }
        
        public static Expression Analyse(string par)
        {
            var res = new Expression();
            res.AdditiveExpression = AdditiveExpression.Analyse(par);
            return res;
        }
        public List<IInstruction> GetIns(string par, int offset)
        {
            return AdditiveExpression.GetIns(par,offset);
        }
    }
}
