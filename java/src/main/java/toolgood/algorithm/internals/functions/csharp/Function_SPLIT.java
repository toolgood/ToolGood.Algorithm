package toolgood.algorithm.internals.functions.csharp;

import java.util.List;
import java.util.function.BiFunction;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.Function_2;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.NoneEngine;

public final class Function_SPLIT extends Function_2 {
    public Function_SPLIT(FunctionBase[] funcs) {
        super(funcs);
    }

    @Override
    public String Name() {
        return "Split";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = GetText_1(engine, tempParameter);
        if (args1.IsErrorOrNone()) { return args1; }

        Operand args2 = GetText_2(engine, tempParameter);
        if (args2.IsErrorOrNone()) { return args2; }

        if (args2.TextValue() == null || args2.TextValue().isEmpty()) {
            return ParameterError(2);
        }

        String[] parts = args1.TextValue().split("[" + java.util.regex.Pattern.quote(args2.TextValue()) + "]");
        return Operand.Create(parts);
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.ARRAY;
    }

    @Override
    public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, String op, String val) {
        func1.GetParameterTypes(noneEngine, result, OperandType.TEXT, null, null);
        func2.GetParameterTypes(noneEngine, result, OperandType.TEXT, null, null);
    }
}
