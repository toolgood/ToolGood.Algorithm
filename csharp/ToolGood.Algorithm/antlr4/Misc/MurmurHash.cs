/* Copyright (c) 2012-2017 The ANTLR Project. All rights reserved.
 * Use of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */
namespace Antlr4.Runtime.Misc
{
    public sealed class MurmurHash
    {
        private const int DefaultSeed = 0;
        private static readonly int C1 = unchecked((int)(0xCC9E2D51));
        private static readonly int C2 = unchecked((int)(0x1B873593));
        private static readonly int M = 5;
        private static readonly int N = unchecked((int)(0xE6546B64));
        private static readonly int Seed1 = unchecked((int)(0x85EBCA6B));
        private static readonly int Seed2 = unchecked((int)(0xC2B2AE35));
        public static int Initialize()
        {
            return DefaultSeed;
        }
        public static int Initialize(int seed)
        {
            return seed;
        }
        public static int Update(int hash, int value)
        {
            int k = value;
            k = k * C1;
            k = (k << 15) | ((int)(((uint)k) >> 17));
            k = k * C2;
            hash = hash ^ k;
            hash = (hash << 13) | ((int)(((uint)hash) >> 19));
            hash = hash * M + N;
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
            hash = hash * Seed1;
            hash = hash ^ ((int)(((uint)hash) >> 13));
            hash = hash * Seed2;
            hash = hash ^ ((int)(((uint)hash) >> 16));
            return hash;
        }
        public static int HashCode<T>(T[] data, int seed)
        {
            int hash = Initialize(seed);
            for (int i = 0; i < data.Length; i++)
            {
                hash = Update(hash, data[i]);
            }
            hash = Finish(hash, data.Length);
            return hash;
        }
        private MurmurHash()
        {
        }
    }
}
