import { Precision } from '../Precision.js';

class Brent {
    /**
     * Finds a root of the function using the Brent-Dekker method.
     * @param {Function} f - The function to find the root of.
     * @param {number} lowerBound - The lower bound of the interval containing the root.
     * @param {number} upperBound - The upper bound of the interval containing the root.
     * @param {number} accuracy - The desired accuracy of the root.
     * @param {number} maxIterations - The maximum number of iterations to perform.
     * @returns {number} The root of the function.
     * @throws {Error} If no root is found within the given iterations.
     */
    static FindRoot(f, lowerBound, upperBound, accuracy = 1e-8, maxIterations = 100) {
        const result = Brent.TryFindRoot(f, lowerBound, upperBound, accuracy, maxIterations);
        if (result.success) {
            return result.root;
        }
        throw new Error('RootFindingFailed');
    }

    /**
     * Tries to find a root of the function using the Brent-Dekker method.
     * @param {Function} f - The function to find the root of.
     * @param {number} lowerBound - The lower bound of the interval containing the root.
     * @param {number} upperBound - The upper bound of the interval containing the root.
     * @param {number} accuracy - The desired accuracy of the root.
     * @param {number} maxIterations - The maximum number of iterations to perform.
     * @returns {Object} An object with success flag and root value.
     */
    static TryFindRoot(f, lowerBound, upperBound, accuracy = 1e-8, maxIterations = 100) {
        let fmin = f(lowerBound);
        let fmax = f(upperBound);
        let froot = fmax;
        let d = 0, e = 0;

        let root = lowerBound;
        froot = fmin;
        let xMid = NaN;

        // Root must be bracketed.
        if (Math.sign(fmin) === Math.sign(fmax)) {
            return { success: false, root: NaN };
        }

        for (let i = 0; i <= maxIterations; i++) {
            // adjust bounds
            if (Math.sign(froot) === Math.sign(fmax)) {
                upperBound = lowerBound;
                fmax = fmin;
                e = d = root - lowerBound;
            }

            if (Math.abs(fmax) < Math.abs(froot)) {
                const temp = lowerBound;
                lowerBound = root;
                root = upperBound;
                upperBound = temp;
                const tempF = fmin;
                fmin = froot;
                froot = fmax;
                fmax = tempF;
            }

            // convergence check
            const xAcc1 = Precision.PositiveDoublePrecision * Math.abs(root) + 0.5 * accuracy;
            let xMidOld = xMid;
            xMid = (upperBound - root) / 2;

            // 检查froot是否足够接近0
            if (Precision.AlmostEqualNormRelative(froot, 0, froot - 0, accuracy)) {
                return { success: true, root };
            }

            // 检查区间是否足够小
            if (Math.abs(xMid) <= xAcc1) {
                // 确保froot足够接近0才返回
                if (Math.abs(froot) < 1e-6) {
                    return { success: true, root };
                }
            }

            if (xMid === xMidOld) {
                // accuracy not sufficient, but cannot be improved further
                return { success: false, root: NaN };
            }

            if (Math.abs(e) >= xAcc1 && Math.abs(fmin) > Math.abs(froot)) {
                // Attempt inverse quadratic interpolation
                const s = froot / fmin;
                let p, q;
                if (Precision.AlmostEqualRelative(lowerBound, upperBound)) {
                    p = 2 * xMid * s;
                    q = 1 - s;
                } else {
                    q = fmin / fmax;
                    const r = froot / fmax;
                    p = s * (2 * xMid * q * (q - r) - (root - lowerBound) * (r - 1));
                    q = (q - 1) * (r - 1) * (s - 1);
                }

                if (p > 0) {
                    // Check whether in bounds
                    q = -q;
                }

                const absP = Math.abs(p);
                if (2 * absP < Math.min(3 * xMid * Math.abs(q) - Math.abs(xAcc1 * q), Math.abs(e * q))) {
                    // Accept interpolation
                    e = d;
                    d = p / q;
                } else {
                    // Interpolation failed, use bisection
                    d = xMid;
                    e = d;
                }
            } else {
                // Bounds decreasing too slowly, use bisection
                d = xMid;
                e = d;
            }

            lowerBound = root;
            fmin = froot;
            if (Math.abs(d) > xAcc1) {
                root += d;
            } else {
                root += Brent.Sign(xAcc1, xMid);
            }

            froot = f(root);
        }

        return { success: false, root: NaN };
    }

    /**
     * Helper method useful for preventing rounding errors.
     * @param {number} a
     * @param {number} b
     * @returns {number} a*sign(b)
     */
    static Sign(a, b) {
        return b >= 0 ? (a >= 0 ? a : -a) : (a >= 0 ? -a : a);
    }
}

export { Brent };