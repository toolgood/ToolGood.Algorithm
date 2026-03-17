package toolgood.algorithm.internals.functions.operator;

import java.util.List;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_2;
import toolgood.algorithm.internals.functions.NoneEngine;

public final class Function_Add extends Function_2 {
    public Function_Add(FunctionBase[] funcs) {
        super(funcs);
    }

    public Function_Add(FunctionBase func1, FunctionBase func2) {
        super(func1, func2);
    }

    @Override
    public String Name() {
        return "+";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = GetNumber_1(engine, tempParameter);
        if (args1.IsErrorOrNone()) { return args1; }
        Operand args2 = GetNumber_2(engine, tempParameter);
        if (args2.IsErrorOrNone()) { return args2; }

        return Operand.Create(args1.NumberValue().add(args2.NumberValue()));
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        if (addBrackets) stringBuilder.append('(');
        func1.toString(stringBuilder, false);
        stringBuilder.append(" + ");
        func2.toString(stringBuilder, false);
        if (addBrackets) stringBuilder.append(')');
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.NUMBER;
    }

    @Override
    public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, String op, String val) {
        func1.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
        func2.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
    }
}
