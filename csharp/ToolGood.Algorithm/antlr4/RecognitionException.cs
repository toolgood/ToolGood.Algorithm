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
    [System.Serializable]
    public class RecognitionException : Exception
    {
        private readonly IRecognizer recognizer;
        private readonly RuleContext ctx;
        private readonly IIntStream input;
        private IToken offendingToken;
        private int offendingState = -1;
        public RecognitionException(Lexer lexer, ICharStream input)
        {
            this.recognizer = lexer;
            this.input = input;
            this.ctx = null;
        }
        public RecognitionException(IRecognizer recognizer, IIntStream input, ParserRuleContext ctx)
        {
            this.recognizer = recognizer;
            this.input = input;
            this.ctx = ctx;
            if (recognizer != null)
            {
                this.offendingState = recognizer.State;
            }
        }
        public virtual IIntStream InputStream
        {
            get
            {
                return input;
            }
        }
        public IToken OffendingToken
        {
            get
            {
                return offendingToken;
            }
            protected set
            {
                IToken offendingToken = value;
                this.offendingToken = offendingToken;
            }
        }
    }
}
