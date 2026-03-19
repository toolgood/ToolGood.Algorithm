package toolgood.algorithm;

import java.math.BigDecimal;
import java.util.List;
import java.util.concurrent.ConcurrentHashMap;

import org.antlr.v4.runtime.CharStreams;
import org.antlr.v4.runtime.CommonTokenStream;

import toolgood.algorithm.enums.AreaUnitType;
import toolgood.algorithm.enums.DistanceUnitType;
import toolgood.algorithm.enums.MassUnitType;
import toolgood.algorithm.enums.VolumeUnitType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.visitors.AntlrCharStream;
import toolgood.algorithm.internals.visitors.AntlrErrorData;
import toolgood.algorithm.internals.visitors.AntlrErrorListener;
import toolgood.algorithm.internals.visitors.MathFunctionVisitor;
import toolgood.algorithm.math.mathLexer;
import toolgood.algorithm.math.mathParser;
import toolgood.algorithm.operands.MyDate;

public class AlgorithmEngine {
    public int ExcelIndex = 1;

    public boolean UseLocalTime = true;

    public DistanceUnitType DistanceUnit = DistanceUnitType.M;

    public AreaUnitType AreaUnit = AreaUnitType.M2;

    public VolumeUnitType VolumeUnit = VolumeUnitType.M3;

    public MassUnitType MassUnit = MassUnitType.KG;

    public String LastError;

    public boolean UseParseCache = false;
    private final ConcurrentHashMap<String, FunctionBase> _parseCache = new ConcurrentHashMap<>();

    public void SetLastError(String value) {
        LastError = value;
    }

    public void setUseExcelIndex(boolean value) {
        ExcelIndex = value ? 1 : 0;
    }

    public Operand GetParameter(String parameter) {
        return Operand.Error("Parameter [" + parameter + "] is missing.");
    }

    public Operand ExecuteDiyFunction(String parameter, List<Operand> args) {
        return Operand.Error("DiyFunction [" + parameter + "] is missing.");
    }

    public FunctionBase Parse(String exp) throws Exception {
        LastError = null;
        if (exp == null || exp.trim().isEmpty()) {
            LastError = "Parameter exp invalid !";
            throw new Exception(LastError);
        }
        if (UseParseCache) {
            return _parseCache.computeIfAbsent(exp.trim(), e -> {
                try {
                    return ParseInternal(e);
                } catch (Exception e1) {
                    e1.printStackTrace();
                }
                return null;
            });
        }
        return ParseInternal(exp);
    }

    private FunctionBase ParseInternal(String exp) throws Exception {
        AntlrCharStream stream = new AntlrCharStream(CharStreams.fromString(exp));
        mathLexer lexer = new mathLexer(stream);
        CommonTokenStream tokens = new CommonTokenStream(lexer);
        mathParser parser = new mathParser(tokens);

        AntlrErrorData data = new AntlrErrorData();
        AntlrErrorListener listener = new AntlrErrorListener(data);
        lexer.removeErrorListeners();
        lexer.addErrorListener(listener);
        parser.removeErrorListeners();
        parser.addErrorListener(listener);

        mathParser.ProgContext context = parser.prog();
        if (data.isError()) {
            LastError = data.getErrorMsg();
            throw new Exception(LastError);
        }
        MathFunctionVisitor visitor = new MathFunctionVisitor();
        return visitor.visit(context);
    }

    public Operand Evaluate(FunctionBase function) throws Exception {
        return function.Evaluate(this);
    }

    public int TryEvaluate(String exp, int def) {
        return TryEvaluateCore(exp, def, o -> o.IsNumber() ? o : o.ToNumber("It can't be converted to number!"), o -> o.IntValue());
    }

    public long TryEvaluate(String exp, long def) {
        return TryEvaluateCore(exp, def, o -> o.IsNumber() ? o : o.ToNumber("It can't be converted to number!"), o -> o.LongValue());
    }

    public double TryEvaluate(String exp, double def) {
        return TryEvaluateCore(exp, def, o -> o.IsNumber() ? o : o.ToNumber("It can't be converted to number!"), o -> o.DoubleValue());
    }

    public BigDecimal TryEvaluate(String exp, BigDecimal def) {
        return TryEvaluateCore(exp, def, o -> o.IsNumber() ? o : o.ToNumber("It can't be converted to number!"), o -> o.NumberValue());
    }

    public String TryEvaluate(String exp, String def) {
        return TryEvaluateCore(exp, def, o -> o.IsText() ? o : o.ToText("It can't be converted to string!"), o -> o.TextValue());
    }

    public boolean TryEvaluate(String exp, boolean def) {
        return TryEvaluateCore(exp, def, o -> o.IsBoolean() ? o : o.ToBoolean("It can't be converted to bool!"), o -> o.BooleanValue());
    }

    public MyDate TryEvaluate_MyDate(String exp, MyDate def) {
        return TryEvaluateCore(exp, def,
                o -> o.IsDate() ? o : o.ToMyDate("It can't be converted to DateTime!"),
                o -> o.DateValue());
    }

    private <T> T TryEvaluateCore(String exp, T def, java.util.function.Function<Operand, Operand> convert, java.util.function.Function<Operand, T> getValue) {
        try {
            FunctionBase function = Parse(exp);
            Operand obj = function.Evaluate(this);
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
}
