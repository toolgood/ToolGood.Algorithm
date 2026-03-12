using System;

namespace ToolGood.Algorithm.MathNet.Numerics
{
    internal static class Evaluate
    {
        public static decimal Polynomial(decimal z, params decimal[] coefficients)
        {
            decimal sum = coefficients[coefficients.Length - 1];
            for (int i = coefficients.Length - 2; i >= 0; --i) {
                sum *= z;
                sum += coefficients[i];
            }

            return sum;
        }
    }
}
