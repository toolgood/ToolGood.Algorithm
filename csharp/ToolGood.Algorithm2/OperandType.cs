namespace ToolGood.Algorithm
{
    /// <summary>
    /// 操作数类型
    /// </summary>
    public enum OperandType : byte
    {
        /// <summary>
        /// NULL
        /// </summary>
        NULL,
        /// <summary>
        /// 错误
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

        /// <summary>
        /// JSON格式
        /// </summary>
        JSON


    }
}
