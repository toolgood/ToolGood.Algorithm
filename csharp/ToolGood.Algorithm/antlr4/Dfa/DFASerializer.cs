/* Copyright (c) 2012-2017 The ANTLR Project. All rights reserved.
 * Use of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */
using System;
using System.Collections.Generic;
using System.Text;
using Antlr4.Runtime.Atn;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Sharpen;

namespace Antlr4.Runtime.Dfa
{
    /// <summary>A DFA walker that knows how to dump them to serialized strings.</summary>
    /// <remarks>A DFA walker that knows how to dump them to serialized strings.</remarks>
    internal class DFASerializer
    {
        
        private readonly DFA dfa;

        
        private readonly IVocabulary vocabulary;

        
        internal readonly string[] ruleNames;

        
        internal readonly ATN atn;

        public DFASerializer(DFA dfa, IVocabulary vocabulary)
            : this(dfa, vocabulary, null, null)
        {
        }

        public DFASerializer(DFA dfa, IVocabulary vocabulary, string[] ruleNames, ATN atn)
        {
            this.dfa = dfa;
            this.vocabulary = vocabulary;
            this.ruleNames = ruleNames;
            this.atn = atn;
        }


        protected internal virtual string GetEdgeLabel(int i)
        {
            return vocabulary.GetDisplayName(i - 1);
        }

        internal virtual string GetStateString(DFAState s)
        {
			if (s == ATNSimulator.ERROR)
            {
                return "ERROR";
            }

			int n = s.stateNumber;
			string baseStateStr = (s.isAcceptState ? ":" : "") + "s" + n + (s.requiresFullContext ? "^" : "");
			if ( s.isAcceptState ) {
				if ( s.predicates!=null ) {
					return baseStateStr + "=>" + Arrays.ToString(s.predicates);
				}
				else {
					return baseStateStr + "=>" + s.prediction;
				}
			}
			else {
				return baseStateStr;
			}
        }
    }
}
