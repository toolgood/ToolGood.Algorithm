using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
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
		//private AntlrErrorData ErrorData;
		public void AddErrorData(AntlrErrorData data)
		{
			//ErrorData = data;
		}
		public void NotifyListeners(LexerNoViableAltException e)
		{
			//ErrorData.IsError = true;
			//ErrorData.ErrorMsg = "Lexer is err";
		}
	}
	abstract class MyRuleContext : ParserRuleContext
	{
		public MyRuleContext() : base() { }

		public MyRuleContext(ParserRuleContext parent, int invokingState) : base(parent, invokingState)
		{
		}

		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor)
		{
			ImathVisitor<TResult> typedVisitor = visitor as ImathVisitor<TResult>;
			return Accept2(typedVisitor);
		}
		public virtual TResult Accept2<TResult>(ImathVisitor<TResult> visitor)
		{
			return visitor.Visit(this);
		}
	}
}
