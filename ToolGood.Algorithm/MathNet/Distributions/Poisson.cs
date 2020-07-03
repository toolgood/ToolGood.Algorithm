using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToolGood.Algorithm.MathNet.Numerics.Distributions
{
    /// <summary>
    /// Discrete Univariate Poisson distribution.
    /// </summary>
    /// <remarks>
    /// <para>Distribution is described at <a href="http://en.wikipedia.org/wiki/Poisson_distribution"> Wikipedia - Poisson distribution</a>.</para>
    /// <para>Knuth's method is used to generate Poisson distributed random variables.</para>
    /// <para>f(x) = exp(-λ)*λ^x/x!;</para>
    /// </remarks>
    public class Poisson
    {
        /// <summary>
        /// Computes the probability mass (PMF) at k, i.e. P(X = k).
        /// </summary>
        /// <param name="k">The location in the domain where we want to evaluate the probability mass function.</param>
        /// <param name="lambda">The lambda (λ) parameter of the Poisson distribution. Range: λ > 0.</param>
        /// <returns>the probability mass at location <paramref name="k"/>.</returns>
        public static double PMF(double lambda, int k)
        {
            //if (!(lambda > 0.0)) {
            //    throw new ArgumentException("InvalidDistributionParameters");
            //}

            return Math.Exp(-lambda + (k * Math.Log(lambda)) - SpecialFunctions.FactorialLn(k));
        }

        /// <summary>
        /// Computes the cumulative distribution (CDF) of the distribution at x, i.e. P(X ≤ x).
        /// </summary>
        /// <param name="x">The location at which to compute the cumulative distribution function.</param>
        /// <param name="lambda">The lambda (λ) parameter of the Poisson distribution. Range: λ > 0.</param>
        /// <returns>the cumulative distribution at location <paramref name="x"/>.</returns>
        ///// <seealso cref="CumulativeDistribution"/>
        public static double CDF(double lambda, double x)
        {
            if (!(lambda > 0.0)) {
                throw new ArgumentException("InvalidDistributionParameters");
            }

            return 1.0 - SpecialFunctions.GammaLowerRegularized(x + 1, lambda);
        }


    }
}
