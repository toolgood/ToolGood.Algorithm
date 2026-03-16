package toolgood.algorithm.internals.functions.mathsum2;

import java.util.List;
import java.util.function.BiFunction;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_2;
import toolgood.algorithm.internals.functions.NoneEngine;

public class Function_BESSELK extends Function_2 {
    public Function_BESSELK(FunctionBase func1, FunctionBase func2) {
        super(func1, func2);
    }

    @Override
    public String Name() {
        return "BesselK";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = GetNumber_1(engine, tempParameter);
        if (args1.IsError()) return args1;

        Operand args2 = GetNumber_2(engine, tempParameter);
        if (args2.IsError()) return args2;

        double x = args1.DoubleValue();
        int n = (int) args2.DoubleValue();

        if (x <= 0) {
            return ParameterError(1);
        }

        return Operand.Create(besselK(n, x));
    }

    private static double besselK(int n, double x) {
        if (n < 0) n = -n;

        if (n == 0) return besselK0(x);
        if (n == 1) return besselK1(x);

        double K0 = besselK0(x);
        double K1 = besselK1(x);
        double Kn = 0.0;
        for (int k = 1; k < n; k++) {
            Kn = K1 + 2.0 * k / x * K0;
            K0 = K1;
            K1 = Kn;
        }
        return K1;
    }

    private static double besselK0(double x) {
        if (x <= 2.0) {
            double y1 = x * x / 4.0;
            return -Math.log(x / 2.0) * besselI0(x) + (-0.5772156649015329
                    + y1 * (0.4227843350984671 + y1 * (0.230697567077446 + y1 * (0.034885890266341
                    + y1 * (0.002626979711643 + y1 * (0.000107502176243 + y1 * 0.000007400456812))))));
        }
        double y2 = 2.0 / x;
        return Math.exp(-x) / Math.sqrt(x) * (1.2533141373155
                + y2 * (-0.07832358285526 + y2 * (0.021895687854228 + y2 * (-0.01062462809774
                + y2 * (0.005878072214632 + y2 * (-0.00251540161764 + y2 * 0.000532080305632))))));
    }

    private static double besselK1(double x) {
        if (x <= 2.0) {
            double y1 = x * x / 4.0;
            return Math.log(x / 2.0) * besselI1(x) + (1.0 / x) * (1.0
                    + y1 * (0.154431442036717 + y1 * (-0.672785797513523 + y1 * (-0.181568943578864
                    + y1 * (-0.019194020400716 + y1 * (0.001104044918568 + y1 * 0.000046862429868))))));
        }
        double y2 = 2.0 / x;
        return Math.exp(-x) / Math.sqrt(x) * (1.2533141373155
                + y2 * (0.234986192707248 + y2 * (-0.03655620203402 + y2 * (0.015042680553908
                + y2 * (-0.007803534366237 + y2 * (0.003256142832609 + y2 * (-0.000682450383692)))))));
    }

    private static double besselI0(double x) {
        double ax = Math.abs(x);
        if (ax < 3.75) {
            double y1 = x / 3.75;
            y1 *= y1;
            return 1.0 + y1 * (3.515622965380465 + y1 * (3.089942465562116 + y1 * (1.206749160761352
                    + y1 * (0.265973256598487 + y1 * (0.036076845538912 + y1 * 0.004581327358717)))));
        }
        double y2 = 3.75 / ax;
        return (Math.exp(ax) / Math.sqrt(2.0 * Math.PI * ax)) * (0.398942280401433 + y2 * (0.01328592134473
                + y2 * (0.002253193626842 + y2 * (-0.001575649875251 + y2 * (0.009162816703917
                + y2 * (-0.020577062932649 + y2 * (0.026355373177924 + y2 * (-0.01647632961291 + y2 * 0.003923769605236))))))));
    }

    private static double besselI1(double x) {
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
        func1.GetParameterTypes(noneEngine, result, OperandType.NUMBER, null, null);
        func2.GetParameterTypes(noneEngine, result, OperandType.NUMBER, null, null);
    }
}
