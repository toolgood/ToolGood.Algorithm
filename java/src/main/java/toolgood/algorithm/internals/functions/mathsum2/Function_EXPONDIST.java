package toolgood.algorithm.internals.functions.mathsum2;

import java.math.BigDecimal;
import java.util.List;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_3;
import toolgood.algorithm.internals.functions.NoneEngine;
import toolgood.algorithm.system.MathEx;

public final class Function_EXPONDIST extends Function_3 {
    public Function_EXPONDIST(FunctionBase[] funcs) {
        super(funcs);
    }

    @Override
    public String Name() {
        return "ExpOnDist";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
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
        return Operand.Create(ExponDist(n1, rate, args3.BooleanValue()));
    }

    public BigDecimal ExponDist(BigDecimal x, BigDecimal rate, boolean state) {
        if (state) {
            return CDF(rate, x);
        }
        return PDF(rate, x);
    }

    public BigDecimal PDF(BigDecimal rate, BigDecimal x) {
        return x.compareTo(BigDecimal.ZERO) < 0 ? BigDecimal.ZERO : rate.multiply(MathEx.Exp(rate.negate().multiply(x)));
    }

    public BigDecimal CDF(BigDecimal rate, BigDecimal x) {
        return x.compareTo(BigDecimal.ZERO) < 0 ? BigDecimal.ZERO : BigDecimal.ONE.subtract(MathEx.Exp(rate.negate().multiply(x)));
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
