namespace ToolGood.Algorithm.math
{
#pragma warning disable 0162
#pragma warning disable 0219
#pragma warning disable 1591
#pragma warning disable 419
using System;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Collections.Generic;
using Antlr4.Runtime;
using Antlr4.Runtime.Atn;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using DFA = Antlr4.Runtime.Dfa.DFA;
partial class mathParser : Parser {
	protected static DFA[] decisionToDFA;
	protected static PredictionContextCache sharedContextCache = new PredictionContextCache();
	public static readonly string[] ruleNames = {};
	private static readonly string[] _LiteralNames = {};
	private static readonly string[] _SymbolicNames = {};
	public static readonly IVocabulary DefaultVocabulary = new Vocabulary(_LiteralNames, _SymbolicNames);
	public override IVocabulary Vocabulary
	{
		get
		{
			return DefaultVocabulary;
		}
	}
	public override string GrammarFileName { get { return ""; } }
	public override string[] RuleNames { get { return ruleNames; } }
	public override int[] SerializedAtn { get { return _serializedATN; } }
	static mathParser() {
		decisionToDFA = new DFA[_ATN.NumberOfDecisions];
		for (int i = 0; i < _ATN.NumberOfDecisions; i++) {
			decisionToDFA[i] = new DFA(_ATN.GetDecisionState(i), i);
		}
	}
		public mathParser(ITokenStream input) : this(input, Console.Out, Console.Error) { }
		public mathParser(ITokenStream input, TextWriter output, TextWriter errorOutput)
		: base(input, output, errorOutput)
	{
		Interpreter = new ParserATNSimulator(this, _ATN, decisionToDFA, sharedContextCache);
	}
	internal sealed class ProgContext : ParserRuleContext {
		[System.Diagnostics.DebuggerNonUserCode] public ExprContext expr() {
			return GetRuleContext<ExprContext>(0);
		}
		public ProgContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return 0; } }
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			ImathVisitor<TResult> typedVisitor = visitor as ImathVisitor<TResult>;
			return typedVisitor.VisitProg(this);
		}
	}
	public ProgContext prog() {
		ProgContext _localctx = new ProgContext(Context, State);
		EnterRule(_localctx, 0, 0);
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 10;
			expr(0);
			Match(-1);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}
	internal class ExprContext : ParserRuleContext {
		public ExprContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return 1; } }
		public ExprContext() { }
		public virtual void CopyFrom(ExprContext context) {
			base.CopyFrom(context);
		}
	}
	internal sealed class Function_funContext : ExprContext {
		public IToken f;
		[System.Diagnostics.DebuggerNonUserCode] public ExprContext[] expr() {
			return GetRuleContexts<ExprContext>();
		}
		
		
		
		
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode PARAMETER() { return GetToken(301, 0); }
		public Function_funContext(ExprContext context) { CopyFrom(context); }
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			ImathVisitor<TResult> typedVisitor = visitor as ImathVisitor<TResult>;
			return typedVisitor.VisitFunction_fun(this);
		}
	}
	internal sealed class Or_funContext : ExprContext {
		[System.Diagnostics.DebuggerNonUserCode] public ExprContext[] expr() {
			return GetRuleContexts<ExprContext>();
		}
		public Or_funContext(ExprContext context) { CopyFrom(context); }
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			ImathVisitor<TResult> typedVisitor = visitor as ImathVisitor<TResult>;
			return typedVisitor.VisitOr_fun(this);
		}
	}
	internal sealed class IF_funContext : ExprContext {
		[System.Diagnostics.DebuggerNonUserCode] public ExprContext[] expr() {
			return GetRuleContexts<ExprContext>();
		}
		public IF_funContext(ExprContext context) { CopyFrom(context); }
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			ImathVisitor<TResult> typedVisitor = visitor as ImathVisitor<TResult>;
			return typedVisitor.VisitIF_fun(this);
		}
	}
	internal sealed class Judge_funContext : ExprContext {
		public IToken op;
		[System.Diagnostics.DebuggerNonUserCode] public ExprContext[] expr() {
			return GetRuleContexts<ExprContext>();
		}
		public Judge_funContext(ExprContext context) { CopyFrom(context); }
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			ImathVisitor<TResult> typedVisitor = visitor as ImathVisitor<TResult>;
			return typedVisitor.VisitJudge_fun(this);
		}
	}
	internal sealed class Percentage_funContext : ExprContext {
		[System.Diagnostics.DebuggerNonUserCode] public ExprContext expr() {
			return GetRuleContext<ExprContext>(0);
		}
		public Percentage_funContext(ExprContext context) { CopyFrom(context); }
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			ImathVisitor<TResult> typedVisitor = visitor as ImathVisitor<TResult>;
			return typedVisitor.VisitPercentage_fun(this);
		}
	}
	internal sealed class STRING_funContext : ExprContext {
		public STRING_funContext(ExprContext context) { CopyFrom(context); }
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			ImathVisitor<TResult> typedVisitor = visitor as ImathVisitor<TResult>;
			return typedVisitor.VisitSTRING_fun(this);
		}
	}
	internal sealed class AddSub_funContext : ExprContext {
		public IToken op;
		[System.Diagnostics.DebuggerNonUserCode] public ExprContext[] expr() {
			return GetRuleContexts<ExprContext>();
		}
		
		public AddSub_funContext(ExprContext context) { CopyFrom(context); }
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			ImathVisitor<TResult> typedVisitor = visitor as ImathVisitor<TResult>;
			return typedVisitor.VisitAddSub_fun(this);
		}
	}
	internal sealed class ArrayJson_funContext : ExprContext {
		[System.Diagnostics.DebuggerNonUserCode] public ArrayJsonContext[] arrayJson() {
			return GetRuleContexts<ArrayJsonContext>();
		}
		[System.Diagnostics.DebuggerNonUserCode] public ArrayJsonContext arrayJson(int i) {
			return GetRuleContext<ArrayJsonContext>(i);
		}
		public ArrayJson_funContext(ExprContext context) { CopyFrom(context); }
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			ImathVisitor<TResult> typedVisitor = visitor as ImathVisitor<TResult>;
			return typedVisitor.VisitArrayJson_fun(this);
		}
	}
	internal sealed class GetJsonValue_funContext : ExprContext {
		[System.Diagnostics.DebuggerNonUserCode] public ExprContext[] expr() {
			return GetRuleContexts<ExprContext>();
		}
		[System.Diagnostics.DebuggerNonUserCode] public Parameter2Context parameter2() {
			return GetRuleContext<Parameter2Context>(0);
		}
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode PARAMETER() { return GetToken(301, 0); }
		public GetJsonValue_funContext(ExprContext context) { CopyFrom(context); }
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			ImathVisitor<TResult> typedVisitor = visitor as ImathVisitor<TResult>;
			return typedVisitor.VisitGetJsonValue_fun(this);
		}
	}
	internal sealed class And_funContext : ExprContext {
		[System.Diagnostics.DebuggerNonUserCode] public ExprContext[] expr() {
			return GetRuleContexts<ExprContext>();
		}
		public And_funContext(ExprContext context) { CopyFrom(context); }
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			ImathVisitor<TResult> typedVisitor = visitor as ImathVisitor<TResult>;
			return typedVisitor.VisitAnd_fun(this);
		}
	}
	internal sealed class Array_funContext : ExprContext {
		[System.Diagnostics.DebuggerNonUserCode] public ExprContext[] expr() {
			return GetRuleContexts<ExprContext>();
		}
		public Array_funContext(ExprContext context) { CopyFrom(context); }
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			ImathVisitor<TResult> typedVisitor = visitor as ImathVisitor<TResult>;
			return typedVisitor.VisitArray_fun(this);
		}
	}
	internal sealed class MulDiv_funContext : ExprContext {
		public IToken op;
		[System.Diagnostics.DebuggerNonUserCode] public ExprContext[] expr() {
			return GetRuleContexts<ExprContext>();
		}
		
		public MulDiv_funContext(ExprContext context) { CopyFrom(context); }
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			ImathVisitor<TResult> typedVisitor = visitor as ImathVisitor<TResult>;
			return typedVisitor.VisitMulDiv_fun(this);
		}
	}
	internal sealed class NOT_funContext : ExprContext {
		[System.Diagnostics.DebuggerNonUserCode] public ExprContext expr() {
			return GetRuleContext<ExprContext>(0);
		}
		public NOT_funContext(ExprContext context) { CopyFrom(context); }
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			ImathVisitor<TResult> typedVisitor = visitor as ImathVisitor<TResult>;
			return typedVisitor.VisitNOT_fun(this);
		}
	}
	internal sealed class CONST2_funContext : ExprContext {
		public IToken f;
		
		public CONST2_funContext(ExprContext context) { CopyFrom(context); }
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			ImathVisitor<TResult> typedVisitor = visitor as ImathVisitor<TResult>;
			return typedVisitor.VisitCONST2_fun(this);
		}
	}
	internal sealed class Bracket_funContext : ExprContext {
		[System.Diagnostics.DebuggerNonUserCode] public ExprContext expr() {
			return GetRuleContext<ExprContext>(0);
		}
		public Bracket_funContext(ExprContext context) { CopyFrom(context); }
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			ImathVisitor<TResult> typedVisitor = visitor as ImathVisitor<TResult>;
			return typedVisitor.VisitBracket_fun(this);
		}
	}
	internal sealed class CONST_funContext : ExprContext {
		public IToken f;
		public CONST_funContext(ExprContext context) { CopyFrom(context); }
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			ImathVisitor<TResult> typedVisitor = visitor as ImathVisitor<TResult>;
			return typedVisitor.VisitCONST_fun(this);
		}
	}
	internal sealed class NUM_funContext : ExprContext {
		public IToken unit;
		[System.Diagnostics.DebuggerNonUserCode] public NumContext num() {
			return GetRuleContext<NumContext>(0);
		}
		public NUM_funContext(ExprContext context) { CopyFrom(context); }
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			ImathVisitor<TResult> typedVisitor = visitor as ImathVisitor<TResult>;
			return typedVisitor.VisitNUM_fun(this);
		}
	}
	internal sealed class PARAMETER_funContext : ExprContext {
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode PARAMETER() { return GetToken(301, 0); }
		public PARAMETER_funContext(ExprContext context) { CopyFrom(context); }
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			ImathVisitor<TResult> typedVisitor = visitor as ImathVisitor<TResult>;
			return typedVisitor.VisitPARAMETER_fun(this);
		}
	}
	private ExprContext expr(int _p) {
		ParserRuleContext _parentctx = Context;
		int _parentState = State;
		ExprContext _localctx = new ExprContext(Context, _parentState);
		int _startState = 2;
		EnterRecursionRule(_localctx, 2, 1, _p);
		int _la;
		try {
			int _alt;
			EnterOuterAlt(_localctx, 1);
			{
			ErrorHandler.Sync(this);
			switch ( Interpreter.AdaptivePredict(TokenStream,7,Context) ) {
			case 1:
				{
				_localctx = new Bracket_funContext(_localctx);
				Context = _localctx;
				Match(1);
				State = 15;
				expr(0);
				Match(2);
				}
				break;
			case 2:
				{
				_localctx = new NUM_funContext(_localctx);
				Context = _localctx;
				State = 18;
				num();
				ErrorHandler.Sync(this);
				switch ( Interpreter.AdaptivePredict(TokenStream,0,Context) ) {
				case 1:
					{
					State = 19;
					((NUM_funContext)_localctx).unit = TokenStream.LT(1);
					_la = TokenStream.LA(1);
					if ( !(_la==28 || _la==118) ) {
						((NUM_funContext)_localctx).unit = ErrorHandler.RecoverInline(this);
					}
					else {
						ErrorHandler.ReportMatch(this);
					    Consume();
					}
					}
					break;
				}
				}
				break;
			case 3:
				{
				_localctx = new STRING_funContext(_localctx);
				Context = _localctx;
				Match(27);
				}
				break;
			case 4:
				{
				_localctx = new PARAMETER_funContext(_localctx);
				Context = _localctx;
				Match(301);
				}
				break;
			case 5:
				{
				_localctx = new NOT_funContext(_localctx);
				Context = _localctx;
				Match(7);
				State = 25;
				expr(14);
				}
				break;
			case 6:
				{
				_localctx = new CONST2_funContext(_localctx);
				Context = _localctx;
				State = 26;
				((CONST2_funContext)_localctx).f = TokenStream.LT(1);
				_la = TokenStream.LA(1);
				if ( !(_la==29 || _la==30 || _la==299 || _la==300) ) {
					((CONST2_funContext)_localctx).f = ErrorHandler.RecoverInline(this);
				}
				else {
					ErrorHandler.ReportMatch(this);
				    Consume();
				}
				ErrorHandler.Sync(this);
				switch ( Interpreter.AdaptivePredict(TokenStream,1,Context) ) {
				case 1:
					{
					Match(1);
					Match(2);
					}
					break;
				}
				}
				break;
			case 7:
				{
				_localctx = new CONST_funContext(_localctx);
				Context = _localctx;
				State = 31;
				((CONST_funContext)_localctx).f = TokenStream.LT(1);
				_la = TokenStream.LA(1);
				if ( !((((_la) & ~0x3f) == 0 && ((1L << _la) & 135291469824L) != 0)) ) {
					((CONST_funContext)_localctx).f = ErrorHandler.RecoverInline(this);
				}
				else {
					ErrorHandler.ReportMatch(this);
				    Consume();
				}
				Match(1);
				Match(2);
				}
				break;
			case 8:
				{
				_localctx = new Function_funContext(_localctx);
				Context = _localctx;
				State = 34;
				((Function_funContext)_localctx).f = TokenStream.LT(1);
				_la = TokenStream.LA(1);
				if ( !((((_la) & ~0x3f) == 0 && ((1L << _la) & -137438953472L) != 0) || ((((_la - 64)) & ~0x3f) == 0 && ((1L << (_la - 64)) & -1L) != 0) || ((((_la - 128)) & ~0x3f) == 0 && ((1L << (_la - 128)) & -1L) != 0) || ((((_la - 192)) & ~0x3f) == 0 && ((1L << (_la - 192)) & -1L) != 0) || ((((_la - 256)) & ~0x3f) == 0 && ((1L << (_la - 256)) & 43980465111039L) != 0)) ) {
					((Function_funContext)_localctx).f = ErrorHandler.RecoverInline(this);
				}
				else {
					ErrorHandler.ReportMatch(this);
				    Consume();
				}
				Match(1);
				State = 36;
				expr(0);
				ErrorHandler.Sync(this);
				_la = TokenStream.LA(1);
				while (_la==4) {
					{
					{
					Match(4);
					State = 38;
					expr(0);
					}
					}
					ErrorHandler.Sync(this);
					_la = TokenStream.LA(1);
				}
				Match(2);
				}
				break;
			case 9:
				{
				_localctx = new ArrayJson_funContext(_localctx);
				Context = _localctx;
				Match(10);
				State = 47;
				arrayJson();
				ErrorHandler.Sync(this);
				_alt = Interpreter.AdaptivePredict(TokenStream,3,Context);
				while ( _alt!=2 && _alt!=global::Antlr4.Runtime.Atn.ATN.INVALID_ALT_NUMBER ) {
					if ( _alt==1 ) {
						{
						{
						Match(4);
						State = 49;
						arrayJson();
						}
						} 
					}
					ErrorHandler.Sync(this);
					_alt = Interpreter.AdaptivePredict(TokenStream,3,Context);
				}
				ErrorHandler.Sync(this);
				_la = TokenStream.LA(1);
				if (_la==4) {
					{
					Match(4);
					}
				}
				Match(11);
				}
				break;
			case 10:
				{
				_localctx = new Array_funContext(_localctx);
				Context = _localctx;
				Match(5);
				State = 61;
				expr(0);
				ErrorHandler.Sync(this);
				_alt = Interpreter.AdaptivePredict(TokenStream,5,Context);
				while ( _alt!=2 && _alt!=global::Antlr4.Runtime.Atn.ATN.INVALID_ALT_NUMBER ) {
					if ( _alt==1 ) {
						{
						{
						Match(4);
						State = 63;
						expr(0);
						}
						} 
					}
					ErrorHandler.Sync(this);
					_alt = Interpreter.AdaptivePredict(TokenStream,5,Context);
				}
				ErrorHandler.Sync(this);
				_la = TokenStream.LA(1);
				if (_la==4) {
					{
					Match(4);
					}
				}
				Match(6);
				}
				break;
			}
			Context.Stop = TokenStream.LT(-1);
			ErrorHandler.Sync(this);
			_alt = Interpreter.AdaptivePredict(TokenStream,13,Context);
			while ( _alt!=2 && _alt!=global::Antlr4.Runtime.Atn.ATN.INVALID_ALT_NUMBER ) {
				if ( _alt==1 ) {
					if ( ParseListeners!=null )
						TriggerExitRuleEvent();
					{
					ErrorHandler.Sync(this);
					switch ( Interpreter.AdaptivePredict(TokenStream,12,Context) ) {
					case 1:
						{
						_localctx = new MulDiv_funContext(new ExprContext(_parentctx, _parentState));
						PushNewRecursionContext(_localctx, _startState, 1);
						State = 77;
						((MulDiv_funContext)_localctx).op = TokenStream.LT(1);
						_la = TokenStream.LA(1);
						if ( !((((_la) & ~0x3f) == 0 && ((1L << _la) & 114688L) != 0)) ) {
							((MulDiv_funContext)_localctx).op = ErrorHandler.RecoverInline(this);
						}
						else {
							ErrorHandler.ReportMatch(this);
						    Consume();
						}
						State = 78;
						expr(13);
						}
						break;
					case 2:
						{
						_localctx = new AddSub_funContext(new ExprContext(_parentctx, _parentState));
						PushNewRecursionContext(_localctx, _startState, 1);
						State = 80;
						((AddSub_funContext)_localctx).op = TokenStream.LT(1);
						_la = TokenStream.LA(1);
						if ( !((((_la) & ~0x3f) == 0 && ((1L << _la) & 143360L) != 0)) ) {
							((AddSub_funContext)_localctx).op = ErrorHandler.RecoverInline(this);
						}
						else {
							ErrorHandler.ReportMatch(this);
						    Consume();
						}
						State = 81;
						expr(12);
						}
						break;
					case 3:
						{
						_localctx = new Judge_funContext(new ExprContext(_parentctx, _parentState));
						PushNewRecursionContext(_localctx, _startState, 1);
						State = 83;
						((Judge_funContext)_localctx).op = TokenStream.LT(1);
						_la = TokenStream.LA(1);
						if ( !((((_la) & ~0x3f) == 0 && ((1L << _la) & 3932160L) != 0)) ) {
							((Judge_funContext)_localctx).op = ErrorHandler.RecoverInline(this);
						}
						else {
							ErrorHandler.ReportMatch(this);
						    Consume();
						}
						State = 84;
						expr(11);
						}
						break;
					case 4:
						{
						_localctx = new Judge_funContext(new ExprContext(_parentctx, _parentState));
						PushNewRecursionContext(_localctx, _startState, 1);
						State = 86;
						((Judge_funContext)_localctx).op = TokenStream.LT(1);
						_la = TokenStream.LA(1);
						if ( !(_la==22 || _la==23) ) {
							((Judge_funContext)_localctx).op = ErrorHandler.RecoverInline(this);
						}
						else {
							ErrorHandler.ReportMatch(this);
						    Consume();
						}
						State = 87;
						expr(10);
						}
						break;
					case 5:
						{
						_localctx = new And_funContext(new ExprContext(_parentctx, _parentState));
						PushNewRecursionContext(_localctx, _startState, 1);
						Match(24);
						State = 90;
						expr(9);
						}
						break;
					case 6:
						{
						_localctx = new Or_funContext(new ExprContext(_parentctx, _parentState));
						PushNewRecursionContext(_localctx, _startState, 1);
						Match(25);
						State = 93;
						expr(8);
						}
						break;
					case 7:
						{
						_localctx = new Function_funContext(new ExprContext(_parentctx, _parentState));
						PushNewRecursionContext(_localctx, _startState, 1);
						Match(3);
						State = 96;
						((Function_funContext)_localctx).f = TokenStream.LT(1);
						_la = TokenStream.LA(1);
						if ( !((((_la) & ~0x3f) == 0 && ((1L << _la) & 18295598608285696L) != 0) || ((((_la - 95)) & ~0x3f) == 0 && ((1L << (_la - 95)) & 6642824844070105211L) != 0) || ((((_la - 168)) & ~0x3f) == 0 && ((1L << (_la - 168)) & 360287970189642823L) != 0) || ((((_la - 238)) & ~0x3f) == 0 && ((1L << (_la - 238)) & -8718968792689803709L) != 0)) ) {
							((Function_funContext)_localctx).f = ErrorHandler.RecoverInline(this);
						}
						else {
							ErrorHandler.ReportMatch(this);
						    Consume();
						}
						Match(1);
						ErrorHandler.Sync(this);
						_la = TokenStream.LA(1);
						if ((((_la) & ~0x3f) == 0 && ((1L << _la) & -335534942L) != 0) || ((((_la - 64)) & ~0x3f) == 0 && ((1L << (_la - 64)) & -1L) != 0) || ((((_la - 128)) & ~0x3f) == 0 && ((1L << (_la - 128)) & -1L) != 0) || ((((_la - 192)) & ~0x3f) == 0 && ((1L << (_la - 192)) & -1L) != 0) || ((((_la - 256)) & ~0x3f) == 0 && ((1L << (_la - 256)) & 70368744177663L) != 0)) {
							{
							State = 98;
							expr(0);
							ErrorHandler.Sync(this);
							_la = TokenStream.LA(1);
							while (_la==4) {
								{
								{
								Match(4);
								State = 100;
								expr(0);
								}
								}
								ErrorHandler.Sync(this);
								_la = TokenStream.LA(1);
							}
							}
						}
						Match(2);
						}
						break;
					case 8:
						{
						_localctx = new GetJsonValue_funContext(new ExprContext(_parentctx, _parentState));
						PushNewRecursionContext(_localctx, _startState, 1);
						ErrorHandler.Sync(this);
						switch (TokenStream.LA(1)) {
						case 5:
							{
							Match(5);
							ErrorHandler.Sync(this);
							switch ( Interpreter.AdaptivePredict(TokenStream,10,Context) ) {
							case 1:
								{
								Match(301);
								}
								break;
							case 2:
								{
								State = 112;
								expr(0);
								}
								break;
							}
							Match(6);
							}
							break;
						case 3:
							{
							Match(3);
							State = 117;
							parameter2();
							}
							break;
						default:
							throw new NoViableAltException(this);
						}
						}
						break;
					case 9:
						{
						_localctx = new Percentage_funContext(new ExprContext(_parentctx, _parentState));
						PushNewRecursionContext(_localctx, _startState, 1);
						Match(16);
						}
						break;
					case 10:
						{
						_localctx = new IF_funContext(new ExprContext(_parentctx, _parentState));
						PushNewRecursionContext(_localctx, _startState, 1);
						Match(8);
						State = 124;
						expr(0);
						Match(9);
						State = 126;
						expr(0);
						}
						break;
					}
					} 
				}
				ErrorHandler.Sync(this);
				_alt = Interpreter.AdaptivePredict(TokenStream,13,Context);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			UnrollRecursionContexts(_parentctx);
		}
		return _localctx;
	}
	internal sealed class NumContext : ParserRuleContext {
		public NumContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return 2; } }
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			ImathVisitor<TResult> typedVisitor = visitor as ImathVisitor<TResult>;
			return typedVisitor.VisitNum(this);
		}
	}
	public NumContext num() {
		NumContext _localctx = new NumContext(Context, State);
		EnterRule(_localctx, 4, 2);
		int _la;
		try {
			EnterOuterAlt(_localctx, 1);
			{
			ErrorHandler.Sync(this);
			_la = TokenStream.LA(1);
			if (_la==13) {
				{
				Match(13);
				}
			}
			Match(26);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}
	internal sealed class ArrayJsonContext : ParserRuleContext {
		public IToken key;
		[System.Diagnostics.DebuggerNonUserCode] public ExprContext expr() {
			return GetRuleContext<ExprContext>(0);
		}
		[System.Diagnostics.DebuggerNonUserCode] public Parameter2Context parameter2() {
			return GetRuleContext<Parameter2Context>(0);
		}
		public ArrayJsonContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return 3; } }
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			ImathVisitor<TResult> typedVisitor = visitor as ImathVisitor<TResult>;
			return typedVisitor.VisitArrayJson(this);
		}
	}
	public ArrayJsonContext arrayJson() {
		ArrayJsonContext _localctx = new ArrayJsonContext(Context, State);
		EnterRule(_localctx, 6, 3);
		int _la;
		try {
			ErrorHandler.Sync(this);
			switch (TokenStream.LA(1)) {
			case 26:
			case 27:
				EnterOuterAlt(_localctx, 1);
				{
				State = 138;
				_localctx.key = TokenStream.LT(1);
				_la = TokenStream.LA(1);
				if ( !(_la==26 || _la==27) ) {
					_localctx.key = ErrorHandler.RecoverInline(this);
				}
				else {
					ErrorHandler.ReportMatch(this);
				    Consume();
				}
				Match(9);
				State = 140;
				expr(0);
				}
				break;
			case 28:
			case 29:
			case 30:
			case 31:
			case 32:
			case 33:
			case 34:
			case 35:
			case 36:
			case 37:
			case 38:
			case 39:
			case 40:
			case 41:
			case 42:
			case 43:
			case 44:
			case 45:
			case 46:
			case 47:
			case 48:
			case 49:
			case 50:
			case 51:
			case 52:
			case 53:
			case 54:
			case 55:
			case 56:
			case 57:
			case 58:
			case 59:
			case 60:
			case 61:
			case 62:
			case 63:
			case 64:
			case 65:
			case 66:
			case 67:
			case 68:
			case 69:
			case 70:
			case 71:
			case 72:
			case 73:
			case 74:
			case 75:
			case 76:
			case 77:
			case 78:
			case 79:
			case 80:
			case 81:
			case 82:
			case 83:
			case 84:
			case 85:
			case 86:
			case 87:
			case 88:
			case 89:
			case 90:
			case 91:
			case 92:
			case 93:
			case 94:
			case 95:
			case 96:
			case 97:
			case 98:
			case 99:
			case 100:
			case 101:
			case 102:
			case 103:
			case 104:
			case 105:
			case 106:
			case 107:
			case 108:
			case 109:
			case 110:
			case 111:
			case 112:
			case 113:
			case 114:
			case 115:
			case 116:
			case 117:
			case 118:
			case 119:
			case 120:
			case 121:
			case 122:
			case 123:
			case 124:
			case 125:
			case 126:
			case 127:
			case 128:
			case 129:
			case 130:
			case 131:
			case 132:
			case 133:
			case 134:
			case 135:
			case 136:
			case 137:
			case 138:
			case 139:
			case 140:
			case 141:
			case 142:
			case 143:
			case 144:
			case 145:
			case 146:
			case 147:
			case 148:
			case 149:
			case 150:
			case 151:
			case 152:
			case 153:
			case 154:
			case 155:
			case 156:
			case 157:
			case 158:
			case 159:
			case 160:
			case 161:
			case 162:
			case 163:
			case 164:
			case 165:
			case 166:
			case 167:
			case 168:
			case 169:
			case 170:
			case 171:
			case 172:
			case 173:
			case 174:
			case 175:
			case 176:
			case 177:
			case 178:
			case 179:
			case 180:
			case 181:
			case 182:
			case 183:
			case 184:
			case 185:
			case 186:
			case 187:
			case 188:
			case 189:
			case 190:
			case 191:
			case 192:
			case 193:
			case 194:
			case 195:
			case 196:
			case 197:
			case 198:
			case 199:
			case 200:
			case 201:
			case 202:
			case 203:
			case 204:
			case 205:
			case 206:
			case 207:
			case 208:
			case 209:
			case 210:
			case 211:
			case 212:
			case 213:
			case 214:
			case 215:
			case 216:
			case 217:
			case 218:
			case 219:
			case 220:
			case 221:
			case 222:
			case 223:
			case 224:
			case 225:
			case 226:
			case 227:
			case 228:
			case 229:
			case 230:
			case 231:
			case 232:
			case 233:
			case 234:
			case 235:
			case 236:
			case 237:
			case 238:
			case 239:
			case 240:
			case 241:
			case 242:
			case 243:
			case 244:
			case 245:
			case 246:
			case 247:
			case 248:
			case 249:
			case 250:
			case 251:
			case 252:
			case 253:
			case 254:
			case 255:
			case 256:
			case 257:
			case 258:
			case 259:
			case 260:
			case 261:
			case 262:
			case 263:
			case 264:
			case 265:
			case 266:
			case 267:
			case 268:
			case 269:
			case 270:
			case 271:
			case 272:
			case 273:
			case 274:
			case 275:
			case 276:
			case 277:
			case 278:
			case 279:
			case 280:
			case 281:
			case 282:
			case 283:
			case 284:
			case 285:
			case 286:
			case 287:
			case 288:
			case 289:
			case 290:
			case 291:
			case 292:
			case 293:
			case 294:
			case 295:
			case 296:
			case 297:
			case 298:
			case 299:
			case 300:
			case 301:
				EnterOuterAlt(_localctx, 2);
				{
				State = 141;
				parameter2();
				Match(9);
				State = 143;
				expr(0);
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}
	internal sealed class Parameter2Context : ParserRuleContext {
		
		
		
		
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode PARAMETER() { return GetToken(301, 0); }
		public Parameter2Context(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return 4; } }
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			ImathVisitor<TResult> typedVisitor = visitor as ImathVisitor<TResult>;
			return typedVisitor.VisitParameter2(this);
		}
	}
	public Parameter2Context parameter2() {
		Parameter2Context _localctx = new Parameter2Context(Context, State);
		EnterRule(_localctx, 8, 4);
		int _la;
		try {
			EnterOuterAlt(_localctx, 1);
			{
			_la = TokenStream.LA(1);
			if ( !((((_la) & ~0x3f) == 0 && ((1L << _la) & -268435456L) != 0) || ((((_la - 64)) & ~0x3f) == 0 && ((1L << (_la - 64)) & -1L) != 0) || ((((_la - 128)) & ~0x3f) == 0 && ((1L << (_la - 128)) & -1L) != 0) || ((((_la - 192)) & ~0x3f) == 0 && ((1L << (_la - 192)) & -1L) != 0) || ((((_la - 256)) & ~0x3f) == 0 && ((1L << (_la - 256)) & 70368744177663L) != 0)) ) {
			ErrorHandler.RecoverInline(this);
			}
			else {
				ErrorHandler.ReportMatch(this);
			    Consume();
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}
	
	private static int[] _serializedATN = {
		4,1,304,150,2,0,7,0,2,1,7,1,2,2,7,2,2,3,7,3,2,4,7,4,1,0,1,0,1,0,1,1,1,
		1,1,1,1,1,1,1,1,1,1,1,3,1,21,8,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,3,1,30,8,
		1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,5,1,40,8,1,10,1,12,1,43,9,1,1,1,1,1,
		1,1,1,1,1,1,1,1,5,1,51,8,1,10,1,12,1,54,9,1,1,1,3,1,57,8,1,1,1,1,1,1,1,
		1,1,1,1,1,1,5,1,65,8,1,10,1,12,1,68,9,1,1,1,3,1,71,8,1,1,1,1,1,3,1,75,
		8,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,
		1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,5,1,102,8,1,10,1,12,1,105,9,1,3,1,107,
		8,1,1,1,1,1,1,1,1,1,1,1,3,1,114,8,1,1,1,1,1,1,1,3,1,119,8,1,1,1,1,1,1,
		1,1,1,1,1,1,1,1,1,1,1,5,1,129,8,1,10,1,12,1,132,9,1,1,2,3,2,135,8,2,1,
		2,1,2,1,3,1,3,1,3,1,3,1,3,1,3,1,3,3,3,146,8,3,1,4,1,4,1,4,0,1,2,5,0,2,
		4,6,8,0,11,2,0,28,28,118,118,2,0,29,30,299,300,1,0,31,36,2,0,37,298,301,
		301,1,0,14,16,2,0,12,13,17,17,1,0,18,21,1,0,22,23,22,0,38,47,54,54,95,
		96,98,101,107,108,115,123,136,138,147,148,153,155,157,157,168,170,174,
		174,178,179,224,224,226,226,238,239,244,244,247,254,272,272,274,274,294,
		296,301,301,1,0,26,27,1,0,28,301,176,0,10,1,0,0,0,2,74,1,0,0,0,4,134,1,
		0,0,0,6,145,1,0,0,0,8,147,1,0,0,0,10,11,3,2,1,0,11,12,5,0,0,1,12,1,1,0,
		0,0,13,14,6,1,-1,0,14,15,5,1,0,0,15,16,3,2,1,0,16,17,5,2,0,0,17,75,1,0,
		0,0,18,20,3,4,2,0,19,21,7,0,0,0,20,19,1,0,0,0,20,21,1,0,0,0,21,75,1,0,
		0,0,22,75,5,27,0,0,23,75,5,301,0,0,24,25,5,7,0,0,25,75,3,2,1,14,26,29,
		7,1,0,0,27,28,5,1,0,0,28,30,5,2,0,0,29,27,1,0,0,0,29,30,1,0,0,0,30,75,
		1,0,0,0,31,32,7,2,0,0,32,33,5,1,0,0,33,75,5,2,0,0,34,35,7,3,0,0,35,36,
		5,1,0,0,36,41,3,2,1,0,37,38,5,4,0,0,38,40,3,2,1,0,39,37,1,0,0,0,40,43,
		1,0,0,0,41,39,1,0,0,0,41,42,1,0,0,0,42,44,1,0,0,0,43,41,1,0,0,0,44,45,
		5,2,0,0,45,75,1,0,0,0,46,47,5,10,0,0,47,52,3,6,3,0,48,49,5,4,0,0,49,51,
		3,6,3,0,50,48,1,0,0,0,51,54,1,0,0,0,52,50,1,0,0,0,52,53,1,0,0,0,53,56,
		1,0,0,0,54,52,1,0,0,0,55,57,5,4,0,0,56,55,1,0,0,0,56,57,1,0,0,0,57,58,
		1,0,0,0,58,59,5,11,0,0,59,75,1,0,0,0,60,61,5,5,0,0,61,66,3,2,1,0,62,63,
		5,4,0,0,63,65,3,2,1,0,64,62,1,0,0,0,65,68,1,0,0,0,66,64,1,0,0,0,66,67,
		1,0,0,0,67,70,1,0,0,0,68,66,1,0,0,0,69,71,5,4,0,0,70,69,1,0,0,0,70,71,
		1,0,0,0,71,72,1,0,0,0,72,73,5,6,0,0,73,75,1,0,0,0,74,13,1,0,0,0,74,18,
		1,0,0,0,74,22,1,0,0,0,74,23,1,0,0,0,74,24,1,0,0,0,74,26,1,0,0,0,74,31,
		1,0,0,0,74,34,1,0,0,0,74,46,1,0,0,0,74,60,1,0,0,0,75,130,1,0,0,0,76,77,
		10,12,0,0,77,78,7,4,0,0,78,129,3,2,1,13,79,80,10,11,0,0,80,81,7,5,0,0,
		81,129,3,2,1,12,82,83,10,10,0,0,83,84,7,6,0,0,84,129,3,2,1,11,85,86,10,
		9,0,0,86,87,7,7,0,0,87,129,3,2,1,10,88,89,10,8,0,0,89,90,5,24,0,0,90,129,
		3,2,1,9,91,92,10,7,0,0,92,93,5,25,0,0,93,129,3,2,1,8,94,95,10,16,0,0,95,
		96,5,3,0,0,96,97,7,8,0,0,97,106,5,1,0,0,98,103,3,2,1,0,99,100,5,4,0,0,
		100,102,3,2,1,0,101,99,1,0,0,0,102,105,1,0,0,0,103,101,1,0,0,0,103,104,
		1,0,0,0,104,107,1,0,0,0,105,103,1,0,0,0,106,98,1,0,0,0,106,107,1,0,0,0,
		107,108,1,0,0,0,108,129,5,2,0,0,109,118,10,15,0,0,110,113,5,5,0,0,111,
		114,5,301,0,0,112,114,3,2,1,0,113,111,1,0,0,0,113,112,1,0,0,0,114,115,
		1,0,0,0,115,119,5,6,0,0,116,117,5,3,0,0,117,119,3,8,4,0,118,110,1,0,0,
		0,118,116,1,0,0,0,119,129,1,0,0,0,120,121,10,13,0,0,121,129,5,16,0,0,122,
		123,10,6,0,0,123,124,5,8,0,0,124,125,3,2,1,0,125,126,5,9,0,0,126,127,3,
		2,1,0,127,129,1,0,0,0,128,76,1,0,0,0,128,79,1,0,0,0,128,82,1,0,0,0,128,
		85,1,0,0,0,128,88,1,0,0,0,128,91,1,0,0,0,128,94,1,0,0,0,128,109,1,0,0,
		0,128,120,1,0,0,0,128,122,1,0,0,0,129,132,1,0,0,0,130,128,1,0,0,0,130,
		131,1,0,0,0,131,3,1,0,0,0,132,130,1,0,0,0,133,135,5,13,0,0,134,133,1,0,
		0,0,134,135,1,0,0,0,135,136,1,0,0,0,136,137,5,26,0,0,137,5,1,0,0,0,138,
		139,7,9,0,0,139,140,5,9,0,0,140,146,3,2,1,0,141,142,3,8,4,0,142,143,5,
		9,0,0,143,144,3,2,1,0,144,146,1,0,0,0,145,138,1,0,0,0,145,141,1,0,0,0,
		146,7,1,0,0,0,147,148,7,10,0,0,148,9,1,0,0,0,16,20,29,41,52,56,66,70,74,
		103,106,113,118,128,130,134,145
	};
	public static readonly ATN _ATN =
		new ATNDeserializer().Deserialize(_serializedATN);
}

}