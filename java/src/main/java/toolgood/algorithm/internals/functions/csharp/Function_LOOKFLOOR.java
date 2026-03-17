package toolgood.algorithm.internals.functions.csharp;

import java.math.BigDecimal;
import java.util.ArrayList;
import java.util.Collections;
import java.util.List;
import java.util.function.BiFunction;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.Function_2;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.FunctionUtil;
import toolgood.algorithm.internals.functions.NoneEngine;

public final class Function_LOOKFLOOR extends Function_2 {
    public Function_LOOKFLOOR(FunctionBase[] funcs) {
        super(funcs);
    }

    @Override
    public String Name() {
        return "LookFloor";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = GetNumber_1(engine, tempParameter);
        if (args1.IsErrorOrNone()) { return args1; }

        Operand args2 = GetArray_2(engine, tempParameter);
        if (args2.IsErrorOrNone()) { return args2; }

        List<BigDecimal> list = new ArrayList<>();
        FunctionUtil.FlattenToList_Operand_BigDecimal(args2, list);
        if (list.isEmpty()) {
            return ParameterError(2);
        }
        Collections.sort(list);
        BigDecimal value = args1.NumberValue();
        BigDecimal result = list.get(0);
        if (result.compareTo(value) == 0) {
            return args1;
        }
        for (int i = 1; i < list.size(); i++) {
            BigDecimal val = list.get(i);
            if (val.compareTo(value) < 0) {
                result = val;
            } else if (val.compareTo(value) == 0) {
                return args1;
            } else {
                break;
            }
        }
        return Operand.Create(result);
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.NUMBER;
    }

    @Override
    public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, String op, String val) {
        func1.GetParameterTypes(noneEngine, result, OperandType.NUMBER, null, null);
        func2.GetParameterTypes(noneEngine, result, OperandType.ARRAY, null, null);
    }
}
