/* Copyright (c) 2012-2017 The ANTLR Project. All rights reserved.
 * Use of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */
using Antlr4.Runtime.Atn;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Sharpen;
namespace Antlr4.Runtime.Atn
{
    public sealed class PredicateTransition : AbstractPredicateTransition
    {
        public readonly int ruleIndex;
        public readonly int predIndex;
        public readonly bool isCtxDependent;
        public PredicateTransition(ATNState target, int ruleIndex, int predIndex, bool isCtxDependent)
            : base(target)
        {
            this.ruleIndex = ruleIndex;
            this.predIndex = predIndex;
            this.isCtxDependent = isCtxDependent;
        }
        public override Antlr4.Runtime.Atn.TransitionType TransitionType
        {
            get
            {
                return Antlr4.Runtime.Atn.TransitionType.PREDICATE;
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
        public SemanticContext.Predicate Predicate
        {
            get
            {
                return new SemanticContext.Predicate(ruleIndex, predIndex, isCtxDependent);
            }
        }
    }
}
