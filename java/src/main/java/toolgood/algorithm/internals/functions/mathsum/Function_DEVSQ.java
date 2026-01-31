package toolgood.algorithm.internals.functions.mathsum;

import toolgood.algorithm.internals.functions.Function_N;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.FunctionUtil;
import toolgood.algorithm.Operand;
import toolgood.algorithm.AlgorithmEngine;

import java.util.ArrayList;
import java.util.List;

public class Function_DEVSQ extends Function_N {
    public Function_DEVSQ(FunctionBase[] funcs) {
        super(funcs);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, java.util.function.Function<AlgorithmEngine, String, Operand> tempParameter) {
        List<Operand> args = new ArrayList<>();
        for (FunctionBase item : funcs) {
            Operand aa = item.Evaluate(work, tempParameter);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        List<Double> list = new ArrayList<>();
        boolean o = FunctionUtil.F_base_GetList(args, list);
        if (!o) {
            return Operand.Error("Function '{0}' parameter is error!", "DevSQ");
        }
        if (list.size() == 0) {
            return Operand.Error("Function '{0}' parameter is error!", "DevSQ");
        }

        double avg = list.stream().mapToDouble(Double::doubleValue).average().orElse(0.0);
        double sum = 0;
        for (double value : list) {
            double diff = value - avg;
            sum += diff * diff;
        }
        return Operand.Create(sum);
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "DevSQ");
    }
}
