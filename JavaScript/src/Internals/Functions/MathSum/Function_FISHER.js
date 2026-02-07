import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';

class Function_FISHER extends Function_1 {
    get Name() {
        return "Fisher";
    }

    constructor(a) {
        super(a);
    }

    evaluate(engine, tempParameter) {
        let args1 = this.getNumber_1(engine, tempParameter);
        if (args1.IsError) { return args1; }
        let x = args1.DoubleValue;
        if (x >= 1 || x <= -1) {
            return this.functionError();
        }
        let n = 0.5 * Math.log((1 + x) / (1 - x));
        return Operand.Create(n);
    }
}

export { Function_FISHER };

