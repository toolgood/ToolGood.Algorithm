using System;
using System.Buffers;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using ToolGood.Algorithm.Internals;
using ToolGood.Algorithm.Internals.Functions;

namespace ToolGood.Algorithm.math
{
	/// <summary>
	/// 解析期缓冲区：按线程常驻，避免每条公式都向 <see cref="ArrayPool{T}"/> 借还。
	/// <para>
	/// 拆解实测：一条 20 字符公式的解析约 1.2 µs，其中约 100 ns 花在「三个数组的 Rent/Return」上
	/// （单次 Rent/Return 约 25~40 ns），占固定开销六成左右。绝大多数公式都在 1 KB 以内，
	/// 用线程本地数组可以把这部分省掉。
	/// </para>
	/// <para>
	/// 容量上限 <see cref="MaxThreadLocalCapacity"/> 之外的公式仍走 <see cref="ArrayPool{T}"/>，
	/// 避免为超长公式在每个线程上常驻大数组；同一线程上的重入解析也回落到池。
	/// </para>
	/// <para>
	/// 归还时清空「实际写过」的区间：<see cref="FastNode"/> 里有字符串、
	/// <see cref="FastHandle"/> 里有整棵函数树，不清会让线程本地长期持有这些引用。
	/// </para>
	/// </summary>
	internal static class FastBuffers
	{
		/// <summary>首次分配的容量</summary>
		private const int MinCapacity = 64;

		/// <summary>线程本地缓冲的最大容量，超出的公式改用共享池</summary>
		private const int MaxThreadLocalCapacity = 1024;

		[ThreadStatic] private static FastLexeme[] _tokens;
		[ThreadStatic] private static FastNode[] _nodes;
		[ThreadStatic] private static FastHandle[] _children;
		[ThreadStatic] private static bool _busy;

		/// <summary>
		/// 尝试取本线程的缓冲。返回 false 表示本次解析应改用 <see cref="ArrayPool{T}"/>
		/// （容量过大或同线程已有解析在进行）。
		/// </summary>
		internal static bool TryAcquire(int capacity, bool needNodes, out FastLexeme[] tokens, out FastNode[] nodes, out FastHandle[] children)
		{
			if(_busy || capacity > MaxThreadLocalCapacity) {
				tokens = null;
				nodes = null;
				children = null;
				return false;
			}
			if(_tokens == null || _tokens.Length < capacity) {
				var size = MinCapacity;
				while(size < capacity) { size <<= 1; }
				_tokens = new FastLexeme[size];
				_children = new FastHandle[size];
				// 容量变了，节点数组的旧尺寸作废，按需重建
				_nodes = needNodes ? new FastNode[size] : null;
			} else if(needNodes && (_nodes == null || _nodes.Length < _tokens.Length)) {
				_nodes = new FastNode[_tokens.Length];
			}
			_busy = true;
			tokens = _tokens;
			nodes = needNodes ? _nodes : null;
			children = _children;
			return true;
		}

		/// <summary>归还本线程的缓冲，并清掉写过的槽位上的引用</summary>
		internal static void Release(int nodeCount, int childCount)
		{
			var nodes = _nodes;
			if(nodes != null && nodeCount > 0) { Array.Clear(nodes, 0, nodeCount); }
			var children = _children;
			if(children != null && childCount > 0) { Array.Clear(children, 0, childCount); }
			_busy = false;
		}
	}

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
	/// 解析途中就地构造 <see cref="FunctionBase"/> 树，不再先落一份语法树节点数组再回扫一遍：
	/// 常量子树的折叠判断跟着句柄上的「是否常量」标记走，节点数组只在条件树 / 计算树 /
	/// DIY 名称收集这些需要原文位置信息的旁路上才记录。
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
		/// <summary>是否构建 <see cref="FunctionBase"/> 树</summary>
		private readonly bool _build;
		/// <summary>是否记录 <see cref="FastNode"/> 节点表（旁路需要原文位置信息）</summary>
		private readonly bool _record;

		private FastLexeme[] _tokens;
		private int _tokenCount;
		private int _index;
		private int _lastEnd;

		private FastNode[] _nodes;
		private int _nodeCount;

		private FastHandle[] _children;
		private int _childCount;
		/// <summary>暂存区达到过的最大深度，归还缓冲时据此清引用</summary>
		private int _childHighWater;

		private FunctionBase _rootFn;
		private int _rootNode;

		private int _depth;
		private bool _fromThreadLocal;

