namespace ToolGood.Algorithm.Internals
{
	/// <summary>
	/// 关键字信息
	/// </summary>
	public struct DiyNameKeyInfo
	{
		/// <summary>
		/// 名称
		/// </summary>
		public string Name { get; internal set; }
		/// <summary>
		/// 开始位置
		/// </summary>
		public int Start { get; internal set; }
		/// <summary>
		/// 结束位置
		/// </summary>
		public int End { get; internal set; }
		///
		public override string ToString()
		{
			return Name;
		}
	}
}