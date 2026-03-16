package toolgood.algorithm.internals.functions.mathsum;

import java.util.List;

import java.util.stream.Collectors;

import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_2;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.functions.FunctionUtil;

public class Function_SMALL extends Function_2 {
    public Function_SMALL(FunctionBase func1, FunctionBase func2) {
        super(func1, func2);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(work, tempParameter);
        if (args1.IsNotArray()) {
            args1 = args1.ToArray("Function '{0}' parameter {1} is error!", "Small", 1);
            if (args1.IsError()) {
                return args1;
            }
        }
        Operand args2 = func2.Evaluate(work, tempParameter);
        if (args2.IsNotNumber()) {
            args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "Small", 2);
            if (args2.IsError()) {
                return args2;
            }
        }

        List<Double> list = new java.util.ArrayList<>();
        boolean o = FunctionUtil.F_base_GetList(args1, list);
        if (!o) {
            return Operand.Error("Function '{0}' parameter {1} is error!", "Small", 1);
        }

        list = list.stream().sorted().collect(Collectors.toList());
        int k = args2.IntValue();
        int excelIndex = 0; // 假设默认Excel索引�?
        try {
            // 尝试获取ExcelIndex属�?
            java.lang.reflect.Field field = work.getClass().getField("ExcelIndex");
            excelIndex = field.getInt(work);
        } catch (Exception e) {
            // 如果获取失败，使用默认�?
        }

        if (k < 1 - excelIndex || k > list.size() - excelIndex) {
            return Operand.Error("Function '{0}' parameter {1} is error!", "Small", 2);
        }
        return Operand.Create((double)list.get(k - excelIndex));
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "Small");
    }
}
