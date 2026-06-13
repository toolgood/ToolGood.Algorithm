package toolgood.algorithm.internals.functions.operator;

import java.util.List;
import java.util.function.BiFunction;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.functions.NoneEngine;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_2;

public final class Function_Connect extends Function_2 {

    public Function_Connect(FunctionBase[] funcs) {
        super(funcs);
    }

    public Function_Connect(FunctionBase func1, FunctionBase func2) {
        super(func1, func2);
    }

    @Override
    public String Name() {
        return "&";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = GetText_1(engine, tempParameter); if (args1.IsErrorOrNone()) { return args1; }
        Operand args2 = GetText_2(engine, tempParameter); if (args2.IsErrorOrNone()) { return args2; }

        return Operand.Create(args1.TextValue() + args2.TextValue());
    }

    @Override
    public void ToString(StringBuilder stringBuilder, boolean addBrackets) {
        if (addBrackets) stringBuilder.append('(');
        func1.ToString(stringBuilder, false);
        stringBuilder.append(" & ");
        func2.ToString(stringBuilder, false);
        if (addBrackets) stringBuilder.append(')');
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.TEXT;
    }

    @Override
    public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, String op, String val) {
        func1.GetParameterTypes(noneEngine, result, OperandType.TEXT);
        func2.GetParameterTypes(noneEngine, result, OperandType.TEXT);
    }
}
