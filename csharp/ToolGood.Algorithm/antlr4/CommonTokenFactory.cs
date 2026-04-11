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
    internal class CommonTokenFactory : ITokenFactory
    {
        public static readonly ITokenFactory Default = new Antlr4.Runtime.CommonTokenFactory();
        protected internal readonly bool copyText;
        public CommonTokenFactory(bool copyText)
        {
            this.copyText = copyText;
        }
        public CommonTokenFactory()
            : this(false)
        {
        }
        public virtual CommonToken Create(Tuple<ITokenSource, ICharStream> source, int type, string text, int channel, int start, int stop, int line, int charPositionInLine)
        {
            CommonToken t = new CommonToken(source, type, channel, start, stop);
            t.Line = line;
            t.Column = charPositionInLine;
            if (text != null)
            {
                t.Text = text;
            }
            else
            {
                if (copyText && source.Item2 != null)
                {
                    t.Text = source.Item2.GetText(Interval.Of(start, stop));
                }
            }
            return t;
        }
        IToken ITokenFactory.Create(Tuple<ITokenSource, ICharStream> source, int type, string text, int channel, int start, int stop, int line, int charPositionInLine)
        {
            return Create(source, type, text, channel, start, stop, line, charPositionInLine);
        }
        public virtual CommonToken Create(int type, string text)
        {
            return new CommonToken(type, text);
        }
        IToken ITokenFactory.Create(int type, string text)
        {
            return Create(type, text);
        }
    }
}
