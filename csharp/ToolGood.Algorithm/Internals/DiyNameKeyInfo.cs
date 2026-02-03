namespace ToolGood.Algorithm.Internals
{
	/// <summary>
	/// 关键字信息
	/// </summary>
	public class DiyNameKeyInfo
	{
		/// <summary>
		/// 名称
		/// </summary>
		public string Name { get; set; }
		/// <summary>
		/// 开始位置
		/// </summary>
		public int Start { get; set; }
		/// <summary>
		/// 结束位置
		/// </summary>
		public int End { get; set; }
		///
		public override string ToString()
		{
			return Name;
		}
	}
}