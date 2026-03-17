package toolgood.algorithm.internals.functions.mathsum2;

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

public final class Function_BESSELJ extends Function_2 {
    public Function_BESSELJ(FunctionBase func1, FunctionBase func2) {
        super(func1, func2);
    }

    public Function_BESSELJ(FunctionBase[] funcs) {
        super(funcs);
    }

    @Override
    public String Name() {
        return "BesselJ";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) throws Exception {
        Operand args1 = GetNumber_1(engine, tempParameter);
        if (args1.IsErrorOrNone()) {
            return args1;
        }
        Operand args2 = GetNumber_2(engine, tempParameter);
        if (args2.IsErrorOrNone()) {
            return args2;
        }

        BigDecimal x = args1.NumberValue();
        int n = (int) Math.floor(args2.NumberValue().doubleValue());

        return Operand.Create(BesselJ(n, x));
    }

    private static BigDecimal BesselJ(int n, BigDecimal x) {
        if (x.compareTo(BigDecimal.ZERO) == 0) {
            return (n == 0) ? BigDecimal.ONE : BigDecimal.ZERO;
        }

        if (n < 0) n = -n;

        BigDecimal ax = x.abs();
        if (ax.compareTo(new BigDecimal("1e-10")) < 0) {
            return (n == 0) ? BigDecimal.ONE : BigDecimal.ZERO;
        }

        if (n == 0) return BesselJ0(x);
        if (n == 1) return BesselJ1(x);

        if (ax.compareTo(new BigDecimal(n)) > 0) {
            BigDecimal J0 = BesselJ0(x);
            BigDecimal J1 = BesselJ1(x);
            BigDecimal Jn = BigDecimal.ZERO;

            for (int k = 1; k < n; k++) {
                Jn = new BigDecimal("2").multiply(new BigDecimal(k)).divide(x, java.math.MathContext.DECIMAL128).multiply(J1).subtract(J0);
                J0 = J1;
                J1 = Jn;
            }
            return J1;
        }

        int m = (int) (1.5 * n + 10);
        BigDecimal[] J = new BigDecimal[m + 2];
        J[m + 1] = BigDecimal.ZERO;
        J[m] = BigDecimal.ONE;

        for (int k = m; k >= 1; k--) {
            J[k - 1] = new BigDecimal("2").multiply(new BigDecimal(k)).divide(x, java.math.MathContext.DECIMAL128).multiply(J[k]).subtract(J[k + 1]);
        }

        BigDecimal sum = BigDecimal.ZERO;
        for (int k = 0; k <= m; k += 2) {
            sum = sum.add(new BigDecimal("2").multiply(J[k]));
        }
        sum = sum.subtract(J[0]);

        return J[n].divide(sum, java.math.MathContext.DECIMAL128);
    }

    private static BigDecimal BesselJ0(BigDecimal x) {
        BigDecimal ax = x.abs();
        if (ax.compareTo(new BigDecimal("8")) < 0) {
            BigDecimal y1 = x.multiply(x);
            BigDecimal t1 = y1.multiply(new BigDecimal("-184.9052456"));
            BigDecimal t2 = y1.multiply(new BigDecimal("77392.33017").add(t1));
            BigDecimal t3 = y1.multiply(new BigDecimal("-11214424.18").add(t2));
            BigDecimal t4 = y1.multiply(new BigDecimal("651619640.7").add(t3));
            BigDecimal ans1 = new BigDecimal("57568490574").add(y1.multiply(new BigDecimal("-13362590354").add(t4)));

            BigDecimal s1 = y1.multiply(BigDecimal.ONE);
            BigDecimal s2 = y1.multiply(new BigDecimal("267.8532712").add(s1));
            BigDecimal s3 = y1.multiply(new BigDecimal("59272.64853").add(s2));
            BigDecimal s4 = y1.multiply(new BigDecimal("9494680.718").add(s3));
            BigDecimal ans2 = new BigDecimal("57568490411").add(y1.multiply(new BigDecimal("1029532985").add(s4)));

            return ans1.divide(ans2, java.math.MathContext.DECIMAL128);
        }
        BigDecimal z = new BigDecimal("8").divide(ax, java.math.MathContext.DECIMAL128);
        BigDecimal y2 = z.multiply(z);
        BigDecimal xx = ax.subtract(new BigDecimal("0.78539816339744830962"));

        BigDecimal a1 = y2.multiply(new BigDecimal("0.0000002093887211"));
        BigDecimal a2 = y2.multiply(new BigDecimal("-0.000002073370639").add(a1));
        BigDecimal a3 = y2.multiply(new BigDecimal("0.00002734510407").add(a2));
        BigDecimal ans3 = BigDecimal.ONE.add(y2.multiply(new BigDecimal("-0.001098628627").add(a3)));

        BigDecimal b1 = y2.multiply(new BigDecimal("0.0000000934935152"));
        BigDecimal b2 = y2.multiply(new BigDecimal("0.0000007621095161").subtract(b1));
        BigDecimal b3 = y2.multiply(new BigDecimal("-0.000006911147651").add(b2));
        BigDecimal ans4 = new BigDecimal("-0.01562499995").add(y2.multiply(new BigDecimal("0.0001430488765").add(b3)));

        return MathEx.Sqrt(new BigDecimal("0.63661977236758134308").divide(ax, java.math.MathContext.DECIMAL128))
            .multiply(MathEx.Cos(xx).multiply(ans3).subtract(z.multiply(MathEx.Sin(xx)).multiply(ans4)));
    }

