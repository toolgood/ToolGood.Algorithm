package toolgood.algorithm.internals.functions.csharp;

import toolgood.algorithm.internals.Function_2;
import toolgood.algorithm.internals.FunctionBase;
import toolgood.algorithm.internals.Operand;
import toolgood.algorithm.AlgorithmEngine;

import java.util.function.Function;

public class Function_TRIMSTART extends Function_2 {
    public Function_TRIMSTART(FunctionBase func1, FunctionBase func2) {
        super(func1, func2);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, Function<String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(work, tempParameter);
        if (args1.IsNotText()) {
            args1 = args1.ToText("Function '{0}' parameter {1} is error!", "TrimStart", 1);
            if (args1.IsError()) {
                return args1;
            }
        }
        if (func2 == null) {
            return Operand.Create(trimStart(args1.getTextValue()));
        }
        Operand args2 = func2.Evaluate(work, tempParameter);
        if (args2.IsNotText()) {
            args2 = args2.ToText("Function '{0}' parameter {1} is error!", "TrimStart", 2);
            if (args2.IsError()) {
                return args2;
            }
        }
        char[] trimChars = args2.getTextValue().toCharArray();
        return Operand.Create(trimStart(args1.getTextValue(), trimChars));
    }

    private String trimStart(String str) {
        if (str == null || str.isEmpty()) {
            return str;
        }
        int start = 0;
        while (start < str.length() && Character.isWhitespace(str.charAt(start))) {
            start++;
        }
        return str.substring(start);
    }

    private String trimStart(String str, char[] trimChars) {
        if (str == null || str.isEmpty()) {
            return str;
        }
        int start = 0;
        while (start < str.length()) {
            boolean found = false;
            char c = str.charAt(start);
            for (char trimChar : trimChars) {
                if (c == trimChar) {
                    found = true;
                    break;
                }
            }
            if (!found) {
                break;
            }
            start++;
        }
        return str.substring(start);
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "TrimStart");
    }
}
