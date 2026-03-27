using Antlr4.Runtime;
using System.IO;

namespace ToolGood.Algorithm.Internals.Visitors
{
	internal class AntlrErrorListener<T> : AntlrErrorData ,IAntlrErrorListener<T>
	{
		public void SyntaxError(TextWriter output, IRecognizer recognizer, T offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
		{
			IsError = true;
			ErrorMsg = msg;
		}
	}
}
