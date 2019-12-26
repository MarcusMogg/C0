using System;
using System.Collections.Generic;
using System.Text;
using C0.Tokenizer;
using C0.Utils;
using C0.Instruction;


namespace C0.Analyser
{
    public class C0Program
    {
        public C0Program()
        {
            VariableDeclarations = new List<VariableDeclaration>();
            FubFunctionDefinitions = new List<FunctionDefinition>();
        }
        public const string Root = "/";
        public List<VariableDeclaration> VariableDeclarations { get; set; }
        public List<FunctionDefinition> FubFunctionDefinitions { get; set; }

        public static C0Program Analyse()
        {
            C0Program p = new C0Program();
            TokenProvider tokenProvider = TokenProvider.GetInstance();
            SymbolTable.SymbolTable syt = SymbolTable.SymbolTable.GetInstance();
            syt.AddFuncName(TokenType.Void,Root);
            while (true)
            {
                Token t = tokenProvider.PeekNextToken();
                if (t.Type == TokenType.Eof)
                    break;
                if (t.Type == TokenType.Const)
                {
                    p.VariableDeclarations.Add(VariableDeclaration.Analyse(Root));
                    continue;
                }

                if (!TypeSpecifier.IsTypeSpecifier(t.Type))
                {
                    throw new MyC0Exception("声明类型错误", t.BeginPos);
                }

                if (t.Type == TokenType.Void)
                {
                    break;
                }
                tokenProvider.Next();
                t = tokenProvider.PeekNextToken();

                if (t.Type != TokenType.Identifier)
                {
                    throw new MyC0Exception("变量声明错误", t.BeginPos);
                }

                tokenProvider.Next();
                t = tokenProvider.PeekNextToken();
                tokenProvider.Unread();
                tokenProvider.Unread();
                if (t.Type == TokenType.BracketsLeftRound)
                {
                    break;
                }
                p.VariableDeclarations.Add(VariableDeclaration.Analyse(Root));
            }
            while (true)
            {
                Token t = tokenProvider.PeekNextToken();
                if (t.Type == TokenType.Eof)
                    break;
                p.FubFunctionDefinitions.Add(FunctionDefinition.Analyse(Root));
            }
            SymbolTable.SymbolTable s = SymbolTable.SymbolTable.GetInstance();
            if (!s.IsFunciton("main"))
            {
                throw MyC0Exception.MissMainErr();
            }
            return p;
        }

        public List<IInstruction> GetIns()
        {
            List<IInstruction> res = new List<IInstruction>();
            res.Add(new Magic());
            res.Add(new C0.Instruction.Version());
            res.Add(new DotConstant((ushort)FubFunctionDefinitions.Count));
            foreach (var i in FubFunctionDefinitions)
            {
                res.Add(new ConstantInfoString(i.Identifier));
            }
            List<IInstruction> start = new List<IInstruction>();
            foreach (var i in VariableDeclarations)
            {
                start.AddRange(i.GetIns(Root, start.Count));
            }
            res.Add(new DotStart((ushort)start.Count));
            res.AddRange(start);
            res.Add(new DotFunction((ushort)FubFunctionDefinitions.Count));
            foreach (var i in FubFunctionDefinitions)
            {
                var tmp = i.GetIns(Root, 0);
                res.Add(new FunctionInfo((ushort)SymbolTable.SymbolTable.GetInstance().GetFuncIndex(i.Identifier),
                    (ushort)i.ParameterDeclarations.Count, 1, (ushort)tmp.Count));
                res.AddRange(tmp);
            }
            return res;
        }
    }
}
