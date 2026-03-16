package toolgood.algorithm.internals.functions.mathbase;

import java.util.List;
import java.util.function.BiFunction;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_2;
import toolgood.algorithm.internals.functions.NoneEngine;

/**
 * ROMAN 函数：将阿拉伯数字转换为罗马数字字符串�?
 * 第二参数 form 保留（兼�?Excel），当前实现仅使用经典形式�?
 * 参数：ROMAN(number [, form])
 */
public class Function_ROMAN extends Function_2 {

    private static final int[]    VALUES   = {1000, 900, 500, 400, 100, 90, 50, 40, 10, 9, 5, 4, 1};
    private static final String[] NUMERALS = {"M", "CM", "D", "CD", "C", "XC", "L", "XL", "X", "IX", "V", "IV", "I"};

    public Function_ROMAN(FunctionBase func1, FunctionBase func2) {
        super(func1, func2);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work,
            BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        if (func1 == null) {
            return ParameterError(1);
        }
        Operand numArg = func1.Evaluate(work, tempParameter);
        if (numArg.IsNotNumber()) {
            numArg = ConvertToNumber(numArg, 1);
            if (numArg.IsError()) {
                return numArg;
            }
        }
        int num = numArg.IntValue();

        if (num < 0 || num > 3999) {
            return Operand.Create("");
        }

        // form 参数（暂不影响输出，保留读取以兼容调用形式）
        if (func2 != null) {
            Operand formArg = func2.Evaluate(work, tempParameter);
            if (formArg.IsNotNumber()) {
                formArg = ConvertToNumber(formArg, 2);
                if (formArg.IsError()) {
                    return formArg;
                }
            }
            // form 值已读取，可扩展为简化罗马数字逻辑；目前经典形式已足够
        }

        return Operand.Create(arabicToRoman(num));
    }

    private String arabicToRoman(int num) {
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
    public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result,
            OperandType operandType, String op, String val) {
        func1.GetParameterTypes(noneEngine, result, OperandType.NUMBER, op, val);
        if (func2 != null) {
            func2.GetParameterTypes(noneEngine, result, OperandType.NUMBER, op, val);
        }
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "ROMAN");
    }
}
