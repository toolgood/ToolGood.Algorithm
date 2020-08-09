package toolgood.algorithm;

import java.text.DecimalFormat;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

import toolgood.algorithm.Operand;
import toolgood.algorithm.litJson.JsonData;
import toolgood.algorithm.litJson.JsonMapper;
import toolgood.algorithm.math.mathLexer;
import toolgood.algorithm.math.mathParser2.*;

import org.antlr.v4.runtime.ANTLRInputStream;
import org.antlr.v4.runtime.CommonTokenStream;

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
    private final Map<String, Operand> _dict = new HashMap<String, Operand>();
    /// <summary>
    /// 自定义 函数
    /// </summary>
    public Func<String, List<Operand>, Operand> DiyFunction;

    /// <summary>
    /// 自定义参数
    /// </summary>
    /// <param name="parameter"></param>
    /// <returns></returns>
    protected Operand GetParameter(final String parameter) {
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
    protected Operand ExecuteDiyFunction(final String funcName, final List<Operand> operands) {
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
    public void AddParameter(final String key, final Operand obj) {
        _dict.put(key, obj);
    }

    /// <summary>
    /// 添加自定义参数
    /// </summary>
    /// <param name="key"></param>
    /// <param name="obj"></param>
    public void AddParameter(final String key, final boolean obj) {
        _dict.put(key, Operand.Create(obj));
    }

    /// <summary>
    /// 添加自定义参数
    /// </summary>
    /// <param name="key"></param>
    /// <param name="obj"></param>
    public void AddParameter(final String key, final short obj) {
        _dict.put(key, Operand.Create(obj));
    }

    /// <summary>
    /// 添加自定义参数
    /// </summary>
    /// <param name="key"></param>
    /// <param name="obj"></param>
    public void AddParameter(final String key, final int obj) {
        _dict.put(key, Operand.Create(obj));
    }

    /// <summary>
    /// 添加自定义参数
    /// </summary>
    /// <param name="key"></param>
    /// <param name="obj"></param>
    public void AddParameter(final String key, final long obj) {
        _dict.put(key, Operand.Create(obj));
    }
 
    /// <summary>
    /// 添加自定义参数
    /// </summary>
    /// <param name="key"></param>
    /// <param name="obj"></param>
    public void AddParameter(final String key, final float obj) {
        _dict.put(key, Operand.Create(obj));
    }

    /// <summary>
    /// 添加自定义参数
    /// </summary>
    /// <param name="key"></param>
    /// <param name="obj"></param>
    public void AddParameter(final String key, final double obj) {
        _dict.put(key, Operand.Create(obj));
    }

    /// <summary>
    /// 添加自定义参数
    /// </summary>
    /// <param name="key"></param>
    /// <param name="obj"></param>
    public void AddParameter(final String key, final BigDecimal obj) {
        _dict.put(key, Operand.Create(obj));
    }

    /// <summary>
    /// 添加自定义参数
    /// </summary>
    /// <param name="key"></param>
    /// <param name="obj"></param>
    public void AddParameter(final String key, final String obj) {
        _dict.put(key, Operand.Create(obj));
    }

    /// <summary>
    /// 添加自定义参数
    /// </summary>
    /// <param name="key"></param>
    /// <param name="obj"></param>
    public void AddParameter(final String key, final Date obj) {
        _dict.put(key, Operand.Create(obj));
    }

    /// <summary>
    /// 添加自定义参数
    /// </summary>
    /// <param name="key"></param>
    /// <param name="obj"></param>
    public void AddParameter(final String key, final DateTime obj) {
        _dict.put(key, Operand.Create(obj));
    }

    /// <summary>
    /// 添加自定义参数
    /// </summary>
    /// <param name="key"></param>
    /// <param name="obj"></param>
    public void AddParameter(final String key, final TimeSpan obj) {
        _dict.put(key, Operand.Create(obj));
    }

    /// <summary>
    /// 添加自定义参数
    /// </summary>
    /// <param name="key"></param>
    /// <param name="obj"></param>
    public void AddParameter(final String key, final List<Operand> obj) {
        _dict.put(key, Operand.Create(obj));
    }

    /// <summary>
    /// 添加自定义参数
    /// </summary>
    /// <param name="key"></param>
    /// <param name="obj"></param>
    public void AddParameter(final String key, final List<String> obj) {
        _dict.put(key, Operand.Create(obj));
    }

    /// <summary>
    /// 添加自定义参数
    /// </summary>
    /// <param name="key"></param>
    /// <param name="obj"></param>
    public void AddParameter(final String key, final List<Double> obj) {
        _dict.put(key, Operand.Create(obj));

    }

    /// <summary>
    /// 添加自定义参数
    /// </summary>
    /// <param name="key"></param>
    /// <param name="obj"></param>
    public void AddParameter(final String key, final List<Integer> obj) {
        _dict.put(key, Operand.Create(obj));
    }

    /// <summary>
    /// 添加自定义参数
    /// </summary>
    /// <param name="key"></param>
    /// <param name="obj"></param>
    public void AddParameter(final String key, final List<Boolean> obj) {
        _dict.put(key, Operand.Create(obj));
    }

    /// <summary>
    /// 添加自定义参数
    /// </summary>
    /// <param name="json"></param>
    public void AddParameterFromJson(final String json)
        {
            if (json.startsWith("{") && json.endsWith("}")) {
                final JsonData jo = (JsonData)JsonMapper.ToObject(json);
                if (jo.IsObject()) {
                    for (JsonData item : jo.inst_object) {
                        
                    }
                    foreach (var item in jo.inst_object) {
                        final JsonData v = item.Value;
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

    class AntlrErrorListener extends AntlrErrorListener<Token> {
        public boolean IsError;
        public String ErrorMsg;

        public void SyntaxError(final TextWriter output, final IRecognizer recognizer, final IToken offendingSymbol,
                final int line, final int charPositionInLine, final String msg, final RecognitionException e) {
            IsError = true;
            ErrorMsg = msg;
        }
    }

    /// <summary>
    /// 编译公式，默认
    /// </summary>
    /// <param name="exp">公式</param>
    /// <returns></returns>
    public boolean Parse(final String exp) throws RecognitionException  {
        if (String.isNullOrWhiteSpace(exp)) {
            LastError = "Parameter exp invalid !";
            return false;
        }
        // try {

        final AntlrInputStream stream = new CaseChangingCharStream(new AntlrInputStream(exp));
        final mathLexer lexer = new mathLexer(stream);
        final var tokens = new CommonTokenStream(lexer);
        final mathParser parser = new mathParser(tokens);
        final var antlrErrorListener = new AntlrErrorListener();
        parser.RemoveErrorListeners();
        parser.AddErrorListener(antlrErrorListener);

        final ProgContext context = parser.prog();
        final var end = context.Stop.StopIndex;
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
        final MathVisitor visitor = new MathVisitor();
        visitor.GetParameter += GetParameter;
        visitor.excelIndex = UseExcelIndex ? 1 : 0;
        visitor.DiyFunction += ExecuteDiyFunction;
        return visitor.Visit(_context);
    }

    /// <summary>
    /// 执行函数,如果异常，返回默认值
    /// </summary>
    /// <param name="exp"></param>
    /// <param name="defvalue"></param>
    /// <returns></returns>
    public int TryEvaluate(final String exp, final int defvalue) {
        try {
            if (Parse(exp)) {

                Operand obj = Evaluate();
                obj = obj.ToNumber("It can't be converted to number!");
                if (obj.IsError()) {
                    LastError = obj.ErrorMsg;
                    return defvalue;
                }
                return obj.IntValue;
            }
        } catch (final Exception ex) {
            LastError = ex.Message;
        }
        return defvalue;
    }

    /// <summary>
    /// 执行函数,如果异常，返回默认值
    /// </summary>
    /// <param name="exp"></param>
    /// <param name="defvalue"></param>
    /// <returns></returns>
    public double TryEvaluate(final String exp, final double defvalue) {
        try {
            if (Parse(exp)) {

                Operand obj = Evaluate();
                obj = obj.ToNumber("It can't be converted to number!");
                if (obj.IsError()) {
                    LastError = obj.ErrorMsg;
                    return defvalue;
                }
                return obj.NumberValue;
            }
        } catch (final Exception ex) {
            LastError = ex.Message;
        }
        return defvalue;
    }

    /// <summary>
    /// 执行函数,如果异常，返回默认值
    /// </summary>
    /// <param name="exp"></param>
    /// <param name="defvalue"></param>
    /// <returns></returns>
    public String TryEvaluate(final String exp, final String defvalue) {
        try {
            if (Parse(exp)) {
                Operand obj = Evaluate();
                if (obj.IsNull) {
                    return null;
                }
                obj = obj.ToText("It can't be converted to String!");
                if (obj.IsError()) {
                    LastError = obj.ErrorMsg;
                    return defvalue;
                }
                return obj.TextValue;
            }
        } catch (final Exception ex) {
            LastError = ex.Message;
        }
        return defvalue;
    }

    /// <summary>
    /// 执行函数,如果异常，返回默认值
    /// </summary>
    /// <param name="exp"></param>
    /// <param name="defvalue"></param>
    /// <returns></returns>
    public boolean TryEvaluate(final String exp, final boolean defvalue) {
        try {
            if (Parse(exp)) {
                Operand obj = Evaluate();
                obj = obj.ToBoolean("It can't be converted to bool!");
                if (obj.IsError()) {
                    LastError = obj.ErrorMsg;
                    return defvalue;
                }
                return obj.BooleanValue;
            }
        } catch (final Exception ex) {
            LastError = ex.Message;
        }
        return defvalue;
    }

    /// <summary>
    /// 执行函数,如果异常，返回默认值
    /// </summary>
    /// <param name="exp"></param>
    /// <param name="defvalue"></param>
    /// <returns></returns>
    public DateTime TryEvaluate(final String exp, final DateTime defvalue) {
        try {
            if (Parse(exp)) {
                Operand obj = Evaluate();
                obj = obj.ToDate("It can't be converted to date!");
                if (obj.IsError()) {
                    LastError = obj.ErrorMsg;
                    return defvalue;
                }
                return (DateTime) obj.DateValue;
            }
        } catch (final Exception ex) {
            LastError = ex.Message;
        }
        return defvalue;
    }

    /// <summary>
    /// 执行函数,如果异常，返回默认值
    /// </summary>
    /// <param name="exp"></param>
    /// <param name="defvalue"></param>
    /// <returns></returns>
    public TimeSpan TryEvaluate(final String exp, final TimeSpan defvalue) {
        try {
            if (Parse(exp)) {
                Operand obj = Evaluate();
                obj = obj.ToDate("It can't be converted to date!");
                if (obj.IsError()) {
                    LastError = obj.ErrorMsg;
                    return defvalue;
                }
                return (TimeSpan) obj.DateValue;
            }
        } catch (final Exception ex) {
            LastError = ex.Message;
        }
        return defvalue;
    }
}