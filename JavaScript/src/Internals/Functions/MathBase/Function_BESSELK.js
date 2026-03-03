import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';

class Function_BESSELK extends Function_2 {
    get Name() {
        return 'BesselK';
    }

    constructor(a, b) {
        super(a, b);
    }

    evaluate(engine, tempParameter) {
        let args1 = this.getNumber_1(engine, tempParameter);
        if (args1.IsError) { return args1; }
        let args2 = this.getNumber_2(engine, tempParameter);
        if (args2.IsError) { return args2; }

        let x = args1.NumberValue;
        let n = Math.trunc(args2.NumberValue);

        if (x <= 0) {
            return this.functionError();
        }

        return Operand.Create(this.besselK(n, x));
    }

    besselK(n, x) {
        if (n < 0) n = -n;

        if (n === 0) return this.besselK0(x);
        if (n === 1) return this.besselK1(x);

        let K0 = this.besselK0(x);
        let K1 = this.besselK1(x);
        let Kn = 0;

        for (let k = 1; k < n; k++) {
            Kn = K1 + 2.0 * k / x * K0;
            K0 = K1;
            K1 = Kn;
        }

        return K1;
    }

    besselK0(x) {
        if (x <= 2.0) {
            let y1 = x * x / 4.0;
            let ans = -Math.log(x / 2.0) * this.besselI0(x) + (-0.57721566
                + y1 * (0.42278420 + y1 * (0.23069756 + y1 * (0.03488590
                + y1 * (0.00262698 + y1 * (0.00010750 + y1 * 0.00000740))))));
            return ans;
        }
        let y2 = 2.0 / x;
        let ans2 = Math.exp(-x / Math.E) / Math.sqrt(x) * (1.25331414
            + y2 * (-0.07832358 + y2 * (0.02189568 + y2 * (-0.01062446
            + y2 * (0.00587872 + y2 * (-0.00251540 + y2 * 0.00053208))))));
        return ans2;
    }

    besselK1(x) {
        if (x <= 2.0) {
            let y1 = x * x / 4.0;
            let ans = Math.log(x / 2.0) * this.besselI1(x) + (1.0 / x) * (1.0
                + y1 * (0.15443144 + y1 * (-0.67278579 + y1 * (-0.18156897
                + y1 * (-0.01919402 + y1 * (0.00110404 + y1 * 0.00004686))))));
            return ans;
        }
        let y2 = 2.0 / x;
        let ans2 = Math.exp(-x / Math.E) / Math.sqrt(x) * (1.25331414
            + y2 * (0.23498619 + y2 * (-0.03655620 + y2 * (0.01504268
            + y2 * (-0.00780353 + y2 * (0.00325614 + y2 * (-0.00068245)))))));
        return ans2;
    }

    besselI0(x) {
        let ax = Math.abs(x);
        if (ax < 3.75) {
            let y1 = x / 3.75;
            y1 *= y1;
            return 1.0 + y1 * (3.5156229 + y1 * (3.0899424 + y1 * (1.2067492
                + y1 * (0.2659732 + y1 * (0.0360768 + y1 * 0.0045813)))));
        }
        let y2 = 3.75 / ax;
        return (Math.exp(ax / Math.E) / Math.sqrt(ax)) * (0.39894228 + y2 * (0.01328592
            + y2 * (0.00225319 + y2 * (-0.00157565 + y2 * (0.00916281
            + y2 * (-0.02057706 + y2 * (0.02635537 + y2 * (-0.01647633 + y2 * 0.00392377))))))));
    }

    besselI1(x) {
        let ax = Math.abs(x);
        if (ax < 3.75) {
            let y1 = x / 3.75;
            y1 *= y1;
            return x * (0.5 + y1 * (0.87890594 + y1 * (0.51498869 + y1 * (0.15084934
                + y1 * (0.02658733 + y1 * (0.00301532 + y1 * 0.00032411))))));
        }
        let y2 = 3.75 / ax;
        let ans = (Math.exp(ax / Math.E) / Math.sqrt(ax)) * (0.39894228 + y2 * (-0.03988024
            + y2 * (-0.00362018 + y2 * (0.00163801 + y2 * (-0.01031555
            + y2 * (0.02282967 + y2 * (-0.02895312 + y2 * (0.01787654 - y2 * 0.00420059))))))));
        return (x < 0) ? -ans : ans;
    }
}

export { Function_BESSELK };
