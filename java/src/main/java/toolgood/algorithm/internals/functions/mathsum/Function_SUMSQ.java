package toolgood.algorithm.internals.functions.mathsum;

import java.util.ArrayList;
import java.util.List;


import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_N;
import toolgood.algorithm.Operand;
import toolgood.algorithm.internals.functions.FunctionUtil;

public class Function_SUMSQ extends Function_N {
    public Function_SUMSQ(FunctionBase[] funcs) {
        super(funcs);
    }

    @Override
    public Operand Evaluate(Object work, Function<Object, String, Operand> tempParameter) {
        List<Operand> args = new ArrayList<>(funcs.length);
        for (int i = 0; i < funcs.length; i++) {
            Operand aa = funcs[i].Evaluate(work, tempParameter);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        List<Double> list = new ArrayList<>();
        boolean o = FunctionUtil.F_base_GetList(args, list);
        if (!o) {
            return Operand.Error("Function '{0}' parameter is error!", "SumSQ");
        }

        double d = 0;
        for (int i = 0; i < list.size(); i++) {
            double a = list.get(i);
            d += a * a;
        }
        return Operand.Create(d);
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "SumSQ");
    }
}
