package toolgood.algorithm.mathNet.Distributions;

import toolgood.algorithm.mathNet.SpecialFunctions;

public class Hypergeometric {
     /// <summary>
        /// Computes the probability mass (PMF) at k, i.e. P(X = k).
        /// </summary>
        /// <param name="k">The location in the domain where we want to Evaluate the probability mass function.</param>
        /// <param name="population">The size of the population (N).</param>
        /// <param name="success">The number successes within the population (K, M).</param>
        /// <param name="draws">The number of draws without replacement (n).</param>
        /// <returns>the probability mass at location <paramref name="k"/>.</returns>
        public static double PMF(int population, int success, int draws, int k)
        {
            //if (!(population >= 0 && success >= 0 && draws >= 0 && success <= population && draws <= population)) {
            //    throw new ArgumentException("InvalidDistributionParameters");
            //}

            return SpecialFunctions.Binomial(success, k) * SpecialFunctions.Binomial(population - success, draws - k) / SpecialFunctions.Binomial(population, draws);
        }

}