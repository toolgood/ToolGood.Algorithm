using System;

namespace ToolGood.Algorithm.MathNet.Numerics.Distributions
{
    sealed class NegativeBinomial
    {
        public static decimal PMF(decimal r, decimal p, int k)
        {
            return MathEx.Exp(PMFLn(r, p, k));
        }

        public static decimal PMFLn(decimal r, decimal p, int k)
        {
            return SpecialFunctions.GammaLn(r + k)
                   - SpecialFunctions.GammaLn(r)
                   - SpecialFunctions.GammaLn(k + 1.0m)
                   + (r * MathEx.Log(p))
                   + (k * MathEx.Log(1.0m - p));
        }
    }
}
