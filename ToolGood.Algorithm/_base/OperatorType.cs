using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToolGood.Algorithm
{
    /// <summary>
    /// 运算符类型(从上到下优先级依次递减)，数值越大，优先级越低
    /// </summary>
    internal enum OperatorType
    {
        /// <summary>
        /// 左括号:(,left bracket
        /// </summary>
        LB,

        /// <summary>
        /// 右括号),right bracket
        /// </summary>
        RB,

        /// <summary>
        /// 创建数组
        /// </summary>
        CREATEARRARY,

        /// <summary>
        /// 函数
        /// </summary>
        FUNC,

        /// <summary>
        /// 参数
        /// </summary>
        PARAMETER,
        ///// <summary>
        ///// 逻辑非,!,NOT
        ///// </summary>
        //NOT,

        ///// <summary>
        ///// 正号,+,positive sign
        ///// </summary>
        //PS,

        ///// <summary>
        ///// 负号,-,negative sign
        ///// </summary>
        //NS,



        #region 乘 除 余
        /// <summary>
        /// 乘,*,multiplication
        /// </summary>
        MUL,

        /// <summary>
        /// 除,/,division
        /// </summary>
        DIV,
        /// <summary>
        /// 余,%,modulus
        /// </summary>
        MOD,
        #endregion

        #region 加 减
        /// <summary>
        /// 加,+,Addition
        /// </summary>
        ADD,
        /// <summary>
        /// 加,+,Addition
        /// </summary>
        StringADD,
        /// <summary>
        /// 减,-,subtraction
        /// </summary>
        SUB,


        #endregion



        #region 小于 小于或等于 大于 大于或等于 等于 不等于
        /// <summary>
        /// 小于,less than
        /// </summary>
        LT,

        /// <summary>
        /// 小于或等于,less than or equal to
        /// </summary>
        LE,

        /// <summary>
        /// 大于,>,greater than
        /// </summary>
        GT,

        /// <summary>
        /// 大于或等于,>=,greater than or equal to
        /// </summary>
        GE,

        /// <summary>
        /// 等于,=,equal to
        /// </summary>
        ET,

        /// <summary>
        /// 不等于,unequal to
        /// </summary>
        UT,
        #endregion

        /// <summary>
        /// 逻辑与,&,AND
        /// </summary>
        AND,

        /// <summary>
        /// 逻辑或,|,OR
        /// </summary>
        OR,

        /// <summary>
        /// 逗号,comma
        /// </summary>
        CA,

        /// <summary>
        /// 结束符号 @
        /// </summary>
        END,

        /// <summary>
        /// 错误符号
        /// </summary>
        ERR

    }
}
