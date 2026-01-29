import { Brent } from '../RootFinding/Brent.js';
import { SpecialFunctions } from '../SpecialFunctions/SpecialFunctions.js';

class Beta {
    /**
     * Beta cumulative distribution function
     * @param {number} a
     * @param {number} b
     * @param {number} x
     * @returns {number}
     */
    static CDF(a, b, x) {
        if (x < 0) {
            return 0;
        }

        if (x >= 1) {
            return 1;
        }

        if (!isFinite(a) && !isFinite(b)) {
            return x < 0.5 ? 0 : 1;
        }

        if (!isFinite(a)) {
            return x < 1 ? 0 : 1;
        }

        if (!isFinite(b)) {
            return x >= 0 ? 1 : 0;
        }

        if (a === 0 && b === 0) {
            if (x >= 0 && x < 1) {
                return 0.5;
            }
            return 1;
        }

        if (a === 0) {
            return 1;
        }

        if (b === 0) {
            return x >= 1 ? 1 : 0;
        }

        if (a === 1 && b === 1) {
            return x;
        }

        return SpecialFunctions.BetaRegularized(a, b, x);
    }

    /**
     * Beta inverse cumulative distribution function
     * @param {number} a
     * @param {number} b
     * @param {number} p
     * @returns {number}
     */
    static InvCDF(a, b, p) {
        return Brent.FindRoot(x => SpecialFunctions.BetaRegularized(a, b, x) - p, 0, 1, 1e-12);
    }
}

export { Beta };