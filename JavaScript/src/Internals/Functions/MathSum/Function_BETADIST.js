import { Function_3 } from '../Function_3.js';
import { Operand } from '../../../Operand.js';

class Function_BETADIST extends Function_3 {
    constructor(func1, func2, func3) {
        super(func1, func2, func3);
    }

    Evaluate(work, tempParameter) {
        const args1 = this.func1.Evaluate(work, tempParameter);
        if (args1.isNotNumber) {
            args1.ToNumber('Function \'{0}\' parameter {1} is error!', 'BetaDist', 1);
            if (args1.isError) return args1;
        }
        const args2 = this.func2.Evaluate(work, tempParameter);
        if (args2.isNotNumber) {
            args2.ToNumber('Function \'{0}\' parameter {1} is error!', 'BetaDist', 2);
            if (args2.isError) return args2;
        }
        const args3 = this.func3.Evaluate(work, tempParameter);
        if (args3.isNotNumber) {
            args3.ToNumber('Function \'{0}\' parameter {1} is error!', 'BetaDist', 3);
            if (args3.isError) return args3;
        }
        const x = args1.doubleValue;
        const alpha = args2.doubleValue;
        const beta = args3.doubleValue;

        if (alpha < 0.0 || beta < 0.0) {
            return Operand.error('Function \'{0}\' parameter is error!', 'BetaDist');
        }
        return Operand.Create(ExcelFunctions.BetaDist(x, alpha, beta));
    }

    toString(stringBuilder, addBrackets) {
        this.addFunction(stringBuilder, 'BetaDist');
    }
}

const ExcelFunctions = {
    BetaDist(x, alpha, beta) {
        return Beta.CDF(alpha, beta, x);
    }
};

const Beta = {
    CDF(a, b, x) {
        if (x < 0.0) {
            return 0.0;
        }

        if (x >= 1.0) {
            return 1.0;
        }

        if (!isFinite(a) && !isFinite(b)) {
            return x < 0.5 ? 0.0 : 1.0;
        }

        if (!isFinite(a)) {
            return x < 1.0 ? 0.0 : 1.0;
        }

        if (!isFinite(b)) {
            return x >= 0.0 ? 1.0 : 0.0;
        }

        if (a === 0.0 && b === 0.0) {
            if (x >= 0.0 && x < 1.0) {
                return 0.5;
            }
            return 1.0;
        }

        if (a === 0.0) {
            return 1.0;
        }

        if (b === 0.0) {
            return x >= 1.0 ? 1.0 : 0.0;
        }

        if (a === 1.0 && b === 1.0) {
            return x;
        }

        return SpecialFunctions.BetaRegularized(a, b, x);
    }
};

const SpecialFunctions = {
    BetaRegularized(a, b, x) {
        const bt = (x === 0.0 || x === 1.0) ? 0.0 : Math.exp(SpecialFunctions.GammaLn(a + b) - SpecialFunctions.GammaLn(a) - SpecialFunctions.GammaLn(b) + (a * Math.log(x)) + (b * Math.log(1.0 - x)));

        const symmetryTransformation = x >= (a + 1.0) / (a + b + 2.0);

        const eps = 2.220446049250313e-16; // Double precision epsilon
        const fpmin = Number.MIN_VALUE / eps;

        let transformedX = x;
        let transformedA = a;
        let transformedB = b;

        if (symmetryTransformation) {
            transformedX = 1.0 - x;
            transformedA = b;
            transformedB = a;
        }

        const qab = transformedA + transformedB;
        const qap = transformedA + 1.0;
        const qam = transformedA - 1.0;
        let c = 1.0;
        let d = 1.0 - (qab * transformedX / qap);

        if (Math.abs(d) < fpmin) {
            d = fpmin;
        }

        d = 1.0 / d;
        let h = d;

        for (let m = 1, m2 = 2; m <= 140; m++, m2 += 2) {
            const aa = m * (transformedB - m) * transformedX / ((qam + m2) * (transformedA + m2));
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
            const aa2 = -(transformedA + m) * (qab + m) * transformedX / ((transformedA + m2) * (qap + m2));
            d = 1.0 + (aa2 * d);

            if (Math.abs(d) < fpmin) {
                d = fpmin;
            }

            c = 1.0 + (aa2 / c);
            if (Math.abs(c) < fpmin) {
                c = fpmin;
            }

            d = 1.0 / d;
            const del = d * c;
            h *= del;

            if (Math.abs(del - 1.0) <= eps) {
                return symmetryTransformation ? 1.0 - (bt * h / a) : bt * h / a;
            }
        }

        return symmetryTransformation ? 1.0 - (bt * h / a) : bt * h / a;
    },

    GammaLn(z) {
        const GammaN = 10;
        const GammaR = 10.900511;
        const GammaDk = [
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
        const Constants = {
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

export { Function_BETADIST };
