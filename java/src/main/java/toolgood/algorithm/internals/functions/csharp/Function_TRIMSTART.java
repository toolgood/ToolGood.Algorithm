package toolgood.algorithm.internals.functions.csharp;

import java.util.List;
import java.util.function.BiFunction;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.Function_2;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.NoneEngine;

public final class Function_TRIMSTART extends Function_2 {
    public Function_TRIMSTART(FunctionBase[] funcs) {
        super(funcs);
    }

    @Override
    public String Name() {
        return "TrimStart";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = GetText_1(engine, tempParameter);
        if (args1.IsError() || args1.IsNone()) {
            return args1;
        }

        if (func2 == null) {
            return Operand.Create(trimStart(args1.TextValue()));
        }

        Operand args2 = GetText_2(engine, tempParameter);
        if (args2.IsError() || args2.IsNone()) {
            return args2;
        }

        char[] trimChars = args2.TextValue().toCharArray();
        return Operand.Create(trimStart(args1.TextValue(), trimChars));
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
    public OperandType GetResultType() {
        return OperandType.TEXT;
    }

    @Override
    public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, String op, String val) {
        func1.GetParameterTypes(noneEngine, result, OperandType.TEXT, null, null);
        if (func2 != null) {
            func2.GetParameterTypes(noneEngine, result, OperandType.TEXT, null, null);
        }
    }
}
