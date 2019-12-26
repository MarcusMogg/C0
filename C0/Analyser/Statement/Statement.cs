using System;
using System.Collections.Generic;
using System.Text;
using C0.Analyser.Expression;
using C0.Instruction;
using C0.Tokenizer;
using C0.Utils;

namespace C0.Analyser.Statement
{
    public class Statement
    {
        public dynamic Seq { get; set; }

        public static Statement Analyse(string par)
        {
            var res = new Statement();
            TokenProvider tokenProvider = TokenProvider.GetInstance();
            SymbolTable.SymbolTable symbolTable = SymbolTable.SymbolTable.GetInstance();
            Token t = tokenProvider.PeekNextToken();
            if (t.Type == TokenType.Semicolon)
            {
                res.Seq = new EmptyStatement();
                tokenProvider.Next();
            }
            else if (t.Type == TokenType.BracketsLeftCurly)
            {
                tokenProvider.Next();
                res.Seq = StatementSeq.Analyse(par);
                t = tokenProvider.PeekNextToken();
                tokenProvider.Next();
                if (t.Type != TokenType.BracketsRightCurly)
                {
                    throw new MyC0Exception("括号不匹配", t.BeginPos);
                }
            }
            else if (t.Type == TokenType.If)
            {
                res.Seq = ConditionStatement.Analyse(par);
            }
            else if (t.Type == TokenType.While)
            {
                res.Seq = LoopStatement.Analyse(par);
            }
            else if (t.Type == TokenType.Return)
            {
                res.Seq = JumpStatement.Analyse(par);
            }
            else if (t.Type == TokenType.Print)
            {
                res.Seq = PrintStatement.Analyse(par);
            }
            else if (t.Type == TokenType.Scan)
            {
                res.Seq = ScanStatement.Analyse(par);
            }
            else if (t.Type == TokenType.Identifier)
            {
                if (symbolTable.IsDeclaredAllDomain(par, t.Content))
                {
                    res.Seq = AssignmentExpression.Analyse(par);
                    t = tokenProvider.PeekNextToken();
                    tokenProvider.Next();
                    if (t.Type != TokenType.Semicolon)
                    {
                        throw MyC0Exception.MissSemicolonErr(t.BeginPos);
                    }

                }
                else if (symbolTable.IsFunciton(t.Content))
                {
                    res.Seq = FunctionCall.Analyse(par);
                    t = tokenProvider.PeekNextToken();
                    tokenProvider.Next();
                    if (t.Type != TokenType.Semicolon)
                    {
                        throw MyC0Exception.MissSemicolonErr(t.BeginPos);
                    }

                }
                else
                {
                    throw MyC0Exception.NotExistErr(t.BeginPos);
                }
            }
            else
            {
                throw new MyC0Exception("statement语法错误", t.BeginPos);
            }

            return res;
        }

        public List<IInstruction> GetIns(string par, int offset)
        {
            List<IInstruction> res = Seq.GetIns(par, offset);
            return res;
        }
    }
}
