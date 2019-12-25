using System;
using System.Collections.Generic;
using System.Text;
using C0.Instruction;
using C0.Tokenizer;

namespace C0.Analyser
{
    public class OP
    {
        public OP(TokenType type)
        {
            Type = type;
        }

        public TokenType Type { get; set; }

        public IInstruction GetIns()
        {
            switch (Type)
            {
                case TokenType.OperatorAdd:
                    return new IAdd();
                case TokenType.OperatorMinus:
                    return new ISub();
                case TokenType.OperatorMultiply:
                    return new IMul();
                case TokenType.OperatorDivision:
                    return new IDiv();
                case TokenType.OperatorLess:
                    return new Jge(0);
                case TokenType.OperatorLessEqual:
                    return new Jg(0);
                case TokenType.OperatorEqual:
                    return new Jne(0);
                case TokenType.OperatorNotEqual:
                    return new Je(0);
                case TokenType.OperatorGreater:
                    return new Jle(0);
                case TokenType.OperatorGreaterEqual:
                    return new Jl(0);
            }

            return null;
        }
    }
}
