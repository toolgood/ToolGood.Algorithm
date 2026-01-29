import { SpecialFunctions } from '../SpecialFunctions/SpecialFunctions.js';

class Gamma {
    /**
     * Gamma cumulative distribution function
     * @param {number} shape
     * @param {number} rate
     * @param {number} x
     * @returns {number}
     */
    static CDF(shape, rate, x) {
        if (!isFinite(rate)) {
            return x >= shape ? 1 : 0;
        }

        if (shape === 0 && rate === 0) {
            return 0;
        }

        return SpecialFunctions.GammaLowerRegularized(shape, x * rate);
    }

    /**
     * Gamma probability density function
     * @param {number} shape
     * @param {number} rate
     * @param {number} x
     * @returns {number}
     */
    static PDF(shape, rate, x) {
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
            return Math.exp(Gamma.PDFLn(shape, rate, x));
        }

        return Math.pow(rate, shape) * Math.pow(x, shape - 1) * Math.exp(-rate * x) / SpecialFunctions.Gamma(shape);
    }

    /**
     * Gamma probability density function logarithm
     * @param {number} shape
     * @param {number} rate
     * @param {number} x
     * @returns {number}
     */
    static PDFLn(shape, rate, x) {
        if (!isFinite(rate)) {
            return x === shape ? Infinity : -Infinity;
        }

        if (shape === 0 && rate === 0) {
            return -Infinity;
        }

        if (shape === 1) {
            return Math.log(rate) - (rate * x);
        }

        return (shape * Math.log(rate)) + ((shape - 1) * Math.log(x)) - (rate * x) - SpecialFunctions.GammaLn(shape);
    }

    /**
     * Gamma inverse cumulative distribution function
     * @param {number} shape
     * @param {number} rate
     * @param {number} p
     * @returns {number}
     */
    static InvCDF(shape, rate, p) {
        return SpecialFunctions.GammaLowerRegularizedInv(shape, p) / rate;
    }
}

export { Gamma };