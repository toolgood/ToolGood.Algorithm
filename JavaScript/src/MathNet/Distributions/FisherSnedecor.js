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
    static cdf(d1, d2, x) {
        return SpecialFunctions.betaRegularized(d1 / 2, d2 / 2, d1 * x / (d1 * x + d2));
    }

    /**
     * Fisher-Snedecor inverse cumulative distribution function
     * @param {number} d1
     * @param {number} d2
     * @param {number} p
     * @returns {number}
     */
    static invCDF(d1, d2, p) {
        // 使用二分法寻找根
        let left = 0;
        let right = 1000;
        let mid;
        let tolerance = 1e-15;
        let maxIterations = 100;
        
        for (let i = 0; i < maxIterations; i++) {
            mid = (left + right) / 2;
            let value = SpecialFunctions.betaRegularized(d1 / 2, d2 / 2, d1 * mid / (d1 * mid + d2)) - p;
            
            if (Math.abs(value) < tolerance) {
                return mid;
            }
            
            if (value < 0) {
                left = mid;
            } else {
                right = mid;
            }
        }
        
        return mid;
    }
}

export { FisherSnedecor };