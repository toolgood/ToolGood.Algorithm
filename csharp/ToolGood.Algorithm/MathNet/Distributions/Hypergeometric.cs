using System;

namespace ToolGood.Algorithm.MathNet.Numerics.Distributions
{
    sealed class Hypergeometric
    {
        public static decimal PMF(int population, int success, int draws, int k)
        {
            if (k < 0 || k > draws || k > success || k < draws + success - population) {
                return 0m;
            }
            var logResult = SpecialFunctions.BinomialLn(success, k)
                         + SpecialFunctions.BinomialLn(population - success, draws - k)
                         - SpecialFunctions.BinomialLn(population, draws);
            return MathEx.Exp(logResult);
        }
    }
}
