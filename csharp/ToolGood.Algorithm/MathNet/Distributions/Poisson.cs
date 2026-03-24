using System;

namespace ToolGood.Algorithm.MathNet.Numerics.Distributions
{
    sealed class Poisson
    {
        public static decimal PMF(decimal lambda, int k)
        {
            return MathEx.Exp(-lambda + (k * MathEx.Log(lambda)) - SpecialFunctions.FactorialLn(k));
        }

        public static decimal CDF(decimal lambda, decimal x)
        {
            return 1.0m - SpecialFunctions.GammaLowerRegularized(x + 1, lambda);
        }
    }
}
