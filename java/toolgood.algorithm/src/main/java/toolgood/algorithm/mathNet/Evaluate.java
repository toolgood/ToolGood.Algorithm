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