using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using ToolGood.Algorithm.Internals;
using ToolGood.Algorithm.LitJson;
using static mathParser;

namespace ToolGood.Algorithm
{
    /// <summary>
    /// 算法引擎
    /// </summary>
    public class AlgorithmEngine
    {
        /// <summary>
        /// 使用EXCEL索引
        /// </summary>
        public bool UseExcelIndex { get; set; } = true;
        /// <summary>
        /// 最后一个错误
        /// </summary>
        public string LastError { get; private set; }
        private ProgContext _context;
        protected Dictionary<string, Operand> _dict = new Dictionary<string, Operand>();
        /// <summary>
        /// 自定义 函数
        /// </summary>
        public event Func<string, List<Operand>, Operand> DiyFunction;

        #region GetParameter
        /// <summary>
        /// 自定义参数
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        protected virtual Operand GetParameter(string parameter)
        {
            if (_dict.TryGetValue(parameter, out Operand operand)) {
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

        #region Parse

        /// <summary>
        /// 编译公式，默认
        /// </summary>
        /// <param name="exp">公式</param>
        /// <returns></returns>
        public bool Parse(string exp)
        {
            if (string.IsNullOrWhiteSpace(exp)) {
                LastError = "Parameter exp invalid !";
                return false;
            }
            //try {

            var stream = new AntlrCharStream(new AntlrInputStream(exp));
            var lexer = new mathLexer(stream);
            var tokens = new CommonTokenStream(lexer);
            var parser = new mathParser(tokens);
            var antlrErrorListener = new AntlrErrorListener();
            parser.RemoveErrorListeners();
            parser.AddErrorListener(antlrErrorListener);

            var context = parser.prog();
            var end = context.Stop.StopIndex;
            if (end + 1 < exp.Length) {
                _context = null;
                LastError = "Parameter exp invalid !";
                return false;
            }
            if (antlrErrorListener.IsError) {
                _context = null;
                LastError = antlrErrorListener.ErrorMsg;
                return false;
            }
            _context = context;
            return true;
            //} catch (Exception ex) {
            //    LastError = ex.Message;
            //    return false;
            //}
        }
        #endregion

        #region Evaluate
        /// <summary>
        /// 执行函数
        /// </summary>
        /// <returns></returns>
        public Operand Evaluate()
        {
            if (_context == null) {
                LastError = "Please use Parse to compile formula !";
                throw new Exception("Please use Parse to compile formula !");
            }
            var visitor = new MathVisitor();
            visitor.GetParameter += GetParameter;
            visitor.excelIndex = UseExcelIndex ? 1 : 0;
            visitor.DiyFunction += ExecuteDiyFunction;
            return visitor.Visit(_context);
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
                if (Parse(exp)) {

                    var obj = Evaluate();
                    obj = obj.ToNumber("It can't be converted to number!");
                    if (obj.IsError) {
                        LastError = obj.ErrorMsg;
                        return def;
                    }
                    return (ushort)obj.IntValue;
                }
            } catch (Exception ex) {
                LastError = ex.Message;
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
                if (Parse(exp)) {

                    var obj = Evaluate();
                    obj = obj.ToNumber("It can't be converted to number!");
                    if (obj.IsError) {
                        LastError = obj.ErrorMsg;
                        return def;
                    }
                    return (uint)obj.IntValue;
                }
            } catch (Exception ex) {
                LastError = ex.Message;
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
                if (Parse(exp)) {

                    var obj = Evaluate();
                    obj = obj.ToNumber("It can't be converted to number!");
                    if (obj.IsError) {
                        LastError = obj.ErrorMsg;
                        return def;
                    }
                    return (ulong)obj.IntValue;
                }
            } catch (Exception ex) {
                LastError = ex.Message;
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
                if (Parse(exp)) {

                    var obj = Evaluate();
                    obj = obj.ToNumber("It can't be converted to number!");
                    if (obj.IsError) {
                        LastError = obj.ErrorMsg;
                        return def;
                    }
                    return (short)obj.IntValue;
                }
            } catch (Exception ex) {
                LastError = ex.Message;
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
                if (Parse(exp)) {

                    var obj = Evaluate();
                    obj = obj.ToNumber("It can't be converted to number!");
                    if (obj.IsError) {
                        LastError = obj.ErrorMsg;
                        return def;
                    }
                    return obj.IntValue;
                }
            } catch (Exception ex) {
                LastError = ex.Message;
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
                if (Parse(exp)) {

                    var obj = Evaluate();
                    obj = obj.ToNumber("It can't be converted to number!");
                    if (obj.IsError) {
                        LastError = obj.ErrorMsg;
                        return def;
                    }
                    return obj.IntValue;
                }
            } catch (Exception ex) {
                LastError = ex.Message;
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
                if (Parse(exp)) {

                    var obj = Evaluate();
                    obj = obj.ToNumber("It can't be converted to number!");
                    if (obj.IsError) {
                        LastError = obj.ErrorMsg;
                        return def;
                    }
                    return (float)obj.NumberValue;
                }
            } catch (Exception ex) {
                LastError = ex.Message;
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
                if (Parse(exp)) {

                    var obj = Evaluate();
                    obj = obj.ToNumber("It can't be converted to number!");
                    if (obj.IsError) {
                        LastError = obj.ErrorMsg;
                        return def;
                    }
                    return obj.NumberValue;
                }
            } catch (Exception ex) {
                LastError = ex.Message;
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
                if (Parse(exp)) {

                    var obj = Evaluate();
                    obj = obj.ToNumber("It can't be converted to number!");
                    if (obj.IsError) {
                        LastError = obj.ErrorMsg;
                        return def;
                    }
                    return (decimal)obj.NumberValue;
                }
            } catch (Exception ex) {
                LastError = ex.Message;
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
                if (Parse(exp)) {
                    var obj = Evaluate();
                    if (obj.IsNull) {
                        return null;
                    }
                    obj = obj.ToText("It can't be converted to string!");
                    if (obj.IsError) {
                        LastError = obj.ErrorMsg;
                        return def;
                    }
                    return obj.TextValue;
                }
            } catch (Exception ex) {
                LastError = ex.Message;
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
                if (Parse(exp)) {
                    var obj = Evaluate();
                    obj = obj.ToBoolean("It can't be converted to bool!");
                    if (obj.IsError) {
                        LastError = obj.ErrorMsg;
                        return def;
                    }
                    return obj.BooleanValue;
                }
            } catch (Exception ex) {
                LastError = ex.Message;
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
                if (Parse(exp)) {
                    var obj = Evaluate();
                    obj = obj.ToMyDate("It can't be converted to MyDate!");
                    if (obj.IsError) {
                        LastError = obj.ErrorMsg;
                        return def;
                    }
                    return (DateTime)obj.DateValue;
                }
            } catch (Exception ex) {
                LastError = ex.Message;
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
                if (Parse(exp)) {
                    var obj = Evaluate();
                    obj = obj.ToMyDate("It can't be converted to MyDate!");
                    if (obj.IsError) {
                        LastError = obj.ErrorMsg;
                        return def;
                    }
                    return (TimeSpan)obj.DateValue;
                }
            } catch (Exception ex) {
                LastError = ex.Message;
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
                if (Parse(exp)) {
                    var obj = Evaluate();
                    obj = obj.ToMyDate("It can't be converted to MyDate!");
                    if (obj.IsError) {
                        LastError = obj.ErrorMsg;
                        return def;
                    }
                    return obj.DateValue;
                }
            } catch (Exception ex) {
                LastError = ex.Message;
            }
            return def;
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
        public String EvaluateFormula(String formula, List<char> splitChars)
        {
            if (string.IsNullOrEmpty(formula)) return "";

            List<String> sp = CharUtil.SplitFormula(formula, splitChars);

            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < sp.Count; i++) {
                String s = sp[i];
                if (s.Length == 1 && splitChars.Contains(s[0])) {
                    stringBuilder.Append(s);
                } else {
                    // TODO 替换此处
                    String d = TryEvaluate(s, "");
                    stringBuilder.Append(d.ToString());
                }
            }
            return stringBuilder.ToString();
        } 
        #endregion

    }
}
