import { Function_3 } from '../Function_3.js';
import { Operand } from '../../../Operand.js';

class Function_BETAINV extends Function_3 {
    constructor(func1, func2, func3) {
        super(func1, func2, func3);
    }

    Evaluate(work, tempParameter) {
        let args1 = this.func1.Evaluate(work, tempParameter);
        if (args1.IsNotNumber) {
            args1 = args1.ToNumber('Function \'{0}\' parameter {1} is error!', 'BetaInv', 1);
            if (args1.IsError) return args1;
        }
        let args2 = this.func2.Evaluate(work, tempParameter);
        if (args2.IsNotNumber) {
            args2 = args2.ToNumber('Function \'{0}\' parameter {1} is error!', 'BetaInv', 2);
            if (args2.IsError) return args2;
        }
        let args3 = this.func3.Evaluate(work, tempParameter);
        if (args3.IsNotNumber) {
            args3 = args3.ToNumber('Function \'{0}\' parameter {1} is error!', 'BetaInv', 3);
            if (args3.IsError) return args3;
        }
        let p = args1.DoubleValue;
        let alpha = args2.DoubleValue;
        let beta = args3.DoubleValue;
        if (alpha < 0.0 || beta < 0.0 || p < 0.0 || p > 1.0) {
            return Operand.Error('Function \'{0}\' parameter is error!', 'BetaInv');
        }
        return Operand.Create(ExcelFunctions.BetaInv(p, alpha, beta));
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, 'BetaInv');
    }
}

let ExcelFunctions = {
    BetaInv(probability, alpha, beta) {
        return Beta.InvCDF(alpha, beta, probability);
    }
};

let Beta = {
    InvCDF(a, b, p) {
        return Brent.FindRoot(x => SpecialFunctions.BetaRegularized(a, b, x) - p, 0.0, 1.0, 1e-12);
    }
};

let Brent = {
    FindRoot(f, lowerBound, upperBound, accuracy = 1e-8, maxIterations = 100) {
        let result = this.TryFindRoot(f, lowerBound, upperBound, accuracy, maxIterations);
        if (result.success) {
            return result.root;
        }
        throw new Error('RootFindingFailed');
    },

    TryFindRoot(f, lowerBound, upperBound, accuracy, maxIterations) {
        let fmin = f(lowerBound);
        let fmax = f(upperBound);
        let froot = fmax;
        let d = 0.0, e = 0.0;

        let root = upperBound;
        let xMid = NaN;

        if (Math.sign(fmin) === Math.sign(fmax)) {
            return { success: false, root: NaN };
        }

        for (let i = 0; i <= maxIterations; i++) {
            if (Math.sign(froot) === Math.sign(fmax)) {
                upperBound = lowerBound;
                fmax = fmin;
                e = d = root - lowerBound;
            }

            if (Math.abs(fmax) < Math.abs(froot)) {
                lowerBound = root;
                root = upperBound;
                upperBound = lowerBound;
                fmin = froot;
                froot = fmax;
                fmax = fmin;
            }

            let xAcc1 = 2.220446049250313e-16 * Math.abs(root) + 0.5 * accuracy;
            let xMidOld = xMid;
            xMid = (upperBound - root) / 2.0;

            if (Math.abs(xMid) <= xAcc1 || this.AlmostEqualNormRelative(froot, 0, froot, accuracy)) {
                return { success: true, root: root };
            }

            if (xMid === xMidOld) {
                return { success: false, root: NaN };
            }

            if (Math.abs(e) >= xAcc1 && Math.abs(fmin) > Math.abs(froot)) {
                let s = froot / fmin;
                let p, q;
                if (this.AlmostEqualRelative(lowerBound, upperBound)) {
                    p = 2.0 * xMid * s;
                    q = 1.0 - s;
                } else {
                    q = fmin / fmax;
                    let r = froot / fmax;
                    p = s * (2.0 * xMid * q * (q - r) - (root - lowerBound) * (r - 1.0));
                    q = (q - 1.0) * (r - 1.0) * (s - 1.0);
                }

                if (p > 0.0) {
                    q = -q;
                }

                p = Math.abs(p);
                if (2.0 * p < Math.min(3.0 * xMid * q - Math.abs(xAcc1 * q), Math.abs(e * q))) {
                    e = d;
                    d = p / q;
                } else {
                    d = xMid;
                    e = d;
                }
            } else {
                d = xMid;
                e = d;
            }

            lowerBound = root;
            fmin = froot;
            if (Math.abs(d) > xAcc1) {
                root += d;
            } else {
                root += this.Sign(xAcc1, xMid);
            }

            froot = f(root);
        }

        return { success: false, root: NaN };
    },

    Sign(a, b) {
        return b >= 0 ? (a >= 0 ? a : -a) : (a >= 0 ? -a : a);
    },

    AlmostEqualRelative(a, b, maxAbsError, relativeError) {
        if (a === b) return true;
        let diff = Math.abs(a - b);
        if (diff <= maxAbsError) return true;
        return diff <= Math.max(Math.abs(a), Math.abs(b)) * relativeError;
    }
};

