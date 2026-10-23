using System;
using System.Collections.Generic;

namespace ToolGood.Algorithm.math
{
	/// <summary>
	/// 手写递归下降语法分析器，等价替代 Antlr4 生成的 <c>mathParser</c>。
	/// <para>
	/// <c>math.g4</c> 中 <c>expr</c> 是左递归规则，Antlr 会把它改写成带优先级循环的实现；
	/// 这里用经典的「优先级爬升（precedence climbing）」还原同一套优先级，语义与文法一致。
	/// </para>
	/// <para>优先级由高到低：<c>函数调用/下标访问</c> &gt; <c>!</c> &gt; <c>%</c> &gt; <c>* / %</c>
	/// &gt; <c>+ - &amp;</c> &gt; <c>&gt; &gt;= &lt; &lt;=</c> &gt; <c>!= =</c> &gt; <c>&amp;&amp;</c>
	/// &gt; <c>||</c> &gt; <c>? :</c></para>
	/// </summary>
	internal sealed class FastParser
	{
		private const int PrecedenceUnary = 14;
		private const int PrecedencePercentage = 13;
		private const int PrecedenceMulDiv = 12;
		private const int PrecedenceAddSub = 11;
		private const int PrecedenceJudge = 10;
		private const int PrecedenceEquality = 9;
		private const int PrecedenceAnd = 8;
		private const int PrecedenceOr = 7;
		private const int PrecedenceConditional = 6;

		private readonly string _text;
		private FastLexeme[] _tokens;
		private int _count;
		private int _index;
		private int _lastEnd;

		private FastParser(string text)
		{
			_text = text;
		}

		#region 入口

		/// <summary>解析公式表达式，失败抛出 <see cref="FormatException"/></summary>
		internal static FastNode Parse(string text)
		{
			var parser = new FastParser(text);
			parser.Tokenize();
			return parser.ParseProgram();
		}

		/// <summary>解析公式表达式，失败返回 null 并通过 <paramref name="error"/> 返回原因</summary>
		internal static FastNode TryParse(string text, out string error)
		{
			try {
				error = null;
				return Parse(text);
			} catch(Exception ex) {
				error = ex.Message;
				return null;
			}
		}

		#endregion

		#region 词法

		private void Tokenize()
		{
			var lexer = new FastLexer(_text);
			var capacity = _text.Length / 3 + 8;
			_tokens = new FastLexeme[capacity < 8 ? 8 : capacity];
			_count = 0;
			var token = new FastLexeme();
			while(true) {
				if(!lexer.Next(ref token)) {
					throw new FormatException(lexer.ErrorMessage);
				}
				if(_count == _tokens.Length) {
					Array.Resize(ref _tokens, _tokens.Length * 2);
				}
				_tokens[_count++] = token;
				if(token.Type == FastToken.EOF) { return; }
			}
		}

		#endregion

		#region 语法

		private FastNode ParseProgram()
		{
			var node = ParseExpr(0);
			if(Current.Type != FastToken.EOF) {
				throw Unexpected();
			}
			return node;
		}

		/// <summary>优先级爬升主循环</summary>
		private FastNode ParseExpr(int minPrecedence)
		{
			var left = ParseUnary();
			while(true) {
				var token = Current;
				int type = token.Type;
				bool postfixPercentage = false;
				int precedence = GetBinaryPrecedence(type);
				if(type == FastToken.OPMOD && !CanStartExpression(_index + 1)) {
					precedence = PrecedencePercentage;
					postfixPercentage = true;
				}
				if(precedence == 0 || precedence < minPrecedence) { break; }

				Next();
				if(postfixPercentage) {
					left = FastNode.Unary(FastNodeKind.Percentage, left, left.Start);
					continue;
				}
				if(type == FastToken.QUESTION) {
					// 与 Antlr 生成代码保持一致：'expr ? expr : expr' 未声明 assoc，
					// Antlr 按左结合生成 —— 中间项按 0 级解析、末项按 prec+1 解析；
					// 因此 a?b:c?d:e 等价于 If(If(a,b,c),d,e)，而 a?b?c:d:e 等价于 If(a, If(b,c,d), e)
					var whenTrue = ParseExpr(0);
					Expect(FastToken.COLON);
					var whenFalse = ParseExpr(precedence + 1);
					left = FastNode.Nary(FastNodeKind.If, new[] { left, whenTrue, whenFalse }, left.Start, whenFalse.End);
					continue;
				}
				var right = ParseExpr(precedence + 1);
				left = FastNode.Binary(GetBinaryKind(type), left, right);
			}
			return left;
		}

