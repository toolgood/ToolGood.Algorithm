using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static mathParser;

namespace ToolGood.Algorithm
{
    /// <summary>
    /// 算法引擎 扩展版
    /// 支持 一层算法 内套 一层算法
    /// 支持多条件过滤
    /// </summary>
    public class AlgorithmEngineEx
    {
        /// <summary>
        /// 使用EXCEL索引
        /// </summary>
        public bool UseExcelIndex { get; set; } = true;
        /// <summary>
        /// 最后一个错误
        /// </summary>
        public string LastError { get; private set; }
        private Dictionary<string, Operand> _dict = new Dictionary<string, Operand>();
        /// <summary>
        /// 自定义 函数
        /// </summary>
        public event Func<string, List<Operand>, Operand> DiyFunction;
        /// <summary>
        /// 多条件缓存
        /// </summary>
        public ConditionCache MultiConditionCache { get; private set; }

        public AlgorithmEngineEx(ConditionCache multiConditionCache)
        {
            MultiConditionCache = multiConditionCache;
        }

        #region GetParameter
        /// <summary>
        /// 自定义参数
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        protected virtual Operand GetParameter(string parameter)
        {
            Operand operand;
            if (_dict.TryGetValue(parameter, out operand)) {
                return operand;
            }
            var conditionCaches = MultiConditionCache.GetConditionCaches(parameter);
            foreach (var conditionCache in conditionCaches) {
                if (conditionCache.FormulaProg == null) continue;
                if (conditionCache.ConditionProg != null) {
                    var b = Evaluate(conditionCache.ConditionProg);
                    if (b.IsError) {
                        return Operand.Error($"Parameter [{parameter}] condition `{conditionCache.ConditionString}` is error.\r\n{b.ErrorMsg}");
                    }
                    if (b.BooleanValue == false) continue;
                }
                operand = Evaluate(conditionCache.FormulaProg);
                if (operand.IsError) {
                    operand = Operand.Error($"Parameter [{parameter}] formula `{conditionCache.FormulaString}` is error.\r\n{operand.ErrorMsg}");
                }
                _dict[parameter] = operand;
                return operand;
            }
            return Operand.Error($"Parameter [{parameter}] is missing.");
        }
        #endregion

        #region ExecuteDiyFunction
        /// <summary>
        /// 自定义 函数
        /// </summary>
        /// <param name="funcName"></param>
        /// <param name="operands"></param>
        /// <returns></returns>
        protected virtual Operand ExecuteDiyFunction(string funcName, List<Operand> operands)
        {
            if (DiyFunction != null) {
                return DiyFunction.Invoke(funcName, operands);
            }
            return Operand.Error($"DiyFunction [{funcName}] is missing.");
        }

        /// <summary>
        /// 清理 自定义函数
        /// </summary>
        public void ClearDiyFunctions()
        {
            DiyFunction = null;
        }

        #endregion

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="categoryName"></param>
        /// <returns></returns>
        public Operand Evaluate(string categoryName)
        {
            Operand operand;
            var conditionCaches = MultiConditionCache.GetConditionCaches(categoryName);
            foreach (var conditionCache in conditionCaches) {
                if (string.IsNullOrEmpty(conditionCache.FormulaString)) continue;
                if (string.IsNullOrEmpty(conditionCache.ConditionString) == false) {
                    if (conditionCache.ConditionProg == null) {
                        return Operand.Error($"CategoryName [{categoryName}] parse condition `{conditionCache.ConditionString}` is error.\r\n{conditionCache.LastError}");
                    }
                    var b = Evaluate(conditionCache.ConditionProg);
                    if (b.IsError) {
                        return Operand.Error($"CategoryName [{categoryName}] condition `{conditionCache.ConditionString}` is error.\r\n{b.ErrorMsg}");
                    }
                    if (b.BooleanValue == false) continue;
                }
                if (conditionCache.FormulaProg == null) {
                    return Operand.Error($"CategoryName [{categoryName}] parse formula `{ conditionCache.FormulaString}` is error.\r\n{conditionCache.LastError}");
                }
                operand = Evaluate(conditionCache.FormulaProg);
                if (operand.IsError) {
                    operand = Operand.Error($"CategoryName [{categoryName}] formula `{conditionCache.FormulaString}` is error.\r\n{operand.ErrorMsg}");
                }
                return operand;
            }
            return Operand.Error($"CategoryName [{categoryName}] is missing.");

        }

