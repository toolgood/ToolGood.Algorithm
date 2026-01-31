package toolgood.algorithm.internals.functions.mathsum;

import java.util.List;
import java.util.function.Function;

import toolgood.algorithm.internals.FunctionBase;
import toolgood.algorithm.internals.Function_3;
import toolgood.algorithm.internals.Operand;
import toolgood.algorithm.internals.OperandType;
import toolgood.algorithm.internals.functions.FunctionUtil;
import toolgood.algorithm.mathnet.ExcelFunctions;

public class Function_PERCENTRANK extends Function_3 {
    public Function_PERCENTRANK(FunctionBase func1, FunctionBase func2, FunctionBase func3) {
        super(func1, func2, func3);
    }

    @Override
    public Operand Evaluate(Object work, Function<Object, String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(work, tempParameter);
        if (args1.getOperandType() != OperandType.Array) {
            args1 = args1.toArray("Function '{0}' parameter {1} is error!", "PercentRank", 1);
            if (args1.IsError()) {
                return args1;
            }
        }
        Operand args2 = func2.Evaluate(work, tempParameter);
        if (args2.getOperandType() != OperandType.Number) {
            args2 = args2.toNumber("Function '{0}' parameter {1} is error!", "PercentRank", 2);
            if (args2.IsError()) {
                return args2;
            }
        }

        List<Double> list = new java.util.ArrayList<>();
        boolean o = FunctionUtil.F_base_GetList(args1, list);
        if (!o) {
            return Operand.Error("Function '{0}' parameter is error!", "PercentRank");
        }

        double k = args2.getDoubleValue();
        double[] array = list.stream().mapToDouble(Double::doubleValue).toArray();
        double v = ExcelFunctions.PercentRank(array, k);
        int d = 3;
        if (func3 != null) {
            Operand args3 = func3.Evaluate(work, tempParameter);
            if (args3.getOperandType() != OperandType.Number) {
                args3 = args3.toNumber("Function '{0}' parameter {1} is error!", "PercentRank", 3);
                if (args3.IsError()) {
                    return args3;
                }
            }
            d = args3.getIntValue();
        }
        return Operand.Create(Math.round(v * Math.pow(10, d)) / Math.pow(10, d));
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        addFunction(stringBuilder, "PercentRank");
    }
}
