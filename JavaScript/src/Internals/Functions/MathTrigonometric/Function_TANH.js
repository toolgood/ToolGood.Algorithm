import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';

class Function_TANH extends Function_1 {
    get Name() {
        return "Tanh";
    }

    constructor(a) {
        super(a);
    }

    evaluate(work, tempParameter) {
        let args1 = this.getNumber_1(work, tempParameter);
        if (args1.IsError) { return args1; }
        return Operand.Create(Math.tanh(args1.DoubleValue));
    }
}

export { Function_TANH };

