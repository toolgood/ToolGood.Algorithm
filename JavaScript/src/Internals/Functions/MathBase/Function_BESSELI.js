import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';

class Function_BESSELI extends Function_2 {
    get Name() {
        return 'BesselI';
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

        return Operand.Create(this.besselI(n, x));
    }

    besselI(n, x) {
        if (x < 0) {
            return (n % 2 === 0 ? 1 : -1) * this.besselI(n, -x);
        }
        if (x === 0) {
            return (n === 0) ? 1.0 : 0.0;
        }

        let ax = Math.abs(x);
        if (ax < 1e-10) {
            return (n === 0) ? 1.0 : 0.0;
        }

        if (n < 0) n = -n;

        if (ax > 700) {
            let factor = Math.exp(ax) / Math.sqrt(2 * Math.PI * ax);
            return factor * (1.0 - (4.0 * n * n - 1.0) / (8.0 * ax));
        }

        if (n === 0) return this.besselI0(x);
        if (n === 1) return this.besselI1(x);

        let I0 = this.besselI0(x);
        let I1 = this.besselI1(x);
        let In = 0;

        for (let k = 1; k < n; k++) {
            In = I1 + 2.0 * k / x * I0;
            I0 = I1;
            I1 = In;
        }

        return I1;
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

export { Function_BESSELI };
