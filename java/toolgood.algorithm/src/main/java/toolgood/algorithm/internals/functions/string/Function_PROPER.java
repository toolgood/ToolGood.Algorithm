package toolgood.algorithm.internals.functions.string;

import java.util.List;
import java.util.function.BiFunction;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_1;
import toolgood.algorithm.internals.functions.NoneEngine;

final class Function_PROPER extends Function_1 {

    public Function_PROPER(FunctionBase[] funcs) {
        super(funcs);
        if (funcs.length != 1) {
            throw new IllegalArgumentException("Function '" + Name() + "' requires exactly 1 parameter.");
        }
    }

    @Override
    public String Name() {
        return "Proper";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = GetText_1(engine, tempParameter);
        if (args1.IsErrorOrNone()) { return args1; }

        String text = args1.TextValue();
        if (text == null || text.isEmpty()) {
            return Operand.Create(text);
        }

        boolean needModify = false;
        boolean isFirst = true;
        for (int i = 0; i < text.length(); i++) {
            char t = text.charAt(i);
            if (!Character.isLetter(t)) {
                isFirst = true;
            } else {
                if (isFirst) {
                    if (Character.isLowerCase(t)) {
                        needModify = true;
                    }
                } else {
                    if (Character.isUpperCase(t)) {
                        needModify = true;
                    }
                }
                if (needModify) break;
                isFirst = false;
            }
        }

        if (!needModify) {
            return args1;
        }

        char[] chars = text.toCharArray();
        isFirst = true;
        for (int i = 0; i < chars.length; i++) {
            char t = chars[i];
            if (!Character.isLetter(t)) {
                isFirst = true;
            } else {
                if (isFirst) {
                    chars[i] = Character.toUpperCase(t);
                } else {
                    chars[i] = Character.toLowerCase(t);
                }
                isFirst = false;
            }
        }
        return Operand.Create(new String(chars));
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
