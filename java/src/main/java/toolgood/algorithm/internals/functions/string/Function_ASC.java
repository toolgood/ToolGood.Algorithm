package toolgood.algorithm.internals.functions.string;

import java.util.List;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_1;
import toolgood.algorithm.internals.functions.NoneEngine;

public final class Function_ASC extends Function_1 {
    public Function_ASC(FunctionBase func1) {
        super(func1);
    }

    @Override
    public String Name() {
        return "Asc";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) throws Exception {
        Operand args1 = GetText_1(engine, tempParameter);
        if (args1.IsErrorOrNone()) { return args1; }
        return Operand.Create(F_base_ToDBC(args1.TextValue()));
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
                chars[i] = (char) 32;
            } else if (c > 65280 && c < 65375) {
                chars[i] = (char) (c - 65248);
            }
        }
        return new String(chars);
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.TEXT;
    }

    @Override
    public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, String op, String val) {
        func1.GetParameterTypes(noneEngine, result, OperandType.TEXT);
    }
}
