/**
 * Continuous Univariate Exponential distribution.
 * The exponential distribution is a distribution over the real numbers parameterized by one non-negative parameter.
 * <a href="http://en.wikipedia.org/wiki/Exponential_distribution">Wikipedia - exponential distribution</a>.
 */
class Exponential {
    /**
     * Computes the probability density of the distribution (pdf) at x, i.e. ∂P(X ≤ x)/∂x.
     * @param {number} rate - The rate (λ) parameter of the distribution. Range: λ ≥ 0.
     * @param {number} x - The location at which to compute the density.
     * @returns {number} the density at x.
     */
    static pdf(rate, x) {
        return x < 0 ? 0 : rate * Math.exp(-rate * x);
    }

    /**
     * Computes the cumulative distribution (cdf) of the distribution at x, i.e. P(X ≤ x).
     * @param {number} rate - The rate (λ) parameter of the distribution. Range: λ ≥ 0.
     * @param {number} x - The location at which to compute the cumulative distribution function.
     * @returns {number} the cumulative distribution at location x.
     */
    static cdf(rate, x) {
        return x < 0 ? 0 : 1 - Math.exp(-rate * x);
    }
}

export { Exponential };