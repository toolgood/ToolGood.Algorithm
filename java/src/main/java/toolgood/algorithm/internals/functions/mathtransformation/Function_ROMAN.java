package toolgood.algorithm.internals.functions.mathtransformation;

import java.util.List;
import java.util.function.BiFunction;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_2;
import toolgood.algorithm.internals.functions.NoneEngine;

public final class Function_ROMAN extends Function_2 {
    private static final int[] VALUES = { 1000, 900, 500, 400, 100, 90, 50, 40, 10, 9, 5, 4, 1 };
    private static final String[] NUMERALS = { "M", "CM", "D", "CD", "C", "XC", "L", "XL", "X", "IX", "V", "IV", "I" };

    public Function_ROMAN(FunctionBase[] funcs) {
        super(funcs);
    }

    @Override
    public String Name() {
        return "ROMAN";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) throws Exception {
        Operand numArg = GetNumber_1(engine, tempParameter);
        if (numArg.IsErrorOrNone()) {
            return numArg;
        }
        int num = numArg.IntValue();

        if (num < 0 || num > 3999) {
            return Operand.Create("");
        }

        int form = 0;
        if (func2 != null) {
            Operand formArg = GetNumber_2(engine, tempParameter);
            if (formArg.IsErrorOrNone()) {
                return formArg;
            }
            form = formArg.IntValue();
        }

        return Operand.Create(arabicToRoman(num, form));
    }

    private String arabicToRoman(int num, int form) {
        if (num == 0) {
            return "";
        }

        StringBuilder sb = new StringBuilder(16);
        for (int i = 0; i < VALUES.length; i++) {
            while (num >= VALUES[i]) {
                sb.append(NUMERALS[i]);
                num -= VALUES[i];
            }
        }

        return sb.toString();
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.TEXT;
    }

    @Override
    public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType,
            String op, String val) {
        func1.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
        if (func2 != null) {
            func2.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
        }
    }
}
