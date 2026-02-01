package toolgood.algorithm.internals.functions.mathsum;

import java.util.ArrayList;
import java.util.Collections;
import java.util.Comparator;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import toolgood.algorithm.Operand;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_N;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.internals.FunctionUtil;

public class Function_MODE extends Function_N {
    public Function_MODE(FunctionBase[] funcs) {
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
            return Operand.Error("Function '{0}' parameter is error!", "Mode");
        }

        Map<Double, Integer> dict = new HashMap<>();
        for (double item : list) {
            dict.put(item, dict.getOrDefault(item, 0) + 1);
        }

        Map.Entry<Double, Integer> maxEntry = Collections.max(dict.entrySet(), Comparator.comparingInt(Map.Entry::getValue));
        return Operand.Create(maxEntry.getKey());
    }

    @Override
    public void toString(java.lang.StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "Mode");
    }
}
