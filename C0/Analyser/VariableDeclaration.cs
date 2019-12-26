using System;
using System.Collections.Generic;
using System.Text;
using C0.Instruction;
using C0.Tokenizer;
using C0.Utils;

namespace C0.Analyser
{
    public class VariableDeclaration
    {
        public VariableDeclaration()
        {
            ConstQualifier = false;
            InitDeclarators = new List<InitDeclarator>();
        }

        public bool ConstQualifier { get; set; }
        public TypeSpecifier TypeSpecifier { get; set; }
        public List<InitDeclarator> InitDeclarators { get; set; }


        public static VariableDeclaration Analyse(string par)
        {
            var res = new VariableDeclaration();
            TokenProvider tokenProvider = TokenProvider.GetInstance();
            Token t = tokenProvider.PeekNextToken();
            if (t.Type != TokenType.Const && !Analyser.TypeSpecifier.IsTypeSpecifier(t.Type))
            {
                throw MyC0Exception.InvalidTokenErr(t.BeginPos);
            }
            if (t.Type == TokenType.Const)
            {
                res.ConstQualifier = true;
                tokenProvider.Next();
                t = tokenProvider.PeekNextToken();
            }
            if (!Analyser.TypeSpecifier.IsTypeSpecifier(t.Type))
            {
                throw MyC0Exception.InvalidTokenErr(t.BeginPos);
            }
            if (t.Type == TokenType.Void)
            {
                throw MyC0Exception.VoidErr(t.BeginPos);
            }
            res.TypeSpecifier = new TypeSpecifier(t.Type);
            tokenProvider.Next();
            while (true)
            {
                t = tokenProvider.PeekNextToken();
                if (t.Type == TokenType.Semicolon)
                {
                    tokenProvider.Next();
                    break;
                }

                if (t.Type == TokenType.Comma)
                {
                    tokenProvider.Next();
                    continue;
                }

                if (t.Type != TokenType.Identifier)
                {
                    throw MyC0Exception.MissSemicolonErr(t.BeginPos);
                }
                res.InitDeclarators.Add(InitDeclarator.Analyse(par, res.ConstQualifier, res.TypeSpecifier.TokenType));
            }

            if (res.InitDeclarators.Count == 0)
            {
                throw MyC0Exception.InvalidTokenErr(t.BeginPos);
            }
            return res;
        }

        public List<IInstruction> GetIns(string par, int offset)
        {
            List<IInstruction> res = new List<IInstruction>();
            foreach (var i in InitDeclarators)
            {
                res.AddRange(i.GetIns(par, ConstQualifier, offset + res.Count));
            }
            return res;
        }
    }
}
