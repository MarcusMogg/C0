using System;
using System.Collections.Generic;
using System.Text;
using C0.Analyser.Statement;
using C0.Instruction;
using C0.Tokenizer;
using C0.Utils;

namespace C0.Analyser
{
    public class FunctionDefinition
    {
        public FunctionDefinition()
        {
            ParameterDeclarations = new List<ParameterDeclaration>();
            VariableDeclarations = new List<VariableDeclaration>();
        }

        public TypeSpecifier TypeSpecifier { get; set; }
        public string Identifier { get; set; }
        public List<ParameterDeclaration> ParameterDeclarations { get; set; }
        public List<VariableDeclaration> VariableDeclarations { get; set; }
        public StatementSeq StatementSeqs { get; set; }

        public static FunctionDefinition Analyse(string par)
        {
            var res = new FunctionDefinition();
            TokenProvider tokenProvider = TokenProvider.GetInstance();
            SymbolTable.SymbolTable symbolTable = SymbolTable.SymbolTable.GetInstance();
            Token t = tokenProvider.PeekNextToken();
            tokenProvider.Next();
            if (!Analyser.TypeSpecifier.IsTypeSpecifier(t.Type))
            {
                throw new MyC0Exception("类型声明错误", t.BeginPos);
                
            }
            res.TypeSpecifier = new TypeSpecifier(t.Type);

            t = tokenProvider.PeekNextToken();
            if (t.Type != TokenType.Identifier)
            {
                throw new MyC0Exception("函数声明错误", t.BeginPos);
            }

            res.Identifier = t.Content;
            tokenProvider.Next();

            if (symbolTable.IsDeclaredAllDomain(par, res.Identifier) || symbolTable.IsFunciton(res.Identifier))
            {
                throw MyC0Exception.AlreadyExistErr(t.BeginPos);
            }
            symbolTable.AddFuncName(res.TypeSpecifier.TokenType, res.Identifier);
            symbolTable.AddParent(par, res.Identifier);


            t = tokenProvider.PeekNextToken();
            tokenProvider.Next();
            if (t.Type != TokenType.BracketsLeftRound)
            {
                throw new MyC0Exception("应该为(", t.BeginPos);
            }
            while (true)
            {
                t = tokenProvider.PeekNextToken();
                if (t.Type == TokenType.Comma)
                {
                    tokenProvider.Next();
                    continue;
                }
                if (t.Type == TokenType.BracketsRightRound)
                {
                    break;
                }
                res.ParameterDeclarations.Add(ParameterDeclaration.Analyse(res.Identifier));
            }
            if (t.Type != TokenType.BracketsRightRound)
            {
                throw new MyC0Exception("应该为)", t.BeginPos);
            }
            tokenProvider.Next();
            foreach (var i in res.ParameterDeclarations)
            {
                symbolTable.AddFunctionParm(res.Identifier, i.Identifier, i.TypeSpecifier.TokenType);
            }
            t = tokenProvider.PeekNextToken();
            tokenProvider.Next();
            if (t.Type != TokenType.BracketsLeftCurly)
            {
                throw MyC0Exception.InvalidTokenErr(t.BeginPos);
            }

            while (true)
            {
                t = tokenProvider.PeekNextToken();
                if (t.Type == TokenType.Const || Analyser.TypeSpecifier.IsTypeSpecifier(t.Type))
                {
                    if (t.Type == TokenType.Void)
                    {
                        throw MyC0Exception.VoidErr(t.BeginPos);
                    }

                    res.VariableDeclarations.Add(VariableDeclaration.Analyse(res.Identifier));
                }
                else
                {
                    break;
                }
            }
            t = tokenProvider.PeekNextToken();
            if (t.Type != TokenType.BracketsRightCurly)
            {
                res.StatementSeqs = StatementSeq.Analyse(res.Identifier);
                t = tokenProvider.PeekNextToken();
            }
            if (t.Type != TokenType.BracketsRightCurly)
            {
                throw new MyC0Exception("应该为}", t.BeginPos);
            }
            tokenProvider.Next();
            return res;
        }

        public List<IInstruction> GetIns(string par, int offset)
        {
            var res = new List<IInstruction>();
            SymbolTable.SymbolTable syt = SymbolTable.SymbolTable.GetInstance();

            foreach (var i in ParameterDeclarations)
            {
                syt.UpdateInitializedOffset(i.Identifier, Identifier);
            }
            foreach (var i in VariableDeclarations)
            {
                res.AddRange(i.GetIns(Identifier, offset + res.Count));
            }
            if (StatementSeqs != null)
                res.AddRange(StatementSeqs.GetIns(Identifier, offset + res.Count));
            res.Add(new BiPush(0));
            res.Add(new IRet());
            return res;
        }
    }
}
