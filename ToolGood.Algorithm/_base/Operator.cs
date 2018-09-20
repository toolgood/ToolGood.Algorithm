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

        /// <summary>
        /// 转换运算符到指定的类型
        /// </summary>
        /// <param name="opt">运算符</param>
        /// <returns>返回指定的运算符类型</returns>
        public static OperatorType ConvertOperator(string opt, string next)
        {
            switch (opt) {
                //case "!": return OperatorType.NOT;
                case ".": return OperatorType.POINT;
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
                default: break; // return OperatorType.FUNC;
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
            //if (opt.StartsWith(".")) {
            //    return OperatorType.POINT;
            //}
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
            }
            object token = tokens.Peek();
            if (token is Operand) {
                return operators.Peek().Type != OperatorType.LB;
            } else {
                return ((Operator)token).Type == OperatorType.RB;
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
            //if (optA == OperatorType.POINT ) {
            //    return 1;
            //}
            //if (optB == OperatorType.POINT) {
            //    return -1;
            //}

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


            if (optA < optB) {
                //A优先级高于B
                return 1;
            }

            //A优先级低于B
            return -1;

        }
    }
}
