/* Copyright (c) 2012-2017 The ANTLR Project. All rights reserved.
 * Use of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Antlr4.Runtime.Atn;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Sharpen;

namespace Antlr4.Runtime
{
    public abstract class Recognizer<Symbol, ATNInterpreter> : IRecognizer
        where ATNInterpreter : ATNSimulator
    {
        public const int Eof = -1;

        //private static readonly ConditionalWeakTable<IVocabulary, IDictionary<string, int>> tokenTypeMapCache = new ConditionalWeakTable<IVocabulary, IDictionary<string, int>>();
        //private static readonly ConditionalWeakTable<string[], IDictionary<string, int>> ruleIndexMapCache = new ConditionalWeakTable<string[], IDictionary<string, int>>();

        [NotNull]
        private IAntlrErrorListener<Symbol>[] _listeners =
        {
            //ConsoleErrorListener<Symbol>.Instance
        };

        private ATNInterpreter _interp;

        private int _stateNumber = -1;

        /// <summary>
        /// Used to print out token names like ID during debugging and
        /// error reporting.
        /// </summary>
        /// <remarks>
        /// Used to print out token names like ID during debugging and
        /// error reporting.  The generated parsers implement a method
        /// that overrides this to point to their String[] tokenNames.
        /// </remarks>

        public abstract string[] RuleNames
        {
            get;
        }


        /// <summary>Get the vocabulary used by the recognizer.</summary>
        /// <remarks>Get the vocabulary used by the recognizer.</remarks>
        /// <returns>
        /// A
        /// <see cref="IVocabulary"/>
        /// instance providing information about the
        /// vocabulary used by the grammar.
        /// </returns>
        public abstract IVocabulary Vocabulary
        {
			get;
       }
         

        /// <summary>
        /// If this recognizer was generated, it will have a serialized ATN
        /// representation of the grammar.
        /// </summary>
        /// <remarks>
        /// If this recognizer was generated, it will have a serialized ATN
        /// representation of the grammar.
        /// <p>For interpreters, we don't know their serialized ATN despite having
        /// created the interpreter from it.</p>
        /// </remarks>
        public virtual int[] SerializedAtn
        {
            [return: NotNull]
            get
            {
                throw new NotSupportedException("there is no serialized ATN");
            }
        }

        /// <summary>For debugging and other purposes, might want the grammar name.</summary>
        /// <remarks>
        /// For debugging and other purposes, might want the grammar name.
        /// Have ANTLR generate an implementation for this method.
        /// </remarks>
        public abstract string GrammarFileName
        {
            get;
        }

        /// <summary>
        /// Get the
        /// <see cref="Antlr4.Runtime.Atn.ATN"/>
        /// used by the recognizer for prediction.
        /// </summary>
        /// <returns>
        /// The
        /// <see cref="Antlr4.Runtime.Atn.ATN"/>
        /// used by the recognizer for prediction.
        /// </returns>
        public virtual ATN Atn
        {
            get
            {
                return _interp.atn;
            }
        }

        /// <summary>Get the ATN interpreter used by the recognizer for prediction.</summary>
        /// <remarks>Get the ATN interpreter used by the recognizer for prediction.</remarks>
        /// <returns>The ATN interpreter used by the recognizer for prediction.</returns>
        /// <summary>Set the ATN interpreter used by the recognizer for prediction.</summary>
        /// <remarks>Set the ATN interpreter used by the recognizer for prediction.</remarks>
        /// <value>
        /// The ATN interpreter used by the recognizer for
        /// prediction.
        /// </value>
        public virtual ATNInterpreter Interpreter
        {
            get
            {
                return _interp;
            }
            protected set
            {
				_interp = value;
            }
        }


        [NotNull]
        public virtual IList<IAntlrErrorListener<Symbol>> ErrorListeners
        {
            get
            {
                return new List<IAntlrErrorListener<Symbol>>(_listeners);
            }
        }

        public virtual IAntlrErrorListener<Symbol> ErrorListenerDispatch
        {
            get
            {
                return new ProxyErrorListener<Symbol>(ErrorListeners);
            }
        }

        // subclass needs to override these if there are sempreds or actions
        // that the ATN interp needs to execute
        public virtual bool Sempred(RuleContext _localctx, int ruleIndex, int actionIndex)
        {
            return true;
        }

        public virtual bool Precpred(RuleContext localctx, int precedence)
        {
            return true;
        }

        public virtual void Action(RuleContext _localctx, int ruleIndex, int actionIndex)
        {
        }

        /// <summary>
        /// Indicate that the recognizer has changed internal state that is
        /// consistent with the ATN state passed in.
        /// </summary>
        /// <remarks>
        /// Indicate that the recognizer has changed internal state that is
        /// consistent with the ATN state passed in.  This way we always know
        /// where we are in the ATN as the parser goes along. The rule
        /// context objects form a stack that lets us see the stack of
        /// invoking rules. Combine this and we have complete ATN
        /// configuration information.
        /// </remarks>
        public int State
        {
            get
            {
                return _stateNumber;
            }
            set
            {
                int atnState = value;
                //		System.err.println("setState "+atnState);
                _stateNumber = atnState;
            }
        }

        public abstract IIntStream InputStream
        {
            get;
        }
        //		if ( traceATNStates ) _ctx.trace(atnState);
    }
}
