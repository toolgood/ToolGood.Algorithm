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

        let k = (x - location) / scale;
        let h = freedom / (freedom + (k * k));
        let ib = 0.5 * SpecialFunctions.BetaRegularized(freedom / 2, 0.5, h);
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

        // 使用二分查找
        let lower = -100;
        let upper = 100;
        let mid;
        let fmid;

        // 扩展搜索区间如果需要
        while (Math.sign(StudentT.InvCDFHelper(location, scale, freedom, p, lower)) === Math.sign(StudentT.InvCDFHelper(location, scale, freedom, p, upper))) {
            lower *= 2;
            upper *= 2;
        }

        // 二分查找
        for (let i = 0; i < 100; i++) {
            mid = (lower + upper) / 2;
            fmid = StudentT.InvCDFHelper(location, scale, freedom, p, mid);

            if (Math.abs(fmid) < 1e-12) {
                return mid;
            }

            if (Math.sign(fmid) === Math.sign(StudentT.InvCDFHelper(location, scale, freedom, p, lower))) {
                lower = mid;
            } else {
                upper = mid;
            }

            if (Math.abs(upper - lower) < 1e-12) {
                return mid;
            }
        }

        return mid;
    }

    /**
     * Helper function for InvCDF
     * @param {number} location
     * @param {number} scale
     * @param {number} freedom
     * @param {number} p
     * @param {number} x
     * @returns {number}
     */
    static InvCDFHelper(location, scale, freedom, p, x) {
        let k = (x - location) / scale;
        let h = freedom / (freedom + (k * k));
        let ib = 0.5 * SpecialFunctions.BetaRegularized(freedom / 2, 0.5, h);
        return x <= location ? ib - p : 1 - ib - p;
    }
}

export { StudentT };