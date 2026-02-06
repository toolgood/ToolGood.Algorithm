import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';

class Function_FISHERINV extends Function_1 {
    get Name() {
        return "FisherInv";
    }

    constructor(a) {
        super(a);
    }

    Evaluate(engine, tempParameter) {
        let args1 = this.GetNumber_1(engine, tempParameter);
        if (args1.IsError) { return args1; }
        let x = args1.DoubleValue;
        let n = (Math.exp(2 * x) - 1) / (Math.exp(2 * x) + 1);
        return Operand.Create(n);
    }
}

export { Function_FISHERINV };

