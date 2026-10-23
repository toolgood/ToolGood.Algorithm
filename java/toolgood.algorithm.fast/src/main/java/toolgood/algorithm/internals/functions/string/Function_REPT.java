package toolgood.algorithm.internals.functions.string;

import java.util.List;
import java.util.function.BiFunction;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_2;
import toolgood.algorithm.internals.functions.NoneEngine;

public final class Function_REPT extends Function_2 {

    public Function_REPT(FunctionBase[] funcs) {
        super(funcs);
        if (funcs.length != 2) {
            throw new IllegalArgumentException("Function '" + Name() + "' requires exactly 2 parameters.");
        }
    }

    @Override
    public String Name() {
        return "Rept";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = GetText_1(engine, tempParameter);
        if (args1.IsErrorOrNone()) { return args1; }
        Operand args2 = GetNumber_2(engine, tempParameter);
        if (args2.IsErrorOrNone()) { return args2; }

        String newtext = args1.TextValue();
        int length = args2.IntValue();
        if (length < 0) {
            return ParameterError(2);
        }
        if (length == 0) {
            return Operand.Create("");
        }
        if (newtext.length() == 0) {
            return args1;
        }
        if (length > 32767 / newtext.length()) {
            return ParameterError(2);
        }
        if (length == 1) {
            return args1;
        }
        StringBuilder sb = new StringBuilder(newtext.length() * length);
        for (int i = 0; i < length; i++) {
            sb.append(newtext);
        }
        return Operand.Create(sb.toString());
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.TEXT;
    }

    @Override
    public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, String op, String val) {
        func1.GetParameterTypes(noneEngine, result, OperandType.TEXT);
        func2.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
    }
}
