import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';

class Function_ERF extends Function_1 {
    get Name() {
        return "Erf";
    }

    constructor(func) {
        super(func);
    }

    evaluate(engine, tempParameter) {
        let args1 = this.getNumber_1(engine, tempParameter);
        if (args1.IsError) { return args1; }
        let x = args1.DoubleValue;
        return Operand.Create(Function_ERF.Erf(x));
    }

    static Erf(x) {
        const a1 = 0.254829592;
        const a2 = -0.284496736;
        const a3 = 1.421413741;
        const a4 = -1.453152027;
        const a5 = 1.061405429;
        const p = 0.3275911;

        let sign = x < 0 ? -1 : 1;
        x = Math.abs(x);

        if (x > 6.0) {
            return sign * 1.0;
        }

        let t = 1.0 / (1.0 + p * x);
        let y = 1.0 - (((((a5 * t + a4) * t) + a3) * t + a2) * t + a1) * t * Math.exp(-x * x);

        return sign * y;
    }
}

export { Function_ERF };