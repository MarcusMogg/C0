using System;
using System.Collections.Generic;
using System.Text;
using C0.Instruction;
using C0.Tokenizer;
using C0.Utils;

namespace C0.Analyser.Expression
{
    public class UnaryExpression
    {
        public OP Op { get; set; }
        public PrimaryExpression PrimaryExpression { get; set; }
        public List<TypeSpecifier> TypeSpecifiers;
        public TokenType Type { get; set; }

        public UnaryExpression()
        {
            TypeSpecifiers = new List<TypeSpecifier>();
        }

        public static UnaryExpression Analyse(string par)
        {
            var res = new UnaryExpression();
            TokenProvider tokenProvider = TokenProvider.GetInstance();
            Token t = tokenProvider.PeekNextToken();

            while (t.Type == TokenType.BracketsLeftRound)
            {
                tokenProvider.Next();
                t = tokenProvider.PeekNextToken();
                if (!Analyser.TypeSpecifier.IsTypeSpecifier(t.Type) )
                {
                    tokenProvider.Unread();
                    t = tokenProvider.PeekNextToken();
                    break;
                    
                }

                if (t.Type == TokenType.Void)
                {
                    throw MyC0Exception.InvalidTokenErr(t.BeginPos);
                }
                res.TypeSpecifiers.Add(new TypeSpecifier(t.Type));
                tokenProvider.Next();
                t = tokenProvider.PeekNextToken();

                if (t.Type != TokenType.BracketsRightRound)
                {
                    throw MyC0Exception.InvalidTokenErr(t.BeginPos);
                }
                tokenProvider.Next();
                t = tokenProvider.PeekNextToken();
            }


            if (t.Type == TokenType.OperatorAdd || t.Type == TokenType.OperatorMinus)
            {
                res.Op = new OP(t.Type);
                tokenProvider.Next();
                t = tokenProvider.PeekNextToken();
            }
            res.PrimaryExpression = PrimaryExpression.Analyse(par);

            res.Type = res.PrimaryExpression.Type;
            if (res.Op != null)
            {
                res.Type = TokenType.Int;
            }

            if (res.TypeSpecifiers.Count > 0)
            {
                res.Type = res.TypeSpecifiers[0].TokenType;
                res.TypeSpecifiers.Reverse();
            }
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

            foreach (var i in TypeSpecifiers)
            {
                if (i.TokenType == TokenType.Char)
                {
                    res.Add(new I2C());
                    break;
                }
            }
            return res;
        }
    }
}
