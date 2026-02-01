package toolgood.algorithm.internals.functions.mathsum;

import toolgood.algorithm.internals.functions.Function_3;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.FunctionUtil;
import toolgood.algorithm.Operand;
import toolgood.algorithm.AlgorithmEngine;

import java.util.ArrayList;
import java.util.List;

public class Function_AVERAGEIF extends Function_3 {
    public Function_AVERAGEIF(FunctionBase func1, FunctionBase func2, FunctionBase func3) {
        super(func1, func2, func3);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(work, tempParameter);
        if (args1.IsError()) {
            return args1;
        }
        Operand args2 = func2.Evaluate(work, tempParameter);
        if (args2.IsError()) {
            return args2;
        }

        List<Double> list = new ArrayList<>();
        boolean o = FunctionUtil.F_base_GetList(args1, list);
        if (!o) {
            return Operand.Error("Function '{0}' parameter {1} is error!", "AverageIf", 1);
        }

        List<Double> sumdbs;
        if (func3 != null) {
            Operand args3 = func3.Evaluate(work, tempParameter);
            if (args3.IsError()) {
                return args3;
            }
            sumdbs = new ArrayList<>();
            boolean o2 = FunctionUtil.F_base_GetList(args3, sumdbs);
            if (!o2) {
                return Operand.Error("Function '{0}' parameter {1} is error!", "AverageIf", 3);
            }
        } else {
            sumdbs = list;
        }

        double sum;
        int count;
        if (args2.IsNumber()) {
            count = FunctionUtil.F_base_countif(list, args2.NumberValue());
            sum = count * args2.NumberValue();
        } else {
            try {
                double d = Double.parseDouble(args2.TextValue().trim());
                count = FunctionUtil.F_base_countif(list, d);
                sum = FunctionUtil.F_base_sumif(list, d, sumdbs);
            } catch (NumberFormatException e) {
                String sunif = args2.TextValue().trim();
                Object[] m2 = FunctionUtil.sumifMatch(sunif);
                if (m2 != null) {
                    String operator = (String) m2[0];
                    double value = (double) m2[1];
                    count = FunctionUtil.F_base_countif(list, operator, value);
                    sum = FunctionUtil.F_base_sumif(list, operator, value, sumdbs);
                } else {
                    return Operand.Error("Function '{0}' parameter {1} is error!", "AverageIf", 2);
                }
            }
        }
        if (count == 0) {
            return Operand.Error("Function '{0}' div 0 error!", "AverageIf");
        }
        return Operand.Create(sum / count);
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "AverageIf");
    }
}
