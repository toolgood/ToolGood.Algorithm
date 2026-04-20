using System;
using System.Collections.Generic;

namespace Antlr4Helper.CSharpHelper.RegexEngine
{
    public sealed class RegexParser
    {
        private List<RegexToken> _tokens;
        private int _position;
        private int _groupCount;

        public RegexNode Parse(string pattern)
        {
            if (string.IsNullOrEmpty(pattern))
                return new EmptyNode();

            var lexer = new RegexLexer(pattern);
            _tokens = lexer.Tokenize();
            _position = 0;
            _groupCount = 0;

            var result = ParseAlternation();

            if (CurrentToken.Type != RegexTokenType.EndOfInput)
                throw new RegexParseException($"意外的标记: {CurrentToken.Type}", CurrentToken.Position);

            return result;
        }

        private RegexToken CurrentToken => _position < _tokens.Count ? _tokens[_position] : _tokens[_tokens.Count - 1];

        private void Advance() => _position++;

        private RegexToken Consume(RegexTokenType expected)
        {
            if (CurrentToken.Type != expected)
                throw new RegexParseException($"期望 {expected}，但找到 {CurrentToken.Type}", CurrentToken.Position);
            var token = CurrentToken;
            Advance();
            return token;
        }

        private RegexNode ParseAlternation()
        {
            var alternatives = new List<RegexNode> { ParseConcat() };

            while (CurrentToken.Type == RegexTokenType.Pipe)
            {
                Advance();
                alternatives.Add(ParseConcat());
            }

            if (alternatives.Count == 1)
                return alternatives[0];

            return new AlternationNode(alternatives);
        }

        private RegexNode ParseConcat()
        {
            var children = new List<RegexNode>();

            while (CurrentToken.Type != RegexTokenType.EndOfInput &&
                   CurrentToken.Type != RegexTokenType.Pipe &&
                   CurrentToken.Type != RegexTokenType.RParen)
            {
                children.Add(ParseQuantified());
            }

            if (children.Count == 0)
                return new EmptyNode();
            if (children.Count == 1)
                return children[0];

            return new ConcatNode(children);
        }

        private RegexNode ParseQuantified()
        {
            var atom = ParseAtom();

            if (CurrentToken.Type == RegexTokenType.Star)
            {
                Advance();
                var greedy = CheckGreedy();
                return new QuantifierNode(atom, 0, null, greedy);
            }

            if (CurrentToken.Type == RegexTokenType.Plus)
            {
                Advance();
                var greedy = CheckGreedy();
                return new QuantifierNode(atom, 1, null, greedy);
            }

            if (CurrentToken.Type == RegexTokenType.Question)
            {
                Advance();
                var greedy = CheckGreedy();
                return new QuantifierNode(atom, 0, 1, greedy);
            }

            if (CurrentToken.Type == RegexTokenType.LBrace)
            {
                return ParseBraceQuantifier(atom);
            }

            return atom;
        }

        private bool CheckGreedy()
        {
            if (CurrentToken.Type == RegexTokenType.Question)
            {
                Advance();
                return false;
            }
            return true;
        }

        private RegexNode ParseBraceQuantifier(RegexNode atom)
        {
            Consume(RegexTokenType.LBrace);

            var minCount = ParseNumber();
            int? maxCount = minCount;

            if (CurrentToken.Type == RegexTokenType.Comma)
            {
                Advance();
                if (CurrentToken.Type == RegexTokenType.Digit)
                {
                    maxCount = ParseNumber();
                }
                else
                {
                    maxCount = null;
                }
            }

            Consume(RegexTokenType.RBrace);
            var greedy = CheckGreedy();

            return new QuantifierNode(atom, minCount, maxCount, greedy);
        }

        private int ParseNumber()
        {
            var numStr = "";
            while (CurrentToken.Type == RegexTokenType.Digit)
            {
                numStr += CurrentToken.Value;
                Advance();
            }

            if (string.IsNullOrEmpty(numStr))
                throw new RegexParseException("期望数字", CurrentToken.Position);

            return int.Parse(numStr);
        }

        private RegexNode ParseAtom()
        {
            switch (CurrentToken.Type)
            {
                case RegexTokenType.Char:
                case RegexTokenType.Digit:
                    var c = CurrentToken.Value;
                    Advance();
                    return new CharNode(c);

                case RegexTokenType.EscapeChar:
                    return ParseEscapeSequence();

                case RegexTokenType.Dot:
                    Advance();
                    return new AnyCharNode();

                case RegexTokenType.Caret:
                    Advance();
                    return new AnchorNode(AnchorType.Start);

                case RegexTokenType.Dollar:
                    Advance();
                    return new AnchorNode(AnchorType.End);

                case RegexTokenType.LParen:
                    return ParseGroup();

                case RegexTokenType.LBracket:
                    return ParseCharClass();

                default:
                    throw new RegexParseException($"意外的标记: {CurrentToken.Type}", CurrentToken.Position);
            }
        }

