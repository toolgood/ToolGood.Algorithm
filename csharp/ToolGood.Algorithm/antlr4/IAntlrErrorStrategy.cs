/* Copyright (c) 2012-2017 The ANTLR Project. All rights reserved.
 * Use of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Sharpen;
using System.IO;
namespace Antlr4.Runtime
{
    internal interface IAntlrErrorStrategy
    {
        void Reset(Parser recognizer);
        IToken RecoverInline(Parser recognizer);
        void Recover(Parser recognizer, RecognitionException e);
        void Sync(Parser recognizer);
        bool InErrorRecoveryMode(Parser recognizer);
        void ReportMatch(Parser recognizer);
        void ReportError(Parser recognizer, RecognitionException e);
    }
}
