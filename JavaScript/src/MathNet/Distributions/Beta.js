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
    static cdf(a, b, x) {
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

        return SpecialFunctions.betaRegularized(a, b, x);
    }

    /**
     * Beta inverse cumulative distribution function
     * @param {number} a
     * @param {number} b
     * @param {number} p
     * @returns {number}
     */
    static invCdf(a, b, p) {
        // 使用二分法寻找根
        let left = 0;
        let right = 1;
        let mid;
        let tolerance = 1e-15;
        let maxIterations = 100;
        
        for (let i = 0; i < maxIterations; i++) {
            mid = (left + right) / 2;
            let value = SpecialFunctions.betaRegularized(a, b, mid) - p;
            
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

export { Beta };