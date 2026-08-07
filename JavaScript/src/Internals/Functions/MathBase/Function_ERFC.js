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
        // Numerical Recipes 6.2 erfcc:对 |x| 做有理逼近,避免 1-erf(x) 在大 x 时精度归零
        let z = Math.abs(x);
        let t = 1.0 / (1.0 + 0.5 * z);
        let ans = t * Math.exp(-z * z - 1.26551223 + t * (1.00002368 + t * (0.37409196 + t * (0.09678418 + t * (-0.18628806 + t * (0.27886807 + t * (-1.13520398 + t * (1.48851587 + t * (-0.82215223 + t * 0.17087277)))))))));
        return x >= 0 ? ans : 2.0 - ans;
    }
}

export { Function_ERFC };
