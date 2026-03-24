using System;

namespace ToolGood.Algorithm.MathNet.Numerics.Distributions
{
    sealed class Binomial
    {
        public static decimal PMF(decimal p, int n, int k)
        {
            if (k < 0 || k > n) {
                return 0.0m;
            }

            if (p == 0.0m) {
                return k == 0 ? 1.0m : 0.0m;
            }

            if (p == 1.0m) {
                return k == n ? 1.0m : 0.0m;
            }

            return MathEx.Exp(SpecialFunctions.BinomialLn(n, k) + (k * MathEx.Log(p)) + ((n - k) * MathEx.Log(1.0m - p)));
        }

        public static decimal CDF(decimal p, int n, decimal x)
        {
            if (x < 0.0m) {
                return 0.0m;
            }

            if (x > n) {
                return 1.0m;
            }

            decimal k = Math.Floor(x);
            return SpecialFunctions.BetaRegularized(n - k, k + 1, 1 - p);
        }
    }
}
