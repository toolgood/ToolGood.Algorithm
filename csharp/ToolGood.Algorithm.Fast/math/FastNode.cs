namespace ToolGood.Algorithm.math
{
	/// <summary>
	/// 手写语法分析生成的语法树节点种类。
	/// </summary>
	internal enum FastNodeKind : byte
	{
		/// <summary>数值字面量（可能带负号或单位后缀）</summary>
		Number,
		/// <summary>字符串字面量（含首尾引号）</summary>
		Text,
		/// <summary>参数名</summary>
		Parameter,
		/// <summary>括号包裹</summary>
		Bracket,
		/// <summary>逻辑非</summary>
		Not,
		/// <summary>百分号后缀</summary>
		Percentage,

		Mul,
		Div,
		Mod,
		Add,
		Sub,
		Concat,
		GreaterThan,
		GreaterOrEqual,
		LessThan,
		LessOrEqual,
		Equal,
		NotEqual,
		And,
		Or,
		/// <summary>三目运算</summary>
		If,

		/// <summary>函数调用，Type 为函数名 Token</summary>
		Call,
		/// <summary>[a, b, c]</summary>
		Array,
		/// <summary>{k:v, ...}</summary>
		ArrayJson,
		/// <summary>json 中的一项</summary>
		ArrayJsonItem,
		/// <summary>expr[index]</summary>
		GetJson,
		/// <summary>expr.name</summary>
		GetJsonProperty,
	}

	/// <summary>
	/// 轻量语法树节点。相比 Antlr 的 ParserRuleContext 没有父链、虚拟 Accept 分派和 RuleContext 分配。
	/// </summary>
	internal sealed class FastNode
	{
		/// <summary>节点种类</summary>
		internal FastNodeKind Kind;
		/// <summary>函数名 Token 类型（仅 Call 使用）</summary>
		internal int Type;
		/// <summary>原文起始下标</summary>
		internal int Start;
		/// <summary>原文结束下标（闭区间）</summary>
		internal int End;
		/// <summary>参数名 / 自定义函数名 / json key</summary>
		internal string Text;
		/// <summary>名称 Token 的起始下标（自定义函数收集用）</summary>
		internal int NameStart;
		/// <summary>名称 Token 的结束下标（闭区间）</summary>
		internal int NameEnd;
		/// <summary>子节点</summary>
		internal FastNode[] Children;

		internal static readonly FastNode[] Empty = new FastNode[0];

		internal FastNode(FastNodeKind kind, int start, int end)
		{
			Kind = kind;
			Start = start;
			End = end;
			Children = Empty;
		}

		internal static FastNode Leaf(FastNodeKind kind, int start, int end, string text = null)
		{
			return new FastNode(kind, start, end) { Text = text };
		}

		internal static FastNode Unary(FastNodeKind kind, FastNode child, int start)
		{
			return new FastNode(kind, start, child.End) { Children = new[] { child } };
		}

		internal static FastNode Binary(FastNodeKind kind, FastNode left, FastNode right)
		{
			return new FastNode(kind, left.Start, right.End) { Children = new[] { left, right } };
		}

		internal static FastNode Call(int type, string name, int nameStart, int nameEnd, FastNode[] args, int start, int end)
		{
			return new FastNode(FastNodeKind.Call, start, end) {
				Type = type,
				Text = name,
				NameStart = nameStart,
				NameEnd = nameEnd,
				Children = args ?? Empty,
			};
		}

		internal static FastNode Nary(FastNodeKind kind, FastNode[] children, int start, int end)
		{
			return new FastNode(kind, start, end) { Children = children ?? Empty };
		}

		internal static FastNode WithText(FastNodeKind kind, FastNode[] children, int start, int end, string text)
		{
			return new FastNode(kind, start, end) { Children = children ?? Empty, Text = text };
		}
	}
}
