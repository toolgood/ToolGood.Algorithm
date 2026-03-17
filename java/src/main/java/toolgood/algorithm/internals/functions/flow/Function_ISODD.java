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

public final class Function_ISODD extends Function_1 {
    public Function_ISODD(FunctionBase func1) {
        super(func1);
    }

    @Override
    public String Name() {
        return "IsOdd";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(engine, tempParameter);
        if (args1.IsNumber()) {
            if (Math.abs(args1.IntValue()) % 2 == 1) {
                return Operand.True;
            }
        }
        return Operand.False;
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.BOOLEAN;
    }

    @Override
    public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType,
            String op, String val) {
        func1.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
    }
}
