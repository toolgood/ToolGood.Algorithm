package toolgood.algorithm.internals.functions.value;

import java.util.List;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_1;
import toolgood.algorithm.internals.functions.NoneEngine;

public final class Function_ERROR extends Function_1 {
    public Function_ERROR(FunctionBase func1) {
        super(func1);
    }

    @Override
    public String Name() {
        return "Error";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) throws Exception {
        Operand args1 = GetText_1(engine, tempParameter);
        if (args1.IsErrorOrNone()) {
            return args1;
        }
        return Operand.Error(args1.TextValue());
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.ERROR;
    }

    @Override
    public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, String op, String val) {
        if (func1 != null) {
            func1.GetParameterTypes(noneEngine, result, OperandType.TEXT);
        }
    }
}
