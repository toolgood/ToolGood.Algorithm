/* Copyright (c) 2012-2017 The ANTLR Project. All rights reserved.
 * Use of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */
using System.Globalization;
using Antlr4.Runtime;
using Antlr4.Runtime.Atn;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Sharpen;
namespace Antlr4.Runtime
{
    [System.Serializable]
    internal class LexerNoViableAltException : RecognitionException
    {
        private readonly int startIndex;
        public LexerNoViableAltException(Lexer lexer, ICharStream input, int startIndex, ATNConfigSet deadEndConfigs)
            : base(lexer, input)
        {
            this.startIndex = startIndex;
        }
        public override IIntStream InputStream
        {
            get
            {
                return (ICharStream)base.InputStream;
            }
        }
    }
}
