using System;

namespace ToolGood.Algorithm.MathNet.Numerics.Distributions
{
    internal sealed class LogNormal
    {
        public static decimal CDF(decimal mu, decimal sigma, decimal x)
        {
            return x < 0.0m ? 0.0m
                : 0.5m * (1.0m + SpecialFunctions.Erf((MathEx.Log(x) - mu) / (sigma * Constants.Sqrt2)));
        }

        public static decimal InvCDF(decimal mu, decimal sigma, decimal p)
        {
            return p <= 0.0m ? 0.0m : p >= 1.0m ? 79228162514264337593543950335M
                : MathEx.Exp(mu - sigma * Constants.Sqrt2 * SpecialFunctions.ErfcInv(2.0m * p));
        }
    }
}
