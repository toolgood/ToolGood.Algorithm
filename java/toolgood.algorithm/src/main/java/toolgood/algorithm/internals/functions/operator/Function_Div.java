package toolgood.algorithm.internals.functions.operator;

import java.math.BigDecimal;
import java.math.RoundingMode;
import java.util.List;
import java.util.function.BiFunction;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.functions.NoneEngine;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_2;

public final class Function_Div extends Function_2 {

    public Function_Div(FunctionBase[] funcs) {
        super(funcs);
    }

    public Function_Div(FunctionBase func1, FunctionBase func2) {
        super(func1, func2);
    }

    @Override
    public String Name() {
        return "/";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = GetNumber_1(engine, tempParameter); if (args1.IsErrorOrNone()) { return args1; }
        Operand args2 = GetNumber_2(engine, tempParameter); if (args2.IsErrorOrNone()) { return args2; }

        if (args2.NumberValue().compareTo(BigDecimal.ZERO) == 0) { return Div0Error(); }
        if (args2.NumberValue().compareTo(BigDecimal.ONE) == 0) { return args1; }

        return Operand.Create(args1.NumberValue().divide(args2.NumberValue(), RoundingMode.HALF_UP));
    }

    @Override
    public void ToString(StringBuilder stringBuilder, boolean addBrackets) {
        if (addBrackets) stringBuilder.append('(');
        func1.ToString(stringBuilder, true);
        stringBuilder.append(" / ");
        func2.ToString(stringBuilder, true);
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
