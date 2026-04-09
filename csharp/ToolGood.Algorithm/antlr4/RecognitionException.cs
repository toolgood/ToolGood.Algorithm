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
    /// <summary>The root of the ANTLR exception hierarchy.</summary>
    /// <remarks>
    /// The root of the ANTLR exception hierarchy. In general, ANTLR tracks just
    /// 3 kinds of errors: prediction errors, failed predicate errors, and
    /// mismatched input errors. In each case, the parser knows where it is
    /// in the input, where it is in the ATN, the rule invocation stack,
    /// and what kind of problem occurred.
    /// </remarks>
    [System.Serializable]
    public class RecognitionException : Exception
    {
        //private const long serialVersionUID = -3861826954750022374L;

        /// <summary>
        /// The
        /// <see cref="IRecognizer"/>
        /// where this exception originated.
        /// </summary>
        //[Nullable]
        private readonly IRecognizer recognizer;

        //[Nullable]
        private readonly RuleContext ctx;

        //[Nullable]
        private readonly IIntStream input;

        /// <summary>
        /// The current
        /// <see cref="IToken"/>
        /// when an error occurred. Since not all streams
        /// support accessing symbols by index, we have to track the
        /// <see cref="IToken"/>
        /// instance itself.
        /// </summary>
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
