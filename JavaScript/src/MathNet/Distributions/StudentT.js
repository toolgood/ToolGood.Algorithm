import { Normal } from './Normal.js';
import { Brent } from '../RootFinding/Brent.js';
import { SpecialFunctions } from '../SpecialFunctions/SpecialFunctions.js';

class StudentT {
    /**
     * Student's t cumulative distribution function
     * @param {number} location
     * @param {number} scale
     * @param {number} freedom
     * @param {number} x
     * @returns {number}
     */
    static CDF(location, scale, freedom, x) {
        if (!isFinite(freedom)) {
            return Normal.CDF(location, scale, x);
        }

        const k = (x - location) / scale;
        const h = freedom / (freedom + (k * k));
        const ib = 0.5 * SpecialFunctions.BetaRegularized(freedom / 2, 0.5, h);
        return x <= location ? ib : 1 - ib;
    }

    /**
     * Student's t inverse cumulative distribution function
     * @param {number} location
     * @param {number} scale
     * @param {number} freedom
     * @param {number} p
     * @returns {number}
     */
    static InvCDF(location, scale, freedom, p) {
        if (!isFinite(freedom)) {
            return Normal.InvCDF(location, scale, p);
        }

        if (p === 0.5) {
            return location;
        }

        return Brent.FindRoot(x => {
            const k = (x - location) / scale;
            const h = freedom / (freedom + (k * k));
            const ib = 0.5 * SpecialFunctions.BetaRegularized(freedom / 2, 0.5, h);
            return x <= location ? ib - p : 1 - ib - p;
        }, -800, 800, 1e-12);
    }
}

export { StudentT };