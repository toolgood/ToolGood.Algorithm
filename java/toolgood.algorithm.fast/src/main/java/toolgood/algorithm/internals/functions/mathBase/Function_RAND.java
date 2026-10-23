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
import toolgood.algorithm.internals.functions.Function_0;

public final class Function_RAND extends Function_0 {
    private static final Random _random = new Random();
    private static final Object _randomLock = new Object();

    public Function_RAND() {
    }

    @Override
    public String Name() {
        return "Rand";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        synchronized (_randomLock) {
            return Operand.Create(BigDecimal.valueOf(_random.nextDouble()));
        }
    }

    @Override
    public void ToString(StringBuilder stringBuilder, boolean addBrackets) {
        stringBuilder.append("Rand()");
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.NUMBER;
    }

    @Override
    public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, String op, String val) {
    }
}
