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

public final class Function_MROUND extends Function_2 {
    public Function_MROUND(FunctionBase[] funcs) {
        super(funcs);
    }

    @Override
    public String Name() {
        return "Mround";
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
        BigDecimal a = args2.NumberValue();
        if (a.compareTo(BigDecimal.ZERO) <= 0) {
            return ParameterError(2);
        }

        BigDecimal b = args1.NumberValue();
        BigDecimal r = b.divide(a, 0, RoundingMode.HALF_UP).multiply(a);
        return Operand.Create(r);
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
