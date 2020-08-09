package toolgood.algorithm;

import java.text.DecimalFormat;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

import toolgood.algorithm.Operand;
import toolgood.algorithm.litJson.JsonData;
import toolgood.algorithm.litJson.JsonMapper;

public class AlgorithmEngine {

    /// <summary>
    /// 使用EXCEL索引
    /// </summary>
    public boolean UseExcelIndex = true;
    /// <summary>
    /// 最后一个错误
    /// </summary>
    public String LastError;
    private ProgContext _context;
    private Map<String, Operand> _dict = new HashMap<String, Operand>();
    /// <summary>
    /// 自定义 函数
    /// </summary>
    public Func<String, List<Operand>, Operand> DiyFunction;

    /// <summary>
    /// 自定义参数
    /// </summary>
    /// <param name="parameter"></param>
    /// <returns></returns>
    protected Operand GetParameter(String parameter) {
        if (_dict.containsKey(parameter)) {
            return _dict.get(parameter);
        }
        return Operand.Error("Parameter [" + parameter + "] is missing.");
    }

    /// <summary>
    /// 自定义 函数
    /// </summary>
    /// <param name="funcName"></param>
    /// <param name="operands"></param>
    /// <returns></returns>
    protected Operand ExecuteDiyFunction(String funcName, List<Operand> operands) {
        if (DiyFunction != null) {
            return DiyFunction.Invoke(funcName, operands);
        }
        return Operand.Error("DiyFunction [" + funcName + "] is missing.");
    }

    /// <summary>
    /// 清理 自定义函数
    /// </summary>
    public void ClearDiyFunctions() {
        DiyFunction = null;
    }

    /// <summary>
    /// 清理参数
    /// </summary>
    public void ClearParameters() {
        _dict.clear();
    }

    /// <summary>
    /// 添加自定义参数
    /// </summary>
    /// <param name="key"></param>
    /// <param name="obj"></param>
    public void AddParameter(String key, Operand obj) {
        _dict.put(key, obj);
    }

    /// <summary>
    /// 添加自定义参数
    /// </summary>
    /// <param name="key"></param>
    /// <param name="obj"></param>
    public void AddParameter(String key, boolean obj) {
        _dict.put(key, Operand.Create(obj));
    }

    /// <summary>
    /// 添加自定义参数
    /// </summary>
    /// <param name="key"></param>
    /// <param name="obj"></param>
    public void AddParameter(String key, short obj) {
        _dict.put(key, Operand.Create(obj));
    }

    /// <summary>
    /// 添加自定义参数
    /// </summary>
    /// <param name="key"></param>
    /// <param name="obj"></param>
    public void AddParameter(String key, int obj) {
        _dict.put(key, Operand.Create(obj));
    }

    /// <summary>
    /// 添加自定义参数
    /// </summary>
    /// <param name="key"></param>
    /// <param name="obj"></param>
    public void AddParameter(String key, long obj) {
        _dict.put(key, Operand.Create(obj));
    }

    /// <summary>
    /// 添加自定义参数
    /// </summary>
    /// <param name="key"></param>
    /// <param name="obj"></param>
    public void AddParameter(String key, ushort obj) {
        _dict.put(key, Operand.Create(obj));
    }

    /// <summary>
    /// 添加自定义参数
    /// </summary>
    /// <param name="key"></param>
    /// <param name="obj"></param>
    public void AddParameter(String key, uint obj) {
        _dict.put(key, Operand.Create(obj));
    }

    /// <summary>
    /// 添加自定义参数
    /// </summary>
    /// <param name="key"></param>
    /// <param name="obj"></param>
    public void AddParameter(String key, ulong obj) {
        _dict.put(key, Operand.Create(obj));
    }

    /// <summary>
    /// 添加自定义参数
    /// </summary>
    /// <param name="key"></param>
    /// <param name="obj"></param>
    public void AddParameter(String key, float obj) {
        _dict.put(key, Operand.Create(obj));
    }

    /// <summary>
    /// 添加自定义参数
    /// </summary>
    /// <param name="key"></param>
    /// <param name="obj"></param>
    public void AddParameter(String key, double obj) {
        _dict.put(key, Operand.Create(obj));
    }

    /// <summary>
    /// 添加自定义参数
    /// </summary>
    /// <param name="key"></param>
    /// <param name="obj"></param>
    public void AddParameter(String key, DecimalFormat obj) {
        _dict.put(key, Operand.Create(obj));
    }

    /// <summary>
    /// 添加自定义参数
    /// </summary>
    /// <param name="key"></param>
    /// <param name="obj"></param>
    public void AddParameter(String key, String obj) {
        _dict.put(key, Operand.Create(obj));
    }

    /// <summary>
    /// 添加自定义参数
    /// </summary>
    /// <param name="key"></param>
    /// <param name="obj"></param>
    public void AddParameter(String key, Date obj) {
        _dict.put(key, Operand.Create(obj));
    }

    /// <summary>
    /// 添加自定义参数
    /// </summary>
    /// <param name="key"></param>
    /// <param name="obj"></param>
    public void AddParameter(String key, DateTime obj) {
        _dict.put(key, Operand.Create(obj));
    }

    /// <summary>
    /// 添加自定义参数
    /// </summary>
    /// <param name="key"></param>
    /// <param name="obj"></param>
    public void AddParameter(String key, TimeSpan obj) {
        _dict.put(key, Operand.Create(obj));
    }

    /// <summary>
    /// 添加自定义参数
    /// </summary>
    /// <param name="key"></param>
    /// <param name="obj"></param>
    public void AddParameter(String key, List<Operand> obj) {
        _dict.put(key, Operand.Create(obj));
    }

