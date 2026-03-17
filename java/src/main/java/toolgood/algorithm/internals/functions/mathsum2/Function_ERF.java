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

public final class Function_ERF extends Function_1 {
    public Function_ERF(FunctionBase func1) {
        super(func1);
    }

    public Function_ERF(FunctionBase[] funcs) {
        super(funcs);
    }

    @Override
    public String Name() {
        return "Erf";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = GetNumber_1(engine, tempParameter);
        if (args1.IsErrorOrNone()) {
            return args1;
        }
        BigDecimal x = args1.NumberValue();
        return Operand.Create(Erf(x));
    }

    private static BigDecimal Erf(BigDecimal x) {
        final BigDecimal a1 = new BigDecimal("0.254829592");
        final BigDecimal a2 = new BigDecimal("-0.284496736");
        final BigDecimal a3 = new BigDecimal("1.421413741");
        final BigDecimal a4 = new BigDecimal("-1.453152027");
        final BigDecimal a5 = new BigDecimal("1.061405429");
        final BigDecimal p = new BigDecimal("0.3275911");

        int sign = x.compareTo(BigDecimal.ZERO) < 0 ? -1 : 1;
        x = x.abs();

        if (x.compareTo(new BigDecimal("6.0")) > 0) {
            return new BigDecimal(sign);
        }

        BigDecimal t = BigDecimal.ONE.divide(BigDecimal.ONE.add(p.multiply(x)), java.math.MathContext.DECIMAL128);
        BigDecimal y = BigDecimal.ONE.subtract((((((a5.multiply(t).add(a4)).multiply(t)).add(a3)).multiply(t).add(a2)).multiply(t).add(a1)).multiply(t).multiply(MathEx.Exp(x.negate().multiply(x))));

        return new BigDecimal(sign).multiply(y);
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
