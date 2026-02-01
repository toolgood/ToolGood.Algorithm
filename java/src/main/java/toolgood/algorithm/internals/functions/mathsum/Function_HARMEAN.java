package toolgood.algorithm.internals.functions.mathsum;

import java.util.ArrayList;
import java.util.List;
import toolgood.algorithm.Operand;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.FunctionUtil;
import toolgood.algorithm.internals.functions.Function_N;
import toolgood.algorithm.AlgorithmEngine;

public class Function_HARMEAN extends Function_N {
    public Function_HARMEAN(FunctionBase[] funcs) {
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
            return args.get(0);
        }

        List<Double> list = new ArrayList<>();
        boolean o = FunctionUtil.F_base_GetList(args, list);
        if (!o) {
            return Operand.Error("Function '{0}' parameter is error!", "HarMean");
        }

        double sum = 0;
        for (double db : list) {
            if (db == 0) {
                return Operand.Error("Function '{0}' parameter is error!", "HarMean");
            }
            sum += 1 / db;
        }
        if (sum == 0) {
            return Operand.Error("Function '{0}' parameter is error!", "HarMean");
        }
        return Operand.Create(list.size() / sum);
    }

    @Override
    public void toString(java.lang.StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "HarMean");
    }
}
