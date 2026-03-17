package toolgood.algorithm.internals.functions.mathsum2;

import java.math.BigDecimal;
import java.util.List;
import java.util.function.Supplier;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_4;
import toolgood.algorithm.internals.functions.NoneEngine;
import toolgood.algorithm.system.MathEx;

public final class Function_WEIBULL extends Function_4 {
    public Function_WEIBULL(FunctionBase[] funcs) {
        super(funcs);
    }

    @Override
    public String Name() {
        return "Weibull";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) throws Exception {
        Operand args1 = GetNumber_1(engine, tempParameter);
        if (args1.IsErrorOrNone()) return args1;

        Operand args2 = GetNumber_2(engine, tempParameter);
        if (args2.IsErrorOrNone()) return args2;

        Operand args3 = GetNumber_3(engine, tempParameter);
        if (args3.IsErrorOrNone()) return args3;

        Operand args4 = GetBoolean_4(engine, tempParameter);
        if (args4.IsErrorOrNone()) return args4;

        BigDecimal x = args1.NumberValue();
        BigDecimal shape = args2.NumberValue();
        if (shape.compareTo(BigDecimal.ZERO) <= 0) {
            return ParameterError(2);
        }
        BigDecimal scale = args3.NumberValue();
        if (scale.compareTo(BigDecimal.ZERO) <= 0) {
            return ParameterError(3);
        }
        boolean state = args4.BooleanValue();

        return Operand.Create(Weibull(x, shape, scale, state));
    }

    public BigDecimal Weibull(BigDecimal x, BigDecimal shape, BigDecimal scale, boolean state) {
        if (state == false) {
            return PDF(shape, scale, x);
        }
        return CDF(shape, scale, x);
    }

    public BigDecimal PDF(BigDecimal shape, BigDecimal scale, BigDecimal x) {
        if (x.compareTo(BigDecimal.ZERO) >= 0) {
            if (x.compareTo(BigDecimal.ZERO) == 0 && shape.compareTo(BigDecimal.ONE) == 0) {
                return shape.divide(scale, java.math.MathContext.DECIMAL128);
            }

            return shape
                    .multiply(MathEx.Pow(x.divide(scale, java.math.MathContext.DECIMAL128), shape.subtract(BigDecimal.ONE)))
                    .multiply(MathEx.Exp(MathEx.Pow(x, shape).multiply(MathEx.Pow(scale, shape.negate())).negate()))
                    .divide(scale, java.math.MathContext.DECIMAL128);
        }
        return BigDecimal.ZERO;
    }

    public BigDecimal CDF(BigDecimal shape, BigDecimal scale, BigDecimal x) {
        if (x.compareTo(BigDecimal.ZERO) < 0) {
            return BigDecimal.ZERO;
        }

        return ExponentialMinusOne(MathEx.Pow(x, shape).multiply(MathEx.Pow(scale, shape.negate())).negate());
    }

    public BigDecimal ExponentialMinusOne(BigDecimal power) {
        BigDecimal x = power.abs();
        if (x.compareTo(new BigDecimal("0.1")) > 0) {
            return MathEx.Exp(power).subtract(BigDecimal.ONE);
        }

        if (x.compareTo(PositiveEpsilonOf(x)) < 0) {
            return x;
        }

        final int[] k = {0};
        final BigDecimal[] term = {BigDecimal.ONE};
        return Series(() -> {
            k[0]++;
            term[0] = term[0].multiply(power);
            term[0] = term[0].divide(new BigDecimal(k[0]), java.math.MathContext.DECIMAL128);
            return term[0];
        });
    }

    public BigDecimal PositiveEpsilonOf(BigDecimal value) {
        return new BigDecimal("2").multiply(EpsilonOf(value));
    }

    public static BigDecimal EpsilonOf(BigDecimal value) {
        if (value.compareTo(BigDecimal.ZERO) == 0) {
            return new BigDecimal("0.0000000000000000000000000001");
        }

        BigDecimal epsilon = new BigDecimal("0.0000000000000000000000000001");
        while (value.subtract(epsilon).compareTo(value) == 0) {
            epsilon = epsilon.multiply(new BigDecimal("0.1"));
        }
        return epsilon;
    }

    BigDecimal Series(Supplier<BigDecimal> nextSummand) {
        BigDecimal compensation = BigDecimal.ZERO;
        BigDecimal current;
        final BigDecimal factor = new BigDecimal(1 << 16);

        BigDecimal sum = nextSummand.get();

        do {
            current = nextSummand.get();
            BigDecimal y = current.subtract(compensation);
            BigDecimal t = sum.add(y);
            compensation = t.subtract(sum);
            compensation = compensation.subtract(y);
            sum = t;
        } while (sum.abs().compareTo(current.abs().multiply(factor)) < 0);
        return sum;
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.NUMBER;
    }

    @Override
    public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, String op, String val) {
        func1.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
        func2.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
        func3.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
        func4.GetParameterTypes(noneEngine, result, OperandType.BOOLEAN);
    }
}
