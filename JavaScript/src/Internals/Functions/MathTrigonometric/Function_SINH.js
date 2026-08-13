import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';

class Function_SINH extends Function_1 {
    get Name() {
        return "Sinh";
    }

    constructor(a) {
        super(a);
    }

    evaluate(work, tempParameter) {
        let args1 = this.getNumber_1(work, tempParameter);
        if (args1.IsError) { return args1; }
        let x = args1.DoubleValue;
        if (x >= 66 || x <= -66) { return this.parameterError(1); }
        return Operand.Create(Math.sinh(x));
    }
}

export { Function_SINH };

