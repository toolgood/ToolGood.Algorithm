using System.Collections.Generic;

namespace ToolGood.Algorithm.Internals
{
    /// <summary>
    /// 自定义类型
    /// </summary>
    public sealed class DiyNameInfo
    {
        /// <summary>
        /// 自定义 参数
        /// </summary>
        public List<KeyInfo> Parameters { get; private set; }

        /// <summary>
        /// 自定义方法
        /// </summary>
        public List<KeyInfo> Functions { get; private set; }

        /// <summary>
        /// 自定义类型
        /// </summary>
        internal DiyNameInfo()
        {
            Parameters = new List<KeyInfo>();
            Functions = new List<KeyInfo>();
        }

		/// <summary>
		/// 关键字信息
		/// </summary>
		public class KeyInfo
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
}