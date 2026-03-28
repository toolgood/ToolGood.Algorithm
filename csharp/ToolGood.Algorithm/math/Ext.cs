using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using System;
using System.Collections.Generic;
using System.Text;
using ToolGood.Algorithm.Internals.Visitors;

namespace ToolGood.Algorithm.math
{
	partial class mathParser
	{
		private AntlrErrorData ErrorData;
		public void AddErrorData(AntlrErrorData data)
		{
			ErrorData = data;
		}

		public override void NotifyErrorListeners(IToken offendingToken, string msg, RecognitionException e)
		{
			ErrorData.IsError = true;
			ErrorData.ErrorMsg = msg;
			//base.NotifyErrorListeners(offendingToken, msg, e);
		}
	}
	partial class mathLexer
	{
		private AntlrErrorData ErrorData;
		public void AddErrorData(AntlrErrorData data)
		{
			ErrorData = data;
		}
		public override void NotifyListeners(LexerNoViableAltException e)
		{
			ErrorData.IsError = true;
			ErrorData.ErrorMsg = "Lexer is err";
		}
	}
}
