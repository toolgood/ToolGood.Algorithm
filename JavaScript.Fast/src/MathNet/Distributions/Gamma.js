import { SpecialFunctions } from '../SpecialFunctions/SpecialFunctions.js';

class Gamma {
    /**
     * Gamma cumulative distribution function
     * @param {number} shape
     * @param {number} rate
     * @param {number} x
     * @returns {number}
     */
    static cdf(shape, rate, x) {
        if (!isFinite(rate)) {
            return x >= shape ? 1 : 0;
        }

        if (shape === 0 && rate === 0) {
            return 0;
        }

        return SpecialFunctions.gammaLowerRegularized(shape, x * rate);
    }

    /**
     * Gamma probability density function
     * @param {number} shape
     * @param {number} rate
     * @param {number} x
     * @returns {number}
     */
    static pdf(shape, rate, x) {
        if (!isFinite(rate)) {
            return x === shape ? Infinity : 0;
        }

        if (shape === 0 && rate === 0) {
            return 0;
        }

        if (shape === 1) {
            return rate * Math.exp(-rate * x);
        }

        if (shape > 160) {
            return Math.exp(Gamma.pdfLn(shape, rate, x));
        }

        return Math.pow(rate, shape) * Math.pow(x, shape - 1) * Math.exp(-rate * x) / SpecialFunctions.gamma(shape);
    }

    /**
     * Gamma probability density function logarithm
     * @param {number} shape
     * @param {number} rate
     * @param {number} x
     * @returns {number}
     */
    static pdfLn(shape, rate, x) {
        if (!isFinite(rate)) {
            return x === shape ? Infinity : -Infinity;
        }

        if (shape === 0 && rate === 0) {
            return -Infinity;
        }

        if (shape === 1) {
            return Math.log(rate) - (rate * x);
        }

        return (shape * Math.log(rate)) + ((shape - 1) * Math.log(x)) - (rate * x) - SpecialFunctions.gammaLn(shape);
    }

    /**
     * Gamma inverse cumulative distribution function
     * @param {number} shape
     * @param {number} rate
     * @param {number} p
     * @returns {number}
     */
    static invCdf(shape, rate, p) {
        return SpecialFunctions.gammaLowerRegularizedInv(shape, p) / rate;
    }
}

export { Gamma };