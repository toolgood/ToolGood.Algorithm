package toolgood.algorithm.internals.functions;

import java.util.List;
import java.util.function.BiFunction;
import java.util.function.Function;
import org.joda.time.DateTime;
import org.joda.time.DateTimeZone;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.MyDate;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.ParameterType;

/**
 * Represents the base class for all function implementations that can be calculated by an algorithm engine.
 */
public abstract class FunctionBase {
    /**
     * 获取函数名称
     */
    public abstract String Name();

    /**
     * 执行函数计算
     */
    public abstract Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter);

    /**
     * 获取结果类型
     */
    public abstract OperandType GetResultType();

    /**
     * 获取参数类型列表
     */
    public List<ParameterType> GetParameterTypes(AlgorithmEngine engine) {
        NoneEngine noneEngine = new NoneEngine(engine);
        List<ParameterType> result = new java.util.ArrayList<>();
        GetParameterTypes(noneEngine, result, OperandType.NONE);
        return result;
    }

    /**
     * 内部方法，获取参数类型
     */
    abstract void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, String op, String val);

    void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType) {
        GetParameterTypes(noneEngine, result, operandType, null, null);
    }

    @Override
    public String toString() {
        StringBuilder stringBuilder = new StringBuilder();
        ToString(stringBuilder, false);
        return stringBuilder.toString();
    }

    public abstract void ToString(StringBuilder stringBuilder, boolean addBrackets);

    // region ConvertToText
    protected Operand ConvertToText(Operand arg, int paramIndex) {
        return arg.ToText(String.format("Function '%s' parameter %d is error!", Name(), paramIndex));
    }

    protected Operand ConvertToBoolean(Operand arg, int paramIndex) {
        return arg.ToBoolean(String.format("Function '%s' parameter %d is error!", Name(), paramIndex));
    }

    protected Operand ConvertToNumber(Operand arg, int paramIndex) {
        return arg.ToNumber(String.format("Function '%s' parameter %d is error!", Name(), paramIndex));
    }

    protected Operand ConvertToArray(Operand arg, int paramIndex) {
        return arg.ToArray(String.format("Function '%s' parameter %d is error!", Name(), paramIndex));
    }

    protected Operand ConvertToDate(Operand arg, int paramIndex) {
        return arg.ToDate(String.format("Function '%s' parameter %d is error!", Name(), paramIndex));
    }
    // endregion

    // region ParameterError FunctionError
    protected Operand ParameterError(int paramIndex) {
        return Operand.Error(String.format("Function '%s' parameter %d is error!", Name(), paramIndex));
    }

    protected Operand FunctionError() {
        return Operand.Error(String.format("Function '%s' parameter is error!", Name()));
    }

    protected Operand CompareError() {
        return Operand.Error(String.format("Function '%s' compare is error.", Name()));
    }

    protected Operand Div0Error() {
        return Operand.Error(String.format("Function '%s' Div 0 error!", Name()));
    }
    // endregion

    // region TryEvaluate
    private <T> T TryEvaluate(AlgorithmEngine engine, T def,
            Function<Operand, Operand> converter,
            Function<Operand, T> resultConverter,
            BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        try {
            Operand obj = Evaluate(engine, tempParameter);
            Operand converted = converter.apply(obj);
            if (converted.IsError()) {
                engine.LastError = converted.ErrorMsg();
                return def;
            }
            return resultConverter.apply(converted);
        } catch (Exception ex) {
            engine.LastError = ex.getMessage();
        }
        return def;
    }

    public int TryEvaluate(AlgorithmEngine engine, int def, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        return TryEvaluate(engine, (Integer) def,
                obj -> obj.Type() == OperandType.NUMBER ? obj : obj.ToNumber("It can't be converted to number!"),
                obj -> obj.IntValue(), tempParameter);
    }

    public java.math.BigDecimal TryEvaluate(AlgorithmEngine engine, java.math.BigDecimal def, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        return TryEvaluate(engine, def,
                obj -> obj.Type() == OperandType.NUMBER ? obj : obj.ToNumber("It can't be converted to number!"),
                obj -> obj.NumberValue(), tempParameter);
    }

    public String TryEvaluate(AlgorithmEngine engine, String def, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        return TryEvaluate(engine, def,
                obj -> obj.Type() == OperandType.TEXT ? obj : obj.ToText("It can't be converted to string!"),
                obj -> obj.TextValue(), tempParameter);
    }

    public boolean TryEvaluate(AlgorithmEngine engine, boolean def, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        return TryEvaluate(engine, (Boolean) def,
                obj -> obj.Type() == OperandType.BOOLEAN ? obj : obj.ToBoolean("It can't be converted to bool!"),
                obj -> obj.BooleanValue(), tempParameter);
    }

    public DateTime TryEvaluate(AlgorithmEngine engine, DateTime def, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        return TryEvaluate(engine, def,
                obj -> obj.Type() == OperandType.DATE ? obj : obj.ToDate("It can't be converted to DateTime!"),
                obj -> {
                    if (engine.UseLocalTime) {
                        return obj.DateValue().ToDateTime(DateTimeZone.getDefault());
                    }
                    return obj.DateValue().ToDateTime(DateTimeZone.UTC);
                }, tempParameter);
    }

    public MyDate TryEvaluate(AlgorithmEngine engine, MyDate def, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        return TryEvaluate(engine, def,
                obj -> obj.Type() == OperandType.DATE ? obj : obj.ToDate("It can't be converted to DateTime!"),
                obj -> obj.DateValue(), tempParameter);
    }
    // endregion
}
