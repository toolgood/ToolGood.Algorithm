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

public final class Function_BESSELY extends Function_2 {
    public Function_BESSELY(FunctionBase func1, FunctionBase func2) {
        super(func1, func2);
    }

    public Function_BESSELY(FunctionBase[] funcs) {
        super(funcs);
    }

    @Override
    public String Name() {
        return "BesselY";
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

        if (x.compareTo(BigDecimal.ZERO) <= 0) {
            return ParameterError(1);
        }

        return Operand.Create(BesselY(n, x));
    }

    private static BigDecimal BesselY(int n, BigDecimal x) {
        if (n < 0) n = -n;

        if (n == 0) return BesselY0(x);
        if (n == 1) return BesselY1(x);

        BigDecimal Y0 = BesselY0(x);
        BigDecimal Y1 = BesselY1(x);
        BigDecimal Yn = BigDecimal.ZERO;

        for (int k = 1; k < n; k++) {
            Yn = new BigDecimal("2").multiply(new BigDecimal(k)).divide(x, java.math.MathContext.DECIMAL128).multiply(Y1).subtract(Y0);
            Y0 = Y1;
            Y1 = Yn;
        }

        return Y1;
    }

    private static BigDecimal BesselY0(BigDecimal x) {
        if (x.compareTo(new BigDecimal("8")) < 0) {
            BigDecimal y1 = x.multiply(x);
            BigDecimal t1 = y1.multiply(new BigDecimal("228.4622733"));
            BigDecimal t2 = y1.multiply(new BigDecimal("-86327.92757").add(t1));
            BigDecimal t3 = y1.multiply(new BigDecimal("10879881.29").add(t2));
            BigDecimal t4 = y1.multiply(new BigDecimal("-512359803.6").add(t3));
            BigDecimal t5 = y1.multiply(new BigDecimal("7062834065").add(t4));
            BigDecimal ans1 = new BigDecimal("-2957821389").add(t5);

            BigDecimal s1 = y1.multiply(BigDecimal.ONE);
            BigDecimal s2 = y1.multiply(new BigDecimal("226.1030244").add(s1));
            BigDecimal s3 = y1.multiply(new BigDecimal("47447.26470").add(s2));
            BigDecimal s4 = y1.multiply(new BigDecimal("7189466.438").add(s3));
            BigDecimal s5 = y1.multiply(new BigDecimal("745249964.8").add(s4));
            BigDecimal ans2 = new BigDecimal("40076544269").add(s5);

            return ans1.divide(ans2, java.math.MathContext.DECIMAL128).add(new BigDecimal("0.63661977236758134308").multiply(BesselJ0(x)).multiply(MathEx.Log(x)));
        }
        BigDecimal z = new BigDecimal("8").divide(x, java.math.MathContext.DECIMAL128);
        BigDecimal y2 = z.multiply(z);
        BigDecimal xx = x.subtract(new BigDecimal("0.78539816339744830962"));

        BigDecimal a1 = y2.multiply(new BigDecimal("0.0000002093887211"));
        BigDecimal a2 = y2.multiply(new BigDecimal("-0.000002073370639").add(a1));
        BigDecimal a3 = y2.multiply(new BigDecimal("0.00002734510407").add(a2));
        BigDecimal ans3 = BigDecimal.ONE.add(y2.multiply(new BigDecimal("-0.001098628627").add(a3)));

        BigDecimal b1 = y2.multiply(new BigDecimal("-0.0000000934935152"));
        BigDecimal b2 = y2.multiply(new BigDecimal("0.0000007621095161").add(b1));
        BigDecimal b3 = y2.multiply(new BigDecimal("-0.000006911147651").add(b2));
        BigDecimal ans4 = new BigDecimal("-0.01562499995").add(y2.multiply(new BigDecimal("0.0001430488765").add(b3)));

        return MathEx.Sqrt(new BigDecimal("0.63661977236758134308").divide(x, java.math.MathContext.DECIMAL128))
            .multiply(MathEx.Sin(xx).multiply(ans3).add(z.multiply(MathEx.Cos(xx)).multiply(ans4)));
    }

