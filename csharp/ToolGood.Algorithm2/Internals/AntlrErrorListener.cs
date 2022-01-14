using Antlr4.Runtime;
using System.IO;

namespace ToolGood.Algorithm.Internals
{
    class AntlrErrorListener : IAntlrErrorListener<IToken>
    {
        public bool IsError { get; private set; }
        public string ErrorMsg { get; private set; }

        public void SyntaxError(TextWriter output, IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
        {
            IsError = true;
            ErrorMsg = msg;
        }
    }
}
