using System;

namespace ToolGood.Algorithm.MathNet.Numerics.Distributions
{
    /// <summary>
    /// Continuous Univariate Weibull distribution.
    /// For details about this distribution, see
    /// <a href="http://en.wikipedia.org/wiki/Weibull_distribution">Wikipedia - Weibull distribution</a>.
    /// </summary>
    /// <remarks>
    /// The Weibull distribution is parametrized by a shape and scale parameter.
    /// </remarks>
    public class Weibull
    {
        /// <summary>
        /// Computes the probability density of the distribution (PDF) at x, i.e. ∂P(X ≤ x)/∂x.
        /// </summary>
        /// <param name="shape">The shape (k) of the Weibull distribution. Range: k > 0.</param>
        /// <param name="scale">The scale (λ) of the Weibull distribution. Range: λ > 0.</param>
        /// <param name="x">The location at which to compute the density.</param>
        /// <returns>the density at <paramref name="x"/>.</returns>
        ///// <seealso cref="Density"/>
        public static double PDF(double shape, double scale, double x)
        {
            //if (shape <= 0.0 || scale <= 0.0) {
            //    throw new ArgumentException("InvalidDistributionParameters");
            //}

            if (x >= 0.0) {
                if (x == 0.0 && shape == 1.0) {
                    return shape / scale;
                }

                return shape
                       * Math.Pow(x / scale, shape - 1.0)
                       * Math.Exp(-Math.Pow(x, shape) * Math.Pow(scale, -shape))
                       / scale;
            }

            return 0.0;
        }
         

        /// <summary>
        /// Computes the cumulative distribution (CDF) of the distribution at x, i.e. P(X ≤ x).
        /// </summary>
        /// <param name="x">The location at which to compute the cumulative distribution function.</param>
        /// <param name="shape">The shape (k) of the Weibull distribution. Range: k > 0.</param>
        /// <param name="scale">The scale (λ) of the Weibull distribution. Range: λ > 0.</param>
        /// <returns>the cumulative distribution at location <paramref name="x"/>.</returns>
        ///// <seealso cref="CumulativeDistribution"/>
        public static double CDF(double shape, double scale, double x)
        {
            //if (shape <= 0.0 || scale <= 0.0) {
            //    throw new ArgumentException("InvalidDistributionParameters");
            //}

            if (x < 0.0) {
                return 0.0;
            }

            return -SpecialFunctions.ExponentialMinusOne(-Math.Pow(x, shape) * Math.Pow(scale, -shape));
        }

    }
}
