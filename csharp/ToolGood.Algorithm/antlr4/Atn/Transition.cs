/* Copyright (c) 2012-2017 The ANTLR Project. All rights reserved.
 * Use of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */
using System;
using System.Collections.ObjectModel;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Sharpen;
namespace Antlr4.Runtime.Atn
{
    public abstract class Transition
    {
        public static readonly ReadOnlyCollection<string> serializationNames = new ReadOnlyCollection<string>(Arrays.AsList("INVALID", "EPSILON", "RANGE", "RULE", "PREDICATE", "ATOM", "ACTION", "SET", "NOT_SET", "WILDCARD", "PRECEDENCE"));
        public ATNState target;
        protected internal Transition(ATNState target)
        {
            if (target == null)
            {
                throw new ArgumentNullException("target cannot be null.");
            }
            this.target = target;
        }
        public abstract TransitionType TransitionType
        {
            get;
        }
        public virtual bool IsEpsilon
        {
            get
            {
                return false;
            }
        }
        public virtual IntervalSet Label
        {
            get
            {
                return null;
            }
        }
        public abstract bool Matches(int symbol, int minVocabSymbol, int maxVocabSymbol);
    }
}
