package toolgood.algorithm.internals.functions.mathSum2;

import java.math.BigDecimal;
import java.util.List;
import java.util.function.BiFunction;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.functions.NoneEngine;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_2;

public final class Function_BESSELI extends Function_2 {

    public Function_BESSELI(FunctionBase func1, FunctionBase func2) {
        super(func1, func2);
    }

    public Function_BESSELI(FunctionBase[] funcs) {
        super(funcs);
        if (funcs.length != 2) {
            throw new IllegalArgumentException("Function '" + Name() + "' requires exactly 2 parameters.");
        }
    }

    @Override
    public String Name() {
        return "BesselI";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = GetNumber_1(engine, tempParameter);
        if (args1.IsErrorOrNone()) {
            return args1;
        }
        Operand args2 = GetNumber_2(engine, tempParameter);
        if (args2.IsErrorOrNone()) {
            return args2;
        }

        double x = args1.NumberValue().doubleValue();
        int n = (int) args2.NumberValue().doubleValue();

        return Operand.Create(BigDecimal.valueOf(BesselI(n, x)));
    }

    private static double BesselI(int n, double x) {
        if (x < 0) {
            return (n % 2 == 0 ? 1 : -1) * BesselI(n, -x);
        }
        if (x == 0) {
            return (n == 0) ? 1.0 : 0.0;
        }

        double ax = Math.abs(x);
        if (ax < 1e-10) {
            return (n == 0) ? 1.0 : 0.0;
        }

        if (n < 0) n = -n;

        if (ax > 700) {
            double factor = Math.exp(ax) / Math.sqrt(2 * Math.PI * ax);
            return factor * (1.0 - (4.0 * n * n - 1.0) / (8.0 * ax));
        }

        if (n == 0) return BesselI0(x);
        if (n == 1) return BesselI1(x);

        double I0 = BesselI0(x);
        double I1 = BesselI1(x);
        double In = 0;

        for (int k = 1; k < n; k++) {
            In = I1 + 2.0 * k / x * I0;
            I0 = I1;
            I1 = In;
        }

        return I1;
    }

    private static double BesselI0(double x) {
        double ax = Math.abs(x);
        if (ax < 3.75) {
            double y1 = x / 3.75;
            y1 *= y1;
            return 1.0 + y1 * (3.515622965380465 + y1 * (3.089942465562116 + y1 * (1.206749160761352
                    + y1 * (0.265973256598487 + y1 * (0.036076845538912 + y1 * 0.004581327358717)))));
        }
        double y2 = 3.75 / ax;
        return (Math.exp(ax) / Math.sqrt(2.0 * Math.PI * ax)) * (0.398942280401433 + y2 * (0.013285921344730
                + y2 * (0.002253193626842 + y2 * (-0.001575649875251 + y2 * (0.009162816703917
                + y2 * (-0.020577062932649 + y2 * (0.026355373177924 + y2 * (-0.016476329612910 + y2 * 0.003923769605236))))))));
    }

    private static double BesselI1(double x) {
        double ax = Math.abs(x);
        if (ax < 3.75) {
            double y1 = x / 3.75;
            y1 *= y1;
            return x * (0.5 + y1 * (0.878905941521392 + y1 * (0.514988692842374 + y1 * (0.150849342225664
                    + y1 * (0.026587328231117 + y1 * (0.003015319414231 + y1 * 0.000324111013968))))));
        }
        double y2 = 3.75 / ax;
        double ans = (Math.exp(ax) / Math.sqrt(2.0 * Math.PI * ax)) * (0.398942280401433 + y2 * (-0.039880242337502
                + y2 * (-0.003620182649157 + y2 * (0.001638105403528 + y2 * (-0.010315550635288
                + y2 * (0.022829679456897 + y2 * (-0.028953129286367 + y2 * (0.017876545768998 - y2 * 0.004200596567986))))))));
        return (x < 0) ? -ans : ans;
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
