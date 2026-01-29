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
    static CDF(mean, stddev, x) {
        return 0.5 * SpecialFunctions.Erfc((mean - x) / (stddev * Constants.Sqrt2));
    }

    /**
     * Normal inverse cumulative distribution function
     * @param {number} mean
     * @param {number} stddev
     * @param {number} p
     * @returns {number}
     */
    static InvCDF(mean, stddev, p) {
        return mean - (stddev * Constants.Sqrt2 * SpecialFunctions.ErfcInv(2 * p));
    }

    /**
     * Normal probability density function
     * @param {number} mean
     * @param {number} stddev
     * @param {number} x
     * @returns {number}
     */
    static PDF(mean, stddev, x) {
        const d = (x - mean) / stddev;
        return Math.exp(-0.5 * d * d) / (Constants.Sqrt2Pi * stddev);
    }
}

export { Normal };