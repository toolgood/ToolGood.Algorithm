using System.Collections.Generic;

namespace ToolGood.Algorithm.Internals
{
    /// <summary>
    /// 自定义类型
    /// </summary>
    public struct DiyNameInfo
    {
        /// <summary>
        /// 自定义 参数
        /// </summary>
        public List<DiyNameKeyInfo> Parameters { get; private set; }

        /// <summary>
        /// 自定义方法
        /// </summary>
        public List<DiyNameKeyInfo> Functions { get; private set; }

		/// <summary>
		/// 自定义类型
		/// </summary>
		public DiyNameInfo(List<DiyNameKeyInfo> p, List<DiyNameKeyInfo>f)
        {
            Parameters = p;
            Functions = f;
        }
	}
}