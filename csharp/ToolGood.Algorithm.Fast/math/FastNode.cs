using System;

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
	/// 语法树节点。**值类型**，存放在 <see cref="FastParser"/> 的节点数组（ArrayPool 租用）中，
	/// 节点之间用下标互相引用，因此解析一条公式的语法树不产生任何托管堆分配。
	/// <para>相比 Antlr 的 ParserRuleContext：无父链、无虚方法分派、无逐节点对象分配。</para>
	/// </summary>
	internal struct FastNode
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
		/// <summary>自定义函数名 Token 的起始下标（仅 Call + PARAMETER 使用，收集 DIY 名称时需要）</summary>
		internal int NameStart;

		private int _c0;
		private int _c1;
		private int _c2;
		private int _count;
		private int[] _more;

		/// <summary>子节点数量</summary>
		internal int ChildCount => _more != null ? _more.Length : _count;

		/// <summary>附加文本（参数名 / 自定义函数名 / json key）</summary>
		internal FastNode WithText(string text)
		{
			Text = text;
			return this;
		}

		/// <summary>获取第 index 个子节点的数组下标</summary>
		internal int GetChild(int index)
		{
			if(_more != null) { return _more[index]; }
			if(index == 0) { return _c0; }
			if(index == 1) { return _c1; }
			return _c2;
		}

		internal static FastNode Leaf(FastNodeKind kind, int start, int end, string text = null)
		{
			return new FastNode { Kind = kind, Start = start, End = end, Text = text };
		}

		internal static FastNode Unary(FastNodeKind kind, int child, int start, int end)
		{
			return new FastNode { Kind = kind, Start = start, End = end, _count = 1, _c0 = child };
		}

		internal static FastNode Binary(FastNodeKind kind, int left, int right, int start, int end)
		{
			return new FastNode { Kind = kind, Start = start, End = end, _count = 2, _c0 = left, _c1 = right };
		}

		internal static FastNode Three(FastNodeKind kind, int first, int second, int third, int start, int end)
		{
			return new FastNode { Kind = kind, Start = start, End = end, _count = 3, _c0 = first, _c1 = second, _c2 = third };
		}

		/// <summary>
		/// 从暂存区 <paramref name="children"/> 的 [<paramref name="offset"/>, offset+count) 区间创建多子节点。
		/// 子节点不超过 3 个时直接放在固定字段里，避免任何数组分配。
		/// </summary>
		internal static FastNode Nary(FastNodeKind kind, int[] children, int offset, int count, int start, int end, string text = null)
		{
			if(count <= 3) {
				var node = new FastNode { Kind = kind, Start = start, End = end, Text = text, _count = count };
				if(count > 0) { node._c0 = children[offset]; }
				if(count > 1) { node._c1 = children[offset + 1]; }
				if(count > 2) { node._c2 = children[offset + 2]; }
				return node;
			}
			var more = new int[count];
			Array.Copy(children, offset, more, 0, count);
			return new FastNode { Kind = kind, Start = start, End = end, Text = text, _more = more, _count = count };
		}
	}
}
