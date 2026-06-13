package toolgood.algorithm.internals.functions.csharp;

import java.util.List;
import java.util.function.BiFunction;
import java.util.regex.Pattern;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_2;
import toolgood.algorithm.internals.functions.NoneEngine;

final class Function_SPLIT extends Function_2 {

    Function_SPLIT(FunctionBase[] funcs) {
        super(funcs);
        if (funcs.length != 2) {
            throw new IllegalArgumentException("Function 'Split' requires exactly 2 parameters.");
        }
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

        String[] parts = args1.TextValue().split(Pattern.quote(args2.TextValue()), -1);
        return Operand.Create(List.of(parts));
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.ARRAY;
    }

    @Override
    public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, String op, String val) {
        func1.GetParameterTypes(noneEngine, result, OperandType.TEXT);
        func2.GetParameterTypes(noneEngine, result, OperandType.TEXT);
    }
}
