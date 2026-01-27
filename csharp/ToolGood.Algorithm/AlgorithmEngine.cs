using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using ToolGood.Algorithm.Enums;
using ToolGood.Algorithm.Internals;
using ToolGood.Algorithm.Internals.Functions;
using ToolGood.Algorithm.Internals.Visitors;
using ToolGood.Algorithm.math;

namespace ToolGood.Algorithm
{
    /// <summary>
    ///
    /// </summary>
    public class AlgorithmEngine
    {
        internal int ExcelIndex = 1;

        /// <summary>
        /// 使用 本地时间， 影响 时间截转化
        /// </summary>
        public bool UseLocalTime = true;

        /// <summary>
        /// 长度单位
        /// </summary>
        public DistanceUnitType DistanceUnit = DistanceUnitType.M;

        /// <summary>
        /// 面积单位
        /// </summary>
        public AreaUnitType AreaUnit = AreaUnitType.M2;

        /// <summary>
        /// 体积单位
        /// </summary>
        public VolumeUnitType VolumeUnit = VolumeUnitType.M3;

        /// <summary>
        /// 重量单位
        /// </summary>
        public MassUnitType MassUnit = MassUnitType.KG;

        /// <summary>
        /// 最后一个错误
        /// </summary>
        public string LastError { get; internal set; }

        /// <summary>
        /// 使用EXCEL索引
        /// </summary>
        public bool UseExcelIndex { set { ExcelIndex = value ? 1 : 0; } }

        /// <summary>
        /// 自定义参数 请重写此方法
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public virtual Operand GetParameter(string parameter)
        {
            return Operand.Error($"Parameter [{parameter}] is missing.");
        }

        /// <summary>
        /// 自定义函数 请重写此方法
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public virtual Operand ExecuteDiyFunction(string parameter, List<Operand> args)
        {
            return Operand.Error($"DiyFunction [{parameter}] is missing.");
        }

        #region Parse Evaluate

        /// <summary>
        /// 编译公式，默认
        /// </summary>
        /// <param name="exp">公式</param>
        /// <returns></returns>
        public FunctionBase Parse(string exp)
        {
            LastError = null;
            if (string.IsNullOrWhiteSpace(exp)) {
                LastError = "Parameter exp invalid !";
                throw new Exception(LastError);
            }
			AntlrErrorTextWriter antlrErrorTextWriter = new AntlrErrorTextWriter();
			var stream = new AntlrCharStream(new AntlrInputStream(exp));
			var lexer = new mathLexer(stream, Console.Out, antlrErrorTextWriter);
			var tokens = new CommonTokenStream(lexer);
			var parser = new mathParser(tokens, Console.Out, antlrErrorTextWriter);

			var context = parser.prog();
            if (antlrErrorTextWriter.IsError) {
                LastError = antlrErrorTextWriter.ErrorMsg;
                throw new Exception(LastError);
            }
            var visitor = new MathFunctionVisitor();
            return visitor.Visit(context);
        }

        /// <summary>
        /// 执行函数
        /// </summary>
        /// <returns></returns>
        public Operand Evaluate(FunctionBase function)
        {
            return function.Evaluate(this);
        }

        #endregion Parse Evaluate

        #region TryEvaluate

        /// <summary>
        /// 执行函数,如果异常，返回默认值
        /// </summary>
        /// <param name="exp"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public ushort TryEvaluate(string exp, ushort def)
        {
            try {
                var function = Parse(exp);
                var obj = function.Evaluate(this);
                if (obj.IsNotNumber) {
                    obj = obj.ToNumber("It can't be converted to number!");
                    if (obj.IsError) {
                        LastError = obj.ErrorMsg;
                        return def;
                    }
                }
                return (ushort)obj.IntValue;
            } catch (Exception ex) {
                LastError = ex.Message + "\r\n" + ex.StackTrace;
            }
            return def;
        }

