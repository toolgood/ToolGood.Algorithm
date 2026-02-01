package toolgood.algorithm.internals.functions.string;

import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.Operand;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.internals.functions.Function_1;

public class Function_CLEAN extends Function_1 {
    public Function_CLEAN(FunctionBase func1) {
        super(func1);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(work, tempParameter);
        if (args1.IsNotText()) {
            args1 = args1.ToText("Function '{0}' parameter is error!", "Clean");
            if (args1.IsError()) {
                return args1;
            }
        }
        String t = args1.TextValue();
        boolean needClean = false;
        for (int i = 0; i < t.length(); i++) {
            char c = t.charAt(i);
            if (c == '\f' || c == '\n' || c == '\r' || c == '\t' || c == '\u000b') {
                needClean = true;
                break;
            }
        }
        if (!needClean) {
            return args1; // no change
        }
        StringBuilder sb = new StringBuilder(t.length());
        for (int i = 0; i < t.length(); i++) {
            char c = t.charAt(i);
            if (c != '\f' && c != '\n' && c != '\r' && c != '\t' && c != '\u000b') {
                sb.append(c);
            }
        }
        return Operand.Create(sb.toString());
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "Clean");
    }
}
