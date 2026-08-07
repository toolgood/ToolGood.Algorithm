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

        // 移植自 MathNet.Numerics.SpecialFunctions.Evaluate.ChebyshevA
        // 系数按逆序存储,即零阶项在数组末尾
        public static decimal ChebyshevA(decimal[] coefficients, decimal x)
        {
            int p = 0;
            decimal b0 = coefficients[p++];
            decimal b1 = 0m;
            int i = coefficients.Length - 1;
            decimal b2;
            do {
                b2 = b1;
                b1 = b0;
                b0 = x * b1 - b2 + coefficients[p++];
            }
            while (--i > 0);

            return 0.5m * (b0 - b2);
        }
    }
}
