using System;

namespace ToolGood.Algorithm.MathNet.Numerics.Distributions
{
    /// <summary>
    /// Discrete Univariate Binomial distribution.
    /// For details about this distribution, see
    /// <a href="http://en.wikipedia.org/wiki/Binomial_distribution">Wikipedia - Binomial distribution</a>.
    /// </summary>
    /// <remarks>
    /// The distribution is parameterized by a probability (between 0.0 and 1.0).
    /// </remarks>
    public sealed class Binomial
    {
        /// <summary>
        /// Computes the probability mass (PMF) at k, i.e. P(X = k).
        /// </summary>
        /// <param name="k">The location in the domain where we want to evaluate the probability mass function.</param>
        /// <param name="p">The success probability (p) in each trial. Range: 0 ≤ p ≤ 1.</param>
        /// <param name="n">The number of trials (n). Range: n ≥ 0.</param>
        /// <returns>the probability mass at location <paramref name="k"/>.</returns>
        public static double PMF(double p, int n, int k)
        {
            //if (!(p >= 0.0 && p <= 1.0 && n >= 0)) {
            //    throw new ArgumentException("InvalidDistributionParameters");
            //}

            if (k < 0 || k > n) {
                return 0.0;
            }

            if (p == 0.0) {
                return k == 0 ? 1.0 : 0.0;
            }

            if (p == 1.0) {
                return k == n ? 1.0 : 0.0;
            }

            return Math.Exp(SpecialFunctions.BinomialLn(n, k) + (k * Math.Log(p)) + ((n - k) * Math.Log(1.0 - p)));
        }

        /// <summary>
        /// Computes the cumulative distribution (CDF) of the distribution at x, i.e. P(X ≤ x).
        /// </summary>
        /// <param name="x">The location at which to compute the cumulative distribution function.</param>
        /// <param name="p">The success probability (p) in each trial. Range: 0 ≤ p ≤ 1.</param>
        /// <param name="n">The number of trials (n). Range: n ≥ 0.</param>
        /// <returns>the cumulative distribution at location <paramref name="x"/>.</returns>
        ///// <seealso cref="CumulativeDistribution"/>
        public static double CDF(double p, int n, double x)
        {
            //if (!(p >= 0.0 && p <= 1.0 && n >= 0)) {
            //    throw new ArgumentException("InvalidDistributionParameters");
            //}

            if (x < 0.0) {
                return 0.0;
            }

            if (x > n) {
                return 1.0;
            }

            double k = Math.Floor(x);
            return SpecialFunctions.BetaRegularized(n - k, k + 1, 1 - p);
        }
    }
}