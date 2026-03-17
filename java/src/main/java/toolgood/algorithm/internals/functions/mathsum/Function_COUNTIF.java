package toolgood.algorithm.internals.functions.mathsum;

import java.math.BigDecimal;
import java.util.ArrayList;
import java.util.List;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.FunctionUtil;
import toolgood.algorithm.internals.functions.Function_2;
import toolgood.algorithm.internals.functions.NoneEngine;
import toolgood.algorithm.internals.functions.Tuple;

public final class Function_COUNTIF extends Function_2 {
    public Function_COUNTIF(FunctionBase[] funcs) {
        super(funcs);
    }

    @Override
    public String Name() {
        return "CountIf";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = GetArray_1(engine, tempParameter);
        if (args1.IsErrorOrNone()) {
            return args1;
        }

        Operand args2 = func2.Evaluate(engine, tempParameter);
        if (args2.IsErrorOrNone()) {
            return args2;
        }

        List<BigDecimal> list = new ArrayList<>();
        boolean o = FunctionUtil.FlattenToList_Operand_BigDecimal(args1, list);
        if (o == false) {
            return ParameterError(1);
        }
        int count;
        if (args2.IsNumber()) {
            count = FunctionUtil.GetCountIf(list, args2.NumberValue());
        } else {
            String text = args2.TextValue().trim();
            try {
                BigDecimal d = new BigDecimal(text);
                count = FunctionUtil.GetCountIf(list, d);
            } catch (NumberFormatException e) {
                Tuple<String, BigDecimal> m2 = FunctionUtil.ParseSumIfMatch(text);
                if (m2 != null) {
                    count = FunctionUtil.GetCountIf(list, m2.getItem1(), m2.getItem2());
                } else {
                    return ParameterError(2);
                }
            }
        }
        return Operand.Create(count);
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
