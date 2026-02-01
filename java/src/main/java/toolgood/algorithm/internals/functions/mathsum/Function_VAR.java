package toolgood.algorithm.internals.functions.mathsum;

import java.util.ArrayList;
import java.util.List;


import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_N;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.internals.functions.FunctionUtil;

public class Function_VAR extends Function_N {
    public Function_VAR(FunctionBase[] funcs) {
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
            return Operand.Error("Function '{0}' parameter only one error!", "Var");
        }

        List<Double> list = new ArrayList<>();
        boolean o = FunctionUtil.F_base_GetList(args, list);
        if (!o) {
            return Operand.Error("Function '{0}' parameter is error!", "Var");
        }
        if (list.size() <= 1) {
            return Operand.Error("Function '{0}' parameter is error!", "Var");
        }

        double sum = 0;
        double sum2 = 0;
        for (int i = 0; i < list.size(); i++) {
            sum += list.get(i) * list.get(i);
            sum2 += list.get(i);
        }

        double result = (list.size() * sum - sum2 * sum2) / list.size() / (list.size() - 1);
        return Operand.Create(result);
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "Var");
    }
}
