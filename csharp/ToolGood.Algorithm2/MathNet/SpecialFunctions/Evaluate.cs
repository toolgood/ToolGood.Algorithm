using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToolGood.Algorithm.MathNet.Numerics
{
    public static class Evaluate
    {
        public static double Polynomial(double z, params double[] coefficients)
        {
            double sum = coefficients[coefficients.Length - 1];
            for (int i = coefficients.Length - 2; i >= 0; --i) {
                sum *= z;
                sum += coefficients[i];
            }

            return sum;
        }

        internal static double Series(Func<double> nextSummand)
        {
            double compensation = 0.0;
            double current;
            const double factor = 1 << 16;

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
            }
            while (Math.Abs(sum) < Math.Abs(factor * current));

            return sum;
        }



    }

}
