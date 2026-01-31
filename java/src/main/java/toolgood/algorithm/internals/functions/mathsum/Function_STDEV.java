package toolgood.algorithm.internals.functions.mathsum;

import java.util.ArrayList;
import java.util.List;
import java.util.function.Function;

import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.Function_N;
import toolgood.algorithm.Operand;
import toolgood.algorithm.internals.functions.FunctionUtil;

public class Function_STDEV extends Function_N {
    public Function_STDEV(FunctionBase[] funcs) {
        super(funcs);
    }

    @Override
    public Operand Evaluate(Object work, Function<Object, String, Operand> tempParameter) {
        List<Operand> args = new ArrayList<>();
        for (FunctionBase item : funcs) {
            Operand aa = item.Evaluate(work, tempParameter);
            if (aa.isError()) {
                return aa;
            }
            args.add(aa);
        }

        List<Double> list = new ArrayList<>();
        boolean o = FunctionUtil.F_base_GetList(args, list);
        if (!o) {
            return Operand.Error("Function '{0}' parameter is error!", "Stdev");
        }
        if (list.size() == 0) {
            return Operand.Error("Function '{0}' parameter is error!", "Stdev");
        }

        double avg = list.stream().mapToDouble(Double::doubleValue).average().orElse(0.0);
        double sum = 0;
        for (int i = 0; i < list.size(); i++) {
            sum += Math.pow(list.get(i) - avg, 2);
        }
        return Operand.Create(Math.sqrt(sum / (list.size() - 1)));
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        addFunction(stringBuilder, "Stdev");
    }
}
