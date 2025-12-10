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
        public List<ParameterInfo> Parameters { get; private set; }

        /// <summary>
        /// 自定义方法
        /// </summary>
        public List<string> Functions { get; private set; }

        /// <summary>
        /// 自定义类型
        /// </summary>
        public DiyNameInfo()
        {
            Parameters = new List<ParameterInfo>();
            Functions = new List<string>();
        }
    }

    /// <summary>
    /// 参数信息
    /// </summary>
    public sealed class ParameterInfo
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="name"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public ParameterInfo(string name, int start, int end)
        {
            Name = name;
            Start = start;
            End = end;
        }

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

        /// <summary>
        /// 重写
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Name;
        }
    }
}