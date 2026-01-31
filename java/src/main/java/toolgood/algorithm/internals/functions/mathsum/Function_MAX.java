package toolgood.algorithm.internals.functions.mathsum;

import java.util.ArrayList;
import java.util.Collections;
import java.util.List;
import toolgood.algorithm.internals.Operand;
import toolgood.algorithm.internals.FunctionBase;
import toolgood.algorithm.internals.Function_N;
import toolgood.algorithm.internals.AlgorithmEngine;
import toolgood.algorithm.internals.FunctionUtil;

public class Function_MAX extends Function_N {
    public Function_MAX(FunctionBase[] funcs) {
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
            return Operand.Error("Function '{0}' parameter is error!", "Max");
        }

        return Operand.Create(Collections.max(list));
    }

    @Override
    public void toString(java.lang.StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "Max");
    }
}
