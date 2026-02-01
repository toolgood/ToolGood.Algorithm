package toolgood.algorithm.internals.functions.string;

import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.Operand;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.internals.functions.Function_1;

public class Function_JIS extends Function_1 {
    public Function_JIS(FunctionBase func1) {
        super(func1);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, java.util.function.BiBiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(work, tempParameter);
        if (args1.IsNotText()) {
            args1 = args1.ToText("Function '{0}' parameter is error!", "JIS");
            if (args1.IsError()) {
                return args1;
            }
        }
        return Operand.Create(F_base_ToSBC(args1.TextValue()));
    }

    private static String F_base_ToSBC(String input) {
        boolean needModify = false;
        for (int i = 0; i < input.length(); i++) {
            char c = input.charAt(i);
            if (c == ' ' || c < 127) {
                needModify = true;
                break;
            }
        }
        if (!needModify) {
            return input;
        }
        char[] chars = input.toCharArray();
        for (int i = 0; i < chars.length; i++) {
            char c = chars[i];
            if (c == ' ') {
                chars[i] = (char)12288;
            } else if (c > 0 && c < 127) {
                chars[i] = (char)(c + 65248);
            }
        }
        return new String(chars);
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "JIS");
    }
}
