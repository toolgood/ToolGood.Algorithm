import { Constants } from '../Constants.js';
import { Precision } from '../Precision.js';

// Gamma function coefficients
const GammaN = 10;
const GammaR = 10.900511;
const GammaDk = [
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

class Beta {
    /**
     * Computes the logarithm of the gamma function.
     * @param {number} z
     * @returns {number}
     */
    static GammaLn(z) {
        if (z < 0.5) {
            let s = GammaDk[0];
            for (let i = 1; i <= GammaN; i++) {
                s += GammaDk[i] / (i - z);
            }

            return Constants.LnPi
                   - Math.log(Math.sin(Math.PI * z))
                   - Math.log(s)
                   - Constants.LogTwoSqrtEOverPi
                   - ((0.5 - z) * Math.log((0.5 - z + GammaR) / Math.E));
        } else {
            let s = GammaDk[0];
            for (let i = 1; i <= GammaN; i++) {
                s += GammaDk[i] / (z + i - 1);
            }

            return Math.log(s)
                   + Constants.LogTwoSqrtEOverPi
                   + ((z - 0.5) * Math.log((z - 0.5 + GammaR) / Math.E));
        }
    }

    /**
     * Computes the regularized beta function.
     * @param {number} a
     * @param {number} b
     * @param {number} x
     * @returns {number}
     */
    static BetaRegularized(a, b, x) {
        const bt = (x === 0 || x === 1)
            ? 0
            : Math.exp(Beta.GammaLn(a + b) - Beta.GammaLn(a) - Beta.GammaLn(b) + (a * Math.log(x)) + (b * Math.log(1 - x)));

        const symmetryTransformation = x >= (a + 1) / (a + b + 2);

        /* Continued fraction representation */
        const eps = Precision.DoublePrecision;
        const fpmin = 1e-300; // Approximation of 0.0.Increment() / eps

        let transformedX = x;
        let transformedA = a;
        let transformedB = b;

        if (symmetryTransformation) {
            transformedX = 1 - x;
            const swap = transformedA;
            transformedA = transformedB;
            transformedB = swap;
        }

        const qab = transformedA + transformedB;
        const qap = transformedA + 1;
        const qam = transformedA - 1;
        let c = 1;
        let d = 1 - (qab * transformedX / qap);

        if (Math.abs(d) < fpmin) {
            d = fpmin;
        }

        d = 1 / d;
        let h = d;

        for (let m = 1, m2 = 2; m <= 140; m++, m2 += 2) {
            const aa = m * (transformedB - m) * transformedX / ((qam + m2) * (transformedA + m2));
            d = 1 + (aa * d);

            if (Math.abs(d) < fpmin) {
                d = fpmin;
            }

            c = 1 + (aa / c);
            if (Math.abs(c) < fpmin) {
                c = fpmin;
            }

            d = 1 / d;
            h *= d * c;
            const aa2 = -(transformedA + m) * (qab + m) * transformedX / ((transformedA + m2) * (qap + m2));
            d = 1 + (aa2 * d);

            if (Math.abs(d) < fpmin) {
                d = fpmin;
            }

            c = 1 + (aa2 / c);

            if (Math.abs(c) < fpmin) {
                c = fpmin;
            }

            d = 1 / d;
            const del = d * c;
            h *= del;

            if (Math.abs(del - 1) <= eps) {
                return symmetryTransformation ? 1 - (bt * h / transformedA) : bt * h / transformedA;
            }
        }

        return symmetryTransformation ? 1 - (bt * h / transformedA) : bt * h / transformedA;
    }
}

export { Beta };