package toolgood.algorithm.mathNet.Distributions;

import toolgood.algorithm.mathNet.SpecialFunctions;

public class Weibull {
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
                       * Math.pow(x / scale, shape - 1.0)
                       * Math.exp(-Math.pow(x, shape) * Math.pow(scale, -shape))
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

            return -SpecialFunctions.ExponentialMinusOne(-Math.pow(x, shape) * Math.pow(scale, -shape));
        }
}