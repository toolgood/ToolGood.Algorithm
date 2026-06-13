package toolgood.algorithm.internals.functions.mathSum2;

import java.math.BigDecimal;
import java.util.List;
import java.util.function.BiFunction;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.functions.NoneEngine;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_3;

public final class Function_EXPONDIST extends Function_3 {
    public Function_EXPONDIST(FunctionBase[] funcs) {
        super(funcs);
        if (funcs.length != 3) {
            throw new IllegalArgumentException("Function '" + Name() + "' requires exactly 3 parameters.");
        }
    }

    @Override
    public String Name() { return "ExpOnDist"; }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = GetNumber_1(engine, tempParameter);
        if (args1.IsErrorOrNone()) return args1;

        Operand args2 = GetNumber_2(engine, tempParameter);
        if (args2.IsErrorOrNone()) return args2;

        Operand args3 = GetBoolean_3(engine, tempParameter);
        if (args3.IsErrorOrNone()) return args3;

        BigDecimal n1 = args1.NumberValue();
        if (n1.compareTo(BigDecimal.ZERO) < 0) {
            return ParameterError(1);
        }
        BigDecimal rate = args2.NumberValue();
        if (rate.compareTo(BigDecimal.ZERO) <= 0) {
            return ParameterError(2);
        }
        return Operand.Create(BigDecimal.valueOf(ExponDist(n1.doubleValue(), rate.doubleValue(), args3.BooleanValue())));
    }

    private static double ExponDist(double x, double rate, boolean state) {
        if (state) {
            return CDF(rate, x);
        }
        return PDF(rate, x);
    }

    private static double PDF(double rate, double x) {
        return x < 0.0 ? 0.0 : rate * Math.exp(-rate * x);
    }

    private static double CDF(double rate, double x) {
        return x < 0.0 ? 0.0 : 1.0 - Math.exp(-rate * x);
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.NUMBER;
    }

    @Override
    public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, String op, String val) {
        func1.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
        func2.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
        func3.GetParameterTypes(noneEngine, result, OperandType.BOOLEAN);
    }
}
