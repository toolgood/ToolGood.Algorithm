package toolgood.algorithm.internals.functions.mathsum;

import toolgood.algorithm.internals.functions.Function_2;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.FunctionUtil;
import toolgood.algorithm.Operand;
import toolgood.algorithm.AlgorithmEngine;

import java.util.ArrayList;
import java.util.List;

public class Function_COUNTIF extends Function_2 {
    public Function_COUNTIF(FunctionBase func1, FunctionBase func2) {
        super(func1, func2);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, java.util.function.Function<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(work, tempParameter);
        if (!args1.IsArray()) {
            args1 = args1.ToArray("Function '{0}' parameter {1} is error!", "CountIf", 1);
            if (args1.IsError()) {
                return args1;
            }
        }
        Operand args2 = func2.Evaluate(work, tempParameter);
        if (args2.IsError()) {
            return args2;
        }

        List<Double> list = new ArrayList<>();
        boolean o = FunctionUtil.F_base_GetList(args1, list);
        if (!o) {
            return Operand.Error("Function '{0}' parameter {1} is error!", "CountIf", 1);
        }

        int count;
        if (args2.IsNumber()) {
            count = FunctionUtil.F_base_countif(list, args2.NumberValue());
        } else {
            try {
                double d = Double.parseDouble(args2.TextValue().trim());
                count = FunctionUtil.F_base_countif(list, d);
            } catch (NumberFormatException e) {
                String sunif = args2.TextValue().trim();
                Object[] m2 = FunctionUtil.sumifMatch(sunif);
                if (m2 != null) {
                    String operator = (String) m2[0];
                    double value = (double) m2[1];
                    count = FunctionUtil.F_base_countif(list, operator, value);
                } else {
                    return Operand.Error("Function '{0}' parameter {1} is error!", "CountIf", 2);
                }
            }
        }
        return Operand.Create(count);
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        addFunction(stringBuilder, "CountIf");
    }
}
