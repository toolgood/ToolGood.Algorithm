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
    /// <summary>A semantic predicate failed during validation.</summary>
    /// <remarks>
    /// A semantic predicate failed during validation.  Validation of predicates
    /// occurs when normally parsing the alternative just like matching a token.
    /// Disambiguating predicate evaluation occurs when we test a predicate during
    /// prediction.
    /// </remarks>
    [System.Serializable]
    internal class FailedPredicateException : RecognitionException
    {
        public FailedPredicateException(Parser recognizer)
            : this(recognizer, null)
        {
        }

        public FailedPredicateException(Parser recognizer, string predicate)
            : this(recognizer, predicate, null)
        {
        }

        public FailedPredicateException(Parser recognizer, string predicate, string message)
			: base(FormatMessage(predicate, message), recognizer, ((ITokenStream)recognizer.InputStream), recognizer.RuleContext)
        {
            this.OffendingToken = recognizer.CurrentToken;
        }


        
        private static string FormatMessage(string predicate, string message)
        {
            if (message != null)
            {
                return message;
            }
            return string.Format(CultureInfo.CurrentCulture, "failed predicate: {{{0}}}?", predicate);
        }
    }
}
