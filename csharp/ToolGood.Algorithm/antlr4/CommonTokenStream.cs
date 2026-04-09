/* Copyright (c) 2012-2017 The ANTLR Project. All rights reserved.
 * Use of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */
using Antlr4.Runtime;
using Antlr4.Runtime.Sharpen;
namespace Antlr4.Runtime
{
    public class CommonTokenStream : BufferedTokenStream
    {
        protected internal int channel = TokenConstants.DefaultChannel;
        public CommonTokenStream(ITokenSource tokenSource)
            : base(tokenSource)
        {
        }
        protected internal override int AdjustSeekIndex(int i)
        {
            return NextTokenOnChannel(i, channel);
        }
        protected internal override IToken Lb(int k)
        {
            if (k == 0 || (p - k) < 0)
            {
                return null;
            }
            int i = p;
            int n = 1;
            while (n <= k)
            {
                i = PreviousTokenOnChannel(i - 1, channel);
                n++;
            }
            if (i < 0)
            {
                return null;
            }
            return tokens[i];
        }
        public override IToken LT(int k)
        {
            LazyInit();
            if (k == 0)
            {
                return null;
            }
            if (k < 0)
            {
                return Lb(-k);
            }
            int i = p;
            int n = 1;
            while (n < k)
            {
                if (Sync(i + 1))
                {
                    i = NextTokenOnChannel(i + 1, channel);
                }
                n++;
            }
            return tokens[i];
        }
    }
}
