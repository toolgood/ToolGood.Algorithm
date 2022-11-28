using System;
using System.Collections.Generic;

namespace ToolGood.Algorithm
{
    /// <summary>
    /// 自定义类型
    /// </summary>
    public sealed class DiyNameInfo
    {
        /// <summary>
        /// 自定义 参数
        /// </summary>
        public List<String> Parameters { get; private set; }
        /// <summary>
        /// 自定义方法
        /// </summary>
        public List<String> Functions { get; private set; }

        /// <summary>
        /// 自定义类型
        /// </summary>
        public DiyNameInfo()
        {
            Parameters = new List<String>();
            Functions = new List<String>();
        }
    }
}
