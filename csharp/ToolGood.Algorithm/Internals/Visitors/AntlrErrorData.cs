using Antlr4.Runtime;
using System.IO;

namespace ToolGood.Algorithm.Internals.Visitors
{

	internal class AntlrErrorData : IAntlrErrorListener<int>
	{
		public bool IsError { get; set; }
		public string ErrorMsg { get; set; }

		public void SyntaxError(TextWriter output, IRecognizer recognizer, int offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
		{
			IsError = true;
			ErrorMsg = msg;
		}
	}
}
