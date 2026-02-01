package toolgood.algorithm.internals.functions.csharp;

import toolgood.algorithm.internals.functions.Function_N;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.Operand;
import toolgood.algorithm.AlgorithmEngine;

import java.util.ArrayList;
import java.util.List;


public class Function_JOIN extends Function_N {
    public Function_JOIN(FunctionBase[] funcs) {
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
        Operand args1 = args.get(0);
        if (args1.IsJson()) {
            Operand o = args1.ToArray(null);
            if (!o.IsError()) {
                args1 = o;
            }
        }
        if (args1.IsArray()) {
            List<String> list = new ArrayList<>();
            boolean o = toolgood.algorithm.internals.FunctionUtil.F_base_GetList(args1, list);
            if (!o) {
                return Operand.Error("Function '{0}' parameter {1} is error!", "Join", 1);
            }

            Operand args2 = args.get(1).ToText("Function '{0}' parameter {1} is error!", "Join", 2);
            if (args2.IsError()) {
                return args2;
            }

            return Operand.Create(String.join(args2.TextValue(), list));
        } else {
            args1 = args1.ToText("Function '{0}' parameter {1} is error!", "Join", 1);
            if (args1.IsError()) {
                return args1;
            }

            List<String> list = new ArrayList<>();
            for (int i = 1; i < args.size(); i++) {
                boolean o = toolgood.algorithm.internals.FunctionUtil.F_base_GetList(args.get(i), list);
                if (!o) {
                    return Operand.Error("Function '{0}' parameter {1} is error!", "Join", i + 1);
                }
            }
            return Operand.Create(String.join(args1.TextValue(), list));
        }
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "Join");
    }
}
