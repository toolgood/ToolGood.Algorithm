package toolgood.algorithm.mathNet;


public class Evaluate {
    public static double Polynomial(double z, double[] coefficients) {
        double sum = coefficients[coefficients.length - 1];
        for (int i = coefficients.length - 2; i >= 0; --i) {
            sum *= z;
            sum += coefficients[i];
        }

        return sum;
    }

    // 移植自 MathNet.Numerics.SpecialFunctions.Evaluate.ChebyshevA
    // 系数按逆序存储,即零阶项在数组末尾
    public static double ChebyshevA(double[] coefficients, double x) {
        int p = 0;
        double b0 = coefficients[p++];
        double b1 = 0.0;
        int i = coefficients.length - 1;
        double b2;
        do {
            b2 = b1;
            b1 = b0;
            b0 = x * b1 - b2 + coefficients[p++];
        }
        while (--i > 0);

        return 0.5 * (b0 - b2);
    }

    // public static double Series(Function<Double, Double> f) {
    //     double compensation = 0.0;
    //     double current;
    //     double factor = 1 << 16;

    //     double sum = f.apply(0.0);

    //     do {
    //         // Kahan Summation
    //         // NOTE (ruegg): do NOT optimize. Now, how to tell that the compiler?
    //         current = f.apply(0.0);
    //         double y = current - compensation;
    //         double t = sum + y;
    //         compensation = t - sum;
    //         compensation -= y;
    //         sum = t;
    //     } while (Math.abs(sum) < Math.abs(factor * current));

    //     return sum;
    // }

}