		private FastNode ParseUnary()
		{
			if(Current.Type == FastToken.BANG) {
				var token = Current;
				Next();
				var operand = ParseExpr(PrecedenceUnary);
				return FastNode.Unary(FastNodeKind.Not, operand, token.Start);
			}
			return ParsePostfix();
		}

		/// <summary>基础表达式 + 后缀（函数调用风格的 .name(...) 与下标/属性访问）</summary>
		private FastNode ParsePostfix()
		{
			var left = ParsePrimary();
			while(true) {
				if(Current.Type == FastToken.DOT) {
					left = ParseMemberAccess(left);
				} else if(Current.Type == FastToken.LBRACKET) {
					Next();
					var index = ParseExpr(0);
					Expect(FastToken.RBRACKET);
					left = FastNode.Nary(FastNodeKind.GetJson, new[] { left, index }, left.Start, _lastEnd);
				} else {
					return left;
				}
			}
		}

		private FastNode ParsePrimary()
		{
			var token = Current;
			switch(token.Type) {
				case FastToken.LPAREN: {
					Next();
					var inner = ParseExpr(0);
					Expect(FastToken.RPAREN);
					return FastNode.Unary(FastNodeKind.Bracket, inner, inner.Start);
				}
				case FastToken.NUM:
					Next();
					return FastNode.Leaf(FastNodeKind.Number, token.Start, token.End);

				case FastToken.OPSUB: {
					// 文法只允许 OPSUB? NUM 形式的负数字面量
					Next();
					if(Current.Type != FastToken.NUM) { throw Unexpected(); }
					var num = Current;
					Next();
					return FastNode.Leaf(FastNodeKind.Number, token.Start, num.End);
				}
				case FastToken.STRING:
					Next();
					return FastNode.Leaf(FastNodeKind.Text, token.Start, token.End);

				case FastToken.PARAMETER: {
					Next();
					if(Current.Type == FastToken.LPAREN) {
						return ParseCall(token, null, false);
					}
					return FastNode.Leaf(FastNodeKind.Parameter, token.Start, token.End, Slice(token));
				}
				case FastToken.TRUE:
				case FastToken.FALSE:
				case FastToken.NULL:
				case FastToken.ALGORITHMVERSION: {
					Next();
					// ('(' ')')?  允许 TRUE() 这种写法
					if(Current.Type == FastToken.LPAREN && Peek(1).Type == FastToken.RPAREN) {
						Next();
						Next();
					}
					return FastNode.Call(token.Type, null, token.Start, token.End, FastNode.Empty, token.Start, _lastEnd);
				}
				case FastToken.RAND:
				case FastToken.NOW:
				case FastToken.TODAY:
				case FastToken.GUID:
				case FastToken.E:
				case FastToken.PI: {
					Next();
					Expect(FastToken.LPAREN);
					Expect(FastToken.RPAREN);
					return FastNode.Call(token.Type, null, token.Start, token.End, FastNode.Empty, token.Start, _lastEnd);
				}
				case FastToken.LBRACE:
					return ParseArrayJson();
				case FastToken.LBRACKET:
					return ParseArray();
				default:
					if(token.Type > 0 && token.Type <= FastToken.PARAMETER && FastToken.IsFunctionHead[token.Type]) {
						Next();
						return ParseCall(token, null, false);
					}
					throw Unexpected();
			}
		}

