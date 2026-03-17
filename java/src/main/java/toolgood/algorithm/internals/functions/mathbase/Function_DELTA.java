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
import toolgood.algorithm.internals.functions.NoneEngine;

public final class Function_DELTA extends Function_2 {
    public Function_DELTA(FunctionBase[] funcs) {
        super(funcs);
    }

    @Override
    public String Name() {
        return "Delta";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        if (func1 == null) return ParameterError(1);

        Operand args1 = GetNumber_1(engine, tempParameter);
        if (args1.IsErrorOrNone()) {
            return args1;
        }
        BigDecimal num1 = args1.NumberValue();

        BigDecimal num2 = BigDecimal.ZERO;
        if (func2 != null) {
            Operand args2 = GetNumber_2(engine, tempParameter);
            if (args2.IsErrorOrNone()) {
                return args2;
            }
            num2 = args2.NumberValue();
        }

        return Operand.Create(num1.compareTo(num2) == 0 ? 1 : 0);
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
