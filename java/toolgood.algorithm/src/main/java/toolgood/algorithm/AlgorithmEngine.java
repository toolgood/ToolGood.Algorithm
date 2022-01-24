package toolgood.algorithm;

import java.math.BigDecimal;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.function.Function;

import toolgood.algorithm.internals.AntlrErrorListener;
import toolgood.algorithm.internals.CharUtil;
import toolgood.algorithm.internals.AntlrCharStream;
import toolgood.algorithm.internals.MathVisitor;
import toolgood.algorithm.internals.MyFunction;
import toolgood.algorithm.litJson.JsonData;
import toolgood.algorithm.litJson.JsonMapper;
import toolgood.algorithm.math.mathLexer;
import toolgood.algorithm.math.mathParser;
import toolgood.algorithm.math.mathParser2.*;

import org.antlr.v4.runtime.CharStreams;
import org.antlr.v4.runtime.CommonTokenStream;
import org.antlr.v4.runtime.RecognitionException;
import org.joda.time.DateTime;


public class AlgorithmEngine {
    public boolean UseExcelIndex = true;
    public String LastError;
    private ProgContext _context;
    protected final Map<String, Operand> _dict = new HashMap<String, Operand>();
    public Function<MyFunction, Operand> DiyFunction;

    protected Operand GetParameter(final String parameter) throws Exception {
        if (_dict.containsKey(parameter)) {
            return _dict.get(parameter);
        }
        return Operand.Error("Parameter [" + parameter + "] is missing.");
    }

    protected Operand ExecuteDiyFunction(final String funcName, final List<Operand> operands) {
        if (DiyFunction != null) {
            MyFunction f=new MyFunction();
            f.Name=funcName;
            f.OperandList=operands;
            return DiyFunction.apply(f);
        }
        return Operand.Error("DiyFunction [" + funcName + "] is missing.");
    }

    public void ClearDiyFunctions() {
        DiyFunction = null;
    }

    public void ClearParameters() {
        _dict.clear();
    }

    public void AddParameter(final String key, final Operand obj) {
        _dict.put(key, obj);
    }

    public void AddParameter(final String key, final boolean obj) {
        _dict.put(key, Operand.Create(obj));
    }

    public void AddParameter(final String key, final short obj) {
        _dict.put(key, Operand.Create(obj));
    }

    public void AddParameter(final String key, final int obj) {
        _dict.put(key, Operand.Create(obj));
    }

    public void AddParameter(final String key, final long obj) {
        _dict.put(key, Operand.Create(obj));
    }

    public void AddParameter(final String key, final float obj) {
        _dict.put(key, Operand.Create(obj));
    }

    public void AddParameter(final String key, final double obj) {
        _dict.put(key, Operand.Create(obj));
    }

    public void AddParameter(final String key, final BigDecimal obj) {
        _dict.put(key, Operand.Create(obj));
    }

    public void AddParameter(final String key, final String obj) {
        _dict.put(key, Operand.Create(obj));
    }

    public void AddParameter(final String key, final MyDate obj) {
        _dict.put(key, Operand.Create(obj));
    }

    public void AddParameter(final String key, final List<Operand> obj) {
        _dict.put(key, Operand.Create(obj));
    }

    public void AddParameterFromJson(final String json) throws Exception {
        if (json.startsWith("{") && json.endsWith("}")) {
            final JsonData jo = (JsonData) JsonMapper.ToObject(json);
            if (jo.IsObject()) {
                for (String item : jo.inst_object.keySet()) {
                    final JsonData v = jo.inst_object.get(item);
                    if (v.IsString())
                        _dict.put(item, Operand.Create(v.StringValue()));
                    else if (v.IsBoolean())
                        _dict.put(item, Operand.Create(v.BooleanValue()));
                    else if (v.IsDouble())
                        _dict.put(item, Operand.Create(v.NumberValue()));
                    else if (v.IsObject())
                        _dict.put(item, Operand.Create(v));
                    else if (v.IsArray())
                        _dict.put(item, Operand.Create(v));
                    else if (v.IsNull())
                        _dict.put(item, Operand.CreateNull());
                }
                return;
            }
        }
        throw new Exception("Parameter is not json String.");
    }