		/// <summary>解析 <c>(a, b, ...)</c>；<paramref name="receiver"/> 非空时为 <c>expr.name(...)</c> 形式</summary>
		private FastNode ParseCall(FastLexeme name, FastNode receiver, bool isMemberStyle)
		{
			Expect(FastToken.LPAREN);
			var args = new List<FastNode>(4);
			if(receiver != null) { args.Add(receiver); }

			if(Current.Type == FastToken.RPAREN) {
				// 文法中 NAME '(' expr (',' expr)* ')' 至少需要一个参数；
				// 而 expr.NAME '(' (expr (',' expr)*)? ')' 允许无参
				if(!isMemberStyle) {
					throw new FormatException($"Function '{Slice(name)}' requires at least one argument.");
				}
			} else {
				args.Add(ParseExpr(0));
				while(Current.Type == FastToken.COMMA) {
					Next();
					args.Add(ParseExpr(0));
				}
			}
			Expect(FastToken.RPAREN);

			var nameText = name.Type == FastToken.PARAMETER ? Slice(name) : null;
			return FastNode.Call(name.Type, nameText, name.Start, name.End, args.ToArray(),
				receiver == null ? name.Start : receiver.Start, _lastEnd);
		}

		/// <summary>解析 <c>expr.name</c> / <c>expr.name(...)</c></summary>
		private FastNode ParseMemberAccess(FastNode receiver)
		{
			Next();  // '.'
			var name = Current;
			if(name.Type == FastToken.PARAMETER) {
				Next();
				if(Current.Type == FastToken.LPAREN) {
					return ParseCall(name, receiver, true);
				}
				return FastNode.WithText(FastNodeKind.GetJsonProperty, new[] { receiver }, receiver.Start, name.End, Slice(name));
			}
			if(name.Type > 0 && name.Type <= FastToken.PARAMETER) {
				if(FastToken.IsMethodHead[name.Type] && Peek(1).Type == FastToken.LPAREN) {
					Next();
					return ParseCall(name, receiver, true);
				}
				if(FastToken.IsParameter2[name.Type]) {
					Next();
					return FastNode.WithText(FastNodeKind.GetJsonProperty, new[] { receiver }, receiver.Start, name.End, Slice(name));
				}
			}
			throw Unexpected();
		}

		private FastNode ParseArray()
		{
			var token = Current;
			Next();  // '['
			var items = new List<FastNode>(4);
			items.Add(ParseExpr(0));
			while(Current.Type == FastToken.COMMA) {
				Next();
				if(Current.Type == FastToken.RBRACKET) { break; }  // 允许尾逗号
				items.Add(ParseExpr(0));
			}
			Expect(FastToken.RBRACKET);
			return FastNode.Nary(FastNodeKind.Array, items.ToArray(), token.Start, _lastEnd);
		}

		private FastNode ParseArrayJson()
		{
			var token = Current;
			Next();  // '{'
			var items = new List<FastNode>(4);
			while(true) {
				items.Add(ParseArrayJsonItem());
				if(Current.Type != FastToken.COMMA) { break; }
				Next();
				if(Current.Type == FastToken.RBRACE) { break; }  // 允许尾逗号
			}
			Expect(FastToken.RBRACE);
			return FastNode.Nary(FastNodeKind.ArrayJson, items.ToArray(), token.Start, _lastEnd);
		}

		private FastNode ParseArrayJsonItem()
		{
			var key = Current;
			string keyText;
			if(key.Type == FastToken.STRING) {
				Next();
				keyText = Slice(key);  // 保留引号，由调用方统一反转义
			} else if(key.Type == FastToken.NUM) {
				Next();
				keyText = Slice(key);
			} else if(key.Type > 0 && key.Type <= FastToken.PARAMETER && FastToken.IsParameter2[key.Type]) {
				Next();
				keyText = Slice(key);
			} else {
				throw Unexpected();
			}
			Expect(FastToken.COLON);
			var value = ParseExpr(0);
			return FastNode.WithText(FastNodeKind.ArrayJsonItem, new[] { value }, key.Start, value.End, keyText);
		}

		#endregion

		#region 辅助

