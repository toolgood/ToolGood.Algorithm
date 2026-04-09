/* Copyright (c) 2012-2017 The ANTLR Project. All rights reserved.
 * Use of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Antlr4.Runtime;
using Antlr4.Runtime.Atn;
using Antlr4.Runtime.Dfa;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Sharpen;
namespace Antlr4.Runtime.Atn
{
    public class ATN
    {
        public const int INVALID_ALT_NUMBER = 0;
        public readonly IList<ATNState> states = new List<ATNState>();
        public readonly IList<DecisionState> decisionToState = new List<DecisionState>();
        public RuleStartState[] ruleToStartState;
        public RuleStopState[] ruleToStopState;
        public readonly ATNType grammarType;
        public readonly int maxTokenType;
        public int[] ruleToTokenType;
        public ILexerAction[] lexerActions;
        public readonly IList<TokensStartState> modeToStartState = new List<TokensStartState>();
		public DFA[] decisionToDFA = Collections.EmptyList<DFA>();
		public DFA[] modeToDFA = Collections.EmptyList<DFA>();
        public ATN(ATNType grammarType, int maxTokenType)
        {
            this.grammarType = grammarType;
            this.maxTokenType = maxTokenType;
        }
        public virtual IntervalSet NextTokens(ATNState s, RuleContext ctx)
        {
            LL1Analyzer anal = new LL1Analyzer(this);
            IntervalSet next = anal.Look(s, ctx);
            return next;
        }
        public virtual IntervalSet NextTokens(ATNState s)
        {
            if (s.nextTokenWithinRule != null)
            {
                return s.nextTokenWithinRule;
            }
			s.nextTokenWithinRule = NextTokens(s, null);
            s.nextTokenWithinRule.SetReadonly(true);
            return s.nextTokenWithinRule;
        }
        public virtual void AddState(ATNState state)
        {
            if (state != null)
            {
                state.atn = this;
                state.stateNumber = states.Count;
            }
            states.Add(state);
        }
        public virtual int DefineDecisionState(DecisionState s)
        {
            decisionToState.Add(s);
            s.decision = decisionToState.Count - 1;
            decisionToDFA = Arrays.CopyOf(decisionToDFA, decisionToState.Count);
            decisionToDFA[decisionToDFA.Length - 1] = new DFA(s, s.decision);
            return s.decision;
        }
        public virtual DecisionState GetDecisionState(int decision)
        {
            if (decisionToState.Count != 0)
            {
                return decisionToState[decision];
            }
            return null;
        }
        public virtual int NumberOfDecisions
        {
            get
            {
                return decisionToState.Count;
            }
        }
        public virtual IntervalSet GetExpectedTokens(int stateNumber, RuleContext context)
        {
            if (stateNumber < 0 || stateNumber >= states.Count)
            {
                throw new ArgumentException("Invalid state number.");
            }
            RuleContext ctx = context;
            ATNState s = states[stateNumber];
            IntervalSet following = NextTokens(s);
            if (!following.Contains(TokenConstants.EPSILON))
            {
                return following;
            }
            IntervalSet expected = new IntervalSet();
            expected.AddAll(following);
            expected.Remove(TokenConstants.EPSILON);
            while (ctx != null && ctx.invokingState >= 0 && following.Contains(TokenConstants.EPSILON))
            {
                ATNState invokingState = states[ctx.invokingState];
                RuleTransition rt = (RuleTransition)invokingState.Transition(0);
                following = NextTokens(rt.followState);
                expected.AddAll(following);
                expected.Remove(TokenConstants.EPSILON);
                ctx = ctx.Parent;
            }
            if (following.Contains(TokenConstants.EPSILON))
            {
                expected.Add(TokenConstants.EOF);
            }
            return expected;
        }
    }
}
