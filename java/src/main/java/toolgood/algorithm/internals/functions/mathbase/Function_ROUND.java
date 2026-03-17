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

public final class Function_ROUND extends Function_2 {
    public Function_ROUND(FunctionBase[] funcs) {
        super(funcs);
    }

    @Override
    public String Name() {
        return "Round";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = GetNumber_1(engine, tempParameter);
        if (args1.IsErrorOrNone()) {
            return args1;
        }

        if (func2 == null) {
            return Operand.Create(args1.NumberValue().setScale(0, RoundingMode.HALF_UP));
        }
        Operand args2 = GetNumber_2(engine, tempParameter);
        if (args2.IsErrorOrNone()) {
            return args2;
        }
        if (args2.IntValue() < -15 || args2.IntValue() > 15) {
            return ParameterError(2);
        }
        return Operand.Create(args1.NumberValue().setScale(args2.IntValue(), RoundingMode.HALF_UP));
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
