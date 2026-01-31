package toolgood.algorithm.mathNet.Distributions;

import toolgood.algorithm.mathNet.SpecialFunctions;

public class Poisson {
        /// <summary>
        /// Computes the probability mass (PMF) at k, i.e. P(X = k).
        /// </summary>
        /// <param name="k">The location in the domain where we want to Evaluate the probability mass function.</param>
        /// <param name="lambda">The lambda (λ) parameter of the Poisson distribution. Range: λ > 0.</param>
        /// <returns>the probability mass at location <paramref name="k"/>.</returns>
        public static double PMF(double lambda, int k)
        {
            //if (!(lambda > 0.0)) {
            //    throw new ArgumentException("InvalidDistributionParameters");
            //}

            return Math.exp(-lambda + (k * Math.log(lambda)) - SpecialFunctions.FactorialLn(k));
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
            //if (!(lambda > 0.0)) {
            //    throw new ArgumentException("InvalidDistributionParameters");
            //}

            return 1.0 - SpecialFunctions.GammaLowerRegularized(x + 1, lambda);
        }
}