package toolgood.algorithm;

import java.math.BigDecimal;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.function.Function;

import org.antlr.v4.runtime.CharStreams;
import org.antlr.v4.runtime.CommonTokenStream;
import org.joda.time.DateTime;
import org.joda.time.DateTimeZone;
import java.time.Duration;

import toolgood.algorithm.enums.AreaUnitType;
import toolgood.algorithm.enums.DistanceUnitType;
import toolgood.algorithm.enums.MassUnitType;
import toolgood.algorithm.enums.VolumeUnitType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.visitors.AntlrCharStream;
import toolgood.algorithm.internals.visitors.AntlrErrorListener;
import toolgood.algorithm.internals.visitors.MathFunctionVisitor;
import toolgood.algorithm.math.mathLexer;
import toolgood.algorithm.math.mathParser;

/**
 * 算法引擎类。
 */
public class AlgorithmEngine {
    int ExcelIndex = 1;

    /**
     * 使用 本地时间，影响 时间戳转化
     */
    public boolean UseLocalTime = true;

    /**
     * 长度单位
     */
    public DistanceUnitType DistanceUnit = DistanceUnitType.M;

    /**
     * 面积单位
     */
    public AreaUnitType AreaUnit = AreaUnitType.M2;

    /**
     * 体积单位
     */
    public VolumeUnitType VolumeUnit = VolumeUnitType.M3;

    /**
     * 重量单位
     */
    public MassUnitType MassUnit = MassUnitType.KG;

    /**
     * 最后一个错误
     */
    public String LastError;

    /**
     * 使用EXCEL索引
     */
    public void SetUseExcelIndex(boolean value) {
        ExcelIndex = value ? 1 : 0;
    }

    /**
     * 使用缓存，不要在并发环境下运行
     */
    public boolean UseParseCache = false;
    private final Map<String, FunctionBase> _parseCache = new HashMap<>();

    /**
     * 自定义参数 请重写此方法
     */
    public Operand GetParameter(String parameter) {
        return Operand.Error("Parameter [" + parameter + "] is missing.");
    }

    /**
     * 自定义函数 请重写此方法
     */
    public Operand ExecuteDiyFunction(String parameter, List<Operand> args) {
        return Operand.Error("DiyFunction [" + parameter + "] is missing.");
    }

    // #region Parse Evaluate

    /**
     * 编译公式，默认
     */
    public FunctionBase Parse(String exp) {
        LastError = null;
        if (exp == null || exp.trim().isEmpty()) {
            LastError = "Parameter exp invalid !";
            throw new IllegalArgumentException(LastError);
        }
        exp = exp.trim();
        if (UseParseCache) {
            FunctionBase r = _parseCache.get(exp);
            if (r != null) {
                return r;
            }
            r = ParseInternal(exp, true);
            _parseCache.put(exp, r);
            return r;
        }
        return ParseInternal(exp, true);
    }

    /**
     * 编译公式，失败尽可能返回null,不抛错
     */
    public FunctionBase ParseWithoutError(String exp) {
        LastError = null;
        if (exp == null || exp.trim().isEmpty()) {
            LastError = "Parameter exp invalid !";
            return null;
        }
        exp = exp.trim();
        if (UseParseCache) {
            FunctionBase r = _parseCache.get(exp);
            if (r != null) {
                return r;
            }
            r = ParseInternal(exp, false);
            if (r != null) {
                _parseCache.put(exp, r);
            }
            return r;
        }
        return ParseInternal(exp, false);
    }

    private FunctionBase ParseInternal(String exp, boolean throwOnError) {
        AntlrCharStream stream = new AntlrCharStream(CharStreams.fromString(exp));
        mathLexer lexer = new mathLexer(stream);
        CommonTokenStream tokens = new CommonTokenStream(lexer);
        mathParser parser = new mathParser(tokens);

        AntlrErrorListener antlrErrorListener = new AntlrErrorListener();
        lexer.removeErrorListeners();
        lexer.addErrorListener(antlrErrorListener);
        parser.removeErrorListeners();
        parser.addErrorListener(antlrErrorListener);

        mathParser.ProgContext context = parser.prog();
        if (antlrErrorListener.IsError) {
            LastError = antlrErrorListener.ErrorMsg;
            if (throwOnError) {
                throw new IllegalArgumentException(LastError);
            }
            return null;
        }
        MathFunctionVisitor visitor = new MathFunctionVisitor();
        return visitor.visit(context);
    }

