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

public final class Function_FACTDOUBLE extends Function_1 {
    public Function_FACTDOUBLE(FunctionBase func1) {
        super(func1);
    }

    @Override
    public String Name() {
        return "FactDouble";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = GetNumber_1(engine, tempParameter);
        if (args1.IsErrorOrNone()) {
            return args1;
        }
        int z = args1.IntValue();
        if (z < 0) {
            return ParameterError(1);
        }
        if (z > 300) {
            return ParameterError(1);
        }

        BigDecimal d = BigDecimal.ONE;
        for (int i = z; i > 0; i -= 2) {
            d = d.multiply(BigDecimal.valueOf(i));
        }
        return Operand.Create(d);
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