let SpecialFunctions = {
    BetaRegularized(a, b, x) {
        let bt = (x === 0.0 || x === 1.0) ? 0.0 : Math.exp(SpecialFunctions.GammaLn(a + b) - SpecialFunctions.GammaLn(a) - SpecialFunctions.GammaLn(b) + (a * Math.log(x)) + (b * Math.log(1.0 - x)));

        let symmetryTransformation = x >= (a + 1.0) / (a + b + 2.0);

        let eps = 2.220446049250313e-16;
        let fpmin = Number.MIN_VALUE / eps;

        let transformedX = x;
        let transformedA = a;
        let transformedB = b;

        if (symmetryTransformation) {
            transformedX = 1.0 - x;
            transformedA = b;
            transformedB = a;
        }

        let qab = transformedA + transformedB;
        let qap = transformedA + 1.0;
        let qam = transformedA - 1.0;
        let c = 1.0;
        let d = 1.0 - (qab * transformedX / qap);

        if (Math.abs(d) < fpmin) {
            d = fpmin;
        }

        d = 1.0 / d;
        let h = d;

        for (let m = 1, m2 = 2; m <= 140; m++, m2 += 2) {
            let aa = m * (transformedB - m) * transformedX / ((qam + m2) * (transformedA + m2));
            d = 1.0 + (aa * d);

            if (Math.abs(d) < fpmin) {
                d = fpmin;
            }

            c = 1.0 + (aa / c);
            if (Math.abs(c) < fpmin) {
                c = fpmin;
            }

            d = 1.0 / d;
            h *= d * c;
            let aa2 = -(transformedA + m) * (qab + m) * transformedX / ((transformedA + m2) * (qap + m2));
            d = 1.0 + (aa2 * d);

            if (Math.abs(d) < fpmin) {
                d = fpmin;
            }

            c = 1.0 + (aa2 / c);
            if (Math.abs(c) < fpmin) {
                c = fpmin;
            }

            d = 1.0 / d;
            let del = d * c;
            h *= del;

            if (Math.abs(del - 1.0) <= eps) {
                return symmetryTransformation ? 1.0 - (bt * h / a) : bt * h / a;
            }
        }

        return symmetryTransformation ? 1.0 - (bt * h / a) : bt * h / a;
    },

    GammaLn(z) {
        let GammaN = 10;
        let GammaR = 10.900511;
        let GammaDk = [
            2.48574089138753565546e-5,
            1.05142378581721974210,
            -3.45687097222016235469,
            4.51227709466894823700,
            -2.98285225311689944323,
            1.05639711578316119432,
            -1.9542866166479562407e-1,
            1.70970543404441224307e-2,
            -5.7192611740430578123e-4,
            4.63399473359905636708e-6,
            -2.7413595497501340645e-8
        ];
        let Constants = {
            LnPi: Math.log(Math.PI),
            LogTwoSqrtEOverPi: Math.log(2 * Math.sqrt(Math.E / Math.PI))
        };

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
                s += GammaDk[i] / (z + i - 1.0);
            }

            return Math.log(s)
                + Constants.LogTwoSqrtEOverPi
                + ((z - 0.5) * Math.log((z - 0.5 + GammaR) / Math.E));
        }
    }
};

export { Function_BETAINV };
