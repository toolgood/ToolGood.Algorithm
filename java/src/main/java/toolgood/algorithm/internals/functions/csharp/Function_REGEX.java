package toolgood.algorithm.internals.functions.csharp;

import java.util.List;
import java.util.function.BiFunction;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.Function_2;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.NoneEngine;

public final class Function_REGEX extends Function_2 {
    public Function_REGEX(FunctionBase[] funcs) {
        super(funcs);
    }

    @Override
    public String Name() {
        return "Regex";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = GetText_1(engine, tempParameter);
        if (args1.IsError() || args1.IsNone()) {
            return args1;
        }

        Operand args2 = GetText_2(engine, tempParameter);
        if (args2.IsError() || args2.IsNone()) {
            return args2;
        }

        try {
            Pattern pattern = Pattern.compile(args2.TextValue());
            Matcher matcher = pattern.matcher(args1.TextValue());
            if (!matcher.find()) {
                return FunctionError();
            }
            return Operand.Create(matcher.group());
        } catch (Exception e) {
            return ParameterError(2);
        }
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.TEXT;
    }

    @Override
    public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, String op, String val) {
        func1.GetParameterTypes(noneEngine, result, OperandType.TEXT, null, null);
        func2.GetParameterTypes(noneEngine, result, OperandType.TEXT, null, null);
    }
}
