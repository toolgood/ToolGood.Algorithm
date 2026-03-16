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

public class Function_BESSELY extends Function_2 {
    public Function_BESSELY(FunctionBase func1, FunctionBase func2) {
        super(func1, func2);
    }

    @Override
    public String Name() {
        return "BesselY";
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

        return Operand.Create(besselY(n, x));
    }

    private static double besselY(int n, double x) {
        if (n < 0) n = -n;

        if (n == 0) return besselY0(x);
        if (n == 1) return besselY1(x);

        double Y0 = besselY0(x);
        double Y1 = besselY1(x);
        double Yn = 0.0;
        for (int k = 1; k < n; k++) {
            Yn = (2.0 * k / x) * Y1 - Y0;
            Y0 = Y1;
            Y1 = Yn;
        }
        return Y1;
    }

    private static double besselY0(double x) {
        if (x < 8.0) {
            double y1 = x * x;
            double ans1 = -2957821389.0 + y1 * (7062834065.0 + y1 * (-512359803.6
                    + y1 * (10879881.29 + y1 * (-86327.92757 + y1 * 228.4622733))));
            double ans2 = 40076544269.0 + y1 * (745249964.8 + y1 * (7189466.438
                    + y1 * (47447.2647 + y1 * (226.1030244 + y1 * 1.0))));
            return (ans1 / ans2) + 0.6366197723675814 * besselJ0(x) * Math.log(x);
        }
        double z = 8.0 / x;
        double y2 = z * z;
        double xx = x - 0.7853981633974483;
        double ans3 = 1.0 + y2 * (-0.1098628627e-2 + y2 * (0.2734510407e-4
                + y2 * (-0.2073370639e-5 + y2 * 0.2093887211e-6)));
        double ans4 = -0.1562499995e-1 + y2 * (0.1430488765e-3
                + y2 * (-0.6911147651e-5 + y2 * (0.7621095161e-6
                + y2 * (-0.934935152e-7))));
        return Math.sqrt(0.6366197723675814 / x) * (Math.sin(xx) * ans3 + z * Math.cos(xx) * ans4);
    }

    private static double besselY1(double x) {
        if (x < 8.0) {
            double y1 = x * x;
            double ans1 = x * (-0.4900604943e13 + y1 * (0.127527439e13
                    + y1 * (-0.5153438139e11 + y1 * (0.7349264551e9
                    + y1 * (-0.4237922726e7 + y1 * 0.8511937935e4)))));
            double ans2 = 0.249958057e14 + y1 * (0.4244419664e12
                    + y1 * (0.3733650367e10 + y1 * (0.2245904002e8
                    + y1 * (0.1020426050e6 + y1 * (0.3549632885e3 + y1)))));
            return (ans1 / ans2) + 0.6366197723675814 * (besselJ1(x) * Math.log(x) - 1.0 / x);
        }
        double z = 8.0 / x;
        double y2 = z * z;
        double xx = x - 2.356194490192345;
        double ans3 = 1.0 + y2 * (0.183105e-2 + y2 * (-0.3516396496e-4
                + y2 * (0.2457520174e-5 + y2 * (-0.240337019e-6))));
        double ans4 = 0.04687499995 + y2 * (-0.2002690873e-3
                + y2 * (0.8449199096e-5 + y2 * (-0.88228987e-6
                + y2 * 0.105787412e-6)));
        return Math.sqrt(0.6366197723675814 / x) * (Math.sin(xx) * ans3 + z * Math.cos(xx) * ans4);
    }

    private static double besselJ0(double x) {
        double ax = Math.abs(x);
        if (ax < 8.0) {
            double y1 = x * x;
            double ans1 = 57568490574.0 + y1 * (-13362590354.0 + y1 * (651619640.7
                    + y1 * (-11214424.18 + y1 * (77392.33017 + y1 * (-184.9052456)))));
            double ans2 = 57568490411.0 + y1 * (1029532985.0 + y1 * (9494680.718
                    + y1 * (59272.64853 + y1 * (267.8532712 + y1 * 1.0))));
            return ans1 / ans2;
        }
        double z = 8.0 / ax;
        double y2 = z * z;
        double xx = ax - 0.7853981633974483;
        double ans3 = 1.0 + y2 * (-0.1098628627e-2 + y2 * (0.2734510407e-4
                + y2 * (-0.2073370639e-5 + y2 * 0.2093887211e-6)));
        double ans4 = -0.1562499995e-1 + y2 * (0.1430488765e-3
                + y2 * (-0.6911147651e-5 + y2 * (0.7621095161e-6
                - y2 * 0.934935152e-7)));
        return Math.sqrt(0.6366197723675814 / ax) * (Math.cos(xx) * ans3 - z * Math.sin(xx) * ans4);
    }

    private static double besselJ1(double x) {
        double ax = Math.abs(x);
        if (ax < 8.0) {
            double y1 = x * x;
            double ans1 = x * (72362614232.0 + y1 * (-7895059235.0 + y1 * (242396853.1
                    + y1 * (-2972611.439 + y1 * (15704.4826 + y1 * (-30.16036606))))));
            double ans2 = 144725228442.0 + y1 * (2300535178.0 + y1 * (18583304.74
                    + y1 * (99447.43394 + y1 * (376.9991397 + y1 * 1.0))));
            return ans1 / ans2;
        }
        double z = 8.0 / ax;
        double y2 = z * z;
        double xx = ax - 2.356194490192345;
        double ans3 = 1.0 + y2 * (0.183105e-2 + y2 * (-0.3516396496e-4
                + y2 * (0.2457520174e-5 + y2 * (-0.240337019e-6))));
        double ans4 = 0.04687499995 + y2 * (-0.2002690873e-3
                + y2 * (0.8449199096e-5 + y2 * (-0.88228987e-6
                + y2 * 0.105787412e-6)));
        double ans = Math.sqrt(0.6366197723675814 / ax) * (Math.cos(xx) * ans3 - z * Math.sin(xx) * ans4);
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
