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

public final class Function_ISNULLORWHITESPACE extends Function_1 {
    public Function_ISNULLORWHITESPACE(FunctionBase func1) {
        super(func1);
    }

    @Override
    public String Name() {
        return "IsNullOrWhitespace";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) throws Exception {
        Operand args1 = func1.Evaluate(engine, tempParameter);
        if (args1.IsNull()) {
            return Operand.True;
        }
        Operand textArg = ConvertToText(args1, 1);
        if (textArg.IsErrorOrNone()) {
            return textArg;
        }
        return Operand.Create(textArg.TextValue() == null || textArg.TextValue().trim().isEmpty());
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.BOOLEAN;
    }

    @Override
    public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType,
            String op, String val) {
        func1.GetParameterTypes(noneEngine, result, OperandType.NONE);
    }
}
