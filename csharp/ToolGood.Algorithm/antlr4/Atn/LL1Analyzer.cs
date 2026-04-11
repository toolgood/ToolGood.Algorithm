using System.Collections.Generic;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Sharpen;
namespace Antlr4.Runtime.Atn
{
    internal class LL1Analyzer
    {
        public const int HitPred = TokenConstants.InvalidType;
        public readonly ATN atn;
        public LL1Analyzer(ATN atn)
        {
            this.atn = atn;
        }
        public virtual IntervalSet Look(ATNState s, RuleContext ctx)
        {
            return Look(s, null, ctx);
        }
        public virtual IntervalSet Look(ATNState s, ATNState stopState, RuleContext ctx)
        {
            IntervalSet r = new IntervalSet();
            bool seeThruPreds = true;
            PredictionContext lookContext = ctx != null ? PredictionContext.FromRuleContext(s.atn, ctx) : null;
            Look_(s, stopState, lookContext, r, new HashSet<ATNConfig>(), new BitSet(), seeThruPreds, true);
            return r;
        }
       
        protected internal virtual void Look_(ATNState s, ATNState stopState, PredictionContext ctx, IntervalSet look, HashSet<ATNConfig> lookBusy, BitSet calledRuleStack, bool seeThruPreds, bool addEOF)
        {
            ATNConfig c = new ATNConfig(s, 0, ctx);
            if (!lookBusy.Add(c))
            {
                return;
            }
            if (s == stopState)
            {
                if (ctx == null)
                {
                    look.Add(TokenConstants.EPSILON);
                    return;
                }
                else if (ctx.IsEmpty && addEOF)
                {
                    look.Add(TokenConstants.EOF);
                    return;
                }
            }
            if (s is RuleStopState)
            {
                if (ctx == null)
                {
                    look.Add(TokenConstants.EPSILON);
                    return;
                }
                else if (ctx.IsEmpty && addEOF)
                {
                    look.Add(TokenConstants.EOF);
                    return;
                }
                if (ctx != EmptyPredictionContext.Instance)
                {
                    bool removed = calledRuleStack.Get(s.ruleIndex);
                    try
                    {
                        calledRuleStack.Clear(s.ruleIndex);
                        for (int i = 0; i < ctx.Size; i++)
                        {
                            ATNState returnState = atn.states[ctx.GetReturnState(i)];
                            Look_(returnState, stopState, ctx.GetParent(i), look, lookBusy, calledRuleStack, seeThruPreds, addEOF);
                        }
                    }
                    finally
                    {
                        if (removed)
                        {
                            calledRuleStack.Set(s.ruleIndex);
                        }
                    }
                    return;
                }
            }
            int n = s.NumberOfTransitions;
            for (int i_1 = 0; i_1 < n; i_1++)
            {
                Transition t = s.Transition(i_1);
                if (t.GetType() == typeof(RuleTransition))
                {
                    RuleTransition ruleTransition = (RuleTransition)t;
                    if (calledRuleStack.Get(ruleTransition.ruleIndex))
                    {
                        continue;
                    }
                    PredictionContext newContext = SingletonPredictionContext.Create(ctx, ruleTransition.followState.stateNumber);
                    try
                    {
                        calledRuleStack.Set(ruleTransition.target.ruleIndex);
                        Look_(t.target, stopState, newContext, look, lookBusy, calledRuleStack, seeThruPreds, addEOF);
                    }
                    finally
                    {
                        calledRuleStack.Clear(ruleTransition.target.ruleIndex);
                    }
                }
                else if (t is AbstractPredicateTransition)
                {
                    if (seeThruPreds)
                    {
                        Look_(t.target, stopState, ctx, look, lookBusy, calledRuleStack, seeThruPreds, addEOF);
                    }
                    else
                    {
                        look.Add(HitPred);
                    }
                }
                else if (t.IsEpsilon)
                {
                    Look_(t.target, stopState, ctx, look, lookBusy, calledRuleStack, seeThruPreds, addEOF);
                }
                else if (t.GetType() == typeof(WildcardTransition))
                {
                    look.AddAll(IntervalSet.Of(TokenConstants.MinUserTokenType, atn.maxTokenType));
                }
                else
                {
                    IntervalSet set = t.Label;
                    if (set != null)
                    {
                        if (t is NotSetTransition)
                        {
                            set = set.Complement(IntervalSet.Of(TokenConstants.MinUserTokenType, atn.maxTokenType));
                        }
                        look.AddAll(set);
                    }
                }
            }
        }
    }
}
