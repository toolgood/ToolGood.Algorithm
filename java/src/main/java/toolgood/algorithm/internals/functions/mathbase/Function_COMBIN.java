package toolgood.algorithm.internals.functions.mathbase;

import java.math.BigDecimal;
import java.math.RoundingMode;
import java.util.List;
import java.util.function.BiFunction;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_2;
import toolgood.algorithm.internals.functions.NoneEngine;

public final class Function_COMBIN extends Function_2 {
    public Function_COMBIN(FunctionBase[] funcs) {
        super(funcs);
    }

    @Override
    public String Name() {
        return "Combin";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = GetNumber_1(engine, tempParameter);
        if (args1.IsErrorOrNone()) {
            return args1;
        }

        Operand args2 = GetNumber_2(engine, tempParameter);
        if (args2.IsErrorOrNone()) {
            return args2;
        }

        int total = args1.IntValue();
        int count = args2.IntValue();
        if (total < 0) {
            return ParameterError(1);
        }
        if (count < 0) {
            return ParameterError(2);
        }
        if (total < count) {
            return ParameterError(2);
        }

        BigDecimal sum = BigDecimal.ONE;
        BigDecimal sum2 = BigDecimal.ONE;
        for (int i = 0; i < count; i++) {
            sum = sum.multiply(BigDecimal.valueOf(total - i));
            sum2 = sum2.multiply(BigDecimal.valueOf(i + 1));
        }
        return Operand.Create(sum.divide(sum2, RoundingMode.HALF_EVEN));
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.NUMBER;
    }

    @Override
    public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType,
            String op, String val) {
        func1.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
        func2.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
    }
}
