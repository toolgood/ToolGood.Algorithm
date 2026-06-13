package toolgood.algorithm.internals.functions.mathSum;

import java.util.AbstractMap.SimpleEntry;
import java.util.List;
import java.util.ArrayList;
import java.math.BigDecimal;
import java.util.function.BiFunction;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.functions.NoneEngine;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_2;
import toolgood.algorithm.internals.functions.FunctionUtil;

final class Function_COUNTIF extends Function_2 {

    Function_COUNTIF(FunctionBase[] funcs) {
        super(funcs);
        if (funcs.length != 2) {
            throw new IllegalArgumentException("Function '" + Name() + "' requires exactly 2 parameters.");
        }
    }

    @Override
    public String Name() {
        return "CountIf";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = GetArray_1(engine, tempParameter);
        if (args1.IsErrorOrNone()) { return args1; }

        Operand args2 = func2.Evaluate(engine, tempParameter);
        if (args2.IsErrorOrNone()) { return args2; }

        List<BigDecimal> list = new ArrayList<>();
        boolean o = FunctionUtil.FlattenToList(args1, list);
        if (o == false) { return ParameterError(1); }
        int count;
        if (args2.IsNumber()) {
            count = FunctionUtil.GetCountIf(list, args2.NumberValue());
        } else {
            String span = args2.TextValue().trim();
            BigDecimal d = TryParseDecimal(span);
            if (d != null) {
                count = FunctionUtil.GetCountIf(list, d);
            } else {
                SimpleEntry<String, BigDecimal> m2 = FunctionUtil.ParseSumIfMatch(args2.TextValue().trim());
                if (m2 != null) {
                    count = FunctionUtil.GetCountIf(list, m2.getKey(), m2.getValue());
                } else {
                    return ParameterError(2);
                }
            }
        }
        return Operand.Create(count);
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
    public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, String op, String val) {
        func1.GetParameterTypes(noneEngine, result, OperandType.ARRAY);
        func2.GetParameterTypes(noneEngine, result, OperandType.NONE);
    }
}
