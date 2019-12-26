using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using C0.Utils;

namespace C0.Tokenizer
{
    public class Tokenizer
    {
        private List<Token> _tokens;
        private readonly FileReader _fileReader;
        private bool _initialized;

        public Tokenizer(String filePath)
        {
            _tokens = new List<Token>();
            _fileReader = new FileReader(filePath);
            _initialized = false;
        }

        public Token NextToken()
        {
            if (!_initialized)
            {
                Init();
            }

            Pos cp = _fileReader.NextPos();
            String s = String.Empty;
            DfaState cd = DfaState.Initial;
            char? cc;
            while (true)
            {
                cc = _fileReader.PeekNextChar();
                if (cc != null)
                {
                    if (cd != DfaState.SingleLineComment
                        && cd != DfaState.MultiLineCommentLeft
                        && cd != DfaState.Char
                        && cd != DfaState.String
                        && !TokenUtils.AcChar(cc.Value))
                    {
                        throw MyC0Exception.InvalidInputErr(_fileReader.CurPos);
                    }
                }
                switch (cd)
                {
                    case DfaState.Initial:
                        if (cc == null)
                        {
                            return new Token(TokenType.Eof, "", cp);
                        }

                        if (TokenUtils.IsSpace(cc.Value))
                        {
                            cd = DfaState.Initial;
                        }
                        else if (TokenUtils.IsLetter(cc.Value))
                        {
                            cd = DfaState.Identifier;
                        }
                        else if (TokenUtils.IsNum(cc.Value))
                        {
                            cd = DfaState.LiteralDecimal;
                        }
                        else
                        {
                            switch (cc.Value)
                            {
                                case '+':
                                    cd = DfaState.OperatorAdd;
                                    break;
                                case '-':
                                    cd = DfaState.OperatorMinus;
                                    break;
                                case '*':
                                    cd = DfaState.OperatorMultiply;
                                    break;
                                case '/':
                                    cd = DfaState.OperatorDivision;
                                    break;
                                case '<':
                                    cd = DfaState.OperatorLess;
                                    break;
                                case '=':
                                    cd = DfaState.OperatorAssignment;
                                    break;
                                case '!':
                                    cd = DfaState.OperatorNotEqual;
                                    break;
                                case '>':
                                    cd = DfaState.OperatorGreater;
                                    break;
                                case '(':
                                    cd = DfaState.BracketsLeftRound;
                                    break;
                                case ')':
                                    cd = DfaState.BracketsRightRound;
                                    break;
                                case '{':
                                    cd = DfaState.BracketsLeftCurly;
                                    break;
                                case '}':
                                    cd = DfaState.BracketsRightCurly;
                                    break;
                                case ';':
                                    cd = DfaState.Semicolon;
                                    break;
                                case ',':
                                    cd = DfaState.Comma;
                                    break;
                                case '\'':
                                    cd = DfaState.Char;
                                    break;
                                case '\"':
                                    cd = DfaState.String;
                                    break;
                                default:
                                    throw MyC0Exception.InvalidInputErr(_fileReader.CurPos);
                            }
                        }

                        if (cd != DfaState.Initial)
                        {
                            cp = _fileReader.NextPos();
                            s += cc.Value;
                        }
                        break;
                    case DfaState.Identifier:
                        if (cc == null || (!TokenUtils.IsLetter(cc.Value) && !TokenUtils.IsNum(cc.Value)))
                        {
                            if (TokenUtils.KeyWords.ContainsKey(s))
                            {
                                return new Token(TokenUtils.KeyWords[s], s, cp);
                            }
                            return new Token(TokenType.Identifier, s, cp);
                        }
                        else
                        {
                            s += cc.Value;
                        }
                        break;
                    case DfaState.LiteralDecimal:
                        if (cc == null || TokenUtils.IsSpace(cc.Value) || TokenUtils.IsPun(cc.Value))
                        {
                            return new Token(TokenType.LiteralDecimal, MyParse.ParseInt(s), cp);
                        }
                        else if (TokenUtils.IsNum(cc.Value))
                        {
                            if (s.Length == 1 && s[0] == '0')
                            {
                                throw MyC0Exception.IllegalNumberErr(cp);
                            }

                            s += cc.Value;
                        }
                        else if ((cc.Value == 'x' || cc.Value == 'X') && s.Length == 1 && s[0] == '0')
                        {
                            cd = DfaState.LiteralHexadecimal;
                            s += char.ToLower(cc.Value);
                        }
                        else
                        {
                            throw MyC0Exception.IllegalNumberErr(_fileReader.CurPos);
                        }
                        break;
                    case DfaState.LiteralHexadecimal:
                        if (cc == null || TokenUtils.IsSpace(cc.Value) || TokenUtils.IsPun(cc.Value))
                        {
                            return new Token(TokenType.LiteralHexadecimal, MyParse.ParseHex(s), cp);
                        }
                        else if (TokenUtils.IsHexLetter(cc.Value))
                        {
                            s += cc.Value;
                        }
                        else
                        {
                            throw MyC0Exception.InvalidInputErr(_fileReader.CurPos);
                        }
                        break;
                    case DfaState.OperatorAdd:
                        return new Token(TokenType.OperatorAdd, s, cp);
                    case DfaState.OperatorMinus:
                        return new Token(TokenType.OperatorMinus, s, cp);
                    case DfaState.OperatorMultiply:
                        return new Token(TokenType.OperatorMultiply, s, cp);
                    case DfaState.OperatorDivision:
                        if (cc != null)
                        {
                            if (cc == '/')
                            {
                                s = "";
                                cd = DfaState.SingleLineComment;
                                break;
                            }
                            if (cc == '*')
                            {
                                s = "";
                                cd = DfaState.MultiLineCommentLeft;
                                break;
                            }
                        }
                        return new Token(TokenType.OperatorDivision, s, cp);
                    case DfaState.OperatorLess:
                        if (cc != null)
                        {
                            if (cc.Value == '=')
                            {
                                s += cc;
                                _fileReader.ReadNext();
                                return new Token(TokenType.OperatorLessEqual, s, cp);
                            }
                        }
                        return new Token(TokenType.OperatorLess, s, cp);
                    case DfaState.OperatorGreater:
                        if (cc != null)
                        {
                            if (cc.Value == '=')
                            {
                                s += cc;
                                _fileReader.ReadNext();
                                return new Token(TokenType.OperatorGreaterEqual, s, cp);
                            }
                        }
                        return new Token(TokenType.OperatorGreater, s, cp);
                    case DfaState.OperatorAssignment:
                        if (cc != null)
                        {
                            if (cc.Value == '=')
                            {
                                s += cc;
                                _fileReader.ReadNext();
                                return new Token(TokenType.OperatorEqual, s, cp);
                            }
                        }
                        return new Token(TokenType.OperatorAssignment, s, cp);
                    case DfaState.BracketsLeftRound:
                        return new Token(TokenType.BracketsLeftRound, s, cp);
                    case DfaState.BracketsRightRound:
                        return new Token(TokenType.BracketsRightRound, s, cp);
                    case DfaState.BracketsLeftCurly:
                        return new Token(TokenType.BracketsLeftCurly, s, cp);
                    case DfaState.BracketsRightCurly:
                        return new Token(TokenType.BracketsRightCurly, s, cp);
                    case DfaState.Semicolon:
                        return new Token(TokenType.Semicolon, s, cp);
                    case DfaState.Comma:
                        return new Token(TokenType.Comma, s, cp);
                    case DfaState.OperatorNotEqual:
                        if (cc == null || cc != '=')
                        {
                            throw MyC0Exception.InvalidTokenErr(cp);
                        }
                        _fileReader.ReadNext();
                        return new Token(TokenType.OperatorNotEqual, s, cp);
                    case DfaState.SingleLineComment:
                        if (cc == null || cc == '\n')
                        {
                            cd = DfaState.Initial;
                        }
                        break;
                    case DfaState.MultiLineCommentLeft:
                        if (cc == null)
                        {
                            throw MyC0Exception.MissRightComment(cp);
                        }
                        if (cc == '*')
                        {
                            cd = DfaState.MultiLineCommentRight;
                        }
                        break;
                    case DfaState.MultiLineCommentRight:
                        if (cc == null)
                        {
                            throw MyC0Exception.MissRightComment(cp);
                        }

                        cd = cc == '/' ? DfaState.Initial : DfaState.MultiLineCommentLeft;
                        break;
                    case DfaState.Char:
                        if (cc == null || cc == '\'')
                        {
                            throw MyC0Exception.InvalidTokenErr(cp);
                        }
                        char tmp = ReadChar();
                        cc = _fileReader.PeekNextChar();
                        _fileReader.ReadNext();
                        if (cc != null && cc == '\'')
                            return new Token(TokenType.Char, tmp, cp);
                        throw MyC0Exception.InvalidTokenErr(cp);
                    case DfaState.String:
                        s = "";
                        while (true)
                        {
                            cc = _fileReader.PeekNextChar();
                            if (cc == null)
                            {
                                throw MyC0Exception.InvalidTokenErr(cp);
                            }

                            if (cc == '\"')
                            {
                                _fileReader.ReadNext();
                                return new Token(TokenType.String, s, cp);
                            }
                            char tc = ReadChar();
                            s += tc;
                        }
                    default:
                        throw MyC0Exception.InvalidTokenErr(cp);
                }
                _fileReader.ReadNext();

            }

        }