    private static BigDecimal BesselY1(BigDecimal x) {
        if (x.compareTo(new BigDecimal("8")) < 0) {
            BigDecimal y1 = x.multiply(x);
            BigDecimal t1 = y1.multiply(new BigDecimal("8511937.935"));
            BigDecimal t2 = y1.multiply(new BigDecimal("-4237922726").add(t1));
            BigDecimal t3 = y1.multiply(new BigDecimal("7349264551").add(t2));
            BigDecimal t4 = y1.multiply(new BigDecimal("-51534381390").add(t3));
            BigDecimal t5 = y1.multiply(new BigDecimal("1275274390000").add(t4));
            BigDecimal ans1 = x.multiply(new BigDecimal("-4900604943000").add(t5));

            BigDecimal s1 = y1.multiply(BigDecimal.ONE);
            BigDecimal s2 = y1.multiply(new BigDecimal("354963.2885").add(s1));
            BigDecimal s3 = y1.multiply(new BigDecimal("102042605").add(s2));
            BigDecimal s4 = y1.multiply(new BigDecimal("224590400.2").add(s3));
            BigDecimal s5 = y1.multiply(new BigDecimal("3733650367").add(s4));
            BigDecimal s6 = y1.multiply(new BigDecimal("424441966400").add(s5));
            BigDecimal ans2 = new BigDecimal("24995805700000").add(s6);

            return ans1.divide(ans2, java.math.MathContext.DECIMAL128)
                .add(new BigDecimal("0.63661977236758134308").multiply(BesselJ1(x).multiply(MathEx.Log(x)).subtract(BigDecimal.ONE.divide(x, java.math.MathContext.DECIMAL128))));
        }
        BigDecimal z = new BigDecimal("8").divide(x, java.math.MathContext.DECIMAL128);
        BigDecimal y2 = z.multiply(z);
        BigDecimal xx = x.subtract(new BigDecimal("2.35619449019234492885"));

        BigDecimal a1 = y2.multiply(new BigDecimal("-0.000000240337019"));
        BigDecimal a2 = y2.multiply(new BigDecimal("0.000002457520174").add(a1));
        BigDecimal a3 = y2.multiply(new BigDecimal("-0.00003516396496").add(a2));
        BigDecimal ans3 = BigDecimal.ONE.add(y2.multiply(new BigDecimal("0.00183105").add(a3)));

        BigDecimal b1 = y2.multiply(new BigDecimal("0.000000105787412"));
        BigDecimal b2 = y2.multiply(new BigDecimal("-0.00000088228987").add(b1));
        BigDecimal b3 = y2.multiply(new BigDecimal("0.000008449199096").add(b2));
        BigDecimal ans4 = new BigDecimal("0.04687499995").add(y2.multiply(new BigDecimal("-0.0002002690873").add(b3)));

        return MathEx.Sqrt(new BigDecimal("0.63661977236758134308").divide(x, java.math.MathContext.DECIMAL128))
            .multiply(MathEx.Sin(xx).multiply(ans3).add(z.multiply(MathEx.Cos(xx)).multiply(ans4)));
    }

    private static BigDecimal BesselJ0(BigDecimal x) {
        BigDecimal ax = x.abs();
        if (ax.compareTo(new BigDecimal("8")) < 0) {
            BigDecimal y1 = x.multiply(x);
            BigDecimal t1 = y1.multiply(new BigDecimal("-184.9052456"));
            BigDecimal t2 = y1.multiply(new BigDecimal("77392.33017").add(t1));
            BigDecimal t3 = y1.multiply(new BigDecimal("-11214424.18").add(t2));
            BigDecimal t4 = y1.multiply(new BigDecimal("651619640.7").add(t3));
            BigDecimal t5 = y1.multiply(new BigDecimal("-13362590354").add(t4));
            BigDecimal ans1 = new BigDecimal("57568490574").add(t5);

            BigDecimal s1 = y1.multiply(BigDecimal.ONE);
            BigDecimal s2 = y1.multiply(new BigDecimal("267.8532712").add(s1));
            BigDecimal s3 = y1.multiply(new BigDecimal("59272.64853").add(s2));
            BigDecimal s4 = y1.multiply(new BigDecimal("9494680.718").add(s3));
            BigDecimal s5 = y1.multiply(new BigDecimal("1029532985").add(s4));
            BigDecimal ans2 = new BigDecimal("57568490411").add(s5);

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
            BigDecimal t5 = y1.multiply(new BigDecimal("-7895059235").add(t4));
            BigDecimal ans1 = x.multiply(new BigDecimal("72362614232").add(t5));

            BigDecimal s1 = y1.multiply(BigDecimal.ONE);
            BigDecimal s2 = y1.multiply(new BigDecimal("376.9991397").add(s1));
            BigDecimal s3 = y1.multiply(new BigDecimal("99447.43394").add(s2));
            BigDecimal s4 = y1.multiply(new BigDecimal("18583304.74").add(s3));
            BigDecimal s5 = y1.multiply(new BigDecimal("2300535178").add(s4));
            BigDecimal ans2 = new BigDecimal("144725228442").add(s5);

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
