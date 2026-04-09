/* Copyright (c) 2012-2017 The ANTLR Project. All rights reserved.
 * Use of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */
using System.IO;
namespace Antlr4.Runtime
{
    public interface IAntlrErrorListener<in TSymbol>
    {
        void SyntaxError(TextWriter output, IRecognizer recognizer, TSymbol offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e);
    }
}
