package toolgood.algorithm;

import java.util.List;

import org.antlr.v4.runtime.CharStreams;
import org.antlr.v4.runtime.CommonTokenStream;

import toolgood.algorithm.enums.AreaUnitType;
import toolgood.algorithm.enums.DistanceUnitType;
import toolgood.algorithm.enums.MassUnitType;
import toolgood.algorithm.enums.VolumeUnitType;
import toolgood.algorithm.internals.MyDate;
import toolgood.algorithm.Operand;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.visitors.AntlrErrorTextWriter;
import toolgood.algorithm.math.mathLexer;
import toolgood.algorithm.math.mathParser;
import toolgood.algorithm.internals.visitors.MathFunctionVisitor;

public class AlgorithmEngine {
    public int ExcelIndex = 1;

    /**
     * 使用 本地时间�?影响 时间截转�?
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
     * 最后一个错�?
     */
    private String LastError;

    /**
     * 获取最后一个错�?
     */
    public String getLastError() {
        return LastError;
    }

    /**
     * 设置最后一个错�?
     */
    public void SetLastError(String error) {
        LastError = error;
    }

    /**
     * 使用EXCEL索引
     */
    public void setUseExcelIndex(boolean value) {
        ExcelIndex = value ? 1 : 0;
    }

    /**
     * 自定义参�?请重写此方法
     */
    public Operand getParameter(String parameter) {
        return Operand.Error("Parameter [" + parameter + "] is missing.");
    }

    /**
     * 自定义函�?请重写此方法
     */
    public Operand executeDiyFunction(String parameter, List<Operand> args) {
        return Operand.Error("DiyFunction [" + parameter + "] is missing.");
    }

    /**
     * 编译公式，默�?
     */
    public FunctionBase Parse(String exp) throws Exception {
        LastError = null;
        if (exp == null || exp.trim().isEmpty()) {
            LastError = "Parameter exp invalid !";
            throw new Exception(LastError);
        }
        AntlrErrorTextWriter antlrErrorTextWriter = new AntlrErrorTextWriter();
        org.antlr.v4.runtime.CharStream stream = org.antlr.v4.runtime.CharStreams.fromString(exp);
        mathLexer lexer = new mathLexer(stream);
        org.antlr.v4.runtime.CommonTokenStream tokens = new org.antlr.v4.runtime.CommonTokenStream(lexer);
        mathParser parser = new mathParser(tokens);

        mathParser.ProgContext context = parser.prog();
        if (antlrErrorTextWriter.IsError()) {
            LastError = antlrErrorTextWriter.ErrorMsg();
            throw new Exception(LastError);
        }
        MathFunctionVisitor visitor = new MathFunctionVisitor();
        return visitor.visit(context);
    }

    /**
     * 执行函数
     */
    public Operand Evaluate(FunctionBase function) {
        return function.Evaluate(this);
    }

    /**
     * 执行函数,如果异常，返回默认�?
     */
    public short TryEvaluate(String exp, short def) {
        try {
            var function = Parse(exp);
            var obj = function.Evaluate(this);
            if (obj.IsNotNumber()) {
                obj = obj.ToNumber("It can't be converted to number!");
                if (obj.IsError()) {
                    SetLastError(obj.ErrorMsg);
                    return def;
                }
            }
            return (short) obj.IntValue;
        } catch (Exception ex) {
            SetLastError(ex.getMessage() + "\r\n" + ex.getStackTrace());
        }
        return def;
    }

    /**
     * 执行函数,如果异常，返回默认�?
     */
    public int TryEvaluate(String exp, int def) {
        try {
            var function = Parse(exp);
            var obj = function.Evaluate(this);
            if (obj.IsNotNumber()) {
                obj = obj.ToNumber("It can't be converted to number!");
                if (obj.IsError()) {
                    SetLastError(obj.ErrorMsg);
                    return def;
                }
            }
            return obj.IntValue;
        } catch (Exception ex) {
            SetLastError(ex.getMessage() + "\r\n" + ex.getStackTrace());
        }
        return def;
    }

    /**
     * 执行函数,如果异常，返回默认�?
     */
    public long TryEvaluate(String exp, long def) {
        try {
            var function = Parse(exp);
            var obj = function.Evaluate(this);
            if (obj.IsNotNumber()) {
                obj = obj.ToNumber("It can't be converted to number!");
                if (obj.IsError()) {
                    SetLastError(obj.ErrorMsg);
                    return def;
                }
            }
            return obj.LongValue;
        } catch (Exception ex) {
            SetLastError(ex.getMessage() + "\r\n" + ex.getStackTrace());
        }
        return def;
    }

    /**
     * 执行函数,如果异常，返回默认�?
     */
    public float TryEvaluate(String exp, float def) {
        try {
            var function = Parse(exp);
            var obj = function.Evaluate(this);
            if (obj.IsNotNumber()) {
                obj = obj.ToNumber("It can't be converted to number!");
                if (obj.IsError()) {
                    SetLastError(obj.ErrorMsg);
                    return def;
                }
            }
            return (float) obj.DoubleValue;
        } catch (Exception ex) {
            SetLastError(ex.getMessage() + "\r\n" + ex.getStackTrace());
        }
        return def;
    }

    /**
     * 执行函数,如果异常，返回默认�?
     */
    public double TryEvaluate(String exp, double def) {
        try {
            var function = Parse(exp);
            var obj = function.Evaluate(this);
            if (obj.IsNotNumber()) {
                obj = obj.ToNumber("It can't be converted to number!");
                if (obj.IsError()) {
                    SetLastError(obj.ErrorMsg);
                    return def;
                }
            }
            return obj.DoubleValue;
        } catch (Exception ex) {
            SetLastError(ex.getMessage() + "\r\n" + ex.getStackTrace());
        }
        return def;
    }

    /**
     * 执行函数,如果异常，返回默认�?
     */
    public String TryEvaluate(String exp, String def) {
        try {
            var function = Parse(exp);
            var obj = function.Evaluate(this);
            if (obj.IsNotText()) {
                obj = obj.ToText("It can't be converted to string!");
                if (obj.IsError()) {
                    SetLastError(obj.ErrorMsg);
                    return def;
                }
            }
            return obj.TextValue;
        } catch (Exception ex) {
            SetLastError(ex.getMessage() + "\r\n" + ex.getStackTrace());
        }
        return def;
    }

    /**
     * 执行函数,如果异常，返回默认�?
     */
    public boolean TryEvaluate(String exp, boolean def) {
        try {
            var function = Parse(exp);
            var obj = function.Evaluate(this);
            if (obj.IsNotBoolean()) {
                obj = obj.ToBoolean("It can't be converted to bool!");
                if (obj.IsError()) {
                    SetLastError(obj.ErrorMsg);
                    return def;
                }
            }
            return obj.BooleanValue;
        } catch (Exception ex) {
            SetLastError(ex.getMessage() + "\r\n" + ex.getStackTrace());
        }
        return def;
    }

    /**
     * 执行函数,如果异常，返回默认�?
     */
    public MyDate TryEvaluate_MyDate(String exp, MyDate def) {
        try {
            var function = Parse(exp);
            var obj = function.Evaluate(this);
            if (obj.IsNotDate()) {
                obj = obj.ToMyDate("It can't be converted to DateTime!");
                if (obj.IsError()) {
                    SetLastError(obj.ErrorMsg);
                    return def;
                }
            }
            return obj.DateValue;
        } catch (Exception ex) {
            SetLastError(ex.getMessage() + "\r\n" + ex.getStackTrace());
        }
        return def;
    }
}
