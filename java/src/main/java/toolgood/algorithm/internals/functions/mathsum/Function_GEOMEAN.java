package toolgood.algorithm.internals.functions.mathsum;

import java.util.ArrayList;
import java.util.List;
import toolgood.algorithm.Operand;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.FunctionUtil;
import toolgood.algorithm.internals.functions.Function_N;
import toolgood.algorithm.AlgorithmEngine;

public class Function_GEOMEAN extends Function_N {
    public Function_GEOMEAN(FunctionBase[] funcs) {
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
            return Operand.Error("Function '{0}' parameter is error!", "GeoMean");
        }
        if (list.isEmpty()) {
            return Operand.Error("Function '{0}' parameter is error!", "GeoMean");
        }
        double product = 1.0;
        for (double num : list) {
            if (num <= 0) {
                return Operand.Error("Function '{0}' parameter is error!", "GeoMean");
            }
            product *= num;
        }
        double geoMean = Math.pow(product, 1.0 / list.size());
        return Operand.Create(geoMean);
    }

    @Override
    public void toString(java.lang.StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "GeoMean");
    }
}
