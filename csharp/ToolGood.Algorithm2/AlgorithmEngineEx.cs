using Antlr4.Runtime;
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
        /// <summary>
        /// 是否忽略大小写
        /// </summary>
        public bool IgnoreCase { get; private set; }
        /// <summary>
        /// 保存到临时文档
        /// </summary>
        public bool UseTempDict { get; set; } = true;
        /// <summary>
        /// 自定义 函数
        /// </summary>
        private ConditionCache MultiConditionCache;
        /// <summary>
        /// 跳过条件错误 
        /// </summary>
        public bool JumpConditionError { get; set; } = true;
        /// <summary>
        /// 跳过公式错误
        /// </summary>
        public bool JumpFormulaError { get; set; } = false;
        private readonly Dictionary<string, Operand> _tempdict;


        #region 构造函数
        /// <summary>
        /// 默认不带缓存
        /// </summary>
        public AlgorithmEngineEx(ConditionCache multiConditionCache)
        {
            MultiConditionCache = multiConditionCache;
            _tempdict = new Dictionary<string, Operand>();
        }

        /// <summary>
        /// 带缓存关键字大小写参数
        /// </summary>
        /// <param name="multiConditionCache"></param>
        /// <param name="ignoreCase"></param>
        public AlgorithmEngineEx(ConditionCache multiConditionCache, bool ignoreCase)
        {
            MultiConditionCache = multiConditionCache;
            IgnoreCase = ignoreCase;
            if (ignoreCase) {
                _tempdict = new Dictionary<string, Operand>(StringComparer.OrdinalIgnoreCase);
            } else {
                _tempdict = new Dictionary<string, Operand>();
            }
        }

        #endregion

        #region GetParameter GetDiyParameterInside ExecuteDiyFunction
        private Operand GetDiyParameterInside(string parameter)
        {
            if (_tempdict.TryGetValue(parameter, out Operand operand)) {
                return operand;
            }
            var result = GetParameter(parameter);
            if (UseTempDict) {
                _tempdict[parameter] = result;
            }
            return result;
        }

        /// <summary>
        /// 自定义参数
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        protected virtual Operand GetParameter(string parameter)
        {
            Operand operand;
            var conditionCaches = MultiConditionCache.GetConditionCaches(parameter);
            foreach (var conditionCache in conditionCaches) {
                if (conditionCache.FormulaProg == null) continue;
                if (conditionCache.ConditionProg != null) {
                    var b = EvaluateCategory(conditionCache.ConditionProg);
                    if (b.IsError) {
                        LastError = $"Parameter [{parameter}],{conditionCache.Remark} condition `{conditionCache.ConditionString}` is error.\r\n{b.ErrorMsg}";
                        if (JumpConditionError) continue;
                        return Operand.Error(LastError);
                    }
                    if (b.BooleanValue == false) continue;
                }
                operand = EvaluateCategory(conditionCache.FormulaProg);
                if (operand.IsError) {
                    LastError = $"Parameter [{parameter}],{conditionCache.Remark} formula `{conditionCache.FormulaString}` is error.\r\n{operand.ErrorMsg}";
                    if (JumpFormulaError) continue;
                    return Operand.Error(LastError);
                }
                //_tempdict[parameter] = operand;
                return operand;
            }
            if (LastError != null) return Operand.Error(LastError);
            return Operand.Error($"Parameter [{parameter}] is missing.");
        }
        /// <summary>
        /// 自定义 函数
        /// </summary>
        /// <param name="funcName"></param>
        /// <param name="operands"></param>
        /// <returns></returns>
        protected virtual Operand ExecuteDiyFunction(string funcName, List<Operand> operands)
        {
            return Operand.Error($"DiyFunction [{funcName}] is missing.");
        }

        #endregion

        #region EvaluateCategory SearchRemark TrySearchRemark
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="categoryName"></param>
        /// <returns></returns>
        public Operand EvaluateCategory(string categoryName)
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
                    var b = EvaluateCategory(conditionCache.ConditionProg);
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
                operand = EvaluateCategory(conditionCache.FormulaProg);
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

        private Operand EvaluateCategory(ProgContext context)
        {
            try {
                var visitor = new MathVisitor();
                visitor.GetParameter += GetDiyParameterInside;
                visitor.excelIndex = UseExcelIndex ? 1 : 0;
                visitor.DiyFunction += ExecuteDiyFunction;
                return visitor.Visit(context);
            } catch (Exception ex) {
                LastError = ex.Message + "\r\n" + ex.StackTrace;
                return Operand.Error(ex.Message);
            }

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
                    var b = EvaluateCategory(conditionCache.ConditionProg);
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
                LastError = ex.Message + "\r\n" + ex.StackTrace;
            }
            return def;
        }
        #endregion

        #region Parameter
        /// <summary>
        /// 清理参数
        /// </summary>
        public void ClearParameters()
        {
            _tempdict.Clear();
        }

        /// <summary>
        /// 添加自定义参数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        public void AddParameter(string key, Operand obj)
        {
            _tempdict[key] = obj;
        }
        /// <summary>
        /// 添加自定义参数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        public void AddParameter(string key, bool obj)
        {
            _tempdict[key] = Operand.Create(obj);
        }
        #region number
        /// <summary>
        /// 添加自定义参数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        public void AddParameter(string key, short obj)
        {
            _tempdict[key] = Operand.Create((int)obj);
        }
        /// <summary>
        /// 添加自定义参数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        public void AddParameter(string key, int obj)
        {
            _tempdict[key] = Operand.Create(obj);
        }
        /// <summary>
        /// 添加自定义参数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        public void AddParameter(string key, long obj)
        {
            _tempdict[key] = Operand.Create((double)obj);
        }
        /// <summary>
        /// 添加自定义参数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        public void AddParameter(string key, ushort obj)
        {
            _tempdict[key] = Operand.Create((int)obj);
        }
        /// <summary>
        /// 添加自定义参数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        public void AddParameter(string key, uint obj)
        {
            _tempdict[key] = Operand.Create((double)obj);
        }
        /// <summary>
        /// 添加自定义参数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        public void AddParameter(string key, ulong obj)
        {
            _tempdict[key] = Operand.Create((double)obj);
        }
        /// <summary>
        /// 添加自定义参数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        public void AddParameter(string key, float obj)
        {
            _tempdict[key] = Operand.Create((double)obj);
        }
        /// <summary>
        /// 添加自定义参数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        public void AddParameter(string key, double obj)
        {
            _tempdict[key] = Operand.Create(obj);
        }
        /// <summary>
        /// 添加自定义参数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        public void AddParameter(string key, decimal obj)
        {
            _tempdict[key] = Operand.Create((double)obj);
        }
        #endregion
        /// <summary>
        /// 添加自定义参数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        public void AddParameter(string key, string obj)
        {
            _tempdict[key] = Operand.Create(obj);
        }
        #region MyDate
        /// <summary>
        /// 添加自定义参数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        public void AddParameter(string key, MyDate obj)
        {
            _tempdict[key] = Operand.Create(obj);
        }
        /// <summary>
        /// 添加自定义参数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        public void AddParameter(string key, DateTime obj)
        {
            _tempdict[key] = Operand.Create(obj);
        }
        /// <summary>
        /// 添加自定义参数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        public void AddParameter(string key, TimeSpan obj)
        {
            _tempdict[key] = Operand.Create(obj);
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
            _tempdict[key] = Operand.Create(obj);
        }
        /// <summary>
        /// 添加自定义参数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        public void AddParameter(string key, ICollection<string> obj)
        {
            _tempdict[key] = Operand.Create(obj);
        }
        /// <summary>
        /// 添加自定义参数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        public void AddParameter(string key, ICollection<double> obj)
        {
            _tempdict[key] = Operand.Create(obj);

        }
        /// <summary>
        /// 添加自定义参数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        public void AddParameter(string key, ICollection<int> obj)
        {
            _tempdict[key] = Operand.Create(obj);
        }
        /// <summary>
        /// 添加自定义参数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        public void AddParameter(string key, ICollection<bool> obj)
        {
            _tempdict[key] = Operand.Create(obj);
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
                            _tempdict[item.Key] = Operand.Create(v.StringValue);
                        else if (v.IsBoolean)
                            _tempdict[item.Key] = Operand.Create(v.BooleanValue);
                        else if (v.IsDouble)
                            _tempdict[item.Key] = Operand.Create(v.NumberValue);
                        else if (v.IsObject)
                            _tempdict[item.Key] = Operand.Create(v);
                        else if (v.IsArray)
                            _tempdict[item.Key] = Operand.Create(v);
                        else if (v.IsNull)
                            _tempdict[item.Key] = Operand.CreateNull();
                    }
                    return;
                }
            }
            throw new Exception("Parameter is not json string.");
        }

        #endregion

        #region TryEvaluateCategory
        /// <summary>
        /// 执行函数,如果异常，返回默认值
        /// </summary>
        /// <param name="categoryName"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public ushort TryEvaluateCategory(string categoryName, ushort def)
        {
            try {
                var obj = EvaluateCategory(categoryName);
                obj = obj.ToNumber("It can't be converted to number!");
                if (obj.IsError) {
                    LastError = obj.ErrorMsg;
                    return def;
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
        /// <param name="categoryName"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public uint TryEvaluateCategory(string categoryName, uint def)
        {
            try {
                var obj = EvaluateCategory(categoryName);
                obj = obj.ToNumber("It can't be converted to number!");
                if (obj.IsError) {
                    LastError = obj.ErrorMsg;
                    return def;
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
        /// <param name="categoryName"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public ulong TryEvaluateCategory(string categoryName, ulong def)
        {
            try {
                var obj = EvaluateCategory(categoryName);
                obj = obj.ToNumber("It can't be converted to number!");
                if (obj.IsError) {
                    LastError = obj.ErrorMsg;
                    return def;
                }
                return (ulong)obj.LongValue;
            } catch (Exception ex) {
                LastError = ex.Message + "\r\n" + ex.StackTrace;
            }
            return def;
        }
        /// <summary>
        /// 执行函数,如果异常，返回默认值
        /// </summary>
        /// <param name="categoryName"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public short TryEvaluateCategory(string categoryName, short def)
        {
            try {
                var obj = EvaluateCategory(categoryName);
                obj = obj.ToNumber("It can't be converted to number!");
                if (obj.IsError) {
                    LastError = obj.ErrorMsg;
                    return def;
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
        /// <param name="categoryName"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public int TryEvaluateCategory(string categoryName, int def)
        {
            try {
                var obj = EvaluateCategory(categoryName);
                obj = obj.ToNumber("It can't be converted to number!");
                if (obj.IsError) {
                    LastError = obj.ErrorMsg;
                    return def;
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
        /// <param name="categoryName"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public long TryEvaluateCategory(string categoryName, long def)
        {
            try {
                var obj = EvaluateCategory(categoryName);
                obj = obj.ToNumber("It can't be converted to number!");
                if (obj.IsError) {
                    LastError = obj.ErrorMsg;
                    return def;
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
        /// <param name="categoryName"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public float TryEvaluateCategory(string categoryName, float def)
        {
            try {
                var obj = EvaluateCategory(categoryName);
                obj = obj.ToNumber("It can't be converted to number!");
                if (obj.IsError) {
                    LastError = obj.ErrorMsg;
                    return def;
                }
                return (float)obj.NumberValue;
            } catch (Exception ex) {
                LastError = ex.Message + "\r\n" + ex.StackTrace;
            }
            return def;
        }
        /// <summary>
        /// 执行函数,如果异常，返回默认值
        /// </summary>
        /// <param name="categoryName"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public double TryEvaluateCategory(string categoryName, double def)
        {
            try {
                var obj = EvaluateCategory(categoryName);
                obj = obj.ToNumber("It can't be converted to number!");
                if (obj.IsError) {
                    LastError = obj.ErrorMsg;
                    return def;
                }
                return obj.NumberValue;
            } catch (Exception ex) {
                LastError = ex.Message + "\r\n" + ex.StackTrace;
            }
            return def;
        }
        /// <summary>
        /// 执行函数,如果异常，返回默认值
        /// </summary>
        /// <param name="categoryName"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public decimal TryEvaluateCategory(string categoryName, decimal def)
        {
            try {
                var obj = EvaluateCategory(categoryName);
                obj = obj.ToNumber("It can't be converted to number!");
                if (obj.IsError) {
                    LastError = obj.ErrorMsg;
                    return def;
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
        /// <param name="categoryName"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public string TryEvaluateCategory(string categoryName, string def)
        {
            try {
                var obj = EvaluateCategory(categoryName);
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
                LastError = ex.Message + "\r\n" + ex.StackTrace;
            }
            return def;
        }
        /// <summary>
        /// 执行函数,如果异常，返回默认值
        /// </summary>
        /// <param name="categoryName"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public bool TryEvaluateCategory(string categoryName, bool def)
        {
            try {
                var obj = EvaluateCategory(categoryName);
                obj = obj.ToBoolean("It can't be converted to bool!");
                if (obj.IsError) {
                    LastError = obj.ErrorMsg;
                    return def;
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
        /// <param name="categoryName"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public DateTime TryEvaluateCategory(string categoryName, DateTime def)
        {
            try {
                var obj = EvaluateCategory(categoryName);
                obj = obj.ToMyDate("It can't be converted to MyDate!");
                if (obj.IsError) {
                    LastError = obj.ErrorMsg;
                    return def;
                }
                return (DateTime)obj.DateValue;
            } catch (Exception ex) {
                LastError = ex.Message + "\r\n" + ex.StackTrace;
            }
            return def;
        }
        /// <summary>
        /// 执行函数,如果异常，返回默认值
        /// </summary>
        /// <param name="categoryName"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public TimeSpan TryEvaluateCategory(string categoryName, TimeSpan def)
        {
            try {
                var obj = EvaluateCategory(categoryName);
                obj = obj.ToMyDate("It can't be converted to MyDate!");
                if (obj.IsError) {
                    LastError = obj.ErrorMsg;
                    return def;
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
        /// <param name="categoryName"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public MyDate TryEvaluateCategory_MyDate(string categoryName, MyDate def)
        {
            try {
                var obj = EvaluateCategory(categoryName);
                obj = obj.ToMyDate("It can't be converted to MyDate!");
                if (obj.IsError) {
                    LastError = obj.ErrorMsg;
                    return def;
                }
                return obj.DateValue;
            } catch (Exception ex) {
                LastError = ex.Message + "\r\n" + ex.StackTrace;
            }
            return def;
        }
        #endregion

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
                var obj = Evaluate(exp);
                obj = obj.ToNumber("It can't be converted to number!");
                if (obj.IsError) {
                    LastError = obj.ErrorMsg;
                    return def;
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
                var obj = Evaluate(exp);
                obj = obj.ToNumber("It can't be converted to number!");
                if (obj.IsError) {
                    LastError = obj.ErrorMsg;
                    return def;
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
                var obj = Evaluate(exp);
                obj = obj.ToNumber("It can't be converted to number!");
                if (obj.IsError) {
                    LastError = obj.ErrorMsg;
                    return def;
                }
                return (ulong)obj.LongValue;
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
                var obj = Evaluate(exp);
                obj = obj.ToNumber("It can't be converted to number!");
                if (obj.IsError) {
                    LastError = obj.ErrorMsg;
                    return def;
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
                var obj = Evaluate(exp);
                obj = obj.ToNumber("It can't be converted to number!");
                if (obj.IsError) {
                    LastError = obj.ErrorMsg;
                    return def;
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
                var obj = Evaluate(exp);
                obj = obj.ToNumber("It can't be converted to number!");
                if (obj.IsError) {
                    LastError = obj.ErrorMsg;
                    return def;
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
                var obj = Evaluate(exp);
                obj = obj.ToNumber("It can't be converted to number!");
                if (obj.IsError) {
                    LastError = obj.ErrorMsg;
                    return def;
                }
                return (float)obj.NumberValue;
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
                var obj = Evaluate(exp);
                obj = obj.ToNumber("It can't be converted to number!");
                if (obj.IsError) {
                    LastError = obj.ErrorMsg;
                    return def;
                }
                return obj.NumberValue;
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
                var obj = Evaluate(exp);
                obj = obj.ToNumber("It can't be converted to number!");
                if (obj.IsError) {
                    LastError = obj.ErrorMsg;
                    return def;
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
                var obj = Evaluate(exp);
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
                var obj = Evaluate(exp);
                obj = obj.ToBoolean("It can't be converted to bool!");
                if (obj.IsError) {
                    LastError = obj.ErrorMsg;
                    return def;
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
                var obj = Evaluate(exp);
                obj = obj.ToMyDate("It can't be converted to MyDate!");
                if (obj.IsError) {
                    LastError = obj.ErrorMsg;
                    return def;
                }
                return (DateTime)obj.DateValue;
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
                var obj = Evaluate(exp);
                obj = obj.ToMyDate("It can't be converted to MyDate!");
                if (obj.IsError) {
                    LastError = obj.ErrorMsg;
                    return def;
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
                var obj = Evaluate(exp);
                obj = obj.ToMyDate("It can't be converted to MyDate!");
                if (obj.IsError) {
                    LastError = obj.ErrorMsg;
                    return def;
                }
                return obj.DateValue;
            } catch (Exception ex) {
                LastError = ex.Message + "\r\n" + ex.StackTrace;
            }
            return def;
        }



        #endregion

        #region GetSimplifiedFormula
        /// <summary>
        /// 获取简化公式
        /// </summary>
        /// <param name="formula">公式</param>
        /// <returns></returns>
        public String GetSimplifiedFormula(String formula)
        {
            try {
                ProgContext _context = Parse(formula);
                MathSimplifiedFormulaVisitor visitor = new MathSimplifiedFormulaVisitor();
                visitor.GetParameter += GetDiyParameterInside;
                visitor.excelIndex = UseExcelIndex ? 1 : 0;
                visitor.DiyFunction += ExecuteDiyFunction;

                Operand obj = visitor.Visit(_context);
                obj = obj.ToText("It can't be converted to String!");
                if (obj.IsError) {
                    LastError = obj.ErrorMsg;
                    return null;
                }
                return obj.TextValue;

            } catch (Exception ex) {
                LastError = ex.Message;
            }
            return null;
        }
        #endregion

        #region EvaluateFormula
        /// <summary>
        /// 计算公式
        /// </summary>
        /// <param name="formula">公式</param>
        /// <param name="splitChars">分隔符</param>
        /// <returns></returns>
        public String EvaluateFormula(String formula, params char[] splitChars)
        {
            if (string.IsNullOrEmpty(formula)) return "";
            return EvaluateFormula(formula, splitChars.ToList());
        }
        /// <summary>
        /// 计算公式
        /// </summary>
        /// <param name="formula">公式</param>
        /// <param name="splitChars">分隔符</param>
        /// <returns></returns>
        public virtual String EvaluateFormula(String formula, List<char> splitChars)
        {
            if (string.IsNullOrEmpty(formula)) return "";

            List<String> sp = CharUtil.SplitFormula(formula, splitChars);

            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < sp.Count; i++) {
                String s = sp[i];
                if (s.Length == 1 && splitChars.Contains(s[0])) {
                    stringBuilder.Append(s);
                } else {
                    String d = "";
                    try {
                        Operand operand = Evaluate(s);
                        d = operand.ToText().TextValue;
                    } catch (Exception) { }
                    stringBuilder.Append(d);
                }
            }
            return stringBuilder.ToString();
        }
        /// <summary>
        /// 执行一次
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public Operand Evaluate(string exp)
        {
            ProgContext context = Parse(exp);
            if (context == null) {
                return Operand.Create(LastError);
            }
            return EvaluateCategory(context);
        }

        private ProgContext Parse(string exp)
        {
            if (string.IsNullOrWhiteSpace(exp)) { return null; }
            try {
                var stream = new AntlrCharStream(new AntlrInputStream(exp));
                var lexer = new mathLexer(stream);
                var tokens = new CommonTokenStream(lexer);
                var parser = new mathParser(tokens);
                var antlrErrorListener = new AntlrErrorListener();
                parser.RemoveErrorListeners();
                parser.AddErrorListener(antlrErrorListener);

                var context = parser.prog();
                if (antlrErrorListener.IsError) {
                    LastError = antlrErrorListener.ErrorMsg;
                    return null;
                }
                return context;
            } catch (Exception ex) {
                LastError = ex.Message + "\r\n" + ex.StackTrace;
            }
            return null;
        }
        #endregion


    }
}
