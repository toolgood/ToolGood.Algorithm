package toolgood.algorithm.internals.functions.mathsum2;

import java.math.BigDecimal;
import java.util.List;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_1;
import toolgood.algorithm.internals.functions.NoneEngine;
import toolgood.algorithm.system.MathEx;

public final class Function_FISHER extends Function_1 {
    public Function_FISHER(FunctionBase func1) {
        super(func1);
    }

    @Override
    public String Name() {
        return "Fisher";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) throws Exception {
        Operand args1 = GetNumber_1(engine, tempParameter);
        if (args1.IsErrorOrNone()) {
            return args1;
        }
        BigDecimal x = args1.NumberValue();
        if (x.compareTo(BigDecimal.ONE) >= 0 || x.compareTo(BigDecimal.ONE.negate()) <= 0) {
            return ParameterError(1);
        }
        BigDecimal n = new BigDecimal("0.5").multiply(MathEx.Log(BigDecimal.ONE.add(x).divide(BigDecimal.ONE.subtract(x), java.math.MathContext.DECIMAL128)));
        return Operand.Create(n);
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.NUMBER;
    }

    @Override
    public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, String op, String val) {
        func1.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
    }
}
