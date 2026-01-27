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
        public List<string> Parameters { get; private set; }

        /// <summary>
        /// 自定义方法
        /// </summary>
        public List<string> Functions { get; private set; }

        /// <summary>
        /// 自定义类型
        /// </summary>
        internal DiyNameInfo()
        {
            Parameters = new List<string>();
            Functions = new List<string>();
        }
    }
}