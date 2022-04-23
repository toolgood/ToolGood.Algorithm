using System;

namespace ToolGood.Algorithm.MathNet.Numerics.Distributions
{
    /// <summary>
    /// Continuous Univariate Log-Normal distribution.
    /// For details about this distribution, see
    /// <a href="http://en.wikipedia.org/wiki/Log-normal_distribution">Wikipedia - Log-Normal distribution</a>.
    /// </summary>
    class LogNormal
    {
        public static double CDF(double mu, double sigma, double x)
        {
            //if (sigma < 0.0) {
            //    throw new ArgumentException("InvalidDistributionParameters");
            //}

            return x < 0.0 ? 0.0
                : 0.5 * (1.0 + SpecialFunctions.Erf((Math.Log(x) - mu) / (sigma * Constants.Sqrt2)));
        }

        public static double InvCDF(double mu, double sigma, double p)
        {
            //if (sigma < 0.0) {
            //    throw new ArgumentException("InvalidDistributionParameters");
            //}

            return p <= 0.0 ? 0.0 : p >= 1.0 ? double.PositiveInfinity
                : Math.Exp(mu - sigma * Constants.Sqrt2 * SpecialFunctions.ErfcInv(2.0 * p));
        }
    }
}
