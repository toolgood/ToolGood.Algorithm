/* Copyright (c) 2012-2017 The ANTLR Project. All rights reserved.
 * Use of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Sharpen;
namespace Antlr4.Runtime.Atn
{
	public class ATNConfigSet
	{
		/** Indicates that the set of configurations is read-only. Do not
		 *  allow any code to manipulate the set; DFA states will point at
		 *  the sets and they must not change. This does not protect the other
		 *  fields; in particular, conflictingAlts is set after
		 *  we've made this readonly.
		 */
		protected bool readOnly = false;
		/**
		 * All configs but hashed by (s, i, _, pi) not including context. Wiped out
		 * when we go readonly as this set becomes a DFA state.
		 */
		public ConfigHashSet configLookup;
		/** Track the elements as they are added to the set; supports get(i) */
		public ArrayList<ATNConfig> configs = new ArrayList<ATNConfig>(7);
		public int uniqueAlt;
		/** Currently this is only used when we detect SLL conflict; this does
		 *  not necessarily represent the ambiguous alternatives. In fact,
		 *  I should also point out that this seems to include predicated alternatives
		 *  that have predicates that evaluate to false. Computed in computeTargetState().
		 */
		public BitSet conflictingAlts;
		public bool hasSemanticContext;
		public bool dipsIntoOuterContext;
		/** Indicates that this configuration set is part of a full context
		 *  LL prediction. It will be used to determine how to merge $. With SLL
		 *  it's a wildcard whereas it is not for LL context merge.
		 */
		public readonly bool fullCtx;
		private int cachedHashCode = -1;
		public ATNConfigSet(bool fullCtx)
		{
			configLookup = new ConfigHashSet();
			this.fullCtx = fullCtx;
		}
		public ATNConfigSet()
		: this(true)
		{
		}
		public bool Add(ATNConfig config)
		{
			return Add(config, null);
		}
		/**
		 * Adding a new config means merging contexts with existing configs for
		 * {@code (s, i, pi, _)}, where {@code s} is the
		 * {@link ATNConfig#state}, {@code i} is the {@link ATNConfig#alt}, and
		 * {@code pi} is the {@link ATNConfig#semanticContext}. We use
		 * {@code (s,i,pi)} as key.
		 *
		 * <p>This method updates {@link #dipsIntoOuterContext} and
		 * {@link #hasSemanticContext} when necessary.</p>
		 */
		public bool Add(ATNConfig config, MergeCache mergeCache)
		{
			if (readOnly)
				throw new Exception("This set is readonly");
			if (config.semanticContext != SemanticContext.Empty.Instance)
			{
				hasSemanticContext = true;
			}
			if (config.OuterContextDepth > 0)
			{
				dipsIntoOuterContext = true;
			}
			ATNConfig existing = configLookup.GetOrAdd(config);
			if (existing == config)
			{ 
				cachedHashCode = -1;
				configs.Add(config);  
				return true;
			}
			bool rootIsWildcard = !fullCtx;
			PredictionContext merged = PredictionContext.Merge(existing.context, config.context, rootIsWildcard, mergeCache);
			existing.reachesIntoOuterContext = Math.Max(existing.reachesIntoOuterContext, config.reachesIntoOuterContext);
			if (config.IsPrecedenceFilterSuppressed)
			{
				existing.SetPrecedenceFilterSuppressed(true);
			}
			existing.context = merged; 
			return true;
		}
		/**
		 * Gets the complete set of represented alternatives for the configuration
		 * set.
		 *
		 * @return the set of represented alternatives in this configuration set
		 *
		 * @since 4.3
		 */
		public BitSet GetAlts()
		{
			BitSet alts = new BitSet();
			foreach (ATNConfig config in configs)
			{
				alts.Set(config.alt);
			}
			return alts;
		}
		public void OptimizeConfigs(ATNSimulator interpreter)
		{
			if (readOnly)
				throw new Exception("This set is readonly");
			if (configLookup.Count == 0)
				return;
			foreach (ATNConfig config in configs)
			{
				config.context = interpreter.getCachedContext(config.context);
			}
		}
		public override bool Equals(Object o)
		{
			if (o == this)
			{
				return true;
			}
			else if (!(o is ATNConfigSet))
			{
				return false;
			}
			ATNConfigSet other = (ATNConfigSet)o;
			bool same = configs != null &&
				configs.Equals(other.configs) &&  
				this.fullCtx == other.fullCtx &&
				this.uniqueAlt == other.uniqueAlt &&
				this.conflictingAlts == other.conflictingAlts &&
				this.hasSemanticContext == other.hasSemanticContext &&
				this.dipsIntoOuterContext == other.dipsIntoOuterContext;
			return same;
		}
		public override int GetHashCode()
		{
			if (IsReadOnly)
			{
				if (cachedHashCode == -1)
				{
					cachedHashCode = configs.GetHashCode();
				}
				return cachedHashCode;
			}
			return configs.GetHashCode();
		}
		public int Count
		{
			get
			{
				return configs.Count;
			}
		}
		public bool Empty
		{
			get
			{
				return configs.Count == 0;
			}
		}
		public bool IsReadOnly
		{
			get
			{
				return readOnly;
			}
			set
			{
				this.readOnly = value;
				configLookup = null; 
			}
		}
	}
	public class OrderedATNConfigSet : ATNConfigSet
	{
		public OrderedATNConfigSet()
		{
			this.configLookup = new LexerConfigHashSet();
		}
		public class LexerConfigHashSet : ConfigHashSet
		{
			public LexerConfigHashSet()
				: base(new ObjectEqualityComparator())
			{
			}
		}
	}
	public class ObjectEqualityComparator : IEqualityComparer<ATNConfig>
	{
		public int GetHashCode(ATNConfig o)
		{
			if (o == null)
				return 0;
			else
				return o.GetHashCode();
		}
		public bool Equals(ATNConfig a, ATNConfig b)
		{
			if (a == b) return true;
			if (a == null || b == null) return false;
			return a.Equals(b);
		}
	}
	/**
	* The reason that we need this is because we don't want the hash map to use
	* the standard hash code and equals. We need all configurations with the same
	* {@code (s,i,_,semctx)} to be equal. Unfortunately, this key effectively doubles
	* the number of objects associated with ATNConfigs. The other solution is to
	* use a hash table that lets us specify the equals/hashcode operation.
	*/
	public class ConfigHashSet : Dictionary<ATNConfig, ATNConfig>
	{
		public ConfigHashSet(IEqualityComparer<ATNConfig> comparer)
			: base(comparer)
		{
		}
		public ConfigHashSet()
			: base(new ConfigEqualityComparator())
		{
		}
		public ATNConfig GetOrAdd(ATNConfig config)
		{
			ATNConfig existing;
			if (this.TryGetValue(config, out existing))
				return existing;
			else
			{
				this.Put(config, config);
				return config;
			}
		}
	}
	public class ConfigEqualityComparator : IEqualityComparer<ATNConfig>
	{
		public int GetHashCode(ATNConfig o)
		{
			unchecked
			{
				int hashCode = 7;
				hashCode = hashCode * 31 + o.state.stateNumber;
				hashCode = hashCode * 31 + o.alt;
				hashCode = hashCode * 31 + o.semanticContext.GetHashCode();
				return hashCode;
			}
		}
		public bool Equals(ATNConfig a, ATNConfig b)
		{
			if (ReferenceEquals(a, b)) return true;
			if (a == null || b == null) return false;
			return a.state.stateNumber == b.state.stateNumber
				&& a.alt == b.alt
				&& a.semanticContext.Equals(b.semanticContext);
		}
	}
}
