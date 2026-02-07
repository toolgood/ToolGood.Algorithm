import { Constants } from '../Constants.js';
import { Precision } from '../Precision.js';
import { Beta } from './Beta.js';
import { SpecialFunctions } from './SpecialFunctions.js';

class Gamma {
    /**
     * Computes the lower regularized gamma function.
     * @param {number} a
     * @param {number} x
     * @returns {number}
     */
    static gammaLowerRegularized(a, x) {
        let epsilon = 0.000000000000001;
        let big = 4503599627370496.0;
        let bigInv = 2.22044604925031308085e-16;

        if (Precision.AlmostEqual(a, 0)) {
            if (Precision.AlmostEqual(x, 0)) {
                //use right hand limit value because so that regularized upper/lower gamma definition holds.
                return 1;
            }
            return 1;
        }

        if (Precision.AlmostEqual(x, 0)) {
            return 0;
        }

        let ax = (a * Math.log(x)) - x - Beta.gammaLn(a);
        if (ax < -709.78271289338399) {
            return a < x ? 1 : 0;
        }

        if (x <= 1 || x <= a) {
            let r2 = a;
            let c2 = 1;
            let ans2 = 1;

            do {
                r2 = r2 + 1;
                c2 = c2 * x / r2;
                ans2 += c2;
            } while ((c2 / ans2) > epsilon);

            return Math.exp(ax) * ans2 / a;
        }

        let c = 0;
        let y = 1 - a;
        let z = x + y + 1;

        let p3 = 1;
        let q3 = x;
        let p2 = x + 1;
        let q2 = z * x;
        let ans = p2 / q2;

        let error;

        do {
            c++;
            y += 1;
            z += 2;
            let yc = y * c;

            let p = (p2 * z) - (p3 * yc);
            let q = (q2 * z) - (q3 * yc);

            if (q !== 0) {
                let nextans = p / q;
                error = Math.abs((ans - nextans) / nextans);
                ans = nextans;
            } else {
                // zero div, skip
                error = 1;
            }

            // shift
            p3 = p2;
            p2 = p;
            q3 = q2;
            q2 = q;

            // normalize fraction when the numerator becomes large
            if (Math.abs(p) > big) {
                p3 *= bigInv;
                p2 *= bigInv;
                q3 *= bigInv;
                q2 *= bigInv;
            }
        } while (error > epsilon);

        return 1 - (Math.exp(ax) * ans);
    }

    /**
     * Computes the inverse of the lower regularized gamma function.
     * @param {number} a
     * @param {number} y0
     * @returns {number}
     */
    static gammaLowerRegularizedInv(a, y0) {
        let epsilon = 0.000000000000001;
        let big = 4503599627370496.0;
        let threshold = 5 * epsilon;

        if (isNaN(a) || isNaN(y0)) {
            return NaN;
        }

        if (Precision.AlmostEqual(y0, 0)) {
            return 0;
        }

        if (Precision.AlmostEqual(y0, 1)) {
            return Infinity;
        }

        let transformedY0 = 1 - y0;

        let xUpper = big;
        let xLower = 0;
        let yUpper = 1;
        let yLower = 0;

        // Initial Guess
        let d = 1 / (9 * a);
        let y = 1 - d - (0.98 * Constants.Sqrt2 * SpecialFunctions.erfInv((2 * transformedY0) - 1) * Math.sqrt(d));
        let x = a * y * y * y;
        let lgm = Beta.gammaLn(a);

        for (let i = 0; i < 10; i++) {
            if (x < xLower || x > xUpper) {
                d = 0.0625;
                break;
            }

            y = 1 - Gamma.gammaLowerRegularized(a, x);
            if (y < yLower || y > yUpper) {
                d = 0.0625;
                break;
            }

            if (y < transformedY0) {
                xUpper = x;
                yLower = y;
            } else {
                xLower = x;
                yUpper = y;
            }

            d = ((a - 1) * Math.log(x)) - x - lgm;
            if (d < -709.78271289338399) {
                d = 0.0625;
                break;
            }

            d = -Math.exp(d);
            d = (y - transformedY0) / d;
            if (Math.abs(d / x) < epsilon) {
                return x;
            }

            if ((d > (x / 4)) && (transformedY0 < 0.05)) {
                // Naive heuristics for cases near the singularity
                d = x / 10;
            }

            x -= d;
        }

        if (xUpper === big) {
            if (x <= 0) {
                x = 1;
            }

            while (xUpper === big) {
                x = (1 + d) * x;
                y = 1 - Gamma.gammaLowerRegularized(a, x);
                if (y < transformedY0) {
                    xUpper = x;
                    yLower = y;
                    break;
                }

                d = d + d;
            }
        }

        let dir = 0;
        d = 0.5;
        for (let i = 0; i < 400; i++) {
            x = xLower + (d * (xUpper - xLower));
            y = 1 - Gamma.gammaLowerRegularized(a, x);
            lgm = (xUpper - xLower) / (xLower + xUpper);
            if (Math.abs(lgm) < threshold) {
                return x;
            }

            lgm = (y - transformedY0) / transformedY0;
            if (Math.abs(lgm) < threshold) {
                return x;
            }

            if (x <= 0) {
                return 0;
            }

            if (y >= transformedY0) {
                xLower = x;
                yUpper = y;
                if (dir < 0) {
                    dir = 0;
                    d = 0.5;
                } else {
                    if (dir > 1) {
                        d = (0.5 * d) + 0.5;
                    } else {
                        d = (transformedY0 - yLower) / (yUpper - yLower);
                    }
                }

                dir = dir + 1;
            } else {
                xUpper = x;
                yLower = y;
                if (dir > 0) {
                    dir = 0;
                    d = 0.5;
                } else {
                    if (dir < -1) {
                        d = 0.5 * d;
                    } else {
                        d = (transformedY0 - yLower) / (yUpper - yLower);
                    }
                }

                dir = dir - 1;
            }
        }

        return x;
    }

    /**
     * Computes the gamma function.
     * @param {number} z
     * @returns {number}
     */
    static Gamma(z) {
        // GammaDk coefficients from Beta.js
        let GammaDk = [
            2.48574089138753565546e-5,
            1.05142378581721974210,
            -3.45687097222016235469,
            4.51227709466894823700,
            -2.98285225323576655721,
            1.05639711577126713077,
            -1.95428773191645869583e-1,
            1.70970543404441224307e-2,
            -5.71926117404305781283e-4,
            4.63399473359905636708e-6,
            0.0
        ];
        let GammaN = 10;
        let GammaR = 10.900511;

        if (z < 0.5) {
            let s = GammaDk[0];
            for (let i = 1; i <= GammaN; i++) {
                s += GammaDk[i] / (i - z);
            }

            return Math.PI / (
                Math.sin(Math.PI * z) *
                s *
                Constants.TwoSqrtEOverPi *
                Math.pow((0.5 - z + GammaR) / Math.E, 0.5 - z)
            );
        } else {
            let s = GammaDk[0];
            for (let i = 1; i <= GammaN; i++) {
                s += GammaDk[i] / (z + i - 1);
            }

            return s * Constants.TwoSqrtEOverPi * Math.pow((z - 0.5 + GammaR) / Math.E, z - 0.5);
        }
    }
}

export { Gamma };