package toolgood.algorithm.internals.functions.string;

import toolgood.algorithm.internals.FunctionBase;
import toolgood.algorithm.internals.Operand;
import toolgood.algorithm.internals.AlgorithmEngine;
import toolgood.algorithm.internals.functions.Function_1;

public class Function_ASC extends Function_1 {
    public Function_ASC(FunctionBase func1) {
        super(func1);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(work, tempParameter);
        if (args1.IsNotText()) {
            args1 = args1.ToText("Function '{0}' parameter is error!", "ASC");
            if (args1.IsError()) {
                return args1;
            }
        }
        return Operand.Create(F_base_ToDBC(args1.getTextValue()));
    }

    private static String F_base_ToDBC(String input) {
        boolean needModify = false;
        for (int i = 0; i < input.length(); i++) {
            char c = input.charAt(i);
            if (c == 12288 || (c > 65280 && c < 65375)) {
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
            if (c == 12288) {
                chars[i] = (char)32;
            } else if (c > 65280 && c < 65375) {
                chars[i] = (char)(c - 65248);
            }
        }
        return new String(chars);
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "ASC");
    }
}
