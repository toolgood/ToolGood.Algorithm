package toolgood.algorithm.internals.functions.operator;

import java.util.List;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_2;
import toolgood.algorithm.internals.functions.NoneEngine;

public final class Function_OR extends Function_2 {
    public Function_OR(FunctionBase[] funcs) {
        super(funcs);
    }

    public Function_OR(FunctionBase func1, FunctionBase func2) {
        super(func1, func2);
    }

    @Override
    public String Name() {
        return "Or";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) throws Exception {
        Operand args1 = GetBoolean_1(engine, tempParameter);
        if (args1.IsErrorOrNone()) { return args1; }
        if (args1.BooleanValue()) {
            Operand args2 = GetBoolean_2(engine, tempParameter);
            if (args2.IsErrorOrNone()) { return args2; }
            return Operand.True;
        }
        return GetBoolean_2(engine, tempParameter);
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        if (addBrackets) stringBuilder.append('(');
        func1.toString(stringBuilder, false);
        stringBuilder.append(" || ");
        func2.toString(stringBuilder, false);
        if (addBrackets) stringBuilder.append(')');
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.BOOLEAN;
    }

    @Override
    public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, String op, String val) {
        func1.GetParameterTypes(noneEngine, result, OperandType.BOOLEAN);
        func2.GetParameterTypes(noneEngine, result, OperandType.BOOLEAN);
    }
}
