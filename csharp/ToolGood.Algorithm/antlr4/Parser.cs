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
    /// <summary>This is all the parsing support code essentially; most of it is error recovery stuff.</summary>
    /// <remarks>This is all the parsing support code essentially; most of it is error recovery stuff.</remarks>
    public abstract class Parser : Recognizer<IToken, ParserATNSimulator>
    {
        /// <summary>The error handling strategy for the parser.</summary>
        /// <remarks>
        /// The error handling strategy for the parser. The default value is a new
        /// instance of
        /// <see cref="DefaultErrorStrategy"/>
        /// .
        /// </remarks>
        /// <seealso cref="ErrorHandler"/>
        [NotNull]
		private IAntlrErrorStrategy _errHandler = new DefaultErrorStrategy();

        /// <summary>The input stream.</summary>
        /// <remarks>The input stream.</remarks>
        /// <seealso cref="InputStream()"/>
    	private ITokenStream _input;

		private readonly List<int> _precedenceStack = new List<int> { 0 };

        /// <summary>
        /// The
        /// <see cref="ParserRuleContext"/>
        /// object for the currently executing rule.
        /// This is always non-null during the parsing process.
        /// </summary>
        private ParserRuleContext _ctx;

        /// <summary>
        /// Specifies whether or not the parser should construct a parse tree during
        /// the parsing process.
        /// </summary>
        /// <remarks>
        /// Specifies whether or not the parser should construct a parse tree during
        /// the parsing process. The default value is
        /// <see langword="true"/>
        /// .
        /// </remarks>
        /// <seealso cref="BuildParseTree"/>
        private bool _buildParseTrees = true;

        /// <summary>
        /// The list of
        /// <see cref="Antlr4.Runtime.Tree.IParseTreeListener"/>
        /// listeners registered to receive
        /// events during the parse.
        /// </summary>
        /// <seealso cref="AddParseListener(Antlr4.Runtime.Tree.IParseTreeListener)"/>
        [Nullable]
        private IList<IParseTreeListener> _parseListeners;

        /// <summary>The number of syntax errors reported during parsing.</summary>
        /// <remarks>
        /// The number of syntax errors reported during parsing. This value is
        /// incremented each time
        /// <see cref="NotifyErrorListeners(string)"/>
        /// is called.
        /// </remarks>
        private int _syntaxErrors;

        protected readonly TextWriter ErrorOutput;

        public Parser(ITokenStream input, TextWriter output, TextWriter errorOutput)
        {
            TokenStream = input;
            ErrorOutput = errorOutput;
        }

        /// <summary>reset the parser's state</summary>
        public virtual void Reset()
        {
            if (((ITokenStream)InputStream) != null)
            {
                ((ITokenStream)InputStream).Seek(0);
            }
            _errHandler.Reset(this);
            _ctx = null;
            _syntaxErrors = 0;
            _precedenceStack.Clear();
            _precedenceStack.Add(0);
            ATNSimulator interpreter = Interpreter;
            if (interpreter != null)
            {
                interpreter.Reset();
            }
        }

        /// <summary>
        /// Match current input symbol against
        /// <paramref name="ttype"/>
        /// . If the symbol type
        /// matches,
        /// <see cref="IAntlrErrorStrategy.ReportMatch(Parser)"/>
        /// and
        /// <see cref="Consume()"/>
        /// are
        /// called to complete the match process.
        /// <p>If the symbol type does not match,
        /// <see cref="IAntlrErrorStrategy.RecoverInline(Parser)"/>
        /// is called on the current error
        /// strategy to attempt recovery. If
        /// <see cref="BuildParseTree()"/>
        /// is
        /// <see langword="true"/>
        /// and the token index of the symbol returned by
        /// <see cref="IAntlrErrorStrategy.RecoverInline(Parser)"/>
        /// is -1, the symbol is added to
        /// the parse tree by calling
        /// <see cref="ParserRuleContext.AddErrorNode(IToken)"/>
        /// .</p>
        /// </summary>
        /// <param name="ttype">the token type to match</param>
        /// <returns>the matched symbol</returns>
        /// <exception cref="RecognitionException">
        /// if the current input symbol did not match
        /// <paramref name="ttype"/>
        /// and the error strategy could not recover from the
        /// mismatched symbol
        /// </exception>
        /// <exception cref="Antlr4.Runtime.RecognitionException"/>
        [return: NotNull]
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
                if (_buildParseTrees && t.TokenIndex == -1)
                {
                    // we must have conjured up a new token during single token insertion
                    // if it's not the current symbol
                    _ctx.AddErrorNode(t);
                }
            }
            return t;
        }

        /// <summary>
        /// Track the
        /// <see cref="ParserRuleContext"/>
        /// objects during the parse and hook
        /// them up using the
        /// <see cref="ParserRuleContext.children"/>
        /// list so that it
        /// forms a parse tree. The
        /// <see cref="ParserRuleContext"/>
        /// returned from the start
        /// rule represents the root of the parse tree.
        /// <p>Note that if we are not building parse trees, rule contexts only point
        /// upwards. When a rule exits, it returns the context but that gets garbage
        /// collected if nobody holds a reference. It points upwards but nobody
        /// points at it.</p>
        /// <p>When we build parse trees, we are adding all of these contexts to
        /// <see cref="ParserRuleContext.children"/>
        /// list. Contexts are then not candidates
        /// for garbage collection.</p>
        /// </summary>
        /// <summary>
        /// Gets whether or not a complete parse tree will be constructed while
        /// parsing.
        /// </summary>
        /// <remarks>
        /// Gets whether or not a complete parse tree will be constructed while
        /// parsing. This property is
        /// <see langword="true"/>
        /// for a newly constructed parser.
        /// </remarks>
        /// <returns>
        ///
        /// <see langword="true"/>
        /// if a complete parse tree will be constructed while
        /// parsing, otherwise
        /// <see langword="false"/>
        /// </returns>
        public virtual bool BuildParseTree
        {
            get
            {
                return _buildParseTrees;
            }
            set
            {
				this._buildParseTrees = value;
            }
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

        /// <summary>Notify any parse listeners of an enter rule event.</summary>
        /// <remarks>Notify any parse listeners of an enter rule event.</remarks>
        /// <seealso cref="AddParseListener(Antlr4.Runtime.Tree.IParseTreeListener)"/>
        protected internal virtual void TriggerEnterRuleEvent()
        {
            foreach (IParseTreeListener listener in _parseListeners)
            {
                listener.EnterEveryRule(_ctx);
                _ctx.EnterRule(listener);
            }
        }

        /// <summary>Notify any parse listeners of an exit rule event.</summary>
        /// <remarks>Notify any parse listeners of an exit rule event.</remarks>
        /// <seealso cref="AddParseListener(Antlr4.Runtime.Tree.IParseTreeListener)"/>
        protected internal virtual void TriggerExitRuleEvent()
        {
            // reverse order walk of listeners
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

        /// <summary>
        /// Match needs to return the current input symbol, which gets put
        /// into the label for the associated token ref; e.g., x=ID.
        /// </summary>
        /// <remarks>
        /// Match needs to return the current input symbol, which gets put
        /// into the label for the associated token ref; e.g., x=ID.
        /// </remarks>
        public virtual IToken CurrentToken
        {
            get
            {
                return _input.LT(1);
            }
        }

        public void NotifyErrorListeners(string msg)
        {
            NotifyErrorListeners(CurrentToken, msg, null);
        }

        public virtual void NotifyErrorListeners(IToken offendingToken, string msg, RecognitionException e)
        {
            _syntaxErrors++;
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

        /// <summary>
        /// Consume and return the
        /// <linkplain>
        /// #getCurrentToken
        /// current symbol
        /// </linkplain>
        /// .
        /// <p>E.g., given the following input with
        /// <c>A</c>
        /// being the current
        /// lookahead symbol, this function moves the cursor to
        /// <c>B</c>
        /// and returns
        /// <c>A</c>
        /// .</p>
        /// <pre>
        /// A B
        /// ^
        /// </pre>
        /// If the parser is not in error recovery mode, the consumed symbol is added
        /// to the parse tree using
        /// <see cref="ParserRuleContext.AddChild(IToken)"/>
        /// , and
        /// <see cref="Antlr4.Runtime.Tree.IParseTreeListener.VisitTerminal(Antlr4.Runtime.Tree.ITerminalNode)"/>
        /// is called on any parse listeners.
        /// If the parser <em>is</em> in error recovery mode, the consumed symbol is
        /// added to the parse tree using
        /// <see cref="ParserRuleContext.AddErrorNode(IToken)"/>
        /// , and
        /// <see cref="Antlr4.Runtime.Tree.IParseTreeListener.VisitErrorNode(Antlr4.Runtime.Tree.IErrorNode)"/>
        /// is called on any parse
        /// listeners.
        /// </summary>
        public virtual IToken Consume()
        {
            IToken o = CurrentToken;
            if (o.Type != Eof)
            {
                ((ITokenStream)InputStream).Consume();
            }
            bool hasListener = _parseListeners != null && _parseListeners.Count != 0;
            if (_buildParseTrees || hasListener)
            {
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
            }
            return o;
        }

        protected internal virtual void AddContextToParseTree()
        {
            ParserRuleContext parent = (ParserRuleContext)_ctx.Parent;
            // add current context to parent if we have a parent
            if (parent != null)
            {
                parent.AddChild(_ctx);
            }
        }

        /// <summary>Always called by generated parsers upon entry to a rule.</summary>
        /// <remarks>
        /// Always called by generated parsers upon entry to a rule. Access field
        /// <see cref="_ctx"/>
        /// get the current context.
        /// </remarks>
        public virtual void EnterRule(ParserRuleContext localctx, int state, int ruleIndex)
        {
            State = state;
            _ctx = localctx;
            _ctx.Start = _input.LT(1);
            if (_buildParseTrees)
            {
                AddContextToParseTree();
            }
            if (_parseListeners != null)
            {
                TriggerEnterRuleEvent();
            }
        }
 
        public virtual void ExitRule()
        {
            _ctx.Stop = _input.LT(-1);
            // trigger event on _ctx, before it reverts to parent
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
            // if we have new localctx, make sure we replace existing ctx
            // that is previous child of parse tree
            if (_buildParseTrees && _ctx != localctx)
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

        /// <summary>Get the precedence level for the top-most precedence rule.</summary>
        /// <remarks>Get the precedence level for the top-most precedence rule.</remarks>
        /// <returns>
        /// The precedence level for the top-most precedence rule, or -1 if
        /// the parser context is not nested within a precedence rule.
        /// </returns>
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

        // simulates rule entry for left-recursive rules
        /// <summary>
        /// Like
        /// <see cref="EnterRule(ParserRuleContext, int, int)"/>
        /// but for recursive rules.
        /// </summary>
        public virtual void PushNewRecursionContext(ParserRuleContext localctx, int state, int ruleIndex)
        {
            ParserRuleContext previous = _ctx;
            previous.Parent = localctx;
            previous.invokingState = state;
            previous.Stop = _input.LT(-1);
            _ctx = localctx;
            _ctx.Start = previous.Start;
            if (_buildParseTrees)
            {
                _ctx.AddChild(previous);
            }
            if (_parseListeners != null)
            {
                TriggerEnterRuleEvent();
            }
        }

        // simulates rule entry for left-recursive rules
        public virtual void UnrollRecursionContexts(ParserRuleContext _parentctx)
        {
            _precedenceStack.RemoveAt(_precedenceStack.Count - 1);
            _ctx.Stop = _input.LT(-1);
            ParserRuleContext retctx = _ctx;
            // save current ctx (return value)
            // unroll so _ctx is as it was before call to recursive method
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
            // hook into tree
            retctx.Parent = _parentctx;
            if (_buildParseTrees && _parentctx != null)
            {
                // add return ctx into invoking rule's tree
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

        /// <summary>
        /// Computes the set of input symbols which could follow the current parser
        /// state and context, as given by
        /// <see cref="Recognizer{Symbol, ATNInterpreter}.State()"/>
        /// and
        /// <see cref="Context()"/>
        /// ,
        /// respectively.
        /// </summary>
        /// <seealso cref="Antlr4.Runtime.Atn.ATN.GetExpectedTokens(int, RuleContext)"/>
        [return: NotNull]
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
