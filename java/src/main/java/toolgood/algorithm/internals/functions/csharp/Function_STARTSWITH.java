package toolgood.algorithm.internals.functions.csharp;

import java.lang.StringBuilder;
import java.util.List;
import java.util.function.BiFunction;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.Function_3;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.NoneEngine;

public final class Function_STARTSWITH extends Function_3 {
    public Function_STARTSWITH(FunctionBase[] funcs) {
        super(funcs);
    }

    @Override
    public String Name() {
        return "StartsWith";
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

        String text = args1.TextValue();
        if (func3 == null) {
            return Operand.Create(text.startsWith(args2.TextValue()));
        }

        Operand args3 = GetBoolean_3(engine, tempParameter);
        if (args3.IsError() || args3.IsNone()) {
            return args3;
        }

        if (args3.BooleanValue()) {
            return Operand.Create(text.toLowerCase().startsWith(args2.TextValue().toLowerCase()));
        }
        return Operand.Create(text.startsWith(args2.TextValue()));
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.BOOLEAN;
    }

    @Override
    public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result,
            OperandType operandType, String op, String val) {
        func1.GetParameterTypes(noneEngine, result, OperandType.TEXT, null, null);
        func2.GetParameterTypes(noneEngine, result, OperandType.TEXT, null, null);
        if (func3 != null) {
            func3.GetParameterTypes(noneEngine, result, OperandType.BOOLEAN, null, null);
        }
    }
}