		private FastParser(string text, bool build, bool record)
		{
			_text = text;
			_build = build;
			_record = record;
			_tokenCount = 0;
			_index = 0;
			_lastEnd = 0;
			_nodeCount = 0;
			_childCount = 0;
			_childHighWater = 0;
			_rootFn = null;
			_rootNode = -1;
			_depth = 0;
			_fromThreadLocal = false;

			// 词法单元最多「每字符一个」再加 EOF，节点数与暂存深度都不会超过它。
			// 超长公式退化为估算，避免为极端输入租下过大的数组。
			var capacity = EstimateCapacity(text.Length);
			if(FastBuffers.TryAcquire(capacity, record, out var tokens, out var nodes, out var children)) {
				_tokens = tokens;
				_nodes = nodes;
				_children = children;
				_fromThreadLocal = true;
			} else {
				_tokens = ArrayPool<FastLexeme>.Shared.Rent(capacity);
				_children = ArrayPool<FastHandle>.Shared.Rent(capacity);
				_nodes = record ? ArrayPool<FastNode>.Shared.Rent(capacity) : null;
			}
		}

		private static int EstimateCapacity(int length)
		{
			if(length <= 4096) { return Math.Max(length + 1, MinCapacity); }
			return Math.Max(length / 2, MinCapacity);
		}

		/// <summary>把缓冲区归还，必须与构造成对出现</summary>
		internal void Release()
		{
			if(_fromThreadLocal) {
				FastBuffers.Release(_nodeCount, _childHighWater);
				_tokens = null;
				_nodes = null;
				_children = null;
				return;
			}
			var tokens = _tokens;
			if(tokens != null) {
				_tokens = null;
				ArrayPool<FastLexeme>.Shared.Return(tokens);   // 纯值类型，无需清空
			}
			var nodes = _nodes;
			if(nodes != null) {
				_nodes = null;
				// 节点里可能残留字符串引用（参数名 / json key），必须清空，避免池中长期持有。
				// 只清实际用到的区间：租来的数组通常比 _nodeCount 大（池按 2 的幂给），
				// 整段清空会白白抹掉大量未使用的槽位。
				Array.Clear(nodes, 0, _nodeCount);
				ArrayPool<FastNode>.Shared.Return(nodes);
			}
			var children = _children;
			if(children != null) {
				_children = null;
				// 句柄里是整棵函数树，同样要清
				Array.Clear(children, 0, _childHighWater);
				ArrayPool<FastHandle>.Shared.Return(children);
			}
		}

		/// <summary>执行词法 + 语法分析，失败抛 <see cref="FormatException"/></summary>
		internal void Run()
		{
			Tokenize();
			var root = ParseExpr(0);
			if(Current.Type != FastToken.EOF) {
				throw Unexpected();
			}
			_rootFn = root.Fn;
			_rootNode = root.NodeIdx;
		}

		#region 对外入口（自动管理缓冲区生命周期）

		/// <summary>解析为可执行的函数树</summary>
		internal static FunctionBase ParseFunction(string text)
		{
			var parser = new FastParser(text, true, false);
			try {
				parser.Run();
				return parser._rootFn;
			} finally {
				parser.Release();
			}
		}

		/// <summary>仅检查语法（不构造函数树），语法错误抛 <see cref="FormatException"/></summary>
		internal static void CheckSyntax(string text)
		{
			var parser = new FastParser(text, false, false);
			try {
				parser.Run();
			} finally {
				parser.Release();
			}
		}

		/// <summary>解析为条件树</summary>
		internal static ConditionTree ParseConditionTree(string text)
		{
			var parser = new FastParser(text, false, true);
			try {
				parser.Run();
				return FastTreeBuilder.ToConditionTree(text, parser._nodes, parser._rootNode);
			} finally {
				parser.Release();
			}
		}

		/// <summary>解析为计算树</summary>
		internal static CalculateTree ParseCalculateTree(string text)
		{
			var parser = new FastParser(text, false, true);
			try {
				parser.Run();
				return FastTreeBuilder.ToCalculateTree(text, parser._nodes, parser._rootNode);
			} finally {
				parser.Release();
			}
		}

