/* Copyright (c) 2012-2017 The ANTLR Project. All rights reserved.
 * Use of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */
using Antlr4.Runtime.Misc;
namespace Antlr4.Runtime.Atn
{
    internal sealed class EpsilonTransition : Transition
    {
        private readonly int outermostPrecedenceReturn;
        public EpsilonTransition(ATNState target)
            : this(target, -1)
        {
        }
        public EpsilonTransition(ATNState target, int outermostPrecedenceReturn)
            : base(target)
        {
            this.outermostPrecedenceReturn = outermostPrecedenceReturn;
        }
        public int OutermostPrecedenceReturn
        {
            get
            {
                return outermostPrecedenceReturn;
            }
        }
        public override Antlr4.Runtime.Atn.TransitionType TransitionType
        {
            get
            {
                return Antlr4.Runtime.Atn.TransitionType.EPSILON;
            }
        }
        public override bool IsEpsilon
        {
            get
            {
                return true;
            }
        }
        public override bool Matches(int symbol, int minVocabSymbol, int maxVocabSymbol)
        {
            return false;
        }
    }
}
