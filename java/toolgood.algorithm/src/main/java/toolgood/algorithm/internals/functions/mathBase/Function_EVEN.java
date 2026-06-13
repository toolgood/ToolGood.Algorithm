package toolgood.algorithm.internals.functions.mathBase;

import java.math.BigDecimal;
import java.math.RoundingMode;
import java.util.List;
import java.util.function.BiFunction;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.functions.NoneEngine;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_1;

public final class Function_EVEN extends Function_1 {
    public Function_EVEN(FunctionBase[] funcs) {
        super(funcs);
        if (funcs.length != 1) {
            throw new IllegalArgumentException("Function '" + Name() + "' requires exactly 1 parameter.");
        }
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

        if (z.compareTo(BigDecimal.ZERO) == 0) {
            return Operand.Zero;
        }

        if (z.compareTo(BigDecimal.ZERO) > 0) {
            if (z.remainder(BigDecimal.valueOf(2)).compareTo(BigDecimal.ZERO) == 0) {
                return args1;
            }
            z = z.setScale(0, RoundingMode.CEILING);
            if (z.remainder(BigDecimal.valueOf(2)).compareTo(BigDecimal.ZERO) == 0) {
                return Operand.Create(z);
            }
            z = z.add(BigDecimal.ONE);
            return Operand.Create(z);
        } else {
            if (z.remainder(BigDecimal.valueOf(2)).compareTo(BigDecimal.ZERO) == 0) {
                return args1;
            }
            z = z.setScale(0, RoundingMode.FLOOR);
            if (z.remainder(BigDecimal.valueOf(2)).compareTo(BigDecimal.ZERO) == 0) {
                return Operand.Create(z);
            }
            z = z.subtract(BigDecimal.ONE);
            return Operand.Create(z);
        }
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.NUMBER;
    }

    @Override
    public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, String op, String val) {
        func1.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
    }
}
