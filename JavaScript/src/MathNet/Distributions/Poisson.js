import { SpecialFunctions } from '../SpecialFunctions/SpecialFunctions.js';

class Poisson {
    /**
     * Computes the probability mass (PMF) at k, i.e. P(X = k).
     * @param {number} lambda - The lambda (λ) parameter of the Poisson distribution. Range: λ > 0.
     * @param {number} k - The location in the domain where we want to evaluate the probability mass function.
     * @returns {number} the probability mass at location k.
     */
    static PMF(lambda, k) {
        return Math.exp(-lambda + (k * Math.log(lambda)) - SpecialFunctions.FactorialLn(k));
    }

    /**
     * Computes the cumulative distribution (CDF) of the distribution at x, i.e. P(X ≤ x).
     * @param {number} lambda - The lambda (λ) parameter of the Poisson distribution. Range: λ > 0.
     * @param {number} x - The location at which to compute the cumulative distribution function.
     * @returns {number} the cumulative distribution at location x.
     */
    static CDF(lambda, x) {
        return 1 - SpecialFunctions.GammaLowerRegularized(x + 1, lambda);
    }
}

export { Poisson };