        /// <summary>
        /// 执行函数,如果异常，返回默认值
        /// </summary>
        /// <param name="exp"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public uint TryEvaluate(string exp, uint def)
        {
            try {
                var function = Parse(exp);
                var obj = function.Evaluate(this);
                if (obj.IsNotNumber) {
                    obj = obj.ToNumber("It can't be converted to number!");
                    if (obj.IsError) {
                        LastError = obj.ErrorMsg;
                        return def;
                    }
                }
                return (uint)obj.IntValue;
            } catch (Exception ex) {
                LastError = ex.Message + "\r\n" + ex.StackTrace;
            }
            return def;
        }

        /// <summary>
        /// 执行函数,如果异常，返回默认值
        /// </summary>
        /// <param name="exp"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public ulong TryEvaluate(string exp, ulong def)
        {
            try {
                var function = Parse(exp);
                var obj = function.Evaluate(this);
                if (obj.IsNotNumber) {
                    obj = obj.ToNumber("It can't be converted to number!");
                    if (obj.IsError) {
                        LastError = obj.ErrorMsg;
                        return def;
                    }
                }
                return (ulong)obj.IntValue;
            } catch (Exception ex) {
                LastError = ex.Message + "\r\n" + ex.StackTrace;
            }
            return def;
        }

        /// <summary>
        /// 执行函数,如果异常，返回默认值
        /// </summary>
        /// <param name="exp"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public short TryEvaluate(string exp, short def)
        {
            try {
                var function = Parse(exp);
                var obj = function.Evaluate(this);
                if (obj.IsNotNumber) {
                    obj = obj.ToNumber("It can't be converted to number!");
                    if (obj.IsError) {
                        LastError = obj.ErrorMsg;
                        return def;
                    }
                }
                return (short)obj.IntValue;
            } catch (Exception ex) {
                LastError = ex.Message + "\r\n" + ex.StackTrace;
            }
            return def;
        }

        /// <summary>
        /// 执行函数,如果异常，返回默认值
        /// </summary>
        /// <param name="exp"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public int TryEvaluate(string exp, int def)
        {
            try {
                var function = Parse(exp);
                var obj = function.Evaluate(this);
                if (obj.IsNotNumber) {
                    obj = obj.ToNumber("It can't be converted to number!");
                    if (obj.IsError) {
                        LastError = obj.ErrorMsg;
                        return def;
                    }
                }
                return obj.IntValue;
            } catch (Exception ex) {
                LastError = ex.Message + "\r\n" + ex.StackTrace;
            }
            return def;
        }

        /// <summary>
        /// 执行函数,如果异常，返回默认值
        /// </summary>
        /// <param name="exp"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public long TryEvaluate(string exp, long def)
        {
            try {
                var function = Parse(exp);
                var obj = function.Evaluate(this);
                if (obj.IsNotNumber) {
                    obj = obj.ToNumber("It can't be converted to number!");
                    if (obj.IsError) {
                        LastError = obj.ErrorMsg;
                        return def;
                    }
                }
                return obj.LongValue;
            } catch (Exception ex) {
                LastError = ex.Message + "\r\n" + ex.StackTrace;
            }
            return def;
        }

        /// <summary>
        /// 执行函数,如果异常，返回默认值
        /// </summary>
        /// <param name="exp"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public float TryEvaluate(string exp, float def)
        {
            try {
                var function = Parse(exp);
                var obj = function.Evaluate(this);
                if (obj.IsNotNumber) {
                    obj = obj.ToNumber("It can't be converted to number!");
                    if (obj.IsError) {
                        LastError = obj.ErrorMsg;
                        return def;
                    }
                }
                return (float)obj.DoubleValue;
            } catch (Exception ex) {
                LastError = ex.Message + "\r\n" + ex.StackTrace;
            }
            return def;
        }

        /// <summary>
        /// 执行函数,如果异常，返回默认值
        /// </summary>
        /// <param name="exp"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public double TryEvaluate(string exp, double def)
        {
            try {
                var function = Parse(exp);
                var obj = function.Evaluate(this);
                if (obj.IsNotNumber) {
                    obj = obj.ToNumber("It can't be converted to number!");
                    if (obj.IsError) {
                        LastError = obj.ErrorMsg;
                        return def;
                    }
                }
                return (double)obj.DoubleValue;
            } catch (Exception ex) {
                LastError = ex.Message + "\r\n" + ex.StackTrace;
            }
            return def;
        }