    private static BigDecimal BesselJ1(BigDecimal x) {
        BigDecimal ax = x.abs();
        if (ax.compareTo(new BigDecimal("8")) < 0) {
            BigDecimal y1 = x.multiply(x);
            BigDecimal t1 = y1.multiply(new BigDecimal("-30.16036606"));
            BigDecimal t2 = y1.multiply(new BigDecimal("15704.48260").add(t1));
            BigDecimal t3 = y1.multiply(new BigDecimal("-2972611.439").add(t2));
            BigDecimal t4 = y1.multiply(new BigDecimal("242396853.1").add(t3));
            BigDecimal ans1 = x.multiply(new BigDecimal("72362614232").add(y1.multiply(new BigDecimal("-7895059235").add(t4))));

            BigDecimal s1 = y1.multiply(BigDecimal.ONE);
            BigDecimal s2 = y1.multiply(new BigDecimal("376.9991397").add(s1));
            BigDecimal s3 = y1.multiply(new BigDecimal("99447.43394").add(s2));
            BigDecimal s4 = y1.multiply(new BigDecimal("18583304.74").add(s3));
            BigDecimal ans2 = new BigDecimal("144725228442").add(y1.multiply(new BigDecimal("2300535178").add(s4)));

            return ans1.divide(ans2, java.math.MathContext.DECIMAL128);
        }
        BigDecimal z = new BigDecimal("8").divide(ax, java.math.MathContext.DECIMAL128);
        BigDecimal y2 = z.multiply(z);
        BigDecimal xx = ax.subtract(new BigDecimal("2.35619449019234492885"));

        BigDecimal a1 = y2.multiply(new BigDecimal("-0.000000240337019"));
        BigDecimal a2 = y2.multiply(new BigDecimal("0.000002457520174").add(a1));
        BigDecimal a3 = y2.multiply(new BigDecimal("-0.00003516396496").add(a2));
        BigDecimal ans3 = BigDecimal.ONE.add(y2.multiply(new BigDecimal("0.00183105").add(a3)));

        BigDecimal b1 = y2.multiply(new BigDecimal("0.000000105787412"));
        BigDecimal b2 = y2.multiply(new BigDecimal("-0.00000088228987").add(b1));
        BigDecimal b3 = y2.multiply(new BigDecimal("0.000008449199096").add(b2));
        BigDecimal ans4 = new BigDecimal("0.04687499995").add(y2.multiply(new BigDecimal("-0.0002002690873").add(b3)));

        BigDecimal ans = MathEx.Sqrt(new BigDecimal("0.63661977236758134308").divide(ax, java.math.MathContext.DECIMAL128))
            .multiply(MathEx.Cos(xx).multiply(ans3).subtract(z.multiply(MathEx.Sin(xx)).multiply(ans4)));
        return (x.compareTo(BigDecimal.ZERO) < 0) ? ans.negate() : ans;
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
