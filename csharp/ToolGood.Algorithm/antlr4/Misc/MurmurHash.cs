/* Copyright (c) 2012-2017 The ANTLR Project. All rights reserved.
 * Use of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */
namespace Antlr4.Runtime.Misc
{
    internal sealed class MurmurHash
    {
        private const int DefaultSeed = 0;
        public static int Initialize()
        {
            return Initialize(DefaultSeed);
        }
        public static int Initialize(int seed)
        {
            return seed;
        }
        public static int Update(int hash, int value)
        {
            int c1 = unchecked((int)(0xCC9E2D51));
            int c2 = unchecked((int)(0x1B873593));
            int r1 = 15;
            int r2 = 13;
            int m = 5;
            int n = unchecked((int)(0xE6546B64));
            int k = value;
            k = k * c1;
            k = (k << r1) | ((int)(((uint)k) >> (32 - r1)));
            k = k * c2;
            hash = hash ^ k;
            hash = (hash << r2) | ((int)(((uint)hash) >> (32 - r2)));
            hash = hash * m + n;
            return hash;
        }
        public static int Update(int hash, object value)
        {
            return Update(hash, value != null ? value.GetHashCode() : 0);
        }
        public static int Finish(int hash, int numberOfWords)
        {
            hash = hash ^ (numberOfWords * 4);
            hash = hash ^ ((int)(((uint)hash) >> 16));
            hash = hash * unchecked((int)(0x85EBCA6B));
            hash = hash ^ ((int)(((uint)hash) >> 13));
            hash = hash * unchecked((int)(0xC2B2AE35));
            hash = hash ^ ((int)(((uint)hash) >> 16));
            return hash;
        }
        public static int HashCode<T>(T[] data, int seed)
        {
            int hash = Initialize(seed);
            foreach (T value in data)
            {
                hash = Update(hash, value);
            }
            hash = Finish(hash, data.Length);
            return hash;
        }
        private MurmurHash()
        {
        }
    }
}