        /// <summary>
        /// 执行函数,如果异常，返回默认值
        /// </summary>
        /// <param name="exp"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public decimal TryEvaluate(string exp, decimal def)
        {
            try {
                var function = Parse(exp);
                var obj = function.Evaluate(this);
                if (obj.IsNotNumber) {
                    obj = obj.ToNumber("It can't be converted to number!");
                    if (obj.IsError) {
                        LastError = obj.ErrorMsg;
                        return def;
                    }
                }
                return (decimal)obj.NumberValue;
            } catch (Exception ex) {
                LastError = ex.Message + "\r\n" + ex.StackTrace;
            }
            return def;
        }

        /// <summary>
        /// 执行函数,如果异常，返回默认值
        /// </summary>
        /// <param name="exp"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public string TryEvaluate(string exp, string def)
        {
            try {
                var function = Parse(exp);
                var obj = function.Evaluate(this);
                if (obj.IsNotText) {
                    obj = obj.ToText("It can't be converted to string!");
                    if (obj.IsError) {
                        LastError = obj.ErrorMsg;
                        return def;
                    }
                }
                return obj.TextValue;
            } catch (Exception ex) {
                LastError = ex.Message + "\r\n" + ex.StackTrace;
            }
            return def;
        }

        /// <summary>
        /// 执行函数,如果异常，返回默认值
        /// </summary>
        /// <param name="exp"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public bool TryEvaluate(string exp, bool def)
        {
            try {
                var function = Parse(exp);
                var obj = function.Evaluate(this);
                if (obj.IsNotBoolean) {
                    obj = obj.ToBoolean("It can't be converted to bool!");
                    if (obj.IsError) {
                        LastError = obj.ErrorMsg;
                        return def;
                    }
                }
                return obj.BooleanValue;
            } catch (Exception ex) {
                LastError = ex.Message + "\r\n" + ex.StackTrace;
            }
            return def;
        }

        /// <summary>
        /// 执行函数,如果异常，返回默认值
        /// </summary>
        /// <param name="exp"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public DateTime TryEvaluate(string exp, DateTime def)
        {
            try {
                var function = Parse(exp);
                var obj = function.Evaluate(this);
                if (obj.IsNotDate) {
                    obj = obj.ToMyDate("It can't be converted to DateTime!");
                    if (obj.IsError) {
                        LastError = obj.ErrorMsg;
                        return def;
                    }
                }
                if (UseLocalTime) {
                    return obj.DateValue.ToDateTime(DateTimeKind.Local);
                }
                return obj.DateValue.ToDateTime(DateTimeKind.Utc);
            } catch (Exception ex) {
                LastError = ex.Message + "\r\n" + ex.StackTrace;
            }
            return def;
        }

        /// <summary>
        /// 执行函数,如果异常，返回默认值
        /// </summary>
        /// <param name="exp"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public TimeSpan TryEvaluate(string exp, TimeSpan def)
        {
            try {
                var function = Parse(exp);
                var obj = function.Evaluate(this);
                if (obj.IsNotDate) {
                    obj = obj.ToMyDate("It can't be converted to DateTime!");
                    if (obj.IsError) {
                        LastError = obj.ErrorMsg;
                        return def;
                    }
                }
                return (TimeSpan)obj.DateValue;
            } catch (Exception ex) {
                LastError = ex.Message + "\r\n" + ex.StackTrace;
            }
            return def;
        }

        /// <summary>
        /// 执行函数,如果异常，返回默认值。
        /// 解决 def 为 null 二义性问题
        /// </summary>
        /// <param name="exp"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public MyDate TryEvaluate_MyDate(string exp, MyDate def)
        {
            try {
                var function = Parse(exp);
                var obj = function.Evaluate(this);
                if (obj.IsNotDate) {
                    obj = obj.ToMyDate("It can't be converted to DateTime!");
                    if (obj.IsError) {
                        LastError = obj.ErrorMsg;
                        return def;
                    }
                }
                return obj.DateValue;
            } catch (Exception ex) {
                LastError = ex.Message + "\r\n" + ex.StackTrace;
            }
            return def;
        }

        #endregion TryEvaluate
    }
}