import { SpecialFunctions } from '../SpecialFunctions/SpecialFunctions.js';

class Weibull {
    /**
     * Computes the probability density of the distribution (pdf) at x, i.e. ∂P(X ≤ x)/∂x.
     * @param {number} shape - The shape (k) of the Weibull distribution. Range: k > 0.
     * @param {number} scale - The scale (λ) of the Weibull distribution. Range: λ > 0.
     * @param {number} x - The location at which to compute the density.
     * @returns {number} the density at x.
     */
    static pdf(shape, scale, x) {
        if (x >= 0) {
            if (x === 0 && shape === 1) {
                return shape / scale;
            }

            return shape
                   * Math.pow(x / scale, shape - 1)
                   * Math.exp(-Math.pow(x, shape) * Math.pow(scale, -shape))
                   / scale;
        }

        return 0;
    }

    /**
     * Computes the cumulative distribution (cdf) of the distribution at x, i.e. P(X ≤ x).
     * @param {number} shape - The shape (k) of the Weibull distribution. Range: k > 0.
     * @param {number} scale - The scale (λ) of the Weibull distribution. Range: λ > 0.
     * @param {number} x - The location at which to compute the cumulative distribution function.
     * @returns {number} the cumulative distribution at location x.
     */
    static cdf(shape, scale, x) {
        if (x < 0) {
            return 0;
        }

        return -SpecialFunctions.exponentialMinusOne(-Math.pow(x, shape) * Math.pow(scale, -shape));
    }
}

export { Weibull };