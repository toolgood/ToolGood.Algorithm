/* Copyright (c) 2012-2017 The ANTLR Project. All rights reserved.
 * Use of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */
using Antlr4.Runtime.Atn;

namespace Antlr4.Runtime
{
    /// <summary>
    /// Indicates that the parser could not decide which of two or more paths
    /// to take based upon the remaining input.
    /// </summary>
    /// <remarks>
    /// Indicates that the parser could not decide which of two or more paths
    /// to take based upon the remaining input. It tracks the starting token
    /// of the offending input and also knows where the parser was
    /// in the various paths when the error. Reported by reportNoViableAlternative()
    /// </remarks>
    [System.Serializable]
    internal class NoViableAltException : RecognitionException
    {
        private readonly IToken startToken;

        public NoViableAltException(Parser recognizer)
			: this(recognizer, ((ITokenStream)recognizer.InputStream), recognizer.CurrentToken, recognizer.CurrentToken, null, recognizer.RuleContext)
        {
        }

        public NoViableAltException(IRecognizer recognizer, ITokenStream input, IToken startToken, IToken offendingToken, ATNConfigSet deadEndConfigs, ParserRuleContext ctx)
            : base(recognizer, input, ctx)
        {
            // LL(1) error
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
