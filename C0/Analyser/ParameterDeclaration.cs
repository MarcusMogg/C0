using System;
using System.Collections.Generic;
using System.Text;
using C0.Tokenizer;
using C0.Utils;

namespace C0.Analyser
{
    public class ParameterDeclaration
    {
        public bool ConstQualifier { get; set; }
        public TypeSpecifier TypeSpecifier { get; set; }
        public string Identifier { get; set; }

        public static ParameterDeclaration Analyse(string par)
        {
            var res = new ParameterDeclaration();
            TokenProvider tokenProvider = TokenProvider.GetInstance();
            Token t = tokenProvider.PeekNextToken();
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
            t = tokenProvider.PeekNextToken();
            tokenProvider.Next();
            if (t.Type != TokenType.Identifier)
            {
                throw MyC0Exception.InvalidTokenErr(t.BeginPos);
            }

            res.Identifier = t.Content;
            SymbolTable.SymbolTable syt = SymbolTable.SymbolTable.GetInstance();
            syt.AddInitializedVariable(par, res.Identifier, res.TypeSpecifier.TokenType);
            return res;
        }
    }
}
