/* Copyright (c) 2012-2017 The ANTLR Project. All rights reserved.
 * Use of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Antlr4.Runtime.Misc
{
    public class IntervalSet : IIntSet
    {
        public static readonly Antlr4.Runtime.Misc.IntervalSet CompleteCharSet = Antlr4.Runtime.Misc.IntervalSet.Of(Lexer.MinCharValue, Lexer.MaxCharValue);
        public static readonly Antlr4.Runtime.Misc.IntervalSet EmptySet = new Antlr4.Runtime.Misc.IntervalSet();
        static IntervalSet()
        {
            CompleteCharSet.SetReadonly(true);
            EmptySet.SetReadonly(true);
        }
        protected internal IList<Interval> intervals;
        protected internal bool @readonly;
        public IntervalSet(Antlr4.Runtime.Misc.IntervalSet set)
            : this()
        {
            AddAll(set);
        }
        public IntervalSet(params int[] els)
        {
            if (els == null)
            {
                intervals = new ArrayList<Interval>(2);
            }
            else
            {
                intervals = new ArrayList<Interval>(els.Length);
                foreach (int e in els)
                {
                    Add(e);
                }
            }
        }
        public static Antlr4.Runtime.Misc.IntervalSet Of(int a)
        {
            Antlr4.Runtime.Misc.IntervalSet s = new Antlr4.Runtime.Misc.IntervalSet();
            s.Add(a);
            return s;
        }
        public static Antlr4.Runtime.Misc.IntervalSet Of(int a, int b)
        {
            Antlr4.Runtime.Misc.IntervalSet s = new Antlr4.Runtime.Misc.IntervalSet();
            s.Add(a, b);
            return s;
        }
        public virtual void Add(int el)
        {
            if (@readonly)
            {
                throw new InvalidOperationException("can't alter readonly IntervalSet");
            }
            Add(el, el);
        }
        public virtual void Add(int a, int b)
        {
            Add(Interval.Of(a, b));
        }
        protected internal virtual void Add(Interval addition)
        {
            if (@readonly)
            {
                throw new InvalidOperationException("can't alter readonly IntervalSet");
            }
            if (addition.b < addition.a)
            {
                return;
            }
            for (int i = 0; i < intervals.Count; i++)
            {
                Interval r = intervals[i];
                if (addition.Equals(r))
                {
                    return;
                }
                if (addition.Adjacent(r) || !addition.Disjoint(r))
                {
                    Interval bigger = addition.Union(r);
                    intervals[i] = bigger;
                    while (i < intervals.Count - 1)
                    {
                        i++;
                        Interval next = intervals[i];
                        if (!bigger.Adjacent(next) && bigger.Disjoint(next))
                        {
                            break;
                        }
                        intervals.RemoveAt(i);
                        i--;
                        intervals[i] = bigger.Union(next);
                    }
                    return;
                }
                if (addition.StartsBeforeDisjoint(r))
                {
                    intervals.Insert(i, addition);
                    return;
                }
            }
            intervals.Add(addition);
        }
        public virtual Antlr4.Runtime.Misc.IntervalSet AddAll(IIntSet set)
        {
            if (set == null)
            {
                return this;
            }
            if (set is Antlr4.Runtime.Misc.IntervalSet)
            {
                Antlr4.Runtime.Misc.IntervalSet other = (Antlr4.Runtime.Misc.IntervalSet)set;
                int n = other.intervals.Count;
                for (int i = 0; i < n; i++)
                {
                    Interval I = other.intervals[i];
                    this.Add(I.a, I.b);
                }
            }
            else
            {
                foreach (int value in set.ToList())
                {
                    Add(value);
                }
            }
            return this;
        }
        public virtual Antlr4.Runtime.Misc.IntervalSet Complement(IIntSet vocabulary)
        {
            if (vocabulary == null || vocabulary.IsNil)
            {
                return null;
            }
            Antlr4.Runtime.Misc.IntervalSet vocabularyIS;
            if (vocabulary is Antlr4.Runtime.Misc.IntervalSet)
            {
                vocabularyIS = (Antlr4.Runtime.Misc.IntervalSet)vocabulary;
            }
            else
            {
                vocabularyIS = new Antlr4.Runtime.Misc.IntervalSet();
                vocabularyIS.AddAll(vocabulary);
            }
            return vocabularyIS.Subtract(this);
        }
        public virtual Antlr4.Runtime.Misc.IntervalSet Subtract(IIntSet a)
        {
            if (a == null || a.IsNil)
            {
                return new Antlr4.Runtime.Misc.IntervalSet(this);
            }
            if (a is Antlr4.Runtime.Misc.IntervalSet)
            {
                return Subtract(this, (Antlr4.Runtime.Misc.IntervalSet)a);
            }
            Antlr4.Runtime.Misc.IntervalSet other = new Antlr4.Runtime.Misc.IntervalSet();
            other.AddAll(a);
            return Subtract(this, other);
        }
        public static Antlr4.Runtime.Misc.IntervalSet Subtract(Antlr4.Runtime.Misc.IntervalSet left, Antlr4.Runtime.Misc.IntervalSet right)
        {
            if (left == null || left.IsNil)
            {
                return new Antlr4.Runtime.Misc.IntervalSet();
            }
            Antlr4.Runtime.Misc.IntervalSet result = new Antlr4.Runtime.Misc.IntervalSet(left);
            if (right == null || right.IsNil)
            {
                return result;
            }
            int resultI = 0;
            int rightI = 0;
            while (resultI < result.intervals.Count && rightI < right.intervals.Count)
            {
                Interval resultInterval = result.intervals[resultI];
                Interval rightInterval = right.intervals[rightI];
                if (rightInterval.b < resultInterval.a)
                {
                    rightI++;
                    continue;
                }
                if (rightInterval.a > resultInterval.b)
                {
                    resultI++;
                    continue;
                }
                Interval? beforeCurrent = null;
                Interval? afterCurrent = null;
                if (rightInterval.a > resultInterval.a)
                {
                    beforeCurrent = new Interval(resultInterval.a, rightInterval.a - 1);
                }
                if (rightInterval.b < resultInterval.b)
                {
                    afterCurrent = new Interval(rightInterval.b + 1, resultInterval.b);
                }
                if (beforeCurrent != null)
                {
                    if (afterCurrent != null)
                    {
                        result.intervals[resultI] = beforeCurrent.Value;
                        result.intervals.Insert(resultI + 1, afterCurrent.Value);
                        resultI++;
                        rightI++;
                        continue;
                    }
                    else
                    {
                        result.intervals[resultI] = beforeCurrent.Value;
                        resultI++;
                        continue;
                    }
                }
                else
                {
                    if (afterCurrent != null)
                    {
                        result.intervals[resultI] = afterCurrent.Value;
                        rightI++;
                        continue;
                    }
                    else
                    {
                        result.intervals.RemoveAt(resultI);
                        continue;
                    }
                }
            }
            return result;
        }
        public virtual Antlr4.Runtime.Misc.IntervalSet Or(IIntSet a)
        {
            Antlr4.Runtime.Misc.IntervalSet o = new Antlr4.Runtime.Misc.IntervalSet();
            o.AddAll(this);
            o.AddAll(a);
            return o;
        }
        public virtual Antlr4.Runtime.Misc.IntervalSet And(IIntSet other)
        {
            if (other == null)
            {
                return null;
            }
            IList<Interval> myIntervals = this.intervals;
            IList<Interval> theirIntervals = ((Antlr4.Runtime.Misc.IntervalSet)other).intervals;
            Antlr4.Runtime.Misc.IntervalSet intersection = null;
            int mySize = myIntervals.Count;
            int theirSize = theirIntervals.Count;
            int i = 0;
            int j = 0;
            while (i < mySize && j < theirSize)
            {
                Interval mine = myIntervals[i];
                Interval theirs = theirIntervals[j];
                if (mine.StartsBeforeDisjoint(theirs))
                {
                    i++;
                }
                else
                {
                    if (theirs.StartsBeforeDisjoint(mine))
                    {
                        j++;
                    }
                    else
                    {
                        if (mine.ProperlyContains(theirs))
                        {
                            if (intersection == null)
                            {
                                intersection = new Antlr4.Runtime.Misc.IntervalSet();
                            }
                            intersection.Add(mine.Intersection(theirs));
                            j++;
                        }
                        else
                        {
                            if (theirs.ProperlyContains(mine))
                            {
                                if (intersection == null)
                                {
                                    intersection = new Antlr4.Runtime.Misc.IntervalSet();
                                }
                                intersection.Add(mine.Intersection(theirs));
                                i++;
                            }
                            else
                            {
                                if (!mine.Disjoint(theirs))
                                {
                                    if (intersection == null)
                                    {
                                        intersection = new Antlr4.Runtime.Misc.IntervalSet();
                                    }
                                    intersection.Add(mine.Intersection(theirs));
                                    if (mine.StartsAfterNonDisjoint(theirs))
                                    {
                                        j++;
                                    }
                                    else
                                    {
                                        if (theirs.StartsAfterNonDisjoint(mine))
                                        {
                                            i++;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (intersection == null)
            {
                return new Antlr4.Runtime.Misc.IntervalSet();
            }
            return intersection;
        }
        public virtual bool Contains(int el)
        {
            int n = intervals.Count;
            for (int i = 0; i < n; i++)
            {
                Interval I = intervals[i];
                int a = I.a;
                int b = I.b;
                if (el < a)
                {
                    break;
                }
                if (el >= a && el <= b)
                {
                    return true;
                }
            }
            return false;
        }
        public virtual bool IsNil
        {
            get
            {
                return intervals == null || intervals.Count == 0;
            }
        }
        public virtual int SingleElement
        {
            get
            {
                if (intervals != null && intervals.Count == 1)
                {
                    Interval I = intervals[0];
                    if (I.a == I.b)
                    {
                        return I.a;
                    }
                }
                return TokenConstants.InvalidType;
            }
        }
        public virtual int MinElement
        {
            get
            {
                if (IsNil)
                {
                    return TokenConstants.InvalidType;
                }
                return intervals[0].a;
            }
        }
        public virtual IList<Interval> GetIntervals()
        {
            return intervals;
        }
        public override int GetHashCode()
        {
            int hash = MurmurHash.Initialize();
            foreach (Interval I in intervals)
            {
                hash = MurmurHash.Update(hash, I.a);
                hash = MurmurHash.Update(hash, I.b);
            }
            hash = MurmurHash.Finish(hash, intervals.Count * 2);
            return hash;
        }
        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Antlr4.Runtime.Misc.IntervalSet))
            {
                return false;
            }
            Antlr4.Runtime.Misc.IntervalSet other = (Antlr4.Runtime.Misc.IntervalSet)obj;
            return this.intervals.SequenceEqual(other.intervals);
        }
        public virtual string ToString(bool elemAreChar)
        {
            StringBuilder buf = new StringBuilder();
            if (this.intervals == null || this.intervals.Count == 0)
            {
                return "{}";
            }
            if (this.Count > 1)
            {
                buf.Append("{");
            }
            bool first = true;
            foreach (Interval I in intervals)
            {
                if (!first)
                    buf.Append(", ");
                first = false;
                int a = I.a;
                int b = I.b;
                if (a == b)
                {
                    if (a == TokenConstants.EOF)
                    {
                        buf.Append("<EOF>");
                    }
                    else
                    {
                        if (elemAreChar)
                        {
                            buf.Append("'").Append((char)a).Append("'");
                        }
                        else
                        {
                            buf.Append(a);
                        }
                    }
                }
                else
                {
                    if (elemAreChar)
                    {
                        buf.Append("'").Append((char)a).Append("'..'").Append((char)b).Append("'");
                    }
                    else
                    {
                        buf.Append(a).Append("..").Append(b);
                    }
                }
            }
            if (this.Count > 1)
            {
                buf.Append("}");
            }
            return buf.ToString();
        }
        public virtual string ToString(IVocabulary vocabulary)
        {
            StringBuilder buf = new StringBuilder();
            if (this.intervals == null || this.intervals.Count == 0)
            {
                return "{}";
            }
            if (this.Count > 1)
            {
                buf.Append("{");
            }
            bool first = true;
            foreach (Interval I in intervals)
            {
                if (!first)
                    buf.Append(", ");
                first = false;
                int a = I.a;
                int b = I.b;
                if (a == b)
                {
                    buf.Append(ElementName(vocabulary, a));
                }
                else
                {
                    for (int i = a; i <= b; i++)
                    {
                        if (i > a)
                        {
                            buf.Append(", ");
                        }
                        buf.Append(ElementName(vocabulary, i));
                    }
                }
            }
            if (this.Count > 1)
            {
                buf.Append("}");
            }
            return buf.ToString();
        }
        protected internal virtual string ElementName(IVocabulary vocabulary, int a)
        {
            if (a == TokenConstants.EOF)
            {
                return "<EOF>";
            }
            else
            {
                if (a == TokenConstants.EPSILON)
                {
                    return "<EPSILON>";
                }
                else
                {
                    return vocabulary.GetDisplayName(a);
                }
            }
        }
        public virtual int Count
        {
            get
            {
                int n = 0;
                int numIntervals = intervals.Count;
                if (numIntervals == 1)
                {
                    Interval firstInterval = this.intervals[0];
                    return firstInterval.b - firstInterval.a + 1;
                }
                for (int i = 0; i < numIntervals; i++)
                {
                    Interval I = intervals[i];
                    n += (I.b - I.a + 1);
                }
                return n;
            }
        }
        public virtual IList<int> ToList()
        {
            IList<int> values = new ArrayList<int>();
            int n = intervals.Count;
            for (int i = 0; i < n; i++)
            {
                Interval I = intervals[i];
                int a = I.a;
                int b = I.b;
                for (int v = a; v <= b; v++)
                {
                    values.Add(v);
                }
            }
            return values;
        }
        public virtual void Remove(int el)
        {
            if (@readonly)
            {
                throw new InvalidOperationException("can't alter readonly IntervalSet");
            }
            int n = intervals.Count;
            for (int i = 0; i < n; i++)
            {
                Interval I = intervals[i];
                int a = I.a;
                int b = I.b;
                if (el < a)
                {
                    break;
                }
                if (el == a && el == b)
                {
                    intervals.RemoveAt(i);
                    break;
                }
                if (el == a)
                {
                    intervals[i] = Interval.Of(I.a + 1, I.b);
                    break;
                }
                if (el == b)
                {
                    intervals[i] = Interval.Of(I.a, I.b - 1);
                    break;
                }
                if (el > a && el < b)
                {
                    int oldb = I.b;
                    intervals[i] = Interval.Of(I.a, el - 1);
                    Add(el + 1, oldb);
                }
            }
        }
        public virtual void SetReadonly(bool @readonly)
        {
            if (this.@readonly && !@readonly)
            {
                throw new InvalidOperationException("can't alter readonly IntervalSet");
            }
            this.@readonly = @readonly;
        }
        IIntSet IIntSet.AddAll(IIntSet set)
        {
            return AddAll(set);
        }
        IIntSet IIntSet.And(IIntSet a)
        {
            return And(a);
        }
        IIntSet IIntSet.Complement(IIntSet elements)
        {
            return Complement(elements);
        }
        IIntSet IIntSet.Or(IIntSet a)
        {
            return Or(a);
        }
        IIntSet IIntSet.Subtract(IIntSet a)
        {
            return Subtract(a);
        }
    }
}
