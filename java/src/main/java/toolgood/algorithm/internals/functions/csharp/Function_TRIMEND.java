package toolgood.algorithm.internals.functions.csharp;

import toolgood.algorithm.internals.functions.Function_2;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.Operand;
import toolgood.algorithm.AlgorithmEngine;

import java.util.function.Function;

public class Function_TRIMEND extends Function_2 {
    public Function_TRIMEND(FunctionBase func1, FunctionBase func2) {
        super(func1, func2);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(work, tempParameter);
        if (args1.IsNotText()) {
            args1 = args1.ToText("Function '{0}' parameter {1} is error!", "TrimEnd", 1);
            if (args1.IsError()) {
                return args1;
            }
        }
        if (func2 == null) {
            return Operand.Create(trimEnd(args1.TextValue()));
        }
        Operand args2 = func2.Evaluate(work, tempParameter);
        if (args2.IsNotText()) {
            args2 = args2.ToText("Function '{0}' parameter {1} is error!", "TrimEnd", 2);
            if (args2.IsError()) {
                return args2;
            }
        }
        char[] trimChars = args2.TextValue().toCharArray();
        return Operand.Create(trimEnd(args1.TextValue(), trimChars));
    }

    private String trimEnd(String str) {
        if (str == null || str.isEmpty()) {
            return str;
        }
        int end = str.length();
        while (end > 0 && Character.isWhitespace(str.charAt(end - 1))) {
            end--;
        }
        return str.substring(0, end);
    }

    private String trimEnd(String str, char[] trimChars) {
        if (str == null || str.isEmpty()) {
            return str;
        }
        int end = str.length();
        while (end > 0) {
            boolean found = false;
            char c = str.charAt(end - 1);
            for (char trimChar : trimChars) {
                if (c == trimChar) {
                    found = true;
                    break;
                }
            }
            if (!found) {
                break;
            }
            end--;
        }
        return str.substring(0, end);
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "TrimEnd");
    }
}
