/* Copyright (c) 2012-2017 The ANTLR Project. All rights reserved.
 * Use of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */
using Antlr4.Runtime;
using Antlr4.Runtime.Sharpen;
namespace Antlr4.Runtime
{
    [System.Serializable]
    internal class InputMismatchException : RecognitionException
    {
        public InputMismatchException(Parser recognizer)
			: base(recognizer, ((ITokenStream)recognizer.InputStream), recognizer.RuleContext)
        {
            this.OffendingToken = recognizer.CurrentToken;
        }
    }
}
