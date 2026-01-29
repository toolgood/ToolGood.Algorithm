import { Brent } from '../RootFinding/Brent.js';
import { SpecialFunctions } from '../SpecialFunctions/SpecialFunctions.js';

class FisherSnedecor {
    /**
     * Fisher-Snedecor cumulative distribution function
     * @param {number} d1
     * @param {number} d2
     * @param {number} x
     * @returns {number}
     */
    static CDF(d1, d2, x) {
        return SpecialFunctions.BetaRegularized(d1 / 2, d2 / 2, d1 * x / (d1 * x + d2));
    }

    /**
     * Fisher-Snedecor inverse cumulative distribution function
     * @param {number} d1
     * @param {number} d2
     * @param {number} p
     * @returns {number}
     */
    static InvCDF(d1, d2, p) {
        return Brent.FindRoot(
            x => SpecialFunctions.BetaRegularized(d1 / 2, d2 / 2, d1 * x / (d1 * x + d2)) - p,
            0, 1000, 1e-12
        );
    }
}

export { FisherSnedecor };