/* Copyright (c) 2012-2017 The ANTLR Project. All rights reserved.
 * Use of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Sharpen;
namespace Antlr4.Runtime
{
    internal interface ITokenStream : IIntStream
    {
        IToken LT(int k);
        IToken Get(int i);
        ITokenSource TokenSource
        {
            get;
        }
        string GetText(Interval interval);
        string GetText();
        string GetText(RuleContext ctx);
        string GetText(IToken start, IToken stop);
    }
}
