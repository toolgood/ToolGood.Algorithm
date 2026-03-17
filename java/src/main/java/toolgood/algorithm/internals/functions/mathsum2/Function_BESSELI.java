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

public final class Function_BESSELI extends Function_2 {
    public Function_BESSELI(FunctionBase func1, FunctionBase func2) {
        super(func1, func2);
    }

    public Function_BESSELI(FunctionBase[] funcs) {
        super(funcs);
    }

    @Override
    public String Name() {
        return "BesselI";
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

        return Operand.Create(BesselI(n, x));
    }

    private static BigDecimal BesselI(int n, BigDecimal x) {
        if (x.compareTo(BigDecimal.ZERO) < 0) {
            return BesselI(n, x.negate()).multiply(n % 2 == 0 ? BigDecimal.ONE : BigDecimal.ONE.negate());
        }
        if (x.compareTo(BigDecimal.ZERO) == 0) {
            return (n == 0) ? BigDecimal.ONE : BigDecimal.ZERO;
        }

        BigDecimal ax = x.abs();
        if (ax.compareTo(new BigDecimal("1e-10")) < 0) {
            return (n == 0) ? BigDecimal.ONE : BigDecimal.ZERO;
        }

        if (n < 0) n = -n;

        if (ax.compareTo(new BigDecimal("700")) > 0) {
            BigDecimal factor = MathEx.Exp(ax).divide(MathEx.Sqrt(new BigDecimal("2").multiply(MathEx.PI).multiply(ax)), java.math.MathContext.DECIMAL128);
            return factor.multiply(BigDecimal.ONE.subtract(new BigDecimal("4").multiply(new BigDecimal(n * n)).subtract(BigDecimal.ONE).divide(new BigDecimal("8").multiply(ax), java.math.MathContext.DECIMAL128)));
        }

        if (n == 0) return BesselI0(x);
        if (n == 1) return BesselI1(x);

        BigDecimal I0 = BesselI0(x);
        BigDecimal I1 = BesselI1(x);
        BigDecimal In = BigDecimal.ZERO;

        for (int k = 1; k < n; k++) {
            In = I1.add(new BigDecimal("2").multiply(new BigDecimal(k)).divide(x, java.math.MathContext.DECIMAL128).multiply(I0));
            I0 = I1;
            I1 = In;
        }

        return I1;
    }

    private static BigDecimal BesselI0(BigDecimal x) {
        BigDecimal ax = x.abs();
        if (ax.compareTo(new BigDecimal("3.75")) < 0) {
            BigDecimal y1 = x.divide(new BigDecimal("3.75"), java.math.MathContext.DECIMAL128);
            y1 = y1.multiply(y1);
            BigDecimal t1 = y1.multiply(new BigDecimal("0.004581327358717"));
            BigDecimal t2 = y1.multiply(new BigDecimal("0.036076845538912").add(t1));
            BigDecimal t3 = y1.multiply(new BigDecimal("0.265973256598487").add(t2));
            BigDecimal t4 = y1.multiply(new BigDecimal("1.206749160761352").add(t3));
            BigDecimal t5 = y1.multiply(new BigDecimal("3.089942465562116").add(t4));
            return BigDecimal.ONE.add(y1.multiply(new BigDecimal("3.515622965380465").add(t5)));
        }
        BigDecimal y2 = new BigDecimal("3.75").divide(ax, java.math.MathContext.DECIMAL128);

        BigDecimal a1 = y2.multiply(new BigDecimal("0.003923769605236"));
        BigDecimal a2 = y2.multiply(new BigDecimal("-0.016476329612910").add(a1));
        BigDecimal a3 = y2.multiply(new BigDecimal("0.026355373177924").add(a2));
        BigDecimal a4 = y2.multiply(new BigDecimal("-0.020577062932649").add(a3));
        BigDecimal a5 = y2.multiply(new BigDecimal("0.009162816703917").add(a4));
        BigDecimal a6 = y2.multiply(new BigDecimal("-0.001575649875251").add(a5));
        BigDecimal a7 = y2.multiply(new BigDecimal("0.002253193626842").add(a6));
        BigDecimal a8 = y2.multiply(new BigDecimal("0.013285921344730").add(a7));
        BigDecimal a9 = new BigDecimal("0.398942280401433").add(a8);

        return MathEx.Exp(ax).divide(MathEx.Sqrt(new BigDecimal("2").multiply(MathEx.PI).multiply(ax)), java.math.MathContext.DECIMAL128).multiply(a9);
    }

    private static BigDecimal BesselI1(BigDecimal x) {
        BigDecimal ax = x.abs();
        if (ax.compareTo(new BigDecimal("3.75")) < 0) {
            BigDecimal y1 = x.divide(new BigDecimal("3.75"), java.math.MathContext.DECIMAL128);
            y1 = y1.multiply(y1);
            BigDecimal t1 = y1.multiply(new BigDecimal("0.000324111013968"));
            BigDecimal t2 = y1.multiply(new BigDecimal("0.003015319414231").add(t1));
            BigDecimal t3 = y1.multiply(new BigDecimal("0.026587328231117").add(t2));
            BigDecimal t4 = y1.multiply(new BigDecimal("0.150849342225664").add(t3));
            BigDecimal t5 = y1.multiply(new BigDecimal("0.514988692842374").add(t4));
            BigDecimal t6 = y1.multiply(new BigDecimal("0.878905941521392").add(t5));
            return x.multiply(new BigDecimal("0.5").add(t6));
        }
        BigDecimal y2 = new BigDecimal("3.75").divide(ax, java.math.MathContext.DECIMAL128);

        BigDecimal a1 = y2.multiply(new BigDecimal("0.004200596567986"));
        BigDecimal a2 = y2.multiply(new BigDecimal("0.017876545768998").subtract(a1));
        BigDecimal a3 = y2.multiply(new BigDecimal("-0.028953129286367").add(a2));
        BigDecimal a4 = y2.multiply(new BigDecimal("0.022829679456897").add(a3));
        BigDecimal a5 = y2.multiply(new BigDecimal("-0.010315550635288").add(a4));
        BigDecimal a6 = y2.multiply(new BigDecimal("0.001638105403528").add(a5));
        BigDecimal a7 = y2.multiply(new BigDecimal("-0.003620182649157").add(a6));
        BigDecimal a8 = y2.multiply(new BigDecimal("-0.039880242337502").add(a7));
        BigDecimal a9 = new BigDecimal("0.398942280401433").add(a8);

        BigDecimal ans = MathEx.Exp(ax).divide(MathEx.Sqrt(new BigDecimal("2").multiply(MathEx.PI).multiply(ax)), java.math.MathContext.DECIMAL128).multiply(a9);
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
