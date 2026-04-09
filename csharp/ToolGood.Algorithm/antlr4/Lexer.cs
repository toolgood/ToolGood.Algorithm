/* Copyright (c) 2012-2017 The ANTLR Project. All rights reserved.
 * Use of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */
using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using Antlr4.Runtime;
using Antlr4.Runtime.Atn;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Sharpen;
namespace Antlr4.Runtime
{
    public abstract class Lexer : Recognizer<int, LexerATNSimulator>, ITokenSource
    {
        public const int DEFAULT_MODE = 0;
        public const int DefaultTokenChannel = TokenConstants.DefaultChannel;
        public const int MinCharValue = 0x0000;
        public const int MaxCharValue = 0x10FFFF;
        private ICharStream _input;
        protected readonly TextWriter ErrorOutput;
		private Tuple<ITokenSource, ICharStream> _tokenFactorySourcePair;
		private ITokenFactory _factory = CommonTokenFactory.Default;
        private IToken _token;
        private int _tokenStartCharIndex = -1;
		private int _tokenStartLine;
		private int _tokenStartColumn;
		private bool _hitEOF;
		private int _channel;
		private int _type;
        private readonly Stack<int> _modeStack = new Stack<int>();
		private int _mode = Antlr4.Runtime.Lexer.DEFAULT_MODE;
		private string _text;
        public Lexer(ICharStream input, TextWriter output, TextWriter errorOutput)
        {
            this._input = input;
            this.ErrorOutput = errorOutput;
            this._tokenFactorySourcePair = Tuple.Create((ITokenSource)this, input);
        }
        public virtual IToken NextToken()
        {
            if (_input == null)
            {
                throw new InvalidOperationException("nextToken requires a non-null input stream.");
            }
            int tokenStartMarker = _input.Mark();
            try
            {
                while (true)
                {
                    if (_hitEOF)
                    {
                        EmitEOF();
                        return _token;
                    }
                    _token = null;
                    _channel = TokenConstants.DefaultChannel;
                    _tokenStartCharIndex = _input.Index;
                    _tokenStartColumn = Interpreter.Column;
                    _tokenStartLine = Interpreter.Line;
                    _text = null;
                    do
                    {
                        _type = TokenConstants.InvalidType;
                        int ttype;
                        try
                        {
                            ttype = Interpreter.Match(_input, _mode);
                        }
                        catch (LexerNoViableAltException e)
                        {
                            NotifyListeners(e);
                            Recover(e);
                            ttype = TokenTypes.Skip;
                        }
                        if (_input.LA(1) == IntStreamConstants.EOF)
                        {
                            _hitEOF = true;
                        }
                        if (_type == TokenConstants.InvalidType)
                        {
                            _type = ttype;
                        }
                        if (_type == TokenTypes.Skip)
                        {
                            goto outer_continue;
                        }
                    }
                    while (_type == TokenTypes.More);
                    if (_token == null)
                    {
                        Emit();
                    }
                    return _token;
outer_continue: ;
                }
            }
            finally
            {
                _input.Release(tokenStartMarker);
            }
        }
        public virtual void Skip()
        {
            _type = TokenTypes.Skip;
        }
        public virtual void More()
        {
            _type = TokenTypes.More;
        }
        public virtual void Mode(int m)
        {
            _mode = m;
        }
        public virtual void PushMode(int m)
        {
            _modeStack.Push(_mode);
            Mode(m);
        }
        public virtual int PopMode()
        {
            if (_modeStack.Count == 0)
            {
                throw new InvalidOperationException();
            }
            int mode = _modeStack.Pop();
            Mode(mode);
            return _mode;
        }
        public virtual ITokenFactory TokenFactory
        {
            get
            {
                return _factory;
            }
            set
            {
                ITokenFactory factory = value;
                this._factory = factory;
            }
        }
        public virtual string SourceName
        {
            get
            {
                return _input.SourceName;
            }
        }
        public override IIntStream InputStream
        {
            get
            {
                return _input;
            }
        }
        ICharStream ITokenSource.InputStream
        {
            get
            {
				return _input;
            }
        }
        public virtual void Emit(IToken token)
        {
            this._token = token;
        }
        public virtual IToken Emit()
        {
            IToken t = _factory.Create(_tokenFactorySourcePair, _type, _text, _channel, _tokenStartCharIndex, CharIndex - 1, _tokenStartLine, _tokenStartColumn);
            Emit(t);
            return t;
        }
        public virtual IToken EmitEOF()
        {
            int cpos = Column;
			int line = Line;
            IToken eof = _factory.Create(_tokenFactorySourcePair, TokenConstants.EOF, null, TokenConstants.DefaultChannel, _input.Index, _input.Index - 1, line, cpos);
            Emit(eof);
            return eof;
        }
        public virtual int Line
        {
            get
            {
                return Interpreter.Line;
            }
            set
            {
                int line = value;
                Interpreter.Line = line;
            }
        }
        public virtual int Column
        {
            get
            {
                return Interpreter.Column;
            }
            set
            {
                int charPositionInLine = value;
                Interpreter.Column = charPositionInLine;
            }
        }
        public virtual int CharIndex
        {
            get
            {
                return _input.Index;
            }
        }
        public virtual int Type
        {
            get
            {
                return _type;
            }
            set
            {
                int ttype = value;
                _type = ttype;
            }
        }
        public virtual int Channel
        {
            get
            {
                return _channel;
            }
            set
            {
                int channel = value;
                _channel = channel;
            }
        }
        public virtual void Recover(LexerNoViableAltException e)
        {
            if (_input.LA(1) != IntStreamConstants.EOF)
            {
                Interpreter.Consume(_input);
            }
        }
        public virtual void NotifyListeners(LexerNoViableAltException e)
        {
        }
    }
}
