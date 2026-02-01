package toolgood.algorithm.internals.functions.mathsum;

import java.util.ArrayList;
import java.util.Collections;
import java.util.Comparator;
import java.util.List;
import toolgood.algorithm.Operand;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_2;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.internals.FunctionUtil;

public class Function_LARGE extends Function_2 {
    public Function_LARGE(FunctionBase func1, FunctionBase func2) {
        super(func1, func2);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(work, tempParameter);
        if (args1.IsNotArray()) {
            args1 = args1.ToArray("Function '{0}' parameter {1} is error!", "Large", 1);
            if (args1.IsError()) {
                return args1;
            }
        }
        Operand args2 = func2.Evaluate(work, tempParameter);
        if (args2.IsNotNumber()) {
            args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "Large", 2);
            if (args2.IsError()) {
                return args2;
            }
        }
        List<Double> list = new ArrayList<>();
        boolean o = FunctionUtil.F_base_GetList(args1, list);
        if (!o) {
            return Operand.Error("Function '{0}' parameter {1} is error!", "Large", 1);
        }

        Collections.sort(list, Comparator.reverseOrder());
        int k = args2.IntValue();
        if (k < 1 - work.ExcelIndex() || k > list.size() - work.ExcelIndex()) {
            return Operand.Error("Function '{0}' parameter {1} is error!", "Large", 2);
        }
        return Operand.Create(list.get(k - work.ExcelIndex()));
    }

    @Override
    public void toString(java.lang.StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "Large");
    }
}
