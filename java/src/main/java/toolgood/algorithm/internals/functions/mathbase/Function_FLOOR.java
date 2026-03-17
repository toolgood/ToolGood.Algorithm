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

public final class Function_FLOOR extends Function_2 {
    public Function_FLOOR(FunctionBase[] funcs) {
        super(funcs);
    }

    @Override
    public String Name() {
        return "Floor";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) throws Exception {
        Operand args1 = GetNumber_1(engine, tempParameter);
        if (args1.IsErrorOrNone()) {
            return args1;
        }

        if (func2 == null) {
            BigDecimal d = args1.NumberValue();
            return Operand.Create(d.setScale(0, RoundingMode.FLOOR));
        }

        Operand args2 = GetNumber_2(engine, tempParameter);
        if (args2.IsErrorOrNone()) {
            return args2;
        }
        BigDecimal b = args2.NumberValue();
        if (b.compareTo(BigDecimal.ZERO) == 0) {
            return Operand.Zero;
        }
        if (b.compareTo(BigDecimal.ZERO) < 0) {
            return ParameterError(2);
        }

        BigDecimal a = args1.NumberValue();
        BigDecimal d = a.divide(b, 0, RoundingMode.FLOOR).multiply(b);
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
        if (func2 != null) {
            func2.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
        }
    }
}
