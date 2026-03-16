using System;

namespace ToolGood.Algorithm.MathNet.Numerics
{
    internal partial class SpecialFunctions
    {
        public static decimal FactorialLn(int x)
        {
            if (x <= 1) {
                return 0m;
            }
            return GammaLn(x + 1);
        }

        public static decimal BinomialLn(int n, int k)
        {
            if (k < 0 || n < 0 || k > n) {
                return decimal.MinValue;
            }

            return FactorialLn(n) - FactorialLn(k) - FactorialLn(n - k);
        }
    }
}
