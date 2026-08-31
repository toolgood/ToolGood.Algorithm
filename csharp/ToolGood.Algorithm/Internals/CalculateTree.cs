using System.Collections.Generic;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals
{
	/// <summary>
	/// 计算树
	/// </summary>
	public sealed class CalculateTree
	{
		private string _source;
		private string _text;

		/// <summary>
		/// 子节点
		/// </summary>
		public List<CalculateTree> Nodes { get; internal set; }
		/// <summary>
		/// 开始位置
		/// </summary>
		public int Start { get; internal set; }

		/// <summary>
		/// 结束位置
		/// </summary>
		public int End { get; internal set; }

		/// <summary>
		/// 类型
		/// </summary>
		public CalculateTreeType Type { get; internal set; }

		/// <summary>
		/// 文本（懒加载，返回原始表达式中 [Start, End] 区间的子串）
		/// </summary>
		public string Text {
			get {
				if(_text == null && _source != null && End >= Start) {
					_text = _source.Substring(Start, End - Start + 1);
				}
				return _text;
			}
		}
		/// <summary>
		/// 外面是否有括号
		/// </summary>
		public bool HasBracket { get; internal set; }

		/// <summary>
		/// 出错信息
		/// </summary>
		public string ErrorMessage { get; internal set; }

		internal void SetSource(string source)
		{
			_source = source;
		}

		internal CalculateTree()
		{
		}
		 
	}
}
