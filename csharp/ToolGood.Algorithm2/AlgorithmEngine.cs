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
        /// <summary>
        /// 保存到临时文档
        /// </summary>
        public bool UseTempDict { get; set; } = false;
        /// <summary>
        /// 是否忽略大小写
        /// </summary>
        public bool IgnoreCase { get; private set; }

        private ProgContext _context;
        private readonly Dictionary<string, Operand> _tempdict;


        #region 构造函数
        /// <summary>
        /// 默认不带缓存
        /// </summary>
        public AlgorithmEngine()
        {
            _tempdict = new Dictionary<string, Operand>();
        }
        /// <summary>
        /// 带缓存关键字大小写参数
        /// </summary>
        /// <param name="ignoreCase"></param>
        public AlgorithmEngine(bool ignoreCase)
        {
            IgnoreCase = ignoreCase;
            if (ignoreCase) {
                _tempdict = new Dictionary<string, Operand>(StringComparer.OrdinalIgnoreCase);
            } else {
                _tempdict = new Dictionary<string, Operand>();
            }
        }
        #endregion


        #region GetParameter GetDiyParameterInside  ExecuteDiyFunction
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

        #region Parse Evaluate

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
            var stream = new AntlrCharStream(new AntlrInputStream(exp));
            var lexer = new mathLexer(stream);
            var tokens = new CommonTokenStream(lexer);
            var parser = new mathParser(tokens);
            var antlrErrorListener = new AntlrErrorListener();
            parser.RemoveErrorListeners();
            parser.AddErrorListener(antlrErrorListener);

            var context = parser.prog();
            if (antlrErrorListener.IsError) {
                _context = null;
                LastError = antlrErrorListener.ErrorMsg;
                return false;
            }
            _context = context;
            return true;
        }

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
            visitor.GetParameter += GetDiyParameterInside;
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
                if (Parse(exp)) {
                    var obj = Evaluate();
                    obj = obj.ToMyDate("It can't be converted to DateTime!");
                    if (obj.IsError) {
                        LastError = obj.ErrorMsg;
                        return def;
                    }
                    return (DateTime)obj.DateValue;
                }
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
                if (Parse(exp)) {
                    var obj = Evaluate();
                    obj = obj.ToMyDate("It can't be converted to TimeSpan!");
                    if (obj.IsError) {
                        LastError = obj.ErrorMsg;
                        return def;
                    }
                    return (TimeSpan)obj.DateValue;
                }
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
                LastError = ex.Message + "\r\n" + ex.StackTrace;
            }
            return def;
        }
        #endregion

        #region GetSimplifiedFormula
        /// <summary>
        /// 获取简化公式
        /// </summary>
        /// <param name="formula"></param>
        /// <returns></returns>
        public String GetSimplifiedFormula(String formula)
        {
            try {
                if (Parse(formula)) {
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
                }
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
                    // TODO 替换此处
                    String d = TryEvaluate(s, "");
                    stringBuilder.Append(d);
                }
            }
            return stringBuilder.ToString();
        }
        #endregion

    }
}
