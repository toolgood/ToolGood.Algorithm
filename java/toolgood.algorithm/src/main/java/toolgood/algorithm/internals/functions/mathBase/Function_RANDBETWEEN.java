package toolgood.algorithm.internals.functions.mathBase;

import java.math.BigDecimal;
import java.util.List;
import java.util.Random;
import java.util.function.BiFunction;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.functions.NoneEngine;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_2;

public final class Function_RANDBETWEEN extends Function_2 {
    private static final Random _random = new Random();
    private static final Object _randomLock = new Object();

    public Function_RANDBETWEEN(FunctionBase[] funcs) {
        super(funcs);
        if (funcs.length != 2) {
            throw new IllegalArgumentException("Function '" + Name() + "' requires exactly 2 parameters.");
        }
    }

    @Override
    public String Name() {
        return "RandBetween";
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
        BigDecimal bottom = args1.NumberValue();
        BigDecimal top = args2.NumberValue();
        if (bottom.compareTo(top) > 0) {
            return ParameterError(1);
        }
        synchronized (_randomLock) {
            double r = Math.floor(_random.nextDouble() * (top.doubleValue() - bottom.doubleValue() + 1) + bottom.doubleValue());
            return Operand.Create(BigDecimal.valueOf(r));
        }
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.NUMBER;
    }

    @Override
    public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, String op, String val) {
        func1.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
        func2.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
    }
}
