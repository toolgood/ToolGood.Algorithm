package toolgood.algorithm.mathNet.Distributions;

import toolgood.algorithm.mathNet.SpecialFunctions;

public class NegativeBinomial {
      /// <summary>
        /// Computes the probability mass (PMF) at k, i.e. P(X = k).
        /// </summary>
        /// <param name="k">The location in the domain where we want to Evaluate the probability mass function.</param>
        /// <param name="r">The number of failures (r) until the experiment stopped. Range: r ≥ 0.</param>
        /// <param name="p">The probability (p) of a trial resulting in success. Range: 0 ≤ p ≤ 1.</param>
        /// <returns>the probability mass at location <paramref name="k"/>.</returns>
        public static double PMF(double r, double p, int k)
        {
            return Math.exp(PMFLn(r, p, k));
        }

        /// <summary>
        /// Computes the log probability mass (lnPMF) at k, i.e. ln(P(X = k)).
        /// </summary>
        /// <param name="k">The location in the domain where we want to Evaluate the log probability mass function.</param>
        /// <param name="r">The number of failures (r) until the experiment stopped. Range: r ≥ 0.</param>
        /// <param name="p">The probability (p) of a trial resulting in success. Range: 0 ≤ p ≤ 1.</param>
        /// <returns>the log probability mass at location <paramref name="k"/>.</returns>
        public static double PMFLn(double r, double p, int k)
        {
            //if (!(r >= 0.0 && p >= 0.0 && p <= 1.0)) {
            //    throw new ArgumentException("InvalidDistributionParameters");
            //}

            return SpecialFunctions.GammaLn(r + k)
                   - SpecialFunctions.GammaLn(r)
                   - SpecialFunctions.GammaLn(k + 1.0)
                   + (r * Math.log(p))
                   + (k * Math.log(1.0 - p));
        }

}