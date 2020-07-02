using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToolGood.Algorithm
{
    /// <summary>
    /// 操作数类型
    /// </summary>
    public enum OperandType
    {
        /// <summary>
        /// 函数
        /// </summary>
        ERROR,

        /// <summary>
        /// 日期
        /// </summary>
        DATE,

        /// <summary>
        /// 数组
        /// </summary>
        ARRARY,

        /// <summary>
        /// 数字
        /// </summary>
        NUMBER,

        /// <summary>
        /// 布尔
        /// </summary>
        BOOLEAN,

        /// <summary>
        /// 字符串
        /// </summary>
        STRING,

        ///// <summary>
        ///// 参数
        ///// </summary>
        //PARAMETER,

        ///// <summary>
        ///// 对象
        ///// </summary>
        //OBJECT,

        /// <summary>
        /// JSON格式
        /// </summary>
        JSON


    }
}
