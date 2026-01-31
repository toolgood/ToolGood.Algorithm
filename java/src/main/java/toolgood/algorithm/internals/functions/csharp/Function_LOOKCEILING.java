package toolgood.algorithm.internals.functions.csharp;

import toolgood.algorithm.internals.Function_2;
import toolgood.algorithm.internals.FunctionBase;
import toolgood.algorithm.internals.Operand;
import toolgood.algorithm.AlgorithmEngine;

import java.math.BigDecimal;
import java.util.ArrayList;
import java.util.Collections;
import java.util.Comparator;
import java.util.List;
import java.util.function.Function;

public class Function_LOOKCEILING extends Function_2 {
    public Function_LOOKCEILING(FunctionBase func1, FunctionBase func2) {
        super(func1, func2);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, Function<String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(work, tempParameter);
        if (args1.IsNotNumber()) {
            args1 = args1.ToNumber("Function '{0}' parameter {1} is error!", "LookCeiling", 1);
            if (args1.IsError()) {
                return args1;
            }
        }
        Operand args2 = func2.Evaluate(work, tempParameter);
        if (args2.IsNotArray()) {
            args2 = args2.ToArray("Function '{0}' parameter {1} is error!", "LookCeiling", 2);
            if (args2.IsError()) {
                return args2;
            }
        }

        List<BigDecimal> list = new ArrayList<>();
        toolgood.algorithm.internals.FunctionUtil.F_base_GetList(args2, list);
        if (list.isEmpty()) {
            return Operand.Error("Function '{0}' parameter {1} is error!", "LookCeiling", 2);
        }
        list.sort(Collections.reverseOrder());
        BigDecimal value = args1.getNumberValue();
        BigDecimal result = list.get(0);
        if (result.compareTo(value) == 0) {
            return args1;
        }
        for (int i = 1; i < list.size(); i++) {
            BigDecimal val = list.get(i);
            if (val.compareTo(value) > 0) {
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
    public void ToString(StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "LookCeiling");
    }
}
