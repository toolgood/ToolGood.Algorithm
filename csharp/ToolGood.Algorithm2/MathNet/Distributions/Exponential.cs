﻿using System;

namespace ToolGood.Algorithm.MathNet.Numerics.Distributions
{
    /// <summary>
    /// Continuous Univariate Exponential distribution.
    /// The exponential distribution is a distribution over the real numbers parameterized by one non-negative parameter.
    /// <a href="http://en.wikipedia.org/wiki/Exponential_distribution">Wikipedia - exponential distribution</a>.
    /// </summary>
    public sealed class Exponential
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
            //if (rate < 0.0) {
            //    throw new ArgumentException("InvalidDistributionParameters");
            //}

            return x < 0.0 ? 0.0 : rate * Math.Exp(-rate * x);
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
            //if (rate < 0.0) {
            //    throw new ArgumentException("InvalidDistributionParameters");
            //}

            return x < 0.0 ? 0.0 : 1.0 - Math.Exp(-rate * x);
        }
    }
}