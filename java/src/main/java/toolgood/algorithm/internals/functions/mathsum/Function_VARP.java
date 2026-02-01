package toolgood.algorithm.internals.functions.mathsum;

import java.util.ArrayList;
import java.util.List;


import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_N;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.internals.functions.FunctionUtil;

public class Function_VARP extends Function_N {
    public Function_VARP(FunctionBase[] funcs) {
        super(funcs);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        List<Operand> args = new ArrayList<>();
        for (FunctionBase item : funcs) {
            Operand aa = item.Evaluate(work, tempParameter);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        if (args.size() == 1) {
            return Operand.Error("Function '{0}' parameter only one error!", "VarP");
        }

        List<Double> list = new ArrayList<>();
        boolean o = FunctionUtil.F_base_GetList(args, list);
        if (!o) {
            return Operand.Error("Function '{0}' parameter is error!", "VarP");
        }
        if (list.size() == 0) {
            return Operand.Error("Function '{0}' parameter is error!", "VarP");
        }
        if (list.size() == 1) {
            return Operand.Zero();
        }

        double avg = list.stream().mapToDouble(Double::doubleValue).average().orElse(0.0);
        double sum = 0;
        for (int i = 0; i < list.size(); i++) {
            sum += Math.pow(avg - list.get(i), 2);
        }

        return Operand.Create(sum / list.size());
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "VarP");
    }
}
