using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToolGood.Algorithm.Internals;
using ToolGood.Algorithm.LitJson;
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
        private ConditionCache MultiConditionCache;
        /// <summary>
        /// 跳过条件错误 
        /// </summary>
        public bool JumpConditionError { get; set; }
        /// <summary>
        /// 跳过公式错误
        /// </summary>
        public bool JumpFormulaError { get; set; }


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
                        LastError = $"Parameter [{parameter}],{conditionCache.Remark} condition `{conditionCache.ConditionString}` is error.\r\n{b.ErrorMsg}";
                        if (JumpConditionError) continue;
                        return Operand.Error(LastError);
                    }
                    if (b.BooleanValue == false) continue;
                }
                operand = Evaluate(conditionCache.FormulaProg);
                if (operand.IsError) {
                    LastError = $"Parameter [{parameter}],{conditionCache.Remark} formula `{conditionCache.FormulaString}` is error.\r\n{operand.ErrorMsg}";
                    if (JumpFormulaError) continue;
                    return Operand.Error(LastError);
                }
                _dict[parameter] = operand;
                return operand;
            }
            if (LastError != null) return Operand.Error(LastError);
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
            LastError = null;
            Operand operand;
            var conditionCaches = MultiConditionCache.GetConditionCaches(categoryName);
            foreach (var conditionCache in conditionCaches) {
                if (string.IsNullOrEmpty(conditionCache.FormulaString)) continue;
                if (string.IsNullOrEmpty(conditionCache.ConditionString) == false) {
                    if (conditionCache.ConditionProg == null) {
                        return Operand.Error($"CategoryName [{categoryName}],{conditionCache.Remark} parse condition `{conditionCache.ConditionString}` is error.\r\n{conditionCache.LastError}");
                    }
                    var b = Evaluate(conditionCache.ConditionProg);
                    if (b.IsError) {
                        LastError = $"CategoryName [{categoryName}],{conditionCache.Remark} condition `{conditionCache.ConditionString}` is error.\r\n{b.ErrorMsg}";
                        if (JumpConditionError) continue;
                        return Operand.Error(LastError);
                    }
                    if (b.BooleanValue == false) continue;
                }
                if (conditionCache.FormulaProg == null) {
                    return Operand.Error($"CategoryName [{categoryName}],{conditionCache.Remark} parse formula `{ conditionCache.FormulaString}` is error.\r\n{conditionCache.LastError}");
                }
                operand = Evaluate(conditionCache.FormulaProg);
                if (operand.IsError) {
                    LastError = $"CategoryName [{categoryName}],{conditionCache.Remark} formula `{conditionCache.FormulaString}` is error.\r\n{operand.ErrorMsg}";
                    if (JumpFormulaError) continue;
                    operand = Operand.Error(LastError);
                }
                return operand;
            }
            if (LastError != null) return Operand.Error(LastError);
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
            LastError = null;
            var conditionCaches = MultiConditionCache.GetConditionCaches(categoryName);
            foreach (var conditionCache in conditionCaches) {
                if (string.IsNullOrEmpty(conditionCache.ConditionString) == false) {
                    if (conditionCache.ConditionProg == null) {
                        throw new Exception($"CategoryName [{categoryName}],{conditionCache.Remark} parse condition `{ conditionCache.ConditionString}` is error.\r\n{conditionCache.LastError}");
                    }
                    var b = Evaluate(conditionCache.ConditionProg);
                    if (b.IsError) {
                        LastError = $"CategoryName [{categoryName}],{conditionCache.Remark} condition `{conditionCache.ConditionString}` is error.\r\n{b.ErrorMsg}";
                        if (JumpConditionError) continue;
                        throw new Exception(LastError);
                    }
                    if (b.BooleanValue == false) continue;
                }
                return conditionCache.Remark;
            }
            if (LastError != null) new Exception(LastError);
            throw new Exception($"CategoryName [{categoryName}] is missing.");
        }

        /// <summary>
        /// 查找备注,如果异常，返回默认值
        /// </summary>
        /// <param name="categoryName"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public string TrySearchRemark(string categoryName, string def)
        {
            try {
                return SearchRemark(categoryName);
            } catch (Exception ex) {
                LastError = ex.Message;
            }
            return def;
        }

        #region Parameter
        /// <summary>
        /// 清理参数
        /// </summary>
        public void ClearParameters()
        {
            _dict.Clear();
        }

        /// <summary>
        /// 添加自定义参数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        public void AddParameter(string key, Operand obj)
        {
            _dict[key] = obj;
        }
        /// <summary>
        /// 添加自定义参数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        public void AddParameter(string key, bool obj)
        {
            _dict[key] = Operand.Create(obj);
        }
        #region number
        /// <summary>
        /// 添加自定义参数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        public void AddParameter(string key, short obj)
        {
            _dict[key] = Operand.Create((int)obj);
        }
        /// <summary>
        /// 添加自定义参数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        public void AddParameter(string key, int obj)
        {
            _dict[key] = Operand.Create(obj);
        }
        /// <summary>
        /// 添加自定义参数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        public void AddParameter(string key, long obj)
        {
            _dict[key] = Operand.Create((double)obj);
        }
        /// <summary>
        /// 添加自定义参数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        public void AddParameter(string key, ushort obj)
        {
            _dict[key] = Operand.Create((int)obj);
        }
        /// <summary>
        /// 添加自定义参数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        public void AddParameter(string key, uint obj)
        {
            _dict[key] = Operand.Create((double)obj);
        }
        /// <summary>
        /// 添加自定义参数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        public void AddParameter(string key, ulong obj)
        {
            _dict[key] = Operand.Create((double)obj);
        }
        /// <summary>
        /// 添加自定义参数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        public void AddParameter(string key, float obj)
        {
            _dict[key] = Operand.Create((double)obj);
        }
        /// <summary>
        /// 添加自定义参数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        public void AddParameter(string key, double obj)
        {
            _dict[key] = Operand.Create(obj);
        }
        /// <summary>
        /// 添加自定义参数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        public void AddParameter(string key, decimal obj)
        {
            _dict[key] = Operand.Create((double)obj);
        }
        #endregion
        /// <summary>
        /// 添加自定义参数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        public void AddParameter(string key, string obj)
        {
            _dict[key] = Operand.Create(obj);
        }
        #region MyDate
        /// <summary>
        /// 添加自定义参数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        public void AddParameter(string key, MyDate obj)
        {
            _dict[key] = Operand.Create(obj);
        }
        /// <summary>
        /// 添加自定义参数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        public void AddParameter(string key, DateTime obj)
        {
            _dict[key] = Operand.Create(obj);
        }
        /// <summary>
        /// 添加自定义参数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        public void AddParameter(string key, TimeSpan obj)
        {
            _dict[key] = Operand.Create(obj);
        }
        #endregion
        #region array
        /// <summary>
        /// 添加自定义参数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        public void AddParameter(string key, List<Operand> obj)
        {
            _dict[key] = Operand.Create(obj);
        }
        /// <summary>
        /// 添加自定义参数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        public void AddParameter(string key, ICollection<string> obj)
        {
            _dict[key] = Operand.Create(obj);
        }
        /// <summary>
        /// 添加自定义参数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        public void AddParameter(string key, ICollection<double> obj)
        {
            _dict[key] = Operand.Create(obj);

        }
        /// <summary>
        /// 添加自定义参数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        public void AddParameter(string key, ICollection<int> obj)
        {
            _dict[key] = Operand.Create(obj);
        }
        /// <summary>
        /// 添加自定义参数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        public void AddParameter(string key, ICollection<bool> obj)
        {
            _dict[key] = Operand.Create(obj);
        }
        #endregion
        /// <summary>
        /// 添加自定义参数
        /// </summary>
        /// <param name="json"></param>
        public void AddParameterFromJson(string json)
        {
            if (json.StartsWith("{") && json.EndsWith("}")) {
                var jo = JsonMapper.ToObject(json);
                if (jo.IsObject) {
                    foreach (var item in jo.inst_object) {
                        var v = item.Value;
                        if (v.IsString)
                            _dict[item.Key] = Operand.Create(v.StringValue);
                        else if (v.IsBoolean)
                            _dict[item.Key] = Operand.Create(v.BooleanValue);
                        else if (v.IsDouble)
                            _dict[item.Key] = Operand.Create(v.NumberValue);
                        else if (v.IsObject)
                            _dict[item.Key] = Operand.Create(v);
                        else if (v.IsArray)
                            _dict[item.Key] = Operand.Create(v);
                        else if (v.IsNull)
                            _dict[item.Key] = Operand.CreateNull();
                    }
                    return;
                }
            }
            throw new Exception("Parameter is not json string.");
        }

        #endregion

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
