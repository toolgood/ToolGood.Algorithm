using System;

namespace ToolGood.Algorithm.MathNet.Numerics
{
    internal static class Evaluate
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
    }
}