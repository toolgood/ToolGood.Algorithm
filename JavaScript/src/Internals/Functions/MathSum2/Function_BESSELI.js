import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';

class Function_BESSELI extends Function_2 {
    get Name() {
        return "BesselI";
    }

    constructor(func1, func2) {
        super(func1, func2);
    }

    evaluate(engine, tempParameter) {
        let args1 = this.getNumber_1(engine, tempParameter);
        if (args1.IsError) { return args1; }
        let args2 = this.getNumber_2(engine, tempParameter);
        if (args2.IsError) { return args2; }

        let x = args1.DoubleValue;
        let n = Math.trunc(args2.DoubleValue);

        return Operand.Create(Function_BESSELI.BesselI(n, x));
    }

    static BesselI(n, x) {
        if (x < 0) {
            return (n % 2 === 0 ? 1 : -1) * BesselI(n, -x);
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

        if (n === 0) return BesselI.BesselI0(x);
        if (n === 1) return BesselI.BesselI1(x);

        let I0 = BesselI.BesselI0(x);
        let I1 = BesselI.BesselI1(x);
        let In = 0;

        for (let k = 1; k < n; k++) {
            In = I1 + 2.0 * k / x * I0;
            I0 = I1;
            I1 = In;
        }

        return I1;
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

export { Function_BESSELI };