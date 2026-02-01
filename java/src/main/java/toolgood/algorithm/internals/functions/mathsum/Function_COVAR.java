package toolgood.algorithm.internals.functions.mathsum;

import toolgood.algorithm.internals.functions.Function_2;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.FunctionUtil;
import toolgood.algorithm.Operand;
import toolgood.algorithm.AlgorithmEngine;

import java.util.ArrayList;
import java.util.List;

public class Function_COVAR extends Function_2 {
    public Function_COVAR(FunctionBase func1, FunctionBase func2) {
        super(func1, func2);
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

        List<Double> list1 = new ArrayList<>();
        List<Double> list2 = new ArrayList<>();
        boolean o1 = FunctionUtil.F_base_GetList(args1, list1);
        boolean o2 = FunctionUtil.F_base_GetList(args2, list2);
        if (!o1) {
            return Operand.Error("Function '{0}' parameter {1} is error!", "Covar", 1);
        }
        if (!o2) {
            return Operand.Error("Function '{0}' parameter {1} is error!", "Covar", 2);
        }
        if (list1.size() != list2.size()) {
            return Operand.Error("Function '{0}' parameter's count error!", "Covar");
        }
        if (list1.size() == 0) {
            return Operand.Error("Function '{0}' parameter's count error!", "Covar");
        }

        double avg1 = list1.stream().mapToDouble(Double::doubleValue).average().orElse(0.0);
        double avg2 = list2.stream().mapToDouble(Double::doubleValue).average().orElse(0.0);
        double sum = 0;
        for (int i = 0; i < list1.size(); i++) {
            sum += (list1.get(i) - avg1) * (list2.get(i) - avg2);
        }
        double val = sum / list1.size();
        return Operand.Create(val);
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "Covar");
    }
}
