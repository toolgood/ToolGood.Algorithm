package toolgood.algorithm.internals.functions.csharp;

import toolgood.algorithm.Operand;
import toolgood.algorithm.internals.AlgorithmEngine;
import toolgood.algorithm.internals.functions.Function_2;
import toolgood.algorithm.internals.functions.FunctionBase;

public class Function_TRIMEND extends Function_2 {
    public Function_TRIMEND(FunctionBase func1, FunctionBase func2) {
        super(func1, func2);
    }

    @Override
    public Operand evaluate(AlgorithmEngine work, java.util.function.Function<String, Operand> tempParameter) {
        Operand args1 = func1.evaluate(work, tempParameter); if(args1.isNotText()) { args1 = args1.toText("Function '{0}' parameter {1} is error!", "TrimEnd", 1); if(args1.isError()) return args1; }
        if(func2 == null) {
            return Operand.create(args1.getTextValue().trim());
        }
        Operand args2 = func2.evaluate(work, tempParameter); if(args2.isNotText()) { args2 = args2.toText("Function '{0}' parameter {1} is error!", "TrimEnd", 2); if(args2.isError()) return args2; }
        char[] trimChars = args2.getTextValue().toCharArray();
        return Operand.create(trimEnd(args1.getTextValue(), trimChars));
    }

    private String trimEnd(String str, char[] trimChars) {
        if (str == null || str.isEmpty()) {
            return str;
        }
        int length = str.length();
        while (length > 0 && contains(trimChars, str.charAt(length - 1))) {
            length--;
        }
        return str.substring(0, length);
    }

    private boolean contains(char[] array, char c) {
        for (char ch : array) {
            if (ch == c) {
                return true;
            }
        }
        return false;
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        addFunction(stringBuilder, "TrimEnd");
    }
}
