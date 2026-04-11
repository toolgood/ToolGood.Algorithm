/* Copyright (c) 2012-2017 The ANTLR Project. All rights reserved.
 * Use of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */
using System.Collections;
using System.Collections.Generic;
namespace Antlr4.Runtime.Dfa
{
    internal abstract class AbstractEdgeMap<T> : IEdgeMap<T>
        where T : class
    {
        protected internal readonly int minIndex;
        protected internal readonly int maxIndex;
        protected AbstractEdgeMap(int minIndex, int maxIndex)
        {
            System.Diagnostics.Debug.Assert(maxIndex - minIndex + 1 >= 0);
            this.minIndex = minIndex;
            this.maxIndex = maxIndex;
        }
        public abstract Antlr4.Runtime.Dfa.AbstractEdgeMap<T> Put(int key, T value);
        public virtual Antlr4.Runtime.Dfa.AbstractEdgeMap<T> PutAll(IEdgeMap<T> m)
        {
            Antlr4.Runtime.Dfa.AbstractEdgeMap<T> result = this;
            foreach (KeyValuePair<int, T> entry in m)
            {
                result = result.Put(entry.Key, entry.Value);
            }
            return result;
        }
        public abstract Antlr4.Runtime.Dfa.AbstractEdgeMap<T> Remove(int key);
        public abstract bool ContainsKey(int arg1);
        public abstract T this[int arg1]
        {
            get;
        }
        public abstract bool IsEmpty
        {
            get;
        }
        public abstract int Count
        {
            get;
        }
        public abstract IReadOnlyDictionary<int, T> ToMap();
        public virtual IEnumerator<KeyValuePair<int, T>> GetEnumerator()
        {
            return ToMap().GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
