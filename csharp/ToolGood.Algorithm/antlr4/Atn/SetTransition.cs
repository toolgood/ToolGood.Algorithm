/* Copyright (c) 2012-2017 The ANTLR Project. All rights reserved.
 * Use of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */
using Antlr4.Runtime.Atn;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Sharpen;
namespace Antlr4.Runtime.Atn
{
    public class SetTransition : Transition
    {
        public readonly IntervalSet set;
        public SetTransition(ATNState target, IntervalSet set)
            : base(target)
        {
            if (set == null)
            {
                set = IntervalSet.Of(TokenConstants.InvalidType);
            }
            this.set = set;
        }
        public override Antlr4.Runtime.Atn.TransitionType TransitionType
        {
            get
            {
                return Antlr4.Runtime.Atn.TransitionType.SET;
            }
        }
        public override IntervalSet Label
        {
            get
            {
                return set;
            }
        }
        public override bool Matches(int symbol, int minVocabSymbol, int maxVocabSymbol)
        {
            return set.Contains(symbol);
        }
    }
}
