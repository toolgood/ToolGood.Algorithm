package toolgood.algorithm.internals.functions.csharp;

import java.util.List;
import java.util.function.BiFunction;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_2;
import toolgood.algorithm.internals.functions.NoneEngine;

final class Function_TRIMSTART extends Function_2 {

    Function_TRIMSTART(FunctionBase[] funcs) {
        super(funcs);
        if (funcs.length < 1 || funcs.length > 2) {
            throw new IllegalArgumentException("Function 'TrimStart' requires 1 to 2 parameters.");
        }
    }

    @Override
    public String Name() {
        return "TrimStart";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = GetText_1(engine, tempParameter);
        if (args1.IsErrorOrNone()) { return args1; }

        if (func2 == null) {
            return Operand.Create(args1.TextValue().stripLeading());
        }

        Operand args2 = GetText_2(engine, tempParameter);
        if (args2.IsErrorOrNone()) { return args2; }

        char[] trimChars = args2.TextValue().toCharArray();
        String text = args1.TextValue();
        int start = 0;
        while (start < text.length()) {
            char c = text.charAt(start);
            boolean found = false;
            for (char tc : trimChars) {
                if (c == tc) {
                    found = true;
                    break;
                }
            }
            if (!found) { break; }
            start++;
        }
        return Operand.Create(text.substring(start));
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.TEXT;
    }

    @Override
    public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, String op, String val) {
        func1.GetParameterTypes(noneEngine, result, OperandType.TEXT);
        if (func2 != null) {
            func2.GetParameterTypes(noneEngine, result, OperandType.TEXT);
        }
    }
}
