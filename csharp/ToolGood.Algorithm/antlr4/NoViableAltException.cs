/* Copyright (c) 2012-2017 The ANTLR Project. All rights reserved.
 * Use of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */
using Antlr4.Runtime;
using Antlr4.Runtime.Atn;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Sharpen;
namespace Antlr4.Runtime
{
    [System.Serializable]
    public class NoViableAltException : RecognitionException
    {
        private readonly IToken startToken;
        public NoViableAltException(Parser recognizer)
			: this(recognizer, ((ITokenStream)recognizer.InputStream), recognizer.CurrentToken, recognizer.CurrentToken, null, recognizer.RuleContext)
        {
        }
        public NoViableAltException(IRecognizer recognizer, ITokenStream input, IToken startToken, IToken offendingToken, ATNConfigSet deadEndConfigs, ParserRuleContext ctx)
            : base(recognizer, input, ctx)
        {
            this.startToken = startToken;
            this.OffendingToken = offendingToken;
        }
        public virtual IToken StartToken
        {
            get
            {
                return startToken;
            }
        }
    }
}
