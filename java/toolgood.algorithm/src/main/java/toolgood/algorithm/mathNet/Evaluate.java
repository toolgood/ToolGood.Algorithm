package toolgood.algorithm.mathNet;

import java.util.function.Function;

public class Evaluate {
    public static double Polynomial(double z, double[] coefficients) {
        double sum = coefficients[coefficients.Length - 1];
        for (int i = coefficients.Length - 2; i >= 0; --i) {
            sum *= z;
            sum += coefficients[i];
        }

        return sum;
    }

    public static double Series(Function<Double> nextSummand) {
        double compensation = 0.0;
        double current;
        double factor = 1 << 16;

        double sum = nextSummand();

        do {
            // Kahan Summation
            // NOTE (ruegg): do NOT optimize. Now, how to tell that the compiler?
            current = nextSummand();
            double y = current - compensation;
            double t = sum + y;
            compensation = t - sum;
            compensation -= y;
            sum = t;
        } while (Math.abs(sum) < Math.abs(factor * current));

        return sum;
    }

}