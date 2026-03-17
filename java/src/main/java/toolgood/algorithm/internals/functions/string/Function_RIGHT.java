package toolgood.algorithm.internals.functions.string;

import java.util.List;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_2;
import toolgood.algorithm.internals.functions.NoneEngine;

public final class Function_RIGHT extends Function_2 {
    public Function_RIGHT(FunctionBase[] funcs) {
        super(funcs);
    }

    @Override
    public String Name() {
        return "Right";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) throws Exception {
        Operand args1 = GetText_1(engine, tempParameter);
        if (args1.IsErrorOrNone()) { return args1; }

        if (args1.TextValue().length() == 0) {
            return Operand.Create("");
        }
        if (func2 == null) {
            return Operand.Create(args1.TextValue().substring(args1.TextValue().length() - 1));
        }
        Operand args2 = GetNumber_2(engine, tempParameter);
        if (args2.IsErrorOrNone()) { return args2; }
        if (args2.IntValue() < 0) {
            return ParameterError(2);
        }
        int length = Math.min(args2.IntValue(), args1.TextValue().length());
        int start = args1.TextValue().length() - length;
        return Operand.Create(args1.TextValue().substring(start));
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.TEXT;
    }

    @Override
    public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, String op, String val) {
        func1.GetParameterTypes(noneEngine, result, OperandType.TEXT);
        if (func2 != null) {
            func2.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
        }
    }
}
