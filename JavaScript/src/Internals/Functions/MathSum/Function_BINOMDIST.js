import { Function_4 } from '../Function_4.js';
import { Operand } from '../../../Operand.js';

class Function_BINOMDIST extends Function_4 {
    constructor(func1, func2, func3, func4) {
        super(func1, func2, func3, func4);
    }

    Evaluate(work, tempParameter) {
        let args1 = this.func1.Evaluate(work, tempParameter);
        if (args1.IsNotNumber) {
            args1 = args1.ToNumber('Function \'{0}\' parameter {1} is error!', 'BinomDist', 1);
            if (args1.IsError) return args1;
        }
        let args2 = this.func2.Evaluate(work, tempParameter);
        if (args2.IsNotNumber) {
            args2 = args2.ToNumber('Function \'{0}\' parameter {1} is error!', 'BinomDist', 2);
            if (args2.IsError) return args2;
        }
        let args3 = this.func3.Evaluate(work, tempParameter);
        if (args3.IsNotNumber) {
            args3 = args3.ToNumber('Function \'{0}\' parameter {1} is error!', 'BinomDist', 3);
            if (args3.IsError) return args3;
        }
        let args4 = this.func4.Evaluate(work, tempParameter);
        if (args4.IsNotBoolean) {
            args4 = args4.ToBoolean('Function \'{0}\' parameter {1} is error!', 'BinomDist', 4);
            if (args4.IsError) return args4;
        }

        let n2 = args2.IntValue;
        let n3 = args3.DoubleValue;
        if (!(n3 >= 0.0 && n3 <= 1.0 && n2 >= 0)) {
            return Operand.Error('Function \'{0}\' parameter is error!', 'BinomDist');
        }
        return Operand.Create(ExcelFunctions.BinomDist(args1.IntValue, n2, n3, args4.BooleanValue));
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, 'BinomDist');
    }
}

let ExcelFunctions = {
    BinomDist(k, n, p, state) {
        if (state === false) {
            return Binomial.PMF(p, n, k);
        }
        return Binomial.CDF(p, n, k);
    }
};

let Binomial = {
    PMF(p, n, k) {
        if (k < 0 || k > n) {
            return 0.0;
        }

        if (p === 0.0) {
            return k === 0 ? 1.0 : 0.0;
        }

        if (p === 1.0) {
            return k === n ? 1.0 : 0.0;
        }

        return Math.exp(SpecialFunctions.BinomialLn(n, k) + (k * Math.log(p)) + ((n - k) * Math.log(1.0 - p)));
    },

    CDF(p, n, x) {
        if (x < 0.0) {
            return 0.0;
        }

        if (x > n) {
            return 1.0;
        }

        let k = Math.floor(x);
        return SpecialFunctions.BetaRegularized(n - k, k + 1, 1 - p);
    }
};

let SpecialFunctions = {
    BinomialLn(n, k) {
        if (k < 0 || n < 0 || k > n) {
            return -Infinity;
        }

        return this.FactorialLn(n) - this.FactorialLn(k) - this.FactorialLn(n - k);
    },

    FactorialLn(x) {
        if (x <= 1) {
            return 0;
        }

        if (x < this._factorialCache.length) {
            return Math.log(this._factorialCache[x]);
        }

        return this.GammaLn(x + 1.0);
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
    },

    BetaRegularized(a, b, x) {
        let bt = (x === 0.0 || x === 1.0) ? 0.0 : Math.exp(this.GammaLn(a + b) - this.GammaLn(a) - this.GammaLn(b) + (a * Math.log(x)) + (b * Math.log(1.0 - x)));

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

    _factorialCache: (function() {
        let cache = new Array(171);
        cache[0] = 1.0;
        for (let i = 1; i < cache.length; i++) {
            cache[i] = cache[i - 1] * i;
        }
        return cache;
    })()
};

export { Function_BINOMDIST };
