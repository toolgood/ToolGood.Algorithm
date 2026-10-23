import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';

class Function_ACOT extends Function_1 {
    get Name() {
        return "Acot";
    }

    constructor(a) {
        super(a);
    }

    evaluate(work, tempParameter) {
        let args1 = this.getNumber_1(work, tempParameter);
        if (args1.IsError) { return args1; }
        return Operand.Create(Math.PI / 2 - Math.atan(args1.DoubleValue));
    }
}

export { Function_ACOT };