        private RegexNode ParseEscapeSequence()
        {
            var token = Consume(RegexTokenType.EscapeChar);
            var c = token.Value;

            switch (c)
            {
                case 'd':
                    return new CharClassNode(new List<(char, char)> { ('0', '9') }, false);
                case 'D':
                    return new CharClassNode(new List<(char, char)> { ('0', '9') }, true);
                case 'w':
                    return new CharClassNode(new List<(char, char)>
                    {
                        ('a', 'z'), ('A', 'Z'), ('0', '9'), ('_', '_')
                    }, false);
                case 'W':
                    return new CharClassNode(new List<(char, char)>
                    {
                        ('a', 'z'), ('A', 'Z'), ('0', '9'), ('_', '_')
                    }, true);
                case 's':
                    return new CharClassNode(new List<(char, char)>
                    {
                        (' ', ' '), ('\t', '\t'), ('\n', '\n'), ('\r', '\r'), ('\f', '\f'), ('\v', '\v')
                    }, false);
                case 'S':
                    return new CharClassNode(new List<(char, char)>
                    {
                        (' ', ' '), ('\t', '\t'), ('\n', '\n'), ('\r', '\r'), ('\f', '\f'), ('\v', '\v')
                    }, true);
                default:
                    return new CharNode(RegexLexer.GetEscapedChar(c));
            }
        }

        private RegexNode ParseGroup()
        {
            Consume(RegexTokenType.LParen);

            bool isCapturing = true;
            if (CurrentToken.Type == RegexTokenType.Question)
            {
                Advance();
                if (CurrentToken.Type == RegexTokenType.Colon)
                {
                    Advance();
                    isCapturing = false;
                }
                else
                {
                    throw new RegexParseException("不支持的非捕获组语法", CurrentToken.Position);
                }
            }

            var groupIndex = isCapturing ? ++_groupCount : 0;
            var child = ParseAlternation();
            Consume(RegexTokenType.RParen);

            return new GroupNode(child, groupIndex, isCapturing);
        }

        private RegexNode ParseCharClass()
        {
            Consume(RegexTokenType.LBracket);

            var negated = false;
            if (CurrentToken.Type == RegexTokenType.Caret)
            {
                Advance();
                negated = true;
            }

            var ranges = new List<(char Min, char Max)>();

            var first = true;
            while (CurrentToken.Type != RegexTokenType.RBracket || first)
            {
                first = false;

                if (CurrentToken.Type == RegexTokenType.RBracket && ranges.Count > 0)
                    break;

                char startChar;

                if (CurrentToken.Type == RegexTokenType.EscapeChar)
                {
                    Advance();
                    startChar = RegexLexer.GetEscapedChar(CurrentToken.Value);
                    Advance();
                }
                else if (CurrentToken.Type == RegexTokenType.Dash && ranges.Count == 0)
                {
                    startChar = '-';
                    Advance();
                }
                else
                {
                    startChar = GetCharClassChar();
                }

                if (CurrentToken.Type == RegexTokenType.Dash &&
                    _position + 1 < _tokens.Count &&
                    _tokens[_position + 1].Type != RegexTokenType.RBracket)
                {
                    Advance();
                    char endChar;

                    if (CurrentToken.Type == RegexTokenType.EscapeChar)
                    {
                        Advance();
                        endChar = RegexLexer.GetEscapedChar(CurrentToken.Value);
                        Advance();
                    }
                    else
                    {
                        endChar = GetCharClassChar();
                    }

                    if (endChar < startChar)
                        throw new RegexParseException($"无效的字符范围: {startChar}-{endChar}", CurrentToken.Position);

                    ranges.Add((startChar, endChar));
                }
                else
                {
                    ranges.Add((startChar, startChar));
                }
            }

            Consume(RegexTokenType.RBracket);
            return new CharClassNode(ranges, negated);
        }

        private char GetCharClassChar()
        {
            char c;
            switch (CurrentToken.Type)
            {
                case RegexTokenType.Char:
                case RegexTokenType.Digit:
                case RegexTokenType.Dot:
                case RegexTokenType.Star:
                case RegexTokenType.Plus:
                case RegexTokenType.Question:
                case RegexTokenType.Pipe:
                case RegexTokenType.LParen:
                case RegexTokenType.RParen:
                case RegexTokenType.LBrace:
                case RegexTokenType.RBrace:
                case RegexTokenType.Caret:
                case RegexTokenType.Dollar:
                case RegexTokenType.Comma:
                    c = CurrentToken.Value;
                    break;
                case RegexTokenType.Dash:
                    c = '-';
                    break;
                default:
                    throw new RegexParseException($"字符类中的无效字符: {CurrentToken.Type}", CurrentToken.Position);
            }
            Advance();
            return c;
        }
    }

    public sealed class RegexParseException : Exception
    {
        public int Position { get; }

        public RegexParseException(string message, int position) : base($"{message} (位置: {position})")
        {
            Position = position;
        }

        public RegexParseException(string message) : base(message)
        {
            Position = -1;
        }
    }
}
