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

public final class Function_BESSELK extends Function_2 {
    public Function_BESSELK(FunctionBase func1, FunctionBase func2) {
        super(func1, func2);
    }

    public Function_BESSELK(FunctionBase[] funcs) {
        super(funcs);
    }

    @Override
    public String Name() {
        return "BesselK";
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

        return Operand.Create(BesselK(n, x));
    }

    private static BigDecimal BesselK(int n, BigDecimal x) {
        if (n < 0) n = -n;

        if (n == 0) return BesselK0(x);
        if (n == 1) return BesselK1(x);

        BigDecimal K0 = BesselK0(x);
        BigDecimal K1 = BesselK1(x);
        BigDecimal Kn = BigDecimal.ZERO;

        for (int k = 1; k < n; k++) {
            Kn = K1.add(new BigDecimal("2").multiply(new BigDecimal(k)).divide(x, java.math.MathContext.DECIMAL128).multiply(K0));
            K0 = K1;
            K1 = Kn;
        }

        return K1;
    }

    private static BigDecimal BesselK0(BigDecimal x) {
        if (x.compareTo(new BigDecimal("2")) <= 0) {
            BigDecimal y1 = x.multiply(x).divide(new BigDecimal("4"), java.math.MathContext.DECIMAL128);
            BigDecimal ans = MathEx.Log(x.divide(new BigDecimal("2"), java.math.MathContext.DECIMAL128)).negate().multiply(BesselI0(x)).add(new BigDecimal("-0.57721566490153286061")
                .add(y1.multiply(new BigDecimal("0.42278433509846713939").add(y1.multiply(new BigDecimal("0.230697567077446").add(y1.multiply(new BigDecimal("0.034885890266341")
                .add(y1.multiply(new BigDecimal("0.002626979711643").add(y1.multiply(new BigDecimal("0.000107502176243").add(y1.multiply(new BigDecimal("0.000007400456812"))))))))))));
            return ans;
        }
        BigDecimal y2 = new BigDecimal("2").divide(x, java.math.MathContext.DECIMAL128);
        BigDecimal ans2 = MathEx.Exp(x.negate()).divide(MathEx.Sqrt(x), java.math.MathContext.DECIMAL128).multiply(new BigDecimal("1.253314137315500")
            .add(y2.multiply(new BigDecimal("-0.078323582855262").add(y2.multiply(new BigDecimal("0.021895687854228").add(y2.multiply(new BigDecimal("-0.010624628097740")
            .add(y2.multiply(new BigDecimal("0.005878072214632").add(y2.multiply(new BigDecimal("-0.002515401617640").add(y2.multiply(new BigDecimal("0.000532080305632"))))))))))));
        return ans2;
    }

    private static BigDecimal BesselK1(BigDecimal x) {
        if (x.compareTo(new BigDecimal("2")) <= 0) {
            BigDecimal y1 = x.multiply(x).divide(new BigDecimal("4"), java.math.MathContext.DECIMAL128);
            BigDecimal ans = MathEx.Log(x.divide(new BigDecimal("2"), java.math.MathContext.DECIMAL128)).multiply(BesselI1(x)).add(BigDecimal.ONE.divide(x, java.math.MathContext.DECIMAL128).multiply(BigDecimal.ONE
                .add(y1.multiply(new BigDecimal("0.154431442036717").add(y1.multiply(new BigDecimal("-0.672785797513523").add(y1.multiply(new BigDecimal("-0.181568943578864")
                .add(y1.multiply(new BigDecimal("-0.019194020400716").add(y1.multiply(new BigDecimal("0.001104044918568").add(y1.multiply(new BigDecimal("0.000046862429868")))))))))))));
            return ans;
        }
        BigDecimal y2 = new BigDecimal("2").divide(x, java.math.MathContext.DECIMAL128);
        BigDecimal ans2 = MathEx.Exp(x.negate()).divide(MathEx.Sqrt(x), java.math.MathContext.DECIMAL128).multiply(new BigDecimal("1.253314137315500")
            .add(y2.multiply(new BigDecimal("0.234986192707248").add(y2.multiply(new BigDecimal("-0.036556202034020").add(y2.multiply(new BigDecimal("0.015042680553908")
            .add(y2.multiply(new BigDecimal("-0.007803534366237").add(y2.multiply(new BigDecimal("0.003256142832609").add(y2.multiply(new BigDecimal("-0.000682450383692"))))))))))));
        return ans2;
    }

    private static BigDecimal BesselI0(BigDecimal x) {
        BigDecimal ax = x.abs();
        if (ax.compareTo(new BigDecimal("3.75")) < 0) {
            BigDecimal y1 = x.divide(new BigDecimal("3.75"), java.math.MathContext.DECIMAL128);
            y1 = y1.multiply(y1);
            return BigDecimal.ONE.add(y1.multiply(new BigDecimal("3.515622965380465").add(y1.multiply(new BigDecimal("3.089942465562116").add(y1.multiply(new BigDecimal("1.206749160761352")
                .add(y1.multiply(new BigDecimal("0.265973256598487").add(y1.multiply(new BigDecimal("0.036076845538912").add(y1.multiply(new BigDecimal("0.004581327358717"))))))))))));
        }
        BigDecimal y2 = new BigDecimal("3.75").divide(ax, java.math.MathContext.DECIMAL128);
        return MathEx.Exp(ax).divide(MathEx.Sqrt(new BigDecimal("2").multiply(MathEx.PI).multiply(ax)), java.math.MathContext.DECIMAL128).multiply(new BigDecimal("0.398942280401433").add(y2.multiply(new BigDecimal("0.013285921344730")
            .add(y2.multiply(new BigDecimal("0.002253193626842").add(y2.multiply(new BigDecimal("-0.001575649875251").add(y2.multiply(new BigDecimal("0.009162816703917")
            .add(y2.multiply(new BigDecimal("-0.020577062932649").add(y2.multiply(new BigDecimal("0.026355373177924").add(y2.multiply(new BigDecimal("-0.016476329612910").add(y2.multiply(new BigDecimal("0.003923769605236"))))))))))))))));
    }

    private static BigDecimal BesselI1(BigDecimal x) {
        BigDecimal ax = x.abs();
        if (ax.compareTo(new BigDecimal("3.75")) < 0) {
            BigDecimal y1 = x.divide(new BigDecimal("3.75"), java.math.MathContext.DECIMAL128);
            y1 = y1.multiply(y1);
            return x.multiply(new BigDecimal("0.5").add(y1.multiply(new BigDecimal("0.878905941521392").add(y1.multiply(new BigDecimal("0.514988692842374").add(y1.multiply(new BigDecimal("0.150849342225664")
                .add(y1.multiply(new BigDecimal("0.026587328231117").add(y1.multiply(new BigDecimal("0.003015319414231").add(y1.multiply(new BigDecimal("0.000324111013968"))))))))))));
        }
        BigDecimal y2 = new BigDecimal("3.75").divide(ax, java.math.MathContext.DECIMAL128);
        BigDecimal ans = MathEx.Exp(ax).divide(MathEx.Sqrt(new BigDecimal("2").multiply(MathEx.PI).multiply(ax)), java.math.MathContext.DECIMAL128).multiply(new BigDecimal("0.398942280401433").add(y2.multiply(new BigDecimal("-0.039880242337502")
            .add(y2.multiply(new BigDecimal("-0.003620182649157").add(y2.multiply(new BigDecimal("0.001638105403528").add(y2.multiply(new BigDecimal("-0.010315550635288")
            .add(y2.multiply(new BigDecimal("0.022829679456897").add(y2.multiply(new BigDecimal("-0.028953129286367").add(y2.multiply(new BigDecimal("0.017876545768998").subtract(y2.multiply(new BigDecimal("0.004200596567986"))))))))))))))));
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
