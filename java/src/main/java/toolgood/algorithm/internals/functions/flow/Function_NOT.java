package toolgood.algorithm.internals.functions.flow;

import java.util.List;
import java.util.function.BiFunction;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_1;
import toolgood.algorithm.internals.functions.NoneEngine;

public final class Function_NOT extends Function_1 {
    public Function_NOT(FunctionBase func1) {
        super(func1);
    }

    @Override
    public String Name() {
        return "Not";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) throws Exception {
        Operand args1 = GetBoolean_1(engine, tempParameter);
        if (args1.IsErrorOrNone()) {
            return args1;
        }
        return args1.BooleanValue() ? Operand.False : Operand.True;
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.BOOLEAN;
    }

    @Override
    public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType,
            String op, String val) {
        func1.GetParameterTypes(noneEngine, result, OperandType.BOOLEAN);
    }
}
