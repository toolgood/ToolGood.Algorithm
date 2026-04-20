using Antlr4Helper.CSharpHelper.RegexEngine;
using System;
using System.Collections.Generic;

namespace Antlr4Helper.CSharpHelper.Regexs
{
    public enum RegexTokenType
    {
        Char,
        Dot,
        Star,
        Plus,
        Question,
        Pipe,
        LParen,
        RParen,
        LBracket,
        RBracket,
        LBrace,
        RBrace,
        Caret,
        Dollar,
        Dash,
        Comma,
        Colon,
        Digit,
        EscapeChar,
        EndOfInput
    }

    public sealed class RegexToken
    {
        public RegexTokenType Type { get; }
        public char Value { get; }
        public int Position { get; }

        public RegexToken(RegexTokenType type, char value, int position)
        {
            Type = type;
            Value = value;
            Position = position;
        }

        public override string ToString() => $"{Type}: '{Value}' at {Position}";
    }

    public sealed class RegexLexer
    {
        private readonly string _input;
        private int _position;
        private readonly List<RegexToken> _tokens;

        public RegexLexer(string input)
        {
            _input = input ?? throw new ArgumentNullException(nameof(input));
            _position = 0;
            _tokens = new List<RegexToken>();
        }

        public List<RegexToken> Tokenize()
        {
            _tokens.Clear();
            _position = 0;

            while (_position < _input.Length)
            {
                var c = _input[_position];
                var token = ScanToken(c);
                _tokens.Add(token);
            }

            _tokens.Add(new RegexToken(RegexTokenType.EndOfInput, '\0', _position));
            return _tokens;
        }

        private RegexToken ScanToken(char c)
        {
            var startPos = _position;
            _position++;

            switch (c)
            {
                case '.':
                    return new RegexToken(RegexTokenType.Dot, c, startPos);
                case '*':
                    return new RegexToken(RegexTokenType.Star, c, startPos);
                case '+':
                    return new RegexToken(RegexTokenType.Plus, c, startPos);
                case '?':
                    return new RegexToken(RegexTokenType.Question, c, startPos);
                case '|':
                    return new RegexToken(RegexTokenType.Pipe, c, startPos);
                case '(':
                    return new RegexToken(RegexTokenType.LParen, c, startPos);
                case ')':
                    return new RegexToken(RegexTokenType.RParen, c, startPos);
                case '[':
                    return new RegexToken(RegexTokenType.LBracket, c, startPos);
                case ']':
                    return new RegexToken(RegexTokenType.RBracket, c, startPos);
                case '{':
                    return new RegexToken(RegexTokenType.LBrace, c, startPos);
                case '}':
                    return new RegexToken(RegexTokenType.RBrace, c, startPos);
                case '^':
                    return new RegexToken(RegexTokenType.Caret, c, startPos);
                case '$':
                    return new RegexToken(RegexTokenType.Dollar, c, startPos);
                case '-':
                    return new RegexToken(RegexTokenType.Dash, c, startPos);
                case ',':
                    return new RegexToken(RegexTokenType.Comma, c, startPos);
                case ':':
                    return new RegexToken(RegexTokenType.Colon, c, startPos);
                case '\\':
                    return ScanEscapeSequence(startPos);
                default:
                    if (char.IsDigit(c))
                        return new RegexToken(RegexTokenType.Digit, c, startPos);
                    return new RegexToken(RegexTokenType.Char, c, startPos);
            }
        }

        private RegexToken ScanEscapeSequence(int startPos)
        {
            if (_position >= _input.Length)
                throw new RegexParseException("正则表达式末尾存在未完成的转义序列", startPos);

            var c = _input[_position];
            _position++;

            switch (c)
            {
                case 'd':
                case 'D':
                case 'w':
                case 'W':
                case 's':
                case 'S':
                case 'n':
                case 'r':
                case 't':
                case 'f':
                case 'v':
                case '0':
                    return new RegexToken(RegexTokenType.EscapeChar, c, startPos);
                default:
                    return new RegexToken(RegexTokenType.EscapeChar, c, startPos);
            }
        }

        public static char GetEscapedChar(char c)
        {
            switch (c)
            {
                case 'n': return '\n';
                case 'r': return '\r';
                case 't': return '\t';
                case 'f': return '\f';
                case 'v': return '\v';
                case '0': return '\0';
                default: return c;
            }
        }
    }
}
