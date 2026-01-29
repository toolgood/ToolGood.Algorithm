import { Constants } from '../Constants.js';
import { SpecialFunctions } from '../SpecialFunctions/SpecialFunctions.js';

class LogNormal {
    /**
     * Log-Normal cumulative distribution function
     * @param {number} mu
     * @param {number} sigma
     * @param {number} x
     * @returns {number}
     */
    static CDF(mu, sigma, x) {
        return x < 0 ? 0
            : 0.5 * (1 + SpecialFunctions.Erf((Math.log(x) - mu) / (sigma * Constants.Sqrt2)));
    }

    /**
     * Log-Normal inverse cumulative distribution function
     * @param {number} mu
     * @param {number} sigma
     * @param {number} p
     * @returns {number}
     */
    static InvCDF(mu, sigma, p) {
        return p <= 0 ? 0 : p >= 1 ? Infinity
            : Math.exp(mu - sigma * Constants.Sqrt2 * SpecialFunctions.ErfcInv(2 * p));
    }
}

export { LogNormal };