/* Copyright (c) 2012-2017 The ANTLR Project. All rights reserved.
 * Use of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */
using System;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Sharpen;
namespace Antlr4.Runtime
{
    public interface ITokenFactory
    {
        IToken Create(Tuple<ITokenSource, ICharStream> source, int type, string text, int channel, int start, int stop, int line, int charPositionInLine);
        IToken Create(int type, string text);
    }
}
