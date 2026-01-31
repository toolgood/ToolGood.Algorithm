package toolgood.algorithm.internals.functions.mathbase;

import toolgood.algorithm.Internals.Operand;
import toolgood.algorithm.Internals.AlgorithmEngine;
import toolgood.algorithm.internals.FunctionBase;
import java.util.ArrayList;
import java.util.List;

public class Function_MULTINOMIAL extends Function_N {
    public Function_MULTINOMIAL(FunctionBase[] funcs) {
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
            return Operand.Error("Function '{0}' parameter is error!", "Multinomial");
        }

        int sum = 0;
        int n = 1;
        for (int i = 0; i < list.size(); i++) {
            int a = (int) list.get(i).doubleValue(); // 小于等于0 时，按0处理
            n *= FunctionUtil.F_base_Factorial(a);
            sum += a;
        }

        double r = FunctionUtil.F_base_Factorial(sum) / n;
        return Operand.Create(r);
    }

    @Override
    public void toString(java.lang.StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "Multinomial");
    }
}
