package toolgood.algorithm.internals.functions.mathsum;

import java.math.BigDecimal;
import java.util.List;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_3;
import toolgood.algorithm.internals.functions.NoneEngine;
import toolgood.algorithm.mathNet.ExcelFunctions;

public final class Function_POISSON extends Function_3 {
    public Function_POISSON(FunctionBase[] funcs) {
        super(funcs);
    }

    @Override
    public String Name() {
        return "Poisson";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) throws Exception {
        Operand args1 = GetNumber_1(engine, tempParameter);
        if (args1.IsErrorOrNone()) return args1;

        Operand args2 = GetNumber_2(engine, tempParameter);
        if (args2.IsErrorOrNone()) return args2;

        Operand args3 = GetBoolean_3(engine, tempParameter);
        if (args3.IsErrorOrNone()) return args3;

        int k = args1.IntValue();
        if (k < 0) {
            return ParameterError(1);
        }
        BigDecimal lambda = args2.NumberValue();
        if (lambda.compareTo(BigDecimal.ZERO) <= 0) {
            return ParameterError(2);
        }
        boolean state = args3.BooleanValue();
        return Operand.Create(ExcelFunctions.Poisson(k, lambda, state));
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
