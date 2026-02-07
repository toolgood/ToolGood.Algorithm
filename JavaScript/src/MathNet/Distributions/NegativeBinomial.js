import { SpecialFunctions } from '../SpecialFunctions/SpecialFunctions.js';

class NegativeBinomial {
    /**
     * Computes the probability mass (pmf) at k, i.e. P(X = k).
     * @param {number} r - The number of failures (r) until the experiment stopped. Range: r ≥ 0.
     * @param {number} p - The probability (p) of a trial resulting in success. Range: 0 ≤ p ≤ 1.
     * @param {number} k - The location in the domain where we want to evaluate the probability mass function.
     * @returns {number} the probability mass at location k.
     */
    static pmf(r, p, k) {
        return Math.exp(NegativeBinomial.pmfLn(r, p, k));
    }

    /**
     * Computes the log probability mass (lnPMF) at k, i.e. ln(P(X = k)).
     * @param {number} r - The number of failures (r) until the experiment stopped. Range: r ≥ 0.
     * @param {number} p - The probability (p) of a trial resulting in success. Range: 0 ≤ p ≤ 1.
     * @param {number} k - The location in the domain where we want to evaluate the log probability mass function.
     * @returns {number} the log probability mass at location k.
     */
    static pmfLn(r, p, k) {
        return SpecialFunctions.gammaLn(r + k)
               - SpecialFunctions.gammaLn(r)
               - SpecialFunctions.gammaLn(k + 1)
               + (r * Math.log(p))
               + (k * Math.log(1 - p));
    }
}

export { NegativeBinomial };