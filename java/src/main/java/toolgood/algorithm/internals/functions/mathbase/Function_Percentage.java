package toolgood.algorithm.internals.functions.mathbase;

import java.math.BigDecimal;
import java.util.List;
import java.util.function.BiFunction;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_1;
import toolgood.algorithm.internals.functions.NoneEngine;

/**
 * Percentage 函数：将数值除�?100（百分比运算），对应 C# 中的 % 运算符�?
 * 参数：number%  �?Percentage(number)
 */
public class Function_Percentage extends Function_1 {

    private static final BigDecimal HUNDRED = new BigDecimal("100");

    public Function_Percentage(FunctionBase func1) {
        super(func1);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work,
            BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(work, tempParameter);
        if (args1.IsNotNumber()) {
            args1 = ConvertToNumber(args1, 1);
            if (args1.IsError()) {
                return args1;
            }
        }
        return Operand.Create(args1.NumberValue().divide(HUNDRED));
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.NUMBER;
    }

    @Override
    public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result,
            OperandType operandType, String op, String val) {
        func1.GetParameterTypes(noneEngine, result, OperandType.NUMBER, op, val);
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        // 百分比的 toString 格式：原参数后加 %，不加括�?
        func1.toString(stringBuilder, false);
        stringBuilder.append('%');
    }
}
