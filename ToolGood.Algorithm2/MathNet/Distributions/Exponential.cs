using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToolGood.Algorithm.MathNet.Numerics.Distributions
{
    /// <summary>
    /// Continuous Univariate Exponential distribution.
    /// The exponential distribution is a distribution over the real numbers parameterized by one non-negative parameter.
    /// <a href="http://en.wikipedia.org/wiki/Exponential_distribution">Wikipedia - exponential distribution</a>.
    /// </summary>
    public class Exponential
    {
        /// <summary>
        /// Computes the probability density of the distribution (PDF) at x, i.e. ∂P(X ≤ x)/∂x.
        /// </summary>
        /// <param name="rate">The rate (λ) parameter of the distribution. Range: λ ≥ 0.</param>
        /// <param name="x">The location at which to compute the density.</param>
        /// <returns>the density at <paramref name="x"/>.</returns>
        ///// <seealso cref="Density"/>
        public static double PDF(double rate, double x)
        {
            if (rate < 0.0) {
                throw new ArgumentException("InvalidDistributionParameters");
            }

            return x < 0.0 ? 0.0 : rate * Math.Exp(-rate * x);
        }

        /// <summary>
        /// Computes the log probability density of the distribution (lnPDF) at x, i.e. ln(∂P(X ≤ x)/∂x).
        /// </summary>
        /// <param name="rate">The rate (λ) parameter of the distribution. Range: λ ≥ 0.</param>
        /// <param name="x">The location at which to compute the density.</param>
        /// <returns>the log density at <paramref name="x"/>.</returns>
        ///// <seealso cref="DensityLn"/>
        public static double PDFLn(double rate, double x)
        {
            if (rate < 0.0) {
                throw new ArgumentException("InvalidDistributionParameters");
            }

            return Math.Log(rate) - (rate * x);
        }

        /// <summary>
        /// Computes the cumulative distribution (CDF) of the distribution at x, i.e. P(X ≤ x).
        /// </summary>
        /// <param name="x">The location at which to compute the cumulative distribution function.</param>
        /// <param name="rate">The rate (λ) parameter of the distribution. Range: λ ≥ 0.</param>
        /// <returns>the cumulative distribution at location <paramref name="x"/>.</returns>
        ///// <seealso cref="CumulativeDistribution"/>
        public static double CDF(double rate, double x)
        {
            if (rate < 0.0) {
                throw new ArgumentException("InvalidDistributionParameters");
            }

            return x < 0.0 ? 0.0 : 1.0 - Math.Exp(-rate * x);
        }

        /// <summary>
        /// Computes the inverse of the cumulative distribution function (InvCDF) for the distribution
        /// at the given probability. This is also known as the quantile or percent point function.
        /// </summary>
        /// <param name="p">The location at which to compute the inverse cumulative density.</param>
        /// <param name="rate">The rate (λ) parameter of the distribution. Range: λ ≥ 0.</param>
        /// <returns>the inverse cumulative density at <paramref name="p"/>.</returns>
        ///// <seealso cref="InverseCumulativeDistribution"/>
        public static double InvCDF(double rate, double p)
        {
            if (rate < 0.0) {
                throw new ArgumentException("InvalidDistributionParameters");
            }

            return p >= 1.0 ? double.PositiveInfinity : -Math.Log(1 - p) / rate;
        }

    }
}
