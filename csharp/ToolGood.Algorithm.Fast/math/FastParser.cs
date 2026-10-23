using System;
using System.Buffers;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using ToolGood.Algorithm.Internals;
using ToolGood.Algorithm.Internals.Functions;

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
	/// <para>
	/// 内存策略：词法单元数组、语法树节点数组、参数暂存数组全部向 <see cref="ArrayPool{T}"/> 租用，
	/// 单次解析的托管堆分配只剩「<see cref="FunctionBase"/> 树本身」与参数名字符串。
	/// </para>
	/// </summary>
	internal struct FastParser
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

		/// <summary>
		/// 逻辑嵌套上限（兜底，防止异常公式构造超大语法树）。
		/// 实际能到多深由 <see cref="RuntimeHelpers.TryEnsureSufficientExecutionStack"/> 按线程栈自适应决定：
		/// 默认 1 MB 线程栈约 2000 层，把公式放到更大栈的线程上还能更深。
		/// </summary>
		private const int MaxDepth = 5000;

		/// <summary>每隔多少层做一次剩余栈探测</summary>
		private const int StackProbeInterval = 16;

		private const int MinCapacity = 16;

		private readonly string _text;
		private FastLexeme[] _tokens;
		private int _tokenCount;
		private int _index;
		private int _lastEnd;

		private FastNode[] _nodes;
		private int _nodeCount;

		private int[] _scratch;
		private int _scratchCount;

		private int _depth;
		private int _root;

		internal FastParser(string text)
		{
			_text = text;
			_tokenCount = 0;
			_index = 0;
			_lastEnd = 0;
			_nodeCount = 0;
			_scratchCount = 0;
			_depth = 0;
			_root = -1;

			var capacity = text.Length / 4 + MinCapacity;
			_tokens = ArrayPool<FastLexeme>.Shared.Rent(capacity);
			_nodes = ArrayPool<FastNode>.Shared.Rent(capacity);
			_scratch = ArrayPool<int>.Shared.Rent(MinCapacity);
		}

		/// <summary>语法树节点数组（仅 [0, NodeCount) 有效）</summary>
		internal FastNode[] Nodes => _nodes;

		/// <summary>根节点下标</summary>
		internal int Root => _root;

		/// <summary>实际使用的节点数量</summary>
		internal int NodeCount => _nodeCount;

		/// <summary>把租用的缓冲区归还池，必须与构造成对出现</summary>
		internal void Release()
		{
			var tokens = _tokens;
			if(tokens != null) {
				_tokens = null;
				ArrayPool<FastLexeme>.Shared.Return(tokens);   // 纯值类型，无需清空
			}
			var nodes = _nodes;
			if(nodes != null) {
				_nodes = null;
				// 节点里可能残留字符串引用（参数名 / json key），必须清空，避免池中长期持有
				ArrayPool<FastNode>.Shared.Return(nodes, true);
			}
			var scratch = _scratch;
			if(scratch != null) {
				_scratch = null;
				ArrayPool<int>.Shared.Return(scratch);
			}
		}

		/// <summary>执行词法 + 语法分析，失败抛 <see cref="FormatException"/></summary>
		internal void Run()
		{
			Tokenize();
			_root = ParseProgram();
		}

		#region 对外入口（自动管理缓冲区生命周期）

		/// <summary>解析为可执行的函数树</summary>
		internal static FunctionBase ParseFunction(string text)
		{
			var parser = new FastParser(text);
			try {
				parser.Run();
				return FastAstBuilder.Build(text, parser._nodes, parser._nodeCount, parser._root);
			} finally {
				parser.Release();
			}
		}

		/// <summary>仅检查语法（不构造函数树），语法错误抛 <see cref="FormatException"/></summary>
		internal static void CheckSyntax(string text)
		{
			var parser = new FastParser(text);
			try {
				parser.Run();
			} finally {
				parser.Release();
			}
		}

		/// <summary>解析为条件树</summary>
		internal static ConditionTree ParseConditionTree(string text)
		{
			var parser = new FastParser(text);
			try {
				parser.Run();
				return FastTreeBuilder.ToConditionTree(text, parser._nodes, parser._root);
			} finally {
				parser.Release();
			}
		}

		/// <summary>解析为计算树</summary>
		internal static CalculateTree ParseCalculateTree(string text)
		{
			var parser = new FastParser(text);
			try {
				parser.Run();
				return FastTreeBuilder.ToCalculateTree(text, parser._nodes, parser._root);
			} finally {
				parser.Release();
			}
		}

		/// <summary>解析并收集自定义参数 / 自定义函数</summary>
		internal static void ParseDiyNames(string text, List<DiyNameKeyInfo> parameters, List<DiyNameKeyInfo> functions)
		{
			var parser = new FastParser(text);
			try {
				parser.Run();
				FastTreeBuilder.CollectDiyNames(parser._nodes, parser._root, parameters, functions);
			} finally {
				parser.Release();
			}
		}

		#endregion

		#region 词法

		private void Tokenize()
		{
			var lexer = new FastLexer(_text);
			var token = new FastLexeme();
			while(true) {
				if(!lexer.Next(ref token)) {
					throw new FormatException(lexer.ErrorMessage);
				}
				if(_tokenCount == _tokens.Length) { GrowTokens(); }
				_tokens[_tokenCount++] = token;
				if(token.Type == FastToken.EOF) { return; }
			}
		}

		private void GrowTokens()
		{
			var next = ArrayPool<FastLexeme>.Shared.Rent(_tokens.Length * 2);
			Array.Copy(_tokens, next, _tokenCount);
			ArrayPool<FastLexeme>.Shared.Return(_tokens);
			_tokens = next;
		}

		private void GrowNodes()
		{
			var next = ArrayPool<FastNode>.Shared.Rent(_nodes.Length * 2);
			Array.Copy(_nodes, next, _nodeCount);
			ArrayPool<FastNode>.Shared.Return(_nodes, true);
			_nodes = next;
		}

		private void GrowScratch()
		{
			var next = ArrayPool<int>.Shared.Rent(_scratch.Length * 2);
			Array.Copy(_scratch, next, _scratchCount);
			ArrayPool<int>.Shared.Return(_scratch);
			_scratch = next;
		}

		private int AddNode(FastNode node)
		{
			if(_nodeCount == _nodes.Length) { GrowNodes(); }
			_nodes[_nodeCount] = node;
			return _nodeCount++;
		}

		private void PushScratch(int node)
		{
			if(_scratchCount == _scratch.Length) { GrowScratch(); }
			_scratch[_scratchCount++] = node;
		}

		#endregion

		#region 语法

		private int ParseProgram()
		{
			var node = ParseExpr(0);
			if(Current.Type != FastToken.EOF) {
				throw Unexpected();
			}
			return node;
		}

		/// <summary>
		/// 优先级爬升主循环。
		/// <para>
		/// 为了在有限的线程栈上支持尽可能深的嵌套，这里刻意把「括号/前缀/后缀」直接内联，
		/// 并把二元运算循环挪到 <see cref="ParseBinaryRest"/>，于是每层括号嵌套只压 1 个栈帧
		/// （ParseExpr），而不是 4 个（旧的 ParseExpr+ParseUnary+ParsePostfix+ParsePrimary）。
		/// </para>
		/// </summary>
		private int ParseExpr(int minPrecedence)
		{
			if(++_depth > MaxDepth) {
				throw new FormatException($"Expression is nested too deeply (more than {MaxDepth} levels).");
			}
			// 逻辑上限之外，还要探测当前线程的实际剩余栈：
			// StackOverflow 无法 try/catch，会直接杀进程，必须提前失败。
			if((_depth & (StackProbeInterval - 1)) == 0 && !RuntimeHelpers.TryEnsureSufficientExecutionStack()) {
				throw new FormatException($"Expression is nested too deeply for the current thread stack (stopped at level {_depth}).");
			}

			int left;
			var token = Current;
			if(token.Type == FastToken.BANG) {
				Next();
				var operand = ParseExpr(PrecedenceUnary);
				left = AddNode(FastNode.Unary(FastNodeKind.Not, operand, token.Start, _nodes[operand].End));
			} else if(token.Type == FastToken.LPAREN) {
				// 括号在这里直接递归，少一个 ParsePrimary 栈帧
				Next();
				var inner = ParseExpr(0);
				Expect(FastToken.RPAREN);
				var innerNode = _nodes[inner];
				left = AddNode(FastNode.Unary(FastNodeKind.Bracket, inner, innerNode.Start, innerNode.End));
			} else {
				left = ParsePrimary();
				left = ParsePostfix(left);
			}

			left = ParseBinaryRest(left, minPrecedence);
			_depth--;
			return left;
		}

		/// <summary>二元/三目运算循环（优先级爬升）</summary>
		private int ParseBinaryRest(int left, int minPrecedence)
		{
			while(true) {
				var token = Current;
				int type = token.Type;
				bool postfixPercentage = false;
				int precedence = GetBinaryPrecedence(type);
				if(type == FastToken.OPMOD && !CanStartExpression(_index + 1)) {
					precedence = PrecedencePercentage;
					postfixPercentage = true;
				}
				if(precedence == 0 || precedence < minPrecedence) { return left; }

				Next();
				if(postfixPercentage) {
					left = AddNode(FastNode.Unary(FastNodeKind.Percentage, left, _nodes[left].Start, _lastEnd));
					continue;
				}
				if(type == FastToken.QUESTION) {
					// 与 Antlr 生成代码保持一致：'expr ? expr : expr' 未声明 assoc，
					// Antlr 按左结合生成 —— 中间项按 0 级解析、末项按 prec+1 解析；
					// 因此 a?b:c?d:e 等价于 If(If(a,b,c),d,e)，而 a?b?c:d:e 等价于 If(a, If(b,c,d), e)
					var whenTrue = ParseExpr(0);
					Expect(FastToken.COLON);
					var whenFalse = ParseExpr(precedence + 1);
					left = AddNode(FastNode.Three(FastNodeKind.If, left, whenTrue, whenFalse, _nodes[left].Start, _nodes[whenFalse].End));
					continue;
				}
				var right = ParseExpr(precedence + 1);
				left = AddNode(FastNode.Binary(GetBinaryKind(type), left, right, _nodes[left].Start, _nodes[right].End));
			}
		}

		/// <summary>后缀链：函数调用风格的 .name(...) 与下标/属性访问</summary>
		private int ParsePostfix(int left)
		{
			while(true) {
				if(Current.Type == FastToken.DOT) {
					left = ParseMemberAccess(left);
				} else if(Current.Type == FastToken.LBRACKET) {
					Next();
					var index = ParseExpr(0);
					Expect(FastToken.RBRACKET);
					left = AddNode(FastNode.Binary(FastNodeKind.GetJson, left, index, _nodes[left].Start, _lastEnd));
				} else {
					return left;
				}
			}
		}

		private int ParsePrimary()
		{
			var token = Current;
			switch(token.Type) {
				case FastToken.NUM:
					Next();
					return AddNode(FastNode.Leaf(FastNodeKind.Number, token.Start, token.End));

				case FastToken.OPSUB: {
					// 文法只允许 OPSUB? NUM 形式的负数字面量
					Next();
					if(Current.Type != FastToken.NUM) { throw Unexpected(); }
					var num = Current;
					Next();
					return AddNode(FastNode.Leaf(FastNodeKind.Number, token.Start, num.End));
				}
				case FastToken.STRING:
					Next();
					return AddNode(FastNode.Leaf(FastNodeKind.Text, token.Start, token.End));

				case FastToken.PARAMETER: {
					Next();
					if(Current.Type == FastToken.LPAREN) {
						return ParseCall(token, -1, false);
					}
					return AddNode(FastNode.Leaf(FastNodeKind.Parameter, token.Start, token.End, Slice(token)));
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
					return NewCall(token.Type, token.Start, _lastEnd, null, 0, 0);
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
					return NewCall(token.Type, token.Start, _lastEnd, null, 0, 0);
				}
				case FastToken.LBRACE:
					return ParseArrayJson();
				case FastToken.LBRACKET:
					return ParseArray();
				default:
					if(token.Type > 0 && token.Type <= FastToken.PARAMETER && FastToken.IsFunctionHead[token.Type]) {
						Next();
						return ParseCall(token, -1, false);
					}
					throw Unexpected();
			}
		}

		/// <summary>构造一个函数调用节点，参数来自暂存区 [offset, offset+count)</summary>
		private int NewCall(int type, int start, int end, string name, int offset, int count, int nameStart = 0)
		{
			var node = FastNode.Nary(FastNodeKind.Call, _scratch, offset, count, start, end, name);
			node.Type = type;
			node.NameStart = nameStart;
			return AddNode(node);
		}

		/// <summary>解析 <c>(a, b, ...)</c>；<paramref name="receiver"/> 非 -1 时为 <c>expr.name(...)</c> 形式</summary>
		private int ParseCall(FastLexeme name, int receiver, bool isMemberStyle)
		{
			Expect(FastToken.LPAREN);
			var mark = _scratchCount;
			if(receiver >= 0) { PushScratch(receiver); }

			if(Current.Type == FastToken.RPAREN) {
				// 文法中 NAME '(' expr (',' expr)* ')' 至少需要一个参数；
				// 而 expr.NAME '(' (expr (',' expr)*)? ')' 允许无参
				if(!isMemberStyle) {
					throw new FormatException($"Function '{Slice(name)}' requires at least one argument.");
				}
			} else {
				PushScratch(ParseExpr(0));
				while(Current.Type == FastToken.COMMA) {
					Next();
					PushScratch(ParseExpr(0));
				}
			}
			Expect(FastToken.RPAREN);

			var start = receiver >= 0 ? _nodes[receiver].Start : name.Start;
			var isDiy = name.Type == FastToken.PARAMETER;
			var node = NewCall(name.Type, start, _lastEnd, isDiy ? Slice(name) : null, mark, _scratchCount - mark, name.Start);
			_scratchCount = mark;
			return node;
		}

		/// <summary>解析 <c>expr.name</c> / <c>expr.name(...)</c></summary>
		private int ParseMemberAccess(int receiver)
		{
			Next();  // '.'
			var name = Current;
			if(name.Type == FastToken.PARAMETER) {
				Next();
				if(Current.Type == FastToken.LPAREN) {
					return ParseCall(name, receiver, true);
				}
				return AddNode(FastNode.Unary(FastNodeKind.GetJsonProperty, receiver, _nodes[receiver].Start, name.End).WithText(Slice(name)));
			}
			if(name.Type > 0 && name.Type <= FastToken.PARAMETER) {
				if(FastToken.IsMethodHead[name.Type] && Peek(1).Type == FastToken.LPAREN) {
					Next();
					return ParseCall(name, receiver, true);
				}
				if(FastToken.IsParameter2[name.Type]) {
					Next();
					return AddNode(FastNode.Unary(FastNodeKind.GetJsonProperty, receiver, _nodes[receiver].Start, name.End).WithText(Slice(name)));
				}
			}
			throw Unexpected();
		}

		private int ParseArray()
		{
			var token = Current;
			Next();  // '['
			var mark = _scratchCount;
			PushScratch(ParseExpr(0));
			while(Current.Type == FastToken.COMMA) {
				Next();
				if(Current.Type == FastToken.RBRACKET) { break; }  // 允许尾逗号
				PushScratch(ParseExpr(0));
			}
			Expect(FastToken.RBRACKET);
			var node = AddNode(FastNode.Nary(FastNodeKind.Array, _scratch, mark, _scratchCount - mark, token.Start, _lastEnd));
			_scratchCount = mark;
			return node;
		}

		private int ParseArrayJson()
		{
			var token = Current;
			Next();  // '{'
			var mark = _scratchCount;
			while(true) {
				PushScratch(ParseArrayJsonItem());
				if(Current.Type != FastToken.COMMA) { break; }
				Next();
				if(Current.Type == FastToken.RBRACE) { break; }  // 允许尾逗号
			}
			Expect(FastToken.RBRACE);
			var node = AddNode(FastNode.Nary(FastNodeKind.ArrayJson, _scratch, mark, _scratchCount - mark, token.Start, _lastEnd));
			_scratchCount = mark;
			return node;
		}

		private int ParseArrayJsonItem()
		{
			var key = Current;
			string keyText;
			if(key.Type == FastToken.STRING || key.Type == FastToken.NUM
				|| (key.Type > 0 && key.Type <= FastToken.PARAMETER && FastToken.IsParameter2[key.Type])) {
				Next();
				keyText = Slice(key);  // STRING 保留引号，由 builder 统一反转义
			} else {
				throw Unexpected();
			}
			Expect(FastToken.COLON);
			var value = ParseExpr(0);
			return AddNode(FastNode.Unary(FastNodeKind.ArrayJsonItem, value, key.Start, _nodes[value].End).WithText(keyText));
		}

		#endregion

		#region 辅助

		private FastLexeme Current {
			get { return _tokens[_index]; }
		}

		private FastLexeme Peek(int offset)
		{
			var i = _index + offset;
			return i < _tokenCount ? _tokens[i] : _tokens[_tokenCount - 1];
		}

		private void Next()
		{
			_lastEnd = Current.End;
			if(_index + 1 < _tokenCount) { _index++; }
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
			if(tokenIndex >= _tokenCount) { return false; }
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
					return tokenIndex + 1 < _tokenCount && _tokens[tokenIndex + 1].Type == FastToken.NUM;
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
