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
import toolgood.algorithm.system.MathEx;

public final class Function_LOG extends Function_2 {
    public Function_LOG(FunctionBase[] funcs) {
        super(funcs);
    }

    public Function_LOG(FunctionBase func1, FunctionBase func2) {
        super(func1, func2);
    }

    @Override
    public String Name() {
        return "Log";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) throws Exception {
        Operand args1 = GetNumber_1(engine, tempParameter);
        if (args1.IsErrorOrNone()) {
            return args1;
        }

        BigDecimal z = args1.NumberValue();
        if (z.compareTo(BigDecimal.ZERO) <= 0) {
            return ParameterError(1);
        }

        if (func2 == null) {
            return Operand.Create(MathEx.Log10(z));
        }

        Operand args2 = GetNumber_2(engine, tempParameter);
        if (args2.IsErrorOrNone()) {
            return args2;
        }
        BigDecimal baseValue = args2.NumberValue();
        if (baseValue.compareTo(BigDecimal.ZERO) <= 0 || baseValue.compareTo(BigDecimal.ONE) == 0) {
            return ParameterError(2);
        }
        return Operand.Create(MathEx.Log(z, baseValue));
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
