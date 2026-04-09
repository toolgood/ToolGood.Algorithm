/* Copyright (c) 2012-2017 The ANTLR Project. All rights reserved.
 * Use of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Sharpen;
namespace Antlr4.Runtime.Atn
{
    public abstract class SemanticContext
    {
        public abstract bool Eval<Symbol, ATNInterpreter>(Recognizer<Symbol, ATNInterpreter> parser, RuleContext parserCallStack)
            where ATNInterpreter : ATNSimulator;
		public virtual SemanticContext EvalPrecedence<Symbol, ATNInterpreter>(Recognizer<Symbol, ATNInterpreter> parser, RuleContext parserCallStack)
            where ATNInterpreter : ATNSimulator
        {
            return this;
        }
        public class Empty : SemanticContext
        {
            public static readonly SemanticContext Instance = new Empty();
            public override bool Eval<Symbol, ATNInterpreter>(Recognizer<Symbol, ATNInterpreter> parser, RuleContext parserCallStack)
            {
                return false;
            }
        }
        public class Predicate : SemanticContext
        {
            public readonly int ruleIndex;
            public readonly int predIndex;
            public readonly bool isCtxDependent;
            public Predicate(int ruleIndex, int predIndex, bool isCtxDependent)
            {
                this.ruleIndex = ruleIndex;
                this.predIndex = predIndex;
                this.isCtxDependent = isCtxDependent;
            }
            public override bool Eval<Symbol, ATNInterpreter>(Recognizer<Symbol, ATNInterpreter> parser, RuleContext parserCallStack)
            {
                RuleContext localctx = isCtxDependent ? parserCallStack : null;
                return parser.Sempred(localctx, ruleIndex, predIndex);
            }
            public override int GetHashCode()
            {
                int hashCode = MurmurHash.Initialize();
                hashCode = MurmurHash.Update(hashCode, ruleIndex);
                hashCode = MurmurHash.Update(hashCode, predIndex);
                hashCode = MurmurHash.Update(hashCode, isCtxDependent ? 1 : 0);
                hashCode = MurmurHash.Finish(hashCode, 3);
                return hashCode;
            }
            public override bool Equals(object obj)
            {
                if (!(obj is SemanticContext.Predicate))
                {
                    return false;
                }
                if (this == obj)
                {
                    return true;
                }
                SemanticContext.Predicate p = (SemanticContext.Predicate)obj;
                return this.ruleIndex == p.ruleIndex && this.predIndex == p.predIndex && this.isCtxDependent == p.isCtxDependent;
            }
        }
        public class PrecedencePredicate : SemanticContext, IComparable<SemanticContext.PrecedencePredicate>
        {
            public readonly int precedence;
            public PrecedencePredicate(int precedence)
            {
                this.precedence = precedence;
            }
            public override bool Eval<Symbol, ATNInterpreter>(Recognizer<Symbol, ATNInterpreter> parser, RuleContext parserCallStack)
            {
                return parser.Precpred(parserCallStack, precedence);
            }
            public override SemanticContext EvalPrecedence<Symbol, ATNInterpreter>(Recognizer<Symbol, ATNInterpreter> parser, RuleContext parserCallStack)
            {
                if (parser.Precpred(parserCallStack, precedence))
                {
                    return SemanticContext.Empty.Instance;
                }
                else
                {
                    return null;
                }
            }
            public virtual int CompareTo(SemanticContext.PrecedencePredicate o)
            {
                return precedence - o.precedence;
            }
            public override int GetHashCode()
            {
                int hashCode = 1;
                hashCode = 31 * hashCode + precedence;
                return hashCode;
            }
            public override bool Equals(object obj)
            {
                if (!(obj is SemanticContext.PrecedencePredicate))
                {
                    return false;
                }
                if (this == obj)
                {
                    return true;
                }
                SemanticContext.PrecedencePredicate other = (SemanticContext.PrecedencePredicate)obj;
                return this.precedence == other.precedence;
            }
        }
        public abstract class Operator : SemanticContext
        {
            public abstract ICollection<SemanticContext> Operands
            {
                get;
            }
        }
        public class AND : SemanticContext.Operator
        {
            public readonly SemanticContext[] opnds;
            public AND(SemanticContext a, SemanticContext b)
            {
                HashSet<SemanticContext> operands = new HashSet<SemanticContext>();
                if (a is SemanticContext.AND)
                {
                    operands.UnionWith(((AND)a).opnds);
                }
                else
                {
                    operands.Add(a);
                }
                if (b is SemanticContext.AND)
                {
                    operands.UnionWith(((AND)b).opnds);
                }
                else
                {
                    operands.Add(b);
                }
                IList<SemanticContext.PrecedencePredicate> precedencePredicates = FilterPrecedencePredicates(operands);
                if (precedencePredicates.Count > 0)
                {
                    SemanticContext.PrecedencePredicate reduced = precedencePredicates.Min();
                    operands.Add(reduced);
                }
                opnds = operands.ToArray();
            }
            public override ICollection<SemanticContext> Operands
            {
                get
                {
                    return Arrays.AsList(opnds);
                }
            }
            public override bool Equals(object obj)
            {
                if (this == obj)
                {
                    return true;
                }
                if (!(obj is SemanticContext.AND))
                {
                    return false;
                }
                SemanticContext.AND other = (SemanticContext.AND)obj;
                return Arrays.Equals(this.opnds, other.opnds);
            }
            public override int GetHashCode()
            {
                return MurmurHash.HashCode(opnds, typeof(SemanticContext.AND).GetHashCode());
            }
            public override bool Eval<Symbol, ATNInterpreter>(Recognizer<Symbol, ATNInterpreter> parser, RuleContext parserCallStack)
            {
                foreach (SemanticContext opnd in opnds)
                {
                    if (!opnd.Eval(parser, parserCallStack))
                    {
                        return false;
                    }
                }
                return true;
            }
            public override SemanticContext EvalPrecedence<Symbol, ATNInterpreter>(Recognizer<Symbol, ATNInterpreter> parser, RuleContext parserCallStack)
            {
                bool differs = false;
                IList<SemanticContext> operands = new List<SemanticContext>();
                foreach (SemanticContext context in opnds)
                {
                    SemanticContext evaluated = context.EvalPrecedence(parser, parserCallStack);
                    differs |= (evaluated != context);
                    if (evaluated == null)
                    {
                        return null;
                    }
                    else
                    {
                        if (evaluated != Empty.Instance)
                        {
                            operands.Add(evaluated);
                        }
                    }
                }
                if (!differs)
                {
                    return this;
                }
                if (operands.Count == 0)
                {
                    return Empty.Instance;
                }
                SemanticContext result = operands[0];
                for (int i = 1; i < operands.Count; i++)
                {
                    result = SemanticContext.AndOp(result, operands[i]);
                }
                return result;
            }
        }
        public class OR : SemanticContext.Operator
        {
            public readonly SemanticContext[] opnds;
            public OR(SemanticContext a, SemanticContext b)
            {
                HashSet<SemanticContext> operands = new HashSet<SemanticContext>();
                if (a is SemanticContext.OR)
                {
                    operands.UnionWith(((OR)a).opnds);
                }
                else
                {
                    operands.Add(a);
                }
                if (b is SemanticContext.OR)
                {
                    operands.UnionWith(((OR)b).opnds);
                }
                else
                {
                    operands.Add(b);
                }
                IList<SemanticContext.PrecedencePredicate> precedencePredicates = FilterPrecedencePredicates(operands);
                if (precedencePredicates.Count > 0)
                {
                    SemanticContext.PrecedencePredicate reduced = precedencePredicates.Max();
                    operands.Add(reduced);
                }
                this.opnds = operands.ToArray();
            }
            public override ICollection<SemanticContext> Operands
            {
                get
                {
                    return Arrays.AsList(opnds);
                }
            }
            public override bool Equals(object obj)
            {
                if (this == obj)
                {
                    return true;
                }
                if (!(obj is SemanticContext.OR))
                {
                    return false;
                }
                SemanticContext.OR other = (SemanticContext.OR)obj;
                return Arrays.Equals(this.opnds, other.opnds);
            }
            public override int GetHashCode()
            {
                return MurmurHash.HashCode(opnds, typeof(SemanticContext.OR).GetHashCode());
            }
            public override bool Eval<Symbol, ATNInterpreter>(Recognizer<Symbol, ATNInterpreter> parser, RuleContext parserCallStack)
            {
                foreach (SemanticContext opnd in opnds)
                {
                    if (opnd.Eval(parser, parserCallStack))
                    {
                        return true;
                    }
                }
                return false;
            }
            public override SemanticContext EvalPrecedence<Symbol, ATNInterpreter>(Recognizer<Symbol, ATNInterpreter> parser, RuleContext parserCallStack)
            {
                bool differs = false;
                IList<SemanticContext> operands = new List<SemanticContext>();
                foreach (SemanticContext context in opnds)
                {
                    SemanticContext evaluated = context.EvalPrecedence(parser, parserCallStack);
                    differs |= (evaluated != context);
                    if (evaluated == Empty.Instance)
                    {
                        return Empty.Instance;
                    }
                    else
                    {
                        if (evaluated != null)
                        {
                            operands.Add(evaluated);
                        }
                    }
                }
                if (!differs)
                {
                    return this;
                }
                if (operands.Count == 0)
                {
                    return null;
                }
                SemanticContext result = operands[0];
                for (int i = 1; i < operands.Count; i++)
                {
                    result = SemanticContext.OrOp(result, operands[i]);
                }
                return result;
            }
        }
        public static SemanticContext AndOp(SemanticContext a, SemanticContext b)
        {
            if (a == null || a == Empty.Instance)
            {
                return b;
            }
            if (b == null || b == Empty.Instance)
            {
                return a;
            }
            SemanticContext.AND result = new SemanticContext.AND(a, b);
            if (result.opnds.Length == 1)
            {
                return result.opnds[0];
            }
            return result;
        }
        public static SemanticContext OrOp(SemanticContext a, SemanticContext b)
        {
            if (a == null)
            {
                return b;
            }
            if (b == null)
            {
                return a;
            }
            if (a == Empty.Instance || b == Empty.Instance)
            {
                return Empty.Instance;
            }
            SemanticContext.OR result = new SemanticContext.OR(a, b);
            if (result.opnds.Length == 1)
            {
                return result.opnds[0];
            }
            return result;
        }
        private static IList<SemanticContext.PrecedencePredicate> FilterPrecedencePredicates(HashSet<SemanticContext> collection)
        {
            if (!collection.OfType<PrecedencePredicate>().Any())
                Collections.EmptyList<PrecedencePredicate>();
            List<PrecedencePredicate> result = collection.OfType<PrecedencePredicate>().ToList();
            collection.ExceptWith(result);
            return result;
        }
    }
}
