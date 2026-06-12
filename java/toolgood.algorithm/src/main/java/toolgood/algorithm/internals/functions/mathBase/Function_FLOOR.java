package toolgood.algorithm.internals.functions.mathBase;

import java.math.BigDecimal;
import java.math.RoundingMode;
import java.util.List;
import java.util.function.BiFunction;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.NoneEngine;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_2;

final class Function_FLOOR extends Function_2 {
    public Function_FLOOR(FunctionBase[] funcs) {
        super(funcs);
        if (funcs.length < 1 || funcs.length > 2) {
            throw new IllegalArgumentException("Function '" + Name() + "' requires 1 to 2 parameters.");
        }
    }

    @Override
    public String Name() {
        return "Floor";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = GetNumber_1(engine, tempParameter);
        if (args1.IsErrorOrNone()) {
            return args1;
        }
        if (func2 == null) {
            return Operand.Create(args1.NumberValue().setScale(0, RoundingMode.FLOOR));
        }
        Operand args2 = GetNumber_2(engine, tempParameter);
        if (args2.IsErrorOrNone()) {
            return args2;
        }
        BigDecimal b = args2.NumberValue();
        if (b.compareTo(BigDecimal.ZERO) == 0) {
            return Operand.Zero;
        }
        BigDecimal a = args1.NumberValue();
        if (b.compareTo(BigDecimal.ZERO) > 0) {
            double d = Math.floor(a.doubleValue() / b.doubleValue()) * b.doubleValue();
            return Operand.Create(BigDecimal.valueOf(d));
        } else {
            if (a.compareTo(BigDecimal.ZERO) > 0) {
                return ParameterError(1);
            }
            double d = Math.floor(a.doubleValue() / b.doubleValue()) * b.doubleValue();
            return Operand.Create(BigDecimal.valueOf(d));
        }
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.NUMBER;
    }

    @Override
    void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, String op, String val) {
        func1.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
        if (func2 != null) {
            func2.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
        }
    }
}
