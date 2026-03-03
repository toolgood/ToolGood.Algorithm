import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';

class Function_ERFC extends Function_1 {
    get Name() {
        return 'Erfc';
    }

    constructor(a) {
        super(a);
    }

    evaluate(engine, tempParameter) {
        let args1 = this.getNumber_1(engine, tempParameter);
        if (args1.IsError) { return args1; }
        let x = args1.NumberValue;
        return Operand.Create(this.erfc(x));
    }

    erfc(x) {
        return 1.0 - this.erf(x);
    }

    erf(x) {
        let a1 = 0.254829592;
        let a2 = -0.284496736;
        let a3 = 1.421413741;
        let a4 = -1.453152027;
        let a5 = 1.061405429;
        let p = 0.3275911;

        let sign = x < 0 ? -1 : 1;
        x = Math.abs(x);

        let t = 1.0 / (1.0 + p * x);
        let y = 1.0 - (((((a5 * t + a4) * t) + a3) * t + a2) * t + a1) * t * Math.exp(-x * x);

        return sign * y;
    }
}

export { Function_ERFC };
