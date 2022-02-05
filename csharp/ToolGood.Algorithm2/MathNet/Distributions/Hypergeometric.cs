namespace ToolGood.Algorithm.MathNet.Numerics.Distributions
{
    /// <summary>
    /// Discrete Univariate Hypergeometric distribution.
    /// This distribution is a discrete probability distribution that describes the number of successes in a sequence
    /// of n draws from a finite population without replacement, just as the binomial distribution
    /// describes the number of successes for draws with replacement
    /// <a href="http://en.wikipedia.org/wiki/Hypergeometric_distribution">Wikipedia - Hypergeometric distribution</a>.
    /// </summary>
    public class Hypergeometric
    {
        /// <summary>
        /// Computes the probability mass (PMF) at k, i.e. P(X = k).
        /// </summary>
        /// <param name="k">The location in the domain where we want to evaluate the probability mass function.</param>
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
}
