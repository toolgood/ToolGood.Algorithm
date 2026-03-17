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
import toolgood.algorithm.internals.functions.Function_1;
import toolgood.algorithm.internals.functions.NoneEngine;

public final class Function_EVEN extends Function_1 {
    public Function_EVEN(FunctionBase func1) {
        super(func1);
    }

    @Override
    public String Name() {
        return "Even";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = GetNumber_1(engine, tempParameter);
        if (args1.IsErrorOrNone()) {
            return args1;
        }
        BigDecimal z = args1.NumberValue();
        if (z.remainder(BigDecimal.valueOf(2)).compareTo(BigDecimal.ZERO) == 0) {
            return args1;
        }
        z = z.setScale(0, RoundingMode.CEILING);
        if (z.remainder(BigDecimal.valueOf(2)).compareTo(BigDecimal.ZERO) == 0) {
            return Operand.Create(z);
        }
        z = z.add(BigDecimal.ONE);
        return Operand.Create(z);
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.NUMBER;
    }

    @Override
    public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType,
            String op, String val) {
        func1.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
    }
}