        private char ReadChar()
        {
            Pos p = _fileReader.CurPos;
            char res = '.';
            char? cc = _fileReader.PeekNextChar();
            if (cc == null || cc == '\n' || cc == '\r')
            {
                throw MyC0Exception.InvalidTokenErr(_fileReader.CurPos);
            }

            if (cc == '\\')
            {
                _fileReader.ReadNext();
                cc = _fileReader.PeekNextChar();
                if (cc == null)
                {
                    throw MyC0Exception.InvalidInputErr(p);
                }

                if (cc == '\\')
                {
                    res = '\\';
                }
                else if (cc == '\'')
                {
                    res = '\'';
                }
                else if (cc == '\"')
                {
                    res = '\"';
                }
                else if (cc == 'n')
                {
                    res = '\n';
                }
                else if (cc == 'r')
                {
                    res = '\r';
                }
                else if (cc == 't')
                {
                    res = '\t';
                }
                else if (cc == 'x')
                {
                    int s = 0;
                    _fileReader.ReadNext();
                    cc = _fileReader.PeekNextChar();
                    if (cc == null || !TokenUtils.IsHexLetter(cc.Value))
                    {
                        throw MyC0Exception.InvalidInputErr(p);
                    }
                    s = s * 16 + TokenUtils.GetHexNum(cc.Value);
                    _fileReader.ReadNext();
                    cc = _fileReader.PeekNextChar();
                    if (cc == null || !TokenUtils.IsHexLetter(cc.Value))
                    {
                        throw MyC0Exception.InvalidInputErr(p);
                    }
                    s = s * 16 + TokenUtils.GetHexNum(cc.Value);
                    res = (char)s;
                }
                else
                {
                    throw MyC0Exception.InvalidInputErr(p);
                }
            }
            else
            {
                if (!TokenUtils.AllAcChar(cc.Value))
                {
                    throw MyC0Exception.InvalidInputErr(p);
                }
                res = cc.Value;
            }

            _fileReader.ReadNext();
            return res;
        }
        private void Init()
        {
            try
            {
                _fileReader.ReadAll();
            }
            catch (FileNotFoundException)
            {
                throw MyC0Exception.FileNotFoundErr();
            }
            catch (Exception)
            {
                throw MyC0Exception.FileIoErr();
            }

            _initialized = true;

        }
    }
}
