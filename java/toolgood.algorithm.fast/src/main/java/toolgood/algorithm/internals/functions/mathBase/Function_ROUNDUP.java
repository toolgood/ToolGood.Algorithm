package toolgood.algorithm.internals.functions.mathBase;

import java.math.BigDecimal;
import java.util.List;
import java.util.function.BiFunction;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.functions.NoneEngine;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_2;

public final class Function_ROUNDUP extends Function_2 {
    public Function_ROUNDUP(FunctionBase[] funcs) {
        super(funcs);
        if (funcs.length != 2) {
            throw new IllegalArgumentException("Function '" + Name() + "' requires exactly 2 parameters.");
        }
    }

    @Override
    public String Name() {
        return "RoundUp";
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
        if (args2.IntValue() < -15 || args2.IntValue() > 15) {
            return ParameterError(2);
        }
        if (args1.NumberValue().compareTo(BigDecimal.ZERO) == 0) {
            return args1;
        }
        BigDecimal b = args1.NumberValue();
        int digits = args2.IntValue();
        double a = Math.pow(10, digits);
        double t = Math.ceil(Math.abs(b.doubleValue()) * a) / a;
        if (b.compareTo(BigDecimal.ZERO) > 0) {
            return Operand.Create(BigDecimal.valueOf(t));
        }
        return Operand.Create(BigDecimal.valueOf(-t));
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
