using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToolGood.Algorithm.MathNet.Numerics
{
    public partial  class SpecialFunctions
    {
        public static double ExponentialMinusOne(double power)
        {
            double x = Math.Abs(power);
            if (x > 0.1) {
                return Math.Exp(power) - 1.0;
            }

            if (x < x.PositiveEpsilonOf()) {
                return x;
            }

            // Series Expansion to x^k / k!
            int k = 0;
            double term = 1.0;
            return Evaluate.Series(
                () => {
                    k++;
                    term *= power;
                    term /= k;
                    return term;
                });
        }


    }
}