		private FastLexeme Current {
			get { return _tokens[_index]; }
		}

		private FastLexeme Peek(int offset)
		{
			var i = _index + offset;
			return i < _count ? _tokens[i] : _tokens[_count - 1];
		}

		private void Next()
		{
			_lastEnd = Current.End;
			if(_index + 1 < _count) { _index++; }
		}

		private void Expect(int type)
		{
			if(Current.Type != type) { throw Unexpected(); }
			Next();
		}

		private string Slice(FastLexeme token)
		{
			if(token.End < token.Start) { return string.Empty; }
			return _text.Substring(token.Start, token.End - token.Start + 1);
		}

		private FormatException Unexpected()
		{
			var token = Current;
			if(token.Type == FastToken.EOF) {
				return new FormatException($"Unexpected end of expression at position {_text.Length}.");
			}
			return new FormatException($"Unexpected token '{Slice(token)}' at position {token.Start}.");
		}

		/// <summary>判断从 tokenIndex 开始的词法单元能否作为一个表达式的开头</summary>
		private bool CanStartExpression(int tokenIndex)
		{
			if(tokenIndex >= _count) { return false; }
			int type = _tokens[tokenIndex].Type;
			switch(type) {
				case FastToken.LPAREN:
				case FastToken.NUM:
				case FastToken.STRING:
				case FastToken.PARAMETER:
				case FastToken.BANG:
				case FastToken.LBRACE:
				case FastToken.LBRACKET:
				case FastToken.TRUE:
				case FastToken.FALSE:
				case FastToken.NULL:
				case FastToken.ALGORITHMVERSION:
				case FastToken.RAND:
				case FastToken.NOW:
				case FastToken.TODAY:
				case FastToken.GUID:
				case FastToken.E:
				case FastToken.PI:
					return true;
				case FastToken.OPSUB:
					// 负号只能引导数字字面量
					return tokenIndex + 1 < _count && _tokens[tokenIndex + 1].Type == FastToken.NUM;
				default:
					return type > 0 && type <= FastToken.PARAMETER && FastToken.IsFunctionHead[type];
			}
		}

		private static int GetBinaryPrecedence(int type)
		{
			switch(type) {
				case FastToken.OPMOD:
				case FastToken.OPMUL:
				case FastToken.OPDIV: return PrecedenceMulDiv;
				case FastToken.OPADD:
				case FastToken.OPSUB:
				case FastToken.OPCAT: return PrecedenceAddSub;
				case FastToken.OPGT:
				case FastToken.OPGE:
				case FastToken.OPLT:
				case FastToken.OPLE: return PrecedenceJudge;
				case FastToken.OPNE:
				case FastToken.OPEQ: return PrecedenceEquality;
				case FastToken.OPAND: return PrecedenceAnd;
				case FastToken.OPOR: return PrecedenceOr;
				case FastToken.QUESTION: return PrecedenceConditional;
				default: return 0;
			}
		}

		private static FastNodeKind GetBinaryKind(int type)
		{
			switch(type) {
				case FastToken.OPMUL: return FastNodeKind.Mul;
				case FastToken.OPDIV: return FastNodeKind.Div;
				case FastToken.OPMOD: return FastNodeKind.Mod;
				case FastToken.OPADD: return FastNodeKind.Add;
				case FastToken.OPSUB: return FastNodeKind.Sub;
				case FastToken.OPCAT: return FastNodeKind.Concat;
				case FastToken.OPGT: return FastNodeKind.GreaterThan;
				case FastToken.OPGE: return FastNodeKind.GreaterOrEqual;
				case FastToken.OPLT: return FastNodeKind.LessThan;
				case FastToken.OPLE: return FastNodeKind.LessOrEqual;
				case FastToken.OPEQ: return FastNodeKind.Equal;
				case FastToken.OPNE: return FastNodeKind.NotEqual;
				case FastToken.OPAND: return FastNodeKind.And;
				case FastToken.OPOR: return FastNodeKind.Or;
				default: return FastNodeKind.Add;
			}
		}

		#endregion
	}
}
