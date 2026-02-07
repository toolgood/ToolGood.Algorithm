import { Constants } from '../Constants.js';
import { SpecialFunctions } from '../SpecialFunctions/SpecialFunctions.js';

class Normal {
    /**
     * Normal cumulative distribution function
     * @param {number} mean
     * @param {number} stddev
     * @param {number} x
     * @returns {number}
     */
    static cdf(mean, stddev, x) {
        return 0.5 * SpecialFunctions.erfc((mean - x) / (stddev * Constants.sqrt2));
    }

    /**
     * Normal inverse cumulative distribution function
     * @param {number} mean
     * @param {number} stddev
     * @param {number} p
     * @returns {number}
     */
    static invCDF(mean, stddev, p) {
        return mean - (stddev * Constants.sqrt2 * SpecialFunctions.erfcInv(2 * p));
    }

    /**
     * Normal probability density function
     * @param {number} mean
     * @param {number} stddev
     * @param {number} x
     * @returns {number}
     */
    static pdf(mean, stddev, x) {
        let d = (x - mean) / stddev;
        return Math.exp(-0.5 * d * d) / (Constants.sqrt2Pi * stddev);
    }
}

export { Normal };