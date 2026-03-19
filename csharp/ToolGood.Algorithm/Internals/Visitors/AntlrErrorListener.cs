using Antlr4.Runtime;
using System;
using System.IO;
using System.Text;

namespace ToolGood.Algorithm.Internals.Visitors
{
	internal sealed class AntlrErrorListener<Symbol> : IAntlrErrorListener<Symbol>
	{
		public AntlrErrorData Data { get; set; }
		public AntlrErrorListener(AntlrErrorData data)
		{
			Data = data;
		}

		public void SyntaxError(TextWriter output, IRecognizer recognizer, Symbol offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
		{
			Data.IsError = true;
			Data.ErrorMsg = msg;
		}
	}
}
