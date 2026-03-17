package toolgood.algorithm.internals.functions.mathbase;

import java.math.BigDecimal;
import java.util.List;
import java.util.function.BiFunction;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_2;
import toolgood.algorithm.system.MathEx;
import toolgood.algorithm.internals.functions.NoneEngine;

public final class Function_POWER extends Function_2 {
    public Function_POWER(FunctionBase[] funcs) {
        super(funcs);
    }

    @Override
    public String Name() {
        return "Power";
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

        BigDecimal baseValue = args1.NumberValue();
        BigDecimal exponent = args2.NumberValue();

        if (baseValue.compareTo(BigDecimal.ZERO) == 0 && exponent.compareTo(BigDecimal.ZERO) < 0) {
            return Div0Error();
        }
        if (baseValue.compareTo(BigDecimal.ZERO) < 0 && exponent.stripTrailingZeros().scale() > 0) {
            return ParameterError(1);
        }

        return Operand.Create(MathEx.Pow(baseValue, exponent));
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