    /**
     * 执行函数
     */
    public Operand Evaluate(FunctionBase function) {
        return function.Evaluate(this, (engine, name) -> engine.GetParameter(name));
    }

    // #endregion Parse Evaluate

    // #region TryEvaluate

    /**
     * 执行函数,如果异常，返回默认值
     */
    public int TryEvaluate(String exp, int def) {
        return TryEvaluateCore(exp, def, o -> o.IsNumber() ? o : o.ToNumber("It can't be converted to number!"), o -> o.IntValue());
    }

    /**
     * 执行函数,如果异常，返回默认值
     */
    public long TryEvaluate(String exp, long def) {
        return TryEvaluateCore(exp, def, o -> o.IsNumber() ? o : o.ToNumber("It can't be converted to number!"), o -> o.LongValue());
    }

    /**
     * 执行函数,如果异常，返回默认值
     */
    public double TryEvaluate(String exp, double def) {
        return TryEvaluateCore(exp, def, o -> o.IsNumber() ? o : o.ToNumber("It can't be converted to number!"), o -> o.DoubleValue());
    }

    /**
     * 执行函数,如果异常，返回默认值
     */
    public BigDecimal TryEvaluate(String exp, BigDecimal def) {
        return TryEvaluateCore(exp, def, o -> o.IsNumber() ? o : o.ToNumber("It can't be converted to number!"), o -> o.NumberValue());
    }

    /**
     * 执行函数,如果异常，返回默认值
     */
    public String TryEvaluate(String exp, String def) {
        return TryEvaluateCore(exp, def, o -> o.IsText() ? o : o.ToText("It can't be converted to string!"), o -> o.TextValue());
    }

    /**
     * 执行函数,如果异常，返回默认值
     */
    public boolean TryEvaluate(String exp, boolean def) {
        return TryEvaluateCore(exp, def, o -> o.IsBoolean() ? o : o.ToBoolean("It can't be converted to bool!"), o -> o.BooleanValue());
    }

    /**
     * 执行函数,如果异常，返回默认值
     */
    public DateTime TryEvaluate(String exp, DateTime def) {
        return TryEvaluateCore(exp, def,
                o -> o.IsDate() ? o : o.ToMyDate("It can't be converted to DateTime!"),
                o -> o.DateValue().ToDateTime(UseLocalTime ? DateTimeZone.getDefault() : DateTimeZone.UTC));
    }

    /**
     * 执行函数,如果异常，返回默认值
     */
    public Duration TryEvaluate(String exp, Duration def) {
        return TryEvaluateCore(exp, def,
                o -> o.IsDate() ? o : o.ToMyDate("It can't be converted to DateTime!"),
                o -> Duration.ofMillis(o.DateValue().ToDateTime().getMillis()));
    }

    /**
     * 执行函数,如果异常，返回默认值。
     * 解决 def 为 null 二义性问题
     */
    public MyDate TryEvaluate_MyDate(String exp, MyDate def) {
        return TryEvaluateCore(exp, def,
                o -> o.IsDate() ? o : o.ToMyDate("It can't be converted to DateTime!"),
                o -> o.DateValue());
    }

    private <T> T TryEvaluateCore(String exp, T def, Function<Operand, Operand> convert, Function<Operand, T> getValue) {
        try {
            FunctionBase function = ParseWithoutError(exp);
            if (function == null) {
                return def;
            }
            Operand obj = Evaluate(function);
            obj = convert.apply(obj);
            if (obj.IsError()) {
                LastError = obj.ErrorMsg();
                return def;
            }
            return getValue.apply(obj);
        } catch (Exception ex) {
            LastError = ex.toString();
        }
        return def;
    }

    // #endregion TryEvaluate

}
