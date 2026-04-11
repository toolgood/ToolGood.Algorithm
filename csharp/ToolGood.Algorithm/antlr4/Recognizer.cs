/* Copyright (c) 2012-2017 The ANTLR Project. All rights reserved.
 * Use of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Antlr4.Runtime.Atn;
using Antlr4.Runtime.Misc;
namespace Antlr4.Runtime
{
    internal abstract class Recognizer<Symbol, ATNInterpreter> : IRecognizer
        where ATNInterpreter : ATNSimulator
    {
        public const int Eof = -1;
        private static readonly ConditionalWeakTable<IVocabulary, IDictionary<string, int>> tokenTypeMapCache = new ConditionalWeakTable<IVocabulary, IDictionary<string, int>>();
        private static readonly ConditionalWeakTable<string[], IDictionary<string, int>> ruleIndexMapCache = new ConditionalWeakTable<string[], IDictionary<string, int>>();
        private IAntlrErrorListener<Symbol>[] _listeners =
        {
        };
        private ATNInterpreter _interp;
        private int _stateNumber = -1;
        public abstract string[] RuleNames
        {
            get;
        }

        public abstract IVocabulary Vocabulary
        {
			get;
       }
        public virtual int[] SerializedAtn
        {
            get
            {
                throw new NotSupportedException("there is no serialized ATN");
            }
        }
        public abstract string GrammarFileName
        {
            get;
        }
        public virtual ATN Atn
        {
            get
            {
                return _interp.atn;
            }
        }
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
        public virtual IList<IAntlrErrorListener<Symbol>> ErrorListeners
        {
            get
            {
                return new List<IAntlrErrorListener<Symbol>>(_listeners);
            }
        }
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
        public int State
        {
            get
            {
                return _stateNumber;
            }
            set
            {
                int atnState = value;
                _stateNumber = atnState;
            }
        }
        public abstract IIntStream InputStream
        {
            get;
        }
    }
}