    /// <summary>
    /// 添加自定义参数
    /// </summary>
    /// <param name="key"></param>
    /// <param name="obj"></param>
    public void AddParameter(String key, List<String> obj) {
        _dict.put(key, Operand.Create(obj));
    }

    /// <summary>
    /// 添加自定义参数
    /// </summary>
    /// <param name="key"></param>
    /// <param name="obj"></param>
    public void AddParameter(String key, List<Double> obj)
        {
            _dict.put(key, Operand.Create(obj));

        }

    /// <summary>
    /// 添加自定义参数
    /// </summary>
    /// <param name="key"></param>
    /// <param name="obj"></param>
    public void AddParameter(String key, List<Integer> obj)
        {
            _dict.put(key, Operand.Create(obj));
        }

    /// <summary>
    /// 添加自定义参数
    /// </summary>
    /// <param name="key"></param>
    /// <param name="obj"></param>
    public void AddParameter(String key, List<Boolean> obj) {
        _dict.put(key, Operand.Create(obj));
    }

    /// <summary>
    /// 添加自定义参数
    /// </summary>
    /// <param name="json"></param>
    public void AddParameterFromJson(String json)
        {
            if (json.startsWith("{") && json.endsWith("}")) {
                JsonData jo = (JsonData)JsonMapper.ToObject(json);
                if (jo.IsObject()) {
                    foreach (var item in jo.inst_object) {
                        var v = item.Value;
                        if (v.IsString())
                            _dict[item.Key] = Operand.Create(v.StringValue);
                        else if (v.IsBoolean())
                            _dict[item.Key] = Operand.Create(v.BooleanValue);
                        else if (v.IsDouble())
                            _dict[item.Key] = Operand.Create(v.NumberValue);
                        else if (v.IsObject())
                            _dict[item.Key] = Operand.Create(v);
                        else if (v.IsArray())
                            _dict[item.Key] = Operand.Create(v);
                        else if (v.IsNull())
                            _dict[item.Key] = Operand.CreateNull();
                    }
                    return;
                }
            }
            throw new Exception("Parameter is not json String.");
        }

    class AntlrErrorListener extends IAntlrErrorListener<IToken> {
        public boolean IsError;
        public String ErrorMsg;

        public void SyntaxError(TextWriter output, IRecognizer recognizer, IToken offendingSymbol, int line,
                int charPositionInLine, String msg, RecognitionException e) {
            IsError = true;
            ErrorMsg = msg;
        }
    }

    /// <summary>
    /// 编译公式，默认
    /// </summary>
    /// <param name="exp">公式</param>
    /// <returns></returns>
    public boolean Parse(String exp) {
        if (String.isNullOrWhiteSpace(exp)) {
            LastError = "Parameter exp invalid !";
            return false;
        }
        // try {

        AntlrInputStream stream = new CaseChangingCharStream(new AntlrInputStream(exp));
        var lexer = new mathLexer(stream);
        var tokens = new CommonTokenStream(lexer);
        var parser = new mathParser(tokens);
        var antlrErrorListener = new AntlrErrorListener();
        parser.RemoveErrorListeners();
        parser.AddErrorListener(antlrErrorListener);

        var context = parser.prog();
        var end = context.Stop.StopIndex;
        if (end + 1 < exp.length()) {
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
        // } catch (Exception ex) {
        // LastError = ex.Message;
        // return false;
        // }
    }

    /// <summary>
    /// 执行函数
    /// </summary>
    /// <returns></returns>
    public Operand Evaluate() {
        if (_context == null) {
            LastError = "Please use Parse to compile formula !";
            throw new Exception("Please use Parse to compile formula !");
        }
        MathVisitor visitor = new MathVisitor();
        visitor.GetParameter += GetParameter;
        visitor.excelIndex = UseExcelIndex ? 1 : 0;
        visitor.DiyFunction += ExecuteDiyFunction;
        return visitor.Visit(_context);
    }

    /// <summary>
    /// 执行函数,如果异常，返回默认值
    /// </summary>
    /// <param name="exp"></param>
    /// <param name="def"></param>
    /// <returns></returns>
    public int TryEvaluate(String exp, int def) {
        try {
            if (Parse(exp)) {

                Operand obj = Evaluate();
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
    public double TryEvaluate(String exp, double def) {
        try {
            if (Parse(exp)) {

                Operand obj = Evaluate();
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
    public String TryEvaluate(String exp, String def) {
        try {
            if (Parse(exp)) {
                Operand obj = Evaluate();
                if (obj.IsNull) {
                    return null;
                }
                obj = obj.ToText("It can't be converted to String!");
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
    public boolean TryEvaluate(String exp, boolean def) {
        try {
            if (Parse(exp)) {
                Operand obj = Evaluate();
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
    public DateTime TryEvaluate(String exp, DateTime def) {
        try {
            if (Parse(exp)) {
                Operand obj = Evaluate();
                obj = obj.ToDate("It can't be converted to date!");
                if (obj.IsError) {
                    LastError = obj.ErrorMsg;
                    return def;
                }
                return (DateTime) obj.DateValue;
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
    public TimeSpan TryEvaluate(String exp, TimeSpan def) {
        try {
            if (Parse(exp)) {
                Operand obj = Evaluate();
                obj = obj.ToDate("It can't be converted to date!");
                if (obj.IsError) {
                    LastError = obj.ErrorMsg;
                    return def;
                }
                return (TimeSpan) obj.DateValue;
            }
        } catch (Exception ex) {
            LastError = ex.Message;
        }
        return def;
    }
}