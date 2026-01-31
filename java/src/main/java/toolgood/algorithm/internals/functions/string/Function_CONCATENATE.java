package toolgood.algorithm.internals.functions.string;

import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.Operand;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.internals.functions.Function_N;

public class Function_CONCATENATE extends Function_N {
    public Function_CONCATENATE(FunctionBase[] funcs) {
        super(funcs);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        if (funcs.length == 0) {
            return Operand.Create("");
        }
        if (funcs.length == 1) {
            Operand a = funcs[0].Evaluate(work, tempParameter);
            if (a.IsNotText()) {
                a = a.ToText("Function '{0}' parameter {1} is error!", "Concatenate", 1);
                if (a.IsError()) {
                    return a;
                }
            }
            return a; // 只有一个
        }
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < funcs.length; i++) {
            Operand a = funcs[i].Evaluate(work, tempParameter);
            if (a.IsNotText()) {
                a = a.ToText("Function '{0}' parameter {1} is error!", "Concatenate", i + 1);
                if (a.IsError()) {
                    return a;
                }
            }
            sb.append(a.getTextValue());
        }
        return Operand.Create(sb.toString());
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "Concatenate");
    }
}
