package toolgood.algorithm.internals.functions.mathSum;

import java.util.AbstractMap.SimpleEntry;
import java.util.List;
import java.util.ArrayList;
import java.math.BigDecimal;
import java.util.function.BiFunction;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.NoneEngine;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_3;
import toolgood.algorithm.internals.functions.FunctionUtil;

final class Function_AVERAGEIF extends Function_3 {

    Function_AVERAGEIF(FunctionBase[] funcs) {
        super(funcs);
        if (funcs.length < 2 || funcs.length > 3) {
            throw new IllegalArgumentException("Function '" + Name() + "' requires 2 to 3 parameters.");
        }
    }

    @Override
    public String Name() {
        return "AverageIf";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = GetArray_1(engine, tempParameter); if (args1.IsErrorOrNone()) { return args1; }
        Operand args2 = func2.Evaluate(engine, tempParameter); if (args2.IsErrorOrNone()) { return args2; }

        List<BigDecimal> list = new ArrayList<>();
        boolean o = FunctionUtil.FlattenToList(args1, list);
        if (o == false) { return ParameterError(1); }

        List<BigDecimal> sumdbs;
        if (func3 != null) {
            Operand args3 = GetArray_3(engine, tempParameter); if (args3.IsErrorOrNone()) { return args3; }
            sumdbs = new ArrayList<>();
            boolean o2 = FunctionUtil.FlattenToList(args3, sumdbs);
            if (o2 == false) { return ParameterError(3); }
        } else {
            sumdbs = list;
        }

        BigDecimal sum;
        int count;
        if (args2.IsNumber()) {
            count = FunctionUtil.GetCountIf(list, args2.NumberValue());
            sum = FunctionUtil.GetSumIf(list, args2.NumberValue(), sumdbs);
        } else {
            BigDecimal d = TryParseDecimal(args2.TextValue().trim());
            if (d != null) {
                count = FunctionUtil.GetCountIf(list, d);
                sum = FunctionUtil.GetSumIf(list, d, sumdbs);
            } else {
                SimpleEntry<String, BigDecimal> m2 = FunctionUtil.ParseSumIfMatch(args2.TextValue().trim());
                if (m2 != null) {
                    count = FunctionUtil.GetCountIf(list, m2.getKey(), m2.getValue());
                    sum = FunctionUtil.GetSumIf(list, m2.getKey(), m2.getValue(), sumdbs);
                } else {
                    return ParameterError(2);
                }
            }
        }
        if (count == 0) {
            return Div0Error();
        }
        return Operand.Create(sum.divide(BigDecimal.valueOf(count), java.math.RoundingMode.HALF_UP));
    }

    private static BigDecimal TryParseDecimal(String s) {
        try {
            return new BigDecimal(s);
        } catch (Exception e) {
            return null;
        }
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.NUMBER;
    }

    @Override
    void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, String op, String val) {
        func1.GetParameterTypes(noneEngine, result, OperandType.ARRAY);
        func2.GetParameterTypes(noneEngine, result, OperandType.NONE);
        if (func3 != null) {
            func3.GetParameterTypes(noneEngine, result, OperandType.ARRAY);
        }
    }
}
