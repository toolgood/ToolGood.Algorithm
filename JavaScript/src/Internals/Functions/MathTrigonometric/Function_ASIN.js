import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';

class Function_ASIN extends Function_1 {
    get Name() {
        return "Asin";
    }

    constructor(a) {
        super(a);
    }

    evaluate(work, tempParameter) {
        let args1 = this.getNumber_1(work, tempParameter);
        if (args1.IsError) { return args1; }
        let x = args1.DoubleValue;
        if (x < -1 || x > 1) {
            return this.functionError();
        }
        return Operand.Create(Math.asin(x));
    }
}

export { Function_ASIN };

