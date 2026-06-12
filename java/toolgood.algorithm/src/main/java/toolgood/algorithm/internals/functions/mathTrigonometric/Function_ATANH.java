package toolgood.algorithm.internals.functions.mathTrigonometric;

import java.math.BigDecimal;
import java.util.List;
import java.util.function.BiFunction;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.NoneEngine;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_1;

final class Function_ATANH extends Function_1 {
    public Function_ATANH(FunctionBase[] funcs) {
        super(funcs);
        if (funcs.length != 1) {
            throw new IllegalArgumentException("Function '" + Name() + "' requires exactly 1 parameter.");
        }
    }

    @Override
    public String Name() { return "Atanh"; }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = GetNumber_1(engine, tempParameter);
        if (args1.IsErrorOrNone()) { return args1; }
        BigDecimal x = args1.NumberValue();
        if (x.compareTo(BigDecimal.valueOf(-1)) <= 0 || x.compareTo(BigDecimal.ONE) >= 0) {
            return ParameterError(1);
        }
        double dx = x.doubleValue();
        return Operand.Create(BigDecimal.valueOf(0.5 * Math.log((1 + dx) / (1 - dx))));
    }

    @Override
    public OperandType GetResultType() { return OperandType.NUMBER; }

    @Override
    void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, String op, String val) {
        func1.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
    }
}
