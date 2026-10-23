/**
 * Discrete Univariate binomial distribution.
 * For details about this distribution, see
 * <a href="http://en.wikipedia.org/wiki/Binomial_distribution">Wikipedia - binomial distribution</a>.
 * 
 * The distribution is parameterized by a probability (between 0.0 and 1.0).
 */
import { SpecialFunctions } from '../SpecialFunctions/SpecialFunctions.js';

class binomial {
    /**
     * Computes the probability mass (pmf) at k, i.e. P(X = k).
     * @param {number} p - The success probability (p) in each trial. Range: 0 ≤ p ≤ 1.
     * @param {number} n - The number of trials (n). Range: n ≥ 0.
     * @param {number} k - The location in the domain where we want to evaluate the probability mass function.
     * @returns {number} the probability mass at location k.
     */
    static pmf(p, n, k) {
        if (k < 0 || k > n) {
            return 0;
        }

        if (p === 0) {
            return k === 0 ? 1 : 0;
        }

        if (p === 1) {
            return k === n ? 1 : 0;
        }

        return Math.exp(SpecialFunctions.binomialLn(n, k) + (k * Math.log(p)) + ((n - k) * Math.log(1 - p)));
    }

    /**
     * Computes the cumulative distribution (cdf) of the distribution at x, i.e. P(X ≤ x).
     * @param {number} p - The success probability (p) in each trial. Range: 0 ≤ p ≤ 1.
     * @param {number} n - The number of trials (n). Range: n ≥ 0.
     * @param {number} x - The location at which to compute the cumulative distribution function.
     * @returns {number} the cumulative distribution at location x.
     */
    static cdf(p, n, x) {
        if (x < 0) {
            return 0;
        }

        if (x > n) {
            return 1;
        }

        let k = Math.floor(x);
        return SpecialFunctions.betaRegularized(n - k, k + 1, 1 - p);
    }
}

export { binomial };