    public boolean Parse(final String exp) throws RecognitionException {
        if (exp == null || exp.equals("")) {
            LastError = "Parameter exp invalid !";
            return false;
        }
        // try {
        final AntlrCharStream stream = new AntlrCharStream(CharStreams.fromString(exp));
        final mathLexer lexer = new mathLexer(stream);
        final CommonTokenStream tokens = new CommonTokenStream(lexer);
        final mathParser parser = new mathParser(tokens);
        final AntlrErrorListener antlrErrorListener = new AntlrErrorListener();
        parser.removeErrorListeners();
        parser.addErrorListener(antlrErrorListener);
        final ProgContext context = parser.prog();

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

    public Operand Evaluate() throws Exception {
        if (_context == null) {
            LastError = "Please use Parse to compile formula !";
            throw new Exception("Please use Parse to compile formula !");
        }
        final MathVisitor visitor = new MathVisitor();
        visitor.GetParameter = f -> {
            try {
                return GetParameter(f);
            } catch (Exception e) {
            }
            return null;
        };
        visitor.excelIndex = UseExcelIndex ? 1 : 0;
        visitor.DiyFunction = f->{ return ExecuteDiyFunction(f.Name,f.OperandList); };
        return visitor.visit(_context);
    }
    public int TryEvaluate(final String exp, final int defvalue) {
        try {
            if (Parse(exp)) {
                Operand obj = Evaluate();
                obj = obj.ToNumber("It can't be converted to number!");
                if (obj.IsError()) {
                    LastError = obj.ErrorMsg();
                    return defvalue;
                }
                return obj.IntValue();
            }
        } catch (final Exception ex) {
            LastError = ex.getMessage();
        }
        return defvalue;
    }
    public double TryEvaluate(final String exp, final double defvalue) {
        try {
            if (Parse(exp)) {
                Operand obj = Evaluate();
                obj = obj.ToNumber("It can't be converted to number!");
                if (obj.IsError()) {
                    LastError = obj.ErrorMsg();
                    return defvalue;
                }
                return obj.NumberValue();
            }
        } catch (final Exception ex) {
            LastError = ex.getMessage();
        }
        return defvalue;
    }
    public String TryEvaluate(final String exp, final String defvalue) {
        try {
            if (Parse(exp)) {
                Operand obj = Evaluate();
                if (obj.IsNull()) {
                    return null;
                }
                obj = obj.ToText("It can't be converted to String!");
                if (obj.IsError()) {
                    LastError = obj.ErrorMsg();
                    return defvalue;
                }
                return obj.TextValue();
            }
        } catch (final Exception ex) {
            LastError = ex.getMessage();
        }
        return defvalue;
    }
    public boolean TryEvaluate(final String exp, final boolean defvalue) {
        try {
            if (Parse(exp)) {
                Operand obj = Evaluate();
                obj = obj.ToBoolean("It can't be converted to bool!");
                if (obj.IsError()) {
                    LastError = obj.ErrorMsg();
                    return defvalue;
                }
                return obj.BooleanValue();
            }
        } catch (final Exception ex) {
            LastError = ex.getMessage();
        }
        return defvalue;
    }
    public DateTime TryEvaluate(final String exp, final DateTime defvalue) {
        try {
            if (Parse(exp)) {
                Operand obj = Evaluate();
                obj = obj.ToDate("It can't be converted to bool!");
                if (obj.IsError()) {
                    LastError = obj.ErrorMsg();
                    return defvalue;
                }
                return obj.DateValue().ToDateTime();
            }
        } catch (final Exception ex) {
            LastError = ex.getMessage();
        }
        return defvalue;
    }
    public MyDate TryEvaluate(final String exp, final MyDate defvalue) {
        try {
            if (Parse(exp)) {
                Operand obj = Evaluate();
                obj = obj.ToDate("It can't be converted to bool!");
                if (obj.IsError()) {
                    LastError = obj.ErrorMsg();
                    return defvalue;
                }
                return obj.DateValue();
            }
        } catch (final Exception ex) {
            LastError = ex.getMessage();
        }
        return defvalue;
    }
    /**
     * 计算公式
     * @param formula 公式
     * @param splitChar 分隔符
     * @return
     */
    public String EvaluateFormula(String formula, Character splitChar) {
        if (formula == null || formula.equals("")) return "";
        List<Character> splitChars = new ArrayList<>();
        splitChars.add(splitChar);
        return EvaluateFormula(formula, splitChars);
    }
    /**
     * 计算公式
     * @param formula 公式
     * @param splitChars 分隔符
     * @return
     */
    public String EvaluateFormula(String formula, List<Character> splitChars) {
        if (formula == null || formula.equals("")) return "";
        List<String> sp = CharUtil.SplitFormula(formula,splitChars);

        StringBuilder stringBuilder = new StringBuilder();
        for (int i = 0; i < sp.size(); i++) {
            String s = sp.get(i);
            if (s.length() == 1 && splitChars.contains(s.charAt(0))) {
                stringBuilder.append(s);
            } else {
                // TODO 替换此处
                String d = TryEvaluate(s, "");
                stringBuilder.append(d);
            }
        }
        return stringBuilder.toString();
    }
}