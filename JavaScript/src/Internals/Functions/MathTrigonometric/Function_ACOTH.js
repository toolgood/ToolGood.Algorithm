import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';

class Function_ACOTH extends Function_1 {
    get Name() {
        return "Acoth";
    }

    constructor(a) {
        super(a);
    }

    evaluate(work, tempParameter) {
        let args1 = this.getNumber_1(work, tempParameter);
        if (args1.IsError) { return args1; }
        let d = args1.DoubleValue;
        if (Math.abs(d) <= 1) {
            return Operand.Error(this.functionError, 'ACOTH');
        }
        return Operand.Create(0.5 * Math.log((d + 1) / (d - 1)));
    }
}

export { Function_ACOTH };
