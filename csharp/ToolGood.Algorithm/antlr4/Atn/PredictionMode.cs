/* Copyright (c) 2012-2017 The ANTLR Project. All rights reserved.
 * Use of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */
using System.Collections.Generic;
using Antlr4.Runtime.Atn;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Sharpen;
namespace Antlr4.Runtime.Atn
{
    [System.Serializable]
    internal sealed class PredictionMode
    {
        public static readonly PredictionMode SLL = new PredictionMode();
        public static readonly PredictionMode LL = new PredictionMode();
        internal class AltAndContextMap : Dictionary<ATNConfig, BitSet>
        {
            public AltAndContextMap()
                : base(PredictionMode.AltAndContextConfigEqualityComparator.Instance)
            {
            }
        }
        private sealed class AltAndContextConfigEqualityComparator : EqualityComparer<ATNConfig>
        {
            public static readonly PredictionMode.AltAndContextConfigEqualityComparator Instance = new PredictionMode.AltAndContextConfigEqualityComparator();
            private AltAndContextConfigEqualityComparator()
            {
            }
            public override int GetHashCode(ATNConfig o)
            {
                int hashCode = MurmurHash.Initialize(7);
                hashCode = MurmurHash.Update(hashCode, o.state.stateNumber);
                hashCode = MurmurHash.Update(hashCode, o.context);
                hashCode = MurmurHash.Finish(hashCode, 2);
                return hashCode;
            }
            public override bool Equals(ATNConfig a, ATNConfig b)
            {
                if (a == b)
                {
                    return true;
                }
                if (a == null || b == null)
                {
                    return false;
                }
                return a.state.stateNumber == b.state.stateNumber && a.context.Equals(b.context);
            }
        }
		public static bool HasSLLConflictTerminatingPrediction(PredictionMode mode, ATNConfigSet configSet)
        {
			if (AllConfigsInRuleStopStates(configSet.configs))
            {
                return true;
            }
            if (mode == PredictionMode.SLL)
            {
                if (configSet.hasSemanticContext)
                {
                    ATNConfigSet dup = new ATNConfigSet();
					foreach (ATNConfig c in configSet.configs)
                    {
						dup.Add(new ATNConfig(c, SemanticContext.Empty.Instance));
                    }
                    configSet = dup;
                }
            }
			ICollection<BitSet> altsets = GetConflictingAltSubsets(configSet.configs);
			bool heuristic = HasConflictingAltSet(altsets) && !HasStateAssociatedWithOneAlt(configSet.configs);
            return heuristic;
        }
        public static bool HasConfigInRuleStopState(IEnumerable<ATNConfig> configs)
        {
            foreach (ATNConfig c in configs)
            {
                if (c.state is RuleStopState)
                {
                    return true;
                }
            }
            return false;
        }
        public static bool AllConfigsInRuleStopStates(IEnumerable<ATNConfig> configs)
        {
            foreach (ATNConfig config in configs)
            {
                if (!(config.state is RuleStopState))
                {
                    return false;
                }
            }
            return true;
        }
        
        public static int ResolvesToJustOneViableAlt(IEnumerable<BitSet> altsets)
        {
            return GetSingleViableAlt(altsets);
        }
        public static bool HasConflictingAltSet(IEnumerable<BitSet> altsets)
        {
            foreach (BitSet alts in altsets)
            {
                if (alts.Cardinality() > 1)
                {
                    return true;
                }
            }
            return false;
        }
        public static BitSet GetAlts(IEnumerable<BitSet> altsets)
        {
            BitSet all = new BitSet();
            foreach (BitSet alts in altsets)
            {
                all.Or(alts);
            }
            return all;
        }
        public static ICollection<BitSet> GetConflictingAltSubsets(IEnumerable<ATNConfig> configs)
        {
            PredictionMode.AltAndContextMap configToAlts = new PredictionMode.AltAndContextMap();
            foreach (ATNConfig c in configs)
            {
                BitSet alts;
                if (!configToAlts.TryGetValue(c, out alts))
                {
                    alts = new BitSet();
                    configToAlts[c] = alts;
                }
                alts.Set(c.alt);
            }
            return configToAlts.Values;
        }
        public static IDictionary<ATNState, BitSet> GetStateToAltMap(IEnumerable<ATNConfig> configs)
        {
            IDictionary<ATNState, BitSet> m = new Dictionary<ATNState, BitSet>();
            foreach (ATNConfig c in configs)
            {
                BitSet alts;
                if (!m.TryGetValue(c.state, out alts))
                {
                    alts = new BitSet();
                    m[c.state] = alts;
                }
                alts.Set(c.alt);
            }
            return m;
        }
        public static bool HasStateAssociatedWithOneAlt(IEnumerable<ATNConfig> configs)
        {
            IDictionary<ATNState, BitSet> x = GetStateToAltMap(configs);
            foreach (BitSet alts in x.Values)
            {
                if (alts.Cardinality() == 1)
                {
                    return true;
                }
            }
            return false;
        }
        public static int GetSingleViableAlt(IEnumerable<BitSet> altsets)
        {
            BitSet viableAlts = new BitSet();
            foreach (BitSet alts in altsets)
            {
                int minAlt = alts.NextSetBit(0);
                viableAlts.Set(minAlt);
                if (viableAlts.Cardinality() > 1)
                {
                    return ATN.INVALID_ALT_NUMBER;
                }
            }
            return viableAlts.NextSetBit(0);
        }
    }
}
