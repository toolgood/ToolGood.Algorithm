/* Copyright (c) 2012-2017 The ANTLR Project. All rights reserved.
 * Use of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */
using Antlr4.Runtime.Atn;
using Antlr4.Runtime.Sharpen;
namespace Antlr4.Runtime.Atn
{
    public sealed class ActionTransition : Transition
    {
        public readonly int actionIndex;
        public ActionTransition(ATNState target, int ruleIndex, int actionIndex, bool isCtxDependent)
            : base(target)
        {
            this.actionIndex = actionIndex;
        }
        public override Antlr4.Runtime.Atn.TransitionType TransitionType
        {
            get
            {
                return Antlr4.Runtime.Atn.TransitionType.ACTION;
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
