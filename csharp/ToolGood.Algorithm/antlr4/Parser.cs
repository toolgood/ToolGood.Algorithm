/* Copyright (c) 2012-2017 The ANTLR Project. All rights reserved.
 * Use of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */
using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Antlr4.Runtime.Atn;
using Antlr4.Runtime.Dfa;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Sharpen;
using Antlr4.Runtime.Tree;
namespace Antlr4.Runtime
{
    public abstract class Parser : Recognizer<IToken, ParserATNSimulator>
    {
		private IAntlrErrorStrategy _errHandler = new DefaultErrorStrategy();
    	private ITokenStream _input;
		private readonly List<int> _precedenceStack = new List<int> { 0 };
        private ParserRuleContext _ctx;
        private IList<IParseTreeListener> _parseListeners;
        protected readonly TextWriter ErrorOutput;
        public Parser(ITokenStream input, TextWriter output, TextWriter errorOutput)
        {
            TokenStream = input;
            ErrorOutput = errorOutput;
        }
        public virtual void Reset()
        {
            if (((ITokenStream)InputStream) != null)
            {
                ((ITokenStream)InputStream).Seek(0);
            }
            _errHandler.Reset(this);
            _ctx = null;
            _precedenceStack.Clear();
            _precedenceStack.Add(0);
            ATNSimulator interpreter = Interpreter;
            if (interpreter != null)
            {
                interpreter.Reset();
            }
        }
        public virtual IToken Match(int ttype)
        {
            IToken t = CurrentToken;
            if (t.Type == ttype)
            {
                _errHandler.ReportMatch(this);
                Consume();
            }
            else
            {
                t = _errHandler.RecoverInline(this);
                if ( t.TokenIndex == -1)
                {
                    _ctx.AddErrorNode(t);
                }
            }
            return t;
        }
        public virtual IList<IParseTreeListener> ParseListeners
        {
            get
            {
                IList<IParseTreeListener> listeners = _parseListeners;
                if (listeners == null)
                {
                    return Sharpen.Collections.EmptyList<IParseTreeListener>();
                }
                return listeners;
            }
        }
        protected internal virtual void TriggerEnterRuleEvent()
        {
            foreach (IParseTreeListener listener in _parseListeners)
            {
                listener.EnterEveryRule(_ctx);
                _ctx.EnterRule(listener);
            }
        }
        protected internal virtual void TriggerExitRuleEvent()
        {
			if (_parseListeners != null) {
				for (int i = _parseListeners.Count - 1; i >= 0; i--) {
					IParseTreeListener listener = _parseListeners [i];
					_ctx.ExitRule (listener);
					listener.ExitEveryRule (_ctx);
				}
			}
        }
        public virtual IAntlrErrorStrategy ErrorHandler
        {
            get
            {
                return _errHandler;
            }
            set
            {
                IAntlrErrorStrategy handler = value;
                this._errHandler = handler;
            }
        }
        public override IIntStream InputStream
		{
			get
			{
				return _input;
			}
		}
		public ITokenStream TokenStream
		{
			get
			{
				return _input;
			}
			set
			{
				this._input = null;
				Reset ();
				this._input = value;
			}
		}
        public virtual IToken CurrentToken
        {
            get
            {
                return _input.LT(1);
            }
        }
        public virtual void NotifyErrorListeners(IToken offendingToken, string msg, RecognitionException e)
        {
            int line = -1;
            int charPositionInLine = -1;
            if (offendingToken != null)
            {
                line = offendingToken.Line;
                charPositionInLine = offendingToken.Column;
            }
            IAntlrErrorListener<IToken> listener = ((IParserErrorListener)ErrorListenerDispatch);
            listener.SyntaxError(ErrorOutput, this, offendingToken, line, charPositionInLine, msg, e);
        }
        public virtual IToken Consume()
        {
            IToken o = CurrentToken;
            if (o.Type != Eof)
            {
                ((ITokenStream)InputStream).Consume();
            }
            bool hasListener = _parseListeners != null && _parseListeners.Count != 0;
                if (_errHandler.InErrorRecoveryMode(this))
                {
                    IErrorNode node = _ctx.AddErrorNode(o);
                    if (_parseListeners != null)
                    {
                        foreach (IParseTreeListener listener in _parseListeners)
                        {
                            listener.VisitErrorNode(node);
                        }
                    }
                }
                else
                {
                    ITerminalNode node = _ctx.AddChild(o);
                    if (_parseListeners != null)
                    {
                        foreach (IParseTreeListener listener in _parseListeners)
                        {
                            listener.VisitTerminal(node);
                        }
                    }
                }
            return o;
        }
        protected internal virtual void AddContextToParseTree()
        {
            ParserRuleContext parent = (ParserRuleContext)_ctx.Parent;
            if (parent != null)
            {
                parent.AddChild(_ctx);
            }
        }
        public virtual void EnterRule(ParserRuleContext localctx, int state, int ruleIndex)
        {
            State = state;
            _ctx = localctx;
            _ctx.Start = _input.LT(1);
                AddContextToParseTree();
            if (_parseListeners != null)
            {
                TriggerEnterRuleEvent();
            }
        }
        public virtual void ExitRule()
        {
            _ctx.Stop = _input.LT(-1);
            if (_parseListeners != null)
            {
                TriggerExitRuleEvent();
            }
            State = _ctx.invokingState;
            _ctx = (ParserRuleContext)_ctx.Parent;
        }
        public virtual void EnterOuterAlt(ParserRuleContext localctx, int altNum)
        {
        	localctx.setAltNumber(altNum);
            if ( _ctx != localctx)
            {
                ParserRuleContext parent = (ParserRuleContext)_ctx.Parent;
                if (parent != null)
                {
                    parent.RemoveLastChild();
                    parent.AddChild(localctx);
                }
            }
            _ctx = localctx;
        }
        public int Precedence
        {
            get
            {
                if (_precedenceStack.Count == 0)
                {
                    return -1;
                }
                return _precedenceStack[_precedenceStack.Count - 1];
            }
        }
        public virtual void EnterRecursionRule(ParserRuleContext localctx, int state, int ruleIndex, int precedence)
        {
            State = state;
            _precedenceStack.Add(precedence);
            _ctx = localctx;
            _ctx.Start = _input.LT(1);
            if (_parseListeners != null)
            {
                TriggerEnterRuleEvent();
            }
        }
        public virtual void PushNewRecursionContext(ParserRuleContext localctx, int state, int ruleIndex)
        {
            ParserRuleContext previous = _ctx;
            previous.Parent = localctx;
            previous.invokingState = state;
            previous.Stop = _input.LT(-1);
            _ctx = localctx;
            _ctx.Start = previous.Start;
                _ctx.AddChild(previous);
            if (_parseListeners != null)
            {
                TriggerEnterRuleEvent();
            }
        }
        public virtual void UnrollRecursionContexts(ParserRuleContext _parentctx)
        {
            _precedenceStack.RemoveAt(_precedenceStack.Count - 1);
            _ctx.Stop = _input.LT(-1);
            ParserRuleContext retctx = _ctx;
            if (_parseListeners != null)
            {
                while (_ctx != _parentctx)
                {
                    TriggerExitRuleEvent();
                    _ctx = (ParserRuleContext)_ctx.Parent;
                }
            }
            else
            {
                _ctx = _parentctx;
            }
            retctx.Parent = _parentctx;
            if (_parentctx != null)
            {
                _parentctx.AddChild(retctx);
            }
        }
        public virtual ParserRuleContext Context
        {
            get
            {
                return _ctx;
            }
            set
            {
                ParserRuleContext ctx = value;
                _ctx = ctx;
            }
        }
        public override bool Precpred(RuleContext localctx, int precedence)
        {
            return precedence >= _precedenceStack[_precedenceStack.Count - 1];
        }
        public new IParserErrorListener ErrorListenerDispatch
        {
            get
            {
                return new ProxyParserErrorListener(ErrorListeners);
            }
        }
        public virtual IntervalSet GetExpectedTokens()
        {
            return Atn.GetExpectedTokens(State, Context);
        }
        public virtual ParserRuleContext RuleContext
        {
            get
            {
                return _ctx;
            }
        }
    }
}
