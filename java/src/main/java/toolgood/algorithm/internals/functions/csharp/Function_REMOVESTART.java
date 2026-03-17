package toolgood.algorithm.internals.functions.csharp;

import java.util.List;
import java.util.function.BiFunction;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.Function_3;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.NoneEngine;

public final class Function_REMOVESTART extends Function_3 {
    public Function_REMOVESTART(FunctionBase[] funcs) {
        super(funcs);
    }

    @Override
    public String Name() {
        return "RemoveStart";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) throws Exception {
        Operand args1 = GetText_1(engine, tempParameter);
        if (args1.IsErrorOrNone()) { return args1; }

        Operand args2 = GetText_2(engine, tempParameter);
        if (args2.IsErrorOrNone()) { return args2; }

        boolean ignoreCase = false;
        if (func3 != null) {
            Operand args3 = GetBoolean_3(engine, tempParameter);
            if (args3.IsErrorOrNone()) { return args3; }
            ignoreCase = args3.BooleanValue();
        }

        String text = args1.TextValue();
        String prefix = args2.TextValue();
        boolean startsWith;
        if (ignoreCase) {
            startsWith = text.toLowerCase().startsWith(prefix.toLowerCase());
        } else {
            startsWith = text.startsWith(prefix);
        }
        if (startsWith) {
            return Operand.Create(text.substring(prefix.length()));
        }
        return args1;
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.TEXT;
    }

    @Override
    public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, String op, String val) {
        func1.GetParameterTypes(noneEngine, result, OperandType.TEXT, null, null);
        func2.GetParameterTypes(noneEngine, result, OperandType.TEXT, null, null);
        if (func3 != null) {
            func3.GetParameterTypes(noneEngine, result, OperandType.BOOLEAN, null, null);
        }
    }
}
