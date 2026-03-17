package toolgood.algorithm.internals.functions.mathtrigonometric;

import java.math.BigDecimal;
import java.util.List;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_2;
import toolgood.algorithm.internals.functions.NoneEngine;
import toolgood.algorithm.system.MathEx;

public final class Function_ATAN2 extends Function_2 {
    public Function_ATAN2(FunctionBase[] funcs) {
        super(funcs);
    }

    @Override
    public String Name() {
        return "Atan2";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) throws Exception {
        Operand args1 = GetNumber_1(engine, tempParameter);
        if (args1.IsErrorOrNone()) { return args1; }
        Operand args2 = GetNumber_2(engine, tempParameter);
        if (args2.IsErrorOrNone()) { return args2; }
        if (args1.NumberValue().compareTo(BigDecimal.ZERO) == 0 && args2.NumberValue().compareTo(BigDecimal.ZERO) == 0) {
            return Div0Error();
        }
        return Operand.Create(MathEx.Atan2(args2.NumberValue(), args1.NumberValue()));
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
