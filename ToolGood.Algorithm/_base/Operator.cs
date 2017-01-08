using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToolGood.Algorithm
{
    /// <summary>
    /// 运算符
    /// </summary>
    internal class Operator
    {
        public Operator(OperatorType type, string value)
        {
            this.Type = type;
            this.Value = value;
        }

        public Operator(OperatorType type, string value, int argsCount)
        {
            this.Type = type;
            this.Value = value;
            ArgsCount = argsCount;
        }

        public int ArgsCount { get; set; }

        /// <summary>
        /// 运算符类型
        /// </summary>
        public OperatorType Type { get; set; }

        /// <summary>
        /// 运算符值
        /// </summary>
        public string Value { get; set; }


        ///// <summary>
        ///// 对于>或者&lt;运算符，判断实际是否为>=,&lt;&gt;、&lt;=，并调整当前运算符位置
        ///// </summary>
        ///// <param name="currentOpt">当前运算符</param>
        ///// <param name="currentExp">当前表达式</param>
        ///// <param name="currentOptPos">当前运算符位置</param>
        ///// <param name="adjustOptPos">调整后运算符位置</param>
        ///// <returns>返回调整后的运算符</returns>
        //public static string AdjustOperator(string currentOpt, string currentExp, ref int currentOptPos)
        //{
        //    switch (currentOpt) {
        //        case "<":
        //            if (currentExp.Substring(currentOptPos, 2) == "<=") {
        //                currentOptPos++;
        //                return "<=";
        //            }
        //            if (currentExp.Substring(currentOptPos, 2) == "<>") {
        //                currentOptPos++;
        //                return "<>";
        //            }
        //            return "<";

        //        case ">":
        //            if (currentExp.Substring(currentOptPos, 2) == ">=") {
        //                currentOptPos++;
        //                return ">=";
        //            }
        //            return ">";
        //        case "t":
        //            if (currentExp.Substring(currentOptPos, 3) == "tan") {
        //                currentOptPos += 2;
        //                return "tan";
        //            }
        //            return "error";
        //        case "a":
        //            if (currentExp.Substring(currentOptPos, 4) == "atan") {
        //                currentOptPos += 3;
        //                return "atan";
        //            }
        //            return "error";
        //        default:
        //            return currentOpt;
        //    }
        //}

        ///// <summary>
        ///// 转换运算符到指定的类型
        ///// </summary>
        ///// <param name="opt">运算符</param>
        ///// <param name="isBinaryOperator">是否为二元运算符</param>
        ///// <returns>返回指定的运算符类型</returns>
        //public static OperatorType ConvertOperator(string opt, bool isBinaryOperator)
        //{
        //    switch (opt) {
        //        case "!": return OperatorType.NOT;
        //        case "+": return isBinaryOperator ? OperatorType.ADD : OperatorType.PS;
        //        case "-": return isBinaryOperator ? OperatorType.SUB : OperatorType.NS;
        //        case "*": return isBinaryOperator ? OperatorType.MUL : OperatorType.ERR;
        //        case "/": return isBinaryOperator ? OperatorType.DIV : OperatorType.ERR;
        //        case "%": return isBinaryOperator ? OperatorType.MOD : OperatorType.ERR;
        //        case "<": return isBinaryOperator ? OperatorType.LT : OperatorType.ERR;
        //        case ">": return isBinaryOperator ? OperatorType.GT : OperatorType.ERR;
        //        case "<=": return isBinaryOperator ? OperatorType.LE : OperatorType.ERR;
        //        case ">=": return isBinaryOperator ? OperatorType.GE : OperatorType.ERR;
        //        case "<>": return isBinaryOperator ? OperatorType.UT : OperatorType.ERR;
        //        case "=": return isBinaryOperator ? OperatorType.ET : OperatorType.ERR;
        //        case "&": return isBinaryOperator ? OperatorType.StringADD : OperatorType.ERR;
        //        case "|": return isBinaryOperator ? OperatorType.OR : OperatorType.ERR;
        //        case ",": return isBinaryOperator ? OperatorType.CA : OperatorType.ERR;
        //        case "@": return isBinaryOperator ? OperatorType.END : OperatorType.ERR;
        //        default: return OperatorType.ERR;
        //    }
        //}

        /// <summary>
        /// 转换运算符到指定的类型
        /// </summary>
        /// <param name="opt">运算符</param>
        /// <returns>返回指定的运算符类型</returns>
        public static OperatorType ConvertOperator(string opt, string next)
        {
            switch (opt) {
                //case "!": return OperatorType.NOT;
                case "+": return OperatorType.ADD;
                case "-": return OperatorType.SUB;
                case "*": return OperatorType.MUL;
                case "/": return OperatorType.DIV;
                case "%": return OperatorType.MOD;
                case "<": return OperatorType.LT;
                case ">": return OperatorType.GT;
                case "<=": return OperatorType.LE;
                case ">=": return OperatorType.GE;
                case "<>": return OperatorType.UT;
                case "!=": return OperatorType.UT;
                case "=": return OperatorType.ET;
                case "==": return OperatorType.ET;
                case "&": return OperatorType.StringADD;
                case "|": return OperatorType.OR;
                case ",": return OperatorType.CA;
                default:break; // return OperatorType.FUNC;
            }
            switch (opt) {
                case "pi":
                case "e":
                case "true":
                case "false":
                    if (next == "(") {
                        return OperatorType.FUNC;
                    }
                    return OperatorType.PARAMETER;
            }
            return OperatorType.FUNC;
        }

        /// <summary>
        /// 运算符是否为二元运算符,该方法有问题，暂不使用
        /// </summary>
        /// <param name="tokens">语法单元堆栈</param>
        /// <param name="operators">运算符堆栈</param>
        /// <param name="currentOpd">当前操作数</param>
        /// <returns>是返回真,否返回假</returns>
        public static bool IsBinaryOperator(ref Stack<object> tokens, ref Stack<Operator> operators, string currentOpd)
        {
            if (currentOpd != "") {
                return true;
            } else {
                object token = tokens.Peek();
                if (token is Operand) {
                    if (operators.Peek().Type != OperatorType.LB) {
                        return true;
                    } else {
                        return false;
                    }
                } else {
                    if (((Operator)token).Type == OperatorType.RB) {
                        return true;
                    } else {
                        return false;
                    }
                }
            }
        }

        /// <summary>
        /// 运算符优先级比较
        /// </summary>
        /// <param name="optA">运算符类型A</param>
        /// <param name="optB">运算符类型B</param>
        /// <returns>A与B相比，-1，低；0,相等；1，高</returns>
        public static int ComparePriority(OperatorType optA, OperatorType optB)
        {
            if (optA == optB) {
                //A、B优先级相等
                return 0;
            }

            //乘,除,余(*,/,%)
            if ((optA >= OperatorType.MUL && optA <= OperatorType.MOD) &&
                (optB >= OperatorType.MUL && optB <= OperatorType.MOD)) {
                return 0;
            }
            //加,减(+,-)
            if ((optA >= OperatorType.ADD && optA <= OperatorType.SUB) &&
                (optB >= OperatorType.ADD && optB <= OperatorType.SUB)) {
                return 0;
            }
            //小于,小于或等于,大于,大于或等于(<,<=,>,>=)
            if ((optA >= OperatorType.LT && optA <= OperatorType.GE) &&
                (optB >= OperatorType.LT && optB <= OperatorType.GE)) {
                return 0;
            }
            //等于,不等于(=,<>)
            if ((optA >= OperatorType.ET && optA <= OperatorType.UT) &&
                (optB >= OperatorType.ET && optB <= OperatorType.UT)) {
                return 0;
            }


            ////三角函数
            //if ((optA >= OperatorType.TAN && optA <= OperatorType.ATAN) &&
            //        (optB >= OperatorType.TAN && optB <= OperatorType.ATAN)) {
            //    return 0;
            //}

            if (optA < optB) {
                //A优先级高于B
                return 1;
            }

            //A优先级低于B
            return -1;

        }
    }
}
