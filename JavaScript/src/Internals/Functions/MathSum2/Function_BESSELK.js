import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';

class Function_BESSELK extends Function_2 {
    get Name() {
        return "BesselK";
    }

    constructor(func1, func2) {
        super(func1, func2);
    }

    evaluate(engine, tempParameter) {
        let args1 = this.getNumber_1(engine, tempParameter);
        if (args1.IsErrorOrNone) { return args1; }
        let args2 = this.getNumber_2(engine, tempParameter);
        if (args2.IsErrorOrNone) { return args2; }

        let x = args1.DoubleValue;
        let n = Math.trunc(args2.DoubleValue);

        if (x <= 0) {
            return this.parameterError(1);
        }

        return Operand.Create(Function_BESSELK.BesselK(n, x));
    }

    static BesselK(n, x) {
        if (n < 0) n = -n;

        if (n === 0) return Function_BESSELK.BesselK0(x);
        if (n === 1) return Function_BESSELK.BesselK1(x);

        let K0 = Function_BESSELK.BesselK0(x);
        let K1 = Function_BESSELK.BesselK1(x);
        let Kn = 0;

        for (let k = 1; k < n; k++) {
            Kn = K1 + 2.0 * k / x * K0;
            K0 = K1;
            K1 = Kn;
        }

        return K1;
    }

    static BesselK0(x) {
        if (x <= 2.0) {
            let y1 = x * x / 4.0;
            let ans = -Math.log(x / 2.0) * Function_BESSELK.BesselI0(x) + (-0.57721566490153286061
                + y1 * (0.42278433509846713939 + y1 * (0.230697567077446 + y1 * (0.034885890266341
                + y1 * (0.002626979711643 + y1 * (0.000107502176243 + y1 * 0.000007400456812))))));
            return ans;
        }
        let y2 = 2.0 / x;
        let ans2 = Math.exp(-x) / Math.sqrt(x) * (1.253314137315500
            + y2 * (-0.078323582855262 + y2 * (0.021895687854228 + y2 * (-0.010624628097740
            + y2 * (0.005878072214632 + y2 * (-0.002515401617640 + y2 * 0.000532080305632))))));
        return ans2;
    }

    static BesselK1(x) {
        if (x <= 2.0) {
            let y1 = x * x / 4.0;
            let ans = Math.log(x / 2.0) * Function_BESSELK.BesselI1(x) + (1.0 / x) * (1.0
                + y1 * (0.154431442036717 + y1 * (-0.672785797513523 + y1 * (-0.181568943578864
                + y1 * (-0.019194020400716 + y1 * (0.001104044918568 + y1 * 0.000046862429868))))));
            return ans;
        }
        let y2 = 2.0 / x;
        let ans2 = Math.exp(-x) / Math.sqrt(x) * (1.253314137315500
            + y2 * (0.234986192707248 + y2 * (-0.036556202034020 + y2 * (0.015042680553908
            + y2 * (-0.007803534366237 + y2 * (0.003256142832609 + y2 * (-0.000682450383692)))))));
        return ans2;
    }

    static BesselI0(x) {
        let ax = Math.abs(x);
        if (ax < 3.75) {
            let y1 = x / 3.75;
            y1 *= y1;
            return 1.0 + y1 * (3.515622965380465 + y1 * (3.089942465562116 + y1 * (1.206749160761352
                + y1 * (0.265973256598487 + y1 * (0.036076845538912 + y1 * 0.004581327358717)))));
        }
        let y2 = 3.75 / ax;
        return (Math.exp(ax) / Math.sqrt(2.0 * Math.PI * ax)) * (0.398942280401433 + y2 * (0.013285921344730
            + y2 * (0.002253193626842 + y2 * (-0.001575649875251 + y2 * (0.009162816703917
            + y2 * (-0.020577062932649 + y2 * (0.026355373177924 + y2 * (-0.016476329612910 + y2 * 0.003923769605236))))))));
    }

    static BesselI1(x) {
        let ax = Math.abs(x);
        if (ax < 3.75) {
            let y1 = x / 3.75;
            y1 *= y1;
            return x * (0.5 + y1 * (0.878905941521392 + y1 * (0.514988692842374 + y1 * (0.150849342225664
                + y1 * (0.026587328231117 + y1 * (0.003015319414231 + y1 * 0.000324111013968))))));
        }
        let y2 = 3.75 / ax;
        let ans = (Math.exp(ax) / Math.sqrt(2.0 * Math.PI * ax)) * (0.398942280401433 + y2 * (-0.039880242337502
            + y2 * (-0.003620182649157 + y2 * (0.001638105403528 + y2 * (-0.010315550635288
            + y2 * (0.022829679456897 + y2 * (-0.028953129286367 + y2 * (0.017876545768998 - y2 * 0.004200596567986))))))));
        return (x < 0) ? -ans : ans;
    }
}

export { Function_BESSELK };