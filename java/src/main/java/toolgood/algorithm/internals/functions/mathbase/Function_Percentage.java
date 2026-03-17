package toolgood.algorithm.internals.functions.mathbase;

import java.math.BigDecimal;
import java.math.MathContext;
import java.util.List;
import java.util.function.BiFunction;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_1;
import toolgood.algorithm.internals.functions.NoneEngine;

public final class Function_Percentage extends Function_1 {
    private static final BigDecimal HUNDRED = new BigDecimal("100");

    public Function_Percentage(FunctionBase func1) {
        super(func1);
    }

    @Override
    public String Name() {
        return "Percentage";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) throws Exception {
        Operand args1 = GetNumber_1(engine, tempParameter);
        if (args1.IsErrorOrNone()) {
            return args1;
        }
        return Operand.Create(args1.NumberValue().divide(HUNDRED, MathContext.DECIMAL128));
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        func1.toString(stringBuilder, false);
        stringBuilder.append('%');
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.NUMBER;
    }

    @Override
    public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType,
            String op, String val) {
        func1.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
    }
}