		/// <summary>解析并收集自定义参数 / 自定义函数</summary>
		internal static void ParseDiyNames(string text, List<DiyNameKeyInfo> parameters, List<DiyNameKeyInfo> functions)
		{
			var parser = new FastParser(text, false, true);
			try {
				parser.Run();
				FastTreeBuilder.CollectDiyNames(parser._nodes, parser._rootNode, parameters, functions);
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
			Array.Clear(_nodes, 0, _nodeCount);   // 只清有效区间，见 Release
			ArrayPool<FastNode>.Shared.Return(_nodes);
			_nodes = next;
		}

		private void GrowChildren()
		{
			var next = ArrayPool<FastHandle>.Shared.Rent(_children.Length * 2);
			Array.Copy(_children, next, _childCount);
			Array.Clear(_children, 0, _childCount);
			ArrayPool<FastHandle>.Shared.Return(_children);
			_children = next;
		}

		private int AddNode(FastNode node)
		{
			if(_nodeCount == _nodes.Length) { GrowNodes(); }
			_nodes[_nodeCount] = node;
			return _nodeCount++;
		}

		private void PushChild(FastHandle handle)
		{
			if(_childCount == _children.Length) { GrowChildren(); }
			_children[_childCount++] = handle;
			if(_childCount > _childHighWater) { _childHighWater = _childCount; }
		}

		#endregion

		#region 节点构造

		/// <summary>子节点的原文起点（仅记录模式有效）</summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private int NodeStart(FastHandle handle)
		{
			return _record ? _nodes[handle.NodeIdx].Start : 0;
		}

		/// <summary>子节点的原文终点（仅记录模式有效）</summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private int NodeEnd(FastHandle handle)
		{
			return _record ? _nodes[handle.NodeIdx].End : 0;
		}

		/// <summary>叶子节点：数值 / 字符串 / 参数名</summary>
		private FastHandle MakeLeaf(FastNodeKind kind, int start, int end, string text)
		{
			FastHandle handle = default;
			handle.NodeIdx = -1;
			if(_build) {
				if(kind == FastNodeKind.Number) {
					handle.Fn = FastAstBuilder.CreateNumber(_text, start, end);
				} else if(kind == FastNodeKind.Text) {
					handle.Fn = FastAstBuilder.CreateText(_text, start, end);
				} else {
					handle.Fn = FastAstBuilder.CreateParameter(text);
				}
				handle.Const = FastAstBuilder.IsConstant(kind, 0, handle.Fn, _children, 0);
			}
			if(_record) {
				handle.NodeIdx = AddNode(FastNode.Leaf(kind, start, end, text));
			}
			return handle;
		}

		/// <summary>
		/// 组合节点：子节点已压入暂存区 [<paramref name="mark"/>, <see cref="_childCount"/>) 区间。
		/// 直接构建模式下就地构造函数树并尝试常量折叠；记录模式下同时写语法树节点。
		/// </summary>
		private FastHandle MakeNode(FastNodeKind kind, int type, int mark, int start, int end, string text, int nameStart)
		{
			var count = _childCount - mark;
			FastHandle handle = default;
			handle.NodeIdx = -1;
			if(_build) {
				var fn = FastAstBuilder.Create(kind, type, _children, mark, count, text);
				var isConst = FastAstBuilder.IsConstant(kind, type, fn, _children, mark);
				if(!isConst && FastAstBuilder.CanFold(kind, type, _children, mark, count)) {
					// 纯常量子树在编译期先算好；算出错误/异常值则保持原节点，
					// 让错误仍发生在求值期，与折叠前行为一致
					var result = fn.Evaluate(FastAstBuilder.FoldEngine, null);
					if(!result.IsErrorOrNone) {
						var folded = FastAstBuilder.WrapConstant(result);
						if(folded != null) {
							fn = folded;
							isConst = true;
						}
					}
				}
				handle.Fn = fn;
				handle.Const = isConst;
			}
			if(_record) {
				var node = FastNode.Nary(kind, _children, mark, count, start, end, text);
				node.Type = type;
				node.NameStart = nameStart;
				handle.NodeIdx = AddNode(node);
			}
			return handle;
		}

		/// <summary>压入一个子节点后构造组合节点，并恢复暂存区</summary>
		private FastHandle MakeUnary(FastNodeKind kind, FastHandle child, int start, int end, string text = null)
		{
			var mark = _childCount;
			PushChild(child);
			var handle = MakeNode(kind, 0, mark, start, end, text, 0);
			_childCount = mark;
			return handle;
		}

		#endregion

		#region 语法

		/// <summary>
		/// 优先级爬升主循环。
		/// <para>
		/// 为了在有限的线程栈上支持尽可能深的嵌套，这里刻意把「括号/前缀/后缀」直接内联，
		/// 并把二元运算循环挪到 <see cref="ParseBinaryRest"/>，于是每层括号嵌套只压 1 个栈帧。
		/// </para>
		/// </summary>
		private FastHandle ParseExpr(int minPrecedence)
		{
			if(++_depth > MaxDepth) {
				throw new FormatException($"Expression is nested too deeply (more than {MaxDepth} levels).");
			}
			// 逻辑上限之外，还要探测当前线程的实际剩余栈：
			// StackOverflow 无法 try/catch，会直接杀进程，必须提前失败。
			if((_depth & (StackProbeInterval - 1)) == 0 && !RuntimeHelpers.TryEnsureSufficientExecutionStack()) {
				throw new FormatException($"Expression is nested too deeply for the current thread stack (stopped at level {_depth}).");
			}

			FastHandle left;
			var token = Current;
			if(token.Type == FastToken.BANG) {
				Next();
				var operand = ParseExpr(PrecedenceUnary);
				left = MakeUnary(FastNodeKind.Not, operand, token.Start, NodeEnd(operand));
			} else if(token.Type == FastToken.LPAREN) {
				// 括号在这里直接递归，少一个 ParsePrimary 栈帧
				Next();
				var inner = ParseExpr(0);
				Expect(FastToken.RPAREN);
				left = MakeUnary(FastNodeKind.Bracket, inner, NodeStart(inner), NodeEnd(inner));
			} else {
				left = ParsePrimary();
				left = ParsePostfix(left);
			}

			left = ParseBinaryRest(left, minPrecedence);
			_depth--;
			return left;
		}

		/// <summary>二元/三目运算循环（优先级爬升）</summary>
		private FastHandle ParseBinaryRest(FastHandle left, int minPrecedence)
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
					left = MakeUnary(FastNodeKind.Percentage, left, NodeStart(left), _lastEnd);
					continue;
				}
				if(type == FastToken.QUESTION) {
					// 与 Antlr 生成代码保持一致：'expr ? expr : expr' 未声明 assoc，
					// Antlr 按左结合生成 —— 中间项按 0 级解析、末项按 prec+1 解析；
					// 因此 a?b:c?d:e 等价于 If(If(a,b,c),d,e)，而 a?b?c:d:e 等价于 If(a, If(b,c,d), e)
					var whenTrue = ParseExpr(0);
					Expect(FastToken.COLON);
					var whenFalse = ParseExpr(precedence + 1);
					var mark = _childCount;
					PushChild(left);
					PushChild(whenTrue);
					PushChild(whenFalse);
					left = MakeNode(FastNodeKind.If, 0, mark, NodeStart(left), NodeEnd(whenFalse), null, 0);
					_childCount = mark;
					continue;
				}
				var right = ParseExpr(precedence + 1);
				var m = _childCount;
				PushChild(left);
				PushChild(right);
				left = MakeNode(GetBinaryKind(type), 0, m, NodeStart(left), NodeEnd(right), null, 0);
				_childCount = m;
			}
		}

		/// <summary>后缀链：函数调用风格的 .name(...) 与下标/属性访问</summary>
		private FastHandle ParsePostfix(FastHandle left)
		{
			while(true) {
				if(Current.Type == FastToken.DOT) {
					left = ParseMemberAccess(left);
				} else if(Current.Type == FastToken.LBRACKET) {
					Next();
					var index = ParseExpr(0);
					Expect(FastToken.RBRACKET);
					var mark = _childCount;
					PushChild(left);
					PushChild(index);
					left = MakeNode(FastNodeKind.GetJson, 0, mark, NodeStart(left), _lastEnd, null, 0);
					_childCount = mark;
				} else {
					return left;
				}
			}
		}

		private FastHandle ParsePrimary()
		{
			var token = Current;
			switch(token.Type) {
				case FastToken.NUM:
					Next();
					return MakeLeaf(FastNodeKind.Number, token.Start, token.End, null);

				case FastToken.OPSUB: {
					// 文法只允许 OPSUB? NUM 形式的负数字面量
					Next();
					if(Current.Type != FastToken.NUM) { throw Unexpected(); }
					var num = Current;
					Next();
					return MakeLeaf(FastNodeKind.Number, token.Start, num.End, null);
				}
				case FastToken.STRING:
					Next();
					return MakeLeaf(FastNodeKind.Text, token.Start, token.End, null);

				case FastToken.PARAMETER: {
					Next();
					if(Current.Type == FastToken.LPAREN) {
						return ParseCall(token, false, default, false);
					}
					return MakeLeaf(FastNodeKind.Parameter, token.Start, token.End, Slice(token));
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
					return MakeNode(FastNodeKind.Call, token.Type, _childCount, token.Start, _lastEnd, null, 0);
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
					return MakeNode(FastNodeKind.Call, token.Type, _childCount, token.Start, _lastEnd, null, 0);
				}
				case FastToken.LBRACE:
					return ParseArrayJson();
				case FastToken.LBRACKET:
					return ParseArray();
				default:
					if(token.Type > 0 && token.Type <= FastToken.PARAMETER && FastToken.IsFunctionHead[token.Type]) {
						Next();
						return ParseCall(token, false, default, false);
					}
					throw Unexpected();
			}
		}

		/// <summary>解析 <c>(a, b, ...)</c>；<paramref name="hasReceiver"/> 为 true 时是 <c>expr.name(...)</c> 形式</summary>
		private FastHandle ParseCall(FastLexeme name, bool hasReceiver, FastHandle receiver, bool isMemberStyle)
		{
			Expect(FastToken.LPAREN);
			var mark = _childCount;
			if(hasReceiver) { PushChild(receiver); }

			if(Current.Type == FastToken.RPAREN) {
				// 文法中 NAME '(' expr (',' expr)* ')' 至少需要一个参数；
				// 而 expr.NAME '(' (expr (',' expr)*)? ')' 允许无参
				if(!isMemberStyle) {
					throw new FormatException($"Function '{Slice(name)}' requires at least one argument.");
				}
			} else {
				PushChild(ParseExpr(0));
				while(Current.Type == FastToken.COMMA) {
					Next();
					PushChild(ParseExpr(0));
				}
			}
			Expect(FastToken.RPAREN);

			var start = hasReceiver ? NodeStart(receiver) : name.Start;
			var isDiy = name.Type == FastToken.PARAMETER;
			var handle = MakeNode(FastNodeKind.Call, name.Type, mark, start, _lastEnd, isDiy ? Slice(name) : null, name.Start);
			_childCount = mark;
			return handle;
		}

		/// <summary>解析 <c>expr.name</c> / <c>expr.name(...)</c></summary>
		private FastHandle ParseMemberAccess(FastHandle receiver)
		{
			Next();  // '.'
			var name = Current;
			if(name.Type == FastToken.PARAMETER) {
				Next();
				if(Current.Type == FastToken.LPAREN) {
					return ParseCall(name, true, receiver, true);
				}
				return MakeUnary(FastNodeKind.GetJsonProperty, receiver, NodeStart(receiver), name.End, Slice(name));
			}
			if(name.Type > 0 && name.Type <= FastToken.PARAMETER) {
				if(FastToken.IsMethodHead[name.Type] && Peek(1).Type == FastToken.LPAREN) {
					Next();
					return ParseCall(name, true, receiver, true);
				}
				if(FastToken.IsParameter2[name.Type]) {
					Next();
					return MakeUnary(FastNodeKind.GetJsonProperty, receiver, NodeStart(receiver), name.End, Slice(name));
				}
			}
			throw Unexpected();
		}

		private FastHandle ParseArray()
		{
			var token = Current;
			Next();  // '['
			var mark = _childCount;
			PushChild(ParseExpr(0));
			while(Current.Type == FastToken.COMMA) {
				Next();
				if(Current.Type == FastToken.RBRACKET) { break; }  // 允许尾逗号
				PushChild(ParseExpr(0));
			}
			Expect(FastToken.RBRACKET);
			var handle = MakeNode(FastNodeKind.Array, 0, mark, token.Start, _lastEnd, null, 0);
			_childCount = mark;
			return handle;
		}

		private FastHandle ParseArrayJson()
		{
			var token = Current;
			Next();  // '{'
			var mark = _childCount;
			while(true) {
				PushChild(ParseArrayJsonItem());
				if(Current.Type != FastToken.COMMA) { break; }
				Next();
				if(Current.Type == FastToken.RBRACE) { break; }  // 允许尾逗号
			}
			Expect(FastToken.RBRACE);
			var handle = MakeNode(FastNodeKind.ArrayJson, 0, mark, token.Start, _lastEnd, null, 0);
			_childCount = mark;
			return handle;
		}

		private FastHandle ParseArrayJsonItem()
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
			return MakeUnary(FastNodeKind.ArrayJsonItem, value, key.Start, NodeEnd(value), keyText);
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
