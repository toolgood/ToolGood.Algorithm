/* Copyright (c) 2012-2017 The ANTLR Project. All rights reserved.
 * Use of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */
using System;
namespace Antlr4.Runtime.Misc
{
    public struct Interval
    {
        public static readonly Antlr4.Runtime.Misc.Interval Invalid = new Antlr4.Runtime.Misc.Interval(-1, -2);
        public readonly int a;
        public readonly int b;
        public Interval(int a, int b)
        {
            this.a = a;
            this.b = b;
        }
        public static Antlr4.Runtime.Misc.Interval Of(int a, int b)
        {
            return new Antlr4.Runtime.Misc.Interval(a, b);
        }
        public override bool Equals(object o)
        {
            if (!(o is Antlr4.Runtime.Misc.Interval))
            {
                return false;
            }
            Antlr4.Runtime.Misc.Interval other = (Antlr4.Runtime.Misc.Interval)o;
            return this.a == other.a && this.b == other.b;
        }
        public override int GetHashCode()
        {
            int hash = 23;
            hash = hash * 31 + a;
            hash = hash * 31 + b;
            return hash;
        }
        public bool StartsBeforeDisjoint(Antlr4.Runtime.Misc.Interval other)
        {
            return this.a < other.a && this.b < other.a;
        }
        public bool StartsAfterDisjoint(Antlr4.Runtime.Misc.Interval other)
        {
            return this.a > other.b;
        }
        public bool StartsAfterNonDisjoint(Antlr4.Runtime.Misc.Interval other)
        {
            return this.a > other.a && this.a <= other.b;
        }
        public bool Disjoint(Antlr4.Runtime.Misc.Interval other)
        {
            return StartsBeforeDisjoint(other) || StartsAfterDisjoint(other);
        }
        public bool Adjacent(Antlr4.Runtime.Misc.Interval other)
        {
            return this.a == other.b + 1 || this.b == other.a - 1;
        }
        public bool ProperlyContains(Antlr4.Runtime.Misc.Interval other)
        {
            return other.a >= this.a && other.b <= this.b;
        }
        public Antlr4.Runtime.Misc.Interval Union(Antlr4.Runtime.Misc.Interval other)
        {
            return Antlr4.Runtime.Misc.Interval.Of(Math.Min(a, other.a), Math.Max(b, other.b));
        }
        public Antlr4.Runtime.Misc.Interval Intersection(Antlr4.Runtime.Misc.Interval other)
        {
            return Antlr4.Runtime.Misc.Interval.Of(Math.Max(a, other.a), Math.Min(b, other.b));
        }
    }
}