        /// <summary>
        /// 查找备注
        /// </summary>
        /// <param name="categoryName"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public string SearchRemark(string categoryName)
        {
            var conditionCaches = MultiConditionCache.GetConditionCaches(categoryName);
            foreach (var conditionCache in conditionCaches) {
                if (string.IsNullOrEmpty(conditionCache.ConditionString) == false) {
                    if (conditionCache.ConditionProg == null) {
                        throw new Exception($"CategoryName [{categoryName}] parse formula `{ conditionCache.FormulaString}` is error.\r\n{conditionCache.LastError}");
                    }
                    var b = Evaluate(conditionCache.ConditionProg);
                    if (b.IsError) {
                        throw new Exception($"CategoryName [{categoryName}] formula `{ conditionCache.FormulaString}` is error.\r\n{b.ErrorMsg}");
                    }
                    if (b.BooleanValue == false) continue;
                }
                return conditionCache.Remark;
            }
            throw new Exception($"CategoryName [{categoryName}] is missing.");
        }


        #region TryEvaluate
        /// <summary>
        /// 执行函数,如果异常，返回默认值
        /// </summary>
        /// <param name="categoryName"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public ushort TryEvaluate(string categoryName, ushort def)
        {
            try {
                var obj = Evaluate(categoryName);
                obj = obj.ToNumber("It can't be converted to number!");
                if (obj.IsError) {
                    LastError = obj.ErrorMsg;
                    return def;
                }
                return (ushort)obj.IntValue;
            } catch (Exception ex) {
                LastError = ex.Message;
            }
            return def;
        }
        /// <summary>
        /// 执行函数,如果异常，返回默认值
        /// </summary>
        /// <param name="categoryName"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public uint TryEvaluate(string categoryName, uint def)
        {
            try {
                var obj = Evaluate(categoryName);
                obj = obj.ToNumber("It can't be converted to number!");
                if (obj.IsError) {
                    LastError = obj.ErrorMsg;
                    return def;
                }
                return (uint)obj.IntValue;
            } catch (Exception ex) {
                LastError = ex.Message;
            }
            return def;
        }
        /// <summary>
        /// 执行函数,如果异常，返回默认值
        /// </summary>
        /// <param name="categoryName"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public ulong TryEvaluate(string categoryName, ulong def)
        {
            try {
                var obj = Evaluate(categoryName);
                obj = obj.ToNumber("It can't be converted to number!");
                if (obj.IsError) {
                    LastError = obj.ErrorMsg;
                    return def;
                }
                return (ulong)obj.IntValue;
            } catch (Exception ex) {
                LastError = ex.Message;
            }
            return def;
        }
        /// <summary>
        /// 执行函数,如果异常，返回默认值
        /// </summary>
        /// <param name="categoryName"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public short TryEvaluate(string categoryName, short def)
        {
            try {
                var obj = Evaluate(categoryName);
                obj = obj.ToNumber("It can't be converted to number!");
                if (obj.IsError) {
                    LastError = obj.ErrorMsg;
                    return def;
                }
                return (short)obj.IntValue;
            } catch (Exception ex) {
                LastError = ex.Message;
            }
            return def;
        }
        /// <summary>
        /// 执行函数,如果异常，返回默认值
        /// </summary>
        /// <param name="categoryName"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public int TryEvaluate(string categoryName, int def)
        {
            try {
                var obj = Evaluate(categoryName);
                obj = obj.ToNumber("It can't be converted to number!");
                if (obj.IsError) {
                    LastError = obj.ErrorMsg;
                    return def;
                }
                return obj.IntValue;
            } catch (Exception ex) {
                LastError = ex.Message;
            }
            return def;
        }
        /// <summary>
        /// 执行函数,如果异常，返回默认值
        /// </summary>
        /// <param name="categoryName"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public long TryEvaluate(string categoryName, long def)
        {
            try {
                var obj = Evaluate(categoryName);
                obj = obj.ToNumber("It can't be converted to number!");
                if (obj.IsError) {
                    LastError = obj.ErrorMsg;
                    return def;
                }
                return obj.IntValue;
            } catch (Exception ex) {
                LastError = ex.Message;
            }
            return def;
        }
        /// <summary>
        /// 执行函数,如果异常，返回默认值
        /// </summary>
        /// <param name="categoryName"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public float TryEvaluate(string categoryName, float def)
        {
            try {
                var obj = Evaluate(categoryName);
                obj = obj.ToNumber("It can't be converted to number!");
                if (obj.IsError) {
                    LastError = obj.ErrorMsg;
                    return def;
                }
                return (float)obj.NumberValue;
            } catch (Exception ex) {
                LastError = ex.Message;
            }
            return def;
        }
        /// <summary>
        /// 执行函数,如果异常，返回默认值
        /// </summary>
        /// <param name="categoryName"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public double TryEvaluate(string categoryName, double def)
        {
            try {
                var obj = Evaluate(categoryName);
                obj = obj.ToNumber("It can't be converted to number!");
                if (obj.IsError) {
                    LastError = obj.ErrorMsg;
                    return def;
                }
                return obj.NumberValue;
            } catch (Exception ex) {
                LastError = ex.Message;
            }
            return def;
        }
        /// <summary>
        /// 执行函数,如果异常，返回默认值
        /// </summary>
        /// <param name="categoryName"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public decimal TryEvaluate(string categoryName, decimal def)
        {
            try {
                var obj = Evaluate(categoryName);
                obj = obj.ToNumber("It can't be converted to number!");
                if (obj.IsError) {
                    LastError = obj.ErrorMsg;
                    return def;
                }
                return (decimal)obj.NumberValue;
            } catch (Exception ex) {
                LastError = ex.Message;
            }
            return def;
        }
        /// <summary>
        /// 执行函数,如果异常，返回默认值
        /// </summary>
        /// <param name="categoryName"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public string TryEvaluate(string categoryName, string def)
        {
            try {
                var obj = Evaluate(categoryName);
                if (obj.IsNull) {
                    return null;
                }
                obj = obj.ToText("It can't be converted to string!");
                if (obj.IsError) {
                    LastError = obj.ErrorMsg;
                    return def;
                }
                return obj.TextValue;
            } catch (Exception ex) {
                LastError = ex.Message;
            }
            return def;
        }
        /// <summary>
        /// 执行函数,如果异常，返回默认值
        /// </summary>
        /// <param name="categoryName"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public bool TryEvaluate(string categoryName, bool def)
        {
            try {
                var obj = Evaluate(categoryName);
                obj = obj.ToBoolean("It can't be converted to bool!");
                if (obj.IsError) {
                    LastError = obj.ErrorMsg;
                    return def;
                }
                return obj.BooleanValue;
            } catch (Exception ex) {
                LastError = ex.Message;
            }
            return def;
        }
        /// <summary>
        /// 执行函数,如果异常，返回默认值
        /// </summary>
        /// <param name="categoryName"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public DateTime TryEvaluate(string categoryName, DateTime def)
        {
            try {
                var obj = Evaluate(categoryName);
                obj = obj.ToMyDate("It can't be converted to MyDate!");
                if (obj.IsError) {
                    LastError = obj.ErrorMsg;
                    return def;
                }
                return (DateTime)obj.DateValue;
            } catch (Exception ex) {
                LastError = ex.Message;
            }
            return def;
        }
        /// <summary>
        /// 执行函数,如果异常，返回默认值
        /// </summary>
        /// <param name="categoryName"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public TimeSpan TryEvaluate(string categoryName, TimeSpan def)
        {
            try {
                var obj = Evaluate(categoryName);
                obj = obj.ToMyDate("It can't be converted to MyDate!");
                if (obj.IsError) {
                    LastError = obj.ErrorMsg;
                    return def;
                }
                return (TimeSpan)obj.DateValue;
            } catch (Exception ex) {
                LastError = ex.Message;
            }
            return def;
        }
        /// <summary>
        /// 执行函数,如果异常，返回默认值。
        /// 解决 def 为 null 二义性问题
        /// </summary>
        /// <param name="categoryName"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public MyDate TryEvaluate_MyDate(string categoryName, MyDate def)
        {
            try {
                var obj = Evaluate(categoryName);
                obj = obj.ToMyDate("It can't be converted to MyDate!");
                if (obj.IsError) {
                    LastError = obj.ErrorMsg;
                    return def;
                }
                return obj.DateValue;
            } catch (Exception ex) {
                LastError = ex.Message;
            }
            return def;
        }
        #endregion

        private Operand Evaluate(ProgContext context)
        {
            try {
                var visitor = new MathVisitor();
                visitor.GetParameter += GetParameter;
                visitor.excelIndex = UseExcelIndex ? 1 : 0;
                visitor.DiyFunction += ExecuteDiyFunction;
                return visitor.Visit(context);
            } catch (Exception ex) {
                LastError = ex.Message;
                return Operand.Error(ex.Message);
            }

        }

    }
}
