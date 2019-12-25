using System.Collections.Generic;
using C0.Instruction;
using C0.Tokenizer;

namespace C0.Analyser.Statement
{
    public class StatementSeq
    {
        public List<Statement> Statements;

        public StatementSeq()
        {
            Statements = new List<Statement>();
        }

        public static StatementSeq Analyse(string par)
        {
            var res = new StatementSeq();
            TokenProvider tokenProvider = TokenProvider.GetInstance();

            while (true)
            {
                Token t = tokenProvider.PeekNextToken();
                if (t.Type == TokenType.BracketsRightCurly)
                {
                    break;
                }
                res.Statements.Add(Statement.Analyse(par));
            }
            return res;
        }
        public List<IInstruction> GetIns(string par, int offset)
        {
            List<IInstruction> res = new List<IInstruction>();
            foreach (var i in Statements)
            {
                res.AddRange(i.GetIns(par, offset + res.Count));
            }
            return res;
        }
    }
}
