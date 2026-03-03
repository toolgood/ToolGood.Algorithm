import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';

class Function_BESSELJ extends Function_2 {
    get Name() {
        return 'BesselJ';
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

        return Operand.Create(this.besselJ(n, x));
    }

    besselJ(n, x) {
        if (x === 0) {
            return (n === 0) ? 1.0 : 0.0;
        }

        if (n < 0) n = -n;

        let ax = Math.abs(x);
        if (ax < 1e-10) {
            return (n === 0) ? 1.0 : 0.0;
        }

        if (n === 0) return this.besselJ0(x);
        if (n === 1) return this.besselJ1(x);

        if (ax > n) {
            let J0 = this.besselJ0(x);
            let J1 = this.besselJ1(x);
            let Jn = 0;

            for (let k = 1; k < n; k++) {
                Jn = (2.0 * k / x) * J1 - J0;
                J0 = J1;
                J1 = Jn;
            }
            return J1;
        }

        let m = Math.floor(1.5 * n + 10);
        let J = new Array(m + 2);
        J[m + 1] = 0.0;
        J[m] = 1.0;

        for (let k = m; k >= 1; k--) {
            J[k - 1] = (2.0 * k / x) * J[k] - J[k + 1];
        }

        let sum = 0.0;
        for (let k = 0; k <= m; k += 2) {
            sum += 2.0 * J[k];
        }
        sum -= J[0];

        return J[n] / sum;
    }

    besselJ0(x) {
        let ax = Math.abs(x);
        if (ax < 8.0) {
            let y1 = x * x;
            let ans1 = 57568490574.0 + y1 * (-13362590354.0 + y1 * (651619640.7
                + y1 * (-11214424.18 + y1 * (77392.33017 + y1 * (-184.9052456)))));
            let ans2 = 57568490411.0 + y1 * (1029532985.0 + y1 * (9494680.718
                + y1 * (59272.64853 + y1 * (267.8532712 + y1 * 1.0))));
            return ans1 / ans2;
        }
        let z = 8.0 / ax;
        let y2 = z * z;
        let xx = ax - 0.785398164;
        let ans3 = 1.0 + y2 * (-0.1098628627e-2 + y2 * (0.2734510407e-4
            + y2 * (-0.2073370639e-5 + y2 * 0.2093887211e-6)));
        let ans4 = -0.1562499995e-1 + y2 * (0.1430488765e-3
            + y2 * (-0.6911147651e-5 + y2 * (0.7621095161e-6
            - y2 * 0.934935152e-7)));
        return Math.sqrt(0.636619772 / ax) * (Math.cos(xx) * ans3 - z * Math.sin(xx) * ans4);
    }

    besselJ1(x) {
        let ax = Math.abs(x);
        if (ax < 8.0) {
            let y1 = x * x;
            let ans1 = x * (72362614232.0 + y1 * (-7895059235.0 + y1 * (242396853.1
                + y1 * (-2972611.439 + y1 * (15704.48260 + y1 * (-30.16036606))))));
            let ans2 = 144725228442.0 + y1 * (2300535178.0 + y1 * (18583304.74
                + y1 * (99447.43394 + y1 * (376.9991397 + y1 * 1.0))));
            return ans1 / ans2;
        }
        let z = 8.0 / ax;
        let y2 = z * z;
        let xx = ax - 2.356194491;
        let ans3 = 1.0 + y2 * (0.183105e-2 + y2 * (-0.3516396496e-4
            + y2 * (0.2457520174e-5 + y2 * (-0.240337019e-6))));
        let ans4 = 0.04687499995 + y2 * (-0.2002690873e-3
            + y2 * (0.8449199096e-5 + y2 * (-0.88228987e-6
            + y2 * 0.105787412e-6)));
        let ans = Math.sqrt(0.636619772 / ax) * (Math.cos(xx) * ans3 - z * Math.sin(xx) * ans4);
        return (x < 0) ? -ans : ans;
    }
}

export { Function_BESSELJ };
