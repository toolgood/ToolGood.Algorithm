package toolgood.algorithm.internals.functions.mathsum;

import java.util.ArrayList;
import java.util.List;


import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_3;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.functions.FunctionUtil;
import toolgood.algorithm.internals.functions.FunctionUtil.Pair;

public class Function_SUMIF extends Function_3 {
    public Function_SUMIF(FunctionBase func1, FunctionBase func2, FunctionBase func3) {
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
            return Operand.Error("Function '{0}' parameter {1} is error!", "SumIf", 1);
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
                return Operand.Error("Function '{0}' parameter {1} is error!", "SumIf", 3);
            }
        } else {
            sumdbs = list;
        }

        double sum = 0;
        if (args2.IsNumber()) {
            // 处理数字条件
            double value = args2.DoubleValue();
            long count = FunctionUtil.F_base_countif(list, value);
            sum = count * value;
        } else {
            // 处理文本条件
            String textValue = args2.TextValue().trim();
            try {
                // 尝试解析为数字
                double d = Double.parseDouble(textValue);
                sum = FunctionUtil.F_base_sumif(list, d, sumdbs);
            } catch (NumberFormatException e) {
                // 处理通配符条件
                String sunif = textValue;
                Pair<String,Double> matchResult = FunctionUtil.sumifMatch(sunif);
                if (matchResult != null) {
                    sum = FunctionUtil.F_base_sumif(list, matchResult.getFirst(), matchResult.getSecond(), sumdbs);
                } else {
                    return Operand.Error("Function '{0}' parameter {1} is error!", "SumIf", 2);
                }
            }
        }
        return Operand.Create(sum);
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "SumIf");
    }
}
