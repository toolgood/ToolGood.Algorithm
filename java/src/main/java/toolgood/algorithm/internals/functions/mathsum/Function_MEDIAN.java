package toolgood.algorithm.internals.functions.mathsum;

import java.util.ArrayList;
import java.util.Collections;
import java.util.List;
import toolgood.algorithm.Operand;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_N;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.internals.functions.FunctionUtil;

public class Function_MEDIAN extends Function_N {
    public Function_MEDIAN(FunctionBase[] funcs) {
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

        List<Double> list = new ArrayList<>();
        boolean o = FunctionUtil.F_base_GetList(args, list);

        if (!o) {
            return Operand.Error("Function '{0}' parameter is error!", "Median");
        }
        if (list.isEmpty()) {
            return Operand.Error("Function '{0}' parameter is error!", "Median");
        }

        Collections.sort(list);
        return Operand.Create((double)list.get(list.size() / 2));
    }

    @Override
    public void toString(java.lang.StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "Median");
    }
}
