/* Copyright (c) 2012-2017 The ANTLR Project. All rights reserved.
 * Use of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */
using System;
using System.Collections.Generic;

namespace Antlr4.Runtime.Atn
{
	public class MergeCache
	{
		private struct CacheKey : IEquatable<CacheKey>
		{
			public readonly PredictionContext A;
			public readonly PredictionContext B;

			public CacheKey(PredictionContext a, PredictionContext b)
			{
				A = a;
				B = b;
			}

			public bool Equals(CacheKey other)
			{
				return ReferenceEquals(A, other.A) && ReferenceEquals(B, other.B);
			}

			public override int GetHashCode()
			{
				unchecked
				{
					return (A?.GetHashCode() ?? 0) * 31 + (B?.GetHashCode() ?? 0);
				}
			}
		}

		private readonly Dictionary<CacheKey, PredictionContext> _cache = new Dictionary<CacheKey, PredictionContext>();

		public PredictionContext Get(PredictionContext a, PredictionContext b)
		{
			var key = new CacheKey(a, b);
			PredictionContext value;
			if (_cache.TryGetValue(key, out value))
				return value;
			return null;
		}

		public void Put(PredictionContext a, PredictionContext b, PredictionContext value)
		{
			var key = new CacheKey(a, b);
			_cache[key] = value;
		}
	}
}
