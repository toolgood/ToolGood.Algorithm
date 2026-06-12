package toolgood.algorithm.internals.functions.mathSum2;

import java.math.BigDecimal;
import java.util.List;
import java.util.function.BiFunction;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.NoneEngine;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_4;

final class Function_WEIBULL extends Function_4 {

    public Function_WEIBULL(FunctionBase[] funcs) {
        super(funcs);
        if (funcs.length != 4) {
            throw new IllegalArgumentException("Function '" + Name() + "' requires exactly 4 parameters.");
        }
    }

    @Override
    public String Name() {
        return "Weibull";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
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

        return Operand.Create(BigDecimal.valueOf(Weibull(x.doubleValue(), shape.doubleValue(), scale.doubleValue(), state)));
    }

    private static double Weibull(double x, double shape, double scale, boolean state) {
        if (state == false) {
            return PDF(shape, scale, x);
        }
        return CDF(shape, scale, x);
    }

    private static double PDF(double shape, double scale, double x) {
        if (x >= 0.0) {
            if (x == 0.0 && shape == 1.0) {
                return shape / scale;
            }

            return shape
                    * Math.pow(x / scale, shape - 1.0)
                    * Math.exp(-Math.pow(x, shape) * Math.pow(scale, -shape))
                    / scale;
        }
        return 0.0;
    }

    private static double CDF(double shape, double scale, double x) {
        if (x < 0.0) {
            return 0.0;
        }

        return -ExponentialMinusOne(-Math.pow(x, shape) * Math.pow(scale, -shape));
    }

    private static double ExponentialMinusOne(double power) {
        double x = Math.abs(power);
        if (x > 0.1) {
            return Math.exp(power) - 1.0;
        }

        if (x < PositiveEpsilonOf(x)) {
            return x;
        }

        // Series Expansion to x^k / k! with Kahan summation
        int k = 0;
        double term = 1.0;
        double compensation = 0.0;
        double current;
        double sum = 0.0;
        final double factor = 1 << 16;
        do {
            k++;
            term *= power;
            term /= k;
            current = term;
            double y = current - compensation;
            double t = sum + y;
            compensation = t - sum;
            compensation -= y;
            sum = t;
        } while (Math.abs(sum) < Math.abs(factor * current));
        return sum;
    }

    private static double PositiveEpsilonOf(double value) {
        return 2 * EpsilonOf(value);
    }

    private static double EpsilonOf(double value) {
        if (value == 0) {
            return 0.0000000000000000000000000001;
        }

        double epsilon = 0.0000000000000000000000000001;
        while (value - epsilon == value) {
            epsilon *= 0.1;
        }
        return epsilon;
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.NUMBER;
    }

    @Override
    void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, String op, String val) {
        func1.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
        func2.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
        func3.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
        func4.GetParameterTypes(noneEngine, result, OperandType.BOOLEAN);
    }
}
