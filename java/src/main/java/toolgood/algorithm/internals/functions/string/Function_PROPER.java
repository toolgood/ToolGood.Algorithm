package toolgood.algorithm.internals.functions.string;

import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.Operand;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.internals.functions.Function_1;

public class Function_PROPER extends Function_1 {
    public Function_PROPER(FunctionBase func1) {
        super(func1);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, java.util.function.BiBiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(work, tempParameter);
        if (args1.IsNotText()) {
            args1 = args1.ToText("Function '{0}' parameter is error!", "Proper");
            if (args1.IsError()) {
                return args1;
            }
        }

        String text = args1.TextValue();
        if (text == null || text.isEmpty()) {
            return Operand.Create(text);
        }
        boolean needModify = false;
        boolean isFirst = true;
        for (int i = 0; i < text.length(); i++) {
            char t = text.charAt(i);
            if (t == ' ' || t == '\r' || t == '\n' || t == '\t' || t == '.') {
                isFirst = true;
            } else if (isFirst) {
                if (Character.isLowerCase(t)) {
                    needModify = true;
                    break;
                }
                isFirst = false;
            }
        }
        if (!needModify) {
            return args1; // no change
        }
        char[] chars = text.toCharArray();
        isFirst = true;
        for (int i = 0; i < chars.length; i++) {
            char t = chars[i];
            if (t == ' ' || t == '\r' || t == '\n' || t == '\t' || t == '.') {
                isFirst = true;
            } else if (isFirst) {
                chars[i] = Character.toUpperCase(t);
                isFirst = false;
            }
        }
        return Operand.Create(new String(chars));
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "Proper");
    }
}
